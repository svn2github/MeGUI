using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using MeGUI.core.util;
using System.Collections;

namespace MeGUI.core.gui
{
    public delegate Pair<string, MultiJobHandler>[] MultiJobMenuGenerator();
    public delegate void JobChangeEvent(object sender, JobQueueEventArgs info);
    public delegate void RequestJobDeleted(Job jobs);
    public delegate void SingleJobHandler(Job job);
    public delegate void MultiJobHandler(List<Job> jobs);
    public enum StartStopMode { Start, Stop };
    public enum PauseResumeMode { Pause, Resume, Disabled };
    public enum Dependencies { DeleteAll, RemoveDependencies }

    public partial class JobQueue : UserControl
    {
        #region pause/play image
#if CSC
        private static readonly string __Name = "";
#else
        private static readonly string __Name = "MeGUI.";
#endif
        private static readonly System.Reflection.Assembly myAssembly = typeof(JobQueue).Assembly;
        private static readonly Bitmap pauseImage = new Bitmap(myAssembly.GetManifestResourceStream(__Name + "pause.ico"));
        private static readonly Bitmap playImage = new Bitmap(myAssembly.GetManifestResourceStream(__Name + "play.ico"));
        #endregion

        private Dictionary<string, Job> jobs = new Dictionary<string, Job>();
        private List<ToolStripItem> singleJobHandlers = new List<ToolStripItem>();
        private List<ToolStripItem> multiJobHandlers = new List<ToolStripItem>();
        private List<Pair<ToolStripMenuItem, MultiJobMenuGenerator>> menuGenerators = new List<Pair<ToolStripMenuItem, MultiJobMenuGenerator>>();
        private StartStopMode startStopMode;
        private PauseResumeMode pauseResumeMode;

