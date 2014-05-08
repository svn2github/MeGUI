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
using System.IO;
using System.Globalization;
using System.Text;
using System.Windows.Forms; // used for the MethodInvoker

using MeGUI.core.util;

namespace MeGUI
{
    class x265Encoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "x265Encoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob && (j as VideoJob).Settings is x265Settings)
            {
                x265Settings xs = (x265Settings)((j as VideoJob).Settings);
                    return new x265Encoder(mf.Settings.X265.Path);
            }
            return null;
        }

        public x265Encoder(string encoderPath)
            : base()
        {
            UpdateCacher.CheckPackage("x265");
            executable = encoderPath;

            string x265Path = Path.Combine(Path.GetDirectoryName(encoderPath), "avs4x265.exe");
            if (File.Exists(x265Path))
                executable = x265Path;
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
        {
            if (line.StartsWith("[")) // status update
            {
                int frameNumberStart = line.IndexOf("]", 4) + 2;
                int frameNumberEnd = line.IndexOf("/");
                if (frameNumberStart > 0 && frameNumberEnd > 0 && frameNumberEnd > frameNumberStart)
                    if (base.setFrameNumber(line.Substring(frameNumberStart, frameNumberEnd - frameNumberStart).Trim()))
                        return;
            }

            if (line.ToLowerInvariant().Contains("[error]:")
                || line.ToLowerInvariant().Contains("error:"))
                oType = ImageType.Error;
            else if (line.ToLowerInvariant().Contains("[warning]:")
                || line.ToLowerInvariant().Contains("warning:"))
                oType = ImageType.Warning;
            base.ProcessLine(line, stream, oType);
        }

        public static string genCommandline(string input, string output, Dar? d, int hres, int vres, int fps_n, int fps_d, x265Settings _xs, Zone[] zones, LogItem log)
        {
            int qp;
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");
            x265Settings xs = (x265Settings)_xs.Clone();

            // log
            if (log != null)
            {
                log.LogEvent("resolution: " + hres + "x" + vres);
                log.LogEvent("frame rate: " + fps_n + "/" + fps_d);
                if (d.HasValue)
                    log.LogValue("aspect ratio", d.Value);
                if (!String.IsNullOrEmpty(xs.CustomEncoderOptions))
                    log.LogEvent("custom command line: " + xs.CustomEncoderOptions);
            }

            #region main tab
            ///<summary>
            /// x265 Main Tab Settings
            ///</summary>

            // x265 Presets
            if (!xs.CustomEncoderOptions.Contains("--preset "))
            {
                switch (xs.x265PresetLevel)
                {
                    case x265Settings.x265PresetLevelModes.ultrafast: sb.Append("--preset ultrafast "); break;
                    case x265Settings.x265PresetLevelModes.superfast: sb.Append("--preset superfast "); break;
                    case x265Settings.x265PresetLevelModes.veryfast: sb.Append("--preset veryfast "); break;
                    case x265Settings.x265PresetLevelModes.faster: sb.Append("--preset faster "); break;
                    case x265Settings.x265PresetLevelModes.fast: sb.Append("--preset fast "); break;
                    //case x265Settings.x265PresetLevelModes.medium: sb.Append("--preset medium "); break; // default value
                    case x265Settings.x265PresetLevelModes.slow: sb.Append("--preset slow "); break;
                    case x265Settings.x265PresetLevelModes.slower: sb.Append("--preset slower "); break;
                    case x265Settings.x265PresetLevelModes.veryslow: sb.Append("--preset veryslow "); break;
                    case x265Settings.x265PresetLevelModes.placebo: sb.Append("--preset placebo "); break;
                }
            }

            // x265 Tunings
            if (!xs.CustomEncoderOptions.Contains("--tune "))
            {
                switch (xs.x265PsyTuning)
                {
                    case x265Settings.x265PsyTuningModes.PSNR: sb.Append("--tune psnr "); break;
                    case x265Settings.x265PsyTuningModes.SSIM: sb.Append("--tune ssim "); break;
                    case x265Settings.x265PsyTuningModes.FastDecode: sb.Append("--tune fastdecode "); break;
                    case x265Settings.x265PsyTuningModes.ZeroLatency: sb.Append("--tune zerolatency "); break;
                    default: break;
                }
            }

            // Encoding Modes
            switch (xs.EncodingMode)
            {
                case 0: // ABR
                    if (!xs.CustomEncoderOptions.Contains("--bitrate ")) 
                        sb.Append("--bitrate " + xs.BitrateQuantizer + " ");
                    break;
                case 1: // CQ
                    if (!xs.CustomEncoderOptions.Contains("--qp "))
                    {
                        qp = (int)xs.QuantizerCRF;
                        sb.Append("--qp " + qp.ToString(ci) + " ");
                    }
                    break;
               /* case 2: // 2 pass first pass
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
                    break;*/
                case 2: // constant quality
                    if (!xs.CustomEncoderOptions.Contains("--crf "))
                        if (xs.QuantizerCRF != 28)
                            sb.Append("--crf " + xs.QuantizerCRF.ToString(ci) + " ");
                    break;
            }

            // Threads
            if (!xs.CustomEncoderOptions.Contains("--frame-threads "))
                if (xs.NbThreads > 0)
                    sb.Append("--frame-threads " + xs.NbThreads + " ");

            if (xs.x265PsyTuning == x265Settings.x265PsyTuningModes.NONE)
                sb.Append(" --no-ssim --no-psnr ");

            if (!String.IsNullOrEmpty(output))
                sb.Append(" --output " + "\"" + output + "\" ");

            if (!String.IsNullOrEmpty(input))
                sb.Append("\"" + input + "\"");

            #endregion

            return sb.ToString();
        }

        protected override string Commandline
        {
            get 
            {
                return genCommandline(job.Input, job.Output, job.DAR, hres, vres, fps_n, fps_d, job.Settings as x265Settings, job.Zones, base.log);
            }
        }

        protected override void doExitConfig()
        {
            base.doExitConfig();
        }
    }
}