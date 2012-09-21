namespace MeGUI.packages.audio.flac
{
    partial class FlacConfigurationPanel
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
            this.tbQuality = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.encoderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).BeginInit();
            this.SuspendLayout();
            // 
            // encoderGroupBox
            // 
            this.encoderGroupBox.Controls.Add(this.label5);
            this.encoderGroupBox.Controls.Add(this.label4);
            this.encoderGroupBox.Controls.Add(this.tbQuality);
            this.encoderGroupBox.Location = new System.Drawing.Point(0, 140);
            this.encoderGroupBox.Size = new System.Drawing.Size(393, 78);
            this.encoderGroupBox.Text = " Flac Options ";
            // 
            // tbQuality
            // 
            this.tbQuality.Location = new System.Drawing.Point(6, 18);
            this.tbQuality.Maximum = 8;
            this.tbQuality.Name = "tbQuality";
            this.tbQuality.Size = new System.Drawing.Size(378, 45);
            this.tbQuality.TabIndex = 10;
            this.tbQuality.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbQuality.Value = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "Biggest file, fast encode";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(260, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "Smallest file, slow encode";
            // 
            // FlacConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Name = "FlacConfigurationPanel";
            this.Size = new System.Drawing.Size(394, 240);
            this.encoderGroupBox.ResumeLayout(false);
            this.encoderGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar tbQuality;

    }
}
