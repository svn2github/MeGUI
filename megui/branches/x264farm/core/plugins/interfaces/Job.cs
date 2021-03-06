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
using MeGUI.core.details;

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
	/// This represents an un-identifiable job. It only has information about how to complete
    /// it and no way to say *which specific job* it is in the queue. Furthermore, it doesn't
    /// have any extraneous information about its storage in the queue. That information is held
    /// in the TaggedJob class.
	/// </summary>
	[XmlInclude(typeof(VideoJob)), XmlInclude(typeof(AudioJob)), XmlInclude(typeof(MuxJob)), 
	XmlInclude (typeof(MuxStream)), XmlInclude(typeof(IndexJob)), XmlInclude(typeof(AviSynthJob)), 
    XmlInclude(typeof(SubtitleIndexJob)), XmlInclude(typeof(AudioSplitJob)), XmlInclude(typeof(AudioJoinJob)),
    XmlInclude(typeof(CleanupJob))]
	public abstract class Job
	{
        #region important details
        public string Input;
        public string Output;
        public List<string> FilesToDelete;
        #endregion

        #region init
        public Job():this(null, null)
		{
        }

        public Job(string input, string output)
        {
            Input = input;
            Output = output;
            if (!string.IsNullOrEmpty(input) && input == output)
                throw new MeGUIException("Input and output files may not be the same");

            FilesToDelete = new List<string>();
        }
        #endregion

        #region queue display details
        public abstract string CodecString
        {
            get;
        }

        public abstract string EncodingMode
        {
            get;
        }

        #endregion
    }
}
