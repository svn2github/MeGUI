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

using MeGUI.core.util;

namespace MeGUI
{
	/// <summary>
	/// Summary description for MeGUISettings.
	/// </summary>
	[LogByMembers]
    public class MeGUISettings
    {
        #region variables
        private string[][] autoUpdateServerLists;
        private string faacPath, lamePath, neroAacEncPath, mencoderPath,  mp4boxPath, mkvmergePath, 
                       encAacPlusPath, ffmpegPath, besplitPath, yadifPath, aftenPath, x264Path, 
                       dgIndexPath, xvidEncrawPath, aviMuxGUIPath, oggEnc2Path, encAudXPath, dgavcIndexPath, dgvc1IndexPath,
                       dgmpgIndexPath, eac3toPath, tsmuxerPath,
                       defaultLanguage1, defaultLanguage2, afterEncodingCommand, videoExtension, audioExtension,
                       httpproxyaddress, httpproxyport, httpproxyuid, httpproxypwd, defaultOutputDir;
        private bool recalculateMainMovieBitrate, autoForceFilm, autoStartQueue, enableMP3inMP4, autoOpenScript,
                     overwriteStats, keep2of3passOutput, deleteCompletedJobs, deleteIntermediateFiles,
                     deleteAbortedOutput, openProgressWindow, useadvancedtooltips, freshOggEnc2, autoscroll, 
                     alwaysOnTop, safeProfileAlteration, autoUpdate, usehttpproxy, addTimePosition;
        private ulong audioSamplesPerUpdate;
        private AfterEncoding afterEncoding;
        private decimal forceFilmThreshold, acceptableFPSError;
		private int nbPasses, acceptableAspectError, maxServersToTry, autoUpdateServerSubList;
        private SourceDetectorSettings sdSettings;
        private AutoEncodeDefaultsSettings aedSettings;
        private DialogSettings dialogSettings;
        private ProcessPriority defaultPriority;
        #endregion
        public MeGUISettings()
		{
            autoscroll = true;
            autoUpdateServerLists = new string[][] { new string[] { "Stable", "http://megui.org/auto/stable/", "http://megui.xvidvideo.ru/auto/stable/" },
                new string[] { "Development", "http://megui.org/auto/", "http://megui.xvidvideo.ru/auto/" } };
            acceptableFPSError = 0.01M;
            autoUpdateServerSubList = 0;
            maxServersToTry = 5;
            dialogSettings = new DialogSettings();
            sdSettings = new SourceDetectorSettings();
            AedSettings = new AutoEncodeDefaultsSettings();
            autoUpdate = true;
            useadvancedtooltips = true;
            audioSamplesPerUpdate = 100000;
            aviMuxGUIPath = "avimux_gui.exe";
            besplitPath = "besplit.exe";
            faacPath = "faac.exe";
            mencoderPath = "mencoder.exe";
			mp4boxPath = "mp4box.exe";
			mkvmergePath = "mkvmerge.exe";
			x264Path = "x264.exe";
            dgIndexPath = "dgindex.exe";
            xvidEncrawPath = "xvid_encraw.exe";
            lamePath = "lame.exe";
            neroAacEncPath = "neroAacEnc.exe";
            oggEnc2Path = "oggenc2.exe";
            encAudXPath = "enc_AudX_CLI.exe";
            encAacPlusPath = "enc_aacPlus.exe";
            ffmpegPath = "ffmpeg.exe";
            aftenPath = "aften.exe";
            yadifPath = "yadif.dll";
            recalculateMainMovieBitrate = false;
			autoForceFilm = true;
			autoStartQueue = false;
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
			openProgressWindow = true;
            freshOggEnc2 = true;
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
            addTimePosition = false;
            dgavcIndexPath = "dgavcindex.exe";
            dgvc1IndexPath = "dgvc1index.exe";
            dgmpgIndexPath = "dgmpgindex.exe";
            eac3toPath = "eac3to.exe";
            tsmuxerPath = "tsmuxer.exe";
        }
        #region properties
        public string YadifPath
        {
            get { return yadifPath; }
            set { yadifPath = value; }
        }

        public ulong AudioSamplesPerUpdate
        {
            get { return audioSamplesPerUpdate; }
            set { audioSamplesPerUpdate = value; }
        }

