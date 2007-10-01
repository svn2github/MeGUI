using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using MeGUI.core.util;
using MeGUI.core.gui;
using System.Xml.Serialization;

namespace MeGUI.core.details
{
    public partial class JobControl : UserControl
    {
        private Dictionary<string, TaggedJob> allJobs = new Dictionary<string, TaggedJob>(); //storage for all the jobs and profiles known to the system
        private Dictionary<string, JobWorker> workers = new Dictionary<string, JobWorker>();
        private MainForm mainForm;
        private WorkerSummary summary;

        #region public interface: process windows, start/stop/abort
        public void ShowAllProcessWindows()
        {
            foreach (JobWorker w in workers.Values)
                if (w.IsProgressWindowAvailable) w.ShowProcessWindow();
        }


        public void HideAllProcessWindows()
        {
            foreach (JobWorker w in workers.Values)
                w.HideProcessWindow();
        }

        public void AbortAll()
        {
            foreach (JobWorker worker in workers.Values)
                if (worker.IsEncoding) worker.Abort();
            refresh();
        }

        public void StartAll(bool restartStopping)
        {
            if (workers.Values.Count == 0)
            {
                DialogResult r = MessageBox.Show("Can't start queue because there are no workers. You can create one from the Workers menu. Do you want to create one now?",
                    "Create new worker?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r == DialogResult.OK)
                    RequestNewWorker();
            }
            foreach (JobWorker w in workers.Values)
                if (!w.IsEncoding) w.StartEncoding(false);
                else if (restartStopping && w.Status == JobWorkerStatus.Stopping) w.SetRunning();
            refresh();
        }

        public void StopAll()
        {
            foreach (JobWorker w in workers.Values)
                if (w.IsEncoding) w.SetStopping();
            refresh();
        }
        #endregion

        public JobControl()
        {
            InitializeComponent();
            addClearButton();
            addSendToWorkerMenuItems();
            addSendToTemporaryWorkerMenuItem();
            jobQueue.RequestJobDeleted = new RequestJobDeleted(this.DeleteJob);
            summary = new WorkerSummary(this);
        }

        private void addSendToTemporaryWorkerMenuItem()
        {
            jobQueue.AddMenuItem("Run in new temporary worker", null, new MultiJobHandler(
                delegate(List<TaggedJob> jobs)
                {
                    // find a good name
                    int number = 0;
                    string name;
                    do
                    {
                        number++;
                        name = "Temporary worker " + number;
                    } while (workers.ContainsKey(name));
                    JobWorker w = NewWorker(name, false);

                    foreach (TaggedJob j in jobs)
                    {
                        ReleaseJob(j);
                        w.AddJob(j);
                    }
                    this.refresh();
                    w.Mode = JobWorkerMode.CloseOnLocalListCompleted;
                    w.StartEncoding(true);
                }));
        }

        internal void ReleaseJob(TaggedJob j)
        {
            if (j.OwningWorker == null)
                return;

            workers[j.OwningWorker].RemoveJobFromQueue(j);
            j.OwningWorker = null;
            if (!jobQueue.HasJob(j))
                jobQueue.enqueueJob(j);
            refresh();
        }

        class SendToWorkerThunk
        {
            JobControl c;
            JobWorker w;
            public void handleEvent(List<TaggedJob> jobs)
            {
                foreach (TaggedJob j in jobs)
                {
                    if (j.Status == JobStatus.PROCESSING)
                    {
                        MessageBox.Show("Can't move '" + j.Name + "' because it is currently processing.", "Can't move job", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }
                    if (j.OwningWorker == w.Name)
                        continue;

                    c.ReleaseJob(j);

                    w.AddJob(j);
                    j.OwningWorker = w.Name;
                }
                c.refresh();
            }

            public SendToWorkerThunk(JobWorker w, JobControl c)
            {
                this.c = c;
                this.w = w;
            }
        }

        private void addSendToWorkerMenuItems()
        {
            jobQueue.AddDynamicSubMenu("Send to worker", null,
                new MultiJobMenuGenerator(delegate
            {
                Pair<string, MultiJobHandler>[] list = new Pair<string, MultiJobHandler>[workers.Count];
                int i = 0;
                foreach (JobWorker w in workers.Values)
                {
                    list[i] = new Pair<string, MultiJobHandler>(w.Name,
                        new MultiJobHandler((new SendToWorkerThunk(w, this)).handleEvent));
                    i++;
                }
                return list;
            }));
        }
                

        #region properties
        public MainForm MainForm
        {
            set
            {
                mainForm = value; 
                mainForm.RegisterForm(summary);
            }
        }

        public bool IsAnyWorkerEncoding
        {
            get
            {
                foreach (JobWorker w in workers.Values)
                    if (w.IsEncoding) return true;
                return false;
            }
        }
        #endregion

        #region Clear button
        private void addClearButton()
        {
            jobQueue.AddButton("Clear", new EventHandler(deleteAllJobsButton_Click));
        }

        private void deleteAllJobsButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to clear the queue? This will delete all jobs in all workers.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                TaggedJob[] jobList = new TaggedJob[allJobs.Count];
                allJobs.Values.CopyTo(jobList, 0);
                foreach (TaggedJob j in jobList)
                    reallyDeleteJob(j);
            }
        }
        #endregion
        #region deleting jobs
        internal void DeleteJob(TaggedJob job)
        {
            if (job.Status == JobStatus.PROCESSING)
            {
                MessageBox.Show("You cannot delete a job while it is being processed.", "Deleting job failed", MessageBoxButtons.OK);
                return;
            }

            if (job.EnabledJobs.Count == 0)
            {
                reallyDeleteJob(job);
                return;
            }

            DialogResult dr = MessageBox.Show("Some jobs depend on the job, '" + job.Name +
                "' being completed for them to run. Do you want to delete all dependant jobs? " +
                "Press Yes to delete all dependant jobs, No to delete this job and " +
                "remove the dependencies, or Cancel to abort",
                "Job dependency detected",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            switch (dr)
            {
                case DialogResult.Yes: // Delete all dependent jobs
                    deleteAllDependantJobs(job);
                    break;

                case DialogResult.No: // Just delete the single job
                    reallyDeleteJob(job);
                    break;

                case DialogResult.Cancel: // do nothing
                    break;
            }
        }
        /// <summary>
        /// removes this job, and any previous jobs that belong to a series of jobs from the
        /// queue, then update the queue positions
        /// </summary>
        /// <param name="job">the job to be removed</param>
        internal void RemoveCompletedJob(TaggedJob job)
        {
            reallyDeleteJob(job);
        }

        private void reallyDeleteJob(TaggedJob job)
        {
            if (job.Status == JobStatus.PROCESSING) return;

            if (job.OwningWorker != null)
                workers[job.OwningWorker].RemoveJobFromQueue(job);

            if (jobQueue.HasJob(job))
                jobQueue.removeJobFromQueue(job);

            foreach (TaggedJob j in job.EnabledJobs)
                j.RequiredJobs.Remove(job);

            string fileName = mainForm.MeGUIPath + "\\jobs\\" + job.Name + ".xml";
            if (File.Exists(fileName))
                File.Delete(fileName);
            
            allJobs.Remove(job.Name);
        }

        private void deleteAllDependantJobs(TaggedJob job)
        {
            reallyDeleteJob(job);

            foreach (TaggedJob j in job.EnabledJobs)
                deleteAllDependantJobs(j);
        }
        #endregion

        public void refresh()
        {
            jobQueue.refreshQueue();
            summary.RefreshInfo();
        }

        #region saving / loading jobs
        internal List<string> toStringList(IEnumerable<TaggedJob> jobList)
        {
            List<string> strings = new List<string>();
            foreach (TaggedJob j in jobList)
                strings.Add(j.Name);
            return strings;
        }
        /// <summary>
        /// saves all the jobs in the queue
        /// </summary>
        public void saveJobs()
        {
            foreach (TaggedJob job in allJobs.Values)
            {
                job.EnabledJobNames = toStringList(job.EnabledJobs);
                job.RequiredJobNames = toStringList(job.RequiredJobs);
                saveJob(job, mainForm.MeGUIPath);
            }

            saveJobLists();
        }

        public class JobListSerializer
        {
            public List<string> mainJobList = new List<string>();
            public List<Pair<string, List<string>>> workersAndTheirJobLists = new List<Pair<string,List<string>>>(); 
        }

        private void saveJobLists()
        {
            JobListSerializer s = new JobListSerializer();

            s.mainJobList = toStringList(jobQueue.JobList);

            foreach (JobWorker w in workers.Values)
                s.workersAndTheirJobLists.Add(new Pair<string, List<string>>(
                    w.Name,
                    toStringList(w.Jobs)));
            string path = Path.Combine(mainForm.MeGUIPath, "joblists.xml");

            Util.XmlSerialize(s, path);
        }

        private void loadJobLists()
        {
            string path = Path.Combine(mainForm.MeGUIPath, "joblists.xml");

            JobListSerializer s = Util.XmlDeserialize<JobListSerializer>(path);
            jobQueue.JobList = toJobList(s.mainJobList);

            foreach (Pair<string, List<string>> p in s.workersAndTheirJobLists)
            {
                JobWorker w = NewWorker(p.fst, false);
                IEnumerable<TaggedJob> list = toJobList(p.snd);
                w.Jobs = list;
                foreach (TaggedJob j in list)
                    j.OwningWorker = w.Name;
            }
        }

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
                TaggedJob job = loadJob(fileName);
                if (job != null)
                {
                    if (allJobs.ContainsKey(job.Name))
                        MessageBox.Show("A job named " + job.Name + " is already in the queue.\nThe job defined in " + fileName + "\nwill be discarded", "Duplicate job name detected",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        allJobs.Add(job.Name, job);
                }
            }

            foreach (TaggedJob job in allJobs.Values)
            {
                job.RequiredJobs = toJobList(job.RequiredJobNames);
                job.EnabledJobs = toJobList(job.EnabledJobNames);
            }
            loadJobLists();
        }

