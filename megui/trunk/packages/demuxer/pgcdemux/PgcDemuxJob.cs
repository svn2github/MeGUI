// ****************************************************************************
// 
// Copyright (C) 2005-2015 Doom9 & al
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
    /// Container object for PgcDemux
    /// </summary>
    public class PgcDemuxJob : Job
    {
        private string _strOutputPath;
        private int _pgcNumber;

        public PgcDemuxJob() : this(null, null, 1) { }

        public PgcDemuxJob(string strInput, string strOutputPath, int pgcNumber)
            : base(strInput, null)
        {
            this._strOutputPath = strOutputPath;
            this._pgcNumber = pgcNumber;
            if (!String.IsNullOrEmpty(strOutputPath))
            {
                FilesToDelete.Add(System.IO.Path.Combine(strOutputPath, "LogFile.txt"));
                FilesToDelete.Add(System.IO.Path.Combine(strOutputPath, "Celltimes.txt"));
            }
        }

        public override string CodecString
        {
            get { return "pgcdemux"; }
        }

        public override string EncodingMode
        {
            get { return "ext"; }
        }

        public string OutputPath
        {
            get { return _strOutputPath; }
            set { _strOutputPath = value; }
        }

        public int PGCNumber
        {
            get { return _pgcNumber; }
            set { _pgcNumber = value; }
        }
    }
}