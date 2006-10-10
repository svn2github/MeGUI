using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.plugins.interfaces
{
    public class ConfigurationWindow<TSettings, TProfileSettings> : Form
        where TProfileSettings : GenericSettings
        where TSettings : TProfileSettings, new()
    {
        ProfileManager profileManager;
        private Gettable<TProfileSettings> s;
        protected GroupBox profilesGroupbox;
        private Button updateButton;
        private Button loadDefaultsButton;
        private ComboBox videoProfile;
        private Button newVideoProfileButton;
        private Panel panel1;
        protected Button cancelButton;
        protected Button okButton;
        private Button deleteVideoProfileButton;
        private string initialProfile;

        public TProfileSettings Settings
        {
            get { return s.Settings; }
            set { s.Settings = value; }
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

        private void InitializeComponent()
        {
            this.profilesGroupbox = new System.Windows.Forms.GroupBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.loadDefaultsButton = new System.Windows.Forms.Button();
            this.videoProfile = new System.Windows.Forms.ComboBox();
            this.newVideoProfileButton = new System.Windows.Forms.Button();
            this.deleteVideoProfileButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.profilesGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // profilesGroupbox
            // 
            this.profilesGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.profilesGroupbox.Controls.Add(this.updateButton);
            this.profilesGroupbox.Controls.Add(this.loadDefaultsButton);
            this.profilesGroupbox.Controls.Add(this.videoProfile);
            this.profilesGroupbox.Controls.Add(this.newVideoProfileButton);
            this.profilesGroupbox.Controls.Add(this.deleteVideoProfileButton);
            this.profilesGroupbox.Location = new System.Drawing.Point(12, 405);
            this.profilesGroupbox.Name = "profilesGroupbox";
            this.profilesGroupbox.Size = new System.Drawing.Size(500, 48);
            this.profilesGroupbox.TabIndex = 44;
            this.profilesGroupbox.TabStop = false;
            this.profilesGroupbox.Text = "Profiles";
            // 
            // updateButton
            // 
            this.updateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updateButton.Location = new System.Drawing.Point(314, 18);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(50, 23);
            this.updateButton.TabIndex = 15;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // loadDefaultsButton
            // 
            this.loadDefaultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadDefaultsButton.Location = new System.Drawing.Point(391, 18);
            this.loadDefaultsButton.Name = "loadDefaultsButton";
            this.loadDefaultsButton.Size = new System.Drawing.Size(103, 23);
            this.loadDefaultsButton.TabIndex = 14;
            this.loadDefaultsButton.Text = "Load Defaults";
            this.loadDefaultsButton.UseVisualStyleBackColor = true;
            this.loadDefaultsButton.Click += new System.EventHandler(this.loadDefaultsButton_Click);
            // 
            // videoProfile
            // 
            this.videoProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.videoProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoProfile.Location = new System.Drawing.Point(8, 18);
            this.videoProfile.Name = "videoProfile";
            this.videoProfile.Size = new System.Drawing.Size(196, 21);
            this.videoProfile.Sorted = true;
            this.videoProfile.TabIndex = 11;
            this.videoProfile.SelectedIndexChanged += new System.EventHandler(this.videoProfile_SelectedIndexChanged);
            // 
            // newVideoProfileButton
            // 
            this.newVideoProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newVideoProfileButton.Location = new System.Drawing.Point(268, 18);
            this.newVideoProfileButton.Name = "newVideoProfileButton";
            this.newVideoProfileButton.Size = new System.Drawing.Size(40, 23);
            this.newVideoProfileButton.TabIndex = 12;
            this.newVideoProfileButton.Text = "New";
            this.newVideoProfileButton.Click += new System.EventHandler(this.newVideoProfileButton_Click);
            // 
            // deleteVideoProfileButton
            // 
            this.deleteVideoProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteVideoProfileButton.Location = new System.Drawing.Point(210, 18);
            this.deleteVideoProfileButton.Name = "deleteVideoProfileButton";
            this.deleteVideoProfileButton.Size = new System.Drawing.Size(48, 23);
            this.deleteVideoProfileButton.TabIndex = 13;
            this.deleteVideoProfileButton.Text = "Delete";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(515, 399);
            this.panel1.TabIndex = 45;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(462, 459);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 47;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(403, 459);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(40, 23);
            this.okButton.TabIndex = 46;
            this.okButton.Text = "OK";
            // 
            // ConfigurationWindow
            // 
            this.ClientSize = new System.Drawing.Size(518, 490);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.profilesGroupbox);
            this.Name = "ConfigurationWindow";
            this.Load += new System.EventHandler(this.ConfigurationWindow_Load);
            this.profilesGroupbox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public ConfigurationWindow(ProfileManager p, Control sPanel, Gettable<TProfileSettings> s, string initialProfile)
            : this(p, sPanel, s, initialProfile, new TSettings().getSettingsType()) { }
        
        public ConfigurationWindow(ProfileManager p, Control sPanel, Gettable<TProfileSettings> s, string initialProfile, string title)
        {
            this.initialProfile = initialProfile;
            InitializeComponent();
            this.Text = title + " configuration dialog";
            this.profileManager = p;
            this.s = s;
            System.Drawing.Size size = Size;
            size.Height += sPanel.Height - panel1.Height;
            size.Width += sPanel.Width - panel1.Width;
            Size = size;
            sPanel.Dock = DockStyle.Fill;
            panel1.Controls.Add(sPanel);
        }

        private void ConfigurationWindow_Load(object sender, EventArgs e)
        {
            int index = -1;
            foreach (Profile prof in this.profileManager.Profiles(new TSettings().getSettingsType()).Values)
            {
                if (prof.BaseSettings is TSettings) // those are the profiles we're interested in
                {
                    this.videoProfile.Items.Add(prof);
                    if (prof.Name == initialProfile)
                        index = videoProfile.Items.IndexOf(prof);
                }
            }
            if (index != -1)
                this.videoProfile.SelectedIndex = index;
        }
        private void loadDefaultsButton_Click(object sender, EventArgs e)
        {
            s.Settings = new TSettings();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            GenericProfile<TProfileSettings> prof = (GenericProfile<TProfileSettings>)this.videoProfile.SelectedItem;
            if (prof == null)
            {
                MessageBox.Show("You must select a profile to update!", "No profile selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            prof.Settings = s.Settings;
        }

        private void newVideoProfileButton_Click(object sender, EventArgs e)
        {
            string profileName = Microsoft.VisualBasic.Interaction.InputBox("Please give the profile a name", "Please give the profile a name", "", -1, -1);
            if (profileName == null)
                return;
            profileName = profileName.Trim();
            if (profileName.Length == 0)
                return;
            GenericProfile<TProfileSettings> prof = new GenericProfile<TProfileSettings>(profileName, s.Settings);
            if (this.profileManager.AddProfile(prof))
            {
                this.videoProfile.Items.Add(prof);
                this.videoProfile.SelectedItem = prof;
//                this.oldVideoProfile = prof;
            }
            else
                MessageBox.Show("Sorry, profiles must have unique names", "Duplicate name detected", MessageBoxButtons.OK);
        }

        private void videoProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenericProfile<TProfileSettings> prof = this.videoProfile.SelectedItem as GenericProfile<TProfileSettings>;
            if (prof == null)
                return;
            this.Settings = prof.Settings;
        }
    }

    public sealed class ProfileMetaType { }

    /** This is the base type for any kind of settings which can be stored in a profile.
     * Nothing need be defined, but this interface must be inherited for type checking reasons.*/
    public interface GenericSettings 
    {
        /// <summary>
        /// Deep-clones the settings
        /// </summary>
        /// <returns></returns>
        GenericSettings baseClone();

        /// <summary>
        /// Returns the meta type of a profile. This is used as a lookup in the ProfileManager class
        /// to group like profile types. There should be one meta-type per settings type.
        /// </summary>
        /// <returns></returns>
        string getSettingsType();
    }

    public interface Gettable<TSettings>
    {
        TSettings Settings
        {
            get;
            set;
        }
    }

    /** This is the base type for a settings configuration panel. This must be extended
     *  to be used by the ConfigurationWindow, which wraps this with the profile management code. */
/*    public class SettingsPanel<TSettings> : System.Windows.Forms.Control
        where TSettings : GenericSettings
    {
        /** Gets and sets the settings that this panel displays. Must be overridden. This is called by ConfigurationWindow when the user updates/changes profiles */
       /* public TSettings Settings
        {
            get { throw new Exception("Settings.get must be overridden"); }
            set { throw new Exception("Settings.set must be overridden"); }
        }
    }*/
}
