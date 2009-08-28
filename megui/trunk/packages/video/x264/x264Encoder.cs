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
            if (line.StartsWith("[")) // status update
            {
                int frameNumberStart = line.IndexOf("]", 4) + 2;
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

        public static bool advSettings(x264Settings xs)
        {
            if ((xs.x264Preset != 4) || (xs.x264Tuning != 0))
                 return true;
            else return false;
        }

        public static string genCommandline(string input, string output, Dar? d, int hres, int vres, x264Settings xs, Zone[] zones)
        {
            int qp;
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");

            #region main tab
            ///<summary>
            /// x264 Main Tab Settings
            ///</summary>
            // AVC Profiles
            switch (xs.Profile)
            {
                case 0: sb.Append("--profile baseline "); break;
                case 1: sb.Append("--profile main "); break;
                case 2: sb.Append("--profile high "); break;
                default: break; // Autoguess                    
            }

            // AVC Levels
            if (xs.Level != 15) // unrestricted
                sb.Append("--level " + AVCLevels.getCLILevelNames()[xs.Level] + " ");

            // x264 Presets
            switch (xs.x264Preset)
            {
                case 0: sb.Append("--preset ultrafast "); break;
                case 1: sb.Append("--preset veryfast "); break;
                case 2: sb.Append("--preset faster "); break;
                case 3: sb.Append("--preset fast "); break;
                case 5: sb.Append("--preset slow "); break;
                case 6: sb.Append("--preset slower "); break;
                case 7: sb.Append("--preset veryslow "); break;
                case 8: sb.Append("--preset placebo "); break;
                default: break; // medium
            }

            // x264 Tunings
            switch (xs.x264Tuning)
            {
                case 1: sb.Append("--tune film "); break;
                case 2: sb.Append("--tune animation "); break;
                case 3: sb.Append("--tune grain "); break;
                case 4: sb.Append("--tune psnr "); break;
                case 5: sb.Append("--tune ssim "); break;
                case 6: sb.Append("--tune touhou "); break;
                case 7: sb.Append("--tune fastdecode "); break;
                default: break; // default
            }

            // Encoding Modes
            switch (xs.EncodingMode)
            {
                case 0: // ABR
                    sb.Append("--bitrate " + xs.BitrateQuantizer + " ");
                    break;
                case 1: // CQ
                    if (xs.Lossless) sb.Append("--qp 0 ");
                    else
                    {
                        qp = (int)xs.QuantizerCRF;
                        sb.Append("--qp " + qp.ToString(ci) + " ");
                    }
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
                    if (xs.QuantizerCRF != 23)
                        sb.Append("--crf " + xs.QuantizerCRF.ToString(ci) + " ");
                    break;
            } 

            // Turbo
            if (xs.Turbo)
            {
                xs.NbRefFrames = 1;
                xs.SubPelRefinement = 1; // Q-Pel 2 iterations
                xs.METype = 0; // diamond search
                xs.I4x4mv = false;
                xs.P4x4mv = false;
                xs.I8x8mv = false;
                xs.P8x8mv = false;
                xs.B8x8mv = false;
                xs.AdaptiveDCT = false;
                xs.MixedRefs = false;
                xs.X264Trellis = 0; // disable trellis
                xs.noFastPSkip = false;
            }

            // Slow 1st Pass
            if ((!xs.Turbo) &&
               ((xs.EncodingMode == 2) || // 2 pass first pass
                (xs.EncodingMode == 4) || // automated twopass
                (xs.EncodingMode == 5) || // 3 pass first pass
                (xs.EncodingMode == 8)))  // automated threepass
                sb.Append("--slow-firstpass ");

            // Threads
            if (xs.ThreadInput)
                sb.Append("--thread-input ");
            if (xs.NbThreads > 0)
                sb.Append("--threads " + xs.NbThreads + " ");
            #endregion

            if (xs.x264AdvancedSettings)
            {
                #region frame-type tab
                ///<summary>
                /// x264 Frame-Type Tab Settings
                ///</summary>

                // H.264 Features
                if (xs.Deblock)
                {
                    if (!advSettings(xs))
                    {
                        if (xs.AlphaDeblock != 0 || xs.BetaDeblock != 0) // 0 is default value
                            sb.Append("--deblock " + xs.AlphaDeblock + ":" + xs.BetaDeblock + " ");
                    }
                    else sb.Append("--deblock " + xs.AlphaDeblock + ":" + xs.BetaDeblock + " ");
                }
                else sb.Append("--nf ");

                if (!xs.Cabac) // no cabac
                    sb.Append("--no-cabac ");            

                // GOP Size
                if (xs.KeyframeInterval != 250) // gop size of 250 is default
                    sb.Append("--keyint " + xs.KeyframeInterval + " ");
                if (xs.MinGOPSize != 25)
                    sb.Append("--min-keyint " + xs.MinGOPSize + " ");

                // B-Frames
                if (xs.NbBframes > 0)
                {
                    if (!advSettings(xs))
                    {
                        if (xs.NbBframes != 3) // 3 is default value
                            sb.Append("--bframes " + xs.NbBframes + " ");
                    }
                    else sb.Append("--bframes " + xs.NbBframes + " ");

                    if (xs.NewAdaptiveBFrames != 1) // adaptive b-frames is conditional on b frames being enabled
                        sb.Append("--b-adapt " + xs.NewAdaptiveBFrames + " ");

                    if (xs.NbBframes > 1 && xs.BFramePyramid) // pyramid needs a minimum of 2 b frames
                        sb.Append("--b-pyramid ");

                    if (!xs.WeightedBPrediction) // weighted BPrediction is conditional on b frames being enabled
                        sb.Append("--no-weightb ");

                    switch (xs.BframePredictionMode)
                    {
                        case 0: sb.Append("--direct none "); break;
                        case 2: sb.Append("--direct temporal "); break;
                        case 3: sb.Append("--direct auto "); break;
                        default: break; // spatial
                    }                    
                }

                // B-Frames bias
                if (!advSettings(xs))
                {
                    if (xs.BframeBias != 0.0M)
                        sb.Append("--b-bias " + xs.BframeBias.ToString(ci) + " ");
                }
                else sb.Append("--b-bias " + xs.BframeBias.ToString(ci) + " "); 

                // Other
                if (xs.EncodeInterlaced)
                    sb.Append("--interlaced ");

                if (xs.Scenecut)
                {
                    if (!advSettings(xs))
                    {
                        if (xs.SCDSensitivity != 40M)
                            sb.Append("--scenecut " + xs.SCDSensitivity.ToString(ci) + " ");
                    }
                    else sb.Append("--scenecut " + xs.SCDSensitivity.ToString(ci) + " ");

                }
                else sb.Append("--no-scenecut ");

                if (!advSettings(xs))
                {
                    if (xs.NbRefFrames != 3) // 3 ref frame is default
                        sb.Append("--ref " + xs.NbRefFrames + " ");
                }
                else sb.Append("--ref " + xs.NbRefFrames + " ");

                if (xs.NoiseReduction > 0)
                    sb.Append("--nr " + xs.NoiseReduction + " ");
                #endregion

                #region rc tab
                ///<summary>
                /// x264 Rate Control Tab Settings
                /// </summary>

                if (xs.EncodingMode != 1) // doesn't apply to CQ mode
                {
                    if (xs.MinQuantizer != 10) // default min quantizer is 10
                        sb.Append("--qpmin " + xs.MinQuantizer + " ");
                    if (xs.MaxQuantizer != 51) // 51 is the default max quanitzer
                        sb.Append("--qpmax " + xs.MaxQuantizer + " ");
                    if (xs.MaxQuantDelta != 4) // 4 is the default value
                        sb.Append("--qpstep " + xs.MaxQuantDelta + " ");
                    if (xs.IPFactor != 1.4M) // 1.4 is the default value
                        sb.Append("--ipratio " + xs.IPFactor.ToString(ci) + " ");
                    if (xs.PBFactor != 1.3M) // 1.3 is the default value here
                        sb.Append("--pbratio " + xs.PBFactor.ToString(ci) + " ");
                    if (xs.ChromaQPOffset != 0.0M)
                        sb.Append("--chroma-qp-offset " + xs.ChromaQPOffset.ToString(ci) + " ");
                    if (xs.VBVBufferSize > 0)
                        sb.Append("--vbv-bufsize " + xs.VBVBufferSize + " ");
                    if (xs.VBVMaxBitrate > 0)
                        sb.Append("--vbv-maxrate " + xs.VBVMaxBitrate + " ");
                    if (xs.VBVInitialBuffer != 0.9M)
                        sb.Append("--vbv-init " + xs.VBVInitialBuffer.ToString(ci) + " ");
                    if (xs.BitrateVariance != 1)
                        sb.Append("--ratetol " + xs.BitrateVariance.ToString(ci) + " ");
                    if (xs.QuantCompression != 0.6M)
                        sb.Append("--qcomp " + xs.QuantCompression.ToString(ci) + " ");
                    if (xs.EncodingMode > 1) // applies only to twopass
                    {
                        if (xs.TempComplexityBlur != 20)
                            sb.Append("--cplxblur " + xs.TempComplexityBlur.ToString(ci) + " ");
                        if (xs.TempQuanBlurCC != 0.5M)
                            sb.Append("--qblur " + xs.TempQuanBlurCC.ToString(ci) + " ");
                    }
                }

                // Dead Zones
                if (xs.DeadZoneInter != 21)
                    sb.Append("--deadzone-inter " + xs.DeadZoneInter + " ");
                if (xs.DeadZoneIntra != 11)
                    sb.Append("--deadzone-intra " + xs.DeadZoneIntra + " ");

                // RC Lookahead
                if (!advSettings(xs))
                {
                    if (xs.Lookahead != 40)
                        sb.Append("--rc-lookahead " + xs.Lookahead + " ");
                }
                else sb.Append("--rc-lookahead " + xs.Lookahead + " ");

                // Disable Macroblok Tree
                if (!xs.NoMBTree)
                    sb.Append("--no-mbtree ");

                // AQ-Mode
                if (xs.AQmode > 0)
                {
                    if (!advSettings(xs))
                    {
                        if (xs.AQmode != 1)
                            sb.Append("--aq-mode " + xs.AQmode.ToString() + " ");
                    }
                    else sb.Append("--aq-mode " + xs.AQmode.ToString() + " ");

                    if (!advSettings(xs))
                    {
                        if (xs.AQstrength != 1.0M)
                            sb.Append("--aq-strength " + xs.AQstrength.ToString(ci) + " ");
                    }
                    else sb.Append("--aq-strength " + xs.AQstrength.ToString(ci) + " ");
                }

                // custom matrices 
                if (xs.QuantizerMatrixType > 0)
                {
                    switch (xs.QuantizerMatrixType)
                    {
                        case 1: sb.Append("--cqm \"jvt\" "); break;
                        case 2: sb.Append("--cqmfile \"" + xs.QuantizerMatrix + "\" "); break;
                    }
                }

                #endregion

                #region analysis tab
                ///<summary>
                /// x264 Analysis Tab Settings
                /// </summary>

                // Disable Chroma Motion Estimation
                if (!xs.ChromaME)
                    sb.Append("--no-chroma-me ");

                // Motion Estimation Range
                if (!advSettings(xs))
                {
                    if (xs.MERange != 16)
                        sb.Append("--merange " + xs.MERange + " ");
                }
                else sb.Append("--merange " + xs.MERange + " ");

                // ME Type
                switch (xs.METype + 1)
                {
                    case 1: sb.Append("--me dia "); break;
                    case 3: sb.Append("--me umh "); break;
                    case 4: sb.Append("--me esa "); break;
                    case 5: sb.Append("--me tesa "); break;
                    default: break; // --me hex
                }                

                // subpel refinement
                if (!advSettings(xs))
                {
                    if (xs.SubPelRefinement != 6)
                        sb.Append("--subme " + (xs.SubPelRefinement + 1) + " ");
                }
                else sb.Append("--subme " + (xs.SubPelRefinement + 1) + " ");

                // macroblock types
                if (xs.MacroBlockOptions != 3)
                {
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
                        if (!xs.AdaptiveDCT) // default is checked
                            sb.Append(" --no-8x8dct ");
                        if (!sb.ToString().EndsWith(" "))
                            sb.Append(" ");
                    }
                    else
                        sb.Append("--partitions none ");
                }

                // Trellis
                if (!advSettings(xs))
                {
                    if (xs.X264Trellis != 1)
                        sb.Append("--trellis " + xs.X264Trellis + " ");
                }
                else sb.Append("--trellis " + xs.X264Trellis + " ");

                if ((xs.SubPelRefinement + 1) > 5)
                {
                    if (!advSettings(xs))
                    {
                        if ((xs.PsyRDO != 1.0M) || (xs.PsyTrellis != 0.0M))
                            sb.Append("--psy-rd " + xs.PsyRDO.ToString(ci) + ":" + xs.PsyTrellis.ToString(ci) + " ");
                    }
                    else sb.Append("--psy-rd " + xs.PsyRDO.ToString(ci) + ":" + xs.PsyTrellis.ToString(ci) + " ");
                }
                else
                {
                    if (!advSettings(xs))
                    {
                        if (xs.PsyTrellis != 0.0M)
                            sb.Append("--psy-rd 0:" + xs.PsyTrellis.ToString(ci) + " ");
                    }
                    else sb.Append("--psy-rd 0:" + xs.PsyTrellis.ToString(ci) + " "); 
                }

                if (!xs.MixedRefs)
                    sb.Append("--no-mixed-refs ");

                if (xs.NoDCTDecimate)
                    sb.Append("--no-dct-decimate ");

                if (xs.noFastPSkip)
                    sb.Append("--no-fast-pskip ");

                if (xs.NoPsy)
                    sb.Append("--no-psy ");

                #endregion

                #region misc tab
                ///<summary>
                /// x264 Misc Tab Settings
                /// </summary>

                // QPFile
                if (xs.UseQPFile)
                {
                    if (xs.EncodingMode == 0 ||
                        xs.EncodingMode == 1 ||
                        xs.EncodingMode == 2 ||
                        xs.EncodingMode == 5 ||
                        xs.EncodingMode == 9)
                        sb.Append("--qpfile " + "\"" + xs.QPFile + "\" ");
                }

                if (xs.PSNRCalculation)
                    sb.Append("--psnr ");

                if (xs.SSIMCalculation)
                    sb.Append("--ssim ");

                if (xs.fullRange)
                    sb.Append("--fullrange on ");

                if (!xs.CustomEncoderOptions.Equals("")) // add custom encoder options
                    sb.Append(xs.CustomEncoderOptions + " ");

                #endregion
            }
            #region zones
            if (zones != null && zones.Length > 0 && xs.CreditsQuantizer >= 1.0M)
            {
                sb.Append("--zones ");
                foreach (Zone zone in zones)
                {
                    sb.Append(zone.startFrame + "," + zone.endFrame + ",");
                    if (zone.mode == ZONEMODE.Quantizer)
                    {
                        sb.Append("q=");
                        sb.Append(zone.modifier + "/");
                    }
                    if (zone.mode == ZONEMODE.Weight)
                    {
                        sb.Append("b=");
                        double mod = (double)zone.modifier / 100.0;
                        sb.Append(mod.ToString(ci) + "/");
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ");
            }
            #endregion

            #region input
            if (d.HasValue)
            {
                Sar s = d.Value.ToSar(hres, vres);
                if ((s.X != 1) && (s.Y != 1))
                    sb.Append("--sar " + s.X + ":" + s.Y + " ");
            }
            #endregion

            #region output
            // recommended by the specs
            if (output.EndsWith(".264"))
                sb.Append("--aud ");  
            
            //add the rest of the commandline regarding the output
            if (xs.EncodingMode == 2 || xs.EncodingMode == 5)
                sb.Append("--output NUL ");
            else
                sb.Append("--output " + "\"" + output + "\" ");
            sb.Append("\"" + input + "\" ");
            #endregion

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
