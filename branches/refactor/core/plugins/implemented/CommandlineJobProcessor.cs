using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MeGUI
{
    public abstract class CommandlineJobProcessor<TJob> : IJobProcessor
        where TJob : Job
    {
        #region variables
        protected TJob job;
        protected bool isProcessing = false;
        protected Process proc = new Process(); // the encoder process
        protected string executable; // path and filename of the commandline encoder to be used
        protected ManualResetEvent mre = new ManualResetEvent(true); // lock used to pause encoding
        protected ManualResetEvent stdoutDone = new ManualResetEvent(false);
        protected ManualResetEvent stderrDone = new ManualResetEvent(false);
        protected event JobProcessingStatusUpdateCallback statusUpdate;
        protected StatusUpdate su = new StatusUpdate();
        protected StringBuilder log = new StringBuilder();
        #endregion
        /// <summary>
        /// checks if the encoder path given to the encoder actually exists
        /// </summary>
        /// <param name="executableName">path and name of the encoder executable</param>
        /// <param name="error">return string for errors</param>
        /// <returns>true if the encoder is there, false if not</returns>
        protected bool checkEncoderExistence(string executableName, out string error)
        {
            error = null;
            if (!File.Exists(executableName))
            {
                error = "Could not find " + executableName + " in the path specified: " + executable
                    + " Please specify the proper path in the settings";
                return false;
            }
            return true;
        }

        protected virtual bool checkJobIO(TJob job, out string error)
        {
            error = null;
            if (!File.Exists(job.Input))
            {
                error = string.Format("Input file {0} does not exist.", job.Input);
                return false;
            }
            return true;
        }

        protected virtual void doExitConfig()
        { }

        /// <summary>
        /// handles the encoder process existing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void proc_Exited(object sender, EventArgs e)
        {
            mre.Set();  // Make sure nothing is waiting for pause to stop
            stdoutDone.WaitOne(); // wait for stdout to finish processing
            stderrDone.WaitOne(); // wait for stderr to finish processing
            if (proc.ExitCode != 0) // check the exitcode because x264.exe sometimes exits with error but without
                su.HasError = true; // any commandline indication as to why
            job.End = DateTime.Now;
            su.IsComplete = true;
            doExitConfig();
            su.Log = log.ToString();
            statusUpdate(su);
        }

        #region IVideoEncoder overridden Members

        public bool setup(Job job2, out string error)
        {
            if (!(job2 is TJob))
                throw new Exception("Job is the wrong type");
            TJob job = (TJob)job2;
            error = null;
            // This enables relative paths, etc
            executable = Path.Combine(System.Windows.Forms.Application.StartupPath, executable);
            if (!checkEncoderExistence(executable, out error))
                return false;

            if (!checkJobIO(job, out error))
                return false;
            
            this.job = job;
            su.JobName = job.Name;
            su.JobType = job.JobType;
            return true;
        }

        public bool start(out string error)
        {
            error = null;
            proc = new Process();
            ProcessStartInfo pstart = new ProcessStartInfo();
            pstart.FileName = executable;
            pstart.Arguments = job.Commandline;
            pstart.RedirectStandardOutput = true;
            pstart.RedirectStandardError = true;
            pstart.WindowStyle = ProcessWindowStyle.Minimized;
            pstart.CreateNoWindow = true;
            pstart.UseShellExecute = false;
            proc.StartInfo = pstart;
            proc.EnableRaisingEvents = true;
            proc.Exited += new EventHandler(proc_Exited);

            try
            {
                bool started = proc.Start();
                isProcessing = true;
                new Thread(new ThreadStart(readStdErr)).Start();
                new Thread(new ThreadStart(readStdOut)).Start();
                new System.Windows.Forms.MethodInvoker(this.RunStatusCycle).BeginInvoke(null, null);
                this.changePriority(job.Priority, out error);
                return true;
            }
            catch (Exception e)
            {
                error = "Exception starting the encoder: " + e.Message;
                return false;
            }
        }

        public bool stop(out string error)
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

        public bool pause(out string error)
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

        public bool resume(out string error)
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

        public bool changePriority(ProcessPriority priority, out string error)
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

        public virtual bool canBeProcessed(Job job)
        {
            return (job is TJob);
        }


        #endregion

        protected virtual void readStream(StreamReader sr, ManualResetEvent rEvent)
        {
            string line;
            if (proc != null)
            {
                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        mre.WaitOne();
                        ProcessLine(line, 1);
                    }
                }
                catch (Exception e)
                {
                    ProcessLine("Exception in readStdErr: " + e.Message, 2);
                }
                rEvent.Set();
            }
        }
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
                stdoutDone.Set();
                return;
            }
            readStream(sr, stdoutDone);
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
                stderrDone.Set();
                return;
            }
            readStream(sr, stderrDone);
        }
        public abstract void ProcessLine(string line, int stream);
        public event JobProcessingStatusUpdateCallback StatusUpdate
        {
            add { statusUpdate += value; }
            remove { statusUpdate -= value; }
        }

        protected void RunStatusCycle()
        {
            while (proc != null && !proc.HasExited)
            {
                su.TimeElapsed = DateTime.Now.Ticks - job.Start.Ticks;
                if (!string.IsNullOrEmpty(job.Output) && File.Exists(job.Output))
                {
                    try
                    {
                        FileInfo fi = new FileInfo(job.Output);
                        su.FileSize = fi.Length / 1024;
                    }
                    catch (Exception)
                    { }
                }
                doStatusCycleOverrides();
                if (statusUpdate != null && proc != null && !proc.HasExited)
                    statusUpdate(su);
                Thread.Sleep(1000);

            }
        }

        protected virtual void doStatusCycleOverrides()
        {}
    }
}
