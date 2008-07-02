using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class hfyuSettings : VideoCodecSettings
    {
        public static string ID = "huffyuv";

        private static readonly string[] m_fourCCs = { "FFVH" };

        public hfyuSettings()
            : base(ID, VideoEncoderType.HFYU)
        {
            base.BitrateQuantizer = 0;
            base.EncodingMode = (int)Mode.CQ;
            base.FourCC = 0;
            FourCCs = m_fourCCs;
        }

        public override bool UsesSAR
        {
            get { return false; }
        }
    }
}
