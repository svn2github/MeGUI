using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using MeGUI.core.util;
using System.Globalization;

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
