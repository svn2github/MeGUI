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
using System.Drawing;
using System.Xml.Serialization;

using MeGUI.core.util;
using MeGUI.core.gui;

namespace MeGUI
{
	/// <summary>
	/// Summary description for MeGUISettings.
	/// </summary>
	[LogByMembers]
    public class MeGUISettings
    {
        #region variables
        public enum OCGUIMode
        {
            [EnumTitle("Show Basic Settings")]
            Basic,
            [EnumTitle("Show Default Settings")]
            Default,
            [EnumTitle("Show Advanced Settings")]
            Advanced
        };
        private string[][] autoUpdateServerLists;
        private string qaacPath, opusPath, lamePath, neroAacEncPath, mp4boxPath, mkvmergePath, strMainAudioFormat,
                       ffmpegPath, besplitPath, yadifPath, aftenPath, x264Path, strMainFileFormat,
                       dgIndexPath, xvidEncrawPath, aviMuxGUIPath, oggEnc2Path, dgavcIndexPath, aviSynthPath,
                       eac3toPath, tsmuxerPath, meguiupdatecache, avisynthpluginspath, ffmsIndexPath, vobSubPath,
                       defaultLanguage1, defaultLanguage2, afterEncodingCommand, videoExtension, audioExtension,
                       strLastDestinationPath, strLastSourcePath, dgnvIndexPath, tempDirMP4, flacPath,
                       httpproxyaddress, httpproxyport, httpproxyuid, httpproxypwd, defaultOutputDir, strMeGUIPath,
                       mkvExtractPath, appendToForcedStreams, pgcDemuxPath, lastUsedOneClickFolder;
        private bool recalculateMainMovieBitrate, autoForceFilm, autoStartQueue, enableMP3inMP4, autoOpenScript,
                     overwriteStats, keep2of3passOutput, autoUpdate, deleteCompletedJobs, deleteIntermediateFiles,
                     deleteAbortedOutput, openProgressWindow, useadvancedtooltips, autoSelectHDStreams, autoscroll, 
                     alwaysOnTop, safeProfileAlteration, usehttpproxy, addTimePosition, alwaysbackupfiles, bUseITU,
                     forcerawavcextension, bAutoLoadDG, bAutoStartQueueStartup, bAlwaysMuxMKV, b64bitX264,
                     bEnsureCorrectPlaybackSpeed, bOpenAVSInThread, bUseDGIndexNV, bUseNeroAacEnc, bExternalProgramsSet;
        private ulong audioSamplesPerUpdate;
        private decimal forceFilmThreshold, acceptableFPSError;
        private int nbPasses, acceptableAspectError, autoUpdateServerSubList, minComplexity, updateFormSplitter,
                    maxComplexity, jobColumnWidth, inputColumnWidth, outputColumnWidth, codecColumnWidth,
                    modeColumnWidth, statusColumnWidth, ownerColumnWidth, startColumnWidth, endColumnWidth, fpsColumnWidth,
                    updateFormUpdateColumnWidth, updateFormNameColumnWidth, updateFormLocalVersionColumnWidth, 
                    updateFormServerVersionColumnWidth, updateFormLocalDateColumnWidth, updateFormServerDateColumnWidth, 
                    updateFormPlatformColumnWidth, updateFormStatusColumnWidth, ffmsThreads;
        private SourceDetectorSettings sdSettings;
        private AutoEncodeDefaultsSettings aedSettings;
        private DialogSettings dialogSettings;
        private ProcessPriority defaultPriority;
        private Point mainFormLocation, updateFormLocation;
        private Size mainFormSize, updateFormSize, jobWorkerSize;
        private FileSize[] customFileSizes;
        private FPS[] customFPSs;
        private Dar[] customDARs;
        private OCGUIMode ocGUIMode;
        private AfterEncoding afterEncoding;
        #endregion
        public MeGUISettings()
		{
            strMeGUIPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            autoscroll = true;
            autoUpdateServerLists = new string[][] { new string[] { "Stable", "http://megui.org/auto/stable/", "http://megui.xvidvideo.ru/auto/stable/" },
                new string[] { "Development", "http://megui.org/auto/", "http://megui.xvidvideo.ru/auto/" }, new string[] { "Custom"}};
            acceptableFPSError = 0.01M;
            autoUpdateServerSubList = 0;
            autoUpdate = true;
            dialogSettings = new DialogSettings();
            sdSettings = new SourceDetectorSettings();
            AedSettings = new AutoEncodeDefaultsSettings();
            useadvancedtooltips = true;
            audioSamplesPerUpdate = 100000;
            aviMuxGUIPath = getDownloadPath(@"tools\avimux_gui\avimux_gui.exe");
            qaacPath = getDownloadPath(@"tools\qaac\qaac.exe");
            opusPath = getDownloadPath(@"tools\opus\opusenc.exe");
			mp4boxPath = getDownloadPath(@"tools\mp4box\mp4box.exe");
			mkvmergePath = getDownloadPath(@"tools\mkvmerge\mkvmerge.exe");
            mkvExtractPath = getDownloadPath(@"tools\mkvmerge\mkvextract.exe");
            pgcDemuxPath = getDownloadPath(@"tools\pgcdemux\pgcdemux.exe");
#if x64
            x264Path = getDownloadPath(@"tools\x264\x264_64.exe");
            b64bitX264 = true;

#endif
#if x86
            x264Path = getDownloadPath(@"tools\x264\x264.exe");
            if (OSInfo.isWow64())
                b64bitX264 = true;
#endif
            dgIndexPath = getDownloadPath(@"tools\dgindex\dgindex.exe");
            ffmsIndexPath = getDownloadPath(@"tools\ffms\ffmsindex.exe");
            xvidEncrawPath = getDownloadPath(@"tools\xvid_encraw\xvid_encraw.exe");
            lamePath = getDownloadPath(@"tools\lame\lame.exe");
            neroAacEncPath = strMeGUIPath + @"\tools\eac3to\neroAacEnc.exe";
            oggEnc2Path = getDownloadPath(@"tools\oggenc2\oggenc2.exe");
            ffmpegPath = getDownloadPath(@"tools\ffmpeg\ffmpeg.exe");
            aftenPath = getDownloadPath(@"tools\aften\aften.exe");
            flacPath = getDownloadPath(@"tools\flac\flac.exe");
            yadifPath = getDownloadPath(@"tools\yadif\yadif.dll");
            vobSubPath = getDownloadPath(@"tools\vobsub\vobsub.dll");
            besplitPath = getDownloadPath(@"tools\besplit\besplit.exe");
            dgavcIndexPath = getDownloadPath(@"tools\dgavcindex\dgavcindex.exe");
            dgnvIndexPath = getDownloadPath(@"tools\dgindexnv\dgindexnv.exe");
            eac3toPath = getDownloadPath(@"tools\eac3to\eac3to.exe");
            tsmuxerPath = getDownloadPath(@"tools\tsmuxer\tsmuxer.exe");
            aviSynthPath = getDownloadPath(@"tools\avs\avisynth.dll");
            meguiupdatecache = System.IO.Path.Combine(strMeGUIPath, "update_cache");
            avisynthpluginspath = System.IO.Path.Combine(strMeGUIPath, @"tools\avisynth_plugin");
            recalculateMainMovieBitrate = false;
			autoForceFilm = true;
            bAutoLoadDG = true;
			autoStartQueue = true;
            bAlwaysMuxMKV = true;
            bAutoStartQueueStartup = false;
			forceFilmThreshold = new decimal(95);
			defaultLanguage1 = "";
			defaultLanguage2 = "";
            defaultPriority = ProcessPriority.IDLE;
            acceptableAspectError = 1;
            afterEncoding = AfterEncoding.DoNothing;
			autoOpenScript = true;
			enableMP3inMP4 = false;
			overwriteStats = true;
			keep2of3passOutput = false;
			deleteCompletedJobs = false;
			nbPasses = 2;
			deleteIntermediateFiles = true;
			deleteAbortedOutput = true;
            autoSelectHDStreams = true;
			openProgressWindow = true;
            videoExtension = "";
            audioExtension = "";
            safeProfileAlteration = false;
            alwaysOnTop = false;
            usehttpproxy = false;
            httpproxyaddress = "";
            httpproxyport = "";
            httpproxyuid = "";
            httpproxypwd = "";
            defaultOutputDir = "";
            tempDirMP4 = "";
            addTimePosition = true;
            alwaysbackupfiles = true;
            forcerawavcextension = false;
            strMainFileFormat = "";
            strMainAudioFormat = "";
            strLastSourcePath = "";
            strLastDestinationPath = "";
            minComplexity = 72;
            maxComplexity = 78;
            mainFormLocation = new Point(0, 0);
            mainFormSize = new Size(524, 558);
            updateFormLocation = new Point(0, 0);
            updateFormSize = new Size(710, 313);
            updateFormSplitter = 180;
            updateFormUpdateColumnWidth = 47;
            updateFormNameColumnWidth = 105;
            updateFormLocalVersionColumnWidth = 117; 
            updateFormServerVersionColumnWidth = 117;
            updateFormLocalDateColumnWidth = 70;
            updateFormServerDateColumnWidth = 70;
            updateFormPlatformColumnWidth = 52;
            updateFormStatusColumnWidth = 111;
            jobWorkerSize = new Size(565, 498);
            jobColumnWidth = 40;
            inputColumnWidth = 89;
            outputColumnWidth = 89;
            codecColumnWidth = 43;
            ModeColumnWidth = 75;
            statusColumnWidth = 51;
            ownerColumnWidth = 60;
            startColumnWidth = 55;
            endColumnWidth = 55;
            fpsColumnWidth = 35;
            bEnsureCorrectPlaybackSpeed = bUseDGIndexNV = bUseNeroAacEnc = bExternalProgramsSet = false;
            ffmsThreads = 1;
            appendToForcedStreams = "";
            ocGUIMode = OCGUIMode.Default;
            bUseITU = true;
            bOpenAVSInThread = true;
            lastUsedOneClickFolder = "";
        }

