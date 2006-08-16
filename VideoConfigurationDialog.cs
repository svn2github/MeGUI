using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class VideoConfigurationDialog : Form
    {
        #region variables
        private bool updating = false;
        private ProfileManager profileManager;
        private double bytesPerFrame;
        private bool advancedToolTips;
        protected int lastEncodingMode = 0;
        private VideoProfile oldVideoProfile = null;
        private string initialProfile;
        private bool safeProfileAlteration;

        private bool loaded, generateCommandline;
        private int introEndFrame = 0, creditsStartFrame = 0;
        private CommandLineGenerator gen;
        private string input = "", output = "", encoderPath = "";
        #endregion
        #region start / stop
        public VideoConfigurationDialog()
            : this(new ProfileManager(""), "", false)
        { }

        public VideoConfigurationDialog(ProfileManager manager, string initialProfile, bool safeProfileAlteration)
        {
            loaded = false;
            generateCommandline = false;
            InitializeComponent();
            zonesControl.UpdateGUIEvent += new ZonesControl.UpdateConfigGUI(genericUpdate);
            this.profileManager = manager;
            this.initialProfile = initialProfile;
            this.safeProfileAlteration = safeProfileAlteration;
            gen = new CommandLineGenerator();
        }

        private void VideoConfigurationDialog_Load(object sender, EventArgs e)
        {
            int index = -1;
            foreach (VideoProfile prof in this.profileManager.VideoProfiles.Values)
            {
                if (isValidSettings(prof.Settings)) // those are the profiles we're interested in
                {
                    this.videoProfile.Items.Add(prof);
                    if (prof.Name == initialProfile)
                        index = videoProfile.Items.IndexOf(prof);
                }
            }
            if (index != -1)
                this.videoProfile.SelectedIndex = index;
            zonesControl.IntroEndFrame = introEndFrame;
            zonesControl.CreditsStartFrame = creditsStartFrame;
            loaded = true;
            doCodecSpecificLoadAdjustments();
            genericUpdate();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            zonesControl.closePlayer();
            base.OnClosing(e);
        }
        #endregion
        #region codec specific adjustments
        /// <summary>
        /// The method by which codecs can do their pre-commandline generation adjustments (eg tri-state adjustment).
        /// </summary>
        protected virtual void doCodecSpecificAdjustments() { }

        /// <summary>
        /// The method by which codecs can add things to the Load event
        /// </summary>
        protected virtual void doCodecSpecificLoadAdjustments() { }
        
        /// <summary>
        /// Returns whether settings is a valid settings object for this instance. Should be implemented by one line:
        /// return (settings is xxxxSettings);
        /// </summary>
        /// <param name="settings">The settings to check</param>
        /// <returns>Whether the settings are valid</returns>
        protected virtual bool isValidSettings(VideoCodecSettings settings)
        {
            throw new Exception("A bug in the program -- ProfilableConfigurationDialog.isValidSettings(GenericSettings) is not overridden");
        }

        /// <summary>
        /// Returns a new instance of the codec settings. This must be specific to the type of the config dialog, so
        /// that it can be set with the Settings.set property.
        /// </summary>
        /// <returns>A new instance of xxxSettings</returns>
        protected virtual VideoCodecSettings defaultSettings()
        {
            throw new Exception("A bug in the program -- ProfilableConfigurationDialog.defaultSettings() is not overridden");
        }
        #endregion
        #region showCommandline
        protected void showCommandLine()
        {
            if (!loaded)
                return;
            if (updating)
                return;
            updating = true;

            doCodecSpecificAdjustments();

            if (generateCommandline) // There's no point in generating the commandline if it isn't visible
                this.commandline.Text = encoderPath + " " + CommandLineGenerator.generateVideoCommandline((VideoCodecSettings)this.Settings, this.input, this.output, -1, -1);
            updating = false;
        }
        #endregion
        #region GUI events
        private void commandlineVisible_CheckedChanged(object sender, EventArgs e)
        {
            if (commandlineVisible.Checked)
            {
                this.Size = new Size(this.Width, this.Height + commandline.Height + 10);
                generateCommandline = true;
                showCommandLine();
            }
            else
            {
                generateCommandline = false;
                this.Size = new Size(this.Width, this.Height - commandline.Height - 10);
            }
        }
        protected void genericUpdate()
        {
            showCommandLine();
        }
        #endregion
        #region properties

        public bool AdvancedToolTips
        {
            get { return advancedToolTips; }
            set { advancedToolTips = value; }
        }

        public double BytesPerFrame
        {
            get { return bytesPerFrame; }
            set { bytesPerFrame = value; }
        }

        public virtual VideoCodecSettings Settings
        {
            get
            {
                return null;
            }
            set
            { }
        }
        /// <summary>
        /// sets the video input (for commandline generation and zone previews)
        /// </summary>
        public string Input
        {
            set
            {
                this.input = value;
                zonesControl.Input = value;
            }
        }
        /// <summary>
        ///  sets the audio output (for commandline generation)
        /// </summary>
        public string Output
        {
            set { this.output = value; }
        }
        /// <summary>
        /// sets the path of besweet
        /// </summary>
        public string EncoderPath
        {
            set { this.encoderPath = value; }
        }
        /// <summary>
        /// gets / sets the start frame of the credits
        /// </summary>
        public int CreditsStartFrame
        {
            get { return this.creditsStartFrame; }
            set { creditsStartFrame = value; }
        }
        /// <summary>
        /// gets / sets the end frame of the intro
        /// </summary>
        public int IntroEndFrame
        {
            get { return this.introEndFrame; }
            set { introEndFrame = value; }
        }
        /// <summary>
        /// gets / sets the zones of the video
        /// </summary>
        public Zone[] Zones
        {
            get { return zonesControl.Zones; }
            set { zonesControl.Zones = value; }
        }
        /// <summary>
        /// gets the name of the currently selected profile
        /// </summary>
        public string CurrentProfile
        {
            get
            {
                return videoProfile.Text;
            }
        }

        #endregion
        #region profiles
        /// <summary>
        /// handles the selection of a profile from the list
        /// the profile is looked up from the profiles Hashtable (it uses the name as unique key), then
        /// the settings from the new profile are displayed in the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profile_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            VideoProfile prof = this.videoProfile.SelectedItem as VideoProfile;
            if (prof == null)
                return;
            this.Settings = prof.Settings;
            /*
            // update the still selected profile with the current settings
            if (!object.ReferenceEquals(prof, oldVideoProfile)) 
            {
                if (oldVideoProfile != null)
                    oldVideoProfile.Settings = this.Settings;
                this.oldVideoProfile = prof;
                this.Settings = prof.Settings;
            }*/

            /*if (this.videoProfile.SelectedIndex != -1) // if it's -1 it's bogus
            {
                VideoProfile prof = this.profileManager.VideoProfiles[this.videoProfile.Items[this.videoProfile.SelectedIndex].ToString()];
                if (this.oldVideoProfileIndex != -1 && !this.newProfile) // -1 means it's never been touched
                    this.profileManager.VideoProfiles[this.videoProfile.Items[this.oldVideoProfileIndex].ToString()].Settings = this.Settings;
                newProfile = false;
                this.Settings = prof.Settings;
                this.oldVideoProfileIndex = this.videoProfile.SelectedIndex;
                this.videoProfile.SelectAll();
            }*/
        }
        /// <summary>
        /// creates a new profile if the entered name does not match an already existing profile
        /// profiles are attached at the bottom of the profile dropdown list and also stored
        /// in the profiles hashtable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newProfileButton_Click(object sender, System.EventArgs e)
        {
            string profileName = Microsoft.VisualBasic.Interaction.InputBox("Please give the profile a name", "Please give the profile a name", "", -1, -1);
            if (profileName == null)
                return;
            profileName = profileName.Trim();
            if (profileName.Length == 0)
                return;
            VideoProfile prof = new VideoProfile(profileName, this.Settings);
            if (this.profileManager.AddVideoProfile(prof))
            {
                this.videoProfile.Items.Add(prof);
                this.videoProfile.SelectedItem = prof;
                this.oldVideoProfile = prof;
            }
            else
                MessageBox.Show("Sorry, profiles must have unique names", "Duplicate name detected", MessageBoxButtons.OK);
        }
        /// <summary>
        /// deletes the currently selected profile
        /// if a profile is selected from the dropdown, its profile object is extracted from the profiles hashtable
        /// then it is removed from the hashtable, dropdown list, and the profile has already been saved to the harddisk
        /// it is also removed from the profiles directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteProfileButton_Click(object sender, System.EventArgs e)
        {
            VideoProfile prof = this.videoProfile.SelectedItem as VideoProfile;
            if (prof == null)
                return;
            if (!this.profileManager.DeleteVideoProfile(prof.Name))
                return;
            this.videoProfile.Items.Remove(prof);
            this.oldVideoProfile = null;
        }
        /// <summary>
        /// loads the default settings for the current codec
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadDefaultsButton_Click(object sender, EventArgs e)
        {
            Settings = defaultSettings();
        }
        /// <summary>
        /// accepts the changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            /*if (this.videoProfile.SelectedIndex != -1)
            {
                VideoProfile pold = (VideoProfile)videoProfile.SelectedItem;
                const string tweakSuffix = "Tweaked";
                bool profileAltered = this.Settings.IsAltered(pold.Settings);
                if (safeProfileAlteration && profileAltered && pold.Name.IndexOf(tweakSuffix) == -1)
                {
                    string newName = pold.Name + tweakSuffix;
                    if (profileManager.VideoProfiles.ContainsKey(newName))
                        profileManager.VideoProfiles[newName].Settings = Settings;
                    else
                    {
                        profileManager.AddVideoProfile(new VideoProfile(newName, Settings));
                        videoProfile.Items.Add(profileManager.VideoProfiles[newName]);
                    }
                    this.oldVideoProfile = null;
                    this.videoProfile.SelectedItem = profileManager.VideoProfiles[newName];
                }
                else
                {
                    pold.Settings = this.Settings;
                }
            }*/
        }
        /// <summary>
        /// updates the currently selected profile with the current settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateButton_Click(object sender, EventArgs e)
        {
            VideoProfile prof = this.videoProfile.SelectedItem as VideoProfile;
            if (prof == null)
            {
                MessageBox.Show("You must select a profile to update!", "No profile selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            prof.Settings = this.Settings;
        }
        #endregion
    }
}