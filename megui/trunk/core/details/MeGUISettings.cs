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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
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
        private DateTime lastUpdateCheck;
        private string strMainAudioFormat, strMainFileFormat, meguiupdatecache,
                       defaultLanguage1, defaultLanguage2, afterEncodingCommand, videoExtension, audioExtension,
                       strLastDestinationPath, strLastSourcePath, tempDirMP4, neroAacEncPath,
                       httpproxyaddress, httpproxyport, httpproxyuid, httpproxypwd, defaultOutputDir,
                       appendToForcedStreams, lastUsedOneClickFolder, lastUpdateServer;
        private bool autoForceFilm, autoStartQueue, autoOpenScript, bUseQAAC, bUseX265, bUseDGIndexNV,
                     overwriteStats, keep2of3passOutput, autoUpdate, deleteCompletedJobs, deleteIntermediateFiles,
                     deleteAbortedOutput, openProgressWindow, autoSelectHDStreams,
                     alwaysOnTop, addTimePosition, alwaysbackupfiles, bUseITU, bEac3toLastUsedFileMode,
                     bAutoLoadDG, bAutoStartQueueStartup, bAlwaysMuxMKV, b64bitX264, bAlwayUsePortableAviSynth,
                     bEnsureCorrectPlaybackSpeed, bOpenAVSInThread, bExternalMuxerX264, bUseNeroAacEnc;
        private decimal forceFilmThreshold, acceptableFPSError;
        private int nbPasses, autoUpdateServerSubList, minComplexity, updateFormSplitter,
                    maxComplexity, jobColumnWidth, inputColumnWidth, outputColumnWidth, codecColumnWidth,
                    modeColumnWidth, statusColumnWidth, ownerColumnWidth, startColumnWidth, endColumnWidth, fpsColumnWidth,
                    updateFormUpdateColumnWidth, updateFormNameColumnWidth, updateFormLocalVersionColumnWidth, 
                    updateFormServerVersionColumnWidth, updateFormLocalDateColumnWidth, updateFormServerDateColumnWidth, 
                    updateFormLastUsedColumnWidth, updateFormStatusColumnWidth, ffmsThreads, chapterCreatorMinimumLength;
        private SourceDetectorSettings sdSettings;
        private AutoEncodeDefaultsSettings aedSettings;
        private DialogSettings dialogSettings;
        private ProcessPriority defaultPriority;
        private Point mainFormLocation, updateFormLocation;
        private Size mainFormSize, updateFormSize;
        private FileSize[] customFileSizes;
        private FPS[] customFPSs;
        private Dar[] customDARs;
        private OCGUIMode ocGUIMode;
        private AfterEncoding afterEncoding;
        private ProxyMode httpProxyMode;
        private ProgramSettings aften, avimuxgui, avisynth, avisynthplugins, besplit, dgavcindex, dgindex, dgindexnv,
                                eac3to, ffmpeg, ffms, flac, lame, lsmash, mkvmerge, mp4box, neroaacenc, oggenc,
                                opus, pgcdemux, qaac, tsmuxer, vobsub, x264, x264_10b, x265, xvid;
        #endregion
        public MeGUISettings()
		{
            string strMeGUIPath = Path.GetDirectoryName(Application.ExecutablePath);
#if x64
            b64bitX264 = true;
#endif
#if x86
            if (OSInfo.isWow64())
                b64bitX264 = true;
#endif

            autoUpdateServerLists = new string[][] { new string[] { "Stable", "http://megui.org/auto/stable/", "http://megui.xvidvideo.ru/auto/stable/" },
                new string[] { "Development", "http://megui.org/auto/", "http://megui.xvidvideo.ru/auto/" }, new string[] { "Custom"}};
            lastUpdateCheck = DateTime.Now.ToUniversalTime();
            lastUpdateServer = String.Empty;
            acceptableFPSError = 0.01M;
            autoUpdateServerSubList = 0;
            autoUpdate = true;
            dialogSettings = new DialogSettings();
            sdSettings = new SourceDetectorSettings();
            AedSettings = new AutoEncodeDefaultsSettings();
            meguiupdatecache = Path.Combine(strMeGUIPath, "update_cache");
			autoForceFilm = true;
            bAutoLoadDG = true;
			autoStartQueue = true;
            bAlwaysMuxMKV = true;
            bAutoStartQueueStartup = false;
			forceFilmThreshold = new decimal(95);
			defaultLanguage1 = "";
			defaultLanguage2 = "";
            defaultPriority = ProcessPriority.IDLE;
            afterEncoding = AfterEncoding.DoNothing;
			autoOpenScript = true;
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
            alwaysOnTop = false;
            httpProxyMode = ProxyMode.None;
            httpproxyaddress = "";
            httpproxyport = "";
            httpproxyuid = "";
            httpproxypwd = "";
            defaultOutputDir = "";
            tempDirMP4 = "";
            addTimePosition = true;
            alwaysbackupfiles = true;
            strMainFileFormat = "";
            strMainAudioFormat = "";
            strLastSourcePath = "";
            strLastDestinationPath = "";
            minComplexity = 72;
            maxComplexity = 78;
            mainFormLocation = new Point(0, 0);
            mainFormSize = new Size(713, 478);
            updateFormLocation = new Point(0, 0);
            updateFormSize = new Size(730, 313);
            updateFormSplitter = 180;
            updateFormUpdateColumnWidth = 47;
            updateFormNameColumnWidth = 105;
            updateFormLocalVersionColumnWidth = 117; 
            updateFormServerVersionColumnWidth = 117;
            updateFormLocalDateColumnWidth = 70;
            updateFormServerDateColumnWidth = 70;
            updateFormLastUsedColumnWidth = 70;
            updateFormStatusColumnWidth = 111;
            jobColumnWidth = 40;
            inputColumnWidth = 89;
            outputColumnWidth = 89;
            codecColumnWidth = 79;
            ModeColumnWidth = 51;
            statusColumnWidth = 65;
            ownerColumnWidth = 60;
            startColumnWidth = 58;
            endColumnWidth = 58;
            fpsColumnWidth = 95;
            bEnsureCorrectPlaybackSpeed = bAlwayUsePortableAviSynth = false;
            ffmsThreads = 1;
            appendToForcedStreams = "";
            ocGUIMode = OCGUIMode.Default;
            bUseITU = true;
            bOpenAVSInThread = true;
            lastUsedOneClickFolder = "";
            bUseNeroAacEnc = bUseQAAC = bUseX265 = bUseDGIndexNV = false;
            chapterCreatorMinimumLength = 900;
            bEac3toLastUsedFileMode = false;
            bExternalMuxerX264 = true;
        }

        #region properties

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

        public int UpdateFormLastUsedColumnWidth
        {
            get { return updateFormLastUsedColumnWidth; }
            set { updateFormLastUsedColumnWidth = value; }
        }

        public int UpdateFormStatusColumnWidth
        {
            get { return updateFormStatusColumnWidth; }
            set { updateFormStatusColumnWidth = value; }
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

        public bool Eac3toLastUsedFileMode
        {
            get { return bEac3toLastUsedFileMode; }
            set { bEac3toLastUsedFileMode = value; }
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
        /// Last update check
        /// </summary>
        public DateTime LastUpdateCheck
        {
            get { return lastUpdateCheck; }
            set { lastUpdateCheck = value; }
        }

        /// <summary>
        /// Last update server
        /// </summary>
        public string LastUpdateServer
        {
            get { return lastUpdateServer; }
            set { lastUpdateServer = value; }
        }

        /// <summary>
        /// List of servers to use for autoupdate
        /// </summary>
        public string[][] AutoUpdateServerLists
        {
            get
            {
#if x86 && !DEBUG
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
#endif
#if x64 && !DEBUG
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
        /// bool to decide whether to use external muxer for the x264 encoder
        /// </summary>
        public bool UseExternalMuxerX264
        {
            get { return bExternalMuxerX264; }
            set { bExternalMuxerX264 = value; }
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
                if (String.IsNullOrEmpty(tempDirMP4) || Path.GetPathRoot(tempDirMP4).Equals(tempDirMP4, StringComparison.CurrentCultureIgnoreCase))
                    return String.Empty;
                return tempDirMP4;
            }
            set { tempDirMP4 = value; }
        }

        ///<summary>
        /// gets / sets whether megui backup files from updater or not
        /// </summary>
        public bool AlwaysBackUpFiles
        {
            get { return alwaysbackupfiles; }
            set { alwaysbackupfiles = value; }
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
                    Microsoft.Win32.RegistryKey key = null;
#if x86
                    key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\HaaliMkx");
                    if (key == null)
#endif
                        key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\HaaliMkx");

                    if (key == null)
                        return String.Empty;

                    string value = (string)key.GetValue("Install_Dir");
                    if (value == null)
                        return String.Empty;

                    if (!Directory.Exists(value))
                        return String.Empty;

                    return value;
                }
                catch
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// folder containing the avisynth plugins
        /// </summary>
        public string AvisynthPluginsPath
        {
            get 
            {
                UpdateCacher.CheckPackage("avisynth_plugin");
                return Path.GetDirectoryName(avisynthplugins.Path); 
            }
        }

        /// <summary>
        /// folder containing local copies of update files
        /// </summary>
        public string MeGUIUpdateCache
        {
            get { return meguiupdatecache; }
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

        /// <summary>
        /// gets / sets the settings for the update mode
        /// </summary>
        public UpdateMode UpdateMode
        {
            get
            {
                if (autoUpdate)
                    return MeGUI.UpdateMode.Manual;
                else
                    return MeGUI.UpdateMode.Disabled;
            }
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
        public ProxyMode HttpProxyMode
        {
            get { return httpProxyMode; }
            set { httpProxyMode = value; }
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

        public int ChapterCreatorMinimumLength
        {
            get { return chapterCreatorMinimumLength; }
            set { chapterCreatorMinimumLength = value; }
        }

        /// <summary>
        /// always use portable avisynth
        /// </summary>
        public bool AlwaysUsePortableAviSynth
        {
            get { return bAlwayUsePortableAviSynth; }
            set { bAlwayUsePortableAviSynth = value; }
        }

        /// <summary>
        /// filename and full path of the neroaacenc executable
        /// </summary>
        public string NeroAacEncPath
        {
            get 
            {
                if (!File.Exists(neroAacEncPath))
                    neroAacEncPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\eac3to\neroAacEnc.exe");
                return neroAacEncPath; 
            }
            set
            {
                if (!File.Exists(value))
                    neroAacEncPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\eac3to\neroAacEnc.exe");
                else
                    neroAacEncPath = value;
            }
        }

        public bool UseDGIndexNV
        {
            get { return bUseDGIndexNV; }
            set { bUseDGIndexNV = value; }
        }

        public bool UseNeroAacEnc
        {
            get { return bUseNeroAacEnc; }
            set { bUseNeroAacEnc = value; }
        }

        public bool UseQAAC
        {
            get { return bUseQAAC; }
            set { bUseQAAC = value; }
        }

        public bool UseX265
        {
            get { return bUseX265; }
            set { bUseX265 = value; }
        }

        public ProgramSettings Aften
        {
            get { return aften; }
            set { aften = value; }
        }

        public ProgramSettings AviMuxGui
        {
            get { return avimuxgui; }
            set { avimuxgui = value; }
        }

        public ProgramSettings AviSynth
        {
            get { return avisynth; }
            set { avisynth = value; }
        }

        public ProgramSettings AviSynthPlugins
        {
            get { return avisynthplugins; }
            set { avisynthplugins = value; }
        }

        public ProgramSettings BeSplit
        {
            get { return besplit; }
            set { besplit = value; }
        }

        public ProgramSettings DGAVCIndex
        {
            get { return dgavcindex; }
            set { dgavcindex = value; }
        }

        public ProgramSettings DGIndex
        {
            get { return dgindex; }
            set { dgindex = value; }
        }

        public ProgramSettings DGIndexNV
        {
            get { return dgindexnv; }
            set { dgindexnv = value; }
        }

        public ProgramSettings Eac3to
        {
            get { return eac3to; }
            set { eac3to = value; }
        }

        /// <summary>
        /// program settings of ffmpeg
        /// </summary>
        public ProgramSettings FFmpeg
        {
            get { return ffmpeg; }
            set { ffmpeg = value; }
        }

        public ProgramSettings FFMS
        {
            get { return ffms; }
            set { ffms = value; }
        }

        public ProgramSettings Flac
        {
            get { return flac; }
            set { flac = value; }
        }

        public ProgramSettings Lame
        {
            get { return lame; }
            set { lame = value; }
        }

        public ProgramSettings LSMASH
        {
            get { return lsmash; }
            set { lsmash = value; }
        }

        /// <summary>
        /// program settings of mkvmerge
        /// </summary>
        public ProgramSettings MkvMerge
        {
            get { return mkvmerge; }
            set { mkvmerge = value; }
        }

        public ProgramSettings Mp4Box
        {
            get { return mp4box; }
            set { mp4box = value; }
        }

        public ProgramSettings NeroAacEnc
        {
            get { return neroaacenc; }
            set { neroaacenc = value; }
        }

        public ProgramSettings OggEnc
        {
            get { return oggenc; }
            set { oggenc = value; }
        }

        public ProgramSettings Opus
        {
            get { return opus; }
            set { opus = value; }
        }

        public ProgramSettings PgcDemux
        {
            get { return pgcdemux; }
            set { pgcdemux = value; }
        }

        public ProgramSettings QAAC
        {
            get { return qaac; }
            set { qaac = value; }
        }

        public ProgramSettings TSMuxer
        {
            get { return tsmuxer; }
            set { tsmuxer = value; }
        }

        public ProgramSettings VobSub
        {
            get { return vobsub; }
            set { vobsub = value; }
        }

        /// <summary>
        /// program settings of x264 8bit
        /// </summary>
        public ProgramSettings X264
        {
            get { return x264; }
            set { x264 = value; }
        }

        /// <summary>
        /// program settings of x264 10bit
        /// </summary>
        public ProgramSettings X264_10B
        {
            get { return x264_10b; }
            set { x264_10b = value; }
        }

        public ProgramSettings X265
        {
            get { return x265; }
            set { x265 = value; }
        }

        public ProgramSettings XviD
        {
            get { return xvid; }
            set { xvid = value; }
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

        private bool bPortableAviSynth;
        /// <summary>
        /// portable avisynth in use
        /// </summary>
        [XmlIgnore()]
        public bool PortableAviSynth
        {
            get { return bPortableAviSynth; }
            set { bPortableAviSynth = value; }
        }

        private bool bAviSynthPlus;
        /// <summary>
        /// avisynth+ in use
        /// </summary>
        [XmlIgnore()]
        public bool AviSynthPlus
        {
            get { return bAviSynthPlus; }
            set { bAviSynthPlus = value; }
        }

        #region Methods

        public bool IsDGIIndexerAvailable()
        {
            if (!bUseDGIndexNV)
                return false;

            // check if the license file is available
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(dgindexnv.Path), "license.txt")))
                return false;

            // DGI is not available in a RDP connection
            if (SystemInformation.TerminalServerSession == true)
                return false;

            return true;
        }

        public void InitializeProgramSettings()
        {
            // initialize program settings if required
            if (aften == null)
                aften = new ProgramSettings("aften");
            if (avimuxgui == null)
                avimuxgui = new ProgramSettings("avimux_gui");
            if (avisynth == null)
                avisynth = new ProgramSettings("avs");
            if (avisynthplugins == null)
                avisynthplugins = new ProgramSettings("avisynth_plugin");
            if (besplit == null)
                besplit = new ProgramSettings("besplit");
            if (dgavcindex == null)
                dgavcindex = new ProgramSettings("dgavcindex");
            if (dgindex == null)
                dgindex = new ProgramSettings("dgindex");
            if (dgindexnv == null)
                dgindexnv = new ProgramSettings("dgindexnv");
            if (eac3to == null)
                eac3to = new ProgramSettings("eac3to");
            if (ffmpeg == null)
                ffmpeg = new ProgramSettings("ffmpeg");
            if (ffms == null)
                ffms = new ProgramSettings("ffms");
            if (flac == null)
                flac = new ProgramSettings("flac");
            if (lame == null)
                lame = new ProgramSettings("lame");
            if (lsmash == null)
                lsmash = new ProgramSettings("lsmash");
            if (mkvmerge == null)
                mkvmerge = new ProgramSettings("mkvmerge");
            if (mp4box == null)
                mp4box = new ProgramSettings("mp4box");
            if (neroaacenc == null)
                neroaacenc = new ProgramSettings("neroaacenc");
            if (oggenc == null)
                oggenc = new ProgramSettings("oggenc2");
            if (opus == null)
                opus = new ProgramSettings("opus");
            if (pgcdemux == null)
                pgcdemux = new ProgramSettings("pgcdemux");
            if (qaac == null)
                qaac = new ProgramSettings("qaac");
            if (tsmuxer == null)
                tsmuxer = new ProgramSettings("tsmuxer");
            if (vobsub == null)
                vobsub = new ProgramSettings("vobsub");
            if (x264 == null)
                x264 = new ProgramSettings("x264");
            if (x264_10b == null)
                x264_10b = new ProgramSettings("x264_10b");
            if (x265 == null)
                x265 = new ProgramSettings("x265");
            if (xvid == null)
                xvid = new ProgramSettings("xvid_encraw");

            // set default name, program paths & files
            aften.UpdateInformation("aften", "Aften", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\aften\aften.exe"));
            avimuxgui.UpdateInformation("avimux_gui", "AVI-Mux GUI", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avimux_gui\avimux_gui.exe"));
            avisynth.UpdateInformation("avs", "AviSynth portable", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avs\AviSynth.dll"));
            avisynth.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avs\DevIL.dll"));
            avisynth.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avs\plugins\DirectShowSource.dll"));
            avisynth.Required = true;
            avisynthplugins.UpdateInformation("avisynth_plugin", "AviSynth plugins", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bass_aac.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\bassaudio.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\colormatrix.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\convolution3dyv12.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\decomb.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\eedi2.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\fluxsmooth.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\leakkerneldeint.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\nicaudio.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\tdeint.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\tivtc.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\tomsmocomp.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\undot.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\vsfilter.dll"));
            avisynthplugins.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\avisynth_plugin\yadif.dll"));
            avisynthplugins.Required = true;
            besplit.UpdateInformation("besplit", "Besplit", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\besplit\besplit.exe"));
            dgavcindex.UpdateInformation("dgavcindex", "DGAVCIndex", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgavcindex\dgavcindex.exe"));
            dgavcindex.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgavcindex\dgavcdecode.dll"));
            dgindex.UpdateInformation("dgindex", "DGIndex", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindex\dgindex.exe"));
            dgindex.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindex\dgdecode.dll"));
            dgindexnv.UpdateInformation("dgindexnv", "DGIndexNV", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindexnv\dgindexnv.exe"));
            dgindexnv.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\dgindexnv\dgdecodenv.dll"));
            eac3to.UpdateInformation("eac3to", "eac3to", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\eac3to\eac3to.exe"));
            ffmpeg.UpdateInformation("ffmpeg", "FFmpeg", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\ffmpeg\ffmpeg.exe"));
            ffms.UpdateInformation("ffms", "FFMS", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\ffms\ffmsindex.exe"));
            ffms.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\ffms\ffms2.dll"));
            flac.UpdateInformation("flac", "FLAC", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\flac\flac.exe"));
            lame.UpdateInformation("lame", "LAME", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\lame\lame.exe"));
            lsmash.UpdateInformation("lsmash", "L-SMASH Works", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\lsmash\LSMASHSource.dll"));
            mkvmerge.UpdateInformation("mkvmerge", "mkvmerge", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\mkvmerge\mkvmerge.exe"));
            mkvmerge.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\mkvmerge\mkvextract.exe"));
            mp4box.UpdateInformation("mp4box", "MP4Box", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\mp4box\mp4box.exe"));
            neroaacenc.UpdateInformation("neroaacenc", "NeroAACEnc", NeroAacEncPath);
            oggenc.UpdateInformation("oggenc2", "OggEnc2", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\oggenc2\oggenc2.exe"));
            opus.UpdateInformation("opus", "Opus", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\opus\opusenc.exe"));
            pgcdemux.UpdateInformation("pgcdemux", "PgcDemux", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\pgcdemux\pgcdemux.exe"));
            qaac.UpdateInformation("qaac", "QAAC", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\qaac\qaac.exe"));
            tsmuxer.UpdateInformation("tsmuxer", "tsMuxeR", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\tsmuxer\tsmuxer.exe"));
            vobsub.UpdateInformation("vobsub", "VobSub", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\vobsub\vobsub.dll"));
#if x64
            x264.UpdateInformation("x264", "x264", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x264\x264_64.exe"));
            x264_10b.UpdateInformation("x264_10b", "x264 10bit", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x264_10b\x264-10b_64.exe"));
#endif
#if x86
            x264.UpdateInformation("x264", "x264", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x264\x264.exe"));
            x264_10b.UpdateInformation("x264_10b", "x264 10bit", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x264_10b\x264-10b.exe"));
            if (OSInfo.isWow64())
            {
                x264.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x264\avs4x264mod.exe"));
                x264.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x264\x264_64.exe"));
                x264_10b.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x264_10b\avs4x264mod.exe"));
                x264_10b.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x264_10b\x264-10b_64.exe"));
            }
#endif
            x265.UpdateInformation("x265", "x265", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x265\x265.exe"));
            x265.Files.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\x265\avs4x265.exe"));
            xvid.UpdateInformation("xvid_encraw", "Xvid", Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\xvid_encraw\xvid_encraw.exe"));
       }
        #endregion
    }

    public enum AfterEncoding { DoNothing = 0, Shutdown = 1, RunCommand = 2, CloseMeGUI = 3 }
    public enum ProxyMode { None = 0, SystemProxy = 1, CustomProxy = 2, CustomProxyWithLogin = 3 }
    public enum UpdateMode
    {
        [EnumTitle("Automatic update")]
        Automatic = 0,
        [EnumTitle("Automatic update check only")]
        Manual = 1,
        [EnumTitle("Automatic update completly disabled")]
        Disabled = 2
    }
}