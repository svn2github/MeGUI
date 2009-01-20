namespace MeGUI.core.details
{
    partial class JobControl
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
            this.afterEncoding = new System.Windows.Forms.Label();
            this.newWorkerButton = new System.Windows.Forms.Button();
            this.jobQueue = new MeGUI.core.gui.JobQueue();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // afterEncoding
            // 
            this.afterEncoding.AutoSize = true;
            this.afterEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.afterEncoding.Location = new System.Drawing.Point(3, 0);
            this.afterEncoding.Name = "afterEncoding";
            this.afterEncoding.Size = new System.Drawing.Size(447, 29);
            this.afterEncoding.TabIndex = 1;
            this.afterEncoding.Text = "After encoding: do nothing";
            this.afterEncoding.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // newWorkerButton
            // 
            this.newWorkerButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.newWorkerButton.AutoSize = true;
            this.newWorkerButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.newWorkerButton.Location = new System.Drawing.Point(456, 3);
            this.newWorkerButton.Name = "newWorkerButton";
            this.newWorkerButton.Size = new System.Drawing.Size(74, 23);
            this.newWorkerButton.TabIndex = 2;
            this.newWorkerButton.Text = "New worker";
            this.newWorkerButton.UseVisualStyleBackColor = true;
            this.newWorkerButton.Click += new System.EventHandler(this.newWorkerButton_Click);
            // 
            // jobQueue
            // 
            this.jobQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jobQueue.Location = new System.Drawing.Point(0, 0);
            this.jobQueue.Name = "jobQueue";
            this.jobQueue.PauseResumeMode = MeGUI.core.gui.PauseResumeMode.Disabled;
            this.jobQueue.SaveSettings = true;
            this.jobQueue.SettingsKey = "JobQueue";
            this.jobQueue.Size = new System.Drawing.Size(553, 521);
            this.jobQueue.StartStopMode = MeGUI.core.gui.StartStopMode.Start;
            this.jobQueue.TabIndex = 0;
            this.jobQueue.StartClicked += new System.EventHandler(this.jobQueue_StartClicked);
            this.jobQueue.AbortClicked += new System.EventHandler(this.jobQueue_AbortClicked);
            this.jobQueue.StopClicked += new System.EventHandler(this.jobQueue_StopClicked);
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.helpButton1.ArticleName = "Main window#Queue";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(536, 8);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(14, 12);
            this.helpButton1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.afterEncoding, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.helpButton1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.newWorkerButton, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 521);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(553, 29);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // JobControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.jobQueue);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "JobControl";
            this.Size = new System.Drawing.Size(553, 550);
            ((System.Configuration.IPersistComponentSettings)(this.jobQueue)).LoadComponentSettings();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label afterEncoding;
        private MeGUI.core.gui.HelpButton helpButton1;
        private MeGUI.core.gui.JobQueue jobQueue;
        private System.Windows.Forms.Button newWorkerButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
