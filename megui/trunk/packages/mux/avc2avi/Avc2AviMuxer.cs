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
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI
{
    /// <summary>
    /// offers raw avc to avi muxing based on avc2avi
    /// </summary>
    class Avc2AviMuxer : CommandlineMuxer
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "Avc2AviMuxer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is MuxJob && (j as MuxJob).MuxType == MuxerType.AVC2AVI)
                return new Avc2AviMuxer(mf.Settings.Avc2aviPath);
            return null;
        }

        public Avc2AviMuxer(string executablePath)
        {
            this.executable = executablePath;
        }

        protected override string Commandline
        {
            get
            {
                if (!job.Settings.Framerate.HasValue)
                    throw new JobRunException("Can't generate commandline because video framerate isn't known");

                return "-f " + job.Settings.Framerate.Value.ToString(new CultureInfo("en-us")) + 
                    " -i \"" + job.Settings.VideoInput + 
                    "\" -o \"" + job.Settings.MuxedOutput + "\"";
            }
        }
    }
}
