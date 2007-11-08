using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class IndividualWorkerSummary : UserControl
    {
        public IndividualWorkerSummary()
        {
            InitializeComponent();
        }

        public JobWorker Worker
        {
            set { w = value; }
        }

        JobWorker w;

        public void RefreshInfo()
        {

            workerNameAndJob.Text = string.Format("{0}: {1}", w.Name, w.StatusString);
            progressBar1.Value = (int)w.Progress;
        }

        private void startEncodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w.StartEncoding(true);
        }

        private void abortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w.UserRequestedAbort();
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w.UserRequestedRename();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stopToolStripMenuItem.Checked)
                w.SetRunning();
            else
                w.SetStopping();
        }

        private void shutDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w.UserRequestShutDown();
        }

        private void showProgressWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showProgressWindowToolStripMenuItem.Checked)
                w.HideProcessWindow();
            else
                w.ShowProcessWindow();
        }

        private void showQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showQueueToolStripMenuItem.Checked)
                w.Hide();
            else
                w.Show();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            startEncodingToolStripMenuItem.Enabled = (!w.IsEncoding);
            abortToolStripMenuItem.Enabled = w.IsEncoding;
            
            stopToolStripMenuItem.Enabled = w.IsEncoding;
            stopToolStripMenuItem.Checked = w.Status == JobWorkerStatus.Stopping;

            showProgressWindowToolStripMenuItem.Enabled = w.IsProgressWindowAvailable;
            showProgressWindowToolStripMenuItem.Checked = w.IsProgressWindowVisible;

            showQueueToolStripMenuItem.Checked = w.Visible;
        }
    }
}
