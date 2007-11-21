using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace MeGUI.core.plugins.interfaces
{
    public class ConfigurationWindow<TSettings, TProfileSettings> : Form
        where TProfileSettings : GenericSettings
        where TSettings : TProfileSettings, new()
    {
        ProfileManager profileManager;
        private Gettable<TProfileSettings> s;
        protected GroupBox profilesGroupbox;
        private Panel panel1;
        protected Button cancelButton;
        protected Button okButton;
        private Button updateButton;
        private Button loadDefaultsButton;
        private ComboBox videoProfile;
        private Button newVideoProfileButton;
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
        public string CurrentProfileName
        {
            get
            {
                return videoProfile.Text;
            }
        }

        private bool createTempSettings = false;
        /// <summary>
        /// Returns true if the selected settings should be copied into a temporary location and no profile selected when returned.
        /// Returns false if the selected profile here should be selected when this returns.
        /// </summary>
        public bool CreateTempSettings
        {
            get
            {
                return createTempSettings;
            }
        }

        public GenericProfile<TProfileSettings> CurrentProfile
        {
            get
            {
                return (GenericProfile<TProfileSettings>)videoProfile.SelectedItem;
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
            this.profilesGroupbox.Location = new System.Drawing.Point(6, 405);
            this.profilesGroupbox.Name = "profilesGroupbox";
            this.profilesGroupbox.Size = new System.Drawing.Size(400, 48);
            this.profilesGroupbox.TabIndex = 44;
            this.profilesGroupbox.TabStop = false;
            this.profilesGroupbox.Text = "Profiles";
            // 
            // updateButton
            // 
            this.updateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updateButton.Location = new System.Drawing.Point(235, 18);
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
            this.loadDefaultsButton.Location = new System.Drawing.Point(291, 18);
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
            this.videoProfile.Size = new System.Drawing.Size(121, 21);
            this.videoProfile.Sorted = true;
            this.videoProfile.TabIndex = 11;
            this.videoProfile.SelectedIndexChanged += new System.EventHandler(this.videoProfile_SelectedIndexChanged);
            // 
            // newVideoProfileButton
            // 
            this.newVideoProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newVideoProfileButton.Location = new System.Drawing.Point(189, 18);
            this.newVideoProfileButton.Name = "newVideoProfileButton";
            this.newVideoProfileButton.Size = new System.Drawing.Size(40, 23);
            this.newVideoProfileButton.TabIndex = 12;
            this.newVideoProfileButton.Text = "New";
            this.newVideoProfileButton.Click += new System.EventHandler(this.newVideoProfileButton_Click);
            // 
            // deleteVideoProfileButton
            // 
            this.deleteVideoProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteVideoProfileButton.Location = new System.Drawing.Point(135, 18);
            this.deleteVideoProfileButton.Name = "deleteVideoProfileButton";
            this.deleteVideoProfileButton.Size = new System.Drawing.Size(48, 23);
            this.deleteVideoProfileButton.TabIndex = 13;
            this.deleteVideoProfileButton.Text = "Delete";
            this.deleteVideoProfileButton.Click += new System.EventHandler(this.deleteVideoProfileButton_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(411, 399);
            this.panel1.TabIndex = 45;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(358, 459);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 47;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(312, 459);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(40, 23);
            this.okButton.TabIndex = 46;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // ConfigurationWindow
            // 
            this.ClientSize = new System.Drawing.Size(414, 490);
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
            size.Width += Math.Max(sPanel.Width - panel1.Width, 0);
            Size = size;
            sPanel.Dock = DockStyle.Fill;
            panel1.Controls.Add(sPanel);
        }

        private void ConfigurationWindow_Load(object sender, EventArgs e)
        {
            Profile selected = null;
            foreach (Profile prof in this.profileManager.Profiles(new TSettings().getSettingsType()).Values)
            {
                if (prof.BaseSettings is TSettings) // those are the profiles we're interested in
                {
                    this.videoProfile.Items.Add(prof);
                    if (prof.Name == initialProfile)
                        selected = prof;
                }
            }
            this.videoProfile.SelectedItem = selected;
        }
        private void loadDefaultsButton_Click(object sender, EventArgs e)
        {
            s.Settings = new TSettings();
            videoProfile.SelectedIndex = -1;
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

        private void deleteVideoProfileButton_Click(object sender, EventArgs e)
        {
            GenericProfile<TProfileSettings> prof = (GenericProfile<TProfileSettings>)this.videoProfile.SelectedItem;
            if (prof == null)
            {
                MessageBox.Show("You must select a profile to delete!", "No profile selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            profileManager.DeleteProfile(prof);
            videoProfile.Items.Remove(prof);
            loadDefaultsButton_Click(null, null);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
        }
    }


    
    /** This is the base type for any kind of settings which can be stored in a profile.*/
    public interface GenericSettings 
    {
        /************************************************************************************
         *                   Classes implementing GenericSettings must                      *
         *                    ensure that object.Equals(object other)                       *
         *                     is overridden and is correct for the                         *
         *                                 given class.                                     *
         ************************************************************************************/

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

        /// <summary>
        /// Substitutes any filenames stored in this profile (eg quantizer matrices) according to
        /// the substitution table
        /// </summary>
        /// <param name="substitutionTable"></param>
        void FixFileNames(Dictionary<string, string> substitutionTable);

        /// <summary>
        /// Lists all the files that these codec settings depend upon
        /// </summary>
        string[] RequiredFiles { get; }

        /// <summary>
        /// Lists all the profiles that these codec settings depend upon
        /// </summary>
        string[] RequiredProfiles { get; }

    }

    public interface Gettable<TSettings>
    {
        TSettings Settings
        {
            get;
            set;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyEqualityIgnoreAttribute : Attribute
    {
        public PropertyEqualityIgnoreAttribute() {}
    }


    public class PropertyEqualityTester
    {
        /// <summary>
        /// Returns whether all of the properties (excluding those with the PropertyEqualityIgnoreAttribute)
        /// of the two objects are equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool AreEqual(object a, object b)
        {
            if (a.GetType() != b.GetType()) return false;
            Type t = a.GetType();
            foreach (PropertyInfo info in t.GetProperties())
            {
                object[] attributes = info.GetCustomAttributes(true);
                if (info.GetCustomAttributes(typeof(PropertyEqualityIgnoreAttribute), true).Length > 0)
                    continue;
                object aVal = null, bVal = null;
                try { aVal = info.GetValue(a, null); }
                catch { }
                try { bVal = info.GetValue(b, null); }
                catch { }
                if (!ArrayEqual(aVal, bVal)) 
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns whether these two objects are equal. Returns object.Equals except for arrays,
        /// where it recursively does an elementwise comparison
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool ArrayEqual(object a, object b)
        {
            if (a == b) return true;
            if (a == null || b == null) return false;

            if (a.GetType() != b.GetType()) return false;
            if (!a.GetType().IsArray)
                return a.Equals(b);

            object[] arrayA = (object[])a;
            object[] arrayB = (object[])b;

            if (arrayA.Length != arrayB.Length) return false;
            for (int i = 0; i < arrayA.Length; i++)
            {
                if (!ArrayEqual(arrayA[i], arrayB[i])) return false;
            }
            return true;
        }
    }

}
