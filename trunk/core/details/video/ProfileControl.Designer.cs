namespace MeGUI.core.details.video
{
    partial class ProfileControl
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
            this.avsConfigButton = new System.Windows.Forms.Button();
            this.avsProfile = new System.Windows.Forms.ComboBox();
            this.avsProfileLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // avsConfigButton
            // 
            this.avsConfigButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.avsConfigButton.Location = new System.Drawing.Point(331, 3);
            this.avsConfigButton.Name = "avsConfigButton";
            this.avsConfigButton.Size = new System.Drawing.Size(48, 23);
            this.avsConfigButton.TabIndex = 21;
            this.avsConfigButton.Text = "Config";
            this.avsConfigButton.Click += new System.EventHandler(this.avsConfigButton_Click);
            // 
            // avsProfile
            // 
            this.avsProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.avsProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.avsProfile.Location = new System.Drawing.Point(71, 3);
            this.avsProfile.Name = "avsProfile";
            this.avsProfile.Size = new System.Drawing.Size(254, 21);
            this.avsProfile.Sorted = true;
            this.avsProfile.TabIndex = 20;
            this.avsProfile.SelectedIndexChanged += new System.EventHandler(this.avsProfile_SelectedIndexChanged);
            // 
            // avsProfileLabel
            // 
            this.avsProfileLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.avsProfileLabel.AutoSize = true;
            this.avsProfileLabel.Location = new System.Drawing.Point(6, 6);
            this.avsProfileLabel.Name = "avsProfileLabel";
            this.avsProfileLabel.Size = new System.Drawing.Size(59, 13);
            this.avsProfileLabel.TabIndex = 19;
            this.avsProfileLabel.Text = "AVS profile";
            // 
            // ProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.avsConfigButton);
            this.Controls.Add(this.avsProfile);
            this.Controls.Add(this.avsProfileLabel);
            this.Name = "ProfileControl";
            this.Size = new System.Drawing.Size(388, 29);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button avsConfigButton;
        internal System.Windows.Forms.ComboBox avsProfile;
        private System.Windows.Forms.Label avsProfileLabel;

    }
}
