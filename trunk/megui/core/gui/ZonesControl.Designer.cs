namespace MeGUI
{
    partial class ZonesControl
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
            this.zonesGroupbox = new System.Windows.Forms.GroupBox();
            this.zoneModifier = new System.Windows.Forms.NumericUpDown();
            this.modifierLabel = new System.Windows.Forms.Label();
            this.zoneLabel = new System.Windows.Forms.Label();
            this.zoneMode = new System.Windows.Forms.ComboBox();
            this.endFrame = new System.Windows.Forms.TextBox();
            this.startFrame = new System.Windows.Forms.TextBox();
            this.zoneListView = new System.Windows.Forms.ListView();
            this.startFrameColumn = new System.Windows.Forms.ColumnHeader();
            this.endFrameColumn = new System.Windows.Forms.ColumnHeader();
            this.modeColumn = new System.Windows.Forms.ColumnHeader();
            this.modifierColumn = new System.Windows.Forms.ColumnHeader();
            this.startFrameLabel = new System.Windows.Forms.Label();
            this.endFrameLabel = new System.Windows.Forms.Label();
            this.addZoneButton = new System.Windows.Forms.Button();
            this.clearZonesButton = new System.Windows.Forms.Button();
            this.updateZoneButton = new System.Windows.Forms.Button();
            this.showVideoButton = new System.Windows.Forms.Button();
            this.removeZoneButton = new System.Windows.Forms.Button();
            this.zonesGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoneModifier)).BeginInit();
            this.SuspendLayout();
            // 
            // zonesGroupbox
            // 
            this.zonesGroupbox.Controls.Add(this.zoneModifier);
            this.zonesGroupbox.Controls.Add(this.modifierLabel);
            this.zonesGroupbox.Controls.Add(this.zoneLabel);
            this.zonesGroupbox.Controls.Add(this.zoneMode);
            this.zonesGroupbox.Controls.Add(this.endFrame);
            this.zonesGroupbox.Controls.Add(this.startFrame);
            this.zonesGroupbox.Controls.Add(this.zoneListView);
            this.zonesGroupbox.Controls.Add(this.startFrameLabel);
            this.zonesGroupbox.Controls.Add(this.endFrameLabel);
            this.zonesGroupbox.Controls.Add(this.addZoneButton);
            this.zonesGroupbox.Controls.Add(this.clearZonesButton);
            this.zonesGroupbox.Controls.Add(this.updateZoneButton);
            this.zonesGroupbox.Controls.Add(this.showVideoButton);
            this.zonesGroupbox.Controls.Add(this.removeZoneButton);
            this.zonesGroupbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zonesGroupbox.Location = new System.Drawing.Point(0, 0);
            this.zonesGroupbox.Name = "zonesGroupbox";
            this.zonesGroupbox.Size = new System.Drawing.Size(317, 288);
            this.zonesGroupbox.TabIndex = 2;
            this.zonesGroupbox.TabStop = false;
            this.zonesGroupbox.Text = "Zones";
            // 
            // zoneModifier
            // 
            this.zoneModifier.Location = new System.Drawing.Point(263, 200);
            this.zoneModifier.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.zoneModifier.Name = "zoneModifier";
            this.zoneModifier.Size = new System.Drawing.Size(48, 20);
            this.zoneModifier.TabIndex = 3;
            this.zoneModifier.Value = new decimal(new int[] {
            26,
            0,
            0,
            0});
            // 
            // modifierLabel
            // 
            this.modifierLabel.Location = new System.Drawing.Point(160, 202);
            this.modifierLabel.Name = "modifierLabel";
            this.modifierLabel.Size = new System.Drawing.Size(56, 16);
            this.modifierLabel.TabIndex = 6;
            this.modifierLabel.Text = "Quantizer";
            // 
            // zoneLabel
            // 
            this.zoneLabel.Location = new System.Drawing.Point(160, 230);
            this.zoneLabel.Name = "zoneLabel";
            this.zoneLabel.Size = new System.Drawing.Size(32, 13);
            this.zoneLabel.TabIndex = 5;
            this.zoneLabel.Text = "Mode";
            // 
            // zoneMode
            // 
            this.zoneMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zoneMode.Items.AddRange(new object[] {
            "Quantizer",
            "Weight"});
            this.zoneMode.Location = new System.Drawing.Point(231, 226);
            this.zoneMode.Name = "zoneMode";
            this.zoneMode.Size = new System.Drawing.Size(80, 21);
            this.zoneMode.TabIndex = 4;
            this.zoneMode.SelectedIndexChanged += new System.EventHandler(this.zoneMode_SelectedIndexChanged);
            // 
            // endFrame
            // 
            this.endFrame.Location = new System.Drawing.Point(88, 226);
            this.endFrame.Name = "endFrame";
            this.endFrame.Size = new System.Drawing.Size(48, 20);
            this.endFrame.TabIndex = 2;
            // 
            // startFrame
            // 
            this.startFrame.Location = new System.Drawing.Point(88, 200);
            this.startFrame.Name = "startFrame";
            this.startFrame.Size = new System.Drawing.Size(48, 20);
            this.startFrame.TabIndex = 1;
            // 
            // zoneListView
            // 
            this.zoneListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.startFrameColumn,
            this.endFrameColumn,
            this.modeColumn,
            this.modifierColumn});
            this.zoneListView.FullRowSelect = true;
            this.zoneListView.HideSelection = false;
            this.zoneListView.Location = new System.Drawing.Point(8, 24);
            this.zoneListView.Name = "zoneListView";
            this.zoneListView.Size = new System.Drawing.Size(303, 168);
            this.zoneListView.TabIndex = 0;
            this.zoneListView.UseCompatibleStateImageBehavior = false;
            this.zoneListView.View = System.Windows.Forms.View.Details;
            // 
            // startFrameColumn
            // 
            this.startFrameColumn.Text = "Start";
            // 
            // endFrameColumn
            // 
            this.endFrameColumn.Text = "End";
            // 
            // modeColumn
            // 
            this.modeColumn.Text = "Mode";
            this.modeColumn.Width = 80;
            // 
            // modifierColumn
            // 
            this.modifierColumn.Text = "Modifier";
            // 
            // startFrameLabel
            // 
            this.startFrameLabel.Location = new System.Drawing.Point(8, 202);
            this.startFrameLabel.Name = "startFrameLabel";
            this.startFrameLabel.Size = new System.Drawing.Size(64, 16);
            this.startFrameLabel.TabIndex = 1;
            this.startFrameLabel.Text = "Start Frame";
            // 
            // endFrameLabel
            // 
            this.endFrameLabel.Location = new System.Drawing.Point(8, 228);
            this.endFrameLabel.Name = "endFrameLabel";
            this.endFrameLabel.Size = new System.Drawing.Size(64, 16);
            this.endFrameLabel.TabIndex = 2;
            this.endFrameLabel.Text = "End Frame";
            // 
            // addZoneButton
            // 
            this.addZoneButton.Location = new System.Drawing.Point(260, 256);
            this.addZoneButton.Name = "addZoneButton";
            this.addZoneButton.Size = new System.Drawing.Size(52, 23);
            this.addZoneButton.TabIndex = 7;
            this.addZoneButton.Text = "Add";
            this.addZoneButton.Click += new System.EventHandler(this.addZoneButton_Click);
            // 
            // clearZonesButton
            // 
            this.clearZonesButton.Location = new System.Drawing.Point(76, 256);
            this.clearZonesButton.Name = "clearZonesButton";
            this.clearZonesButton.Size = new System.Drawing.Size(52, 23);
            this.clearZonesButton.TabIndex = 5;
            this.clearZonesButton.Text = "Clear";
            this.clearZonesButton.Click += new System.EventHandler(this.clearZonesButton_Click);
            // 
            // updateZoneButton
            // 
            this.updateZoneButton.Location = new System.Drawing.Point(134, 256);
            this.updateZoneButton.Name = "updateZoneButton";
            this.updateZoneButton.Size = new System.Drawing.Size(52, 23);
            this.updateZoneButton.TabIndex = 9;
            this.updateZoneButton.Text = "Update";
            this.updateZoneButton.Click += new System.EventHandler(this.updateZoneButton_Click);
            // 
            // showVideoButton
            // 
            this.showVideoButton.Enabled = false;
            this.showVideoButton.Location = new System.Drawing.Point(9, 256);
            this.showVideoButton.Name = "showVideoButton";
            this.showVideoButton.Size = new System.Drawing.Size(61, 23);
            this.showVideoButton.TabIndex = 9;
            this.showVideoButton.Text = "Preview";
            this.showVideoButton.Click += new System.EventHandler(this.showVideoButton_Click);
            // 
            // removeZoneButton
            // 
            this.removeZoneButton.Location = new System.Drawing.Point(192, 256);
            this.removeZoneButton.Name = "removeZoneButton";
            this.removeZoneButton.Size = new System.Drawing.Size(62, 23);
            this.removeZoneButton.TabIndex = 6;
            this.removeZoneButton.Text = "Remove";
            this.removeZoneButton.Click += new System.EventHandler(this.removeZoneButton_Click);
            // 
            // ZonesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zonesGroupbox);
            this.Name = "ZonesControl";
            this.Size = new System.Drawing.Size(317, 288);
            this.Load += new System.EventHandler(this.ZonesControl_Load);
            this.zonesGroupbox.ResumeLayout(false);
            this.zonesGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoneModifier)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox zonesGroupbox;
        private System.Windows.Forms.NumericUpDown zoneModifier;
        private System.Windows.Forms.Label modifierLabel;
        private System.Windows.Forms.Label zoneLabel;
        private System.Windows.Forms.ComboBox zoneMode;
        private System.Windows.Forms.TextBox endFrame;
        private System.Windows.Forms.TextBox startFrame;
        private System.Windows.Forms.ListView zoneListView;
        private System.Windows.Forms.ColumnHeader startFrameColumn;
        private System.Windows.Forms.ColumnHeader endFrameColumn;
        private System.Windows.Forms.ColumnHeader modeColumn;
        private System.Windows.Forms.ColumnHeader modifierColumn;
        private System.Windows.Forms.Label startFrameLabel;
        private System.Windows.Forms.Label endFrameLabel;
        private System.Windows.Forms.Button addZoneButton;
        private System.Windows.Forms.Button clearZonesButton;
        private System.Windows.Forms.Button updateZoneButton;
        private System.Windows.Forms.Button showVideoButton;
        private System.Windows.Forms.Button removeZoneButton;
    }
}
