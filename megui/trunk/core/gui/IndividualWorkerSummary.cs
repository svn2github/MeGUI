// ****************************************************************************
// 
// Copyright (C) 2005-2012 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class IndividualWorkerSummary : UserControl
    {
        private JobWorker w;

        public IndividualWorkerSummary()
        {
            InitializeComponent();
        }

        public JobWorker Worker
        {
            set { w = value; }
        }

        public void RefreshInfo()
        {
            workerNameAndJob.Text = string.Format("{0}: {1}", w.Name, w.StatusString);
            progressBar1.Value = (int)w.Progress;
        }

        private void startEncodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w.StartEncoding(true);
            RefreshInfo();
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

        private void shutDownLaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w.ShutDownWhenFinished();
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
            startEncodingToolStripMenuItem.Enabled = !w.IsEncoding;
            abortToolStripMenuItem.Enabled = w.IsEncoding;
            
            stopToolStripMenuItem.Enabled = w.IsEncoding;
            stopToolStripMenuItem.Checked = w.Status == JobWorkerStatus.Stopping;

            shutDownLaterToolStripMenuItem.Enabled = (w.LocalJobCount > 0 || w.Status != JobWorkerStatus.Idle);
            shutDownLaterToolStripMenuItem.Checked = w.Mode == JobWorkerMode.CloseOnLocalListCompleted;

            showProgressWindowToolStripMenuItem.Enabled = w.IsProgressWindowAvailable;
            showProgressWindowToolStripMenuItem.Checked = w.IsProgressWindowVisible;

            showQueueToolStripMenuItem.Checked = w.Visible;
        }
    }
}
