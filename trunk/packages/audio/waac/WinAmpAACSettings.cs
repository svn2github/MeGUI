using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class WinAmpAACSettings: AudioCodecSettings
	{

        public enum AacStereoMode
        {
            Joint,
            Independent,
            Dual
        }

        public static readonly AacProfile[] SupportedProfiles = new AacProfile[] { AacProfile.PS, AacProfile.HE, AacProfile.LC };

        public WinAmpAACSettings()
            : base()
		{
            this.Bitrate = 48;
            this.Profile = AacProfile.PS;
            this.StereoMode = AacStereoMode.Joint;
            this.Mpeg2AAC = false;

            this.Codec = AudioCodec.AAC;
            this.EncoderType = AudioEncoderType.WAAC;
		}

        public AacProfile Profile;
        public AacStereoMode StereoMode;
        public bool Mpeg2AAC;



        public override BitrateManagementMode BitrateMode
        {
            get
            {
                return BitrateManagementMode.CBR;
            }
            set
            {
                // Do Nothing
            }
        }


	}
}
