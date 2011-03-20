// ****************************************************************************
// 
// Copyright (C) 2005-2011  Doom9 & al
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
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MeGUI
{
    /// <summary>
    /// Container object for MkvExtract
    /// </summary>
    public class MkvExtractJob : Job
    {
        private List<MkvInfoTrack> _oTracks;
        private string _strOutputPath;

        public MkvExtractJob() : this(null, null, null) { }

        public MkvExtractJob(string strInput, string strOutputPath, List<MkvInfoTrack> oTracks)
            : base(strInput, null)
        {
            this._oTracks = oTracks;
            this._strOutputPath = strOutputPath;
        }

        public override string CodecString
        {
            get { return "mkvextract"; }
        }

        public override string EncodingMode
        {
            get { return "ext"; }
        }

        public List<MkvInfoTrack> MkvTracks
        {
            get { return _oTracks; }
            set { _oTracks = value; }
        }

        public string OutputPath
        {
            get { return _strOutputPath; }
            set { _strOutputPath = value; }
        }
    }
}