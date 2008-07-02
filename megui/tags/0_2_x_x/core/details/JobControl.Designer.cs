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
            this.SuspendLayout();
            // 
            // afterEncoding
            // 
            this.afterEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.afterEncoding.AutoSize = true;
            this.afterEncoding.Location = new System.Drawing.Point(6, 527);
            this.afterEncoding.Name = "afterEncoding";
            this.afterEncoding.Size = new System.Drawing.Size(132, 13);
            this.afterEncoding.TabIndex = 1;
            this.afterEncoding.Text = "After encoding: do nothing";
            // 
            // newWorkerButton
            // 
            this.newWorkerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.newWorkerButton.Location = new System.Drawing.Point(430, 522);
            this.newWorkerButton.Name = "newWorkerButton";
            this.newWorkerButton.Size = new System.Drawing.Size(75, 23);
            this.newWorkerButton.TabIndex = 2;
            this.newWorkerButton.Text = "New worker";
            this.newWorkerButton.UseVisualStyleBackColor = true;
            this.newWorkerButton.Click += new System.EventHandler(this.newWorkerButton_Click);
            // 
            // jobQueue
            // 
            this.jobQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.jobQueue.Location = new System.Drawing.Point(3, 0);
            this.jobQueue.Name = "jobQueue";
            this.jobQueue.PauseResumeMode = MeGUI.core.gui.PauseResumeMode.Disabled;
            this.jobQueue.Size = new System.Drawing.Size(554, 521);
            this.jobQueue.StartStopMode = MeGUI.core.gui.StartStopMode.Start;
            this.jobQueue.TabIndex = 0;
            this.jobQueue.StopClicked += new System.EventHandler(this.jobQueue_StopClicked);
            this.jobQueue.AbortClicked += new System.EventHandler(this.jobQueue_AbortClicked);
            this.jobQueue.StartClicked += new System.EventHandler(this.jobQueue_StartClicked);
            // 
            // helpButton1
            // 
            this.helpButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton1.ArticleName = "Main window#Queue";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(511, 522);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(39, 23);
            this.helpButton1.TabIndex = 3;
            // 
            // JobControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.newWorkerButton);
            this.Controls.Add(this.jobQueue);
            this.Controls.Add(this.helpButton1);
            this.Controls.Add(this.afterEncoding);
            this.Name = "JobControl";
            this.Size = new System.Drawing.Size(553, 550);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label afterEncoding;
        private MeGUI.core.gui.HelpButton helpButton1;
        private MeGUI.core.gui.JobQueue jobQueue;
        private System.Windows.Forms.Button newWorkerButton;
    }
}
