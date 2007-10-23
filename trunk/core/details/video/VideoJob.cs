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
using MeGUI.core.util;

namespace MeGUI
{
	/// <summary>
	/// Summary description for VideoJob.
	/// </summary>
	public class VideoJob : Job
	{
		private VideoCodecSettings settings;
        public BitrateCalculationInfo BitrateCalculationInfo;

		public VideoJob():base()
		{
		}

        public VideoJob(string input, string output,
            VideoCodecSettings settings, Dar? dar)
            : base(input, output)
        {
            Settings = settings;
            DAR = dar;
        }

        private Dar? dar;

        public Dar? DAR
        {
            get { return dar; }
            set { dar = value; }
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
		/// codec used as presentable string
		/// </summary>
		public override string CodecString
		{
			get
			{
                return settings.Codec.ID;
			}
		}
		/// <summary>
		/// returns the encoding mode as a human readable string
		/// (this string is placed in the appropriate column in the queue)
		/// </summary>
		public override string EncodingMode
		{
			get
			{
				int mode = settings.EncodingMode;
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
