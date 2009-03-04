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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.util;
using MeGUI.packages.tools.calculator;

namespace MeGUI
{
 
	/// <summary>
	/// Summary description for Calculator.
	/// </summary>
	public class Calculator : System.Windows.Forms.Form
	{
		#region variables
        private bool updatingContainers = false;
        private List<AudioTrackSizeTab> audioTabs = new List<AudioTrackSizeTab>();
        private MainForm mainForm;
        private MuxProvider muxProvider;
        private CodecManager codecs = new CodecManager();
		private bool isUpdating = false;
		private System.Windows.Forms.GroupBox videoGroupbox;
		private System.Windows.Forms.NumericUpDown hours;
		private System.Windows.Forms.NumericUpDown minutes;
		private System.Windows.Forms.NumericUpDown seconds;
		private System.Windows.Forms.Label hoursLabel;
		private System.Windows.Forms.Label minutesLabel;
		private System.Windows.Forms.Label secondsLabel;
        private System.Windows.Forms.Label framerateLabel;
        private System.Windows.Forms.Label totalSecondsLabel;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.GroupBox codecGroupbox;
        private System.Windows.Forms.GroupBox sizeGroupbox;
		private System.Windows.Forms.RadioButton averageBitrateRadio;
        private System.Windows.Forms.RadioButton fileSizeRadio;
        private System.Windows.Forms.Label AverageBitrateLabel;
        private System.Windows.Forms.GroupBox resultGroupbox;
        private System.Windows.Forms.Label nbFramesLabel;
		private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.GroupBox containerGroupbox;
        private System.Windows.Forms.CheckBox bframes;
        private ComboBox videoCodec;
        private ComboBox containerFormat;
        private NumericUpDown nbFrames;
        private NumericUpDown totalSeconds;
        private NumericUpDown projectedBitrate;
        private MeGUI.core.gui.HelpButton helpButton1;
        private MeGUI.core.gui.TargetSizeSCBox targetSize;
        private TextBox videoSize;
        private Label VideoFileSizeLabel;
        private MeGUI.core.gui.FPSChooser fpsChooser;
        private TabControl audio;
        private TabPage audioPage1;
        private AudioTrackSizeTab audioTrackSizeTab1;
		#endregion
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem addTrackToolStripMenuItem;
        private ToolStripMenuItem removeTrackToolStripMenuItem;
        private IContainer components;
		#region start / stop
		public Calculator(MainForm mainForm)
		{
            InitializeComponent();
            this.mainForm = mainForm;
            this.muxProvider = mainForm.MuxProvider;
            this.videoCodec.Items.AddRange(CodecManager.VideoEncoderTypes.ValuesArray);
            videoCodec.SelectedItem = CodecManager.VideoEncoderTypes["X264"];
            this.containerFormat.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
            containerFormat.SelectedItem = ContainerType.MKV;
            audioTabs.Add(audioTrackSizeTab1);
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
            this.components = new System.ComponentModel.Container();
            this.videoGroupbox = new System.Windows.Forms.GroupBox();
            this.fpsChooser = new MeGUI.core.gui.FPSChooser();
            this.nbFrames = new System.Windows.Forms.NumericUpDown();
            this.totalSeconds = new System.Windows.Forms.NumericUpDown();
            this.bframes = new System.Windows.Forms.CheckBox();
            this.nbFramesLabel = new System.Windows.Forms.Label();
            this.totalSecondsLabel = new System.Windows.Forms.Label();
            this.framerateLabel = new System.Windows.Forms.Label();
            this.secondsLabel = new System.Windows.Forms.Label();
            this.minutesLabel = new System.Windows.Forms.Label();
            this.hoursLabel = new System.Windows.Forms.Label();
            this.seconds = new System.Windows.Forms.NumericUpDown();
            this.minutes = new System.Windows.Forms.NumericUpDown();
            this.hours = new System.Windows.Forms.NumericUpDown();
            this.applyButton = new System.Windows.Forms.Button();
            this.codecGroupbox = new System.Windows.Forms.GroupBox();
            this.videoCodec = new System.Windows.Forms.ComboBox();
            this.containerGroupbox = new System.Windows.Forms.GroupBox();
            this.containerFormat = new System.Windows.Forms.ComboBox();
            this.sizeGroupbox = new System.Windows.Forms.GroupBox();
            this.targetSize = new MeGUI.core.gui.TargetSizeSCBox();
            this.fileSizeRadio = new System.Windows.Forms.RadioButton();
            this.averageBitrateRadio = new System.Windows.Forms.RadioButton();
            this.AverageBitrateLabel = new System.Windows.Forms.Label();
            this.resultGroupbox = new System.Windows.Forms.GroupBox();
            this.videoSize = new System.Windows.Forms.TextBox();
            this.projectedBitrate = new System.Windows.Forms.NumericUpDown();
            this.VideoFileSizeLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.audio = new System.Windows.Forms.TabControl();
            this.audioPage1 = new System.Windows.Forms.TabPage();
            this.audioTrackSizeTab1 = new MeGUI.packages.tools.calculator.AudioTrackSizeTab();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.videoGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hours)).BeginInit();
            this.codecGroupbox.SuspendLayout();
            this.containerGroupbox.SuspendLayout();
            this.sizeGroupbox.SuspendLayout();
            this.resultGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectedBitrate)).BeginInit();
            this.audio.SuspendLayout();
            this.audioPage1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // videoGroupbox
            // 
            this.videoGroupbox.Controls.Add(this.fpsChooser);
            this.videoGroupbox.Controls.Add(this.nbFrames);
            this.videoGroupbox.Controls.Add(this.totalSeconds);
            this.videoGroupbox.Controls.Add(this.bframes);
            this.videoGroupbox.Controls.Add(this.nbFramesLabel);
            this.videoGroupbox.Controls.Add(this.totalSecondsLabel);
            this.videoGroupbox.Controls.Add(this.framerateLabel);
            this.videoGroupbox.Controls.Add(this.secondsLabel);
            this.videoGroupbox.Controls.Add(this.minutesLabel);
            this.videoGroupbox.Controls.Add(this.hoursLabel);
            this.videoGroupbox.Controls.Add(this.seconds);
            this.videoGroupbox.Controls.Add(this.minutes);
            this.videoGroupbox.Controls.Add(this.hours);
            this.videoGroupbox.Location = new System.Drawing.Point(8, 2);
            this.videoGroupbox.Name = "videoGroupbox";
            this.videoGroupbox.Size = new System.Drawing.Size(320, 160);
            this.videoGroupbox.TabIndex = 0;
            this.videoGroupbox.TabStop = false;
            this.videoGroupbox.Text = "Video";
            // 
            // fpsChooser
            // 
            this.fpsChooser.Location = new System.Drawing.Point(185, 73);
            this.fpsChooser.MaximumSize = new System.Drawing.Size(1000, 29);
            this.fpsChooser.MinimumSize = new System.Drawing.Size(64, 29);
            this.fpsChooser.Name = "fpsChooser";
            this.fpsChooser.NullString = null;
            this.fpsChooser.SelectedIndex = 0;
            this.fpsChooser.Size = new System.Drawing.Size(129, 29);
            this.fpsChooser.TabIndex = 13;
            this.fpsChooser.SelectionChanged += new MeGUI.StringChanged(this.fpsChooser_SelectionChanged);
            // 
            // nbFrames
            // 
            this.nbFrames.Location = new System.Drawing.Point(189, 105);
            this.nbFrames.Maximum = new decimal(new int[] {
            10000000,
            10000000,
            10000000,
            0});
            this.nbFrames.Name = "nbFrames";
            this.nbFrames.Size = new System.Drawing.Size(120, 21);
            this.nbFrames.TabIndex = 11;
            this.nbFrames.ValueChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // totalSeconds
            // 
            this.totalSeconds.Location = new System.Drawing.Point(189, 51);
            this.totalSeconds.Maximum = new decimal(new int[] {
            90000,
            0,
            0,
            0});
            this.totalSeconds.Name = "totalSeconds";
            this.totalSeconds.Size = new System.Drawing.Size(120, 21);
            this.totalSeconds.TabIndex = 7;
            this.totalSeconds.ValueChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // bframes
            // 
            this.bframes.Checked = true;
            this.bframes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bframes.Location = new System.Drawing.Point(8, 136);
            this.bframes.Name = "bframes";
            this.bframes.Size = new System.Drawing.Size(128, 16);
            this.bframes.TabIndex = 12;
            this.bframes.Text = "B-frames";
            this.bframes.CheckedChanged += new System.EventHandler(this.bframes_CheckedChanged);
            // 
            // nbFramesLabel
            // 
            this.nbFramesLabel.Location = new System.Drawing.Point(6, 112);
            this.nbFramesLabel.Name = "nbFramesLabel";
            this.nbFramesLabel.Size = new System.Drawing.Size(128, 16);
            this.nbFramesLabel.TabIndex = 10;
            this.nbFramesLabel.Text = "Number of Frames";
            // 
            // totalSecondsLabel
            // 
            this.totalSecondsLabel.Location = new System.Drawing.Point(6, 58);
            this.totalSecondsLabel.Name = "totalSecondsLabel";
            this.totalSecondsLabel.Size = new System.Drawing.Size(128, 16);
            this.totalSecondsLabel.TabIndex = 6;
            this.totalSecondsLabel.Text = "Total Length in Seconds";
            // 
            // framerateLabel
            // 
            this.framerateLabel.Location = new System.Drawing.Point(6, 86);
            this.framerateLabel.Name = "framerateLabel";
            this.framerateLabel.Size = new System.Drawing.Size(128, 16);
            this.framerateLabel.TabIndex = 8;
            this.framerateLabel.Text = "Framerate";
            // 
            // secondsLabel
            // 
            this.secondsLabel.Location = new System.Drawing.Point(207, 26);
            this.secondsLabel.Name = "secondsLabel";
            this.secondsLabel.Size = new System.Drawing.Size(48, 16);
            this.secondsLabel.TabIndex = 4;
            this.secondsLabel.Text = "Seconds";
            // 
            // minutesLabel
            // 
            this.minutesLabel.Location = new System.Drawing.Point(99, 26);
            this.minutesLabel.Name = "minutesLabel";
            this.minutesLabel.Size = new System.Drawing.Size(48, 16);
            this.minutesLabel.TabIndex = 2;
            this.minutesLabel.Text = "Minutes";
            // 
            // hoursLabel
            // 
            this.hoursLabel.Location = new System.Drawing.Point(7, 26);
            this.hoursLabel.Name = "hoursLabel";
            this.hoursLabel.Size = new System.Drawing.Size(40, 16);
            this.hoursLabel.TabIndex = 0;
            this.hoursLabel.Text = "Hours";
            // 
            // seconds
            // 
            this.seconds.Location = new System.Drawing.Point(261, 24);
            this.seconds.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.seconds.Name = "seconds";
            this.seconds.Size = new System.Drawing.Size(48, 21);
            this.seconds.TabIndex = 5;
            this.seconds.ValueChanged += new System.EventHandler(this.time_ValueChanged);
            // 
            // minutes
            // 
            this.minutes.Location = new System.Drawing.Point(153, 24);
            this.minutes.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.minutes.Name = "minutes";
            this.minutes.Size = new System.Drawing.Size(48, 21);
            this.minutes.TabIndex = 3;
            this.minutes.ValueChanged += new System.EventHandler(this.time_ValueChanged);
            // 
            // hours
            // 
            this.hours.Location = new System.Drawing.Point(53, 24);
            this.hours.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.hours.Name = "hours";
            this.hours.Size = new System.Drawing.Size(40, 21);
            this.hours.TabIndex = 1;
            this.hours.ValueChanged += new System.EventHandler(this.time_ValueChanged);
            // 
            // applyButton
            // 
            this.applyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.applyButton.Location = new System.Drawing.Point(517, 342);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(48, 23);
            this.applyButton.TabIndex = 0;
            this.applyButton.Text = "Apply";
            // 
            // codecGroupbox
            // 
            this.codecGroupbox.Controls.Add(this.videoCodec);
            this.codecGroupbox.Location = new System.Drawing.Point(336, 4);
            this.codecGroupbox.Name = "codecGroupbox";
            this.codecGroupbox.Size = new System.Drawing.Size(232, 55);
            this.codecGroupbox.TabIndex = 1;
            this.codecGroupbox.TabStop = false;
            this.codecGroupbox.Text = "Codec";
            // 
            // videoCodec
            // 
            this.videoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoCodec.FormattingEnabled = true;
            this.videoCodec.Location = new System.Drawing.Point(6, 24);
            this.videoCodec.Name = "videoCodec";
            this.videoCodec.Size = new System.Drawing.Size(121, 21);
            this.videoCodec.TabIndex = 0;
            this.videoCodec.SelectedIndexChanged += new System.EventHandler(this.audio_SelectedIndexChanged);
            // 
            // containerGroupbox
            // 
            this.containerGroupbox.Controls.Add(this.containerFormat);
            this.containerGroupbox.Location = new System.Drawing.Point(336, 65);
            this.containerGroupbox.Name = "containerGroupbox";
            this.containerGroupbox.Size = new System.Drawing.Size(232, 50);
            this.containerGroupbox.TabIndex = 2;
            this.containerGroupbox.TabStop = false;
            this.containerGroupbox.Text = "Container";
            // 
            // containerFormat
            // 
            this.containerFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.containerFormat.FormattingEnabled = true;
            this.containerFormat.Location = new System.Drawing.Point(6, 20);
            this.containerFormat.Name = "containerFormat";
            this.containerFormat.Size = new System.Drawing.Size(121, 21);
            this.containerFormat.TabIndex = 0;
            this.containerFormat.SelectedIndexChanged += new System.EventHandler(this.audio_SelectedIndexChanged);
            // 
            // sizeGroupbox
            // 
            this.sizeGroupbox.Controls.Add(this.targetSize);
            this.sizeGroupbox.Controls.Add(this.fileSizeRadio);
            this.sizeGroupbox.Location = new System.Drawing.Point(336, 119);
            this.sizeGroupbox.Name = "sizeGroupbox";
            this.sizeGroupbox.Size = new System.Drawing.Size(232, 84);
            this.sizeGroupbox.TabIndex = 3;
            this.sizeGroupbox.TabStop = false;
            this.sizeGroupbox.Text = "Total Size";
            // 
            // targetSize
            // 
            this.targetSize.Location = new System.Drawing.Point(18, 46);
            this.targetSize.MaximumSize = new System.Drawing.Size(1000, 29);
            this.targetSize.MinimumSize = new System.Drawing.Size(64, 29);
            this.targetSize.Name = "targetSize";
            this.targetSize.NullString = "Not calculated";
            this.targetSize.SelectedIndex = 0;
            this.targetSize.Size = new System.Drawing.Size(208, 29);
            this.targetSize.TabIndex = 1;
            this.targetSize.SelectionChanged += new MeGUI.StringChanged(this.targetSize_SelectionChanged);
            // 
            // fileSizeRadio
            // 
            this.fileSizeRadio.Checked = true;
            this.fileSizeRadio.Location = new System.Drawing.Point(8, 20);
            this.fileSizeRadio.Name = "fileSizeRadio";
            this.fileSizeRadio.Size = new System.Drawing.Size(100, 20);
            this.fileSizeRadio.TabIndex = 0;
            this.fileSizeRadio.TabStop = true;
            this.fileSizeRadio.Text = "File Size";
            this.fileSizeRadio.CheckedChanged += new System.EventHandler(this.calculationMode_CheckedChanged);
            // 
            // averageBitrateRadio
            // 
            this.averageBitrateRadio.AutoSize = true;
            this.averageBitrateRadio.Location = new System.Drawing.Point(8, 25);
            this.averageBitrateRadio.Name = "averageBitrateRadio";
            this.averageBitrateRadio.Size = new System.Drawing.Size(101, 17);
            this.averageBitrateRadio.TabIndex = 0;
            this.averageBitrateRadio.Text = "Average Bitrate";
            this.averageBitrateRadio.CheckedChanged += new System.EventHandler(this.calculationMode_CheckedChanged);
            // 
            // AverageBitrateLabel
            // 
            this.AverageBitrateLabel.AutoSize = true;
            this.AverageBitrateLabel.Location = new System.Drawing.Point(190, 26);
            this.AverageBitrateLabel.Name = "AverageBitrateLabel";
            this.AverageBitrateLabel.Size = new System.Drawing.Size(33, 13);
            this.AverageBitrateLabel.TabIndex = 2;
            this.AverageBitrateLabel.Text = "kbit/s";
            // 
            // resultGroupbox
            // 
            this.resultGroupbox.Controls.Add(this.videoSize);
            this.resultGroupbox.Controls.Add(this.projectedBitrate);
            this.resultGroupbox.Controls.Add(this.AverageBitrateLabel);
            this.resultGroupbox.Controls.Add(this.VideoFileSizeLabel);
            this.resultGroupbox.Controls.Add(this.averageBitrateRadio);
            this.resultGroupbox.Location = new System.Drawing.Point(336, 211);
            this.resultGroupbox.Name = "resultGroupbox";
            this.resultGroupbox.Size = new System.Drawing.Size(232, 119);
            this.resultGroupbox.TabIndex = 7;
            this.resultGroupbox.TabStop = false;
            this.resultGroupbox.Text = "Results";
            // 
            // videoSize
            // 
            this.videoSize.Location = new System.Drawing.Point(18, 74);
            this.videoSize.Name = "videoSize";
            this.videoSize.ReadOnly = true;
            this.videoSize.Size = new System.Drawing.Size(188, 21);
            this.videoSize.TabIndex = 4;
            // 
            // projectedBitrate
            // 
            this.projectedBitrate.Location = new System.Drawing.Point(112, 23);
            this.projectedBitrate.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.projectedBitrate.Name = "projectedBitrate";
            this.projectedBitrate.ReadOnly = true;
            this.projectedBitrate.Size = new System.Drawing.Size(72, 21);
            this.projectedBitrate.TabIndex = 1;
            this.projectedBitrate.ValueChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // VideoFileSizeLabel
            // 
            this.VideoFileSizeLabel.Location = new System.Drawing.Point(17, 51);
            this.VideoFileSizeLabel.Name = "VideoFileSizeLabel";
            this.VideoFileSizeLabel.Size = new System.Drawing.Size(82, 13);
            this.VideoFileSizeLabel.TabIndex = 3;
            this.VideoFileSizeLabel.Text = "Video File Size:";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(463, 342);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            // 
            // audio
            // 
            this.audio.Controls.Add(this.audioPage1);
            this.audio.Location = new System.Drawing.Point(8, 168);
            this.audio.Name = "audio";
            this.audio.SelectedIndex = 0;
            this.audio.Size = new System.Drawing.Size(316, 148);
            this.audio.TabIndex = 10;
            // 
            // audioPage1
            // 
            this.audioPage1.Controls.Add(this.audioTrackSizeTab1);
            this.audioPage1.Location = new System.Drawing.Point(4, 22);
            this.audioPage1.Name = "audioPage1";
            this.audioPage1.Padding = new System.Windows.Forms.Padding(3);
            this.audioPage1.Size = new System.Drawing.Size(308, 122);
            this.audioPage1.TabIndex = 0;
            this.audioPage1.Text = "Audio 1";
            this.audioPage1.UseVisualStyleBackColor = true;
            // 
            // audioTrackSizeTab1
            // 
            this.audioTrackSizeTab1.AllowDrop = true;
            this.audioTrackSizeTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioTrackSizeTab1.Location = new System.Drawing.Point(3, 3);
            this.audioTrackSizeTab1.Name = "audioTrackSizeTab1";
            this.audioTrackSizeTab1.PlayLength = ((long)(0));
            this.audioTrackSizeTab1.Size = new System.Drawing.Size(302, 116);
            this.audioTrackSizeTab1.TabIndex = 0;
            this.audioTrackSizeTab1.SomethingChanged += new System.EventHandler(this.audioTrackSizeTab1_SomethingChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTrackToolStripMenuItem,
            this.removeTrackToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(147, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // addTrackToolStripMenuItem
            // 
            this.addTrackToolStripMenuItem.Name = "addTrackToolStripMenuItem";
            this.addTrackToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.addTrackToolStripMenuItem.Text = "Add track";
            this.addTrackToolStripMenuItem.Click += new System.EventHandler(this.addTrackToolStripMenuItem_Click);
            // 
            // removeTrackToolStripMenuItem
            // 
            this.removeTrackToolStripMenuItem.Name = "removeTrackToolStripMenuItem";
            this.removeTrackToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.removeTrackToolStripMenuItem.Text = "Remove track";
            this.removeTrackToolStripMenuItem.Click += new System.EventHandler(this.removeTrackToolStripMenuItem_Click);
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Bitrate calculator";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(8, 342);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 8;
            // 
            // Calculator
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(576, 377);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.audio);
            this.Controls.Add(this.resultGroupbox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.sizeGroupbox);
            this.Controls.Add(this.containerGroupbox);
            this.Controls.Add(this.codecGroupbox);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.videoGroupbox);
            this.Controls.Add(this.applyButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Calculator";
            this.Text = "MeGUI - Bitrate Calculator";
            this.videoGroupbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nbFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hours)).EndInit();
            this.codecGroupbox.ResumeLayout(false);
            this.containerGroupbox.ResumeLayout(false);
            this.sizeGroupbox.ResumeLayout(false);
            this.resultGroupbox.ResumeLayout(false);
            this.resultGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectedBitrate)).EndInit();
            this.audio.ResumeLayout(false);
            this.audioPage1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		#region data setters and getters
        private void setFPSToBest(double fps)
        {
            fpsChooser.Value = (decimal)fps;

        }
		/// <summary>
		/// sets video, audio and codec defaults
		/// </summary>
		/// <param name="nbFrames">number of frames of the video source</param>
		/// <param name="framerate">framerate of the video source</param>
		/// <param name="codec">codec selected</param>
		/// <param name="container">container</param>
		/// <param name="audio1Bitrate">bitrate of the first audio track</param>
		/// <param name="audio2Bitrate">bitrate of the second audio track</param>
        public void setDefaults(ulong nbFrames, double framerate, VideoCodecSettings vSettings, List<AudioJob> audioStreams)
		{
            setFPSToBest(framerate);
            try
            {
                bframes.Checked = vSettings.NbBframes > 0;
            }
            catch (Exception) { }

			if (nbFrames > 0)
				this.nbFrames.Value = nbFrames;

            if (videoCodec.Items.Contains(vSettings.EncoderType))
                videoCodec.SelectedItem = vSettings.EncoderType;

            int i = 0;
            foreach (AudioJob s in audioStreams)
            {
                if (audioTabs.Count == i)
                    AddTab();

                audioTabs[i].Job = s;
                ++i;
            }
		}

        private void AddTab()
        {
            TabPage p = new TabPage("Audio " + (audioTabs.Count + 1));
            p.UseVisualStyleBackColor = audio.TabPages[0].UseVisualStyleBackColor;
            p.Padding = audio.TabPages[0].Padding;

            AudioTrackSizeTab a = new AudioTrackSizeTab();
            a.Dock = audioTabs[0].Dock;
            a.Padding = audioTabs[0].Padding;
            a.PlayLength = audioTabs[0].PlayLength;
            a.SomethingChanged += audioTrackSizeTab1_SomethingChanged;

            audio.TabPages.Add(p);
            p.Controls.Add(a);
            audioTabs.Add(a);
        }

        private void RemoveTab()
        {
            audio.TabPages.RemoveAt(audio.TabPages.Count - 1);
            audioTabs.RemoveAt(audioTabs.Count - 1);
        }
		/// <summary>
		/// gets the selected codec
		/// </summary>
		/// <returns></returns>
