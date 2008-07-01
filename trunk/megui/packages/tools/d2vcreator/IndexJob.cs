using System;

namespace MeGUI
{
	/// <summary>
	/// Summary description for IndexJob.
	/// </summary>
	public class IndexJob : Job
	{
		private bool loadSources;
		private int demuxMode;
		private DGIndexPostprocessingProperties postprocessingProperties;
		
		public IndexJob():base()
		{
			loadSources = false;
			demuxMode = 0;
		}

        public IndexJob(string input, string output, int demuxType,
            DGIndexPostprocessingProperties properties, bool loadSources)
        {
            Input = input;
            Output = output;
            DemuxMode = demuxType;
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