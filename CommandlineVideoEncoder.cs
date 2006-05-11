using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace MeGUI
{
    public delegate void EncoderOutputCallback(string line, int type);

    public class CommandlineVideoEncoder : VideoEncoder
    {
        #region variables
        protected Process proc; // the encoder process
        protected int lastStatusUpdateFramePosition = 0;
        protected string executable; // path and filename of the commandline encoder to be used
        protected System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(true); // lock used to pause encoding
        protected System.Threading.ManualResetEvent stdoutDone = new System.Threading.ManualResetEvent(false);
        protected System.Threading.ManualResetEvent stderrDone = new System.Threading.ManualResetEvent(false);
        public event EncoderOutputCallback EncoderOutputReceived;
        private int hres = 0, vres = 0, darX, darY;
        protected bool usesSAR = false;
        #endregion
        public CommandlineVideoEncoder()
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
        /// tries to open the video source and gets the number of frames from it, or 
        /// exits with an error
        /// </summary>
        /// <param name="videoSource">the AviSynth script</param>
        /// <param name="error">return parameter for all errors</param>
        /// <returns>true if the file could be opened, false if not</returns>
        protected bool getInputProperties(string videoSource, out string error)
        {
            double f;
            error = JobUtil.GetAllInputProperties( out numberOfFrames, out f, out hres, out vres, out darX, out darY, videoSource);
            return error == null;
        }
        /// <summary>
        /// checks if the encoder path given to the encoder actually exists
        /// </summary>
        /// <param name="executableName">path and name of the encoder executable</param>
        /// <param name="error">return string for errors</param>
        /// <returns>true if the encoder is there, false if not</returns>
        protected bool checkEncoderExistence(string executableName, out string error)
        {
            error = null;
            if (!File.Exists(executable))
            {
                error = "Could not find " + executableName + " in the path specified: " + executable 
                    + " Please specify the proper path in the settings";
                return false;
            }
            return true;
        }
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
            if (proc.ExitCode != 0) // check the exitcode because x264.exe sometimes exits with error but without
                su.HasError = true; // any commandline indication as to why
            job.End = DateTime.Now;
            su.IsComplete = true;
            TimeSpan ts = TimeSpan.FromTicks(job.End.Ticks - job.Start.Ticks);
            double seconds = ts.TotalSeconds;
            if (seconds > 0)
                su.FPS = (double)su.NbFramesDone / seconds;
            else
                su.FPS = 0;
            if (!su.HasError && !su.WasAborted)
                compileFinalStats();
            su.Log = log.ToString();
            base.sendStatusUpdateToGUI(su);
        }
        /// <summary>
        /// compiles final bitrate statistics
        /// </summary>
        private void compileFinalStats()
        {
            try
            {
                if (File.Exists(job.Output))
                {
                    FileInfo fi = new FileInfo(job.Output);
                    long size = fi.Length; // size in bytes
                    double numberOfSeconds = job.NumberOfFrames / job.Framerate;
                    double bitrate = (double)(size * 8) / (numberOfSeconds * (double)1000);
                    if (job.Settings.EncodingMode != 1)
                        log.Append("desired video bitrate of this job: " + job.Settings.BitrateQuantizer + " kbit/s - obtained video bitrate: " + bitrate + " kbit/s");
                    else
                        log.Append("This is a CQ job so there's no desired bitrate. Obtained video bitrate: " + bitrate + " kbit/s");
                }
            }
            catch (Exception e)
            {
                log.Append("Exception in compileFinalStats. Message: " + e.Message + " stacktrace: " + e.StackTrace);
            }
        }
        #endregion
        #region IVideoEncoder overridden Members

        public override bool setup(VideoJob job, out string error)
        {
            error = null;
            // This enables relative paths, etc
            executable = Path.Combine(System.Windows.Forms.Application.StartupPath, executable);
            if (!checkEncoderExistence(executable, out error))
            {
                return false;
            }
            executable = "\"" + executable + "\"";
            bool inputOK = getInputProperties(job.Input, out error);
            if (!inputOK)
                return false;
            
            // Automatically set AR signalling based on source AR, unless some other AR is already present in codec settings:
            if ((job.PARX < 1) || (job.PARY < 1))
            {
                job.PARX = darX;
                job.PARY = darY;
            }
            if (usesSAR)
            {
                int sarX, sarY;
                VideoUtil.findSAR(job.PARX, job.PARY, hres, vres, out sarX, out sarY);
                job.Commandline = CommandLineGenerator.generateVideoCommandline(job.Settings, job.Input, job.Output, sarX, sarY);
            }
            this.job = job;
            su = new StatusUpdate();
            su.FPS = 0;
            su.JobName = job.Name;
            su.NbFramesTotal = this.numberOfFrames;
            su.JobType = JobTypes.VIDEO;
            log = new StringBuilder();
            return true;
        }

        public override bool start(out string error)
        {
            error = null;
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
            return true;
        }

        public override bool stop(out string error)
        {
            error = null;
            if (proc != null && !proc.HasExited)
            {
                try
                {
                    su.WasAborted = true;
                    proc.Kill();
                    return true;
                }
                catch (Exception e)
                {
                    error = "Error killing process: " + e.Message;
                    return false;
                }
            }
            else
            {
                if (proc == null)
                    error = "Encoder process does not exist";
                else
                    error = "Encoder process has already existed";
                return false;
            }
        }

        public override bool pause(out string error)
        {
            error = null;
            if (mre.Reset())
                return true;
            else
            {
                error = "Could not reset mutex. pause failed";
                return false;
            }
        }

        public override bool resume(out string error)
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
}
