using System;
using System.Collections.Generic;
using System.Windows.Forms; // used for the MethodInvoker
using System.Text;

namespace MeGUI
{
    class x264Encoder : CommandlineVideoEncoder
    {
        public x264Encoder(string encoderPath)
            : base()
        {
            executable = encoderPath;
        }

        public override string GetFrameString(string line, int stream)
        {
            if (line.StartsWith("encoded frames:")) // status update
            {
                int frameNumberStart = line.IndexOf(":", 4) + 2;
                int frameNumberEnd = line.IndexOf("/");
                return line.Substring(frameNumberStart, frameNumberEnd - frameNumberStart).Trim();
            }
            return null;
        }

        public override string GetErrorString(string line, int stream)
        {
            if (line.IndexOf("Syntax") != -1 ||
                (line.IndexOf("error") != -1)
                || line.IndexOf("could not open") != -1)
            {
                if (line.IndexOf("converge") == -1 && line.IndexOf("try reducing") == -1 &&
                    (line.IndexOf("target:") == -1 || line.IndexOf("expected:") == -1))
                    return line;
            }
            return null;
        }
    }
}
