using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI.packages.tools.besplitter
{
    class Splitter : CommandlineJobProcessor<AudioSplitJob>
    {
        protected decimal totalTime; // in ms

        public static readonly JobProcessorFactory Factory =
  new JobProcessorFactory(new ProcessorFactory(init), "BeSplit_Splitter");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is AudioSplitJob) return new Splitter(mf.Settings.BeSplitPath);
            return null;
        }

        protected override bool checkExitCode()
        {
            return false;
        }

        public Splitter(string exe)
        {
            executable = exe;
        }

        protected override void checkJobIO()
        {
            int endFrame = job.TheCuts.AllCuts[job.TheCuts.AllCuts.Count - 1].endFrame;
            totalTime = ((decimal)endFrame) / ((decimal)job.TheCuts.Framerate) * 1000M;
            base.checkJobIO();
        }

        public override void ProcessLine(string line, StreamType stream)
        {
            if (line.IndexOf("Writing") != -1)
            {
                // this is a progress line
                try
                {
                    int hours = int.Parse(line.Substring(1, 2));
                    int mins = int.Parse(line.Substring(4, 2));
                    int secs = int.Parse(line.Substring(7, 2));
                    int millis = int.Parse(line.Substring(10, 3));
                    TimeSpan position = new TimeSpan(0, hours, mins, secs, millis);
                    decimal percentDone = ((decimal)position.Milliseconds) / totalTime;
                    su.PercentageDoneExact = percentDone;

                }
                catch (FormatException)
                {
                    log.AppendLine(line);
                }
                catch (ArgumentOutOfRangeException)
                {
                    log.AppendLine(line);
                }
            }
            else if (line.IndexOf("Usage") != -1)
            {
                su.HasError = true;
                log.AppendLine(line);
            }
        }
    }
}
