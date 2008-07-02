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
/*using System;
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
    public class VideoEncoder : IJobProcessor
	{
        public static readonly JobProcessorFactory Factory =
            new JobProcessorFactory(new ProcessorFactory(init), "VideoEncoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is VideoJob) return new VideoEncoder(mf.Settings);
            return null;
        }
        
        protected MeGUISettings settings;
        protected VideoJob job;
        protected StatusUpdate su;
        protected int numberOfFrames = 0;
        protected StringBuilder log; // holds logging information
        private IJobProcessor encoder;

		public VideoEncoder(MeGUISettings settings)
		{
            this.settings = settings;
		}

        #region IVideoEncoder Members

        public virtual void setup(Job job)
        {
            if (!(job is VideoJob))
                throw new Exception("Setup was called on a non-video job");
            VideoJob vJob = (VideoJob)job;
            if (vJob.Settings is x264Settings)
                encoder = new x264Encoder(settings.X264Path);
            else if (vJob.Settings is xvidSettings)
                encoder = new XviDEncoder(settings.XviDEncrawPath);
            else if (vJob.Settings is x264farmSettings)
                encoder = new x264farmEncoder(settings.X264farmControllerPath);
            else 
                encoder = new mencoderEncoder(settings.MencoderPath);

            encoder.setup(job);
        }

        public virtual void start()
        {
            error = null;
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

        public event JobProcessingStatusUpdateCallback StatusUpdate
        {
            add
            {
                encoder.StatusUpdate += value;
            }
            remove
            {
                encoder.StatusUpdate -= value;
            }
        }

        public bool canBeProcessed(Job job)
        {
            return (job is VideoJob);
        }
        #endregion
    }
}*/
