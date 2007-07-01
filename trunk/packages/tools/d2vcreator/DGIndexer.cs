// ****************************************************************************
// 
// Copyright (C) 2005  Doom9
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
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using System.IO;

namespace MeGUI
{
    public class DGIndexer : CommandlineJobProcessor<IndexJob>
    {
        public static readonly JobProcessorFactory Factory =
            new JobProcessorFactory(new ProcessorFactory(init), "DGIndexer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is IndexJob) return new DGIndexer(mf.Settings.DgIndexPath);
            return null;
        }
        protected override bool checkExitCode()
        {
            return true;
        }
        public DGIndexer(string executableName)
            : base()
        {
            executable = executableName;
        }
        
        protected override void doExitConfig()
        {
            if (job.AutoForceFilm && !su.HasError && !su.WasAborted )
            {
                double filmPercent;
                try
                {
                    filmPercent = d2vFile.GetFilmPercent(job.Output);
                }
                catch (Exception error)
                {
                    su.Error = "Applying force film failed. Consult the log for details";
                    log.AppendFormat("Cannot open d2v file, '{0}'. Error message for your reference: {1}{2}", job.Output, error.Message, Environment.NewLine);
                    log.AppendLine("Applying force film failed.");
                    su.HasError = true;
                    return;
                }
                if (!su.HasError)
                {
                    if (job.ForceFilmThreshold <= filmPercent)
                    {
                        log.Append("Film percentage: " + filmPercent + " meets force film threshold");
                        bool success = applyForceFilm(job.Output);
                        if (success)
                            log.Append("Successfully applied force film");
                        else
                        {
                            su.Error = "Applying force film failed";
                            log.Append("Applying force film failed");
                            su.HasError = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// opens a DGIndex project file and applies force film to it
        /// </summary>
        /// <param name="fileName">the dgindex project where force film is to be applied</param>
        private bool applyForceFilm(string fileName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.IndexOf("Field_Operation") != -1) // this is the line we have to replace
                            sb.Append("Field_Operation=1" + Environment.NewLine);
                        else if (line.IndexOf("Frame_Rate") != -1)
                        {
                            if (line.IndexOf("/") != -1) // If it has a slash, it means the framerate is signalled as a fraction, like below
                                sb.Append("Frame_Rate=23976 (24000/1001)" + Environment.NewLine);
                            else // If it doesn't, then it doesn't
                                sb.Append("Frame_Rate=23976" + Environment.NewLine);
                        }
                        else
                        {
                            sb.Append(line);
                            sb.Append(Environment.NewLine);
                        }
                    }
                }
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write(sb.ToString());
                }
                return true;
            }
            catch (Exception e)
            {
                log.Append("Exception in applyForceFilm: " + e.Message);
                return false;
            }
        }

        public override void ProcessLine(string line, int stream)
        {
            // We don't expect any lines, but lets put them on the log just in case
            log.AppendLine(line);
        }
    }
}