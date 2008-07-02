namespace MeGUI.packages.tools.dvddecrypter
{
    partial class DVDDecrypterTool
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox driveGroupBox;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.GroupBox pgcGroupBox;
            System.Windows.Forms.Label label3;
            this.volumeLabel = new System.Windows.Forms.Label();
            this.driveLetter = new System.Windows.Forms.ComboBox();
            this.pgcMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectOnePGCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectMultiplePGCsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pgcs = new System.Windows.Forms.CheckedListBox();
            this.queueButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.outputFolder = new MeGUI.FileBar();
            driveGroupBox = new System.Windows.Forms.GroupBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            pgcGroupBox = new System.Windows.Forms.GroupBox();
            label3 = new System.Windows.Forms.Label();
            driveGroupBox.SuspendLayout();
            pgcGroupBox.SuspendLayout();
            this.pgcMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // driveGroupBox
            // 
            driveGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            driveGroupBox.Controls.Add(this.volumeLabel);
            driveGroupBox.Controls.Add(label2);
            driveGroupBox.Controls.Add(label1);
            driveGroupBox.Controls.Add(this.driveLetter);
            driveGroupBox.Location = new System.Drawing.Point(12, 12);
            driveGroupBox.Name = "driveGroupBox";
            driveGroupBox.Size = new System.Drawing.Size(496, 78);
            driveGroupBox.TabIndex = 2;
            driveGroupBox.TabStop = false;
            driveGroupBox.Text = "Drive";
            // 
            // volumeLabel
            // 
            this.volumeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeLabel.Location = new System.Drawing.Point(61, 51);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(429, 13);
            this.volumeLabel.TabIndex = 4;
            this.volumeLabel.Text = "VOLUME_NAME";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 51);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(36, 13);
            label2.TabIndex = 3;
            label2.Text = "Label:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 22);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(35, 13);
            label1.TabIndex = 2;
            label1.Text = "Drive:";
            // 
            // driveLetter
            // 
            this.driveLetter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.driveLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driveLetter.FormattingEnabled = true;
            this.driveLetter.Location = new System.Drawing.Point(64, 19);
            this.driveLetter.Name = "driveLetter";
            this.driveLetter.Size = new System.Drawing.Size(426, 21);
            this.driveLetter.TabIndex = 1;
            this.driveLetter.SelectedIndexChanged += new System.EventHandler(this.driveLetter_SelectedIndexChanged);
            // 
            // pgcGroupBox
            // 
            pgcGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            pgcGroupBox.ContextMenuStrip = this.pgcMenu;
            pgcGroupBox.Controls.Add(this.pgcs);
            pgcGroupBox.Location = new System.Drawing.Point(12, 96);
            pgcGroupBox.Name = "pgcGroupBox";
            pgcGroupBox.Padding = new System.Windows.Forms.Padding(7);
            pgcGroupBox.Size = new System.Drawing.Size(496, 217);
            pgcGroupBox.TabIndex = 3;
            pgcGroupBox.TabStop = false;
            pgcGroupBox.Text = "PGCs";
            // 
            // pgcMenu
            // 
            this.pgcMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectOnePGCToolStripMenuItem,
            this.selectMultiplePGCsToolStripMenuItem});
            this.pgcMenu.Name = "pgcMenu";
            this.pgcMenu.Size = new System.Drawing.Size(171, 48);
            // 
            // selectOnePGCToolStripMenuItem
            // 
            this.selectOnePGCToolStripMenuItem.Name = "selectOnePGCToolStripMenuItem";
            this.selectOnePGCToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.selectOnePGCToolStripMenuItem.Text = "Select one PGC";
            // 
            // selectMultiplePGCsToolStripMenuItem
            // 
            this.selectMultiplePGCsToolStripMenuItem.Name = "selectMultiplePGCsToolStripMenuItem";
            this.selectMultiplePGCsToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.selectMultiplePGCsToolStripMenuItem.Text = "Select multiple PGCs";
            // 
            // pgcs
            // 
            this.pgcs.CheckOnClick = true;
            this.pgcs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgcs.FormattingEnabled = true;
            this.pgcs.HorizontalScrollbar = true;
            this.pgcs.IntegralHeight = false;
            this.pgcs.Location = new System.Drawing.Point(7, 20);
            this.pgcs.Name = "pgcs";
            this.pgcs.Size = new System.Drawing.Size(482, 190);
            this.pgcs.TabIndex = 0;
            // 
            // label3
            // 
            label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(18, 336);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(92, 13);
            label3.TabIndex = 5;
            label3.Text = "Destination folder:";
            // 
            // queueButton
            // 
            this.queueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.queueButton.Location = new System.Drawing.Point(352, 364);
            this.queueButton.Name = "queueButton";
            this.queueButton.Size = new System.Drawing.Size(75, 23);
            this.queueButton.TabIndex = 6;
            this.queueButton.Text = "Queue";
            this.queueButton.UseVisualStyleBackColor = true;
            this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(433, 364);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // outputFolder
            // 
            this.outputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outputFolder.Filename = "";
            this.outputFolder.Filter = null;
            this.outputFolder.FolderMode = true;
            this.outputFolder.Location = new System.Drawing.Point(113, 329);
            this.outputFolder.Name = "outputFolder";
            this.outputFolder.ReadOnly = true;
            this.outputFolder.SaveMode = false;
            this.outputFolder.Size = new System.Drawing.Size(388, 26);
            this.outputFolder.TabIndex = 4;
            this.outputFolder.Title = null;
            // 
            // DVDDecrypterTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 399);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.queueButton);
            this.Controls.Add(label3);
            this.Controls.Add(this.outputFolder);
            this.Controls.Add(pgcGroupBox);
            this.Controls.Add(driveGroupBox);
            this.Name = "DVDDecrypterTool";
            this.Text = "DVDDecrypterTool";
            driveGroupBox.ResumeLayout(false);
            driveGroupBox.PerformLayout();
            pgcGroupBox.ResumeLayout(false);
            this.pgcMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox pgcs;
        private System.Windows.Forms.ComboBox driveLetter;
        private System.Windows.Forms.Label volumeLabel;
        private FileBar outputFolder;
        private System.Windows.Forms.Button queueButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ContextMenuStrip pgcMenu;
        private System.Windows.Forms.ToolStripMenuItem selectOnePGCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectMultiplePGCsToolStripMenuItem;
    }
}