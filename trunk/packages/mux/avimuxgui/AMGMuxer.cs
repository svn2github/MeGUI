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
        string script_filename;

        public AMGMuxer(string executablePath)
        {
            this.executable = executablePath;
        }
        #region setup/start overrides
        public override bool setup(Job job, out string error)
        {
            error = null;
            MuxJob mjob = (MuxJob)job;
            script_filename = writeScript(mjob);
            mjob.Commandline = "\"" + script_filename + "\"";

            // Since AviMuxGUI is so sensitive to errors, let's be extra careful, and check that the files
            // all exist, so we can give the user a nicer error, and continue with the queue
            if (base.setup(job, out error))
                return true;
            else
                return false;
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
            if (settings.SplitSize != 0)
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

        public override bool start(out string error)
        {
            error = null;
            base.start(out error); // always return true so we don't check the return value
            try
            {
                proc.Exited += new EventHandler(proc_Exited);
                bool started = proc.Start();
                new MethodInvoker(this.readStdOut).BeginInvoke(null, null);
                new MethodInvoker(this.readStdErr).BeginInvoke(null, null);
                this.changePriority(job.Priority, out error);
                return true;
            }
            catch (Exception e)
            {
                error = "Exception starting the process: " + e.Message;
                return false;
            }
        }

        void proc_Exited(object sender, EventArgs e)
        {
            try
            {
                File.Delete(script_filename);
            }
            catch (IOException) { }
        }
        #endregion
    }
}