        private string getDownloadPath(string strPath)
        {
            strPath = System.IO.Path.Combine(strMeGUIPath, @strPath);
            return strPath;
        }

        #region properties
        public string YadifPath
        {
            get { return yadifPath; }
        }

        public Point MainFormLocation
        {
            get { return mainFormLocation; }
            set { mainFormLocation = value; }
        }

        public Size MainFormSize
        {
            get { return mainFormSize; }
            set { mainFormSize = value; }
        }

        public Point UpdateFormLocation
        {
            get { return updateFormLocation; }
            set { updateFormLocation = value; }
        }

        public Size UpdateFormSize
        {
            get { return updateFormSize; }
            set { updateFormSize = value; }
        }

        public int UpdateFormSplitter
        {
            get { return updateFormSplitter; }
            set { updateFormSplitter = value; }
        }

        public int UpdateFormUpdateColumnWidth
        {
            get { return updateFormUpdateColumnWidth; }
            set { updateFormUpdateColumnWidth = value; }
        }

        public int UpdateFormNameColumnWidth
        {
            get { return updateFormNameColumnWidth; }
            set { updateFormNameColumnWidth = value; }
        }

        public int UpdateFormLocalVersionColumnWidth
        {
            get { return updateFormLocalVersionColumnWidth; }
            set { updateFormLocalVersionColumnWidth = value; }
        }

