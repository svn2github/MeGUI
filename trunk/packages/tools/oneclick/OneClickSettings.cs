using System;
using MeGUI.core.plugins.interfaces;
using System.Collections.Generic;
namespace MeGUI
{
	/// <summary>
	/// Summary description for OneClickDefaults.
	/// </summary>
    public class OneClickSettings : GenericSettings
	{
        public override bool Equals(object obj)
        {
            return PropertyEqualityTester.Equals(this, obj);
        }

        public string getSettingsType()
        {
            return "OneClick";
        }
        public string VideoProfileName, StorageMediumName, AudioProfileName, AvsProfileName;
		public bool PrerenderVideo, DontEncodeAudio, SignalAR, Split, AutomaticDeinterlacing;
		public long OutputResolution, Filesize, SplitSize;
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
			StorageMediumName = "";
            AvsProfileName = "";
            AutomaticDeinterlacing = true;
            PrerenderVideo = false;
			DontEncodeAudio = false;
			SignalAR = false;
			Split = false;
			OutputResolution = 640;
			Filesize = -1;
			SplitSize = -1;
            ContainerCandidates = new string[0];
		}
	}
}