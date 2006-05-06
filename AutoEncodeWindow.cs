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
using System.IO;
using System.Text;

namespace MeGUI
{
	/// <summary>
	/// Summary description for AutoEncode.
	/// </summary>
	public class AutoEncodeWindow : System.Windows.Forms.Form
	{
		#region variables
		private BitrateCalculator calc;
		private AudioStream[] audioStreams;
        private bool prerender;
        private MeGUI mainForm;
		private JobUtil jobUtil;
		private VideoUtil vUtil;
		private StringBuilder logBuilder;
		private CommandLineGenerator gen;
		private bool isBitrateMode = true;

		private System.Windows.Forms.GroupBox AutomaticEncodingGroup;
		private System.Windows.Forms.Label VideoFileSizeLabel;
		private System.Windows.Forms.Label StorageMediumLabel;
		private System.Windows.Forms.RadioButton FileSizeRadio;
        private System.Windows.Forms.Label SplitSizeLabel;
		private System.Windows.Forms.Label AverageBitrateLabel;
		private System.Windows.Forms.Label FileSizeLabel;
        private System.Windows.Forms.GroupBox OutputGroupBox;
        private System.Windows.Forms.TextBox muxedOutput;
		private System.Windows.Forms.SaveFileDialog saveDialog;
		private System.Windows.Forms.Button muxedOutputButton;
		private System.Windows.Forms.CheckBox splitOutput;
		private System.Windows.Forms.TextBox splitSize;
		private System.Windows.Forms.Button queueButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox muxedSize;
		private System.Windows.Forms.ComboBox sizeSelection;
        private System.Windows.Forms.TextBox projectedBitrate;
		private System.Windows.Forms.TextBox videoSize;
		private System.Windows.Forms.Label videoSizeLabel;
		private System.Windows.Forms.CheckBox addSubsNChapters;
		private System.Windows.Forms.RadioButton averageBitrateRadio;
        private RadioButton noTargetRadio;
        private Label muxedOutputLabel;
        private Label containerLabel;
        private ComboBox container;
        private ToolTip defaultToolTip;
        private MuxProvider muxProvider;
        private VideoStream videoStream;
		#endregion
        private IContainer components;
		#region start / stop
		public AutoEncodeWindow()
		{
			InitializeComponent();
			calc = new BitrateCalculator();
			logBuilder = new StringBuilder();
			gen = new CommandLineGenerator();
			sizeSelection.Items.AddRange(calc.getPredefinedOutputSizes());
            muxProvider = new MuxProvider();
            this.container.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
        }
        public AutoEncodeWindow(VideoStream videoStream, AudioStream[] audioStreams, MeGUI mainForm, bool prerender)
            : this()
        {
            if (videoStream.Settings.EncodingMode == 1 || videoStream.Settings.EncodingMode == 9) // CQ and CRF -- no bitrate possible
            {
                averageBitrateRadio.Enabled = false;
                FileSizeRadio.Enabled = false;
                noTargetRadio.Checked = true;
            }
            this.videoStream = videoStream;
            this.audioStreams = audioStreams;
            this.prerender = prerender;
			this.mainForm = mainForm;
			jobUtil = new JobUtil(mainForm);
			vUtil = new VideoUtil(mainForm);
        }
        /// <summary>
        /// does the final initialization of the dialog
        /// gets all audio types from the audio streams, then asks the muxprovider for a list of containers it can mux the video and audio streams into
        /// if there is no muxer that can deliver any container for the video / audio combination, we can abort right away
        /// </summary>
        /// <returns>true if the given video/audio combination can be muxed to at least a single container, false if not</returns>
        public bool init()
        {
            List<AudioCodec> aTypes = new List<AudioCodec>();
            AudioCodec[] audioTypes;
            foreach (AudioStream stream in this.audioStreams)
            {
                if (stream.settings != null && !String.IsNullOrEmpty(stream.path) && !String.IsNullOrEmpty(stream.output))
                    aTypes.Add(stream.settings.Codec);
            }
            audioTypes = aTypes.ToArray();
            List<ContainerFileType> supportedOutputTypes = this.muxProvider.GetSupportedContainers(this.videoStream.Settings.Codec, audioTypes, 1 + audioTypes.Length);
            if (supportedOutputTypes.Count > 0)
            {
                this.container.Items.Clear();
                this.container.Items.AddRange(supportedOutputTypes.ToArray());
                this.container.SelectedIndex = 0;
                string muxedName = MeGUI.GetDirectoryName(mainForm.VideoIO[1]) + @"\" + Path.GetFileNameWithoutExtension(mainForm.VideoIO[1]) + "-muxed.";
                this.muxedOutput.Text = Path.ChangeExtension(muxedName, (this.container.SelectedItem as ContainerFileType).Extension);
                this.sizeSelection.SelectedIndex = 2;
                return true;
            }
            else
                return false;
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
            this.AutomaticEncodingGroup = new System.Windows.Forms.GroupBox();
            this.noTargetRadio = new System.Windows.Forms.RadioButton();
            this.splitOutput = new System.Windows.Forms.CheckBox();
            this.videoSizeLabel = new System.Windows.Forms.Label();
            this.videoSize = new System.Windows.Forms.TextBox();
            this.VideoFileSizeLabel = new System.Windows.Forms.Label();
            this.sizeSelection = new System.Windows.Forms.ComboBox();
            this.StorageMediumLabel = new System.Windows.Forms.Label();
            this.averageBitrateRadio = new System.Windows.Forms.RadioButton();
            this.FileSizeRadio = new System.Windows.Forms.RadioButton();
            this.splitSize = new System.Windows.Forms.TextBox();
            this.SplitSizeLabel = new System.Windows.Forms.Label();
            this.projectedBitrate = new System.Windows.Forms.TextBox();
            this.AverageBitrateLabel = new System.Windows.Forms.Label();
            this.muxedSize = new System.Windows.Forms.TextBox();
            this.FileSizeLabel = new System.Windows.Forms.Label();
            this.queueButton = new System.Windows.Forms.Button();
            this.OutputGroupBox = new System.Windows.Forms.GroupBox();
            this.container = new System.Windows.Forms.ComboBox();
            this.containerLabel = new System.Windows.Forms.Label();
            this.muxedOutputLabel = new System.Windows.Forms.Label();
            this.muxedOutput = new System.Windows.Forms.TextBox();
            this.muxedOutputButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.addSubsNChapters = new System.Windows.Forms.CheckBox();
            this.defaultToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.AutomaticEncodingGroup.SuspendLayout();
            this.OutputGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AutomaticEncodingGroup
            // 
            this.AutomaticEncodingGroup.Controls.Add(this.noTargetRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.videoSizeLabel);
            this.AutomaticEncodingGroup.Controls.Add(this.videoSize);
            this.AutomaticEncodingGroup.Controls.Add(this.VideoFileSizeLabel);
            this.AutomaticEncodingGroup.Controls.Add(this.sizeSelection);
            this.AutomaticEncodingGroup.Controls.Add(this.StorageMediumLabel);
            this.AutomaticEncodingGroup.Controls.Add(this.averageBitrateRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.FileSizeRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.projectedBitrate);
            this.AutomaticEncodingGroup.Controls.Add(this.AverageBitrateLabel);
            this.AutomaticEncodingGroup.Controls.Add(this.muxedSize);
            this.AutomaticEncodingGroup.Controls.Add(this.FileSizeLabel);
            this.AutomaticEncodingGroup.Location = new System.Drawing.Point(12, 86);
            this.AutomaticEncodingGroup.Name = "AutomaticEncodingGroup";
            this.AutomaticEncodingGroup.Size = new System.Drawing.Size(424, 104);
            this.AutomaticEncodingGroup.TabIndex = 17;
            this.AutomaticEncodingGroup.TabStop = false;
            this.AutomaticEncodingGroup.Text = "Size and Bitrate";
            // 
            // noTargetRadio
            // 
            this.noTargetRadio.Location = new System.Drawing.Point(16, 72);
            this.noTargetRadio.Name = "noTargetRadio";
            this.noTargetRadio.Size = new System.Drawing.Size(197, 18);
            this.noTargetRadio.TabIndex = 22;
            this.noTargetRadio.TabStop = true;
            this.noTargetRadio.Text = "No Target Size (use profile settings)";
            this.defaultToolTip.SetToolTip(this.noTargetRadio, "Checking this allows the use of a previously defined bitrate or a non bitrate mod" +
                    "e (CQ, CRF)");
            this.noTargetRadio.UseVisualStyleBackColor = true;
            this.noTargetRadio.CheckedChanged += new System.EventHandler(this.calculationMode_CheckedChanged);
            // 
            // splitOutput
            // 
            this.splitOutput.Location = new System.Drawing.Point(192, 21);
            this.splitOutput.Name = "splitOutput";
            this.splitOutput.Size = new System.Drawing.Size(97, 18);
            this.splitOutput.TabIndex = 21;
            this.splitOutput.Text = "Split Output";
            this.splitOutput.CheckedChanged += new System.EventHandler(this.splitOutput_CheckedChanged);
            // 
            // videoSizeLabel
            // 
            this.videoSizeLabel.Location = new System.Drawing.Point(376, 47);
            this.videoSizeLabel.Name = "videoSizeLabel";
            this.videoSizeLabel.Size = new System.Drawing.Size(24, 13);
            this.videoSizeLabel.TabIndex = 20;
            this.videoSizeLabel.Text = "KB";
            // 
            // videoSize
            // 
            this.videoSize.Enabled = false;
            this.videoSize.Location = new System.Drawing.Point(308, 44);
            this.videoSize.Name = "videoSize";
            this.videoSize.Size = new System.Drawing.Size(64, 21);
            this.videoSize.TabIndex = 19;
            this.videoSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // VideoFileSizeLabel
            // 
            this.VideoFileSizeLabel.Location = new System.Drawing.Point(222, 47);
            this.VideoFileSizeLabel.Name = "VideoFileSizeLabel";
            this.VideoFileSizeLabel.Size = new System.Drawing.Size(82, 18);
            this.VideoFileSizeLabel.TabIndex = 18;
            this.VideoFileSizeLabel.Text = "Video File Size";
            // 
            // sizeSelection
            // 
            this.sizeSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeSelection.Location = new System.Drawing.Point(308, 16);
            this.sizeSelection.Name = "sizeSelection";
            this.sizeSelection.Size = new System.Drawing.Size(64, 21);
            this.sizeSelection.TabIndex = 0;
            this.sizeSelection.SelectedIndexChanged += new System.EventHandler(this.sizeSelection_SelectedIndexChanged);
            // 
            // StorageMediumLabel
            // 
            this.StorageMediumLabel.Location = new System.Drawing.Point(222, 20);
            this.StorageMediumLabel.Name = "StorageMediumLabel";
            this.StorageMediumLabel.Size = new System.Drawing.Size(90, 18);
            this.StorageMediumLabel.TabIndex = 17;
            this.StorageMediumLabel.Text = "Storage Medium";
            // 
            // averageBitrateRadio
            // 
            this.averageBitrateRadio.Location = new System.Drawing.Point(16, 47);
            this.averageBitrateRadio.Name = "averageBitrateRadio";
            this.averageBitrateRadio.Size = new System.Drawing.Size(100, 18);
            this.averageBitrateRadio.TabIndex = 16;
            this.averageBitrateRadio.Text = "Average Bitrate";
            this.averageBitrateRadio.CheckedChanged += new System.EventHandler(this.calculationMode_CheckedChanged);
            // 
            // FileSizeRadio
            // 
            this.FileSizeRadio.Checked = true;
            this.FileSizeRadio.Location = new System.Drawing.Point(16, 20);
            this.FileSizeRadio.Name = "FileSizeRadio";
            this.FileSizeRadio.Size = new System.Drawing.Size(100, 18);
            this.FileSizeRadio.TabIndex = 15;
            this.FileSizeRadio.TabStop = true;
            this.FileSizeRadio.Text = "File Size";
            this.FileSizeRadio.CheckedChanged += new System.EventHandler(this.calculationMode_CheckedChanged);
            // 
            // splitSize
            // 
            this.splitSize.Enabled = false;
            this.splitSize.Location = new System.Drawing.Point(308, 20);
            this.splitSize.Name = "splitSize";
            this.splitSize.Size = new System.Drawing.Size(64, 21);
            this.splitSize.TabIndex = 13;
            this.splitSize.Text = "0";
            this.splitSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // SplitSizeLabel
            // 
            this.SplitSizeLabel.Location = new System.Drawing.Point(381, 23);
            this.SplitSizeLabel.Name = "SplitSizeLabel";
            this.SplitSizeLabel.Size = new System.Drawing.Size(32, 16);
            this.SplitSizeLabel.TabIndex = 14;
            this.SplitSizeLabel.Text = "MB";
            // 
            // projectedBitrate
            // 
            this.projectedBitrate.Enabled = false;
            this.projectedBitrate.Location = new System.Drawing.Point(118, 44);
            this.projectedBitrate.Name = "projectedBitrate";
            this.projectedBitrate.Size = new System.Drawing.Size(64, 21);
            this.projectedBitrate.TabIndex = 9;
            this.projectedBitrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.projectedBitrate.TextChanged += new System.EventHandler(this.projectedBitrate_TextChanged);
            // 
            // AverageBitrateLabel
            // 
            this.AverageBitrateLabel.Location = new System.Drawing.Point(187, 47);
            this.AverageBitrateLabel.Name = "AverageBitrateLabel";
            this.AverageBitrateLabel.Size = new System.Drawing.Size(32, 23);
            this.AverageBitrateLabel.TabIndex = 10;
            this.AverageBitrateLabel.Text = "kbit/s";
            // 
            // muxedSize
            // 
            this.muxedSize.Location = new System.Drawing.Point(118, 16);
            this.muxedSize.Name = "muxedSize";
            this.muxedSize.Size = new System.Drawing.Size(64, 21);
            this.muxedSize.TabIndex = 1;
            this.muxedSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.muxedSize.TextChanged += new System.EventHandler(this.muxedSize_TextChanged);
            // 
            // FileSizeLabel
            // 
            this.FileSizeLabel.Location = new System.Drawing.Point(187, 20);
            this.FileSizeLabel.Name = "FileSizeLabel";
            this.FileSizeLabel.Size = new System.Drawing.Size(24, 13);
            this.FileSizeLabel.TabIndex = 4;
            this.FileSizeLabel.Text = "MB";
            // 
            // queueButton
            // 
            this.queueButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.queueButton.Location = new System.Drawing.Point(360, 196);
            this.queueButton.Name = "queueButton";
            this.queueButton.Size = new System.Drawing.Size(74, 23);
            this.queueButton.TabIndex = 8;
            this.queueButton.Text = "Queue";
            this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
            // 
            // OutputGroupBox
            // 
            this.OutputGroupBox.Controls.Add(this.container);
            this.OutputGroupBox.Controls.Add(this.splitOutput);
            this.OutputGroupBox.Controls.Add(this.containerLabel);
            this.OutputGroupBox.Controls.Add(this.muxedOutputLabel);
            this.OutputGroupBox.Controls.Add(this.muxedOutput);
            this.OutputGroupBox.Controls.Add(this.muxedOutputButton);
            this.OutputGroupBox.Controls.Add(this.SplitSizeLabel);
            this.OutputGroupBox.Controls.Add(this.splitSize);
            this.OutputGroupBox.Location = new System.Drawing.Point(10, 4);
            this.OutputGroupBox.Name = "OutputGroupBox";
            this.OutputGroupBox.Size = new System.Drawing.Size(424, 76);
            this.OutputGroupBox.TabIndex = 18;
            this.OutputGroupBox.TabStop = false;
            this.OutputGroupBox.Text = "Output Options";
            // 
            // container
            // 
            this.container.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.container.FormattingEnabled = true;
            this.container.Location = new System.Drawing.Point(118, 20);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(64, 21);
            this.container.TabIndex = 25;
            this.container.SelectedIndexChanged += new System.EventHandler(this.container_SelectedIndexChanged);
            // 
            // containerLabel
            // 
            this.containerLabel.AutoSize = true;
            this.containerLabel.Location = new System.Drawing.Point(6, 23);
            this.containerLabel.Name = "containerLabel";
            this.containerLabel.Size = new System.Drawing.Size(54, 13);
            this.containerLabel.TabIndex = 24;
            this.containerLabel.Text = "Container";
            // 
            // muxedOutputLabel
            // 
            this.muxedOutputLabel.AutoSize = true;
            this.muxedOutputLabel.Location = new System.Drawing.Point(6, 51);
            this.muxedOutputLabel.Name = "muxedOutputLabel";
            this.muxedOutputLabel.Size = new System.Drawing.Size(82, 13);
            this.muxedOutputLabel.TabIndex = 23;
            this.muxedOutputLabel.Text = "Name of output";
            // 
            // muxedOutput
            // 
            this.muxedOutput.Location = new System.Drawing.Point(118, 48);
            this.muxedOutput.Name = "muxedOutput";
            this.muxedOutput.ReadOnly = true;
            this.muxedOutput.Size = new System.Drawing.Size(256, 21);
            this.muxedOutput.TabIndex = 0;
            // 
            // muxedOutputButton
            // 
            this.muxedOutputButton.Location = new System.Drawing.Point(381, 46);
            this.muxedOutputButton.Name = "muxedOutputButton";
            this.muxedOutputButton.Size = new System.Drawing.Size(24, 23);
            this.muxedOutputButton.TabIndex = 19;
            this.muxedOutputButton.Text = "...";
            this.muxedOutputButton.Click += new System.EventHandler(this.muxedOutputButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(280, 196);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(72, 23);
            this.cancelButton.TabIndex = 19;
            this.cancelButton.Text = "Cancel";
            // 
            // addSubsNChapters
            // 
            this.addSubsNChapters.Location = new System.Drawing.Point(18, 195);
            this.addSubsNChapters.Name = "addSubsNChapters";
            this.addSubsNChapters.Size = new System.Drawing.Size(256, 24);
            this.addSubsNChapters.TabIndex = 20;
            this.addSubsNChapters.Text = "Add additional content (audio, subs, chapters)";
            this.defaultToolTip.SetToolTip(this.addSubsNChapters, "Checking this option allows you to specify pre-encoded audio and subtitle files t" +
                    "o be added to your output, as well as assign audio/subtitle languages and assign" +
                    " a chapter file");
            // 
            // AutoEncodeWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(444, 223);
            this.Controls.Add(this.addSubsNChapters);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OutputGroupBox);
            this.Controls.Add(this.AutomaticEncodingGroup);
            this.Controls.Add(this.queueButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoEncodeWindow";
            this.ShowInTaskbar = false;
            this.Text = "Automatic Encoding";
            this.TopMost = true;
            this.AutomaticEncodingGroup.ResumeLayout(false);
            this.AutomaticEncodingGroup.PerformLayout();
            this.OutputGroupBox.ResumeLayout(false);
            this.OutputGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
		#region dropdowns
        /// <summary>
        /// adjusted the desired size indication when another item in the dropdown has been chosen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sizeSelection_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int size = calc.getOutputSize(sizeSelection.SelectedIndex) / 1024;
			this.muxedSize.Text = size.ToString();
		}
        /// <summary>
        /// adjusts the output extension when the container is being changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void container_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(muxedOutput.Text))
            {
                this.muxedOutput.Text = Path.ChangeExtension(muxedOutput.Text, (this.container.SelectedItem as ContainerFileType).Extension);
            }
        }
        #endregion
		#region additional events
		private void muxedOutputButton_Click(object sender, System.EventArgs e)
		{
            ContainerFileType cot = this.container.SelectedItem as ContainerFileType;
            this.saveDialog.Filter = cot.OutputFilterString;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                this.muxedOutput.Text = saveDialog.FileName;
            }
        }