        public int UpdateFormServerVersionColumnWidth
        {
            get { return updateFormServerVersionColumnWidth; }
            set { updateFormServerVersionColumnWidth = value; }
        }

        public int UpdateFormLocalDateColumnWidth
        {
            get { return updateFormLocalDateColumnWidth; }
            set { updateFormLocalDateColumnWidth = value; }
        }
                
        public int UpdateFormServerDateColumnWidth
        {
            get { return updateFormServerDateColumnWidth; }
            set { updateFormServerDateColumnWidth = value; }
        }

        public int UpdateFormPlatformColumnWidth
        {
            get { return updateFormPlatformColumnWidth; }
            set { updateFormPlatformColumnWidth = value; }
        }

        public int UpdateFormStatusColumnWidth
        {
            get { return updateFormStatusColumnWidth; }
            set { updateFormStatusColumnWidth = value; }
        }

        public Size JobWorkerSize
        {
            get { return jobWorkerSize; }
            set { jobWorkerSize = value; }
        }

        public int JobColumnWidth
        {
            get { return jobColumnWidth; }
            set { jobColumnWidth = value; }
        }

        public int InputColumnWidth
        {
            get { return inputColumnWidth; }
            set { inputColumnWidth = value; }
        }

        public int OutputColumnWidth
        {
            get { return outputColumnWidth; }
            set { outputColumnWidth = value; }
        }

        public int CodecColumnWidth
        {
            get { return codecColumnWidth; }
            set { codecColumnWidth = value; }
        }

        public int ModeColumnWidth
        {
            get { return modeColumnWidth; }
            set { modeColumnWidth = value; }
        }

        public int StatusColumnWidth
        {
            get { return statusColumnWidth; }
            set { statusColumnWidth = value; }
        }

        public int OwnerColumnWidth
        {
            get { return ownerColumnWidth; }
            set { ownerColumnWidth = value; }
        }

        public int StartColumnWidth
        {
            get { return startColumnWidth; }
            set { startColumnWidth = value; }
        }

        public int EndColumnWidth
        {
            get { return endColumnWidth; }
            set { endColumnWidth = value; }
        }

        public int FPSColumnWidth
        {
            get { return fpsColumnWidth; }
            set { fpsColumnWidth = value; }
        }

