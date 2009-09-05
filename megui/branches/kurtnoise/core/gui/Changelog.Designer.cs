namespace MeGUI
{
    partial class Changelog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Changelog));
            this.txtChangelog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtChangelog
            // 
            this.txtChangelog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChangelog.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChangelog.Location = new System.Drawing.Point(0, 0);
            this.txtChangelog.Multiline = true;
            this.txtChangelog.Name = "txtChangelog";
            this.txtChangelog.ReadOnly = true;
            this.txtChangelog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtChangelog.Size = new System.Drawing.Size(572, 492);
            this.txtChangelog.TabIndex = 1;
            this.txtChangelog.WordWrap = false;
            // 
            // Changelog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = global::MeGUI.Properties.Settings.Default.ChangelogSize;
            this.Controls.Add(this.txtChangelog);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::MeGUI.Properties.Settings.Default, "ChangelogLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("ClientSize", global::MeGUI.Properties.Settings.Default, "ChangelogSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("WindowState", global::MeGUI.Properties.Settings.Default, "ChangelogWindowState", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = global::MeGUI.Properties.Settings.Default.ChangelogLocation;
            this.Name = "Changelog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MeGUI - Changelog";
            this.WindowState = global::MeGUI.Properties.Settings.Default.ChangelogWindowState;
            this.Load += new System.EventHandler(this.Changelog_Load);
            this.Resize += new System.EventHandler(this.Changelog_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtChangelog;
    }
}