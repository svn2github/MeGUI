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
	/// this class contains all the settings for the Nero AAC encoder
	/// </summary>
	public enum VBRPPRESET:int {TAPE = 0, RADIO, INTERNET, STREAMING, NORMAL, EXTREME, AUDIOPHILE, TRANSCODING};
	public enum AACLEVEL:int {PROFILE_HE = 0, PROFILE_LC};
	public enum QUALITYMODE:int {QUALITY_HIGH = 0, QUALITY_FAST};
	public class NeroAACSettings : AudioCodecSettings
	{
		private VBRPPRESET vbrPreset;
		private AACLEVEL profile;
		private QUALITYMODE quality;
		private bool profileEnabled;
		private bool qualityEnabled;

		public NeroAACSettings() : base()
		{
			vbrPreset = VBRPPRESET.STREAMING;
			profile = AACLEVEL.PROFILE_HE;
			quality = QUALITYMODE.QUALITY_HIGH;
			profileEnabled = false;
			qualityEnabled = false;
            Codec = AudioCodec.AAC;
		}
		public VBRPPRESET VbrPreset
		{
			get {return vbrPreset;}
			set {vbrPreset = value;}
		}
		public AACLEVEL Profile
		{
			get {return profile;}
			set {profile = value;}
		}
		public QUALITYMODE Quality
		{
			get {return quality;}
			set {quality = value;}
		}
		public bool ProfileEnabled
		{
			get {return profileEnabled;}
			set {profileEnabled = value;}
		}
		public bool QualityEnabled
		{
			get {return qualityEnabled;}
			set {qualityEnabled = value;}
		}
	}
}