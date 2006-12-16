
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
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace MeGUI
{
	/// <summary>
	/// This class contains the x264 codec configuration GUI
	/// </summary>
	public class avsConfigurationDialog : System.Windows.Forms.Form
	{
		#region variable declaration
        private string initialProfile;
		private int oldAviSynthProfileIndex;
		private string path;
		private string pluginsDirectory;
        private ProfileManager profileManager;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Button insertCrop;
		private System.Windows.Forms.Button openDLLButton;
		private System.Windows.Forms.TextBox dllPath;
		private System.Windows.Forms.TextBox avisynthScript;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox profilesGroupbox;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button insertInput;
		private System.Windows.Forms.Button insertDeinterlace;
		private System.Windows.Forms.Button insertDenoise;
		private System.Windows.Forms.Button insertResize;
		private System.Windows.Forms.TabPage templatePage;
		private System.Windows.Forms.TabPage extraSetupPage;
        private System.Windows.Forms.GroupBox filtersGroupbox;
        private System.Windows.Forms.ComboBox noiseFilterType;
        private System.Windows.Forms.CheckBox noiseFilter;
        private System.Windows.Forms.ComboBox resizeFilterType;
		private System.Windows.Forms.ComboBox avsProfile;
		private System.Windows.Forms.Button newAvsProfileButton;
		private System.Windows.Forms.Button deleteAvsProfileButton;
		private System.Windows.Forms.GroupBox mpegOptGroupBox;
		private System.Windows.Forms.CheckBox colourCorrect;
        private CheckBox resize;
        private ComboBox mod16Box;
        private CheckBox signalAR;
        private Button loadDefaultsButton;
        private Button updateButton;
		private System.Windows.Forms.CheckBox mpeg2Deblocking;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		#endregion
		#region start/stop
        public avsConfigurationDialog(ProfileManager profileManager, string initialProfile, string path, string pluginsDirectory)
		{
			InitializeComponent();
            this.resizeFilterType.DataSource = ScriptServer.ListOfResizeFilterType;
            this.resizeFilterType.BindingContext = new BindingContext();
            this.noiseFilterType.DataSource = ScriptServer.ListOfDenoiseFilterType;
            this.noiseFilterType.BindingContext = new BindingContext();
            this.profileManager = profileManager;
			this.initialProfile = initialProfile;
			this.path = path;
			this.pluginsDirectory = pluginsDirectory;
		}
		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.templatePage = new System.Windows.Forms.TabPage();
            this.insertCrop = new System.Windows.Forms.Button();
            this.openDLLButton = new System.Windows.Forms.Button();
            this.dllPath = new System.Windows.Forms.TextBox();
            this.avisynthScript = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.profilesGroupbox = new System.Windows.Forms.GroupBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.loadDefaultsButton = new System.Windows.Forms.Button();
            this.avsProfile = new System.Windows.Forms.ComboBox();
            this.newAvsProfileButton = new System.Windows.Forms.Button();
            this.deleteAvsProfileButton = new System.Windows.Forms.Button();
            this.insertInput = new System.Windows.Forms.Button();
            this.insertDeinterlace = new System.Windows.Forms.Button();
            this.insertDenoise = new System.Windows.Forms.Button();
            this.insertResize = new System.Windows.Forms.Button();
            this.extraSetupPage = new System.Windows.Forms.TabPage();
            this.mod16Box = new System.Windows.Forms.ComboBox();
            this.signalAR = new System.Windows.Forms.CheckBox();
            this.mpegOptGroupBox = new System.Windows.Forms.GroupBox();
            this.colourCorrect = new System.Windows.Forms.CheckBox();
            this.mpeg2Deblocking = new System.Windows.Forms.CheckBox();
            this.filtersGroupbox = new System.Windows.Forms.GroupBox();
            this.noiseFilterType = new System.Windows.Forms.ComboBox();
            this.resize = new System.Windows.Forms.CheckBox();
            this.noiseFilter = new System.Windows.Forms.CheckBox();
            this.resizeFilterType = new System.Windows.Forms.ComboBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.templatePage.SuspendLayout();
            this.profilesGroupbox.SuspendLayout();
            this.extraSetupPage.SuspendLayout();
            this.mpegOptGroupBox.SuspendLayout();
            this.filtersGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "log";
            this.saveFileDialog.FileName = "2pass.log";
            this.saveFileDialog.Filter = "Logfiles (*.log)|*.log";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.templatePage);
            this.tabControl1.Controls.Add(this.extraSetupPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(424, 501);
            this.tabControl1.TabIndex = 0;
            // 
            // templatePage
            // 
            this.templatePage.Controls.Add(this.insertCrop);
            this.templatePage.Controls.Add(this.openDLLButton);
            this.templatePage.Controls.Add(this.dllPath);
            this.templatePage.Controls.Add(this.avisynthScript);
            this.templatePage.Controls.Add(this.label1);
            this.templatePage.Controls.Add(this.profilesGroupbox);
            this.templatePage.Controls.Add(this.insertInput);
            this.templatePage.Controls.Add(this.insertDeinterlace);
            this.templatePage.Controls.Add(this.insertDenoise);
            this.templatePage.Controls.Add(this.insertResize);
            this.templatePage.Location = new System.Drawing.Point(4, 22);
            this.templatePage.Name = "templatePage";
            this.templatePage.Size = new System.Drawing.Size(416, 475);
            this.templatePage.TabIndex = 0;
            this.templatePage.Text = "Template";
            // 
            // insertCrop
            // 
            this.insertCrop.Location = new System.Drawing.Point(148, 8);
            this.insertCrop.Name = "insertCrop";
            this.insertCrop.Size = new System.Drawing.Size(104, 23);
            this.insertCrop.TabIndex = 48;
            this.insertCrop.Text = "Add crop";
            this.insertCrop.Click += new System.EventHandler(this.insertCrop_Click);
            // 
            // openDLLButton
            // 
            this.openDLLButton.Location = new System.Drawing.Point(380, 368);
            this.openDLLButton.Name = "openDLLButton";
            this.openDLLButton.Size = new System.Drawing.Size(24, 23);
            this.openDLLButton.TabIndex = 44;
            this.openDLLButton.Text = "...";
            this.openDLLButton.Click += new System.EventHandler(this.openDLLButton_Click);
            // 
            // dllPath
            // 
            this.dllPath.Location = new System.Drawing.Point(76, 368);
            this.dllPath.Name = "dllPath";
            this.dllPath.ReadOnly = true;
            this.dllPath.Size = new System.Drawing.Size(288, 21);
            this.dllPath.TabIndex = 43;
            // 
            // avisynthScript
            // 
            this.avisynthScript.Location = new System.Drawing.Point(4, 72);
            this.avisynthScript.Multiline = true;
            this.avisynthScript.Name = "avisynthScript";
            this.avisynthScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.avisynthScript.Size = new System.Drawing.Size(400, 288);
            this.avisynthScript.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 368);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 42;
            this.label1.Text = "Load DLL";
            // 
            // profilesGroupbox
            // 
            this.profilesGroupbox.Controls.Add(this.updateButton);
            this.profilesGroupbox.Controls.Add(this.loadDefaultsButton);
            this.profilesGroupbox.Controls.Add(this.avsProfile);
            this.profilesGroupbox.Controls.Add(this.newAvsProfileButton);
            this.profilesGroupbox.Controls.Add(this.deleteAvsProfileButton);
            this.profilesGroupbox.Location = new System.Drawing.Point(4, 400);
            this.profilesGroupbox.Name = "profilesGroupbox";
            this.profilesGroupbox.Size = new System.Drawing.Size(400, 72);
            this.profilesGroupbox.TabIndex = 40;
            this.profilesGroupbox.TabStop = false;
            this.profilesGroupbox.Text = "Profiles";
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(213, 47);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 15;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // loadDefaultsButton
            // 
            this.loadDefaultsButton.Location = new System.Drawing.Point(306, 47);
            this.loadDefaultsButton.Name = "loadDefaultsButton";
            this.loadDefaultsButton.Size = new System.Drawing.Size(86, 23);
            this.loadDefaultsButton.TabIndex = 14;
            this.loadDefaultsButton.Text = "Load Defaults";
            this.loadDefaultsButton.UseVisualStyleBackColor = true;
            this.loadDefaultsButton.Click += new System.EventHandler(this.loadDefaultsButton_Click);
            // 
            // avsProfile
            // 
            this.avsProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.avsProfile.Location = new System.Drawing.Point(8, 18);
            this.avsProfile.Name = "avsProfile";
            this.avsProfile.Size = new System.Drawing.Size(280, 21);
            this.avsProfile.Sorted = true;
            this.avsProfile.TabIndex = 11;
            this.avsProfile.SelectedIndexChanged += new System.EventHandler(this.avsProfile_SelectedIndexChanged);
            // 
            // newAvsProfileButton
            // 
            this.newAvsProfileButton.Location = new System.Drawing.Point(352, 18);
            this.newAvsProfileButton.Name = "newAvsProfileButton";
            this.newAvsProfileButton.Size = new System.Drawing.Size(40, 23);
            this.newAvsProfileButton.TabIndex = 12;
            this.newAvsProfileButton.Text = "New";
            this.newAvsProfileButton.Click += new System.EventHandler(this.newAviSynthProfileButton_Click);
            // 
            // deleteAvsProfileButton
            // 
            this.deleteAvsProfileButton.Location = new System.Drawing.Point(296, 18);
            this.deleteAvsProfileButton.Name = "deleteAvsProfileButton";
            this.deleteAvsProfileButton.Size = new System.Drawing.Size(48, 23);
            this.deleteAvsProfileButton.TabIndex = 13;
            this.deleteAvsProfileButton.Text = "Delete";
            this.deleteAvsProfileButton.Click += new System.EventHandler(this.deleteAviSynthProfileButton_Click);
            // 
            // insertInput
            // 
            this.insertInput.Location = new System.Drawing.Point(44, 8);
            this.insertInput.Name = "insertInput";
            this.insertInput.Size = new System.Drawing.Size(96, 23);
            this.insertInput.TabIndex = 49;
            this.insertInput.Text = "Add input";
            this.insertInput.Click += new System.EventHandler(this.insertInput_Click);
            // 
            // insertDeinterlace
            // 
            this.insertDeinterlace.Location = new System.Drawing.Point(260, 8);
            this.insertDeinterlace.Name = "insertDeinterlace";
            this.insertDeinterlace.Size = new System.Drawing.Size(96, 23);
            this.insertDeinterlace.TabIndex = 50;
            this.insertDeinterlace.Text = "Add deinterlace";
            this.insertDeinterlace.Click += new System.EventHandler(this.insertDeinterlace_Click);
            // 
            // insertDenoise
            // 
            this.insertDenoise.Location = new System.Drawing.Point(104, 40);
            this.insertDenoise.Name = "insertDenoise";
            this.insertDenoise.Size = new System.Drawing.Size(96, 23);
            this.insertDenoise.TabIndex = 45;
            this.insertDenoise.Text = "Add denoise";
            this.insertDenoise.Click += new System.EventHandler(this.insertDenoise_Click);
            // 
            // insertResize
            // 
            this.insertResize.Location = new System.Drawing.Point(208, 40);
            this.insertResize.Name = "insertResize";
            this.insertResize.Size = new System.Drawing.Size(96, 23);
            this.insertResize.TabIndex = 46;
            this.insertResize.Text = "Add resize";
            this.insertResize.Click += new System.EventHandler(this.insertResize_Click);
            // 
            // extraSetupPage
            // 
            this.extraSetupPage.Controls.Add(this.mod16Box);
            this.extraSetupPage.Controls.Add(this.signalAR);
            this.extraSetupPage.Controls.Add(this.mpegOptGroupBox);
            this.extraSetupPage.Controls.Add(this.filtersGroupbox);
            this.extraSetupPage.Location = new System.Drawing.Point(4, 22);
            this.extraSetupPage.Name = "extraSetupPage";
            this.extraSetupPage.Size = new System.Drawing.Size(416, 475);
            this.extraSetupPage.TabIndex = 1;
            this.extraSetupPage.Text = "Extra Setup";
            // 
            // mod16Box
            // 
            this.mod16Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mod16Box.Enabled = false;
            this.mod16Box.FormattingEnabled = true;
            this.mod16Box.Items.AddRange(new object[] {
            "Resize to mod16",
            "Overcrop to achieve mod16",
            "Encode non-mod16",
            "Crop mod4 horizontally"});
            this.mod16Box.Location = new System.Drawing.Point(210, 181);
            this.mod16Box.Name = "mod16Box";
            this.mod16Box.Size = new System.Drawing.Size(157, 21);
            this.mod16Box.TabIndex = 21;
            // 
            // signalAR
            // 
            this.signalAR.AutoSize = true;
            this.signalAR.Location = new System.Drawing.Point(13, 183);
            this.signalAR.Name = "signalAR";
            this.signalAR.Size = new System.Drawing.Size(190, 17);
            this.signalAR.TabIndex = 20;
            this.signalAR.Text = "Clever (TM) anamorphic encoding:";
            this.signalAR.CheckedChanged += new System.EventHandler(this.signalAR_CheckedChanged);
            // 
            // mpegOptGroupBox
            // 
            this.mpegOptGroupBox.Controls.Add(this.colourCorrect);
            this.mpegOptGroupBox.Controls.Add(this.mpeg2Deblocking);
            this.mpegOptGroupBox.Location = new System.Drawing.Point(8, 98);
            this.mpegOptGroupBox.Name = "mpegOptGroupBox";
            this.mpegOptGroupBox.Size = new System.Drawing.Size(197, 79);
            this.mpegOptGroupBox.TabIndex = 12;
            this.mpegOptGroupBox.TabStop = false;
            this.mpegOptGroupBox.Text = "Mpeg Options";
            // 
            // colourCorrect
            // 
            this.colourCorrect.Location = new System.Drawing.Point(5, 45);
            this.colourCorrect.Name = "colourCorrect";
            this.colourCorrect.Size = new System.Drawing.Size(111, 18);
            this.colourCorrect.TabIndex = 9;
            this.colourCorrect.Text = "Colour Correction";
            // 
            // mpeg2Deblocking
            // 
            this.mpeg2Deblocking.Location = new System.Drawing.Point(5, 20);
            this.mpeg2Deblocking.Name = "mpeg2Deblocking";
            this.mpeg2Deblocking.Size = new System.Drawing.Size(124, 18);
            this.mpeg2Deblocking.TabIndex = 8;
            this.mpeg2Deblocking.Text = "Mpeg2 Deblocking";
            // 
            // filtersGroupbox
            // 
            this.filtersGroupbox.Controls.Add(this.noiseFilterType);
            this.filtersGroupbox.Controls.Add(this.resize);
            this.filtersGroupbox.Controls.Add(this.noiseFilter);
            this.filtersGroupbox.Controls.Add(this.resizeFilterType);
            this.filtersGroupbox.Location = new System.Drawing.Point(8, 8);
            this.filtersGroupbox.Name = "filtersGroupbox";
            this.filtersGroupbox.Size = new System.Drawing.Size(400, 84);
            this.filtersGroupbox.TabIndex = 1;
            this.filtersGroupbox.TabStop = false;
            this.filtersGroupbox.Text = "Filters";
            // 
            // noiseFilterType
            // 
            this.noiseFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.noiseFilterType.Enabled = false;
            this.noiseFilterType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noiseFilterType.ItemHeight = 13;
            this.noiseFilterType.Location = new System.Drawing.Point(152, 51);
            this.noiseFilterType.Name = "noiseFilterType";
            this.noiseFilterType.Size = new System.Drawing.Size(121, 21);
            this.noiseFilterType.TabIndex = 5;
            // 
            // resize
            // 
            this.resize.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resize.Location = new System.Drawing.Point(6, 24);
            this.resize.Name = "resize";
            this.resize.Size = new System.Drawing.Size(104, 24);
            this.resize.TabIndex = 3;
            this.resize.Text = "Resize Filter";
            // 
            // noiseFilter
            // 
            this.noiseFilter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noiseFilter.Location = new System.Drawing.Point(6, 49);
            this.noiseFilter.Name = "noiseFilter";
            this.noiseFilter.Size = new System.Drawing.Size(104, 24);
            this.noiseFilter.TabIndex = 3;
            this.noiseFilter.Text = "Noise Filter";
            // 
            // resizeFilterType
            // 
            this.resizeFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resizeFilterType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resizeFilterType.ItemHeight = 13;
            this.resizeFilterType.Location = new System.Drawing.Point(152, 24);
            this.resizeFilterType.Name = "resizeFilterType";
            this.resizeFilterType.Size = new System.Drawing.Size(121, 21);
            this.resizeFilterType.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(372, 507);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 39;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(304, 507);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(48, 23);
            this.okButton.TabIndex = 38;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // avsConfigurationDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(426, 542);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "avsConfigurationDialog";
            this.ShowInTaskbar = false;
            this.Text = "AviSynth Profile Configuration";
            this.Load += new System.EventHandler(this.avsConfigurationDialog_Load);
            this.tabControl1.ResumeLayout(false);
            this.templatePage.ResumeLayout(false);
            this.templatePage.PerformLayout();
            this.profilesGroupbox.ResumeLayout(false);
            this.extraSetupPage.ResumeLayout(false);
            this.extraSetupPage.PerformLayout();
            this.mpegOptGroupBox.ResumeLayout(false);
            this.filtersGroupbox.ResumeLayout(false);
            this.ResumeLayout(false);

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
		private void avsProfile_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.avsProfile.SelectedIndex != -1) // if it's -1 it's bogus
			{
                AviSynthProfile prof = this.profileManager.AvsProfiles[this.avsProfile.SelectedItem.ToString()];
                this.Settings = prof.Settings;
                /*if (this.oldAviSynthProfileIndex != -1 && !this.newProfile) // -1 means it's never been touched
                    this.profileManager.AvsProfiles[this.avsProfile.Items[this.oldAviSynthProfileIndex].ToString()].Settings = this.Settings;
                newProfile = false;
                this.Settings = prof.Settings;
                this.oldAviSynthProfileIndex = this.avsProfile.SelectedIndex;
                this.avsProfile.SelectAll();*/
			}
		}
		/// <summary>
		/// creates a new profile if the entered name does not match an already existing profile
		/// profiles are attached at the bottom of the profile dropdown list and also stored
		/// in the profiles hashtable
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void newAviSynthProfileButton_Click(object sender, System.EventArgs e)
		{
            string profileName = Microsoft.VisualBasic.Interaction.InputBox("Please give the profile a name", "Please give the profile a name", "", -1, -1);
            if (profileName == null)
                return;
            profileName = profileName.Trim();
            if (profileName.Length == 0)
                return;
            AviSynthProfile prof = new AviSynthProfile(profileName, this.Settings);
            if (this.profileManager.AddAviSynthProfile(prof))
            {
                this.avsProfile.Items.Add(prof.Name);
				this.avsProfile.SelectedIndex = this.avsProfile.Items.IndexOf(prof.Name);
                this.oldAviSynthProfileIndex = this.avsProfile.SelectedIndex;
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
		private void deleteAviSynthProfileButton_Click(object sender, System.EventArgs e)
		{
			if (this.avsProfile.SelectedIndex != -1) // if it's -1 it's bogus
			{
                string name = this.avsProfile.SelectedItem.ToString();
                if (profileManager.DeleteAviSynthProfile(name))
                {
                    this.avsProfile.BeginUpdate(); // now make GUI changes
                    this.avsProfile.Items.Remove(name);
                    this.avsProfile.EndUpdate();
                    this.oldAviSynthProfileIndex = -1;
                }
			}
		}
        /// <summary>
        /// loads the default settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadDefaultsButton_Click(object sender, EventArgs e)
        {
            this.Settings = new AviSynthSettings();
        }
        /// <summary>
        /// updates the currently selected settings with what's currently being shown in the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateButton_Click(object sender, EventArgs e)
        {
            if (this.avsProfile.SelectedIndex != -1) // if it's -1 it's bogus
            {
                AviSynthProfile prof = this.profileManager.AvsProfiles[this.avsProfile.SelectedItem.ToString()];
                prof.Settings = this.Settings;
            }
            else
            {
                MessageBox.Show("You must select a profile to update!", "No profile selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
		#endregion
		#region start/stop events
		/// <summary>
		/// executes when the form is being loaded
		/// initializes the profiles and dropdowns for any non initialized dropdown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void avsConfigurationDialog_Load(object sender, System.EventArgs e)
		{
            foreach (string name in profileManager.AvsProfiles.Keys)
                avsProfile.Items.Add(name);
			int index = this.avsProfile.Items.IndexOf(this.initialProfile);
			if (index != -1)
				this.avsProfile.SelectedIndex = index;
		}

        private void okButton_Click(object sender, System.EventArgs e)
        {
            /*if (this.avsProfile.SelectedIndex != -1)
            {
                this.profileManager.AvsProfiles[this.avsProfile.SelectedItem.ToString()].Settings = this.Settings;
            }*/
        }
		#endregion
		#region helper methods
		private void insert(string text)
		{
			string avsScript = avisynthScript.Text;
			string avsScriptA = avsScript.Substring(0, avisynthScript.SelectionStart);
			string avsScriptB = avsScript.Substring(avisynthScript.SelectionStart + avisynthScript.SelectionLength);
			avisynthScript.Text = avsScriptA + text + avsScriptB;
		}
		#endregion
		#region insert buttons
		private void insertInput_Click(object sender, System.EventArgs e)
		{
			insert("<input>");
		}
		private void insertCrop_Click(object sender, System.EventArgs e)
		{
			insert("<crop>");
		}

		private void insertDeinterlace_Click(object sender, System.EventArgs e)
		{
			insert("<deinterlace>");
		}
		private void insertDenoise_Click(object sender, System.EventArgs e)
		{
			insert("<denoise>");
		}
		private void insertResize_Click(object sender, System.EventArgs e)
		{
			insert("<resize>");
		}
        private void openDLLButton_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog openFilterDialog = new OpenFileDialog();
			openFilterDialog.InitialDirectory = pluginsDirectory;
			if (openFilterDialog.ShowDialog() == DialogResult.OK)
			{
				dllPath.Text = openFilterDialog.FileName;
				avisynthScript.Text = "LoadPlugin(\"" + openFilterDialog.FileName + "\")\r\n" + avisynthScript.Text;
			}
		}
		#endregion
		#region properties

		public AviSynthSettings Settings
		{
			get
			{
                mod16Method method = (mod16Method)mod16Box.SelectedIndex;
                if (!signalAR.Checked)
                    method = mod16Method.none;
                return new AviSynthSettings(avisynthScript.Text,
					(ResizeFilterType)(resizeFilterType.SelectedItem as EnumProxy).RealValue,
                    resize.Checked,
					(DenoiseFilterType)(noiseFilterType.SelectedItem as EnumProxy).RealValue,
					noiseFilter.Checked,
					mpeg2Deblocking.Checked,
					colourCorrect.Checked,
                    method);
			}
			set
			{
				avisynthScript.Text = value.Template;
                resize.Checked = value.Resize;
				resizeFilterType.SelectedItem = EnumProxy.Create(value.ResizeMethod);
                noiseFilterType.SelectedItem = EnumProxy.Create(value.DenoiseMethod);
				noiseFilter.Checked = value.Denoise;
				mpeg2Deblocking.Checked = value.MPEG2Deblock;
				colourCorrect.Checked = value.ColourCorrect;
                signalAR.Checked = (value.Mod16Method != mod16Method.none);
                mod16Box.SelectedIndex = (int)value.Mod16Method;
			}
		}
		/// <summary>
		/// gets the name of the currently selected profile
		/// </summary>
		public string CurrentProfile
		{
			get
			{
				return avsProfile.Text;
			}
		}
		#endregion

        private void signalAR_CheckedChanged(object sender, EventArgs e)
        {
            mod16Box.Enabled = signalAR.Checked;
        }
	}
}