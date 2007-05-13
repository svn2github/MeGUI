namespace MeGUI
{
    partial class VobSubIndexWindow
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
            this.inputGroupbox = new System.Windows.Forms.GroupBox();
            this.pgc = new System.Windows.Forms.NumericUpDown();
            this.pgcLabel = new System.Windows.Forms.Label();
            this.openButton = new System.Windows.Forms.Button();
            this.input = new System.Windows.Forms.TextBox();
            this.inputLabel = new System.Windows.Forms.Label();
            this.outputGroupbox = new System.Windows.Forms.GroupBox();
            this.pickOutputButton = new System.Windows.Forms.Button();
            this.projectName = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.subtitleGroupbox = new System.Windows.Forms.GroupBox();
            this.subtitleTracks = new System.Windows.Forms.CheckedListBox();
            this.demuxSelectedTracks = new System.Windows.Forms.RadioButton();
            this.keepAllTracks = new System.Windows.Forms.RadioButton();
            this.closeOnQueue = new System.Windows.Forms.CheckBox();
            this.queueButton = new System.Windows.Forms.Button();
            this.openIFODialog = new System.Windows.Forms.OpenFileDialog();
            this.openOutputDialog = new System.Windows.Forms.SaveFileDialog();
            this.inputGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pgc)).BeginInit();
            this.outputGroupbox.SuspendLayout();
            this.subtitleGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputGroupbox
            // 
            this.inputGroupbox.Controls.Add(this.pgc);
            this.inputGroupbox.Controls.Add(this.pgcLabel);
            this.inputGroupbox.Controls.Add(this.openButton);
            this.inputGroupbox.Controls.Add(this.input);
            this.inputGroupbox.Controls.Add(this.inputLabel);
            this.inputGroupbox.Location = new System.Drawing.Point(2, 2);
            this.inputGroupbox.Name = "inputGroupbox";
            this.inputGroupbox.Size = new System.Drawing.Size(424, 70);
            this.inputGroupbox.TabIndex = 1;
            this.inputGroupbox.TabStop = false;
            this.inputGroupbox.Text = "Input";
            // 
            // pgc
            // 
            this.pgc.Location = new System.Drawing.Point(120, 45);
            this.pgc.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.pgc.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pgc.Name = "pgc";
            this.pgc.Size = new System.Drawing.Size(50, 21);
            this.pgc.TabIndex = 4;
            this.pgc.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // pgcLabel
            // 
            this.pgcLabel.AutoSize = true;
            this.pgcLabel.Location = new System.Drawing.Point(16, 47);
            this.pgcLabel.Name = "pgcLabel";
            this.pgcLabel.Size = new System.Drawing.Size(27, 13);
            this.pgcLabel.TabIndex = 3;
            this.pgcLabel.Text = "PGC";
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(382, 16);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(24, 23);
            this.openButton.TabIndex = 2;
            this.openButton.Text = "...";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // input
            // 
            this.input.Location = new System.Drawing.Point(118, 17);
            this.input.Name = "input";
            this.input.ReadOnly = true;
            this.input.Size = new System.Drawing.Size(256, 21);
            this.input.TabIndex = 1;
            // 
            // inputLabel
            // 
            this.inputLabel.Location = new System.Drawing.Point(16, 20);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(100, 13);
            this.inputLabel.TabIndex = 0;
            this.inputLabel.Text = "Input";
            // 
            // outputGroupbox
            // 
            this.outputGroupbox.Controls.Add(this.pickOutputButton);
            this.outputGroupbox.Controls.Add(this.projectName);
            this.outputGroupbox.Controls.Add(this.nameLabel);
            this.outputGroupbox.Location = new System.Drawing.Point(2, 309);
            this.outputGroupbox.Name = "outputGroupbox";
            this.outputGroupbox.Size = new System.Drawing.Size(424, 49);
            this.outputGroupbox.TabIndex = 13;
            this.outputGroupbox.TabStop = false;
            this.outputGroupbox.Text = "Output";
            // 
            // pickOutputButton
            // 
            this.pickOutputButton.Location = new System.Drawing.Point(384, 16);
            this.pickOutputButton.Name = "pickOutputButton";
            this.pickOutputButton.Size = new System.Drawing.Size(24, 23);
            this.pickOutputButton.TabIndex = 5;
            this.pickOutputButton.Text = "...";
            this.pickOutputButton.Click += new System.EventHandler(this.pickOutputButton_Click);
            // 
            // projectName
            // 
            this.projectName.Location = new System.Drawing.Point(120, 17);
            this.projectName.Name = "projectName";
            this.projectName.ReadOnly = true;
            this.projectName.Size = new System.Drawing.Size(256, 21);
            this.projectName.TabIndex = 4;
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(16, 20);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(100, 13);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Ouput";
            // 
            // subtitleGroupbox
            // 
            this.subtitleGroupbox.Controls.Add(this.subtitleTracks);
            this.subtitleGroupbox.Controls.Add(this.demuxSelectedTracks);
            this.subtitleGroupbox.Controls.Add(this.keepAllTracks);
            this.subtitleGroupbox.Location = new System.Drawing.Point(2, 78);
            this.subtitleGroupbox.Name = "subtitleGroupbox";
            this.subtitleGroupbox.Size = new System.Drawing.Size(424, 225);
            this.subtitleGroupbox.TabIndex = 14;
            this.subtitleGroupbox.TabStop = false;
            this.subtitleGroupbox.Text = "Subtitles";
            // 
            // subtitleTracks
            // 
            this.subtitleTracks.FormattingEnabled = true;
            this.subtitleTracks.Location = new System.Drawing.Point(50, 72);
            this.subtitleTracks.Name = "subtitleTracks";
            this.subtitleTracks.Size = new System.Drawing.Size(356, 148);
            this.subtitleTracks.TabIndex = 9;
            // 
            // demuxSelectedTracks
            // 
            this.demuxSelectedTracks.Checked = true;
            this.demuxSelectedTracks.Location = new System.Drawing.Point(10, 46);
            this.demuxSelectedTracks.Name = "demuxSelectedTracks";
            this.demuxSelectedTracks.Size = new System.Drawing.Size(336, 24);
            this.demuxSelectedTracks.TabIndex = 8;
            this.demuxSelectedTracks.TabStop = true;
            this.demuxSelectedTracks.Text = "Select Subtitle Streams  (Stream Info File required)";
            // 
            // keepAllTracks
            // 
            this.keepAllTracks.Location = new System.Drawing.Point(10, 20);
            this.keepAllTracks.Name = "keepAllTracks";
            this.keepAllTracks.Size = new System.Drawing.Size(160, 24);
            this.keepAllTracks.TabIndex = 7;
            this.keepAllTracks.Text = "Keep all Subtitle tracks";
            // 
            // closeOnQueue
            // 
            this.closeOnQueue.Location = new System.Drawing.Point(272, 364);
            this.closeOnQueue.Name = "closeOnQueue";
            this.closeOnQueue.Size = new System.Drawing.Size(72, 24);
            this.closeOnQueue.TabIndex = 16;
            this.closeOnQueue.Text = "and close";
            // 
            // queueButton
            // 
            this.queueButton.Location = new System.Drawing.Point(352, 364);
            this.queueButton.Name = "queueButton";
            this.queueButton.Size = new System.Drawing.Size(74, 23);
            this.queueButton.TabIndex = 15;
            this.queueButton.Text = "Queue";
            this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
            // 
            // openIFODialog
            // 
            this.openIFODialog.DefaultExt = "ifo";
            this.openIFODialog.Filter = "IFO Files|*.ifo";
            // 
            // openOutputDialog
            // 
            this.openOutputDialog.DefaultExt = "idx";
            this.openOutputDialog.Filter = "VobSub Files|*.idx";
            this.openOutputDialog.Title = "Choose an output file";
            // 
            // VobSubIndexWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 393);
            this.Controls.Add(this.closeOnQueue);
            this.Controls.Add(this.queueButton);
            this.Controls.Add(this.subtitleGroupbox);
            this.Controls.Add(this.outputGroupbox);
            this.Controls.Add(this.inputGroupbox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VobSubIndexWindow";
            this.ShowInTaskbar = false;
            this.Text = "VobSub Indexer";
            this.inputGroupbox.ResumeLayout(false);
            this.inputGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pgc)).EndInit();
            this.outputGroupbox.ResumeLayout(false);
            this.outputGroupbox.PerformLayout();
            this.subtitleGroupbox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox inputGroupbox;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.TextBox input;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.GroupBox outputGroupbox;
        private System.Windows.Forms.Button pickOutputButton;
        private System.Windows.Forms.TextBox projectName;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.GroupBox subtitleGroupbox;
        private System.Windows.Forms.RadioButton demuxSelectedTracks;
        private System.Windows.Forms.RadioButton keepAllTracks;
        private System.Windows.Forms.CheckBox closeOnQueue;
        private System.Windows.Forms.Button queueButton;
        private System.Windows.Forms.Label pgcLabel;
        private System.Windows.Forms.NumericUpDown pgc;
        private System.Windows.Forms.CheckedListBox subtitleTracks;
        private System.Windows.Forms.OpenFileDialog openIFODialog;
        private System.Windows.Forms.SaveFileDialog openOutputDialog;

    }
}