        internal List<TaggedJob> toJobList(IEnumerable<string> list)
        {
            List<TaggedJob> jobList = new List<TaggedJob>();
            foreach (string name in list)
            {
                try
                {
                    jobList.Add(allJobs[name]);
                }
                catch (KeyNotFoundException) { }
            }
            return jobList;
        }

        #region individual job saving and loading
        #region loading and saving jobs
        /// <summary>
        /// saves a job to programdirectory\jobs\jobname.xml
        /// using the XML Serializer we get a humanly readable file
        /// </summary>
        /// <param name="job">the Job object to be saved</param>
        /// <param name="path">The path where the program was launched from</param>
        internal void saveJob(TaggedJob job, string path)
        {
            string fileName = path + "\\jobs\\" + job.Name + ".xml";
            Util.XmlSerialize(job, fileName);
        }
        /// <summary>
        /// loads a job with a given name from programdirectory\jobs\jobname.xml
        /// </summary>
        /// <param name="name">name of the job to be loaded (corresponds to the filename)</param>
        /// <returns>the Job object that was read from the harddisk</returns>
        internal TaggedJob loadJob(string name)
        {
            XmlSerializer ser = null;
            using (Stream s = File.OpenRead(name))
            {
                try
                {
                    ser = new XmlSerializer(typeof(TaggedJob));
                    return (TaggedJob)ser.Deserialize(s);
                }
                catch (Exception e)
                {
                    DialogResult r = MessageBox.Show("Job " + name + " could not be loaded. Delete?", "Error loading Job", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (r == DialogResult.Yes)
                    {
                        try { s.Close(); File.Delete(name); }
                        catch (Exception) { }
                    }
                    Console.Write(e.Message);
                    return null;
                }
            }
        }
                #endregion
        #endregion
        #endregion
        #region free job name
        /// <summary>
        /// looks up the first free job number
        /// </summary>
        /// <returns>the job number that can be attributed to the next job to be added to the queue</returns>
        private string getFreeJobName()
        {
            int jobNr = 1;
            while (true)
            {
                string name = "job" + jobNr;
                bool found = false;
                foreach (string jobName in allJobs.Keys)
                    if (jobName.StartsWith(name))
                    {
                        found = true;
                        break;
                    }
                if (!found) return name;
                jobNr++;
            }
        }
        #endregion
        
        #region adding jobs to queue
        public void addJobsWithDependencies(JobChain c)
        {
            throw new Exception();
        }

        /// <summary>
        /// adds a job to the Queue (Hashtable) and the listview for graphical display
        /// </summary>
        /// <param name="job">the Job to be added to the next free spot in the queue</param>
        public void addJobsToQueue(params Job[] jobs)
        {
            foreach (Job j in jobs)
                addJob(new TaggedJob(j));
            saveJobs();
            if (mainForm.Settings.AutoStartQueue)
                StartAll(false);
            refresh();
        }

        private void addJob(TaggedJob job)
        {
            job.Name = getFreeJobName();
            allJobs[job.Name] = job;
            jobQueue.enqueueJob(job);
        }
        #endregion

        private void updateProcessingStatus()
        {
            throw new Exception();
            /*
            if (this.isEncoding)
            {
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
            }*/
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


        /// <summary>
        /// Returns the job under the given name
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal TaggedJob ByName(string p)
        {
            return allJobs[p];
        }

        /// <summary>
        /// Returns whether all the jobs that j depends on have been successfully completed
        /// </summary>
        /// <param name="j"></param>
        /// <returns></returns>
        internal bool areDependenciesMet(TaggedJob j)
        {
            foreach (TaggedJob job in j.RequiredJobs)
                if (job.Status != JobStatus.DONE)
                    return false;

            return true;
        }

        /// <summary>
        /// Returns the first job on the queue whose dependencies have been met, whose status
        /// is set to 'waiting', and which isn't owned by any JobWorkers
        /// </summary>
        /// <returns></returns>
        internal TaggedJob getJobToProcess()
        {
            foreach (TaggedJob job in jobQueue.JobList)
            {
                if (job.Status == JobStatus.WAITING &&
                    job.OwningWorker == null &&
                    areDependenciesMet(job))
                    return job;
            }
            return null;
        }

        internal void RenameWorker(string name, string value)
        {
            if (workers.ContainsKey(value))
                throw new NameTakenException(value);

            JobWorker w = workers[name];
            w.Name = value;
            workers.Remove(name);
            workers[value] = w;
            summary.Rename(name, value);

            foreach (TaggedJob job in allJobs.Values)
            {
                if (name == job.OwningWorker)
                    job.OwningWorker = value;
            }

        }

        private void jobQueue_StartClicked(object sender, EventArgs e)
        {
            StartAll(true);
        }

        private void jobQueue_StopClicked(object sender, EventArgs e)
        {
            StopAll();
        }

        private void jobQueue_AbortClicked(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Do you really want to abort all jobs?", "Really abort?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes)
                AbortAll();
        }


        private string freeWorkerName()
        {
            int num = 0;
            string name;
            do {
                num++;
                name = "Worker " + num;
            } while (workers.ContainsKey(name));

            return name;
        }

        internal List<Pair<string, bool>> ListWorkers()
        {
            List<Pair<string, bool>> ans = new List<Pair<string,bool>>();
            
            foreach (JobWorker w in workers.Values)
            {
                ans.Add(new Pair<string,bool>(w.Name, w.Visible));
            }

            return ans;
        }

        internal void HideAllWorkers()
        {
            foreach (JobWorker w in workers.Values)
                w.Hide();
        }

        internal void ShowAllWorkers()
        {
            foreach (JobWorker w in workers.Values)
                w.Show();
        }

        internal void RequestNewWorker()
        {
            string name = 
                Microsoft.VisualBasic.Interaction.InputBox(
                "Please enter a name for this new worker", "Please enter a name",
                freeWorkerName(), -1, -1);
            if (string.IsNullOrEmpty(name)) return;
            if (workers.ContainsKey(name))
            {
                MessageBox.Show("A worker by this name already exists. Adding worker failed", "Adding worker failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NewWorker(name, true);
        }

        private JobWorker NewWorker(string name, bool show)
        {
            Debug.Assert(!workers.ContainsKey(name));

            JobWorker w = new JobWorker(mainForm);
            w.Name = name;
            workers.Add(w.Name, w);
            summary.Add(w);
            mainForm.RegisterForm(w);
            if (show) w.Show();

            return w;
        }

        internal void ShowSummary()
        {
            summary.Show();
        }

        internal void HideSummary()
        {
            summary.Hide();
        }

        public bool SummaryVisible
        {
            get { return summary.Visible; }
        }

        internal void SetWorkerVisible(string p, bool p_2)
        {
            if (p_2)
                workers[p].Show();
            else
                workers[p].Hide();
        }

        private void newWorkerButton_Click(object sender, EventArgs e)
        {
            RequestNewWorker();
        }

        internal void UpdateProgress(string name)
        {
            summary.RefreshInfo(name);
        }

        internal void ShutDown(JobWorker w)
        {
            workers.Remove(w.Name);
            if (w.Visible) w.Close();
            summary.Remove(w.Name);
        }

        internal List<Pair<string, bool>> ListProgressWindows()
        {
            List<Pair<string, bool>> ans = new List<Pair<string,bool>>();
            foreach (JobWorker w in workers.Values)
            {
                if (w.IsProgressWindowAvailable)
                    ans.Add(new Pair<string, bool>(w.Name, w.IsProgressWindowVisible));
            }
            return ans;
        }

        internal void HideProgressWindow(string p)
        {
            if (workers[p].IsProgressWindowAvailable)
                workers[p].HideProcessWindow();
        }

        internal void ShowProgressWindow(string p)
        {
            if (workers[p].IsProgressWindowAvailable)
                workers[p].ShowProcessWindow();
        }
    }
    enum JobStartInfo { JOB_STARTED, NO_JOBS_WAITING, COULDNT_START }
}
