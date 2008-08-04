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
                ((j as VideoJob).Settings is snowSettings || (j as VideoJob).Settings is lavcSettings || (j as VideoJob).Settings is hfyuSettings))
                return new mencoderEncoder(mf.Settings.MencoderPath);
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
                if (job.Settings is lavcSettings)
                    return genLavcCommandline(job.Input, job.Output, job.DAR, job.Settings as lavcSettings, job.Zones);
                else if (job.Settings is snowSettings)
                    return genSnowCommandline(job.Input, job.Output, job.Settings as snowSettings, job.Zones);
                else if (job.Settings is hfyuSettings)
                    return genHfyuCommandline();
                throw new Exception();
            }
        }

        private static int getVideoOutputType(string path)
        {
            if (path != null)
            {
                string extension = Path.GetExtension(path).ToLower();
                if (extension.Equals(".avi"))
                    return 0;
                if (extension.Equals(".raw") || extension.Equals(".264") || extension.Equals(".m4v"))
                    return 1;
                if (extension.Equals(".mp4"))
                    return 2;
            }
            return -1;
        }


        private string genHfyuCommandline()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"" + job.Input + "\" -o \"" + job.Output + "\" ");
            sb.Append("-of avi -forceidx "); // Make sure we don't get problems with filesizes > 2GB
            sb.Append("-ovc lavc -nosound -lavcopts vcodec=ffvhuff:vstrict=-2:pred=2:context=1");
            return sb.ToString();
        }

        public static string genSnowCommandline(string input, string output, snowSettings ss, Zone[] zones)
        {
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");
            sb.Append("\"" + input + "\" -ovc lavc -nosound ");
            switch (ss.EncodingMode)
            {
                case 0: // CBR
                    sb.Append("-lavcopts vcodec=snow:vbitrate=" + ss.BitrateQuantizer + ":"); // add bitrate
                    break;
                case 1: // CQ
                    sb.Append("-lavcopts vcodec=snow:vqscale=" + ss.Quantizer + ":");
                    break;
                case 2: // 2 pass first pass
                    sb.Append("-o NUL: -passlogfile " + "\"" + ss.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vcodec=snow:vpass=1:vqscale=5:"); // workaround for corrupted first passes
                    break;
                case 3: // 2 pass second pass
                case 4: // automated twopass
                    sb.Append(" -passlogfile " + "\"" + ss.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vcodec=snow:vpass=2:vbitrate=" + ss.BitrateQuantizer + ":"); // add pass & bitrate
                    break;
                case 5:
                    sb.Append("-o NUL: -passlogfile " + "\"" + ss.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vcodec=snow:vpass=1:vqscale=5:"); // workaround for corrupted first passes
                    break;
                case 6:
                    sb.Append(" -passlogfile " + "\"" + ss.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vcodec=snow:vpass=3:vbitrate=" + ss.BitrateQuantizer + ":"); // add pass & bitrate
                    break;
                case 7:
                    sb.Append(" -passlogfile " + "\"" + ss.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vcodec=snow:vpass=3:vbitrate=" + ss.BitrateQuantizer + ":"); // add pass & bitrate
                    break;
            }
            if (ss.PredictionMode != 0)
                sb.Append("pred=" + ss.PredictionMode + ":");
            switch (ss.MECompFullpel)
            {
                case 0:
                    break;
                case 1:
                    sb.Append("cmp=1:");
                    break;
                case 2:
                    sb.Append("cmp=11:");
                    break;
                case 3:
                    sb.Append("cmp=12:");
                    break;
            }
            switch (ss.MECompHpel)
            {
                case 0:
                    break;
                case 1:
                    sb.Append("subcmp=1:");
                    break;
                case 2:
                    sb.Append("subcmp=11:");
                    break;
                case 3:
                    sb.Append("subcmp=12:");
                    break;
            }
            switch (ss.MBComp)
            {
                case 0:
                    break;
                case 1:
                    sb.Append("mbcmp=1:");
                    break;
                case 2:
                    sb.Append("mbcmp=11:");
                    break;
                case 3:
                    sb.Append("mbcmp=12:");
                    break;
            }
            if (ss.QPel) // default is unchecked
                sb.Append("qpel:");
            if (ss.V4MV) // default is unchecked
                sb.Append("v4mv:");
            if (ss.NbMotionPredictors != 0)
                sb.Append("last_pred=" + ss.NbMotionPredictors.ToString(ci) + ":");
            sb.Append("vstrict=-2:");
            if (zones != null && zones.Length > 0 && ss.CreditsQuantizer >= new decimal(1))
            {
                sb.Append("-vrc_override=");
                foreach (Zone zone in zones)
                {
                    if (zone.mode == ZONEMODE.QUANTIZER)
                        sb.Append(zone.startFrame + "," + zone.endFrame + "," + zone.modifier + "/");
                    else
                        sb.Append(zone.startFrame + "," + zone.endFrame + ",-" + zone.modifier + "/");
                }
                sb.Remove(sb.Length - 1, 1); // remove trailing /(zone separator)
                sb.Append(":");
            }
            if (sb.ToString().EndsWith(":")) // remove the last :
                sb.Remove(sb.Length - 1, 1);
            if (ss.EncodingMode != 2)
            {
                sb.Append(" -o \"" + output + "\" -of "); // rest of mencoder options
                int outputType = getVideoOutputType(output);
                if (outputType == 0) // AVI
                    sb.Append("avi -ffourcc " + ss.FourCCs[ss.FourCC] + " ");
                if (outputType >= 1) // RAW
                    sb.Append("rawvideo ");
            }
            return sb.ToString();
        }

        public static string genLavcCommandline(string input, string output, Dar? d, lavcSettings ls, Zone[] zones)
        {
            CultureInfo ci = new CultureInfo("en-us");
            StringBuilder sb = new StringBuilder();
            sb.Append("\"" + input + "\" -ovc lavc -nosound ");
            if (ls.EncodingMode == 4 || ls.EncodingMode == 7)
                ls.Turbo = false;
            switch (ls.EncodingMode)
            {
                case 0: // CBR
                    sb.Append("-lavcopts vbitrate=" + ls.BitrateQuantizer + ":"); // add bitrate
                    break;
                case 1: // CQ
                    sb.Append("-lavcopts vqscale=" + ls.BitrateQuantizer + ":"); // add quantizer
                    break;
                case 2: // 2 pass first pass
                    sb.Append("-o NUL: -passlogfile " + "\"" + ls.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vpass=1:vbitrate=" + ls.BitrateQuantizer + ":"); // add pass & quantizer
                    if (ls.Turbo)
                        sb.Append("turbo:");
                    break;
                case 3: // 2 pass second pass
                case 4: // 2 pass automated encoding
                    sb.Append(" -passlogfile " + "\"" + ls.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vpass=2:vbitrate=" + ls.BitrateQuantizer + ":"); // add pass & bitrate
                    break;
                case 5: // 3 pass first pass
                    sb.Append("-o NUL: -passlogfile " + "\"" + ls.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vpass=1:vbitrate=" + ls.BitrateQuantizer + ":"); // add pass & quantizer
                    break;
                case 6: // 3 pass 2nd pass
                    sb.Append(" -passlogfile " + "\"" + ls.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vpass=3:vbitrate=" + ls.BitrateQuantizer + ":"); // add pass & bitrate
                    break;
                case 7:
                    sb.Append(" -passlogfile " + "\"" + ls.Logfile + "\" "); // add logfile
                    sb.Append("-lavcopts vpass=3:vbitrate=" + ls.BitrateQuantizer + ":"); // add pass & bitrate
                    break;
            } // now add the rest of the lavc encoder options
            if (ls.KeyframeInterval != 250) // gop size of 250 is default
                sb.Append("keyint=" + ls.KeyframeInterval + ":");
            if (ls.NbBframes != 0) // 0 is default value
                sb.Append("vmax_b_frames=" + ls.NbBframes + ":");
            if (ls.AvoidHighMoBframes && (ls.EncodingMode == 2 || ls.EncodingMode == 5)) // is only available in first pass
                sb.Append("vb_strategy=1:");
            if (ls.MbDecisionAlgo > 0) // default is 0
                sb.Append("mbd=" + ls.MbDecisionAlgo + ":");
            if (ls.V4MV)
                sb.Append("v4mv:");
            if (ls.SCD != 0)
                sb.Append("sc_treshold=" + ls.SCD + ":");
            if (ls.QPel)
                sb.Append("qpel:");
            if (ls.LumiMasking != new decimal(0.0))
                sb.Append("lumi_mask=" + ls.LumiMasking.ToString(ci) + ":");
            if (ls.DarkMask != new decimal(0.0))
                sb.Append("dark_mask=" + ls.DarkMask.ToString(ci) + ":");
            if (ls.SubpelRefinement != 8)
                sb.Append("subq=" + ls.SubpelRefinement + ":");
            if (ls.GreyScale)
                sb.Append("gray:");
            if (ls.Interlaced)
            {
                sb.Append("ildct:ilme");
                if (ls.FieldOrder != -1)
                    sb.Append("top=" + ls.FieldOrder + ":");
            }
            if (ls.MERange != (decimal)0)
                sb.Append("me_range=" + ls.MERange.ToString(ci) + ":");
            if (ls.InitialBufferOccupancy != (decimal)0.9)
                sb.Append("vrc_init_occupancy=" + ls.InitialBufferOccupancy.ToString(ci) + ":");
            if (ls.BorderMask != (decimal)0.0)
                sb.Append("border_mask=" + ls.BorderMask.ToString(ci) + ":");
            if (ls.TemporalMask != (decimal)0.0)
                sb.Append("tcplx_mask=" + ls.TemporalMask.ToString(ci) + ":");
            if (ls.SpatialMask != (decimal)0.0)
                sb.Append("scplx_mask=" + ls.SpatialMask.ToString(ci) + ":");
            if (ls.NbThreads > 1)
                sb.Append("threads=" + ls.NbThreads + ":");
            if (ls.MinQuantizer != 2)
                sb.Append("vqmin=" + ls.MinQuantizer + ":");
            if (ls.MaxQuantizer != 31)
                sb.Append("vqmax=" + ls.MaxQuantizer + ":");
            if (ls.MaxQuantDelta != 3)
                sb.Append("vqdiff=" + ls.MaxQuantDelta + ":");
            if (ls.EncodingMode == 0 || ls.EncodingMode == 1)
            {
                if (ls.IPFactor != (decimal)0.8)
                    sb.Append("vi_qfactor=" + ls.IPFactor.ToString(ci) + ":");
                if (ls.PBFactor != (decimal)1.25)
                    sb.Append("vb_qfactor=" + ls.PBFactor.ToString(ci) + ":");
                if (ls.BQuantFactor != (decimal)1.25)
                    sb.Append("vb_qoffset=" + ls.BQuantFactor.ToString(ci) + ":");
            }
            if (ls.QuantizerBlur != new decimal(0.5))
                sb.Append("vqblur=" + ls.QuantizerBlur.ToString(ci) + ":");
            if (ls.QuantizerCompression != new decimal(0.5))
                sb.Append("vqcomp=" + ls.QuantizerCompression.ToString(ci) + ":");
            if (ls.Trellis)
                sb.Append("trell:");
            if (ls.MinBitrate != 0) // 0 = unlimited is default
                sb.Append("vrc_minrate=" + ls.MinBitrate + ":");
            if (ls.MaxBitrate != 0) // 0 = unlimited is default
                sb.Append("vrc_maxrate=" + ls.MaxBitrate + ":");
            if (ls.BufferSize != 0) // default = empty
                sb.Append("vrc_buf_size=" + ls.BufferSize + ":");
            if (ls.FilesizeTolerance != 8000) // default is 8000 kbits
                sb.Append("vratetol=" + ls.FilesizeTolerance + ":");
            if (zones != null && zones.Length > 0 && ls.CreditsQuantizer >= new decimal(1))
            {
                sb.Append("vrc_override=");
                foreach (Zone zone in zones)
                {
                    if (zone.mode == ZONEMODE.QUANTIZER)
                        sb.Append(zone.startFrame + "," + zone.endFrame + "," + zone.modifier + "/");
                    else
                        sb.Append(zone.startFrame + "," + zone.endFrame + ",-" + zone.modifier + "/");
                }
                sb.Remove(sb.Length - 1, 1); // remove trailing slash
                sb.Append(":");
            }
            if (!ls.IntraMatrix.Equals(""))
                sb.Append("intra_matrix=" + ls.IntraMatrix + ":");
            if (!ls.InterMatrix.Equals(""))
                sb.Append("inter_matrix=" + ls.InterMatrix + ":");
            if (d.HasValue)
                sb.Append("aspect=" + d.Value.X + "/" + d.Value.Y + ":");
            if (ls.NbMotionPredictors != (decimal)0)
                sb.Append("last_pred=" + ls.NbMotionPredictors.ToString(ci) + ":");

            if (sb.ToString().EndsWith(":")) // remove the last :
                sb.Remove(sb.Length - 1, 1);
            //add the rest of the mencoder commandline regarding the output
            if (ls.EncodingMode != 2 && ls.EncodingMode != 5) // not 2 pass vbr first pass and 3 pass first pass, add output filename and output type
            {
                sb.Append(" -o \"" + output + "\" -of "); // rest of mencoder options
                int outputType = getVideoOutputType(output);
                if (outputType == 0) // AVI
                    sb.Append("avi -ffourcc " + ls.FourCCs[ls.FourCC] + " ");
                if (outputType >= 1) // RAW
                    sb.Append("rawvideo ");
            }
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
