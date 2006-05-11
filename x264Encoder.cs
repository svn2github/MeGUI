using System;
using System.Collections.Generic;
using System.Windows.Forms; // used for the MethodInvoker
using System.Text;

namespace MeGUI
{
    class x264Encoder : CommandlineVideoEncoder
    {
        private bool ignoreFurtherLines;
        private string lastPos;

        public x264Encoder(string executablePath)
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
            this.EncoderOutputReceived += new EncoderOutputCallback(x264Encoder_EncoderOutputReceived);
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
                error = "Exception starting the encoder: " + e.Message;
                return false;
            }
        }
        #endregion
        #region line processing
        void x264Encoder_EncoderOutputReceived(string line, int type)
        {
            switch (type)
            {
                case 1: // stderr line
                    if (line.StartsWith("encoded frames:")) // status update
                    {
                        su.NbFramesDone = getFrameNumber(line);
                        if (su.NbFramesDone >= lastStatusUpdateFramePosition + 10) // send out status updates every 10 frames
                        {
                            su.FPS = getFPS(line);
                            base.sendStatusUpdate(su); // sends statusupdate to GUI
                            lastStatusUpdateFramePosition = su.NbFramesDone;
                        }
                        lastPos = line;
                    }
                    else if (line.IndexOf("frames,") != -1) // this is the 2nd last line from x264.exe indicating the actual video bitrate obtained
                        log.Append("Actual bitrate after encoding without container overhead: " + getBitrate(line) + "\r\n");
                    else
                        log.Append(line + "\r\n");
                    break;
                case 0:
                    if (!ignoreFurtherLines)
                    {
                        if (line.IndexOf("Syntax") != -1) // we get the usage message if there's an unrecognized parameter
                        {
                            log.Append("Error: Unrecognized commandline parameter detected. \r\n");
                            su.Error = "Error: Unrecognized commandline parameter detected.";
                            su.HasError = true;
                            this.ignoreFurtherLines = true;
                        }
                        else if (line.IndexOf("error") != -1 || line.IndexOf("could not open") != -1) // this normally signifies the encoder cannot handle the input file
                        {
                            log.Append(line + "\r\n");
                            su.Error = line;
                            su.HasError = true;
                        }
                        else
                            log.Append(line + "\r\n");
                    }
                    break; // stderr line
                case 2:
                    log.Append(line + "\r\n");
                    break;
            }
        }
        
        /// <summary>
        /// gets the bitrate from an x264.exe status update message
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string getBitrate(string line)
        {
            try
            {
                int bitrateStart = line.IndexOf("fps,") + 5;
                int bitrateEnd = line.IndexOf("kb/s") - 1;
                string bitrate = line.Substring(bitrateStart, bitrateEnd - bitrateStart);
                return bitrate;
            }
            catch (Exception e)
            {
                log.Append("Exception in getX264Bitrate(" + line + ") " + e.Message);
                return "";
            }
           
        }
        /// <summary>
        /// gets the framenumber from an x264.exe status update line
        /// </summary>
        /// <param name="line">x264 stdout line</param>
        /// <returns>the framenumber included in the line</returns>
        public int getFrameNumber(string line)
        {
            try
            {
                int frameNumberStart = line.IndexOf(":", 4) + 2;
                int frameNumberEnd = line.IndexOf("/");
                string frameNumber = line.Substring(frameNumberStart, frameNumberEnd - frameNumberStart).Trim();
                return Int32.Parse(frameNumber);
            }
            catch (Exception e)
            {
                log.Append("Exception in x264FrameNumber(" + line + ") " + e.Message);
                return 0;
            }
        }
        /// <summary>
        /// gets the encoding speed from an x264 status update line
        /// </summary>
        /// <param name="line">x264 stdout line</param>
        /// <returns>fps included in the line</returns>
        public double getFPS(string line)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-us");
                int fpsStart = line.IndexOf("%)") + 4;
                int fpsEnd = line.IndexOf("fps");
                string fps = line.Substring(fpsStart, fpsEnd - fpsStart).Trim();
                return Double.Parse(fps);
            }
            catch (Exception e)
            {
                log.Append("Exception in getX264FPS(" + line + ") " + e.Message);
                return 0.0;
            }
        }
        #endregion
    }
}
