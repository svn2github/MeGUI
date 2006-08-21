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
	public class Muxer : IJobProcessor
	{
        private MeGUISettings settings;
        protected MuxJob job;
        protected StatusUpdate su;
        protected StringBuilder log; // holds logging information
        private Muxer muxer;

        public Muxer(MeGUISettings settings)
		{
            this.settings = settings;
		}
        public Muxer()
        {
        }

        protected void sendStatusUpdateToGUI(StatusUpdate su)
        {
            statusUpdate(su);
        }

        #region IJobProcessor Members
        public bool canBeProcessed(Job job)
        {
            if (job is MuxJob)
                return true;
            return false;
        }

        public virtual bool setup(Job job, out string error)
        {
            muxer = new MuxProvider().GetMuxer(((MuxJob)job).MuxType, settings);
            if (muxer == null)
            {
                error = "No suitable muxer found";
                return false;
            }
            error = null;
            return muxer.setup(job, out error);
        }

        public virtual bool start(out string error)
        {
            error = null;
            muxer.job.Start = DateTime.Now;
            return muxer.start(out error);
        }

        public virtual bool stop(out string error)
        {
            error = null;
            return muxer.stop(out error);
        }

        public virtual bool pause(out string error)
        {
            error = null;
            return muxer.pause(out error);
        }

        public virtual bool resume(out string error)
        {
            error = null;
            return muxer.resume(out error);
        }

        public virtual bool changePriority(ProcessPriority priority, out string error)
        {
            error = null;
            return muxer.changePriority(priority, out error);
        }

        private event JobProcessingStatusUpdateCallback statusUpdate;

        public event JobProcessingStatusUpdateCallback StatusUpdate
        {
            add
            {
                muxer.statusUpdate += value;
            }
            remove
            {
                muxer.statusUpdate -= value;
            }
        }
        #endregion
    }
}