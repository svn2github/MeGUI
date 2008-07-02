using System;
using System.Collections.Generic;
using System.Text;
using MeGUI.core.util;
using System.Text.RegularExpressions;

namespace MeGUI.packages.tools.dvddecrypter
{
    class DVDDecrypterRunner : CommandlineJobProcessor<DVDJob>
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(
            delegate(MainForm mf, Job j)
            {
                if (j is DVDJob) return new DVDDecrypterRunner(mf.Settings.DVDDecrypterPath);
                return null;
            }, "DVDDecrypter_runner");

        protected override string Commandline
        {
            get
            {
                return string.Format(
                    @"/MODE IFO /SRC {0} /DEST ""{1}""  /VTS {2} /PGC {3} /START /CLOSE /OVERWRITE YES /NAMING PGC",
                    job.Input.TrimEnd('\\', '/'), job.Output.TrimEnd('\\', '/') + '\\', job.VTS, job.PGC);
            }
        }

        public DVDDecrypterRunner(string exe)
            : base()
        {
            executable = exe;
        }

        protected override void checkJobIO()
        {
            
        }

        private static readonly TimeSpan TwoSeconds = new TimeSpan(0, 0, 2);
        private static readonly Regex DDPercent = new Regex("^(?<num>[0-9]+)% ", RegexOptions.Compiled);
        protected override void doStatusCycleOverrides()
        {
            try
            {
                string text = WindowUtil.GetText(windowHandle);

                Match m = DDPercent.Match(text);
                if (m.Success)
                {
                    su.PercentageDoneExact = int.Parse(m.Groups["num"].Value);
                }
            }
            catch (Exception) { }
        }
    }
}
