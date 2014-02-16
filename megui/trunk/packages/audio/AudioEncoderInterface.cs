// ****************************************************************************
// 
// Copyright (C) 2005-2014 Doom9 & al
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MeGUI.core.util;

using MediaInfoWrapper;

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
                ((j as AudioJob).Settings is OggVorbisSettings) ||
                ((j as AudioJob).Settings is NeroAACSettings) ||
                ((j as AudioJob).Settings is FlacSettings) ||
                ((j as AudioJob).Settings is AftenSettings) ||
                ((j as AudioJob).Settings is QaacSettings) ||
                ((j as AudioJob).Settings is OpusSettings)))
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

        private ManualResetEvent _mre = new System.Threading.ManualResetEvent(true); // lock used to pause encoding
        private Thread _encoderThread = null;

        private ManualResetEvent stdoutDone = new ManualResetEvent(false);
        private ManualResetEvent stderrDone = new ManualResetEvent(false);
        private Thread _readFromStdOutThread = null;
        private Thread _readFromStdErrThread = null;
        private LogItem stdoutLog;
        private LogItem stderrLog;
        private LogItem _log;
        private string _encoderStdErr;
        private static readonly System.Text.RegularExpressions.Regex _cleanUpStringRegex = new System.Text.RegularExpressions.Regex(@"\n[^\n]+\r", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        private MeGUISettings _settings = null;
        private AudioJob audioJob;
        private StatusUpdate su;
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
            {
                _mre.Set();  // Make sure nothing is waiting for pause to stop
                stdoutDone.WaitOne(); // wait for stdout to finish processing
                stderrDone.WaitOne(); // wait for stderr to finish processing

                if (!su.HasError && !su.WasAborted)
                {
                    if (!String.IsNullOrEmpty(audioJob.Output) && File.Exists(audioJob.Output))
                    {
                        MediaInfoFile oInfo = new MediaInfoFile(audioJob.Output, ref _log);
                    }
                }
                else if (su.HasError && audioJob.Settings is QaacSettings && _encoderStdErr.ToLowerInvariant().Contains("coreaudiotoolbox.dll"))
                {
                    _log.LogEvent("CoreAudioToolbox.dll is missing and must be installed. Please have a look at https://sites.google.com/site/qaacpage", ImageType.Error);
                    if (MessageBox.Show("CoreAudioToolbox.dll is missing and must be installed.\r\nOtherwise QAAC cannot be used.\r\n\r\nDo you want to open the installation instructions?", "CoreAudioToolbox.dll missing", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                        System.Diagnostics.Process.Start("https://sites.google.com/site/qaacpage");
                }
            }
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

        private void updateTime()
        {
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
            _settings = settings;
        }

        private void readStdOut()
        {
            StreamReader sr = null;
            try
            {
                sr = _encoderProcess.StandardOutput;
            }
            catch (Exception e)
            {
                _log.LogValue("Exception getting IO reader for stdout", e, ImageType.Error);
                stdoutDone.Set();
                return;
            }
            readStream(sr, stdoutDone, StreamType.Stdout);
        }

        private void readStdErr()
        {
            StreamReader sr = null;
            try
            {
                sr = _encoderProcess.StandardError;
            }
            catch (Exception e)
            {
                _log.LogValue("Exception getting IO reader for stderr", e, ImageType.Error);
                stderrDone.Set();
                return;
            }
            readStream(sr, stderrDone, StreamType.Stderr);
        }

        private void readStream(StreamReader sr, ManualResetEvent rEvent, StreamType str)
        {
            string line;
            if (_encoderProcess != null)
            {
                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        _mre.WaitOne();
                        ProcessLine(line, str, ImageType.Information);
                    }
                }
                catch (Exception e)
                {
                    ProcessLine("Exception in readStream. Line cannot be processed. " + e.Message, str, ImageType.Error);
                }
                rEvent.Set();
            }
        }

        private void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            line = _cleanUpStringRegex.Replace(line.Replace(Environment.NewLine, "\n"), Environment.NewLine);
            if (String.IsNullOrEmpty(line.Trim()))
                return;

            if (audioJob.Settings is QaacSettings)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line, @"^[0-9]*:"))
                    return;
                if (line.ToLowerInvariant().StartsWith("error:"))
                    oType = ImageType.Error;
            }
            else if (audioJob.Settings is OggVorbisSettings)
            {
                if (line.ToLowerInvariant().StartsWith("\tencoding ["))
                    return;
            }
            else if (audioJob.Settings is NeroAACSettings)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line.ToLowerInvariant(), @"^processed\s?[0-9]{0,5}\s?seconds..."))
                    return;
            }
            else if (audioJob.Settings is AftenSettings)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line.ToLowerInvariant(), @"^progress: "))
                    return;
            }
            else if (audioJob.Settings is AC3Settings || audioJob.Settings is MP2Settings)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(line.ToLowerInvariant(), @"^size= "))
                    return;
            }

            if (stream == StreamType.Stderr)
                _encoderStdErr += line + "\n";

            if (stream == StreamType.Stdout)
                stdoutLog.LogEvent(line, oType);
            if (stream == StreamType.Stderr)
                stderrLog.LogEvent(line, oType);

            if (oType == ImageType.Error)
                su.HasError = true;
        }

        private void encode()
        {
            string fileErr = MainForm.verifyInputFile(this.audioJob.Input);
            if (fileErr != null)
            {
                _log.LogEvent("Problem with audio input file: " + fileErr, ImageType.Error);
                su.HasError = su.IsComplete = true;
                raiseEvent();
                return;
            }

            Thread t = null;
            try
            {
                raiseEvent("Opening file....please wait, it may take some time");
                _start = DateTime.Now;
                t = new Thread(new ThreadStart(delegate
                {
                    while (true)
                    {
                        updateTime();
                        Thread.Sleep(1000);
                    }
                }));
                t.Start();
                createAviSynthScript();
                raiseEvent("Preprocessing...please wait, it may take some time");
                _start = DateTime.Now;
                using (AviSynthScriptEnvironment env = new AviSynthScriptEnvironment())
                {
                    _log.LogEvent("Avisynth script environment opened");
                    using (AviSynthClip a = env.ParseScript(_avisynthAudioScript))
                    {
                        _log.LogEvent("Script loaded");
                        if (0 == a.ChannelsCount)
                            throw new ApplicationException("Can't find audio stream");

                        LogItem inputLog = _log.LogEvent("Output Decoder", ImageType.Information);
                        inputLog.LogValue("Channels", a.ChannelsCount);
                        inputLog.LogValue("Bits per sample", a.BitsPerSample);
                        inputLog.LogValue("Sample rate", a.AudioSampleRate);

                        if (audioJob.Settings is FlacSettings)
                            _encoderCommandLine += " --channels=" + a.ChannelsCount + " --bps=" + a.BitsPerSample + " --sample-rate=" + a.AudioSampleRate;

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
                                        {
                                            if (_encoderProcess.HasExited)
                                            {
                                                string strError = WindowUtil.GetErrorText(_encoderProcess.ExitCode);
                                                throw new ApplicationException("Abnormal encoder termination. Exit code: " + strError);
                                            }
                                        }
                                        int nHowMany = Math.Min((int)(a.SamplesCount - frameSample), MAX_SAMPLES_PER_ONCE);

                                        a.ReadAudio(address, frameSample, nHowMany);
                                        
                                        _mre.WaitOne();
                                        if (!hasStartedEncoding)
                                        {
                                            t.Abort();
                                            raiseEvent("Encoding audio...");
                                            hasStartedEncoding = true;
                                        }

                                        target.Write(frameBuffer, 0, nHowMany * a.ChannelsCount * a.BytesPerSample);
                                        target.Flush();
                                        frameSample += nHowMany;
                                        if (frameSample - lastUpdateSample > 100000) // 100000 samples per update
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
                            while (!_encoderProcess.HasExited) // wait until the process has terminated without locking the GUI
                            {
                                System.Windows.Forms.Application.DoEvents();
                                System.Threading.Thread.Sleep(100);
                            }
                            _encoderProcess.WaitForExit();
                            _readFromStdErrThread.Join();
                            _readFromStdOutThread.Join();
                            if (0 != _encoderProcess.ExitCode)
                            {
                                string strError = WindowUtil.GetErrorText(_encoderProcess.ExitCode);
                                throw new ApplicationException("Abnormal encoder termination. Exit code: " + strError);
                            }
                        }
                        finally
                        {
                            if (!_encoderProcess.HasExited)
                            {
                                _encoderProcess.Kill();
                                while (!_encoderProcess.HasExited) // wait until the process has terminated without locking the GUI
                                {
                                    System.Windows.Forms.Application.DoEvents();
                                    System.Threading.Thread.Sleep(100);
                                }
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
                    _log.LogEvent("Aborting...");
                    su.WasAborted = true;
                    raiseEvent();
                    return;
                }
                else if (e is AviSynthException)
                {
                    stderrDone.Set();
                    stdoutDone.Set();
                    _log.LogValue("An error occurred", e.Message, ImageType.Error);
                    su.HasError = true;
                }
                else if (e is ApplicationException)
                {
                    _log.LogValue("An error occurred", e.Message, ImageType.Error);
                    su.HasError = true;
                }  
                else
                {
                    _log.LogValue("An error occurred", e, ImageType.Error);
                    su.HasError = true;
                    raiseEvent();
                    return;
                }
            }
            finally
            {
                t.Abort();
                deleteTempFiles();
            }
            su.IsComplete = true;
            raiseEvent();
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
                _log.LogValue("Job commandline", _encoderExecutablePath + " " + info.Arguments);
                info.UseShellExecute = false;
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.CreateNoWindow = true;
                _encoderProcess.StartInfo = info;
                _encoderProcess.Start();

                // Take priority from Avisynth thread rather than default in settings
                // just in case user has managed to change job setting before getting here.
                if (_encoderThread.Priority == ThreadPriority.Lowest)
                    this.changePriority(ProcessPriority.IDLE);
                else if (_encoderThread.Priority == ThreadPriority.BelowNormal)
                    this.changePriority(ProcessPriority.BELOW_NORMAL);
                else if (_encoderThread.Priority == ThreadPriority.Normal)
                    this.changePriority(ProcessPriority.NORMAL);
                else if (_encoderThread.Priority == ThreadPriority.AboveNormal)
                    this.changePriority(ProcessPriority.ABOVE_NORMAL);
                else if (_encoderThread.Priority == ThreadPriority.Highest)
                    this.changePriority(ProcessPriority.HIGH);

                _log.LogEvent("Process started");
                stdoutLog = _log.Info(string.Format("[{0:G}] {1}", DateTime.Now, "Standard output stream"));
                stderrLog = _log.Info(string.Format("[{0:G}] {1}", DateTime.Now, "Standard error stream"));
                _readFromStdOutThread = new Thread(new ThreadStart(readStdOut));
                _readFromStdErrThread = new Thread(new ThreadStart(readStdErr));
                _readFromStdOutThread.Start();
                _readFromStdErrThread.Start();
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
            bool useFaadTrick = a.AudioSizeInBytes >= ((long)uint.MaxValue - WAV_HEADER_SIZE);
            target.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, 4);
            target.Write(BitConverter.GetBytes(useFaadTrick ? FAAD_MAGIC_VALUE : (uint)(a.AudioSizeInBytes + WAV_HEADER_SIZE)), 0, 4);
            target.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "), 0, 8);
            target.Write(BitConverter.GetBytes((uint)0x10), 0, 4);
            target.Write(BitConverter.GetBytes((a.SampleType==AudioSampleType.FLOAT) ? (short)0x03 : (short)0x01), 0, 2);
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
            Util.ensureExists(audioJob.Input);
            _encoderThread = new Thread(new ThreadStart(this.encode));
            if (MainForm.Instance.Settings.ProcessingPriority == ProcessPriority.HIGH)
                _encoderThread.Priority = ThreadPriority.Highest;
            else if (MainForm.Instance.Settings.ProcessingPriority == ProcessPriority.ABOVE_NORMAL)
                _encoderThread.Priority = ThreadPriority.AboveNormal;
            else if (MainForm.Instance.Settings.ProcessingPriority == ProcessPriority.NORMAL)
                _encoderThread.Priority = ThreadPriority.Normal;
            else if (MainForm.Instance.Settings.ProcessingPriority == ProcessPriority.BELOW_NORMAL)
                _encoderThread.Priority = ThreadPriority.BelowNormal;
            else
                _encoderThread.Priority = ThreadPriority.Lowest;
            _encoderThread.Start();
        }

        internal void Abort()
        {
            _encoderThread.Abort();
            _encoderThread = null;
        }

        private bool OpenSourceWithFFAudioSource(out StringBuilder sbOpen)
        {
            sbOpen = new StringBuilder();
            sbOpen.Append(VideoUtil.getFFMSAudioInputLine(audioJob.Input, null, -1));
            _log.LogEvent("Trying to open the file with FFAudioSource()", ImageType.Information);
            string strErrorText = String.Empty;
            if (AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out strErrorText))
            {
                _log.LogEvent("Successfully opened the file with FFAudioSource()", ImageType.Information);
                audioJob.FilesToDelete.Add(audioJob.Input + ".ffindex");
                return true;
            }
            sbOpen = new StringBuilder();
            FileUtil.DeleteFile(audioJob.Input + ".ffindex");
            if (String.IsNullOrEmpty(strErrorText))
                _log.LogEvent("Failed opening the file with FFAudioSource()", ImageType.Information);
            else
                _log.LogEvent("Failed opening the file with FFAudioSource(). " + strErrorText, ImageType.Information);
            return false;
        }

        private bool OpenSourceWithLSMASHAudioSource(out StringBuilder sbOpen)
        {
            sbOpen = new StringBuilder();
            sbOpen.Append(VideoUtil.getLSMASHAudioInputLine(audioJob.Input, null, -1));
            _log.LogEvent("Trying to open the file with LWLibavAudioSource()", ImageType.Information);
            string strErrorText = String.Empty;
            if (AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out strErrorText))
            {
                _log.LogEvent("Successfully opened the file with LWLibavAudioSource()", ImageType.Information);
                audioJob.FilesToDelete.Add(audioJob.Input + ".lwi");
                return true;
            }
            sbOpen = new StringBuilder();
            FileUtil.DeleteFile(audioJob.Input + ".lwi");
            if (String.IsNullOrEmpty(strErrorText))
                _log.LogEvent("Failed opening the file with LWLibavAudioSource()", ImageType.Information);
            else
                _log.LogEvent("Failed opening the file with LWLibavAudioSource(). " + strErrorText, ImageType.Information);
            return false;
        }

        private bool OpenSourceWithBassAudio(out StringBuilder sbOpen)
        {
            sbOpen = new StringBuilder();
            sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.AvisynthPluginsPath), "BassAudio.dll"), Environment.NewLine);
            sbOpen.AppendFormat("BassAudioSource(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
            _log.LogEvent("Trying to open the file with BassAudioSource()", ImageType.Information);
            string strErrorText = String.Empty;
            if (AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out strErrorText))
            {
                _log.LogEvent("Successfully opened the file with BassAudioSource()", ImageType.Information);
                return true;
            }
            sbOpen = new StringBuilder();
            if (String.IsNullOrEmpty(strErrorText))
                _log.LogEvent("Failed opening the file with BassAudioSource()", ImageType.Information);
            else
                _log.LogEvent("Failed opening the file with BassAudioSource(). " + strErrorText, ImageType.Information);
            return false;
        }

        private bool OpenSourceWithDirectShow(out StringBuilder sbOpen, MediaInfoFile oInfo)
        {
            sbOpen = new StringBuilder();

            try
            {
                if (oInfo.HasAudio)
                {
                    if (MainForm.Instance.Settings.PortableAviSynth)
                        sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.AviSynth.Path), @"plugins\directshowsource.dll"), Environment.NewLine);
                    if (oInfo.HasVideo)
                        sbOpen.AppendFormat("DirectShowSource(\"{0}\", video=false){1}", audioJob.Input, Environment.NewLine);
                    else 
                        sbOpen.AppendFormat("DirectShowSource(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                    sbOpen.AppendFormat("EnsureVBRMP3Sync(){0}", Environment.NewLine);
                }
            }
            catch {}

            string strErrorText = String.Empty;
            _log.LogEvent("Trying to open the file with DirectShowSource()", ImageType.Information);
            if (sbOpen.Length > 0 && AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out strErrorText))
            {
                _log.LogEvent("Successfully opened the file with DirectShowSource()", ImageType.Information);
                return true;
            }
            sbOpen = new StringBuilder();
            if (String.IsNullOrEmpty(strErrorText))
                _log.LogEvent("Failed opening the file with DirectShowSource()", ImageType.Information);
            else
                _log.LogEvent("Failed opening the file with DirectShowSource(). " + strErrorText, ImageType.Information);
            return false;
        }

        private bool OpenSourceWithImportAVS(out StringBuilder sbOpen, MediaInfoFile oInfo)
        {
            sbOpen = new StringBuilder();

            try
            {
                if (oInfo.HasAudio)
                    sbOpen.AppendFormat("Import(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
            }
            catch { }

            string strErrorText = String.Empty;
            _log.LogEvent("Trying to open the file with Import()", ImageType.Information);
            if (sbOpen.Length > 0 && AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out strErrorText))
            {
                _log.LogEvent("Successfully opened the file with Import()", ImageType.Information);
                return true;
            }
            sbOpen = new StringBuilder();
            if (String.IsNullOrEmpty(strErrorText))
                _log.LogEvent("Failed opening the file with Import()", ImageType.Information);
            else
                _log.LogEvent("Failed opening the file with Import(). " + strErrorText, ImageType.Information);
            return false;
        }

        private bool OpenSourceWithNicAudio(out StringBuilder sbOpen, MediaInfoFile oInfo, bool bForce)
        {
            sbOpen = new StringBuilder();
            switch (Path.GetExtension(audioJob.Input).ToLower(System.Globalization.CultureInfo.InvariantCulture))
            {
                case ".ac3":
                case ".ddp":
                case ".eac3":
                    sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                    sbOpen.AppendFormat("NicAc3Source(\"{0}\"", audioJob.Input);
                    if (audioJob.Settings.ApplyDRC)
                        sbOpen.AppendFormat(", DRC=1){0}", Environment.NewLine);
                    else
                        sbOpen.AppendFormat("){0}", Environment.NewLine);
                    break;
                case ".avi":
                    sbOpen.AppendFormat("AVISource(\"{0}\", audio=true){1}", audioJob.Input, Environment.NewLine);
                    sbOpen.AppendFormat("EnsureVBRMP3Sync(){0}", Environment.NewLine);
                    sbOpen.AppendFormat("Trim(0,0){0}", Environment.NewLine); // to match audio length
                    break;
                case ".avs":
                    sbOpen.AppendFormat("Import(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                    break;
                case ".dtshd":
                case ".dtsma":
                case ".dts":
                    sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                    sbOpen.AppendFormat("NicDtsSource(\"{0}\"", audioJob.Input);
                    if (audioJob.Settings.ApplyDRC)
                        sbOpen.AppendFormat(", DRC=1){0}", Environment.NewLine);
                    else
                        sbOpen.AppendFormat("){0}", Environment.NewLine);
                    break;
                case ".mpa":
                case ".mpg":
                case ".mp2":
                case ".mp3":
                    sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                    sbOpen.AppendFormat("NicMPG123Source(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                    audioJob.FilesToDelete.Add(audioJob.Input + ".d2a");
                    break;
                case ".wav":
                    BinaryReader r = new BinaryReader(File.Open(audioJob.Input, FileMode.Open, FileAccess.Read));

                    try
                    {
                        r.ReadBytes(20);
                        UInt16 AudioFormat = r.ReadUInt16();  // read a LE int_16, offset 20 + 2 = 22

                        switch (AudioFormat)
                        {
                            case 0x0001:         // PCM Format Int
                                r.ReadBytes(22);   // 22 + 22 = 44
                                UInt32 DtsHeader = r.ReadUInt32(); // read a LE int_32
                                sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                                if (DtsHeader == 0xE8001FFF)
                                {
                                    sbOpen.AppendFormat("NicDtsSource(\"{0}\"", audioJob.Input);
                                    if (audioJob.Settings.ApplyDRC)
                                        sbOpen.AppendFormat(", DRC=1){0}", Environment.NewLine);
                                    else
                                        sbOpen.AppendFormat("){0}", Environment.NewLine);
                                }
                                else
                                    sbOpen.AppendFormat("RaWavSource(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                                break;
                            case 0x0003:         // IEEE Float
                            case 0xFFFE:         // WAVE_FORMAT_EXTENSIBLE header
                                sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                                sbOpen.AppendFormat("RaWavSource(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                                break;
                            case 0x0055:         // MPEG Layer 3
                                sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                                sbOpen.AppendFormat("NicMPG123Source(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                                break;
                            case 0x2000:         // AC3
                                sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                                sbOpen.AppendFormat("NicAc3Source(\"{0}\"", audioJob.Input);
                                if (audioJob.Settings.ApplyDRC)
                                    sbOpen.AppendFormat(", DRC=1){0}", Environment.NewLine);
                                else
                                    sbOpen.AppendFormat("){0}", Environment.NewLine);
                                break;
                            default:
                                sbOpen.AppendFormat("WavSource(\"{0}\"){1}", audioJob.Input, Environment.NewLine);
                                break;
                        }
                    }
                    catch (EndOfStreamException e)
                    {
                        LogItem _oLog = MainForm.Instance.Log.Info("Error");
                        _oLog.LogValue(e.GetType().Name + ", wavfile can't be read.", e, ImageType.Error);
                    }
                    finally
                    {
                        r.Close();
                    }
                    break;
                case ".w64":
                case ".aif":
                case ".au":
                case ".caf":
                case ".bwf":
                    sbOpen.AppendFormat("LoadPlugin(\"{0}\"){1}", Path.Combine(MainForm.Instance.Settings.AvisynthPluginsPath, "NicAudio.dll"), Environment.NewLine);
                    sbOpen.AppendFormat("RaWavSource(\"{0}\", 2){1}", audioJob.Input, Environment.NewLine);
                    break;
            }

            _log.LogEvent("Trying to open the file with NicAudio", ImageType.Information);

            string strErrorText = String.Empty;
            if (oInfo.AudioInfo.Tracks.Count > 0 && oInfo.AudioInfo.Tracks[0].Codec.Equals("DTS-HD Master Audio") && !bForce)
            {
                _log.LogEvent("DTS-MA is blocked in the first place when using NicAudio", ImageType.Information);
            }
            else if (sbOpen.Length > 0 && AudioUtil.AVSScriptHasAudio(sbOpen.ToString(), out strErrorText))
            {
                _log.LogEvent("Successfully opened the file with NicAudio", ImageType.Information);
                return true;
            }
            
            sbOpen = new StringBuilder();
            if (String.IsNullOrEmpty(strErrorText))
                _log.LogEvent("Failed opening the file with NicAudio()", ImageType.Information);
            else
                _log.LogEvent("Failed opening the file with NicAudio(). " + strErrorText, ImageType.Information);
            return false;
        }

        #endregion

        #region IJobProcessor Members


        public void setup(Job job, StatusUpdate su, LogItem log)
        {
            this._log = log;
            this.audioJob = (AudioJob)job;
            this.su = su;
        }

        private void createAviSynthScript()
        {
            //let's create avisynth script
            StringBuilder script = new StringBuilder();

            string id = _uniqueId;
            string tmp = Path.Combine(Path.GetTempPath(), id);

            MediaInfoFile oInfo = new MediaInfoFile(audioJob.Input, ref _log);

            bool bFound = false;
            if (oInfo.ContainerFileTypeString.Equals("AVS"))
            {
                bFound = OpenSourceWithImportAVS(out script, oInfo);
            }
            else if (audioJob.Settings.PreferredDecoder == AudioDecodingEngine.NicAudio)
            {
                bFound = OpenSourceWithNicAudio(out script, oInfo, false);
                if (!bFound)
                    bFound = OpenSourceWithBassAudio(out script);
                if (!bFound)
                    bFound = OpenSourceWithFFAudioSource(out script);
                if (!bFound)
                    bFound = OpenSourceWithLSMASHAudioSource(out script);
                if (!bFound)
                    bFound = OpenSourceWithDirectShow(out script, oInfo);
            }
            else if (audioJob.Settings.PreferredDecoder == AudioDecodingEngine.BassAudio)
            {
                bFound = OpenSourceWithBassAudio(out script);
                if (!bFound)
                    bFound = OpenSourceWithNicAudio(out script, oInfo, false);
                if (!bFound)
                    bFound = OpenSourceWithFFAudioSource(out script);
                if (!bFound)
                    bFound = OpenSourceWithLSMASHAudioSource(out script);
                if (!bFound)
                    bFound = OpenSourceWithDirectShow(out script, oInfo);
            }
            else if (audioJob.Settings.PreferredDecoder == AudioDecodingEngine.LWLibavAudioSource)
            {
                bFound = OpenSourceWithLSMASHAudioSource(out script);
                if (!bFound)
                    bFound = OpenSourceWithNicAudio(out script, oInfo, false);
                if (!bFound)
                    bFound = OpenSourceWithBassAudio(out script);
                if (!bFound)
                    bFound = OpenSourceWithFFAudioSource(out script);
                if (!bFound)
                    bFound = OpenSourceWithDirectShow(out script, oInfo);
            }
            else if (audioJob.Settings.PreferredDecoder == AudioDecodingEngine.FFAudioSource)
            {
                bFound = OpenSourceWithFFAudioSource(out script);
                if (!bFound)
                    bFound = OpenSourceWithNicAudio(out script, oInfo, false);
                if (!bFound)
                    bFound = OpenSourceWithBassAudio(out script);
                if (!bFound)
                    bFound = OpenSourceWithLSMASHAudioSource(out script);
                if (!bFound)
                    bFound = OpenSourceWithDirectShow(out script, oInfo);
            }
            else
            {
                bFound = OpenSourceWithDirectShow(out script, oInfo);
                if (!bFound)
                    bFound = OpenSourceWithNicAudio(out script, oInfo, false);
                if (!bFound)
                    bFound = OpenSourceWithBassAudio(out script);
                if (!bFound)
                    bFound = OpenSourceWithFFAudioSource(out script);
                if (!bFound)
                    bFound = OpenSourceWithLSMASHAudioSource(out script);
            }

            if (!bFound && oInfo.AudioInfo.Tracks.Count > 0 && oInfo.AudioInfo.Tracks[0].Codec.Equals("DTS-HD Master Audio"))
                bFound = OpenSourceWithNicAudio(out script, oInfo, true);

            if (!bFound)
            {
                deleteTempFiles();
                throw new JobRunException("Input file cannot be opened: " + audioJob.Input);
            }

            if (MainForm.Instance.Settings.PortableAviSynth && MainForm.Instance.Settings.AviSynthPlus)
            {
                script.Insert(0, String.Format("ClearAutoloadDirs(){0}AddAutoloadDir(\"{1}\"){0}", 
                    Environment.NewLine, 
                    Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.AviSynth.Path), @"plugins")));
            }

            if (audioJob.Delay != 0)
                script.AppendFormat("DelayAudio({0}.0/1000.0){1}", audioJob.Delay, Environment.NewLine);

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

            switch (audioJob.Settings.DownmixMode)
            {
                case ChannelMode.KeepOriginal:
                    break;
                case ChannelMode.ConvertToMono:
                    script.AppendFormat("ConvertToMono(){0}", Environment.NewLine);
                    break;
                case ChannelMode.DPLDownmix:
                case ChannelMode.DPLIIDownmix:
                case ChannelMode.StereoDownmix:
                    string strChannelPositions;
                    int iChannelCount = 0;
                    if (Path.GetExtension(audioJob.Input).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".avs"))
                    {
                        if (!AudioUtil.AVSFileHasAudio(audioJob.Input))
                        {
                            _log.LogEvent("avs file has no audio: " + audioJob.Input, ImageType.Error);
                            break;
                        }
                        iChannelCount = AudioUtil.getChannelCountFromAVSFile(audioJob.Input);
                        script.Append(@"# detected channels: " + iChannelCount + " channels" + Environment.NewLine);
                        strChannelPositions = AudioUtil.getChannelPositionsFromAVSFile(audioJob.Input);
                    }
                    else
                    {
                        if (!oInfo.HasAudio)
                        {
                            _log.LogEvent("no audio file detected: " + audioJob.Input, ImageType.Error);
                            break;
                        }
                        strChannelPositions = oInfo.AudioInfo.Tracks[0].ChannelPositions;
                        script.Append(@"# detected channels: " + oInfo.AudioInfo.Tracks[0].NbChannels + Environment.NewLine);
                        int.TryParse(oInfo.AudioInfo.Tracks[0].NbChannels.Split(' ')[0], out iChannelCount);
                    }

                    if (!String.IsNullOrEmpty(strChannelPositions))
                        script.Append(@"# detected channel positions: " + strChannelPositions + Environment.NewLine);
                    else
                        _log.LogEvent("no channel positions found. Downmix result may be wrong.", ImageType.Information);

                    if (iChannelCount <= 2)
                    {
                        _log.LogEvent("ignoring downmix as there are only " + iChannelCount + " channels", ImageType.Information);
                        break;
                    }

                    int iAVSChannelCount = 0;
                    using (AvsFile avi = AvsFile.ParseScript(script.ToString()))
                        iAVSChannelCount = avi.Clip.ChannelsCount;

                    if (iAVSChannelCount != iChannelCount)
                    {
                        _log.LogEvent("channel count mismatch! ignoring downmix as the input file is reporting " + iChannelCount + " channels and the AviSynth script is reporting " + iAVSChannelCount + " channels", ImageType.Warning);
                        break;
                    }
                    
                    switch (strChannelPositions)
                    {
                        // http://forum.doom9.org/showthread.php?p=1461787#post1461787
                        case "3/0/0":
                        case "2/0/0.1": script.Append(@"c3_stereo(ConvertAudioToFloat(last))" + Environment.NewLine); break;
                        case "2/1/0":
                        case "2/0/1":   if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                                            script.Append(@"c3_stereo(ConvertAudioToFloat(last))" + Environment.NewLine); 
                                        else
                                            script.Append(@"c3_dpl(ConvertAudioToFloat(last))" + Environment.NewLine); 
                                        break;
                        case "2/2/0":
                        case "2/0/2":   if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix) 
                                            script.Append(@"c4_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else if (audioJob.Settings.DownmixMode == ChannelMode.DPLDownmix)
                                            script.Append(@"c4_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else
                                            script.Append(@"c4_dpl2(ConvertAudioToFloat(last))" + Environment.NewLine); 
                                        break;
                        case "2/1/0.1":
                        case "2/0/1.1": if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                                            script.Append(@"c42_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else
                                                script.Append(@"c42_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        break;
                        case "3/0/0.1": if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                                            script.Append(@"c42_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else
                                            script.Append(@"c3_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        break;

                        case "3/1/0":
                        case "3/0/1":   if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                                            script.Append(@"c42_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else
                                            script.Append(@"c43_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        break;
                        case "3/2/0":
                        case "3/0/2":   if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                                            script.Append(@"c5_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else if (audioJob.Settings.DownmixMode == ChannelMode.DPLDownmix)
                                            script.Append(@"c5_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else
                                            script.Append(@"c5_dpl2(ConvertAudioToFloat(last))" + Environment.NewLine); 
                                        break;
                        case "2/2/0.1":
                        case "2/0/2.1": if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                                            script.Append(@"c5_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else if (audioJob.Settings.DownmixMode == ChannelMode.DPLDownmix)
                                            script.Append(@"c52_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else
                                            script.Append(@"c52_dpl2(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        break;
                        case "3/1/0.1":
                        case "3/0/1.1": if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                                            script.Append(@"c52_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else
                                            script.Append(@"c53_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        break;
                        case "3/2/0.1":
                        case "3/0/2.1": if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                                            script.Append(@"c6_stereo(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else if (audioJob.Settings.DownmixMode == ChannelMode.DPLDownmix)
                                            script.Append(@"c6_dpl(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        else
                                            script.Append(@"c6_dpl2(ConvertAudioToFloat(last))" + Environment.NewLine);
                                        break;
                        default:        if (audioJob.Settings.DownmixMode == ChannelMode.StereoDownmix)
                                        {
                                            script.Append(@"6<=Audiochannels(last)?c6_stereo(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                            script.Append(@"5==Audiochannels(last)?c5_stereo(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                            script.Append(@"4==Audiochannels(last)?c4_stereo(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                            script.Append(@"3==Audiochannels(last)?c3_stereo(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        }
                                        else if (audioJob.Settings.DownmixMode == ChannelMode.DPLDownmix)
                                        {
                                            script.Append(@"6<=Audiochannels(last)?c6_dpl(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                            script.Append(@"5==Audiochannels(last)?c5_dpl(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                            script.Append(@"4==Audiochannels(last)?c4_dpl(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                            script.Append(@"3==Audiochannels(last)?c3_dpl(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        }
                                        else
                                        {
                                            script.Append(@"6<=Audiochannels(last)?c6_dpl2(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                            script.Append(@"5==Audiochannels(last)?c5_dpl2(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                            script.Append(@"4==Audiochannels(last)?c4_dpl2(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                            script.Append(@"3==Audiochannels(last)?c3_dpl2(ConvertAudioToFloat(last)):last" + Environment.NewLine);
                                        }
                                        break;
                    }
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

            // SampleRate
            switch (audioJob.Settings.SampleRateType)
            {
                case 0:
                    break;
                case 1:
                    script.Append("SSRC(8000)" + Environment.NewLine);
                    break;
                case 2:
                    script.Append("SSRC(11025)" + Environment.NewLine);
                    break;
                case 3:
                    script.Append("SSRC(22050)" + Environment.NewLine);
                    break;
                case 4:
                    script.Append("SSRC(32000)" + Environment.NewLine);
                    break;
                case 5:
                    script.Append("SSRC(44100)" + Environment.NewLine);
                    break;
                case 6:
                    script.Append("SSRC(48000)" + Environment.NewLine);
                    break;
                case 7: // Speed-up (23.976 to 25)
                    script.Append("AssumeSampleRate((AudioRate()*1001+480)/960).SSRC(AudioRate()).TimeStretch(pitch=Float((AudioRate()*1001+480)/960)*100.0/Float(AudioRate()))" + Environment.NewLine);
                    break;
                case 8: // Slow-down (25 to 23.976)
                    script.Append("SSRC((AudioRate()*1001+480)/960).AssumeSampleRate(AudioRate()).TimeStretch(pitch=Float((AudioRate()*1001+480)/960)*100.0/Float(AudioRate()))" + Environment.NewLine);
                    break;
                case 9: // Speed-up (24 to 25)
                    script.Append("AssumeSampleRate((AudioRate()*25)/24).SSRC(AudioRate()).TimeStretch(pitch=Float((AudioRate()*25)/24)*100.0/Float(AudioRate()))" + Environment.NewLine);
                    break;
                case 10: // Slow-down (25 to 24)
                    script.Append("SSRC((AudioRate()*25)/24).AssumeSampleRate(AudioRate()).TimeStretch(pitch=Float((AudioRate()*25)/24)*100.0/Float(AudioRate()))" + Environment.NewLine);
                    break;
            }

            // put Normalize() after downmix cases >> http://forum.doom9.org/showthread.php?p=1166117#post1166117
            if (audioJob.Settings.AutoGain)
            {
                if (audioJob.Settings.Normalize != 100)
                    script.AppendFormat("Normalize(" + (audioJob.Settings.Normalize / 100.0).ToString(new CultureInfo("en-us")) + "){0}", Environment.NewLine);
                else 
                    script.AppendFormat("Normalize(){0}", Environment.NewLine);
            }

            //let's obtain command line & other stuff
            if (audioJob.Settings is AftenSettings)
            {
                UpdateCacher.CheckPackage("aften");
                _mustSendWavHeaderToEncoderStdIn = true;
                AftenSettings n = audioJob.Settings as AftenSettings;
                _encoderExecutablePath = this._settings.Aften.Path;
                _encoderCommandLine = "-readtoeof 1 -b " + n.Bitrate + " - \"{0}\"";
            }
            else if (audioJob.Settings is FlacSettings)
            {
                UpdateCacher.CheckPackage("flac");
                script.Append("AudioBits(last)>24?ConvertAudioTo24bit(last):last " + Environment.NewLine); // flac encoder doesn't support 32bits streams
                _mustSendWavHeaderToEncoderStdIn = false;
                FlacSettings n = audioJob.Settings as FlacSettings;
                _encoderExecutablePath = this._settings.Flac.Path;
                _encoderCommandLine = "--force --force-raw-format --endian=little --sign=signed -" + n.CompressionLevel + " - -o \"{0}\""; 
            }
            else if (audioJob.Settings is AC3Settings)
            {
                UpdateCacher.CheckPackage("ffmpeg");
                script.Append("6<=Audiochannels(last)?GetChannel(last,1,3,2,5,6,4):last" + Environment.NewLine);
                script.Append("32==Audiobits(last)?ConvertAudioTo16bit(last):last" + Environment.NewLine); // ffac3 encoder doesn't support 32bits streams
                _mustSendWavHeaderToEncoderStdIn = true;
                AC3Settings n = audioJob.Settings as AC3Settings;
                _encoderExecutablePath = this._settings.FFmpeg.Path;
                _encoderCommandLine = "-i - -y -acodec ac3 -ab " + n.Bitrate + "k \"{0}\"";
            }
            else if (audioJob.Settings is MP2Settings)
            {
                UpdateCacher.CheckPackage("ffmpeg");
                script.Append("32==Audiobits(last)?ConvertAudioTo16bit(last):last" + Environment.NewLine); // ffmp2 encoder doesn't support 32 bits streams
                _mustSendWavHeaderToEncoderStdIn = true;
                MP2Settings n = audioJob.Settings as MP2Settings;
                _encoderExecutablePath = this._settings.FFmpeg.Path;
                _encoderCommandLine = "-i - -y -acodec mp2 -ab " + n.Bitrate + "k \"{0}\"";
            } 
            else if (audioJob.Settings is OggVorbisSettings)
            {
                UpdateCacher.CheckPackage("oggenc2");
                _mustSendWavHeaderToEncoderStdIn = true;
                OggVorbisSettings n = audioJob.Settings as OggVorbisSettings;
                _encoderExecutablePath = this._settings.OggEnc.Path;
                _encoderCommandLine = "--ignorelength --quality " + n.Quality.ToString(System.Globalization.CultureInfo.InvariantCulture) + " -o \"{0}\" -";
            }
            else if (audioJob.Settings is NeroAACSettings)
            {
                UpdateCacher.CheckPackage("neroaacenc");
                _mustSendWavHeaderToEncoderStdIn = true;
                NeroAACSettings n = audioJob.Settings as NeroAACSettings;
                NeroAACSettings nas = n;
                _encoderExecutablePath = this._settings.NeroAacEnc.Path;
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
            else if (audioJob.Settings is MP3Settings)
            {
                UpdateCacher.CheckPackage("lame");
                MP3Settings m = audioJob.Settings as MP3Settings;
                _mustSendWavHeaderToEncoderStdIn = true;
                _encoderExecutablePath = this._settings.Lame.Path;
                script.Append("32==Audiobits(last)?ConvertAudioTo16bit(last):last" + Environment.NewLine); // lame encoder doesn't support 32bits streams

                switch (m.BitrateMode)
                {
                    case BitrateManagementMode.VBR:
                        _encoderCommandLine = "-V" + m.Quality + " - \"{0}\"";
                        break;
                    case BitrateManagementMode.CBR:
                        _encoderCommandLine = "-b " + m.Bitrate + " --cbr -h - \"{0}\"";
                        break;
                    case BitrateManagementMode.ABR:
                        _encoderCommandLine = "--abr " + m.AbrBitrate + " -h - \"{0}\"";
                        break;
                }
            }
            else if (audioJob.Settings is QaacSettings)
            {
                UpdateCacher.CheckPackage("qaac");
                QaacSettings q = audioJob.Settings as QaacSettings;
                _encoderExecutablePath = this._settings.QAAC.Path;
                _mustSendWavHeaderToEncoderStdIn = true;
                StringBuilder sb = new StringBuilder("--ignorelength --threading ");

                if (q.Profile == QaacProfile.ALAC)
                    sb.Append("-A ");
                else
                {
                    if (q.Profile == QaacProfile.HE) sb.Append("--he ");

                    switch (q.Mode)
                    {
                        case QaacMode.TVBR: sb.Append("-V " + q.Quality); break;
                        case QaacMode.CVBR: sb.Append("-v " + q.Bitrate); break;
                        case QaacMode.ABR: sb.Append("-a " + q.Bitrate); break;
                        case QaacMode.CBR: sb.Append("-c " + q.Bitrate); break;
                    }
                }

                if (q.NoDelay) // To resolve some A/V sync issues 
                    sb.Append(" --no-delay");

                sb.Append(" - -o \"{0}\"");

                _encoderCommandLine = sb.ToString();
            }
            else if (audioJob.Settings is OpusSettings)
            {
                UpdateCacher.CheckPackage("opus");
                OpusSettings o = audioJob.Settings as OpusSettings;
                _encoderExecutablePath = this._settings.Opus.Path;
                _mustSendWavHeaderToEncoderStdIn = true;
                StringBuilder sb = new StringBuilder("--ignorelength ");

                switch (o.Mode)
                {
                    case OpusMode.CVBR: sb.Append("--cvbr --bitrate " + o.Bitrate); break;
                    case OpusMode.HCBR: sb.Append("--hard-cbr --bitrate " + o.Bitrate); break;
                    case OpusMode.VBR:  sb.Append("--vbr --bitrate " + o.Bitrate); break;
                }
                
                sb.Append(" - \"{0}\"");

                _encoderCommandLine = sb.ToString();
            }

            //Just check encoder existance
            _encoderExecutablePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, _encoderExecutablePath);
            if (!File.Exists(_encoderExecutablePath))
            {
                deleteTempFiles();             
                throw new EncoderMissingException(_encoderExecutablePath);
            }

            script.AppendLine(Environment.NewLine);
            script.AppendLine(@"return last");
            script.AppendLine(Environment.NewLine);

            // copy the appropriate function at the end of the script
            switch (audioJob.Settings.DownmixMode)
            {
                case ChannelMode.KeepOriginal:
                    break;
                case ChannelMode.ConvertToMono:
                    break;
                case ChannelMode.DPLDownmix:
                case ChannelMode.DPLIIDownmix:
                case ChannelMode.StereoDownmix:
                    script.AppendLine(@"
# 5.1 Channels L,R,C,LFE,SL,SR -> stereo + LFE
function c6_stereo(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     fc = GetChannel(a, 3)
     lf = GetChannel(a, 4)
     sl = GetChannel(a, 5)
     sr = GetChannel(a, 6)
     fl_sl = MixAudio(fl, sl, 0.2929, 0.2929)
     fr_sr = MixAudio(fr, sr, 0.2929, 0.2929)
     fc_lf = MixAudio(fc, lf, 0.2071, 0.2071)
     l = MixAudio(fl_sl, fc_lf, 1.0, 1.0)
     r = MixAudio(fr_sr, fc_lf, 1.0, 1.0)
     return MergeChannels(l, r)
  }
# 5 Channels L,R,C,SL,SR or L,R,LFE,SL,SR-> Stereo
function c5_stereo(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     fc = GetChannel(a, 3)
     sl = GetChannel(a, 4)
     sr = GetChannel(a, 5)
     fl_sl = MixAudio(fl, sl, 0.3694, 0.3694)
     fr_sr = MixAudio(fr, sr, 0.3694, 0.3694)
     l = MixAudio(fl_sl, fc, 1.0, 0.2612)
     r = MixAudio(fr_sr, fc, 1.0, 0.2612)
     return MergeChannels(l, r)
  }
# 5 Channels L,R,C,LFE,S -> Stereo
function c52_stereo(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     fc = GetChannel(a, 3)
     lf = GetChannel(a, 4)
     bc = GetChannel(a, 5)
     fl_bc = MixAudio(fl, bc, 0.3205, 0.2265)
     fr_bc = MixAudio(fr, bc, 0.3205, 0.2265)
     fc_lf = MixAudio(fc, lf, 0.2265, 0.2265)
     l = MixAudio(fl_bc, fc_lf, 1.0, 1.0)
     r = MixAudio(fr_bc, fc_lf, 1.0, 1.0)
     return MergeChannels(l, r)
  }
# 4 Channels Quadro L,R,SL,SR -> Stereo
function c4_stereo(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     sl = GetChannel(a, 3)
     sr = GetChannel(a, 4)
     l = MixAudio(fl, sl, 0.5, 0.5)
     r = MixAudio(fr, sr, 0.5, 0.5)
     return MergeChannels(l, r)
  }
# 4 Channels L,R,C,LFE or L,R,S,LFE or L,R,C,S -> Stereo
function c42_stereo(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     fc = GetChannel(a, 3)
     lf = GetChannel(a, 4)
     fc_lf = MixAudio(fc, lf, 0.2929, 0.2929)
     l = MixAudio(fl, fc_lf, 0.4142, 1.0)
     r = MixAudio(fr, fc_lf, 0.4142, 1.0)
     return MergeChannels(l, r)
  }
# 3 Channels L,R,C or L,R,S or L,R,LFE -> Stereo
function c3_stereo(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     fc = GetChannel(a, 3)
     l = MixAudio(fl, fc, 0.5858, 0.4142)
     r = MixAudio(fr, fc, 0.5858, 0.4142)
     return MergeChannels(l, r)
  }
# 5.1 Channels L,R,C,LFE,SL,SR -> Dolby ProLogic
function c6_dpl(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     fc = GetChannel(a, 3)
     sl = GetChannel(a, 5)
     sr = GetChannel(a, 6)
     bc = MixAudio(sl, sr, 0.2265, 0.2265)
     fl_fc = MixAudio(fl, fc, 0.3205, 0.2265)
     fr_fc = MixAudio(fr, fc, 0.3205, 0.2265)
     l = MixAudio(fl_fc, bc, 1.0, 1.0)
     r = MixAudio(fr_fc, bc, 1.0, -1.0)
     return MergeChannels(l, r)
  }
# 5 Channels L,R,C,SL,SR -> Dolby ProLogic
function c5_dpl(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     fc = GetChannel(a, 3)
     sl = GetChannel(a, 4)
     sr = GetChannel(a, 5)
     bc = MixAudio(sl, sr, 0.2265, 0.2265)
     fl_fc = MixAudio(fl, fc, 0.3205, 0.2265)
     fr_fc = MixAudio(fr, fc, 0.3205, 0.2265)
     l = MixAudio(fl_fc, bc, 1.0, 1.0)
     r = MixAudio(fr_fc, bc, 1.0, -1.0)
     return MergeChannels(l, r)
  }
# 5 Channels L,R,LFE,SL,SR -> Dolby ProLogic
function c52_dpl(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     sl = GetChannel(a, 4)
     sr = GetChannel(a, 5)
     bc = MixAudio(sl, sr, 0.2929, 0.2929)
     l = MixAudio(fl, bc, 0.4142, 1.0)
     r = MixAudio(fr, bc, 0.4142, -1.0)
     return MergeChannels(l, r)
  }
# 5 Channels L,R,C,LFE,S -> Dolby ProLogic
function c53_dpl(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     fc = GetChannel(a, 3)
     bc = GetChannel(a, 5)
     fl_fc = MixAudio(fl, fc, 0.4142, 0.2929)
     fr_fc = MixAudio(fr, fc, 0.4142, 0.2929)
     l = MixAudio(fl_fc, bc, 1.0, 0.2929)
     r = MixAudio(fr_fc, bc, 1.0, -0.2929)
     return MergeChannels(l, r)
  }
# 4 Channels Quadro L,R,SL,SR -> Dolby ProLogic
function c4_dpl(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     sl = GetChannel(a, 3)
     sr = GetChannel(a, 4)
     bc = MixAudio(sl, sr, 0.2929, 0.2929)
     l = MixAudio(fl, bc, 0.4142, 1.0)
     r = MixAudio(fr, bc, 0.4142, -1.0)
     return MergeChannels(l, r)
  }
# 4 Channels L,R,LFE,S  -> Dolby ProLogic
function c42_dpl(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     bc = GetChannel(a, 4)
     l = MixAudio(fl, bc, 0.5858, 0.4142)
     r = MixAudio(fr, bc, 0.5858, -0.4142)
     return MergeChannels(l, r)
  }
# 4 Channels L,R,C,S -> Dolby ProLogic
function c43_dpl(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     fc = GetChannel(a, 3)
     bc = GetChannel(a, 4)
     fl_fc = MixAudio(fl, fc, 0.4142, 0.2929)
     fr_fc = MixAudio(fr, fc, 0.4142, 0.2929)
     l = MixAudio(fl_fc, bc, 1.0, 0.2929)
     r = MixAudio(fr_fc, bc, 1.0, -0.2929)
     return MergeChannels(l, r)
  }
# 3 Channels L,R,S  -> Dolby ProLogic
function c3_dpl(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     bc = GetChannel(a, 3)
     l = MixAudio(fl, bc, 0.5858, 0.4142)
     r = MixAudio(fr, bc, 0.5858, -0.4142)
     return MergeChannels(l, r)
  }
# 5.1 Channels L,R,C,LFE,SL,SR -> Dolby ProLogic II
function c6_dpl2(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     fc = GetChannel(a, 3)
     sl = GetChannel(a, 5)
     sr = GetChannel(a, 6)
     ssl = MixAudio(sl, sr, 0.2818, 0.1627)
     ssr = MixAudio(sl, sr, -0.1627, -0.2818)
     fl_fc = MixAudio(fl, fc, 0.3254, 0.2301)
     fr_fc = MixAudio(fr, fc, 0.3254, 0.2301)
     l = MixAudio(fl_fc, ssl, 1.0, 1.0)
     r = MixAudio(fr_fc, ssr, 1.0, 1.0)
     return MergeChannels(l, r)
  }
# 5 Channels L,R,C,SL,SR -> Dolby ProLogic II
function c5_dpl2(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     fc = GetChannel(a, 3)
     sl = GetChannel(a, 4)
     sr = GetChannel(a, 5)
     ssl = MixAudio(sl, sr, 0.2818, 0.1627)
     ssr = MixAudio(sl, sr, -0.1627, -0.2818)
     fl_fc = MixAudio(fl, fc, 0.3254, 0.2301)
     fr_fc = MixAudio(fr, fc, 0.3254, 0.2301)
     l = MixAudio(fl_fc, ssl, 1.0, 1.0)
     r = MixAudio(fr_fc, ssr, 1.0, 1.0)
     return MergeChannels(l, r)
  }
# 5 Channels L,R,LFE,SL,SR -> Dolby ProLogic II
function c52_dpl2(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     sl = GetChannel(a, 4)
     sr = GetChannel(a, 5)
     ssl = MixAudio(sl, sr, 0.3714, 0.2144)
     ssr = MixAudio(sl, sr, -0.2144, -0.3714)
     l = MixAudio(fl, ssl, 0.4142, 1.0)
     r = MixAudio(fr, ssr, 0.4142, 1.0)
     return MergeChannels(l, r)
  }
# 4 Channels Quadro L,R,SL,SR -> Dolby ProLogic II
function c4_dpl2(clip a)
  {
     fl = GetChannel(a, 1)
     fr = GetChannel(a, 2)
     sl = GetChannel(a, 3)
     sr = GetChannel(a, 4)
     ssl = MixAudio(sl, sr, 0.3714, 0.2144)
     ssr = MixAudio(sl, sr, -0.2144, -0.3714)
     l = MixAudio(fl, ssl, 0.4142, 1.0)
     r = MixAudio(fr, ssr, 0.4142, 1.0)
     return MergeChannels(l, r)
  }");
                    break;
                case ChannelMode.Upmix:
                    script.AppendLine(@"
function x_upmix" + id + @"(clip a) 
 {
    m = ConvertToMono(a)
    a1 = GetLeftChannel(a)
    a2 = GetRightChannel(a)
    fl = SuperEQ(a1,""" + tmp + @"front.feq"")
    fr = SuperEQ(a2,""" + tmp + @"front.feq"")
    c = SuperEQ(m,""" + tmp + @"center.feq"") 
    lfe = SuperEQ(m,""" + tmp + @"lfe.feq"") 
    sl = SuperEQ(a1,""" + tmp + @"back.feq"")
    sr = SuperEQ(a2,""" + tmp + @"back.feq"")
    return MergeChannels(fl,fr,c,lfe,sl,sr)
 }");
                    break;
                case ChannelMode.UpmixUsingSoxEq:
                    script.AppendLine(@"
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
 }");
                    break;
                case ChannelMode.UpmixWithCenterChannelDialog:
                    script.AppendLine(@"
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
 }");
                    break;
            }

            _avisynthAudioScript = script.ToString();

            _log.LogValue("Avisynth script", _avisynthAudioScript);
            _log.LogValue("Commandline used", _encoderCommandLine);
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
                    switch (priority)
					{
					    case ProcessPriority.IDLE:
							_encoderThread.Priority = ThreadPriority.Lowest;
                            _encoderProcess.PriorityClass = ProcessPriorityClass.Idle;
							break;
						case ProcessPriority.BELOW_NORMAL:
							_encoderThread.Priority = ThreadPriority.BelowNormal;
                            _encoderProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
							break;
						case ProcessPriority.NORMAL:
							_encoderThread.Priority = ThreadPriority.Normal;
                            _encoderProcess.PriorityClass = ProcessPriorityClass.Normal;
							break;
						case ProcessPriority.ABOVE_NORMAL:
							_encoderThread.Priority = ThreadPriority.AboveNormal;
                            _encoderProcess.PriorityClass = ProcessPriorityClass.AboveNormal;
							break;
						case ProcessPriority.HIGH:
							_encoderThread.Priority = ThreadPriority.Highest;
                            _encoderProcess.PriorityClass = ProcessPriorityClass.High;
							break;
				    }
                    VistaStuff.SetProcessPriority(_encoderProcess.Handle, _encoderProcess.PriorityClass);
                    MainForm.Instance.Settings.ProcessingPriority = priority;
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