        [Browsable(false)]
        public StartStopMode StartStopMode
        {
            get { return startStopMode; }
            set
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate { StartStopMode = value; }));
                    return;
                }

                startStopMode = value;
                switch (value)
                {
                    case StartStopMode.Start:
                        startStopButton.Text = "Start";;
                        break;

                    case StartStopMode.Stop:
                        startStopButton.Text = "Stop";;
                        break;
                }
            }
        }

        [Browsable(false)]
        public PauseResumeMode PauseResumeMode
        {
            get { return pauseResumeMode; }
            set
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate { PauseResumeMode = value; }));
                    return;
                }

                pauseResumeMode = value;
                switch (value)
                {
                    case PauseResumeMode.Disabled:
                        pauseButton.Enabled = false;
                        pauseButton.Image = pauseImage;
                        break;

                    case PauseResumeMode.Pause:
                        pauseButton.Enabled = true;
                        pauseButton.Image = pauseImage;
                        break;

                    case PauseResumeMode.Resume:
                        pauseButton.Enabled = true;
                        pauseButton.Image = playImage;
                        break;
                }
            }
        }

        #region public interface: jobs
        [Browsable(false)]
        public IEnumerable<Job> JobList
        {
            get
            {
                if (InvokeRequired)
                {
                    return (IEnumerable<Job>)Invoke(new Getter<IEnumerable<Job>>(delegate { return JobList; }));
                }

                Job[] jobList = new Job[jobs.Count];
                foreach (Job j in jobs.Values)
                {
                    jobList[indexOf(j)] = j;
                }
                return jobList;
            }

            set
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate { JobList = value; }));
                    return;
                }

                queueListView.BeginUpdate();
                queueListView.Items.Clear();
                foreach (Job job in value)
                {
                    queueListView.Items.Add(new ListViewItem(new string[] { job.Name, "", "", "", "", "", "", "", "", "" }));
                    jobs[job.Name] = job;
                }
                queueListView.EndUpdate();
                refreshQueue();
            }
        }

        public void enqueueJob(Job j)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { enqueueJob(j); }));
                return;
            }

            queueListView.Items.Add(new ListViewItem(new string[] { j.Name, "", "", "", "", "", "", "", "", "" }));
            jobs[j.Name] = j;
            refreshQueue();
        }

        public void removeJobFromQueue(Job job)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { removeJobFromQueue(job); }));
                return;
            }

            queueListView.Items[indexOf(job)].Remove();
            jobs.Remove(job.Name);
            queueListView.Refresh();
        }
        #endregion

        #region adding GUI elements
        

        private void addItem(ToolStripMenuItem item, string parent)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { addItem(item, parent); }));
                return;
            }

            if (parent == null)
                queueContextMenu.Items.Add(item);
            else
                foreach (ToolStripMenuItem i in queueContextMenu.Items)
                    if (i.Text == parent)
                    {
                        i.DropDownItems.Add(item);
                        break;
                    }
        }

        public void AddDynamicSubMenu(string name, string parent, MultiJobMenuGenerator gen)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(name);
            menuGenerators.Add(new Pair<ToolStripMenuItem, MultiJobMenuGenerator>(item, gen));
            addItem(item, parent);
        }

        public void AddMenuItem(string name, string parent)
        {
            addItem(new ToolStripMenuItem(name), parent);
        }

        public void AddMenuItem(string name, string parent, SingleJobHandler handler)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(name);
            item.Click += delegate(object sender, EventArgs e)
            {
                Debug.Assert(queueListView.SelectedItems.Count == 1);
                Job j = jobs[queueListView.SelectedItems[0].Text];
                handler(j);
            };
            singleJobHandlers.Add(item);
            addItem(item, parent);
        }

        public void AddMenuItem(string name, string parent, MultiJobHandler handler)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(name);
            item.Click += delegate(object sender, EventArgs e)
            {
                Debug.Assert(queueListView.SelectedItems.Count > 0);
                List<Job> list = new List<Job>();
                foreach (ListViewItem i in queueListView.SelectedItems)
                    list.Add(jobs[i.Text]);
                handler(list);
            };
            addItem(item, parent);
            multiJobHandlers.Add(item);
        }

        public void AddButton(string name, EventHandler handler)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { AddButton(name, handler); }));
                return;
            }

            Button b = new Button();
            b.Text = name;
            b.Click += handler;
            b.AutoSize = true;
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(b);
        }

        public void SetStartStopButtonsTogether()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { SetStartStopButtonsTogether(); }));
                return;
            }

            flowLayoutPanel1.Controls.Remove(stopButton);
        }


        #endregion

        #region indexOf
        private int indexOf(Job j)
        {
            Debug.Assert(jobs.ContainsKey(j.Name), "Looking for a job which isn't in the jobs dictionary");
            foreach (ListViewItem i in queueListView.Items)
            {
                if (i.Text == j.Name)
                {
                    int index = i.Index;
                    Debug.Assert(index >= 0);
                    return index;
                }
            }
            Debug.Assert(false, "Couldn't find job in the GUI queue");
            throw new Exception();
        }
        #endregion

        public JobQueue()
        {
            InitializeComponent();
            StartStopMode = StartStopMode.Start;
            PauseResumeMode = PauseResumeMode.Disabled;
        }

        #region job deletion
        public RequestJobDeleted RequestJobDeleted;
        public event EventHandler AbortClicked;
        public event EventHandler StartClicked;
        public event EventHandler StopClicked;
        public event EventHandler PauseClicked;
        public event EventHandler ResumeClicked;

        private List<Job> removeAllDependantJobsFromQueue(Job job)
        {
            removeJobFromQueue(job);
            List<Job> list = new List<Job>();
            foreach (Job j in job.EnabledJobs)
            {
                if (jobs.ContainsKey(j.Name))
                    list.AddRange(removeAllDependantJobsFromQueue(j));
                else
                    list.Add(j);
            }
            return list;
        }

        private void deleteJobButton_Click(object sender, EventArgs e)
        {
            if (queueListView.SelectedItems.Count <= 0) return;

            foreach (ListViewItem item in this.queueListView.SelectedItems)
            {
                processUserRequestToDelete(item.Text);
            }
        }

        private void processUserRequestToDelete(string name)
        {
            if (!jobs.ContainsKey(name)) // Check if it has already been deleted
                return;
            Job job = jobs[name];
            if (job == null) return;
            RequestJobDeleted(job);
        }
        #endregion

        #region list movement
        enum Direction { Up, Down }
        private void downButton_Click(object sender, EventArgs e)
        {
            MoveListViewItem(Direction.Up);
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            MoveListViewItem(Direction.Down);
        }

        /// <summary>
        /// moves the currently selected listviewitem up/down
        /// adapted from code by Less Smith @ KnotDot.Net
        /// </summary>
        /// <param name="lv">reference to ListView</param>
        /// <param name="moveUp">whether the currently selected item should be moved up or down</param>
        private void MoveListViewItem(Direction d)
        {
            // We can trust that the button will be disabled unless this condition is met
            Debug.Assert(isSelectionMovable(d));

            ListView lv = queueListView;
            ListView.ListViewItemCollection items = lv.Items;

            int[] indices = new int[lv.SelectedIndices.Count];
            lv.SelectedIndices.CopyTo(indices, 0);
            Array.Sort(indices);
            int min = indices[0];
            int max = indices[indices.Length - 1];

            lv.BeginUpdate();
            if (d == Direction.Up)
            {
                items[max].Selected = false;
                items[min - 1].Selected = true;

                for (int i = min; i <= max; i++)
                    swapContents(items[i], items[i - 1]);
            }
            else if (d == Direction.Down)
            {
                items[min].Selected = false;
                items[max + 1].Selected = true;

                for (int i = max; i >= min; i--)
                    swapContents(items[i], items[i + 1]);
            }
            lv.EndUpdate();
            lv.Refresh();
        }

        /// <summary>
        /// swaps the contents of the two items
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void swapContents(ListViewItem a, ListViewItem b)
        {
            for (int i = 0; i < a.SubItems.Count; i++)
            {
                string cache = b.SubItems[i].Text;
                b.SubItems[i].Text = a.SubItems[i].Text;
                a.SubItems[i].Text = cache;
            }
        }

        /// <summary>
        /// Tells if the current selection can be moved in direction d.
        /// Checks:
        ///     whether it's at the top / bottom
        ///     if anything is actually selected
        ///     whether the selection is contiguous
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private bool isSelectionMovable(Direction d)
        {
            ListView lv = queueListView;
            int[] indices = new int[lv.SelectedIndices.Count];
            lv.SelectedIndices.CopyTo(indices, 0);
            Array.Sort(indices);

            if (indices.Length == 0) return false;
            if (d == Direction.Up && indices[0] == 0) return false;
            if (d == Direction.Down &&
                indices[indices.Length - 1] == queueListView.Items.Count - 1)
                return false;
            if (!consecutiveIndices(indices)) return false;

            return true;
        }

        /// <summary>
        /// Tells if the given list of indices is consecutive; if so, sets min and 
        /// max to the min and max indices
        /// </summary>
        /// <param name="indices"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        private bool consecutiveIndices(int[] indices)
        {
            Debug.Assert(indices.Length > 0);

            int last = indices[0] - 1;
            foreach (int i in indices)
            {
                if (i != last + 1) return false;
                last = i;
            }

            return true;
        }

        /// <summary>
        /// Updates the up/down buttons according to whether the selection CAN be moved up/down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queueListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            upButton.Enabled = isSelectionMovable(Direction.Up);
            downButton.Enabled = isSelectionMovable(Direction.Down);
        }

        #endregion

        #region load/update
        private void updateJobButton_Click(object sender, EventArgs e)
        {
            throw new Exception("Not yet implemented!");
        }

        private void loadJobButton_Click(object sender, EventArgs e)
        {
            throw new Exception("Not yet implemented!");
        }
        #endregion

        #region contextmenu events
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


        private void queueContextMenu_Opened(object sender, EventArgs e)
        {
            int count = queueListView.SelectedItems.Count;
            foreach (ToolStripItem i in multiJobHandlers)
                i.Enabled = (count > 0);
            foreach (ToolStripItem i in singleJobHandlers)
                i.Enabled = (count == 1);

            // here we generate our submenus as required, giving them the event handlers
            if (count > 0)
            {
                foreach (Pair<ToolStripMenuItem, MultiJobMenuGenerator> p in menuGenerators)
                {
                    p.fst.DropDownItems.Clear();
                    foreach (Pair<string, MultiJobHandler> pair in p.snd())
                    {
                        ToolStripItem i = p.fst.DropDownItems.Add(pair.fst);
                        i.Tag = pair.snd;
                        i.Click += delegate(object sender1, EventArgs e2)
                        {
                            Debug.Assert(queueListView.SelectedItems.Count > 0);
                            List<Job> list = new List<Job>();
                            foreach (ListViewItem i2 in queueListView.SelectedItems)
                                list.Add(jobs[i2.Text]);
                            ((MultiJobHandler)((ToolStripItem)sender1).Tag)(list);
                        };
                    }
                }
            }
        
            AbortMenuItem.Enabled = AllJobsHaveStatus(JobStatus.PROCESSING) || AllJobsHaveStatus(JobStatus.ABORTED);
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
        #endregion

        #region start / stop / abort / pause
        private void abortButton_Click(object sender, EventArgs e)
        {
            AbortClicked(this, e);
        }

        private void startStopButton_Click(object sender, EventArgs e)
        {
            switch (startStopMode)
            {
                case StartStopMode.Start:
                    StartClicked(this, e);
                    break;

                case StartStopMode.Stop:
                    StopClicked(this, e);
                    break;
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            switch (pauseResumeMode)
            {
                case PauseResumeMode.Disabled:
                    throw new Exception("The supposedly disabled pause button was clicked");
                    break;

                case PauseResumeMode.Pause:
				    if (PauseClicked != null)
				    {
				        PauseClicked(this, e);
				    }
                    break;

                case PauseResumeMode.Resume:
					if (ResumeClicked != null)
					{
						ResumeClicked(this, e);
					}
                    break;
            }
        }
        #endregion

        #region redrawing

        public void refreshQueue()
        {
            if (!Visible) return;
            if (queueListView.InvokeRequired)
            {
                queueListView.Invoke(new MethodInvoker(delegate { refreshQueue(); }));
                return;
            }

            queueListView.BeginUpdate();
            foreach (ListViewItem item in queueListView.Items)
            {
                Job job = jobs[item.Text];
                item.SubItems[1].Text = job.InputFileName;
                item.SubItems[2].Text = job.OutputFileName;
                item.SubItems[3].Text = job.CodecString;
                item.SubItems[4].Text = job.EncodingMode;
                item.SubItems[5].Text = job.StatusString;
                item.SubItems[6].Text = job.OwningWorker ?? "";
                
                if (job.Status == JobStatus.DONE)
                {
                    item.SubItems[8].Text = job.End.ToLongTimeString();
                    item.SubItems[9].Text = job.EncodingSpeed;
                }
                else
                {
                    item.SubItems[8].Text = "";
                    item.SubItems[9].Text = "";
                }
                if (job.Status == JobStatus.DONE || job.Status == JobStatus.PROCESSING)
                    item.SubItems[7].Text = job.Start.ToLongTimeString();
                else
                    item.SubItems[7].Text = "";
            }
            queueListView.EndUpdate();
            queueListView.Refresh();
        }
        #endregion

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopClicked(this, e);
        }

        internal bool HasJob(Job job)
        {
            return jobs.ContainsKey(job.Name);
        }

        private void queueListView_VisibleChanged(object sender, EventArgs e)
        {
            refreshQueue();
        }

        private void AbortMenuItem_Click(object sender, EventArgs e)
        {
            if (AllJobsHaveStatus(JobStatus.ABORTED) && AbortMenuItem.Checked) // set them back to waiting
            {
                foreach (ListViewItem item in queueListView.SelectedItems)
                {
                    jobs[item.Text].Status = JobStatus.WAITING;
                }
                refreshQueue();
            }
            else if (!AbortMenuItem.Checked)
                AbortClicked(this, e);
        }

        private void queueListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                deleteJobButton_Click(sender, e);
            if (e.KeyCode == Keys.Up)
                upButton_Click(sender, e);
            if (e.KeyCode == Keys.Down)
                downButton_Click(sender, e);
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Escape))
                startStopButton_Click(sender, e);
        }
    }

    class JobQueueEventArgs : EventArgs
    {
        private Job job;
        public Job ModifiedJob
        {
            get { return job; }
        }

        public JobQueueEventArgs(Job j)
        {
            this.job = j;
        }
    }

}
