using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using MeGUI.core.util;

namespace MeGUI
{
    public class VobSubIndexer : CommandlineJobProcessor<SubtitleIndexJob>
    {
        public static readonly JobProcessorFactory Factory =
       new JobProcessorFactory(new ProcessorFactory(init), "VobSubIndexer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is SubtitleIndexJob) return new VobSubIndexer();
            return null;
        }

        public VobSubIndexer()
        {
            executable = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\rundll32.exe";
//            executable = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\vobsub.dll";
        }
        #region IJobProcessor Members
        protected override void checkJobIO()
        {
            base.checkJobIO();
            Util.ensureExists(job.ScriptFile);
        }

        public override bool canPause
        {
            get { return false; }
        }

        #endregion

        protected override bool checkExitCode
        {
            get { return false; }
        }
    }
}
