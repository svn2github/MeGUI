using System;
using MeGUI.core.plugins.interfaces;
namespace MeGUI
{
	/// <summary>
	/// Summary description for OneClickDefaults.
	/// </summary>
    public class OneClickSettings : GenericSettings
	{
        public string getSettingsType()
        {
            return "OneClick";
        }
        public string VideoProfileName, AudioProfileName, StorageMediumName, ContainerFormatName, AvsProfileName;
		public bool DontEncodeAudio, SignalAR, Split, AutomaticDeinterlacing;
		public long OutputResolution, Filesize, SplitSize;

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
			VideoProfileName = "default";
			AudioProfileName = "default";
			StorageMediumName = "default";
			ContainerFormatName = "default";
            AvsProfileName = "default";
            AutomaticDeinterlacing = true;
			DontEncodeAudio = false;
			SignalAR = false;
			Split = false;
			OutputResolution = 640;
			Filesize = -1;
			SplitSize = -1;
		}
	}
}