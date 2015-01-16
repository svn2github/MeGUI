// ****************************************************************************
// 
// Copyright (C) 2005-2015 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************


namespace MeGUI
{
  partial class frmStreamSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStreamSelect));
            this.btnOK = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnSortDuration = new System.Windows.Forms.RadioButton();
            this.btnSortName = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSortChapter = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(127, 264);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 22);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(13, 41);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(423, 212);
            this.listBox1.TabIndex = 2;
            this.listBox1.DoubleClick += new System.EventHandler(this.btnOK_Click);
            // 
            // btnSortDuration
            // 
            this.btnSortDuration.AutoSize = true;
            this.btnSortDuration.Location = new System.Drawing.Point(13, 12);
            this.btnSortDuration.Name = "btnSortDuration";
            this.btnSortDuration.Size = new System.Drawing.Size(102, 17);
            this.btnSortDuration.TabIndex = 3;
            this.btnSortDuration.Text = "sort by duration";
            this.btnSortDuration.UseVisualStyleBackColor = true;
            this.btnSortDuration.CheckedChanged += new System.EventHandler(this.btnSort_CheckedChanged);
            // 
            // btnSortName
            // 
            this.btnSortName.AutoSize = true;
            this.btnSortName.Checked = true;
            this.btnSortName.Location = new System.Drawing.Point(175, 12);
            this.btnSortName.Name = "btnSortName";
            this.btnSortName.Size = new System.Drawing.Size(88, 17);
            this.btnSortName.TabIndex = 4;
            this.btnSortName.Text = "sort by name";
            this.btnSortName.UseVisualStyleBackColor = true;
            this.btnSortName.CheckedChanged += new System.EventHandler(this.btnSort_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(227, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 22);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSortChapter
            // 
            this.btnSortChapter.AutoSize = true;
            this.btnSortChapter.Location = new System.Drawing.Point(307, 12);
            this.btnSortChapter.Name = "btnSortChapter";
            this.btnSortChapter.Size = new System.Drawing.Size(129, 17);
            this.btnSortChapter.TabIndex = 7;
            this.btnSortChapter.Text = "sort by chapter count";
            this.btnSortChapter.UseVisualStyleBackColor = true;
            this.btnSortChapter.CheckedChanged += new System.EventHandler(this.btnSort_CheckedChanged);
            // 
            // frmStreamSelect
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(448, 298);
            this.ControlBox = false;
            this.Controls.Add(this.btnSortChapter);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSortName);
            this.Controls.Add(this.btnSortDuration);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStreamSelect";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Select your list";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.RadioButton btnSortDuration;
    private System.Windows.Forms.RadioButton btnSortName;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.RadioButton btnSortChapter;
  }
}