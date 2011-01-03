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
    class mencoderEncoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "MencoderEncoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob &&
                (j as VideoJob).Settings is snowSettings)
                return new mencoderEncoder(mf.Settings.FFMpegPath);
            if (j is VideoJob &&
                (j as VideoJob).Settings is hfyuSettings)
                    return new mencoderEncoder(mf.Settings.FFMpegPath);
            return null;
        }

        public mencoderEncoder(string encoderPath)
            : base()
        {
                executable = encoderPath;
        }

        #region commandline generation
        protected override string Commandline
        {
            get
            {
                if (job.Settings is snowSettings)
                    return genSnowCommandline(job.Input, job.Output, job.Settings as snowSettings, job.Zones);
                else if (job.Settings is hfyuSettings)
                    return genHfyuCommandline();
                throw new Exception();
            }
        }

        private string genHfyuCommandline()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("-y -i \"" + job.Input + "\" -vcodec ffvhuff -context 1 -vstrict -2 -pred 2 -an \"" + job.Output + "\" ");
            return sb.ToString();
        }

        public static string genSnowCommandline(string input, string output, snowSettings ss, Zone[] zones)
        {
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");
            sb.Append("-y -an -i \"" + input + "\" ");
            switch (ss.EncodingMode)
            {
                case 0: // CBR
                    sb.Append(" -vcodec snow -b " + ss.BitrateQuantizer + "k"); // add bitrate
                    break;
                case 1: // CQ
                    sb.Append(" -vcodec snow -qscale " + ss.Quantizer);
                    break;
                case 2: // 2 pass first pass
                    sb.Append(" -passlogfile " + "\"" + ss.Logfile + "\""); // add logfile
                    sb.Append(" -vcodec snow -pass 1 -qscale 5 "); // workaround for corrupted first passes
                    break;
                case 3: // 2 pass second pass
                case 4: // automated twopass
                    sb.Append(" -passlogfile " + "\"" + ss.Logfile + "\""); // add logfile
                    sb.Append(" -vcodec snow -pass 2 -b " + ss.BitrateQuantizer + "k"); // add pass & bitrate
                    break;
                case 5:
                    sb.Append(" -passlogfile " + "\"" + ss.Logfile + "\""); // add logfile
                    sb.Append(" -vcodec snow -pass 1 -qscale 5 "); // workaround for corrupted first passes
                    break;
                case 6:
                    sb.Append(" -passlogfile " + "\"" + ss.Logfile + "\""); // add logfile
                    sb.Append(" -vcodec snow -pass 3 -b " + ss.BitrateQuantizer + "k"); // add pass & bitrate
                    break;
                case 7:
                    sb.Append(" -passlogfile " + "\"" + ss.Logfile + "\""); // add logfile
                    sb.Append(" -vcodec snow -pass 3 -b " + ss.BitrateQuantizer + "k"); // add pass & bitrate
                    break;
            }
            if (ss.PredictionMode != 0)
                sb.Append(" -pred " + ss.PredictionMode);
            switch (ss.MECompFullpel)
            {
                case 0:
                    break;
                case 1:
                    sb.Append(" -cmp 1");
                    break;
                case 2:
                    sb.Append(" -cmp 11");
                    break;
                case 3:
                    sb.Append(" -cmp 12");
                    break;
            }
            switch (ss.MECompHpel)
            {
                case 0:
                    break;
                case 1:
                    sb.Append(" -subcmp 1");
                    break;
                case 2:
                    sb.Append(" -subcmp 11");
                    break;
                case 3:
                    sb.Append(" -subcmp 12");
                    break;
            }
            switch (ss.MBComp)
            {
                case 0:
                    break;
                case 1:
                    sb.Append(" -mbcmp 1");
                    break;
                case 2:
                    sb.Append(" -mbcmp 11");
                    break;
                case 3:
                    sb.Append(" -mbcmp 12");
                    break;
            }
            if (ss.QPel) // default is unchecked
                sb.Append(" -qpel");
            if (ss.V4MV) // default is unchecked
                sb.Append(" -v4mv");
            if (ss.NbMotionPredictors != 0)
                sb.Append(" -last_pred " + ss.NbMotionPredictors.ToString(ci));
            sb.Append(" -vstrict -2");
            if (zones != null && zones.Length > 0 && ss.CreditsQuantizer >= new decimal(1))
            {
                sb.Append(" -vrc_override ");
                foreach (Zone zone in zones)
                {
                    if (zone.mode == ZONEMODE.Quantizer)
                        sb.Append(zone.startFrame + "," + zone.endFrame + "," + zone.modifier + "/");
                    else
                        sb.Append(zone.startFrame + "," + zone.endFrame + ",-" + zone.modifier + "/");
                }
                sb.Remove(sb.Length - 1, 1); // remove trailing /(zone separator)
            }
            if (ss.EncodingMode == 2 || ss.EncodingMode == 5)
                sb.Append(" -f rawvideo NUL"); // rest of mencoder options
            else
                sb.Append(" \"" + output + "\""); // rest of mencoder options
            return sb.ToString();
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
            if (line.IndexOf("error") != -1 || line.IndexOf("not an MEncoder option") != -1)
                return line;
            return null;
        }
    }
}
