namespace MeGUI.packages.audio.waac
{
    partial class WinAmpAACConfigurationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinAmpAACConfigurationPanel));
            this.label3 = new System.Windows.Forms.Label();
            this.vBitrate = new System.Windows.Forms.TrackBar();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.gradientPanel1 = new MeGUI.GradientPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.gbBitrate = new System.Windows.Forms.GroupBox();
            this.tbBitrate = new System.Windows.Forms.TrackBar();
            this.gbChannelMode = new System.Windows.Forms.GroupBox();
            this.cbChannelMode = new System.Windows.Forms.ComboBox();
            this.gbProfile = new System.Windows.Forms.GroupBox();
            this.cbProfile = new System.Windows.Forms.ComboBox();
            this.chmpeg2 = new System.Windows.Forms.CheckBox();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vBitrate)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.gbBitrate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBitrate)).BeginInit();
            this.gbChannelMode.SuspendLayout();
            this.gbProfile.SuspendLayout();
            this.SuspendLayout();
            // 
            // besweetOptionsGroupbox
            // 
            this.besweetOptionsGroupbox.Size = new System.Drawing.Size(387, 212);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.chmpeg2);
            this.tabPage1.Controls.Add(this.gbProfile);
            this.tabPage1.Controls.Add(this.gbChannelMode);
            this.tabPage1.Controls.Add(this.linkLabel1);
            this.tabPage1.Controls.Add(this.gbBitrate);
            this.tabPage1.Size = new System.Drawing.Size(394, 256);
            this.tabPage1.UseVisualStyleBackColor = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(4, 96);
            this.tabControl1.Size = new System.Drawing.Size(402, 282);
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel1.BackgroundImage")));
            this.gradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientPanel1.Controls.Add(this.pictureBox2);
            this.gradientPanel1.Controls.Add(this.label4);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.PageEndColor = System.Drawing.Color.Empty;
            this.gradientPanel1.PageStartColor = System.Drawing.Color.SlateGray;
            this.gradientPanel1.Size = new System.Drawing.Size(409, 90);
            this.gradientPanel1.TabIndex = 45;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(290, 10);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(87, 66);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(23, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(207, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Adjust your Settings here...";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(208, 223);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(153, 13);
            this.linkLabel1.TabIndex = 16;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Official Winamp AAC+ Website";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // gbBitrate
            // 
            this.gbBitrate.Controls.Add(this.tbBitrate);
            this.gbBitrate.Location = new System.Drawing.Point(5, 6);
            this.gbBitrate.Name = "gbBitrate";
            this.gbBitrate.Size = new System.Drawing.Size(385, 71);
            this.gbBitrate.TabIndex = 14;
            this.gbBitrate.TabStop = false;
            this.gbBitrate.Text = "Bitrate";
            // 
            // tbBitrate
            // 
            this.tbBitrate.Location = new System.Drawing.Point(6, 19);
            this.tbBitrate.Maximum = 320;
            this.tbBitrate.Minimum = 16;
            this.tbBitrate.Name = "tbBitrate";
            this.tbBitrate.Size = new System.Drawing.Size(373, 45);
            this.tbBitrate.TabIndex = 0;
            this.tbBitrate.TickFrequency = 8;
            this.tbBitrate.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbBitrate.Value = 16;
            this.tbBitrate.Scroll += new System.EventHandler(this.tbBitrate_Scroll);
            // 
            // gbChannelMode
            // 
            this.gbChannelMode.Controls.Add(this.cbChannelMode);
            this.gbChannelMode.Location = new System.Drawing.Point(5, 83);
            this.gbChannelMode.Name = "gbChannelMode";
            this.gbChannelMode.Size = new System.Drawing.Size(385, 59);
            this.gbChannelMode.TabIndex = 17;
            this.gbChannelMode.TabStop = false;
            this.gbChannelMode.Text = "Channel Mode";
            // 
            // cbChannelMode
            // 
            this.cbChannelMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChannelMode.FormattingEnabled = true;
            this.cbChannelMode.Location = new System.Drawing.Point(14, 23);
            this.cbChannelMode.Name = "cbChannelMode";
            this.cbChannelMode.Size = new System.Drawing.Size(351, 21);
            this.cbChannelMode.TabIndex = 0;
            // 
            // gbProfile
            // 
            this.gbProfile.Controls.Add(this.cbProfile);
            this.gbProfile.Location = new System.Drawing.Point(5, 147);
            this.gbProfile.Name = "gbProfile";
            this.gbProfile.Size = new System.Drawing.Size(385, 57);
            this.gbProfile.TabIndex = 18;
            this.gbProfile.TabStop = false;
            this.gbProfile.Text = "Profile";
            // 
            // cbProfile
            // 
            this.cbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile.FormattingEnabled = true;
            this.cbProfile.Location = new System.Drawing.Point(17, 18);
            this.cbProfile.Name = "cbProfile";
            this.cbProfile.Size = new System.Drawing.Size(351, 21);
            this.cbProfile.TabIndex = 1;
            // 
            // chmpeg2
            // 
            this.chmpeg2.AutoSize = true;
            this.chmpeg2.Location = new System.Drawing.Point(19, 222);
            this.chmpeg2.Name = "chmpeg2";
            this.chmpeg2.Size = new System.Drawing.Size(164, 17);
            this.chmpeg2.TabIndex = 19;
            this.chmpeg2.Text = "Force to use MPEG-2 Stream";
            this.chmpeg2.UseVisualStyleBackColor = true;
            // 
            // WinAmpAACConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.gradientPanel1);
            this.Name = "WinAmpAACConfigurationPanel";
            this.Size = new System.Drawing.Size(409, 387);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.gradientPanel1, 0);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vBitrate)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.gbBitrate.ResumeLayout(false);
            this.gbBitrate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBitrate)).EndInit();
            this.gbChannelMode.ResumeLayout(false);
            this.gbProfile.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TrackBar vBitrate;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private GradientPanel gradientPanel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.GroupBox gbBitrate;
        private System.Windows.Forms.TrackBar tbBitrate;
        private System.Windows.Forms.GroupBox gbChannelMode;
        private System.Windows.Forms.ComboBox cbChannelMode;
        private System.Windows.Forms.GroupBox gbProfile;
        private System.Windows.Forms.ComboBox cbProfile;
        private System.Windows.Forms.CheckBox chmpeg2;

    }
}
