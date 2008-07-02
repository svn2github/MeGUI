using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using MeGUI.core.util;
using System.IO;

namespace MeGUI
{
    public class VobSubIndexer : CommandlineJobProcessor<SubtitleIndexJob>
    {
        public static readonly JobProcessorFactory Factory =
       new JobProcessorFactory(new ProcessorFactory(init), "VobSubIndexer");

        private string configFile = null;

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is SubtitleIndexJob) return new VobSubIndexer();
            return null;
        }

        protected override string Commandline
        {
            get
            {
                return "vobsub.dll,Configure " + configFile;
            }
        }

        public VobSubIndexer()
        {
            executable = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\rundll32.exe";
        }
        #region IJobProcessor Members
        protected override void checkJobIO()
        {
            base.checkJobIO();
            generateScript();
            Util.ensureExists(configFile);
        }

        private void generateScript()
        {
            configFile = Path.ChangeExtension(job.Input, ".vobsub");

            using (StreamWriter sw = new StreamWriter(configFile, false, Encoding.Default))
            {
                sw.WriteLine(job.Input);
                sw.WriteLine(FileUtil.GetPathWithoutExtension(job.Output));
                sw.WriteLine(job.PGC);
                sw.WriteLine("0"); // we presume angle processing has been done before
                if (job.IndexAllTracks)
                    sw.WriteLine("ALL");
                else
                {
                    foreach (int id in job.TrackIDs)
                    {
                        sw.Write(id + " ");
                    }
                    sw.Write(sw.NewLine);
                }
                sw.WriteLine("CLOSE");
            }

            job.FilesToDelete.Add(configFile);
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
