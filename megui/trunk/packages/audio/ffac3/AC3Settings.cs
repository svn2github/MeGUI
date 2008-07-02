using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class AC3Settings : AudioCodecSettings
    {
        public static string ID = "FFmpeg AC-3";

        public static readonly object[] SupportedBitrates = new object[] { 64, 128, 160, 192, 224, 256, 288, 320, 352, 384, 448, 512, 576, 640};

        public AC3Settings()
            : base(ID, AudioCodec.AC3, AudioEncoderType.FFAC3, 384)
        {
            base.supportedBitrates = Array.ConvertAll<object, int>(SupportedBitrates, delegate(object o) { return (int)o; });
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
