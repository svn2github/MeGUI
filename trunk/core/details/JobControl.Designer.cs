namespace MeGUI.core.details
{
    partial class JobControl
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
            this.components = new System.ComponentModel.Container();
            this.pauseButton = new System.Windows.Forms.Button();
            this.updateJobButton = new System.Windows.Forms.Button();
            this.deleteAllJobsButton = new System.Windows.Forms.Button();
            this.jobProgress = new System.Windows.Forms.ProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.loadJobButton = new System.Windows.Forms.Button();
            this.abortButton = new System.Windows.Forms.Button();
            this.startStopButton = new System.Windows.Forms.Button();
            this.deleteJobButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.queueListView = new System.Windows.Forms.ListView();
            this.jobColumHeader = new System.Windows.Forms.ColumnHeader();
            this.inputColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.outputColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.codecHeader = new System.Windows.Forms.ColumnHeader();
            this.modeHeader = new System.Windows.Forms.ColumnHeader();
            this.statusColumn = new System.Windows.Forms.ColumnHeader();
            this.startColumn = new System.Windows.Forms.ColumnHeader();
            this.endColumn = new System.Windows.Forms.ColumnHeader();
            this.fpsColumn = new System.Windows.Forms.ColumnHeader();
            this.queueContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PostponedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WaitingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AbortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.afterEncoding = new System.Windows.Forms.Label();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.queueContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pauseButton
            // 
            this.pauseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pauseButton.Enabled = false;
            this.pauseButton.Location = new System.Drawing.Point(55, 272);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(24, 24);
            this.pauseButton.TabIndex = 26;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // updateJobButton
            // 
            this.updateJobButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.updateJobButton.Location = new System.Drawing.Point(187, 273);
            this.updateJobButton.Name = "updateJobButton";
            this.updateJobButton.Size = new System.Drawing.Size(50, 23);
            this.updateJobButton.TabIndex = 25;
            this.updateJobButton.Text = "Update";
            this.updateJobButton.Click += new System.EventHandler(this.updateJobButton_Click);
            // 
            // deleteAllJobsButton
            // 
            this.deleteAllJobsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteAllJobsButton.Location = new System.Drawing.Point(365, 272);
            this.deleteAllJobsButton.Name = "deleteAllJobsButton";
            this.deleteAllJobsButton.Size = new System.Drawing.Size(48, 23);
            this.deleteAllJobsButton.TabIndex = 24;
            this.deleteAllJobsButton.Text = "Clear";
            this.deleteAllJobsButton.Click += new System.EventHandler(this.deleteAllJobsButton_Click);
            // 
            // jobProgress
            // 
            this.jobProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.jobProgress.Location = new System.Drawing.Point(62, 328);
            this.jobProgress.Name = "jobProgress";
            this.jobProgress.Size = new System.Drawing.Size(405, 23);
            this.jobProgress.TabIndex = 23;
            // 
            // progressLabel
            // 
            this.progressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressLabel.Location = new System.Drawing.Point(6, 332);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(50, 15);
            this.progressLabel.TabIndex = 22;
            this.progressLabel.Text = "Progress";
            // 
            // loadJobButton
            // 
            this.loadJobButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadJobButton.Location = new System.Drawing.Point(141, 272);
            this.loadJobButton.Name = "loadJobButton";
            this.loadJobButton.Size = new System.Drawing.Size(40, 23);
            this.loadJobButton.TabIndex = 21;
            this.loadJobButton.Text = "Load";
            this.loadJobButton.Click += new System.EventHandler(this.loadJobButton_Click);
            // 
            // abortButton
            // 
            this.abortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.abortButton.Enabled = false;
            this.abortButton.Location = new System.Drawing.Point(85, 272);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(42, 23);
            this.abortButton.TabIndex = 20;
            this.abortButton.Text = "Abort";
            this.abortButton.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // startStopButton
            // 
            this.startStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startStopButton.Location = new System.Drawing.Point(9, 272);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(40, 23);
            this.startStopButton.TabIndex = 19;
            this.startStopButton.Text = "Start";
            this.startStopButton.Click += new System.EventHandler(this.startStopButton_Click);
            // 
            // deleteJobButton
            // 
            this.deleteJobButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteJobButton.Location = new System.Drawing.Point(419, 272);
            this.deleteJobButton.Name = "deleteJobButton";
            this.deleteJobButton.Size = new System.Drawing.Size(48, 23);
            this.deleteJobButton.TabIndex = 18;
            this.deleteJobButton.Text = "Delete";
            this.deleteJobButton.Click += new System.EventHandler(this.deleteJobButton_Click);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Location = new System.Drawing.Point(299, 272);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(44, 23);
            this.downButton.TabIndex = 17;
            this.downButton.Text = "Down";
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Location = new System.Drawing.Point(253, 272);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(40, 24);
            this.upButton.TabIndex = 16;
            this.upButton.Text = "Up";
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // queueListView
            // 
            this.queueListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.queueListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.jobColumHeader,
            this.inputColumnHeader,
            this.outputColumnHeader,
            this.codecHeader,
            this.modeHeader,
            this.statusColumn,
            this.startColumn,
            this.endColumn,
            this.fpsColumn});
            this.queueListView.ContextMenuStrip = this.queueContextMenu;
            this.queueListView.FullRowSelect = true;
            this.queueListView.HideSelection = false;
            this.queueListView.Location = new System.Drawing.Point(3, 0);
            this.queueListView.Name = "queueListView";
            this.queueListView.Size = new System.Drawing.Size(464, 256);
            this.queueListView.TabIndex = 15;
            this.queueListView.UseCompatibleStateImageBehavior = false;
            this.queueListView.View = System.Windows.Forms.View.Details;
            this.queueListView.DoubleClick += new System.EventHandler(this.queueListView_DoubleClick);
            this.queueListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.queueListView_ItemSelectionChanged);
            // 
            // jobColumHeader
            // 
            this.jobColumHeader.Text = "Name";
            this.jobColumHeader.Width = 40;
            // 
            // inputColumnHeader
            // 
            this.inputColumnHeader.Text = "Input";
            this.inputColumnHeader.Width = 89;
            // 
            // outputColumnHeader
            // 
            this.outputColumnHeader.Text = "Output";
            this.outputColumnHeader.Width = 89;
            // 
            // codecHeader
            // 
            this.codecHeader.Text = "Codec";
            this.codecHeader.Width = 43;
            // 
            // modeHeader
            // 
            this.modeHeader.Text = "Mode";
            this.modeHeader.Width = 75;
            // 
            // statusColumn
            // 
            this.statusColumn.Text = "Status";
            this.statusColumn.Width = 51;
            // 
            // startColumn
            // 
            this.startColumn.Text = "Start";
            this.startColumn.Width = 55;
            // 
            // endColumn
            // 
            this.endColumn.Text = "End";
            this.endColumn.Width = 55;
            // 
            // fpsColumn
            // 
            this.fpsColumn.Text = "FPS";
            this.fpsColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.fpsColumn.Width = 35;
            // 
            // queueContextMenu
            // 
            this.queueContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteMenuItem,
            this.StatusMenuItem,
            this.AbortMenuItem,
            this.LoadMenuItem});
            this.queueContextMenu.Name = "queueContextMenu";
            this.queueContextMenu.Size = new System.Drawing.Size(145, 92);
            this.queueContextMenu.Opened += new System.EventHandler(this.queueContextMenu_Opened);
            // 
            // DeleteMenuItem
            // 
            this.DeleteMenuItem.Name = "DeleteMenuItem";
            this.DeleteMenuItem.ShortcutKeyDisplayString = "";
            this.DeleteMenuItem.Size = new System.Drawing.Size(144, 22);
            this.DeleteMenuItem.Text = "&Delete";
            this.DeleteMenuItem.ToolTipText = "Delete this job";
            this.DeleteMenuItem.Click += new System.EventHandler(this.deleteJobButton_Click);
            // 
            // StatusMenuItem
            // 
            this.StatusMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PostponedMenuItem,
            this.WaitingMenuItem});
            this.StatusMenuItem.Name = "StatusMenuItem";
            this.StatusMenuItem.Size = new System.Drawing.Size(144, 22);
            this.StatusMenuItem.Text = "&Change status";
            // 
            // PostponedMenuItem
            // 
            this.PostponedMenuItem.Name = "PostponedMenuItem";
            this.PostponedMenuItem.Size = new System.Drawing.Size(125, 22);
            this.PostponedMenuItem.Text = "&Postponed";
            this.PostponedMenuItem.Click += new System.EventHandler(this.postponeMenuItem_Click);
            // 
            // WaitingMenuItem
            // 
            this.WaitingMenuItem.Name = "WaitingMenuItem";
            this.WaitingMenuItem.Size = new System.Drawing.Size(125, 22);
            this.WaitingMenuItem.Text = "&Waiting";
            this.WaitingMenuItem.Click += new System.EventHandler(this.waitingMenuItem_Click);
            // 
            // AbortMenuItem
            // 
            this.AbortMenuItem.Name = "AbortMenuItem";
            this.AbortMenuItem.ShortcutKeyDisplayString = "";
            this.AbortMenuItem.Size = new System.Drawing.Size(144, 22);
            this.AbortMenuItem.Text = "&Abort";
            this.AbortMenuItem.ToolTipText = "Abort this job";
            this.AbortMenuItem.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // LoadMenuItem
            // 
            this.LoadMenuItem.Name = "LoadMenuItem";
            this.LoadMenuItem.ShortcutKeyDisplayString = "";
            this.LoadMenuItem.Size = new System.Drawing.Size(144, 22);
            this.LoadMenuItem.Text = "&Load";
            this.LoadMenuItem.ToolTipText = "Load into MeGUI";
            this.LoadMenuItem.Click += new System.EventHandler(this.loadJobButton_Click);
            // 
            // afterEncoding
            // 
            this.afterEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.afterEncoding.AutoSize = true;
            this.afterEncoding.Location = new System.Drawing.Point(6, 304);
            this.afterEncoding.Name = "afterEncoding";
            this.afterEncoding.Size = new System.Drawing.Size(132, 13);
            this.afterEncoding.TabIndex = 27;
            this.afterEncoding.Text = "After encoding: do nothing";
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton1.ArticleName = "Main window#Queue";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(428, 299);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(39, 23);
            this.helpButton1.TabIndex = 28;
            // 
            // JobControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.afterEncoding);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.updateJobButton);
            this.Controls.Add(this.deleteAllJobsButton);
            this.Controls.Add(this.jobProgress);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.loadJobButton);
            this.Controls.Add(this.abortButton);
            this.Controls.Add(this.startStopButton);
            this.Controls.Add(this.deleteJobButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.queueListView);
            this.Name = "JobControl";
            this.Size = new System.Drawing.Size(470, 355);
            this.queueContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button updateJobButton;
        private System.Windows.Forms.Button deleteAllJobsButton;
        private System.Windows.Forms.ProgressBar jobProgress;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button loadJobButton;
        private System.Windows.Forms.Button abortButton;
        private System.Windows.Forms.Button startStopButton;
        private System.Windows.Forms.Button deleteJobButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.ListView queueListView;
        private System.Windows.Forms.ColumnHeader jobColumHeader;
        private System.Windows.Forms.ColumnHeader inputColumnHeader;
        private System.Windows.Forms.ColumnHeader outputColumnHeader;
        private System.Windows.Forms.ColumnHeader codecHeader;
        private System.Windows.Forms.ColumnHeader modeHeader;
        private System.Windows.Forms.ColumnHeader statusColumn;
        private System.Windows.Forms.ColumnHeader startColumn;
        private System.Windows.Forms.ColumnHeader endColumn;
        private System.Windows.Forms.ColumnHeader fpsColumn;
        private System.Windows.Forms.ContextMenuStrip queueContextMenu;
        private System.Windows.Forms.ToolStripMenuItem DeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StatusMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PostponedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem WaitingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AbortMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadMenuItem;
        private System.Windows.Forms.Label afterEncoding;
        private MeGUI.core.gui.HelpButton helpButton1;
    }
}
