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
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(182, 264);
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
            this.btnSortDuration.Location = new System.Drawing.Point(91, 13);
            this.btnSortDuration.Name = "btnSortDuration";
            this.btnSortDuration.Size = new System.Drawing.Size(102, 17);
            this.btnSortDuration.TabIndex = 3;
            this.btnSortDuration.Text = "sort by duration";
            this.btnSortDuration.UseVisualStyleBackColor = true;
            this.btnSortDuration.CheckedChanged += new System.EventHandler(this.btnSortDuration_CheckedChanged);
            // 
            // btnSortName
            // 
            this.btnSortName.AutoSize = true;
            this.btnSortName.Checked = true;
            this.btnSortName.Location = new System.Drawing.Point(253, 13);
            this.btnSortName.Name = "btnSortName";
            this.btnSortName.Size = new System.Drawing.Size(88, 17);
            this.btnSortName.TabIndex = 4;
            this.btnSortName.TabStop = true;
            this.btnSortName.Text = "sort by name";
            this.btnSortName.UseVisualStyleBackColor = true;
            this.btnSortName.CheckedChanged += new System.EventHandler(this.btnSortName_CheckedChanged);
            // 
            // frmStreamSelect
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 298);
            this.ControlBox = false;
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
  }
}