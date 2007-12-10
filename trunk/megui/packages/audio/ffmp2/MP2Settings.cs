using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class MP2Settings : AudioCodecSettings
    {
        public static readonly string ID = "FFmpeg MP2";

        public static readonly object[] SupportedBitrates = new object[] { 64, 128, 160, 192, 224, 256, 288, 320 };


        public MP2Settings()
            : base(ID)
        {
            this.Codec = AudioCodec.MP2;
            this.EncoderType = AudioEncoderType.FFMP2;
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

        public override int Bitrate
        {
            get
            {
                return AC3Settings.NormalizeVar(base.Bitrate, SupportedBitrates);
            }
            set
            {
                base.Bitrate = value;
            }
        }
    }
}
