namespace MeGUI.core.details.mux
{
    partial class MuxStreamControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.subName = new System.Windows.Forms.TextBox();
            this.SubNamelabel = new System.Windows.Forms.Label();
            this.removeSubtitleTrack = new System.Windows.Forms.Button();
            this.subtitleLanguage = new System.Windows.Forms.ComboBox();
            this.subtitleLanguageLabel = new System.Windows.Forms.Label();
            this.subtitleInputLabel = new System.Windows.Forms.Label();
            this.delayLabel = new System.Windows.Forms.Label();
            this.audioDelay = new System.Windows.Forms.NumericUpDown();
            this.input = new MeGUI.FileBar();
            ((System.ComponentModel.ISupportInitialize)(this.audioDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // subName
            // 
            this.subName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.subName.Location = new System.Drawing.Point(278, 34);
            this.subName.MaxLength = 100;
            this.subName.Name = "subName";
            this.subName.Size = new System.Drawing.Size(119, 20);
            this.subName.TabIndex = 40;
            // 
            // SubNamelabel
            // 
            this.SubNamelabel.AutoSize = true;
            this.SubNamelabel.Location = new System.Drawing.Point(237, 38);
            this.SubNamelabel.Name = "SubNamelabel";
            this.SubNamelabel.Size = new System.Drawing.Size(35, 13);
            this.SubNamelabel.TabIndex = 39;
            this.SubNamelabel.Text = "Name";
            // 
            // removeSubtitleTrack
            // 
            this.removeSubtitleTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeSubtitleTrack.Location = new System.Drawing.Point(403, 33);
            this.removeSubtitleTrack.Name = "removeSubtitleTrack";
            this.removeSubtitleTrack.Size = new System.Drawing.Size(24, 23);
            this.removeSubtitleTrack.TabIndex = 38;
            this.removeSubtitleTrack.Text = "X";
            this.removeSubtitleTrack.Click += new System.EventHandler(this.removeSubtitleTrack_Click);
            // 
            // subtitleLanguage
            // 
            this.subtitleLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.subtitleLanguage.Location = new System.Drawing.Point(112, 34);
            this.subtitleLanguage.Name = "subtitleLanguage";
            this.subtitleLanguage.Size = new System.Drawing.Size(121, 21);
            this.subtitleLanguage.Sorted = true;
            this.subtitleLanguage.TabIndex = 37;
            // 
            // subtitleLanguageLabel
            // 
            this.subtitleLanguageLabel.AutoSize = true;
            this.subtitleLanguageLabel.Location = new System.Drawing.Point(8, 38);
            this.subtitleLanguageLabel.Name = "subtitleLanguageLabel";
            this.subtitleLanguageLabel.Size = new System.Drawing.Size(55, 13);
            this.subtitleLanguageLabel.TabIndex = 36;
            this.subtitleLanguageLabel.Text = "Language";
            // 
            // subtitleInputLabel
            // 
            this.subtitleInputLabel.AutoSize = true;
            this.subtitleInputLabel.Location = new System.Drawing.Point(8, 10);
            this.subtitleInputLabel.Name = "subtitleInputLabel";
            this.subtitleInputLabel.Size = new System.Drawing.Size(31, 13);
            this.subtitleInputLabel.TabIndex = 33;
            this.subtitleInputLabel.Text = "Input";
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(8, 65);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(34, 13);
            this.delayLabel.TabIndex = 43;
            this.delayLabel.Text = "Delay";
            // 
            // audioDelay
            // 
            this.audioDelay.Location = new System.Drawing.Point(112, 61);
            this.audioDelay.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.audioDelay.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
            this.audioDelay.Name = "audioDelay";
            this.audioDelay.Size = new System.Drawing.Size(121, 20);
            this.audioDelay.TabIndex = 42;
            // 
            // input
            // 
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.input.Filename = "";
            this.input.Filter = null;
            this.input.FolderMode = false;
            this.input.Location = new System.Drawing.Point(112, 3);
            this.input.Name = "input";
            this.input.ReadOnly = true;
            this.input.SaveMode = false;
            this.input.Size = new System.Drawing.Size(315, 26);
            this.input.TabIndex = 41;
            this.input.Title = null;
            this.input.FileSelected += new MeGUI.FileBarEventHandler(this.input_FileSelected);
            // 
            // MuxStreamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.delayLabel);
            this.Controls.Add(this.audioDelay);
            this.Controls.Add(this.input);
            this.Controls.Add(this.subName);
            this.Controls.Add(this.SubNamelabel);
            this.Controls.Add(this.removeSubtitleTrack);
            this.Controls.Add(this.subtitleLanguage);
            this.Controls.Add(this.subtitleLanguageLabel);
            this.Controls.Add(this.subtitleInputLabel);
            this.Name = "MuxStreamControl";
            this.Size = new System.Drawing.Size(434, 90);
            ((System.ComponentModel.ISupportInitialize)(this.audioDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.TextBox subName;
        protected System.Windows.Forms.Label SubNamelabel;
        protected System.Windows.Forms.Button removeSubtitleTrack;
        protected System.Windows.Forms.ComboBox subtitleLanguage;
        protected System.Windows.Forms.Label subtitleLanguageLabel;
        protected System.Windows.Forms.Label subtitleInputLabel;
        private FileBar input;
        protected System.Windows.Forms.Label delayLabel;
        protected System.Windows.Forms.NumericUpDown audioDelay;
    }
}
