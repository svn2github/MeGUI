using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using MeGUI.core.util;

namespace MeGUI
{
    /// <summary>
    /// offers raw avc to avi muxing based on avc2avi
    /// </summary>
    class Avc2AviMuxer : CommandlineMuxer
    {
        public Avc2AviMuxer(string executablePath)
        {
            this.executable = executablePath;
        }

        public override void ProcessLine(string line, StreamType stream)
        {
            log.AppendLine(line);
        }

        protected override bool checkExitCode()
        {
            return true;
        }
    }
}
