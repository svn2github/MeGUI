using System;
using System.Collections.Generic;
using System.Windows.Forms; // used for the MethodInvoker
using System.Text;
using System.Globalization;

namespace MeGUI
{
    internal class x264farmEncoder : CommandlineVideoEncoder
    {
        public static readonly JobProcessorFactory Factory =
            new JobProcessorFactory(new ProcessorFactory(init), "x264farmEncoder");

        protected override string Commandline
        {
            get {
                return genCommandline(job.Input, job.Output, job.DAR, hres, vres, job.Settings as x264farmSettings);
            }
        }

        public static string genCommandline(string input, string output, MeGUI.core.util.Dar? dar, int hres, int vres, x264farmSettings xs)
        {
            StringBuilder sb = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-us");

            //Append Required Fields
            //--first, --second, --bitrate, --avs, and --output
            sb.Append("--first " + "\"" + xs.FirstPassSettings + "\" ");
            sb.Append("--second " + "\"" + xs.SecondPassSettings + "\" ");
            sb.Append("--avs " + "\"" + input + "\" ");
            sb.Append("--output " + "\"" + output + "\" ");

            //here are nonMandatory parameters
            //string --firstavs, --fastavs, --config "config.xml", --zones ""
            //int --preseek 0, --batch 5000, --split 250, --3gops 1073741823(???), --rerc 0, --seek 0, --frames 0, --qpmin 10, --qpmax 51
            //    --qpstep 4
            //decimal --thresh 20.0, --3thresh 0.8, --3ratio 0.05, --sizeprec 1.0, --qcomp 0.6, 
            //        --qcomp 0.6, --qcomp 0.6, --cplxblur 20.0, --qblur 0.5, --ipratio 1.4, --pbratio 1.3
            //bool --force, --restart, --savedisk, --nocomp

            if (!xs.FirstPassAVS.Equals(""))
                sb.Append("--firstavs " + "\"" + xs.FirstPassAVS + "\" ");
            if (!xs.FirstPassAVSFast.Equals(""))
                sb.Append("--fastavs " + "\"" + xs.FirstPassAVSFast + "\" ");

            if (xs.BitrateQuantizer >= 0)
                if (xs.EncodingMode == 0)
                {
                    sb.Append("--bitrate " + "\"" + xs.BitrateQuantizer + "kbps\" ");
                }
                else
                {
                    sb.Append("--bitrate " + "\"" + xs.BitrateQuantizer + "\" ");
                }

            if (xs.PreseekFrames > 0)
                sb.Append("--preseek " + xs.PreseekFrames + " ");
            if (xs.ForceSPReEnc)
                sb.Append("--force ");
            if (xs.RestartTotal)
                sb.Append("--restart ");
            if (xs.SavediskOnMerge)
                sb.Append("--savedisk ");
            if (xs.NoCompression)
                sb.Append("--nocomp ");
            if (xs.ConfigExt)
                sb.Append("--config " + "\"" + xs.ConfigPath + "\" ");

            if (xs.BatchFPSize != 5000)
                sb.Append("--batch " + xs.BatchFPSize + " ");
            if (xs.BatchFPMulti != new decimal(0.5))
                sb.Append("--batchmult " + xs.BatchFPMulti.ToString(ci) + " ");
            if (xs.SplitFPFrames != 250)
                sb.Append("--split " + xs.SplitFPFrames + " ");
            if (xs.ThreshFPSplit != new decimal(20.0))
                sb.Append("--thresh " + xs.ThreshFPSplit.ToString(ci) + " ");
            if (xs.ThreshFPReSplitDont != true)
                sb.Append("--rethresh " + xs.ThreshFPReSplit.ToString(ci) + " ");

            if (xs.ThreshTPAccuracy != new decimal(0.8))
                sb.Append("--3thresh " + xs.ThreshTPAccuracy.ToString(ci) + " ");
            if (xs.GopsReEncDont != true)
                sb.Append("--3gops " + xs.GopsReEnc + " ");
            if (xs.RatioTPGopsReEnc != new decimal(0.05))
                sb.Append("--3ratio " + xs.RatioTPGopsReEnc.ToString(ci) + " ");
            if (xs.ReRateControlGopsReEnc != 0)
                sb.Append("--rerc " + xs.ReRateControlGopsReEnc + " ");
            if (xs.SizePrecision != new decimal(1.0))
                sb.Append("--sizeprec " + xs.SizePrecision.ToString(ci) + " ");

            if (xs.X264Seek > 0)
                sb.Append("--seek " + xs.X264Seek + " ");
            if (xs.X264Frames > 0)
                sb.Append("--frames " + xs.X264Frames + " ");
            if (xs.X264MinQuantizer != 10)
                sb.Append("--qpmin " + xs.X264MinQuantizer + " ");
            if (xs.X264MaxQuantizer != 51)
                sb.Append("--qpmax " + xs.X264MaxQuantizer + " ");
            if (xs.X264MaxQuantDelta != 4)
                sb.Append("--qpstep " + xs.X264MaxQuantDelta + " ");
            if (xs.X264QuantCompression != new decimal(0.6))
                sb.Append("--qcomp " + xs.X264QuantCompression.ToString(ci) + " ");
            if (xs.X264TempComplexityBlur != 20)
                sb.Append("--cplxblur " + xs.X264TempComplexityBlur.ToString(ci) + " ");
            if (xs.X264TempQuanBlurCC != new decimal(0.5))
                sb.Append("--qblur " + xs.X264TempQuanBlurCC.ToString(ci) + " ");
            if (xs.X264IPFactor != new decimal(1.4))
                sb.Append("--ipratio " + xs.X264IPFactor.ToString(ci) + " ");
            if (xs.X264PBFactor != new decimal(1.3))
                sb.Append("--pbratio " + xs.X264PBFactor.ToString(ci) + " ");
            if (!xs.CustomEncoderOptions.Equals("")) // add custom encoder options
                sb.Append(xs.CustomEncoderOptions);

            if (xs.Zones != null && xs.Zones.Length > 0 && xs.CreditsQuantizer >= new decimal(1))
            {
                sb.Append("--zones \"");
                foreach (Zone zone in xs.Zones)
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
                sb.Append("\" ");
            }

            if (!xs.Logfile.Equals(string.Empty))
                sb.Append("--logfile " + "\"" + xs.Logfile + "\" ");

            return sb.ToString();
        }

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob &&
                (j as VideoJob).Settings is x264farmSettings)
            {
                return new x264farmEncoder(mf.Settings.X264farmControllerPath);
            }
            return null;
        }

        public x264farmEncoder(string encoderPath)
            : base()
        {
            executable = encoderPath;
        }

        //WorkAround for deafultProgressWindow
        private int framesCounter = 0;
        private int framesCounterMod = 0;
        private int currentPass = 0;
        private bool errorsStart = false;
        private bool doneErrorCount = false;
        private int errorsCount = 0;
        private int errorCurrent = 0;
        private string lastError = string.Empty;

        public override string GetFrameString(string line, StreamType stream)
        {
            if (line.IndexOf("FIRST PASS") != -1)
            {
                currentPass = 1;
            }
            if (line.IndexOf("SECOND PASS") != -1)
            {
                currentPass = 2;
            }
            if ((line.IndexOf("% done (") != -1) && (currentPass > 0)) // status update
            {
                int frameNumberStart = line.IndexOf("(", 4) + 1;
                int frameNumberEnd = line.IndexOf(" /");
                framesCounter = int.Parse(line.Substring(frameNumberStart, frameNumberEnd - frameNumberStart).Trim());
                framesCounterMod =
                    (int)decimal.Floor((Math.Abs(currentPass - 1) * (int)su.NbFramesTotal + framesCounter) / 2);
            }

            return framesCounterMod.ToString();
        }

        public override string GetErrorString(string line, StreamType stream)
        {
            //SPW
            // 0% done (0 / 4197) at 0.00 FPS                 
            //Recent errors:                                  
            // ~                                              
            // ~                                              
            // ~                                              
            // ~                                              
            if ((line.IndexOf("% done (") != -1) && (errorsCount != 0))
            {
                errorsStart = false;
                if (!doneErrorCount)
                {
                    doneErrorCount = true;
                }
                return null;
            }
            if (line.IndexOf("Recent errors:") != -1)
            {
                errorsStart = true;
                errorCurrent = 0;
                return null;
            }
            if (errorsStart)
            {
                errorCurrent++;
                if (!doneErrorCount)
                {
                    errorsCount++;
                    lastError = line;
                    if (!string.IsNullOrEmpty(line) && !(line.IndexOf("~") > -1))
                    {
                        log.AppendLine(line);
                    }
                }
                else
                {
                    if ((errorCurrent == errorsCount) && !string.IsNullOrEmpty(line) && (lastError != line)
                        && !(line.IndexOf("~") > -1))
                    {
                        lastError = line;
                        log.AppendLine(line);
                    }
                }
                return null;
            }
            if ((line.IndexOf("WARNING") != -1) ||
                (line.IndexOf("ERROR") != -1))
            {
                return line;
            }
            return null;
        }
    }
}