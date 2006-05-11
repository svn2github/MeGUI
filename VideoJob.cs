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

namespace MeGUI
{
	/// <summary>
	/// Summary description for VideoJob.
	/// </summary>
	public class VideoJob : Job
	{
		private VideoCodecSettings settings;
		private int outputType, numberOfFrames, parX, parY;
		private long desiredSize;
		private double framerate;
		public VideoJob():base()
		{
            parX = parY = 0;
		}
        public int PARX
        {
            get { return parX; }
            set { parX = value; }
        }
        public int PARY
        {
            get { return parY; }
            set { parY = value; }
        }
		/// <summary>
		/// the codec settings for this job
		/// </summary>
		public VideoCodecSettings Settings
		{
			get {return settings;}
			set {settings = value;}
		}
		/// <summary>
		/// output type for this job: avi, raw or mp4
		/// </summary>
		public int OutputType
		{
			get {return outputType;}
			set {outputType = value;}
		}
		/// <summary>
		/// the desired output size this job shall have, in bytes
		/// </summary>
		public long DesiredSize
		{
			get {return desiredSize;}
			set {desiredSize = value;}
		}
		/// <summary>
		/// the number of frames the source of this job has
		/// </summary>
		public int NumberOfFrames
		{
			get {return numberOfFrames;}
			set {numberOfFrames = value;}
		}
		/// <summary>
		/// the framerate of the source of this job
		/// </summary>
		public double Framerate
		{
			get {return framerate;}
			set {framerate = value;}
		}
		/// <summary>
		/// codec used as presentable string
		/// </summary>
		public string CodecString
		{
			get
			{
				if (settings is lavcSettings)
					return "ASP";
				if (settings is x264Settings)
					return "AVC";
				if (settings is snowSettings)
					return "Snow";
				if (settings is xvidSettings)
					return "XviD";
				return "";
			}
		}
		/// <summary>
		/// returns the encoding mode as a human readable string
		/// (this string is placed in the appropriate column in the queue)
		/// </summary>
		public string EncodingMode
		{
			get
			{
				int mode = ((VideoCodecSettings)settings).EncodingMode;
				switch (mode)
				{
					case (int)VideoCodecSettings.Mode.CBR:
						if (settings is x264Settings)
							return "ABR";
						else
							return "CBR";
					case (int)VideoCodecSettings.Mode.CQ:
						return "CQ";
					case (int)VideoCodecSettings.Mode.twopass1:
						return "2 pass 1st pass";
					case (int)VideoCodecSettings.Mode.twopass2:
						return "2 pass 2nd pass";
					case (int)VideoCodecSettings.Mode.twopassAutomated:
						return "2 pass automated";
					case (int)VideoCodecSettings.Mode.threepass1:
						return "3 pass 1st pass";
					case (int)VideoCodecSettings.Mode.threepass2:
						return "3 pass 2nd pass";
					case (int)VideoCodecSettings.Mode.threepass3:
						return "3 pass 3rd pass";
				}
				return "";
			}
		}
	}
}
