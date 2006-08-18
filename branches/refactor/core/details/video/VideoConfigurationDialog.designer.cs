namespace MeGUI
{
    partial class VideoConfigurationDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.commandline = new System.Windows.Forms.TextBox();
            this.commandlineVisible = new System.Windows.Forms.CheckBox();
            this.profilesGroupbox = new System.Windows.Forms.GroupBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.loadDefaultsButton = new System.Windows.Forms.Button();
            this.videoProfile = new System.Windows.Forms.ComboBox();
            this.newVideoProfileButton = new System.Windows.Forms.Button();
            this.deleteVideoProfileButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.zoneTabPage = new System.Windows.Forms.TabPage();
            this.zonesControl = new MeGUI.ZonesControl();
            this.mainTabPage = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tooltipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.profilesGroupbox.SuspendLayout();
            this.zoneTabPage.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // commandline
            // 
            this.commandline.Location = new System.Drawing.Point(4, 557);
            this.commandline.Multiline = true;
            this.commandline.Name = "commandline";
            this.commandline.ReadOnly = true;
            this.commandline.Size = new System.Drawing.Size(496, 64);
            this.commandline.TabIndex = 40;
            // 
            // commandlineVisible
            // 
            this.commandlineVisible.Location = new System.Drawing.Point(12, 526);
            this.commandlineVisible.Name = "commandlineVisible";
            this.commandlineVisible.Size = new System.Drawing.Size(128, 24);
            this.commandlineVisible.TabIndex = 39;
            this.commandlineVisible.Text = "Show Commandline";
            this.commandlineVisible.CheckedChanged += new System.EventHandler(this.commandlineVisible_CheckedChanged);
            // 
            // profilesGroupbox
            // 
            this.profilesGroupbox.Controls.Add(this.updateButton);
            this.profilesGroupbox.Controls.Add(this.loadDefaultsButton);
            this.profilesGroupbox.Controls.Add(this.videoProfile);
            this.profilesGroupbox.Controls.Add(this.newVideoProfileButton);
            this.profilesGroupbox.Controls.Add(this.deleteVideoProfileButton);
            this.profilesGroupbox.Location = new System.Drawing.Point(12, 472);
            this.profilesGroupbox.Name = "profilesGroupbox";
            this.profilesGroupbox.Size = new System.Drawing.Size(472, 48);
            this.profilesGroupbox.TabIndex = 43;
            this.profilesGroupbox.TabStop = false;
            this.profilesGroupbox.Text = "Profiles";
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(286, 18);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(50, 23);
            this.updateButton.TabIndex = 15;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // loadDefaultsButton
            // 
            this.loadDefaultsButton.Location = new System.Drawing.Point(363, 18);
            this.loadDefaultsButton.Name = "loadDefaultsButton";
            this.loadDefaultsButton.Size = new System.Drawing.Size(103, 23);
            this.loadDefaultsButton.TabIndex = 14;
            this.loadDefaultsButton.Text = "Load Defaults";
            this.loadDefaultsButton.UseVisualStyleBackColor = true;
            this.loadDefaultsButton.Click += new System.EventHandler(this.loadDefaultsButton_Click);
            // 
            // videoProfile
            // 
            this.videoProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoProfile.Location = new System.Drawing.Point(8, 18);
            this.videoProfile.Name = "videoProfile";
            this.videoProfile.Size = new System.Drawing.Size(168, 21);
            this.videoProfile.Sorted = true;
            this.videoProfile.TabIndex = 11;
            this.videoProfile.SelectedIndexChanged += new System.EventHandler(this.profile_SelectedIndexChanged);
            // 
            // newVideoProfileButton
            // 
            this.newVideoProfileButton.Location = new System.Drawing.Point(240, 18);
            this.newVideoProfileButton.Name = "newVideoProfileButton";
            this.newVideoProfileButton.Size = new System.Drawing.Size(40, 23);
            this.newVideoProfileButton.TabIndex = 12;
            this.newVideoProfileButton.Text = "New";
            this.newVideoProfileButton.Click += new System.EventHandler(this.newProfileButton_Click);
            // 
            // deleteVideoProfileButton
            // 
            this.deleteVideoProfileButton.Location = new System.Drawing.Point(182, 18);
            this.deleteVideoProfileButton.Name = "deleteVideoProfileButton";
            this.deleteVideoProfileButton.Size = new System.Drawing.Size(48, 23);
            this.deleteVideoProfileButton.TabIndex = 13;
            this.deleteVideoProfileButton.Text = "Delete";
            this.deleteVideoProfileButton.Click += new System.EventHandler(this.deleteProfileButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(444, 523);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 42;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(385, 523);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(40, 23);
            this.okButton.TabIndex = 41;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // zoneTabPage
            // 
            this.zoneTabPage.Controls.Add(this.zonesControl);
            this.zoneTabPage.Location = new System.Drawing.Point(4, 22);
            this.zoneTabPage.Name = "zoneTabPage";
            this.zoneTabPage.Size = new System.Drawing.Size(488, 426);
            this.zoneTabPage.TabIndex = 2;
            this.zoneTabPage.Text = "Zones";
            this.zoneTabPage.UseVisualStyleBackColor = true;
            // 
            // zonesControl
            // 
            this.zonesControl.CreditsStartFrame = 0;
            this.zonesControl.Input = "";
            this.zonesControl.IntroEndFrame = 0;
            this.zonesControl.Location = new System.Drawing.Point(0, 3);
            this.zonesControl.Name = "zonesControl";
            this.zonesControl.Size = new System.Drawing.Size(310, 305);
            this.zonesControl.TabIndex = 0;
            this.zonesControl.Zones = new MeGUI.Zone[0];
            // 
            // mainTabPage
            // 
            this.mainTabPage.Location = new System.Drawing.Point(4, 22);
            this.mainTabPage.Name = "mainTabPage";
            this.mainTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mainTabPage.Size = new System.Drawing.Size(488, 426);
            this.mainTabPage.TabIndex = 0;
            this.mainTabPage.Text = "Main";
            this.mainTabPage.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.mainTabPage);
            this.tabControl1.Controls.Add(this.zoneTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(496, 452);
            this.tabControl1.TabIndex = 38;
            // 
            // tooltipHelp
            // 
            this.tooltipHelp.AutoPopDelay = 30000;
            this.tooltipHelp.InitialDelay = 500;
            this.tooltipHelp.IsBalloon = true;
            this.tooltipHelp.ReshowDelay = 100;
            this.tooltipHelp.ShowAlways = true;
            // 
            // VideoConfigurationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(496, 550);
            this.Controls.Add(this.profilesGroupbox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.commandline);
            this.Controls.Add(this.commandlineVisible);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VideoConfigurationDialog";
            this.ShowInTaskbar = false;
            this.Text = "newVideoConfigurationDialog";
            this.Load += new System.EventHandler(this.VideoConfigurationDialog_Load);
            this.profilesGroupbox.ResumeLayout(false);
            this.zoneTabPage.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.TextBox commandline;
        protected System.Windows.Forms.CheckBox commandlineVisible;
        protected System.Windows.Forms.GroupBox profilesGroupbox;
        private System.Windows.Forms.Button loadDefaultsButton;
        private System.Windows.Forms.ComboBox videoProfile;
        private System.Windows.Forms.Button newVideoProfileButton;
        private System.Windows.Forms.Button deleteVideoProfileButton;
        protected System.Windows.Forms.Button cancelButton;
        protected System.Windows.Forms.Button okButton;
        protected System.Windows.Forms.TabPage zoneTabPage;
        private ZonesControl zonesControl;
        protected System.Windows.Forms.TabPage mainTabPage;
        protected System.Windows.Forms.TabControl tabControl1;
        protected System.Windows.Forms.ToolTip tooltipHelp;
        private System.Windows.Forms.Button updateButton;
    }
}