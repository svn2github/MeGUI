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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MuxWindow));
            this.label1 = new System.Windows.Forms.Label();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.muxedInput = new MeGUI.FileBar();
            this.SuspendLayout();
            // 
            // audio
            // 
            this.audio.Size = new System.Drawing.Size(428, 115);
            // 
            // subtitles
            // 
            this.subtitles.Size = new System.Drawing.Size(428, 98);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Muxed Input";
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Manual mux window";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(12, 479);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 7;
            // 
            // muxedInput
            // 
            this.muxedInput.Filename = "";
            this.muxedInput.Filter = null;
            this.muxedInput.FilterIndex = 0;
            this.muxedInput.FolderMode = false;
            this.muxedInput.Location = new System.Drawing.Point(118, 45);
            this.muxedInput.Name = "muxedInput";
            this.muxedInput.ReadOnly = true;
            this.muxedInput.SaveMode = false;
            this.muxedInput.Size = new System.Drawing.Size(289, 26);
            this.muxedInput.TabIndex = 3;
            this.muxedInput.Title = null;
            this.muxedInput.FileSelected += new MeGUI.FileBarEventHandler(this.muxedInput_FileSelected);
            // 
            // MuxWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(434, 514);
            this.Controls.Add(this.helpButton1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(440, 26);
            this.Name = "MuxWindow";
            this.Text = "MeGUI - Muxer";
            this.Controls.SetChildIndex(this.helpButton1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private MeGUI.core.gui.HelpButton helpButton1;
        private FileBar muxedInput;
    }
}
