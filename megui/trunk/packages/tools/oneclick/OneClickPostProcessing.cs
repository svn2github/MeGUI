// ****************************************************************************
// 
// Copyright (C) 2005-2012 Doom9 & al
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
using System.Collections.Generic;
using System.IO;
using System.Threading;

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI
{
    public sealed class OneClickPostProcessing : IJobProcessor
    {
        public static readonly JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(init), "OneClickPostProcessing");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is OneClickPostProcessingJob)
                return new OneClickPostProcessing(mf);
            return null;
        }

        private Thread _processThread = null;
        private DateTime _start;
        private StatusUpdate su;
        private OneClickPostProcessingJob job;
        private LogItem _log;

        #region OneClick properties
        private MainForm mainForm;
        Dictionary<int, string> audioFiles;
        private VideoUtil vUtil;
        private AVCLevels al = new AVCLevels();
        private bool finished = false;
        private bool interlaced = false;
        private DeinterlaceFilter[] filters;
        string qpfile = string.Empty;
        #endregion

        internal OneClickPostProcessing(MainForm mf)
        {
            mainForm = mf;
            this.vUtil = new VideoUtil(mainForm);
        }

        #region JobHandling

        internal void Start()
        {
            Util.ensureExists(job.Input);
            _processThread = new Thread(new ThreadStart(this.StartPostProcessing));
            _processThread.Priority = ThreadPriority.BelowNormal;
            _processThread.Start();
        }

        internal void Abort()
        {
            _processThread.Abort();
            _processThread = null;
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

        private void raiseEvent()
        {
            if (StatusUpdate != null)
                StatusUpdate(su);
        }

        private void setProgress(decimal n)
        {
            su.PercentageDoneExact = n * 100M;
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

        #endregion
        #region OneClickPostProcessor

        private void StartPostProcessing()
        {
            Thread t = null;
            try
            {
                _log.LogEvent("Processing thread started");
                raiseEvent("Preprocessing...   ***PLEASE WAIT***");
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

                List<string> arrAudioFilesDelete = new List<string>();
                audioFiles = new Dictionary<int, string>();
                List<AudioTrackInfo> arrAudioTracks = new List<AudioTrackInfo>();
                List<AudioJob> arrAudioJobs = new List<AudioJob>();
                List<MuxStream> arrMuxStreams = new List<MuxStream>();
 
                foreach (OneClickAudioTrack oAudioTrack in job.PostprocessingProperties.AudioTracks)
                {
                    if (oAudioTrack.AudioTrackInfo != null)
                    {
                        audioFiles.Add(oAudioTrack.AudioTrackInfo.TrackID, Path.GetDirectoryName(job.PostprocessingProperties.FinalOutput) + "\\" + oAudioTrack.AudioTrackInfo.DemuxFileName);
                        arrAudioFilesDelete.Add(Path.GetDirectoryName(job.PostprocessingProperties.FinalOutput) + "\\" + oAudioTrack.AudioTrackInfo.DemuxFileName);
                    }
                    else if (oAudioTrack.AudioTrackInfo != null)
                        arrAudioTracks.Add(oAudioTrack.AudioTrackInfo);
                    if (oAudioTrack.AudioJob != null)
                        arrAudioJobs.Add(oAudioTrack.AudioJob);
                    if (oAudioTrack.DirectMuxAudio != null)
                        arrMuxStreams.Add(oAudioTrack.DirectMuxAudio);
                }
                if (audioFiles.Count == 0)
                    audioFiles = vUtil.getAllDemuxedAudio(arrAudioTracks, new List<AudioTrackInfo>(), out arrAudioFilesDelete, job.IndexFile, _log);

                fillInAudioInformation(arrAudioJobs, arrMuxStreams);

                //job.PostprocessingProperties.AudioJobs = AudioUtil.getConfiguredAudioJobs(job.PostprocessingProperties.AudioJobs);

                if (job.PostprocessingProperties.VideoFileToMux != null)
                    _log.LogEvent("Don't encode video: True");
                else
                    _log.LogEvent("Desired size: " + job.PostprocessingProperties.OutputSize);
                _log.LogEvent("Split size: " + job.PostprocessingProperties.Splitting);
                

                string videoInput = String.Empty;
                VideoStream myVideo = new VideoStream();

                VideoCodecSettings videoSettings = job.PostprocessingProperties.VideoSettings;
                if (job.PostprocessingProperties.VideoFileToMux == null)
                {
                    //Open the video
                    Dar? dar;
                    videoInput = openVideo(job.IndexFile, job.PostprocessingProperties.DAR,
                        job.PostprocessingProperties.HorizontalOutputResolution, job.PostprocessingProperties.SignalAR, _log,
                        job.PostprocessingProperties.AvsSettings, job.PostprocessingProperties.AutoDeinterlace, videoSettings, out dar,
                        job.PostprocessingProperties.AutoCrop, job.PostprocessingProperties.KeepInputResolution,
                        job.PostprocessingProperties.UseChaptersMarks);

                    ulong length;
                    double framerate;
                    JobUtil.getInputProperties(out length, out framerate, videoInput);
                    myVideo.Input = videoInput;
                    myVideo.Output = Path.Combine(Path.GetDirectoryName(job.IndexFile),
                        Path.GetFileNameWithoutExtension(job.IndexFile) + "_Video");
                    myVideo.NumberOfFrames = length;
                    myVideo.Framerate = (decimal)framerate;
                    myVideo.DAR = dar;
                    myVideo.VideoType = new MuxableType((new VideoEncoderProvider().GetSupportedOutput(videoSettings.EncoderType))[0], videoSettings.Codec);
                    myVideo.Settings = videoSettings;
                }
                else
                {
                    myVideo.Output = job.PostprocessingProperties.VideoFileToMux;
                    MediaInfoFile oInfo = new MediaInfoFile(myVideo.Output, ref _log);
                    videoSettings.VideoName = oInfo.VideoInfo.Track.Name;
                    myVideo.Settings = videoSettings;
                    myVideo.Framerate = (decimal)oInfo.VideoInfo.FPS;
                    
                }

                List<string> intermediateFiles = new List<string>();
                intermediateFiles.Add(videoInput);
                intermediateFiles.Add(job.IndexFile);
                intermediateFiles.AddRange(audioFiles.Values);
                if (!string.IsNullOrEmpty(qpfile))
                    intermediateFiles.Add(qpfile);
                foreach (string file in arrAudioFilesDelete)
                    intermediateFiles.Add(file);
                if (File.Exists(Path.Combine(Path.GetDirectoryName(job.Input), Path.GetFileNameWithoutExtension(job.Input) + "._log")))
                    intermediateFiles.Add(Path.Combine(Path.GetDirectoryName(job.Input), Path.GetFileNameWithoutExtension(job.Input) + "._log"));
                foreach (string file in job.PostprocessingProperties.FilesToDelete)
                    intermediateFiles.Add(file);

                if (!string.IsNullOrEmpty(videoInput) || job.PostprocessingProperties.VideoFileToMux != null)
                {
                    MuxStream[] subtitles;
                    if (job.PostprocessingProperties.SubtitleTracks.Count == 0)
                    {
                        //Create empty subtitles for muxing
                        subtitles = new MuxStream[0];
                    }
                    else
                    {
                        subtitles = new MuxStream[job.PostprocessingProperties.SubtitleTracks.Count];
                        int i = 0;
                        foreach (SubtitleTrackInfo oTrack in job.PostprocessingProperties.SubtitleTracks)
                        {
                            subtitles[i] = new MuxStream(oTrack.SourceFileName, oTrack.Language, oTrack.Name, 0, oTrack.DefaultTrack, oTrack.ForcedTrack, oTrack);
                            i++;
                        }
                    }

                    if (job.PostprocessingProperties.Container == ContainerType.AVI)
                        job.PostprocessingProperties.ChapterFile = null;

                    JobChain c = vUtil.GenerateJobSeries(myVideo, job.PostprocessingProperties.FinalOutput, arrAudioJobs.ToArray(), 
                        subtitles, job.PostprocessingProperties.ChapterFile, job.PostprocessingProperties.OutputSize,
                        job.PostprocessingProperties.Splitting, job.PostprocessingProperties.Container,
                        job.PostprocessingProperties.PrerenderJob, arrMuxStreams.ToArray(),
                        _log, job.PostprocessingProperties.DeviceOutputType, null, job.PostprocessingProperties.VideoFileToMux, job.PostprocessingProperties.AudioTracks.ToArray());
                    if (c == null)
                    {
                        _log.Warn("Job creation aborted");
                        return;
                    }

                    c = CleanupJob.AddAfter(c, intermediateFiles);
                    mainForm.Jobs.addJobsWithDependencies(c);
                }
            }
            catch (Exception e)
            {
                t.Abort();
                if (e is ThreadAbortException)
                {
                    _log.LogEvent("Aborting...");
                    su.WasAborted = true;
                    su.IsComplete = true;
                    raiseEvent();
                }
                else
                {
                    _log.LogValue("An error occurred", e, ImageType.Error);
                    su.HasError = true;
                    su.IsComplete = true;
                    raiseEvent();
                }
                return;
            }
            t.Abort();
            su.IsComplete = true;
            raiseEvent();
        }

        private void fillInAudioInformation(List<AudioJob> arrAudioJobs, List<MuxStream> arrMuxStreams)
        {
            foreach (MuxStream m in arrMuxStreams)
                m.path = convertTrackNumberToFile(m.path, ref m.delay);

            foreach (AudioJob a in arrAudioJobs)
            {
                a.Input = convertTrackNumberToFile(a.Input, ref a.Delay);
                if (String.IsNullOrEmpty(a.Output) && !String.IsNullOrEmpty(a.Input))
                    a.Output = FileUtil.AddToFileName(a.Input, "_audio");
            }
        }

        /// <summary>
        /// if input is a track number (of the form, "::&lt;number&gt;::")
        /// then it returns the file path of that track number. Otherwise,
        /// it returns the string only
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string convertTrackNumberToFile(string input, ref int delay)
        {
            if (input.StartsWith("::") && input.EndsWith("::") && input.Length > 4)
            {
                string sub = input.Substring(2, input.Length - 4);
                try
                {
                    int t = int.Parse(sub);
                    string s = audioFiles[t];
                    delay = PrettyFormatting.getDelay(s) ?? 0;
                    return s;
                }
                catch (Exception)
                {
                    _log.Warn(string.Format("Couldn't find audio file for track {0}. Skipping track.", input));
                    return null;
                }
            }

            return input;
        }
            
        /// <summary>
        /// opens a dgindex script
        /// if the file can be properly opened, auto-cropping is performed, then depending on the AR settings
        /// the proper resolution for automatic resizing, taking into account the derived cropping values
        /// is calculated, and finally the avisynth script is written and its name returned
        /// </summary>
        /// <param name="path">dgindex script</param>
        /// <param name="aspectRatio">aspect ratio selection to be used</param>
        /// <param name="customDAR">custom display aspect ratio for this source</param>
        /// <param name="horizontalResolution">desired horizontal resolution of the output</param>
        /// <param name="settings">the codec settings (used only for x264)</param>
        /// <param name="sarX">pixel aspect ratio X</param>
        /// <param name="sarY">pixel aspect ratio Y</param>
        /// <param name="height">the final height of the video</param>
        /// <param name="signalAR">whether or not ar signalling is to be used for the output 
        /// (depending on this parameter, resizing changes to match the source AR)</param>
        /// <param name="autoCrop">whether or not autoCrop is used for the input</param>
        /// <returns>the name of the AviSynth script created, empty of there was an error</returns>
        private string openVideo(string path, Dar? AR, int horizontalResolution,
            bool signalAR, LogItem _log, AviSynthSettings avsSettings, bool autoDeint,
            VideoCodecSettings settings, out Dar? dar, bool autoCrop, bool keepInputResolution, bool useChaptersMarks)
        {
            dar = null;
            CropValues final = new CropValues();
            Dar customDAR;
            IMediaFile d2v = null;
            IVideoReader reader;
            PossibleSources oPossibleSource;

            if (job.IndexerUsed == FileIndexerWindow.IndexType.DGI)
            {
                d2v = new dgiFile(path);
                oPossibleSource = PossibleSources.dgi;
            }
            else if (job.IndexerUsed == FileIndexerWindow.IndexType.D2V)
            {
                d2v = new d2vFile(path);
                oPossibleSource = PossibleSources.d2v;
            }
            else if (job.IndexerUsed == FileIndexerWindow.IndexType.DGA)
            {
                d2v = new dgaFile(path);
                oPossibleSource = PossibleSources.dga;
            }
            else if (job.IndexerUsed == FileIndexerWindow.IndexType.FFMS)
            {
                d2v = new ffmsFile(null, path);
                oPossibleSource = PossibleSources.ffindex;
            }
            else
            {
                _log.Error("This project cannot be opened!");
                return "";
            }
            reader = d2v.GetVideoReader();
            if (reader.FrameCount < 1)
            {
                _log.Error("There are 0 frames in this file. This is a fatal error. Please recreate the DGIndex project");
                return "";
            }

            if (!keepInputResolution && autoCrop)
            {
                //Autocrop
                final = Autocrop.autocrop(reader);

                if (signalAR)
                {
                    if (avsSettings.Mod16Method == mod16Method.overcrop)
                        ScriptServer.overcrop(ref final);
                    else if (avsSettings.Mod16Method == mod16Method.mod4Horizontal)
                        ScriptServer.cropMod4Horizontal(ref final);
                    else if (avsSettings.Mod16Method == mod16Method.undercrop)
                        ScriptServer.undercrop(ref final);
                }

                bool error = (final.left == -1);
                if (!error)
                    _log.LogValue("Autocrop values", final);
                else
                {
                    _log.Error("Autocrop failed, aborting now");
                    return "";
                }
            }

            _log.LogValue("Auto-detect aspect ratio now", AR == null);
            //Check if AR needs to be autodetected now
            if (AR == null) // it does
            {
                customDAR = d2v.VideoInfo.DAR;
                if (customDAR.ar > 0)
                    _log.LogValue("Aspect ratio", customDAR);
                else
                {
                    if (MainForm.Instance.Settings.UseITUValues)
                        customDAR = Dar.ITU16x9PAL;
                    else
                        customDAR = Dar.STATIC16x9;
                    _log.Warn(string.Format("No aspect ratio found, defaulting to {0}.", customDAR));
                }
            }
            else
                customDAR = AR.Value;

            if (keepInputResolution)
            {
                horizontalResolution = (int)d2v.VideoInfo.Width;
                dar = customDAR;
            }
            else
            {
                // Minimise upsizing
                int sourceHorizontalResolution = (int)d2v.VideoInfo.Width - final.right - final.left;
                if (autoCrop)
                    sourceHorizontalResolution = (int)d2v.VideoInfo.Width;

                if (horizontalResolution > sourceHorizontalResolution)
                {
                    if (avsSettings.Mod16Method == mod16Method.resize)
                        while (horizontalResolution > sourceHorizontalResolution + 16)
                            horizontalResolution -= 16;
                    else
                        horizontalResolution = sourceHorizontalResolution;
                }
            }

            //Suggest a resolution (taken from AvisynthWindow.suggestResolution_CheckedChanged)
            int scriptVerticalResolution = 0;
            if (keepInputResolution)
                scriptVerticalResolution = (int)d2v.VideoInfo.Height;
            else
                scriptVerticalResolution = Resolution.suggestResolution(d2v.VideoInfo.Height, d2v.VideoInfo.Width, (double)customDAR.ar,
                    final, horizontalResolution, signalAR, mainForm.Settings.AcceptableAspectErrorPercent, out dar);
            _log.LogValue("Output resolution", horizontalResolution + "x" + scriptVerticalResolution);

            // create qpf file if necessary
            if (useChaptersMarks && settings != null && settings is x264Settings)
            {
                x264Settings xs = (x264Settings)settings;
                qpfile = job.PostprocessingProperties.ChapterFile;
                if ((Path.GetExtension(qpfile).ToLower()) == ".txt")
                    qpfile = VideoUtil.convertChaptersTextFileTox264QPFile(job.PostprocessingProperties.ChapterFile, d2v.VideoInfo.FPS);
                if (File.Exists(qpfile))
                {
                    xs.UseQPFile = true;
                    xs.QPFile = qpfile;
                }
            }

            //Generate the avs script based on the template
            string inputLine = "#input";
            string deinterlaceLines = "#deinterlace";
            string denoiseLines = "#denoise";
            string cropLine = "#crop";
            string resizeLine = "#resize";

            inputLine = ScriptServer.GetInputLine(path, null, false, oPossibleSource,
                false, false, false, 0, false);
            if (!inputLine.EndsWith(")"))
                inputLine += ")";

            raiseEvent("Automatic deinterlacing...   ***PLEASE WAIT***");
            _log.LogValue("Automatic deinterlacing", autoDeint);
            if (autoDeint)
            {
                string d2vPath = path;
                SourceDetector sd = new SourceDetector(inputLine, d2vPath, false,
                    mainForm.Settings.SourceDetectorSettings,
                    new UpdateSourceDetectionStatus(analyseUpdate),
                    new FinishedAnalysis(finishedAnalysis));
                finished = false;
                sd.analyse();
                waitTillAnalyseFinished();
                sd.stop();
                deinterlaceLines = filters[0].Script;
                _log.LogValue("Deinterlacing used", deinterlaceLines);
            }

            raiseEvent("Finalizing preprocessing...   ***PLEASE WAIT***");
            inputLine = ScriptServer.GetInputLine(path, null, interlaced, oPossibleSource, avsSettings.ColourCorrect, avsSettings.MPEG2Deblock, false, 0, false);
            if (!inputLine.EndsWith(")"))
                inputLine += ")";

            if (autoCrop)
                cropLine = ScriptServer.GetCropLine(true, final);
            else
                cropLine = ScriptServer.GetCropLine(false, final);

            denoiseLines = ScriptServer.GetDenoiseLines(avsSettings.Denoise, (DenoiseFilterType)avsSettings.DenoiseMethod);

            if (!keepInputResolution)
            {
                if (horizontalResolution <= 0 || scriptVerticalResolution <= 0)
                    _log.Error("Error in detection of output resolution: " + horizontalResolution + "x" + scriptVerticalResolution);
                else
                    resizeLine = ScriptServer.GetResizeLine(!signalAR || avsSettings.Mod16Method == mod16Method.resize, horizontalResolution, scriptVerticalResolution, 0, 0, (ResizeFilterType)avsSettings.ResizeMethod,
                                                            autoCrop, final, (int)d2v.VideoInfo.Width, (int)d2v.VideoInfo.Height);
            }

            string newScript = ScriptServer.CreateScriptFromTemplate(avsSettings.Template, inputLine, cropLine, resizeLine, denoiseLines, deinterlaceLines);

            if (dar.HasValue)
                newScript = string.Format("global MeGUI_darx = {0}\r\nglobal MeGUI_dary = {1}\r\n{2}", dar.Value.X, dar.Value.Y, newScript);

            _log.LogValue("Generated Avisynth script", newScript);
            try
            {
                StreamWriter sw = new StreamWriter(Path.ChangeExtension(path, ".avs"), false, System.Text.Encoding.Default);
                sw.Write(newScript);
                sw.Close();
            }
            catch (IOException i)
            {
                _log.LogValue("Error saving AviSynth script", i, ImageType.Error);
                return "";
            }
            return Path.ChangeExtension(path, ".avs");
        }

        public void finishedAnalysis(SourceInfo info, bool error, string errorMessage)
        {
            if (error || info == null)
            {
                LogItem oSourceLog = _log.LogEvent("Source detection");
                oSourceLog.LogEvent("Source detection failed: " + errorMessage, ImageType.Error);
                filters = new DeinterlaceFilter[] {
                new DeinterlaceFilter("Error", "#An error occurred in source detection. Doing no processing")};
                interlaced = false;
            }
            else
            {
                LogItem oSourceLog = _log.LogValue("Source detection", info.analysisResult);
                if (info.sourceType == SourceType.NOT_ENOUGH_SECTIONS)
                {
                    oSourceLog.LogEvent("Source detection failed: Could not find enough useful sections to determine source type for " + job.Input, ImageType.Error);
                    filters = new DeinterlaceFilter[] {
                new DeinterlaceFilter("Error", "#Not enough useful sections for source detection. Doing no processing")};
                }
                else
                    this.filters = ScriptServer.GetDeinterlacers(info).ToArray();
                interlaced = (info.sourceType != SourceType.PROGRESSIVE);
            }
            finished = true;
        }

        public void analyseUpdate(int amountDone, int total)
        {
            try
            {
                setProgress((decimal)amountDone / (decimal)total);
            }
            catch (Exception) { } // If we get any errors, just ignore -- it's only a cosmetic thing.
        }

        private void waitTillAnalyseFinished()
        {
            while (!finished)
            {
                Thread.Sleep(500);
            }
        }

        #endregion

        #region IJobProcessor Members

        public void setup(Job job, StatusUpdate su, LogItem _log)
        {
            this._log = _log;
            this.job = (OneClickPostProcessingJob)job;
            this.su = su;
        }

        public void resume()
        {

        }

        public void pause()
        {

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

        public void changePriority(ProcessPriority priority)
        {
            if (this._processThread != null && _processThread.IsAlive)
            {
                try
                {
                    switch (priority)
                    {
                        case ProcessPriority.IDLE:
                            _processThread.Priority = ThreadPriority.Lowest;
                            break;
                        case ProcessPriority.BELOW_NORMAL:
                            _processThread.Priority = ThreadPriority.BelowNormal;
                            break;
                        case ProcessPriority.NORMAL:
                            _processThread.Priority = ThreadPriority.Normal;
                            break;
                        case ProcessPriority.ABOVE_NORMAL:
                            _processThread.Priority = ThreadPriority.AboveNormal;
                            break;
                        case ProcessPriority.HIGH:
                            _processThread.Priority = ThreadPriority.Highest;
                            break;
                    }
                    return;
                }
                catch (Exception e) // process could not be running anymore
                {
                    throw new JobRunException(e);
                }
            }
            else
            {
                if (_processThread == null)
                    throw new JobRunException("Thread has not been started yet");
                else
                    throw new JobRunException("Thread has exited");
            }
        }

        public event JobProcessingStatusUpdateCallback StatusUpdate;
        #endregion
    }
}
