// ****************************************************************************
// 
// Copyright (C) 2005-2008  Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

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
