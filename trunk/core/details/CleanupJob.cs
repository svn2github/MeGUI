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
            get { return "cleanup"; }
        }

        private string type;
        public override string EncodingMode
        {
            get { return type; }
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

        #region IJobProcessor Members

        private CleanupJobRunner(MainForm m)
        {
            this.mf = m;
        }

        StatusUpdate su;
        List<string> files;
        MainForm mf = MainForm.Instance;

        void IJobProcessor.setup(Job job, StatusUpdate su)
        {
            CleanupJob j = (CleanupJob)job;
            this.su = su;
            this.files = j.files;
        }

        void run()
        {
            Thread.Sleep(2000); // just so that the job has properly registered as starting

            if (!mf.Settings.DeleteIntermediateFiles)
                su.Log = "Cleanup job running, but deletion of intermediate files is not set. Nothing will be deleted.";
            else
            {
                StringBuilder log = new StringBuilder();
                foreach (string file in files)
                {
                    log.AppendFormat("Found intermediate output file '{0}', deleting...", file);

                    try
                    {
                        File.Delete(file);
                        log.AppendLine("Deletion succeeded.");
                    }
                    catch (IOException)
                    {
                        log.AppendLine("Deletion failed.");
                    }
                }
                su.Log = log.ToString();
            }
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
