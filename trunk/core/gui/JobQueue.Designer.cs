namespace MeGUI.core.gui
{
    partial class JobQueue
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
            System.Windows.Forms.Button abortButton;
            System.Windows.Forms.Button loadJobButton;
            System.Windows.Forms.Button updateJobButton;
            System.Windows.Forms.Button button9;
            System.Windows.Forms.ColumnHeader jobColumHeader;
            System.Windows.Forms.ColumnHeader inputColumnHeader;
            System.Windows.Forms.ColumnHeader outputColumnHeader;
            System.Windows.Forms.ColumnHeader codecHeader;
            System.Windows.Forms.ColumnHeader modeHeader;
            System.Windows.Forms.ColumnHeader statusColumn;
            System.Windows.Forms.ColumnHeader startColumn;
            System.Windows.Forms.ColumnHeader endColumn;
            System.Windows.Forms.ColumnHeader fpsColumn;
            this.upButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.queueListView = new System.Windows.Forms.ListView();
            this.ownerHeader = new System.Windows.Forms.ColumnHeader();
            this.queueContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PostponedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WaitingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AbortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.startStopButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            abortButton = new System.Windows.Forms.Button();
            loadJobButton = new System.Windows.Forms.Button();
            updateJobButton = new System.Windows.Forms.Button();
            button9 = new System.Windows.Forms.Button();
            jobColumHeader = new System.Windows.Forms.ColumnHeader();
            inputColumnHeader = new System.Windows.Forms.ColumnHeader();
            outputColumnHeader = new System.Windows.Forms.ColumnHeader();
            codecHeader = new System.Windows.Forms.ColumnHeader();
            modeHeader = new System.Windows.Forms.ColumnHeader();
            statusColumn = new System.Windows.Forms.ColumnHeader();
            startColumn = new System.Windows.Forms.ColumnHeader();
            endColumn = new System.Windows.Forms.ColumnHeader();
            fpsColumn = new System.Windows.Forms.ColumnHeader();
            this.queueContextMenu.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // abortButton
            // 
            abortButton.AutoSize = true;
            abortButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            abortButton.Location = new System.Drawing.Point(123, 3);
            abortButton.Margin = new System.Windows.Forms.Padding(3, 3, 7, 3);
            abortButton.Name = "abortButton";
            abortButton.Size = new System.Drawing.Size(42, 23);
            abortButton.TabIndex = 0;
            abortButton.Text = "Abort";
            abortButton.UseVisualStyleBackColor = true;
            abortButton.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // loadJobButton
            // 
            loadJobButton.AutoSize = true;
            loadJobButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            loadJobButton.Enabled = false;
            loadJobButton.Location = new System.Drawing.Point(179, 3);
            loadJobButton.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            loadJobButton.Name = "loadJobButton";
            loadJobButton.Size = new System.Drawing.Size(41, 23);
            loadJobButton.TabIndex = 0;
            loadJobButton.Text = "Load";
            loadJobButton.UseVisualStyleBackColor = true;
            // 
            // updateJobButton
            // 
            updateJobButton.AutoSize = true;
            updateJobButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            updateJobButton.Enabled = false;
            updateJobButton.Location = new System.Drawing.Point(226, 3);
            updateJobButton.Margin = new System.Windows.Forms.Padding(3, 3, 7, 3);
            updateJobButton.Name = "updateJobButton";
            updateJobButton.Size = new System.Drawing.Size(52, 23);
            updateJobButton.TabIndex = 0;
            updateJobButton.Text = "Update";
            updateJobButton.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            button9.AutoSize = true;
            button9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button9.Location = new System.Drawing.Point(384, 3);
            button9.Name = "button9";
            button9.Size = new System.Drawing.Size(48, 23);
            button9.TabIndex = 0;
            button9.Text = "Delete";
            button9.UseVisualStyleBackColor = true;
            button9.Click += new System.EventHandler(this.deleteJobButton_Click);
            // 
            // jobColumHeader
            // 
            jobColumHeader.Text = "Name";
            jobColumHeader.Width = global::MeGUI.Properties.Settings.Default.JobColumnWidth;
            // 
            // inputColumnHeader
            // 
            inputColumnHeader.Text = "Input";
            inputColumnHeader.Width = global::MeGUI.Properties.Settings.Default.InputColumnWidth;
            // 
            // outputColumnHeader
            // 
            outputColumnHeader.Text = "Output";
            outputColumnHeader.Width = global::MeGUI.Properties.Settings.Default.OutputColumnWidth;
            // 
            // codecHeader
            // 
            codecHeader.Text = "Codec";
            codecHeader.Width = global::MeGUI.Properties.Settings.Default.CodecColumnWidth;
            // 
            // modeHeader
            // 
            modeHeader.Text = "Mode";
            modeHeader.Width = global::MeGUI.Properties.Settings.Default.ModeColumnWidth;
            // 
            // statusColumn
            // 
            statusColumn.Text = "Status";
            statusColumn.Width = global::MeGUI.Properties.Settings.Default.StatusColumnWidth;
            // 
            // startColumn
            // 
            startColumn.Text = "Start";
            startColumn.Width = global::MeGUI.Properties.Settings.Default.StartColumnWidth;
            // 
            // endColumn
            // 
            endColumn.Text = "End";
            endColumn.Width = global::MeGUI.Properties.Settings.Default.EndColumnWidth;
            // 
            // fpsColumn
            // 
            fpsColumn.Text = "FPS";
            fpsColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            fpsColumn.Width = global::MeGUI.Properties.Settings.Default.FPSColumnWidth;
            // 
            // upButton
            // 
            this.upButton.AutoSize = true;
            this.upButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.upButton.Location = new System.Drawing.Point(292, 3);
            this.upButton.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(31, 23);
            this.upButton.TabIndex = 0;
            this.upButton.Text = "Up";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // downButton
            // 
            this.downButton.AutoSize = true;
            this.downButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.downButton.Location = new System.Drawing.Point(329, 3);
            this.downButton.Margin = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(45, 23);
            this.downButton.TabIndex = 0;
            this.downButton.Text = "Down";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // queueListView
            // 
            this.queueListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            jobColumHeader,
            inputColumnHeader,
            outputColumnHeader,
            codecHeader,
            modeHeader,
            statusColumn,
            this.ownerHeader,
            startColumn,
            endColumn,
            fpsColumn});
            this.queueListView.ContextMenuStrip = this.queueContextMenu;
            this.queueListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queueListView.FullRowSelect = true;
            this.queueListView.HideSelection = false;
            this.queueListView.Location = new System.Drawing.Point(0, 0);
            this.queueListView.Name = "queueListView";
            this.queueListView.Size = new System.Drawing.Size(692, 513);
            this.queueListView.TabIndex = 27;
            this.queueListView.UseCompatibleStateImageBehavior = false;
            this.queueListView.View = System.Windows.Forms.View.Details;
            this.queueListView.DoubleClick += new System.EventHandler(this.queueListView_DoubleClick);
            this.queueListView.VisibleChanged += new System.EventHandler(this.queueListView_VisibleChanged);
            // 
            // ownerHeader
            // 
            this.ownerHeader.Text = "Owner";
            this.ownerHeader.Width = global::MeGUI.Properties.Settings.Default.OwnerWidth;
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
            this.AbortMenuItem.Click += new System.EventHandler(this.AbortMenuItem_Click);
            // 
            // LoadMenuItem
            // 
            this.LoadMenuItem.Enabled = false;
            this.LoadMenuItem.Name = "LoadMenuItem";
            this.LoadMenuItem.ShortcutKeyDisplayString = "";
            this.LoadMenuItem.Size = new System.Drawing.Size(144, 22);
            this.LoadMenuItem.Text = "&Load";
            this.LoadMenuItem.ToolTipText = "Load into MeGUI";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.startStopButton);
            this.flowLayoutPanel1.Controls.Add(this.stopButton);
            this.flowLayoutPanel1.Controls.Add(this.pauseButton);
            this.flowLayoutPanel1.Controls.Add(abortButton);
            this.flowLayoutPanel1.Controls.Add(loadJobButton);
            this.flowLayoutPanel1.Controls.Add(updateJobButton);
            this.flowLayoutPanel1.Controls.Add(this.upButton);
            this.flowLayoutPanel1.Controls.Add(this.downButton);
            this.flowLayoutPanel1.Controls.Add(button9);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 513);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(692, 29);
            this.flowLayoutPanel1.TabIndex = 37;
            // 
            // startStopButton
            // 
            this.startStopButton.AutoSize = true;
            this.startStopButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.startStopButton.Location = new System.Drawing.Point(3, 3);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(39, 23);
            this.startStopButton.TabIndex = 0;
            this.startStopButton.Text = "Start";
            this.startStopButton.UseVisualStyleBackColor = true;
            this.startStopButton.Click += new System.EventHandler(this.startStopButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.AutoSize = true;
            this.stopButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.stopButton.Location = new System.Drawing.Point(48, 3);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(39, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(93, 3);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(24, 23);
            this.pauseButton.TabIndex = 0;
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // JobQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.queueListView);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "JobQueue";
            this.Size = new System.Drawing.Size(692, 542);
            this.queueContextMenu.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView queueListView;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button startStopButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.ContextMenuStrip queueContextMenu;
        private System.Windows.Forms.ToolStripMenuItem DeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StatusMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PostponedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem WaitingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AbortMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadMenuItem;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ColumnHeader ownerHeader;
    }
}
