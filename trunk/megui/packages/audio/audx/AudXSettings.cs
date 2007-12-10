using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class AudXSettings: AudioCodecSettings
	{
        public static readonly string ID = "Aud-X MP3";
        public enum QualityMode
        {
            //0 (STRQ 80 kbps), 1 (STDQ 128 kbps), 2 (HGHQ 192 kbps) or 3 (SPBQ 192 kbps), default is 1
            [EnumTitle("STRQ 80 kbps", 80)]
            STRQ = 0,
            [EnumTitle("STDQ 128 kbps", 128)]
            STDQ = 1,
            [EnumTitle("HGHQ 192 kbps", 192)]
            HGHQ = 2,
            [EnumTitle("SPBQ 192 kbps", 192)]
            SPBQ = 3
        }

        public QualityMode Quality;

        public AudXSettings()
            : base(ID)
		{
            this.Quality = QualityMode.STDQ;
            this.Codec = AudioCodec.MP3;
            this.EncoderType = AudioEncoderType.AUDX;
		}


        public override int Bitrate
        {
            get
            {
                return (int)EnumProxy.Create(this.Quality).Tag ;
            }
            set
            {
                // Do Nothing
            }
        }

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
