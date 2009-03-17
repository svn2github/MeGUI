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
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI
{
    public partial class DGVinputWindow : System.Windows.Forms.Form
    {
        #region variables
        private DGVIndexJob lastJob = null;
        private MainForm mainForm;
        private VideoUtil vUtil;
        private JobUtil jobUtil;

        private bool configured = false;
        private bool dialogMode = false;
        private bool processing = false;
        #endregion

        public DGVinputWindow(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.vUtil = new VideoUtil(mainForm);
            this.jobUtil = new JobUtil(mainForm);
        }

        public DGVinputWindow(MainForm mainForm, string fileName): this(mainForm)
		{
			openVideo(fileName);
		}

        public DGVinputWindow(MainForm mainForm, string fileName, bool autoReturn) : this(mainForm, fileName)
        {
            openVideo(fileName);
            this.loadOnComplete.Checked = true;
            this.closeOnQueue.Checked = true;
            checkIndexIO();
        }

        private void input_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            openVideo(input.Filename);
            checkIndexIO();
        }

        private void openVideo(string fileName)
        {
            if (input.Filename != fileName)
                input.Filename = fileName;

            string projectPath;
            bool vc1Stream = VideoUtil.detecVC1StreamFromFile(fileName);
            string fileNameNoPath = Path.GetFileName(fileName);
            if (string.IsNullOrEmpty(projectPath = mainForm.Settings.DefaultOutputDir))
                projectPath = Path.GetDirectoryName(fileName);
            
            AudioTracks.Items.Clear();

            if (vc1Stream)
            {
                int unused;
                List<AudioTrackInfo> audioTracks;
                projectName.Text = Path.Combine(projectPath, Path.ChangeExtension(fileNameNoPath, ".dgv"));
                vUtil.getSourceMediaInfo(fileName, out audioTracks, out unused);
                foreach (AudioTrackInfo atrack in audioTracks)
                    AudioTracks.Items.Add(atrack);
            }
            else MessageBox.Show("MeGUI is not able to find a VC-1 stream from " + Path.GetFileName(fileName) + "...",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (AudioTracks.Items.Count < 1)
            {
                MessageBox.Show("MeGUI cannot find audio track information. Audio Tracks selection will be disabled.");
                demuxNoAudiotracks.Enabled = false;
                demuxAll.Enabled = false;
            }
            else
            {
                demuxNoAudiotracks.Enabled = true;
                demuxAll.Enabled = true;
            }
        }

        private void checkIndexIO()
        {
            configured = (!input.Filename.Equals("") && !projectName.Text.Equals(""));
            if (configured && dialogMode)
                queueButton.DialogResult = DialogResult.OK;
            else
                queueButton.DialogResult = DialogResult.None;
        }

        private void pickOutputButton_Click(object sender, EventArgs e)
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

        /// <summary>
        /// creates a dgvindex project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queueButton_Click(object sender, EventArgs e)
        {
            if (Drives.ableToWriteOnThisDrive(Path.GetPathRoot(projectName.Text)))
            {
                if (configured)
                {
                    if (!dialogMode)
                    {
                        DGVIndexJob job = generateIndexJob();
                        lastJob = job;
                        mainForm.Jobs.addJobsToQueue(job);
                        if (this.closeOnQueue.Checked)
                            this.Close();
                    }
                }
                else
                    MessageBox.Show("You must select a Video Input and DGVC1Index project file to continue",
                        "Configuration incomplete", MessageBoxButtons.OK);
            }
            else
                MessageBox.Show("MeGUI cannot write on " + Path.GetPathRoot(projectName.Text) +
                                ". Please, select an other Output path.", "Configuration Incomplete", MessageBoxButtons.OK);
        }

        private DGVIndexJob generateIndexJob()
        {
            int demuxType = 0;
            if (demuxNoAudiotracks.Checked)
                demuxType = 0;
            else
                demuxType = 2;

            List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
            foreach (AudioTrackInfo ati in AudioTracks.Items)
            {
                audioTracks.Add(ati);
            }

            return new DGVIndexJob(this.input.Filename, this.projectName.Text, demuxType, audioTracks, null, loadOnComplete.Checked);
        }


        #region properties
        /// <summary>
        /// gets the index job created from the current configuration
        /// </summary>
        public DGVIndexJob Job
        {
            get { return generateIndexJob(); }
        }

        public DGVIndexJob LastJob
        {
            get { return lastJob; }
            set { lastJob = value; }
        }

        public bool JobCreated
        {
            get { return lastJob != null; }
        }

        public string ID
        {
            get { return "dgv_creator"; }
        }

        #endregion
    }

    public class dgvc1IndexJobPostProcessor
    {
        public static JobPostProcessor PostProcessor = new JobPostProcessor(postprocess, "Dgv_postprocessor");
        private static LogItem postprocess(MainForm mainForm, Job ajob)
        {
            if (!(ajob is DGVIndexJob)) return null;
            DGVIndexJob job = (DGVIndexJob)ajob;
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
}
