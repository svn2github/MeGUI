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
using System.IO;

namespace MeGUI
{
	/// <summary>
	/// Summary description for Calculator.
	/// </summary>
	public class Calculator : System.Windows.Forms.Form
	{
		#region variables
		private BitrateCalculator calc;
		private AudioType aud1Type;
		private AudioType aud2Type;
		private bool isUpdating = false, internalUpdate = false;
		private int oldValue = 9999;
		double containerOverhead = 10.4;
		private System.Windows.Forms.GroupBox videoGroupbox;
		private System.Windows.Forms.NumericUpDown hours;
		private System.Windows.Forms.NumericUpDown minutes;
		private System.Windows.Forms.NumericUpDown seconds;
		private System.Windows.Forms.Label hoursLabel;
		private System.Windows.Forms.Label minutesLabel;
		private System.Windows.Forms.Label secondsLabel;
		private System.Windows.Forms.Label framerateLabel;
		private System.Windows.Forms.TextBox totalSeconds;
		private System.Windows.Forms.Label totalSecondsLabel;
		private System.Windows.Forms.GroupBox audio1Groupbox;
		private System.Windows.Forms.GroupBox audio2Groupbox;
		private System.Windows.Forms.ComboBox audio2Bitrate;
		private System.Windows.Forms.ComboBox framerate;
		private System.Windows.Forms.RadioButton audio2BitrateRadio;
		private System.Windows.Forms.RadioButton audio2SizeRadio;
		private System.Windows.Forms.TextBox audio2SizeKB;
		private System.Windows.Forms.TextBox audio2SizeMB;
		private System.Windows.Forms.Button selectAudio2Button;
		private System.Windows.Forms.Label audio2KBLabel;
		private System.Windows.Forms.Label audio2MBLabel;
		private System.Windows.Forms.Button selectAudio1Button;
		private System.Windows.Forms.TextBox audio1SizeMB;
		private System.Windows.Forms.TextBox audio1SizeKB;
		private System.Windows.Forms.RadioButton audio1SizeRadio;
		private System.Windows.Forms.RadioButton audio1BitrateRadio;
		private System.Windows.Forms.ComboBox audio1Bitrate;
		private System.Windows.Forms.ComboBox audio1Type;
		private System.Windows.Forms.Label audio1TypeLabel;
		private System.Windows.Forms.ComboBox audio2Type;
		private System.Windows.Forms.Label audio2TypeLabel;
		private System.Windows.Forms.ComboBox sizeSelection;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Label audio1MBLabel;
		private System.Windows.Forms.Label audio1KBLabel;
		private System.Windows.Forms.GroupBox codecGroupbox;
		private System.Windows.Forms.RadioButton lavc;
		private System.Windows.Forms.RadioButton snow;
		private System.Windows.Forms.RadioButton x264;
		private System.Windows.Forms.RadioButton xvid;
		private System.Windows.Forms.GroupBox sizeGroupbox;
		private System.Windows.Forms.Label storageMediumLabel;
		private System.Windows.Forms.Label videoSizeKBLabel;
		private System.Windows.Forms.TextBox videoSizeKB;
		private System.Windows.Forms.Label VideoFileSizeLabel;
		private System.Windows.Forms.RadioButton averageBitrateRadio;
		private System.Windows.Forms.RadioButton fileSizeRadio;
		private System.Windows.Forms.TextBox projectedBitrate;
		private System.Windows.Forms.Label AverageBitrateLabel;
		private System.Windows.Forms.TextBox muxedSizeKB;
		private System.Windows.Forms.Label muxedSizeKBLabel;
		private System.Windows.Forms.GroupBox resultGroupbox;
		private System.Windows.Forms.TextBox videoSizeMB;
		private System.Windows.Forms.Label videoSizeMBLabel;
		private System.Windows.Forms.Label muxedSizeMBLabel;
		private System.Windows.Forms.TextBox muxedSizeMB;
		private System.Windows.Forms.VScrollBar filesizeScrollbar;
		private System.Windows.Forms.Label nbFramesLabel;
		private System.Windows.Forms.TextBox nbFrames;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button clearAudio1Button;
		private System.Windows.Forms.Button clearAudio2Button;
		private System.Windows.Forms.GroupBox containerGroupbox;
		private System.Windows.Forms.CheckBox bframes;
		private System.Windows.Forms.RadioButton mp4;
		private System.Windows.Forms.RadioButton mkv;
		private System.Windows.Forms.RadioButton avi;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		#region start / stop
		public Calculator()
		{
			InitializeComponent();
			calc = new BitrateCalculator();
			sizeSelection.Items.AddRange(calc.getPredefinedOutputSizes());
			this.framerate.Items.AddRange(new object[] {23.976, 24.0, 25.0, 29.97, 30.0, 50.0, 59.94, 60.0});
			aud1Type = null;
			aud2Type = null;
			this.audio1Bitrate.Items.AddRange(new object[] {0, 16, 20, 24, 28, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 388, 448, 512, 576, 660, 1375, 1550});
            this.audio2Bitrate.Items.AddRange(new object[] {0, 16, 20, 24, 28, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 388, 448, 512, 576, 660, 1375, 1550});
			this.audio1Bitrate.SelectedIndex = 0;
			this.audio2Bitrate.SelectedIndex = 0;
			framerate.SelectedIndex = 2; // 25 fps is default
			filesizeScrollbar.Value = filesizeScrollbar.Maximum;
			sizeSelection.SelectedIndex = 2;
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
            this.bframes = new System.Windows.Forms.CheckBox();
            this.nbFrames = new System.Windows.Forms.TextBox();
            this.nbFramesLabel = new System.Windows.Forms.Label();
            this.totalSecondsLabel = new System.Windows.Forms.Label();
            this.totalSeconds = new System.Windows.Forms.TextBox();
            this.framerateLabel = new System.Windows.Forms.Label();
            this.secondsLabel = new System.Windows.Forms.Label();
            this.minutesLabel = new System.Windows.Forms.Label();
            this.hoursLabel = new System.Windows.Forms.Label();
            this.seconds = new System.Windows.Forms.NumericUpDown();
            this.minutes = new System.Windows.Forms.NumericUpDown();
            this.hours = new System.Windows.Forms.NumericUpDown();
            this.framerate = new System.Windows.Forms.ComboBox();
            this.applyButton = new System.Windows.Forms.Button();
            this.audio1Groupbox = new System.Windows.Forms.GroupBox();
            this.audio1MBLabel = new System.Windows.Forms.Label();
            this.audio1KBLabel = new System.Windows.Forms.Label();
            this.selectAudio1Button = new System.Windows.Forms.Button();
            this.audio1SizeMB = new System.Windows.Forms.TextBox();
            this.audio1SizeKB = new System.Windows.Forms.TextBox();
            this.audio1SizeRadio = new System.Windows.Forms.RadioButton();
            this.audio1BitrateRadio = new System.Windows.Forms.RadioButton();
            this.audio1Bitrate = new System.Windows.Forms.ComboBox();
            this.audio1Type = new System.Windows.Forms.ComboBox();
            this.audio1TypeLabel = new System.Windows.Forms.Label();
            this.clearAudio1Button = new System.Windows.Forms.Button();
            this.audio2Groupbox = new System.Windows.Forms.GroupBox();
            this.clearAudio2Button = new System.Windows.Forms.Button();
            this.audio2Type = new System.Windows.Forms.ComboBox();
            this.audio2TypeLabel = new System.Windows.Forms.Label();
            this.audio2MBLabel = new System.Windows.Forms.Label();
            this.audio2KBLabel = new System.Windows.Forms.Label();
            this.selectAudio2Button = new System.Windows.Forms.Button();
            this.audio2SizeMB = new System.Windows.Forms.TextBox();
            this.audio2SizeKB = new System.Windows.Forms.TextBox();
            this.audio2SizeRadio = new System.Windows.Forms.RadioButton();
            this.audio2BitrateRadio = new System.Windows.Forms.RadioButton();
            this.audio2Bitrate = new System.Windows.Forms.ComboBox();
            this.codecGroupbox = new System.Windows.Forms.GroupBox();
            this.xvid = new System.Windows.Forms.RadioButton();
            this.x264 = new System.Windows.Forms.RadioButton();
            this.snow = new System.Windows.Forms.RadioButton();
            this.lavc = new System.Windows.Forms.RadioButton();
            this.containerGroupbox = new System.Windows.Forms.GroupBox();
            this.mp4 = new System.Windows.Forms.RadioButton();
            this.avi = new System.Windows.Forms.RadioButton();
            this.mkv = new System.Windows.Forms.RadioButton();
            this.sizeGroupbox = new System.Windows.Forms.GroupBox();
            this.muxedSizeMBLabel = new System.Windows.Forms.Label();
            this.muxedSizeMB = new System.Windows.Forms.TextBox();
            this.sizeSelection = new System.Windows.Forms.ComboBox();
            this.storageMediumLabel = new System.Windows.Forms.Label();
            this.fileSizeRadio = new System.Windows.Forms.RadioButton();
            this.muxedSizeKBLabel = new System.Windows.Forms.Label();
            this.muxedSizeKB = new System.Windows.Forms.TextBox();
            this.filesizeScrollbar = new System.Windows.Forms.VScrollBar();
            this.videoSizeKBLabel = new System.Windows.Forms.Label();
            this.videoSizeKB = new System.Windows.Forms.TextBox();
            this.VideoFileSizeLabel = new System.Windows.Forms.Label();
            this.averageBitrateRadio = new System.Windows.Forms.RadioButton();
            this.projectedBitrate = new System.Windows.Forms.TextBox();
            this.AverageBitrateLabel = new System.Windows.Forms.Label();
            this.resultGroupbox = new System.Windows.Forms.GroupBox();
            this.videoSizeMBLabel = new System.Windows.Forms.Label();
            this.videoSizeMB = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.videoGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hours)).BeginInit();
            this.audio1Groupbox.SuspendLayout();
            this.audio2Groupbox.SuspendLayout();
            this.codecGroupbox.SuspendLayout();
            this.containerGroupbox.SuspendLayout();
            this.sizeGroupbox.SuspendLayout();
            this.resultGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // videoGroupbox
            // 
            this.videoGroupbox.Controls.Add(this.bframes);
            this.videoGroupbox.Controls.Add(this.nbFrames);
            this.videoGroupbox.Controls.Add(this.nbFramesLabel);
            this.videoGroupbox.Controls.Add(this.totalSecondsLabel);
            this.videoGroupbox.Controls.Add(this.totalSeconds);
            this.videoGroupbox.Controls.Add(this.framerateLabel);
            this.videoGroupbox.Controls.Add(this.secondsLabel);
            this.videoGroupbox.Controls.Add(this.minutesLabel);
            this.videoGroupbox.Controls.Add(this.hoursLabel);
            this.videoGroupbox.Controls.Add(this.seconds);
            this.videoGroupbox.Controls.Add(this.minutes);
            this.videoGroupbox.Controls.Add(this.hours);
            this.videoGroupbox.Controls.Add(this.framerate);
            this.videoGroupbox.Location = new System.Drawing.Point(8, 8);
            this.videoGroupbox.Name = "videoGroupbox";
            this.videoGroupbox.Size = new System.Drawing.Size(320, 160);
            this.videoGroupbox.TabIndex = 0;
            this.videoGroupbox.TabStop = false;
            this.videoGroupbox.Text = "Video";
            // 
            // bframes
            // 
            this.bframes.Checked = true;
            this.bframes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bframes.Location = new System.Drawing.Point(8, 136);
            this.bframes.Name = "bframes";
            this.bframes.Size = new System.Drawing.Size(120, 16);
            this.bframes.TabIndex = 45;
            this.bframes.Text = "B-frames";
            this.bframes.CheckedChanged += new System.EventHandler(this.bframes_CheckedChanged);
            // 
            // nbFrames
            // 
            this.nbFrames.Location = new System.Drawing.Point(208, 104);
            this.nbFrames.MaxLength = 7;
            this.nbFrames.Name = "nbFrames";
            this.nbFrames.Size = new System.Drawing.Size(100, 21);
            this.nbFrames.TabIndex = 44;
            this.nbFrames.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.nbFrames.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // nbFramesLabel
            // 
            this.nbFramesLabel.Location = new System.Drawing.Point(8, 112);
            this.nbFramesLabel.Name = "nbFramesLabel";
            this.nbFramesLabel.Size = new System.Drawing.Size(100, 16);
            this.nbFramesLabel.TabIndex = 43;
            this.nbFramesLabel.Text = "Number of Frames";
            // 
            // totalSecondsLabel
            // 
            this.totalSecondsLabel.Location = new System.Drawing.Point(8, 58);
            this.totalSecondsLabel.Name = "totalSecondsLabel";
            this.totalSecondsLabel.Size = new System.Drawing.Size(128, 16);
            this.totalSecondsLabel.TabIndex = 42;
            this.totalSecondsLabel.Text = "Total Length in Seconds";
            // 
            // totalSeconds
            // 
            this.totalSeconds.Location = new System.Drawing.Point(208, 56);
            this.totalSeconds.MaxLength = 0;
            this.totalSeconds.Name = "totalSeconds";
            this.totalSeconds.Size = new System.Drawing.Size(100, 21);
            this.totalSeconds.TabIndex = 40;
            this.totalSeconds.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.totalSeconds.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // framerateLabel
            // 
            this.framerateLabel.Location = new System.Drawing.Point(8, 86);
            this.framerateLabel.Name = "framerateLabel";
            this.framerateLabel.Size = new System.Drawing.Size(75, 16);
            this.framerateLabel.TabIndex = 39;
            this.framerateLabel.Text = "Framerate";
            // 
            // secondsLabel
            // 
            this.secondsLabel.Location = new System.Drawing.Point(211, 26);
            this.secondsLabel.Name = "secondsLabel";
            this.secondsLabel.Size = new System.Drawing.Size(48, 16);
            this.secondsLabel.TabIndex = 38;
            this.secondsLabel.Text = "Seconds";
            // 
            // minutesLabel
            // 
            this.minutesLabel.Location = new System.Drawing.Point(103, 26);
            this.minutesLabel.Name = "minutesLabel";
            this.minutesLabel.Size = new System.Drawing.Size(48, 16);
            this.minutesLabel.TabIndex = 37;
            this.minutesLabel.Text = "Minutes";
            // 
            // hoursLabel
            // 
            this.hoursLabel.Location = new System.Drawing.Point(8, 26);
            this.hoursLabel.Name = "hoursLabel";
            this.hoursLabel.Size = new System.Drawing.Size(40, 16);
            this.hoursLabel.TabIndex = 36;
            this.hoursLabel.Text = "Hours";
            // 
            // seconds
            // 
            this.seconds.Location = new System.Drawing.Point(262, 24);
            this.seconds.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.seconds.Name = "seconds";
            this.seconds.Size = new System.Drawing.Size(48, 21);
            this.seconds.TabIndex = 35;
            this.seconds.ValueChanged += new System.EventHandler(this.time_ValueChanged);
            // 
            // minutes
            // 
            this.minutes.Location = new System.Drawing.Point(151, 24);
            this.minutes.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.minutes.Name = "minutes";
            this.minutes.Size = new System.Drawing.Size(48, 21);
            this.minutes.TabIndex = 34;
            this.minutes.ValueChanged += new System.EventHandler(this.time_ValueChanged);
            // 
            // hours
            // 
            this.hours.Location = new System.Drawing.Point(48, 24);
            this.hours.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.hours.Name = "hours";
            this.hours.Size = new System.Drawing.Size(40, 21);
            this.hours.TabIndex = 33;
            this.hours.ValueChanged += new System.EventHandler(this.time_ValueChanged);
            // 
            // framerate
            // 
            this.framerate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.framerate.Location = new System.Drawing.Point(230, 80);
            this.framerate.Name = "framerate";
            this.framerate.Size = new System.Drawing.Size(80, 21);
            this.framerate.TabIndex = 32;
            this.framerate.SelectedIndexChanged += new System.EventHandler(this.framerate_SelectedIndexChanged);
            // 
            // applyButton
            // 
            this.applyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.applyButton.Location = new System.Drawing.Point(512, 352);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(48, 23);
            this.applyButton.TabIndex = 1;
            this.applyButton.Text = "Apply";
            // 
            // audio1Groupbox
            // 
            this.audio1Groupbox.Controls.Add(this.audio1MBLabel);
            this.audio1Groupbox.Controls.Add(this.audio1KBLabel);
            this.audio1Groupbox.Controls.Add(this.selectAudio1Button);
            this.audio1Groupbox.Controls.Add(this.audio1SizeMB);
            this.audio1Groupbox.Controls.Add(this.audio1SizeKB);
            this.audio1Groupbox.Controls.Add(this.audio1SizeRadio);
            this.audio1Groupbox.Controls.Add(this.audio1BitrateRadio);
            this.audio1Groupbox.Controls.Add(this.audio1Bitrate);
            this.audio1Groupbox.Controls.Add(this.audio1Type);
            this.audio1Groupbox.Controls.Add(this.audio1TypeLabel);
            this.audio1Groupbox.Controls.Add(this.clearAudio1Button);
            this.audio1Groupbox.Location = new System.Drawing.Point(8, 168);
            this.audio1Groupbox.Name = "audio1Groupbox";
            this.audio1Groupbox.Size = new System.Drawing.Size(156, 216);
            this.audio1Groupbox.TabIndex = 2;
            this.audio1Groupbox.TabStop = false;
            this.audio1Groupbox.Text = "AudioTrack 1";
            // 
            // audio1MBLabel
            // 
            this.audio1MBLabel.Location = new System.Drawing.Point(128, 120);
            this.audio1MBLabel.Name = "audio1MBLabel";
            this.audio1MBLabel.Size = new System.Drawing.Size(24, 16);
            this.audio1MBLabel.TabIndex = 19;
            this.audio1MBLabel.Text = "MB";
            // 
            // audio1KBLabel
            // 
            this.audio1KBLabel.Location = new System.Drawing.Point(128, 96);
            this.audio1KBLabel.Name = "audio1KBLabel";
            this.audio1KBLabel.Size = new System.Drawing.Size(24, 16);
            this.audio1KBLabel.TabIndex = 18;
            this.audio1KBLabel.Text = "KB";
            // 
            // selectAudio1Button
            // 
            this.selectAudio1Button.Location = new System.Drawing.Point(16, 184);
            this.selectAudio1Button.Name = "selectAudio1Button";
            this.selectAudio1Button.Size = new System.Drawing.Size(75, 23);
            this.selectAudio1Button.TabIndex = 17;
            this.selectAudio1Button.Text = "Select";
            this.selectAudio1Button.Click += new System.EventHandler(this.selectAudio1Button_Click);
            // 
            // audio1SizeMB
            // 
            this.audio1SizeMB.Enabled = false;
            this.audio1SizeMB.Location = new System.Drawing.Point(16, 120);
            this.audio1SizeMB.MaxLength = 4;
            this.audio1SizeMB.Name = "audio1SizeMB";
            this.audio1SizeMB.Size = new System.Drawing.Size(100, 21);
            this.audio1SizeMB.TabIndex = 16;
            this.audio1SizeMB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.audio1SizeMB.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // audio1SizeKB
            // 
            this.audio1SizeKB.Enabled = false;
            this.audio1SizeKB.Location = new System.Drawing.Point(16, 96);
            this.audio1SizeKB.MaxLength = 7;
            this.audio1SizeKB.Name = "audio1SizeKB";
            this.audio1SizeKB.Size = new System.Drawing.Size(100, 21);
            this.audio1SizeKB.TabIndex = 15;
            this.audio1SizeKB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.audio1SizeKB.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // audio1SizeRadio
            // 
            this.audio1SizeRadio.Location = new System.Drawing.Point(8, 72);
            this.audio1SizeRadio.Name = "audio1SizeRadio";
            this.audio1SizeRadio.Size = new System.Drawing.Size(48, 24);
            this.audio1SizeRadio.TabIndex = 14;
            this.audio1SizeRadio.Text = "Size";
            this.audio1SizeRadio.CheckedChanged += new System.EventHandler(this.audio1Radio_CheckedChanged);
            // 
            // audio1BitrateRadio
            // 
            this.audio1BitrateRadio.Checked = true;
            this.audio1BitrateRadio.Location = new System.Drawing.Point(8, 16);
            this.audio1BitrateRadio.Name = "audio1BitrateRadio";
            this.audio1BitrateRadio.Size = new System.Drawing.Size(80, 24);
            this.audio1BitrateRadio.TabIndex = 12;
            this.audio1BitrateRadio.TabStop = true;
            this.audio1BitrateRadio.Text = "Bitrate";
            this.audio1BitrateRadio.CheckedChanged += new System.EventHandler(this.audio1Radio_CheckedChanged);
            // 
            // audio1Bitrate
            // 
            this.audio1Bitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audio1Bitrate.Location = new System.Drawing.Point(16, 40);
            this.audio1Bitrate.Name = "audio1Bitrate";
            this.audio1Bitrate.Size = new System.Drawing.Size(72, 21);
            this.audio1Bitrate.TabIndex = 13;
            this.audio1Bitrate.SelectedIndexChanged += new System.EventHandler(this.audioBitrate_SelectedIndexChanged);
            // 
            // audio1Type
            // 
            this.audio1Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audio1Type.Enabled = false;
            this.audio1Type.Location = new System.Drawing.Point(64, 152);
            this.audio1Type.Name = "audio1Type";
            this.audio1Type.Size = new System.Drawing.Size(56, 21);
            this.audio1Type.TabIndex = 6;
            this.audio1Type.SelectedIndexChanged += new System.EventHandler(this.audio1Type_SelectedIndexChanged);
            // 
            // audio1TypeLabel
            // 
            this.audio1TypeLabel.Location = new System.Drawing.Point(16, 152);
            this.audio1TypeLabel.Name = "audio1TypeLabel";
            this.audio1TypeLabel.Size = new System.Drawing.Size(40, 16);
            this.audio1TypeLabel.TabIndex = 6;
            this.audio1TypeLabel.Text = "Type";
            // 
            // clearAudio1Button
            // 
            this.clearAudio1Button.Location = new System.Drawing.Point(124, 184);
            this.clearAudio1Button.Name = "clearAudio1Button";
            this.clearAudio1Button.Size = new System.Drawing.Size(24, 23);
            this.clearAudio1Button.TabIndex = 34;
            this.clearAudio1Button.Text = "X";
            this.clearAudio1Button.Click += new System.EventHandler(this.clearAudioButton_Click);
            // 
            // audio2Groupbox
            // 
            this.audio2Groupbox.Controls.Add(this.clearAudio2Button);
            this.audio2Groupbox.Controls.Add(this.audio2Type);
            this.audio2Groupbox.Controls.Add(this.audio2TypeLabel);
            this.audio2Groupbox.Controls.Add(this.audio2MBLabel);
            this.audio2Groupbox.Controls.Add(this.audio2KBLabel);
            this.audio2Groupbox.Controls.Add(this.selectAudio2Button);
            this.audio2Groupbox.Controls.Add(this.audio2SizeMB);
            this.audio2Groupbox.Controls.Add(this.audio2SizeKB);
            this.audio2Groupbox.Controls.Add(this.audio2SizeRadio);
            this.audio2Groupbox.Controls.Add(this.audio2BitrateRadio);
            this.audio2Groupbox.Controls.Add(this.audio2Bitrate);
            this.audio2Groupbox.Location = new System.Drawing.Point(168, 168);
            this.audio2Groupbox.Name = "audio2Groupbox";
            this.audio2Groupbox.Size = new System.Drawing.Size(160, 216);
            this.audio2Groupbox.TabIndex = 3;
            this.audio2Groupbox.TabStop = false;
            this.audio2Groupbox.Text = "AudioTrack 2";
            // 
            // clearAudio2Button
            // 
            this.clearAudio2Button.Location = new System.Drawing.Point(128, 184);
            this.clearAudio2Button.Name = "clearAudio2Button";
            this.clearAudio2Button.Size = new System.Drawing.Size(24, 23);
            this.clearAudio2Button.TabIndex = 35;
            this.clearAudio2Button.Text = "X";
            this.clearAudio2Button.Click += new System.EventHandler(this.clearAudioButton_Click);
            // 
            // audio2Type
            // 
            this.audio2Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audio2Type.Enabled = false;
            this.audio2Type.Location = new System.Drawing.Point(64, 152);
            this.audio2Type.Name = "audio2Type";
            this.audio2Type.Size = new System.Drawing.Size(56, 21);
            this.audio2Type.TabIndex = 13;
            this.audio2Type.SelectedIndexChanged += new System.EventHandler(this.audio2Type_SelectedIndexChanged);
            // 
            // audio2TypeLabel
            // 
            this.audio2TypeLabel.Location = new System.Drawing.Point(16, 152);
            this.audio2TypeLabel.Name = "audio2TypeLabel";
            this.audio2TypeLabel.Size = new System.Drawing.Size(40, 16);
            this.audio2TypeLabel.TabIndex = 12;
            this.audio2TypeLabel.Text = "Type";
            // 
            // audio2MBLabel
            // 
            this.audio2MBLabel.Location = new System.Drawing.Point(128, 120);
            this.audio2MBLabel.Name = "audio2MBLabel";
            this.audio2MBLabel.Size = new System.Drawing.Size(24, 16);
            this.audio2MBLabel.TabIndex = 11;
            this.audio2MBLabel.Text = "MB";
            // 
            // audio2KBLabel
            // 
            this.audio2KBLabel.Location = new System.Drawing.Point(128, 96);
            this.audio2KBLabel.Name = "audio2KBLabel";
            this.audio2KBLabel.Size = new System.Drawing.Size(24, 16);
            this.audio2KBLabel.TabIndex = 10;
            this.audio2KBLabel.Text = "KB";
            // 
            // selectAudio2Button
            // 
            this.selectAudio2Button.Location = new System.Drawing.Point(16, 184);
            this.selectAudio2Button.Name = "selectAudio2Button";
            this.selectAudio2Button.Size = new System.Drawing.Size(75, 23);
            this.selectAudio2Button.TabIndex = 9;
            this.selectAudio2Button.Text = "Select";
            this.selectAudio2Button.Click += new System.EventHandler(this.selectAudio2Button_Click);
            // 
            // audio2SizeMB
            // 
            this.audio2SizeMB.Enabled = false;
            this.audio2SizeMB.Location = new System.Drawing.Point(16, 120);
            this.audio2SizeMB.MaxLength = 4;
            this.audio2SizeMB.Name = "audio2SizeMB";
            this.audio2SizeMB.Size = new System.Drawing.Size(100, 21);
            this.audio2SizeMB.TabIndex = 8;
            this.audio2SizeMB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.audio2SizeMB.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // audio2SizeKB
            // 
            this.audio2SizeKB.Enabled = false;
            this.audio2SizeKB.Location = new System.Drawing.Point(16, 96);
            this.audio2SizeKB.MaxLength = 7;
            this.audio2SizeKB.Name = "audio2SizeKB";
            this.audio2SizeKB.Size = new System.Drawing.Size(100, 21);
            this.audio2SizeKB.TabIndex = 7;
            this.audio2SizeKB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.audio2SizeKB.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // audio2SizeRadio
            // 
            this.audio2SizeRadio.Location = new System.Drawing.Point(8, 72);
            this.audio2SizeRadio.Name = "audio2SizeRadio";
            this.audio2SizeRadio.Size = new System.Drawing.Size(48, 24);
            this.audio2SizeRadio.TabIndex = 6;
            this.audio2SizeRadio.Text = "Size";
            this.audio2SizeRadio.CheckedChanged += new System.EventHandler(this.audio2Radio_CheckedChanged);
            // 
            // audio2BitrateRadio
            // 
            this.audio2BitrateRadio.Checked = true;
            this.audio2BitrateRadio.Location = new System.Drawing.Point(8, 16);
            this.audio2BitrateRadio.Name = "audio2BitrateRadio";
            this.audio2BitrateRadio.Size = new System.Drawing.Size(80, 24);
            this.audio2BitrateRadio.TabIndex = 0;
            this.audio2BitrateRadio.TabStop = true;
            this.audio2BitrateRadio.Text = "Bitrate";
            this.audio2BitrateRadio.CheckedChanged += new System.EventHandler(this.audio2Radio_CheckedChanged);
            // 
            // audio2Bitrate
            // 
            this.audio2Bitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audio2Bitrate.Location = new System.Drawing.Point(16, 40);
            this.audio2Bitrate.Name = "audio2Bitrate";
            this.audio2Bitrate.Size = new System.Drawing.Size(72, 21);
            this.audio2Bitrate.TabIndex = 5;
            this.audio2Bitrate.SelectedIndexChanged += new System.EventHandler(this.audioBitrate_SelectedIndexChanged);
            // 
            // codecGroupbox
            // 
            this.codecGroupbox.Controls.Add(this.xvid);
            this.codecGroupbox.Controls.Add(this.x264);
            this.codecGroupbox.Controls.Add(this.snow);
            this.codecGroupbox.Controls.Add(this.lavc);
            this.codecGroupbox.Location = new System.Drawing.Point(336, 8);
            this.codecGroupbox.Name = "codecGroupbox";
            this.codecGroupbox.Size = new System.Drawing.Size(232, 48);
            this.codecGroupbox.TabIndex = 4;
            this.codecGroupbox.TabStop = false;
            this.codecGroupbox.Text = "Codec";
            // 
            // xvid
            // 
            this.xvid.Location = new System.Drawing.Point(176, 16);
            this.xvid.Name = "xvid";
            this.xvid.Size = new System.Drawing.Size(48, 24);
            this.xvid.TabIndex = 3;
            this.xvid.Text = "XviD";
            this.xvid.CheckedChanged += new System.EventHandler(this.codec_CheckedChanged);
            // 
            // x264
            // 
            this.x264.Checked = true;
            this.x264.Location = new System.Drawing.Point(120, 16);
            this.x264.Name = "x264";
            this.x264.Size = new System.Drawing.Size(56, 24);
            this.x264.TabIndex = 2;
            this.x264.TabStop = true;
            this.x264.Text = "x264";
            this.x264.CheckedChanged += new System.EventHandler(this.codec_CheckedChanged);
            // 
            // snow
            // 
            this.snow.Location = new System.Drawing.Point(64, 16);
            this.snow.Name = "snow";
            this.snow.Size = new System.Drawing.Size(56, 24);
            this.snow.TabIndex = 1;
            this.snow.Text = "Snow";
            this.snow.CheckedChanged += new System.EventHandler(this.codec_CheckedChanged);
            // 
            // lavc
            // 
            this.lavc.Location = new System.Drawing.Point(8, 16);
            this.lavc.Name = "lavc";
            this.lavc.Size = new System.Drawing.Size(48, 24);
            this.lavc.TabIndex = 0;
            this.lavc.Text = "lavc";
            this.lavc.CheckedChanged += new System.EventHandler(this.codec_CheckedChanged);
            // 
            // containerGroupbox
            // 
            this.containerGroupbox.Controls.Add(this.mp4);
            this.containerGroupbox.Controls.Add(this.avi);
            this.containerGroupbox.Controls.Add(this.mkv);
            this.containerGroupbox.Location = new System.Drawing.Point(336, 58);
            this.containerGroupbox.Name = "containerGroupbox";
            this.containerGroupbox.Size = new System.Drawing.Size(232, 48);
            this.containerGroupbox.TabIndex = 5;
            this.containerGroupbox.TabStop = false;
            this.containerGroupbox.Text = "Container";
            // 
            // mp4
            // 
            this.mp4.Checked = true;
            this.mp4.Location = new System.Drawing.Point(64, 16);
            this.mp4.Name = "mp4";
            this.mp4.Size = new System.Drawing.Size(48, 24);
            this.mp4.TabIndex = 1;
            this.mp4.TabStop = true;
            this.mp4.Text = "MP4";
            this.mp4.CheckedChanged += new System.EventHandler(this.container_CheckedChanged);
            // 
            // avi
            // 
            this.avi.Location = new System.Drawing.Point(8, 16);
            this.avi.Name = "avi";
            this.avi.Size = new System.Drawing.Size(48, 24);
            this.avi.TabIndex = 0;
            this.avi.Text = "AVI";
            this.avi.CheckedChanged += new System.EventHandler(this.container_CheckedChanged);
            // 
            // mkv
            // 
            this.mkv.Location = new System.Drawing.Point(128, 16);
            this.mkv.Name = "mkv";
            this.mkv.Size = new System.Drawing.Size(48, 24);
            this.mkv.TabIndex = 1;
            this.mkv.Text = "MKV";
            this.mkv.CheckedChanged += new System.EventHandler(this.container_CheckedChanged);
            // 
            // sizeGroupbox
            // 
            this.sizeGroupbox.Controls.Add(this.muxedSizeMBLabel);
            this.sizeGroupbox.Controls.Add(this.muxedSizeMB);
            this.sizeGroupbox.Controls.Add(this.sizeSelection);
            this.sizeGroupbox.Controls.Add(this.storageMediumLabel);
            this.sizeGroupbox.Controls.Add(this.fileSizeRadio);
            this.sizeGroupbox.Controls.Add(this.muxedSizeKBLabel);
            this.sizeGroupbox.Controls.Add(this.muxedSizeKB);
            this.sizeGroupbox.Controls.Add(this.filesizeScrollbar);
            this.sizeGroupbox.Location = new System.Drawing.Point(336, 112);
            this.sizeGroupbox.Name = "sizeGroupbox";
            this.sizeGroupbox.Size = new System.Drawing.Size(232, 120);
            this.sizeGroupbox.TabIndex = 6;
            this.sizeGroupbox.TabStop = false;
            this.sizeGroupbox.Text = "Total Size";
            // 
            // muxedSizeMBLabel
            // 
            this.muxedSizeMBLabel.Location = new System.Drawing.Point(200, 48);
            this.muxedSizeMBLabel.Name = "muxedSizeMBLabel";
            this.muxedSizeMBLabel.Size = new System.Drawing.Size(24, 16);
            this.muxedSizeMBLabel.TabIndex = 35;
            this.muxedSizeMBLabel.Text = "MB";
            // 
            // muxedSizeMB
            // 
            this.muxedSizeMB.Location = new System.Drawing.Point(120, 48);
            this.muxedSizeMB.MaxLength = 4;
            this.muxedSizeMB.Name = "muxedSizeMB";
            this.muxedSizeMB.Size = new System.Drawing.Size(64, 21);
            this.muxedSizeMB.TabIndex = 34;
            this.muxedSizeMB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.muxedSizeMB.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // sizeSelection
            // 
            this.sizeSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeSelection.Location = new System.Drawing.Point(120, 88);
            this.sizeSelection.Name = "sizeSelection";
            this.sizeSelection.Size = new System.Drawing.Size(88, 21);
            this.sizeSelection.TabIndex = 18;
            this.sizeSelection.SelectedIndexChanged += new System.EventHandler(this.sizeSelection_SelectedIndexChanged);
            // 
            // storageMediumLabel
            // 
            this.storageMediumLabel.Location = new System.Drawing.Point(16, 91);
            this.storageMediumLabel.Name = "storageMediumLabel";
            this.storageMediumLabel.Size = new System.Drawing.Size(90, 13);
            this.storageMediumLabel.TabIndex = 19;
            this.storageMediumLabel.Text = "Storage Medium";
            // 
            // fileSizeRadio
            // 
            this.fileSizeRadio.Checked = true;
            this.fileSizeRadio.Location = new System.Drawing.Point(8, 24);
            this.fileSizeRadio.Name = "fileSizeRadio";
            this.fileSizeRadio.Size = new System.Drawing.Size(100, 20);
            this.fileSizeRadio.TabIndex = 26;
            this.fileSizeRadio.TabStop = true;
            this.fileSizeRadio.Text = "File Size";
            this.fileSizeRadio.CheckedChanged += new System.EventHandler(this.calculationMode_CheckedChanged);
            // 
            // muxedSizeKBLabel
            // 
            this.muxedSizeKBLabel.Location = new System.Drawing.Point(192, 26);
            this.muxedSizeKBLabel.Name = "muxedSizeKBLabel";
            this.muxedSizeKBLabel.Size = new System.Drawing.Size(24, 14);
            this.muxedSizeKBLabel.TabIndex = 23;
            this.muxedSizeKBLabel.Text = "KB";
            // 
            // muxedSizeKB
            // 
            this.muxedSizeKB.Location = new System.Drawing.Point(120, 24);
            this.muxedSizeKB.MaxLength = 7;
            this.muxedSizeKB.Name = "muxedSizeKB";
            this.muxedSizeKB.Size = new System.Drawing.Size(64, 21);
            this.muxedSizeKB.TabIndex = 22;
            this.muxedSizeKB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.muxedSizeKB.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // filesizeScrollbar
            // 
            this.filesizeScrollbar.LargeChange = 1;
            this.filesizeScrollbar.Location = new System.Drawing.Point(184, 48);
            this.filesizeScrollbar.Maximum = 9999;
            this.filesizeScrollbar.Name = "filesizeScrollbar";
            this.filesizeScrollbar.Size = new System.Drawing.Size(14, 18);
            this.filesizeScrollbar.TabIndex = 34;
            this.filesizeScrollbar.ValueChanged += new System.EventHandler(this.filesizeScrollbar_ValueChanged);
            // 
            // videoSizeKBLabel
            // 
            this.videoSizeKBLabel.Location = new System.Drawing.Point(184, 48);
            this.videoSizeKBLabel.Name = "videoSizeKBLabel";
            this.videoSizeKBLabel.Size = new System.Drawing.Size(24, 13);
            this.videoSizeKBLabel.TabIndex = 31;
            this.videoSizeKBLabel.Text = "KB";
            // 
            // videoSizeKB
            // 
            this.videoSizeKB.Enabled = false;
            this.videoSizeKB.Location = new System.Drawing.Point(112, 48);
            this.videoSizeKB.MaxLength = 7;
            this.videoSizeKB.Name = "videoSizeKB";
            this.videoSizeKB.Size = new System.Drawing.Size(64, 21);
            this.videoSizeKB.TabIndex = 30;
            this.videoSizeKB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.videoSizeKB.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // VideoFileSizeLabel
            // 
            this.VideoFileSizeLabel.Location = new System.Drawing.Point(8, 51);
            this.VideoFileSizeLabel.Name = "VideoFileSizeLabel";
            this.VideoFileSizeLabel.Size = new System.Drawing.Size(82, 13);
            this.VideoFileSizeLabel.TabIndex = 29;
            this.VideoFileSizeLabel.Text = "Video File Size";
            // 
            // averageBitrateRadio
            // 
            this.averageBitrateRadio.Location = new System.Drawing.Point(8, 25);
            this.averageBitrateRadio.Name = "averageBitrateRadio";
            this.averageBitrateRadio.Size = new System.Drawing.Size(100, 20);
            this.averageBitrateRadio.TabIndex = 27;
            this.averageBitrateRadio.Text = "Average Bitrate";
            this.averageBitrateRadio.CheckedChanged += new System.EventHandler(this.calculationMode_CheckedChanged);
            // 
            // projectedBitrate
            // 
            this.projectedBitrate.Enabled = false;
            this.projectedBitrate.Location = new System.Drawing.Point(112, 24);
            this.projectedBitrate.MaxLength = 5;
            this.projectedBitrate.Name = "projectedBitrate";
            this.projectedBitrate.Size = new System.Drawing.Size(64, 21);
            this.projectedBitrate.TabIndex = 24;
            this.projectedBitrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.projectedBitrate.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // AverageBitrateLabel
            // 
            this.AverageBitrateLabel.Location = new System.Drawing.Point(176, 24);
            this.AverageBitrateLabel.Name = "AverageBitrateLabel";
            this.AverageBitrateLabel.Size = new System.Drawing.Size(32, 23);
            this.AverageBitrateLabel.TabIndex = 25;
            this.AverageBitrateLabel.Text = "kbit/s";
            // 
            // resultGroupbox
            // 
            this.resultGroupbox.Controls.Add(this.videoSizeMBLabel);
            this.resultGroupbox.Controls.Add(this.videoSizeMB);
            this.resultGroupbox.Controls.Add(this.projectedBitrate);
            this.resultGroupbox.Controls.Add(this.AverageBitrateLabel);
            this.resultGroupbox.Controls.Add(this.VideoFileSizeLabel);
            this.resultGroupbox.Controls.Add(this.videoSizeKBLabel);
            this.resultGroupbox.Controls.Add(this.videoSizeKB);
            this.resultGroupbox.Controls.Add(this.averageBitrateRadio);
            this.resultGroupbox.Location = new System.Drawing.Point(336, 240);
            this.resultGroupbox.Name = "resultGroupbox";
            this.resultGroupbox.Size = new System.Drawing.Size(232, 100);
            this.resultGroupbox.TabIndex = 32;
            this.resultGroupbox.TabStop = false;
            this.resultGroupbox.Text = "Results";
            // 
            // videoSizeMBLabel
            // 
            this.videoSizeMBLabel.Location = new System.Drawing.Point(184, 72);
            this.videoSizeMBLabel.Name = "videoSizeMBLabel";
            this.videoSizeMBLabel.Size = new System.Drawing.Size(24, 16);
            this.videoSizeMBLabel.TabIndex = 33;
            this.videoSizeMBLabel.Text = "MB";
            // 
            // videoSizeMB
            // 
            this.videoSizeMB.Enabled = false;
            this.videoSizeMB.Location = new System.Drawing.Point(112, 72);
            this.videoSizeMB.MaxLength = 4;
            this.videoSizeMB.Name = "videoSizeMB";
            this.videoSizeMB.Size = new System.Drawing.Size(64, 21);
            this.videoSizeMB.TabIndex = 32;
            this.videoSizeMB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.videoSizeMB.TextChanged += new System.EventHandler(this.textField_TextChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(448, 352);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 33;
            this.cancelButton.Text = "Cancel";
            // 
            // Calculator
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(576, 386);
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
            this.videoGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hours)).EndInit();
            this.audio1Groupbox.ResumeLayout(false);
            this.audio1Groupbox.PerformLayout();
            this.audio2Groupbox.ResumeLayout(false);
            this.audio2Groupbox.PerformLayout();
            this.codecGroupbox.ResumeLayout(false);
            this.containerGroupbox.ResumeLayout(false);
            this.sizeGroupbox.ResumeLayout(false);
            this.sizeGroupbox.PerformLayout();
            this.resultGroupbox.ResumeLayout(false);
            this.resultGroupbox.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
		#region data setters and getters
		/// <summary>
		/// sets video, audio and codec defaults
		/// </summary>
		/// <param name="nbFrames">number of frames of the video source</param>
		/// <param name="framerate">framerate of the video source</param>
		/// <param name="codec">codec selected</param>
		/// <param name="container">container</param>
		/// <param name="audio1Bitrate">bitrate of the first audio track</param>
		/// <param name="audio2Bitrate">bitrate of the second audio track</param>
		public void setDefaults(int nbFrames, double framerate, int codec, int container, int audio1Bitrate, 
			int audio2Bitrate)
		{
            int index = this.framerate.Items.IndexOf((double)Math.Round(framerate, 3));
			if (index != -1)
				this.framerate.SelectedIndex = index;
			if (nbFrames > 0)
				this.nbFrames.Text = nbFrames.ToString();
			switch (codec)
			{
				case 0: // lavc
					this.lavc.Checked = true;
					break;
				case 1:
					this.x264.Checked = true;
					break;
				case 2:
					this.snow.Checked = true;
					break;
				case 3:
					this.xvid.Checked = true;
					break;
			}
			if (container == 0)
				this.avi.Checked = true;
			else if (container == 2)
			{
				this.aud1Type = AudioType.MP4AAC;
				this.aud2Type = AudioType.MP4AAC;
				this.mkv.Checked = true;
			}
			else if (container == 3)
				this.mp4.Checked = true;

			index = this.audio1Bitrate.Items.IndexOf(audio1Bitrate);
			if (index != -1)
				this.audio1Bitrate.SelectedIndex = index;
			else 
				this.audio1Bitrate.SelectedIndex = 0;
			index = this.audio2Bitrate.Items.IndexOf(audio2Bitrate);
			if (index != -1)
				this.audio2Bitrate.SelectedIndex = index;
			else
				this.audio2Bitrate.SelectedIndex = 0;
		}
		/// <summary>
		/// gets the selected codec
		/// </summary>
		/// <returns></returns>
		public int getSelectedCodec()
		{
			if (lavc.Checked)
				return 0;
			else if (x264.Checked)
				return 1;
			else if (snow.Checked)
				return 2;
			else
				return 3;
		}
		/// <summary>
		/// gets the selected container
		/// </summary>
		/// <returns></returns>
		public int getSelectedContainer()
		{
			if (mp4.Checked)
				return 3;
			else if  (avi.Checked)
				return 0;
			else if (mkv.Checked)
				return 2;
			else
				return 1;
		}
		/// <summary>
		/// gets the calculated bitrate
		/// </summary>
		/// <returns></returns>
		public int getBitrate()
		{
			if (!projectedBitrate.Text.Equals(""))
			{
				return Int32.Parse(projectedBitrate.Text);
			}
			return 0;
		}
		#endregion
		#region audio
		#region audio1
		//// <summary>
		/// handles radiobutton switches for the second audio track
		/// if bitrate is selected, the entry for size is disabled, and the bitrate entry
		/// is disabled otherwise
		/// if the audio size is larger than 0 and no audio type has been defined yet
		/// one is assigned automatically
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void audio1Radio_CheckedChanged(object sender, System.EventArgs e)
		{
			if (audio1BitrateRadio.Checked)
			{
				audio1Bitrate.Enabled = true;
				audio1Bitrate.SelectedIndex = 0;
				audio1SizeKB.Enabled = false;
				audio1SizeMB.Enabled = false;
			}
			else if (audio1SizeRadio.Checked)
			{
				audio1Bitrate.Enabled = false;
				audio1Bitrate.SelectedIndex = -1;
				audio1SizeKB.Enabled = true;
				audio1SizeMB.Enabled = true;
				int size = 0;
				try
				{
					size = Int32.Parse(audio1SizeKB.Text);
				}
				catch (Exception){}
				if (size > 0 && aud1Type == null)
					aud1Type = getAudioTypeFromTrackNumber(1);
			}
		}
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
			if (avi.Checked)
				openFileDialog.Filter = "Audio Files (*.ac3, *.mp3)|*.ac3;*.mp3";
			else if (mp4.Checked)
				openFileDialog.Filter = "Audio Files (*.aac, *.mp4)|*.aac;*.mp4";
			else if (mkv.Checked)
				openFileDialog.Filter = "Audio Files (*.aac, *.mp4, *.ac3, *.mp3, *.ogg)|*.ogg;*.ac3;*.mp3;*.aac;*.mp4";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				if (!audio1SizeRadio.Checked)
					audio1SizeRadio.Checked = true;
				FileInfo fi = new FileInfo(openFileDialog.FileName);
				int sizeKB = (int)(fi.Length / 1024);
				int sizeMB = sizeKB/1024;
				aud1Type = this.getAudioType(openFileDialog.FileName);
				audio1SizeKB.Text = sizeKB.ToString();
				audio1SizeMB.Text = sizeMB.ToString();
				if (mkv.Checked)
				{
					if (!audio1Type.Enabled)
						audio1Type.Enabled = true;
			        if (aud1Type == AudioType.MP4AAC)
							audio1Type.SelectedIndex = 0;
                    else if (aud1Type == AudioType.AC3)
							audio1Type.SelectedIndex = 1;
                    else if (aud1Type == AudioType.VBRMP3)
							audio1Type.SelectedIndex = 2;
                    else if (aud1Type == AudioType.VORBIS)
							audio1Type.SelectedIndex = 3;
				}
				else
				{
					if (aud1Type == AudioType.VBRMP3)
					{
						if (avi.Checked)
						{
							audio1Type.Enabled = true;
							audio1Type.SelectedIndex = 1;
						}
					}
					else
					{
						audio1Type.Enabled = false;
						audio1Type.SelectedIndex = -1;
					}
				}
			}
		}
		/// <summary>
		/// handles changes of the audio type dropdown for the first audio track
		/// assigns the proper audio type depending on which item was selected from the dropdown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void audio1Type_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (audio1Type.SelectedIndex != -1)
			{
				if (avi.Checked)
				{
					if (audio1Type.SelectedIndex == 0)
						aud1Type = AudioType.CBRMP3;
					else if (audio1Type.SelectedIndex == 1)
						aud1Type = AudioType.VBRMP3;
				}
				else if (mkv.Checked)
				{
					if (audio1Type.SelectedIndex == 0)
						aud1Type = AudioType.MP4AAC;
					else if (audio1Type.SelectedIndex == 1)
						aud1Type = AudioType.AC3;
					else if (audio1Type.SelectedIndex == 2)
						aud1Type = AudioType.VBRMP3;
					else
						aud1Type = AudioType.VORBIS;
				}
				updateBitrateSize();
			}
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
			ComboBox cb = (ComboBox)sender;
			if (cb.SelectedIndex >= 0)
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
			int length = 0;
			if (!totalSeconds.Text.Equals(""))
				length = Int32.Parse(totalSeconds.Text);
			if (track1)
			{
				int bitrate = (int)audio1Bitrate.Items[audio1Bitrate.SelectedIndex];
				double bytesPerSecond = bitrate * 1000 / 8;
				long sizeInBytes = (long)(length * bytesPerSecond);
				long size = sizeInBytes / (long)1024;
				audio1SizeKB.Text = size.ToString();
				size = size / 1024;
				audio1SizeMB.Text = size.ToString();
				if (bitrate > 0)
					aud1Type = getAudioTypeFromTrackNumber(1);
			}
			if (track2)
			{
				int bitrate = (int)audio2Bitrate.Items[audio2Bitrate.SelectedIndex];
				double bytesPerSecond = bitrate * 1000 / 8;
				long sizeInBytes = (long)(length * bytesPerSecond);
				long size = sizeInBytes / (long)1024;
				audio2SizeKB.Text = size.ToString();
				size = size / 1024;
				audio2SizeMB.Text = size.ToString();
				if (bitrate > 0)
					aud2Type = getAudioTypeFromTrackNumber(2);
			}
		}
		/// <summary>
		/// determines the audio track type of the given audio track
		/// if the type is defined, the defined type is returned
		/// if no type is defined, the default is assigned in function of the selected codec
		/// </summary>
		/// <param name="trackNumber">the audio track in question</param>
		/// <returns>the type of the audio</returns>
		private AudioType getAudioTypeFromTrackNumber(int trackNumber)
		{
			AudioType retval = null;
			bool doDetermination = false;
			if (trackNumber == 1)
			{
				if (aud1Type == null)
					doDetermination = true;
				else
					retval = aud1Type;
			}
			if (trackNumber == 2)
			{
				if (aud2Type == null)
					doDetermination = true;
				else
					retval = aud2Type;
			}
			if (doDetermination)
			{
				if (avi.Checked)
					retval = AudioType.VBRMP3;
				else if (mp4.Checked)
					retval = AudioType.MP4AAC;
				else if (mkv.Checked)
					retval = AudioType.VBRMP3;
			}
			return retval;
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
				aud1Type = null;
				audio1BitrateRadio.Checked = true;
				audio1Type.SelectedIndex = -1;
			}
			else if (button == this.clearAudio2Button)
			{
				aud2Type = null;
				audio2BitrateRadio.Checked = true;
				audio2Type.SelectedIndex = -1;
			}
			this.updateBitrateSize();
		}
		#endregion
		#region audio 2
		/// <summary>
		/// handles radiobutton switches for the second audio track
		/// if bitrate is selected, the entry for size is disabled, and the bitrate entry
		/// is disabled otherwise
		/// if the audio size is larger than 0 and no audio type has been defined yet
		/// one is assigned automatically
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void audio2Radio_CheckedChanged(object sender, System.EventArgs e)
		{
			if (audio2BitrateRadio.Checked)
			{
				audio2Bitrate.Enabled = true;
				audio2SizeKB.Enabled = false;
				audio2SizeMB.Enabled = false;
				audio2Bitrate.SelectedIndex = 0;
			}
			else if (audio2SizeRadio.Checked)
			{
				audio2Bitrate.Enabled = false;
				audio2Bitrate.SelectedIndex = -1;
				audio2SizeKB.Enabled = true;
				audio2SizeMB.Enabled = true;
				int size = 0;
				try
				{
					size = Int32.Parse(audio2SizeKB.Text);
				}
				catch (Exception){}
				if (size > 0 && aud2Type == null)
					aud2Type = getAudioTypeFromTrackNumber(2);
			}
		}
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
			if (avi.Checked)
				openFileDialog.Filter = "Audio Files (*.ac3, *.mp3)|*.ac3;*.mp3";
			else if (mp4.Checked)
				openFileDialog.Filter = "Audio Files (*.aac, *.mp4)|*.aac;*.mp4";
			else if (mkv.Checked)
				openFileDialog.Filter = "Audio Files (*.aac, *.mp4, *.ac3, *.mp3)|*.ac3;*.mp3;*.aac;*.mp4";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				if (!audio2SizeRadio.Checked)
					audio2SizeRadio.Checked = true;
				FileInfo fi = new FileInfo(openFileDialog.FileName);
				int sizeKB = (int)(fi.Length / 1024);
				int sizeMB = sizeKB/1024;
				aud2Type = this.getAudioType(openFileDialog.FileName);
				audio2SizeKB.Text = sizeKB.ToString();
				audio2SizeMB.Text = sizeMB.ToString();
                if (mkv.Checked)
                {
                    if (!audio2Type.Enabled)
                        audio2Type.Enabled = true;
                    if (aud2Type == AudioType.MP4AAC)
                        audio2Type.SelectedIndex = 0;
                    if (aud2Type == AudioType.AC3)
                        audio2Type.SelectedIndex = 1;
                    if (aud2Type == AudioType.VBRMP3)
                        audio2Type.SelectedIndex = 2;
                    if (aud2Type == AudioType.VORBIS)
                        audio2Type.SelectedIndex = 3;
                }
                else
                {
                    if (aud2Type == AudioType.VBRMP3)
                    {
                        audio2Type.Enabled = true;
                        audio2Type.SelectedIndex = 1;
                    }
                    else
                    {
                        audio2Type.Enabled = false;
                        audio2Type.SelectedIndex = -1;
                    }
                }
			}
		}
		/// <summary>
		/// handles changes of the audio type dropdown for the second audio track
		/// assigns the proper audio type depending on which item was selected from the dropdown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void audio2Type_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (audio2Type.SelectedIndex != -1)
			{
				if (avi.Checked)
				{
					if (audio2Type.SelectedIndex == 0)
						aud2Type = AudioType.CBRMP3;
					else if (audio2Type.SelectedIndex == 1)
						aud2Type = AudioType.VBRMP3;
				}
				else if (mkv.Checked)
				{
					if (audio2Type.SelectedIndex == 0)
						aud2Type = AudioType.MP4AAC;
					else if (audio2Type.SelectedIndex == 1)
						aud2Type = AudioType.AC3;
					else if (audio2Type.SelectedIndex == 2)
						aud2Type = AudioType.VBRMP3;
					else
						aud2Type = AudioType.VORBIS;
				}
				updateBitrateSize();
			}
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
                if(sender is TextBox && !this.isUpdating)
                {
                    this.isUpdating = true;
                    TextBox tb = (TextBox)sender;
                    if(tb.Text.Equals("")) // unparsable, abort
                    {
                        this.isUpdating = false;
                        return;
                    }
                    int size = Int32.Parse(tb.Text);
                    int newVal;
                    if(tb == muxedSizeKB)
                    {
                        if(!averageBitrateRadio.Checked)
                        {
                            newVal = size / 1024;
                            muxedSizeMB.Text = newVal.ToString();
                        }
                    }
                    else if(tb == audio1SizeKB)
                    {
                        newVal = size / 1024;
                        audio1SizeMB.Text = newVal.ToString();
                    }
                    else if(tb == audio1SizeMB)
                    {
                        newVal = size * 1024;
                        audio1SizeKB.Text = newVal.ToString();
                    }
                    else if(tb == audio2SizeKB)
                    {
                        newVal = size / 1024; ;
                        audio2SizeMB.Text = newVal.ToString();
                    }
                    else if(tb == audio2SizeMB)
                    {
                        newVal = size * 1024;
                        audio2SizeKB.Text = newVal.ToString();
                    }
                    else if(tb == muxedSizeMB)
                    {
                        if(!averageBitrateRadio.Checked)
                        {
                            newVal = size * 1024;
                            muxedSizeKB.Text = newVal.ToString();
                        }
                    }
                    else if(tb == videoSizeKB)
                    {
                        if(!averageBitrateRadio.Checked)
                        {
                            newVal = size / 1024;
                            videoSizeMB.Text = newVal.ToString();
                        }
                    }
                    else if(tb == videoSizeMB)
                    {
                        if(!averageBitrateRadio.Checked)
                        {
                            newVal = size * 1024;
                            videoSizeKB.Text = newVal.ToString();
                        }
                    }
                    else if(tb == totalSeconds)
                    {
                        int hours = size / 3600;
                        size -= hours * 3600;
                        int minutes = size / 60;
                        size -= minutes * 60;
                        if(hours <= this.hours.Maximum)
                        {
                            this.hours.Value = hours;
                            this.minutes.Value = minutes;
                            this.seconds.Value = size;
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
                    else if(tb == nbFrames)
                    {
                        if(framerate.SelectedIndex >= 0)
                        {
                            int secs = (int)((double)size / (double)framerate.Items[framerate.SelectedIndex]);
                            totalSeconds.Text = secs.ToString();
                            int hours = secs / 3600;
                            secs -= hours * 3600;
                            int minutes = secs / 60;
                            secs -= minutes * 60;
                            if(hours < this.hours.Maximum)
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
                    }
                    else if(tb == projectedBitrate)
                    {
                        if(averageBitrateRadio.Checked) // only do something here if we're in size calculation mode
                            updateSize();
                    }
                    tb.Select(tb.Text.Length, 0);
                    if((averageBitrateRadio.Checked && tb != videoSizeMB
                        && tb != videoSizeKB
                        && tb != muxedSizeMB
                        && tb != muxedSizeKB
                        && tb != projectedBitrate)
                        || !averageBitrateRadio.Checked)
                        updateBitrateSize();
                    
                    this.isUpdating = false;
                }
            }
		}
		#endregion
		#region helper methods
		/// <summary>
		/// gets the audio type from a filename based on the extension
		/// </summary>
		/// <param name="fileName">the file name to be checked</param>
		/// <returns>the audio type or none if it cannot be identified</returns>
		private AudioType getAudioType(string fileName)
		{
#warning look here
			string extension = Path.GetExtension(fileName).ToLower();
            if (extension.Equals(".mp3"))
                return AudioType.VBRMP3;
            else if (extension.Equals(".aac") || extension.Equals(".mp4"))
                return AudioType.MP4AAC;
            else if (extension.Equals(".ac3"))
                return AudioType.AC3;
            else if (extension.Equals(".ogg"))
                return AudioType.VORBIS;
            else
                return null;
		}
		#endregion
		#region updown controls
		private void time_ValueChanged(object sender, System.EventArgs e)
		{
			if (!this.isUpdating)
			{
				this.isUpdating = true;
				NumericUpDown ud = (NumericUpDown)sender;
                if (this.hours.Value.Equals(this.hours.Maximum))
                {
                    if (this.minutes.Value == 60)
                    {
                        this.minutes.Value = 59;
                        UpdateTotalSeconds();
                        UpdateTotalFrames();
                        this.isUpdating = false;
                        return; // we can't increase the time
                    }
                    else if (this.seconds.Value == 60 && this.minutes.Value == 59)
                    {
                        this.seconds.Value = 59;
                        UpdateTotalSeconds();
                        UpdateTotalFrames();
                        this.isUpdating = false;
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
                            if(!this.hours.Value.Equals(this.hours.Maximum))
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
				if (this.framerate.SelectedIndex >= 0)
				{
                    UpdateTotalFrames();
					updateBitrateSize();
				}
				
                this.isUpdating = false;
			}
		}

        private void UpdateTotalFrames()
        {
            int secs = (int)this.hours.Value * 3600 + (int)this.minutes.Value * 60 + (int)this.seconds.Value;
            double fps = (double)framerate.Items[framerate.SelectedIndex];
            int frameNumber = (int)((double)secs * fps);
            nbFrames.Text = frameNumber.ToString();
        }
        private void UpdateTotalSeconds()
        {
            int secs = (int)this.hours.Value * 3600 + (int)this.minutes.Value * 60 + (int)this.seconds.Value;
            totalSeconds.Text = secs.ToString();
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
				if (rb == averageBitrateRadio) // switch from size to bitrate mode
				{
					muxedSizeKB.Enabled = false;
					muxedSizeMB.Enabled = false;
					filesizeScrollbar.Enabled = false;
					projectedBitrate.Enabled = true;
					sizeSelection.Enabled = true;
					fileSizeRadio.Checked = false;
					sizeSelection.Enabled = false;
				}
				else if (rb.Checked)
				{
					muxedSizeKB.Enabled = true;
					muxedSizeMB.Enabled = true;
					filesizeScrollbar.Enabled = true;
					projectedBitrate.Enabled = false;
					sizeSelection.Enabled = false;
					averageBitrateRadio.Checked = false;
					sizeSelection.Enabled = true;
				}
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
			RadioButton rb = (RadioButton)sender;
			if (rb == snow)
			{
				avi.Checked = true;
				mp4.Enabled = false;
			}
			else if (!mp4.Enabled)
				mp4.Enabled = true;
            updateBitrateSize();
		}
		/// <summary>
		/// handles change of the container
		/// in case audio is set to "by filetype", the audio type is checked and if a type
		/// that doesn't match the selected container, the audio will be zeroed out
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void container_CheckedChanged(object sender, System.EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			if (rb.Checked)
			{
				if (avi.Checked)
				{
					this.bframes.Checked = false;
					this.bframes.Enabled = false;
					audio1Type.Enabled = true;
					audio1Type.Items.Clear();
					audio1Type.Items.AddRange(new object[] {"CBR", "VBR"});
					audio1Type.SelectedIndex = -1;
					if (aud1Type == AudioType.MP4AAC || aud1Type == AudioType.VORBIS)
					{
						if (audio1SizeRadio.Checked)
						{
							aud1Type = null;
							audio1SizeKB.Text = "0";
						}
						else // selection by bitrate
						{
							if (audio1Bitrate.SelectedIndex != 0) // a bitrate was chosen
							{
								aud1Type = AudioType.VBRMP3;
								audio1Type.SelectedIndex = 1;
							}
							else // bitrate = 0
								aud1Type = null;
						}
					}
					else // compatible audio type
					{
						if (aud1Type == AudioType.AC3)
							audio1Type.SelectedIndex = 0; // select cbr
						else if (aud1Type == AudioType.VBRMP3)
							audio1Type.SelectedIndex = 1; // select vbr
					}
					audio2Type.Enabled = true;
					audio2Type.Items.Clear();
					audio2Type.Items.AddRange(new object[] {"CBR", "VBR"});
					audio2Type.SelectedIndex = -1;
					if (aud2Type == AudioType.MP4AAC || aud2Type == AudioType.VORBIS)
					{
						if (audio2SizeRadio.Checked)
						{
							aud2Type = null;
							audio2SizeKB.Text = "0";
						}
						else
						{
							if (audio2Bitrate.SelectedIndex != 0) // a bitrate was chosen
							{
								aud2Type = AudioType.VBRMP3;
								audio2Type.SelectedIndex = 1;
							}
							else // bitrate = 0
								aud2Type = null;
						}
					}
					else // compatible audio type
					{
						if (aud2Type == AudioType.AC3)
							audio2Type.SelectedIndex = 0; // select cbr
						else if (aud2Type == AudioType.VBRMP3) // select vbr
							audio2Type.SelectedIndex = 1;
					}
				}
				else if (mp4.Checked)
				{
					this.bframes.Enabled = true;
					this.bframes.Checked = true;
					audio1Type.Enabled = false;
					audio1Type.SelectedIndex = -1;
					audio2Type.Enabled = false;
					audio2Type.SelectedIndex = -1;
					if (aud1Type != AudioType.MP4AAC) // mp4 requires aac audio
					{
						if (audio1SizeRadio.Checked)
						{
							aud1Type = null;
							audio1SizeKB.Text = "0";
						}
						else
						{
							if (audio1Bitrate.SelectedIndex != 0) // a bitrate was chosen
								aud1Type = AudioType.MP4AAC;
							else // bitrate = 0
								aud1Type = null;
						}
					}
					if (aud2Type != AudioType.MP4AAC)
					{
						if (audio2SizeRadio.Checked)
						{
							aud2Type = null;
							audio2SizeKB.Text = "0";
						}
						else
						{
							if (audio2Bitrate.SelectedIndex != 0) // a bitrate was chosen
								aud2Type = AudioType.MP4AAC;
							else // bitrate = 0
								aud2Type = null;
						}
					}
				}
                else if (mkv.Checked)
                {
                    this.bframes.Enabled = true;
                    this.bframes.Checked = true;
                    audio1Type.Enabled = true;
                    audio1Type.Items.Clear();
                    audio1Type.Items.AddRange(new object[] { "AAC", "AC3", "MP3", "Vorbis" });
                    if (aud1Type == AudioType.MP4AAC)
                        audio1Type.SelectedIndex = 0;
                    else if (aud1Type == AudioType.AC3)
                        audio1Type.SelectedIndex = 1;
                    else if (aud1Type == AudioType.VBRMP3)
                        audio1Type.SelectedIndex = 2;
                    else if (aud1Type == AudioType.VORBIS)
                        audio1Type.SelectedIndex = 3;
                    audio2Type.Enabled = true;
                    audio2Type.Items.Clear();
                    audio2Type.Items.AddRange(new object[] { "AAC", "AC3", "MP3", "Vorbis" });
                    if (aud2Type == AudioType.MP4AAC)
                        audio2Type.SelectedIndex = 0;
                    else if (aud2Type == AudioType.AC3)
                        audio2Type.SelectedIndex = 1;
                    else if (aud2Type == AudioType.VBRMP3)
                        audio2Type.SelectedIndex = 2;
                    else if (aud2Type == AudioType.VORBIS)
                        audio2Type.SelectedIndex = 3;
                }
				updateBitrateSize();
			}
		}
		#endregion
		#region size
		private void sizeSelection_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			internalUpdate = true;
			int sizeKB = calc.getOutputSize(sizeSelection.SelectedIndex);
			muxedSizeKB.Text = sizeKB.ToString();
			int size = sizeKB / 1024;
	
			filesizeScrollbar.Value = filesizeScrollbar.Maximum - size;
			updateBitrateSize();
		}
		private void filesizeScrollbar_ValueChanged(object sender, System.EventArgs e)
		{
			int sizeMB = 0;
			if (!muxedSizeMB.Text.Equals(""))
				sizeMB = Int32.Parse(muxedSizeMB.Text);
			int newValue = filesizeScrollbar.Value;
			if (!internalUpdate)
			{
				if (newValue < oldValue) // user pressed up
				{
					if (sizeMB < 9999)
					{
						sizeMB++;
						muxedSizeMB.Text = sizeMB.ToString();
					}
				}
				if (newValue > oldValue) // user pressed down
				{
					if (sizeMB > 1)
					{
						sizeMB--;
						muxedSizeMB.Text = sizeMB.ToString();
					}
				}
			}
			this.oldValue = filesizeScrollbar.Value;
			internalUpdate = false;
		}

		#endregion
		#region dropdowns
		/// <summary>
		/// handles the video framerate selection
		/// if the length of the video is defined, the number of frames are updated
		/// in function of the selected framerate
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void framerate_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.isUpdating = true;
			double framerate = (double)this.framerate.Items[this.framerate.SelectedIndex];
			int length = 0;
			if (!this.totalSeconds.Text.Equals(""))
				length = Int32.Parse(totalSeconds.Text);
			int numberOfFrames = (int)(length * framerate);
			nbFrames.Text = numberOfFrames.ToString();
			this.updateBitrateSize();
		}
		#endregion
		#region checkboxes
		private void bframes_CheckedChanged(object sender, System.EventArgs e)
		{
			if (bframes.Checked)
				this.containerOverhead = 10.4;
			else
				this.containerOverhead = 4.3;
			updateBitrateSize();
		}
		#endregion
		#region bitrate calculations
		private void updateSize()
		{
			if (!nbFrames.Text.Equals("") && !projectedBitrate.Text.Equals("") && !totalSeconds.Text.Equals(""))
			{
				int numberOfFrames = Int32.Parse(nbFrames.Text); 
				double framerate = (double)this.framerate.Items[this.framerate.SelectedIndex];
				int bitrate = Int32.Parse(projectedBitrate.Text);
				bool isXviD = false;
				long[] audioSizes = {Int64.Parse(audio1SizeKB.Text) * 1024, Int64.Parse(audio2SizeKB.Text) * 1024};
				isXviD = xvid.Checked;
				int vidSize = 0;
				int totalSize = 0;
				if (avi.Checked)
					totalSize = calc.calculateAVISize2(audioSizes[0], audioSizes[1], bitrate, numberOfFrames, framerate,
						isXviD, this.aud1Type, this.aud2Type, out vidSize);
				else if (mp4.Checked)
					totalSize = calc.calculateMP4Size(audioSizes[0] + audioSizes[1], bitrate, numberOfFrames, containerOverhead, framerate, 
						isXviD, out vidSize);
				else if (mkv.Checked)
				{
					int nbBframes = 0;
					if (this.bframes.Checked)
						nbBframes = 2;
					totalSize = calc.calculateMKVSize(audioSizes[0], audioSizes[1], bitrate, numberOfFrames, framerate, 
						nbBframes, isXviD, aud1Type, aud2Type, out vidSize);
				}
				int totalSizeMB = totalSize / 1024;
				int sizeMB = vidSize / 1024;
				muxedSizeKB.Text = totalSize.ToString();
				muxedSizeMB.Text = totalSizeMB.ToString();
				videoSizeKB.Text = vidSize.ToString();
				videoSizeMB.Text = sizeMB.ToString();
			}
		}
		private void updateBitrateSize()
		{
			if (!nbFrames.Text.Equals("") && !nbFrames.Text.Equals("0"))
			{
				if (fileSizeRadio.Checked)
					updateBitrate();
				else
					updateSize();
			}
			else
			{
				if (fileSizeRadio.Checked)
				{
					projectedBitrate.Text = "0";
					videoSizeKB.Text = "0";
					videoSizeMB.Text = "0";
				}
				else
				{
					muxedSizeKB.Text = "0";
					muxedSizeMB.Text = "0";
					videoSizeKB.Text = "0";
					videoSizeMB.Text = "0";
				}
			}
			this.isUpdating = false;
		}
		/// <summary>
		/// calculates the bitrate and shows the results of the calculation in the appropriate gui fields
		/// </summary>
		private void updateBitrate()
		{
			if (!nbFrames.Text.Equals("") && !muxedSizeKB.Text.Equals("") && !totalSeconds.Text.Equals("")) 
				// w/o those elements we can't calculate
			{
				int numberOfFrames = Int32.Parse(nbFrames.Text);
				long muxedSizeBytes = Int64.Parse(muxedSizeKB.Text) * 1024;
				double framerate = (double)this.framerate.Items[this.framerate.SelectedIndex];
				int bitrate = 0;
				bool isXviD = false;
				long[] audioSizes = {Int64.Parse(audio1SizeKB.Text) * 1024, Int64.Parse(audio2SizeKB.Text) * 1024};
				isXviD = xvid.Checked;
				int videoSize = 0;
				if (mp4.Checked)
				{
					bitrate = calc.calculateMP4VideoBitrate(audioSizes[0] + audioSizes[1], muxedSizeBytes, 
						numberOfFrames, this.containerOverhead, framerate, isXviD, out videoSize);
				}
				else if (mkv.Checked)
				{
					int nbBframes = 0;
					if (bframes.Checked)
						nbBframes = 2;
					bitrate = calc.calculateMKVVideoBitrate(audioSizes[0], audioSizes[1], muxedSizeBytes, 
						numberOfFrames, nbBframes, framerate, isXviD, aud1Type, aud2Type, out videoSize);
				}
				else if (avi.Checked)
				{
					bitrate = calc.calculateAVIBitrate2(audioSizes[0], audioSizes[1], muxedSizeBytes, 
						numberOfFrames, framerate, isXviD, aud1Type, aud2Type, out videoSize);
				}
				this.isUpdating = true;
				int videoSize2 = videoSize / 1024;
				if (audioSizes[0] + audioSizes[1] >= muxedSizeBytes)
				{
					MessageBox.Show("Your audio is larger than the desired output size.\r\nThis is impossible. Please reduce audio bitrate or increase target size",
						"Unobtainable configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
				else
				{
					videoSizeKB.Text = videoSize.ToString();
					videoSizeMB.Text = videoSize2.ToString();
					this.projectedBitrate.Text = bitrate.ToString();
				}
                // UNSET HERE
			}
		}
		#endregion
	}
}