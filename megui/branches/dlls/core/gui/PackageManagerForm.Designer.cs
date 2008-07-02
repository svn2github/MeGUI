namespace MeGUI.core.gui
{
    partial class PackageManagerForm
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.TabControl tabControl1;
            System.Windows.Forms.TabPage tabPage1;
            System.Windows.Forms.SplitContainer splitContainer2;
            System.Windows.Forms.TabPage tabPage2;
            System.Windows.Forms.SplitContainer splitContainer1;
            this.availableLibraries = new System.Windows.Forms.CheckedListBox();
            this.libraryInformation = new System.Windows.Forms.TextBox();
            this.componentTypes = new System.Windows.Forms.TreeView();
            this.activeComponents = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            tabPage2 = new System.Windows.Forms.TabPage();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tabPage2.SuspendLayout();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.Dock = System.Windows.Forms.DockStyle.Top;
            label2.Location = new System.Drawing.Point(0, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(490, 13);
            label2.TabIndex = 7;
            label2.Text = "Selected library information";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(490, 17);
            label1.TabIndex = 5;
            label1.Text = "Available Libraries";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(504, 518);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(splitContainer2);
            tabPage1.Location = new System.Drawing.Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(496, 492);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Libraries";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(3, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(this.availableLibraries);
            splitContainer2.Panel1.Controls.Add(label1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(label2);
            splitContainer2.Panel2.Controls.Add(this.libraryInformation);
            splitContainer2.Size = new System.Drawing.Size(490, 486);
            splitContainer2.SplitterDistance = 240;
            splitContainer2.TabIndex = 8;
            // 
            // availableLibraries
            // 
            this.availableLibraries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.availableLibraries.FormattingEnabled = true;
            this.availableLibraries.Location = new System.Drawing.Point(0, 17);
            this.availableLibraries.Name = "availableLibraries";
            this.availableLibraries.Size = new System.Drawing.Size(490, 214);
            this.availableLibraries.TabIndex = 4;
            // 
            // libraryInformation
            // 
            this.libraryInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.libraryInformation.Location = new System.Drawing.Point(5, 16);
            this.libraryInformation.Multiline = true;
            this.libraryInformation.Name = "libraryInformation";
            this.libraryInformation.ReadOnly = true;
            this.libraryInformation.Size = new System.Drawing.Size(480, 221);
            this.libraryInformation.TabIndex = 6;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(splitContainer1);
            tabPage2.Location = new System.Drawing.Point(4, 22);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(496, 492);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Components";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(3, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(this.componentTypes);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this.activeComponents);
            splitContainer1.Size = new System.Drawing.Size(490, 486);
            splitContainer1.SplitterDistance = 153;
            splitContainer1.TabIndex = 2;
            // 
            // componentTypes
            // 
            this.componentTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.componentTypes.Location = new System.Drawing.Point(0, 0);
            this.componentTypes.Name = "componentTypes";
            this.componentTypes.Size = new System.Drawing.Size(153, 486);
            this.componentTypes.TabIndex = 0;
            // 
            // activeComponents
            // 
            this.activeComponents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.activeComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.activeComponents.FullRowSelect = true;
            this.activeComponents.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.activeComponents.Location = new System.Drawing.Point(0, 0);
            this.activeComponents.Name = "activeComponents";
            this.activeComponents.Size = new System.Drawing.Size(333, 486);
            this.activeComponents.TabIndex = 1;
            this.activeComponents.UseCompatibleStateImageBehavior = false;
            this.activeComponents.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Active";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 172;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Contained in";
            this.columnHeader3.Width = 148;
            // 
            // PackageManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 518);
            this.Controls.Add(tabControl1);
            this.Name = "PackageManagerForm";
            this.Text = "PackageManagerForm";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            splitContainer2.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox availableLibraries;
        private System.Windows.Forms.TextBox libraryInformation;
        private System.Windows.Forms.TreeView componentTypes;
        private System.Windows.Forms.ListView activeComponents;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;

    }
}