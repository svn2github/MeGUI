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
using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;

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
        private MainForm mainForm;
		private JobUtil jobUtil;
		private VideoUtil vUtil;
		private StringBuilder logBuilder;
		private CommandLineGenerator gen;
		private bool isBitrateMode = true;

		private System.Windows.Forms.GroupBox AutomaticEncodingGroup;
		private System.Windows.Forms.Label VideoFileSizeLabel;
		private System.Windows.Forms.Label StorageMediumLabel;
        private System.Windows.Forms.RadioButton FileSizeRadio;
        private System.Windows.Forms.Label AverageBitrateLabel;
        private System.Windows.Forms.GroupBox OutputGroupBox;
        private System.Windows.Forms.TextBox muxedOutput;
		private System.Windows.Forms.SaveFileDialog saveDialog;
		private System.Windows.Forms.Button muxedOutputButton;
        private System.Windows.Forms.CheckBox splitOutput;
		private System.Windows.Forms.Button queueButton;
        private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ComboBox sizeSelection;
        private System.Windows.Forms.TextBox projectedBitrateKBits;
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
        private MeGUI.core.gui.FileSizeBar muxedSize;
        private MeGUI.core.gui.FileSizeBar videoSize;
        private MeGUI.core.gui.FileSizeBar splitSize;
        private MeGUI.core.gui.HelpButton helpButton1;


        private IContainer components;
		#region start / stop
		public AutoEncodeWindow()
		{
			InitializeComponent();
			calc = new BitrateCalculator();
			logBuilder = new StringBuilder();
			gen = new CommandLineGenerator();
			sizeSelection.Items.AddRange(calc.getPredefinedOutputSizes());
        }
        public AutoEncodeWindow(VideoStream videoStream, AudioStream[] audioStreams, MainForm mainForm, bool prerender)
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
            muxProvider = mainForm.MuxProvider;
            container.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
        }
        /// <summary>
        /// does the final initialization of the dialog
        /// gets all audio types from the audio streams, then asks the muxprovider for a list of containers it can mux the video and audio streams into
        /// if there is no muxer that can deliver any container for the video / audio combination, we can abort right away
        /// </summary>
        /// <returns>true if the given video/audio combination can be muxed to at least a single container, false if not</returns>
        public bool init()
        {
            List<AudioEncoderType> aTypes = new List<AudioEncoderType>();
            AudioEncoderType[] audioTypes;
            foreach (AudioStream stream in this.audioStreams)
            {
                if (stream.settings != null && !String.IsNullOrEmpty(stream.path) && !String.IsNullOrEmpty(stream.output))
                    aTypes.Add(stream.settings.EncoderType);
            }
            audioTypes = aTypes.ToArray();
            List<ContainerType> supportedOutputTypes = this.muxProvider.GetSupportedContainers(
                this.videoStream.Settings.EncoderType, audioTypes);
            if (supportedOutputTypes.Count > 0)
            {
                this.container.Items.Clear();
                this.container.Items.AddRange(supportedOutputTypes.ToArray());
                this.container.SelectedIndex = 0;
                string muxedName = Path.Combine(Path.GetDirectoryName(mainForm.Video.Info.VideoOutput), Path.GetFileNameWithoutExtension(mainForm.Video.Info.VideoOutput) + "-muxed.");
                this.muxedOutput.Text = Path.ChangeExtension(muxedName, (this.container.SelectedItem as ContainerType).Extension);
                this.sizeSelection.SelectedIndex = 2;

                if (mainForm.Settings.AedSettings.SplitOutput)
                {
                    splitOutput.Checked = true;
                    splitSize.Value = new FileSize(Unit.MB,
                        mainForm.Settings.AedSettings.SplitSize);
                }
                if (mainForm.Settings.AedSettings.FileSizeMode && FileSizeRadio.Enabled)
                {
                    FileSizeRadio.Checked = true;
                    muxedSize.Value = new FileSize(Unit.MB,
                        mainForm.Settings.AedSettings.FileSize);
                }
                else if (mainForm.Settings.AedSettings.BitrateMode)
                {
                    averageBitrateRadio.Checked = true;
                    projectedBitrateKBits.Text = mainForm.Settings.AedSettings.Bitrate.ToString();
                }
                else
                    noTargetRadio.Checked = true;
                if (mainForm.Settings.AedSettings.AddAdditionalContent)
                    addSubsNChapters.Checked = true;
                foreach (object o in container.Items) // I know this is ugly, but using the ContainerType doesn't work unless we're switching to manual serialization
                {
                    if (o.ToString().Equals(mainForm.Settings.AedSettings.Container))
                    {
                        container.SelectedItem = o;
                        break;
                    }
                }


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
            this.videoSize = new MeGUI.core.gui.FileSizeBar();
            this.muxedSize = new MeGUI.core.gui.FileSizeBar();
            this.noTargetRadio = new System.Windows.Forms.RadioButton();
            this.VideoFileSizeLabel = new System.Windows.Forms.Label();
            this.sizeSelection = new System.Windows.Forms.ComboBox();
            this.StorageMediumLabel = new System.Windows.Forms.Label();
            this.averageBitrateRadio = new System.Windows.Forms.RadioButton();
            this.FileSizeRadio = new System.Windows.Forms.RadioButton();
            this.projectedBitrateKBits = new System.Windows.Forms.TextBox();
            this.AverageBitrateLabel = new System.Windows.Forms.Label();
            this.splitOutput = new System.Windows.Forms.CheckBox();
            this.queueButton = new System.Windows.Forms.Button();
            this.OutputGroupBox = new System.Windows.Forms.GroupBox();
            this.splitSize = new MeGUI.core.gui.FileSizeBar();
            this.container = new System.Windows.Forms.ComboBox();
            this.containerLabel = new System.Windows.Forms.Label();
            this.muxedOutputLabel = new System.Windows.Forms.Label();
            this.muxedOutput = new System.Windows.Forms.TextBox();
            this.muxedOutputButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.addSubsNChapters = new System.Windows.Forms.CheckBox();
            this.defaultToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.AutomaticEncodingGroup.SuspendLayout();
            this.OutputGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AutomaticEncodingGroup
            // 
            this.AutomaticEncodingGroup.Controls.Add(this.videoSize);
            this.AutomaticEncodingGroup.Controls.Add(this.muxedSize);
            this.AutomaticEncodingGroup.Controls.Add(this.noTargetRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.VideoFileSizeLabel);
            this.AutomaticEncodingGroup.Controls.Add(this.sizeSelection);
            this.AutomaticEncodingGroup.Controls.Add(this.StorageMediumLabel);
            this.AutomaticEncodingGroup.Controls.Add(this.averageBitrateRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.FileSizeRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.projectedBitrateKBits);
            this.AutomaticEncodingGroup.Controls.Add(this.AverageBitrateLabel);
            this.AutomaticEncodingGroup.Location = new System.Drawing.Point(12, 86);
            this.AutomaticEncodingGroup.Name = "AutomaticEncodingGroup";
            this.AutomaticEncodingGroup.Size = new System.Drawing.Size(456, 104);
            this.AutomaticEncodingGroup.TabIndex = 17;
            this.AutomaticEncodingGroup.TabStop = false;
            this.AutomaticEncodingGroup.Text = "Size and Bitrate";
            // 
            // videoSize
            // 
            this.videoSize.CurrentUnit = MeGUI.core.util.Unit.KB;
            this.videoSize.Location = new System.Drawing.Point(341, 45);
            this.videoSize.Name = "videoSize";
            this.videoSize.Size = new System.Drawing.Size(106, 24);
            this.videoSize.TabIndex = 24;
            // 
            // muxedSize
            // 
            this.muxedSize.CurrentUnit = MeGUI.core.util.Unit.MB;
            this.muxedSize.Location = new System.Drawing.Point(119, 14);
            this.muxedSize.Name = "muxedSize";
            this.muxedSize.Size = new System.Drawing.Size(115, 24);
            this.muxedSize.TabIndex = 23;
            this.muxedSize.Load += new System.EventHandler(this.fileSizeBar1_Load);
            // 
            // noTargetRadio
            // 
            this.noTargetRadio.Location = new System.Drawing.Point(16, 72);
            this.noTargetRadio.Name = "noTargetRadio";
            this.noTargetRadio.Size = new System.Drawing.Size(218, 18);
            this.noTargetRadio.TabIndex = 22;
            this.noTargetRadio.TabStop = true;
            this.noTargetRadio.Text = "No Target Size (use profile settings)";
            this.defaultToolTip.SetToolTip(this.noTargetRadio, "Checking this allows the use of a previously defined bitrate or a non bitrate mod" +
                    "e (CQ, CRF)");
            this.noTargetRadio.UseVisualStyleBackColor = true;
            this.noTargetRadio.CheckedChanged += new System.EventHandler(this.calculationMode_CheckedChanged);
            // 
            // VideoFileSizeLabel
            // 
            this.VideoFileSizeLabel.Location = new System.Drawing.Point(250, 47);
            this.VideoFileSizeLabel.Name = "VideoFileSizeLabel";
            this.VideoFileSizeLabel.Size = new System.Drawing.Size(82, 18);
            this.VideoFileSizeLabel.TabIndex = 18;
            this.VideoFileSizeLabel.Text = "Video File Size";
            // 
            // sizeSelection
            // 
            this.sizeSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeSelection.Enabled = false;
            this.sizeSelection.Location = new System.Drawing.Point(341, 17);
            this.sizeSelection.Name = "sizeSelection";
            this.sizeSelection.Size = new System.Drawing.Size(64, 21);
            this.sizeSelection.TabIndex = 0;
            this.sizeSelection.SelectedIndexChanged += new System.EventHandler(this.sizeSelection_SelectedIndexChanged);
            // 
            // StorageMediumLabel
            // 
            this.StorageMediumLabel.Location = new System.Drawing.Point(246, 17);
            this.StorageMediumLabel.Name = "StorageMediumLabel";
            this.StorageMediumLabel.Size = new System.Drawing.Size(90, 18);
            this.StorageMediumLabel.TabIndex = 17;
            this.StorageMediumLabel.Text = "Storage Medium";
            // 
            // averageBitrateRadio
            // 
            this.averageBitrateRadio.AutoSize = true;
            this.averageBitrateRadio.Location = new System.Drawing.Point(16, 45);
            this.averageBitrateRadio.Name = "averageBitrateRadio";
            this.averageBitrateRadio.Size = new System.Drawing.Size(101, 17);
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
            // projectedBitrateKBits
            // 
            this.projectedBitrateKBits.Enabled = false;
            this.projectedBitrateKBits.Location = new System.Drawing.Point(119, 41);
            this.projectedBitrateKBits.Name = "projectedBitrateKBits";
            this.projectedBitrateKBits.Size = new System.Drawing.Size(85, 21);
            this.projectedBitrateKBits.TabIndex = 9;
            this.projectedBitrateKBits.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.projectedBitrateKBits.TextChanged += new System.EventHandler(this.projectedBitrate_TextChanged);
            // 
            // AverageBitrateLabel
            // 
            this.AverageBitrateLabel.AutoSize = true;
            this.AverageBitrateLabel.Location = new System.Drawing.Point(207, 47);
            this.AverageBitrateLabel.Name = "AverageBitrateLabel";
            this.AverageBitrateLabel.Size = new System.Drawing.Size(33, 13);
            this.AverageBitrateLabel.TabIndex = 10;
            this.AverageBitrateLabel.Text = "kbit/s";
            // 
            // splitOutput
            // 
            this.splitOutput.AutoSize = true;
            this.splitOutput.Location = new System.Drawing.Point(212, 22);
            this.splitOutput.Name = "splitOutput";
            this.splitOutput.Size = new System.Drawing.Size(83, 17);
            this.splitOutput.TabIndex = 21;
            this.splitOutput.Text = "Split Output";
            this.splitOutput.CheckedChanged += new System.EventHandler(this.splitOutput_CheckedChanged);
            // 
            // queueButton
            // 
            this.queueButton.AutoSize = true;
            this.queueButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.queueButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.queueButton.Location = new System.Drawing.Point(358, 196);
            this.queueButton.Name = "queueButton";
            this.queueButton.Size = new System.Drawing.Size(49, 23);
            this.queueButton.TabIndex = 8;
            this.queueButton.Text = "Queue";
            this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
            // 
            // OutputGroupBox
            // 
            this.OutputGroupBox.Controls.Add(this.splitSize);
            this.OutputGroupBox.Controls.Add(this.container);
            this.OutputGroupBox.Controls.Add(this.splitOutput);
            this.OutputGroupBox.Controls.Add(this.containerLabel);
            this.OutputGroupBox.Controls.Add(this.muxedOutputLabel);
            this.OutputGroupBox.Controls.Add(this.muxedOutput);
            this.OutputGroupBox.Controls.Add(this.muxedOutputButton);
            this.OutputGroupBox.Location = new System.Drawing.Point(10, 4);
            this.OutputGroupBox.Name = "OutputGroupBox";
            this.OutputGroupBox.Size = new System.Drawing.Size(458, 76);
            this.OutputGroupBox.TabIndex = 18;
            this.OutputGroupBox.TabStop = false;
            this.OutputGroupBox.Text = "Output Options";
            // 
            // splitSize
            // 
            this.splitSize.CurrentUnit = MeGUI.core.util.Unit.MB;
            this.splitSize.Enabled = false;
            this.splitSize.Location = new System.Drawing.Point(304, 20);
            this.splitSize.Name = "splitSize";
            this.splitSize.Size = new System.Drawing.Size(148, 24);
            this.splitSize.TabIndex = 0;
            // 
            // container
            // 
            this.container.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.container.FormattingEnabled = true;
            this.container.Location = new System.Drawing.Point(97, 20);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(85, 21);
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
            this.muxedOutput.Location = new System.Drawing.Point(97, 48);
            this.muxedOutput.Name = "muxedOutput";
            this.muxedOutput.ReadOnly = true;
            this.muxedOutput.Size = new System.Drawing.Size(322, 21);
            this.muxedOutput.TabIndex = 0;
            // 
            // muxedOutputButton
            // 
            this.muxedOutputButton.Location = new System.Drawing.Point(425, 46);
            this.muxedOutputButton.Name = "muxedOutputButton";
            this.muxedOutputButton.Size = new System.Drawing.Size(24, 23);
            this.muxedOutputButton.TabIndex = 19;
            this.muxedOutputButton.Text = "...";
            this.muxedOutputButton.Click += new System.EventHandler(this.muxedOutputButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.AutoSize = true;
            this.cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(413, 196);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(49, 23);
            this.cancelButton.TabIndex = 19;
            this.cancelButton.Text = "Cancel";
            // 
            // addSubsNChapters
            // 
            this.addSubsNChapters.Location = new System.Drawing.Point(88, 196);
            this.addSubsNChapters.Name = "addSubsNChapters";
            this.addSubsNChapters.Size = new System.Drawing.Size(256, 24);
            this.addSubsNChapters.TabIndex = 20;
            this.addSubsNChapters.Text = "Add additional content (audio, subs, chapters)";
            this.defaultToolTip.SetToolTip(this.addSubsNChapters, "Checking this option allows you to specify pre-encoded audio and subtitle files t" +
                    "o be added to your output, as well as assign audio/subtitle languages and assign" +
                    " a chapter file");
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "AutoEncode window";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(10, 196);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 21;
            // 
            // AutoEncodeWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(471, 226);
            this.Controls.Add(this.helpButton1);
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
            this.AutomaticEncodingGroup.ResumeLayout(false);
            this.AutomaticEncodingGroup.PerformLayout();
            this.OutputGroupBox.ResumeLayout(false);
            this.OutputGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
			int sizeMBs = calc.getOutputSizeKBs(sizeSelection.SelectedIndex) / 1024;
			muxedSize.Value = new FileSize(Unit.MB, sizeMBs);
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
                this.muxedOutput.Text = Path.ChangeExtension(muxedOutput.Text, (this.container.SelectedItem as ContainerType).Extension);
            }
        }
        #endregion
		#region additional events
		private void muxedOutputButton_Click(object sender, System.EventArgs e)
		{
            ContainerType cot = this.container.SelectedItem as ContainerType;
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
            ContainerType cot = this.container.SelectedItem as ContainerType;
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
		private void separateEncodableAndMuxableAudioStreams(out AudioStream[] encodable, out SubStream[] muxable, out KnownAudioType[] muxTypes)
		{
			encodable = this.getConfiguredAudioJobs(); // discards improperly configured ones
			// the rest of the job is all encodeable
			muxable = new SubStream[encodable.Length];
            muxTypes = new KnownAudioType[encodable.Length];
			int j = 0;
			foreach (AudioStream stream in encodable)
			{
				muxable[j].path = stream.output;
				muxable[j].language = "";
                muxTypes[j] = new KnownAudioType(new MuxableType(stream.Type, stream.settings.Codec), stream.settings.EncoderType);
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
                long desiredSizeBytes;
                checked { desiredSizeBytes = (long)muxedSize.Value.Bytes; }
                long videoSize;
                int bitrateKbits = calc.CalculateBitrateKBits(videoStream.Settings.Codec,
                    (videoStream.Settings.NbBframes > 0),
                    ((ContainerType)container.SelectedItem),
                    audioStreams,
                    desiredSizeBytes,
                    videoStream.NumberOfFrames,
                    videoStream.Framerate,
                    out videoSize);
#warning check whether codecs use k=1000 or k=1024 for kbits // kick those that still use 1024...
                this.videoSize.Value = new FileSize(Unit.KB, videoSize);
                this.projectedBitrateKBits.Text = bitrateKbits.ToString();
            }
            catch (Exception)
            {
                this.projectedBitrateKBits.Text = "";
                videoSize.Value = FileSize.Empty;
            }
        }

		/// <summary>
		/// sets the size of the output given the desired bitrate
		/// </summary>
		private void setTargetSize()
		{
			try
            {
                int desiredBitrate = Int32.Parse(this.projectedBitrateKBits.Text);
                long outputSize = 0;
                int rawVideoSize = 0;
                outputSize = calc.CalculateFileSizeKB(videoStream.Settings.Codec,
                    (videoStream.Settings.NbBframes > 0),
                    ((ContainerType)container.SelectedItem),
                    audioStreams,
                    desiredBitrate,
                    videoStream.NumberOfFrames,
                    videoStream.Framerate,
                    out rawVideoSize) / 1024L ;
                this.videoSize.Value = new FileSize(Unit.KB, rawVideoSize);
                this.muxedSize.Value = new FileSize(Unit.MB, outputSize);
            }
            catch (Exception)
            {
                videoSize.Value = FileSize.Empty;
                muxedSize.Value = FileSize.Empty;
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
                    this.audioStreams[index].SizeBytes = sizeInBytes;
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
            return (int)this.splitSize.Value.KB;
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
				long desiredSizeBytes;
                if (!noTargetRadio.Checked)
                {
                    try
                    {
                        checked { desiredSizeBytes = (long)muxedSize.Value.Bytes; }
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
                    desiredSizeBytes = -1;
                }
				int splitSize = 0;
				if (splitOutput.Checked)
				{
					splitSize = (int)this.splitSize.Value.MB;
				}
                if (desiredSizeBytes > 0)
                    logBuilder.Append("Desired size of this automated encoding series: " + desiredSizeBytes + " bytes, split size: " + splitSize + "MB\r\n");
                else
                    logBuilder.Append("No desired size of this encode. The profile settings will be used");
				SubStream[] audio;
				AudioStream[] aStreams;
                KnownAudioType[] muxTypes;
				separateEncodableAndMuxableAudioStreams(out aStreams, out audio, out muxTypes);
				SubStream[] subtitles = new SubStream[0];
				string chapters = "";
				string videoInput = mainForm.Video.Info.VideoInput;
                string videoOutput = mainForm.Video.Info.VideoOutput;
                string muxedOutput = this.muxedOutput.Text;
                ContainerType cot = this.container.SelectedItem as ContainerType;
                if (addSubsNChapters.Checked)
				{
                    AdaptiveMuxWindow amw = new AdaptiveMuxWindow(mainForm);
                    amw.setMinimizedMode(videoOutput, videoStream.Settings.EncoderType, jobUtil.getFramerate(videoInput), audio,
                        muxTypes, muxedOutput, splitSize, cot);
                    if (amw.ShowDialog() == DialogResult.OK)
                        amw.getAdditionalStreams(out audio, out subtitles, out chapters, out muxedOutput, out cot);
                    else // user aborted, abort the whole process
                        return;
                }
                removeStreamsToBeEncoded(ref audio, aStreams);
                this.vUtil.GenerateJobSeries(this.videoStream, muxedOutput, aStreams, subtitles, chapters,
                    desiredSizeBytes, splitSize, cot, this.prerender, audio, new List<string>());
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
                videoSize.Enabled = true;
				this.projectedBitrateKBits.Enabled = true;
				this.isBitrateMode = false;
				this.sizeSelection.Enabled = false;
			}
            else if (noTargetRadio.Checked)
            {
                muxedSize.Enabled = false;
                videoSize.Enabled = false;
                this.projectedBitrateKBits.Enabled = false;
                this.isBitrateMode = false;
                this.sizeSelection.Enabled = false;
            } 
            else
			{
				muxedSize.Enabled = true;
                videoSize.Enabled = false;
				this.projectedBitrateKBits.Enabled = false;
				this.isBitrateMode = true;
				this.sizeSelection.Enabled = true;
			}
		}
		#endregion

        private void fileSizeBar1_Load(object sender, EventArgs e)
        {

        }


    }
    public class AutoEncodeTool : ITool
    {
        #region ITool Members

        public string Name
        {
            get { return "AutoEncode"; }
        }

        public void Run(MainForm info)
        {
            // normal video verification
            string error = null;
            // update the current audio stream with the latest data
            //            updateAudioStreams();
            if ((error = info.Video.verifyVideoSettings()) != null)
            {
                MessageBox.Show(error, "Unsupported video configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if ((error = info.Audio.verifyAudioSettings()) != null && !error.Equals("No audio input defined."))            {
                MessageBox.Show(error, "Unsupported audio configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
#warning must be fixed up to be more generic
            if (info.Video.CurrentVideoCodecSettings.EncodingMode == 2 || info.Video.CurrentVideoCodecSettings.EncodingMode == 5)
            {
                MessageBox.Show("First pass encoding is not supported for automated encoding as no output is generated.\nPlease choose another encoding mode", "Improper configuration",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            VideoCodecSettings vSettings = info.Video.CurrentVideoCodecSettings.clone();
            bool cont = info.JobUtil.getFinalZoneConfiguration(vSettings, info.Video.Info.IntroEndFrame, info.Video.Info.CreditsStartFrame);
            if (cont)
            {
                ulong length = 0;
                double framerate = 0.0;
                VideoStream myVideo = new VideoStream();
                info.JobUtil.getInputProperties(out length, out framerate, info.Video.VideoInput);
                myVideo.Input = info.Video.Info.VideoInput;
                myVideo.Output = info.Video.Info.VideoOutput;
                myVideo.NumberOfFrames = length;
                myVideo.Framerate = framerate;
                myVideo.DAR = info.Video.Info.DAR;
                myVideo.VideoType = info.Video.CurrentMuxableVideoType;
                myVideo.Settings = vSettings;
#warning check delays here. Doom9 did them, but I'm not sure how they work
                foreach (AudioStream aStream in info.Audio.AudioStreams)
                {
                    if (aStream.Delay != 0)
                    {
                        aStream.settings.DelayEnabled = true;
                        aStream.settings.Delay = aStream.Delay;
                    }
                }
                info.Audio.AudioStreams[0].Delay = 0;
                info.Audio.AudioStreams[1].Delay = 0;
                AutoEncodeWindow aew = new AutoEncodeWindow(myVideo, info.Audio.AudioStreams, info, info.Video.PrerenderJob);
                if (aew.init())
                    aew.ShowDialog();
                else
                    MessageBox.Show("The currently selected combination of video and audio output cannot be muxed", "Unsupported configuration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }           
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.Ctrl3 }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "AutoEncode"; }
        }

        #endregion
    }

    public class KnownAudioType
    {
        public MuxableType muxable;
        public AudioEncoderType encoderType;
        public KnownAudioType(MuxableType m, AudioEncoderType e)
        {
            muxable = m;
            encoderType = e;
        }
    }
}