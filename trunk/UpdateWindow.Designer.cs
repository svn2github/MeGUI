namespace MeGUI
{
    partial class UpdateWindow
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.statusToolStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setIgnoreValue = new System.Windows.Forms.ToolStripMenuItem();
            this.checkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listViewDetails = new System.Windows.Forms.ListView();
            this.colUpdate = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colExistingVersion = new System.Windows.Forms.ColumnHeader();
            this.colLatestVersion = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.txtBoxLog = new System.Windows.Forms.TextBox();
            this.statusToolStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(474, 30);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAbort.Enabled = false;
            this.btnAbort.Location = new System.Drawing.Point(555, 30);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(72, 23);
            this.btnAbort.TabIndex = 4;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar.Location = new System.Drawing.Point(0, 0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(639, 23);
            this.progressBar.TabIndex = 7;
            // 
            // statusToolStrip
            // 
            this.statusToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setIgnoreValue,
            this.checkToolStripMenuItem,
            this.uncheckToolStripMenuItem});
            this.statusToolStrip.Name = "statusToolStrip";
            this.statusToolStrip.Size = new System.Drawing.Size(203, 70);
            // 
            // setIgnoreValue
            // 
            this.setIgnoreValue.CheckOnClick = true;
            this.setIgnoreValue.Name = "setIgnoreValue";
            this.setIgnoreValue.Size = new System.Drawing.Size(202, 22);
            this.setIgnoreValue.Text = "Ignore updates for this file";
            this.setIgnoreValue.Click += new System.EventHandler(this.setIgnoreValue_Click);
            // 
            // checkToolStripMenuItem
            // 
            this.checkToolStripMenuItem.Name = "checkToolStripMenuItem";
            this.checkToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.checkToolStripMenuItem.Text = "Check";
            this.checkToolStripMenuItem.Click += new System.EventHandler(this.checkToolStripMenuItem_Click);
            // 
            // uncheckToolStripMenuItem
            // 
            this.uncheckToolStripMenuItem.Name = "uncheckToolStripMenuItem";
            this.uncheckToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.uncheckToolStripMenuItem.Text = "Uncheck";
            this.uncheckToolStripMenuItem.Click += new System.EventHandler(this.uncheckToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.helpButton1);
            this.panel1.Controls.Add(this.progressBar);
            this.panel1.Controls.Add(this.btnUpdate);
            this.panel1.Controls.Add(this.btnAbort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 310);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(639, 65);
            this.panel1.TabIndex = 12;
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Update window";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(12, 30);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(39, 23);
            this.helpButton1.TabIndex = 8;
            // 
            // splitContainer1
            // 
            this.splitContainer1.DataBindings.Add(new System.Windows.Forms.Binding("SplitterDistance", global::MeGUI.Properties.Settings.Default, "UpdateWindowVerticalSplitterDistance", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(639, 310);
            this.splitContainer1.SplitterDistance = global::MeGUI.Properties.Settings.Default.UpdateWindowVerticalSplitterDistance;
            this.splitContainer1.TabIndex = 11;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(139, 310);
            this.treeView.TabIndex = 10;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // splitContainer2
            // 
            this.splitContainer2.DataBindings.Add(new System.Windows.Forms.Binding("SplitterDistance", global::MeGUI.Properties.Settings.Default, "UpdateWindowHorizontalSplitterDistance", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listViewDetails);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtBoxLog);
            this.splitContainer2.Size = new System.Drawing.Size(496, 310);
            this.splitContainer2.SplitterDistance = global::MeGUI.Properties.Settings.Default.UpdateWindowHorizontalSplitterDistance;
            this.splitContainer2.TabIndex = 0;
            // 
            // listViewDetails
            // 
            this.listViewDetails.CheckBoxes = true;
            this.listViewDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUpdate,
            this.colName,
            this.colExistingVersion,
            this.colLatestVersion,
            this.colStatus});
            this.listViewDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDetails.FullRowSelect = true;
            this.listViewDetails.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewDetails.Location = new System.Drawing.Point(0, 0);
            this.listViewDetails.Name = "listViewDetails";
            this.listViewDetails.Size = new System.Drawing.Size(496, 220);
            this.listViewDetails.TabIndex = 6;
            this.listViewDetails.UseCompatibleStateImageBehavior = false;
            this.listViewDetails.View = System.Windows.Forms.View.Details;
            this.listViewDetails.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewDetails_MouseClick);
            this.listViewDetails.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listViewDetails_ItemCheck);
            // 
            // colUpdate
            // 
            this.colUpdate.Text = "Update";
            this.colUpdate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colUpdate.Width = global::MeGUI.Properties.Settings.Default.UpdateColumnWidth;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = global::MeGUI.Properties.Settings.Default.UpdateNameColumnWidth;
            // 
            // colExistingVersion
            // 
            this.colExistingVersion.Text = "Existing Version";
            this.colExistingVersion.Width = global::MeGUI.Properties.Settings.Default.UpdateExistingVersionColumnWidth;
            // 
            // colLatestVersion
            // 
            this.colLatestVersion.Text = "Latest Version";
            this.colLatestVersion.Width = global::MeGUI.Properties.Settings.Default.UpdateLatestVersionColumnWidth;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            this.colStatus.Width = global::MeGUI.Properties.Settings.Default.UpdateStatusColumnWidth;
            // 
            // txtBoxLog
            // 
            this.txtBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxLog.Location = new System.Drawing.Point(0, 0);
            this.txtBoxLog.Multiline = true;
            this.txtBoxLog.Name = "txtBoxLog";
            this.txtBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxLog.Size = new System.Drawing.Size(496, 86);
            this.txtBoxLog.TabIndex = 9;
            // 
            // UpdateWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = global::MeGUI.Properties.Settings.Default.UpdateWindowSize;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::MeGUI.Properties.Settings.Default, "UpdateWindowLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("ClientSize", global::MeGUI.Properties.Settings.Default, "UpdateWindowSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("WindowState", global::MeGUI.Properties.Settings.Default, "UpdateWindowState", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Location = global::MeGUI.Properties.Settings.Default.UpdateWindowLocation;
            this.Name = "UpdateWindow";
            this.Text = "UpdateWindow";
            this.WindowState = global::MeGUI.Properties.Settings.Default.UpdateWindowState;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdateWindow_FormClosing);
            this.Load += new System.EventHandler(this.UpdateWindow_Load);
            this.statusToolStrip.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.ListView listViewDetails;
        private System.Windows.Forms.ColumnHeader colUpdate;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colExistingVersion;
        private System.Windows.Forms.ColumnHeader colLatestVersion;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtBoxLog;
        private System.Windows.Forms.ContextMenuStrip statusToolStrip;
        private System.Windows.Forms.ToolStripMenuItem setIgnoreValue;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ToolStripMenuItem checkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private MeGUI.core.gui.HelpButton helpButton1;
    }
}