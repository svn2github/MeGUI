using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace MeGUI.core.details
{
    public partial class JobControl : UserControl
    {
        private Bitmap pauseImage;
        private Bitmap playImage;

        private bool isEncoding = false, paused = false; // encoding status and whether or not we're in queue encoding mode
        private bool stopTheQueue = false;
        private Dictionary<string, Job> jobs = new Dictionary<string,Job>(); //storage for all the jobs and profiles known to the system
        private IJobProcessor currentProcessor;
        private MainForm mainForm;
        private ProgressWindow pw; // window that shows the encoding progress
        private Job currentJob; // The job being processed at the moment

        private List<Job> jobsToEncode = null; // List of jobs to be encoded if only a certain number are to be encoded (different from normal queue behaviour)

        #region process window opening and closing
        public void HideProcessWindow()
        {
            if (pw != null)
                pw.Hide();
        }

        public void ShowProcessWindow()
        {
            if (pw != null)
                pw.Show();
        }

        public bool ProcessWindowAccessible
        {
            get { return (pw != null); }
        }
        /// <summary>
        /// callback for the Progress Window
        /// This is called when the progress window has been closed and ensures that
        /// no futher attempt is made to send a statusupdate to the progress window
        /// </summary>
        private void pw_WindowClosed(bool hideOnly)
        {
            mainForm.ProcessStatusChecked = false;
            if (!hideOnly)
                pw = null;
        }
        /// <summary>
        /// callback for the progress window
        /// this method is called if the abort button in the progress window is called
        /// it stops the encoder cold
        /// </summary>
        private void pw_Abort()
        {
            Abort();
        }
        /// <summary>
        /// catches the ChangePriority event from the progresswindow and forward it to the encoder class
        /// </summary>
        /// <param name="priority"></param>
        private void pw_PriorityChanged(ProcessPriority priority)
        {
            string error;
            if (!currentProcessor.changePriority(priority, out error))
            {
                mainForm.addToLog("Error when attempting to change priority: " + error);
            }
        }
        #endregion

        public JobControl()
        {
            InitializeComponent();

            System.Reflection.Assembly myAssembly = this.GetType().Assembly;
            string name = "MeGUI.";
#if CSC
			name = "";
#endif
            string[] resources = myAssembly.GetManifestResourceNames();
            try
            {
                this.pauseImage = new Bitmap(myAssembly.GetManifestResourceStream(name + "pause.ico"));
                this.playImage = new Bitmap(myAssembly.GetManifestResourceStream(name + "play.ico"));
                this.pauseButton.Image = (Image)pauseImage;
                this.pauseButton.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            }
            catch (Exception) { }
        }

        #region properties
        public MainForm MainForm
        {
            set { mainForm = value; }
        }

        public bool IsEncoding
        {
            get { return isEncoding; }
        }
        #endregion

        /// <summary>
        /// Sets the job status of the selected jobs to postponed.
        /// No selected jobs should currently be running.
        /// </summary>
        /// <param name="sender">This parameter is ignored</param>
        /// <param name="e">This parameter is ignored</param>
        private void postponeMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                int position = item.Index;
                Job job = jobs[item.Text];
                
                Debug.Assert(job.Status != JobStatus.PROCESSING, "shouldn't be able to postpone an active job");

                job.Status = JobStatus.POSTPONED;
                this.queueListView.Items[position].SubItems[5].Text = job.StatusString;
            }
        }

        /// <summary>
        /// Sets the jobs status of the selected jobs to waiting.
        /// No selected jobs should currently be running.
        /// </summary>
        /// <param name="sender">This parameter is ignored</param>
        /// <param name="e">This parameter is ignored</param>
        private void waitingMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                int position = item.Index;
                Job job = jobs[item.Text];

                Debug.Assert(job.Status != JobStatus.PROCESSING, "shouldn't be able to set an active job back to waiting");

                job.Status = JobStatus.WAITING;
                queueListView.Items[position].SubItems[5].Text = job.StatusString;
            }
        }        
        
        /// <summary>
        /// event callback for the encoder
        /// Each time the encoder has a statusupdate, it fires an event that is catched here
        /// The actual GUI update is triggered via an Invoke on the current form, which ensures
        /// that the update is made in the GUI thread rather than the encoder thread, which could cause
        /// access problems
        /// </summary>
        /// <param name="su">StatusUpdate object that contains the current encoding statuts</param>
        private void enc_StatusUpdate(StatusUpdate su)
        {
            this.Invoke(new UpdateGUIStatusCallback(this.UpdateGUIStatus), new object[] { su });
        }