        /// <summary>
        /// Gets / sets whether the log should be autoscrolled
        /// </summary>
        public bool AutoScrollLog
        {
            get { return autoscroll; }
            set { autoscroll = value; }
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
        /// Maximum servers that auto update should try before aborting.
        /// </summary>
        public int MaxServersToTry
        {
            get { return maxServersToTry; }
            set { maxServersToTry = value; }
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
            get { return autoUpdateServerLists; }
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
        /// path of besplit.exe
        /// </summary>
        public string BeSplitPath
        {
            get { return besplitPath; }
            set { besplitPath = value; }
        }

        /// <summary>
        /// filename and full path of the ffmpeg executable
        /// </summary>
        public string FFMpegPath
        {
            get { return ffmpegPath; }
            set { ffmpegPath = value; }
        }
        
        /// <summary>
        /// filename and full path of the enc_aacPlus executable
        /// </summary>
        public string EncAacPlusPath
        {
            get { return encAacPlusPath; }
            set { encAacPlusPath = value; }
        }

        /// <summary>
        /// filename and full path of the enc_AudX_CLI executable
        /// </summary>
        public string EncAudXPath
        {
            get { return encAudXPath; }
            set { encAudXPath = value; }
        }

        /// <summary>
        /// filename and full path of the oggenc2 executable
        /// </summary>
        public string OggEnc2Path
        {
            get { return oggEnc2Path; }
            set { oggEnc2Path = value; }
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
        public string NeroAacEncPath
        {
            get { return neroAacEncPath; }
            set { neroAacEncPath = value; }
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
        /// gets / sets the path of the avimuxgui executable
        /// </summary>
        public string AviMuxGUIPath
        {
            get { return aviMuxGUIPath; }
            set { aviMuxGUIPath = value; }
        }
        /// <summary>
        /// filename and full path of the aften executable
        /// </summary>
        public string AftenPath
        {
            get { return aftenPath; }
            set { aftenPath = value; }
        }	
        /// <summary>
        /// filename and full path of the dgavcindex executable
        /// </summary>
        public string DgavcIndexPath
        {
            get { return dgavcIndexPath; }
            set { dgavcIndexPath = value; }
        }
        /// <summary>
        /// filename and full path of the dgvc1index executable
        /// </summary>
        public string Dgvc1IndexPath
        {
            get { return dgvc1IndexPath; }
            set { dgvc1IndexPath = value; }
        }
        /// <summary>
        /// filename and full path of the dgmpgindex executable
        /// </summary>
        public string DgmpgIndexPath
        {
            get { return dgmpgIndexPath; }
            set { dgmpgIndexPath = value; }
        }
        /// <summary>
        /// filename and full path of the eac3to executable
        /// </summary>
        public string EAC3toPath
        {
            get { return eac3toPath; }
            set { eac3toPath = value; }
        }
         /// <summary>
        /// filename and full path of the tsmuxer executable
        /// </summary>
        public string TSMuxerPath
        {
            get { return tsmuxerPath; }
            set { tsmuxerPath = value; }
        }
        /// <summary>
        /// folder containing the avisynth plugins
        /// </summary>
        public static string AvisynthPluginsPath
        {
            get
            {
                try
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\AviSynth");
                    if (key == null)
                        return null;
                    else
                        return (string)key.GetValue("plugindir2_5");
                }
                catch
                {
                    return null;
                }
            }
            set 
            {
                if(!System.IO.Path.IsPathRooted(value))
                    throw new ArgumentException("Path must be absolute");
                if(!System.IO.Directory.Exists(value))
                    throw new ArgumentException("Directory " + value + " does not exists");
                try
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\AviSynth", true);
                    key.SetValue("plugindir2_5", value);
                }
                catch
                {
                    // Swallow the error
                }
            }
        }

        /// <summary>
        /// folder containing local copies of update files
        /// </summary>
        public static string MeGUIUpdateCache
        {
            get
            {
                try
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MeGUI");
                    if (key == null)
                        return null;
                    else
                        return (string)key.GetValue("update_cache");
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (!System.IO.Path.IsPathRooted(value))
                    throw new ArgumentException("Path must be absolute");
                try
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MeGUI");
                    key.SetValue("update_cache", value);
                }
                catch
                {
                    // Swallow the error
                }
            }
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
        /// true if oggenc2 is v2.8 or later
        /// </summary>
        public bool FreshOggEnc2
        {
            get { return freshOggEnc2; }
            set { freshOggEnc2 = value; }
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
        #endregion
    }
    public enum AfterEncoding { DoNothing = 0, Shutdown = 1, RunCommand = 2 }
}
