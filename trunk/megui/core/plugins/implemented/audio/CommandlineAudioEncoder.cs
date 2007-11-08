/*using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using MeGUI.core.util;

namespace MeGUI
{
    public delegate void AudioEncoderOutputCallback(string line, int type);

    class CommandlineAudioEncoder : AudioEncoder
    {
        protected Process proc; // the encoder process
        protected string executable; // path and filename of the commandline encoder to be used
        protected System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(true); // lock used to pause encoding
        protected System.Threading.ManualResetEvent stdoutDone = new System.Threading.ManualResetEvent(false);
        protected System.Threading.ManualResetEvent stderrDone = new System.Threading.ManualResetEvent(false);
        public event AudioEncoderOutputCallback EncoderOutputReceived;
        public CommandlineAudioEncoder()
        {
        }
        #region input processing
        protected void readStdOut()
        {
            StreamReader sr = null;
            try
            {
                sr = proc.StandardOutput;
            }
            catch (Exception e)
            {
                log.Append("exception getting io reader for stdout: " + e.Message + "\r\nAborting CommandlineVideoEncoder.readStdOut");
                return;
            }
            string line;
            if (proc != null)
            {
                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        mre.WaitOne();
                        EncoderOutputReceived(line, 0);
                    }
                }
                catch (Exception e)
                {
                    EncoderOutputReceived("Exception in readStdOut: " + e.Message, 2);
                }
                stdoutDone.Set();
            }
        }
        protected void readStdErr()
        {
            StreamReader sr = null;
            try
            {
                sr = proc.StandardError;
            }
            catch (Exception e)
            {
                log.Append("exception getting io reader for stderr: " + e.Message + "\r\nAborting CommandlineVideoEncoder.readStdErr");
                return;
            }
            string line;
            if (proc != null)
            {
                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        mre.WaitOne();
                        EncoderOutputReceived(line, 1);
                    }
                }
                catch (Exception e)
                {
                    EncoderOutputReceived("Exception in readStdErr: " + e.Message, 2);
                }
                stderrDone.Set();
            }
        }
        #endregion
        #region helper methods
        /// <summary>
        /// sends a status update up the chain where it will be thrown as an event
        /// </summary>
        /// <param name="su">the status update object to be send back to the GUI</param>
        protected void sendStatusUpdate(StatusUpdate su)
        {
            base.sendStatusUpdateToGUI(su);
        }
        /// <summary>
        /// handles the encoder process existing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void proc_Exited(object sender, EventArgs e)
        {
            stdoutDone.WaitOne(); // wait for stdout to finish processing
            stderrDone.WaitOne(); // wait for stderr to finish processing
            job.End = DateTime.Now;
            su.IsComplete = true;
            su.Log = log.ToString();
            base.sendStatusUpdateToGUI(su);
        }
        #endregion
        #region IJobProcessor overridden members

        public override void setup(Job job)
        {
            Util.ensureExists(executable);
            
            executable = "\"" + executable + "\"";
            this.job = job;
            su = new StatusUpdate();
            su.FPS = 0;
            su.JobName = job.Name;
            su.JobType = JobTypes.AUDIO;
            log = new StringBuilder();
        }

        public override void start()
        {
            proc = new Process();
            ProcessStartInfo pstart = new ProcessStartInfo();
            pstart.FileName = executable;
            pstart.Arguments = job.Commandline;
            pstart.RedirectStandardOutput = true;
            pstart.RedirectStandardError = true;
            pstart.CreateNoWindow = true;
            pstart.UseShellExecute = false;
            proc.StartInfo = pstart;
            proc.EnableRaisingEvents = true;
            proc.Exited += new EventHandler(proc_Exited);
        }

        public override void stop()
        {
            if (proc != null && !proc.HasExited)
            {
                try
                {
                    su.WasAborted = true;
                    proc.Kill();
                    return;
                }
                catch (Exception e)
                {
                    throw new JobRunException(e);
                }
            }
            else
            {
                if (proc == null)
                    throw new JobRunException("Encoder process does not exist");
                else
                    throw new JobRunException("Encoder process has already exited");
            }
        }

        public override void pause()
        {
            if (mre.Reset())
                return;
            else
            {
                throw new JobRunException("Could not reset mutex. pause failed")
            }
        }

        public override void resume()
        {
            error = null;
            if (mre.Set())
                return true;
            else
            {
                error = "Could not set mutex. pause failed";
                return false;
            }
        }

        public override bool changePriority(ProcessPriority priority, out string error)
        {
            error = null;
            if (proc != null && !proc.HasExited)
            {
                try
                {
                    if (priority == ProcessPriority.IDLE)
                        proc.PriorityClass = ProcessPriorityClass.Idle;
                    else if (priority == ProcessPriority.NORMAL)
                        proc.PriorityClass = ProcessPriorityClass.Normal;
                    else if (priority == ProcessPriority.HIGH)
                        proc.PriorityClass = ProcessPriorityClass.High;
                    return true;
                }
                catch (Exception e) // process could not be running anymore
                {
                    error = "exception in changeProcessPriority: " + e.Message;
                    return false;
                }
            }
            else
            {
                if (proc == null)
                    error = "Process has not been started yet";
                else
                    error = "Process has exited";
                return false;
            }
        }

        #endregion
    }
}*/