        public FileSize[] CustomFileSizes
        {
            get { return customFileSizes; }
            set { customFileSizes = value; }
        }

        public FPS[] CustomFPSs
        {
            get { return customFPSs; }
            set { customFPSs = value; }
        }

        public Dar[] CustomDARs
        {
            get { return customDARs; }
            set { customDARs = value; }
        }

        public ulong AudioSamplesPerUpdate
        {
            get { return audioSamplesPerUpdate; }
            set { audioSamplesPerUpdate = value; }
        }

        public string LastSourcePath
        {
            get { return strLastSourcePath; }
            set { strLastSourcePath = value; }
        }

        public string LastDestinationPath
        {
            get { return strLastDestinationPath; }
            set { strLastDestinationPath = value; }
        }

        /// <summary>
        /// Gets / sets whether the log should be autoscrolled
        /// </summary>
        public bool AutoScrollLog
        {
            get { return autoscroll; }
            set { autoscroll = value; }
        }

        /// <summary>
        /// Gets / sets whether the one-click advanced settings will be shown
        /// </summary>
        public OCGUIMode OneClickGUIMode
        {
            get { return ocGUIMode; }
            set { ocGUIMode = value; }
        }

        /// <summary>
        /// Gets / sets whether the playback speed in video preview should match the fps
        /// </summary>
        public bool EnsureCorrectPlaybackSpeed
        {
            get { return bEnsureCorrectPlaybackSpeed; }
            set { bEnsureCorrectPlaybackSpeed = value; }
        }

        public bool SafeProfileAlteration
        {
            get { return safeProfileAlteration; }
            set { safeProfileAlteration = value; }
        }

        /// <summary>
        /// Maximum error that the bitrate calculator should accept when rounding the framerate
        /// </summary>
        public decimal AcceptableFPSError
        {
            get { return acceptableFPSError; }
            set { acceptableFPSError = value; }
        }
        /// <summary>
        /// Which sublist to look in for the update servers
        /// </summary>
        public int AutoUpdateServerSubList
        {
            get { return autoUpdateServerSubList; }
            set { autoUpdateServerSubList = value; }
        }
        /// <summary>
        /// List of servers to use for autoupdate
        /// </summary>
        public string[][] AutoUpdateServerLists
        {
            get 
            {
                if (autoUpdateServerLists.Length > 2)
                {
                    autoUpdateServerLists[0] = new string[] { "Stable", "http://megui.org/auto/stable/", "http://megui.xvidvideo.ru/auto/stable/" };
                    autoUpdateServerLists[1] = new string[] { "Development", "http://megui.org/auto/", "http://megui.xvidvideo.ru/auto/" };
                }
                else
                {
                    autoUpdateServerLists = new string[][] { new string[] { "Stable", "http://megui.org/auto/stable/", "http://megui.xvidvideo.ru/auto/stable/" },
                                                             new string[] { "Development", "http://megui.org/auto/", "http://megui.xvidvideo.ru/auto/" }, new string[] {"Custom"}};
                }
#if x64
                autoUpdateServerLists = new string[][] { new string[] { "Stable", "http://megui.org/auto/", "http://megui.xvidvideo.ru/auto/" },
                                                         new string[] { "Development", "http://megui.org/auto/", "http://megui.xvidvideo.ru/auto/" },
                                                         new string[] { "Custom", "http://megui.org/auto/", "http://megui.xvidvideo.ru/auto/" }};
#endif
#if DEBUG
                autoUpdateServerLists = new string[][] { new string[] { "Stable", "http://megui.org/auto/" },
                                                         new string[] { "Development", "http://megui.org/auto/" },
                                                         new string[] { "Custom", "http://megui.org/auto/" }};
#endif
                return autoUpdateServerLists;
            }
            set { autoUpdateServerLists = value; }
        }
        /// <summary>
        /// What to do after all encodes are finished
        /// </summary>
        public AfterEncoding AfterEncoding
        {
            get { return afterEncoding; }
            set { afterEncoding = value; }
        }
        /// <summary>
        /// Command to run after encoding is finished (only if AfterEncoding is RunCommand)
        /// </summary>
        public string AfterEncodingCommand
        {
            get { return afterEncodingCommand; }
            set { afterEncodingCommand = value; }
        }
        /// <summary>
        /// Maximum aspect error (%) to allow in resizing.
        /// </summary>
        public int AcceptableAspectErrorPercent
        {
            get { return acceptableAspectError; }
            set { acceptableAspectError = value; }
        }
        /// <summary>
        /// bool to decide whether to use advanced or basic tooltips
        /// </summary>
        public bool UseAdvancedTooltips
        {
            get { return useadvancedtooltips; }
            set { useadvancedtooltips = value; }
        }
        /// <summary>
        /// bool to decide whether to use 64bit x264
        /// </summary>
        public bool Use64bitX264
        {
            get { return b64bitX264; }
            set { b64bitX264 = value; }
        }
        ///<summary>
        /// gets / sets whether megui puts the Video Preview Form "Alwyas on Top" or not
        /// </summary>
        public bool AlwaysOnTop
        {
            get { return alwaysOnTop; }
            set { alwaysOnTop = value; }
        }
        ///<summary>
        /// gets / sets whether megui add the Time Position or not to the Video Player
        /// </summary>
        public bool AddTimePosition
        {
            get { return addTimePosition; }
            set { addTimePosition = value; }
        }
        /// <summary>
        /// gets / sets the default output directory
        /// </summary>
        public string DefaultOutputDir
        {
            get { return defaultOutputDir; }
            set { defaultOutputDir = value; }
        }
        /// <summary>
        /// gets / sets the temp directory for MP4 Muxer
        /// </summary>
        public string TempDirMP4
        {
            get 
            {
                if (String.IsNullOrEmpty(tempDirMP4) || System.IO.Path.GetPathRoot(tempDirMP4).Equals(tempDirMP4, StringComparison.CurrentCultureIgnoreCase))
                    return String.Empty;
                return tempDirMP4;
            }
            set { tempDirMP4 = value; }
        }
        /// <summary>
        /// path of besplit.exe
        /// </summary>
        public string BeSplitPath
        {
            get { return besplitPath; }
        }

