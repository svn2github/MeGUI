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

namespace MeGUI
{
	/// <summary>
	/// Summary description for neroConfigurationDialog.
	/// </summary>
	public class baseAudioConfigurationDialog : System.Windows.Forms.Form
	{
		#region variables
        private ProfileManager profileManager;
		private AudioProfile oldAudioProfile = null;
		private string path, initialProfile;
		private string input, output;
		private MeGUISettings settings;

		private System.Windows.Forms.GroupBox besweetOptionsGroupbox;
		private System.Windows.Forms.TextBox besweetDelay;
		private System.Windows.Forms.CheckBox besweetDelayCorrection;
        private System.Windows.Forms.ComboBox besweetDownmixMode;
		private System.Windows.Forms.Label besweetDelayLabel;
		private System.Windows.Forms.Label BesweetChannelsLabel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.GroupBox profileGroupbox;
		private System.Windows.Forms.Button deleteAudioProfileButton;
		private System.Windows.Forms.Button newAudioProfileButton;
        private System.Windows.Forms.ComboBox audioProfile;
        private System.Windows.Forms.CheckBox autoGain;
		private System.Windows.Forms.CheckBox negativeDelay;
        private CheckBox forceDShowDecoding;
        protected GroupBox encoderGroupBox;
        private EnumProxy[] _avisynthChannelSet;
        private CheckBox improvedAccuracy;
        private Button updateButton;
        private Button defaultSettingsButton;
	    
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
        #region polyphormism
	    protected virtual Type supportedType
	    {
	        get
	        {
                return typeof(AudioCodecSettings);
	        }
	    }
        #endregion
        #region start / stop

	    public baseAudioConfigurationDialog()
	    {
            _avisynthChannelSet =
                EnumProxy.CreateArray(
                    this.IsMultichanelSupported
                    ?
                    (
                    this.IsMultichanelRequed
                    ?
                    new object[]{
                                ChannelMode.Upmix,
                                ChannelMode.UpmixUsingSoxEq,
                                ChannelMode.UpmixWithCenterChannelDialog
                    }
                    :
                    new object[]{
                                ChannelMode.KeepOriginal,
                                ChannelMode.StereoDownmix,
                                ChannelMode.DPLDownmix,
                                ChannelMode.DPLIIDownmix,
                                ChannelMode.ConvertToMono,
                                ChannelMode.Upmix,
                                ChannelMode.UpmixUsingSoxEq,
                                ChannelMode.UpmixWithCenterChannelDialog
                    }

                    )
                                :
                    new object[]{
                                ChannelMode.StereoDownmix,
                                ChannelMode.DPLDownmix,
                                ChannelMode.DPLIIDownmix,
                                ChannelMode.ConvertToMono
                    }
                    );

            InitializeComponent();
            this.besweetDownmixMode.DataSource = _avisynthChannelSet;
            this.besweetDownmixMode.BindingContext = new BindingContext();
        }
	    
	    protected void performSizeAndLayoutCorrection()
	    {
            this.SuspendLayout();
            encoderGroupBox.Top = besweetOptionsGroupbox.Bottom + besweetOptionsGroupbox.Top;
            profileGroupbox.Top = encoderGroupBox.Bottom + besweetOptionsGroupbox.Top;
            okButton.Top = cancelButton.Top = profileGroupbox.Bottom + besweetOptionsGroupbox.Top;
            this.ClientSize = new Size(this.ClientSize.Width, okButton.Bottom + besweetOptionsGroupbox.Top);
            this.ResumeLayout(false);
	        
	    }

	    public baseAudioConfigurationDialog(ProfileManager profileManager, string path, MeGUISettings settings, 
			string initialProfile):this()
		{
            InitDialog(profileManager, path, settings, initialProfile);
        }

