using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.details.video;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.gui;
using System.Diagnostics;

namespace MeGUI
{
    public partial class AudioEncodingComponent : UserControl
    {
        private List<AudioEncodingTab> tabs = new List<AudioEncodingTab>();

        #region init
        public AudioEncodingComponent()
        {
            InitializeComponent();
            tabs.Add(audioEncodingTab1);

            audioEncodingTab1.QueueJob = delegate(AudioJob a)
            {
                MainForm.Instance.Jobs.addJobsToQueue(a);
            };
        }
        #endregion

        public void AddTab()
        {
            AudioEncodingTab a = new AudioEncodingTab();
            tabs.Add(a);
            a.Dock = System.Windows.Forms.DockStyle.Fill;
            a.InitializeDropdowns();
            a.QueueJob = tabs[0].QueueJob;
            
            TabPage p = new TabPage("Track " + tabs.Count);
            tabControl1.TabPages.Add(p);
            p.Controls.Add(a);
            p.Padding = tabControl1.TabPages[0].Padding;
            a.FileTypeComboBoxSize = tabs[0].FileTypeComboBoxSize; // has to go after padding
            p.UseVisualStyleBackColor = tabControl1.TabPages[0].UseVisualStyleBackColor;

        }

        /// <summary>
        /// returns the audio streams registered
        /// </summary>
        public List<AudioJob> AudioStreams
        {
            get
            {
                List<AudioJob> streams = new List<AudioJob>();
                foreach (AudioEncodingTab t in tabs)
                {
                    AudioJob a = t.AudioJob;
                    if (a != null)
                        streams.Add(a);
                }
                return streams;
            }
        }

        /// <summary>
        /// Returns null if all audio configurations are valid or incomplete. Returns
        /// an error message if any audio configuration issues a serious (not just incomplete)
        /// error message
        /// </summary>
        /// <returns></returns>
        internal string verifyAudioSettings()
        {
            foreach (AudioEncodingTab t in tabs)
            {
                AudioJob a = t.AudioJob;
                if (a == null) continue;
                string s = t.verifyAudioSettings();
                if (s != null)
                    return s;
            }
            return null;
        }

        internal void Reset()
        {
            foreach (AudioEncodingTab t in tabs)
                t.Reset();
        }

        internal void openAudioFile(params string[] files)
        {
            for (int i = 0; i < files.Length; ++i)
            {
                Debug.Assert(i <= tabs.Count);

                if (i == tabs.Count)
                    AddTab();

                tabs[i].openAudioFile(files[i]);
            }
            tabControl1.SelectedIndex = files.Length - 1;
        }

        internal void RefreshProfiles()
        {
            foreach (AudioEncodingTab t in tabs)
                t.RefreshProfiles();
        }

        internal void InitializeDropdowns()
        {
            foreach (AudioEncodingTab t in tabs)
                t.InitializeDropdowns();
        }

        private void newTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void removeTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        private void RemoveTab()
        {
            tabs.RemoveAt(tabs.Count - 1);
            tabControl1.TabPages.RemoveAt(tabControl1.TabPages.Count - 1);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (tabs.Count == 1)
                removeTrackToolStripMenuItem.Enabled = false;
            else
                removeTrackToolStripMenuItem.Enabled = true;
        }
    }
}