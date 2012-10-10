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
    public class AudioTrackInfo : TrackInfo
    {
        private string nbChannels, samplingRate, channelPositions;
        private int aacFlag;

        public AudioTrackInfo() : this(null, null, 0)
        {
        }

        public AudioTrackInfo(string language, string codec, int trackID)
        {
            base.TrackType = TrackType.Audio;
            base.Language = language;
            base.Codec = codec;
            base.TrackID = trackID;
            this.aacFlag = -1;
            this.nbChannels = "unknown";
            this.samplingRate = "unknown";
            this.channelPositions = "unknown";
        }

        public string TrackIDx
        {
            get { return ContainerType == "MPEG-TS" ? TrackID.ToString("x3") : TrackID.ToString("x"); }
            set { TrackID = Int32.Parse(value, System.Globalization.NumberStyles.HexNumber); }
        }

        public string DgIndexID
        {
            get { return ContainerType == "MPEG-TS" ? TrackIndex.ToString() : TrackIDx; }
        }

        public string NbChannels
        {
            get { return nbChannels; }
            set { nbChannels = value; }
        }

        public string ChannelPositions
        {
            get { return channelPositions; }
            set { channelPositions = value; }
        }

        public string SamplingRate
        {
            get { return samplingRate; }
            set { samplingRate = value; }
        }

        public int AACFlag
        {
            get { return aacFlag; }
            set { aacFlag = value; }
        }

        public override string ToString()
        {
            string fullString = "[" + TrackIDx + "] - " + this.Codec;
            if (!string.IsNullOrEmpty(nbChannels))
                fullString += " - " + this.nbChannels;
            if (!string.IsNullOrEmpty(samplingRate))
                fullString += " / " + samplingRate;
            if (!string.IsNullOrEmpty(Language))
                fullString += " / " + Language;
            return fullString.Trim();
        }
    }
}
