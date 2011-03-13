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

using MeGUI.core.util;

namespace MeGUI
{
    class ffmpegEncoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "FFmpegEncoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob &&
                ((j as VideoJob).Settings is hfyuSettings))
                return new ffmpegEncoder(mf.Settings.FFMpegPath);
            return null;
        }

        public ffmpegEncoder(string encoderPath)
            : base()
        {
                executable = encoderPath;
        }

        #region commandline generation
        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("-y -i \"" + job.Input + "\" -vcodec ffvhuff -context 1 -vstrict -2 -pred 2 -an \"" + job.Output + "\" ");
                return sb.ToString();
                throw new Exception();
            }
        }
        #endregion

        public override string GetFrameString(string line, StreamType stream)
        {
            if (line.StartsWith("Pos:")) // status update
            {
                int frameNumberStart = line.IndexOf("s", 4) + 1;
                int frameNumberEnd = line.IndexOf("f");
                return line.Substring(frameNumberStart, frameNumberEnd - frameNumberStart).Trim();
            }
            else if (line.StartsWith("frame=")) // status update for ffmpeg
            {
                int frameNumberEnd = line.IndexOf("f", 6);
                return line.Substring(6, frameNumberEnd - 6).Trim();
            }
            return null;
        }
        public override string GetErrorString(string line, StreamType stream)
        {
            if (line.IndexOf("error") != -1)
                return line;
            return null;
        }
    }
}
