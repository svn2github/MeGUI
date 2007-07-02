using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using MeGUI.core.util;
using MeGUI.core.plugins.implemented;

namespace MeGUI
{
    public enum StreamType : ushort { None = 0, Stderr = 1, Stdout = 2 }

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
        protected StatusUpdate su = new StatusUpdate();
        protected StringBuilder log = new StringBuilder();
        #endregion

        protected virtual void checkJobIO()
        {
            Util.ensureExists(job.Input);
        }

        protected virtual void doExitConfig()
        { }

        // returns true if the exit code yields a meaningful answer
        protected virtual bool checkExitCode
        {
            get { return true; }
        }


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
            if (checkExitCode && proc.ExitCode != 0) // check the exitcode because x264.exe sometimes exits with error but without
                su.HasError = true; // any commandline indication as to why
            job.End = DateTime.Now;
            su.IsComplete = true;
            doExitConfig();
            su.Log = log.ToString();
            StatusUpdate(su);
        }

        #region IVideoEncoder overridden Members

        public void setup(Job job2)
        {
            Debug.Assert(job2 is TJob, "Job is the wrong type");

            TJob job = (TJob)job2;
            this.job = job;

            // This enables relative paths, etc
            executable = Path.Combine(System.Windows.Forms.Application.StartupPath, executable);

            Util.ensureExists(executable);

            checkJobIO();
            
            su.JobName = job.Name;
            su.JobType = job.JobType;
        }

        public void start()
        {
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
                this.changePriority(job.Priority);
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }

        public void stop()
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
                    throw new JobRunException("Encoder process has already existed");
            }
        }

        public void pause()
        {
            if (!canPause)
                throw new JobRunException("Can't pause this kind of job.");
            if (!mre.Reset())
                throw new JobRunException("Could not reset mutex. pause failed");
        }

        public void resume()
        {
            if (!canPause)
                throw new JobRunException("Can't resume this kind of job.");
            if (!mre.Set())
                throw new JobRunException("Could not set mutex. pause failed");
        }

        public bool isRunning()
        {
            return (proc != null && !proc.HasExited);
        }

        public void changePriority(ProcessPriority priority)
        {
            if (isRunning())
            {
                try
                {
                    if (priority == ProcessPriority.IDLE)
                        proc.PriorityClass = ProcessPriorityClass.Idle;
                    else if (priority == ProcessPriority.NORMAL)
                        proc.PriorityClass = ProcessPriorityClass.Normal;
                    else if (priority == ProcessPriority.HIGH)
                        proc.PriorityClass = ProcessPriorityClass.High;
                    return;
                }
                catch (Exception e) // process could not be running anymore
                {
                    throw new JobRunException(e);
                }
            }
            else 
            {
                if (proc == null)
                    throw new JobRunException("Process has not been started yet");
                else
                {
                    Debug.Assert(proc.HasExited);
                    throw new JobRunException("Process has exited");
                }
            }
        }

/*        public virtual bool canBeProcessed(Job job)
        {
            return (job is TJob);
        }
        */
        public virtual bool canPause
        {
            get { return true; }
        }


        #endregion
        #region reading process output
        protected virtual void readStream(StreamReader sr, ManualResetEvent rEvent, StreamType str)
        {
            string line;
            if (proc != null)
            {
                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        mre.WaitOne();
                        ProcessLine(line, str);
                    }
                }
                catch (Exception e)
                {
                    ProcessLine("Exception in readStdErr: " + e.Message, str);
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
            readStream(sr, stdoutDone, StreamType.Stdout);
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
            readStream(sr, stderrDone, StreamType.Stderr);
        }

        public virtual void ProcessLine(string line, StreamType stream)
        {
            log.AppendLine(line);
        }

        #endregion
        #region status updates
        public event JobProcessingStatusUpdateCallback StatusUpdate;
/*        {
            add { statusUpdate += value; }
            remove { statusUpdate -= value; }
        }*/

        protected void RunStatusCycle()
        {
            while (isRunning())
            {
                su.TimeElapsed = DateTime.Now - job.Start;
                su.CurrentFileSize = FileSize.Of2(job.Output);

                doStatusCycleOverrides();
                su.FillValues();
                if (StatusUpdate != null && proc != null && !proc.HasExited)
                    StatusUpdate(su);
                Thread.Sleep(1000);

            }
        }

        protected virtual void doStatusCycleOverrides()
        { }
        #endregion
    }
}
