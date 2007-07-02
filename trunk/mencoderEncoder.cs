using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MeGUI
{
    class mencoderEncoder : CommandlineVideoEncoder
    {
        public mencoderEncoder(string encoderPath)
            : base()
        {
            executable = encoderPath;
        }

        protected override bool checkExitCode()
        {
            return true;
        }

        public override string GetFrameString(string line, StreamType stream)
        {
            if (line.StartsWith("Pos:")) // status update
            {
                int frameNumberStart = line.IndexOf("s", 4) + 1;
                int frameNumberEnd = line.IndexOf("f");
                return line.Substring(frameNumberStart, frameNumberEnd - frameNumberStart).Trim();
            }
            return null;
        }
        public override string GetErrorString(string line, StreamType stream)
        {
            if (line.IndexOf("error") != -1 || line.IndexOf("not an MEncoder option") != -1)
                return line;
            return null;
        }
    }
}
