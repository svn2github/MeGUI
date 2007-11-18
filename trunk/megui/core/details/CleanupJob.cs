using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using MeGUI.core.util;

namespace MeGUI.core.details
{
    public class CleanupJob : Job
    {
        public List<string> files;

        private CleanupJob() { }

        public static JobChain AddAfter(JobChain other, List<string> files)
        {
            return AddAfter(other, null, files);
        }

        public static JobChain AddAfter(JobChain other, string type, List<string> files)
        {
            CleanupJob j = new CleanupJob();
            j.files = files;
            j.type = type;
            return new SequentialChain(other, j);
        }

        public override string CodecString
        {
            get { return ""; }
        }

        private string type;
        public override string EncodingMode
        {
            get { return "cleanup"; }
        }
    }

    class CleanupJobRunner : IJobProcessor
    {
        public static JobProcessorFactory Factory = new JobProcessorFactory(new ProcessorFactory(
            delegate(MainForm f, Job j)
            {
                if (j is CleanupJob)
                    return new CleanupJobRunner(f);
                return null;
            }), "cleanup");

        public static readonly JobPostProcessor DeleteIntermediateFilesPostProcessor = new JobPostProcessor(
            delegate(MainForm mf, Job j)
            {
                if (mf.Settings.DeleteIntermediateFiles)
                    return deleteIntermediateFiles(j.FilesToDelete);
                return null;
            }
            , "DeleteIntermediateFiles");


        
        /// <summary>
        /// Attempts to delete all files listed in job.FilesToDelete if settings.DeleteIntermediateFiles is checked
        /// </summary>
        /// <param name="job">the job which should just have been completed</param>
        private static LogItem deleteIntermediateFiles(List<string> files)
        {
            LogItem i = new LogItem("Deleting intermediate files");
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    i.LogEvent("Successfully deleted " + file);
                }
                catch (IOException e)
                {
                    i.LogValue("Error deleting " + file, e, ImageType.Error);
                }
            }
            return i;
        }

        #region IJobProcessor Members

        private CleanupJobRunner(MainForm m)
        {
            this.mf = m;
        }

        StatusUpdate su;
        List<string> files;
        MainForm mf = MainForm.Instance;
        LogItem log;

        void IJobProcessor.setup(Job job, StatusUpdate su, LogItem log)
        {
            CleanupJob j = (CleanupJob)job;
            this.log = log;
            this.su = su;
            this.files = j.files;
        }

        void run()
        {
            Thread.Sleep(2000); // just so that the job has properly registered as starting

            log.LogValue("Delete Intermediate Files option set", mf.Settings.DeleteIntermediateFiles);
            if (mf.Settings.DeleteIntermediateFiles)
                log.Add(deleteIntermediateFiles(files));

            su.IsComplete = true;
            statusUpdate(su);
        }

        void IJobProcessor.start()
        {
            new Thread(run).Start();
        }
        

        void IJobProcessor.stop()
        {
            throw new JobRunException("Not supported");
        }

        void IJobProcessor.pause()
        {
            throw new JobRunException("Not supported");
        }

        void IJobProcessor.resume()
        {
            throw new JobRunException("Not supported");
        }

        void IJobProcessor.changePriority(ProcessPriority priority)
        {
            throw new JobRunException("Not supported");
        }

        event JobProcessingStatusUpdateCallback statusUpdate;

        event JobProcessingStatusUpdateCallback IJobProcessor.StatusUpdate
        {
            add { statusUpdate += value; }
            remove { statusUpdate -= value; }
        }

        #endregion
    }
}
