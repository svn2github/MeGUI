using System;

namespace MeGUI
{
	/// <summary>
	/// Summary description for IndexJob.
	/// </summary>
	public class IndexJob : Job
	{
		private bool autoForceFilm, loadSources;
		private int audioTrackID1, audioTrackID2, demuxMode;
		private double forceFilmThreshold;
		private DGIndexPostprocessingProperties postprocessingProperties;
		
		public IndexJob():base()
		{
			autoForceFilm = true;
			loadSources = false;
			demuxMode = 0;
			forceFilmThreshold = 95.0;
			audioTrackID1 = audioTrackID2 = -1;
		}
		/// <summary>
		/// gets / sets whether force film should be applied after indexing
		/// </summary>
		public bool AutoForceFilm
		{
			get {return autoForceFilm;}
			set {autoForceFilm = value;}
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
		/// gets / sets the track ID of the first audio track to be demuxed
		/// </summary>
		public int AudioTrackID1
		{
			get {return audioTrackID1;}
			set {audioTrackID1 = value;}
		}
		/// <summary>
		///  gets / sets the track ID of the second audio track to be demuxed
		/// </summary>
		public int AudioTrackID2
		{
			get {return audioTrackID2;}
			set {audioTrackID2 = value;}
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
		/// gets / sets the percentage above which force film should be automatically applied
		/// </summary>
		public double ForceFilmThreshold
		{
			get {return forceFilmThreshold;}
			set {forceFilmThreshold = value;}
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
	}
}