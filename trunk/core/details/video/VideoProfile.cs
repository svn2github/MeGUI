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
	/// Contains all the info from a video profile
	/// </summary>
	public class VideoProfile : Profile
	{
		private VideoCodecSettings settings;

		public VideoProfile()
		{
			this.Name = "Default";
		}
		public VideoProfile(string name, VideoCodecSettings settings)
		{
			this.Name = name;
			this.settings = settings;
		}
		public VideoCodecSettings Settings
		{
			get {return settings;}
			set {settings = value;}
		}
        public override string ToString()
        {
            return Name;
        }
        public VideoProfile clone()
        {
            return new VideoProfile(this.Name, Settings.clone());
        }
	}
}
