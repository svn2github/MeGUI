namespace MeGUI
{
    partial class AdaptiveMuxWindow
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
            this.containerFormat = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.videoGroupbox.SuspendLayout();
            this.outputGroupbox.SuspendLayout();
            this.chaptersGroupbox.SuspendLayout();
            this.audioPanel.SuspendLayout();
            this.subtitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // videoInputLabel
            // 
            this.videoInputLabel.Location = new System.Drawing.Point(9, 19);
            // 
            // muxButton
            // 
            this.muxButton.Location = new System.Drawing.Point(303, 478);
            this.muxButton.TabIndex = 6;
            // 
            // MuxFPSLabel
            // 
            this.MuxFPSLabel.Location = new System.Drawing.Point(9, 46);
            this.MuxFPSLabel.TabIndex = 3;
            // 
            // chaptersInputLabel
            // 
            this.chaptersInputLabel.Location = new System.Drawing.Point(9, 20);
            this.chaptersInputLabel.TabIndex = 0;
            // 
            // muxedOutputLabel
            // 
            this.muxedOutputLabel.Location = new System.Drawing.Point(10, 18);
            this.muxedOutputLabel.TabIndex = 2;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(365, 478);
            this.cancelButton.TabIndex = 7;
            // 
            // videoGroupbox
            // 
            this.videoGroupbox.Size = new System.Drawing.Size(414, 70);
            this.videoGroupbox.TabIndex = 0;
            // 
            // outputGroupbox
            // 
            this.outputGroupbox.Controls.Add(this.containerFormat);
            this.outputGroupbox.Controls.Add(this.label1);
            this.outputGroupbox.Location = new System.Drawing.Point(7, 361);
            this.outputGroupbox.Size = new System.Drawing.Size(414, 111);
            this.outputGroupbox.TabIndex = 4;
            this.outputGroupbox.Controls.SetChildIndex(this.muxedOutputLabel, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.output, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.splitting, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.splittingLabel, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.label1, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.containerFormat, 0);
            // 
            // chaptersGroupbox
            // 
            this.chaptersGroupbox.Location = new System.Drawing.Point(7, 307);
            this.chaptersGroupbox.Size = new System.Drawing.Size(414, 48);
            this.chaptersGroupbox.TabIndex = 3;
            // 
            // videoName
            // 
            this.videoName.Location = new System.Drawing.Point(283, 43);
            this.videoName.TabIndex = 6;
            // 
            // videoNameLabel
            // 
            this.videoNameLabel.Location = new System.Drawing.Point(243, 46);
            this.videoNameLabel.TabIndex = 5;
            // 
            // audioPanel
            // 
            this.audioPanel.Location = new System.Drawing.Point(4, 83);
            // 
            // subtitlePanel
            // 
            this.subtitlePanel.Location = new System.Drawing.Point(4, 204);
            // 
            // splittingLabel
            // 
            this.splittingLabel.Location = new System.Drawing.Point(10, 84);
            // 
            // splitting
            // 
            this.splitting.Location = new System.Drawing.Point(114, 76);
            // 
            // fps
            // 
            this.fps.Location = new System.Drawing.Point(115, 40);
            // 
            // containerFormat
            // 
            this.containerFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.containerFormat.FormattingEnabled = true;
            this.containerFormat.Location = new System.Drawing.Point(118, 45);
            this.containerFormat.Name = "containerFormat";
            this.containerFormat.Size = new System.Drawing.Size(141, 21);
            this.containerFormat.TabIndex = 1;
            this.containerFormat.SelectedIndexChanged += new System.EventHandler(this.containerFormat_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Container Format";
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Adaptive mux window";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(7, 478);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 5;
            // 
            // AdaptiveMuxWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 513);
            this.Controls.Add(this.helpButton1);
            this.Name = "AdaptiveMuxWindow";
            this.Text = "Adaptive Mux Window";
            this.Controls.SetChildIndex(this.muxButton, 0);
            this.Controls.SetChildIndex(this.outputGroupbox, 0);
            this.Controls.SetChildIndex(this.chaptersGroupbox, 0);
            this.Controls.SetChildIndex(this.helpButton1, 0);
            this.Controls.SetChildIndex(this.subtitlePanel, 0);
            this.Controls.SetChildIndex(this.audioPanel, 0);
            this.Controls.SetChildIndex(this.cancelButton, 0);
            this.Controls.SetChildIndex(this.videoGroupbox, 0);
            this.videoGroupbox.ResumeLayout(false);
            this.videoGroupbox.PerformLayout();
            this.outputGroupbox.ResumeLayout(false);
            this.outputGroupbox.PerformLayout();
            this.chaptersGroupbox.ResumeLayout(false);
            this.audioPanel.ResumeLayout(false);
            this.subtitlePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox containerFormat;
        private System.Windows.Forms.Label label1;
        private MeGUI.core.gui.HelpButton helpButton1;
    }
}
