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
using MeGUI.core.util;

namespace MeGUI
{
	/// <summary>
	/// Class that is used to send an encoding update from the encoder to the GUI
	/// it contains all the elements that will be updated in the GUI at some point
	/// </summary>
	public class StatusUpdate
	{
		private bool hasError, isComplete, wasAborted;
		private string error, log, jobName, audioPosition;
        private int nbFramesDone, nbFramesTotal;
        private JobTypes jobType;
		private long timeElapsed;
        private FileSize? filesize, audioFileSize, projectedFileSize;
		private decimal percentage;
        private double fps;
		public StatusUpdate()
		{
			hasError = false;
			isComplete = false;
			wasAborted = false;
			error = null;
			log = null;
			jobName = null;
			audioPosition = null;
			audioFileSize = null;
			nbFramesDone = 0;
			nbFramesTotal = 0;
			fps = 0;
			projectedFileSize = null;
			timeElapsed = 0;
			filesize = null;
		}
		/// <summary>
		/// does the job have any errors?
		/// </summary>
		public bool HasError
		{
			get {return hasError;}
			set {hasError = value;}
		}
		/// <summary>
		///  has the encoding job completed?
		/// </summary>
		public bool IsComplete
		{
			get {return isComplete;}
			set {isComplete = value;}
		}
		/// <summary>
		/// did we get this statusupdate because the job was aborted?
		/// </summary>
		public bool WasAborted
		{
			get {return wasAborted;}
			set {wasAborted = value;}
		}
		/// <summary>
		/// error message in case of an encoding error
		/// </summary>
		public string Error
		{
			get {return error;}
			set {error = value;}
		}
		/// <summary>
		/// gets / sets the contents of the log for this job
		/// </summary>
		public string Log
		{
			get {return log;}
			set {log = value;}
		}
		/// <summary>
		/// name of the job this statusupdate is refering to
		/// </summary>
		public string JobName
		{
			get {return jobName;}
			set {jobName = value;}
		}
		/// <summary>
		///  current audio encoding position in hh:mm:ss:ccc format
		/// </summary>
		public string AudioPosition
		{
			get {return audioPosition;}
			set {audioPosition = value;}
		}
		/// <summary>
		/// number of frames that have been encoded so far
		/// </summary>
		public int NbFramesDone
		{
			get {return nbFramesDone;}
			set {nbFramesDone = value;}
		}
		/// <summary>
		/// number of frames of the source
		/// </summary>
		public int NbFramesTotal
		{
			get {return nbFramesTotal;}
			set {nbFramesTotal = value;}
		}
		/// <summary>
		///  current encoding speed
		/// </summary>
		public double FPS
		{
			get {return fps;}
			set {fps = value;}
		}
		/// <summary>
		/// projected output size
		/// </summary>
		public FileSize? ProjectedFileSize
		{
			get {return projectedFileSize;}
			set {projectedFileSize = value;}
		}
        public int PercentageDone
        {
            get
            {
                return (int)PercentageDoneExact;
            }
        }
		/// <summary>
		/// gets / sets the exact percentage of the encoding progress
		/// </summary>
		public decimal PercentageDoneExact
		{
			get
			{
                return percentage;
			}
			set
			{
				this.percentage = value;
			}
		}
		/// <summary>
		/// size of the encoded file at this point
		/// </summary>
		public FileSize? FileSize
		{
			get {return filesize;}
			set {filesize = value;}
		}

		/// <summary>
		/// current size of the audio
		/// this field is filled when muxing and contains the current size of the audio data
		/// </summary>
		public FileSize? AudioFileSize
		{
			get {return audioFileSize;}
			set {audioFileSize = value;}
		}
		/// <summary>
		/// time elapsed between start of encoding and the point where this status update is being sent
		/// </summary>
		public long TimeElapsed
		{
			get {return timeElapsed;}
			set {timeElapsed = value;}
		}
		/// <summary>
		/// gets the elapsed time as a pretty string
		/// </summary>
		public string TimeElapsedString
		{
			get
			{
				TimeSpan t = new TimeSpan(timeElapsed);
                string retval = t.Days + ":";
                if (t.Hours < 10)
                    retval += "0";
                retval += t.Hours + ":";
				if (t.Minutes < 10)
					retval += "0";
				retval += t.Minutes + ":";
				if (t.Seconds < 10)
					retval += "0";
				retval += t.Seconds;
				return retval;
			}
		}
		/// <summary>
		/// the type of job this is, video, audio or mux
		/// </summary>
		public JobTypes JobType
		{
			get {return jobType;}
			set {jobType = value;}
		}
	}
}