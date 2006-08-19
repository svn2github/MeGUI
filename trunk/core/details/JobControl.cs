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
        private bool isEncoding = false, queueEncoding = false, paused = false; // encoding status and whether or not we're in queue encoding mode
        private Dictionary<string, Job> jobs = new Dictionary<string,Job>(); //storage for all the jobs and profiles known to the system
        private List<Job> skipJobs = new List<Job>(); // contains jobs to be skipped (chained with a previously errored out job)
        private int jobNr = 1; // number of jobs in the queue
        private IJobProcessor currentProcessor;
        private MainForm mainForm;
        private ProgressWindow pw; // window that shows the encoding progress

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

        public JobControl()
        {
            InitializeComponent();

            System.Reflection.Assembly myAssembly = this.GetType().Assembly;
            string name = /*this.GetType().Namespace + "."*/ "MeGUI.";
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

        public MainForm MainForm
        {
            set { mainForm = value; }
        }

        public bool IsEncoding
        {
            get { return isEncoding; }
        }

        public bool QueueEncoding
        {
            get { return queueEncoding; }
            set { queueEncoding = value; }
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
        private void abortMenuItem_Click(object sender, EventArgs e)
        {
            abortButton_Click(sender, e);
        }

        void deleteMenuItem_Click(object sender, EventArgs e)
        {
            deleteJobButton_Click(sender, e);
        }

        private void loadMenuItem_Click(object sender, EventArgs e)
        {
            loadJobButton_Click(sender, e);
        }

        private void postponeMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                int position = item.Index;
                Job job = jobs[item.Text];
                if (job.Status != JobStatus.PROCESSING)
                {
                    job.Status = JobStatus.POSTPONED;
                    this.queueListView.Items[position].SubItems[5].Text = job.StatusString;
                }
                else
                {
                    Debug.Assert(false, "shouldn't be able to postpone an active job");
                    return;
                }
            }
        }

        private void waitingMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                int position = item.Index;
                Job job = jobs[item.Text];
                if (job.Status != JobStatus.PROCESSING)
                {
                    job.Status = JobStatus.WAITING;
                    this.queueListView.Items[position].SubItems[5].Text = job.StatusString;
                }
                else
                {
                    Debug.Assert(false, "shouldn't be able to set an active job back to waiting");
                    return;
                }
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

        public Job GetJobByNameSafe(string name)
        {
            try
            {
                return jobs[name];
            }
            catch (Exception)
            {
                return null;
            }
        }

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
                mainForm.TitleText = Application.ProductName + " " + Application.ProductVersion;
                Job job = markJobDone(su);
                currentProcessor = null;
                this.jobProgress.Value = 0;
                if (pw != null)
                {
                    pw.IsUserAbort = false; // ensures that the window will be closed
                    pw.Close();
                    pw = null;
                }
                mainForm.addToLog("Processing ended at " + DateTime.Now.ToLongTimeString() + "\r\n");
                mainForm.addToLog("----------------------------------------------------------------------------------------------------------" +
                    "\r\n\r\nLog for job " + su.JobName + "\r\n\r\n" + su.Log +
                    "\r\n----------------------------------------------------------------------------------------------------------\r\n");
                bool jobCompletedSuccessfully = true; // true = no errors; false = some reason to stop
                if (su.WasAborted)
                {
                    mainForm.addToLog("The current job was aborted. Stopping queue mode\r\n");
                    jobCompletedSuccessfully = false;
                }
                if (su.HasError)
                {
                    mainForm.addToLog("The current job contains errors. Skipping chained jobs\r\n");
                    Job j = null;
                    jobCompletedSuccessfully = false;
                    if (job.Next != null)
                    {
                        j = job;
                        while (j.Next != null) // find all chained jobs
                        {
                            j = j.Next;
                            this.skipJobs.Add(j);
                        }
                    }
                }
