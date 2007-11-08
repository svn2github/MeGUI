using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class AviSynthJob : Job
    {
        public AviSynthJob() : base() { }

        public AviSynthJob(string input) : base(input, null)
        {
        }

        public override string CodecString
        {
            get { return ""; }
        }

        public override string EncodingMode
        {
            get { return "avs"; }
        }
    }
}
