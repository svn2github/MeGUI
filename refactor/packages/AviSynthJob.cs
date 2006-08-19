using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class AviSynthJob : Job
    {
        private int nbFrames;
        private double framerate;

        public double Framerate
        {
            get { return framerate; }
            set { framerate = value; }
        }

        public int NumberOfFrames
        {
            get { return nbFrames; }
            set { nbFrames = value; }
        }

        public override JobTypes JobType
        {
            get { return JobTypes.AVS; }
        }

        public AviSynthJob() : base()
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

        public override string FPSString
        {
            get { return ""; }
        }
    }
}
