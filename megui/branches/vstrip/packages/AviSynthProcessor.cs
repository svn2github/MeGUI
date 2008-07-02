using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Text;
using MeGUI.core.util;

namespace MeGUI
{
 	public delegate void AviSynthStatusUpdateCallback(StatusUpdate su);

    public class AviSynthProcessor : IJobProcessor
    {
        DateTime startTime;
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
        private ulong position;
        private Thread processorThread, statusThread;
//       private ulong lastFPSUpdateFrame;
//        private long lastFPSUpdateTime;
        private StatusUpdate stup;
        private AviSynthJob job;
        #endregion
        #region start / stop
        public AviSynthProcessor()
        {
        }
        #endregion
        #region processing
/*        public bool canBeProcessed(Job job)
        {
            if (job is AviSynthJob)
                return true;
            return false;
        }*/
        private void update()
        {
            while (!aborted && position < stup.NbFramesTotal)
            {
                stup.NbFramesDone = position;
                stup.TimeElapsed = DateTime.Now - startTime;
                stup.FillValues();
                StatusUpdate(stup);
                Thread.Sleep(1000);
            }
            stup.IsComplete = true;
            StatusUpdate(stup);
        }

        private void process()
        {
            IntPtr zero = new IntPtr(0);
            for (position = 0; position < stup.NbFramesTotal && !aborted; position++)
            {
                file.Clip.ReadFrame(zero, 0, (int)position);
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
        public void setup(Job job, StatusUpdate su)
        {
            Debug.Assert(job is AviSynthJob, "Job isn't an AviSynthJob");

            this.job = (AviSynthJob)job;

            try 
            {
                file = AvsFile.OpenScriptFile(job.Input);
                reader = file.GetVideoReader();
            }
            catch (Exception ex)
            {
                throw new JobRunException(ex);
            }
            stup.NbFramesTotal = (ulong)reader.FrameCount;
            stup.ClipLength = TimeSpan.FromSeconds((double)stup.NbFramesTotal / file.Info.FPS);
            stup.Status = "Playing through file...";
            position = 0;
            try
            {
                processorThread = new Thread(new ThreadStart(process));
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
            try
            {
                statusThread = new Thread(new ThreadStart(update));
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }
        /// <summary>
        /// starts the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully started, false if not</returns>
        public void start()
        {
            try
            {
                statusThread.Start();
                processorThread.Start();
                startTime = DateTime.Now;
            }
            catch (Exception e)
            {
                throw new JobRunException(e);
            }
        }
        /// <summary>
        /// stops the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully stopped, false if not</returns>
        public void stop()
        {
            aborted = true;
        }
        /// <summary>
        /// pauses the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully paused, false if not</returns>
        public void pause()
        {
            if (!mre.Reset())
                throw new JobRunException("Could not reset mutex");
        }
        /// <summary>
        /// resumes the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully started, false if not</returns>
        public void resume()
        {
            if (!mre.Set())
                throw new JobRunException("Could not set mutex");
        }
        /// <summary>
        /// changes the priority of the encoding process/thread
        /// </summary>
        /// <param name="priority">the priority to change to</param>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if the priority has been changed, false if not</returns>
        public void changePriority(ProcessPriority priority)
        {
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
                    return;
                }
                catch (Exception e) // process could not be running anymore
                {
                    throw new JobRunException(e);
                }
            }
            else
            {
                if (processorThread == null)
                    throw new JobRunException("Process has not been started yet");
                else
                    throw new JobRunException("Process has exited");
            }
        }
        public event JobProcessingStatusUpdateCallback StatusUpdate;
        #endregion
    }
}
