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
using MeGUI.core.plugins.interfaces;

namespace MeGUI
{
	/// <summary>
	/// Container object for audio encoding
	/// holds all the parameters relevant for aac encoding in besweet
	/// </summary>
	public class AudioJob : Job
	{
        private string cutFile;

        public string CutFile
        {
            get { return cutFile; }
            set { cutFile = value; }
        }


		private AudioCodecSettings settings;
		public AudioJob():base()
		{
			settings = null;
		}

		/// <summary>
		/// the settings for this audio job
		/// </summary>
		public AudioCodecSettings Settings
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
				if (settings is NeroAACSettings)
					return "AAC Nero";
				else if (settings is MP3Settings)
					return "MP3";
				else if (settings is FaacSettings)
					return "AAC FAAC";
				return "";
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
				switch (((AudioCodecSettings)settings).BitrateMode)
				{
					case BitrateManagementMode.CBR:
						return "CBR";
					case BitrateManagementMode.ABR:
						return "ABR";
					case BitrateManagementMode.VBR:
						return "VBR";
				}
				return "";
			}
		}
    }
}