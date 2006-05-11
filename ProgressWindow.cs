// ****************************************************************************
// 
// Copyright (C) 2005  Doom9
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MeGUI
{
	public delegate void WindowClosedCallback(bool hideOnly); // delegate for WindowClosed event
	public delegate void AbortCallback(); // delegate for Abort event
	public delegate void UpdateStatusCallback(StatusUpdate su); // deletgate for UpdateStatus event
	public delegate void PriorityChangedCallback(ProcessPriority priority); // delegate for PriorityChanged event
	/// <summary>
	/// ProgressWindow is a window that is being shown during encoding and shows the current encoding status
	/// it is being fed by UpdateStatus events fired from the main GUI class (Form1)
	/// </summary>
	public class ProgressWindow : System.Windows.Forms.Form
    {
        #region variables
        public event WindowClosedCallback WindowClosed; // event fired if the window closes
		public event AbortCallback Abort; // event fired if the abort button has been pressed
		public event PriorityChangedCallback PriorityChanged; // event fired if the priority dropdown has changed
		private System.Windows.Forms.Label currentVideoFrameLabel;
		private System.Windows.Forms.TextBox currentVideoFrame;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button abortButton;
		private System.Windows.Forms.Label videoDataLabel;
		private System.Windows.Forms.TextBox videoData;
		private System.Windows.Forms.Label filesizeLabel;
		private System.Windows.Forms.TextBox filesize;
		private System.Windows.Forms.Label fpsLabel;
		private System.Windows.Forms.TextBox fps;
		private System.Windows.Forms.Label timeElapsedLabel;
		private System.Windows.Forms.TextBox timeElapsed;
		private System.Windows.Forms.Label totalTimeLabel;
		private System.Windows.Forms.TextBox totalTime;
		private System.Windows.Forms.Label progressLabel;
		private System.Windows.Forms.ProgressBar progress;
		private System.Windows.Forms.Label priorityLabel;
		private System.Windows.Forms.ComboBox priority;
		private bool isUserClosing;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        #endregion
        #region start / stop
        /// <summary>
		/// default constructor, initializes the GUI components
		/// </summary>
		public ProgressWindow(JobTypes type)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			isUserClosing = true;
			if (type == JobTypes.AUDIO)
			{
				currentVideoFrameLabel.Text = "Current audio position:";
				videoDataLabel.Text = "Audio data:";
				fpsLabel.Text = "";
			}
            if (type == JobTypes.MUX)
			{
				currentVideoFrameLabel.Text = "Audio data:";
				fpsLabel.Text = "Current operation:";
			}
		}
		/// <summary>
		/// handles the onclosing event
		/// ensures that if the user closed the window, it will only be hidden
		/// whereas if the system closed it, it is allowed to close
		/// </summary>
		/// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.IsUserAbort)
            {
                e.Cancel = true;
                this.Hide();
                WindowClosed(true);
            }
            else
            {
                WindowClosed(false);
                base.OnClosing(e);
            }
        }
        /// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
        }
        #endregion
        #region Windows Form Designer generated code
        /// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.currentVideoFrameLabel = new System.Windows.Forms.Label();
			this.currentVideoFrame = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.totalTime = new System.Windows.Forms.TextBox();
			this.totalTimeLabel = new System.Windows.Forms.Label();
			this.timeElapsed = new System.Windows.Forms.TextBox();
			this.timeElapsedLabel = new System.Windows.Forms.Label();
			this.fps = new System.Windows.Forms.TextBox();
			this.fpsLabel = new System.Windows.Forms.Label();
			this.filesize = new System.Windows.Forms.TextBox();
			this.filesizeLabel = new System.Windows.Forms.Label();
			this.videoData = new System.Windows.Forms.TextBox();
			this.videoDataLabel = new System.Windows.Forms.Label();
			this.abortButton = new System.Windows.Forms.Button();
			this.progressLabel = new System.Windows.Forms.Label();
			this.progress = new System.Windows.Forms.ProgressBar();
			this.priorityLabel = new System.Windows.Forms.Label();
			this.priority = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// currentVideoFrameLabel
			// 
			this.currentVideoFrameLabel.Location = new System.Drawing.Point(16, 24);
			this.currentVideoFrameLabel.Name = "currentVideoFrameLabel";
			this.currentVideoFrameLabel.Size = new System.Drawing.Size(120, 23);
			this.currentVideoFrameLabel.TabIndex = 0;
			this.currentVideoFrameLabel.Text = "Current Video Frame:";
			// 
			// currentVideoFrame
			// 
			this.currentVideoFrame.Enabled = false;
			this.currentVideoFrame.Location = new System.Drawing.Point(136, 24);
			this.currentVideoFrame.Name = "currentVideoFrame";
			this.currentVideoFrame.Size = new System.Drawing.Size(128, 21);
			this.currentVideoFrame.TabIndex = 1;
			this.currentVideoFrame.Text = "";
			this.currentVideoFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// encoderGroupBox
			// 
			this.groupBox1.Controls.Add(this.totalTime);
			this.groupBox1.Controls.Add(this.totalTimeLabel);
			this.groupBox1.Controls.Add(this.timeElapsed);
			this.groupBox1.Controls.Add(this.timeElapsedLabel);
			this.groupBox1.Controls.Add(this.fps);
			this.groupBox1.Controls.Add(this.fpsLabel);
			this.groupBox1.Controls.Add(this.filesize);
			this.groupBox1.Controls.Add(this.filesizeLabel);
			this.groupBox1.Controls.Add(this.videoData);
			this.groupBox1.Controls.Add(this.videoDataLabel);
			this.groupBox1.Controls.Add(this.currentVideoFrame);
			this.groupBox1.Controls.Add(this.currentVideoFrameLabel);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(272, 176);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			// 
			// totalTime
			// 
			this.totalTime.Enabled = false;
			this.totalTime.Location = new System.Drawing.Point(136, 144);
			this.totalTime.Name = "totalTime";
			this.totalTime.Size = new System.Drawing.Size(128, 21);
			this.totalTime.TabIndex = 11;
			this.totalTime.Text = "";
			this.totalTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// totalTimeLabel
			// 
			this.totalTimeLabel.Location = new System.Drawing.Point(16, 144);
			this.totalTimeLabel.Name = "totalTimeLabel";
			this.totalTimeLabel.Size = new System.Drawing.Size(120, 23);
			this.totalTimeLabel.TabIndex = 10;
			this.totalTimeLabel.Text = "Remaining Time";
			// 
			// timeElapsed
			// 
			this.timeElapsed.Enabled = false;
			this.timeElapsed.Location = new System.Drawing.Point(136, 120);
			this.timeElapsed.Name = "timeElapsed";
			this.timeElapsed.Size = new System.Drawing.Size(128, 21);
			this.timeElapsed.TabIndex = 9;
			this.timeElapsed.Text = "";
			this.timeElapsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// timeElapsedLabel
			// 
			this.timeElapsedLabel.Location = new System.Drawing.Point(16, 120);
			this.timeElapsedLabel.Name = "timeElapsedLabel";
			this.timeElapsedLabel.TabIndex = 8;
			this.timeElapsedLabel.Text = "Time elapsed";
			// 
			// fps
			// 
			this.fps.Enabled = false;
			this.fps.Location = new System.Drawing.Point(136, 96);
			this.fps.Name = "fps";
			this.fps.Size = new System.Drawing.Size(128, 21);
			this.fps.TabIndex = 7;
			this.fps.Text = "";
			this.fps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// fpsLabel
			// 
			this.fpsLabel.Location = new System.Drawing.Point(16, 96);
			this.fpsLabel.Name = "fpsLabel";
			this.fpsLabel.Size = new System.Drawing.Size(112, 23);
			this.fpsLabel.TabIndex = 6;
			this.fpsLabel.Text = "Video rendering rate";
			// 
			// filesize
			// 
			this.filesize.Enabled = false;
			this.filesize.Location = new System.Drawing.Point(136, 72);
			this.filesize.Name = "filesize";
			this.filesize.Size = new System.Drawing.Size(128, 21);
			this.filesize.TabIndex = 5;
			this.filesize.Text = "";
			this.filesize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// filesizeLabel
			// 
			this.filesizeLabel.Location = new System.Drawing.Point(16, 72);
			this.filesizeLabel.Name = "filesizeLabel";
			this.filesizeLabel.TabIndex = 4;
			this.filesizeLabel.Text = "Projected Filesize";
			// 
			// videoData
			// 
			this.videoData.Enabled = false;
			this.videoData.Location = new System.Drawing.Point(136, 48);
			this.videoData.Name = "videoData";
			this.videoData.Size = new System.Drawing.Size(128, 21);
			this.videoData.TabIndex = 3;
			this.videoData.Text = "";
			this.videoData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// videoDataLabel
			// 
			this.videoDataLabel.Location = new System.Drawing.Point(16, 48);
			this.videoDataLabel.Name = "videoDataLabel";
			this.videoDataLabel.TabIndex = 2;
			this.videoDataLabel.Text = "Video data:";
			// 
			// abortButton
			// 
			this.abortButton.Location = new System.Drawing.Point(200, 256);
			this.abortButton.Name = "abortButton";
			this.abortButton.TabIndex = 3;
			this.abortButton.Text = "Abort";
			this.abortButton.Click += new System.EventHandler(this.abortButton_Click);
			// 
			// progressLabel
			// 
			this.progressLabel.Location = new System.Drawing.Point(16, 192);
			this.progressLabel.Name = "progressLabel";
			this.progressLabel.TabIndex = 4;
			this.progressLabel.Text = "Progress";
			// 
			// progress
			// 
			this.progress.Location = new System.Drawing.Point(144, 192);
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(128, 23);
			this.progress.TabIndex = 5;
			// 
			// priorityLabel
			// 
			this.priorityLabel.Location = new System.Drawing.Point(16, 224);
			this.priorityLabel.Name = "priorityLabel";
			this.priorityLabel.TabIndex = 6;
			this.priorityLabel.Text = "Priority";
			// 
			// priority
			// 
			this.priority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.priority.Items.AddRange(new object[] {
														  "LOW",
														  "NORMAL",
														  "HIGH"});
			this.priority.Location = new System.Drawing.Point(192, 224);
			this.priority.Name = "priority";
			this.priority.Size = new System.Drawing.Size(80, 21);
			this.priority.TabIndex = 7;
			this.priority.SelectedIndexChanged += new System.EventHandler(this.priority_SelectedIndexChanged);
			// 
			// ProgressWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(292, 286);
			this.Controls.Add(this.priority);
			this.Controls.Add(this.priorityLabel);
			this.Controls.Add(this.progress);
			this.Controls.Add(this.progressLabel);
			this.Controls.Add(this.abortButton);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "ProgressWindow";
			this.Text = "Status";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
        #region statusupdate processing
        /// <summary>
		/// catches the StatusUpdate event fired from Form1 and updates the GUI accordingly
		/// </summary>
		/// <param name="su"></param>
		public void UpdateStatus(StatusUpdate su)
		{
			if (su.JobType == JobTypes.VIDEO) // video status update
			{
				this.currentVideoFrame.Text = su.NbFramesDone + "/" + su.NbFramesTotal;
				this.videoData.Text = su.FileSize.ToString() + " KB";
				long projectedSize = (long)((double)su.FileSize/su.PercentageDoneExact * (double)100);
				if (projectedSize < 0)
					this.filesize.Text = "unknown";
				else
					this.filesize.Text = projectedSize.ToString() + " KB";
                this.fps.Text = su.FPS.ToString("##.##") + " FPS";
				this.timeElapsed.Text = su.TimeElapsedString;
				this.totalTime.Text = getTimeString(su.TimeElapsed, su.PercentageDoneExact);
				this.progress.Value = su.PercentageDone;
				this.Text = "Status: " + su.PercentageDoneExact.ToString("##.##") + " %";
			}
			if (su.JobType == JobTypes.AUDIO) // audio status update
			{
				this.currentVideoFrame.Text = su.AudioPosition;
				this.videoData.Text = su.FileSize.ToString() + " KB";
                if (su.FileSize == 0) // first pass
                {
                    this.filesize.Text = "N/A (first pass)";
                    this.totalTime.Text = "N/A (first pass)";
                    this.Text = "Status: first pass";
                }
                else
				{
					long projectedSize = (long)((double)su.FileSize/su.PercentageDoneExact * (double)100);
					this.filesize.Text = projectedSize.ToString() + " KB";
					this.totalTime.Text = getTimeString(su.TimeElapsed, su.PercentageDoneExact);
					this.progress.Value = su.PercentageDone;
                    this.Text = "Status: " + su.PercentageDoneExact.ToString("##.##") + " %";
				}
				this.timeElapsed.Text = su.TimeElapsedString;
			}
			if (su.JobType == JobTypes.MUX) // mux status update
			{
				this.currentVideoFrame.Text = su.AudioFileSize.ToString() + " KB"; // audio data
				this.videoData.Text = su.FileSize.ToString() + " KB";
				this.filesize.Text = su.ProjectedFileSize.ToString() + " KB";
				this.fps.Text = su.AudioPosition;
                this.Text = "Status: " + su.PercentageDoneExact.ToString("##.##") + " %";
				this.progress.Value = su.PercentageDone;
				this.timeElapsed.Text = su.TimeElapsedString;
			}
            if (su.JobType == JobTypes.AVS) // video status update
            {
                this.currentVideoFrame.Text = su.NbFramesDone + "/" + su.NbFramesTotal;
                this.videoData.Text = "N/A";
                this.filesize.Text = "N/A";
                this.fps.Text = su.FPS.ToString("##.##") + " FPS";
                this.timeElapsed.Text = su.TimeElapsedString;
                this.totalTime.Text = getTimeString(su.TimeElapsed, su.PercentageDoneExact);
                this.progress.Value = su.PercentageDone;
                this.Text = "Status: " + su.PercentageDoneExact.ToString("##.##") + " %";
            }
        }
        #endregion
        #region helper methods
        /// <summary>
		/// calculates the remaining encoding time from the elapsed timespan and the percentage the job is done
		/// </summary>
		/// <param name="span">timespan elapsed since the start of the job</param>
		/// <param name="percentageDone">percentage the job is currently done</param>
		/// <returns>presentable string in hh:mm:ss format</returns>
		private string getTimeString(long span, double percentageDone)
		{
			if (percentageDone == 0)
				return "n/a";
			else
			{
				long ratio = (long)((double)span / percentageDone * (double)100);
				TimeSpan t = new TimeSpan(ratio - span);
                string retval = t.Days + ":";
                if (t.Hours < 10)
                    retval += "0";
                retval += t.Hours + ":";
				if (t.Minutes < 10)
					retval += "0";
				retval += t.Minutes + ":";
				if (t.Seconds < 10)
					retval += "0";
				retval += t.Seconds;
				return retval;
			}
		}
        /// <summary>
        /// sets the priority
        /// </summary>
        /// <param name="priority"></param>
        public void setPriority(ProcessPriority priority)
        {
            this.priority.SelectedIndex = (int)priority;
        }
        #endregion
        #region events
        /// <summary>
		///  handles the abort button
		///  fires an Abort() event to the main GUI, which looks up the encoder and makes it stop
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void abortButton_Click(object sender, System.EventArgs e)
		{
			DialogResult dr = MessageBox.Show("Are you sure you want to abort encoding?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dr == DialogResult.Yes)
				Abort();
		}
		/// <summary>
		/// handles changes in the priority dropdwon
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void priority_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (PriorityChanged != null)
            {
                PriorityChanged((ProcessPriority)priority.SelectedIndex);
            }
        }
        #endregion
        #region properties
        /// <summary>
		/// gets / sets whether the user closed this window or if the system is closing it
		/// </summary>
		public bool IsUserAbort
		{
			get {return isUserClosing;}
			set {isUserClosing = value;}
        }
        #endregion
    }
}