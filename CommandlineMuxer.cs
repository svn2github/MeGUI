using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace MeGUI
{
    public delegate void MuxerOutputCallback(string line, int type);

    class CommandlineMuxer : Muxer
    {
        #region variables
        protected Process proc; // the encoder process
        protected string executable; // path and filename of the muxer executable to be used
        protected long videoSize = 0, audioSize1 = 0, audioSize2, subtitleSize;
        protected System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(true); // lock used to pause encoding
        protected System.Threading.ManualResetEvent stdoutDone = new System.Threading.ManualResetEvent(false);
        protected System.Threading.ManualResetEvent stderrDone = new System.Threading.ManualResetEvent(false);
        public event MuxerOutputCallback MuxerOutputReceived;
        #endregion
        public CommandlineMuxer()
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
                        MuxerOutputReceived(line, 0);
                    }
                }
                catch (Exception e)
                {
                    MuxerOutputReceived("Exception in readStdOut: " + e.Message, 2);
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
                        MuxerOutputReceived(line, 1);
                    }
                }
                catch (Exception e)
                {
                    MuxerOutputReceived("Exception in readStdErr: " + e.Message, 2);
                }
                stderrDone.Set();
            }
        }
        #endregion
        #region helper methods
        /// <summary>
        /// checks if the encoder path given to the encoder actually exists
        /// </summary>
        /// <param name="executableName">path and name of the encoder executable</param>
        /// <param name="error">return string for errors</param>
        /// <returns>true if the encoder is there, false if not</returns>
        protected bool checkExecutable(string executableName, out string error)
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
        void setProjectedFileSize()
        {
            MuxJob mjob = (MuxJob)job;
            FileInfo fi = new FileInfo(job.Input);
            try
            {
                videoSize = fi.Length; 
                
            }
            catch (IOException) { videoSize = 0; }
            su.ProjectedFileSize = (int) (videoSize / 1024L);
            int count = 0;
            foreach (object o in mjob.Settings.AudioStreams)
            {
                SubStream audioStream = (SubStream)o;
                fi = new FileInfo(audioStream.path);
                int fileLength = 0;
                try
                {
                    fileLength = (int)(fi.Length / 1024);
                }
                catch (IOException) { }
                su.ProjectedFileSize += fileLength;
                if (count == 0)
                    audioSize1 = fileLength;
                else if (count == 1)
                    audioSize2 = fileLength;
                count++;
            }
            foreach (object o in mjob.Settings.SubtitleStreams)
            {
                SubStream subtitleStream = (SubStream)o;
                fi = new FileInfo(subtitleStream.path);
                su.ProjectedFileSize += (int)(fi.Length / 1024);
                subtitleSize += fi.Length;
            }
        }
        #endregion
        #region IJobProcessor overridden members

        public override bool setup(Job job, out string error)
        {
            error = null;
            // This allows relative paths
            executable = Path.Combine(System.Windows.Forms.Application.StartupPath, executable);
            if (!checkExecutable(executable, out error))
            {
                return false;
            }
            executable = "\"" + executable + "\"";
            this.job = (MuxJob)job;
            su = new StatusUpdate();
            su.JobName = job.Name;
            su.JobType = JobTypes.MUX;
            su.FileSize = 0;
            log = new StringBuilder();
            setProjectedFileSize();
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
