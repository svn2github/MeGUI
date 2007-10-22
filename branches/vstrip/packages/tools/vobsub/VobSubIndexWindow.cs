using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;

namespace MeGUI
{
    public partial class VobSubIndexWindow : Form
    {
        #region variables
        private bool dialogMode = false; // $%£%$^>*"%$%%$#{"!!! Affects the public behaviour!
        private bool configured = false;
        private MainForm mainForm;
        private VideoUtil vUtil;
        private JobUtil jobUtil;
        #endregion
        #region start / stop
        public VobSubIndexWindow(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.vUtil = new VideoUtil(mainForm);
            this.jobUtil = new JobUtil(mainForm);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        #endregion
        #region button handlers
        private void queueButton_Click(object sender, EventArgs e)
        {
            if (configured)
            {
                if (!dialogMode)
                {
                    SubtitleIndexJob job = generateJob();
                    mainForm.Jobs.addJobsToQueue(job);
                    if (this.closeOnQueue.Checked)
                        this.Close();
                }
            }
            else
                MessageBox.Show("You must select an Input and Output file to continue",
                    "Configuration incomplete", MessageBoxButtons.OK);
        }
        #endregion
        private void openVideo(string fileName)
        {
            input.Filename = fileName;
            Dar? ar;
            int maxHorizontalResolution;
            List<AudioTrackInfo> audioTracks;
            List<SubtitleInfo> subtitles;
            keepAllTracks.Checked = vUtil.openVideoSource(fileName, out audioTracks, out subtitles, out ar, out maxHorizontalResolution);
            subtitleTracks.Items.Clear();
            subtitleTracks.Items.AddRange(subtitles.ToArray());
            demuxSelectedTracks.Checked = !keepAllTracks.Checked;
        }
        private void checkIndexIO()
        {
            configured = (!input.Filename.Equals("") && !output.Filename.Equals(""));
            if (configured && dialogMode)
                queueButton.DialogResult = DialogResult.OK;
            else
                queueButton.DialogResult = DialogResult.None;
        }

        private SubtitleIndexJob generateJob()
        {
            List<int> trackIDs = new List<int>();
            foreach (object itemChecked in subtitleTracks.CheckedItems)
            {
                SubtitleInfo si = (SubtitleInfo)itemChecked;
                trackIDs.Add(si.Index);
            }
            return new SubtitleIndexJob(input.Filename, output.Filename, keepAllTracks.Checked, trackIDs, (int)pgc.Value);
        }
        public void setConfig(string input, string output, bool indexAllTracks, List<int> trackIDs, int pgc)
        {
            this.dialogMode = true;
            queueButton.Text = "Update";
            this.input.Filename = input;
            openVideo(input);
            this.output.Filename = output;
            checkIndexIO();
            if (indexAllTracks)
                keepAllTracks.Checked = true;
            else
            {
                demuxSelectedTracks.Checked = true;
                int index = 0;
                List<int> checkedItems = new List<int>();
                foreach (object item in subtitleTracks.Items)
                {
                    SubtitleInfo si = (SubtitleInfo)item;
                    if (trackIDs.Contains(si.Index))
                        checkedItems.Add(index);
                    index++;
                }
                foreach (int idx in checkedItems)
                {
                    subtitleTracks.SetItemChecked(idx, true);
                }
            }
            this.pgc.Value = pgc;
        }
        /// <summary>
        /// gets the index job created from the current configuration
        /// </summary>
        public SubtitleIndexJob Job
        {
            get { return generateJob(); }
        }

        private void input_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            openVideo(input.Filename);
            output.Filename = Path.ChangeExtension(input.Filename, ".idx");
            checkIndexIO();
        }

        private void output_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            checkIndexIO();
        }
    }

    public class VobSubTool : ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "VobSubber"; }
        }

        /// <summary>
        /// launches the vobsub indexer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Run(MainForm info)
        {
            using (VobSubIndexWindow vsi = new VobSubIndexWindow(info))
            {
                vsi.ShowDialog();
            }
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlN }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "VobSubber"; }
        }

        #endregion
    }
}