using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.plugins.interfaces;
using System.Diagnostics;
using MeGUI.core.util;

namespace MeGUI.core.gui
{
    public partial class ProfileConfigurationWindow<TSettings, TPanel> : Form
        where TSettings : GenericSettings, new()
        where TPanel : Control, Editable<TSettings>
    {
        private GenericProfile<TSettings> scratchPadProfile
        {
            get
            {
                return byName(ProfileManager.ScratchPadName);
            }
        }

        private TPanel s;

        private TSettings Settings
        {
            get { return s.Settings; }
            set { s.Settings = value; }
        }

        /// <summary>
        /// gets the name of the currently selected profile
        /// </summary>
        public string CurrentProfileName
        {
            get
            {
                return SelectedProfile.Name;
            }
        }



/*        public ProfileConfigurationWindow(ProfileManager p, Control sPanel, Gettable<TSettings> s, string initialProfile)
            : this(p, sPanel, s, initialProfile, new TSettings().getSettingsType()) { }*/

        public ProfileConfigurationWindow(TPanel t, string title)
        {
            InitializeComponent();
            this.Text = title + " configuration dialog";
            this.s = t;
            System.Drawing.Size size = Size;
            size.Height += t.Height - panel1.Height;
            size.Width += Math.Max(t.Width - panel1.Width, 0);
            Size = size;
            t.Dock = DockStyle.Fill;
            panel1.Controls.Add(t);
        }

        private void loadDefaultsButton_Click(object sender, EventArgs e)
        {
            s.Settings = new TSettings();
            putSettingsInScratchpad();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            GenericProfile<TSettings> prof = SelectedProfile;
            prof.Settings = s.Settings;
        }

        private void newVideoProfileButton_Click(object sender, EventArgs e)
        {
            string profileName = InputBox.Show("Please give the preset a name", "Please give the preset a name", "");
            if (profileName == null)
                return;
            profileName = profileName.Trim();
            if (profileName.Length == 0)
                return;
            GenericProfile<TSettings> prof = new GenericProfile<TSettings>(profileName, s.Settings);
            if (byName(profileName) != null)
                MessageBox.Show("Sorry, presets must have unique names", "Duplicate preset name", MessageBoxButtons.OK);
            else
            {
                videoProfile.Items.Add(prof);
                videoProfile.SelectedItem = prof;
            }
        }

        public GenericProfile<TSettings> SelectedProfile
        {
            get { return (GenericProfile<TSettings>)videoProfile.SelectedItem; }
            set
            {
                // We can't just set videoProfile.SelectedItem = value, because the profiles in videoProfile are cloned
                foreach (GenericProfile<TSettings> p in videoProfile.Items)
                    if (p.Name == value.Name)
                    {
                        videoProfile.SelectedItem = p;
                        return;
                    }
            }
        }

        public Tuple<IEnumerable<GenericProfile<TSettings>>, GenericProfile<TSettings>> Profiles
        {
            get
            {
                return new Tuple<IEnumerable<GenericProfile<TSettings>>, GenericProfile<TSettings>>(
                    Util.CastAll<GenericProfile<TSettings>>(videoProfile.Items),
                    SelectedProfile);
            }

            set
            {
                videoProfile.Items.Clear();
                foreach (GenericProfile<TSettings> p in value.a)
                    videoProfile.Items.Add(p.clone());

                SelectedProfile = value.b;
            }
        }

        private GenericProfile<TSettings> byName(string profileName)
        {
            foreach (GenericProfile<TSettings> p in Profiles.a)
                if (p.Name == profileName)
                    return p;

            return null;
        }

        private void videoProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Settings = SelectedProfile.Settings;
        }

        private void deleteVideoProfileButton_Click(object sender, EventArgs e)
        {
            GenericProfile<TSettings> prof = (GenericProfile<TSettings>)this.videoProfile.SelectedItem;
            Debug.Assert(prof != null);

            videoProfile.Items.Remove(prof);
            
            if (prof.Name == ProfileManager.ScratchPadName && videoProfile.Items.Count > 0)
                videoProfile.SelectedIndex = 0;
            else
                loadDefaultsButton_Click(null, null);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Profile prof = SelectedProfile;
            if (prof.Name == ProfileManager.ScratchPadName)
                prof.BaseSettings = Settings;
            else if (!Settings.Equals(prof.BaseSettings))
            {
                switch (MessageBox.Show("Profile has been changed. Update the selected profile? (Pressing No will save your changes to the scratchpad)",
                    "Profile update", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        prof.BaseSettings = Settings;
                        break;

                    case DialogResult.No:
                        putSettingsInScratchpad();
                        break;

                    case DialogResult.Cancel:
                        return;
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void putSettingsInScratchpad()
        {
            TSettings s = Settings;
            GenericProfile<TSettings> p = scratchPadProfile;

            if (p == null)
            {
                p = new GenericProfile<TSettings>(ProfileManager.ScratchPadName, s);
                videoProfile.Items.Add(p);
            }

            p.Settings = s;
            videoProfile.SelectedItem = p;
        }
    }




}
