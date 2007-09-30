using System;
using System.Collections.Generic;
using MeGUI.core.util;

namespace MeGUI
{
	/// <summary>
	/// Summary description for MuxSettings.
	/// </summary>
	public class MuxSettings
	{
		private List<SubStream> audioStreams, subtitleStreams;
		private double framerate;
		private string chapterFile, videoName;
        private string muxedInput, videoInput, muxedOutput;

		private int splitSize, parX, parY;

		public MuxSettings()
		{
			audioStreams = new List<SubStream>();
			subtitleStreams = new List<SubStream>();
			framerate = 0.0;
            muxedInput = "";
			chapterFile = "";
            videoName = "";
            videoInput = "";
            muxedOutput = "";
			splitSize = 0;
			parX = -1;
			parY = -1;
		}

        public string MuxedInput
        {
            get { return muxedInput; }
            set { muxedInput = value; }
        }
        public string MuxedOutput
        {
            get { return muxedOutput; }
            set { muxedOutput = value; }
        }
        public string VideoInput
        {
            get { return videoInput; }
            set { videoInput = value; }
        }

		/// <summary>
		/// Array of all the audio streams to be muxed
		/// </summary>
		public List<SubStream> AudioStreams
		{
			get {return audioStreams;}
			set {audioStreams = value;}
		}
		/// <summary>
		/// Array of subtitle tracks to be muxed
		/// </summary>
		public List<SubStream> SubtitleStreams
		{
			get {return subtitleStreams;}
			set {subtitleStreams = value;}
		}
		/// <summary>
		/// framerate of the video
		/// </summary>
		public double Framerate
		{
			get {return framerate;}
			set {framerate = value;}
		}
		/// <summary>
		/// the file containing the chapter information
		/// </summary>
		public string ChapterFile
		{
			get {return chapterFile;}
			set {chapterFile = value;}
		}
		/// <summary>
		/// file size at which the output file is to be split
		/// </summary>
		public int SplitSize
		{
			get {return splitSize;}
			set {splitSize = value;}
		}

        private Dar? dar;

        public Dar? DAR
        {
            get { return dar; }
            set { dar = value; }
        }

		
        /// <summary>
        /// gets / sets the name of the video track
        /// </summary>
        public string VideoName
        {
           get { return videoName; }
           set { videoName = value; }
        }
	
	}
}