#if UNUSED
        /// <summary>
        /// Returns a the job by the given name or, if it doesn't exist, null.
        /// </summary>
        /// <param name="name">The name of the job to look for.</param>
        /// <returns>The job by that name, or else null.</returns>
        public Job GetJobByNameSafe(string name)
        {
            try
            {
                return jobs[name];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
#endif

        /// <summary>
        /// updates the actual GUI with the status information received as parameter
        /// If the StatusUpdate indicates that the job has ended, the Progress window is closed
        /// and the logging messages from the StatusUpdate object are added to the log tab
        /// if the job mentioned in the statusupdate has a next job name defined, the job is looked
        /// up and processing of that job starts - this applies even in queue encoding mode
        /// the linked jobs will always be encoded first, regardless of their position in the queue
        /// If we're in queue encoding mode, the next nob in the queue is also started
        /// </summary>
        /// <param name="su">StatusUpdate object containing the current encoder stats</param>
        private void UpdateGUIStatus(StatusUpdate su)
        {
            if (su.IsComplete)
            {
                Job job = currentJob;
                copyInfoIntoJob(job, su);
                updateListViewInfo(job);

                // Update the GUI to show that the job is completed
                mainForm.TitleText = Application.ProductName + " " + Application.ProductVersion;
                currentProcessor = null;
                this.jobProgress.Value = 0;
                ensureProgressWindowClosed();
                
                // Logging
                mainForm.addToLog("Processing ended at " + DateTime.Now.ToLongTimeString() + "\r\n");
                mainForm.addToLog("----------------------------------------------------------------------------------------------------------" +
                    "\r\n\r\nLog for job " + su.JobName + "\r\n\r\n" + su.Log +
                    "\r\n----------------------------------------------------------------------------------------------------------\r\n");
                if (su.WasAborted)
                    mainForm.addToLog("The current job was aborted. Stopping queue mode\r\n");
                if (su.HasError)
                {
                    mainForm.addToLog("The current job contains errors. Skipping chained jobs\r\n");
                    skipChainedJobs(currentJob);
                }

                // Postprocessing
                bool jobCompletedSuccessfully = (job.Status == JobStatus.DONE);
                if (jobCompletedSuccessfully)
                {
                    postprocessJob(job);
                }

                if (job.Next == null && jobCompletedSuccessfully && mainForm.Settings.DeleteCompletedJobs)
                    removeCompletedJob(job);
                
                // Clear the encoding info
                this.isEncoding = false;
                this.currentJob = null;
                this.currentProcessor = null;

                if (job.Status != JobStatus.ABORTED)
                    startNextJobInQueue();

                if (!isEncoding && job.Status != JobStatus.ABORTED) mainForm.runAfterEncodingCommands();
                updateProcessingStatus();
            }
            else // job is not complete yet
            {
                try
                {
                    if (pw.IsHandleCreated) // the window is there, send the update to the window
                    {
                        pw.Invoke(new UpdateStatusCallback(pw.UpdateStatus), new object[] { su });
                    }
                }
                catch (Exception e)
                {
                    mainForm.addToLog("Exception when trying to update status while a job is running. Text: " + e.Message + " stacktrace: " + e.StackTrace);
                }
                string percentage = su.PercentageDoneExact.ToString("##.##");
                if (percentage.IndexOf(".") != -1 && percentage.Substring(percentage.IndexOf(".")).Length == 1)
                    percentage += "0";
                mainForm.TitleText = "MeGUI " + su.JobName + " " + percentage + "% ";
                if (mainForm.Settings.AfterEncoding == AfterEncoding.Shutdown)
                    mainForm.TitleText += "- SHUTDOWN after encode";
                this.jobProgress.Value = su.PercentageDone;
            }
        }

        /// <summary>
        /// Adds all chained jobs to the skip jobs list, but doesn't add the passed job.
        /// </summary>
        /// <param name="job"></param>
        private void skipChainedJobs(Job job)
        {
            while (job.Next != null)
            {
                job = job.Next;
                job.Status = JobStatus.SKIP;
                updateListViewInfo(job);
            }
        }

        /// <summary>
        /// Updates all columns of the ListViewItem that shows this job.
        /// </summary>
        /// <param name="job"></param>
        private void updateListViewInfo(Job job)
        {
            if (queueListView.InvokeRequired)
            {
                queueListView.Invoke(new MethodInvoker(delegate { updateListViewInfo(job); }));
                return;
            }
            ListViewItem item = queueListView.Items[job.Position];
            Debug.Assert(item.Text == job.Name);

            item.SubItems[1].Text = job.InputFileName;
            item.SubItems[2].Text = job.OutputFileName;
            item.SubItems[3].Text = job.CodecString;
            item.SubItems[4].Text = job.EncodingMode;
            item.SubItems[5].Text = job.StatusString;
            if (job.Status == JobStatus.DONE)
            {
                item.SubItems[7].Text = job.End.ToLongTimeString();
                item.SubItems[8].Text = job.FPSString;
            }
            else
            {
                item.SubItems[7].Text = "";
                item.SubItems[8].Text = "";
            }
            if (job.Status == JobStatus.DONE || job.Status == JobStatus.PROCESSING)
                item.SubItems[6].Text = job.Start.ToLongTimeString();
            else
                item.SubItems[6].Text = "";
        }

        /// <summary>
        /// Copies completion info into the job: end time, FPS, status.
        /// </summary>
        /// <param name="job">Job to fill with info</param>
        /// <param name="su">StatusUpdate with info</param>
        private void copyInfoIntoJob(Job job, StatusUpdate su)
        {
            Debug.Assert(su.IsComplete);

            job.End = DateTime.Now;
            job.FPS = su.FPS;
            
            JobStatus s;
            if (su.WasAborted)
                s = JobStatus.ABORTED;
            else if (su.HasError)
                s = JobStatus.ERROR;
            else
                s = JobStatus.DONE;
            job.Status = s;
        }

        /// <summary>
        /// Makes sure that the progress window is closed
        /// </summary>
        private void ensureProgressWindowClosed()
        {
            if (pw != null)
            {
                pw.IsUserAbort = false; // ensures that the window will be closed
                pw.Close();
                pw = null;
            }
        }

        /// <summary>
        /// Postprocesses the given job according to the JobPostProcessors in the mainForm's PackageSystem
        /// </summary>
        /// <param name="job"></param>
        private void postprocessJob(Job job)
        {
            mainForm.addToLog("Starting postprocessing of job...\r\n");
            foreach (JobPostProcessor pp in mainForm.PackageSystem.JobPostProcessors.Values)
            {
                pp.PostProcessor(mainForm, job);
            }
            mainForm.addToLog("Postprocessing finished!\r\n");
        }

        /// <summary>
        /// Preprocesses the given job according to the JobPreProcessors in the mainForm's PackageSystem
        /// </summary>
        /// <param name="job"></param>
        private void preprocessJob(Job job)
        {
            mainForm.addToLog("Starting preprocessing of job...\r\n");
            foreach (JobPreProcessor pp in mainForm.PackageSystem.JobPreProcessors.Values)
            {
                pp.PreProcessor(mainForm, job);
            }
            mainForm.addToLog("Preprocessing finished!\r\n");
        }

        private IJobProcessor getProcessor(Job job)
        {
            mainForm.addToLog("Looking for job processor for job...\r\n");
            foreach (JobProcessorFactory f in mainForm.PackageSystem.JobProcessors.Values)
            {
                IJobProcessor p = f.Factory(mainForm, job);
                if (p != null)
                {
                    mainForm.addToLog("Processor found!\r\n");
                    return p;
                }
            }
            mainForm.addToLog("No processor found!\r\n");
            return null;
        }

        

        #region queue buttons
        /// <summary>
        /// handles the start/stop button in the queue tab
        /// if we're already encoding in queue mode, the button acts as a stop button
        /// if we're encoding but are not in queue mode, the queue mode will be started
        /// if we're not encoding, and there is at least one job in the queue, encoding is started
        /// if we're not encoding and there's no job in the queue, a warning is displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void startStopButton_Click(object sender, System.EventArgs e)
        {
            if (this.isEncoding) // we're already encoding
                this.stopTheQueue = !this.stopTheQueue;
            else // we're not encoding yet
                StartEncoding(true);
            updateProcessingStatus();
        }

        /// <summary>
        /// handles the abort button in the queue tab
        /// if we're currently encoding), the encoder is stopped and the GUI reset to non encoding mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void abortButton_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to abort encoding?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
                Abort();
        }
        
        /// <summary>
        /// deletes all the jobs from the jobs queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteAllJobsButton_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to clear the queue?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
                foreach (ListViewItem item in this.queueListView.Items)
                {
                    Job job = jobs[item.Text];
                    if (job.Status != JobStatus.PROCESSING)
                        this.removeJobFromQueue(job);
                    else
                        MessageBox.Show("You cannot delete a job while it is being encoded.", "Deleting job failed", MessageBoxButtons.OK);
                    updateJobPositions();
                }
        }

        /// <summary>
        /// handles the pause button
        /// enables / disables the main encoding thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void pauseButton_Click(object sender, System.EventArgs e)
        {
            if (!this.paused) // we're encoding
            {
                string error;
                if (currentProcessor != null)
                {
                    if (currentProcessor.pause(out error))
                    {
                        paused = true;
                    }
                    else
                        mainForm.addToLog("Error when trying to pause encoding: " + error);
                }
            }
            else
            {
                string error;
                if (currentProcessor != null)
                {
                    if (currentProcessor.resume(out error))
                    {
                        paused = false;
                    }
                    else
                        mainForm.addToLog("Error when trying to resume encoding: " + error);
                }
            }
            updateProcessingStatus();
        }
        #endregion
        #region misc action
        /// <summary>
        /// moves the job one position up in the queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upButton_Click(object sender, System.EventArgs e)
        {
            if (queueListView.SelectedItems.Count > 0)
            {
                MoveListViewItem(ref this.queueListView, true);
                updateJobPositions();
            }
        }

        /// <summary>
        /// moves a job one position down in the queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downButton_Click(object sender, System.EventArgs e)
        {
            if (queueListView.SelectedItems.Count > 0)
            {
                MoveListViewItem(ref this.queueListView, false);
                updateJobPositions();
            }
        }
        
        /// <summary>
        /// deletes a job from the job queue
        /// if a line in the job queue is selected, and its status is not processing (status processing means we're currently
        /// encoding), the job is removed from the listview and the hashtable containing the jobs
        /// in addition, of the job exists as a file in the jobs directory, it is being deleted as well
        /// deleting of a job that is currently being encoded is not supported and shows an error message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteJobButton_Click(object sender, System.EventArgs e)
        {
            if (queueListView.Items.Count <= 0) return;

            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                if (!jobs.ContainsKey(item.Text)) // Check if it has already been deleted
                    continue;
                Job job = jobs[item.Text];
                if (job == null) continue;

                if (job.Status == JobStatus.PROCESSING)
                {
                    MessageBox.Show("You cannot delete a job while it is being processed.", "Deleting job failed", MessageBoxButtons.OK);
                    continue;
                }

                if (job.Next == null && job.Previous == null)
                {
                    removeJobFromQueue(job);
                    continue;
                }

                DialogResult dr = MessageBox.Show("This job is part of a series of jobs. Deleting it alone will cause corruption of the dependant jobs\r\n"
                    + "Press Yes to delete all dependant jobs, No to delete just this job or Cancel to abort.", "Job dependency detected", MessageBoxButtons.YesNoCancel);
                switch (dr)
                {
                    case DialogResult.Yes: // Delete all dependent jobs
                        this.removeJobFromQueue(job);
                        Job j = job.Previous;
                        while (j != null)
                        {
                            removeJobFromQueue(j);
                            j = j.Previous;
                        }
                        j = job.Next;
                        while (j != null)
                        {
                            removeJobFromQueue(j);
                            j = j.Next;
                        }
                        break;

                    case DialogResult.No: // Just delete the single job
                        removeJobFromQueue(job);
                        break;

                    case DialogResult.Cancel: // do nothing
                        break;
                }

            }
            updateJobPositions();
        }

        /// <summary>
        /// goes through all jobs in the listview and updates their position
        /// this is called when moving or deleting jobs
        /// </summary>
        private void updateJobPositions()
        {
            foreach (ListViewItem item in this.queueListView.Items) // go through all remaining jobs and update their position
            {
                Job job = jobs[item.Text];
                job.Position = item.Index;
            }
        }
