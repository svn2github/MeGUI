// ****************************************************************************
// 
// Copyright (C) 2005-2012 Doom9 & al
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MeGUI
{
    public class SubtitleTrackInfo : TrackInfo
    {
        public SubtitleTrackInfo() : this(0, null, null)
        {
        }

        public SubtitleTrackInfo(int mmgTrackID, string language, string name)
        {
            base.TrackType = TrackType.Subtitle;
            base.Language = language;
            base.TrackID = -1;
            base.MMGTrackID = mmgTrackID;
            base.Name = name;
        }

        public override string ToString()
        {
            string strCodec = this.Codec.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            if (IsMKVContainer())
            {
                string[] arrCodec = new string[] { };
                arrCodec = this.Codec.Split('/');
                if (arrCodec[0].Substring(1, 1).Equals("_"))
                    arrCodec[0] = arrCodec[0].Substring(2);
                strCodec = arrCodec[0].ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            }

            string fullString = "[" + MMGTrackID + "] - " + strCodec;
            if (!string.IsNullOrEmpty(Language))
                fullString += " / " + Language;
            return fullString.Trim();
        }
    }
}
