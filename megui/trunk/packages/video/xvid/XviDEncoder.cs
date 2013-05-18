// ****************************************************************************
// 
// Copyright (C) 2005-2013 Doom9 & al
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

using MeGUI.core.plugins.implemented;
using MeGUI.core.util;

namespace MeGUI
{
    class XviDEncoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "XviDEncoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob &&
                (j as VideoJob).Settings is xvidSettings)
                return new XviDEncoder(mf.Settings.XviDEncrawPath);
            return null;
        }

        public XviDEncoder(string exePath)
            : base()
        {
            executable = exePath;
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (line.IndexOf(": key") != -1) // we found a position line, parse it
            {
                int frameNumberEnd = line.IndexOf(":");
                if (base.setFrameNumber(line.Substring(0, frameNumberEnd).Trim()))
                    return;
            }

            if (line.ToLowerInvariant().Contains("error")
                || line.ToLowerInvariant().Contains("usage") // we get the usage message if there's an unrecognized parameter
                || line.ToLowerInvariant().Contains("avistreamwrite"))
                oType = ImageType.Error;
            else if (line.ToLowerInvariant().Contains("warning"))
                oType = ImageType.Warning;
            base.ProcessLine(line, stream, oType);
        }

        protected override string Commandline
        {
            get
            {
                return genCommandline(job.Input, job.Output, job.DAR, job.Settings as xvidSettings, hres, vres, job.Zones);
            }
        }

        public static string genCommandline(string input, string output, Dar? d, xvidSettings xs, int hres, int vres, Zone[] zones)
        {
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");
            sb.Append("-i \"" + input + "\" ");
            switch (xs.EncodingMode)
            {
                case 0: // CBR
                    sb.Append("-single -bitrate " + xs.BitrateQuantizer + " "); // add bitrate
                    break;
                case 1: // CQ
                    sb.Append("-single -cq " + xs.Quantizer.ToString(ci) + " "); // add quantizer
                    break;
                case 2: // 2 pass first pass
                    sb.Append("-pass1 " + "\"" + xs.Logfile + "\" -bitrate " + xs.BitrateQuantizer + " "); // add logfile
                    break;
                case 3: // 2 pass second pass
                case 4: // automated twopass
                    sb.Append("-pass2 " + "\"" + xs.Logfile + "\" -bitrate " + xs.BitrateQuantizer + " "); // add logfile
                    break;
            }
            if (xs.EncodingMode <= 1) // 1 pass modes
            {
                if (!xs.CustomEncoderOptions.Contains("-reaction ") && xs.ReactionDelayFactor != 16)
                    sb.Append("-reaction " + xs.ReactionDelayFactor + " ");
                if (!xs.CustomEncoderOptions.Contains("-averaging ") && xs.AveragingPeriod != 100)
                    sb.Append("-averaging " + xs.AveragingPeriod + " ");
                if (!xs.CustomEncoderOptions.Contains("-smoother ") && xs.RateControlBuffer != 100)
                    sb.Append("-smoother " + xs.RateControlBuffer + " ");
            }
            else // two pass modes
            {
                if (!xs.CustomEncoderOptions.Contains("-kboost ") && xs.KeyFrameBoost != 10)
                    sb.Append("-kboost " + xs.KeyFrameBoost + " ");
                if (!xs.CustomEncoderOptions.Contains("-kthresh ") && xs.KeyframeThreshold != 1)
                    sb.Append("-kthresh " + xs.KeyframeThreshold + " ");
                if (!xs.CustomEncoderOptions.Contains("-kreduction ") && xs.KeyframeReduction != 20)
                    sb.Append("-kreduction " + xs.KeyframeReduction + " ");
                if (!xs.CustomEncoderOptions.Contains("-ostrength ") && xs.OverflowControlStrength != 5)
                    sb.Append("-ostrength " + xs.OverflowControlStrength + " ");
                if (!xs.CustomEncoderOptions.Contains("-oimprove ") && xs.MaxOverflowImprovement != 5)
                    sb.Append("-oimprove " + xs.MaxOverflowImprovement + " ");
                if (!xs.CustomEncoderOptions.Contains("-odegrade ") && xs.MaxOverflowDegradation != 5)
                    sb.Append("-odegrade " + xs.MaxOverflowDegradation + " ");
                if (!xs.CustomEncoderOptions.Contains("-chigh ") && xs.HighBitrateDegradation != 0)
                    sb.Append("-chigh " + xs.HighBitrateDegradation + " ");
                if (!xs.CustomEncoderOptions.Contains("-clow ") && xs.LowBitrateImprovement != 0)
                    sb.Append("-clow " + xs.LowBitrateImprovement + " ");
                if (xs.XvidProfile != 0)
                {
                    int ivbvmax = 0, ivbvsize = 0, ivbvpeak = 0;
                    switch (xs.XvidProfile)
                    {
                        case 1:
                            ivbvmax = 4854000;
                            ivbvsize = 3145728;
                            ivbvpeak = 2359296;
                            break;
                        case 2:
                            ivbvmax = 9708400;
                            ivbvsize = 6291456;
                            ivbvpeak = 4718592;
                            break;
                        case 3:
                            ivbvmax = 20000000;
                            ivbvsize = 16000000;
                            ivbvpeak = 12000000;
                            break;
                        case 4:
                            ivbvmax = 200000;
                            ivbvsize = 262144;
                            ivbvpeak = 196608;
                            break;
                        case 5:
                            ivbvmax = 600000;
                            ivbvsize = 655360;
                            ivbvpeak = 491520;
                            break;
                        case 6:
                            ivbvmax = xs.VbvMaxRate;
                            ivbvsize = xs.VbvBuffer;
                            ivbvpeak = xs.VbvPeakRate;
                            break;
                    }
                    if (!xs.CustomEncoderOptions.Contains("-vbvsize ") && ivbvsize != 0)
                        sb.Append("-vbvsize " + ivbvsize + " ");
                    if (!xs.CustomEncoderOptions.Contains("-vbvmax ") && ivbvmax != 0)
                        sb.Append("-vbvmax " + ivbvmax + " ");
                    if (!xs.CustomEncoderOptions.Contains("-vbvpeak ") && ivbvpeak != 0)
                        sb.Append("-vbvpeak " + ivbvpeak + " ");
                }
            }
            if (!xs.CustomEncoderOptions.Contains("-turbo") && xs.Turbo)
                sb.Append("-turbo ");
            if (!xs.CustomEncoderOptions.Contains("-max_key_interval ") && xs.KeyframeInterval != 300)
                sb.Append("-max_key_interval " + xs.KeyframeInterval + " ");
            if (!xs.CustomEncoderOptions.Contains("-nopacked") && !xs.PackedBitstream) // default is on in encraw
                sb.Append("-nopacked ");
            if (!xs.CustomEncoderOptions.Contains("-quality ") && xs.MotionSearchPrecision != 6)
                sb.Append("-quality " + xs.MotionSearchPrecision + " ");
            if (!xs.CustomEncoderOptions.Contains("-vhqmode ") && xs.VHQMode != 1)
                sb.Append("-vhqmode " + xs.VHQMode + " ");
            if (!xs.CustomEncoderOptions.Contains("-qpel ") && xs.QPel)
                sb.Append("-qpel ");
            if (!xs.CustomEncoderOptions.Contains("-gmc ") && xs.GMC)
                sb.Append("-gmc ");
            if (!xs.CustomEncoderOptions.Contains("-qtype ") && xs.QuantizerMatrix == xvidSettings.MPEGMatrix)
                sb.Append("-qtype 1 ");
            else if (!xs.CustomEncoderOptions.Contains("-qmatrix ") && xs.QuantizerMatrix != xvidSettings.H263Matrix && !string.IsNullOrEmpty(xs.QuantizerMatrix))
                sb.Append("-qmatrix \"" + xs.QuantizerMatrix + "\" ");
            if (!xs.CustomEncoderOptions.Contains("-interlaced ") && xs.Interlaced)
            {
                sb.Append("-interlaced ");
                if (xs.BottomFieldFirst)
                    sb.Append("1 ");
                else
                    sb.Append("2 ");
            }
            if (!xs.CustomEncoderOptions.Contains("-lumimasking") && xs.HVSMasking != 0)
                sb.Append("-lumimasking ");
            if (!xs.CustomEncoderOptions.Contains("-notrellis") && !xs.Trellis)
                sb.Append("-notrellis ");
            if (!xs.CustomEncoderOptions.Contains("-nochromame") && !xs.ChromaMotion)
                sb.Append("-nochromame ");
            if (!xs.CustomEncoderOptions.Contains("-imin ") && xs.MinQuantizer != 2)
                sb.Append("-imin " + xs.MinQuantizer + " ");
            if (!xs.CustomEncoderOptions.Contains("-imax ") && xs.MaxQuantizer != 31)
                sb.Append("-imax " + xs.MaxQuantizer + " ");
            if (!xs.CustomEncoderOptions.Contains("-pmin ") && xs.MinPQuant != 2)
                sb.Append("-pmin " + xs.MinPQuant + " ");
            if (!xs.CustomEncoderOptions.Contains("-pmax ") && xs.MaxPQuant != 31)
                sb.Append("-pmax " + xs.MaxPQuant + " ");
            if (!xs.CustomEncoderOptions.Contains("-noclosed_gop") && !xs.ClosedGOP)
                sb.Append("-noclosed_gop ");
            if (!xs.CustomEncoderOptions.Contains("-drop ") && xs.FrameDropRatio != 0)
                sb.Append("-drop " + xs.FrameDropRatio + " ");
            if (!xs.CustomEncoderOptions.Contains("-max_bframes ") && xs.NbBframes != 2) 
                sb.Append("-max_bframes " + xs.NbBframes + " ");
            if (xs.NbBframes > 0)
            {
                if (!xs.CustomEncoderOptions.Contains("-bvhq ") && xs.VHQForBframes)
                    sb.Append("-bvhq ");
                if (!xs.CustomEncoderOptions.Contains("-bquant_ratio ") && xs.BQuantRatio != 150)
                    sb.Append("-bquant_ratio " + xs.BQuantRatio + " ");
                if (!xs.CustomEncoderOptions.Contains("-bquant_offset ") && xs.BQuantOffset != 100)
                    sb.Append("-bquant_offset " + xs.BQuantOffset + " ");
                if (!xs.CustomEncoderOptions.Contains("-bmin ") && xs.MinBQuant != 2)
                    sb.Append("-bmin " + xs.MinBQuant + " ");
                if (!xs.CustomEncoderOptions.Contains("-bmax ") && xs.MaxBQuant != 31)
                    sb.Append("-bmax " + xs.MaxBQuant + " ");
            }
            if (!xs.CustomEncoderOptions.Contains("-par ") && d.HasValue) // custom PAR mode
            {
                Sar s = d.Value.ToSar(hres, vres);
                if (s.X == 1 && s.Y == 1)
                    sb.Append("-par 1 ");
                else if (s.X == 12 && s.Y == 11)
                    sb.Append("-par 2 ");
                else if (s.X == 10 && s.Y == 11)
                    sb.Append("-par 3 ");
                else if (s.X == 16 && s.Y == 11)
                    sb.Append("-par 4 ");
                else if (s.X == 40 && s.Y == 33)
                    sb.Append("-par 5 ");
                else
                    sb.Append("-par " + s.X + ":" + s.Y + " ");
            }
            if (!xs.CustomEncoderOptions.Contains("-threads ") && xs.NbThreads > 0)
                sb.Append("-threads " + xs.NbThreads + " ");
            if (zones != null && zones.Length > 0 && xs.CreditsQuantizer >= new decimal(1)
                && xs.EncodingMode != 1) // only for non CQ mode at the moment
            {
                foreach (Zone zone in zones)
                {
                    if (zone.mode == ZONEMODE.Quantizer)
                        sb.Append("-zq " + zone.startFrame + " " + zone.modifier + " ");
                    if (zone.mode == ZONEMODE.Weight)
                    {
                        sb.Append("-zw " + zone.startFrame + " ");
                        double mod = (double)zone.modifier / 100.0;
                        sb.Append(mod.ToString(ci) + " ");
                    }
                }
            }
            if (xs.EncodingMode != 2) // not 2 pass vbr first pass, add output filename and output type
            {
                string extension = Path.GetExtension(output).ToLower(System.Globalization.CultureInfo.InvariantCulture);
                if (extension.Equals(".mkv"))
                    sb.Append("-mkv \"" + output + "\"");
                else if (extension.Equals(".avi"))
                    sb.Append("-avi \"" + output + "\"");
                else
                    sb.Append("-o \"" + output + "\"");
            }
            if (!String.IsNullOrEmpty(xs.CustomEncoderOptions.Trim())) // add custom encoder options
                sb.Append(" " + xs.CustomEncoderOptions);
            return sb.ToString();
        }
    }
}