using System;
using System.Collections.Generic;
using System.Windows.Forms; // used for the MethodInvoker
using System.Text;
using System.IO;

namespace MeGUI
{
    
    public class XviDEncoder: CommandlineVideoEncoder
    {
        private bool ignoreFurtherLines;
        private long lastFPSUpdateTime; // time we compiled the last fps number
        private int lastFPSUpdateFrame = 0;

        public XviDEncoder(string executablePath)
        {
            usesSAR = true;
            this.executable = executablePath;
            ignoreFurtherLines = false;
        }
        #region IVideoEncoder Members
        public override bool start(out string error)
        {
            error = null;
            base.start(out error); // always return true so we don't check the return value
            this.EncoderOutputReceived += new EncoderOutputCallback(XviDEncoder_EncoderOutputReceived);
            try
            {
                bool started = proc.Start();
                lastFPSUpdateTime = DateTime.Now.Ticks;
                new MethodInvoker(this.readStdOut).BeginInvoke(null, null);
                new MethodInvoker(this.readStdErr).BeginInvoke(null, null);
                this.changePriority(job.Priority, out error);
                return true;
            }
            catch (Exception e)
            {
                error = "Exception starting the encoder: " + e.Message;
                return false;
            }
        }
        #endregion
        #region line processing
        /// <summary>
        /// processes a line received from the xvid_encraw commandline encoder
        /// </summary>
        /// <param name="line">the line to be looked at</param>
        /// <param name="type">where the line is from, 0 = stdout, 1 = stderr, 2 = megui internal</param>
        void XviDEncoder_EncoderOutputReceived(string line, int type)
        {
                switch (type)
                {
                    case 0: // stdout
                        if (line.IndexOf(": key") != -1) // we found a position line, parse it
                        {
                            int nbFramesEncoded = getXviDFrameNumber(line);
                            if (nbFramesEncoded > su.NbFramesDone) // workaround for the two -1 lines at the end of encoding
                                su.NbFramesDone = nbFramesEncoded;
                            if (su.NbFramesDone >= lastStatusUpdateFramePosition + 10)
                            {
                                long now = DateTime.Now.Ticks;
                                if (su.NbFramesDone >= lastFPSUpdateFrame + 100) // after 100 frames, recalculate the FPS
                                {
                                    TimeSpan ts = TimeSpan.FromTicks(now - lastFPSUpdateTime); // time elapsed since the last 100 frames
                                    su.FPS = (double)(su.NbFramesDone - lastFPSUpdateFrame) / ts.TotalSeconds;
                                    lastFPSUpdateFrame = su.NbFramesDone;
                                    lastFPSUpdateTime = now;
                                }
                                base.sendStatusUpdate(su); // little trick since we can't send the update from derived class
                                lastStatusUpdateFramePosition = su.NbFramesDone;
                            }
                        }
                        else
                            log.Append(line + "\r\n");
                        break;
                    case 1: // stderr
                        if (!ignoreFurtherLines)
                        {
                            if (line.IndexOf("Usage") != -1) // we get the usage message if there's an unrecognized parameter
                            {
                                log.Append("Error: Unrecognized commandline parameter detected.\r\n");
                                su.Error = "Error: Unrecognized commandline parameter detected.";
                                su.HasError = true;
                                this.ignoreFurtherLines = true;
                            }
                            else
                                log.Append(line + "\r\n");
                        }
                        break;
                    case 2: // internal errors
                        log.Append(line + "\r\n");
                        break;
            }
        }
        /// <summary>
        /// analyzes an xvid_encraw stdout line and gets the frame number from it
        /// </summary>
        /// <param name="line">the line to be analyzed</param>
        /// <returns>the frame number</returns>
        private int getXviDFrameNumber(string line)
        {
            int frameNumberEnd = line.IndexOf(":");
            try
            {
                string frameNumber = line.Substring(0, frameNumberEnd).Trim();
                return Int32.Parse(frameNumber);
            }
            catch (Exception e)
            {
                log.Append("Error in getXviDFrameNumber(" + line + ") : " + e.Message + " \r\n");
                return 0;
            }
        }
        #endregion
    }
}
