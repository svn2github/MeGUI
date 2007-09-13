// ****************************************************************************
// 
// Copyright (C) 2005  Doom9
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MeGUI.core.util;

namespace MeGUI
{
    public sealed class AviSynthAudioEncoder : IJobProcessor // : AudioEncoder
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "AviSynthAudioEncoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is AudioJob &&
                (((j as AudioJob).Settings is MP3Settings) ||
                ((j as AudioJob).Settings is MP2Settings) ||
                ((j as AudioJob).Settings is AC3Settings) ||
                ((j as AudioJob).Settings is WinAmpAACSettings) ||
                ((j as AudioJob).Settings is AudXSettings) ||
                ((j as AudioJob).Settings is OggVorbisSettings) ||
                ((j as AudioJob).Settings is FaacSettings) ||
                ((j as AudioJob).Settings is NeroAACSettings)))
                return new AviSynthAudioEncoder(mf.Settings);
            return null;
        }

        #region fields
        private Process _encoderProcess;
        private string _avisynthAudioScript;
        private string _encoderExecutablePath;
        private string _encoderCommandLine;
        private bool _mustSendWavHeaderToEncoderStdIn;

        private int _sampleRate;


        private System.Threading.ManualResetEvent _mre = new System.Threading.ManualResetEvent(true); // lock used to pause encoding
        private Thread _encoderThread = null;
        private Thread _readFromStdOutThread = null;
        private Thread _readFromStdErrThread = null;
        private string _encoderStdErr = null;
        private string _encoderStdOut = null;
        private StringBuilder _logBuilder = new StringBuilder();
        private static readonly System.Text.RegularExpressions.Regex _cleanUpStringRegex = new System.Text.RegularExpressions.Regex(@"\n[^\n]+\r", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        private MeGUISettings _settings = null;
        private int SAMPLES_PER_UPDATE;
        private AudioJob audioJob;
        private StatusUpdate su = new StatusUpdate();
        private DateTime _start;

        private List<string> _tempFiles = new List<string>();
        private readonly string _uniqueId = Guid.NewGuid().ToString("N");
        #endregion

        #region methods

        private void writeTempTextFile(string filePath, string text)
        {
            using (Stream temp = new FileStream(filePath, System.IO.FileMode.Create))
            {
                using (TextWriter avswr = new StreamWriter(temp, System.Text.Encoding.Default))
                {
                    avswr.WriteLine(text);
                }
            }
            _tempFiles.Add(filePath);
        }


        private void deleteTempFiles()
        {
            foreach (string filePath in _tempFiles)
                safeDelete(filePath);

        }

        private static void safeDelete(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                // Do Nothing
            }
        }

        private void createTemporallyEqFiles(string tempPath)
        {
            // http://forum.doom9.org/showthread.php?p=778156#post778156
            writeTempTextFile(tempPath + "front.feq", @"-96
-96
-96
-4
-4
-4
-4
-4
-4
-4
-4
-4
-4
-4
-4
-96
-96
-96
");
            writeTempTextFile(tempPath + "center.feq", @"-96
-96
-96
-96
-96
-96
3
3
3
3
3
3
3
3
3
3
3
3
");
            writeTempTextFile(tempPath + "lfe.feq", @"0
0
0
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
-96
");
            writeTempTextFile(tempPath + "back.feq", @"-96
-96
-96
-6
-6
-6
-6
-6
-6
-6
-6
-6
-6
-6
-6
-96
-96
-96
");
        }

        private void raiseEvent()
        {
            if (su.IsComplete = (su.IsComplete || su.WasAborted || su.HasError))
                su.Log = createLog();
            if (StatusUpdate != null)
                StatusUpdate(su);
        }

        private void setProgress(decimal n)
        {
            su.PercentageDoneExact = n * 100M;
            su.CurrentFileSize = FileSize.Of2(audioJob.Output);
            su.TimeElapsed = DateTime.Now - _start;
            su.FillValues();
            raiseEvent();
        }

        private void raiseEvent(string s)
        {
            su.Status = s;
            raiseEvent();
        }


        internal AviSynthAudioEncoder(MeGUISettings settings)
        {
            su.JobType = JobTypes.AUDIO;
            SAMPLES_PER_UPDATE = (int)settings.AudioSamplesPerUpdate;
            _settings = settings;
        }

        private void readStdOut()
        {
            readStdStream(true);
        }

        private void readStdErr()
        {
            readStdStream(false);
        }

        private string cleanUpString(string s)
        {
            return _cleanUpStringRegex.Replace(s.Replace(Environment.NewLine, "\n"), Environment.NewLine);
        }

        private void readStdStream(bool bStdOut)
        {
            //EncoderCallbackEventArgs e = new EncoderCallbackEventArgs(bStdOut?EncoderCallbackEventArgs.EventType.StdOut:EncoderCallbackEventArgs.EventType.StdErr);
            using (StreamReader r = bStdOut ? _encoderProcess.StandardOutput : _encoderProcess.StandardError)
            {
                while (!_encoderProcess.HasExited)
                {
                    Thread.Sleep(0);
                    string text1 = r.ReadToEnd(); //r.ReadLine();
                    if (text1 != null)
                    {
                        if (text1.Length > 0)
                        {
                            if (bStdOut)
                                _encoderStdOut = cleanUpString(text1);
                            else
                                _encoderStdErr = cleanUpString(text1);
                        }
                    }
                    Thread.Sleep(0);
                }
            }
        }

        private void encode()
        {
            try
            {

                using (AviSynthScriptEnvironment env = new AviSynthScriptEnvironment())
                {
                    using (AviSynthClip a = env.ParseScript(_avisynthAudioScript))
                    {
                        if (0 == a.ChannelsCount)
                            throw new ApplicationException("Can't find audio stream");

                        _logBuilder.AppendFormat("Channels={0}, BitsPerSample={1}, SampleRate={2}Hz{3}", a.ChannelsCount, a.BitsPerSample, a.AudioSampleRate, Environment.NewLine);
                        _start = DateTime.Now;

                        const int MAX_SAMPLES_PER_ONCE = 4096;
                        int frameSample = 0;
                        int lastUpdateSample = 0;
                        int frameBufferTotalSize = MAX_SAMPLES_PER_ONCE * a.ChannelsCount * a.BytesPerSample;
                        byte[] frameBuffer = new byte[frameBufferTotalSize];
                        createEncoderProcess(a);
                        try
                        {
                            using (Stream target = _encoderProcess.StandardInput.BaseStream)
                            {
                                // let's write WAV Header
                                if (_mustSendWavHeaderToEncoderStdIn)
                                    writeHeader(target, a);

                                _sampleRate = a.AudioSampleRate;

                                raiseEvent("Preprocessing...");
                                bool hasStartedEncoding = false;

                                GCHandle h = GCHandle.Alloc(frameBuffer, GCHandleType.Pinned);
                                IntPtr address = h.AddrOfPinnedObject();
                                try
                                {
                                    su.ClipLength = TimeSpan.FromSeconds((double)a.SamplesCount / (double)_sampleRate);
                                    while (frameSample < a.SamplesCount)
                                    {
                                        _mre.WaitOne();
                                        
                                        if (_encoderProcess != null)
                                            if (_encoderProcess.HasExited)
                                                throw new ApplicationException("Abnormal encoder termination " + _encoderProcess.ExitCode.ToString());
                                        int nHowMany = Math.Min((int)(a.SamplesCount - frameSample), MAX_SAMPLES_PER_ONCE);
                                        a.ReadAudio(address, frameSample, nHowMany);
                                        
                                        _mre.WaitOne();
                                        if (!hasStartedEncoding)
                                        {
                                            raiseEvent("Encoding audio...");
                                            hasStartedEncoding = true;
                                        }


                                        target.Write(frameBuffer, 0, nHowMany * a.ChannelsCount * a.BytesPerSample);
                                        target.Flush();
                                        frameSample += nHowMany;
                                        if (frameSample - lastUpdateSample > SAMPLES_PER_UPDATE)
                                        {
                                            setProgress((decimal)frameSample / (decimal)a.SamplesCount);
                                            lastUpdateSample = frameSample;
                                        }
                                        Thread.Sleep(0);
                                    }
                                }
                                finally
                                {
                                    h.Free();
                                }
                                setProgress(1M);

                                if (_mustSendWavHeaderToEncoderStdIn && a.BytesPerSample % 2 == 1)
                                    target.WriteByte(0);
                            }
                            raiseEvent("Finalizing encoder");
                            _encoderProcess.WaitForExit();
                            _readFromStdErrThread.Join();
                            _readFromStdOutThread.Join();
                            if (0 != _encoderProcess.ExitCode)
                                throw new ApplicationException("Abnormal encoder termination " + _encoderProcess.ExitCode.ToString());

                        }
                        finally
                        {
                            if (!_encoderProcess.HasExited)
                            {
                                _encoderProcess.Kill();
                                _encoderProcess.WaitForExit();
                                _readFromStdErrThread.Join();
                                _readFromStdOutThread.Join();
                            }
                            _readFromStdErrThread = null;
                            _readFromStdOutThread = null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                deleteOutputFile();
                if (e is ThreadAbortException)
                {
                    _logBuilder.Append("ABORTING!\n");
                    su.WasAborted = true;
                    raiseEvent();
                }
                else
                {
                    _logBuilder.Append("Error:\n" + e.ToString());
                    su.HasError = true;
                    raiseEvent();
                }
                return;
            }
            finally
            {
                deleteTempFiles();
            }
            su.IsComplete = true;
            raiseEvent();
        }

        private string createLog()
        {
            if (_encoderStdErr != null)
                _logBuilder.Append(_encoderStdErr + Environment.NewLine);

            if (_encoderStdOut != null)
                _logBuilder.Append(_encoderStdOut + Environment.NewLine);

            return _logBuilder.ToString().Replace(Environment.NewLine, "\n").Replace("\n", Environment.NewLine);
        }

        private void deleteOutputFile()
        {
            safeDelete(audioJob.Output);
        }

        private void createEncoderProcess(AviSynthClip a)
        {
            try
            {
                _encoderProcess = new Process();
                ProcessStartInfo info = new ProcessStartInfo();
                // Command line arguments, to be passed to encoder
                // {0} means output file name
                // {1} means samplerate in Hz
                // {2} means bits per sample
                // {3} means channel count
                // {4} means samplecount
                // {5} means size in bytes
                info.Arguments = string.Format(_encoderCommandLine,
                    audioJob.Output, a.AudioSampleRate, a.BitsPerSample, a.ChannelsCount, a.SamplesCount, a.AudioSizeInBytes);
                info.FileName = _encoderExecutablePath;
                _logBuilder.AppendFormat("{0} {1}", _encoderExecutablePath, info.Arguments, Environment.NewLine);
                info.UseShellExecute = false;
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.CreateNoWindow = true;
                _encoderProcess.StartInfo = info;
                _encoderProcess.Start();
                _encoderProcess.PriorityClass = ProcessPriorityClass.Idle;
                _readFromStdOutThread = new Thread(new ThreadStart(readStdOut));
                _readFromStdErrThread = new Thread(new ThreadStart(readStdErr));
                _readFromStdOutThread.Start();
                _readFromStdOutThread.Priority = ThreadPriority.Normal;
                _readFromStdErrThread.Start();
                _readFromStdErrThread.Priority = ThreadPriority.Normal;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Can't start encoder: " + e.Message, e);
            }
        }

        private void writeHeader(Stream target, AviSynthClip a)
        {
            const uint FAAD_MAGIC_VALUE = 0xFFFFFF00;
            const uint WAV_HEADER_SIZE = 36;
            bool useFaadTrick = a.AudioSizeInBytes >= (uint.MaxValue - WAV_HEADER_SIZE);
            target.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, 4);
            target.Write(BitConverter.GetBytes(useFaadTrick ? FAAD_MAGIC_VALUE : (uint)(a.AudioSizeInBytes + WAV_HEADER_SIZE)), 0, 4);
            target.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "), 0, 8);
            target.Write(BitConverter.GetBytes((uint)0x10), 0, 4);
            target.Write(BitConverter.GetBytes((short)0x01), 0, 2);
            target.Write(BitConverter.GetBytes(a.ChannelsCount), 0, 2);
            target.Write(BitConverter.GetBytes(a.AudioSampleRate), 0, 4);
            target.Write(BitConverter.GetBytes(a.AvgBytesPerSec), 0, 4);
            target.Write(BitConverter.GetBytes(a.BytesPerSample*a.ChannelsCount), 0, 2);
            target.Write(BitConverter.GetBytes(a.BitsPerSample), 0, 2);
            target.Write(System.Text.Encoding.ASCII.GetBytes("data"), 0, 4);
            target.Write(BitConverter.GetBytes(useFaadTrick ? (FAAD_MAGIC_VALUE - WAV_HEADER_SIZE) : (uint)a.AudioSizeInBytes), 0, 4);
        }


        internal void Start()
        {
            _encoderThread = new Thread(new ThreadStart(this.encode));
            _encoderThread.Priority = ThreadPriority.Lowest;
            _encoderThread.Start();
        }

        internal void Abort()
        {
            _encoderThread.Abort();
            _encoderThread = null;
        }

        #endregion

        #region IJobProcessor Members


        public void setup(Job job)
        {
            this.audioJob = (AudioJob)job;
            su.JobName = audioJob.Name;


            //let's create avisynth script
            StringBuilder script = new StringBuilder();

            string id = _uniqueId;
            string tmp = Path.Combine(Path.GetTempPath(), id);



            bool directShow = audioJob.Settings.ForceDecodingViaDirectShow;
            if (!directShow)
            {
                switch (Path.GetExtension(audioJob.InputFileName).ToLower())
                {
                    case ".ac3":
                        script.AppendFormat("NicAc3Source(\"{0}\"", audioJob.Input);
                        if (audioJob.Settings.AutoGain)
                            script.AppendFormat(", DRC=1){0}", Environment.NewLine);
                        else
                            script.Append(")");
                        break;
                    case ".avs":
                        script.AppendFormat("Import(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                        break;
                    case ".wav":
                        script.AppendFormat("WavSource(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                        break;
                    case ".dts":
                        script.AppendFormat("NicDtsSource(\"{0}\")", audioJob.Input);
                        if (audioJob.Settings.AutoGain)
                            script.AppendFormat(", DRC=1){0}", Environment.NewLine);
                        else
                            script.Append(")");
                        break;
                    case ".mpa":
                    case ".mpg":
                    case ".mp2":
                        script.AppendFormat("NicMPASource(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                        break;
                    case ".mp3":
                        script.AppendFormat("NicMPG123Source(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                        break;
                    default:
                        directShow = true;
                        break;
                }
            }
            if (directShow)
                script.AppendFormat("DirectShowSource(\"{0}\"){1}", audioJob.Input, Environment.NewLine);

            script.AppendFormat("EnsureVBRMP3Sync(){0}", Environment.NewLine);


            if (audioJob.Settings.DelayEnabled && audioJob.Settings.Delay != 0)
                script.AppendFormat("DelayAudio({0}.0/1000.0){1}", audioJob.Settings.Delay, Environment.NewLine);

            if (audioJob.Settings.ImproveAccuracy || audioJob.Settings.AutoGain /* to fix the bug */)
                script.AppendFormat("ConvertAudioToFloat(){0}", Environment.NewLine);

            if (!string.IsNullOrEmpty(audioJob.CutFile))
            {
                try
                {
                    Cuts cuts = FilmCutter.ReadCutsFromFile(audioJob.CutFile);
                    script.AppendLine(FilmCutter.GetCutsScript(cuts, true));
                }
                catch (FileNotFoundException)
                {
                    deleteTempFiles();
                    throw new MissingFileException(audioJob.CutFile);
                }
                catch (Exception)
                {
                    deleteTempFiles();
                    throw new JobRunException("Broken cuts file, " + audioJob.CutFile + ", can't continue.");
                }
            }

            if (audioJob.Settings.AutoGain)
                script.AppendFormat("Normalize(){0}", Environment.NewLine);

            switch (audioJob.Settings.DownmixMode)
            {
                case ChannelMode.KeepOriginal:
                    break;
                case ChannelMode.ConvertToMono:
                    script.AppendFormat("ConvertToMono(){0}", Environment.NewLine);
                    break;
                case ChannelMode.DPLDownmix:
                    script.Append("6<=Audiochannels(last)?x_dpl" + id + @"(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                    break;
                case ChannelMode.DPLIIDownmix:
                    script.Append("6<=Audiochannels(last)?x_dpl2" + id + @"(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                    break;
                case ChannelMode.StereoDownmix:
                    script.Append("6<=Audiochannels(last)?x_stereo" + id + @"(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                    break;
                case ChannelMode.Upmix:
                    createTemporallyEqFiles(tmp);
                    script.Append("2==Audiochannels(last)?x_upmix" + id + @"(last):last" + Environment.NewLine);
                    break;
                case ChannelMode.UpmixUsingSoxEq:
                    script.Append("2==Audiochannels(last)?x_upmixR" + id + @"(last):last" + Environment.NewLine);
                    break;
                case ChannelMode.UpmixWithCenterChannelDialog:
                    script.Append("2==Audiochannels(last)?x_upmixC" + id + @"(last):last" + Environment.NewLine);
                    break;
            }

            //let's obtain command line & other staff
            if (audioJob.Settings is AC3Settings)
            {
                script.Append("6<=Audiochannels(last)?GetChannel(last,1,3,2,5,6,4):last" + Environment.NewLine);
                _mustSendWavHeaderToEncoderStdIn = true;
                AC3Settings n = audioJob.Settings as AC3Settings;
                _encoderExecutablePath = this._settings.FFMpegPath;
                _encoderCommandLine = "-i - -y -acodec ac3 -ab " + n.Bitrate + "k \"{0}\"";
            }
            if (audioJob.Settings is MP2Settings)
            {
                _mustSendWavHeaderToEncoderStdIn = true;
                MP2Settings n = audioJob.Settings as MP2Settings;
                _encoderExecutablePath = this._settings.FFMpegPath;
                _encoderCommandLine = "-i - -y -acodec mp2 -ab " + n.Bitrate + "k \"{0}\"";
            } 
            if (audioJob.Settings is WinAmpAACSettings)
            {
                _mustSendWavHeaderToEncoderStdIn = false;
                WinAmpAACSettings n = audioJob.Settings as WinAmpAACSettings;
                _encoderExecutablePath = this._settings.EncAacPlusPath;
                StringBuilder sb = new StringBuilder("- \"{0}\" --rawpcm {1} {3} {2} --cbr ");
                sb.Append(n.Bitrate * 1000);
                if (n.Mpeg2AAC)
                    sb.Append(" --mpeg2aac");
                switch (n.Profile)
                {
                    case AacProfile.PS:
                        break;
                    case AacProfile.HE:
                        sb.Append(" --nops");
                        break;
                    case AacProfile.LC:
                        sb.Append(" --lc");
                        break;
                }
                switch (n.StereoMode)
                {
                    case WinAmpAACSettings.AacStereoMode.Dual:
                        sb.Append(" --dc");
                        break;
                    case WinAmpAACSettings.AacStereoMode.Joint:
                        break;
                    case WinAmpAACSettings.AacStereoMode.Independent:
                        sb.Append(" --is");
                        break;

                }
                _encoderCommandLine = sb.ToString();
            }

            if (audioJob.Settings is AudXSettings)
            {
                script.Append("ResampleAudio(last,48000)" + Environment.NewLine);
                script.Append("6==Audiochannels(last)?last:GetChannel(last,1,1,1,1,1,1)" + Environment.NewLine);
                _mustSendWavHeaderToEncoderStdIn = false;
                AudXSettings n = audioJob.Settings as AudXSettings;
                _encoderExecutablePath = this._settings.EncAudXPath;
                _encoderCommandLine = "- \"{0}\" --q " + ((int)n.Quality) + " --raw {1}";
            }
            if (audioJob.Settings is OggVorbisSettings)
            {
                // http://forum.doom9.org/showthread.php?p=831098#post831098
                //if(!this._settings.FreshOggEnc2)
                    script.Append("6==Audiochannels(last)?GetChannel(last,1,3,2,5,6,4):last" + Environment.NewLine);
                _mustSendWavHeaderToEncoderStdIn = false;
                OggVorbisSettings n = audioJob.Settings as OggVorbisSettings;
                _encoderExecutablePath = this._settings.OggEnc2Path;
                _encoderCommandLine = "-Q --raw --raw-bits={2} --raw-chan={3} --raw-rate={1} --quality " + n.Quality.ToString(System.Globalization.CultureInfo.InvariantCulture) + " -o \"{0}\" -";
            }
            if (audioJob.Settings is NeroAACSettings)
            {
                _mustSendWavHeaderToEncoderStdIn = true;
                NeroAACSettings n = audioJob.Settings as NeroAACSettings;
                NeroAACSettings nas = n;
                _encoderExecutablePath = this._settings.NeroAacEncPath;
                StringBuilder sb = new StringBuilder("-ignorelength ");
                switch (n.Profile)
                {
                    case AacProfile.HE:
                        sb.Append("-he ");
                        break;
                    case AacProfile.PS:
                        sb.Append("-hev2 ");
                        break;
                    case AacProfile.LC:
                        sb.Append("-lc ");
                        break;
                }
                if (n.CreateHintTrack)
                    sb.Append("-hinttrack ");

                switch (n.BitrateMode)
                {
                    case BitrateManagementMode.ABR:
                        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "-br {0} ", n.Bitrate*1000);
                        break;
                    case BitrateManagementMode.CBR:
                        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "-cbr {0} ", n.Bitrate*1000);
                        break;
                    case BitrateManagementMode.VBR:
                        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "-q {0} ", n.Quality);
                        break;
                }

                sb.Append("-if - -of \"{0}\"");

                _encoderCommandLine = sb.ToString();
            }
            if (audioJob.Settings is FaacSettings)
            {
                FaacSettings f = audioJob.Settings as FaacSettings;
                _encoderExecutablePath = this._settings.FaacPath;
                _mustSendWavHeaderToEncoderStdIn = false;
                switch (f.BitrateMode)
                {
                    // {0} means output file name
                    // {1} means samplerate in Hz
                    // {2} means bits per sample
                    // {3} means channel count
                    // {4} means samplecount
                    // {5} means size in bytes

                    case BitrateManagementMode.VBR:
                        _encoderCommandLine = "-q " + f.Quality + " -o \"{0}\" -P -X -R {1} -B {2} -C {3} --mpeg-vers 4 -";
                        break;
                    default:
                        _encoderCommandLine = "-b " + f.Bitrate + " -o \"{0}\" -P -X -R {1} -B {2} -C {3} --mpeg-vers 4 -";
                        break;
                }
            }
            if (audioJob.Settings is MP3Settings)
            {
                MP3Settings m = audioJob.Settings as MP3Settings;
                _mustSendWavHeaderToEncoderStdIn = true;
                _encoderExecutablePath = this._settings.LamePath;

                switch (m.BitrateMode)
                {
                    case BitrateManagementMode.VBR:
                        _encoderCommandLine = "-V " + (m.Quality / 10 - 1) + " -h --silent - \"{0}\"";
                        break;
                    case BitrateManagementMode.CBR:
                        _encoderCommandLine = "-b " + m.Bitrate + " --cbr -h --silent - \"{0}\"";
                        break;
                    case BitrateManagementMode.ABR:
                        _encoderCommandLine = "--abr " + m.Bitrate + " -h --silent - \"{0}\"";
                        break;
                }


            }

            //Just check encoder existance
            _encoderExecutablePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, _encoderExecutablePath);
            if (!File.Exists(_encoderExecutablePath))
            {
                deleteTempFiles();
                throw new EncoderMissingException(_encoderExecutablePath);
            }

            script.AppendFormat("ConvertAudioTo16bit(){0}", Environment.NewLine);


            script.AppendLine(
@"

return last

function x_dpl" + id + @"(clip a) 
{
	fl = GetChannel(a, 1)
	fr = GetChannel(a, 2)
	c = GetChannel(a, 3)
	sl = GetChannel(a, 5)
	sr = GetChannel(a, 6)
	ssr = MixAudio(sl, sr, 0.2222, 0.2222)
	ssl = Amplify(ssr, -1.0)
	fl_c = MixAudio(fl, c, 0.3254, 0.2301)
	fr_c = MixAudio(fr, c, 0.3254, 0.2301)
	l = MixAudio(ssl, fl_c, 1.0, 1.0)
	r = MixAudio(ssr, fr_c, 1.0, 1.0)
	return MergeChannels(l, r)
}

function x_dpl2" + id + @"(clip a) 
{
	fl = GetChannel(a, 1)
	fr = GetChannel(a, 2)
	c = GetChannel(a, 3)
	sl = GetChannel(a, 5)
	sr = GetChannel(a, 6)
	ssl = MixAudio(sl, sr, 0.2818, 0.1627).Amplify(-1.0)
	fl_c = MixAudio(fl, c, 0.3254, 0.2301)
	ssr = MixAudio(sl, sr, 0.1627, 0.2818)
	fr_c = MixAudio(fr, c, 0.3254, 0.2301)
	l = MixAudio(ssl, fl_c, 1.0, 1.0)
	r = MixAudio(ssr, fr_c, 1.0, 1.0)
	return MergeChannels(l, r)
}

function x_stereo" + id + @"(clip a) 
{
	fl = GetChannel(a, 1)
	fr = GetChannel(a, 2)
	c = GetChannel(a, 3)
	lfe = GetChannel(a, 4)
	sl = GetChannel(a, 5)
	sr = GetChannel(a, 6)
	l_sl = MixAudio(fl, sl, 0.2929, 0.2929)
	c_lfe = MixAudio(lfe, c, 0.2071, 0.2071)
	r_sr = MixAudio(fr, sr, 0.2929, 0.2929)
	l = MixAudio(l_sl, c_lfe, 1.0, 1.0)
	r = MixAudio(r_sr, c_lfe, 1.0, 1.0)
	return MergeChannels(l, r)
}

function x_upmix" + id + @"(clip a) 
{
    m = ConvertToMono(a)
    f = SuperEQ(a,""" + tmp + @"front.feq"")
    s = SuperEQ(a,""" + tmp + @"back.feq"") 
    c = SuperEQ(m,""" + tmp + @"center.feq"") 
    lfe = SuperEQ(m,""" + tmp + @"lfe.feq"") 
    return MergeChannels( f.getleftchannel, f.getrightchannel , c, lfe, s.getleftchannel, s.getrightchannel)
}

function x_upmixR" + id + @"(clip Stereo) 
{
	Front = mixaudio(Stereo.soxfilter(""filter 0-600""),mixaudio(Stereo.soxfilter(""filter 600-1200""),Stereo.soxfilter(""filter 1200-7000""),0.45,0.25),0.50,1)
	Back = mixaudio(Stereo.soxfilter(""filter 0-600""),mixaudio(Stereo.soxfilter(""filter 600-1200""),Stereo.soxfilter(""filter 1200-7000""),0.35,0.15),0.40,1)
	fl = GetLeftChannel(Front)
	fr = GetRightChannel(Front)
	cc = ConvertToMono(stereo).SoxFilter(""filter 625-24000"")
	lfe = ConvertToMono(stereo).SoxFilter(""lowpass 100"",""vol -0.5"")
	sl = GetLeftChannel(Back)
	sr = GetRightChannel(Back)
	sl = DelayAudio(sl,0.02)
	sr = DelayAudio(sr,0.02)
    return MergeChannels(fl,fr,cc,lfe,sl,sr)
}

function x_upmixC" + id + @"(clip stereo) 
{
	left = stereo.GetLeftChannel()
	right = stereo.GetRightChannel()
	fl = mixaudio(left.soxfilter(""filter 0-24000""),right.soxfilter(""filter 0-24000""),0.6,-0.5)
	fr = mixaudio(right.soxfilter(""filter 0-24000""),left.soxfilter(""filter 0-24000""),0.6,-0.5)
	cc = ConvertToMono(stereo).SoxFilter(""filter 625-24000"")
	lfe = ConvertToMono(stereo).SoxFilter(""lowpass 100"",""vol -0.5"")
	sl = mixaudio(left.soxfilter(""filter 0-24000""),right.soxfilter(""filter 0-24000""),0.5,-0.4)
	sr = mixaudio(right.soxfilter(""filter 0-24000""),left.soxfilter(""filter 0-24000""),0.5,-0.4)
	sl = DelayAudio(sl,0.02)
	sr = DelayAudio(sr,0.02)
    return MergeChannels(fl,fr,cc,lfe,sl,sr)
}
                                                                                                                                                     

"
        );
            _avisynthAudioScript = script.ToString();

        }

        public void start()
        {
            try
            {
                this.Start();
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }

        public void stop()
        {
            try
            {
                this.Abort();
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }

        public void pause()
        {
            if (!_mre.Reset())
                throw new JobRunException("Could not reset mutex. pause failed");
        }

        public void resume()
        {
            if (!_mre.Set())
                throw new JobRunException("Could not set mutex. pause failed");
        }

        public void changePriority(ProcessPriority priority)
        {
            if (this._encoderThread != null && _encoderThread.IsAlive)
            {
                try
                {
                    if (priority == ProcessPriority.IDLE)
                        _encoderThread.Priority = ThreadPriority.Lowest;
                    else if (priority == ProcessPriority.NORMAL)
                        _encoderThread.Priority = ThreadPriority.Normal;
                    else if (priority == ProcessPriority.HIGH)
                        _encoderThread.Priority = ThreadPriority.AboveNormal;
                    return;
                }
                catch (Exception e) // process could not be running anymore
                {
                    throw new JobRunException(e);
                }
            }
            else
            {
                if (_encoderThread == null)
                    throw new JobRunException("Thread has not been started yet");
                else
                    throw new JobRunException("Thread has exited");
            }
        }

        public event JobProcessingStatusUpdateCallback StatusUpdate;
        #endregion
    }
}