#warning eventually turn deleteIntermediateFiles into a job postprocessor
                if (jobCompletedSuccessfully)
                    deleteIntermediateFiles(job);
                if (jobCompletedSuccessfully)
                {
                    postprocessJob(job);
                }
                this.isEncoding = false;
                if (job.Next != null && jobCompletedSuccessfully) // try finding a chained job
                {
#warning this will fail if the user deletes the next job
                    Job next = job.Next;
                    mainForm.addToLog("job " + job.Name + " has been processed. This job is linked to the next job: " + next.Name + "\r\n");
                }
#warning use enums instead of int's here
                int nextJobStart = 0;
                if (this.queueEncoding)
                {
                    nextJobStart = startNextJobInQueue(); //new with the return value to check if there was another job
                }
                else
                {
                    nextJobStart = 2;
                }
                if (nextJobStart == 2)
                {	//new test if this was the last job or a job was stoped
                    this.isEncoding = false;	//moved out the else before
                    this.queueEncoding = false;
                    this.startStopButton.Text = "Start";
                    this.abortButton.Enabled = false;
                    mainForm.shutdown();
                }
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
                if (mainForm.Settings.Shutdown)
                    mainForm.TitleText += "- SHUTDOWN after encode";
                this.jobProgress.Value = su.PercentageDone;
            }
        }

        private void postprocessJob(Job job)
        {
            mainForm.addToLog("Starting postprocessing of job...\n");
            foreach (JobPostProcessor pp in mainForm.PackageSystem.JobPostProcessors.Values)
            {
                pp.PostProcessor(mainForm, job);
            }
            mainForm.addToLog("Postprocessing finished!\n");
        }

        private void preprocessJob(Job job)
        {
            mainForm.addToLog("Starting preprocessing of job...\n");
            foreach (JobPreProcessor pp in mainForm.PackageSystem.JobPreProcessors.Values)
            {
                pp.PreProcessor(mainForm, job);
            }
            mainForm.addToLog("Preprocessing finished!\n");
        }
        /// <summary>
        /// Attempts to delete all files listed in job.FilesToDelete if settings.DeleteIntermediateFiles is checked
        /// </summary>
        /// <param name="job">the job which should just have been completed</param>
        private void deleteIntermediateFiles(Job job)
        {
            if (mainForm.Settings.DeleteIntermediateFiles)
            {
                mainForm.addToLog("Job completed successfully and deletion of intermediate files is activated\r\n");
                foreach (string file in job.FilesToDelete)
                {
                    try
                    {
                        mainForm.addToLog("Found intermediate output file '" + ((string)file)
                            + "', deleting...\r\n");
                        File.Delete(file);
                        mainForm.addToLog("Deletion succeeded.");
                    }
                    catch (Exception)
                    {
                        mainForm.addToLog("Deletion failed.");
                    }
                }
            }
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
            {
                this.queueEncoding = !this.queueEncoding;
            }
            else // we're not encoding yet
            {
                if (this.queueListView.Items.Count > 0) // we can't start encoding if there are no jobs
                {
                    mainForm.ClosePlayer();
                    this.skipJobs.Clear();
                    this.queueEncoding = true;
                    int retval = startNextJobInQueue();
                    if (retval == 1)
                        MessageBox.Show("Couldn't start processing. Please consult the log for more details", "Processing failed", MessageBoxButtons.OK);
                    else if (retval == 2)
                        MessageBox.Show("No jobs are waiting. Nothing to do", "No jobs waiting", MessageBoxButtons.OK);
                }
                else
                    MessageBox.Show("Please give me something to do", "No jobs found", MessageBoxButtons.OK);
            }
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
                        this.pauseButton.Image = (Image)this.pauseImage;
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
            if (this.queueListView.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in this.queueListView.SelectedItems)
                {
                    if (!jobs.ContainsKey(item.Text)) // Check if it has already been deleted
                        continue;
                    Job job = jobs[item.Text];
                    if (job != null)
                    {
                        if (job.Status != JobStatus.PROCESSING)
                        {
                            if (job.Next != null || job.Previous != null)
                            {
                                DialogResult dr = MessageBox.Show("This job is part of a series of jobs. Deleting it alone will cause corruption of the dependant jobs\r\n"
                                    + "Press Yes to delete all dependant jobs, No to delete just this job or Cancel to abort.", "Job dependency detected", MessageBoxButtons.YesNoCancel);
                                switch (dr)
                                {
                                    case DialogResult.Yes:
                                        this.removeJobFromQueue(job);
                                        Job prev = null;
                                        if (job.Previous != null)
                                            prev = job.Previous;
                                        while (prev != null)
                                        {
                                            removeJobFromQueue(prev);
                                            if (prev.Previous != null)
                                                prev = prev.Previous;
                                            else
                                                prev = null;
                                        }
                                        Job next = null;
                                        if (job.Next != null)
                                            next = job.Next;
                                        while (next != null)
                                        {
                                            removeJobFromQueue(next);
                                            if (next.Next != null)
                                                next = next.Next;
                                            else
                                                next = null;
                                        }
                                        break;
                                    case DialogResult.No:
                                        removeJobFromQueue(job);
                                        break;
                                    case DialogResult.Cancel: // do nothing
                                        break;
                                }
                            }
                            else // no dependent jobs
                            {
                                removeJobFromQueue(job);
                            }
                        }
                        else
                            MessageBox.Show("You cannot delete a job while it is being processed.", "Deleting job failed", MessageBoxButtons.OK);
                    }
                }
                updateJobPositions();
            }
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
        private void shutdownCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mainForm.Settings.Shutdown = this.shutdownCheckBox.Checked;
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
                if (job.Status == JobStatus.POSTPONED || job.Status == JobStatus.ERROR || job.Status == JobStatus.ABORTED || job.Status == JobStatus.DONE) // postponed/error/aborted/done -> waiting
                    job.Status = JobStatus.WAITING;
                else if (job.Status == JobStatus.WAITING) // waiting -> postponed
                    job.Status = JobStatus.POSTPONED;
                else if (job.Status == JobStatus.PROCESSING && !this.isEncoding) // b0rked processing job
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

        private int getJobPosition(string name)
        {
            foreach (ListViewItem item in this.queueListView.Items)
            {
                if (item.SubItems[0].Text.Equals(name))
                {
                    return item.Index;
                }
            }
            return -1;
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
        /// <summary>
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
            if (job != null && job.Status == JobStatus.DONE && job.Next == null && mainForm.Settings.DeleteCompletedJobs)
                removeCompletedJob(job);
            return job;
        }
        /// <summary>
        /// removes this job, and any previous jobs that belong to a series of jobs from the
        /// queue, then update the queue positions
        /// </summary>
        /// <param name="job">the job to be removed</param>
        private void removeCompletedJob(Job job)
        {
            List<Job> jobs = new List<Job>();
            jobs.Add(job);
            Job j = job;
            while (j.Previous != null) // find all previous jobs
            {
                jobs.Add(j.Previous);
                j = j.Previous;
            }
            ListViewItem item2Flush = null;
            foreach (Job j2 in jobs)
            {
                this.jobs.Remove(j2.Name);
                foreach (ListViewItem item in this.queueListView.Items)
                {
                    if (item.Text.Equals(j2.Name))
                    {
                        item2Flush = item;
                        break;
                    }
                }
                this.queueListView.Items.Remove(item2Flush);
            }
            updateJobPositions();
        }
        #endregion
        #region starting jobs
        /// <summary>
        /// starts the job provided as parameters
        /// </summary>
        /// <param name="job">the Job object containing all the parameters</param>
        /// <returns>success / failure indicator</returns>
        public bool startEncoding(Job job)
        {
            mainForm.ClosePlayer();
            bool retval = false;
            if (this.isEncoding) // we're already encoding and can't start another job
                return false;
            string error;
            preprocessJob(job);
            mainForm.addToLog("Starting job " + job.Name + " at " + DateTime.Now.ToLongTimeString() + "\r\n");
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
                mainForm.addToLog(" encoder commandline:\r\n" + job.Commandline + "\r\n");
                ListViewItem item = this.queueListView.Items[getJobPosition(job.Name)];
                item.SubItems[5].Text = "processing";
                item.SubItems[6].Text = DateTime.Now.ToLongTimeString();
                item.SubItems[7].Text = "";
                item.SubItems[8].Text = "";
                job.Status = JobStatus.PROCESSING;
                currentProcessor.StatusUpdate += new JobProcessingStatusUpdateCallback(enc_StatusUpdate);
                pw = new ProgressWindow(job.JobType);
                pw.WindowClosed += new WindowClosedCallback(pw_WindowClosed);
                pw.Abort += new AbortCallback(pw_Abort);
                pw.setPriority(job.Priority);
                pw.PriorityChanged += new PriorityChangedCallback(pw_PriorityChanged);
                if (mainForm.Settings.OpenProgressWindow && this.Visible)
                {
                    mainForm.ProcessStatusChecked = true;
                    pw.Show();
                }
                if (currentProcessor.start(out error))
                {
                    this.isEncoding = true;
                    retval = true;
                    mainForm.addToLog("successfully started encoding\r\n");
                }
                else
                {
                    mainForm.addToLog("starting encoder failed with error " + error + "\r\n");
                    this.isEncoding = false;
                    retval = false;
                }
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
        public int startNextJobInQueue()
        {
            ListViewItem i = null;
            Job job = null;
            foreach (ListViewItem item in this.queueListView.Items)
            {
                if (item.SubItems[5].Text.Equals("waiting")) // this is an item to be encoded
                {
                    i = item;
                    job = jobs[item.SubItems[0].Text];
                    if (this.skipJobs.Contains(job)) // this job is to be skipped
                        continue;
                    if (startEncoding(job))
                    {
                        i.SubItems[5].Text = "processing";
                        job.Start = DateTime.Now;
                        i.SubItems[6].Text = job.Start.ToLongTimeString();
                        job.Status = JobStatus.PROCESSING;
                        return 0;
                    }
                    else
                        return 1;
                }
            }
            updateProcessingStatus();
            return 2;
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
            Dictionary<string, Job> remaining = new Dictionary<string, Job>(jobs);
            Job next = null;
            while (remaining.Count > 0)
            {
                foreach (Job job in remaining.Values)
                {
                    if (next == null ||
                        job.Position < next.Position)
                    {
                        next = job;
                    }
                }
                remaining.Remove(next.Name);
                string codec = "", encodingMode = "", start = "", end = "", fps = "";
                ListViewItem item = new ListViewItem(new string[] { next.Name, next.InputFileName, next.OutputFileName });
                codec = next.CodecString;
                encodingMode = next.EncodingMode;
                fps = next.FPSString;
/*                if (next is VideoJob)
                {
                    codec = ((VideoJob)next).CodecString;
                    encodingMode = ((VideoJob)next).EncodingMode;
                    if (next.Status == JobStatus.DONE)
                        fps = next.FPS.ToString();
                }
                else if (next is AudioJob)
                {
                    codec = ((AudioJob)next).CodecString;
                    encodingMode = ((AudioJob)next).EncodingMode;
                }
                else if (next is MuxJob)
                {
                    encodingMode = "mux";
                }
                else if (next is IndexJob)
                {
                    encodingMode = "idx";
                }
                else if (next is AviSynthJob)
                {
                    encodingMode = "avs";
                }*/
                switch (next.Status)
                {
                    case JobStatus.ABORTED:
                    case JobStatus.ERROR:
                    case JobStatus.DONE:
                        start = next.Start.ToLongTimeString();
                        end = next.End.ToLongTimeString();
                        break;
                    case JobStatus.PROCESSING:
                        start = next.Start.ToLongTimeString();
                        break;
                }
                item.SubItems.AddRange(new string[] { codec, encodingMode, next.StatusString, start, end, fps });
                this.queueListView.Items.Add(item);
                next = null;
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
            jobNr = 1;
            string name = "job" + this.jobNr;
            while (jobs.ContainsKey(name) || jobs.ContainsKey(name + "-1") || jobs.ContainsKey(name + "-2") || jobs.ContainsKey(name + "-3") || jobs.ContainsKey(name + "-4")
                || jobs.ContainsKey(name + "-5"))
            {
                name = "job" + this.jobNr;
                jobNr++;
            }
            if (jobNr > 1)
                return jobNr - 1;
            else
                return jobNr;
        }
        #endregion
        #region adding jobs to queue
        /// <summary>
        /// adds a job to the Queue (Hashtable) and the listview for graphical display
        /// </summary>
        /// <param name="job">the Job to be added to the next free spot in the queue</param>
        public void addJobToQueue(Job job)
        {
            job.Position = this.jobs.Count;
            ListViewItem item;
            if (job is VideoJob)
            {
                jobs.Add(job.Name, job); // adds job to the queue
                item = new ListViewItem(new string[] {job.Name, job.InputFileName, job.OutputFileName, 
							((VideoJob)job).CodecString, ((VideoJob)job).EncodingMode, "waiting", "", "", ""});
                this.queueListView.Items.Add(item);
            }
            if (job is AudioJob)
            {
                jobs.Add(job.Name, job); // adds job to the queue
                item = new ListViewItem(new string[] {job.Name, job.InputFileName, job.OutputFileName, 
														 ((AudioJob)job).CodecString, ((AudioJob)job).EncodingMode, "waiting", "", "", ""});
                this.queueListView.Items.Add(item);
            }
            if (job is MuxJob)
            {
                jobs.Add(job.Name, job);
                item = new ListViewItem(new string[] {job.Name, job.InputFileName, job.OutputFileName, 
						"", "mux", "waiting", "", "", ""});
                this.queueListView.Items.Add(item);
            }
            if (job is IndexJob)
            {
                jobs.Add(job.Name, job);
                item = new ListViewItem(new string[] {job.Name, job.InputFileName, job.OutputFileName, 
														 "", "idx", "waiting", "", "", ""});
                this.queueListView.Items.Add(item);
            }
            if (job is AviSynthJob)
            {
                jobs.Add(job.Name, job);
                item = new ListViewItem(new string[] {job.Name, job.InputFileName, job.OutputFileName, 
														 "", "avs", "waiting", "", "", ""});
                this.queueListView.Items.Add(item);
            }
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
            int position = getJobPosition(job.Name);
            this.queueListView.Items[position].Remove();
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
                if (this.queueEncoding)
                {
                    startStopButton.Text = "Stop";
                    mainForm.StartStopMenuItemTS.Text = "Stop";
                }
                else
                {
                    startStopButton.Text = "Start";
                    mainForm.StartStopMenuItemTS.Text = "Start";
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
            string error;
            if (this.isEncoding)
            {
                if (!currentProcessor.stop(out error))
                {
                    mainForm.addToLog("Error when trying to stop processing: " + error + "\r\n");
                }
            }
            this.markJobAborted();
            this.isEncoding = false;
            this.queueEncoding = false;
            if (this.paused) // aborting directly causes problems so prevent it
            {
                this.paused = false;
                if (currentProcessor.resume(out error))
                {
                    mainForm.addToLog("Error when trying to resume processing: " + error + "\r\n");
                }
            }
            this.shutdownCheckBox.Checked = false;
            mainForm.Settings.Shutdown = false;
            updateProcessingStatus();
        }

        public bool Shutdown
        {
            get { return shutdownCheckBox.Checked; }
            set { shutdownCheckBox.Checked = value; }
        }
    }
}
