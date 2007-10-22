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
        public virtual void FixFileNames(Dictionary<string, string> _) { }
        public override bool Equals(object obj)
        {
            return PropertyEqualityTester.AreEqual(this, obj);
        }

        public string getSettingsType()
        {
            return "OneClick";
        }
        public string VideoProfileName, AudioProfileName, AvsProfileName;
		public bool PrerenderVideo, DontEncodeAudio, SignalAR, AutomaticDeinterlacing;
		public long OutputResolution;
        public FileSize? Filesize;
        public FileSize? SplitSize;
        public string[] ContainerCandidates;

        public GenericSettings baseClone()
        {
            return clone();
        }
        
        public OneClickSettings clone()
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
