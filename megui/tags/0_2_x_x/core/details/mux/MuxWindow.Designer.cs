namespace MeGUI
{
    partial class MuxWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.muxedInput = new MeGUI.FileBar();
            this.videoGroupbox.SuspendLayout();
            this.outputGroupbox.SuspendLayout();
            this.chaptersGroupbox.SuspendLayout();
            this.audioPanel.SuspendLayout();
            this.subtitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // videoInputLabel
            // 
            this.videoInputLabel.Location = new System.Drawing.Point(16, 24);
            // 
            // muxButton
            // 
            this.muxButton.Location = new System.Drawing.Point(294, 468);
            // 
            // MuxFPSLabel
            // 
            this.MuxFPSLabel.AutoSize = true;
            this.MuxFPSLabel.Location = new System.Drawing.Point(16, 79);
            this.MuxFPSLabel.Size = new System.Drawing.Size(25, 13);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(376, 468);
            // 
            // videoGroupbox
            // 
            this.videoGroupbox.Controls.Add(this.label1);
            this.videoGroupbox.Controls.Add(this.muxedInput);
            this.videoGroupbox.Location = new System.Drawing.Point(8, 3);
            this.videoGroupbox.Size = new System.Drawing.Size(428, 102);
            this.videoGroupbox.Controls.SetChildIndex(this.fps, 0);
            this.videoGroupbox.Controls.SetChildIndex(this.vInput, 0);
            this.videoGroupbox.Controls.SetChildIndex(this.videoNameLabel, 0);
            this.videoGroupbox.Controls.SetChildIndex(this.MuxFPSLabel, 0);
            this.videoGroupbox.Controls.SetChildIndex(this.videoName, 0);
            this.videoGroupbox.Controls.SetChildIndex(this.muxedInput, 0);
            this.videoGroupbox.Controls.SetChildIndex(this.label1, 0);
            this.videoGroupbox.Controls.SetChildIndex(this.videoInputLabel, 0);
            // 
            // outputGroupbox
            // 
            this.outputGroupbox.Location = new System.Drawing.Point(8, 382);
            this.outputGroupbox.Size = new System.Drawing.Size(428, 80);
            // 
            // chaptersGroupbox
            // 
            this.chaptersGroupbox.Location = new System.Drawing.Point(8, 328);
            this.chaptersGroupbox.Size = new System.Drawing.Size(428, 48);
            // 
            // videoName
            // 
            this.videoName.Location = new System.Drawing.Point(283, 75);
            // 
            // videoNameLabel
            // 
            this.videoNameLabel.Location = new System.Drawing.Point(243, 79);
            // 
            // audioPanel
            // 
            this.audioPanel.Location = new System.Drawing.Point(8, 107);
            // 
            // subtitlePanel
            // 
            this.subtitlePanel.Location = new System.Drawing.Point(8, 224);
            // 
            // splitting
            // 
            this.splitting.CustomSizes = new MeGUI.core.util.FileSize[0];
            // 
            // fps
            // 
            this.fps.Location = new System.Drawing.Point(115, 71);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Muxed Input";
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Manual mux window";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(8, 468);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 30;
            // 
            // muxedInput
            // 
            this.muxedInput.Filename = "";
            this.muxedInput.Filter = null;
            this.muxedInput.FolderMode = false;
            this.muxedInput.Location = new System.Drawing.Point(118, 45);
            this.muxedInput.Name = "muxedInput";
            this.muxedInput.ReadOnly = true;
            this.muxedInput.SaveMode = false;
            this.muxedInput.Size = new System.Drawing.Size(289, 26);
            this.muxedInput.TabIndex = 37;
            this.muxedInput.Title = null;
            this.muxedInput.FileSelected += new MeGUI.FileBarEventHandler(this.muxedInput_FileSelected);
            // 
            // MuxWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 503);
            this.Controls.Add(this.helpButton1);
            this.Name = "MuxWindow";
            this.Text = "MeGUI - Muxer";
            this.Controls.SetChildIndex(this.outputGroupbox, 0);
            this.Controls.SetChildIndex(this.chaptersGroupbox, 0);
            this.Controls.SetChildIndex(this.muxButton, 0);
            this.Controls.SetChildIndex(this.helpButton1, 0);
            this.Controls.SetChildIndex(this.subtitlePanel, 0);
            this.Controls.SetChildIndex(this.cancelButton, 0);
            this.Controls.SetChildIndex(this.audioPanel, 0);
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

        private System.Windows.Forms.Label label1;
        private MeGUI.core.gui.HelpButton helpButton1;
        private FileBar muxedInput;
    }
}
