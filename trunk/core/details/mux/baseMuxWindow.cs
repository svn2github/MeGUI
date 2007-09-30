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

namespace MeGUI
{
    public struct SubStream
    {
        public string path, language, name;
        public int delay;
    }
    /// <summary>
    /// Summary description for Mux.
    /// </summary>
    public class baseMuxWindow : System.Windows.Forms.Form
    {
        #region variables
        protected int parX, parY;
        protected string audioFilter, videoInputFilter, subtitleFilter, chaptersFilter, outputFilter;
        protected SubStream[] audioStreams;
        protected SubStream[] subtitleStreams;
        private MainForm mainForm;
        private MeGUISettings settings;
        private int lastSubtitle = 0, lastAudioTrack = 0;
        private Dictionary<string, string> languages;
        protected bool[] preconfigured;
        protected Button openSubtitleButton;
        protected TextBox subtitleInput;
        protected Label subtitleInputLabel;
        protected RadioButton subtitleTrack5;
        protected RadioButton subtitleTrack4;
        protected RadioButton subtitleTrack3;
        protected RadioButton subtitleTrack2;
        protected RadioButton subtitleTrack1;
        protected RadioButton audioTrack2;
        protected RadioButton audioTrack1;
        protected TextBox audioInput;
        protected Button audioInputOpenButton;
        protected TextBox videoInput;
        protected Button inputOpenButton;
        protected Label videoInputLabel;
        protected TextBox chaptersInput;
        protected Button openChaptersButton;
        protected OpenFileDialog openFileDialog;
        protected Label audioLanguageLabel;
        protected ComboBox audioLanguage;
        protected Label subtitleLanguageLabel;
        protected ComboBox subtitleLanguage;
        protected Button muxButton;
        protected Button outputButton;
        protected Label MuxFPSLabel;
        protected ComboBox muxFPS;
        protected TextBox muxedOutput;
        protected Label audioInputLabel;
        protected Label chaptersInputLabel;
        protected Label muxedOutputLabel;
        protected CheckBox enableSplit;
        protected TextBox splitSize;
        protected Label mbLabel;
        protected Button removeAudioTrackButton;
        protected Button removeSubtitleTrack;
        protected Button cancelButton;
        protected SaveFileDialog saveFileDialog;
        protected GroupBox videoGroupbox;
        protected GroupBox outputGroupbox;
        protected GroupBox audioGroupbox;
        protected GroupBox subtitleGroupbox;
        protected GroupBox chaptersGroupbox;
        protected TextBox audioName;
        protected TextBox videoName;
        protected Label videoNameLabel;
        protected Label audioNameLabel;
        protected NumericUpDown audioDelay;
        protected Label delayLabel;
        protected TextBox subName;
        protected Label SubNamelabel;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        #endregion
        #region start/stop
        public baseMuxWindow()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            audioStreams = new SubStream[2];
            audioStreams[0].path = "";
            audioStreams[0].language = "";
            audioStreams[0].name = "";
            audioStreams[1].path = "";
            audioStreams[1].language = "";
            audioStreams[1].name = "";
            subtitleStreams = new SubStream[5];
            subtitleStreams[0].path = "";
            subtitleStreams[0].language = "";
            subtitleStreams[1].path = "";
            subtitleStreams[1].language = "";
            subtitleStreams[2].path = "";
            subtitleStreams[2].language = "";
            subtitleStreams[3].path = "";
            subtitleStreams[3].language = "";
            subtitleStreams[4].path = "";
            subtitleStreams[4].language = "";
            this.languages = LanguageSelectionContainer.Languages;
            subtitleLanguage.DataSource = audioLanguage.DataSource = new List<string>(this.languages.Keys);
            audioLanguage.BindingContext = new BindingContext();
            subtitleLanguage.BindingContext = new BindingContext();
            audioLanguage.SelectedItem = "English";
            preconfigured = new bool[] { false, false };
            this.muxFPS.Items.AddRange(new object[] { 23.976, 24.0, 25.0, 29.97, 30.0, 50, 59.94, 60.0 });
        }
        public baseMuxWindow(MainForm mainForm)
            : this()
        {
            this.mainForm = mainForm;
            this.settings = mainForm.Settings;
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion
        #region config
        /// <summary>
        /// sets the configuration of the GUI
        /// used when a job is loaded (jobs have everything already filled out)
        /// </summary>
        /// <param name="videoInput">the video input (raw or mp4)</param>
        /// <param name="framerate">framerate of the input</param>
        /// <param name="audioStreams">the audiostreams</param>
        /// <param name="subtitleStreams">the subtitle streams</param>
        /// <param name="output">name of the output</param>
        /// <param name="splitSize">split size of the output</param>
        public void setConfig(string videoInput, double framerate, SubStream[] audioStreams, SubStream[] subtitleStreams, string chapterFile, string output, int splitSize, int parX, int parY)
        {
            this.parX = parX;
            this.parY = parY;
            this.videoInput.Text = videoInput;
            int fpsIndex = muxFPS.Items.IndexOf(framerate);
            if (fpsIndex != -1)
                muxFPS.SelectedIndex = fpsIndex;
            int index = 0;
            foreach (SubStream stream in audioStreams)
            {
                string lang = LanguageSelectionContainer.lookupISOCode(stream.language);
                this.audioStreams[index] = stream;
                if (lang != null)
                    this.audioStreams[index].language = lang;
                index++;
            }
            if (audioStreams.Length > 0) // set GUI elements
            {
                audioInput.Text = this.audioStreams[0].path;
                audioLanguage.SelectedIndex = audioLanguage.Items.IndexOf(this.audioStreams[0].language);
                audioDelay.Value = this.audioStreams[0].delay;
            }
            index = 0;
            foreach (SubStream stream in subtitleStreams)
            {
                string lang = LanguageSelectionContainer.lookupISOCode(stream.language);
                this.subtitleStreams[index] = stream;
                if (lang != null)
                    this.subtitleStreams[index].language = lang;
                index++;
            }
            if (subtitleStreams.Length > 0)
            {
                subtitleInput.Text = this.subtitleStreams[0].path;
                subtitleLanguage.SelectedIndex = subtitleLanguage.Items.IndexOf(this.subtitleStreams[0].language);
            }
            chaptersInput.Text = chapterFile;
            muxedOutput.Text = output;
            if (splitSize > 0)
            {
                enableSplit.Checked = true;
                int size = splitSize / 1024;
                this.splitSize.Text = size.ToString();
            }
            this.muxButton.Text = "Update";
            checkIO();
        }
        /// <summary>
        /// gets the additionally configured stream configuration from this window
        /// this method is used when the muxwindow is created from the AutoEncodeWindow in order to configure audio languages
        /// add subtitles and chapters
        /// </summary>
        /// <param name="aStreams">the configured audio streams(language assignments)</param>
        /// <param name="sStreams">the newly added subtitle streams</param>
        /// <param name="chapterFile">the assigned chapter file</param>
        public void getAdditionalStreams(out SubStream[] aStreams, out SubStream[] sStreams, out string chapterFile)
        {
            convertLanguagesToISO();
            aStreams = this.audioStreams;
            sStreams = this.subtitleStreams;
            chapterFile = chaptersInput.Text;
        }
        #endregion
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.muxButton = new System.Windows.Forms.Button();
            this.subtitleGroupbox = new System.Windows.Forms.GroupBox();
            this.removeSubtitleTrack = new System.Windows.Forms.Button();
            this.subtitleLanguage = new System.Windows.Forms.ComboBox();
            this.subtitleLanguageLabel = new System.Windows.Forms.Label();
            this.openSubtitleButton = new System.Windows.Forms.Button();
            this.subtitleInput = new System.Windows.Forms.TextBox();
            this.subtitleInputLabel = new System.Windows.Forms.Label();
            this.subtitleTrack5 = new System.Windows.Forms.RadioButton();
            this.subtitleTrack4 = new System.Windows.Forms.RadioButton();
            this.subtitleTrack3 = new System.Windows.Forms.RadioButton();
            this.subtitleTrack2 = new System.Windows.Forms.RadioButton();
            this.subtitleTrack1 = new System.Windows.Forms.RadioButton();
            this.audioGroupbox = new System.Windows.Forms.GroupBox();
            this.delayLabel = new System.Windows.Forms.Label();
            this.audioDelay = new System.Windows.Forms.NumericUpDown();
            this.audioName = new System.Windows.Forms.TextBox();
            this.audioNameLabel = new System.Windows.Forms.Label();
            this.removeAudioTrackButton = new System.Windows.Forms.Button();
            this.audioLanguage = new System.Windows.Forms.ComboBox();
            this.audioLanguageLabel = new System.Windows.Forms.Label();
            this.audioTrack2 = new System.Windows.Forms.RadioButton();
            this.audioTrack1 = new System.Windows.Forms.RadioButton();
            this.audioInput = new System.Windows.Forms.TextBox();
            this.audioInputOpenButton = new System.Windows.Forms.Button();
            this.audioInputLabel = new System.Windows.Forms.Label();
            this.videoGroupbox = new System.Windows.Forms.GroupBox();
            this.videoName = new System.Windows.Forms.TextBox();
            this.videoNameLabel = new System.Windows.Forms.Label();
            this.MuxFPSLabel = new System.Windows.Forms.Label();
            this.muxFPS = new System.Windows.Forms.ComboBox();
            this.videoInput = new System.Windows.Forms.TextBox();
            this.inputOpenButton = new System.Windows.Forms.Button();
            this.videoInputLabel = new System.Windows.Forms.Label();
            this.chaptersGroupbox = new System.Windows.Forms.GroupBox();
            this.chaptersInputLabel = new System.Windows.Forms.Label();
            this.chaptersInput = new System.Windows.Forms.TextBox();
            this.openChaptersButton = new System.Windows.Forms.Button();
            this.outputGroupbox = new System.Windows.Forms.GroupBox();
            this.mbLabel = new System.Windows.Forms.Label();
            this.splitSize = new System.Windows.Forms.TextBox();
            this.enableSplit = new System.Windows.Forms.CheckBox();
            this.muxedOutputLabel = new System.Windows.Forms.Label();
            this.muxedOutput = new System.Windows.Forms.TextBox();
            this.outputButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.subName = new System.Windows.Forms.TextBox();
            this.SubNamelabel = new System.Windows.Forms.Label();
            this.subtitleGroupbox.SuspendLayout();
            this.audioGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.audioDelay)).BeginInit();
            this.videoGroupbox.SuspendLayout();
            this.chaptersGroupbox.SuspendLayout();
            this.outputGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // muxButton
            // 
            this.muxButton.Location = new System.Drawing.Point(291, 430);
            this.muxButton.Name = "muxButton";
            this.muxButton.Size = new System.Drawing.Size(56, 23);
            this.muxButton.TabIndex = 26;
            this.muxButton.Text = "Queue";
            this.muxButton.Click += new System.EventHandler(this.muxButton_Click);
            // 
            // subtitleGroupbox
            // 
            this.subtitleGroupbox.Controls.Add(this.subName);
            this.subtitleGroupbox.Controls.Add(this.SubNamelabel);
            this.subtitleGroupbox.Controls.Add(this.removeSubtitleTrack);
            this.subtitleGroupbox.Controls.Add(this.subtitleLanguage);
            this.subtitleGroupbox.Controls.Add(this.subtitleLanguageLabel);
            this.subtitleGroupbox.Controls.Add(this.openSubtitleButton);
            this.subtitleGroupbox.Controls.Add(this.subtitleInput);
            this.subtitleGroupbox.Controls.Add(this.subtitleInputLabel);
            this.subtitleGroupbox.Controls.Add(this.subtitleTrack5);
            this.subtitleGroupbox.Controls.Add(this.subtitleTrack4);
            this.subtitleGroupbox.Controls.Add(this.subtitleTrack3);
            this.subtitleGroupbox.Controls.Add(this.subtitleTrack2);
            this.subtitleGroupbox.Controls.Add(this.subtitleTrack1);
            this.subtitleGroupbox.Location = new System.Drawing.Point(8, 204);
            this.subtitleGroupbox.Name = "subtitleGroupbox";
            this.subtitleGroupbox.Size = new System.Drawing.Size(424, 80);
            this.subtitleGroupbox.TabIndex = 24;
            this.subtitleGroupbox.TabStop = false;
            this.subtitleGroupbox.Text = "Subtitles";
            // 
            // removeSubtitleTrack
            // 
            this.removeSubtitleTrack.Location = new System.Drawing.Point(382, 48);
            this.removeSubtitleTrack.Name = "removeSubtitleTrack";
            this.removeSubtitleTrack.Size = new System.Drawing.Size(24, 23);
            this.removeSubtitleTrack.TabIndex = 29;
            this.removeSubtitleTrack.Text = "X";
            this.removeSubtitleTrack.Click += new System.EventHandler(this.removeSubtitleTrack_Click);
            // 
            // subtitleLanguage
            // 
            this.subtitleLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.subtitleLanguage.Location = new System.Drawing.Point(120, 50);
            this.subtitleLanguage.Name = "subtitleLanguage";
            this.subtitleLanguage.Size = new System.Drawing.Size(121, 21);
            this.subtitleLanguage.Sorted = true;
            this.subtitleLanguage.TabIndex = 28;
            this.subtitleLanguage.SelectedIndexChanged += new System.EventHandler(this.subtitleLanguage_SelectedIndexChanged);
            // 
            // subtitleLanguageLabel
            // 
            this.subtitleLanguageLabel.Location = new System.Drawing.Point(16, 48);
            this.subtitleLanguageLabel.Name = "subtitleLanguageLabel";
            this.subtitleLanguageLabel.Size = new System.Drawing.Size(100, 23);
            this.subtitleLanguageLabel.TabIndex = 5;
            this.subtitleLanguageLabel.Text = "Language";
            // 
            // openSubtitleButton
            // 
            this.openSubtitleButton.Location = new System.Drawing.Point(382, 19);
            this.openSubtitleButton.Name = "openSubtitleButton";
            this.openSubtitleButton.Size = new System.Drawing.Size(24, 23);
            this.openSubtitleButton.TabIndex = 3;
            this.openSubtitleButton.Text = "...";
            this.openSubtitleButton.Click += new System.EventHandler(this.openSubtitleButton_Click);
            // 
            // subtitleInput
            // 
            this.subtitleInput.Location = new System.Drawing.Point(120, 20);
            this.subtitleInput.Name = "subtitleInput";
            this.subtitleInput.ReadOnly = true;
            this.subtitleInput.Size = new System.Drawing.Size(256, 21);
            this.subtitleInput.TabIndex = 2;
            // 
            // subtitleInputLabel
            // 
            this.subtitleInputLabel.Location = new System.Drawing.Point(16, 22);
            this.subtitleInputLabel.Name = "subtitleInputLabel";
            this.subtitleInputLabel.Size = new System.Drawing.Size(100, 16);
            this.subtitleInputLabel.TabIndex = 1;
            this.subtitleInputLabel.Text = "Subtitle File";
            // 
            // subtitleTrack5
            // 
            this.subtitleTrack5.Location = new System.Drawing.Point(163, 1);
            this.subtitleTrack5.Name = "subtitleTrack5";
            this.subtitleTrack5.Size = new System.Drawing.Size(27, 13);
            this.subtitleTrack5.TabIndex = 4;
            this.subtitleTrack5.Text = "5";
            this.subtitleTrack5.CheckedChanged += new System.EventHandler(this.subtitleTrack_CheckedChanged);
            // 
            // subtitleTrack4
            // 
            this.subtitleTrack4.Location = new System.Drawing.Point(136, 1);
            this.subtitleTrack4.Name = "subtitleTrack4";
            this.subtitleTrack4.Size = new System.Drawing.Size(27, 13);
            this.subtitleTrack4.TabIndex = 3;
            this.subtitleTrack4.Text = "4";
            this.subtitleTrack4.CheckedChanged += new System.EventHandler(this.subtitleTrack_CheckedChanged);
            // 
            // subtitleTrack3
            // 
            this.subtitleTrack3.Location = new System.Drawing.Point(109, 1);
            this.subtitleTrack3.Name = "subtitleTrack3";
            this.subtitleTrack3.Size = new System.Drawing.Size(27, 13);
            this.subtitleTrack3.TabIndex = 2;
            this.subtitleTrack3.Text = "3";
            this.subtitleTrack3.CheckedChanged += new System.EventHandler(this.subtitleTrack_CheckedChanged);
            // 
            // subtitleTrack2
            // 
            this.subtitleTrack2.Location = new System.Drawing.Point(82, 1);
            this.subtitleTrack2.Name = "subtitleTrack2";
            this.subtitleTrack2.Size = new System.Drawing.Size(27, 13);
            this.subtitleTrack2.TabIndex = 1;
            this.subtitleTrack2.Text = "2";
            this.subtitleTrack2.CheckedChanged += new System.EventHandler(this.subtitleTrack_CheckedChanged);
            // 
            // subtitleTrack1
            // 
            this.subtitleTrack1.Checked = true;
            this.subtitleTrack1.Location = new System.Drawing.Point(55, 1);
            this.subtitleTrack1.Name = "subtitleTrack1";
            this.subtitleTrack1.Size = new System.Drawing.Size(27, 13);
            this.subtitleTrack1.TabIndex = 0;
            this.subtitleTrack1.TabStop = true;
            this.subtitleTrack1.Text = "1";
            // 
            // audioGroupbox
            // 
            this.audioGroupbox.Controls.Add(this.delayLabel);
            this.audioGroupbox.Controls.Add(this.audioDelay);
            this.audioGroupbox.Controls.Add(this.audioName);
            this.audioGroupbox.Controls.Add(this.audioNameLabel);
            this.audioGroupbox.Controls.Add(this.removeAudioTrackButton);
            this.audioGroupbox.Controls.Add(this.audioLanguage);
            this.audioGroupbox.Controls.Add(this.audioLanguageLabel);
            this.audioGroupbox.Controls.Add(this.audioTrack2);
            this.audioGroupbox.Controls.Add(this.audioTrack1);
            this.audioGroupbox.Controls.Add(this.audioInput);
            this.audioGroupbox.Controls.Add(this.audioInputOpenButton);
            this.audioGroupbox.Controls.Add(this.audioInputLabel);
            this.audioGroupbox.Location = new System.Drawing.Point(8, 92);
            this.audioGroupbox.Name = "audioGroupbox";
            this.audioGroupbox.Size = new System.Drawing.Size(424, 106);
            this.audioGroupbox.TabIndex = 23;
            this.audioGroupbox.TabStop = false;
            this.audioGroupbox.Text = "Audio";
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(19, 76);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(34, 13);
            this.delayLabel.TabIndex = 32;
            this.delayLabel.Text = "Delay";
            // 
            // audioDelay
            // 
            this.audioDelay.Location = new System.Drawing.Point(120, 76);
            this.audioDelay.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.audioDelay.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
            this.audioDelay.Name = "audioDelay";
            this.audioDelay.Size = new System.Drawing.Size(78, 21);
            this.audioDelay.TabIndex = 31;
            this.audioDelay.ValueChanged += new System.EventHandler(this.audioDelay_ValueChanged);
            // 
            // audioName
            // 
            this.audioName.Location = new System.Drawing.Point(286, 49);
            this.audioName.MaxLength = 100;
            this.audioName.Name = "audioName";
            this.audioName.Size = new System.Drawing.Size(90, 21);
            this.audioName.TabIndex = 30;
            this.audioName.Leave += new System.EventHandler(this.audioName_Leave);
            // 
            // audioNameLabel
            // 
            this.audioNameLabel.AutoSize = true;
            this.audioNameLabel.Location = new System.Drawing.Point(245, 53);
            this.audioNameLabel.Name = "audioNameLabel";
            this.audioNameLabel.Size = new System.Drawing.Size(34, 13);
            this.audioNameLabel.TabIndex = 29;
            this.audioNameLabel.Text = "Name";
            // 
            // removeAudioTrackButton
            // 
            this.removeAudioTrackButton.Location = new System.Drawing.Point(382, 48);
            this.removeAudioTrackButton.Name = "removeAudioTrackButton";
            this.removeAudioTrackButton.Size = new System.Drawing.Size(24, 23);
            this.removeAudioTrackButton.TabIndex = 28;
            this.removeAudioTrackButton.Text = "X";
            this.removeAudioTrackButton.Click += new System.EventHandler(this.removeAudioTrackButton_Click);
            // 
            // audioLanguage
            // 
            this.audioLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioLanguage.Location = new System.Drawing.Point(120, 48);
            this.audioLanguage.Name = "audioLanguage";
            this.audioLanguage.Size = new System.Drawing.Size(121, 21);
            this.audioLanguage.Sorted = true;
            this.audioLanguage.TabIndex = 27;
            this.audioLanguage.SelectedIndexChanged += new System.EventHandler(this.audioLanguage_SelectedIndexChanged);
            // 
            // audioLanguageLabel
            // 
            this.audioLanguageLabel.Location = new System.Drawing.Point(16, 48);
            this.audioLanguageLabel.Name = "audioLanguageLabel";
            this.audioLanguageLabel.Size = new System.Drawing.Size(100, 23);
            this.audioLanguageLabel.TabIndex = 26;
            this.audioLanguageLabel.Text = "Language";
            // 
            // audioTrack2
            // 
            this.audioTrack2.Location = new System.Drawing.Point(67, 1);
            this.audioTrack2.Name = "audioTrack2";
            this.audioTrack2.Size = new System.Drawing.Size(27, 13);
            this.audioTrack2.TabIndex = 25;
            this.audioTrack2.Text = "2";
            this.audioTrack2.Click += new System.EventHandler(this.audioTrack_CheckedChanged);
            // 
            // audioTrack1
            // 
            this.audioTrack1.Checked = true;
            this.audioTrack1.Location = new System.Drawing.Point(40, 1);
            this.audioTrack1.Name = "audioTrack1";
            this.audioTrack1.Size = new System.Drawing.Size(27, 13);
            this.audioTrack1.TabIndex = 24;
            this.audioTrack1.TabStop = true;
            this.audioTrack1.Text = "1";
            this.audioTrack1.Click += new System.EventHandler(this.audioTrack_CheckedChanged);
            // 
            // audioInput
            // 
            this.audioInput.Location = new System.Drawing.Point(120, 21);
            this.audioInput.Name = "audioInput";
            this.audioInput.ReadOnly = true;
            this.audioInput.Size = new System.Drawing.Size(256, 21);
            this.audioInput.TabIndex = 17;
            // 
            // audioInputOpenButton
            // 
            this.audioInputOpenButton.Location = new System.Drawing.Point(382, 19);
            this.audioInputOpenButton.Name = "audioInputOpenButton";
            this.audioInputOpenButton.Size = new System.Drawing.Size(24, 23);
            this.audioInputOpenButton.TabIndex = 18;
            this.audioInputOpenButton.Text = "...";
            this.audioInputOpenButton.Click += new System.EventHandler(this.audioInputOpenButton_Click);
            // 
            // audioInputLabel
            // 
            this.audioInputLabel.Location = new System.Drawing.Point(16, 22);
            this.audioInputLabel.Name = "audioInputLabel";
            this.audioInputLabel.Size = new System.Drawing.Size(100, 16);
            this.audioInputLabel.TabIndex = 16;
            this.audioInputLabel.Text = "Audio Input";
            // 
            // videoGroupbox
            // 
            this.videoGroupbox.Controls.Add(this.videoName);
            this.videoGroupbox.Controls.Add(this.videoNameLabel);
            this.videoGroupbox.Controls.Add(this.MuxFPSLabel);
            this.videoGroupbox.Controls.Add(this.muxFPS);
            this.videoGroupbox.Controls.Add(this.videoInput);
            this.videoGroupbox.Controls.Add(this.inputOpenButton);
            this.videoGroupbox.Controls.Add(this.videoInputLabel);
            this.videoGroupbox.Location = new System.Drawing.Point(8, 7);
            this.videoGroupbox.Name = "videoGroupbox";
            this.videoGroupbox.Size = new System.Drawing.Size(424, 80);
            this.videoGroupbox.TabIndex = 22;
            this.videoGroupbox.TabStop = false;
            this.videoGroupbox.Text = "Video";
            // 
            // videoName
            // 
            this.videoName.Location = new System.Drawing.Point(283, 52);
            this.videoName.MaxLength = 100;
            this.videoName.Name = "videoName";
            this.videoName.Size = new System.Drawing.Size(90, 21);
            this.videoName.TabIndex = 34;
            // 
            // videoNameLabel
            // 
            this.videoNameLabel.AutoSize = true;
            this.videoNameLabel.Location = new System.Drawing.Point(243, 55);
            this.videoNameLabel.Name = "videoNameLabel";
            this.videoNameLabel.Size = new System.Drawing.Size(34, 13);
            this.videoNameLabel.TabIndex = 33;
            this.videoNameLabel.Text = "Name";
            // 
            // MuxFPSLabel
            // 
            this.MuxFPSLabel.Location = new System.Drawing.Point(16, 54);
            this.MuxFPSLabel.Name = "MuxFPSLabel";
            this.MuxFPSLabel.Size = new System.Drawing.Size(40, 16);
            this.MuxFPSLabel.TabIndex = 32;
            this.MuxFPSLabel.Text = "FPS";
            // 
            // muxFPS
            // 
            this.muxFPS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.muxFPS.Location = new System.Drawing.Point(118, 52);
            this.muxFPS.Name = "muxFPS";
            this.muxFPS.Size = new System.Drawing.Size(80, 21);
            this.muxFPS.TabIndex = 31;
            this.muxFPS.SelectedIndexChanged += new System.EventHandler(this.muxFPS_SelectedIndexChanged);
            // 
            // videoInput
            // 
            this.videoInput.Location = new System.Drawing.Point(118, 16);
            this.videoInput.Name = "videoInput";
            this.videoInput.ReadOnly = true;
            this.videoInput.Size = new System.Drawing.Size(256, 21);
            this.videoInput.TabIndex = 3;
            // 
            // inputOpenButton
            // 
            this.inputOpenButton.Location = new System.Drawing.Point(382, 15);
            this.inputOpenButton.Name = "inputOpenButton";
            this.inputOpenButton.Size = new System.Drawing.Size(24, 23);
            this.inputOpenButton.TabIndex = 4;
            this.inputOpenButton.Text = "...";
            this.inputOpenButton.Click += new System.EventHandler(this.inputOpenButton_Click);
            // 
            // videoInputLabel
            // 
            this.videoInputLabel.Location = new System.Drawing.Point(16, 20);
            this.videoInputLabel.Name = "videoInputLabel";
            this.videoInputLabel.Size = new System.Drawing.Size(100, 13);
            this.videoInputLabel.TabIndex = 0;
            this.videoInputLabel.Text = "Video Input";
            // 
            // chaptersGroupbox
            // 
            this.chaptersGroupbox.Controls.Add(this.chaptersInputLabel);
            this.chaptersGroupbox.Controls.Add(this.chaptersInput);
            this.chaptersGroupbox.Controls.Add(this.openChaptersButton);
            this.chaptersGroupbox.Location = new System.Drawing.Point(8, 290);
            this.chaptersGroupbox.Name = "chaptersGroupbox";
            this.chaptersGroupbox.Size = new System.Drawing.Size(424, 48);
            this.chaptersGroupbox.TabIndex = 25;
            this.chaptersGroupbox.TabStop = false;
            this.chaptersGroupbox.Text = "Chapter";
            // 
            // chaptersInputLabel
            // 
            this.chaptersInputLabel.Location = new System.Drawing.Point(16, 22);
            this.chaptersInputLabel.Name = "chaptersInputLabel";
            this.chaptersInputLabel.Size = new System.Drawing.Size(100, 16);
            this.chaptersInputLabel.TabIndex = 17;
            this.chaptersInputLabel.Text = "Chapters File";
            // 
            // chaptersInput
            // 
            this.chaptersInput.Location = new System.Drawing.Point(120, 19);
            this.chaptersInput.Name = "chaptersInput";
            this.chaptersInput.ReadOnly = true;
            this.chaptersInput.Size = new System.Drawing.Size(256, 21);
            this.chaptersInput.TabIndex = 18;
            // 
            // openChaptersButton
            // 
            this.openChaptersButton.Location = new System.Drawing.Point(382, 19);
            this.openChaptersButton.Name = "openChaptersButton";
            this.openChaptersButton.Size = new System.Drawing.Size(24, 23);
            this.openChaptersButton.TabIndex = 16;
            this.openChaptersButton.Text = "...";
            this.openChaptersButton.Click += new System.EventHandler(this.openChaptersButton_Click);
            // 
            // outputGroupbox
            // 
            this.outputGroupbox.Controls.Add(this.mbLabel);
            this.outputGroupbox.Controls.Add(this.splitSize);
            this.outputGroupbox.Controls.Add(this.enableSplit);
            this.outputGroupbox.Controls.Add(this.muxedOutputLabel);
            this.outputGroupbox.Controls.Add(this.muxedOutput);
            this.outputGroupbox.Controls.Add(this.outputButton);
            this.outputGroupbox.Location = new System.Drawing.Point(8, 344);
            this.outputGroupbox.Name = "outputGroupbox";
            this.outputGroupbox.Size = new System.Drawing.Size(424, 80);
            this.outputGroupbox.TabIndex = 28;
            this.outputGroupbox.TabStop = false;
            this.outputGroupbox.Text = "Output";
            // 
            // mbLabel
            // 
            this.mbLabel.Location = new System.Drawing.Point(182, 53);
            this.mbLabel.Name = "mbLabel";
            this.mbLabel.Size = new System.Drawing.Size(40, 16);
            this.mbLabel.TabIndex = 21;
            this.mbLabel.Text = "MB";
            // 
            // splitSize
            // 
            this.splitSize.Enabled = false;
            this.splitSize.Location = new System.Drawing.Point(120, 51);
            this.splitSize.Name = "splitSize";
            this.splitSize.Size = new System.Drawing.Size(56, 21);
            this.splitSize.TabIndex = 20;
            this.splitSize.Text = "0";
            this.splitSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.splitSize_KeyPress);
            // 
            // enableSplit
            // 
            this.enableSplit.Location = new System.Drawing.Point(14, 49);
            this.enableSplit.Name = "enableSplit";
            this.enableSplit.Size = new System.Drawing.Size(104, 24);
            this.enableSplit.TabIndex = 19;
            this.enableSplit.Text = "Split output";
            this.enableSplit.CheckedChanged += new System.EventHandler(this.enableSplit_CheckedChanged);
            // 
            // muxedOutputLabel
            // 
            this.muxedOutputLabel.Location = new System.Drawing.Point(14, 23);
            this.muxedOutputLabel.Name = "muxedOutputLabel";
            this.muxedOutputLabel.Size = new System.Drawing.Size(100, 16);
            this.muxedOutputLabel.TabIndex = 17;
            this.muxedOutputLabel.Text = "Muxed Output";
            // 
            // muxedOutput
            // 
            this.muxedOutput.Location = new System.Drawing.Point(120, 20);
            this.muxedOutput.Name = "muxedOutput";
            this.muxedOutput.Size = new System.Drawing.Size(256, 21);
            this.muxedOutput.TabIndex = 18;
            // 
            // outputButton
            // 
            this.outputButton.Location = new System.Drawing.Point(380, 20);
            this.outputButton.Name = "outputButton";
            this.outputButton.Size = new System.Drawing.Size(24, 23);
            this.outputButton.TabIndex = 16;
            this.outputButton.Text = "...";
            this.outputButton.Click += new System.EventHandler(this.outputButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(376, 430);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(56, 23);
            this.cancelButton.TabIndex = 29;
            this.cancelButton.Text = "Cancel";
            // 
            // subName
            // 
            this.subName.Location = new System.Drawing.Point(286, 50);
            this.subName.MaxLength = 100;
            this.subName.Name = "subName";
            this.subName.Size = new System.Drawing.Size(90, 21);
            this.subName.TabIndex = 32;
            this.subName.Leave += new System.EventHandler(this.subName_Leave);
            // 
            // SubNamelabel
            // 
            this.SubNamelabel.AutoSize = true;
            this.SubNamelabel.Location = new System.Drawing.Point(245, 54);
            this.SubNamelabel.Name = "SubNamelabel";
            this.SubNamelabel.Size = new System.Drawing.Size(34, 13);
            this.SubNamelabel.TabIndex = 31;
            this.SubNamelabel.Text = "Name";
            // 
            // baseMuxWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(444, 460);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.outputGroupbox);
            this.Controls.Add(this.muxButton);
            this.Controls.Add(this.subtitleGroupbox);
            this.Controls.Add(this.audioGroupbox);
            this.Controls.Add(this.videoGroupbox);
            this.Controls.Add(this.chaptersGroupbox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "baseMuxWindow";
            this.ShowInTaskbar = false;
            this.Text = "Mux";
            this.TopMost = true;
            this.subtitleGroupbox.ResumeLayout(false);
            this.subtitleGroupbox.PerformLayout();
            this.audioGroupbox.ResumeLayout(false);
            this.audioGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.audioDelay)).EndInit();
            this.videoGroupbox.ResumeLayout(false);
            this.videoGroupbox.PerformLayout();
            this.chaptersGroupbox.ResumeLayout(false);
            this.chaptersGroupbox.PerformLayout();
            this.outputGroupbox.ResumeLayout(false);
            this.outputGroupbox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
        #region helper method
        protected virtual void checkIO()
        {
            if (videoInput.Text.Equals(""))
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else if (muxedOutput.Text.Equals(""))
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else if (muxFPS.SelectedIndex == -1 && isFPSRequired())
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else
                muxButton.DialogResult = DialogResult.OK;
        }

        private void splitSize_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
                e.Handled = true;
        }
        #endregion
        protected virtual bool isFPSRequired()
        {
            try
            {
                if (videoInput.Text.Length > 0)
                    return (VideoUtil.guessVideoType(videoInput.Text).ContainerType == null);
                return true;
            }
            catch (NullReferenceException) // This will throw if it can't guess the video type
            {
                return true;
            }
        }

        #region button event handlers
        private void inputOpenButton_Click(object sender, System.EventArgs e)
        {
            openFileDialog.Filter = VideoInputFilter;
            openFileDialog.Title = "Select your video file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                videoInput.Text = openFileDialog.FileName;
                if (Path.GetExtension(openFileDialog.FileName).ToLower().Equals(".mp4")) // for mp4 input we don't need to specify a framerate
                {
                    muxFPS.SelectedIndex = -1;
                    muxFPS.Enabled = false;
                }
                else
                {
                    muxFPS.Enabled = true;
                }
                checkIO();
                fileUpdated();
                if (string.IsNullOrEmpty(muxedOutput.Text))
                    chooseOutputFilename();
            }
        }

        private void chooseOutputFilename()
        {
            string file = videoInput.Text;
            muxedOutput.Text = Path.Combine(Path.GetDirectoryName(file),
                Path.GetFileNameWithoutExtension(file) + "-muxed" + Path.GetExtension(file));
        }

        private void audioInputOpenButton_Click(object sender, System.EventArgs e)
        {
            openFileDialog.Filter = AudioFilter;
            openFileDialog.Title = "Select your audio file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                int index = this.getSelectedAudioTrack();
                audioInput.Text = openFileDialog.FileName;
                audioStreams[index].path = openFileDialog.FileName;
                audioStreams[index].language = audioLanguage.Text;
                audioStreams[index].delay = AudioEncodingComponent.getDelay(openFileDialog.FileName);
                audioDelay.Value = audioStreams[index].delay;
                fileUpdated();
            }
        }

        private void openSubtitleButton_Click(object sender, System.EventArgs e)
        {
            openFileDialog.Filter = SubtitleFilter;
            openFileDialog.Title = "Select your subtitle";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                subtitleInput.Text = openFileDialog.FileName;
                int index = this.getSelectedSubTitle();
                subtitleStreams[index].path = openFileDialog.FileName;
                subtitleStreams[index].language = subtitleLanguage.Text;
                fileUpdated();
            }
        }

        private void openChaptersButton_Click(object sender, System.EventArgs e)
        {
            openFileDialog.Filter = "Chapter files|*.txt";
            openFileDialog.Title = "Select a chapter file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                chaptersInput.Text = openFileDialog.FileName;
                fileUpdated();
            }
        }

        private void outputButton_Click(object sender, System.EventArgs e)
        {
            saveFileDialog.Filter = OutputFilter;
            saveFileDialog.Title = "Select your output";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                muxedOutput.Text = saveFileDialog.FileName;
                checkIO();
                fileUpdated();
            }
        }

        protected virtual void muxButton_Click(object sender, System.EventArgs e)
        {
            if (muxButton.DialogResult != DialogResult.OK)
            {
                if (videoInput.Text.Equals(""))
                {
                    MessageBox.Show("You must configure a video input file", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else if (muxedOutput.Text.Equals(""))
                {
                    MessageBox.Show("You must configure an output file", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else if (muxFPS.SelectedIndex == -1 && isFPSRequired())
                {
                    MessageBox.Show("You must select a framerate", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
        }
        #endregion
        #region job generation
        /// <summary>
        /// goes through all audio and subtitle tracks, and discards the improperly configured ones
        /// in the same process, the language strings are converted to ISO strings that mp4box understands
        /// </summary>
        protected void convertLanguagesToISO()
        {
            ArrayList tracks = new ArrayList();
            foreach (SubStream stream in this.audioStreams)
            {
                if (!stream.path.Equals("")) // get all configured audio tracks
                    tracks.Add(stream);
            }
            this.audioStreams = new SubStream[tracks.Count];
            int index = 0;
            foreach (object o in tracks)
            {
                audioStreams[index] = (SubStream)o;
                object lang = languages[audioStreams[index].language];
                if (lang != null)
                    audioStreams[index].language = (string)lang;
                index++;
            }
            index = 0;
            tracks = new ArrayList();
            foreach (SubStream stream in this.subtitleStreams)
            {
                if (!stream.path.Equals("")) // get all configured audio tracks
                    tracks.Add(stream);
            }
            this.subtitleStreams = new SubStream[tracks.Count];
            foreach (object o in tracks)
            {
                subtitleStreams[index] = (SubStream)o;
                object lang = languages[subtitleStreams[index].language];
                if (lang != null)
                    subtitleStreams[index].language = (string)lang;
                index++;
            }
            tracks = null;
        }
        /// <summary>
        /// generates the settings and muxjob based on the GUI settings
        /// each non empty audio stream and subtitle stream is added
        /// 
        /// </summary>
        /// <returns></returns>
        #endregion
        #region radiobuttons
        private void audioTrack_CheckedChanged(object sender, System.EventArgs e)
        {
            int currentAudioTrack = this.getSelectedAudioTrack();
            this.audioStreams[lastAudioTrack].path = audioInput.Text;
            this.audioStreams[lastAudioTrack].language = audioLanguage.Text;
            this.audioStreams[lastAudioTrack].name = audioName.Text;
            this.audioStreams[lastAudioTrack].delay = (int)audioDelay.Value;
            audioInput.Text = audioStreams[currentAudioTrack].path;
            audioName.Text = audioStreams[currentAudioTrack].name;
            if (preconfigured[currentAudioTrack])
            {
                audioInputOpenButton.Enabled = false;
                removeAudioTrackButton.Enabled = false;
                audioDelay.Enabled = false;
                audioDelay.Value = 0;
            }
            else
            {
                audioInputOpenButton.Enabled = true;
                removeAudioTrackButton.Enabled = true;
                audioDelay.Enabled = true;
                audioDelay.Value = audioStreams[currentAudioTrack].delay;
            }
            if (!audioStreams[currentAudioTrack].language.Equals(""))
            {
                int ind = audioLanguage.Items.IndexOf(audioStreams[currentAudioTrack].language);
                audioLanguage.SelectedIndex = ind;
            }
            else
                audioLanguage.SelectedIndex = -1;
            this.lastAudioTrack = currentAudioTrack;
        }
        /// <summary>
        /// gets the currently selected audio track
        /// </summary>
        /// <returns>0 based index of the currently selected audio track</returns>
        private int getSelectedAudioTrack()
        {
            if (audioTrack1.Checked)
                return 0;
            else
                return 1;
        }
        /// <summary>
        /// gets the index of the currently selected subtitle
        /// </summary>
        /// <returns>0 based index of the currently selected subtitle</returns>
        private int getSelectedSubTitle()
        {
            if (subtitleTrack1.Checked)
                return 0;
            else if (subtitleTrack2.Checked)
                return 1;
            else if (subtitleTrack3.Checked)
                return 2;
            else if (subtitleTrack4.Checked)
                return 3;
            else
                return 4;
        }
        private void subtitleTrack_CheckedChanged(object sender, System.EventArgs e)
        {
            subtitleStreams[lastSubtitle].path = subtitleInput.Text;
            subtitleStreams[lastSubtitle].language = subtitleLanguage.Text;
            int index = this.getSelectedSubTitle();
            subtitleInput.Text = subtitleStreams[index].path;
            if (!subtitleStreams[index].language.Equals(""))
            {
                int ind = audioLanguage.Items.IndexOf(subtitleStreams[index].language);
                subtitleLanguage.SelectedIndex = ind;
            }
            else
                subtitleLanguage.SelectedIndex = -1;
            this.lastSubtitle = index;
        }
        #endregion
        #region language dropdowns
        private void audioLanguage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (audioLanguage.SelectedIndex != -1)
            {
                int index = this.getSelectedAudioTrack();
                audioStreams[index].language = audioLanguage.Text;
            }
        }
        private void subtitleLanguage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (subtitleLanguage.SelectedIndex != -1)
            {
                int index = this.getSelectedSubTitle();
                subtitleStreams[index].language = subtitleLanguage.Text;
            }
        }
        #endregion
        #region track removal
        private void removeAudioTrackButton_Click(object sender, System.EventArgs e)
        {
            audioInput.Text = "";
            int index = this.getSelectedAudioTrack();
            audioStreams[index].path = "";
            audioStreams[index].language = "";
            audioStreams[index].name = "";
            this.audioLanguage.SelectedIndex = -1;
            fileUpdated();
        }

        private void removeSubtitleTrack_Click(object sender, System.EventArgs e)
        {
            subtitleInput.Text = "";
            int index = this.getSelectedSubTitle();
            subtitleStreams[index].path = "";
            subtitleStreams[index].language = "";
            subtitleLanguage.SelectedIndex = -1;
            fileUpdated();
        }
        #endregion
        #region other events
        private void muxFPS_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            checkIO();
        }

        private void enableSplit_CheckedChanged(object sender, System.EventArgs e)
        {
            if (enableSplit.Checked)
                splitSize.Enabled = true;
            else
                splitSize.Enabled = false;
        }

        private void audioName_Leave(object sender, EventArgs e)
        {
            int index = this.getSelectedAudioTrack();
            audioStreams[index].name = audioName.Text;
        }
        #endregion
        #region properties
        public virtual string AudioFilter
        {
            get { return audioFilter; }
        }
        public virtual string VideoInputFilter
        {
            get { return videoInputFilter; }
        }
        public virtual string SubtitleFilter
        {
            get { return subtitleFilter; }
        }
        public virtual string ChaptersFilter
        {
            get { return chaptersFilter; }
        }
        public virtual string OutputFilter
        {
            get { return outputFilter; }
        }
        #endregion
        protected virtual void fileUpdated() { }

        private void audioDelay_ValueChanged(object sender, EventArgs e)
        {
            audioStreams[getSelectedAudioTrack()].delay = (int)audioDelay.Value;
        }

        private void subName_Leave(object sender, EventArgs e)
        {
            int index = this.getSelectedSubTitle();
            subtitleStreams[index].name = subName.Text;
        }
    }
}