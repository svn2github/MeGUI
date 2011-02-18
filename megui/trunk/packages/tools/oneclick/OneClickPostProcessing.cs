// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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
            //Util.ensureExists(processingJob.Input);
            _processThread = new Thread(new ThreadStart(this.StartPostProcessing));
            _processThread.Priority = ThreadPriority.BelowNormal;
            _processThread.Start();
        }

        internal void Abort()
        {
            _processThread.Abort();
            _processThread = null;
        }

        private void deleteOutputFile()
        {
            //safeDelete(processingJob.Output);
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

                List<string> arrAudioFilesDelete;
                audioFiles = vUtil.getAllDemuxedAudio(job.AudioTracks, out arrAudioFilesDelete, job.IndexFile, _log);

                fillInAudioInformation();

                job.PostprocessingProperties.AudioJobs = AudioUtil.getConfiguredAudioJobs(job.PostprocessingProperties.AudioJobs);

                _log.LogEvent("Desired size: " + job.PostprocessingProperties.OutputSize);
                _log.LogEvent("Split size: " + job.PostprocessingProperties.Splitting);

                VideoCodecSettings videoSettings = job.PostprocessingProperties.VideoSettings;

                string videoOutput = Path.Combine(Path.GetDirectoryName(job.IndexFile),
                    Path.GetFileNameWithoutExtension(job.IndexFile) + "_Video");
                string muxedOutput = job.PostprocessingProperties.FinalOutput;

                //Open the video
                Dar? dar;
                string videoInput = openVideo(job.IndexFile, job.PostprocessingProperties.DAR,
                    job.PostprocessingProperties.HorizontalOutputResolution, job.PostprocessingProperties.SignalAR, _log,
                    job.PostprocessingProperties.AvsSettings, job.PostprocessingProperties.AutoDeinterlace, videoSettings, out dar,
                    job.PostprocessingProperties.AutoCrop, job.PostprocessingProperties.KeepInputResolution,
                    job.PostprocessingProperties.UseChaptersMarks);

                VideoStream myVideo = new VideoStream();
                ulong length;
                double framerate;
                JobUtil.getInputProperties(out length, out framerate, videoInput);
                myVideo.Input = videoInput;
                myVideo.Output = videoOutput;
                myVideo.NumberOfFrames = length;
                myVideo.Framerate = (decimal)framerate;
                myVideo.DAR = dar;
                myVideo.VideoType = new MuxableType((new VideoEncoderProvider().GetSupportedOutput(videoSettings.EncoderType))[0], videoSettings.Codec);
                myVideo.Settings = videoSettings;
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

                if (!string.IsNullOrEmpty(videoInput))
                {
                    //Create empty subtitles for muxing (subtitles not supported in one click mode)
                    MuxStream[] subtitles = new MuxStream[0];

                    if (job.PostprocessingProperties.Container == ContainerType.AVI)
                        job.PostprocessingProperties.ChapterFile = null;

                    JobChain c = vUtil.GenerateJobSeries(myVideo, muxedOutput, job.PostprocessingProperties.AudioJobs, subtitles,
                        job.PostprocessingProperties.ChapterFile, job.PostprocessingProperties.OutputSize,
                        job.PostprocessingProperties.Splitting, job.PostprocessingProperties.Container,
                        job.PostprocessingProperties.PrerenderJob, job.PostprocessingProperties.DirectMuxAudio, _log, job.PostprocessingProperties.DeviceOutputType, null);
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
                deleteOutputFile();
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

        private void fillInAudioInformation()
        {
            foreach (MuxStream m in job.PostprocessingProperties.DirectMuxAudio)
                m.path = convertTrackNumberToFile(m.path, ref m.delay);

            foreach (AudioJob a in job.PostprocessingProperties.AudioJobs)
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
                d2v = new ffmsFile(path);
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

            if (!keepInputResolution)
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

                if (autoCrop)
                {
                    bool error = (final.left == -1);
                    if (!error)
                        _log.LogValue("Autocrop values", final);
                    else
                    {
                        _log.Error("Autocrop failed, aborting now");
                        return "";
                    }
                }
            }

            _log.LogValue("Auto-detect aspect ratio now", AR == null);
            //Check if AR needs to be autodetected now
            if (AR == null) // it does
            {
                customDAR = d2v.Info.DAR;
                if (customDAR.ar > 0)
                    _log.LogValue("Aspect ratio", customDAR);
                else
                {
                    customDAR = Dar.ITU16x9PAL;
                    _log.Warn(string.Format("No aspect ratio found, defaulting to {0}.", customDAR));
                }
            }
            else
                customDAR = AR.Value;

            if (keepInputResolution)
            {
                horizontalResolution = (int)d2v.Info.Width;
                dar = customDAR;
            }
            else
            {
                // Minimise upsizing
                int sourceHorizontalResolution = (int)d2v.Info.Width - final.right - final.left;
                if (autoCrop)
                    sourceHorizontalResolution = (int)d2v.Info.Width;

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
            {
                scriptVerticalResolution = (int)d2v.Info.Height;
                _log.LogValue("Output resolution", horizontalResolution + "x" + scriptVerticalResolution);
            }
            else
            {
                scriptVerticalResolution = Resolution.suggestResolution(d2v.Info.Height, d2v.Info.Width, (double)customDAR.ar,
                final, horizontalResolution, signalAR, mainForm.Settings.AcceptableAspectErrorPercent, out dar);
                _log.LogValue("Output resolution", horizontalResolution + "x" + scriptVerticalResolution);
                if (settings != null && settings is x264Settings) // verify that the video corresponds to the chosen avc level, if not, change the resolution until it does fit
                {
                    x264Settings xs = (x264Settings)settings;
                    if (xs.Level != 15)
                    {
                        AVCLevels al = new AVCLevels();
                        _log.LogValue("AVC level", al.getLevels()[xs.Level]);

                        int compliantLevel = 15;
                        while (!this.al.validateAVCLevel(horizontalResolution, scriptVerticalResolution, d2v.Info.FPS, xs, out compliantLevel))
                        { // resolution not profile compliant, reduce horizontal resolution by 16, get the new vertical resolution and try again
                            string levelName = al.getLevels()[xs.Level];
                            horizontalResolution -= 16;
                            scriptVerticalResolution = Resolution.suggestResolution(d2v.Info.Height, d2v.Info.Width, (double)customDAR.ar,
                                final, horizontalResolution, signalAR, mainForm.Settings.AcceptableAspectErrorPercent, out dar);
                        }
                        _log.LogValue("Resolution adjusted for AVC Level", horizontalResolution + "x" + scriptVerticalResolution);
                    }
                    if (useChaptersMarks)
                    {
                        qpfile = job.PostprocessingProperties.ChapterFile;
                        if ((Path.GetExtension(qpfile).ToLower()) == ".txt")
                            qpfile = VideoUtil.convertChaptersTextFileTox264QPFile(job.PostprocessingProperties.ChapterFile, d2v.Info.FPS);
                        if (File.Exists(qpfile))
                        {
                            xs.UseQPFile = true;
                            xs.QPFile = qpfile;
                        }
                    }
                }
            }

            //Generate the avs script based on the template
            string inputLine = "#input";
            string deinterlaceLines = "#deinterlace";
            string denoiseLines = "#denoise";
            string cropLine = "#crop";
            string resizeLine = "#resize";

            inputLine = ScriptServer.GetInputLine(path, false, oPossibleSource,
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
                deinterlaceLines = filters[0].Script;
                _log.LogValue("Deinterlacing used", deinterlaceLines);
            }

            raiseEvent("Finalizing preprocessing...   ***PLEASE WAIT***");
            inputLine = ScriptServer.GetInputLine(path, interlaced, oPossibleSource, avsSettings.ColourCorrect, avsSettings.MPEG2Deblock, false, 0, false);
            if (!inputLine.EndsWith(")"))
                inputLine += ")";

            if (autoCrop)
                cropLine = ScriptServer.GetCropLine(true, final);
            else
                cropLine = ScriptServer.GetCropLine(false, final);

            denoiseLines = ScriptServer.GetDenoiseLines(avsSettings.Denoise, (DenoiseFilterType)avsSettings.DenoiseMethod);

            if (!keepInputResolution)
                resizeLine = ScriptServer.GetResizeLine(!signalAR || avsSettings.Mod16Method == mod16Method.resize, horizontalResolution, scriptVerticalResolution, (ResizeFilterType)avsSettings.ResizeMethod);

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
            LogItem oSourceLog = _log.LogValue("Source detection", info.analysisResult);
            if (error)
            {
                oSourceLog.LogEvent("Source detection failed: " + errorMessage, ImageType.Error);
                filters = new DeinterlaceFilter[] {
                new DeinterlaceFilter("Error", "#An error occurred in source detection. Doing no processing")};
            }
            else if (info.sourceType == SourceType.NOT_ENOUGH_SECTIONS)
            {
                oSourceLog.LogEvent("Source detection failed: Could not find enough useful sections to determine source type for " + job.Input, ImageType.Error);
                filters = new DeinterlaceFilter[] {
                new DeinterlaceFilter("Error", "#Not enough useful sections for source detection. Doing no processing")};
            }
            else
                this.filters = ScriptServer.GetDeinterlacers(info).ToArray();
            interlaced = (info.sourceType != SourceType.PROGRESSIVE);
            finished = true;
        }

        public void analyseUpdate(int amountDone, int total)
        { /*Do nothing*/ }

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
