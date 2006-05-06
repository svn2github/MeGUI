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
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace MeGUI
{
	/// <summary>
	/// Summary description for neroConfigurationDialog.
	/// </summary>
	public class neroConfigurationDialog : baseAudioConfigurationDialog
	{
		#region variables
		private System.Windows.Forms.CheckBox neroAACQualityCheckbox;
		private System.Windows.Forms.ComboBox neroAACQuality;
		private System.Windows.Forms.ComboBox neroAACProfile;
		private System.Windows.Forms.CheckBox neroAACProfileCheckbox;
		private System.Windows.Forms.ComboBox neroAACVBRMode;
		private System.Windows.Forms.RadioButton neroAACVBRModeRadioButton;
		private System.Windows.Forms.ComboBox neroAACCBRBitrate;
		private System.Windows.Forms.RadioButton neroAACCBRBitrateRadioButton;
        #endregion
	    #region start / stop
        public neroConfigurationDialog(ProfileManager profileManager, string path, MeGUISettings settings, 
			string initialProfile)
            : base(profileManager, path, settings, initialProfile)

		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            performSizeAndLayoutCorrection();
		}


		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.neroAACQualityCheckbox = new System.Windows.Forms.CheckBox();
            this.neroAACQuality = new System.Windows.Forms.ComboBox();
            this.neroAACProfile = new System.Windows.Forms.ComboBox();
            this.neroAACProfileCheckbox = new System.Windows.Forms.CheckBox();
            this.neroAACVBRMode = new System.Windows.Forms.ComboBox();
            this.neroAACVBRModeRadioButton = new System.Windows.Forms.RadioButton();
            this.neroAACCBRBitrate = new System.Windows.Forms.ComboBox();
            this.neroAACCBRBitrateRadioButton = new System.Windows.Forms.RadioButton();
            this.encoderGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Controls.Add(this.neroAACQualityCheckbox);
            this.encoderGroupBox.Controls.Add(this.neroAACQuality);
            this.encoderGroupBox.Controls.Add(this.neroAACProfile);
            this.encoderGroupBox.Controls.Add(this.neroAACProfileCheckbox);
            this.encoderGroupBox.Controls.Add(this.neroAACVBRMode);
            this.encoderGroupBox.Controls.Add(this.neroAACVBRModeRadioButton);
            this.encoderGroupBox.Controls.Add(this.neroAACCBRBitrate);
            this.encoderGroupBox.Controls.Add(this.neroAACCBRBitrateRadioButton);
            this.encoderGroupBox.Size = new System.Drawing.Size(355, 147);
            this.encoderGroupBox.Text = "Nero AAC Options";
            // 
            // neroAACQualityCheckbox
            // 
            this.neroAACQualityCheckbox.Location = new System.Drawing.Point(8, 120);
            this.neroAACQualityCheckbox.Name = "neroAACQualityCheckbox";
            this.neroAACQualityCheckbox.Size = new System.Drawing.Size(104, 24);
            this.neroAACQualityCheckbox.TabIndex = 7;
            this.neroAACQualityCheckbox.Text = "Quality";
            this.neroAACQualityCheckbox.CheckedChanged += new System.EventHandler(this.neroAACQualityCheckbox_CheckedChanged);
            // 
            // neroAACQuality
            // 
            this.neroAACQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.neroAACQuality.Enabled = false;
            this.neroAACQuality.Items.AddRange(new object[] {
            "High",
            "Fast"});
            this.neroAACQuality.Location = new System.Drawing.Point(160, 120);
            this.neroAACQuality.Name = "neroAACQuality";
            this.neroAACQuality.Size = new System.Drawing.Size(121, 21);
            this.neroAACQuality.TabIndex = 6;
            this.neroAACQuality.SelectedIndexChanged += new System.EventHandler(this.neroAACQualityCheckbox_CheckedChanged);
            // 
            // neroAACProfile
            // 
            this.neroAACProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.neroAACProfile.Enabled = false;
            this.neroAACProfile.Items.AddRange(new object[] {
            "HE (High Efficiency) AAC",
            "LC (Low Complexity) AAC"});
            this.neroAACProfile.Location = new System.Drawing.Point(160, 88);
            this.neroAACProfile.Name = "neroAACProfile";
            this.neroAACProfile.Size = new System.Drawing.Size(121, 21);
            this.neroAACProfile.TabIndex = 5;
            this.neroAACProfile.SelectedIndexChanged += new System.EventHandler(this.neroAACProfileCheckbox_CheckedChanged);
            // 
            // neroAACProfileCheckbox
            // 
            this.neroAACProfileCheckbox.Location = new System.Drawing.Point(8, 88);
            this.neroAACProfileCheckbox.Name = "neroAACProfileCheckbox";
            this.neroAACProfileCheckbox.Size = new System.Drawing.Size(104, 24);
            this.neroAACProfileCheckbox.TabIndex = 4;
            this.neroAACProfileCheckbox.Text = "Profile";
            this.neroAACProfileCheckbox.CheckedChanged += new System.EventHandler(this.neroAACProfileCheckbox_CheckedChanged);
            // 
            // neroAACVBRMode
            // 
            this.neroAACVBRMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.neroAACVBRMode.Enabled = false;
            this.neroAACVBRMode.Items.AddRange(new object[] {
            "tape",
            "radio",
            "internet",
            "streaming",
            "normal",
            "extreme",
            "audiophile",
            "transcoding"});
            this.neroAACVBRMode.Location = new System.Drawing.Point(160, 56);
            this.neroAACVBRMode.Name = "neroAACVBRMode";
            this.neroAACVBRMode.Size = new System.Drawing.Size(121, 21);
            this.neroAACVBRMode.TabIndex = 3;
            // 
            // neroAACVBRModeRadioButton
            // 
            this.neroAACVBRModeRadioButton.Location = new System.Drawing.Point(8, 56);
            this.neroAACVBRModeRadioButton.Name = "neroAACVBRModeRadioButton";
            this.neroAACVBRModeRadioButton.Size = new System.Drawing.Size(48, 24);
            this.neroAACVBRModeRadioButton.TabIndex = 2;
            this.neroAACVBRModeRadioButton.Text = "VBR";
            this.neroAACVBRModeRadioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // neroAACCBRBitrate
            // 
            this.neroAACCBRBitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.neroAACCBRBitrate.Items.AddRange(new object[] {
            "16",
            "20",
            "24",
            "28",
            "32",
            "40",
            "48",
            "56",
            "64",
            "80",
            "96",
            "112",
            "128",
            "160",
            "192",
            "224",
            "256",
            "320",
            "388",
            "448"});
            this.neroAACCBRBitrate.Location = new System.Drawing.Point(160, 24);
            this.neroAACCBRBitrate.Name = "neroAACCBRBitrate";
            this.neroAACCBRBitrate.Size = new System.Drawing.Size(121, 21);
            this.neroAACCBRBitrate.TabIndex = 1;
            // 
            // neroAACCBRBitrateRadioButton
            // 
            this.neroAACCBRBitrateRadioButton.Checked = true;
            this.neroAACCBRBitrateRadioButton.Location = new System.Drawing.Point(8, 24);
            this.neroAACCBRBitrateRadioButton.Name = "neroAACCBRBitrateRadioButton";
            this.neroAACCBRBitrateRadioButton.Size = new System.Drawing.Size(56, 24);
            this.neroAACCBRBitrateRadioButton.TabIndex = 0;
            this.neroAACCBRBitrateRadioButton.TabStop = true;
            this.neroAACCBRBitrateRadioButton.Text = "CBR";
            this.neroAACCBRBitrateRadioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // neroConfigurationDialog
            // 
            this.ClientSize = new System.Drawing.Size(376, 502);
            this.Name = "neroConfigurationDialog";
            this.Text = "Nero AAC Encoder Configuration";
            this.encoderGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		#region dropdowns
		private void neroAACCBRBitrate_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (neroAACCBRBitrateRadioButton.Checked)
			{
				neroAACCBRBitrate.Enabled = true;
				neroAACVBRMode.Enabled = false;
			}
			else
			{
				neroAACCBRBitrate.Enabled = false;
				neroAACVBRMode.Enabled = true;
			}
		}
		#endregion
		#region checkboxes

		private void checkbox_CheckedChanged(object sender, System.EventArgs e)
		{
			this.showCommandLine();
		}
		private void neroAACProfileCheckbox_CheckedChanged(object sender, System.EventArgs e)
		{
			if (neroAACProfileCheckbox.Checked)
			{
				neroAACProfile.Enabled = true;
				if (neroAACProfile.SelectedIndex == -1) // make sure one mode is selected
					neroAACProfile.SelectedIndex = 0;
			}
			else
				neroAACProfile.Enabled = false;
			showCommandLine();
		}

		private void neroAACQualityCheckbox_CheckedChanged(object sender, System.EventArgs e)
		{
			if (neroAACQualityCheckbox.Checked)
			{
				neroAACQuality.Enabled = true;
				if (neroAACQuality.SelectedIndex == -1)
					neroAACQuality.SelectedIndex = 0;
			}
			else
				neroAACQuality.Enabled = false;
			showCommandLine();
		}


		private void radioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			if (neroAACCBRBitrateRadioButton.Checked)
			{
				neroAACCBRBitrate.Enabled = true;
				neroAACVBRMode.Enabled = false;
				if (neroAACCBRBitrate.SelectedIndex == -1)
					neroAACCBRBitrate.SelectedIndex = 4;
			}
			else
			{
				neroAACCBRBitrate.Enabled = false;
				neroAACVBRMode.Enabled = true;
				if (neroAACVBRMode.SelectedIndex == -1) // make sure something is selected
					neroAACVBRMode.SelectedIndex = 3;
			}
		}
		#endregion
		#region properties
        protected override Type supportedType
        {
            get { return typeof(NeroAACSettings); }
        }
		/// <summary>
		/// gets / sets the settings that are being shown in this configuration dialog
		/// </summary>
		protected override AudioCodecSettings CodecSettings
		{
			get
			{
				NeroAACSettings nas = new NeroAACSettings();
				if (neroAACCBRBitrateRadioButton.Checked)
				{
					nas.BitrateMode = BitrateManagementMode.CBR;
					try
					{
						nas.Bitrate = Int32.Parse(neroAACCBRBitrate.Text);
					}
					catch (Exception e)
					{
						Console.Write("somebody tried to be smart with the audio bitrate: " + e.Message);
					}
				}
				else // we're in VBR mode
				{
					nas.BitrateMode = BitrateManagementMode.VBR;
					nas.VbrPreset = (VBRPPRESET)neroAACVBRMode.SelectedIndex;
				}
				if (neroAACProfileCheckbox.Checked)
				{
					nas.ProfileEnabled = true;
					nas.Profile = (AACLEVEL)neroAACProfile.SelectedIndex;
				}
				if (neroAACQualityCheckbox.Checked)
				{
					nas.QualityEnabled = true;
					nas.Quality = (QUALITYMODE)neroAACQuality.SelectedIndex;
				}
				return nas;
			}
			set
			{
                NeroAACSettings nas = value as NeroAACSettings;
				if (nas.BitrateMode == BitrateManagementMode.CBR)
				{
					neroAACCBRBitrateRadioButton.Checked = true;
					neroAACCBRBitrate.Text = nas.Bitrate.ToString();
				}
				if (nas.BitrateMode == BitrateManagementMode.VBR)
				{
					neroAACVBRModeRadioButton.Checked = true;
					neroAACVBRMode.SelectedIndex = (int)nas.VbrPreset;
				}
				if (nas.ProfileEnabled)
				{
					neroAACProfileCheckbox.Checked = true;
					neroAACProfile.SelectedIndex = (int)nas.Profile;
				}
				else
					neroAACProfileCheckbox.Checked = false;
				if (nas.QualityEnabled)
				{
					neroAACQualityCheckbox.Checked = true;
					neroAACQuality.SelectedIndex = (int)nas.Quality;
				}
				else
					neroAACQualityCheckbox.Checked = false;
			}
		}
		#endregion
	}
}