        /// <summary>
        /// filename and full path of the ffmpeg executable
        /// </summary>
        public string FFMpegPath
        {
            get { return ffmpegPath; }
        }

        /// <summary>
        /// filename and full path of the vobsub dll
        /// </summary>
        public string VobSubPath
        {
            get { return vobSubPath; }
        }
        
        /// <summary>
        /// filename and full path of the oggenc2 executable
        /// </summary>
        public string OggEnc2Path
        {
            get { return oggEnc2Path; }
        }

        /// <summary>
        /// filename and full path of the qaac executable
        /// </summary>
        public string QaacPath
        {
            get { return qaacPath; }
        }

        /// <summary>
        /// filename and full path of the opus executable
        /// </summary>
        public string OpusPath
        {
            get { return opusPath; }
        }

        /// <summary>
        /// filename and full path of the faac executable
        /// </summary>
        public string LamePath
        {
            get { return lamePath; }
        }	
	    
	    /// <summary>
		/// filename and full path of the mkvmerge executable
		/// </summary>
		public string MkvmergePath
		{
			get {return mkvmergePath;}
		}

        /// <summary>
        /// filename and full path of the mkvextract executable
        /// </summary>
        public string MkvExtractPath
        {
            get { return mkvExtractPath; }
        }

        /// <summary>
		/// filename and full path of the mp4creator executable
		/// </summary>
		public string Mp4boxPath
		{
			get {return mp4boxPath;}
		}

        /// <summary>
        /// filename and full path of the pgcdemux executable
        /// </summary>
        public string PgcDemuxPath
        {
            get { return pgcDemuxPath; }
        }

		/// <summary>
		/// filename and full path of the x264 executable
		/// </summary>
		public string X264Path
		{
			get {return x264Path;}
		}

