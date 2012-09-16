// ****************************************************************************
// 
// Copyright (C) 2005-2012 Doom9 & al
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
using System.Windows.Forms;

namespace MeGUI.packages.tools.oneclick
{
    public partial class OneClickConfigPanel : UserControl, Editable<OneClickSettings>
    {
        private MainForm mainForm;
        #region profiles
        #region AVS profiles
        private void initAvsHandler()
        {
            // Init AVS handlers
            avsProfile.Manager = mainForm.Profiles;
        }
        #endregion
        #region Video profiles
        private void initVideoHandler()
        {
            videoProfile.Manager = mainForm.Profiles;
        }
        #endregion
        #region Audio profiles
        private void initAudioHandler()
        {
            audioProfile.Manager = mainForm.Profiles;
        }
        #endregion
        #endregion
        
        public OneClickConfigPanel() 
        {
            InitializeComponent();
            mainForm = MainForm.Instance;
            // We do this because the designer will attempt to put such a long string in the resources otherwise
            containerFormatLabel.Text = "Since the possible output filetypes are not known until the input is configured, the output type cannot be configured in a profile. Instead, here is a list of known file-types. You choose which you are happy with, and MeGUI will attempt to encode to one of those on the list.";

            foreach (ContainerType t in mainForm.MuxProvider.GetSupportedContainers())
                containerTypeList.Items.Add(t.ToString());
            initAudioHandler();
            initAvsHandler();
            initVideoHandler();
        }

        #region Gettable<OneClickSettings> Members

