#if UNRELEASED

using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    class JobHandler : IJobProcessor
    {
        private MeGUISettings settings;
        protected Job job;
        protected StatusUpdate su;
        protected StringBuilder log; // holds logging information
        private JobHandler handler;
        public JobHandler(MeGUISettings settings)
        {
            this.settings = settings;
        }
        public JobHandler()
        {
        }

        #region IJobProcessor Members

        public bool setup(Job job, out string error)
        {
            if (job is AudioJob)
            {
                //handler = new AudioEncoder();
            }
            error = null;
            return handler.setup(job, out error);
        }

        public bool start(out string error)
        {
            error = null;
            handler.job.Start = DateTime.Now;
            return handler.start(out error);
        }

        public bool stop(out string error)
        {
            error = null;
            return handler.stop(out error);
        }

        public bool pause(out string error)
        {
            error = null;
            return handler.pause(out error);
        }

        public bool resume(out string error)
        {
            error = null;
            return handler.resume(out error);
        }

        public bool changePriority(ProcessPriority priority, out string error)
        {
            error = null;
            return handler.changePriority(priority, out error);
        }

        private event JobProcessingStatusUpdateCallback statusUpdate;

        public event JobProcessingStatusUpdateCallback StatusUpdate
        {
            add
            {
                handler.statusUpdate += value;
            }
            remove
            {
                handler.statusUpdate -= value;
            }
        }

        #endregion
    }
}
#endif