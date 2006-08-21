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
            this.listViewDetails = new System.Windows.Forms.ListView();
            this.colUpdate = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colExistingVersion = new System.Windows.Forms.ColumnHeader();
            this.colLatestVersion = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtBoxLog = new System.Windows.Forms.TextBox();
            this.statusToolStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setIgnoreValue = new System.Windows.Forms.ToolStripMenuItem();
            this.checkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView = new System.Windows.Forms.TreeView();
            this.statusToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(185, 309);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Enabled = false;
            this.btnAbort.Location = new System.Drawing.Point(266, 309);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(72, 23);
            this.btnAbort.TabIndex = 4;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
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
            this.listViewDetails.FullRowSelect = true;
            this.listViewDetails.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewDetails.Location = new System.Drawing.Point(170, 12);
            this.listViewDetails.Name = "listViewDetails";
            this.listViewDetails.Size = new System.Drawing.Size(504, 185);
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
            this.colUpdate.Width = 50;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 100;
            // 
            // colExistingVersion
            // 
            this.colExistingVersion.Text = "Existing Version";
            this.colExistingVersion.Width = 100;
            // 
            // colLatestVersion
            // 
            this.colLatestVersion.Text = "Latest Version";
            this.colLatestVersion.Width = 100;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            this.colStatus.Width = 135;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(170, 282);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(504, 21);
            this.progressBar.TabIndex = 7;
            // 
            // txtBoxLog
            // 
            this.txtBoxLog.Location = new System.Drawing.Point(170, 203);
            this.txtBoxLog.Multiline = true;
            this.txtBoxLog.Name = "txtBoxLog";
            this.txtBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxLog.Size = new System.Drawing.Size(504, 73);
            this.txtBoxLog.TabIndex = 9;
            // 
            // statusToolStrip
            // 
            this.statusToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setIgnoreValue,
            this.checkToolStripMenuItem,
            this.uncheckToolStripMenuItem});
            this.statusToolStrip.Name = "statusToolStrip";
            this.statusToolStrip.Size = new System.Drawing.Size(214, 70);
            // 
            // setIgnoreValue
            // 
            this.setIgnoreValue.CheckOnClick = true;
            this.setIgnoreValue.Name = "setIgnoreValue";
            this.setIgnoreValue.Size = new System.Drawing.Size(213, 22);
            this.setIgnoreValue.Text = "Ignore updates for this file";
            this.setIgnoreValue.Click += new System.EventHandler(this.setIgnoreValue_Click);
            // 
            // checkToolStripMenuItem
            // 
            this.checkToolStripMenuItem.Name = "checkToolStripMenuItem";
            this.checkToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.checkToolStripMenuItem.Text = "Check";
            this.checkToolStripMenuItem.Click += new System.EventHandler(this.checkToolStripMenuItem_Click);
            // 
            // uncheckToolStripMenuItem
            // 
            this.uncheckToolStripMenuItem.Name = "uncheckToolStripMenuItem";
            this.uncheckToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.uncheckToolStripMenuItem.Text = "Uncheck";
            this.uncheckToolStripMenuItem.Click += new System.EventHandler(this.uncheckToolStripMenuItem_Click);
            // 
            // treeView
            // 
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(12, 12);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(152, 291);
            this.treeView.TabIndex = 10;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // UpdateWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 343);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.listViewDetails);
            this.Controls.Add(this.txtBoxLog);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UpdateWindow";
            this.Text = "UpdateWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdateWindow_FormClosing);
            this.Load += new System.EventHandler(this.UpdateWindow_Load);
            this.statusToolStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}