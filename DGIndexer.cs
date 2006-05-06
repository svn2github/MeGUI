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
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using System.IO;

namespace MeGUI
{
	public delegate void IndexStatusUpdateCallback(StatusUpdate su);
	/// <summary>
	/// Summary description for MP4Muxer.
	/// </summary>
	public class DGIndexer : Encoder
	{
		#region variables
		private IndexJob job;
		private StatusUpdate stup;
		public event IndexStatusUpdateCallback StatusUpdate; // event to update the status in the GUI
		#endregion
		#region start / stop
		public DGIndexer(IndexJob job, string path)
		{
			this.job = job;
			this.path = path;
			hasStarted = false;
			log = new StringBuilder();
			stup = new StatusUpdate();
            stup.JobType = JobTypes.INDEX;
			stup.JobName = job.Name;
		}
		#endregion
		#region indexing
		/// <summary>
		/// sets up the encoder
		/// generates the commandline from the job, then sets up the process
		/// </summary>
		/// <returns>boolean indicating whether we're ready for encoding or not</returns>
		public override bool setup()
		{
			return prepareProcess();
		}
		/// <summary>
		/// prepares the indexing process by creating the commandline required
		/// </summary>
		/// <returns>true if we're ready to go, false if we're missing some files</returns>
		public override bool prepareProcess()
		{
			int endpos = job.Commandline.ToLower().IndexOf("dgindex.exe") + 10;
			string executablePath = job.Commandline.Substring(1, endpos);
			string arguments = job.Commandline.Substring(executablePath.Length + 3);
			if (!File.Exists(executablePath))
			{
				if (!File.Exists(path + @"\dgindex.exe"))
				{
					MessageBox.Show("dgindex.exe not found.\r\n Please configure its location in the Settings tab.", "Required program not found", 
						MessageBoxButtons.OK);
					return false;
				}
				else
					executablePath = path + @"\dgindex.exe";
			}
			proc = new Process();
			ProcessStartInfo pstart = new ProcessStartInfo();
			pstart.FileName = executablePath;
			pstart.Arguments = arguments;
			pstart.UseShellExecute = true;
			pstart.WindowStyle = ProcessWindowStyle.Minimized;
			pstart.UseShellExecute = true;
			pstart.RedirectStandardOutput = false;
			proc.StartInfo = pstart;
			proc.EnableRaisingEvents = true;
			proc.Exited += new EventHandler(proc_Exited);
			return true;
		}
		/// <summary>
		/// performs the actual muxing process and returs all the output from mp4box
		/// </summary>
		/// <returns>all stdout output from mp4box</returns>
		public override void encode()
		{
			job.Start = DateTime.Now;
			bool started = false;
			try
			{
				started = proc.Start();
				if (!started)
				{
					stup.HasError = true;
					stup.Error = "dgindex could not be started";
					Trace.WriteLine("dgindex could not be started");
				}
				else
				{
					if (job.Priority == ProcessPriority.IDLE)
						proc.PriorityClass = ProcessPriorityClass.Idle;
					else if (job.Priority == ProcessPriority.HIGH)
						proc.PriorityClass = ProcessPriorityClass.High;
				}
			}
			catch (Exception f) // if we get here, the process aborted
			{
				stup.HasError = true;
				stup.Error = f.Message;
				log.Append("DGIndex processing aborted with an exception : " + f.Message + "\r\n");
				log.Append("DGIndex arguments: path: " + proc.StartInfo.FileName + " arguments : " 
					+ proc.StartInfo.Arguments);
				job.End = DateTime.Now;
				stup.IsComplete = true;
				stup.HasError = true;
				if (this.aborted)
					stup.WasAborted = true;
				stup.Log = log.ToString();
				if (!started) // in this case, process.exited is never fired so we have to fire the stup here
					StatusUpdate(stup);
			}
		}

		/// <summary>
		/// handles the dgindex process exiting
		/// if there was no error and auto-force-film is active, the file is
		/// analyzed and if the force film treshold is met, force film is automatically applied
		/// then the call back function is called to inform the launcher of the dgindex process
		/// that is has completed, and if or not the call has been successful
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void proc_Exited(object sender, EventArgs e)
		{
			stup.IsComplete = true;
			if (!stup.HasError)
				Trace.WriteLine("DGIndex exited after successfully indexing");
			else
				Trace.WriteLine("DGIndex process has exited");
			if (job.AutoForceFilm && !stup.HasError && !stup.WasAborted && !aborted)
			{
				d2vReader reader = null;
                try
                {
                    reader = new d2vReader(job.Output);
                }
                catch (Exception error)
                {
                    stup.Error = "Applying force film failed. Consult the log for details";
                    log.AppendFormat("Cannot open d2v file, '{0}'. Error message for your reference: {1}{2}", job.Output, error.Message, Environment.NewLine);
                    log.AppendLine("Applying force film failed.");
                    stup.HasError = true;
                }
                if (!stup.HasError)
                {
                    reader.closeD2V();
                    reader.Close();
                    Trace.WriteLine("Successfully opened and closed d2v to get the film percentage");
                    if (job.ForceFilmThreshold <= reader.FilmPercentage)
                    {
                        log.Append("Film percentage: " + reader.FilmPercentage + " meets force film treshold");
                        bool success = applyForceFilm(job.Output);
                        if (success)
                            log.Append("Successfully applied force film");
                        else
                        {
                            stup.Error = "Applying force film failed";
                            log.Append("Applying force film failed");
                            stup.HasError = true;
                        }
                    }
                }
			}
            stup.Log = log.ToString();
			StatusUpdate(stup);
		}


		/// <summary>
		/// opens a DGIndex project file and applies force film to it
		/// </summary>
		/// <param name="fileName">the dgindex project where force film is to be applied</param>
		private bool applyForceFilm(string fileName)
		{
            try
            {
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.IndexOf("Field_Operation") != -1) // this is the line we have to replace
                            sb.Append("Field_Operation=1" + Environment.NewLine);
                        else if (line.IndexOf("Frame_Rate") != -1)
                        {
                            if (line.IndexOf("/") != -1) // If it has a slash, it means the framerate is signalled as a fraction, like below
                                sb.Append("Frame_Rate=23976 (24000/1001)" + Environment.NewLine);
                            else // If it doesn't, then it doesn't
                                sb.Append("Frame_Rate=23976" + Environment.NewLine);
                        }
                        else
                        {
                            sb.Append(line);
                            sb.Append(Environment.NewLine);
                        }
                    }
                }
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write(sb.ToString());
                }
                return true;
			}
			catch (Exception e)
			{
				log.Append("Exception in applyForceFilm: " + e.Message);
				return false;
			}
		}

		#endregion
	}
}