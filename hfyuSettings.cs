using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class hfyuSettings : VideoCodecSettings
    {
        private static readonly string[] m_fourCCs = { "FFVH" };

        public hfyuSettings()
            : base()
        {
            base.BitrateQuantizer = 0;
            base.EncodingMode = (int)Mode.CQ;
            base.FourCC = 0;
            FourCCs = m_fourCCs;
        }

        public override bool IsAltered(VideoCodecSettings settings)
        {
            return true;
        }

    }
}
