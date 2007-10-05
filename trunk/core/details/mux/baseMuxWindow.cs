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
using MeGUI.core.util;
using MeGUI.core.details;
using MeGUI.core.details.mux;

namespace MeGUI
{
    /// <summary>
    /// Summary description for Mux.
    /// </summary>
    public class baseMuxWindow : System.Windows.Forms.Form
    {
        protected List<MuxStreamControl> audioTracks;
        protected List<MuxStreamControl> subtitleTracks;


        #region variables
        //protected int parX, parY;
        protected Dar? dar;
        protected string audioFilter, videoInputFilter, subtitleFilter, chaptersFilter, outputFilter;
        private MainForm mainForm;
        private MeGUISettings settings;
        private int lastSubtitle = 0, lastAudioTrack = 0;
        protected Label videoInputLabel;
        protected OpenFileDialog openFileDialog;
        protected Button muxButton;
        protected Label MuxFPSLabel;
        protected Label chaptersInputLabel;
        protected Label muxedOutputLabel;
        protected Button cancelButton;
        protected SaveFileDialog saveFileDialog;
        protected GroupBox videoGroupbox;
        protected GroupBox outputGroupbox;
        protected GroupBox chaptersGroupbox;
        protected TextBox videoName;
        protected Label videoNameLabel;
        #endregion
        protected Panel audioPanel;

