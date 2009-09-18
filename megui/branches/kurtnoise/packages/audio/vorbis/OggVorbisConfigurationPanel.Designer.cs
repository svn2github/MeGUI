namespace MeGUI.packages.audio.vorbis
{
    partial class OggVorbisConfigurationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OggVorbisConfigurationPanel));
            this.vQuality = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.gradientPanel1 = new MeGUI.GradientPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gbQuality = new System.Windows.Forms.GroupBox();
            this.tbQuality = new System.Windows.Forms.TrackBar();
            this.gbBitrate = new System.Windows.Forms.GroupBox();
            this.tbBitrate = new System.Windows.Forms.TrackBar();
            this.gbTarget = new System.Windows.Forms.GroupBox();
            this.rbQuality = new System.Windows.Forms.RadioButton();
            this.rbBitrate = new System.Windows.Forms.RadioButton();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vQuality)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.gbQuality.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).BeginInit();
            this.gbBitrate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBitrate)).BeginInit();
            this.gbTarget.SuspendLayout();
            this.SuspendLayout();
            // 
            // besweetOptionsGroupbox
            // 
            this.besweetOptionsGroupbox.Size = new System.Drawing.Size(387, 212);
            this.besweetOptionsGroupbox.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.linkLabel1);
            this.tabPage1.Controls.Add(this.gbQuality);
            this.tabPage1.Controls.Add(this.gbBitrate);
            this.tabPage1.Controls.Add(this.gbTarget);
            this.tabPage1.Size = new System.Drawing.Size(394, 256);
            this.tabPage1.UseVisualStyleBackColor = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(4, 96);
            this.tabControl1.Size = new System.Drawing.Size(402, 282);
            // 
            // vQuality
            // 
            this.vQuality.Location = new System.Drawing.Point(0, 0);
            this.vQuality.Name = "vQuality";
            this.vQuality.Size = new System.Drawing.Size(104, 45);
            this.vQuality.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gradientPanel1.BackgroundImage")));
            this.gradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gradientPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientPanel1.Controls.Add(this.pictureBox2);
            this.gradientPanel1.Controls.Add(this.label3);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.PageEndColor = System.Drawing.Color.Empty;
            this.gradientPanel1.PageStartColor = System.Drawing.Color.SlateGray;
            this.gradientPanel1.Size = new System.Drawing.Size(408, 90);
            this.gradientPanel1.TabIndex = 45;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(310, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(76, 61);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(23, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(207, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Adjust your Settings here...";
            // 
            // gbQuality
            // 
            this.gbQuality.Controls.Add(this.tbQuality);
            this.gbQuality.Location = new System.Drawing.Point(6, 135);
            this.gbQuality.Name = "gbQuality";
            this.gbQuality.Size = new System.Drawing.Size(385, 75);
            this.gbQuality.TabIndex = 11;
            this.gbQuality.TabStop = false;
            this.gbQuality.Text = "Quality";
            // 
            // tbQuality
            // 
            this.tbQuality.Location = new System.Drawing.Point(6, 24);
            this.tbQuality.Maximum = 1000;
            this.tbQuality.Minimum = -200;
            this.tbQuality.Name = "tbQuality";
            this.tbQuality.Size = new System.Drawing.Size(373, 45);
            this.tbQuality.TabIndex = 0;
            this.tbQuality.TickFrequency = 25;
            this.tbQuality.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbQuality.Scroll += new System.EventHandler(this.tbQuality_Scroll);
            // 
            // gbBitrate
            // 
            this.gbBitrate.Controls.Add(this.tbBitrate);
            this.gbBitrate.Location = new System.Drawing.Point(6, 54);
            this.gbBitrate.Name = "gbBitrate";
            this.gbBitrate.Size = new System.Drawing.Size(385, 75);
            this.gbBitrate.TabIndex = 10;
            this.gbBitrate.TabStop = false;
            this.gbBitrate.Text = "Bitrate";
            // 
            // tbBitrate
            // 
            this.tbBitrate.Location = new System.Drawing.Point(6, 19);
            this.tbBitrate.Maximum = 640;
            this.tbBitrate.Minimum = 64;
            this.tbBitrate.Name = "tbBitrate";
            this.tbBitrate.Size = new System.Drawing.Size(373, 45);
            this.tbBitrate.TabIndex = 0;
            this.tbBitrate.TickFrequency = 16;
            this.tbBitrate.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbBitrate.Value = 64;
            this.tbBitrate.Scroll += new System.EventHandler(this.tbBitrate_Scroll);
            // 
            // gbTarget
            // 
            this.gbTarget.Controls.Add(this.rbQuality);
            this.gbTarget.Controls.Add(this.rbBitrate);
            this.gbTarget.Location = new System.Drawing.Point(6, 2);
            this.gbTarget.Name = "gbTarget";
            this.gbTarget.Size = new System.Drawing.Size(385, 46);
            this.gbTarget.TabIndex = 9;
            this.gbTarget.TabStop = false;
            this.gbTarget.Text = "Target";
            // 
            // rbQuality
            // 
            this.rbQuality.AutoSize = true;
            this.rbQuality.Checked = true;
            this.rbQuality.Location = new System.Drawing.Point(240, 19);
            this.rbQuality.Name = "rbQuality";
            this.rbQuality.Size = new System.Drawing.Size(57, 17);
            this.rbQuality.TabIndex = 21;
            this.rbQuality.TabStop = true;
            this.rbQuality.Text = "Quality";
            this.rbQuality.UseVisualStyleBackColor = true;
            this.rbQuality.CheckedChanged += new System.EventHandler(this.target_CheckedChanged);
            // 
            // rbBitrate
            // 
            this.rbBitrate.AutoSize = true;
            this.rbBitrate.Location = new System.Drawing.Point(117, 19);
            this.rbBitrate.Name = "rbBitrate";
            this.rbBitrate.Size = new System.Drawing.Size(55, 17);
            this.rbBitrate.TabIndex = 20;
            this.rbBitrate.Text = "Bitrate";
            this.rbBitrate.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(16, 227);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(156, 13);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Official Vorbis Encoder Website";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // OggVorbisConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.gradientPanel1);
            this.Name = "OggVorbisConfigurationPanel";
            this.Size = new System.Drawing.Size(408, 383);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.gradientPanel1, 0);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vQuality)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.gbQuality.ResumeLayout(false);
            this.gbQuality.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).EndInit();
            this.gbBitrate.ResumeLayout(false);
            this.gbBitrate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbBitrate)).EndInit();
            this.gbTarget.ResumeLayout(false);
            this.gbTarget.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TrackBar vQuality;
        private System.Windows.Forms.Label label1;
        private GradientPanel gradientPanel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbQuality;
        private System.Windows.Forms.TrackBar tbQuality;
        private System.Windows.Forms.GroupBox gbBitrate;
        private System.Windows.Forms.TrackBar tbBitrate;
        private System.Windows.Forms.GroupBox gbTarget;
        private System.Windows.Forms.RadioButton rbQuality;
        private System.Windows.Forms.RadioButton rbBitrate;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}
