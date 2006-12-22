using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MeGUI
{
    public class VobSubIndexer : IJobProcessor
    {
        public static readonly JobProcessorFactory Factory =
       new JobProcessorFactory(new ProcessorFactory(init), "VobSubIndexer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is SubtitleIndexJob) return new VobSubIndexer();
            return null;
        }

        private StatusUpdate stup;
        private SubtitleIndexJob job;
        private Process proc;
        private bool isProcessing;
        public VobSubIndexer()
        {
            stup = new StatusUpdate();
            stup.JobType = JobTypes.VOBSUB;
            isProcessing = false;
        }
        #region IJobProcessor Members

        public bool setup(Job job, out string error)
        {
            error = "";
            if (job is SubtitleIndexJob)
            {
                this.job = (SubtitleIndexJob)job;
            }
            else
            {
                error = "Job '" + job.Name + "' has been given to the VobSubIndexer, even though it is not a SubtitleIndexJob.";
                return false;
            }
            if (!System.IO.File.Exists(this.job.ScriptFile))
            {
                error = "Unable to find the script file " + this.job.ScriptFile;
                return false;
            }
            string vobsubPath = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\vobsub.dll";
            if (!System.IO.File.Exists(vobsubPath))
            {
                error = "Unable to find vobsub.dll in your system32 directory";
                return false;
            }
            stup.JobName = this.job.Name;
            return true;
        }

        public bool start(out string error)
        {
            error = null;
            proc = new Process();
            ProcessStartInfo pstart = new ProcessStartInfo();
            pstart.FileName = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\rundll32.exe";
            //pstart.FileName = "rundll32";
            pstart.Arguments = job.Commandline;
            pstart.WindowStyle = ProcessWindowStyle.Minimized;
            pstart.UseShellExecute = true;
            pstart.CreateNoWindow = true;
            proc.StartInfo = pstart;
            proc.EnableRaisingEvents = true;
            proc.Exited += new EventHandler(proc_Exited);
            try
            {
                job.Start = DateTime.Now;
                bool started = proc.Start();
                isProcessing = true;
                this.changePriority(job.Priority, out error);
                return true;
            }
            catch (Exception e)
            {
                error = "Exception starting the encoder: " + e.Message;
                return false;
            }
        }

        void proc_Exited(object sender, EventArgs e)
        {
            job.End = DateTime.Now;
            stup.IsComplete = true;
            StatusUpdate(stup);
        }

        public bool stop(out string error)
        {
            error = null;
            if (proc != null && !proc.HasExited)
            {
                try
                {
                    stup.WasAborted = true;
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
            error = "Cannot pause a vobsub job";
            return false;
        }

        public bool resume(out string error)
        {
            error = "Cannot resume a vobsub job";
            return false;
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

        public bool canBeProcessed(Job job)
        {
            if (job is SubtitleIndexJob)
                return true;
            return false;
        }

        public event JobProcessingStatusUpdateCallback StatusUpdate;

        #endregion
    }
}
