namespace MeGUI.core.gui
{
    partial class LogTree
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
            this.treeView = new System.Windows.Forms.TreeView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editIndividualNode = new System.Windows.Forms.ToolStripMenuItem();
            this.editBranch = new System.Windows.Forms.ToolStripMenuItem();
            this.editLog = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveBranch = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLog = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.ContextMenuStrip = this.contextMenu;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(596, 478);
            this.treeView.TabIndex = 0;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editTextToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // editTextToolStripMenuItem
            // 
            this.editTextToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editIndividualNode,
            this.editBranch,
            this.editLog});
            this.editTextToolStripMenuItem.Name = "editTextToolStripMenuItem";
            this.editTextToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editTextToolStripMenuItem.Text = "Edit text";
            // 
            // editIndividualNode
            // 
            this.editIndividualNode.Name = "editIndividualNode";
            this.editIndividualNode.Size = new System.Drawing.Size(152, 22);
            this.editIndividualNode.Text = "node";
            this.editIndividualNode.Click += new System.EventHandler(this.ofIndividualNodeToolStripMenuItem_Click);
            // 
            // editBranch
            // 
            this.editBranch.Name = "editBranch";
            this.editBranch.Size = new System.Drawing.Size(152, 22);
            this.editBranch.Text = "branch";
            this.editBranch.Click += new System.EventHandler(this.ofBranchToolStripMenuItem_Click);
            // 
            // editLog
            // 
            this.editLog.Name = "editLog";
            this.editLog.Size = new System.Drawing.Size(152, 22);
            this.editLog.Text = "log";
            this.editLog.Click += new System.EventHandler(this.editLog_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveBranch,
            this.saveLog});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveBranch
            // 
            this.saveBranch.Name = "saveBranch";
            this.saveBranch.Size = new System.Drawing.Size(107, 22);
            this.saveBranch.Text = "branch";
            this.saveBranch.Click += new System.EventHandler(this.saveBranch_Click);
            // 
            // saveLog
            // 
            this.saveLog.Name = "saveLog";
            this.saveLog.Size = new System.Drawing.Size(107, 22);
            this.saveLog.Text = "log";
            this.saveLog.Click += new System.EventHandler(this.saveLog_Click);
            // 
            // saveDialog
            // 
            this.saveDialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            this.saveDialog.FilterIndex = 0;
            this.saveDialog.Title = "Select output file";
            // 
            // LogTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView);
            this.Name = "LogTree";
            this.Size = new System.Drawing.Size(596, 478);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem editTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editIndividualNode;
        private System.Windows.Forms.ToolStripMenuItem editBranch;
        private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveBranch;
        private System.Windows.Forms.ToolStripMenuItem saveLog;
        private System.Windows.Forms.ToolStripMenuItem editLog;
    }
}