#endregion
        #region load and update jobs
        /// <summary>
        /// loads a job listed in the job queue
        /// if a line is selected in the queue, its associated job is extracted from the jobs hashtable, then its settings
        /// are being loaded. In addition, the commandline stored in the job is displayed, overriding the automatically
        /// generated commandline
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadJobButton_Click(object sender, System.EventArgs e)
        {
            if (this.queueListView.SelectedItems.Count > 0) // otherwise we have something bogus on our hands
            {
                int position = this.queueListView.SelectedItems[0].Index;
                Job job = jobs[this.queueListView.Items[position].Text];
#warning implement polymorphic job loading here
                /*                if (job is VideoJob)
                {
                    VideoJob vjob = (VideoJob)job;
                    this.videoInput.Text = vjob.Input;
                    parX = vjob.DARX;
                    parY = vjob.DARY;
                    CurrentVideoCodecSettings = vjob.Settings;
                    this.videoOutput.Text = vjob.Output;
                    this.tabControl1.SelectedIndex = 0;
                    this.videoProfile.SelectedIndex = -1;
                }
                if (job is AudioJob)
                {
                    AudioJob ajob = (AudioJob)job;
                    foreach (IAudioSettingsProvider p in audioCodec.Items)
                    {
                        if (p.IsSameType(ajob.Settings))
                        {
                            p.LoadSettings(ajob.Settings);
                            audioCodec.SelectedItem = p;
                            break;
                        }
                    }
                    this.audioInput.Text = job.Input;
                    this.audioOutput.Text = job.Output;
                    this.tabControl1.SelectedIndex = 0;
                    this.audioProfile.SelectedIndex = -1;
                }
                if (job is MuxJob)
                {
                    MuxJob mjob = (MuxJob)job;
                    MuxWindow mw = new MuxWindow(muxProvider.GetMuxer(mjob.MuxType));
                    mw.Job = mjob;
                    if (mw.ShowDialog() == DialogResult.OK)
                    {
                        MuxJob newJob = mw.Job;
                        transferJobSettings(mjob, newJob);
                        jobs.Remove(job.Name);
                        jobs.Add(job.Name, newJob);
                    }
                }
                else if (job is IndexJob)
                {
                    IndexJob ijob = (IndexJob)job;
                    if (ijob.PostprocessingProperties == null)
                    {
                        using (VobinputWindow viw = new VobinputWindow(this))
                        {
                        viw.setConfig(ijob.Input, ijob.Output, ijob.DemuxMode, ijob.AudioTrackID1, ijob.AudioTrackID2,
                            false, true, ijob.LoadSources, true);
                        if (viw.ShowDialog() == DialogResult.OK)
                        {
                            IndexJob newJob = viw.Job;
                            transferJobSettings(ijob, newJob);
                            jobs.Remove(job.Name);
                            jobs.Add(job.Name, newJob);
                        }
                        }
                    }
                    else
                        MessageBox.Show("Loading of OneClick index jobs not supported", "Load not possible", MessageBoxButtons.OK);
                }
                else if (job is SubtitleIndexJob)
                {
                    SubtitleIndexJob sij = (SubtitleIndexJob)job;
                    using (VobSubIndexWindow vsiw = new VobSubIndexWindow(this))
                    {
                        vsiw.setConfig(sij.Input, sij.Output, sij.IndexAllTracks, sij.TrackIDs, sij.PGC);
                        if (vsiw.ShowDialog() == DialogResult.OK)
                        {
                            SubtitleIndexJob newJob = vsiw.Job;
                            transferJobSettings(sij, newJob);
                            jobs.Remove(job.Name);
                            jobs.Add(job.Name, newJob);
                        }
                    }
                }        
*/
            }
            else
                MessageBox.Show("You need to select a job first.", "No job selected", MessageBoxButtons.OK);
        }

        /// <summary>
        /// updates a selected job in the queue with what's currently configured in the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateJobButton_Click(object sender, System.EventArgs e)
        {
            if (this.queueListView.SelectedItems.Count > 0) // otherwise we have something bogus on our hands
            {
                int position = this.queueListView.SelectedItems[0].Index;
                ListViewItem item = this.queueListView.SelectedItems[0];
                Job job = jobs[this.queueListView.Items[position].Text];
#warning implement polymorphic job loading/updating
                /*if (job is VideoJob)
                {
                    VideoJob vjob = (VideoJob)job;
                    if (vjob.Settings.GetType() == this.CurrentVideoCodecSettings.GetType())
                    {
                        if (this.CurrentVideoCodecSettings.EncodingMode != 4 && this.CurrentVideoCodecSettings.EncodingMode != 8)
                        {
                            vjob.Input = VideoIO[0];
                            vjob.Output = VideoIO[1];
                            vjob.Settings = this.CurrentVideoCodecSettings.clone();
                            item.SubItems[1].Text = vjob.InputFileName;
                            item.SubItems[2].Text = vjob.OutputFileName;
                            item.SubItems[4].Text = vjob.EncodingMode;
                        }
                        else
                        {
                            MessageBox.Show("You cannot turn a single job into a series of jobs", "Unsupported update", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else
                        MessageBox.Show("You cannot change the codec of an existing job.\nIf you want to change codecs, delete the job and create a new one",
                            "Unsupported update", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else if (job is AudioJob)
                {
                    AudioJob ajob = (AudioJob)job;
                    if (ajob.Settings.GetType() == this.CurrentAudioSettingsProvider.GetCurrentSettings().GetType())
                    {
                        ajob.Input = this.audioInput.Text;
                        ajob.Output = this.audioOutput.Text;
                        ajob.Settings = this.CurrentAudioSettingsProvider.GetCurrentSettings();
                        item.SubItems[1].Text = ajob.InputFileName;
                        item.SubItems[2].Text = ajob.OutputFileName;
                        item.SubItems[4].Text = ajob.EncodingMode;
                    }
                    else
                        MessageBox.Show("You cannot change the change the codec of an existing job.\nIf you want to change codecs, delete the job and create a new one",
                            "Unsupported update", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
                else if (job is MuxJob)
                {
                    MessageBox.Show("To update a mux job, simply select it and press config.\nThen make changes in the window that pops up and press\nthe Update button to update the job.",
                        "Not supported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (job is IndexJob)
                {
                    MessageBox.Show("You cannot update an indexing job", "Not supported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }*/
            }
        }
        #endregion

        #region GUI action
        /// <summary>
        /// moves the currently selected listviewitem up/down
        /// code by Less Smith @ KnotDot.Net
        /// </summary>
        /// <param name="lv">reference to ListView</param>
        /// <param name="moveUp">whether the currently selected item should be moved up or down</param>
        private void MoveListViewItem(ref ListView lv, bool moveUp)
        {
            string cache;
            int selIdx;

            selIdx = lv.SelectedItems[0].Index;
            lv.Items[selIdx].Selected = false;
            if (moveUp)
            {
                // ignore moveup of row(0)
                if (selIdx == 0)
                    return;

                // move the subitems for the previous row
                // to cache to make room for the selected row
                for (int i = 0; i < lv.Items[selIdx].SubItems.Count; i++)
                {
                    cache = lv.Items[selIdx - 1].SubItems[i].Text;
                    lv.Items[selIdx - 1].SubItems[i].Text =
                        lv.Items[selIdx].SubItems[i].Text;
                    lv.Items[selIdx].SubItems[i].Text = cache;
                }
                lv.Items[selIdx - 1].Selected = true;
                lv.Refresh();
            }
            else
            {
                // ignore movedown of last item
                if (selIdx == lv.Items.Count - 1)
                    return;
                // move the subitems for the next row
                // to cache so we can move the selected row down
                for (int i = 0; i < lv.Items[selIdx].SubItems.Count; i++)
                {
                    cache = lv.Items[selIdx + 1].SubItems[i].Text;
                    lv.Items[selIdx + 1].SubItems[i].Text =
                        lv.Items[selIdx].SubItems[i].Text;
                    lv.Items[selIdx].SubItems[i].Text = cache;
                }
                lv.Items[selIdx + 1].Selected = true;
                lv.Refresh();
            }
        }
        
        /// <summary>
        /// handles the doubleclick event for the listview
        /// changes the job status from waiting -> postponed to waiting
        /// from done -> waiting -> postponed -> waiting
        /// from error -> waiting -> postponed -> waiting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queueListView_DoubleClick(object sender, EventArgs e)
        {
            if (this.queueListView.SelectedItems.Count > 0) // otherwise 
            {
                int position = this.queueListView.SelectedItems[0].Index;
                Job job = jobs[this.queueListView.SelectedItems[0].Text];
                if (job.Status == JobStatus.WAITING) // waiting -> postponed
                    job.Status = JobStatus.POSTPONED;
                else
                    job.Status = JobStatus.WAITING;
                this.queueListView.Items[position].SubItems[5].Text = job.StatusString;
            }
        }

        private void queueContextMenu_Opened(object sender, EventArgs e)
        {
            AbortMenuItem.Enabled = AllJobsHaveStatus(JobStatus.PROCESSING);
            AbortMenuItem.Checked = AllJobsHaveStatus(JobStatus.ABORTED);

            LoadMenuItem.Enabled = this.queueListView.SelectedItems.Count == 1;
            LoadMenuItem.Checked = false;

            bool canModifySelectedJobs = !AnyJobsHaveStatus(JobStatus.PROCESSING) && this.queueListView.SelectedItems.Count > 0;
            DeleteMenuItem.Enabled = PostponedMenuItem.Enabled = WaitingMenuItem.Enabled = canModifySelectedJobs;

            DeleteMenuItem.Checked = false;
            PostponedMenuItem.Checked = AllJobsHaveStatus(JobStatus.POSTPONED);
            WaitingMenuItem.Checked = AllJobsHaveStatus(JobStatus.WAITING);

            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                item.SubItems[5].Text = (jobs[item.Text]).StatusString;
            }
        }

        /// <summary>
        /// Returns true if all selected jobs have the requested status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool AllJobsHaveStatus(JobStatus status)
        {
            if (this.queueListView.SelectedItems.Count <= 0)
            {
                return false;
            }
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                Job job = jobs[item.Text];
                if (job.Status != status)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if any selected jobs have the requested status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool AnyJobsHaveStatus(JobStatus status)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                Job job = jobs[item.Text];
                if (job.Status == status)
                    return true;
            }
            return false;
        }

        #endregion
        #region update queue
        /// <summary>
        /// marks job currently marked as processing as aborted
        /// </summary>
        private void markJobAborted()
        {
            foreach (ListViewItem item in this.queueListView.Items)
            {
                Job job = jobs[item.Text];
                if (job.Status == JobStatus.PROCESSING) // processing -> done
                {
                    job.Status = JobStatus.ABORTED;
                    DateTime now = DateTime.Now;
                    job.End = now;
                    item.SubItems[5].Text = job.StatusString;
                    item.SubItems[7].Text = job.End.ToShortDateString();
                    if (mainForm.Settings.DeleteAbortedOutput)
                    {
                        mainForm.addToLog("Job aborted, deleting output file...");
                        try
                        {
                            File.Delete(job.Output);
                            mainForm.addToLog("Deletion successful.\r\n");
                        }
                        catch (Exception)
                        {
                            mainForm.addToLog("Deletion failed.\r\n");
                        }
                    }
                }
            }
        }

