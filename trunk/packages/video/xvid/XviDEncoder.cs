using System;
using System.Collections.Generic;
using System.Windows.Forms; // used for the MethodInvoker
using System.Text;
using System.IO;

namespace MeGUI
{
    class XviDEncoder : CommandlineVideoEncoder
    {
        public XviDEncoder(string exePath)
            : base()
        {
            executable = exePath;
        }

        public override string GetFrameString(string line, int stream)
        {
            if (line.IndexOf(": key") != -1) // we found a position line, parse it
            {
                int frameNumberEnd = line.IndexOf(":");
                return line.Substring(0, frameNumberEnd).Trim();
            }
            return null;
        }

        public override string GetErrorString(string line, int stream)
        {
            if (line.IndexOf("Usage") != -1) // we get the usage message if there's an unrecognized parameter
                return line;
            return null;
        }

        protected override bool checkExitCode()
        {
            return true;
        }
    }
}
