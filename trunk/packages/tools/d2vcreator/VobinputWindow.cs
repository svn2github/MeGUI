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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace MeGUI
{
    public class D2VCreatorTool : MeGUI.core.plugins.interfaces.ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "D2V Creator"; }
        }

        public void Run(MainForm info)
        {
            VobinputWindow vobInput = new VobinputWindow(info);
            vobInput.ShowDialog();

        }

        public string[] Shortcuts
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "d2v_creator"; }
        }

        #endregion
    }

    public class IndexJobPostProcessor
    {
        public static JobPostProcessor PostProcessor = new JobPostProcessor(postprocess, "D2V_postprocessor");
        private static void postprocess(MainForm mainForm, Job ajob)
        {
            if (!(ajob is IndexJob)) return;
            IndexJob job = (IndexJob)ajob;
            if (job.PostprocessingProperties != null) return;

            StringBuilder logBuilder = new StringBuilder();
            VideoUtil vUtil = new VideoUtil(mainForm);
            Dictionary<int, string> audioFiles = vUtil.getAllDemuxedAudio(job.Output, 8);
            if (job.LoadSources)
            {
                if (job.DemuxMode != 0)
                {
                    int counter = 0;
                    foreach (int i in audioFiles.Keys)
                    {
                        mainForm.setAudioTrack(counter, audioFiles[i]);
                        if (counter >= 2)
                            break;
                        counter++;
                    }
                }
                AviSynthWindow asw = new AviSynthWindow(mainForm, job.Output);
                asw.OpenScript += new OpenScriptCallback(mainForm.Video.openVideoFile);
                asw.Show();
            }
        }
    }

    public delegate void ProjectCreationComplete(); // this event is fired when the dgindex thread finishes
	/// <summary>
	/// Summary description for Vobinput.
	/// </summary>
	public class VobinputWindow : System.Windows.Forms.Form
	{
		#region variables
        private IndexJob lastJob = null;

        private bool dialogMode = false; // $%£%$^>*"%$%%$#{"!!! Affects the public behaviour!
		private bool configured = false;
		private bool processing = false;
		private MainForm mainForm;
		private VideoUtil vUtil;
		private JobUtil jobUtil;

		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button openButton;
		private System.Windows.Forms.TextBox input;
		private System.Windows.Forms.Label inputLabel;
		private System.Windows.Forms.TextBox projectName;
		private System.Windows.Forms.Label projectNameLabel;
		private System.Windows.Forms.SaveFileDialog saveProjectDialog;
		private System.Windows.Forms.OpenFileDialog openIFODialog;
		private System.Windows.Forms.Button pickOutputButton;
		private System.Windows.Forms.GroupBox inputGroupbox;
		private System.Windows.Forms.RadioButton demuxAllTracks;
		private System.Windows.Forms.RadioButton demuxSelectedTracks;
		private System.Windows.Forms.ComboBox track1;
		private System.Windows.Forms.ComboBox track2;
		private System.Windows.Forms.Label track1Label;
		private System.Windows.Forms.Label track2Label;
		private System.Windows.Forms.RadioButton demuxNoAudiotracks;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button clearAudio1Button;
		private System.Windows.Forms.Button queueButton;
		private System.Windows.Forms.CheckBox loadOnComplete;
		private System.Windows.Forms.CheckBox closeOnQueue;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		#region start / stop
		public void setConfig(string input, string projectName, int demuxType, int track1Index, int track2Index,
            bool showCloseOnQueue, bool closeOnQueue, bool loadOnComplete, bool updateMode)
		{
			openVideo(input);
			this.input.Text = input;
			this.projectName.Text = projectName;
			if (demuxType == 0)
				demuxNoAudiotracks.Checked = true;
			else if (demuxType == 1)
				demuxSelectedTracks.Checked = true;
			else
				demuxAllTracks.Checked = true;
			track1.SelectedIndex = track1Index;
			track2.SelectedIndex = track2Index;
			this.loadOnComplete.Checked = loadOnComplete;
            if (updateMode)
            {
                this.dialogMode = true;
                queueButton.Text = "Update";
            }
            else
                this.dialogMode = false;
			checkIndexIO();
            if (!showCloseOnQueue)
            {
                this.closeOnQueue.Hide();
                this.Controls.Remove(this.closeOnQueue);
            }
            this.closeOnQueue.Checked = closeOnQueue;

		}
		
		public VobinputWindow(MainForm mainForm)
		{
			InitializeComponent();
			this.mainForm = mainForm;
			this.vUtil = new VideoUtil(mainForm);
			this.jobUtil = new JobUtil(mainForm);
			this.Closing += new CancelEventHandler(VobinputWindow_Closing);
		}

		public VobinputWindow(MainForm mainForm, string fileName): this(mainForm)
		{
			openVideo(fileName);
		}

        public VobinputWindow(MainForm mainForm, string fileName, bool autoReturn) : this(mainForm, fileName)
        {
            openVideo(fileName);
            this.loadOnComplete.Checked = true;
            this.closeOnQueue.Checked = true;
            projectName.Text = Path.ChangeExtension(fileName, ".d2v");
            checkIndexIO();
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
		/// <summary>
		///  prevents the form from closing if we're still processing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void VobinputWindow_Closing(object sender, CancelEventArgs e)
		{
			if (this.processing)
				e.Cancel = true;
		}
		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.inputGroupbox = new System.Windows.Forms.GroupBox();
            this.openButton = new System.Windows.Forms.Button();
            this.input = new System.Windows.Forms.TextBox();
            this.inputLabel = new System.Windows.Forms.Label();
            this.queueButton = new System.Windows.Forms.Button();
            this.loadOnComplete = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.clearAudio1Button = new System.Windows.Forms.Button();
            this.demuxNoAudiotracks = new System.Windows.Forms.RadioButton();
            this.track2Label = new System.Windows.Forms.Label();
            this.track1Label = new System.Windows.Forms.Label();
            this.track2 = new System.Windows.Forms.ComboBox();
            this.track1 = new System.Windows.Forms.ComboBox();
            this.demuxSelectedTracks = new System.Windows.Forms.RadioButton();
            this.demuxAllTracks = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pickOutputButton = new System.Windows.Forms.Button();
            this.projectName = new System.Windows.Forms.TextBox();
            this.projectNameLabel = new System.Windows.Forms.Label();
            this.saveProjectDialog = new System.Windows.Forms.SaveFileDialog();
            this.openIFODialog = new System.Windows.Forms.OpenFileDialog();
            this.closeOnQueue = new System.Windows.Forms.CheckBox();
            this.inputGroupbox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputGroupbox
            // 
            this.inputGroupbox.Controls.Add(this.openButton);
            this.inputGroupbox.Controls.Add(this.input);
            this.inputGroupbox.Controls.Add(this.inputLabel);
            this.inputGroupbox.Location = new System.Drawing.Point(10, 8);
            this.inputGroupbox.Name = "inputGroupbox";
            this.inputGroupbox.Size = new System.Drawing.Size(424, 48);
            this.inputGroupbox.TabIndex = 0;
            this.inputGroupbox.TabStop = false;
            this.inputGroupbox.Text = "Video Input";
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(382, 16);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(24, 23);
            this.openButton.TabIndex = 2;
            this.openButton.Text = "...";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // input
            // 
            this.input.Location = new System.Drawing.Point(118, 17);
            this.input.Name = "input";
            this.input.ReadOnly = true;
            this.input.Size = new System.Drawing.Size(256, 21);
            this.input.TabIndex = 1;
            // 
            // inputLabel
            // 
            this.inputLabel.Location = new System.Drawing.Point(16, 20);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(100, 13);
            this.inputLabel.TabIndex = 0;
            this.inputLabel.Text = "Video Input";
            // 
            // queueButton
            // 
            this.queueButton.Location = new System.Drawing.Point(360, 264);
            this.queueButton.Name = "queueButton";
            this.queueButton.Size = new System.Drawing.Size(74, 23);
            this.queueButton.TabIndex = 10;
            this.queueButton.Text = "Queue";
            this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
            // 
            // loadOnComplete
            // 
            this.loadOnComplete.Location = new System.Drawing.Point(8, 264);
            this.loadOnComplete.Name = "loadOnComplete";
            this.loadOnComplete.Size = new System.Drawing.Size(144, 24);
            this.loadOnComplete.TabIndex = 11;
            this.loadOnComplete.Text = "On completion load files";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.clearAudio1Button);
            this.groupBox3.Controls.Add(this.demuxNoAudiotracks);
            this.groupBox3.Controls.Add(this.track2Label);
            this.groupBox3.Controls.Add(this.track1Label);
            this.groupBox3.Controls.Add(this.track2);
            this.groupBox3.Controls.Add(this.track1);
            this.groupBox3.Controls.Add(this.demuxSelectedTracks);
            this.groupBox3.Controls.Add(this.demuxAllTracks);
            this.groupBox3.Location = new System.Drawing.Point(10, 56);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(424, 152);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Audio";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(382, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "X";
            this.button1.Click += new System.EventHandler(this.clearAudio2Button_Click);
            // 
            // clearAudio1Button
            // 
            this.clearAudio1Button.Location = new System.Drawing.Point(382, 94);
            this.clearAudio1Button.Name = "clearAudio1Button";
            this.clearAudio1Button.Size = new System.Drawing.Size(24, 23);
            this.clearAudio1Button.TabIndex = 19;
            this.clearAudio1Button.Text = "X";
            this.clearAudio1Button.Click += new System.EventHandler(this.clearAudio1Button_Click);
            // 
            // demuxNoAudiotracks
            // 
            this.demuxNoAudiotracks.Location = new System.Drawing.Point(16, 16);
            this.demuxNoAudiotracks.Name = "demuxNoAudiotracks";
            this.demuxNoAudiotracks.Size = new System.Drawing.Size(120, 24);
            this.demuxNoAudiotracks.TabIndex = 13;
            this.demuxNoAudiotracks.Text = "No Audio demux";
            this.demuxNoAudiotracks.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // track2Label
            // 
            this.track2Label.Location = new System.Drawing.Point(16, 120);
            this.track2Label.Name = "track2Label";
            this.track2Label.Size = new System.Drawing.Size(72, 23);
            this.track2Label.TabIndex = 12;
            this.track2Label.Text = "Track 2";
            // 
            // track1Label
            // 
            this.track1Label.Location = new System.Drawing.Point(16, 96);
            this.track1Label.Name = "track1Label";
            this.track1Label.Size = new System.Drawing.Size(72, 23);
            this.track1Label.TabIndex = 11;
            this.track1Label.Text = "Track1";
            // 
            // track2
            // 
            this.track2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.track2.Location = new System.Drawing.Point(118, 120);
            this.track2.Name = "track2";
            this.track2.Size = new System.Drawing.Size(256, 21);
            this.track2.TabIndex = 10;
            // 
            // track1
            // 
            this.track1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.track1.Location = new System.Drawing.Point(118, 96);
            this.track1.Name = "track1";
            this.track1.Size = new System.Drawing.Size(256, 21);
            this.track1.TabIndex = 9;
            // 
            // demuxSelectedTracks
            // 
            this.demuxSelectedTracks.Checked = true;
            this.demuxSelectedTracks.Location = new System.Drawing.Point(16, 64);
            this.demuxSelectedTracks.Name = "demuxSelectedTracks";
            this.demuxSelectedTracks.Size = new System.Drawing.Size(336, 24);
            this.demuxSelectedTracks.TabIndex = 8;
            this.demuxSelectedTracks.TabStop = true;
            this.demuxSelectedTracks.Text = "Select Audio Streams to demux (Stream Info File required)";
            this.demuxSelectedTracks.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // demuxAllTracks
            // 
            this.demuxAllTracks.Location = new System.Drawing.Point(16, 40);
            this.demuxAllTracks.Name = "demuxAllTracks";
            this.demuxAllTracks.Size = new System.Drawing.Size(160, 24);
            this.demuxAllTracks.TabIndex = 7;
            this.demuxAllTracks.Text = "Demux all Audio Tracks";
            this.demuxAllTracks.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pickOutputButton);
            this.groupBox2.Controls.Add(this.projectName);
            this.groupBox2.Controls.Add(this.projectNameLabel);
            this.groupBox2.Location = new System.Drawing.Point(8, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 49);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Video";
            // 
            // pickOutputButton
            // 
            this.pickOutputButton.Location = new System.Drawing.Point(384, 16);
            this.pickOutputButton.Name = "pickOutputButton";
            this.pickOutputButton.Size = new System.Drawing.Size(24, 23);
            this.pickOutputButton.TabIndex = 5;
            this.pickOutputButton.Text = "...";
            this.pickOutputButton.Click += new System.EventHandler(this.pickOutputButton_Click);
            // 
            // projectName
            // 
            this.projectName.Location = new System.Drawing.Point(120, 17);
            this.projectName.Name = "projectName";
            this.projectName.ReadOnly = true;
            this.projectName.Size = new System.Drawing.Size(256, 21);
            this.projectName.TabIndex = 4;
            // 
            // projectNameLabel
            // 
            this.projectNameLabel.Location = new System.Drawing.Point(16, 20);
            this.projectNameLabel.Name = "projectNameLabel";
            this.projectNameLabel.Size = new System.Drawing.Size(100, 13);
            this.projectNameLabel.TabIndex = 3;
            this.projectNameLabel.Text = "d2v Project Ouput";
            // 
            // saveProjectDialog
            // 
            this.saveProjectDialog.Filter = "DGIndex project files|*.d2v";
            this.saveProjectDialog.Title = "Pick a name for your DGIndex project";
            // 
            // openIFODialog
            // 
            this.openIFODialog.Filter = "VOB Files (*.vob)|*.vob|MPEG-1/2 Program Streams (*.mpg)|*.mpg|Transport Streams " +
                "(*.ts)|*.ts|All DGIndex supported files|*.vob;*.mpg;*.mpeg;*.m2v;*.mpv;*.tp;*.ts" +
                ";*.trp;*.pva;*.vro";
            this.openIFODialog.FilterIndex = 4;
            // 
            // closeOnQueue
            // 
            this.closeOnQueue.Location = new System.Drawing.Point(280, 264);
            this.closeOnQueue.Name = "closeOnQueue";
            this.closeOnQueue.Size = new System.Drawing.Size(72, 24);
            this.closeOnQueue.TabIndex = 13;
            this.closeOnQueue.Text = "and close";
            // 
            // VobinputWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(444, 292);
            this.Controls.Add(this.closeOnQueue);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.loadOnComplete);
            this.Controls.Add(this.queueButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.inputGroupbox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "VobinputWindow";
            this.Text = "DGIndex Project Creator";
            this.inputGroupbox.ResumeLayout(false);
            this.inputGroupbox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
		#region buttons
		private void pickOutputButton_Click(object sender, System.EventArgs e)
		{
			if (!processing)
			{
				if (saveProjectDialog.ShowDialog() == DialogResult.OK)
				{
					projectName.Text = saveProjectDialog.FileName;
					checkIndexIO();
				}
			}
		}

		private void openButton_Click(object sender, System.EventArgs e)
		{
			if (!processing)
			{
				if (openIFODialog.ShowDialog() == DialogResult.OK)
				{
					openVideo(openIFODialog.FileName);
					projectName.Text = Path.ChangeExtension(openIFODialog.FileName, ".d2v");
					checkIndexIO();
				}
			}
		}
		private void openVideo(string fileName)
		{
			input.Text = fileName;
			track1.Items.Clear();
			track2.Items.Clear();
			AspectRatio ar;
            int maxHorizontalResolution;
            List<AudioTrackInfo> audioTracks;
            List<SubtitleInfo> subtitles;
            int pgc;
            demuxSelectedTracks.Checked = vUtil.openVideoSource(fileName, out audioTracks, out subtitles, out ar, out maxHorizontalResolution, out pgc);
            track1.Items.AddRange(audioTracks.ToArray());
            track2.Items.AddRange(audioTracks.ToArray());
            foreach (AudioTrackInfo ati in audioTracks)
            {
                if (ati.Language.ToLower().Equals(mainForm.Settings.DefaultLanguage1.ToLower()))
                {
                    track1.SelectedItem = ati;
                    continue;
                }
                if (ati.Language.ToLower().Equals(mainForm.Settings.DefaultLanguage2.ToLower()))
                {
                    track2.SelectedItem = ati;
                    continue;
                }
            }
			demuxSelectedTracks.Checked = !demuxAllTracks.Checked;
		}
		/// <summary>
		/// creates a dgindex project
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void queueButton_Click(object sender, System.EventArgs e)
		{
			if (configured)
			{
				if (!dialogMode)
				{
					IndexJob job = generateIndexJob();
					int jobNumber = mainForm.Jobs.getFreeJobNumber();
					job.Name = "job" + jobNumber;
                    lastJob = job;
					mainForm.Jobs.addJobToQueue(job);
                    if (mainForm.Settings.AutoStartQueue)
                        mainForm.Jobs.startEncoding(job);
					if (this.closeOnQueue.Checked)
						this.Close();
				}
			}
			else
				MessageBox.Show("You must select a Video Input and DGIndex project file to continue", 
					"Configuration incomplete", MessageBoxButtons.OK);
		}
		private void clearAudio1Button_Click(object sender, System.EventArgs e)
		{
			track1.SelectedIndex = -1;
		}

		private void clearAudio2Button_Click(object sender, System.EventArgs e)
		{
			track2.SelectedIndex = -1;
		}
		#endregion
		#region radio buttons
		private void radioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			if (demuxSelectedTracks.Checked)
			{
				track1.Enabled = true;
				track2.Enabled = true;
			}
			else
			{
				track1.Enabled = false;
				track2.Enabled = false;
			}
		}
		#endregion
		#region helper methods
		private void checkIndexIO()
		{
			configured = (!input.Text.Equals("") && !projectName.Text.Equals(""));
			if (configured && dialogMode)
				queueButton.DialogResult = DialogResult.OK;
			else
                queueButton.DialogResult = DialogResult.None;
		}
		private IndexJob generateIndexJob()
		{
			int demuxType = 0;
			if (demuxAllTracks.Checked)
				demuxType = 2;
			else if (demuxSelectedTracks.Checked)
				demuxType = 1;
			IndexJob job = jobUtil.generateIndexJob(this.input.Text, this.projectName.Text, demuxType, 
				track1.SelectedIndex, track2.SelectedIndex, null);
			if (this.loadOnComplete.Checked)
				job.LoadSources = true;
			return job;
		}
		#endregion
		#region properties
		/// <summary>
		/// gets the index job created from the current configuration
		/// </summary>
		public IndexJob Job
		{
			get {return generateIndexJob();}
		}
        
        public IndexJob LastJob
        {
            get { return lastJob; }
            set { lastJob = value; }
        }
        public bool JobCreated
        {
            get { return lastJob != null; }
        }
        #endregion
    }
}
