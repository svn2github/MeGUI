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
	/// Summary description for FaacSettings.
	/// </summary>
	public class FaacSettings : AudioCodecSettings
	{
        public static string ID = "FAAC";

        public static readonly int[] SupportedBitrates = new int[] {
            64,
            80,
            96,
            112,
            128,
            160,
            192,
            224,
            256,
            320,
            388,
            448};

		private decimal quality;
		public FaacSettings() : base(ID)
		{
			Quality = 100;
            Bitrate = 128;
            BitrateMode = BitrateManagementMode.VBR;
            Codec = AudioCodec.AAC;
            EncoderType = AudioEncoderType.FAAC;
		}
		/// <summary>
		/// gets / sets the vbr quality
		/// </summary>
		public decimal Quality
		{
			get {return quality;}
			set {quality = value;}
		}
	}
}