        public OneClickSettings Settings
        {
            get
            {
                OneClickSettings val = new OneClickSettings();
                val.AudioProfileName = audioProfile.SelectedProfile.FQName;
                val.AutomaticDeinterlacing = autoDeint.Checked;
                val.AvsProfileName = avsProfile.SelectedProfile.FQName;
                val.ContainerCandidates = ContainerCandidates;
                val.AudioEncodingMode = (AudioEncodingMode)cbAudioEncoding.SelectedIndex;
                val.DontEncodeVideo = chkDontEncodeVideo.Checked;
                val.Filesize = fileSize.Value;
                val.OutputResolution = (long)horizontalResolution.Value;
                val.PrerenderVideo = preprocessVideo.Checked;
                val.SignalAR = signalAR.Checked;
                val.SplitSize = splitSize.Value;
                val.AutoCrop = autoCrop.Checked;
                val.KeepInputResolution = keepInputResolution.Checked;
                val.VideoProfileName = videoProfile.SelectedProfile.FQName;
                val.UseChaptersMarks = usechaptersmarks.Checked;
                val.DefaultWorkingDirectory = workingDirectory.Filename;
                val.DefaultOutputDirectory = outputDirectory.Filename;

                List<string> arrDefaultAudio = new List<string>();
                foreach (string s in lbDefaultAudio.Items)
                    arrDefaultAudio.Add(s);
                val.DefaultAudioLanguage = arrDefaultAudio;
                List<string> arrDefaultSubtitle = new List<string>();
                foreach (string s in lbDefaultSubtitle.Items)
                    arrDefaultSubtitle.Add(s);
                val.DefaultSubtitleLanguage = arrDefaultSubtitle;
                List<string> arrIndexerPriority = new List<string>();
                foreach (string s in lbIndexerPriority.Items)
                    arrIndexerPriority.Add(s);

                if (cbLanguageSelect.SelectedText.Equals("none"))
                    val.UseNoLanguagesAsFallback = true;
                else
                    val.UseNoLanguagesAsFallback = false;

                val.IndexerPriority = arrIndexerPriority;
                val.WorkingNameReplace = txtWorkingNameDelete.Text;
                val.WorkingNameReplaceWith = txtWorkingNameReplaceWith.Text;
                return val;
            }
            set
            {
                audioProfile.SetProfileNameOrWarn(value.AudioProfileName);
                autoDeint.Checked = value.AutomaticDeinterlacing;
                avsProfile.SetProfileNameOrWarn(value.AvsProfileName);
                ContainerCandidates = value.ContainerCandidates;
                cbAudioEncoding.SelectedIndex = (int)value.AudioEncodingMode;
                chkDontEncodeVideo.Checked = value.DontEncodeVideo;
                fileSize.Value = value.Filesize;
                horizontalResolution.Value = value.OutputResolution;
                preprocessVideo.Checked = value.PrerenderVideo;
                signalAR.Checked = value.SignalAR;
                splitSize.Value = value.SplitSize;
                autoCrop.Checked = value.AutoCrop;
                keepInputResolution.Checked = value.KeepInputResolution;
                videoProfile.SetProfileNameOrWarn(value.VideoProfileName);
                usechaptersmarks.Checked = value.UseChaptersMarks;
                workingDirectory.Filename = value.DefaultWorkingDirectory;
                outputDirectory.Filename = value.DefaultOutputDirectory;
                txtWorkingNameDelete.Text = value.WorkingNameReplace;
                txtWorkingNameReplaceWith.Text = value.WorkingNameReplaceWith;
                
                List<string> arrNonDefaultAudio = new List<string>(LanguageSelectionContainer.Languages.Keys);
                arrNonDefaultAudio.Add("[none]");
                foreach (string strLanguage in value.DefaultAudioLanguage)
                    arrNonDefaultAudio.Remove(strLanguage);
                List<string> arrNonDefaultSubtitle = new List<string>(LanguageSelectionContainer.Languages.Keys);
                arrNonDefaultSubtitle.Add("[none]");
                foreach (string strLanguage in value.DefaultSubtitleLanguage)
                    arrNonDefaultSubtitle.Remove(strLanguage);

                lbDefaultAudio.Items.Clear();
                lbDefaultAudio.Items.AddRange(value.DefaultAudioLanguage.ToArray());
                lbDefaultAudio_SelectedIndexChanged(null, null);
                lbNonDefaultAudio.Items.Clear();
                lbNonDefaultAudio.Items.AddRange(arrNonDefaultAudio.ToArray());
                lbNonDefaultAudio_SelectedIndexChanged(null, null);

                lbDefaultSubtitle.Items.Clear();
                lbDefaultSubtitle.Items.AddRange(value.DefaultSubtitleLanguage.ToArray());
                lbDefaultSubtitle_SelectedIndexChanged(null, null);
                lbNonDefaultSubtitle.Items.Clear();
                lbNonDefaultSubtitle.Items.AddRange(arrNonDefaultSubtitle.ToArray());
                lbNonDefaultSubtitle_SelectedIndexChanged(null, null);

                lbIndexerPriority.Items.Clear();
                lbIndexerPriority.Items.AddRange(value.IndexerPriority.ToArray());

                if (!value.UseNoLanguagesAsFallback)
                    cbLanguageSelect.SelectedItem = "all";
                else
                    cbLanguageSelect.SelectedItem = "none";
            }
        }

        #endregion

        private string[] ContainerCandidates
        {
            get
            {
                string[] val = new string[containerTypeList.CheckedItems.Count];
                containerTypeList.CheckedItems.CopyTo(val, 0);
                return val;
            }
            set
            {
                for (int i = 0; i < containerTypeList.Items.Count; i++)
                    containerTypeList.SetItemChecked(i, false);

                foreach (string val in value)
                {
                    int index = containerTypeList.Items.IndexOf(val);
                    if (index > -1)
                        containerTypeList.SetItemChecked(index, true);
                }
            }
        }

        private void keepInputResolution_CheckedChanged(object sender, EventArgs e)
        {
            if (keepInputResolution.Checked)
                horizontalResolution.Enabled = autoCrop.Enabled = signalAR.Enabled = false;
            else
                horizontalResolution.Enabled = autoCrop.Enabled = signalAR.Enabled = true;
        }

