using System;
using MeGUI.core.plugins.interfaces;
using System.Collections.Generic;
using MeGUI.core.util;
namespace MeGUI
{
	/// <summary>
	/// Summary description for OneClickDefaults.
	/// </summary>
    public class OneClickSettings : GenericSettings
	{
        public string SettingsID { get { return "OneClick"; } }

        public virtual void FixFileNames(Dictionary<string, string> _) { }
        public override bool Equals(object obj)
        {
            return Equals(obj as GenericSettings);
        }

        public bool Equals(GenericSettings other)
        {
            return other == null ? false : PropertyEqualityTester.AreEqual(this, other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private string videoProfileName;
        public string VideoProfileName
        {
            get { return videoProfileName; }
            set { videoProfileName = value; }
        }

        private string audioProfileName;
        public string AudioProfileName
        {
            get { return audioProfileName; }
            set { audioProfileName = value; }
        }

        private string avsProfileName;
        public string AvsProfileName
        {
            get { return avsProfileName; }
            set { avsProfileName = value; }
        }

        private bool prerenderVideo;
        public bool PrerenderVideo
        {
            get { return prerenderVideo; }
            set { prerenderVideo = value; }
        }

        private bool dontEncodeAudio;
        public bool DontEncodeAudio
        {
            get { return dontEncodeAudio; }
            set { dontEncodeAudio = value; }
        }

        private bool signalAR;
        public bool SignalAR
        {
            get { return signalAR; }
            set { signalAR = value; }
        }

        private bool automaticDeinterlacing;
        public bool AutomaticDeinterlacing
        {
            get { return automaticDeinterlacing; }
            set { automaticDeinterlacing = value; }
        }

        private long outputResolution;
        public long OutputResolution
        {
            get { return outputResolution; }
            set { outputResolution = value; }
        }

        private FileSize? filesize;
        public FileSize? Filesize
        {
            get { return filesize; }
            set { filesize = value; }
        }

        private FileSize? splitSize;
        public FileSize? SplitSize
        {
            get { return splitSize; }
            set { splitSize = value; }
        }

        private string[] containerCandidates;
        public string[] ContainerCandidates
        {
            get { return containerCandidates; }
            set { containerCandidates = value; }
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        GenericSettings GenericSettings.Clone()
        {
            return Clone();
        }
        
        public OneClickSettings Clone()
        {
            return this.MemberwiseClone() as OneClickSettings;
        }

		public OneClickSettings()
		{
			VideoProfileName = "";
			AudioProfileName = "";
            AvsProfileName = "";
            AutomaticDeinterlacing = true;
            PrerenderVideo = false;
			DontEncodeAudio = false;
			SignalAR = false;
			OutputResolution = 640;
            SplitSize = null;
            ContainerCandidates = new string[0];
		}

        #region GenericSettings Members


        public string[] RequiredFiles
        {
            get { return new string[0]; }
        }

        public string[] RequiredProfiles
        {
            get { return new string[]{VideoProfileName, AudioProfileName};}
        }

        #endregion
    }
}
