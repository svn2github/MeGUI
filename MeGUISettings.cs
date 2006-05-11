using System;

namespace MeGUI
{
	/// <summary>
	/// Summary description for MeGUISettings.
	/// </summary>
	public class MeGUISettings
	{
        private string faacPath, lamePath, nerorawPath, mencoderPath, besweetPath, mp4boxPath, mkvmergePath,
            x264Path, dgIndexPath, xvidEncrawPath, avc2aviPath, aviMuxGUIPath,
            audioProfileName, videoProfileName, defaultLanguage1, defaultLanguage2, divxMuxerPath;
        private bool recalculateMainMovieBitrate, autoForceFilm, autoStartQueue, enableMP3inMP4, shutdown, autoOpenScript,
            overwriteStats, keep2of3passOutput, deleteCompletedJobs, autoSetNbThreads, deleteIntermediateFiles,
            deleteAbortedOutput, openProgressWindow, useadvancedtooltips;
		private decimal forceFilmThreshold;
		private int nbPasses, acceptableAspectError;
        private string videoExtension, audioExtension, avsProfile, oneClickProfile;
        private bool safeProfileAlteration;
        private SourceDetectorSettings sdSettings;
        private DialogSettings dialogSettings;
        private ProcessPriority defaultPriority;
        private bool usingNero6;

        public bool UsingNero6
        {
            get { return usingNero6; }
            set { usingNero6 = value; }
        }

