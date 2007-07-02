using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MeGUI.core.util;

namespace MeGUI.packages.tools.besplitter
{
    class Joiner : CommandlineJobProcessor<AudioJoinJob>
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "BeSplit_Joiner");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is AudioJoinJob) return new Joiner(mf.Settings.BeSplitPath);
            return null;
        }

        string tmpfile;

        public Joiner(string exe)
        {
            executable = exe;
        }

        protected override bool checkExitCode()
        {
            return false;
        }

        protected override void doExitConfig()
        {
            base.doExitConfig();
            try
            {
                if (File.Exists(tmpfile))
                    File.Delete(tmpfile);
            }
            catch (IOException) { }

        }

        protected override void checkJobIO()
        {
            base.checkJobIO();

            try
            {
                // now create the temporary file
                tmpfile = Path.GetTempFileName();
                using (StreamWriter w = new StreamWriter(File.OpenWrite(tmpfile)))
                {
                    foreach (string file in job.InputFiles)
                        w.WriteLine(file);
                }
                job.Commandline = job.generateJoinCommandline(tmpfile);
            }
            catch (Exception e)
            {
                throw new JobRunException("Error generating temporary *.lst file: " + e.Message, e);
            }
        }

        public override void ProcessLine(string line, StreamType stream)
        {
            if (line.IndexOf("Usage") != -1)
                su.HasError = true;
            log.AppendLine(line);
        }
    }
}
