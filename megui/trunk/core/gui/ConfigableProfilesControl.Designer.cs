namespace MeGUI.core.gui
{
    partial class ConfigableProfilesControl
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
            this.config = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Size = new System.Drawing.Size(194, 21);
            // 
            // config
            // 
            this.config.AutoSize = true;
            this.config.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.config.Dock = System.Windows.Forms.DockStyle.Right;
            this.config.Location = new System.Drawing.Point(194, 0);
            this.config.Name = "config";
            this.config.Size = new System.Drawing.Size(47, 22);
            this.config.TabIndex = 1;
            this.config.Text = "Config";
            this.config.UseVisualStyleBackColor = true;
            this.config.Click += new System.EventHandler(this.config_Click);
            // 
            // ConfigableProfilesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.config);
            this.Name = "ConfigableProfilesControl";
            this.Size = new System.Drawing.Size(241, 22);
            this.Controls.SetChildIndex(this.config, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button config;
    }
}
