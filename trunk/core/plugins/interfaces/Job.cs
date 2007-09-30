// ****************************************************************************
// 
// Copyright (C) 2005  Doom9
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
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using MeGUI.core.plugins.interfaces;
using MeGUI.packages.tools.besplitter;
using MeGUI.core.util;
using System.Diagnostics;

namespace MeGUI
{
    public class JobID
    {
        private static readonly Random gen = new Random();

        private string name;
        public string Name
        {
            get { return name; }
        }
        private int uniqueID;
        public int UniqueID
        {
            get { return uniqueID; }
        }

        public JobID(string name)
        {
            for (int i = 0; i < name.Length; i++)
                if (!(Char.IsLetterOrDigit(name[i]) || name[i] == '_'))
                    throw new MeGUIException("The name must be alphanumeric, including underscores.");
            
            this.name = name;
            this.uniqueID = gen.Next();
        }

        public override string ToString()
        {
            return name + " " + uniqueID.ToString();
        }

        public override bool Equals(object obj)
        {
            JobID other = obj as JobID;
            if (other == null) return false;

            return (name == other.name && uniqueID == other.uniqueID);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

	public enum ProcessPriority: int {IDLE=0, NORMAL, HIGH};
	public enum JobTypes: int {VIDEO=0, AUDIO, MUX, MERGE, INDEX, AVS, VOBSUB, CUT};
    public enum JobStatus: int {WAITING = 0, PROCESSING, POSTPONED, ERROR, ABORTED, DONE, SKIP };
    // status of job, 0: waiting, 1: processing, 2: postponed, 3: error, 4: aborted, 5: done
	/// <summary>
	/// This class contains all the information required for a job
	/// A job is a collection of all the settings required to start an encoding session
	/// </summary>
	[XmlInclude(typeof(VideoJob)), XmlInclude(typeof(AudioJob)), XmlInclude(typeof(MuxJob)), 
	XmlInclude (typeof(SubStream)), XmlInclude(typeof(IndexJob)), XmlInclude(typeof(AviSynthJob)), 
    XmlInclude(typeof(SubtitleIndexJob)), XmlInclude(typeof(AudioSplitJob)), XmlInclude(typeof(AudioJoinJob))]
	public abstract class Job
	{
        private ProcessPriority priority;
        private JobStatus status;
		private DateTime start, end; // time the job was started / ended
		private string input, output, name; //name of the input and output for this job
        private Job next, previous;
        private string commandline;
        private List<string> filesToDelete = new List<string>();
        
        private string owningWorker;

        public string OwningWorker
        {
            get { return owningWorker; }
            set { owningWorker = value; }
        }


        private List<string> requiredJobNames;


        public void AddDependency(Job other)
        {
            // we can't have each job depending on the other
            Debug.Assert(!other.RequiredJobs.Contains(this));
            RequiredJobs.Add(other);
            other.EnabledJobs.Add(this);
        }

        /// <summary>
        /// List of jobs which need to be completed before this can be processed
        /// </summary>
        public List<string> RequiredJobNames
        {
            get { return requiredJobNames; }
            set { requiredJobNames = value; }
        }

        private List<string> enabledJobNames;
        /// <summary>
        /// List of jobs which completing this job enables
        /// </summary>
        public List<string> EnabledJobNames
        {
            get { return enabledJobNames; }
            set { enabledJobNames = value; }
        }

        [XmlIgnore]
        public List<Job> EnabledJobs = new List<Job>();

        [XmlIgnore]
        public List<Job> RequiredJobs = new List<Job>();

        /// <summary>
        /// List of files to delete when this job is successfullly completed.
        /// </summary>
        public List<string> FilesToDelete
        {
            get { return filesToDelete; }
            set { filesToDelete = value; }
        }

		/// <summary>
		/// standard constructor, initializes the status to waiting and its position in the GUI to 1 million
		/// Note that if you have more than 1 million jobs, the order in which jobs are displayed in the GUI after a program
		/// start will not match the status they had prior to exiting the program
		/// </summary>
		public Job()
		{
            status = JobStatus.WAITING;
			input = "";
			output = "";
			next = null;
			previous = null;
			commandline = "";
		}

        public abstract string CodecString
        {
            get;
        }

        public abstract string EncodingMode
        {
            get;
        }

        public abstract JobTypes JobType
        {
            get;
        }

        public string EncodingSpeed = "";
	
        /// <summary>
        /// the source file for this job
        /// </summary>
		public string Input
		{
			get {return input;}
			set {input = value;}
		}
        /// <summary>
        /// the output of this job
        /// </summary>
		public string Output
		{
			get {return output;}
			set {output = value;}
		}
        /// <summary>
        /// the name of this job
        /// </summary>
		public string Name
		{
			get {return name;}
			set {name = value;}
		}
		/// <summary>
		/// status of the job
		/// </summary>
		public JobStatus Status
		{
			get {return status;}
			set {status = value;}
		}
		/// <summary>
		/// time the job was started
		/// </summary>
		public DateTime Start
		{
			get {return start;}
			set {start = value;}
		}
		/// <summary>
		///  time the job was completed
		/// </summary>
		public DateTime End
		{
			get {return end;}
			set {end = value;}
		}
        private string nextJobName;

        public string NextJobName
        {
            get { return nextJobName; }
            set { nextJobName = value; }
        }
        private string previousJobName;

        public string PreviousJobName
        {
            get { return previousJobName; }
            set { previousJobName = value; }
        }

		/// <summary>
		/// commandline for this job
		/// </summary>
		public string Commandline
		{
			get {return commandline;}
			set {commandline = value;}
		}
		/// <summary>
		/// gets a humanly readable status tring
		/// </summary>
		public string StatusString
		{
			get
			{
                switch (status)
                {
                    case JobStatus.WAITING:
                        return "waiting";
                    case JobStatus.PROCESSING:
                        return "processing";
                    case JobStatus.POSTPONED:
                        return "postponed";
                    case JobStatus.ERROR:
                        return "error";
                    case JobStatus.ABORTED:
                        return "aborted";
                    case JobStatus.DONE:
                        return "done"; 
                    case JobStatus.SKIP:
                        return "skip";
                    default:
                        return "";
                }
			}
		}
		/// <summary>
		/// filename without path of the source for this job
		/// </summary>
		public string InputFileName
		{
			get
			{
                return System.IO.Path.GetFileName(this.input);
			}
		}
		/// <summary>
		///  filename without path of the destination of this job
		/// </summary>
		public string OutputFileName
		{
			get 
			{
                return System.IO.Path.GetFileName(this.output);
			}
		}
	}
}
