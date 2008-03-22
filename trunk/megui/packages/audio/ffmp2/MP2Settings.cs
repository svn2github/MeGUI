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
            : base(ID, AudioCodec.MP2, AudioEncoderType.FFMP2, 128)
        {

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
