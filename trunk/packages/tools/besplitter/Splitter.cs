using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI.packages.tools.besplitter
{
    class Splitter : CommandlineJobProcessor<AudioSplitJob>
    {
        public static readonly JobProcessorFactory Factory =
  new JobProcessorFactory(new ProcessorFactory(init), "BeSplit_Splitter");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is AudioSplitJob) return new Splitter(mf.Settings.BeSplitPath);
            return null;
        }

        protected override bool checkExitCode
        {
            get { return false; }
        }

        public Splitter(string exe)
        {
            executable = exe;
        }

        protected override void checkJobIO()
        {
            int endFrame = job.TheCuts.AllCuts[job.TheCuts.AllCuts.Count - 1].endFrame;
            su.ClipLength = TimeSpan.FromSeconds((double)endFrame / job.TheCuts.Framerate);
            base.checkJobIO();
        }

        public override void ProcessLine(string line, StreamType stream)
        {
            if (line.IndexOf("Writing") != -1 || line.IndexOf("Seeking") != -1)
            {
                // this is a progress line
                try
                {
                    int hours = int.Parse(line.Substring(1, 2));
                    int mins = int.Parse(line.Substring(4, 2));
                    int secs = int.Parse(line.Substring(7, 2));
                    int millis = int.Parse(line.Substring(10, 3));
                    su.ClipPosition = new TimeSpan(0, hours, mins, secs, millis);
                }
                catch (FormatException)
                {
                    log.AppendLine(line);
                }
                catch (ArgumentOutOfRangeException)
                {
                    log.AppendLine(line);
                }
                return;
            }
            
            if (line.IndexOf("Usage") != -1)
                su.HasError = true;

            log.AppendLine(line);
        }
    }
}
