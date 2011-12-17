// ****************************************************************************
// 
// Copyright (C) 2005-2011  Doom9 & al
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
using System.Text;
using System.Text.RegularExpressions;

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI
{
    public class HDStreamExtractorIndexer: CommandlineJobProcessor<HDStreamsExJob>
    {
        public static readonly JobProcessorFactory Factory =
            new JobProcessorFactory(new ProcessorFactory(init), "HDStreamExtIndexer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is HDStreamsExJob) return new HDStreamExtractorIndexer(mf.Settings.EAC3toPath);
            return null;
        }

        private string lastLine;
        private bool bSecondPass;

        public HDStreamExtractorIndexer(string executablePath)
        {
            this.executable = executablePath;
            lastLine = "";
        }

        /// <summary>
        /// gets the completion percentage
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private decimal? getPercentage(string line)
        {
            try
            {
                if (Regex.IsMatch(line, "^process: [0-9]{1,3}%$", RegexOptions.Compiled))
                {
                    int pct = Int32.Parse(Regex.Match(line, "[0-9]{1,3}").Value);
                    return pct;
                }
                else if (Regex.IsMatch(line, "^analyze: [0-9]{1,3}%$", RegexOptions.Compiled))
                {
                    int pct = Int32.Parse(Regex.Match(line, "[0-9]{1,3}").Value);
                    return pct;
                }
                else return null;
            }
            catch (Exception e)
            {
                log.LogValue("Exception in getPercentage(" + line + ") ", e, ImageType.Warning);
                return null;
            }
        }

        protected override void checkJobIO()
        {
            // do nothing
        }

        public override void ProcessLine(string line, StreamType stream)
        {
            if (line.StartsWith("process: ")) //status update
            {
                su.PercentageDoneExact = getPercentage(line);
                if (bSecondPass)
                    su.Status = "Fixing audio gaps/overlaps...";
                else
                    su.Status = "Extracting Tracks...";
            }
            else if (line.StartsWith("analyze: "))
            {
                su.PercentageDoneExact = getPercentage(line);
                su.Status = "Analyzing...";
            }
            else if (line.ToLower().Contains("2nd"))
            {
                startTime = DateTime.Now;
                su.TimeElapsed = TimeSpan.Zero;
                bSecondPass = true;
                su.Status = "Fixing audio gaps/overlaps...";
            }
            else if (line.ToLower().Contains("error"))
            {
                log.LogValue("An error occurred", line, ImageType.Error);
                su.HasError = true;
                base.ProcessLine(line, stream);
            }
            else if (line.ToLower().Contains("without making use of the gap/overlap information"))
            {
                log.LogEvent("Job will be executed a second time to make use of the gap/overlap information");
                base.bRunSecondTime = true;
                base.ProcessLine(line, stream);
            }
            else if (line.ToLower().Contains("warning") ||
                     line.ToLower().Contains("without making use of the gap/overlap information") ||
                     line.ToLower().Contains("doesn") ||
                     (su.PercentageDoneExact > 0 && su.PercentageDoneExact < 100 &&
                      !line.ToLower().Contains("creating file ")))
            {
                log.LogValue("A warning occurred", line, ImageType.Warning);
                base.ProcessLine(line, stream);
            }
            else
                base.ProcessLine(line, stream);
            
            lastLine = line;
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (job.InputType == 1) // Folder as Input
                {
                    if (job.Input.IndexOf("BDMV") > 0 && (job.Input.ToLower().EndsWith(".m2ts") || job.Input.ToLower().EndsWith(".mpls")))
                        sb.Append(string.Format("\"{0}\" {1}) {2}", job.Input.Substring(0, job.Input.IndexOf("BDMV")), job.FeatureNb, job.Args + " -progressnumbers"));
                    else if (job.Input.ToLower().EndsWith(".evo"))
                        sb.Append(string.Format("\"{0}\" {1}) {2}", job.Input.Substring(0, job.Input.IndexOf("HVDVD_TS")), job.FeatureNb, job.Args + " -progressnumbers"));
                    else
                        sb.Append(string.Format("\"{0}\" {1}) {2}", job.Input, job.FeatureNb, job.Args + " -progressnumbers"));
                }
                else
                    sb.Append(string.Format("\"{0}\" {1}", job.Input, job.Args + " -progressnumbers"));

                return sb.ToString();
            }
        }
    }
}
