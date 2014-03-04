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
        private Thread _processTime = null;
        private SourceDetector _sourceDetector = null;
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
            if (_processTime != null)
            {
                _processTime.Abort();
                _processTime = null;
            }
            if (_sourceDetector != null)
            {
                _sourceDetector.stop();
                _sourceDetector = null;
            }
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
            if (n * 100M < su.PercentageDoneExact)
                _start = DateTime.Now;
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
            
            try
            {
                _log.LogEvent("Processing thread started");
                raiseEvent("Preprocessing...   ***PLEASE WAIT***");
                _start = DateTime.Now;
                _processTime = new Thread(new ThreadStart(delegate
                {
                    while (true)
                    {
                        updateTime();
                        Thread.Sleep(1000);
                    }
                }));
                _processTime.Start();

                List<string> arrAudioFilesDelete = new List<string>();
                audioFiles = new Dictionary<int, string>();
                List<AudioTrackInfo> arrAudioTracks = new List<AudioTrackInfo>();
                List<AudioJob> arrAudioJobs = new List<AudioJob>();
                List<MuxStream> arrMuxStreams = new List<MuxStream>();
                List<string> intermediateFiles = new List<string>();

                FileUtil.ensureDirectoryExists(job.PostprocessingProperties.WorkingDirectory);

                // audio handling
                foreach (OneClickAudioTrack oAudioTrack in job.PostprocessingProperties.AudioTracks)
                {
                    if (oAudioTrack.AudioTrackInfo != null)
                    {
                        if (oAudioTrack.AudioTrackInfo.ExtractMKVTrack)
                        {
                            if (job.PostprocessingProperties.ApplyDelayCorrection && File.Exists(job.PostprocessingProperties.IntermediateMKVFile))
                            {
                                MediaInfoFile oFile = new MediaInfoFile(job.PostprocessingProperties.IntermediateMKVFile, ref _log);
                                bool bFound = false;
                                foreach (AudioTrackInfo oAudioInfo in oFile.AudioInfo.Tracks)
                                {
                                    if (oAudioInfo.MMGTrackID == oAudioTrack.AudioTrackInfo.MMGTrackID)
                                        bFound = true;
                                }
                                int mmgTrackID = 0;
                                if (!bFound)
                                    mmgTrackID = oFile.AudioInfo.Tracks[oAudioTrack.AudioTrackInfo.TrackIndex].MMGTrackID;
                                else
                                    mmgTrackID = oAudioTrack.AudioTrackInfo.MMGTrackID;
                                foreach (AudioTrackInfo oAudioInfo in oFile.AudioInfo.Tracks)
                                {
                                    if (oAudioInfo.MMGTrackID == mmgTrackID)
                                    {
                                        if (oAudioTrack.DirectMuxAudio != null)
                                            oAudioTrack.DirectMuxAudio.delay = oAudioInfo.Delay;
                                        if (oAudioTrack.AudioJob != null)
                                            oAudioTrack.AudioJob.Delay = oAudioInfo.Delay;
                                        break;
                                    }
                                }
                            }
                            if (!audioFiles.ContainsKey(oAudioTrack.AudioTrackInfo.TrackID))
                            {
                                audioFiles.Add(oAudioTrack.AudioTrackInfo.TrackID, job.PostprocessingProperties.WorkingDirectory + "\\" + oAudioTrack.AudioTrackInfo.DemuxFileName);
                                arrAudioFilesDelete.Add(job.PostprocessingProperties.WorkingDirectory + "\\" + oAudioTrack.AudioTrackInfo.DemuxFileName);
                            }
                        }
                        else
                            arrAudioTracks.Add(oAudioTrack.AudioTrackInfo);
                    }
                    if (oAudioTrack.AudioJob != null)
                    {
                        if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.NONE
                            && String.IsNullOrEmpty(oAudioTrack.AudioJob.Input))
                            oAudioTrack.AudioJob.Input = job.Input;
                        arrAudioJobs.Add(oAudioTrack.AudioJob);
                    }
                    if (oAudioTrack.DirectMuxAudio != null)
                        arrMuxStreams.Add(oAudioTrack.DirectMuxAudio);
                }
                if (audioFiles.Count == 0 && job.PostprocessingProperties.IndexType != FileIndexerWindow.IndexType.NONE && !job.PostprocessingProperties.Eac3toDemux)
                    audioFiles = vUtil.getAllDemuxedAudio(arrAudioTracks, new List<AudioTrackInfo>(), out arrAudioFilesDelete, job.IndexFile, _log);

                fillInAudioInformation(ref arrAudioJobs, arrMuxStreams);

                if (!String.IsNullOrEmpty(job.PostprocessingProperties.VideoFileToMux))
                    _log.LogEvent("Don't encode video: True");
                else
                    _log.LogEvent("Desired size: " + job.PostprocessingProperties.OutputSize);
                _log.LogEvent("Split size: " + job.PostprocessingProperties.Splitting);


                // chapter file handling
                if (String.IsNullOrEmpty(job.PostprocessingProperties.ChapterFile))
                {
                    job.PostprocessingProperties.ChapterFile = null;
                }
                else if (job.PostprocessingProperties.Container == ContainerType.AVI)
                {
                    _log.LogEvent("Chapter handling disabled because of the AVI target container");
                    job.PostprocessingProperties.ChapterFile = null;
                }
                else if (!File.Exists(job.PostprocessingProperties.ChapterFile))
                {
                    if (job.PostprocessingProperties.ChapterFile.StartsWith("<") || job.PostprocessingProperties.ChapterExtracted)
                    {
                        // internal chapter file
                        string strTempFile = job.PostprocessingProperties.ChapterFile;
                        if (File.Exists(job.PostprocessingProperties.IFOInput))
                        {
                            job.PostprocessingProperties.ChapterFile = VideoUtil.getChaptersFromIFO(job.PostprocessingProperties.IFOInput, false, job.PostprocessingProperties.WorkingDirectory, job.PostprocessingProperties.TitleNumberToProcess);
                            if (!String.IsNullOrEmpty(job.PostprocessingProperties.ChapterFile))
                            {
                                intermediateFiles.Add(job.PostprocessingProperties.ChapterFile);
                                job.PostprocessingProperties.ChapterExtracted = true;
                            }
                            else
                                job.PostprocessingProperties.ChapterFile = strTempFile;
                        }
                        else if (Path.GetExtension(job.PostprocessingProperties.VideoInput).ToLowerInvariant().Equals(".mkv"))
                        {
                            MediaInfoFile oInfo = new MediaInfoFile(job.PostprocessingProperties.VideoInput, ref _log);
                            if (oInfo.hasMKVChapters())
                            {
                                job.PostprocessingProperties.ChapterFile = Path.Combine(job.PostprocessingProperties.WorkingDirectory, Path.GetFileNameWithoutExtension(job.IndexFile) + " - Chapter Information.txt");
                                if (oInfo.extractMKVChapters(job.PostprocessingProperties.ChapterFile))
                                {
                                    intermediateFiles.Add(job.PostprocessingProperties.ChapterFile);
                                    job.PostprocessingProperties.ChapterExtracted = true;
                                }
                                else
                                    job.PostprocessingProperties.ChapterFile = strTempFile;
                            }
                        }
                    }
                    if (!File.Exists(job.PostprocessingProperties.ChapterFile))
                    {
                        _log.LogEvent("File not found: " + job.PostprocessingProperties.ChapterFile, ImageType.Error);
                        job.PostprocessingProperties.ChapterFile = null;
                    }
                }
                else if (job.PostprocessingProperties.ChapterExtracted)
                {
                    intermediateFiles.Add(job.PostprocessingProperties.ChapterFile);
                }

                string avsFile = String.Empty;
                VideoStream myVideo = new VideoStream();
                VideoCodecSettings videoSettings = job.PostprocessingProperties.VideoSettings;
                if (String.IsNullOrEmpty(job.PostprocessingProperties.VideoFileToMux))
                {
                    //Open the video
                    Dar? dar;
                    avsFile = createAVSFile(job.IndexFile, job.Input, job.PostprocessingProperties.DAR,
                        job.PostprocessingProperties.HorizontalOutputResolution, job.PostprocessingProperties.SignalAR, _log,
                        job.PostprocessingProperties.AvsSettings, job.PostprocessingProperties.AutoDeinterlace, videoSettings, out dar,
                        job.PostprocessingProperties.AutoCrop, job.PostprocessingProperties.KeepInputResolution,
                        job.PostprocessingProperties.UseChaptersMarks);

                    ulong length;
                    double framerate;
                    JobUtil.getInputProperties(out length, out framerate, avsFile);
                    myVideo.Input = avsFile;
                    myVideo.Output = Path.Combine(job.PostprocessingProperties.WorkingDirectory,
                        Path.GetFileNameWithoutExtension(job.Input) + "_Video");
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
                    if (Path.GetExtension(job.PostprocessingProperties.VideoFileToMux).Equals(".unknown") && !String.IsNullOrEmpty(oInfo.ContainerFileTypeString))
                    {
                        job.PostprocessingProperties.VideoFileToMux = Path.ChangeExtension(job.PostprocessingProperties.VideoFileToMux, oInfo.ContainerFileTypeString.ToLowerInvariant());
                        File.Move(myVideo.Output, job.PostprocessingProperties.VideoFileToMux);
                        myVideo.Output = job.PostprocessingProperties.VideoFileToMux;
                        job.PostprocessingProperties.FilesToDelete.Add(myVideo.Output);
                    }

                    myVideo.Settings = videoSettings;
                    videoSettings.VideoName = oInfo.VideoInfo.Track.Name;
                    myVideo.Framerate = (decimal)oInfo.VideoInfo.FPS;
                }

                intermediateFiles.Add(avsFile);
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

                if (!string.IsNullOrEmpty(avsFile) || !String.IsNullOrEmpty(job.PostprocessingProperties.VideoFileToMux))
                {
                    // subtitle handling
                    List<MuxStream> subtitles = new List<MuxStream>();
                    if (job.PostprocessingProperties.SubtitleTracks.Count > 0)
                    {
                        foreach (OneClickStream oTrack in job.PostprocessingProperties.SubtitleTracks)
                        {
                            if (oTrack.TrackInfo.ExtractMKVTrack)
                            {
                                //demuxed MKV
                                string trackFile = Path.GetDirectoryName(job.IndexFile) + "\\" + oTrack.TrackInfo.DemuxFileName;
                                if (File.Exists(trackFile))
                                {
                                    intermediateFiles.Add(trackFile);
                                    if (Path.GetExtension(trackFile).ToLowerInvariant().Equals(".idx"))
                                        intermediateFiles.Add(FileUtil.GetPathWithoutExtension(trackFile) + ".sub");

                                    subtitles.Add(new MuxStream(trackFile, oTrack.Language, oTrack.Name, oTrack.Delay, oTrack.DefaultStream, oTrack.ForcedStream, null));
                                }
                                else
                                    _log.LogEvent("Ignoring subtitle as the it cannot be found: " + trackFile, ImageType.Warning);
                            }
                            else
                            {
                                if (File.Exists(oTrack.DemuxFilePath))
                                    subtitles.Add(new MuxStream(oTrack.DemuxFilePath, oTrack.Language, oTrack.Name, oTrack.Delay, oTrack.DefaultStream, oTrack.ForcedStream, null));
                                else
                                    _log.LogEvent("Ignoring subtitle as the it cannot be found: " + oTrack.DemuxFilePath, ImageType.Warning);
                            }
                        }
                    }

                    JobChain c = vUtil.GenerateJobSeries(myVideo, job.PostprocessingProperties.FinalOutput, arrAudioJobs.ToArray(), 
                        subtitles.ToArray(), job.PostprocessingProperties.ChapterFile, job.PostprocessingProperties.OutputSize,
                        job.PostprocessingProperties.Splitting, job.PostprocessingProperties.Container,
                        job.PostprocessingProperties.PrerenderJob, arrMuxStreams.ToArray(),
                        _log, job.PostprocessingProperties.DeviceOutputType, null, job.PostprocessingProperties.VideoFileToMux, 
                        job.PostprocessingProperties.AudioTracks.ToArray(), true);
                    if (c == null)
                    {
                        _log.Warn("Job creation aborted");
                        return;
                    }

                    c = CleanupJob.AddAfter(c, intermediateFiles, job.PostprocessingProperties.FinalOutput);
                    mainForm.Jobs.addJobsWithDependencies(c, false);

                    // batch processing other input files if necessary
                    if (job.PostprocessingProperties.FilesToProcess.Count > 0)
                    {
                        OneClickWindow ocw = new OneClickWindow(mainForm);
                        ocw.setBatchProcessing(job.PostprocessingProperties.FilesToProcess, job.PostprocessingProperties.OneClickSetting);
                    }
                }
            }
            catch (Exception e)
            {
                _processTime.Abort();
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
            _processTime.Abort();
            su.IsComplete = true;
            raiseEvent();
        }

        private void fillInAudioInformation(ref List<AudioJob> arrAudioJobs, List<MuxStream> arrMuxStreams)
        {
            foreach (MuxStream m in arrMuxStreams)
                m.path = convertTrackNumberToFile(m.path, ref m.delay);

            List<AudioJob> tempList = new List<AudioJob>();
            foreach (AudioJob a in arrAudioJobs)
            {
                a.Input = convertTrackNumberToFile(a.Input, ref a.Delay);
                if (String.IsNullOrEmpty(a.Output) && !String.IsNullOrEmpty(a.Input))
                    a.Output = FileUtil.AddToFileName(a.Input, "_audio");
                if (!String.IsNullOrEmpty(a.Input))
                    tempList.Add(a);
            }
            arrAudioJobs = tempList;
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
            if (String.IsNullOrEmpty(input))
            {
                _log.Warn("Couldn't find audio file. Skipping track.");
                return null;
            }

            if (input.StartsWith("::") && input.EndsWith("::") && input.Length > 4)
            {
                string sub = input.Substring(2, input.Length - 4);
                try
                {
                    int t = int.Parse(sub);
                    string s = audioFiles[t];
                    if (PrettyFormatting.getDelay(s) != null)
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
        /// creates the AVS Script file
        /// if the file can be properly opened, auto-cropping is performed, then depending on the AR settings
        /// the proper resolution for automatic resizing, taking into account the derived cropping values
        /// is calculated, and finally the avisynth script is written and its name returned
        /// </summary>
        /// <param name="path">dgindex script</param>
        /// <param name="aspectRatio">aspect ratio selection to be used</param>
        /// <param name="customDAR">custom display aspect ratio for this source</param>
        /// <param name="desiredOutputWidth">desired horizontal resolution of the output</param>
        /// <param name="settings">the codec settings (used only for x264)</param>
        /// <param name="sarX">pixel aspect ratio X</param>
        /// <param name="sarY">pixel aspect ratio Y</param>
        /// <param name="height">the final height of the video</param>
        /// <param name="signalAR">whether or not ar signalling is to be used for the output 
        /// (depending on this parameter, resizing changes to match the source AR)</param>
        /// <param name="autoCrop">whether or not autoCrop is used for the input</param>
        /// <returns>the name of the AviSynth script created, empty if there was an error</returns>
        private string createAVSFile(string indexFile, string inputFile, Dar? AR, int desiredOutputWidth,
            bool signalAR, LogItem _log, AviSynthSettings avsSettings, bool autoDeint,
            VideoCodecSettings settings, out Dar? dar, bool autoCrop, bool keepInputResolution, bool useChaptersMarks)
        {
            dar = null;
            Dar customDAR;
            IMediaFile iMediaFile = null;
            IVideoReader reader;
            PossibleSources oPossibleSource;
            x264Device xTargetDevice = null;
            CropValues cropValues = new CropValues();

            int outputWidthIncludingPadding = 0;
            int outputHeightIncludingPadding = 0;
            int outputWidthCropped = 0;
            int outputHeightCropped = 0;

            // open index file to retrieve information
            if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.DGI)
            {
                iMediaFile = new dgiFile(indexFile);
                oPossibleSource = PossibleSources.dgi;
            }
            else if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.D2V)
            {
                iMediaFile = new d2vFile(indexFile);
                oPossibleSource = PossibleSources.d2v;
            }
            else if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.DGA)
            {
                iMediaFile = new dgaFile(indexFile);
                oPossibleSource = PossibleSources.dga;
            }
            else if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.FFMS)
            {
                iMediaFile = new ffmsFile(inputFile, indexFile);
                oPossibleSource = PossibleSources.ffindex;
            }
            else if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.LSMASH)
            {
                iMediaFile = new lsmashFile(inputFile, indexFile);
                oPossibleSource = PossibleSources.lsmash;
            }
            else if (job.PostprocessingProperties.IndexType == FileIndexerWindow.IndexType.AVISOURCE)
            {
                string tempAvs = "AVISource(\"" + inputFile + "\", audio=false)" + VideoUtil.getAssumeFPS(0, inputFile);
                iMediaFile = AvsFile.ParseScript(tempAvs);
                oPossibleSource = PossibleSources.directShow;
            }
            else
            {
                iMediaFile = AvsFile.OpenScriptFile(inputFile);
                oPossibleSource = PossibleSources.avs;
            }
            reader = iMediaFile.GetVideoReader();
            
            // abort if the index file is invalid
            if (reader.FrameCount < 1)
            {
                _log.Error("There are " + reader.FrameCount + " frames in the index file. Aborting...");
                return "";
            }

            if (AR == null)
            {
                // AR needs to be detected automatically now
                _log.LogValue("Auto-detect aspect ratio", AR == null);
                customDAR = iMediaFile.VideoInfo.DAR;
                if (customDAR.AR <= 0)
                {
                    customDAR = Dar.ITU16x9PAL;
                    _log.Warn(string.Format("No aspect ratio found, defaulting to {0}.", customDAR));
                }
            }
            else
                customDAR = AR.Value;
            _log.LogValue("Aspect ratio", customDAR);

            // check x264 settings (target device, chapter file)
            if (settings != null && settings is x264Settings)
            {
                x264Settings xs = (x264Settings)settings;
                xTargetDevice = xs.TargetDevice;
                _log.LogValue("Target device", xTargetDevice.Name);

                // create qpf file if necessary
                if (!String.IsNullOrEmpty(job.PostprocessingProperties.ChapterFile) && useChaptersMarks)
                {
                    qpfile = job.PostprocessingProperties.ChapterFile;
                    if ((Path.GetExtension(qpfile).ToLowerInvariant()) == ".txt")
                        qpfile = VideoUtil.convertChaptersTextFileTox264QPFile(job.PostprocessingProperties.ChapterFile, iMediaFile.VideoInfo.FPS);
                    if (File.Exists(qpfile))
                    {
                        xs.UseQPFile = true;
                        xs.QPFile = qpfile;
                    }
                }
            }

            // get mod value for resizing
            int mod = Resolution.GetModValue(avsSettings.ModValue, avsSettings.Mod16Method, signalAR);

            // crop input as it may be required (autoCrop && !keepInputResolution or Blu-Ray)
            if (Autocrop.autocrop(out cropValues, reader, signalAR, avsSettings.Mod16Method, avsSettings.ModValue) == false)
            {
                _log.Error("Autocrop failed. Aborting...");
                return "";
            }

            Dar? suggestedDar = null;
            if (desiredOutputWidth == 0)
                desiredOutputWidth = outputWidthIncludingPadding = (int)iMediaFile.VideoInfo.Width;
            else if (!avsSettings.Upsize && desiredOutputWidth > (int)iMediaFile.VideoInfo.Width)
                outputWidthIncludingPadding = (int)iMediaFile.VideoInfo.Width;
            else
                outputWidthIncludingPadding = desiredOutputWidth;
            CropValues paddingValues;

            bool resizeEnabled;
            int outputWidthWithoutUpsizing = outputWidthIncludingPadding;
            if (avsSettings.Upsize)
            {
                resizeEnabled = !keepInputResolution;
                CropValues cropValuesTemp = cropValues.Clone();
                int outputHeightIncludingPaddingTemp = 0;
                Resolution.GetResolution((int)iMediaFile.VideoInfo.Width, (int)iMediaFile.VideoInfo.Height, customDAR,
                    ref cropValuesTemp, autoCrop && !keepInputResolution, mod, ref resizeEnabled, false, signalAR, true,
                    avsSettings.AcceptableAspectError, xTargetDevice, Convert.ToDouble(iMediaFile.VideoInfo.FPS_N) / iMediaFile.VideoInfo.FPS_D,
                    ref outputWidthWithoutUpsizing, ref outputHeightIncludingPaddingTemp, out paddingValues, out suggestedDar, _log);
            }

            resizeEnabled = !keepInputResolution;
            Resolution.GetResolution((int)iMediaFile.VideoInfo.Width, (int)iMediaFile.VideoInfo.Height, customDAR,
                ref cropValues, autoCrop && !keepInputResolution, mod, ref resizeEnabled, avsSettings.Upsize, signalAR, true,
                avsSettings.AcceptableAspectError, xTargetDevice, Convert.ToDouble(iMediaFile.VideoInfo.FPS_N) / iMediaFile.VideoInfo.FPS_D, 
                ref outputWidthIncludingPadding, ref outputHeightIncludingPadding, out paddingValues, out suggestedDar, _log);
            keepInputResolution = !resizeEnabled;
            if ((keepInputResolution || signalAR) && suggestedDar.HasValue)
                dar = suggestedDar;

            // log calculated output resolution
            outputWidthCropped = outputWidthIncludingPadding - paddingValues.left - paddingValues.right;
            outputHeightCropped = outputHeightIncludingPadding - paddingValues.bottom - paddingValues.top;
            _log.LogValue("Input resolution", iMediaFile.VideoInfo.Width + "x" + iMediaFile.VideoInfo.Height);
            _log.LogValue("Desired maximum width", desiredOutputWidth);
            if (!avsSettings.Upsize && outputWidthIncludingPadding < desiredOutputWidth)
                _log.LogEvent("Desired maximum width not reached. Enable upsizing in the AviSynth profile if you want to force it.");
            if (avsSettings.Upsize && outputWidthIncludingPadding > outputWidthWithoutUpsizing)
                _log.LogValue("Desired maximum width reached with upsizing. Target width without upsizing", outputWidthWithoutUpsizing);
            if (cropValues.isCropped())
            {
                _log.LogValue("Autocrop values", cropValues);
                _log.LogValue("Cropped output resolution", outputWidthCropped + "x" + outputHeightCropped);
            }
            else
                _log.LogValue("Output resolution", outputWidthCropped + "x" + outputHeightCropped);
            if (paddingValues.isCropped())
                _log.LogValue("Padded output resolution", outputWidthIncludingPadding + "x" + outputHeightIncludingPadding);
            
            // generate the avs script based on the template
            string inputLine = "#input";
            string deinterlaceLines = "#deinterlace";
            string denoiseLines = "#denoise";
            string cropLine = "#crop";
            string resizeLine = "#resize";

            inputLine = ScriptServer.GetInputLine(inputFile, indexFile, false, oPossibleSource, false, false, false, 0, avsSettings.DSS2);
            if (!inputLine.EndsWith(")"))
                inputLine += ")";

            _log.LogValue("Automatic deinterlacing", autoDeint);
            if (autoDeint)
            {
                raiseEvent("Automatic deinterlacing...   ***PLEASE WAIT***");
                string d2vPath = indexFile;
                _sourceDetector = new SourceDetector(inputLine, d2vPath, false,
                    mainForm.Settings.SourceDetectorSettings,
                    new UpdateSourceDetectionStatus(analyseUpdate),
                    new FinishedAnalysis(finishedAnalysis));
                finished = false;
                _sourceDetector.analyse();
                waitTillAnalyseFinished();
                _sourceDetector.stop();
                _sourceDetector = null;
                deinterlaceLines = filters[0].Script;
                if (interlaced)
                    _log.LogValue("Deinterlacing used", deinterlaceLines, ImageType.Warning);
                else
                    _log.LogValue("Deinterlacing used", deinterlaceLines);              
            }

            raiseEvent("Finalizing preprocessing...   ***PLEASE WAIT***");
            inputLine = ScriptServer.GetInputLine(inputFile, indexFile, interlaced, oPossibleSource, avsSettings.ColourCorrect, avsSettings.MPEG2Deblock, false, 0, avsSettings.DSS2);
            if (!inputLine.EndsWith(")"))
                inputLine += ")";

            if (!keepInputResolution && autoCrop)
                cropLine = ScriptServer.GetCropLine(true, cropValues);

            denoiseLines = ScriptServer.GetDenoiseLines(avsSettings.Denoise, (DenoiseFilterType)avsSettings.DenoiseMethod);

            if (!keepInputResolution)
            {
                resizeLine = ScriptServer.GetResizeLine(!signalAR || avsSettings.Mod16Method == mod16Method.resize || outputWidthIncludingPadding > 0 || (int)iMediaFile.VideoInfo.Width != outputWidthCropped,
                                                        outputWidthCropped, outputHeightCropped, outputWidthIncludingPadding, outputHeightIncludingPadding, (ResizeFilterType)avsSettings.ResizeMethod,
                                                        autoCrop, cropValues, (int)iMediaFile.VideoInfo.Width, (int)iMediaFile.VideoInfo.Height);
            }

            string newScript = ScriptServer.CreateScriptFromTemplate(avsSettings.Template, inputLine, cropLine, resizeLine, denoiseLines, deinterlaceLines);

            if (dar.HasValue)
                newScript = string.Format("global MeGUI_darx = {0}\r\nglobal MeGUI_dary = {1}\r\n{2}", dar.Value.X, dar.Value.Y, newScript);
            else
            {
                if (xTargetDevice != null && xTargetDevice.BluRay)
                {
                    string strResolution = outputWidthIncludingPadding + "x" + outputHeightIncludingPadding;
                    x264Settings _xs = (x264Settings)settings;

                    if (strResolution.Equals("720x480"))
                    {
                        _xs.SampleAR = 4;
                        _log.LogEvent("Set --sar to 10:11 as only 40:33 or 10:11 are supported with a resolution of " +
                            strResolution + " as required for " + xTargetDevice.Name + ".");
                    }
                    else if (strResolution.Equals("720x576"))
                    {
                        _xs.SampleAR = 5;
                        _log.LogEvent("Set --sar to 12:11 as only 16:11 or 12:11 are supported with a resolution of "
                            + strResolution + " as required for " + xTargetDevice.Name + ".");
                    }
                    else if (strResolution.Equals("1280x720") || strResolution.Equals("1920x1080"))
                    {
                        _xs.SampleAR = 1;
                        _log.LogEvent("Set --sar to 1:1 as only 1:1 is supported with a resolution of "
                            + strResolution + " as required for " + xTargetDevice.Name + ".");
                    }
                    else if (strResolution.Equals("1440x1080"))
                    {
                        _xs.SampleAR = 2;
                        _log.LogEvent("Set --sar to 4:3 as only 4:3 is supported with a resolution of "
                            + strResolution + " as required for " + xTargetDevice.Name + ".");
                    }
                }
            }

            _log.LogValue("Generated Avisynth script", newScript);
            string strOutputAVSFile;
            if (String.IsNullOrEmpty(indexFile))
                strOutputAVSFile = Path.ChangeExtension(Path.Combine(job.PostprocessingProperties.WorkingDirectory, Path.GetFileName(inputFile)), ".avs");
            else
                strOutputAVSFile = Path.ChangeExtension(indexFile, ".avs");

            try
            {
                StreamWriter sw = new StreamWriter(strOutputAVSFile, false, System.Text.Encoding.Default);
                sw.Write(newScript);
                sw.Close();
            }
            catch (IOException i)
            {
                _log.LogValue("Error saving AviSynth script", i, ImageType.Error);
                return "";
            }
            return strOutputAVSFile;
        }

        public void finishedAnalysis(SourceInfo info, bool error, string errorMessage)
        {
            if (error || info == null)
            {
                LogItem oSourceLog = _log.LogEvent("Source detection");
                oSourceLog.LogValue("Source detection failed", errorMessage, ImageType.Warning);
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