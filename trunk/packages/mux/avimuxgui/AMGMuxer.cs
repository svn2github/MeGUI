using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MeGUI.core.util;

namespace MeGUI
{
    class AMGMuxer : CommandlineMuxer
    {
        public static readonly JobProcessorFactory Factory =
new JobProcessorFactory(new ProcessorFactory(init), "AMGMuxer");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is MuxJob && (j as MuxJob).MuxType == MuxerType.AVIMUXGUI)
                return new AMGMuxer(mf.Settings.AviMuxGUIPath);
            return null;
        }

        string script_filename;

        public AMGMuxer(string executablePath)
        {
            this.executable = executablePath;
        }
        #region setup/start overrides
        protected override void checkJobIO()
        {
            script_filename = writeScript(job);
            job.Commandline = "\"" + script_filename + "\"";
            
            base.checkJobIO();
        }

        private string writeScript(MuxJob job)
        {
            MuxSettings settings = job.Settings;

            // First, generate the script

            StringBuilder script = new StringBuilder();
            script.AppendLine("CLEAR");

            int fileNum = 1; // the number of the file at the top
            
            int audioNum = 1; // the audio track number
            // add the audio streams
            foreach (SubStream s in settings.AudioStreams)
            {
                script.AppendFormat("LOAD {1}{0}", Environment.NewLine, s.path);
                script.AppendLine("SET OUTPUT OPTIONS");
                if (!string.IsNullOrEmpty(s.name))
                    script.AppendFormat("SET OPTION AUDIO NAME {0} {1}{2}", audioNum, s.name, Environment.NewLine);
                if (!string.IsNullOrEmpty(s.language))
                    script.AppendFormat("SET OPTION AUDIO LNGCODE {0} {1}{2}", audioNum, s.language, Environment.NewLine);
                if (s.delay != 0)
                    script.AppendFormat("SET OPTION DELAY {0} {1}{2}", audioNum, s.delay, Environment.NewLine);

                audioNum++;
                fileNum++;
            }

            int subtitleNum = 1; // the subtitle track number
            // add the subtitle streams
            foreach (SubStream s in settings.SubtitleStreams)
            {
                script.AppendFormat("LOAD {1}{0}", Environment.NewLine, s.path);
                script.AppendLine("SET OUTPUT OPTIONS");
                if (!string.IsNullOrEmpty(s.name))
                    script.AppendFormat("SET OPTION SUBTITLE NAME {0} {1}{2}", subtitleNum, s.name, Environment.NewLine);

                subtitleNum++;
                fileNum++;
            }

            // add the video stream if it exists
            if (!string.IsNullOrEmpty(settings.VideoInput))
            {
                script.AppendFormat("LOAD {1}{0}SELECT FILE {2}{0}ADD VIDEOSOURCE{0}DESELECT FILE {2}{0}", 
                    Environment.NewLine, settings.VideoInput, fileNum);
                fileNum++;
            }

            // mux in the rest if it exists
            if (!string.IsNullOrEmpty(settings.MuxedInput))
            {
                script.AppendFormat("LOAD {1}{0}SELECT FILE {2}{0}ADD VIDEOSOURCE{0}DESELECT FILE {2}{0}",
                    Environment.NewLine, settings.MuxedInput, fileNum);
                fileNum++;
            }
            
            // AR can't be signalled in AVI

            script.AppendLine("SET OUTPUT OPTIONS");
            // split size
            if (settings.SplitSize.HasValue)
                script.AppendFormat("SET OPTION MAXFILESIZE {0}{1}", settings.SplitSize, Environment.NewLine);

            // Now do the rest of the setup
            script.AppendLine(
@"SET INPUT OPTIONS
SET OPTION MP3 VERIFY CBR ALWAYS
SET OPTION MP3 VERIFY RESDLG OFF
SET OPTION AVI FIXDX50 1
SET OPTION CHAPTERS IMPORT 1
SET OUTPUT OPTIONS
SET OPTION ALL SUBTITLES 1
SET OPTION ALL AUDIO 1
SET OPTION CLOSEAPP 1
SET OPTION DONEDLG 0
SET OPTION OVERWRITEDLG 0
SET OPTION STDIDX AUTO");

            script.AppendFormat("START {0}{1}", settings.MuxedOutput, Environment.NewLine);

            

            /// the script is now created; let's write it to a temp file
            string filename = Path.GetTempFileName();
            using (StreamWriter output = new StreamWriter(File.OpenWrite(filename)))
            {
                output.Write(script.ToString());
            }
            return filename;
        }

        protected override void doExitConfig()
        {
            try
            {
                File.Delete(script_filename);
            }
            catch (IOException) { }
            base.doExitConfig();
        }

        #endregion

        protected override bool checkExitCode
        {
            get { return false; }
        }
    }
}
