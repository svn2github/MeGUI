using System;

namespace MeGUI
{
	/// <summary>
	/// Summary description for OneClickDefaults.
	/// </summary>
	public class OneClickSettings
	{
		public string VideoProfileName, AudioProfileName, StorageMediumName, ContainerFormatName, AvsProfileName;
		public bool DontEncodeAudio, SignalAR, Split, AutomaticDeinterlacing;
		public long OutputResolution, Filesize, SplitSize;

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