        protected TabControl audio;
        private TabPage audioPage1;
        private MeGUI.core.details.mux.MuxStreamControl muxStreamControl2;
        private ContextMenuStrip audioMenu;
        private ContextMenuStrip subtitleMenu;
        private ToolStripMenuItem audioAddTrack;
        private ToolStripMenuItem audioRemoveTrack;
        private ToolStripMenuItem subtitleAddTrack;
        private ToolStripMenuItem subtitleRemoveTrack;
        protected Panel subtitlePanel;
        protected TabControl subtitles;
        private TabPage subPage1;
        private MeGUI.core.details.mux.MuxStreamControl muxStreamControl1;
        protected FileBar vInput;
        protected FileBar chapters;
        protected Label splittingLabel;
        protected MeGUI.core.gui.TargetSizeSCBox splitting;
        protected FileBar output;
        protected MeGUI.core.gui.FPSChooser fps;
        private IContainer components;
        #region start/stop
        public baseMuxWindow()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            
            audioTracks = new List<MuxStreamControl>();
            audioTracks.Add(muxStreamControl2);
            subtitleTracks = new List<MuxStreamControl>();
            subtitleTracks.Add(muxStreamControl1);

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
        public void setConfig(string videoInput, decimal? framerate, MuxStream[] audioStreams, MuxStream[] subtitleStreams, string chapterFile, string output, FileSize? splitSize, Dar? dar)
        {
            this.dar = dar;
            vInput.Filename = videoInput;
            fps.Value = framerate;
            
            int index = 0;
            foreach (MuxStream stream in audioStreams)
            {
                if (audioTracks.Count == index)
                    AudioAddTrack();
                audioTracks[index].Stream = stream;
                index++;
            }

            index = 0;
            foreach (MuxStream stream in subtitleStreams)
            {
                if (subtitleTracks.Count == index)
                    SubtitleAddTrack();
                subtitleTracks[index].Stream = stream;
                index++;
            }

            chapters.Filename = chapterFile;
            this.output.Filename = output;
            this.splitting.Value = splitSize;
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
        public void getAdditionalStreams(out MuxStream[] aStreams, out MuxStream[] sStreams, out string chapterFile)
        {
            aStreams = getStreams(audioTracks);
            sStreams = getStreams(subtitleTracks);
            chapterFile = chapters.Filename;
        }

        private MuxStream[] getStreams(List<MuxStreamControl> controls)
        {
            List<MuxStream> streams = new List<MuxStream>();
            foreach (MuxStreamControl t in controls)
            {
                if (t.Stream != null)
                    streams.Add(t.Stream);
            }
            return streams.ToArray();
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
            this.muxButton = new System.Windows.Forms.Button();
            this.videoGroupbox = new System.Windows.Forms.GroupBox();
            this.videoName = new System.Windows.Forms.TextBox();
            this.videoNameLabel = new System.Windows.Forms.Label();
            this.MuxFPSLabel = new System.Windows.Forms.Label();
            this.videoInputLabel = new System.Windows.Forms.Label();
            this.chaptersGroupbox = new System.Windows.Forms.GroupBox();
            this.chaptersInputLabel = new System.Windows.Forms.Label();
            this.outputGroupbox = new System.Windows.Forms.GroupBox();
            this.splittingLabel = new System.Windows.Forms.Label();
            this.muxedOutputLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.audioPanel = new System.Windows.Forms.Panel();
            this.audioMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.audioAddTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.audioRemoveTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.audio = new System.Windows.Forms.TabControl();
            this.audioPage1 = new System.Windows.Forms.TabPage();
            this.subtitleMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.subtitleAddTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.subtitleRemoveTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.subtitlePanel = new System.Windows.Forms.Panel();
            this.subtitles = new System.Windows.Forms.TabControl();
            this.subPage1 = new System.Windows.Forms.TabPage();
            this.muxStreamControl1 = new MeGUI.core.details.mux.MuxStreamControl();
            this.muxStreamControl2 = new MeGUI.core.details.mux.MuxStreamControl();
            this.splitting = new MeGUI.core.gui.TargetSizeSCBox();
            this.output = new MeGUI.FileBar();
            this.fps = new MeGUI.core.gui.FPSChooser();
            this.vInput = new MeGUI.FileBar();
            this.chapters = new MeGUI.FileBar();
            this.videoGroupbox.SuspendLayout();
            this.chaptersGroupbox.SuspendLayout();
            this.outputGroupbox.SuspendLayout();
            this.audioPanel.SuspendLayout();
            this.audioMenu.SuspendLayout();
            this.audio.SuspendLayout();
            this.audioPage1.SuspendLayout();
            this.subtitleMenu.SuspendLayout();
            this.subtitlePanel.SuspendLayout();
            this.subtitles.SuspendLayout();
            this.subPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // muxButton
            // 
            this.muxButton.Location = new System.Drawing.Point(291, 458);
            this.muxButton.Name = "muxButton";
            this.muxButton.Size = new System.Drawing.Size(56, 23);
            this.muxButton.TabIndex = 26;
            this.muxButton.Text = "Queue";
            this.muxButton.Click += new System.EventHandler(this.muxButton_Click);
            // 
            // videoGroupbox
            // 
            this.videoGroupbox.Controls.Add(this.fps);
            this.videoGroupbox.Controls.Add(this.vInput);
            this.videoGroupbox.Controls.Add(this.videoName);
            this.videoGroupbox.Controls.Add(this.videoNameLabel);
            this.videoGroupbox.Controls.Add(this.MuxFPSLabel);
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
            this.videoName.Location = new System.Drawing.Point(283, 49);
            this.videoName.MaxLength = 100;
            this.videoName.Name = "videoName";
            this.videoName.Size = new System.Drawing.Size(95, 21);
            this.videoName.TabIndex = 34;
            // 
            // videoNameLabel
            // 
            this.videoNameLabel.AutoSize = true;
            this.videoNameLabel.Location = new System.Drawing.Point(243, 53);
            this.videoNameLabel.Name = "videoNameLabel";
            this.videoNameLabel.Size = new System.Drawing.Size(34, 13);
            this.videoNameLabel.TabIndex = 33;
            this.videoNameLabel.Text = "Name";
            // 
            // MuxFPSLabel
            // 
            this.MuxFPSLabel.Location = new System.Drawing.Point(16, 51);
            this.MuxFPSLabel.Name = "MuxFPSLabel";
            this.MuxFPSLabel.Size = new System.Drawing.Size(40, 16);
            this.MuxFPSLabel.TabIndex = 32;
            this.MuxFPSLabel.Text = "FPS";
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
            this.chaptersGroupbox.Controls.Add(this.chapters);
            this.chaptersGroupbox.Controls.Add(this.chaptersInputLabel);
            this.chaptersGroupbox.Location = new System.Drawing.Point(8, 318);
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
            // outputGroupbox
            // 
            this.outputGroupbox.Controls.Add(this.splittingLabel);
            this.outputGroupbox.Controls.Add(this.splitting);
            this.outputGroupbox.Controls.Add(this.output);
            this.outputGroupbox.Controls.Add(this.muxedOutputLabel);
            this.outputGroupbox.Location = new System.Drawing.Point(8, 372);
            this.outputGroupbox.Name = "outputGroupbox";
            this.outputGroupbox.Size = new System.Drawing.Size(424, 80);
            this.outputGroupbox.TabIndex = 28;
            this.outputGroupbox.TabStop = false;
            this.outputGroupbox.Text = "Output";
            // 
            // splittingLabel
            // 
            this.splittingLabel.AutoSize = true;
            this.splittingLabel.Location = new System.Drawing.Point(14, 53);
            this.splittingLabel.Name = "splittingLabel";
            this.splittingLabel.Size = new System.Drawing.Size(45, 13);
            this.splittingLabel.TabIndex = 37;
            this.splittingLabel.Text = "Splitting";
            // 
            // muxedOutputLabel
            // 
            this.muxedOutputLabel.Location = new System.Drawing.Point(14, 23);
            this.muxedOutputLabel.Name = "muxedOutputLabel";
            this.muxedOutputLabel.Size = new System.Drawing.Size(100, 16);
            this.muxedOutputLabel.TabIndex = 17;
            this.muxedOutputLabel.Text = "Muxed Output";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(376, 458);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(56, 23);
            this.cancelButton.TabIndex = 29;
            this.cancelButton.Text = "Cancel";
            // 
            // audioPanel
            // 
            this.audioPanel.ContextMenuStrip = this.audioMenu;
            this.audioPanel.Controls.Add(this.audio);
            this.audioPanel.Location = new System.Drawing.Point(8, 93);
            this.audioPanel.Name = "audioPanel";
            this.audioPanel.Size = new System.Drawing.Size(424, 115);
            this.audioPanel.TabIndex = 31;
            // 
            // audioMenu
            // 
            this.audioMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audioAddTrack,
            this.audioRemoveTrack});
            this.audioMenu.Name = "audioMenu";
            this.audioMenu.Size = new System.Drawing.Size(141, 48);
            this.audioMenu.Opening += new System.ComponentModel.CancelEventHandler(this.audioMenu_Opening);
            // 
            // audioAddTrack
            // 
            this.audioAddTrack.Name = "audioAddTrack";
            this.audioAddTrack.Size = new System.Drawing.Size(140, 22);
            this.audioAddTrack.Text = "Add track";
            this.audioAddTrack.Click += new System.EventHandler(this.audioAddTrack_Click);
            // 
            // audioRemoveTrack
            // 
            this.audioRemoveTrack.Name = "audioRemoveTrack";
            this.audioRemoveTrack.Size = new System.Drawing.Size(140, 22);
            this.audioRemoveTrack.Text = "Remove track";
            this.audioRemoveTrack.Click += new System.EventHandler(this.audioRemoveTrack_Click);
            // 
            // audio
            // 
            this.audio.Controls.Add(this.audioPage1);
            this.audio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audio.Location = new System.Drawing.Point(0, 0);
            this.audio.Name = "audio";
            this.audio.SelectedIndex = 0;
            this.audio.Size = new System.Drawing.Size(424, 115);
            this.audio.TabIndex = 32;
            // 
            // audioPage1
            // 
            this.audioPage1.Controls.Add(this.muxStreamControl2);
            this.audioPage1.Location = new System.Drawing.Point(4, 22);
            this.audioPage1.Name = "audioPage1";
            this.audioPage1.Padding = new System.Windows.Forms.Padding(3);
            this.audioPage1.Size = new System.Drawing.Size(416, 89);
            this.audioPage1.TabIndex = 0;
            this.audioPage1.Text = "Audio 1";
            this.audioPage1.UseVisualStyleBackColor = true;
            // 
            // subtitleMenu
            // 
            this.subtitleMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subtitleAddTrack,
            this.subtitleRemoveTrack});
            this.subtitleMenu.Name = "subtitleMenu";
            this.subtitleMenu.Size = new System.Drawing.Size(141, 48);
            this.subtitleMenu.Opening += new System.ComponentModel.CancelEventHandler(this.subtitleMenu_Opening);
            // 
            // subtitleAddTrack
            // 
            this.subtitleAddTrack.Name = "subtitleAddTrack";
            this.subtitleAddTrack.Size = new System.Drawing.Size(140, 22);
            this.subtitleAddTrack.Text = "Add track";
            this.subtitleAddTrack.Click += new System.EventHandler(this.subtitleAddTrack_Click);
            // 
            // subtitleRemoveTrack
            // 
            this.subtitleRemoveTrack.Name = "subtitleRemoveTrack";
            this.subtitleRemoveTrack.Size = new System.Drawing.Size(140, 22);
            this.subtitleRemoveTrack.Text = "Remove track";
            this.subtitleRemoveTrack.Click += new System.EventHandler(this.subtitleRemoveTrack_Click);
            // 
            // subtitlePanel
            // 
            this.subtitlePanel.ContextMenuStrip = this.subtitleMenu;
            this.subtitlePanel.Controls.Add(this.subtitles);
            this.subtitlePanel.Location = new System.Drawing.Point(8, 214);
            this.subtitlePanel.Name = "subtitlePanel";
            this.subtitlePanel.Size = new System.Drawing.Size(424, 98);
            this.subtitlePanel.TabIndex = 34;
            // 
            // subtitles
            // 
            this.subtitles.Controls.Add(this.subPage1);
            this.subtitles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subtitles.Location = new System.Drawing.Point(0, 0);
            this.subtitles.Name = "subtitles";
            this.subtitles.SelectedIndex = 0;
            this.subtitles.Size = new System.Drawing.Size(424, 98);
            this.subtitles.TabIndex = 31;
            // 
            // subPage1
            // 
            this.subPage1.Controls.Add(this.muxStreamControl1);
            this.subPage1.Location = new System.Drawing.Point(4, 22);
            this.subPage1.Name = "subPage1";
            this.subPage1.Padding = new System.Windows.Forms.Padding(3);
            this.subPage1.Size = new System.Drawing.Size(416, 72);
            this.subPage1.TabIndex = 0;
            this.subPage1.Text = "Subtitle 1";
            this.subPage1.UseVisualStyleBackColor = true;
            // 
            // muxStreamControl1
            // 
            this.muxStreamControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.muxStreamControl1.Filter = null;
            this.muxStreamControl1.Location = new System.Drawing.Point(3, 3);
            this.muxStreamControl1.Name = "muxStreamControl1";
            this.muxStreamControl1.ShowDelay = false;
            this.muxStreamControl1.Size = new System.Drawing.Size(410, 66);
            this.muxStreamControl1.TabIndex = 0;
            // 
            // muxStreamControl2
            // 
            this.muxStreamControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.muxStreamControl2.Filter = null;
            this.muxStreamControl2.Location = new System.Drawing.Point(3, 3);
            this.muxStreamControl2.Name = "muxStreamControl2";
            this.muxStreamControl2.ShowDelay = true;
            this.muxStreamControl2.Size = new System.Drawing.Size(410, 83);
            this.muxStreamControl2.TabIndex = 0;
            // 
            // splitting
            // 
            this.splitting.Location = new System.Drawing.Point(115, 45);
            this.splitting.MaximumSize = new System.Drawing.Size(1000, 29);
            this.splitting.MinimumSize = new System.Drawing.Size(64, 29);
            this.splitting.Name = "splitting";
            this.splitting.NullString = "No splitting";
            this.splitting.SelectedIndex = 0;
            this.splitting.Size = new System.Drawing.Size(181, 29);
            this.splitting.TabIndex = 36;
            // 
            // output
            // 
            this.output.Filename = "";
            this.output.Filter = null;
            this.output.FolderMode = false;
            this.output.Location = new System.Drawing.Point(118, 13);
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.SaveMode = false;
            this.output.Size = new System.Drawing.Size(289, 26);
            this.output.TabIndex = 35;
            this.output.Title = null;
            this.output.FileSelected += new MeGUI.FileBarEventHandler(this.output_FileSelected);
            // 
            // fps
            // 
            this.fps.Location = new System.Drawing.Point(115, 45);
            this.fps.MaximumSize = new System.Drawing.Size(1000, 29);
            this.fps.MinimumSize = new System.Drawing.Size(64, 29);
            this.fps.Name = "fps";
            this.fps.NullString = "Not set";
            this.fps.SelectedIndex = 0;
            this.fps.Size = new System.Drawing.Size(119, 29);
            this.fps.TabIndex = 36;
            this.fps.SelectionChanged += new MeGUI.StringChanged(this.fps_SelectionChanged);
            // 
            // vInput
            // 
            this.vInput.Filename = "";
            this.vInput.Filter = null;
            this.vInput.FolderMode = false;
            this.vInput.Location = new System.Drawing.Point(118, 13);
            this.vInput.Name = "vInput";
            this.vInput.ReadOnly = true;
            this.vInput.SaveMode = false;
            this.vInput.Size = new System.Drawing.Size(289, 26);
            this.vInput.TabIndex = 35;
            this.vInput.Title = null;
            this.vInput.FileSelected += new MeGUI.FileBarEventHandler(this.vInput_FileSelected);
            // 
            // chapters
            // 
            this.chapters.Filename = "";
            this.chapters.Filter = "\"Chapter files (*.txt)|*.txt\";";
            this.chapters.FolderMode = false;
            this.chapters.Location = new System.Drawing.Point(118, 12);
            this.chapters.Name = "chapters";
            this.chapters.ReadOnly = true;
            this.chapters.SaveMode = false;
            this.chapters.Size = new System.Drawing.Size(289, 26);
            this.chapters.TabIndex = 35;
            this.chapters.Title = null;
            this.chapters.FileSelected += new MeGUI.FileBarEventHandler(this.chapters_FileSelected);
            // 
            // baseMuxWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(444, 493);
            this.Controls.Add(this.subtitlePanel);
            this.Controls.Add(this.audioPanel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.outputGroupbox);
            this.Controls.Add(this.muxButton);
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
            this.videoGroupbox.ResumeLayout(false);
            this.videoGroupbox.PerformLayout();
            this.chaptersGroupbox.ResumeLayout(false);
            this.outputGroupbox.ResumeLayout(false);
            this.outputGroupbox.PerformLayout();
            this.audioPanel.ResumeLayout(false);
            this.audioMenu.ResumeLayout(false);
            this.audio.ResumeLayout(false);
            this.audioPage1.ResumeLayout(false);
            this.subtitleMenu.ResumeLayout(false);
            this.subtitlePanel.ResumeLayout(false);
            this.subtitles.ResumeLayout(false);
            this.subPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        #region helper method
        protected virtual void checkIO()
        {
            if (string.IsNullOrEmpty(vInput.Filename))
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else if (string.IsNullOrEmpty(output.Filename))
            {
                muxButton.DialogResult = DialogResult.None;
                return;
            }
            else if (fps.Value == null && isFPSRequired())
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
                if (vInput.Filename.Length > 0)
                    return (VideoUtil.guessVideoType(vInput.Filename).ContainerType == null);
                return true;
            }
            catch (NullReferenceException) // This will throw if it can't guess the video type
            {
                return true;
            }
        }

        #region button event handlers
        private void vInput_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            try
            {
                fps.Value = (decimal)new MediaInfoFile(vInput.Filename).Info.FPS;
            }
            catch (Exception) { fps.Value = null; }

            if (string.IsNullOrEmpty(output.Filename))
                chooseOutputFilename();

            fileUpdated();
            checkIO();
        }

        protected virtual void ChangeOutputExtension() {}

        private void chooseOutputFilename()
        {
            output.Filename = FileUtil.AddToFileName(vInput.Filename, "-muxed");
            ChangeOutputExtension();
        }

        private void chapters_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            fileUpdated();
        }

        private void output_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            checkIO();
            fileUpdated();
        }

        protected virtual void muxButton_Click(object sender, System.EventArgs e)
        {
            if (muxButton.DialogResult != DialogResult.OK)
            {
                if (string.IsNullOrEmpty(vInput.Filename))
                {
                    MessageBox.Show("You must configure a video input file", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else if (string.IsNullOrEmpty(output.Filename))
                {
                    MessageBox.Show("You must configure an output file", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else if (!fps.Value.HasValue)
                {
                    MessageBox.Show("You must select a framerate", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
        }
        #endregion
        #region language dropdowns
        #endregion
        #region other events
        private void fps_SelectionChanged(object sender, string val)
        {
            checkIO();
        }
        #endregion
        #region properties
/*        public virtual string AudioFilter
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
        }*/
        #endregion
        protected virtual void fileUpdated() { }


        #region adding / removing tracks
        private void audioAddTrack_Click(object sender, EventArgs e)
        {
            AudioAddTrack();
        }

        protected void AudioAddTrack()
        {
            TabPage p = new TabPage("Audio " + (audioTracks.Count + 1));
            p.UseVisualStyleBackColor = audio.TabPages[0].UseVisualStyleBackColor;
            p.Padding = audio.TabPages[0].Padding;

            MuxStreamControl a = new MuxStreamControl();
            a.Dock = audioTracks[0].Dock;
            a.Padding = audioTracks[0].Padding;
            a.ShowDelay = audioTracks[0].ShowDelay;
            a.Filter = audioTracks[0].Filter;

            audio.TabPages.Add(p);
            p.Controls.Add(a);
            audioTracks.Add(a);
        }

        private void audioRemoveTrack_Click(object sender, EventArgs e)
        {
            AudioRemoveTrack();
        }

        protected void AudioRemoveTrack()
        {
            audio.TabPages.RemoveAt(audio.TabPages.Count - 1);
            audioTracks.RemoveAt(audioTracks.Count - 1);
        }

        private void subtitleAddTrack_Click(object sender, EventArgs e)
        {
            SubtitleAddTrack();
        }

        protected void SubtitleAddTrack()
        {
            TabPage p = new TabPage("Subtitle " + (subtitleTracks.Count + 1));
            p.UseVisualStyleBackColor = subtitles.TabPages[0].UseVisualStyleBackColor;
            p.Padding = subtitles.TabPages[0].Padding;

            MuxStreamControl a = new MuxStreamControl();
            a.Dock = subtitleTracks[0].Dock;
            a.Padding = subtitleTracks[0].Padding;
            a.ShowDelay = subtitleTracks[0].ShowDelay;
            a.Filter = subtitleTracks[0].Filter;

            subtitles.TabPages.Add(p);
            p.Controls.Add(a);
            subtitleTracks.Add(a);
        }

        private void subtitleRemoveTrack_Click(object sender, EventArgs e)
        {
            SubtitleRemoveTrack();
        }

        protected void SubtitleRemoveTrack()
        {
            subtitles.TabPages.RemoveAt(subtitles.TabPages.Count - 1);
            subtitleTracks.RemoveAt(subtitleTracks.Count - 1);
        }
        #endregion

        private void audioMenu_Opening(object sender, CancelEventArgs e)
        {
            audioRemoveTrack.Enabled = audioTracks.Count > 1;
        }

        private void subtitleMenu_Opening(object sender, CancelEventArgs e)
        {
            subtitleRemoveTrack.Enabled = subtitleTracks.Count > 1;
        }


    }
}