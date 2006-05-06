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
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Text;

namespace MeGUI
{
	public delegate void StatusUpdateCallback(StatusUpdate su);
	/// <summary>
	/// superclass handling all the common encoder task
	/// </summary>
	public class Encoder
	{
		public string path; // path the program was launched from
		public StringBuilder log; // used to build the log
		public Thread thread, encoderThread; // threads to read data
		public Process proc; // the acutal encoding system process
		public bool hasStarted = false, aborted = false, error = false;
        protected ManualResetEvent mre = new ManualResetEvent(true);
		//public event StatusUpdateCallback StatusUpdate; // event to update the status in the GUI
		/// <summary>
		/// default constructor
		/// </summary>
		public Encoder(){}
		/// <summary>
		/// sets up the encoder
		/// </summary>
		/// <returns>boolean indicating whether we're ready for encoding or not</returns>
		public virtual bool setup(){return true;}
		/// <summary>
		///  prepares the actual encoding process
		/// </summary>
		/// <returns>true if we're ready, false if something has gone wrong (a program is missing)</returns>
		public virtual bool prepareProcess(){return true;}
		/// <summary>
		/// starts encoding
		/// launches a thread that does the actual encoding and starts the thread
		/// </summary>
		public void start()
		{
			encoderThread = new Thread(new ThreadStart(this.encode));
			encoderThread.Start();
		}
		/// <summary>
		///  stops encoding
		///  if the process is still running, the process is being killed
		///  and if the thread reading from stderr is still running, it is aborted as well
		/// </summary>
		public void stop()
		{
			aborted = true;
			try
			{
				if (!proc.HasExited) // process is still running
				{
					proc.Kill();
				}
			}
			catch (Exception e)
			{
				log.Append("exception in Encoder.stop: " + e.Message + " stacktrace: " + e.StackTrace);
			}
		}
		/// <summary>
		/// pauses encoding
		/// </summary>
		public void pause()
		{
            mre.Reset();
		}
		/// <summary>
		/// resumes encoding
		/// </summary>
		public void resume()
		{
            mre.Set();
		}
		/// <summary>
		/// changes the priority of the encoding job
		/// </summary>
		/// <param name="priority">the new process priority</param>
        public void changePriority(ProcessPriority priority)
		{
			if (this.hasStarted)
			{
				try
				{
					if (priority == ProcessPriority.IDLE)
						proc.PriorityClass = ProcessPriorityClass.Idle;
                    else if (priority == ProcessPriority.NORMAL)
						proc.PriorityClass = ProcessPriorityClass.Normal;
                    else if (priority == ProcessPriority.HIGH)
						proc.PriorityClass = ProcessPriorityClass.High;
				}
				catch (Exception e) // mp4box could not be running anymore
				{
					log.Append("exception in Encoder.changePriority: " + e.Message);
				}
			}
		}
		/// <summary>
		/// reads data from the standarderror of the encoder process
		/// mencoder mixes info and error messages
		/// in stderr so it's important to read this data as well
		/// everything read is added to the log
		/// </summary>
		public void readFromStdErr()
		{
			StreamReader sr = null;
			try
			{
				sr = proc.StandardError;
			}
			catch (Exception e)
			{
				log.Append("exception geting io reader for stderr: " + e.Message + "\r\nAborting Encoder.readFromStdErr");
			}
			if (sr != null)
			{
				string line;
				while (!proc.HasExited)
				{
					try
					{
                        mre.WaitOne();
						line = sr.ReadLine();
						if (line != null)
						{
							if (line.IndexOf("dezicycles") == -1 && line.IndexOf("qp<=0.0") == -1 && line.IndexOf("frameno.avi") == -1) // workaround for tons of weird snow lines
							{
								log.Append(line + "\r\n");
							}
						}
					}
					catch (Exception e)
					{
						log.Append("exception in readFromStdErr: " + e.Message + " stacktrace: " + e.StackTrace);
					}
				}
			}
		}
		/// <summary>
		/// reads data from the stdout from the running process
		/// </summary>
		public void readFromStdOut()
		{
			StreamReader sr = null;
			try
			{
				sr = proc.StandardOutput;
			}
			catch (Exception e)
			{
				log.Append("exception geting io reader for stdout: " + e.Message + "\r\nAborting Encoder.readFromStdOut");
			}
			if (sr != null)
			{
				string line;
				while (!proc.HasExited)
				{
					try
					{
                        mre.WaitOne();
						line = sr.ReadLine();
						if (line != null)
						{
							log.Append(line + "\r\n");
						}
					}
					catch (Exception e)
					{
						log.Append("exception in readFromStdErr: " + e.Message + " stacktrace: " + e.StackTrace);
					}
				}
			}
		}
		/// <summary>
		/// actual encoding method. 
		/// </summary>
		public virtual void encode(){}
	}
}