// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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

using System.ComponentModel;

namespace MeGUI
{
    partial class DGAinputWindow
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

        /// <summary>
        ///  prevents the form from closing if we're still processing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGAinputWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.processing)
                e.Cancel = true;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DGAinputWindow));
            this.closeOnQueue = new System.Windows.Forms.CheckBox();
            this.gbOutput = new System.Windows.Forms.GroupBox();
            this.pickOutputButton = new System.Windows.Forms.Button();
            this.projectName = new System.Windows.Forms.TextBox();
            this.projectNameLabel = new System.Windows.Forms.Label();
            this.queueButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.AudioTracks = new System.Windows.Forms.ListBox();
            this.demuxAll = new System.Windows.Forms.RadioButton();
            this.demuxNoAudiotracks = new System.Windows.Forms.RadioButton();
            this.gbInput = new System.Windows.Forms.GroupBox();
            this.input = new MeGUI.FileBar();
            this.inputLabel = new System.Windows.Forms.Label();
            this.saveProjectDialog = new System.Windows.Forms.SaveFileDialog();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.loadOnComplete = new System.Windows.Forms.CheckBox();
            this.gbOutput.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeOnQueue
            // 
            this.closeOnQueue.Checked = true;
            this.closeOnQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.closeOnQueue.Location = new System.Drawing.Point(283, 343);
            this.closeOnQueue.Name = "closeOnQueue";
            this.closeOnQueue.Size = new System.Drawing.Size(72, 24);
            this.closeOnQueue.TabIndex = 20;
            this.closeOnQueue.Text = "and close";
            // 
            // gbOutput
            // 
            this.gbOutput.Controls.Add(this.pickOutputButton);
            this.gbOutput.Controls.Add(this.projectName);
            this.gbOutput.Controls.Add(this.projectNameLabel);
            this.gbOutput.Location = new System.Drawing.Point(8, 287);
            this.gbOutput.Name = "gbOutput";
            this.gbOutput.Size = new System.Drawing.Size(424, 49);
            this.gbOutput.TabIndex = 19;
            this.gbOutput.TabStop = false;
            this.gbOutput.Text = "Output";
            // 
            // pickOutputButton
            // 
            this.pickOutputButton.Location = new System.Drawing.Point(384, 16);
            this.pickOutputButton.Name = "pickOutputButton";
            this.pickOutputButton.Size = new System.Drawing.Size(24, 23);
            this.pickOutputButton.TabIndex = 5;
            this.pickOutputButton.Text = "...";
            this.pickOutputButton.Click += new System.EventHandler(this.pickOutputButton_Click);
            // 
            // projectName
            // 
            this.projectName.Location = new System.Drawing.Point(120, 17);
            this.projectName.Name = "projectName";
            this.projectName.ReadOnly = true;
            this.projectName.Size = new System.Drawing.Size(256, 20);
            this.projectName.TabIndex = 4;
            // 
            // projectNameLabel
            // 
            this.projectNameLabel.Location = new System.Drawing.Point(16, 20);
            this.projectNameLabel.Name = "projectNameLabel";
            this.projectNameLabel.Size = new System.Drawing.Size(100, 13);
            this.projectNameLabel.TabIndex = 3;
            this.projectNameLabel.Text = "dga Project Output";
            // 
            // queueButton
            // 
            this.queueButton.Location = new System.Drawing.Point(363, 343);
            this.queueButton.Name = "queueButton";
            this.queueButton.Size = new System.Drawing.Size(74, 23);
            this.queueButton.TabIndex = 17;
            this.queueButton.Text = "Queue";
            this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.AudioTracks);
            this.groupBox3.Controls.Add(this.demuxAll);
            this.groupBox3.Controls.Add(this.demuxNoAudiotracks);
            this.groupBox3.Location = new System.Drawing.Point(8, 58);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(424, 223);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Audio";
            // 
            // AudioTracks
            // 
            this.AudioTracks.Enabled = false;
            this.AudioTracks.FormattingEnabled = true;
            this.AudioTracks.Location = new System.Drawing.Point(24, 54);
            this.AudioTracks.Name = "AudioTracks";
            this.AudioTracks.Size = new System.Drawing.Size(384, 147);
            this.AudioTracks.TabIndex = 16;
            // 
            // demuxAll
            // 
            this.demuxAll.Location = new System.Drawing.Point(142, 16);
            this.demuxAll.Name = "demuxAll";
            this.demuxAll.Size = new System.Drawing.Size(179, 24);
            this.demuxAll.TabIndex = 15;
            this.demuxAll.TabStop = true;
            this.demuxAll.Text = "Demux All Tracks";
            this.demuxAll.UseVisualStyleBackColor = true;
            // 
            // demuxNoAudiotracks
            // 
            this.demuxNoAudiotracks.Checked = true;
            this.demuxNoAudiotracks.Location = new System.Drawing.Point(16, 16);
            this.demuxNoAudiotracks.Name = "demuxNoAudiotracks";
            this.demuxNoAudiotracks.Size = new System.Drawing.Size(120, 24);
            this.demuxNoAudiotracks.TabIndex = 13;
            this.demuxNoAudiotracks.TabStop = true;
            this.demuxNoAudiotracks.Text = "No Audio demux";
            // 
            // gbInput
            // 
            this.gbInput.Controls.Add(this.input);
            this.gbInput.Controls.Add(this.inputLabel);
            this.gbInput.Location = new System.Drawing.Point(10, 8);
            this.gbInput.Name = "gbInput";
            this.gbInput.Size = new System.Drawing.Size(424, 48);
            this.gbInput.TabIndex = 22;
            this.gbInput.TabStop = false;
            this.gbInput.Text = "Input";
            // 
            // input
            // 
            this.input.Filename = "";
            this.input.Filter = resources.GetString("input.Filter");
            this.input.FilterIndex = 6;
            this.input.FolderMode = false;
            this.input.Location = new System.Drawing.Point(87, 14);
            this.input.Name = "input";
            this.input.ReadOnly = true;
            this.input.SaveMode = false;
            this.input.Size = new System.Drawing.Size(321, 26);
            this.input.TabIndex = 1;
            this.input.Title = null;
            this.input.FileSelected += new MeGUI.FileBarEventHandler(this.input_FileSelected);
            // 
            // inputLabel
            // 
            this.inputLabel.AutoSize = true;
            this.inputLabel.Location = new System.Drawing.Point(20, 20);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(61, 13);
            this.inputLabel.TabIndex = 0;
            this.inputLabel.Text = "Video Input";
            // 
            // saveProjectDialog
            // 
            this.saveProjectDialog.Filter = "DGAVCIndex project files|*.dga";
            this.saveProjectDialog.Title = "Pick a name for your DGAVCIndex project";
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "D2v creator window";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(11, 342);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(39, 23);
            this.helpButton1.TabIndex = 21;
            // 
            // loadOnComplete
            // 
            this.loadOnComplete.Checked = true;
            this.loadOnComplete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadOnComplete.Location = new System.Drawing.Point(68, 343);
            this.loadOnComplete.Name = "loadOnComplete";
            this.loadOnComplete.Size = new System.Drawing.Size(144, 24);
            this.loadOnComplete.TabIndex = 23;
            this.loadOnComplete.Text = "On completion load files";
            // 
            // DGAinputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 377);
            this.Controls.Add(this.loadOnComplete);
            this.Controls.Add(this.gbInput);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.closeOnQueue);
            this.Controls.Add(this.gbOutput);
            this.Controls.Add(this.queueButton);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "DGAinputWindow";
            this.Text = "MeGUI - DGA Project Creator";
            this.gbOutput.ResumeLayout(false);
            this.gbOutput.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.gbInput.ResumeLayout(false);
            this.gbInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MeGUI.core.gui.HelpButton helpButton1;
        private System.Windows.Forms.CheckBox closeOnQueue;
        private System.Windows.Forms.GroupBox gbOutput;
        private System.Windows.Forms.Button pickOutputButton;
        private System.Windows.Forms.TextBox projectName;
        private System.Windows.Forms.Label projectNameLabel;
        private System.Windows.Forms.Button queueButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton demuxAll;
        private System.Windows.Forms.RadioButton demuxNoAudiotracks;
        private System.Windows.Forms.ListBox AudioTracks;
        private System.Windows.Forms.GroupBox gbInput;
        private System.Windows.Forms.Label inputLabel;
        private FileBar input;
        private System.Windows.Forms.SaveFileDialog saveProjectDialog;
        private System.Windows.Forms.CheckBox loadOnComplete;
    }
}