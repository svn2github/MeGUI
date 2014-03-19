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
            if (j is HDStreamsExJob) 
                return new HDStreamExtractorIndexer(mf.Settings.Eac3to.Path);
            return null;
        }

        private bool bSecondPass;

        public HDStreamExtractorIndexer(string executablePath)
        {
            UpdateCacher.CheckPackage("eac3to");
            this.executable = executablePath;
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
                else 
                    return null;
            }
            catch (Exception e)
            {
                log.LogValue("Exception in getPercentage(" + line + ") ", e, ImageType.Warning);
                return null;
            }
        }

        protected override void checkJobIO()
        {
            foreach (string strSource in job.Source)
                Util.ensureExists(strSource);
        }

        /// <summary>
        /// search for the log file and add error messages to the stdoutLog
        /// required as the error lines in stderr or stdout are not readable
        /// </summary>
        protected override void getErrorLine()
        {
            string[] entry = Regex.Split(job.Args, "[0-9]{1,3}:\\\"");
            if (entry.Length < 2 || entry[1].Length < 3)
                return;

            string fileName = entry[1].Substring(0, entry[1].Length - 2);
            fileName = FileUtil.AddToFileName(System.IO.Path.ChangeExtension(fileName, "txt"), " - Log");
            if (!System.IO.File.Exists(fileName))
                return;

            using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (line.ToLowerInvariant().Contains("<error>"))
                        stdoutLog.LogEvent(line, ImageType.Error);
                }
            }
        }

        public override void ProcessLine(string line, StreamType stream, ImageType oType)
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
            else if (line.ToLowerInvariant().Contains("2nd"))
            {
                startTime = DateTime.Now;
                su.TimeElapsed = TimeSpan.Zero;
                bSecondPass = true;
                su.Status = "Fixing audio gaps/overlaps...";
                base.ProcessLine(line, stream, oType);
            }
            else if (line.ToLowerInvariant().Contains("without making use of the gap/overlap information"))
            {
                log.LogEvent("Job will be executed a second time to make use of the gap/overlap information");
                base.bRunSecondTime = true;
                base.ProcessLine(line, stream, oType);
            }
            else if (line.ToLowerInvariant().Contains("<error>"))
            {
                base.ProcessLine(line, stream, ImageType.Error);
            }
            else if (line.ToLowerInvariant().Contains("<warning>")
                || (su.PercentageDoneExact > 0 && su.PercentageDoneExact < 100
                && !line.ToLowerInvariant().Contains("creating file ") 
                && !line.ToLowerInvariant().Contains("(seamless branching)...")))
            {
                base.ProcessLine(line, stream, ImageType.Warning);
            }
            else
                base.ProcessLine(line, stream, oType);
        }

        protected override string Commandline
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (job.InputType == 1) // Folder as Input
                {
                    if (job.Input.IndexOf("BDMV") > 0 && (job.Input.ToLowerInvariant().EndsWith(".m2ts") || job.Input.ToLowerInvariant().EndsWith(".mpls")))
                        sb.Append(string.Format("\"{0}\" {1}) {2}", job.Input.Substring(0, job.Input.IndexOf("BDMV")), job.FeatureNb, job.Args.Trim() + " -progressnumbers"));
                    else if (job.Input.ToLowerInvariant().EndsWith(".evo"))
                        sb.Append(string.Format("\"{0}\" {1}) {2}", job.Input.Substring(0, job.Input.IndexOf("HVDVD_TS")), job.FeatureNb, job.Args.Trim() + " -progressnumbers"));
                    else
                        sb.Append(string.Format("\"{0}\" {1}) {2}", job.Input, job.FeatureNb, job.Args.Trim() + " -progressnumbers"));
                }
                else
                {
                    if (job.Source.Count == 0 && !String.IsNullOrEmpty(job.Input))
                        job.Source.Add(job.Input);

                    string strSource = string.Format("\"{0}\"", job.Source[0]);
                    for (int i = 1; i < job.Source.Count; i++)
                        strSource += string.Format("+\"{0}\"", job.Source[i]);
                    sb.Append(string.Format("{0} {1}", strSource, job.Args.Trim() + " -progressnumbers"));
                }

                return sb.ToString();
            }
        }
    }
}