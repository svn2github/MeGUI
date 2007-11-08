namespace MeGUI.packages.tools.calculator
{
    partial class AudioTrackSizeTab
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.audio1Bitrate = new System.Windows.Forms.NumericUpDown();
            this.selectAudio1Button = new System.Windows.Forms.Button();
            this.audio1Type = new System.Windows.Forms.ComboBox();
            this.audio1TypeLabel = new System.Windows.Forms.Label();
            this.clearAudio1Button = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.size = new MeGUI.core.gui.TargetSizeSCBox();
            ((System.ComponentModel.ISupportInitialize)(this.audio1Bitrate)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Bitrate";
            // 
            // audio1Bitrate
            // 
            this.audio1Bitrate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audio1Bitrate.Enabled = false;
            this.audio1Bitrate.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.audio1Bitrate.Location = new System.Drawing.Point(59, 3);
            this.audio1Bitrate.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.audio1Bitrate.Name = "audio1Bitrate";
            this.audio1Bitrate.Size = new System.Drawing.Size(196, 20);
            this.audio1Bitrate.TabIndex = 21;
            this.audio1Bitrate.ValueChanged += new System.EventHandler(this.audio1Bitrate_ValueChanged);
            // 
            // selectAudio1Button
            // 
            this.selectAudio1Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectAudio1Button.Location = new System.Drawing.Point(150, 91);
            this.selectAudio1Button.Name = "selectAudio1Button";
            this.selectAudio1Button.Size = new System.Drawing.Size(75, 23);
            this.selectAudio1Button.TabIndex = 28;
            this.selectAudio1Button.Text = "Select";
            this.selectAudio1Button.Click += new System.EventHandler(this.selectAudio1Button_Click);
            // 
            // audio1Type
            // 
            this.audio1Type.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.audio1Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audio1Type.Location = new System.Drawing.Point(59, 64);
            this.audio1Type.Name = "audio1Type";
            this.audio1Type.Size = new System.Drawing.Size(196, 21);
            this.audio1Type.TabIndex = 27;
            this.audio1Type.SelectedIndexChanged += new System.EventHandler(this.audio1Type_SelectedIndexChanged);
            // 
            // audio1TypeLabel
            // 
            this.audio1TypeLabel.Location = new System.Drawing.Point(3, 67);
            this.audio1TypeLabel.Name = "audio1TypeLabel";
            this.audio1TypeLabel.Size = new System.Drawing.Size(40, 16);
            this.audio1TypeLabel.TabIndex = 26;
            this.audio1TypeLabel.Text = "Type";
            // 
            // clearAudio1Button
            // 
            this.clearAudio1Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearAudio1Button.Location = new System.Drawing.Point(231, 91);
            this.clearAudio1Button.Name = "clearAudio1Button";
            this.clearAudio1Button.Size = new System.Drawing.Size(24, 23);
            this.clearAudio1Button.TabIndex = 29;
            this.clearAudio1Button.Text = "X";
            this.clearAudio1Button.Click += new System.EventHandler(this.clearAudio1Button_Click);
            // 
            // size
            // 
            this.size.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.size.Enabled = false;
            this.size.Location = new System.Drawing.Point(56, 29);
            this.size.MaximumSize = new System.Drawing.Size(1000, 29);
            this.size.MinimumSize = new System.Drawing.Size(64, 29);
            this.size.Name = "size";
            this.size.NullString = "";
            this.size.SelectedIndex = 0;
            this.size.Size = new System.Drawing.Size(202, 29);
            this.size.TabIndex = 30;
            this.size.SelectionChanged += new MeGUI.StringChanged(this.size_SelectionChanged);
            // 
            // AudioTrackSizeTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.size);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.audio1Bitrate);
            this.Controls.Add(this.selectAudio1Button);
            this.Controls.Add(this.audio1Type);
            this.Controls.Add(this.audio1TypeLabel);
            this.Controls.Add(this.clearAudio1Button);
            this.Name = "AudioTrackSizeTab";
            this.Size = new System.Drawing.Size(264, 123);
            ((System.ComponentModel.ISupportInitialize)(this.audio1Bitrate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown audio1Bitrate;
        private System.Windows.Forms.Button selectAudio1Button;
        private System.Windows.Forms.ComboBox audio1Type;
        private System.Windows.Forms.Label audio1TypeLabel;
        private System.Windows.Forms.Button clearAudio1Button;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private MeGUI.core.gui.TargetSizeSCBox size;
    }
}
