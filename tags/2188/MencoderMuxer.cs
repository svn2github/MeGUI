using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    class MencoderMuxer : CommandlineMuxer
    {
        int counter;
        public MencoderMuxer(string executablePath)
        {
            this.executable = executablePath;
            counter = 0;
        }

        public override bool start(out string error)
        {
            error = null;
            base.start(out error); // always return true so we don't check the return value
            this.MuxerOutputReceived += new MuxerOutputCallback(MencoderMuxer_MuxerOutputReceived);
            try
            {
                bool started = proc.Start();
                new MethodInvoker(this.readStdOut).BeginInvoke(null, null);
                new MethodInvoker(this.readStdErr).BeginInvoke(null, null);
                this.changePriority(job.Priority, out error);
                return true;
            }
            catch (Exception e)
            {
                error = "Exception starting the process: " + e.Message;
                return false;
            }
        }

        void MencoderMuxer_MuxerOutputReceived(string line, int type)
        {
            switch (type)
            {
                case 0:
                    if (line.StartsWith("Pos:")) // status update
                    {
                        counter++;
                        if (counter == 10)
                        {
                            double percentage = 0;
                            su.NbFramesDone = getFrameNumber(line);
                            if (job.NbOfFrames == 0)
                            {
                                if (System.IO.File.Exists(job.Output))
                                {
                                    System.IO.FileInfo fi = new System.IO.FileInfo(job.Output);
                                    int currentSize = (int)(fi.Length / 1024);
                                    percentage = (double)100 / (double)su.ProjectedFileSize * (double)currentSize;
                                }
                            }
                            else
                                percentage = (double)100 / (double)job.NbOfFrames * (double)su.NbFramesDone;
                            if (percentage > 100)
                                percentage = 100; // to compensate for errors
                            su.PercentageDoneExact = percentage;
                            su.TimeElapsed = DateTime.Now.Ticks - job.Start.Ticks;
                            su.FileSize = (long)(videoSize * percentage / 100) / 1024;
                            su.AudioFileSize = (long)(audioSize1 * percentage / 100) / 1024;
                            base.sendStatusUpdateToGUI(su);
                            counter = 0;
                        }
                    }
                    else if (line.IndexOf("error") != -1)
                    {
                        log.Append(line + "\r\n");
                        su.HasError = true;
                    }
                    else
                        log.Append(line + "\r\n");
                    break;
                case 1:
                    if (line.IndexOf("not an MEncoder option") != -1)
                    {
                        log.Append("Error: Unrecognized commandline parameter detected. \r\n");
                        su.Error = line;
                        su.HasError = true;
                    }
                    else
                        log.Append(line + "\r\n");
                    break;
                case 2:
                    log.Append(line + "\r\n");
                    break;
            }
        }

        /// <summary>
        /// gets the framenumber from an mencoder status update line
        /// </summary>
        /// <param name="line">mencoder stdout line</param>
        /// <returns>the framenumber included in the line</returns>
        public int getFrameNumber(string line)
        {
            try
            {
                int frameNumberStart = line.IndexOf("s", 4) + 1;
                int frameNumberEnd = line.IndexOf("f");
                string frameNumber = line.Substring(frameNumberStart, frameNumberEnd - frameNumberStart).Trim();
                return Int32.Parse(frameNumber);
            }
            catch (Exception e)
            {
                log.Append("Exception in getFrameNumber(" + line + ") " + e.Message);
                return 0;
            }
        }
    }
}