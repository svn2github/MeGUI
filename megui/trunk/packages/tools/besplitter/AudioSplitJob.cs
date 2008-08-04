using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using MeGUI.core.util;

namespace MeGUI.packages.tools.besplitter
{
    public class AudioSplitJob : Job
    {
        private Cuts c;

        public string generateSplitCommandline()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("-core( -input \"{0}\" -prefix \"{1}\" -type {2} -a )", Input,
                Output, Path.GetExtension(Input).Substring(1));
            sb.Append(" -split( ");
            foreach (CutSection s in c.AllCuts)
            {
                double start = ((double)s.startFrame) / c.Framerate;
                double end = ((double)s.endFrame) / c.Framerate;
                sb.AppendFormat("{0} {1} ", start, end);
            }
            sb.Append(")");

            return sb.ToString();
        }

        public AudioSplitJob() { }

        public AudioSplitJob(string input, string output, Cuts c)
        {
            this.c = c;
            this.Input = input;
            this.Output = output;
        }

        public Cuts TheCuts
        {
            get { return c; }
            set { c = value; }
        }

        public override string CodecString
        {
            get { return "cut"; }
        }

        public override string EncodingMode
        {
            get { return "split"; }
        }

    }
}
