// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms; // used for the MethodInvoker

using MeGUI.core.util;

namespace MeGUI
{
    class DivXAVCEncoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "divxAVCEncoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob &&
                (j as VideoJob).Settings is DivXAVCSettings)
                return new DivXAVCEncoder(mf.Settings.DivXAVCPath);
            return null;
        }

        public DivXAVCEncoder(string encoderPath)
            : base()
        {
            executable = encoderPath;
        }
        
       public override string GetFrameString(string line, StreamType stream)
        {
            if (line.StartsWith("[")) // we found a position line, parse it
            {
                int frameNumberStart = line.LastIndexOf(":") + 1;
                int frameNumberEnd = line.IndexOf("/");
                return line.Substring(frameNumberStart, frameNumberEnd - frameNumberStart).Trim();
            }
            return null;
        }

        public override string GetErrorString(string line, StreamType stream)
        {
            if (line.IndexOf("Error") != -1) // we get the usage message if there's an unrecognized parameter
                return line;
            return null;
        }

        protected override string Commandline
        {
            get
            {
                return genCommandline(job.Input, job.Output, job.DAR, job.Settings as DivXAVCSettings, hres, vres, job.Zones);
            }
        }

        public static string genCommandline(string input, string output, Dar? d, DivXAVCSettings xs, int hres, int vres, Zone[] zones)
        {
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");
            sb.Append(" -i \"" + input + "\"");
            sb.Append(" -o \"" + output + "\"");
            
            switch (xs.EncodingMode)
            {
                case 0: // 2pass - 1st pass
                    sb.Append(" -npass 1 -sf " + "\"" + xs.Logfile + "\" -br " + xs.BitrateQuantizer); // add logfile
                    break;
                case 1:  // 2pass - 2nd pass
                case 2:  // Automated 2 pass
                    sb.Append(" -npass 2 -sf " + "\"" + xs.Logfile + "\" -br " + xs.BitrateQuantizer); // add logfile
                    break;
            }

            double framerate = 0.0;
            using (AvsFile avi = AvsFile.ParseScript(Path.GetDirectoryName(input)))
            {
                framerate = avi.Info.FPS;
            }
            sb.Append(" -fps " + framerate.ToString(ci));

            if (xs.Turbo)
            {
                xs.AQO = 0;
                xs.Pyramid = false;
                xs.BasRef = false;
                xs.MaxRefFrames = 1;
                xs.MaxBFrames = 0;
            }

            if (xs.InterlaceMode != 0)
                sb.Append(" -fmode " + xs.InterlaceMode);
            if (xs.AQO != 1)
                sb.Append(" -aqo " + xs.AQO);
            if (xs.GOPLength != 4)
                sb.Append(" -I " + xs.GOPLength);
            if (xs.MaxBFrames != 2)
                sb.Append(" -bf " + xs.MaxBFrames);
            if (xs.MaxRefFrames != 4)
                sb.Append(" -ref " + xs.MaxRefFrames);
            if (xs.BasRef)
                sb.Append(" -bref");
            if (xs.Pyramid)
                sb.Append(" -pyramid");

            sb.Append(" -threads " + xs.NbThreads);

            return sb.ToString();
        }
    }
}
