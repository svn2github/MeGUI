using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class AviSynthJob : Job
    {
        private ulong nbFrames;
        private double framerate;

        public double Framerate
        {
            get { return framerate; }
            set { framerate = value; }
        }

        public ulong NumberOfFrames
        {
            get { return nbFrames; }
            set { nbFrames = value; }
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
    }
}
