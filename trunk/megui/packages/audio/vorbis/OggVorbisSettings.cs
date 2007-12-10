using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class OggVorbisSettings: AudioCodecSettings
	{
        public static readonly string ID = "Vorbis";

        public OggVorbisSettings()
            : base(ID)
		{
            this.Quality = 1.0M;
            this.Codec = AudioCodec.VORBIS;
            this.EncoderType = AudioEncoderType.VORBIS;
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
