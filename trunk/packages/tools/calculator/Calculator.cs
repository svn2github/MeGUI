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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using MeGUI.core.util;

namespace MeGUI
{
 
	/// <summary>
	/// Summary description for Calculator.
	/// </summary>
	public class Calculator : System.Windows.Forms.Form
	{
		#region variables
        private bool updatingContainers = false;
		private BitrateCalculator calc;
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
		private System.Windows.Forms.GroupBox audio1Groupbox;
        private System.Windows.Forms.GroupBox audio2Groupbox;
		private System.Windows.Forms.Button selectAudio2Button;
		private System.Windows.Forms.Label audio2KBLabel;
		private System.Windows.Forms.Label audio2MBLabel;
        private System.Windows.Forms.Button selectAudio1Button;
		private System.Windows.Forms.ComboBox audio1Type;
		private System.Windows.Forms.Label audio1TypeLabel;
		private System.Windows.Forms.ComboBox audio2Type;
        private System.Windows.Forms.Label audio2TypeLabel;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Label audio1MBLabel;
		private System.Windows.Forms.Label audio1KBLabel;
        private System.Windows.Forms.GroupBox codecGroupbox;
        private System.Windows.Forms.GroupBox sizeGroupbox;
		private System.Windows.Forms.RadioButton averageBitrateRadio;
        private System.Windows.Forms.RadioButton fileSizeRadio;
        private System.Windows.Forms.Label AverageBitrateLabel;
        private System.Windows.Forms.GroupBox resultGroupbox;
        private System.Windows.Forms.Label nbFramesLabel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button clearAudio1Button;
		private System.Windows.Forms.Button clearAudio2Button;
		private System.Windows.Forms.GroupBox containerGroupbox;
        private System.Windows.Forms.CheckBox bframes;
        private ComboBox videoCodec;
        private ComboBox containerFormat;
        private NumericUpDown audio1Bitrate;
        private NumericUpDown audio2Bitrate;
        private NumericUpDown nbFrames;
        private NumericUpDown totalSeconds;
        private NumericUpDown audio1SizeMB;
        private NumericUpDown audio1SizeKB;
        private NumericUpDown audio2SizeMB;
        private NumericUpDown audio2SizeKB;
        private NumericUpDown projectedBitrate;
        private Label label2;
        private Label label1;
        private Label label4;
        private Label label3;
        private MeGUI.core.gui.HelpButton helpButton1;
        private MeGUI.core.gui.TargetSizeSCBox targetSize;
        private TextBox videoSize;
        private Label VideoFileSizeLabel;
        private MeGUI.core.gui.FPSChooser fpsChooser;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		#region start / stop
		public Calculator(MainForm mainForm)
		{
            InitializeComponent();
            this.mainForm = mainForm;
            this.muxProvider = mainForm.MuxProvider;
            this.videoCodec.Items.AddRange(CodecManager.ListOfVideoCodecs);
            this.audio1Type.Items.AddRange(ContainerManager.AudioTypes.ValuesArray);
            this.audio2Type.Items.AddRange(ContainerManager.AudioTypes.ValuesArray);
            videoCodec.SelectedItem = CodecManager.X264;
            this.containerFormat.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
            containerFormat.SelectedItem = ContainerType.MKV;
			calc = new BitrateCalculator();
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
            this.audio1Groupbox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.audio1SizeMB = new System.Windows.Forms.NumericUpDown();
            this.audio1SizeKB = new System.Windows.Forms.NumericUpDown();
            this.audio1Bitrate = new System.Windows.Forms.NumericUpDown();
            this.audio1MBLabel = new System.Windows.Forms.Label();
            this.audio1KBLabel = new System.Windows.Forms.Label();
            this.selectAudio1Button = new System.Windows.Forms.Button();
            this.audio1Type = new System.Windows.Forms.ComboBox();
            this.audio1TypeLabel = new System.Windows.Forms.Label();
            this.clearAudio1Button = new System.Windows.Forms.Button();
            this.audio2Groupbox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.audio2SizeMB = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.audio2SizeKB = new System.Windows.Forms.NumericUpDown();
            this.audio2Bitrate = new System.Windows.Forms.NumericUpDown();
            this.clearAudio2Button = new System.Windows.Forms.Button();
            this.audio2Type = new System.Windows.Forms.ComboBox();
            this.audio2TypeLabel = new System.Windows.Forms.Label();
            this.audio2MBLabel = new System.Windows.Forms.Label();
            this.audio2KBLabel = new System.Windows.Forms.Label();
            this.selectAudio2Button = new System.Windows.Forms.Button();
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
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.videoGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hours)).BeginInit();
            this.audio1Groupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.audio1SizeMB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.audio1SizeKB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.audio1Bitrate)).BeginInit();
            this.audio2Groupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.audio2SizeMB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.audio2SizeKB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.audio2Bitrate)).BeginInit();
            this.codecGroupbox.SuspendLayout();
            this.containerGroupbox.SuspendLayout();
            this.sizeGroupbox.SuspendLayout();
            this.resultGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectedBitrate)).BeginInit();
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
            this.fpsChooser.SelectedIndex = 0;
            this.fpsChooser.Size = new System.Drawing.Size(129, 29);
            this.fpsChooser.TabIndex = 13;
            this.fpsChooser.SelectionChanged += new MeGUI.StringChanged(this.fpsChooser_SelectionChanged);
            // 
            // nbFrames
            // 
            this.nbFrames.Location = new System.Drawing.Point(189, 105);
            this.nbFrames.Maximum = new decimal(new int[] {
            5400000,
            0,
            0,
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
            this.applyButton.Location = new System.Drawing.Point(517, 336);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(48, 23);
            this.applyButton.TabIndex = 0;
            this.applyButton.Text = "Apply";
            // 
            // audio1Groupbox
            // 
            this.audio1Groupbox.Controls.Add(this.label2);
            this.audio1Groupbox.Controls.Add(this.label1);
            this.audio1Groupbox.Controls.Add(this.audio1SizeMB);
            this.audio1Groupbox.Controls.Add(this.audio1SizeKB);
            this.audio1Groupbox.Controls.Add(this.audio1Bitrate);
            this.audio1Groupbox.Controls.Add(this.audio1MBLabel);
            this.audio1Groupbox.Controls.Add(this.audio1KBLabel);
            this.audio1Groupbox.Controls.Add(this.selectAudio1Button);
            this.audio1Groupbox.Controls.Add(this.audio1Type);
            this.audio1Groupbox.Controls.Add(this.audio1TypeLabel);
            this.audio1Groupbox.Controls.Add(this.clearAudio1Button);
            this.audio1Groupbox.Location = new System.Drawing.Point(8, 168);
            this.audio1Groupbox.Name = "audio1Groupbox";
            this.audio1Groupbox.Size = new System.Drawing.Size(156, 200);
            this.audio1Groupbox.TabIndex = 5;
            this.audio1Groupbox.TabStop = false;
            this.audio1Groupbox.Text = "AudioTrack 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bitrate";
            // 
            // audio1SizeMB
            // 
            this.audio1SizeMB.Location = new System.Drawing.Point(8, 110);
            this.audio1SizeMB.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.audio1SizeMB.Name = "audio1SizeMB";
            this.audio1SizeMB.Size = new System.Drawing.Size(97, 21);
            this.audio1SizeMB.TabIndex = 5;
            this.audio1SizeMB.ValueChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // audio1SizeKB
            // 
            this.audio1SizeKB.Location = new System.Drawing.Point(8, 83);
            this.audio1SizeKB.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.audio1SizeKB.Name = "audio1SizeKB";
            this.audio1SizeKB.Size = new System.Drawing.Size(97, 21);
            this.audio1SizeKB.TabIndex = 3;
            this.audio1SizeKB.ValueChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // audio1Bitrate
            // 
            this.audio1Bitrate.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.audio1Bitrate.Location = new System.Drawing.Point(8, 43);
            this.audio1Bitrate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.audio1Bitrate.Name = "audio1Bitrate";
            this.audio1Bitrate.Size = new System.Drawing.Size(97, 21);
            this.audio1Bitrate.TabIndex = 1;
            this.audio1Bitrate.ValueChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // audio1MBLabel
            // 
            this.audio1MBLabel.Location = new System.Drawing.Point(108, 112);
            this.audio1MBLabel.Name = "audio1MBLabel";
            this.audio1MBLabel.Size = new System.Drawing.Size(24, 16);
            this.audio1MBLabel.TabIndex = 19;
            this.audio1MBLabel.Text = "MB";
            // 
            // audio1KBLabel
            // 
            this.audio1KBLabel.Location = new System.Drawing.Point(108, 85);
            this.audio1KBLabel.Name = "audio1KBLabel";
            this.audio1KBLabel.Size = new System.Drawing.Size(24, 16);
            this.audio1KBLabel.TabIndex = 4;
            this.audio1KBLabel.Text = "KB";
            // 
            // selectAudio1Button
            // 
            this.selectAudio1Button.Location = new System.Drawing.Point(8, 164);
            this.selectAudio1Button.Name = "selectAudio1Button";
            this.selectAudio1Button.Size = new System.Drawing.Size(75, 23);
            this.selectAudio1Button.TabIndex = 8;
            this.selectAudio1Button.Text = "Select";
            this.selectAudio1Button.Click += new System.EventHandler(this.selectAudio1Button_Click);
            // 
            // audio1Type
            // 
            this.audio1Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audio1Type.Location = new System.Drawing.Point(54, 137);
            this.audio1Type.Name = "audio1Type";
            this.audio1Type.Size = new System.Drawing.Size(84, 21);
            this.audio1Type.TabIndex = 7;
            this.audio1Type.SelectedIndexChanged += new System.EventHandler(this.audio_SelectedIndexChanged);
            // 
            // audio1TypeLabel
            // 
            this.audio1TypeLabel.Location = new System.Drawing.Point(8, 140);
            this.audio1TypeLabel.Name = "audio1TypeLabel";
            this.audio1TypeLabel.Size = new System.Drawing.Size(40, 16);
            this.audio1TypeLabel.TabIndex = 6;
            this.audio1TypeLabel.Text = "Type";
            // 
            // clearAudio1Button
            // 
            this.clearAudio1Button.Location = new System.Drawing.Point(89, 164);
            this.clearAudio1Button.Name = "clearAudio1Button";
            this.clearAudio1Button.Size = new System.Drawing.Size(24, 23);
            this.clearAudio1Button.TabIndex = 9;
            this.clearAudio1Button.Text = "X";
            this.clearAudio1Button.Click += new System.EventHandler(this.clearAudioButton_Click);
            // 
            // audio2Groupbox
            // 
            this.audio2Groupbox.Controls.Add(this.label4);
            this.audio2Groupbox.Controls.Add(this.audio2SizeMB);
            this.audio2Groupbox.Controls.Add(this.label3);
            this.audio2Groupbox.Controls.Add(this.audio2SizeKB);
            this.audio2Groupbox.Controls.Add(this.audio2Bitrate);
            this.audio2Groupbox.Controls.Add(this.clearAudio2Button);
            this.audio2Groupbox.Controls.Add(this.audio2Type);
            this.audio2Groupbox.Controls.Add(this.audio2TypeLabel);
            this.audio2Groupbox.Controls.Add(this.audio2MBLabel);
            this.audio2Groupbox.Controls.Add(this.audio2KBLabel);
            this.audio2Groupbox.Controls.Add(this.selectAudio2Button);
            this.audio2Groupbox.Location = new System.Drawing.Point(168, 168);
            this.audio2Groupbox.Name = "audio2Groupbox";
            this.audio2Groupbox.Size = new System.Drawing.Size(160, 200);
            this.audio2Groupbox.TabIndex = 6;
            this.audio2Groupbox.TabStop = false;
            this.audio2Groupbox.Text = "AudioTrack 2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Size";
            // 
            // audio2SizeMB
            // 
            this.audio2SizeMB.Location = new System.Drawing.Point(16, 110);
            this.audio2SizeMB.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.audio2SizeMB.Name = "audio2SizeMB";
            this.audio2SizeMB.Size = new System.Drawing.Size(100, 21);
            this.audio2SizeMB.TabIndex = 5;
            this.audio2SizeMB.ValueChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Bitrate";
            // 
            // audio2SizeKB
            // 
            this.audio2SizeKB.Location = new System.Drawing.Point(16, 83);
            this.audio2SizeKB.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.audio2SizeKB.Name = "audio2SizeKB";
            this.audio2SizeKB.Size = new System.Drawing.Size(100, 21);
            this.audio2SizeKB.TabIndex = 3;
            this.audio2SizeKB.ValueChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // audio2Bitrate
            // 
            this.audio2Bitrate.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.audio2Bitrate.Location = new System.Drawing.Point(16, 43);
            this.audio2Bitrate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.audio2Bitrate.Name = "audio2Bitrate";
            this.audio2Bitrate.Size = new System.Drawing.Size(100, 21);
            this.audio2Bitrate.TabIndex = 1;
            this.audio2Bitrate.ValueChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // clearAudio2Button
            // 
            this.clearAudio2Button.Location = new System.Drawing.Point(97, 164);
            this.clearAudio2Button.Name = "clearAudio2Button";
            this.clearAudio2Button.Size = new System.Drawing.Size(24, 23);
            this.clearAudio2Button.TabIndex = 10;
            this.clearAudio2Button.Text = "X";
            this.clearAudio2Button.Click += new System.EventHandler(this.clearAudioButton_Click);
            // 
            // audio2Type
            // 
            this.audio2Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audio2Type.Location = new System.Drawing.Point(59, 137);
            this.audio2Type.Name = "audio2Type";
            this.audio2Type.Size = new System.Drawing.Size(84, 21);
            this.audio2Type.TabIndex = 8;
            this.audio2Type.SelectedIndexChanged += new System.EventHandler(this.audio_SelectedIndexChanged);
            // 
            // audio2TypeLabel
            // 
            this.audio2TypeLabel.Location = new System.Drawing.Point(13, 140);
            this.audio2TypeLabel.Name = "audio2TypeLabel";
            this.audio2TypeLabel.Size = new System.Drawing.Size(40, 16);
            this.audio2TypeLabel.TabIndex = 7;
            this.audio2TypeLabel.Text = "Type";
            // 
            // audio2MBLabel
            // 
            this.audio2MBLabel.Location = new System.Drawing.Point(122, 112);
            this.audio2MBLabel.Name = "audio2MBLabel";
            this.audio2MBLabel.Size = new System.Drawing.Size(24, 16);
            this.audio2MBLabel.TabIndex = 6;
            this.audio2MBLabel.Text = "MB";
            // 
            // audio2KBLabel
            // 
            this.audio2KBLabel.Location = new System.Drawing.Point(122, 85);
            this.audio2KBLabel.Name = "audio2KBLabel";
            this.audio2KBLabel.Size = new System.Drawing.Size(24, 16);
            this.audio2KBLabel.TabIndex = 4;
            this.audio2KBLabel.Text = "KB";
            // 
            // selectAudio2Button
            // 
            this.selectAudio2Button.Location = new System.Drawing.Point(16, 164);
            this.selectAudio2Button.Name = "selectAudio2Button";
            this.selectAudio2Button.Size = new System.Drawing.Size(75, 23);
            this.selectAudio2Button.TabIndex = 9;
            this.selectAudio2Button.Text = "Select";
            this.selectAudio2Button.Click += new System.EventHandler(this.selectAudio2Button_Click);
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
            10000000,
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
            this.cancelButton.Location = new System.Drawing.Point(462, 336);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Bitrate calculator";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(344, 336);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 8;
            // 
            // Calculator
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(576, 377);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.resultGroupbox);
            this.Controls.Add(this.sizeGroupbox);
            this.Controls.Add(this.containerGroupbox);
            this.Controls.Add(this.codecGroupbox);
            this.Controls.Add(this.audio2Groupbox);
            this.Controls.Add(this.audio1Groupbox);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.videoGroupbox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Calculator";
            this.Text = "Calculator";
            this.videoGroupbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nbFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hours)).EndInit();
            this.audio1Groupbox.ResumeLayout(false);
            this.audio1Groupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.audio1SizeMB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.audio1SizeKB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.audio1Bitrate)).EndInit();
            this.audio2Groupbox.ResumeLayout(false);
            this.audio2Groupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.audio2SizeMB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.audio2SizeKB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.audio2Bitrate)).EndInit();
            this.codecGroupbox.ResumeLayout(false);
            this.containerGroupbox.ResumeLayout(false);
            this.sizeGroupbox.ResumeLayout(false);
            this.resultGroupbox.ResumeLayout(false);
            this.resultGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectedBitrate)).EndInit();
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
		public void setDefaults(ulong nbFrames, double framerate, ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> codec, AudioStream audioStream1, AudioStream audioStream2)
		{
            setFPSToBest(framerate);
            try
            {
                bframes.Checked = codec.GetCurrentSettings().NbBframes > 0;
            }
            catch (Exception) { }

			if (nbFrames > 0)
				this.nbFrames.Value = nbFrames;

            if (videoCodec.Items.Contains(codec))
                videoCodec.SelectedItem = codec;

            if (audioStream1.settings != null)
            {
                audio1Bitrate.Value = audioStream1.settings.Bitrate;
                if (audioStream1.Type != null && audio1Type.Items.Contains(audioStream1.Type))
                    audio1Type.SelectedItem = audioStream1.Type;
            }
            if (audioStream2.settings != null)
            {
                audio2Bitrate.Value = audioStream2.settings.Bitrate;
                if (audioStream2.Type != null && audio2Type.Items.Contains(audioStream2.Type))
                    audio2Type.SelectedItem = audioStream2.Type;
            }
		}
		/// <summary>
		/// gets the selected codec
		/// </summary>
		/// <returns></returns>
		public ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> getSelectedCodec()
		{
            return (ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>)videoCodec.SelectedItem;
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
		#region audio1
		/// <summary>
		/// handles the select button for the first open audio track
		/// launches a file open dialog to allow selection of an audio track
		/// of a type supported by the currently selected container
		/// after opening, the audio type is gotten from the file extension, 
		/// and finally, depending on the container type selected, the audio
		/// type is set in the audio type dropdown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void selectAudio1Button_Click(object sender, System.EventArgs e)
		{
    		openFileDialog.Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.AudioTypes.ValuesArray);
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				FileInfo fi = new FileInfo(openFileDialog.FileName);
				int sizeKB = (int)(fi.Length / 1024);
				int sizeMB = sizeKB/1024;
				AudioType aud1Type = VideoUtil.guessAudioType(openFileDialog.FileName);
				audio1SizeKB.Text = sizeKB.ToString();
				audio1SizeMB.Text = sizeMB.ToString();
                if (audio1Type.Items.Contains(aud1Type))
                    audio1Type.SelectedItem = aud1Type;
			}
            updateContainers();
		}
		/// <summary>
		/// handles a change in the audio bitrate dropdowns
		/// gets the bitrate from the selected item in the dropdown and assigns it to the
		/// currently active audio track
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void audioBitrate_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			NumericUpDown cb = (NumericUpDown)sender;
			if (cb.Value > 0)
			{
				if (cb == audio1Bitrate)
					calculateAudioSize(true, false);
				else if (cb == audio2Bitrate)
					calculateAudioSize(false, true);
			}
		}
		
		#endregion
		#region generic
		/// <summary>
		/// calculates the size of a bitrate based audio track and sets its type
		/// </summary>
		/// <param name="track1">calculate for track1?</param>
		/// <param name="track2">calculate for track2?</param>
		private void calculateAudioSize(bool track1, bool track2)
		{
			int length = (int)totalSeconds.Value;
            if (length <= 0)
                return;
			if (track1)
			{
                int bitrate = (int)audio1Bitrate.Value;
                if (bitrate > 0 && audio1Type.SelectedIndex == -1)
                    audio1Type.SelectedItem = AudioType.VBRMP3;
				double bytesPerSecond = bitrate * 1000 / 8;
				long sizeInBytes = (long)(length * bytesPerSecond);
				long size = sizeInBytes / (long)1024;
				audio1SizeKB.Text = size.ToString();
				size = size / 1024;
				audio1SizeMB.Text = size.ToString();
			}
			if (track2)
			{
                int bitrate = (int)audio2Bitrate.Value; ;
                if (bitrate > 0 && audio2Type.SelectedIndex == -1)
                    audio2Type.SelectedItem = AudioType.VBRMP3;
                double bytesPerSecond = bitrate * 1000 / 8;
				long sizeInBytes = (long)(length * bytesPerSecond);
				long size = sizeInBytes / (long)1024;
				audio2SizeKB.Text = size.ToString();
				size = size / 1024;
				audio2SizeMB.Text = size.ToString();
			}
		}

        private void calculateAudioBitrate(bool track1, bool track2)
        {
            int length = (int)totalSeconds.Value;
            if (length <= 0)
                return;
            if (track1)
            {
                long sizeInBytes = (long)audio1SizeKB.Value * 1024;
                if (sizeInBytes > 0 && audio1Type.SelectedIndex == -1)
                    audio1Type.SelectedItem = AudioType.VBRMP3;
                double bytesPerSecond = (double)sizeInBytes / (double)length;
                int bitrate = (int)(bytesPerSecond * 8.0 / 1000.0);
                audio1Bitrate.Value = bitrate;
            }
            if (track2)
            {
                long sizeInBytes = (long)audio2SizeKB.Value * 1024;
                if (sizeInBytes > 0 && audio2Type.SelectedIndex == -1)
                    audio2Type.SelectedItem = AudioType.VBRMP3;
                double bytesPerSecond = (double)sizeInBytes / (double)length;
                int bitrate = (int)(bytesPerSecond * 8.0 / 1000.0);
                audio2Bitrate.Value = bitrate;
            }
        }
		/// <summary>
		/// clears out an audio track
		/// sets the audio type back to none, sets the bitrate to undefined
		/// and activate bitrate mode
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void clearAudioButton_Click(object sender, System.EventArgs e)
		{
			Button button = (Button)sender;
			if (button == this.clearAudio1Button)
			{
                audio1SizeKB.Value = 0;
                audio1Type.SelectedIndex = -1;
			}
			else if (button == this.clearAudio2Button)
			{
                audio2SizeKB.Value = 0;
                audio2Type.SelectedIndex = -1;
			}
            updateContainers();
			this.updateBitrateSize();
		}
		#endregion
		#region audio 2
		/// <summary>
		/// handles the select button for the first open audio track
		/// launches a file open dialog to allow selection of an audio track
		/// of a type supported by the currently selected container
		/// after opening, the audio type is gotten from the file extension, 
		/// and finally, depending on the container type selected, the audio
		/// type is set in the audio type dropdown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void selectAudio2Button_Click(object sender, System.EventArgs e)
		{
            openFileDialog.Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.AudioTypes.ValuesArray);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(openFileDialog.FileName);
                int sizeKB = (int)(fi.Length / 1024);
                int sizeMB = sizeKB / 1024;
                AudioType aud2Type = VideoUtil.guessAudioType(openFileDialog.FileName);
                audio2SizeKB.Text = sizeKB.ToString();
                audio2SizeMB.Text = sizeMB.ToString();
                if (audio2Type.Items.Contains(aud2Type))
                    audio2Type.SelectedItem = aud2Type;
            }
            updateContainers();
		}
		#endregion
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
                    if (tb == audio1Bitrate)
                    {
                        calculateAudioSize(true, false);
                    }
                    else if (tb == audio2Bitrate)
                    {
                        calculateAudioSize(false, true);
                    }
                    else if (tb == audio1SizeKB)
                    {
                        audio1SizeMB.Value = value / 1024;
                        calculateAudioBitrate(true, false);
                    }
                    else if (tb == audio1SizeMB)
                    {
                        audio1SizeKB.Value = value * 1024;
                        calculateAudioBitrate(true, false);
                    }
                    else if (tb == audio2SizeKB)
                    {
                        audio2SizeMB.Value = value / 1024;
                        calculateAudioBitrate(false, true);
                    }
                    else if (tb == audio2SizeMB)
                    {
                        audio2SizeKB.Value = value * 1024;
                        calculateAudioBitrate(false, true);
                    }
                    else if (tb == totalSeconds)
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
                this.calculateAudioSize(true, true);
                UpdateTotalFrames();
                updateBitrateSize();

                this.isUpdating = false;
            }
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
            VideoEncoderType vCodec = (videoCodec.SelectedItem as ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>).EncoderType;
            List<MuxableType> muxableTypes = new List<MuxableType>();
            AudioType type;
            if (audio1Type.SelectedIndex > -1)
            {
                type = audio1Type.SelectedItem as AudioType;
                if (type.SupportedCodecs.Length > 0)
                    muxableTypes.Add(new MuxableType(type, type.SupportedCodecs[0]));
            }
            if (audio2Type.SelectedIndex > -1)
            {
                type = audio2Type.SelectedItem as AudioType;
                if (type.SupportedCodecs.Length > 0)
                    muxableTypes.Add(new MuxableType(type, type.SupportedCodecs[0]));
            }
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
        #endregion
		#region checkboxes
		private void bframes_CheckedChanged(object sender, System.EventArgs e)
		{
			updateBitrateSize();
		}
		#endregion
		#region bitrate calculations
        private bool getInfo(out VideoCodec codec, out AudioStream[] audioStreamsArray, out ContainerType containerType,
            out ulong numberOfFrames, out double framerate)
        {
            numberOfFrames = (ulong)nbFrames.Value;
            framerate = (double)fpsChooser.Value;
            long[] audioSizes = { ((long)audio1SizeKB.Value)* 1024L, ((long)audio2SizeKB.Value) * 1024L };
            List<AudioStream> audioStreams = new List<AudioStream>();
            AudioStream stream;
            if (audio1Type.SelectedIndex > -1)
            {
                stream = new AudioStream();
                stream.SizeBytes = audioSizes[0];
                stream.Type = audio1Type.SelectedItem as AudioType;
                audioStreams.Add(stream);
            }
            if (audio2Type.SelectedIndex > -1)
            {
                stream = new AudioStream();
                stream.SizeBytes = audioSizes[1];
                stream.Type = audio2Type.SelectedItem as AudioType;
                audioStreams.Add(stream);
            }
            audioStreamsArray = audioStreams.ToArray();
            try
            {
                codec = (videoCodec.SelectedItem as ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>).CodecType;
                containerType = (containerFormat.SelectedItem as ContainerType);
            }
            catch (Exception)
            {
                codec = null;
                containerType = null;
                return false;
            }
            if (numberOfFrames <= 0 || framerate <= 0)
                return false;
            return true;
        }

		private bool updateSize()
		{
            AudioStream[] audioStreams;
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
            long totalSize = calc.CalculateFileSizeKB(vCodec, bframes.Checked, containerType,
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

            long muxedSizeBytes;
            checked { muxedSizeBytes = (long)targetSize.Value.Value.Bytes; }
            if (muxedSizeBytes <= 0)
                return false;
            ulong numberOfFrames;
            double framerate;
            AudioStream[] audioStreams;
            VideoCodec vCodec;
            ContainerType containerType;
            if (!getInfo(out vCodec, out audioStreams, out containerType, out numberOfFrames, out framerate))
                return false;
			int bitrateKBits = 0;
			long videoSizeKBs = 0;
            bitrateKBits = calc.CalculateBitrateKBits(vCodec, bframes.Checked, containerType, audioStreams, muxedSizeBytes,
                numberOfFrames, framerate, out videoSizeKBs);
			int videoSizeMBs = (int) (videoSizeKBs / 1024);
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
                if (!info.Video.VideoInput.Equals(""))
                    info.JobUtil.getInputProperties(out nbFrames, out framerate, info.Video.VideoInput);
                calc.setDefaults(nbFrames, framerate, info.Video.CurrentSettingsProvider, info.Audio.AudioStreams[0], info.Audio.AudioStreams[1]);

                DialogResult dr = calc.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    info.Video.CurrentSettingsProvider = calc.getSelectedCodec();
                    VideoCodecSettings settings = info.Video.CodecHandler.Getter();
                    if (settings.EncodingMode == 1 || settings.EncodingMode == 9)
                    {
                        settings.EncodingMode = 0;
                    }
                    settings.BitrateQuantizer = calc.getBitrate();
                }
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
