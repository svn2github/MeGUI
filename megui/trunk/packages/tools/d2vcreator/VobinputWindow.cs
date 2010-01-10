// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI
{

	/// <summary>
	/// Summary description for Vobinput.
	/// </summary>
	public class VobinputWindow : System.Windows.Forms.Form
	{ 
		#region variables
        private IndexJob lastJob = null;

        private enum DGIndexType
        {
            D2V, DGA, DGI, D2VorDGA
        };
        private DGIndexType DGIndexerUsed = DGIndexType.DGI;

        private bool dialogMode = false; // $%£%$^>*"%$%%$#{"!!! Affects the public behaviour!
		private bool configured = false;
		private bool processing = false;
		private MainForm mainForm;
		private VideoUtil vUtil;
		private JobUtil jobUtil;

		private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox gbOutput;
		private System.Windows.Forms.Label inputLabel;
		private System.Windows.Forms.TextBox projectName;
		private System.Windows.Forms.Label projectNameLabel;
        private System.Windows.Forms.SaveFileDialog saveProjectDialog;
		private System.Windows.Forms.Button pickOutputButton;
		private System.Windows.Forms.GroupBox gbInput;
        private System.Windows.Forms.RadioButton demuxTracks;
        private System.Windows.Forms.RadioButton demuxNoAudiotracks;
		private System.Windows.Forms.Button queueButton;
		private System.Windows.Forms.CheckBox loadOnComplete;
		private System.Windows.Forms.CheckBox closeOnQueue;
        private MeGUI.core.gui.HelpButton helpButton1;
        private CheckedListBox AudioTracks;
        private RadioButton demuxAll;
        private FileBar input;
        private CheckBox demuxVideo;
        private GroupBox groupBox1;
        private RadioButton btnDGI;
        private RadioButton btnDGA;
        private Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		#region start / stop
		public void setConfig(string input, string projectName, int demuxType,
            bool showCloseOnQueue, bool closeOnQueue, bool loadOnComplete, bool updateMode)
		{
			openVideo(input);
			if (!string.IsNullOrEmpty(projectName))
                this.projectName.Text = projectName;
			if (demuxType == 0)
				demuxNoAudiotracks.Checked = true;
			else
				demuxTracks.Checked = true;
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

            if (File.Exists(MainForm.Instance.Settings.DgnvIndexPath) &&
                File.Exists(Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.DgnvIndexPath),"license.txt")))
            {
                btnDGI.Checked = true;
            }
            else
            {
                btnDGA.Checked = true;
                btnDGI.Enabled = false;
            }
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
            checkIndexIO();
        }

        private void changeDGIndexer(DGIndexType dgType)
        {
            switch (dgType)
            {
                case DGIndexType.DGI:
                {
                    this.Text = "MeGUI - DGI Project Creator";
                    this.saveProjectDialog.Filter = "DGIndexNV project files|*.dgi";
                    this.input.Filter = "All DGIndexNV supported files|*.264;*.h264;*.avc;*.m2v;*.mpv;*.vc1;*.mkv;*.vob;*.mpg;*.mpeg;*.m2t;*.m2ts;*.mts;*.tp;*.ts;*.trp";
                    this.projectNameLabel.Text = "dgi Project Output";
                    if (this.demuxTracks.Checked)
                        this.demuxAll.Checked = true;
                    this.demuxTracks.Enabled = false;
                    DGIndexerUsed = DGIndexType.DGI;
                    break;
                }
                case DGIndexType.DGA:
                {
                    this.Text = "MeGUI - D2V/DGA Project Creator";
                    this.input.Filter = "All DGIndex / DGAVCIndex supported files|*.264;*.h264;*.avc;*.vob;*.mpg;*.mpeg;*.m2t*;*.m1v;*.m2v;*.m2p;*.mpv;*.tp;*.ts;*.trp;*.pva;*.vro";
                    this.saveProjectDialog.Filter = "DGAVCIndex project files|*.dga";
                    this.projectNameLabel.Text = "dga Project Output";
                    DGIndexerUsed = DGIndexType.DGA;
                    break;
                }
                case DGIndexType.D2V:
                {
                    this.Text = "MeGUI - D2V/DGA Project Creator";
                    this.input.Filter = "All DGIndex / DGAVCIndex supported files|*.264;*.h264;*.avc;*.vob;*.mpg;*.mpeg;*.m2t*;*.m1v;*.m2v;*.m2p;*.mpv;*.tp;*.ts;*.trp;*.pva;*.vro";
                    this.saveProjectDialog.Filter = "DGIndex project files|*.d2v";
                    this.projectNameLabel.Text = "d2v Project Output";
                    DGIndexerUsed = DGIndexType.D2V;
                    break;
                }
                case DGIndexType.D2VorDGA:
                {
                    this.Text = "MeGUI - D2V/DGA Project Creator";
                    this.input.Filter = "All DGIndex / DGAVCIndex supported files|*.264;*.h264;*.avc;*.vob;*.mpg;*.mpeg;*.m2t*;*.m1v;*.m2v;*.m2p;*.mpv;*.tp;*.ts;*.trp;*.pva;*.vro";
                    this.saveProjectDialog.Filter = "DGIndex project files|*.d2v";
                    this.projectNameLabel.Text = "d2v/dga Project Output";
                    DGIndexerUsed = DGIndexType.D2VorDGA;
                    break;
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VobinputWindow));
            this.gbInput = new System.Windows.Forms.GroupBox();
            this.input = new MeGUI.FileBar();
            this.inputLabel = new System.Windows.Forms.Label();
            this.queueButton = new System.Windows.Forms.Button();
            this.loadOnComplete = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.demuxAll = new System.Windows.Forms.RadioButton();
            this.AudioTracks = new System.Windows.Forms.CheckedListBox();
            this.demuxNoAudiotracks = new System.Windows.Forms.RadioButton();
            this.demuxTracks = new System.Windows.Forms.RadioButton();
            this.gbOutput = new System.Windows.Forms.GroupBox();
            this.demuxVideo = new System.Windows.Forms.CheckBox();
            this.pickOutputButton = new System.Windows.Forms.Button();
            this.projectName = new System.Windows.Forms.TextBox();
            this.projectNameLabel = new System.Windows.Forms.Label();
            this.saveProjectDialog = new System.Windows.Forms.SaveFileDialog();
            this.closeOnQueue = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDGA = new System.Windows.Forms.RadioButton();
            this.btnDGI = new System.Windows.Forms.RadioButton();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.gbInput.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbOutput.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInput
            // 
            this.gbInput.Controls.Add(this.input);
            this.gbInput.Controls.Add(this.inputLabel);
            this.gbInput.Location = new System.Drawing.Point(10, 50);
            this.gbInput.Name = "gbInput";
            this.gbInput.Size = new System.Drawing.Size(424, 50);
            this.gbInput.TabIndex = 0;
            this.gbInput.TabStop = false;
            this.gbInput.Text = " Input ";
            // 
            // input
            // 
            this.input.AllowDrop = true;
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.input.Filename = "";
            this.input.Filter = resources.GetString("input.Filter");
            this.input.FilterIndex = 4;
            this.input.FolderMode = false;
            this.input.Location = new System.Drawing.Point(79, 12);
            this.input.Name = "input";
            this.input.ReadOnly = true;
            this.input.SaveMode = false;
            this.input.Size = new System.Drawing.Size(329, 34);
            this.input.TabIndex = 4;
            this.input.Title = null;
            this.input.FileSelected += new MeGUI.FileBarEventHandler(this.input_FileSelected);
            // 
            // inputLabel
            // 
            this.inputLabel.Location = new System.Drawing.Point(16, 22);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(100, 13);
            this.inputLabel.TabIndex = 0;
            this.inputLabel.Text = "Input File";
            // 
            // queueButton
            // 
            this.queueButton.Location = new System.Drawing.Point(365, 373);
            this.queueButton.Name = "queueButton";
            this.queueButton.Size = new System.Drawing.Size(74, 23);
            this.queueButton.TabIndex = 10;
            this.queueButton.Text = "Queue";
            this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
            // 
            // loadOnComplete
            // 
            this.loadOnComplete.Checked = true;
            this.loadOnComplete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadOnComplete.Location = new System.Drawing.Point(64, 372);
            this.loadOnComplete.Name = "loadOnComplete";
            this.loadOnComplete.Size = new System.Drawing.Size(144, 24);
            this.loadOnComplete.TabIndex = 11;
            this.loadOnComplete.Text = "On completion load files";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.demuxAll);
            this.groupBox3.Controls.Add(this.AudioTracks);
            this.groupBox3.Controls.Add(this.demuxNoAudiotracks);
            this.groupBox3.Controls.Add(this.demuxTracks);
            this.groupBox3.Location = new System.Drawing.Point(10, 106);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(424, 185);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Audio ";
            // 
            // demuxAll
            // 
            this.demuxAll.Location = new System.Drawing.Point(303, 16);
            this.demuxAll.Name = "demuxAll";
            this.demuxAll.Size = new System.Drawing.Size(115, 24);
            this.demuxAll.TabIndex = 15;
            this.demuxAll.TabStop = true;
            this.demuxAll.Text = "Demux All Tracks";
            this.demuxAll.UseVisualStyleBackColor = true;
            this.demuxAll.CheckedChanged += new System.EventHandler(this.rbtracks_CheckedChanged);
            // 
            // AudioTracks
            // 
            this.AudioTracks.CheckOnClick = true;
            this.AudioTracks.Enabled = false;
            this.AudioTracks.FormattingEnabled = true;
            this.AudioTracks.Location = new System.Drawing.Point(16, 43);
            this.AudioTracks.Name = "AudioTracks";
            this.AudioTracks.Size = new System.Drawing.Size(390, 132);
            this.AudioTracks.TabIndex = 14;
            // 
            // demuxNoAudiotracks
            // 
            this.demuxNoAudiotracks.Checked = true;
            this.demuxNoAudiotracks.Location = new System.Drawing.Point(142, 16);
            this.demuxNoAudiotracks.Name = "demuxNoAudiotracks";
            this.demuxNoAudiotracks.Size = new System.Drawing.Size(120, 24);
            this.demuxNoAudiotracks.TabIndex = 13;
            this.demuxNoAudiotracks.TabStop = true;
            this.demuxNoAudiotracks.Text = "No Audio demux";
            this.demuxNoAudiotracks.CheckedChanged += new System.EventHandler(this.rbtracks_CheckedChanged);
            // 
            // demuxTracks
            // 
            this.demuxTracks.Enabled = false;
            this.demuxTracks.Location = new System.Drawing.Point(19, 16);
            this.demuxTracks.Name = "demuxTracks";
            this.demuxTracks.Size = new System.Drawing.Size(120, 24);
            this.demuxTracks.TabIndex = 7;
            this.demuxTracks.Text = "Select Audio Tracks";
            this.demuxTracks.CheckedChanged += new System.EventHandler(this.rbtracks_CheckedChanged);
            // 
            // gbOutput
            // 
            this.gbOutput.Controls.Add(this.demuxVideo);
            this.gbOutput.Controls.Add(this.pickOutputButton);
            this.gbOutput.Controls.Add(this.projectName);
            this.gbOutput.Controls.Add(this.projectNameLabel);
            this.gbOutput.Location = new System.Drawing.Point(10, 297);
            this.gbOutput.Name = "gbOutput";
            this.gbOutput.Size = new System.Drawing.Size(424, 69);
            this.gbOutput.TabIndex = 12;
            this.gbOutput.TabStop = false;
            this.gbOutput.Text = " Output ";
            // 
            // demuxVideo
            // 
            this.demuxVideo.AutoSize = true;
            this.demuxVideo.Location = new System.Drawing.Point(19, 46);
            this.demuxVideo.Name = "demuxVideo";
            this.demuxVideo.Size = new System.Drawing.Size(125, 17);
            this.demuxVideo.TabIndex = 6;
            this.demuxVideo.Text = "Demux Video Stream";
            this.demuxVideo.UseVisualStyleBackColor = true;
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
            this.projectNameLabel.Text = "d2v Project Output";
            // 
            // saveProjectDialog
            // 
            this.saveProjectDialog.Filter = "DGIndex project files|*.d2v";
            this.saveProjectDialog.Title = "Pick a name for your DGIndex project";
            // 
            // closeOnQueue
            // 
            this.closeOnQueue.Checked = true;
            this.closeOnQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.closeOnQueue.Location = new System.Drawing.Point(285, 373);
            this.closeOnQueue.Name = "closeOnQueue";
            this.closeOnQueue.Size = new System.Drawing.Size(72, 24);
            this.closeOnQueue.TabIndex = 13;
            this.closeOnQueue.Text = "and close";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnDGA);
            this.groupBox1.Controls.Add(this.btnDGI);
            this.groupBox1.Location = new System.Drawing.Point(10, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 38);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Indexer ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select DG Indexer";
            // 
            // btnDGA
            // 
            this.btnDGA.AutoSize = true;
            this.btnDGA.Location = new System.Drawing.Point(142, 16);
            this.btnDGA.Name = "btnDGA";
            this.btnDGA.Size = new System.Drawing.Size(139, 17);
            this.btnDGA.TabIndex = 1;
            this.btnDGA.TabStop = true;
            this.btnDGA.Text = "DGIndex / DGAVCIndex";
            this.btnDGA.UseVisualStyleBackColor = true;
            this.btnDGA.CheckedChanged += new System.EventHandler(this.btnDGA_CheckedChanged);
            // 
            // btnDGI
            // 
            this.btnDGI.AutoSize = true;
            this.btnDGI.Location = new System.Drawing.Point(326, 16);
            this.btnDGI.Name = "btnDGI";
            this.btnDGI.Size = new System.Drawing.Size(80, 17);
            this.btnDGI.TabIndex = 0;
            this.btnDGI.TabStop = true;
            this.btnDGI.Text = "DGIndexNV";
            this.btnDGI.UseVisualStyleBackColor = true;
            this.btnDGI.CheckedChanged += new System.EventHandler(this.btnDGI_CheckedChanged);
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "D2v creator window";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(13, 372);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 14;
            // 
            // VobinputWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(444, 403);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.closeOnQueue);
            this.Controls.Add(this.gbOutput);
            this.Controls.Add(this.loadOnComplete);
            this.Controls.Add(this.queueButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbInput);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "VobinputWindow";
            this.Text = "MeGUI - D2V Project Creator";
            this.gbInput.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.gbOutput.ResumeLayout(false);
            this.gbOutput.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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

        private void input_FileSelected(FileBar sender, FileBarEventArgs args)
        {
			//AAA: processing doesn't seem to get set to true anywhere so hopefully safe to remove check
			openVideo(input.Filename);
			checkIndexIO();
		}
		private void openVideo(string fileName)
		{
			if (input.Filename != fileName)
                input.Filename = fileName;

            string projectPath;
            string fileNameNoPath = Path.GetFileName(fileName);
            string strVideoFormat = VideoUtil.detectVideoStreamType(fileName);

            AudioTracks.Items.Clear();
            demuxNoAudiotracks.Checked = true; // here to trigger rbtracks_CheckedChanged on new File selection

            if (string.IsNullOrEmpty(projectPath = mainForm.Settings.DefaultOutputDir))
                projectPath = Path.GetDirectoryName(fileName);

            if (strVideoFormat.Equals("vc1") && DGIndexerUsed != DGIndexType.DGI)
            {
                MessageBox.Show("VC-1 is only supported by DGIndexNV", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DGIndexerUsed == DGIndexType.DGI)
            {
                projectName.Text = Path.Combine(projectPath, Path.ChangeExtension(fileNameNoPath, ".dgi"));
            }
            else if (strVideoFormat.Equals("avc"))
            {
                changeDGIndexer(DGIndexType.DGA);
                projectName.Text = Path.Combine(projectPath, Path.ChangeExtension(fileNameNoPath, ".dga"));
            }
            else
            {
                changeDGIndexer(DGIndexType.D2V);
                projectName.Text = Path.Combine(projectPath, Path.ChangeExtension(fileNameNoPath, ".d2v"));
            }

            if (AudioTracks.Items.Count < 1)
            {
                int unused;
                Dar? unused2;
                List<AudioTrackInfo> audioTracks; 
                vUtil.getSourceMediaInfo(fileName, out audioTracks, out unused, out unused2);
                foreach (AudioTrackInfo atrack in audioTracks)
                    AudioTracks.Items.Add(atrack);
            }

            if (AudioTracks.Items.Count > 0)
            {
                if (DGIndexerUsed == DGIndexType.D2V)
                {
                    demuxTracks.Checked = true;
                    AudioTracks.Enabled = true;
                    demuxTracks.Enabled = true;
                }
                else
                {
                    AudioTracks.Enabled = false;
                    demuxTracks.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("MeGUI cannot find audio track information. Audio Tracks selection will be disabled.", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }       
		}
		/// <summary>
		/// creates a dgindex project
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void queueButton_Click(object sender, System.EventArgs e)
		{
            if (Drives.ableToWriteOnThisDrive(Path.GetPathRoot(projectName.Text)))
            {
                if (configured)
                {
                    if (!dialogMode)
                    {
                        switch (DGIndexerUsed)
                        {
                            case DGIndexType.D2V:
                            {
                                IndexJob job = generateIndexJob();
                                lastJob = job;
                                mainForm.Jobs.addJobsToQueue(job);
                                if (this.closeOnQueue.Checked)
                                    this.Close();
                                break;
                            }
                            case DGIndexType.DGI:
                            {
                                DGNVIndexJob job = generateDGNVIndexJob();
                                //lastJob = job;
                                mainForm.Jobs.addJobsToQueue(job);
                                if (this.closeOnQueue.Checked)
                                    this.Close();
                                break;
                            }
                            case DGIndexType.DGA:
                            {
                                DGAIndexJob job = generateDGAIndexJob();
                                //lastJob = job;
                                mainForm.Jobs.addJobsToQueue(job);
                                if (this.closeOnQueue.Checked)
                                    this.Close();
                                break;
                            }
                        }
                    }
                }
                else
                    MessageBox.Show("You must select a Video Input and DGIndex project file to continue",
                        "Configuration incomplete", MessageBoxButtons.OK);
            }
            else
                MessageBox.Show("MeGUI cannot write on the disc " + Path.GetPathRoot(projectName.Text) +"\n" +
                                "Please, select another output path to save your project...", "Configuration Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
		#endregion
		#region helper methods
		private void checkIndexIO()
		{
			configured = (!input.Filename.Equals("") && !projectName.Text.Equals(""));
			if (configured && dialogMode)
				queueButton.DialogResult = DialogResult.OK;
			else
                queueButton.DialogResult = DialogResult.None;
		}
		private IndexJob generateIndexJob()
		{
			int demuxType = 0;
			if (demuxTracks.Checked)
				demuxType = 1;
            else if (demuxNoAudiotracks.Checked)
                demuxType = 0;
            else
                demuxType = 2;

            List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
            foreach (AudioTrackInfo ati in AudioTracks.CheckedItems)
            {
                audioTracks.Add(ati);
            }

            return new IndexJob(this.input.Filename, this.projectName.Text, demuxType, audioTracks, null, loadOnComplete.Checked, demuxVideo.Checked);
		}
        private DGNVIndexJob generateDGNVIndexJob()
        {
            int demuxType = 0;
            if (demuxTracks.Checked)
                demuxType = 1;
            else if (demuxNoAudiotracks.Checked)
                demuxType = 0;
            else
                demuxType = 2;

            List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
            foreach (AudioTrackInfo ati in AudioTracks.CheckedItems)
            {
                audioTracks.Add(ati);
            }

            return new DGNVIndexJob(this.input.Filename, this.projectName.Text, demuxType, audioTracks, null, loadOnComplete.Checked, demuxVideo.Checked);
        }
        private DGAIndexJob generateDGAIndexJob()
        {
            int demuxType = 0;
            if (demuxTracks.Checked)
                demuxType = 1;
            else if (demuxNoAudiotracks.Checked)
                demuxType = 0;
            else
                demuxType = 2;

            List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
            foreach (AudioTrackInfo ati in AudioTracks.CheckedItems)
            {
                audioTracks.Add(ati);
            }

            return new DGAIndexJob(this.input.Filename, this.projectName.Text, demuxType, audioTracks, null, loadOnComplete.Checked, demuxVideo.Checked);
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

        private void rbtracks_CheckedChanged(object sender, EventArgs e)
        {
            // Now defaults to starting with every track selected
            for (int i = 0; i < AudioTracks.Items.Count; i++)
                AudioTracks.SetItemChecked(i, !demuxNoAudiotracks.Checked);
            AudioTracks.Enabled = demuxTracks.Checked;
        }

        private void btnDGA_CheckedChanged(object sender, EventArgs e)
        {
            changeDGIndexer(DGIndexType.D2VorDGA);
        }

        private void btnDGI_CheckedChanged(object sender, EventArgs e)
        {
            changeDGIndexer(DGIndexType.DGI);
        }
    }

    public class D2VCreatorTool : MeGUI.core.plugins.interfaces.ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "DG Creator"; }
        }

        public void Run(MainForm info)
        {
            new VobinputWindow(info).Show();

        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.Ctrl2 }; }
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
        private static LogItem postprocess(MainForm mainForm, Job ajob)
        {
            if (!(ajob is IndexJob)) return null;
            IndexJob job = (IndexJob)ajob;
            if (job.PostprocessingProperties != null) return null;

            StringBuilder logBuilder = new StringBuilder();
            VideoUtil vUtil = new VideoUtil(mainForm);
            Dictionary<int, string> audioFiles = vUtil.getAllDemuxedAudio(job.AudioTracks, job.Output, 8);
            if (job.LoadSources)
            {
                if (job.DemuxMode != 0 && audioFiles.Count > 0)
                {
                    string[] files = new string[audioFiles.Values.Count];
                    audioFiles.Values.CopyTo(files, 0);
                    Util.ThreadSafeRun(mainForm, new MethodInvoker(
                        delegate
                        {
                            mainForm.Audio.openAudioFile(files);
                        }));
                }
                // if the above needed delegation for openAudioFile this needs it for openVideoFile?
                // It seems to fix the problem of ASW dissapearing as soon as it appears on a system (Vista X64)
                Util.ThreadSafeRun(mainForm, new MethodInvoker(
                    delegate
                    {
                        AviSynthWindow asw = new AviSynthWindow(mainForm, job.Output);
                        asw.OpenScript += new OpenScriptCallback(mainForm.Video.openVideoFile);
                        asw.Show();
                    }));
            }

            return null;
        }
    }

    public class dgnvIndexJobPostProcessor
    {
        public static JobPostProcessor PostProcessor = new JobPostProcessor(postprocess, "Dgi_postprocessor");
        private static LogItem postprocess(MainForm mainForm, Job ajob)
        {
            if (!(ajob is DGNVIndexJob)) return null;
            DGNVIndexJob job = (DGNVIndexJob)ajob;
            if (job.PostprocessingProperties != null) return null;

            StringBuilder logBuilder = new StringBuilder();
            VideoUtil vUtil = new VideoUtil(mainForm);
            Dictionary<int, string> audioFiles = vUtil.getAllDemuxedAudio(job.AudioTracks, job.Output, 8);
            if (job.LoadSources)
            {
                if (job.DemuxMode != 0 && audioFiles.Count > 0)
                {
                    string[] files = new string[audioFiles.Values.Count];
                    audioFiles.Values.CopyTo(files, 0);
                    Util.ThreadSafeRun(mainForm, new MethodInvoker(
                        delegate
                        {
                            mainForm.Audio.openAudioFile(files);
                        }));
                }
                // if the above needed delegation for openAudioFile this needs it for openVideoFile?
                // It seems to fix the problem of ASW dissapearing as soon as it appears on a system (Vista X64)
                Util.ThreadSafeRun(mainForm, new MethodInvoker(
                    delegate
                    {
                        AviSynthWindow asw = new AviSynthWindow(mainForm, job.Output);
                        asw.OpenScript += new OpenScriptCallback(mainForm.Video.openVideoFile);
                        asw.Show();
                    }));
            }

            return null;
        }
    }

    public class dgavcIndexJobPostProcessor
    {
        public static JobPostProcessor PostProcessor = new JobPostProcessor(postprocess, "Dga_postprocessor");
        private static LogItem postprocess(MainForm mainForm, Job ajob)
        {
            if (!(ajob is DGAIndexJob)) return null;
            DGAIndexJob job = (DGAIndexJob)ajob;
            if (job.PostprocessingProperties != null) return null;

            StringBuilder logBuilder = new StringBuilder();
            VideoUtil vUtil = new VideoUtil(mainForm);
            Dictionary<int, string> audioFiles = vUtil.getAllDemuxedAudio(job.AudioTracks, job.Output, 8);
            if (job.LoadSources)
            {
                if (job.DemuxMode != 0 && audioFiles.Count > 0)
                {
                    string[] files = new string[audioFiles.Values.Count];
                    audioFiles.Values.CopyTo(files, 0);
                    Util.ThreadSafeRun(mainForm, new MethodInvoker(
                        delegate
                        {
                            mainForm.Audio.openAudioFile(files);
                        }));
                }
                // if the above needed delegation for openAudioFile this needs it for openVideoFile?
                // It seems to fix the problem of ASW dissapearing as soon as it appears on a system (Vista X64)
                Util.ThreadSafeRun(mainForm, new MethodInvoker(
                    delegate
                    {
                        AviSynthWindow asw = new AviSynthWindow(mainForm, job.Output);
                        asw.OpenScript += new OpenScriptCallback(mainForm.Video.openVideoFile);
                        asw.Show();
                    }));
            }

            return null;
        }
    }

    public delegate void ProjectCreationComplete(); // this event is fired when the dgindex thread finishes
}
