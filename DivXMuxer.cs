using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace MeGUI
{
    class DivXMuxer : CommandlineMuxer
    {
        public DivXMuxer(string executablePath)
        {
            this.executable = executablePath;
        }
        public override bool start(out string error)
        {
            error = null;
            base.start(out error); // always return true so we don't check the return value
            this.MuxerOutputReceived += new MuxerOutputCallback(DivXMuxer_MuxerOutputReceived);
            try
            {
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

        void DivXMuxer_MuxerOutputReceived(string line, int type)
        {
            if (line.Contains("Muxing"))
                return;
            if (line.ToLower().Contains("error"))
            {
                su.Error = line;
                su.HasError = true;
            }
            log.AppendLine(line);
        }
    }
}