/*		public ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> getSelectedCodec()
		{
            return (ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>)videoCodec.SelectedItem;
		}*/

        public VideoEncoderType SelectedVCodec
        {
            get { return (VideoEncoderType)videoCodec.SelectedItem; }
        }
		/// <summary>
		/// gets the calculated bitrate
		/// </summary>
		/// <returns></returns>
		public int getBitrate()
		{
            return (int)projectedBitrate.Value;
		}
		#endregion
		#region audio
        private void audio_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            updateContainers();
            updateBitrateSize();
        }
		#endregion
		#region generic eventhandlers
		private void textField_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (! char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
				e.Handled = true;
		}
		#endregion
		#region textfields
		private void textField_TextChanged(object sender, System.EventArgs e)
		{
            lock(this)
            {
                if(sender is NumericUpDown && !this.isUpdating)
                {
                    this.isUpdating = true;
                    NumericUpDown tb = (NumericUpDown)sender;
                    decimal value = tb.Value;
                    if (tb == totalSeconds)
                    {
                        int hours = (int)value / 3600;
                        value -= hours * 3600;
                        int minutes = (int)value / 60;
                        value -= minutes * 60;
                        if (hours <= this.hours.Maximum)
                        {
                            this.hours.Value = hours;
                            this.minutes.Value = minutes;
                            this.seconds.Value = value;
                        }
                        else // We set to max available time and set frames accordingly
                        {
                            this.hours.Value = this.hours.Maximum;
                            this.minutes.Value = this.minutes.Maximum - 1; //59 mins
                            this.seconds.Value = this.seconds.Maximum - 1; //59 seconds
                            UpdateTotalSeconds();
                        }
                        setAudioLength();
                        UpdateTotalFrames();

                    }
                    else if (tb == nbFrames)
                    {
                        int secs = (int)(value / fpsChooser.Value);
                        totalSeconds.Text = secs.ToString();
                        int hours = secs / 3600;
                        secs -= hours * 3600;
                        int minutes = secs / 60;
                        secs -= minutes * 60;
                        if (hours < this.hours.Maximum)
                        {
                            this.hours.Value = hours;
                            this.minutes.Value = minutes;
                            this.seconds.Value = secs;
                        }
                        else //Set to max available time and set frames accordingly
                        {
                            this.hours.Value = this.hours.Maximum;
                            this.minutes.Value = this.minutes.Maximum - 1; //59 minutes
                            this.seconds.Value = this.seconds.Maximum - 1; //59 seconds
                            UpdateTotalFrames();
                        }
                        UpdateTotalSeconds();
                        setAudioLength();
                    }
                    else if (tb == projectedBitrate)
                    {
                        if (averageBitrateRadio.Checked) // only do something here if we're in size calculation mode
                            updateSize();
                    }
                    tb.Select(tb.Text.Length, 0);
                    if((averageBitrateRadio.Checked
                        && tb != projectedBitrate)
                        || !averageBitrateRadio.Checked)
                        updateBitrateSize();
                    
                    this.isUpdating = false;
                }
            }
		}
		#endregion
		#region updown controls
        private void time_ValueChanged(object sender, System.EventArgs e)
        {
            lock (this)
            {
                if (isUpdating)
                    return;

                this.isUpdating = true;
                NumericUpDown ud = (NumericUpDown)sender;

                if (this.hours.Value.Equals(this.hours.Maximum))
                {
                    if (this.minutes.Value == 60)
                    {
                        this.minutes.Value = 59;
                        UpdateTotalSeconds();
                        UpdateTotalFrames();
                        isUpdating = false;
                        return; // we can't increase the time
                    }
                    else if (this.seconds.Value == 60 && this.minutes.Value == 59)
                    {
                        this.seconds.Value = 59;
                        UpdateTotalSeconds();
                        UpdateTotalFrames();
                        isUpdating = false;
                        return; // we can't increase the time
                    }
                }
                if (ud.Value == 60) // time to wrap
                {
                    ud.Value = 0;
                    if (ud == seconds)
                    {
                        if (minutes.Value == 59)
                        {
                            minutes.Value = 0;
                            if (!this.hours.Value.Equals(this.hours.Maximum))
                                hours.Value += 1;
                        }
                        else
                            minutes.Value += 1;
                    }
                    else if (ud == minutes)
                    {
                        minutes.Value = 0;
                        if (this.hours.Value < this.hours.Maximum)
                            hours.Value += 1;
                    }
                }
                UpdateTotalSeconds();
                setAudioLength();
                UpdateTotalFrames();
                updateBitrateSize();

                this.isUpdating = false;
            }
        }

        private void setAudioLength()
        {
            foreach (AudioTrackSizeTab t in audioTabs)
                t.PlayLength = (int)totalSeconds.Value;
        }

        private void UpdateTotalFrames()
        {
            int secs = (int)totalSeconds.Value;
            double fps = (double)fpsChooser.Value;
            int frameNumber = (int)((double)secs * fps);
            nbFrames.Value = frameNumber;
        }
        private void UpdateTotalSeconds()
        {
            int secs = (int)this.hours.Value * 3600 + (int)this.minutes.Value * 60 + (int)this.seconds.Value;
            totalSeconds.Value = secs;
        }
		#endregion
		#region radio buttons
		/// <summary>
		/// handles the change in between bitrate and size calculation mode
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void calculationMode_CheckedChanged(object sender, System.EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			if (rb.Checked)
			{
                bool isBitrate = (rb == averageBitrateRadio);
                targetSize.Enabled = !isBitrate;
                projectedBitrate.ReadOnly = !isBitrate;
                if (isBitrate)
                    fileSizeRadio.Checked = false;
                else
                    averageBitrateRadio.Checked = false;
			}
		}
		/// <summary>
		/// handles codec selection change
		/// if snow is checked, the container must be avi and mp4 has to be disabled
		/// the choice is not limited for any other codec
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void codec_CheckedChanged(object sender, System.EventArgs e)
		{
            updateContainers();
            updateBitrateSize();
		}

        private void updateContainers()
        {
            if (updatingContainers)
                return;
            updatingContainers = true;
            VideoEncoderType vCodec = SelectedVCodec;
            List<MuxableType> muxableTypes = new List<MuxableType>();
            muxableTypes.AddRange(getAudioTypes());
            ContainerType previousContainer = null;
            try 
            {
                previousContainer = containerFormat.SelectedItem as ContainerType;
            }
            catch (Exception) {}

            containerFormat.Items.Clear();
            containerFormat.Items.AddRange(muxProvider.GetSupportedContainers(vCodec, new AudioEncoderType[0], muxableTypes.ToArray()).ToArray());
            if (previousContainer != null && containerFormat.Items.Contains(previousContainer))
                containerFormat.SelectedItem = previousContainer;
            updatingContainers = false;
        }

        private IEnumerable<MuxableType> getAudioTypes()
        {
            List<MuxableType> l = new List<MuxableType>();
            foreach (AudioTrackSizeTab t in audioTabs)
            {
                if (t.Stream == null) continue;

                AudioType type = (AudioType)t.Stream.Type;
                l.Add(new MuxableType(type, type.SupportedCodecs[0]));
            }
            return l;
        }

        #endregion
		#region checkboxes
		private void bframes_CheckedChanged(object sender, System.EventArgs e)
		{
			updateBitrateSize();
		}
		#endregion
		#region bitrate calculations
        private bool getInfo(out VideoCodec codec, out AudioBitrateCalculationStream[] audioStreamsArray, out ContainerType containerType,
            out ulong numberOfFrames, out double framerate)
        {
            numberOfFrames = (ulong)nbFrames.Value;
            framerate = (double)fpsChooser.Value;
            audioStreamsArray = getAudioStreams().ToArray();
            try
            {
                codec = SelectedVCodec.VCodec;
                containerType = (containerFormat.SelectedItem as ContainerType);
            }
            catch (Exception)
            {
                codec = null;
                containerType = null;
                return false;
            }
            if (numberOfFrames <= 0 || framerate <= 0 || containerType == null)
                return false;
            return true;
        }

        private List<AudioBitrateCalculationStream> getAudioStreams()
        {
            List<AudioBitrateCalculationStream> l = new List<AudioBitrateCalculationStream>();
            foreach (AudioTrackSizeTab t in audioTabs)
                if (t.Stream != null)
                    l.Add(t.Stream);
            return l;
        }

		private bool updateSize()
		{
            AudioBitrateCalculationStream[] audioStreams;
            VideoCodec vCodec;
            ContainerType containerType;
            ulong nbOfFrames;
            double framerate;
            if (!getInfo(out vCodec, out audioStreams, out containerType,
                out nbOfFrames, out framerate))
                return false;
            int vidSize = 0;
            int bitrate = (int)projectedBitrate.Value;
            if (bitrate <= 0)
                return false;
            long totalSize = BitrateCalculator.CalculateFileSizeKB(vCodec, bframes.Checked, containerType,
                audioStreams, bitrate, nbOfFrames, framerate, out vidSize);
			long totalSizeMB = totalSize / 1024L;
			int sizeMB = vidSize / 1024;
            try
            {
                targetSize.Value = new FileSize(Unit.KB, totalSize);
                videoSize.Text = new FileSize(Unit.KB, vidSize).ToString();
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
            return true;
		}

        private bool updating = false;
		private void updateBitrateSize()
		{
            if (updating) return;
            updating = true;
			bool succeeded = false;	
            if (fileSizeRadio.Checked)
				succeeded = updateBitrate();
			else
				succeeded = updateSize();
            if (!succeeded)
            {
                videoSize.Text = "";
                if (fileSizeRadio.Checked)
                    projectedBitrate.Value = 0;
				else
                    targetSize.Value = null;
			}
            updating = false;
		}
		/// <summary>
		/// calculates the bitrate and shows the results of the calculation in the appropriate gui fields
		/// </summary>
		private bool updateBitrate()
		{
            if (!targetSize.Value.HasValue) return false;

            ulong muxedSizeBytes;
            checked { muxedSizeBytes = targetSize.Value.Value.Bytes; }
            ulong numberOfFrames;
            double framerate;
            AudioBitrateCalculationStream[] audioStreams;
            VideoCodec vCodec;
            ContainerType containerType;
            if (!getInfo(out vCodec, out audioStreams, out containerType, out numberOfFrames, out framerate))
                return false;
			int bitrateKBits;
			ulong videoSizeKBs = 0;

            try
            {
                bitrateKBits = BitrateCalculator.CalculateBitrateKBits(vCodec, bframes.Checked, containerType, audioStreams, muxedSizeBytes,
                numberOfFrames, framerate, out videoSizeKBs);
            }
            catch (CalculationException)
            {
                videoSize.Text = "Calculation failed";
                projectedBitrate.Value = 0;
                return false;
            }

            try
            {
                videoSize.Text = new FileSize(Unit.KB, videoSizeKBs).ToString();
                this.projectedBitrate.Value = bitrateKBits;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
            // UNSET HERE
            return true;
		}
		#endregion

        private void targetSize_SelectionChanged(object sender, string val)
        {
            updateBitrateSize();
        }

        private void fpsChooser_SelectionChanged(object sender, string val)
        {
            double framerate = (double)fpsChooser.Value;
            int length = (int)totalSeconds.Value;
            int numberOfFrames = (int)(length * framerate);
            nbFrames.Value = numberOfFrames;
            this.updateBitrateSize();

        }

        private void addTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void removeTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (audioTabs.Count == 1)
                removeTrackToolStripMenuItem.Enabled = false;
            else
                removeTrackToolStripMenuItem.Enabled = true;
        }

        private void audioTrackSizeTab1_SomethingChanged(object sender, EventArgs e)
        {
            updateContainers();
            updateBitrateSize();
        }

	}
    public class CalculatorTool : MeGUI.core.plugins.interfaces.ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "Bitrate Calculator"; }
        }

        public void Run(MainForm info)
        {
            using (Calculator calc = new Calculator(info))
            {
                ulong nbFrames = 0;
                double framerate = 0.0;
                if (!string.IsNullOrEmpty(info.Video.VideoInput))
                    JobUtil.getInputProperties(out nbFrames, out framerate, info.Video.VideoInput);

                calc.setDefaults(nbFrames, framerate, info.Video.CurrentSettings, info.Audio.AudioStreams);

                DialogResult dr = calc.ShowDialog();
                if (dr != DialogResult.OK)
                    return;

                if (info.Video.CurrentSettings.EncoderType != calc.SelectedVCodec)
                    return;

                dr = MessageBox.Show("Copy calculated bitrate into video settings?", "Save calculated bitrate?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                    return;

                VideoCodecSettings settings = info.Video.CurrentSettings;
                if (settings.EncodingMode == 1 || settings.EncodingMode == 9)
                {
                    settings.EncodingMode = 0;
                }
                settings.BitrateQuantizer = calc.getBitrate();
            }
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlB }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "bitrate_calculator_window"; }
        }

        #endregion
    }
}
