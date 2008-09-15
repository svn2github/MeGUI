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
using System.Text;
using System.Windows.Forms; // used for the MethodInvoker

using MeGUI.core.util;

namespace MeGUI
{
    class x264Encoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "x264Encoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob &&
                (j as VideoJob).Settings is x264Settings)
                return new x264Encoder(mf.Settings.X264Path);
            return null;
        }

        public x264Encoder(string encoderPath)
            : base()
        {
            executable = encoderPath;
        }

        public override string GetFrameString(string line, StreamType stream)
        {
            if (line.StartsWith("encoded frames:")) // status update
            {
                int frameNumberStart = line.IndexOf(":", 4) + 2;
                int frameNumberEnd = line.IndexOf("/");
                return line.Substring(frameNumberStart, frameNumberEnd - frameNumberStart).Trim();
            }
            return null;
        }

        public override string GetErrorString(string line, StreamType stream)
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

        public static string genCommandline(string input, string output, Dar? d, int hres, int vres, x264Settings xs, Zone[] zones)
        {
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");
            if (xs.EncodingMode == 4 || xs.EncodingMode == 7)
                xs.Turbo = false; // turn off turbo to prevent inconsistent commandline preview
            switch (xs.EncodingMode)
            {
                case 0: // ABR
                    sb.Append("--bitrate " + xs.BitrateQuantizer + " ");
                    break;
                case 1: // CQ
                    if (xs.Lossless) sb.Append("--qp 0 ");
                    else sb.Append("--qp " + xs.QuantizerCRF.ToString(new CultureInfo("en-us")) + " ");
                    break;
                case 2: // 2 pass first pass
                    sb.Append("--pass 1 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case 3: // 2 pass second pass
                case 4: // automated twopass
                    sb.Append("--pass 2 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case 5: // 3 pass first pass
                    sb.Append("--pass 1 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case 6: // 3 pass 2nd pass
                    sb.Append("--pass 3 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case 7: // 3 pass 3rd pass
                case 8: // automated threepass, show third pass options
                    sb.Append("--pass 3 --bitrate " + xs.BitrateQuantizer + " --stats " + "\"" + xs.Logfile + "\" ");
                    break;
                case 9: // constant quality
                    sb.Append("--crf " + xs.QuantizerCRF.ToString(new CultureInfo("en-us")) + " ");
                    break;
            } // now add the rest of the x264 encoder options

            // AVC Level
            if (xs.Level != 15) // unrestricted (x264.exe default)
                sb.Append("--level " + AVCLevels.getCLILevelNames()[xs.Level] + " ");
            if (xs.KeyframeInterval != 250) // gop size of 250 is default
                sb.Append("--keyint " + xs.KeyframeInterval + " ");
            if (xs.MinGOPSize != 25)
                sb.Append("--min-keyint " + xs.MinGOPSize + " ");
            if (xs.Turbo)
            {
                xs.NbRefFrames = 1;
                xs.SubPelRefinement = 0; // Q-Pel 1 iteration
                xs.METype = 0; // diamond search
                xs.I4x4mv = false;
                xs.P4x4mv = false;
                xs.I8x8mv = false;
                xs.P8x8mv = false;
                xs.B8x8mv = false;
                xs.AdaptiveDCT = false;
                xs.MixedRefs = false;
                xs.BRDO = false;
                xs.X264Trellis = 0; // disable trellis
                xs.noFastPSkip = false;
                xs.WeightedBPrediction = false;
                xs.biME = false;
            }
            if (xs.DeadZoneInter != 21)
                sb.Append("--deadzone-inter " + xs.DeadZoneInter + " ");
            if (xs.DeadZoneIntra != 11)
                sb.Append("--deadzone-intra " + xs.DeadZoneIntra + " ");
            if (xs.NbRefFrames != 1) // 1 ref frame is default
                sb.Append("--ref " + xs.NbRefFrames + " ");
            if (xs.MixedRefs)
                sb.Append("--mixed-refs ");
            if (xs.noFastPSkip)
                sb.Append("--no-fast-pskip ");
            if (xs.NbBframes != 0) // 0 is default value, adaptive and pyramid are conditional on b frames being enabled
            {
                sb.Append("--bframes " + xs.NbBframes + " ");
                if (xs.NewAdaptiveBFrames != 1)
                    sb.Append("--b-adapt " + xs.NewAdaptiveBFrames + " ");
                if (xs.NbBframes > 1 && xs.BFramePyramid) // pyramid needs a minimum of 2 b frames
                    sb.Append("--b-pyramid ");
                if (xs.BRDO)
                    sb.Append("--b-rdo ");
                if (xs.biME)
                    sb.Append("--bime ");
                if (xs.WeightedBPrediction)
                    sb.Append("--weightb ");
                if (xs.BframePredictionMode != 1)
                {
                    sb.Append("--direct ");
                    if (xs.BframePredictionMode == 0)
                        sb.Append("none ");
                    else if (xs.BframePredictionMode == 2)
                        sb.Append("temporal ");
                    else if (xs.BframePredictionMode == 3)
                        sb.Append("auto ");
                }
            }
            if (xs.Deblock) // deblocker active, add options
            {
                if (xs.AlphaDeblock != 0 || xs.BetaDeblock != 0) // 0 is default value
                    sb.Append("--filter " + xs.AlphaDeblock + ":" + xs.BetaDeblock + " ");
            }
            else // no deblocking
                sb.Append("--nf ");
            if (!xs.Cabac) // no cabac
                sb.Append("--no-cabac ");
            if (xs.SubPelRefinement + 1 != 6) // non default subpel refinement
            {
                int subq = xs.SubPelRefinement + 1;
                sb.Append("--subme " + subq + " ");
            }
            if (!xs.ChromaME)
                sb.Append("--no-chroma-me ");
            if (xs.X264Trellis > 0)
                sb.Append("--trellis " + xs.X264Trellis + " ");
            if ((xs.PsyRDO != new decimal(1.0) || xs.PsyTrellis != new decimal(0.0)) && xs.SubPelRefinement + 1 > 5)
                sb.Append("--psy-rd " + xs.PsyRDO.ToString(ci) + ":" + xs.PsyTrellis.ToString(ci) + " ");
            // now it's time for the macroblock types
            if (xs.P8x8mv || xs.B8x8mv || xs.I4x4mv || xs.I8x8mv || xs.P4x4mv || xs.AdaptiveDCT)
            {
                sb.Append("--partitions ");
                if (xs.I4x4mv && xs.P4x4mv && xs.I8x8mv && xs.P8x8mv && xs.B8x8mv)
                    sb.Append("all ");
                else
                {
                    if (xs.P8x8mv) // default is checked
                        sb.Append("p8x8,");
                    if (xs.B8x8mv) // default is checked
                        sb.Append("b8x8,");
                    if (xs.I4x4mv) // default is checked
                        sb.Append("i4x4,");
                    if (xs.P4x4mv) // default is unchecked
                        sb.Append("p4x4,");
                    if (xs.I8x8mv) // default is checked
                        sb.Append("i8x8");
                    if (sb.ToString().EndsWith(","))
                        sb.Remove(sb.Length - 1, 1);
                }
                if (xs.AdaptiveDCT) // default is unchecked
                    sb.Append(" --8x8dct ");
                if (!sb.ToString().EndsWith(" "))
                    sb.Append(" ");
            }
            else
            {
                sb.Append("--partitions none ");
            }
            if (xs.EncodingMode != 1) // doesn't apply to CQ mode
            {
                if (xs.MinQuantizer != 10) // default min quantizer is 10
                    sb.Append("--qpmin " + xs.MinQuantizer + " ");
                if (xs.MaxQuantizer != 51) // 51 is the default max quanitzer
                    sb.Append("--qpmax " + xs.MaxQuantizer + " ");
                if (xs.MaxQuantDelta != 4) // 4 is the default value
                    sb.Append("--qpstep " + xs.MaxQuantDelta + " ");
                if (xs.IPFactor != new decimal(1.4)) // 1.4 is the default value
                    sb.Append("--ipratio " + xs.IPFactor.ToString(ci) + " ");
                if (xs.PBFactor != new decimal(1.3)) // 1.3 is the default value here
                    sb.Append("--pbratio " + xs.PBFactor.ToString(ci) + " ");
                if (xs.ChromaQPOffset != new decimal(0.0))
                    sb.Append("--chroma-qp-offset " + xs.ChromaQPOffset.ToString(ci) + " ");
                if (xs.VBVBufferSize > 0)
                    sb.Append("--vbv-bufsize " + xs.VBVBufferSize + " ");
                if (xs.VBVMaxBitrate > 0)
                    sb.Append("--vbv-maxrate " + xs.VBVMaxBitrate + " ");
                if (xs.VBVInitialBuffer != new decimal(0.9))
                    sb.Append("--vbv-init " + xs.VBVInitialBuffer.ToString(ci) + " ");
                if (xs.BitrateVariance != 1)
                    sb.Append("--ratetol " + xs.BitrateVariance.ToString(ci) + " ");
                if (xs.QuantCompression != new decimal(0.6))
                    sb.Append("--qcomp " + xs.QuantCompression.ToString(ci) + " ");
                if (xs.EncodingMode > 1) // applies only to twopass
                {
                    if (xs.TempComplexityBlur != 20)
                        sb.Append("--cplxblur " + xs.TempComplexityBlur.ToString(ci) + " ");
                    if (xs.TempQuanBlurCC != new decimal(0.5))
                        sb.Append("--qblur " + xs.TempQuanBlurCC.ToString(ci) + " ");
                }
            }
            if (xs.SCDSensitivity != new decimal(40))
                sb.Append("--scenecut " + xs.SCDSensitivity.ToString(ci) + " ");
            if (xs.BframeBias != new decimal(0))
                sb.Append("--b-bias " + xs.BframeBias.ToString(ci) + " ");
            if (xs.METype + 1 != 2)
            {
                sb.Append("--me ");
                if (xs.METype + 1 == 1)
                    sb.Append("dia ");
                if (xs.METype + 1 == 3)
                    sb.Append("umh ");
                if (xs.METype + 1 == 4)
                    sb.Append("esa ");
                if (xs.METype + 1 == 5)
                    sb.Append("tesa ");
            }
            if (xs.MERange != 16)
                sb.Append("--merange " + xs.MERange + " ");
            if (xs.NbThreads > 1)
                sb.Append("--threads " + xs.NbThreads + " ");
            if (xs.NbThreads == 0)
                sb.Append("--threads auto ");
            sb.Append("--thread-input ");

            if (xs.AQmode == 0)
            {
                sb.Append("--aq-mode 0 ");
            }
            if (xs.AQmode > 0)
            {
                if (xs.AQstrength != new decimal(1.0))
                    sb.Append("--aq-strength " + xs.AQstrength.ToString(ci) + " ");
            }
            if (zones != null && zones.Length > 0 && xs.CreditsQuantizer >= new decimal(1))
            {
                sb.Append("--zones ");
                foreach (Zone zone in zones)
                {
                    sb.Append(zone.startFrame + "," + zone.endFrame + ",");
                    if (zone.mode == ZONEMODE.QUANTIZER)
                    {
                        sb.Append("q=");
                        sb.Append(zone.modifier + "/");
                    }
                    if (zone.mode == ZONEMODE.WEIGHT)
                    {
                        sb.Append("b=");
                        double mod = (double)zone.modifier / 100.0;
                        sb.Append(mod.ToString(ci) + "/");
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ");
            }
            if (d.HasValue)
            {
                Sar s = d.Value.ToSar(hres, vres);
                sb.Append("--sar " + s.X + ":" + s.Y + " ");
            }
            if (xs.QuantizerMatrixType > 0) // custom matrices enabled
            {
                if (xs.QuantizerMatrixType == 1)
                    sb.Append("--cqm \"jvt\" ");
                if (xs.QuantizerMatrixType == 2)
                    sb.Append("--cqmfile \"" + xs.QuantizerMatrix + "\" ");
            }
            sb.Append("--progress "); // ensure that the progress is shown
            if (xs.NoDCTDecimate)
                sb.Append("--no-dct-decimate ");
            if (!xs.PSNRCalculation)
                sb.Append("--no-psnr ");
            if (!xs.SSIMCalculation)
                sb.Append("--no-ssim ");
            if (xs.EncodeInterlaced)
                sb.Append("--interlaced ");
            if (xs.NoiseReduction > 0)
                sb.Append("--nr " + xs.NoiseReduction + " ");
            //add the rest of the mencoder commandline regarding the output
            if (xs.EncodingMode == 2 || xs.EncodingMode == 5)
                sb.Append("--output NUL ");
            else
                sb.Append("--output " + "\"" + output + "\" ");
            sb.Append("\"" + input + "\" ");
            if (!xs.CustomEncoderOptions.Equals("")) // add custom encoder options
                sb.Append(xs.CustomEncoderOptions);
            return sb.ToString();
        }

        protected override string Commandline
        {
            get {
                return genCommandline(job.Input, job.Output, job.DAR, hres, vres, job.Settings as x264Settings, job.Zones);
            }
        }
    }
}