        public string DivXMuxPath
        {
            get { return divxMuxerPath; }
            set { divxMuxerPath = value; }
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

        public MeGUISettings()
		{
            dialogSettings = new DialogSettings();
            sdSettings = new SourceDetectorSettings();
            useadvancedtooltips = true;
            avc2aviPath = "avc2avi.exe";
            aviMuxGUIPath = "avimux_gui.exe";
            avsProfile = "Default Profile";
            oneClickProfile = "default";
            faacPath = "faac.exe";
            mencoderPath = "mencoder.exe";
			besweetPath = "besweet.exe";
			mp4boxPath = "mp4box.exe";
			mkvmergePath = "mkvmerge.exe";
			x264Path = "x264.exe";
            divxMuxerPath = "divxmux.exe";
			dgIndexPath = "dgindex.exe";
            xvidEncrawPath = "xvid_encraw.exe";
            lamePath = "lame.exe";
            nerorawPath = "neroraw.exe";
			audioProfileName = "";
			videoProfileName = "";
            usingNero6 = false;
            recalculateMainMovieBitrate = false;
			autoForceFilm = true;
			autoStartQueue = false;
			forceFilmThreshold = new decimal(95);
			defaultLanguage1 = "";
			defaultLanguage2 = "";
            defaultPriority = ProcessPriority.IDLE;
            acceptableAspectError = 5;
			shutdown = false;
			autoOpenScript = true;
			enableMP3inMP4 = false;
			overwriteStats = true;
			keep2of3passOutput = false;
			deleteCompletedJobs = false;
			autoSetNbThreads = true;
			nbPasses = 2;
			deleteIntermediateFiles = true;
			deleteAbortedOutput = true;
			openProgressWindow = true;
            videoExtension = "";
            audioExtension = "";
            safeProfileAlteration = false;
        }
        #region properties

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
        public string AvsProfileName
        {
            get { return avsProfile; }
            set { avsProfile = value; }
        }
        public string OneClickProfileName
        {
            get { return oneClickProfile; }
            set { oneClickProfile = value; }
        }
		/// <summary>
		/// filename and full path of the mencoder executable
		/// </summary>
		public string MencoderPath
		{
			get {return mencoderPath;}
			set {mencoderPath = value;}
		}
        /// <summary>
        /// filename and full path of the faac executable
        /// </summary>
        public string FaacPath
        {
            get { return faacPath; }
            set { faacPath = value; }
        }
        /// <summary>
        /// filename and full path of the faac executable
        /// </summary>
        public string LamePath
        {
            get { return lamePath; }
            set { lamePath = value; }
        }
        /// <summary>
        /// filename and full path of the faac executable
        /// </summary>
        public string NerorawPath
        {
            get { return nerorawPath; }
            set { nerorawPath = value; }
        }		
	    
	    /// <summary>
		/// filename and full path of the mkvemerge executable
		/// </summary>
		public string MkvmergePath
		{
			get {return mkvmergePath;}
			set {mkvmergePath = value;}
		}
		/// <summary>
		/// filename and full path of the besweet executable
		/// </summary>
		public string BesweetPath
		{
			get {return besweetPath;}
			set {besweetPath = value;}
		}
		/// <summary>
		/// filename and full path of the mp4creator executable
		/// </summary>
		public string Mp4boxPath
		{
			get {return mp4boxPath;}
			set {mp4boxPath = value;}
		}
		/// <summary>
		/// filename and full path of the x264 executable
		/// </summary>
		public string X264Path
		{
			get {return x264Path;}
			set {x264Path = value;}
		}
		/// <summary>
		/// filename and full path of the dgindex executable
		/// </summary>
		public string DgIndexPath
		{
			get {return dgIndexPath;}
			set {dgIndexPath = value;}
		}
        /// <summary>
        /// filename and full path of the xvid_encraw executable
        /// </summary>
        public string XviDEncrawPath
        {
            get { return xvidEncrawPath; }
            set { xvidEncrawPath = value; }
        }
        /// <summary>
        /// gets / sets the path of the avc2avi executable
        /// </summary>
        public string Avc2aviPath
        {
            get { return avc2aviPath; }
            set { avc2aviPath = value; }
        }
        /// <summary>
        /// gets / sets the path of the avimuxgui executable
        /// </summary>
        public string AviMuxGUIPath
        {
            get { return aviMuxGUIPath; }
            set { aviMuxGUIPath = value; }
        }
        /// <summary>
        /// folder containing the avisynth plugins
        /// </summary>
        public static string AvisynthPluginsPath
        {
            get 
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\AviSynth");
                if(key==null)
                    return null;
                else
                    return (string)key.GetValue("plugindir2_5");
            }
            set 
            {
                if(!System.IO.Path.IsPathRooted(value))
                    throw new ArgumentException("Path must be absolute");
                if(!System.IO.Directory.Exists(value))
                    throw new ArgumentException("Directory " + value + " does not exists");
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\AviSynth", true);
                key.SetValue("plugindir2_5", value);
            }
        }
		/// <summary>
		/// name of the currently selected audio profile
		/// </summary>
		public string AudioProfileName
		{
			get {return audioProfileName;}
			set {audioProfileName = value;}
		}
		/// <summary>
		/// name of the currently selected video profile
		/// </summary>
		public string VideoProfileName
		{
			get {return videoProfileName;}
			set {videoProfileName = value;}
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
		/// gets / sets whether pressing Queue should automatically start encoding
		/// </summary>
		public bool AutoStartQueue
		{
			get {return autoStartQueue;}
			set {autoStartQueue = value;}
		}
		/// <summary>
		/// gets / sets whether megui shuts down the pc after encoding has completed
		/// </summary>
		public bool Shutdown
		{
			get {return shutdown;}
			set {shutdown = value;}
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
		/// gets / sets whether megui automatically detects multicore / HT capable CPUs and sets the number of 
		/// encoder threads accordingly
		/// </summary>
		public bool AutoSetNbThreads
		{
			get {return autoSetNbThreads;}
			set {autoSetNbThreads = value;}
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
		/// gets / sets whether the configuration dialog should create a new profile on alteration of a stock one
		/// </summary>
        public bool SafeProfileAlteration
        {
            get {return safeProfileAlteration;}
            set {safeProfileAlteration = value;}
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
        #endregion
    }
}