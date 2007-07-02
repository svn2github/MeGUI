using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    class MkvMergeMuxer : CommandlineMuxer
    {
        public MkvMergeMuxer(string executablePath)
        {
            this.executable = executablePath;
        }


        #region line processing
        /// <summary>
        /// gets the framenumber from an mkvmerge status update line
        /// </summary>
        /// <param name="line">mkvmerge commandline output</param>
        /// <returns>the framenumber included in the line</returns>
        public decimal? getPercentage(string line)
        {
            try
            {
                int percentageStart = 10;
                int percentageEnd = line.IndexOf("%");
                string frameNumber = line.Substring(percentageStart, percentageEnd - percentageStart).Trim();
                return Int32.Parse(frameNumber);
            }
            catch (Exception e)
            {
                log.AppendLine("Exception in getPercentage(" + line + ") " + e.Message);
                return null;
            }
        }
        #endregion

        protected override bool checkExitCode()
        {
            return false;
        }

        public override void ProcessLine(string line, StreamType stream)
        {
            if (line.StartsWith("progress: ")) //status update
                su.PercentageDoneExact = getPercentage(line);
            else if (line.IndexOf("Error") != -1)
            {
                log.Append(line);
                su.HasError = true;
                su.Error = line;
            }
            else
                log.AppendLine(line);
        }
    }
}
