namespace MeGUI.core.gui
{
    partial class NiceComboBox
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
            this.dropDownButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.helloToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dropDownButton
            // 
            this.dropDownButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dropDownButton.AutoSize = true;
            this.dropDownButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dropDownButton.Font = new System.Drawing.Font("Marlett", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.dropDownButton.Location = new System.Drawing.Point(177, 3);
            this.dropDownButton.Name = "dropDownButton";
            this.dropDownButton.Size = new System.Drawing.Size(28, 22);
            this.dropDownButton.TabIndex = 0;
            this.dropDownButton.Text = "6";
            this.dropDownButton.UseVisualStyleBackColor = true;
            this.dropDownButton.Click += new System.EventHandler(this.dropDownButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(170, 20);
            this.textBox1.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helloToolStripMenuItem,
            this.hereToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripTextBox1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 77);
            // 
            // helloToolStripMenuItem
            // 
            this.helloToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hiToolStripMenuItem});
            this.helloToolStripMenuItem.Name = "helloToolStripMenuItem";
            this.helloToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.helloToolStripMenuItem.Text = "Hello";
            // 
            // hiToolStripMenuItem
            // 
            this.hiToolStripMenuItem.Name = "hiToolStripMenuItem";
            this.hiToolStripMenuItem.Size = new System.Drawing.Size(83, 22);
            this.hiToolStripMenuItem.Text = "Hi";
            // 
            // hereToolStripMenuItem
            // 
            this.hereToolStripMenuItem.Name = "hereToolStripMenuItem";
            this.hereToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.hereToolStripMenuItem.Text = "Here";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 21);
            this.toolStripTextBox1.Text = "aoeu";
            // 
            // NiceComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dropDownButton);
            this.MaximumSize = new System.Drawing.Size(1000, 29);
            this.MinimumSize = new System.Drawing.Size(64, 29);
            this.Name = "NiceComboBox";
            this.Size = new System.Drawing.Size(208, 29);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button dropDownButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helloToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hereToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
    }
}
