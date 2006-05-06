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
	/// <summary>
	/// Summary description for VideoEncoder.
	/// </summary>
    public class VideoEncoder : IVideoEncoder
	{
        protected MeGUISettings settings;
        protected VideoJob job;
        protected StatusUpdate su;
        protected int numberOfFrames = 0;
        protected StringBuilder log; // holds logging information
        private VideoEncoder encoder;

		public VideoEncoder(MeGUISettings settings)
		{
            this.settings = settings;
		}
        /// <summary>
        ///  dummy default constructor so that the derived video encoders that have no constructor with the settings can still be used
        /// </summary>
        public VideoEncoder()
        { 
        }

        protected void sendStatusUpdateToGUI(StatusUpdate su)
        {
            su.PercentageDoneExact = PercentageComplete;
            su.TimeElapsed = DateTime.Now.Ticks - job.Start.Ticks;
            if (File.Exists(job.Output))
            {
                FileInfo fi = new FileInfo(job.Output);
                su.FileSize = fi.Length / 1024;
            }
            if (statusUpdate != null)
                statusUpdate(su);
        }

        protected double PercentageComplete
        {
            get
            {
                double div = (double)su.NbFramesDone / (double)su.NbFramesTotal;
                double percentage = ((double)100 * div);
                return percentage;
            }
        }

        #region IVideoEncoder Members

        public virtual bool setup(VideoJob job, out string error)
        {
            if (job.Settings is x264Settings)
                encoder = new x264Encoder(settings.X264Path);
            else if (job.Settings is xvidSettings)
                encoder = new XviDEncoder(settings.XviDEncrawPath);
            else
                encoder = new mencoderEncoder(settings.MencoderPath);
            error = null;
            encoder.settings = settings;
            return encoder.setup(job, out error);
        }

        public virtual bool start(out string error)
        {
            error = null;
            encoder.job.Start = DateTime.Now;
            return encoder.start(out error);
        }

        public virtual bool stop(out string error)
        {
            error = null;
            return encoder.stop(out error);
        }

        public virtual bool pause(out string error)
        {
            error = null;
            return encoder.pause(out error);
        }

        public virtual bool resume(out string error)
        {
            error = null;
            return encoder.resume(out error);
        }

        public virtual bool changePriority(ProcessPriority priority, out string error)
        {
            error = null;
            return encoder.changePriority(priority, out error);
        }

        private event VideoEncodingStatusUpdateCallback statusUpdate;
        public event VideoEncodingStatusUpdateCallback StatusUpdate
        {
            add
            {
                encoder.statusUpdate += value;
            }
            remove
            {
                encoder.statusUpdate -= value;
            }
        }
        #endregion
    }
}