        private void videoProfile_SelectedProfileChanged(object sender, EventArgs e)
        {
            if (videoProfile.SelectedProfile.FQName.StartsWith("x264") && !chkDontEncodeVideo.Checked)
                usechaptersmarks.Enabled = true;
            else
                usechaptersmarks.Enabled = false;
        }

        private void chkDontEncodeVideo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDontEncodeVideo.Checked)
            {
                horizontalResolution.Enabled = autoCrop.Enabled = signalAR.Enabled = videoProfile.Enabled = false;
                usechaptersmarks.Enabled = keepInputResolution.Enabled = preprocessVideo.Enabled = false;
                autoDeint.Enabled = fileSize.Enabled = avsProfile.Enabled = false;
            }
            else
            {
                videoProfile.Enabled = keepInputResolution.Enabled = preprocessVideo.Enabled = true;
                autoDeint.Enabled = fileSize.Enabled = avsProfile.Enabled = true;
                if (videoProfile.SelectedProfile.FQName.StartsWith("x264"))
                    usechaptersmarks.Enabled = true;
                else
                    usechaptersmarks.Enabled = false;
                if (keepInputResolution.Checked)
                    horizontalResolution.Enabled = autoCrop.Enabled = signalAR.Enabled = false;
                else
                    horizontalResolution.Enabled = autoCrop.Enabled = signalAR.Enabled = true;
            }
        }

        private void dontEncodeAudio_CheckedChanged(object sender, EventArgs e)
        {
            audioProfile.Enabled = !cbAudioEncoding.SelectedText.Equals("never");
        }

        private void btnAddAudio_Click(object sender, EventArgs e)
        {
            List<string> arrAudio = new List<string>();
            foreach (string s in lbNonDefaultAudio.SelectedItems)
            {
                lbDefaultAudio.Items.Add(s);
                arrAudio.Add(s);
            }
            foreach (string s in arrAudio)
                lbNonDefaultAudio.Items.Remove(s);
        }

        private void btnRemoveAudio_Click(object sender, EventArgs e)
        {
            List<string> arrAudio = new List<string>();
            foreach (string s in lbDefaultAudio.SelectedItems)
            {
                lbNonDefaultAudio.Items.Add(s);
                arrAudio.Add(s);
            }
            foreach (string s in arrAudio)
                lbDefaultAudio.Items.Remove(s);
        }

        private void btnAudioUp_Click(object sender, EventArgs e)
        {
            int iPos = lbDefaultAudio.SelectedIndex;
            if (iPos < 1)
                return;

            object o = lbDefaultAudio.SelectedItem;
            lbDefaultAudio.Items.RemoveAt(iPos);
            lbDefaultAudio.Items.Insert(iPos - 1, o);
            lbDefaultAudio.SelectedIndex = iPos - 1;
        }

        private void btnAudioDown_Click(object sender, EventArgs e)
        {
            int iPos = lbDefaultAudio.SelectedIndex;
            if (iPos < 0 || iPos > lbDefaultAudio.Items.Count - 2)
                return;

            object o = lbDefaultAudio.SelectedItem;
            lbDefaultAudio.Items.RemoveAt(iPos);
            lbDefaultAudio.Items.Insert(iPos + 1, o);
            lbDefaultAudio.SelectedIndex = iPos + 1;
        }

        private void lbDefaultAudio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbDefaultAudio.SelectedIndex < 0)
            {
                btnRemoveAudio.Enabled = btnAudioUp.Enabled = btnAudioDown.Enabled = false;
                return;
            }
            btnRemoveAudio.Enabled = true;
            if (lbDefaultAudio.SelectedIndex == 0)
                btnAudioUp.Enabled = false;
            else
                btnAudioUp.Enabled = true;
            if (lbDefaultAudio.SelectedIndex == lbDefaultAudio.Items.Count - 1)
                btnAudioDown.Enabled = false;
            else
                btnAudioDown.Enabled = true;
        }

        private void lbNonDefaultAudio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbNonDefaultAudio.SelectedIndex < 0)
                btnAddAudio.Enabled = false;
            else
                btnAddAudio.Enabled = true;
        }

        private void btnAddSubtitle_Click(object sender, EventArgs e)
        {
            List<string> arrSubtitle = new List<string>();
            foreach (string s in lbNonDefaultSubtitle.SelectedItems)
            {
                lbDefaultSubtitle.Items.Add(s);
                arrSubtitle.Add(s);
            }
            foreach (string s in arrSubtitle)
                lbNonDefaultSubtitle.Items.Remove(s);
        }

        private void btnRemoveSubtitle_Click(object sender, EventArgs e)
        {
            List<string> arrSubtitle = new List<string>();
            foreach (string s in lbDefaultSubtitle.SelectedItems)
            {
                lbNonDefaultSubtitle.Items.Add(s);
                arrSubtitle.Add(s);
            }
            foreach (string s in arrSubtitle)
                lbDefaultSubtitle.Items.Remove(s);
        }

        private void btnSubtitleUp_Click(object sender, EventArgs e)
        {
            int iPos = lbDefaultSubtitle.SelectedIndex;
            if (iPos < 1)
                return;

            object o = lbDefaultSubtitle.SelectedItem;
            lbDefaultSubtitle.Items.RemoveAt(iPos);
            lbDefaultSubtitle.Items.Insert(iPos - 1, o);
            lbDefaultSubtitle.SelectedIndex = iPos - 1;
        }

        private void btnSubtitleDown_Click(object sender, EventArgs e)
        {
            int iPos = lbDefaultSubtitle.SelectedIndex;
            if (iPos < 0 || iPos > lbDefaultSubtitle.Items.Count - 2)
                return;

            object o = lbDefaultSubtitle.SelectedItem;
            lbDefaultSubtitle.Items.RemoveAt(iPos);
            lbDefaultSubtitle.Items.Insert(iPos + 1, o);
            lbDefaultSubtitle.SelectedIndex = iPos + 1;
        }

        private void lbDefaultSubtitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbDefaultSubtitle.SelectedIndex < 0)
            {
                btnRemoveSubtitle.Enabled = btnSubtitleUp.Enabled = btnSubtitleDown.Enabled = false;
                return;
            }
            btnRemoveSubtitle.Enabled = true;
            if (lbDefaultSubtitle.SelectedIndex == 0)
                btnSubtitleUp.Enabled = false;
            else
                btnSubtitleUp.Enabled = true;
            if (lbDefaultSubtitle.SelectedIndex == lbDefaultSubtitle.Items.Count - 1)
                btnSubtitleDown.Enabled = false;
            else
                btnSubtitleDown.Enabled = true;
        }

        private void lbNonDefaultSubtitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbNonDefaultSubtitle.SelectedIndex < 0)
                btnAddSubtitle.Enabled = false;
            else
                btnAddSubtitle.Enabled = true;
        }

        private void lbIndexerPriority_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                int iPos = lbIndexerPriority.SelectedIndex;
                if (iPos < 1)
                    return;

                object o = lbIndexerPriority.SelectedItem;
                lbIndexerPriority.Items.RemoveAt(iPos);
                lbIndexerPriority.Items.Insert(iPos - 1, o);
                lbIndexerPriority.SelectedIndex = iPos - 1;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                int iPos = lbIndexerPriority.SelectedIndex;
                if (iPos < 0 || iPos > lbIndexerPriority.Items.Count - 2)
                    return;

                object o = lbIndexerPriority.SelectedItem;
                lbIndexerPriority.Items.RemoveAt(iPos);
                lbIndexerPriority.Items.Insert(iPos + 1, o);
                lbIndexerPriority.SelectedIndex = iPos + 1;
                e.SuppressKeyPress = true;
            }
        }
    }
}
