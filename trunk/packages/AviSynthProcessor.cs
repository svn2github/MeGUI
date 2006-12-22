using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Text;

namespace MeGUI
{
 	public delegate void AviSynthStatusUpdateCallback(StatusUpdate su);

    public class AviSynthProcessor : IJobProcessor
    {
        public static readonly JobProcessorFactory Factory =
       new JobProcessorFactory(new ProcessorFactory(init), "AviSynthProcessor");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is AviSynthJob) return new AviSynthProcessor();
            return null;
        }

        #region variables
        protected System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(true); // lock used to pause encoding
#warning AvsFile should be refactored to IMediaFile once fast frame reading is supported
        private AvsFile file;
        private IVideoReader reader;
        private bool aborted;
        private int position;
        private Thread processorThread, statusThread;
        private int lastFPSUpdateFrame;
        private long lastFPSUpdateTime;
        private StatusUpdate stup;
        private AviSynthJob job;
        #endregion
        #region start / stop
        public AviSynthProcessor()
        {
            stup = new StatusUpdate();
            stup.JobType = JobTypes.AVS;
        }
        #endregion
        #region processing
        public bool canBeProcessed(Job job)
        {
            if (job is AviSynthJob)
                return true;
            return false;
        }
        private void update()
        {
            lastFPSUpdateFrame = 0;
            lastFPSUpdateTime = DateTime.Now.Ticks;
            while (!aborted && position < stup.NbFramesTotal)
            {
                stup.NbFramesDone = position;
                stup.PercentageDoneExact = ((double)stup.NbFramesDone / (double)stup.NbFramesTotal) * 100.0;
                stup.TimeElapsed = DateTime.Now.Ticks - job.Start.Ticks;
                long now = DateTime.Now.Ticks;
                if (stup.NbFramesDone >= lastFPSUpdateFrame + 100) // after 100 frames, recalculate the FPS
                {
                    TimeSpan ts = TimeSpan.FromTicks(now - lastFPSUpdateTime); // time elapsed since the last 100 frames
                    stup.FPS = (double)(stup.NbFramesDone - lastFPSUpdateFrame) / ts.TotalSeconds;
                    lastFPSUpdateFrame = stup.NbFramesDone;
                    lastFPSUpdateTime = now;
                }
                StatusUpdate(stup);
                Thread.Sleep(500);
            }
            stup.IsComplete = true;
            StatusUpdate(stup);
        }

        private void process()
        {
            IntPtr zero = new IntPtr(0);
            for (position = 0; position < stup.NbFramesTotal && !aborted; position++)
            {
                file.Clip.ReadFrame(zero, 0, position);
                mre.WaitOne();
            }
            file.Dispose();
        }
        /// <summary>
        /// sets up encoding
        /// </summary
        /// <param name="job">the job to be processed</param>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if the setup has succeeded, false if it has not</returns>
        public bool setup(Job job, out string error)
        {
            error = "";
            if (job is AviSynthJob)
            {
                this.job = (AviSynthJob)job;
            }
            else
            {
                 error = "Job '" + job.Name + "' has been given to the AviSynthProcessor, even though it is not an AviSynthJob.";
                return false;
            }
            stup.JobName = job.Name;

            try 
            {
                file = AvsFile.OpenScriptFile(job.Input);
                reader = file.GetVideoReader();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            stup.NbFramesTotal = reader.FrameCount;
            position = 0;
            try
            {
                processorThread = new Thread(new ThreadStart(process));
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            try
            {
                statusThread = new Thread(new ThreadStart(update));
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// starts the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully started, false if not</returns>
        public bool start(out string error)
        {
            error = "";
            try
            {
                statusThread.Start();
                processorThread.Start();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// stops the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully stopped, false if not</returns>
        public bool stop(out string error)
        {
            error = "";
            aborted = true;
            return true;
        }
        /// <summary>
        /// pauses the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully paused, false if not</returns>
        public bool pause(out string error)
        {
            error = "";
            try
            {
                mre.Reset();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// resumes the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully started, false if not</returns>
        public bool resume(out string error)
        {
            error = "";
            try
            {
                mre.Set();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// changes the priority of the encoding process/thread
        /// </summary>
        /// <param name="priority">the priority to change to</param>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if the priority has been changed, false if not</returns>
        public bool changePriority(ProcessPriority priority, out string error)
        {
            error = null;
            if (processorThread != null && processorThread.IsAlive)
            {
                try
                {
                    if (priority == ProcessPriority.IDLE)
                        processorThread.Priority = ThreadPriority.Lowest;
                    else if (priority == ProcessPriority.NORMAL)
                        processorThread.Priority = ThreadPriority.Normal;
                    else if (priority == ProcessPriority.HIGH)
                        processorThread.Priority = ThreadPriority.Highest;
                    return true;
                }
                catch (Exception e) // process could not be running anymore
                {
                    error = "exception in changePriority: " + e.Message;
                    return false;
                }
            }
            else
            {
                if (processorThread == null)
                    error = "Process has not been started yet";
                else
                    error = "Process has exited";
                return false;
            }
        }
        public event JobProcessingStatusUpdateCallback StatusUpdate;
        #endregion
    }
}