/*        /// <summary>
        /// marks the first job found in "processing" state in the job queue as done and returns its name
        /// </summary>
        /// <returns>name of the job marked as done</returns>
        private Job markJobDone(StatusUpdate su)
        {
            Job job = null;
            foreach (ListViewItem item in this.queueListView.Items)
            {
                job = jobs[item.Text];
                if (su.JobName == job.Name)
                {
                    if (job.Status == JobStatus.PROCESSING) // processing -> done
                    {
                        if (su.HasError)
                        {
                            job.Status = JobStatus.ERROR;
                            item.SubItems[5].Text = "error";
                        }
                        else if (su.WasAborted)
                        {
                            job.Status = JobStatus.ABORTED;
                            item.SubItems[5].Text = "aborted";
                        }
                        else
                        {
                            job.Status = JobStatus.DONE;
                            item.SubItems[5].Text = "done";

                        }
                        DateTime now = DateTime.Now;
                        job.End = now;
                        if (su.JobType == JobTypes.VIDEO) // video job
                        {
                            job.FPS = su.FPS;
                            item.SubItems[8].Text = su.FPS.ToString("##.##");
                        }
                        item.SubItems[7].Text = job.End.ToLongTimeString();
                    }
                    break;
                }
            }

            return job;
        }*/

        /// <summary>
        /// removes this job, and any previous jobs that belong to a series of jobs from the
        /// queue, then update the queue positions
        /// </summary>
        /// <param name="job">the job to be removed</param>
        private void removeCompletedJob(Job job)
        {
            // List all jobs to delete
            do {
                removeJobFromQueue(job);
                job = job.Previous;
            } while (job != null);
            updateJobPositions();
        }
        #endregion
        #region starting jobs
        /// <summary>
        /// starts the job provided as parameters
        /// </summary>
        /// <param name="job">the Job object containing all the parameters</param>
        /// <returns>success / failure indicator</returns>
        private bool startEncoding(Job job)
        {
            Debug.Assert(!this.isEncoding);
            string error;
            mainForm.ClosePlayer();
            
            // Get IJobProcessor
            currentProcessor = getProcessor(job);
            if (currentProcessor == null)
            {
                mainForm.addToLog("Skipping job\r\n");
                return false;
            }
            
            mainForm.addToLog("Starting job " + job.Name + " at " + DateTime.Now.ToLongTimeString() + "\r\n");
            
            // Preprocess
            preprocessJob(job);
            
            // Setup
            if (!currentProcessor.setup(job, out error))
            {
                mainForm.addToLog("Calling setup of processor failed with error " + error + "\r\n");
                currentProcessor = null;
                return false;
            }

            // Do JobControl setup
            mainForm.addToLog(" encoder commandline:\r\n" + job.Commandline + "\r\n");
            job.Status = JobStatus.PROCESSING;
            currentProcessor.StatusUpdate += new JobProcessingStatusUpdateCallback(enc_StatusUpdate);
            
            // Progress window
            pw = new ProgressWindow(job.JobType);
            pw.WindowClosed += new WindowClosedCallback(pw_WindowClosed);
            pw.Abort += new AbortCallback(pw_Abort);
            pw.setPriority(job.Priority);
            pw.PriorityChanged += new PriorityChangedCallback(pw_PriorityChanged);
            if (mainForm.Settings.OpenProgressWindow && mainForm.Visible)
            {
                mainForm.ProcessStatusChecked = true;
                pw.Show();
            }

            // Start
            if (!currentProcessor.start(out error))
            {
                mainForm.addToLog("starting encoder failed with error " + error + "\r\n");
                return false;
            }
            
            mainForm.addToLog("successfully started encoding\r\n");
            return true;
#if UNUSED
#warning make IJobProcessor init polymorphic
            if (job is VideoJob)
                currentProcessor = new VideoEncoder(mainForm.Settings);
            else if (job is AudioJob)
                currentProcessor = new AudioEncoder(mainForm.Settings);
            else if (job is MuxJob)
                currentProcessor = new Muxer(mainForm);
            else if (job is AviSynthJob)
                currentProcessor = new AviSynthProcessor();
            else if (job is IndexJob)
                currentProcessor = new DGIndexer(mainForm.Settings.DgIndexPath);
            else if (job is SubtitleIndexJob)
                currentProcessor = new VobSubIndexer();
            else
            {
                mainForm.addToLog("Unknown job type found. No job started.\r\n");
                return false;
            }
            if (currentProcessor.setup(job, out error))
            {

            }
            else // setup failed, probably a program is missing
            {
                mainForm.addToLog("calling setup failed with error " + error + "\r\n");
                this.isEncoding = false;
                currentProcessor = null;
                retval = false;
            }
            updateProcessingStatus();
            return retval;
#endif
        }
        /// <summary>
        /// starts the next job in status "waiting" from the queue list
        /// all items in the job queue are analyzed and the first found in status waiting is changed to 
        /// status processing, then a new Encoder object is created for it, the statusupdate callbacks
        /// are registered, a new progresswindow is created and finally the encoding is startred.
        /// if there's no remaining job in status waiting, the internal status indicating whether we're encoding
        /// is set back to false, as well as the queue encoding indicator
        /// </summary>
        /// <returns>0 = successfully started nex tjob, 1 = starting the next job failed, 2 = no jobs with status pending</returns>
        private JobStartInfo startNextJobInQueue()
        {
            bool triedToStart = false;
            foreach (ListViewItem item in this.queueListView.Items)
            {
                Job job = jobs[item.Text];
                if (job.Status != JobStatus.WAITING)
                    continue;
                if (!startEncoding(job))
                {
                    triedToStart = true;
                    job.Status = JobStatus.ERROR;
                    skipChainedJobs(job);
                    updateListViewInfo(job);
                    continue;
                }
                currentJob = job;
                job.Status = JobStatus.PROCESSING;
                job.Start = DateTime.Now;
                this.isEncoding = true;
                updateListViewInfo(job);
                updateProcessingStatus();
                return JobStartInfo.JOB_STARTED;
            }
            this.isEncoding = false;
            updateProcessingStatus();
            if (triedToStart) return JobStartInfo.COULDNT_START;
            return JobStartInfo.NO_JOBS_WAITING;
        }
        #endregion
        #region saving jobs
        /// <summary>
        /// saves all the jobs in the queue
        /// </summary>
        public void saveJobs()
        {
            int position = 0;
            foreach (ListViewItem item in this.queueListView.Items)
            {
                Job job = (Job)jobs[item.Text];
                job.Position = position;
                if (job.Next != null) job.NextJobName = job.Next.Name;
                else job.NextJobName = "";
                if (job.Previous != null) job.PreviousJobName = job.Previous.Name;
                else job.PreviousJobName = "";
                this.mainForm.JobUtil.saveJob(job, mainForm.MeGUIPath);
                position++;
            }
        }
        #endregion
        #region loading jobs
        /// <summary>
        /// loads all the jobs from the harddisk
        /// upon loading, the jobs are ordered according to their position field
        /// so that the order in which the jobs were previously shown in the GUI is preserved
        /// </summary>
        public void loadJobs()
        {
            string jobsPath = mainForm.MeGUIPath + "\\jobs\\";
            if (!Directory.Exists(jobsPath))
                Directory.CreateDirectory(jobsPath);
            DirectoryInfo di = new DirectoryInfo(mainForm.MeGUIPath + "\\jobs\\");
            FileInfo[] files = di.GetFiles("*.xml");
            foreach (FileInfo fi in files)
            {
                string fileName = fi.FullName;
                Job job = this.mainForm.JobUtil.loadJob(fileName);
                if (job != null)
                {
                    if (jobs.ContainsKey(job.Name))
                        MessageBox.Show("A job named " + job.Name + " is already in the queue.\nThe job defined in " + fileName + "\nwill be discarded", "Duplicate job name detected",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        jobs.Add(job.Name, job);
                }
            }
            Job[] orderedJobs = new Job[jobs.Count];
            jobs.Values.CopyTo(orderedJobs, 0);
            Array.Sort<Job>(orderedJobs, new Comparison<Job>(delegate(Job a, Job b) { return a.Position - b.Position; }));
            Debug.Assert(orderedJobs.Length < 2 || orderedJobs[0].Position < orderedJobs[1].Position);
            foreach (Job job in orderedJobs)
            {
                ListViewItem item = new ListViewItem(new string[] { job.Name, "", "", "", "", "", "", "", "" });
                queueListView.Items.Add(item);
                updateListViewInfo(job);
            }
            foreach (Job job in jobs.Values)
            {
                try { job.Next = jobs[job.NextJobName]; }
                catch (Exception) { job.Next = null; job.NextJobName = null; }
                try { job.Previous = jobs[job.PreviousJobName]; }
                catch (Exception) { job.Previous = null; job.PreviousJobName = null; }
            }
        }
        #endregion
        #region misc
        /// <summary>
        /// looks up the first free job number
        /// </summary>
        /// <returns>the job number that can be attributed to the next job to be added to the queue</returns>
        public int getFreeJobNumber()
        {
            int jobNr = 1;
            while (true)
            {
                string name = "job" + jobNr;
                bool found = false;
                foreach (string jobName in jobs.Keys)
                    if (jobName.StartsWith(name))
                    {
                        found = true;
                        break;
                    }
                if (!found) return jobNr;
                jobNr++;
            }
        }
        #endregion
        #region adding jobs to queue
        /// <summary>
        /// adds a job to the Queue (Hashtable) and the listview for graphical display
        /// </summary>
        /// <param name="job">the Job to be added to the next free spot in the queue</param>
        public void addJobsToQueue(params Job[] jobs)
        {
            foreach (Job j in jobs)
                addJob(j);
            if (!isEncoding && mainForm.Settings.AutoStartQueue)
                StartEncoding(false);
        }

        private void addJob(Job job)
        {
            job.Position = this.jobs.Count;
            jobs.Add(job.Name, job);
            ListViewItem item = new ListViewItem(
                new string[] { job.Name, "", "", "", "", "", "", "", "" });
            queueListView.Items.Add(item);
            updateListViewInfo(job);
        }
        #endregion
        #region deleting jobs
        /// <summary>
        /// removes the given job from the job queue, the listview and from the harddisk if the xml job file exists
        /// </summary>
        /// <param name="job">the job to be removed</param>
        private void removeJobFromQueue(Job job)
        {
            this.queueListView.BeginUpdate();
            jobs.Remove(job.Name);
            this.queueListView.Items[job.Position].Remove();
            this.queueListView.Refresh();
            this.queueListView.EndUpdate();
            string fileName = mainForm.MeGUIPath + "\\jobs\\" + job.Name + ".xml";
            if (File.Exists(fileName))
                File.Delete(fileName);
        }
        #endregion
        private void updateProcessingStatus()
        {
            if (this.isEncoding)
            {
                if (this.paused)
                {
                    pauseButton.Image = playImage;
                    pauseButton.Enabled = true;
                    mainForm.PauseMenuItemTS.Image = playImage;
                    mainForm.PauseMenuItemTS.Text = "Resume";
                }
                else
                {
                    pauseButton.Image = pauseImage;
                    pauseButton.Enabled = true;
                    mainForm.PauseMenuItemTS.Image = pauseImage;
                    mainForm.PauseMenuItemTS.Text = "Pause";
                    mainForm.PauseMenuItemTS.Enabled = true;
                }
                if (this.stopTheQueue)
                {
                    startStopButton.Text = "Start";
                    mainForm.StartStopMenuItemTS.Text = "Start";
                }
                else
                {
                    startStopButton.Text = "Stop";
                    mainForm.StartStopMenuItemTS.Text = "Stop";
                }
                mainForm.AbortMenuItemTS.Enabled = true;
                abortButton.Enabled = true;
            }
            else
            {
                startStopButton.Text = "Start";
                mainForm.StartStopMenuItemTS.Text = "Start";
                pauseButton.Image = pauseImage;
                pauseButton.Enabled = false;
                mainForm.PauseMenuItemTS.Image = pauseImage;
                mainForm.PauseMenuItemTS.Text = "Pause";
                mainForm.PauseMenuItemTS.Enabled = false;
                mainForm.AbortMenuItemTS.Enabled = false;
                abortButton.Enabled = false;
            }
        }
        /// <summary>
        /// aborts the currently active job
        /// </summary>
        public void Abort()
        {
            Debug.Assert(isEncoding);
            string error;
            if (!currentProcessor.stop(out error))
            {
                mainForm.addToLog("Error when trying to stop processing: " + error + "\r\n");
            }
            this.markJobAborted();
            this.isEncoding = false;
            if (this.paused) // aborting directly causes problems so prevent it
            {
                this.paused = false;
                if (currentProcessor.resume(out error))
                {
                    mainForm.addToLog("Error when trying to resume processing: " + error + "\r\n");
                }
            }
            updateProcessingStatus();
        }

        internal void showAfterEncodingStatus(MeGUISettings Settings)
        {
            if (Settings.AfterEncoding == AfterEncoding.DoNothing)
                afterEncoding.Text = "After encoding: do nothing";
            else if (Settings.AfterEncoding == AfterEncoding.Shutdown)
                afterEncoding.Text = "After encoding: shutdown";
            else
                afterEncoding.Text = "After encoding: run '" + Settings.AfterEncodingCommand + "'";
        }
        
        private void StartEncoding(bool showMessageBoxes)
        {
            if (this.queueListView.Items.Count <= 0) // we can't start encoding if there are no jobs
            {
                MessageBox.Show("Please give me something to do", "No jobs found", MessageBoxButtons.OK);
                return;
            }

            mainForm.ClosePlayer();
            this.stopTheQueue = false;
            JobStartInfo retval = startNextJobInQueue();
            if (showMessageBoxes)
            {
                if (retval == JobStartInfo.COULDNT_START)
                    MessageBox.Show("Couldn't start processing. Please consult the log for more details", "Processing failed", MessageBoxButtons.OK);
                else if (retval == JobStartInfo.NO_JOBS_WAITING)
                    MessageBox.Show("No jobs are waiting. Nothing to do", "No jobs waiting", MessageBoxButtons.OK);
            }
        }

        public static readonly JobPostProcessor DeleteIntermediateFilesPostProcessor = new JobPostProcessor(
            new Processor(deleteIntermediateFiles), "DeleteIntermediateFiles");

        /// <summary>
        /// Attempts to delete all files listed in job.FilesToDelete if settings.DeleteIntermediateFiles is checked
        /// </summary>
        /// <param name="job">the job which should just have been completed</param>
        private static void deleteIntermediateFiles(MainForm mainForm, Job job)
        {
            if (mainForm.Settings.DeleteIntermediateFiles)
            {
                mainForm.addToLog("Job completed successfully and deletion of intermediate files is activated\r\n");
                foreach (string file in job.FilesToDelete)
                {
                    mainForm.addToLog("Found intermediate output file '" + ((string)file)
                        + "', deleting...\r\n");
                    try
                    {
                        File.Delete(file);
                        mainForm.addToLog("Deletion succeeded.");
                    }
                    catch (IOException)
                    {
                        mainForm.addToLog("Deletion failed.");
                    }
                }
            }
        }
    
    }
    enum JobStartInfo { JOB_STARTED, NO_JOBS_WAITING, COULDNT_START }
}
