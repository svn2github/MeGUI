using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI.packages.tools.dvddecrypter
{
    public class DVDJob : Job
    {
        public DVDJob() { }

        public override string CodecString
        {
            get { return ""; }
        }

        public override string EncodingMode
        {
            get { return "dvd"; }
        }

        public DVDJob(string driveletter, string outputfolder, int vts, int pgc)
            :base(driveletter, outputfolder)
        {
            this.VTS = vts;
            this.PGC = pgc;
        }

        public int VTS;
        public int PGC;
    }
}
