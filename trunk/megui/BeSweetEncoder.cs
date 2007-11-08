using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms; // used for the MethodInvoker

namespace MeGUI
{
    /*
    class BeSweetEncoder : CommandlineAudioEncoder
    {
        bool firstpassStarted, secondpassStarted;
        int counter;
        string lastPos;
        long duration;
        public BeSweetEncoder(string executablePath)
        {
            this.executable = executablePath;
            firstpassStarted = false;
            secondpassStarted = false;
            lastPos = "";
            duration = 0;
        }
        public override bool start(out string error)
        {
            error = null;
            base.start(out error); // always return true so we don't check the return value
            this.EncoderOutputReceived += new AudioEncoderOutputCallback(BeSweetEncoder_EncoderOutputReceived);
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

        void BeSweetEncoder_EncoderOutputReceived(string line, int type)
        {
            switch (type)
            {
                case 0:
                    log.Append(line + "\r\n");
                    break;
                case 1: // stderr
                    if (line.IndexOf("Error") != -1)
                    {
                        su.HasError = true;
                        su.Error = line;
                    }
                    if (line.IndexOf("Maximum") == -1 && line.IndexOf("transcoding") == -1) // neither first nor 2nd pass line
                    {
                        log.Append(line + "\r\n");
                        if (firstpassStarted && !secondpassStarted && line.StartsWith("[")) // we're in the middle of passes, get latest timecode
                        {
                            duration = getDuration(line);
                            secondpassStarted = true;
                        }
                    }
                    if (line.IndexOf("Maximum Gain Found") != -1) // a first pass line
                    {
                        firstpassStarted = true;
                        lastPos = line;
                        counter++;
                    }
                    if (line.IndexOf("transcoding") != -1) // not a second pass line
                    {
                        counter++;
                    }
                    if (counter == 50)
                    {
                        counter = 0;
                        su.TimeElapsed = DateTime.Now.Ticks - job.Start.Ticks;
                        su.AudioPosition = line.Substring(1, line.IndexOf("]") - 1);
                        su.PercentageDoneExact = getPercentage(line);
                        sendStatusUpdate(su);
                    }
                    break;
                case 2:
                    log.Append(line + "\r\n");
                    break;
            }
        }

        double getPercentage(string line)
        {
            long position = getDuration(line);
            if (duration == 0)
                return 0;
            else
            {
                double div = (double)position / (double)duration;
                double percentage = ((double)100 * div);
                if (percentage > 100)
                    return 100;
                else
                    return percentage;
            }
        }
        long getDuration(string line)
        {
            try
            {
                string timecode = line.Substring(1, line.IndexOf("]") - 1);
                int hh = Int32.Parse(timecode.Substring(0, 2));
                int mm = Int32.Parse(timecode.Substring(3, 2));
                int ss = Int32.Parse(timecode.Substring(6, 2));
                int cc = Int32.Parse(timecode.Substring(9, 3));
                TimeSpan pos = TimeSpan.FromMilliseconds(cc + ss * 1000 + mm * 60 * 1000 + hh * 60 * 60 * 1000);
                if (pos.Ticks < 0)
                    pos = new TimeSpan(0);
                return pos.Ticks;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
    */
}