		/// <summary>
		/// filename and full path of the dgindex executable
		/// </summary>
		public string DgIndexPath
		{
			get {return dgIndexPath;}
		}
        /// <summary>
        /// filename and full path of the ffmsindex executable
        /// </summary>
        public string FFMSIndexPath
        {
            get { return ffmsIndexPath; }
        }
        /// <summary>
        /// filename and full path of the avisynth dll
        /// </summary>
        public string AviSynthPath
        {
            get { return aviSynthPath; }
        }
        /// <summary>
        /// filename and full path of the xvid_encraw executable
        /// </summary>
        public string XviDEncrawPath
        {
            get { return xvidEncrawPath; }
        }
        /// <summary>
        /// gets / sets the path of the avimuxgui executable
        /// </summary>
        public string AviMuxGUIPath
        {
            get { return aviMuxGUIPath; }
        }
        /// <summary>
        /// filename and full path of the aften executable
        /// </summary>
        public string AftenPath
        {
            get { return aftenPath; }
        }
        /// <summary>
        /// filename and full path of the flac executable
        /// </summary>
        public string FlacPath
        {
            get { return flacPath; }
        }
        /// <summary>
        /// filename and full path of the dgavcindex executable
        /// </summary>
        public string DgavcIndexPath
        {
            get { return dgavcIndexPath; }
        }
        /// <summary>
        /// filename and full path of the dgmpgindex executable
        /// </summary>
        public string DgnvIndexPath
        {
            get { return dgnvIndexPath; }
        }
        /// <summary>
        /// filename and full path of the eac3to executable
        /// </summary>
        public string EAC3toPath
        {
            get { return eac3toPath; }
        }
        /// <summary>
        /// filename and full path of the tsmuxer executable
        /// </summary>
        public string TSMuxerPath
        {
            get { return tsmuxerPath; }
        }
        ///<summary>
        /// gets / sets whether megui backup files from updater or not
        /// </summary>
        public bool AlwaysBackUpFiles
        {
            get { return alwaysbackupfiles; }
            set { alwaysbackupfiles = value; }
        }
        ///<summary>
        /// gets / sets whether to force raw AVC file Extension for QuickTime compatibility
        /// more infos here : http://forum.doom9.org/showthread.php?p=1243370#post1243370
        /// </summary>
        public bool ForceRawAVCExtension
        {
            get { return forcerawavcextension; }
            set { forcerawavcextension = value; }
        }

