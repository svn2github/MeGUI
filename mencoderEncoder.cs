using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MeGUI
{
    class mencoderEncoder : CommandlineVideoEncoder
    {
        public mencoderEncoder(string executablePath)
        {
            this.executable = executablePath;
        }
        #region IVideoEncoder Members
        public override bool start(out string error)
        {
            error = null;
            base.start(out error); // always return true so we don't check the return value
            this.EncoderOutputReceived += new EncoderOutputCallback(mencoderEncoder_EncoderOutputReceived);
            try
            {
                bool started = proc.Start();
                new System.Windows.Forms.MethodInvoker(this.readStdOut).BeginInvoke(null, null);
                new System.Windows.Forms.MethodInvoker(this.readStdErr).BeginInvoke(null, null);
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
        void mencoderEncoder_EncoderOutputReceived(string line, int type)
        {
            switch (type)
            {
                case 0:
                    if (line.StartsWith("Pos:")) // status update
                    {
                        su.NbFramesDone = getFrameNumber(line);
                        if (su.NbFramesDone >= lastStatusUpdateFramePosition + 10)
                        {
                            su.FPS = this.getFPS(line);
                            base.sendStatusUpdate(su); // sends statusupdate to GUI
                            lastStatusUpdateFramePosition = su.NbFramesDone;
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
        /// <summary>
        /// gets the encoding speed from an mencoder status update line
        /// </summary>
        /// <param name="line">mencoder stdout line</param>
        /// <returns>fps included in the line</returns>
        public double getFPS(string line)
        {
           try
           {
               System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-us");
               int fpsStart = line.IndexOf("%)") + 2;
               int fpsEnd = line.IndexOf("fps");
               string fps = line.Substring(fpsStart, fpsEnd - fpsStart).Trim();
               return Double.Parse(fps);
            }
            catch (Exception e)
           {
               log.Append("Exception in getFPS(" + line + ") " + e.Message);
               return 0.0;
           }
        }
        #endregion
    }
}