		private void splitOutput_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.splitOutput.Checked)
				splitSize.Enabled = true;
			else
				splitSize.Enabled = false;
		}
		/// <summary>
		/// handles the selection of the output format
		/// in case of avi, if an encodeable audio stream is already present,
		/// the selection of additional streams needs to be completely disabled
		/// if not, it an be left enabled bt the text has to indicate the fact
		/// that you can only add an audio track and nothing else
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void outputFormat_CheckedChanged(object sender, System.EventArgs e)
		{
            ContainerFileType cot = this.container.SelectedItem as ContainerFileType;
            this.muxedOutput.Text = Path.ChangeExtension(this.muxedOutput.Text, cot.Extension);
        }
		/// <summary>
		/// separates encodable from muxable audio streams
		/// in addition to returning the two types separately an array of SubStreams is returned
		/// which is plugged into the muxer.. it contains the names of all the audio files
		/// that have to be muxed
		/// </summary>
		/// <param name="encodable">encodeable audio streams</param>
		/// <param name="muxable">muxable Audio Streams with the path filled out and a blank language</param>
		private void separateEncodableAndMuxableAudioStreams(out AudioStream[] encodable, out SubStream[] muxable)
		{
			encodable = this.getConfiguredAudioJobs(); // discards improperly configured ones
			// the rest of the job is all encodeable
			muxable = new SubStream[encodable.Length];
			int j = 0;
			foreach (AudioStream stream in encodable)
			{
				muxable[j].path = stream.output;
				muxable[j].language = "";
				j++;
			}
		}
		#endregion
		#region helper methods
		/// <summary>
		/// sets the projected video bitrate field in the GUI
		/// </summary>
		private void setVideoBitrate()
		{
            try
            {
                long desiredSize = Int64.Parse(muxedSize.Text) * 1024L * 1024L;
                long videoSize;
                int bitrate = calc.CalculateBitrate(videoStream.Settings.Codec,
                    (videoStream.Settings.NbBframes > 0),
                    ((ContainerFileType)container.SelectedItem).ContainerType,
                    audioStreams,
                    desiredSize,
                    videoStream.NumberOfFrames,
                    videoStream.Framerate,
                    out videoSize);
                this.videoSize.Text = videoSize.ToString();
                this.projectedBitrate.Text = bitrate.ToString();
            }
            catch (Exception)
            {
                this.projectedBitrate.Text = "";
                this.videoSize.Text = "";
            }
        }

		/// <summary>
		/// sets the size of the output given the desired bitrate
		/// </summary>
		private void setTargetSize()
		{
			try
            {
                int desiredBitrate = Int32.Parse(this.projectedBitrate.Text); 
                int outputSize = 0, rawVideoSize = 0;
                outputSize = calc.CalculateFileSize(videoStream.Settings.Codec,
                    (videoStream.Settings.NbBframes > 0),
                    ((ContainerFileType)container.SelectedItem).ContainerType,
                    audioStreams,
                    desiredBitrate,
                    videoStream.NumberOfFrames,
                    videoStream.Framerate,
                    out rawVideoSize);
                this.videoSize.Text = rawVideoSize.ToString();
                this.muxedSize.Text = outputSize.ToString();
            }
            catch (Exception)
            {
				this.videoSize.Text = "";
				this.muxedSize.Text = "";
			}
		}
		#region audio
        /// <summary>
        /// sets the projected audio size for all audio streams that use CBR mode
        /// </summary>
        private void setAudioSize()
        {
            long[] sizes = new long[this.audioStreams.Length];
            int index = 0;
            foreach (AudioStream stream in this.audioStreams)
            {
                if (!stream.output.Equals("")) // if we don't have the video length or the audio is not fully configured we can give up now
                {
                    long bytesPerSecond = 0;
                    if (stream.BitrateMode == BitrateManagementMode.CBR)
                    {
                        bytesPerSecond = stream.settings.Bitrate * 1000 / 8;
                    }
                    double lengthInSeconds = (double)this.videoStream.NumberOfFrames / (double)this.videoStream.Framerate;
                    long sizeInBytes = (long)(lengthInSeconds * bytesPerSecond);
                    this.audioStreams[index].Size = sizeInBytes;
                    index++;
                }
            }
        }
		#endregion
		/// <summary>
		/// gets the split size for the muxed output
		/// </summary>
		/// <returns></returns>
		public int getSplitSize()
		{
			int splitSize = 0;
			try
			{
				splitSize = Int32.Parse(this.splitSize.Text) * 1024;
			}
			catch (Exception e)
			{
				MessageBox.Show("I'm not sure how you want me to split the output at an undefinied position.\r\nWhere I'm from that number doesn't exist.\r\n" +
					"I'm going to assume you meant to not split the output", "Split size undefined", MessageBoxButtons.OK);
				Console.Write(e.Message);
			}
			return splitSize;
		}

		/// <summary>
		/// returns all audio streams that can be encoded or muxed
		/// </summary>
		/// <returns></returns>
		private AudioStream[] getConfiguredAudioJobs()
		{
            List<AudioStream> list = new List<AudioStream>();
			foreach (AudioStream stream in audioStreams)
			{
                if (String.IsNullOrEmpty(stream.path))
                {
                    // no audio is ok, just skip
                    break;
                }
                list.Add(stream);

			}
            return list.ToArray();
		}
		#endregion
		#region button events
		/// <summary>
		/// handles the go button for automated encoding
		/// checks if we're in automated 2 pass video mode and that we're not using the snow codec
		/// then the video and audio configuration is checked, and if it checks out
		/// the audio job, video jobs and muxing job are generated, audio and video job are linked
		/// and encoding is started
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void queueButton_Click(object sender, System.EventArgs e)
		{
			if (!this.muxedOutput.Text.Equals(""))
			{
				long desiredSize;
                if (!noTargetRadio.Checked)
                {
                    try
                    {
                        desiredSize = Int64.Parse(this.muxedSize.Text) * 1048576L;
                    }
                    catch (Exception f)
                    {
                        MessageBox.Show("I'm not sure how you want me to reach a target size of <empty>.\r\nWhere I'm from that number doesn't exist.\r\n",
                            "Target size undefined", MessageBoxButtons.OK);
                        Console.Write(f.Message);
                        return;
                    }
                }
                else
                {
                    desiredSize = -1;
                }
				int splitSize = 0;
				if (splitOutput.Checked)
				{
					splitSize = Int32.Parse(this.splitSize.Text);
				}
                if (desiredSize > 0)
                    logBuilder.Append("Desired size of this automated encoding series: " + desiredSize + " bytes, split size: " + splitSize + "\r\n");
                else
                    logBuilder.Append("No desired size of this encode. The profile settings will be used");
				SubStream[] audio;
				AudioStream[] aStreams;
				separateEncodableAndMuxableAudioStreams(out aStreams, out audio);
				SubStream[] subtitles = new SubStream[0];
				string chapters = "";
				string videoInput = mainForm.VideoIO[0];
				string videoOutput = mainForm.VideoIO[1];
                string muxedOutput = this.muxedOutput.Text;
                ContainerFileType cot = this.container.SelectedItem as ContainerFileType;
                if (addSubsNChapters.Checked)
				{
                    AdaptiveMuxWindow amw = new AdaptiveMuxWindow(mainForm);
                    amw.setMinimizedMode(videoOutput, jobUtil.getFramerate(videoInput), audio,
                        muxedOutput, this.getSplitSize(), cot);
                    if (amw.ShowDialog() == DialogResult.OK)
                        amw.getAdditionalStreams(out audio, out subtitles, out chapters, out muxedOutput, out cot);
                }
                removeStreamsToBeEncoded(ref audio, aStreams);
                this.vUtil.GenerateJobSeries(this.videoStream, muxedOutput, aStreams, subtitles, chapters,
                    desiredSize, splitSize, cot.ContainerType, this.prerender, audio);
                this.Close();
			}
		}

        /// <summary>
        /// Reallocates the audio array so that it only has the files to be muxed and not the files to be encoded, then muxed
        /// </summary>
        /// <param name="audio">All files to be muxed (including the ones which will be encoded first)</param>
        /// <param name="aStreams">All files being encoded (these will be removed from the audio array)</param>
        private void removeStreamsToBeEncoded(ref SubStream[] audio, AudioStream[] aStreams)
        {
            List<SubStream> newAudio = new List<SubStream>();
            foreach (SubStream stream in audio)
            {
                bool matchFound = false;
                foreach (AudioStream a in aStreams)
                {
                    if (stream.path == a.output)
                    {
                        matchFound = true; // In this case we have found a file which needs to be encoded
                        break;
                    }
                }
                if (!matchFound) // in this case we have not found any files which will be encoded first to produce this file
                {
                    newAudio.Add(stream);
                }
            }
            audio = newAudio.ToArray();
        }
		#endregion
		#region event helper methods
		private void textField_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (! char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
				e.Handled = true;
		}

		private void muxedSize_TextChanged(object sender, System.EventArgs e)
		{
			if (isBitrateMode)
				this.setVideoBitrate();
		}

		private void containerOverhead_ValueChanged(object sender, System.EventArgs e)
		{
			if (isBitrateMode)
				this.setVideoBitrate();
			else
				this.setTargetSize();
		}
		private void projectedBitrate_TextChanged(object sender, System.EventArgs e)
		{
			if (!this.isBitrateMode)
				this.setTargetSize();
		}
		private void calculationMode_CheckedChanged(object sender, System.EventArgs e)
		{
			if (averageBitrateRadio.Checked)
			{
				muxedSize.Enabled = false;
				this.projectedBitrate.Enabled = true;
				this.isBitrateMode = false;
				this.sizeSelection.Enabled = false;
			}
            else if (noTargetRadio.Checked)
            {
                muxedSize.Enabled = false;
                this.projectedBitrate.Enabled = false;
                this.isBitrateMode = false;
                this.sizeSelection.Enabled = false;
            } 
            else
			{
				muxedSize.Enabled = true;
				this.projectedBitrate.Enabled = false;
				this.isBitrateMode = true;
				this.sizeSelection.Enabled = true;
			}
		}
		#endregion
	}
}