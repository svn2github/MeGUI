using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class OneClickConfigurationDialog : Form
    {
        private MainForm mainForm;
        private BitrateCalculator calc;
        private int oldOneClickProfileIndex = -1;
        private string path;

        public OneClickConfigurationDialog(int videoIndex, int audioIndex, MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.path = mainForm.MeGUIPath;
            this.calc = new BitrateCalculator();
            this.filesizeComboBox.Items.AddRange(calc.getPredefinedOutputSizes());
            this.filesizeComboBox.Items.Add("Custom");
            this.filesizeComboBox.Items.Add("Don't care");

            this.filesizeComboBox.SelectedIndex = 2;
            this.containerFormat.SelectedIndex = 1;
            foreach (string name in mainForm.Profiles.AvsProfiles.Keys)
            {
                this.avsProfile.Items.Add(name);
            }
            if (avsProfile.Items.Contains(mainForm.Settings.AvsProfileName))
                avsProfile.SelectedItem = mainForm.Settings.AvsProfileName;
            foreach (string name in mainForm.Profiles.VideoProfiles.Keys)
            {
                this.videoProfile.Items.Add(name);
            }
            if (videoIndex > -1)
                videoProfile.SelectedIndex = videoIndex;
            // This will have already been done by containerFormat.SelectedIndex, 
            // which triggers containerFormat_SelectedIndexChanged which refreshes the profiles
            /*foreach (string name in mainForm.Profiles.AudioProfiles.Keys)
              {
                  this.audioProfile.Items.Add(name);
              }
              if (audioIndex > -1)
                  audioProfile.SelectedIndex = audioIndex;
             */
            foreach (string name in mainForm.Profiles.OneClickProfiles.Keys)
            {
                this.playbackMethod.Items.Add(name);
            }
            if (playbackMethod.Items.Contains(mainForm.Settings.OneClickProfileName))
                playbackMethod.SelectedItem = mainForm.Settings.OneClickProfileName;
            playbackMethod_SelectedIndexChanged(null, null);
        }

        private void filesizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.filesizeKB.ReadOnly = true;
            filesizeKB.Text = calc.getOutputSizeKBs(filesizeComboBox.SelectedIndex).ToString();
            if (filesizeComboBox.SelectedIndex == 10) // Custom
                this.filesizeKB.ReadOnly = false;
            if (filesizeComboBox.SelectedIndex == 11) // Don't care
                this.filesizeKB.Text = "-1";

        }

        private void videoProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.videoProfile.SelectedIndex != -1) // if it's -1 it's bogus
            {
                if (!dontEncodeAudio.Checked)
                    containerFormat.Enabled = true;
                VideoProfile prof = this.mainForm.Profiles.VideoProfiles[this.videoProfile.SelectedItem.ToString()];
                if (prof.Settings is lavcSettings)
                {
                    videoCodec.SelectedIndex = (int)VideoCodec.ASP;
                }
                if (prof.Settings is x264Settings)
                {
                    videoCodec.SelectedIndex = (int)VideoCodec.AVC;
                }
                if (prof.Settings is snowSettings)
                {
                    videoCodec.SelectedIndex = (int)VideoCodec.SNOW;
                    if (!dontEncodeAudio.Checked)
                    {
                        containerFormat.SelectedIndex = 0;
                        containerFormat_SelectedIndexChanged(null, null);
                        containerFormat.Enabled = false;
                    }
                }
                if (prof.Settings is xvidSettings)
                {
                    videoCodec.SelectedIndex = (int)VideoCodec.ASP;
                }
            }
        }

        private void containerFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.audioProfile.Items.Clear();
            foreach (AudioProfile prof in this.mainForm.Profiles.AudioProfiles.Values)
            {
                if (containerFormat.SelectedIndex == 0)
                {
                    if (prof.Settings is MP3Settings)
                        audioProfile.Items.Add(prof.Name);
                }
                else
                {
                    if (prof.Settings is NeroAACSettings || prof.Settings is FaacSettings || containerFormat.SelectedIndex == 1)
                        audioProfile.Items.Add(prof.Name);
                }
            }
            if (audioProfile.Items.Count > 0)
                audioProfile.SelectedIndex = 0;

        }

        private void splitOutput_CheckedChanged(object sender, EventArgs e)
        {
            if (splitOutput.Checked)
                splitSize.Enabled = true;
            else
            {
                splitSize.Enabled = false;
                splitSize.Text = "0";
            }
        }

        private void dontEncodeAudio_CheckedChanged(object sender, EventArgs e)
        {
            if (dontEncodeAudio.Checked)
            {
                this.audioProfile.Enabled = false;
                containerFormat.SelectedIndex = 1; //MKV
                containerFormat_SelectedIndexChanged(null, null);
                containerFormat.Enabled = false;
            }
            else
            {
                // this does the required stuff to the container format
                videoProfile_SelectedIndexChanged(null, null);
                this.audioProfile.Enabled = true;
            }
        }

        private void playbackMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (playbackMethod.SelectedIndex != -1) // if it's -1 it's bogus
            {
                OneClickProfile prof = this.mainForm.Profiles.OneClickProfiles[this.playbackMethod.SelectedItem.ToString()];
                this.Settings = prof.Settings;
                /*
                if (this.oldOneClickProfileIndex != -1 && !newProfile) // -1 means it's never been touched
                    this.mainForm.Profiles.OneClickProfiles[this.playbackMethod.Items[this.oldOneClickProfileIndex].ToString()].Settings = this.Settings;
                newProfile = false;
                this.Settings = prof.Settings;
                this.oldOneClickProfileIndex = this.playbackMethod.SelectedIndex;
                this.playbackMethod.SelectAll();*/
            }
        }

        public OneClickSettings Settings
        {
            get
            {
                OneClickSettings settings = new OneClickSettings();
                settings.AudioProfileName = audioProfile.Text;
                settings.AutomaticDeinterlacing = autoDeint.Checked;
                settings.AvsProfileName = avsProfile.Text;
                settings.ContainerFormatName = containerFormat.Text;
                settings.DontEncodeAudio = dontEncodeAudio.Checked;
                if (filesizeKB.Text.Length > 0)
                    settings.Filesize = Int64.Parse(filesizeKB.Text);
                settings.OutputResolution = (int)horizontalResolution.Value;
                settings.SignalAR = signalAR.Checked;
                settings.Split = splitOutput.Checked;
                if (splitSize.Text.Length > 0)
                    settings.SplitSize = Int64.Parse(splitSize.Text);
                settings.StorageMediumName = filesizeComboBox.Text;
                settings.VideoProfileName = videoProfile.Text;
                return settings;
            }
            set
            {
                if (audioProfile.Items.Contains(value.AudioProfileName))
                    audioProfile.SelectedItem = value.AudioProfileName;
                autoDeint.Checked = value.AutomaticDeinterlacing;
                if (avsProfile.Items.Contains(value.AvsProfileName))
                    avsProfile.SelectedItem = value.AvsProfileName;
                if (containerFormat.Items.Contains(value.ContainerFormatName))
                    containerFormat.SelectedItem = value.ContainerFormatName;
                dontEncodeAudio.Checked = value.DontEncodeAudio;
                filesizeKB.Text = value.Filesize.ToString();
                horizontalResolution.Value = value.OutputResolution;
                signalAR.Checked = value.SignalAR;
                splitOutput.Checked = value.Split;
                splitSize.Text = value.SplitSize.ToString();
                if (filesizeComboBox.Items.Contains(value.StorageMediumName))
                    filesizeComboBox.SelectedItem = value.StorageMediumName;
                if (videoProfile.Items.Contains(value.VideoProfileName))
                    videoProfile.SelectedItem = value.VideoProfileName;
            }
        }

        private void newProfileButton_Click(object sender, EventArgs e)
        {
            string profileName = Microsoft.VisualBasic.Interaction.InputBox("Please give the profile a name", "Please give the profile a name", "", -1, -1);
            if (profileName == null)
                return;
            profileName = profileName.Trim();
            if (profileName.Length == 0)
                return;
            OneClickProfile prof = new OneClickProfile(profileName, this.Settings);
            if (this.mainForm.Profiles.AddOneClickProfile(prof))
            {
                this.playbackMethod.Items.Add(prof.Name);
                this.playbackMethod.SelectedIndex = this.playbackMethod.Items.IndexOf(prof.Name);
                this.oldOneClickProfileIndex = this.playbackMethod.SelectedIndex;
            }
            else
                MessageBox.Show("Sorry, profiles must have unique names", "Duplicate name detected", MessageBoxButtons.OK);
        }

        private void deleteProfileButton_Click(object sender, EventArgs e)
        {
            if (this.playbackMethod.SelectedIndex != -1) // if it's -1 it's bogus
            {
                string name = this.playbackMethod.SelectedItem.ToString();
                if (this.mainForm.Profiles.DeleteOneClickProfile(name))
                {
                    this.playbackMethod.BeginUpdate(); // now make GUI changes
                    this.playbackMethod.Items.Remove(name);
                    this.playbackMethod.EndUpdate();
                    this.oldOneClickProfileIndex = -1;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            /*if (this.playbackMethod.SelectedIndex != -1)
            {
                if (this.mainForm.Profiles.OneClickProfiles[this.playbackMethod.SelectedItem.ToString()] != null)
                {
                    this.mainForm.Profiles.OneClickProfiles[this.playbackMethod.SelectedItem.ToString()].Settings = this.Settings;
                    mainForm.Settings.OneClickProfileName = playbackMethod.Text;
                }
            }*/
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (playbackMethod.SelectedIndex != -1) // if it's -1 it's bogus
            {
                OneClickProfile prof = this.mainForm.Profiles.OneClickProfiles[this.playbackMethod.SelectedItem.ToString()];
                prof.Settings = this.Settings;
                /*
                if (this.oldOneClickProfileIndex != -1 && !newProfile) // -1 means it's never been touched
                    this.mainForm.Profiles.OneClickProfiles[this.playbackMethod.Items[this.oldOneClickProfileIndex].ToString()].Settings = this.Settings;
                newProfile = false;
                this.Settings = prof.Settings;
                this.oldOneClickProfileIndex = this.playbackMethod.SelectedIndex;
                this.playbackMethod.SelectAll();*/
            }
            else
            {
                MessageBox.Show("You must select a profile to update!", "No profile selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}