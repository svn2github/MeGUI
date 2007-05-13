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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MeGUI.core.util;


namespace MeGUI
{


    public class AudioEncoder : IJobProcessor
    {
        public static readonly JobProcessorFactory Factory =
    new JobProcessorFactory(new ProcessorFactory(init), "AudioEncoder");

        private static IJobProcessor init(MainForm mf, Job j)
        {
            if (j is AudioJob) return new AudioEncoder(mf.Settings);
            return null;
        }

        private MeGUISettings settings;
        protected Job job;
        protected StatusUpdate su;
        protected StringBuilder log; // holds logging information
        private AudioEncoder encoder;

        public AudioEncoder(MeGUISettings settings)
        {
            this.settings = settings;
        }
        public AudioEncoder()
        {
        }

        protected void sendStatusUpdateToGUI(StatusUpdate su)
        {
            su.TimeElapsed = DateTime.Now.Ticks - job.Start.Ticks;
            su.FileSize = FileSize.Of2(job.Output);
            if (statusUpdate != null)
                statusUpdate(su);
        }

        public bool canBeProcessed(Job job)
        {
            if (job is AudioJob)
                return true;
            return false;
        }
        #region IJobProcessor Members

        public virtual bool setup(Job job, out string error)
        {
            error = null;
            
            encoder = new AviSynthAudioEncoder(settings);

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

        private event JobProcessingStatusUpdateCallback statusUpdate;

        public event JobProcessingStatusUpdateCallback StatusUpdate
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