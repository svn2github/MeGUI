namespace MeGUI.core.gui
{
    partial class ProfileImporter
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.Text = "Select the profiles to import";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(269, 323);
            this.button2.Size = new System.Drawing.Size(46, 23);
            this.button2.Text = "Import";
            this.button2.Click += new System.EventHandler(this.import_Click);
            // 
            // ProfileImporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(383, 358);
            this.Name = "ProfileImporter";
            this.Text = "Profile Importer";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.button2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
