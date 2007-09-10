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
            this.audioGroupbox.SuspendLayout();
            this.subtitleGroupbox.SuspendLayout();
            this.chaptersGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.audioDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // openSubtitleButton
            // 
            this.openSubtitleButton.Location = new System.Drawing.Point(382, 18);
            this.openSubtitleButton.TabIndex = 7;
            // 
            // subtitleInput
            // 
            this.subtitleInput.TabIndex = 6;
            // 
            // subtitleInputLabel
            // 
            this.subtitleInputLabel.Location = new System.Drawing.Point(9, 23);
            this.subtitleInputLabel.TabIndex = 5;
            // 
            // audioTrack2
            // 
            this.audioTrack2.TabIndex = 1;
            // 
            // audioTrack1
            // 
            this.audioTrack1.TabIndex = 0;
            // 
            // audioInput
            // 
            this.audioInput.Location = new System.Drawing.Point(120, 19);
            this.audioInput.TabIndex = 3;
            // 
            // audioInputOpenButton
            // 
            this.audioInputOpenButton.Location = new System.Drawing.Point(382, 17);
            this.audioInputOpenButton.TabIndex = 4;
            // 
            // videoInput
            // 
            this.videoInput.TabIndex = 1;
            // 
            // inputOpenButton
            // 
            this.inputOpenButton.Location = new System.Drawing.Point(382, 14);
            this.inputOpenButton.TabIndex = 2;
            // 
            // videoInputLabel
            // 
            this.videoInputLabel.Location = new System.Drawing.Point(9, 19);
            // 
            // chaptersInput
            // 
            this.chaptersInput.Location = new System.Drawing.Point(120, 17);
            this.chaptersInput.TabIndex = 1;
            // 
            // openChaptersButton
            // 
            this.openChaptersButton.Location = new System.Drawing.Point(382, 17);
            this.openChaptersButton.TabIndex = 2;
            // 
            // audioLanguageLabel
            // 
            this.audioLanguageLabel.Location = new System.Drawing.Point(9, 48);
            this.audioLanguageLabel.TabIndex = 5;
            // 
            // audioLanguage
            // 
            this.audioLanguage.Location = new System.Drawing.Point(120, 46);
            this.audioLanguage.TabIndex = 6;
            // 
            // subtitleLanguageLabel
            // 
            this.subtitleLanguageLabel.Location = new System.Drawing.Point(9, 50);
            this.subtitleLanguageLabel.Size = new System.Drawing.Size(100, 16);
            this.subtitleLanguageLabel.TabIndex = 8;
            // 
            // subtitleLanguage
            // 
            this.subtitleLanguage.Location = new System.Drawing.Point(120, 47);
            this.subtitleLanguage.TabIndex = 9;
            // 
            // muxButton
            // 
            this.muxButton.Location = new System.Drawing.Point(304, 448);
            this.muxButton.TabIndex = 6;
            // 
            // outputButton
            // 
            this.outputButton.Location = new System.Drawing.Point(380, 47);
            this.outputButton.TabIndex = 4;
            // 
            // MuxFPSLabel
            // 
            this.MuxFPSLabel.Location = new System.Drawing.Point(9, 46);
            this.MuxFPSLabel.TabIndex = 3;
            // 
            // muxFPS
            // 
            this.muxFPS.Location = new System.Drawing.Point(118, 43);
            this.muxFPS.TabIndex = 4;
            // 
            // muxedOutput
            // 
            this.muxedOutput.Location = new System.Drawing.Point(120, 47);
            this.muxedOutput.TabIndex = 3;
            // 
            // audioInputLabel
            // 
            this.audioInputLabel.Location = new System.Drawing.Point(9, 22);
            this.audioInputLabel.TabIndex = 2;
            // 
            // chaptersInputLabel
            // 
            this.chaptersInputLabel.Location = new System.Drawing.Point(9, 20);
            this.chaptersInputLabel.TabIndex = 0;
            // 
            // muxedOutputLabel
            // 
            this.muxedOutputLabel.Location = new System.Drawing.Point(9, 50);
            this.muxedOutputLabel.TabIndex = 2;
            // 
            // enableSplit
            // 
            this.enableSplit.Location = new System.Drawing.Point(9, 79);
            this.enableSplit.Size = new System.Drawing.Size(100, 16);
            this.enableSplit.TabIndex = 5;
            // 
            // splitSize
            // 
            this.splitSize.Location = new System.Drawing.Point(120, 77);
            this.splitSize.TabIndex = 6;
            // 
            // mbLabel
            // 
            this.mbLabel.Location = new System.Drawing.Point(182, 80);
            this.mbLabel.TabIndex = 7;
            // 
            // removeAudioTrackButton
            // 
            this.removeAudioTrackButton.Location = new System.Drawing.Point(382, 44);
            this.removeAudioTrackButton.TabIndex = 9;
            // 
            // removeSubtitleTrack
            // 
            this.removeSubtitleTrack.Location = new System.Drawing.Point(382, 45);
            this.removeSubtitleTrack.TabIndex = 12;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(366, 448);
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
            this.outputGroupbox.Location = new System.Drawing.Point(8, 331);
            this.outputGroupbox.Size = new System.Drawing.Size(414, 111);
            this.outputGroupbox.TabIndex = 4;
            this.outputGroupbox.Controls.SetChildIndex(this.mbLabel, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.splitSize, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.enableSplit, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.muxedOutputLabel, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.outputButton, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.muxedOutput, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.label1, 0);
            this.outputGroupbox.Controls.SetChildIndex(this.containerFormat, 0);
            // 
            // audioGroupbox
            // 
            this.audioGroupbox.Location = new System.Drawing.Point(8, 83);
            this.audioGroupbox.Size = new System.Drawing.Size(414, 102);
            this.audioGroupbox.TabIndex = 1;
            // 
            // subtitleGroupbox
            // 
            this.subtitleGroupbox.Location = new System.Drawing.Point(8, 191);
            this.subtitleGroupbox.Size = new System.Drawing.Size(414, 80);
            this.subtitleGroupbox.TabIndex = 2;
            // 
            // chaptersGroupbox
            // 
            this.chaptersGroupbox.Location = new System.Drawing.Point(8, 277);
            this.chaptersGroupbox.Size = new System.Drawing.Size(414, 48);
            this.chaptersGroupbox.TabIndex = 3;
            // 
            // audioName
            // 
            this.audioName.Location = new System.Drawing.Point(286, 46);
            this.audioName.TabIndex = 8;
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
            // audioNameLabel
            // 
            this.audioNameLabel.Location = new System.Drawing.Point(245, 49);
            this.audioNameLabel.TabIndex = 7;
            // 
            // audioDelay
            // 
            this.audioDelay.Location = new System.Drawing.Point(120, 73);
            this.audioDelay.TabIndex = 11;
            // 
            // delayLabel
            // 
            this.delayLabel.Location = new System.Drawing.Point(9, 75);
            this.delayLabel.TabIndex = 10;
            // 
            // subName
            // 
            this.subName.Location = new System.Drawing.Point(286, 47);
            this.subName.TabIndex = 11;
            // 
            // SubNamelabel
            // 
            this.SubNamelabel.Location = new System.Drawing.Point(245, 50);
            this.SubNamelabel.TabIndex = 10;
            // 
            // containerFormat
            // 
            this.containerFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.containerFormat.FormattingEnabled = true;
            this.containerFormat.Location = new System.Drawing.Point(120, 18);
            this.containerFormat.Name = "containerFormat";
            this.containerFormat.Size = new System.Drawing.Size(141, 21);
            this.containerFormat.TabIndex = 1;
            this.containerFormat.SelectedIndexChanged += new System.EventHandler(this.containerFormat_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 21);
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
            this.helpButton1.Location = new System.Drawing.Point(8, 448);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 5;
            // 
            // AdaptiveMuxWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 478);
            this.Controls.Add(this.helpButton1);
            this.Name = "AdaptiveMuxWindow";
            this.Text = "Adaptive Mux Window";
            this.Controls.SetChildIndex(this.helpButton1, 0);
            this.Controls.SetChildIndex(this.chaptersGroupbox, 0);
            this.Controls.SetChildIndex(this.videoGroupbox, 0);
            this.Controls.SetChildIndex(this.audioGroupbox, 0);
            this.Controls.SetChildIndex(this.subtitleGroupbox, 0);
            this.Controls.SetChildIndex(this.muxButton, 0);
            this.Controls.SetChildIndex(this.outputGroupbox, 0);
            this.Controls.SetChildIndex(this.cancelButton, 0);
            this.videoGroupbox.ResumeLayout(false);
            this.videoGroupbox.PerformLayout();
            this.outputGroupbox.ResumeLayout(false);
            this.outputGroupbox.PerformLayout();
            this.audioGroupbox.ResumeLayout(false);
            this.audioGroupbox.PerformLayout();
            this.subtitleGroupbox.ResumeLayout(false);
            this.subtitleGroupbox.PerformLayout();
            this.chaptersGroupbox.ResumeLayout(false);
            this.chaptersGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.audioDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox containerFormat;
        private System.Windows.Forms.Label label1;
        private MeGUI.core.gui.HelpButton helpButton1;
    }
}