        /// <summary>
        /// Haali Media Splitter Path
        /// </summary>
        public static string HaaliMSPath
        {
            get
            {
                try
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\HaaliMkx");

                    if (key == null)
                        key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\HaaliMkx");

                    if (key == null)
                        return null;
                    else
                        return (string)key.GetValue("Install_Dir");
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// folder containing the avisynth plugins
        /// </summary>
        public string AvisynthPluginsPath
        {
            get { return avisynthpluginspath; }
        }

        /// <summary>
        /// folder containing local copies of update files
        /// </summary>
        public string MeGUIUpdateCache
        {
            get { return meguiupdatecache; }
        }

        /// <summary>
		/// should the video bitrate be recalculated after credits encoding in video only mode?
		/// </summary>
		public bool RecalculateMainMovieBitrate
		{
			get {return recalculateMainMovieBitrate;}
			set {recalculateMainMovieBitrate = value;}
		}
		/// <summary>
		/// should force film automatically be applies if the film percentage crosses the forceFilmTreshold?
		/// </summary>
		public bool AutoForceFilm
		{
			get {return autoForceFilm;}
			set {autoForceFilm = value;}
		}
        /// <summary>
        /// should the file autoloaded incrementally if VOB
        /// </summary>
        public bool AutoLoadDG
        {
            get { return bAutoLoadDG; }
            set { bAutoLoadDG = value; }
        }
        /// <summary>
        /// true if HD Streams Extractor should automatically select tracks
        /// </summary>
        public bool AutoSelectHDStreams
        {
            get { return autoSelectHDStreams; }
            set { autoSelectHDStreams = value; }
        }
        /// <summary>
		/// gets / sets whether pressing Queue should automatically start encoding at startup
		/// </summary>
		public bool AutoStartQueueStartup
		{
            get { return bAutoStartQueueStartup; }
            set { bAutoStartQueueStartup = value; }
		}
        /// <summary>
        /// gets / sets whether MKV files should always be muxed with mkvmerge even if x264 can output it directly
        /// </summary>
        public bool AlwaysMuxMKV
        {
            get { return bAlwaysMuxMKV; }
            set { bAlwaysMuxMKV = value; }
        }
		/// <summary>
		/// gets / sets whether pressing Queue should automatically start encoding
		/// </summary>
		public bool AutoStartQueue
		{
			get {return autoStartQueue;}
			set {autoStartQueue = value;}
		}
		/// <summary>
		/// gets / sets whether megui automatically opens the preview window upon loading an avisynth script
		/// </summary>
		public bool AutoOpenScript
		{
			get {return autoOpenScript;}
			set {autoOpenScript = value;}
		}
		/// <summary>
		/// gets / sets whether the progress window should be opened for each job
		/// </summary>
		public bool OpenProgressWindow
		{
			get {return openProgressWindow;}
			set {openProgressWindow = value;}
		}
		/// <summary>
		/// the threshold to apply force film. If the film percentage is higher than this threshold,
		/// force film will be applied
		/// </summary>
        public decimal ForceFilmThreshold
		{
			get {return forceFilmThreshold;}
			set {forceFilmThreshold = value;}
		}
		/// <summary>
		/// <summary>
		/// first default language
		/// </summary>
		public string DefaultLanguage1
		{
			get {return defaultLanguage1;}
			set {defaultLanguage1 = value;}
		}
		/// <summary>
		/// second default language
		/// </summary>
		public string DefaultLanguage2
		{
			get {return defaultLanguage2;}
			set {defaultLanguage2 = value;}
		}
		/// <summary>
		/// default priority for all processes
		/// </summary>
        public ProcessPriority DefaultPriority
		{
			get {return defaultPriority;}
			set {defaultPriority = value;}
		}
        private ProcessPriority processingPriority;
        private bool processingPrioritySet;
        /// <summary>
        /// default priority for all new processes during this session
        /// </summary>
        [XmlIgnore()]
        public ProcessPriority ProcessingPriority
        {
            get 
            {
                if (!processingPrioritySet)
                {
                    processingPriority = defaultPriority;
                    processingPrioritySet = true;
                }
                return processingPriority; 
            }
            set { processingPriority = value; }
        }
        /// <summary>
        /// open AVS files in a thread
        /// </summary>
        public bool OpenAVSInThread
        {
            get { return bOpenAVSInThread; }
            set { bOpenAVSInThread = value; }
        }
        private bool bOpenAVSInThreadDuringSession;
        private bool bOpenAVSInThreadDuringSessionSet;
        /// <summary>
        /// default priority for all new processes during this session
        /// </summary>
        [XmlIgnore()]
        public bool OpenAVSInThreadDuringSession
        {
            get
            {
                if (!bOpenAVSInThreadDuringSessionSet)
                {
                    bOpenAVSInThreadDuringSession = bOpenAVSInThread;
                    bOpenAVSInThreadDuringSessionSet = true;
                }
                return bOpenAVSInThreadDuringSession;
            }
            set { bOpenAVSInThreadDuringSession = value; }
        }
		/// <summary>
		/// enables no spec compliant mp3 in mp4 muxing
		/// </summary>
		public bool EnableMP3inMP4
		{
			get {return enableMP3inMP4;}
			set {enableMP3inMP4 = value;}
		}
		/// <summary>
		/// sets / gets if the stats file is updated in the 3rd pass of 3 pass encoding
		/// </summary>
		public bool OverwriteStats
		{
			get {return overwriteStats;}
			set {overwriteStats = value;}
		}
		/// <summary>
		///  gets / sets if the output is video output of the 2nd pass is overwritten in the 3rd pass of 3 pass encoding
		/// </summary>
		public bool Keep2of3passOutput
		{
			get {return keep2of3passOutput;}
			set {keep2of3passOutput = value;}
		}
		/// <summary>
		/// sets the number of passes to be done in automated encoding mode
		/// </summary>
		public int NbPasses
		{
			get {return nbPasses;}
			set {nbPasses = value;}
		}
		/// <summary>
		/// sets / gets whether completed jobs will be deleted
		/// </summary>
		public bool DeleteCompletedJobs
		{
			get {return deleteCompletedJobs;}
			set {deleteCompletedJobs = value;}
		}
		/// <summary>
		/// sets / gets if intermediate files are to be deleted after encoding
		/// </summary>
		public bool DeleteIntermediateFiles
		{
			get {return deleteIntermediateFiles;}
			set {deleteIntermediateFiles = value;}
		}
		/// <summary>
		/// gets / sets if the output of an aborted job is to be deleted
		/// </summary>
		public bool DeleteAbortedOutput
		{
			get {return deleteAbortedOutput;}
			set {deleteAbortedOutput = value;}
		}
        public string VideoExtension
        {
            get {return videoExtension;}
            set {videoExtension = value;}
        }
        public string AudioExtension
        {
            get { return audioExtension; }
            set { audioExtension = value; }
        }
        public bool AutoUpdate
        {
            get { return autoUpdate; }
            set { autoUpdate = value; }
        }
        public DialogSettings DialogSettings
        {
            get { return dialogSettings; }
            set { dialogSettings = value; }
        }
        public SourceDetectorSettings SourceDetectorSettings
        {
            get { return sdSettings; }
            set { sdSettings = value; }
        }
        /// <summary>
        /// gets / sets the default settings for the autoencode window
        /// </summary>
        public AutoEncodeDefaultsSettings AedSettings
        {
            get { return aedSettings; }
            set { aedSettings = value; }
        }
        /// <summary>
        /// gets / sets the default settings for the Proxy
        /// </summary>
        public bool UseHttpProxy 
        {
            get { return usehttpproxy; }
            set { usehttpproxy = value; }
        }
        /// <summary>
        /// gets / sets the default settings for the Proxy Adress
        /// </summary>
        public string HttpProxyAddress
        { 
            get { return httpproxyaddress;}
            set {httpproxyaddress = value;}
        }
        /// <summary>
        /// gets / sets the default settings for the Proxy Port
        /// </summary>
        public string HttpProxyPort
        { 
            get { return httpproxyport;}
            set {httpproxyport = value;}
        }
        /// <summary>
        /// gets / sets the default settings for the Proxy Uid
        /// </summary>
        public string HttpProxyUid
        { 
            get { return httpproxyuid;}
            set {httpproxyuid = value;}
        }
        /// <summary>
        /// gets / sets the default settings for the Proxy Password
        /// </summary>
        public string HttpProxyPwd
        {
            get { return httpproxypwd; }
            set { httpproxypwd = value; }
        }

        /// <summary>
        /// gets / sets the text to append to forced streams
        /// </summary>
        public string AppendToForcedStreams
        {
            get { return appendToForcedStreams; }
            set { appendToForcedStreams = value; }
        }

        public string MainAudioFormat
        {
            get { return strMainAudioFormat; }
            set { strMainAudioFormat = value; }
        }

        public string MainFileFormat
        {
            get { return strMainFileFormat; }
            set { strMainFileFormat = value; }
        }

        public string LastUsedOneClickFolder
        {
            get { return lastUsedOneClickFolder; }
            set { lastUsedOneClickFolder = value; }
        }

        public int MinComplexity
        {
            get { return minComplexity; }
            set { minComplexity = value; }
        }

        public int MaxComplexity
        {
            get { return maxComplexity; }
            set { maxComplexity = value; }
        }

        public int FFMSThreads
        {
            get { return ffmsThreads; }
            set { ffmsThreads = value; }
        }

        public bool UseITUValues
        {
            get { return bUseITU; }
            set { bUseITU = value; }
        }

        public bool UseDGIndexNV
        {
            get 
            {
                if (!bExternalProgramsSet)
                    SetExternalPrograms();
                return bUseDGIndexNV; 
            }
            set { bUseDGIndexNV = value; }
        }

        /// <summary>
        /// filename and full path of the neroaacenc executable
        /// </summary>
        public string NeroAacEncPath
        {
            get { return neroAacEncPath; }
            set
            {
                if (!System.IO.File.Exists(value))
                    neroAacEncPath = strMeGUIPath + @"\tools\eac3to\neroAacEnc.exe";
                else
                    neroAacEncPath = value;
            }
        }	

        public bool UseNeroAacEnc
        {
            get 
            {
                if (!bExternalProgramsSet)
                    SetExternalPrograms();
                return bUseNeroAacEnc; 
            }
            set { bUseNeroAacEnc = value; }
        }

        /// <summary>
        /// if the external programs (nero & dgindexnv) have been already set
        /// </summary>
        public bool ExternalProgramsSet
        {
            get { return bExternalProgramsSet; }
            set { bExternalProgramsSet = value; }
        }

        #endregion

        private bool bAutoUpdateSession;
        /// <summary>
        /// automatic update process
        /// </summary>
        [XmlIgnore()]
        public bool AutoUpdateSession
        {
            get { return bAutoUpdateSession; }
            set { bAutoUpdateSession = value; }
        }

        #region Methods
#warning Deprecated after next stable release
        private void SetExternalPrograms()
        {
            // check NeroAacEnc
            bUseNeroAacEnc = System.IO.File.Exists(neroAacEncPath);

            // check if the DGIndexNV license file is available
            if (System.IO.File.Exists(MainForm.Instance.Settings.DgnvIndexPath) && 
                System.IO.File.Exists(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.DgnvIndexPath), "license.txt")))
                bUseDGIndexNV = true;
            else
                bUseDGIndexNV = false;

            bExternalProgramsSet = true;
        }

        public bool IsNeroAACEncAvailable()
        {
            if (!bExternalProgramsSet)
                SetExternalPrograms();
            return bUseNeroAacEnc && System.IO.File.Exists(neroAacEncPath);
        }

        public bool IsDGIIndexerAvailable()
        {
            if (!bUseDGIndexNV)
                return false;

            // check if the license file is available
            if (!System.IO.File.Exists(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(MainForm.Instance.Settings.DgnvIndexPath), "license.txt")))
                return false;

            // DGI is not available in a RDP connection
            if (System.Windows.Forms.SystemInformation.TerminalServerSession == true)
                return false;

            // check if the indexer is available
            if (!System.IO.File.Exists(MainForm.Instance.Settings.DgnvIndexPath))
                return false;

            return true;
        }
        #endregion
    }
    public enum AfterEncoding { DoNothing = 0, Shutdown = 1, RunCommand = 2, CloseMeGUI = 3 }
}