        public void InitDialog(ProfileManager profileManager, string path, MeGUISettings settings, 
			string initialProfile)
        {
            this.profileManager = profileManager;
            this.path = path;
            this.settings = settings;
            this.initialProfile = initialProfile;
            this.Load += new System.EventHandler(this.OnFormLoad);

        }


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
	    
	    
		/// <summary>
		/// called when the form loads, goes through all the audio profiles
		/// filters out the FAAC ones and adds them to the profile list
		/// if the name of the initial profile is found, that profile is selected
		/// and loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFormLoad(object sender, System.EventArgs e)
		{
            foreach (AudioProfile prof in this.profileManager.AudioProfiles.Values)
            {
                if (prof.Settings.GetType() == this.supportedType) // those are the profiles we're interested in
                {
                    this.audioProfile.Items.Add(prof);
                    if( 0 == string.Compare(prof.Name, this.initialProfile, true))
                        this.audioProfile.SelectedItem = prof;
                }
            }
		}
		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.besweetOptionsGroupbox = new System.Windows.Forms.GroupBox();
            this.improvedAccuracy = new System.Windows.Forms.CheckBox();
            this.forceDShowDecoding = new System.Windows.Forms.CheckBox();
            this.negativeDelay = new System.Windows.Forms.CheckBox();
            this.autoGain = new System.Windows.Forms.CheckBox();
            this.besweetDelay = new System.Windows.Forms.TextBox();
            this.besweetDelayCorrection = new System.Windows.Forms.CheckBox();
            this.besweetDownmixMode = new System.Windows.Forms.ComboBox();
            this.besweetDelayLabel = new System.Windows.Forms.Label();
            this.BesweetChannelsLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.profileGroupbox = new System.Windows.Forms.GroupBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.defaultSettingsButton = new System.Windows.Forms.Button();
            this.deleteAudioProfileButton = new System.Windows.Forms.Button();
            this.newAudioProfileButton = new System.Windows.Forms.Button();
            this.audioProfile = new System.Windows.Forms.ComboBox();
            this.encoderGroupBox = new System.Windows.Forms.GroupBox();
            this.besweetOptionsGroupbox.SuspendLayout();
            this.profileGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // besweetOptionsGroupbox
            // 
            this.besweetOptionsGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.besweetOptionsGroupbox.Controls.Add(this.improvedAccuracy);
            this.besweetOptionsGroupbox.Controls.Add(this.forceDShowDecoding);
            this.besweetOptionsGroupbox.Controls.Add(this.negativeDelay);
            this.besweetOptionsGroupbox.Controls.Add(this.autoGain);
            this.besweetOptionsGroupbox.Controls.Add(this.besweetDelay);
            this.besweetOptionsGroupbox.Controls.Add(this.besweetDelayCorrection);
            this.besweetOptionsGroupbox.Controls.Add(this.besweetDownmixMode);
            this.besweetOptionsGroupbox.Controls.Add(this.besweetDelayLabel);
            this.besweetOptionsGroupbox.Controls.Add(this.BesweetChannelsLabel);
            this.besweetOptionsGroupbox.Location = new System.Drawing.Point(8, 8);
            this.besweetOptionsGroupbox.Name = "besweetOptionsGroupbox";
            this.besweetOptionsGroupbox.Size = new System.Drawing.Size(356, 149);
            this.besweetOptionsGroupbox.TabIndex = 2;
            this.besweetOptionsGroupbox.TabStop = false;
            this.besweetOptionsGroupbox.Text = "Audio Options";
            // 
            // improvedAccuracy
            // 
            this.improvedAccuracy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.improvedAccuracy.AutoSize = true;
            this.improvedAccuracy.Location = new System.Drawing.Point(16, 93);
            this.improvedAccuracy.Name = "improvedAccuracy";
            this.improvedAccuracy.Size = new System.Drawing.Size(272, 17);
            this.improvedAccuracy.TabIndex = 12;
            this.improvedAccuracy.Text = "Improve Accuracy using 32bit && Float computations";
            this.improvedAccuracy.UseVisualStyleBackColor = true;
            // 
            // forceDShowDecoding
            // 
            this.forceDShowDecoding.AutoSize = true;
            this.forceDShowDecoding.Location = new System.Drawing.Point(16, 16);
            this.forceDShowDecoding.Name = "forceDShowDecoding";
            this.forceDShowDecoding.Size = new System.Drawing.Size(174, 17);
            this.forceDShowDecoding.TabIndex = 8;
            this.forceDShowDecoding.Text = "Force Decoding via DirectShow";
            this.forceDShowDecoding.UseVisualStyleBackColor = true;
            // 
            // negativeDelay
            // 
            this.negativeDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.negativeDelay.Enabled = false;
            this.negativeDelay.Location = new System.Drawing.Point(168, 118);
            this.negativeDelay.Name = "negativeDelay";
            this.negativeDelay.Size = new System.Drawing.Size(32, 24);
            this.negativeDelay.TabIndex = 7;
            this.negativeDelay.Text = "-";
            // 
            // autoGain
            // 
            this.autoGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.autoGain.Location = new System.Drawing.Point(16, 63);
            this.autoGain.Name = "autoGain";
            this.autoGain.Size = new System.Drawing.Size(184, 24);
            this.autoGain.TabIndex = 6;
            this.autoGain.Text = "Increase Volume automatically";
            // 
            // besweetDelay
            // 
            this.besweetDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.besweetDelay.Enabled = false;
            this.besweetDelay.Location = new System.Drawing.Point(200, 119);
            this.besweetDelay.Name = "besweetDelay";
            this.besweetDelay.Size = new System.Drawing.Size(48, 21);
            this.besweetDelay.TabIndex = 5;
            this.besweetDelay.Text = "0";
            this.besweetDelay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            this.besweetDelay.TextChanged += new System.EventHandler(this.besweetDelay_TextChanged);
            // 
            // besweetDelayCorrection
            // 
            this.besweetDelayCorrection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.besweetDelayCorrection.Location = new System.Drawing.Point(16, 123);
            this.besweetDelayCorrection.Name = "besweetDelayCorrection";
            this.besweetDelayCorrection.Size = new System.Drawing.Size(112, 16);
            this.besweetDelayCorrection.TabIndex = 4;
            this.besweetDelayCorrection.Text = "Delay Correction";
            this.besweetDelayCorrection.CheckedChanged += new System.EventHandler(this.besweetDelayCorrection_CheckedChanged);
            // 
            // besweetDownmixMode
            // 
            this.besweetDownmixMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.besweetDownmixMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.besweetDownmixMode.Location = new System.Drawing.Point(107, 39);
            this.besweetDownmixMode.Name = "besweetDownmixMode";
            this.besweetDownmixMode.Size = new System.Drawing.Size(240, 21);
            this.besweetDownmixMode.TabIndex = 3;
            // 
            // besweetDelayLabel
            // 
            this.besweetDelayLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.besweetDelayLabel.Location = new System.Drawing.Point(256, 121);
            this.besweetDelayLabel.Name = "besweetDelayLabel";
            this.besweetDelayLabel.Size = new System.Drawing.Size(24, 23);
            this.besweetDelayLabel.TabIndex = 2;
            this.besweetDelayLabel.Text = "ms";
            // 
            // BesweetChannelsLabel
            // 
            this.BesweetChannelsLabel.Location = new System.Drawing.Point(13, 42);
            this.BesweetChannelsLabel.Name = "BesweetChannelsLabel";
            this.BesweetChannelsLabel.Size = new System.Drawing.Size(96, 16);
            this.BesweetChannelsLabel.TabIndex = 2;
            this.BesweetChannelsLabel.Text = "Output Channels";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(315, 365);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(259, 365);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(48, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // profileGroupbox
            // 
            this.profileGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.profileGroupbox.Controls.Add(this.updateButton);
            this.profileGroupbox.Controls.Add(this.defaultSettingsButton);
            this.profileGroupbox.Controls.Add(this.deleteAudioProfileButton);
            this.profileGroupbox.Controls.Add(this.newAudioProfileButton);
            this.profileGroupbox.Controls.Add(this.audioProfile);
            this.profileGroupbox.Location = new System.Drawing.Point(8, 284);
            this.profileGroupbox.Name = "profileGroupbox";
            this.profileGroupbox.Size = new System.Drawing.Size(356, 75);
            this.profileGroupbox.TabIndex = 6;
            this.profileGroupbox.TabStop = false;
            this.profileGroupbox.Text = "Profiles";
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(148, 46);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 22;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // defaultSettingsButton
            // 
            this.defaultSettingsButton.Location = new System.Drawing.Point(251, 46);
            this.defaultSettingsButton.Name = "defaultSettingsButton";
            this.defaultSettingsButton.Size = new System.Drawing.Size(96, 23);
            this.defaultSettingsButton.TabIndex = 21;
            this.defaultSettingsButton.Text = "Load Defaults";
            this.defaultSettingsButton.UseVisualStyleBackColor = true;
            this.defaultSettingsButton.Click += new System.EventHandler(this.defaultSettingsButton_Click);
            // 
            // deleteAudioProfileButton
            // 
            this.deleteAudioProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteAudioProfileButton.Location = new System.Drawing.Point(251, 16);
            this.deleteAudioProfileButton.Name = "deleteAudioProfileButton";
            this.deleteAudioProfileButton.Size = new System.Drawing.Size(48, 23);
            this.deleteAudioProfileButton.TabIndex = 20;
            this.deleteAudioProfileButton.Text = "Delete";
            this.deleteAudioProfileButton.Click += new System.EventHandler(this.deleteAudioProfileButton_Click);
            // 
            // newAudioProfileButton
            // 
            this.newAudioProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newAudioProfileButton.Location = new System.Drawing.Point(307, 16);
            this.newAudioProfileButton.Name = "newAudioProfileButton";
            this.newAudioProfileButton.Size = new System.Drawing.Size(40, 23);
            this.newAudioProfileButton.TabIndex = 19;
            this.newAudioProfileButton.Text = "New";
            this.newAudioProfileButton.Click += new System.EventHandler(this.newAudioProfileButton_Click);
            // 
            // audioProfile
            // 
            this.audioProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audioProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioProfile.Location = new System.Drawing.Point(8, 16);
            this.audioProfile.Name = "audioProfile";
            this.audioProfile.Size = new System.Drawing.Size(237, 21);
            this.audioProfile.Sorted = true;
            this.audioProfile.TabIndex = 18;
            this.audioProfile.SelectedIndexChanged += new System.EventHandler(this.audioProfile_SelectedIndexChanged);
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.encoderGroupBox.Location = new System.Drawing.Point(9, 214);
            this.encoderGroupBox.Name = "encoderGroupBox";
            this.encoderGroupBox.Size = new System.Drawing.Size(355, 64);
            this.encoderGroupBox.TabIndex = 7;
            this.encoderGroupBox.TabStop = false;
            this.encoderGroupBox.Text = "placeholder for encoder options";
            // 
            // baseAudioConfigurationDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(376, 395);
            this.Controls.Add(this.encoderGroupBox);
            this.Controls.Add(this.profileGroupbox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.besweetOptionsGroupbox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "baseAudioConfigurationDialog";
            this.ShowInTaskbar = false;
            this.Text = "Encoder Configuration";
            this.besweetOptionsGroupbox.ResumeLayout(false);
            this.besweetOptionsGroupbox.PerformLayout();
            this.profileGroupbox.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		#region dropdowns
			
		#endregion
		#region checkboxes

		private void besweetDelayCorrection_CheckedChanged(object sender, System.EventArgs e)
		{
			if (besweetDelayCorrection.Checked)
			{
				besweetDelay.Enabled = true;
				this.negativeDelay.Enabled = true;
			}
			else
			{
				besweetDelay.Enabled = false;
				this.negativeDelay.Enabled = false;
			}
			showCommandLine();
		}

	    protected void showCommandLine()
	    {
	        
	    }

		#endregion
		#region properties


	    /// <summary>
	    /// Must collect data from UI / Fill UI from Data
	    /// </summary>
        protected virtual AudioCodecSettings CodecSettings
        {
            get
            {
                return new AudioCodecSettings();
            }
            set
            {
                // Do nothing
            }
        }
	    
	    protected virtual bool IsMultichanelSupported
	    {
	        get
	        {
                return true;
	        }
	    }

        protected virtual bool IsMultichanelRequed
        {
            get
            {
                return false;
            }
        }
	    
		/// <summary>
		/// gets / sets the settings that are being shown in this configuration dialog
		/// </summary>
        [Browsable(false)]
		internal AudioCodecSettings Settings
		{
			get
			{
                AudioCodecSettings fas = CodecSettings;
                fas.ImproveAccuracy = improvedAccuracy.Checked;
                fas.ForceDecodingViaDirectShow = forceDShowDecoding.Checked;
                EnumProxy o = besweetDownmixMode.SelectedItem as EnumProxy;
			    if(o!=null)
				    fas.DownmixMode = (ChannelMode) o.RealValue ;
				if (besweetDelayCorrection.Checked)
				{
					fas.DelayEnabled = true;
					if (!fas.Delay.Equals(""))
					{
						fas.Delay = Int32.Parse(besweetDelay.Text);
						if (negativeDelay.Checked)
							fas.Delay -= 2* fas.Delay;
					}
				}
				fas.AutoGain = autoGain.Checked;
				return fas;
			}
			set
			{
				AudioCodecSettings fas = value;
                besweetDownmixMode.SelectedItem = EnumProxy.Create(fas.DownmixMode);
                improvedAccuracy.Checked = fas.ImproveAccuracy;
                forceDShowDecoding.Checked = fas.ForceDecodingViaDirectShow;
				if (fas.DelayEnabled)
				{
					besweetDelayCorrection.Checked = true;
					if (fas.Delay < 0)
						this.negativeDelay.Checked = true;
					besweetDelay.Text = Math.Abs(fas.Delay).ToString();
				}
				else
				{
					besweetDelayCorrection.Checked = false;
					besweetDelay.Text = "0";
				}
				autoGain.Checked = fas.AutoGain;
                CodecSettings = fas;
			}
		}
		/// <summary>
		/// gets the name of the currently selected profile
		/// </summary>
		public string CurrentProfile
		{
			get
			{
				return audioProfile.Text;
			}
		}
		/// <summary>
		/// sets the audio input (for commandline generation)
		/// </summary>
		public string Input
		{
			set {this.input = value;}
		}
		/// <summary>
		///  sets the audio output (for commandline generation)
		/// </summary>
		public string Output
		{
			set {this.output = value;}
		}
		#endregion
		#region profiles
		/// <summary>
		/// creates a new audio profile from the currently configured settings in the audio tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void newAudioProfileButton_Click(object sender, System.EventArgs e)
		{
            string profileName =  Microsoft.VisualBasic.Interaction.InputBox("Please give the profile a name", "Please give the profile a name", "", -1, -1);
            if (profileName == null)
                return;
            profileName = profileName.Trim();
            if (profileName.Length == 0)
                return;
            AudioProfile prof = new AudioProfile(profileName, this.Settings);
            if (this.profileManager.AddAudioProfile(prof))
            {
                this.audioProfile.Items.Add(prof);
                this.audioProfile.SelectedItem = prof;
                this.oldAudioProfile = prof;
            }
            else
                MessageBox.Show("Sorry, profiles must have unique names", "Duplicate name detected", MessageBoxButtons.OK);
		}

		/// <summary>
		/// deletes the currently selected audio profile and erases the associated xml file from the harddisk if it exists
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void deleteAudioProfileButton_Click(object sender, System.EventArgs e)
		{
            AudioProfile prof = this.audioProfile.SelectedItem as AudioProfile;
            if(prof==null)
                return;
            if (!this.profileManager.DeleteAudioProfile(prof.Name))
                return;
            this.audioProfile.Items.Remove(prof);
            this.oldAudioProfile = null;
		}
		private void audioProfile_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            AudioProfile prof = this.audioProfile.SelectedItem as AudioProfile;
            if (prof == null)
                return;
            this.Settings = prof.Settings;
            /*if (!object.ReferenceEquals(prof, oldAudioProfile))
            {
                if (oldAudioProfile != null)
                    oldAudioProfile.Settings = Settings;
                this.oldAudioProfile = prof;
                this.Settings = prof.Settings;
            }*/
		}
		#endregion
		#region buttons
		private void okButton_Click(object sender, System.EventArgs e)
		{
            /*if (this.oldAudioProfile != null)
			{
                this.oldAudioProfile.Settings = this.Settings;
			}*/
		}
		/// <summary>
		/// handles entires into textfiels, blocks entry of non digit characters
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textField_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (! char.IsDigit(e.KeyChar) && (int)Keys.Back != (int)e.KeyChar)
				e.Handled = true;
		}
        /// <summary>
        /// updates the currently selected profile with the currently active settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateButton_Click(object sender, EventArgs e)
        {
            AudioProfile prof = this.audioProfile.SelectedItem as AudioProfile;
            if (prof == null)
            {
                MessageBox.Show("You must select a profile to update!", "No profile selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            prof.Settings = this.Settings;
        }
        private void defaultSettingsButton_Click(object sender, EventArgs e)
        {
            this.Settings = defaultSettings();
        }
		#endregion
		#region commandline
		private void besweetDelay_TextChanged(object sender, System.EventArgs e)
		{
		}
		#endregion
        /// <summary>
        /// Returns a new instance of the codec settings. This must be specific to the type of the config dialog, so
        /// that it can be set with the Settings.set property.
        /// </summary>
        /// <returns>A new instance of xxxSettings</returns>
        protected virtual AudioCodecSettings defaultSettings()
        {
            throw new Exception("A bug in the program -- ProfilableConfigurationDialog.defaultSettings() is not overridden");
        }
    }
}