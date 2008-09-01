// ****************************************************************************
// 
// Copyright (C) 2005-2008  Doom9 & al
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
using System.Collections.Generic;

namespace MeGUI
{
	/// <summary>
	/// Summary description for IndexJob.
	/// </summary>
	public class IndexJob : Job
	{
		private bool loadSources;
		private int demuxMode;
        private List<string> trackIDs;
		private DGIndexPostprocessingProperties postprocessingProperties;
		
		public IndexJob():base()
		{
			loadSources = false;
			demuxMode = 0;
            trackIDs = new List<string>();
		}

        public IndexJob(string input, string output, int demuxType, List<string> trackIDs,
            DGIndexPostprocessingProperties properties, bool loadSources)
        {
            Input = input;
            Output = output;
            DemuxMode = demuxType;
            TrackIDs = trackIDs;
            PostprocessingProperties = properties;
            LoadSources = loadSources;
        }


		/// <summary>
		/// gets / sets whether the audio and video files should be loaded after indexing
		/// </summary>
		public bool LoadSources
		{
			get {return loadSources;}
			set {loadSources = value;}
		}
    	/// <summary>
		/// gets / sets the demux mode
		/// 0 = no audio demux
		/// 1 = demux selected audio track
		/// 2 = demux all audio tracks
		/// </summary>
		public int DemuxMode
		{
			get {return demuxMode;}
			set {demuxMode = value;}
		}

        public List<string> TrackIDs
        {
            get { return trackIDs; }
            set { trackIDs = value; }
        }

		/// <summary>
		/// gets / sets the postprocessing properties
		/// if this is not set, we're just dealing with a regular demuxing job
		/// if it is defined, we're dealing with an index job in one click mode
		/// and all the postprocessing that has to be done prior to audio encoding
		/// is defined in this property
		/// </summary>
		public DGIndexPostprocessingProperties PostprocessingProperties
		{
			get {return postprocessingProperties;}
			set {postprocessingProperties = value;}
		}

        public override string CodecString
        {
            get { return ""; }
        }

        public override string EncodingMode
        {
            get { return "idx"; }
        }
    }
}