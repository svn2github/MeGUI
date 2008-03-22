using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class OggVorbisSettings: AudioCodecSettings
	{
        public static readonly string ID = "Vorbis";

        public OggVorbisSettings()
            : base(ID, AudioCodec.VORBIS, AudioEncoderType.VORBIS, 0)
		{
            this.Quality = 1.0M;
		}

        public decimal Quality;

        public override BitrateManagementMode BitrateMode
        {
            get
            {
                return BitrateManagementMode.VBR;
            }
            set
            {
                // Do Nothing
            }
        }
	}
}
