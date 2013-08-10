// ****************************************************************************
// 
// Copyright (C) 2005-2013 Doom9 & al
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
using System.Text;
using System.Xml.Serialization;

namespace MeGUI
{
    public enum TrackType
    {
        Audio,
        Subtitle,
        Unknown,
        Video
    }

    [XmlInclude(typeof(SubtitleTrackInfo)), XmlInclude(typeof(AudioTrackInfo)), XmlInclude(typeof(VideoTrackInfo))]
    public class TrackInfo
    {
        private string _codec, _containerType, _language, _name, _sourceFileName;
        private int _trackID, _mmgTrackID, _delay, _trackIndex;
        private bool _bDefault, _bForced, _bMKVTrack;
        private TrackType _trackType;

        public TrackInfo() : this(null, null)
        {

        }

        public TrackInfo(string language, string name)
        {
            this._language = language;
            this._name = name;
            this._trackType = TrackType.Unknown;
            this._trackID = 0;
            this._mmgTrackID = 0;
            this._delay = 0;
            this._trackIndex = 0;
            this._codec = _containerType = String.Empty;
            this._bMKVTrack = false;
        }

        /// <summary>
        /// The Track Type
        /// </summary>
        public TrackType TrackType
        {
            get { return _trackType; }
            set { _trackType = value; }
        }

        /// <summary>
        /// The full language string
        /// </summary>
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        /// <summary>
        /// The full source file name incuding path
        /// </summary>
        public string SourceFileName
        {
            get { return _sourceFileName; }
            set { _sourceFileName = value; }
        }

        /// <summary>
        /// The track name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The track ID
        /// </summary>
        public int TrackID
        {
            get { return _trackID; }
            set { _trackID = value; }
        }

        /// <summary>
        /// The MMG track ID
        /// </summary>
        public int MMGTrackID
        {
            get { return _mmgTrackID; }
            set { _mmgTrackID = value; }
        }

        /// <summary>
        /// The Container Type
        /// </summary>
        public string ContainerType
        {
            get { return _containerType; }
            set { _containerType = value; }
        }

        /// <summary>
        /// The track index
        /// </summary>
        public int TrackIndex
        {
            get { return _trackIndex; }
            set { _trackIndex = value; }
        }

        /// <summary>
        /// The Codec String
        /// </summary>
        public string Codec
        {
            get { return _codec; }
            set { _codec = value; }
        }

        /// <summary>
        /// The delay of the track
        /// </summary>
        public int Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        /// <summary>
        /// Default Track
        /// </summary>
        public bool DefaultTrack
        {
            get { return _bDefault; }
            set { _bDefault = value; }
        }

        /// <summary>
        /// Forced Track
        /// </summary>
        public bool ForcedTrack
        {
            get { return _bForced; }
            set { _bForced = value; }
        }

        public bool IsMKVContainer()
        {
            if (String.IsNullOrEmpty(_containerType))
                return false;
            else
                return _containerType.Trim().ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MATROSKA");
        }

        public bool ExtractMKVTrack
        {
            get { return _bMKVTrack; }
            set { _bMKVTrack = value; }
        }

        [XmlIgnore()]
        public String DemuxFileName
        {
            get
            {
                if (String.IsNullOrEmpty(_sourceFileName))
                    return null;

                string strExtension = String.Empty;
                string strCodec = String.Empty;
                string strFileName = String.Empty;

                if (!String.IsNullOrEmpty(_codec))
                    strCodec = _codec.ToUpper(System.Globalization.CultureInfo.InvariantCulture);

                if (IsMKVContainer())
                {
                    string[] arrCodec = new string[] { };
                    arrCodec = _codec.Split('/');
                    if (arrCodec[0].Substring(1, 1).Equals("_"))
                        arrCodec[0] = arrCodec[0].Substring(2);
                    strCodec = arrCodec[0].ToUpper(System.Globalization.CultureInfo.InvariantCulture);
                }

                if (strCodec.StartsWith("DTS", StringComparison.InvariantCultureIgnoreCase))
                    strCodec = "DTS";

                if (strCodec.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Contains("TRUEHD"))
                    strCodec = "TRUEHD";

                switch (strCodec)
                {
                    case "AC-3": strExtension = "ac3"; break;
                    case "TRUEHD": strExtension = "thd"; break;
                    case "DTS": strExtension = "dts"; break;
                    case "MP3": strExtension = "mp3"; break;
                    case "MP2": strExtension = "mp2"; break;
                    case "PCM": strExtension = "w64"; break;
                    case "MS/ACM": strExtension = "w64"; break;
                    case "VORBIS": strExtension = "ogg"; break;
                    case "FLAC": strExtension = "flac"; break;
                    case "REAL": strExtension = "ra"; break;
                    case "AAC": strExtension = "aac"; break;
                    case "VOBSUB": strExtension = "idx"; break;
                    case "ASS": strExtension = "ass"; break;
                    case "UTF-8": strExtension = "srt"; break;
                    case "SSA": strExtension = "ssa"; break;
                    case "USF": strExtension = "usf"; break;
                    case "HDMV": strExtension = "sup"; break;
                    case "PGS": strExtension = "sup"; break;
                    case "AVS": strExtension = "avs"; break;
                    default: strExtension = strCodec + ".unknown"; break;
                }

                if (!strExtension.Equals("avs", StringComparison.InvariantCultureIgnoreCase))
                {
                    strFileName = System.IO.Path.GetFileNameWithoutExtension(_sourceFileName) + " - [" + _trackIndex + "]";
                    if (!String.IsNullOrEmpty(_language))
                        strFileName += " " + _language;
                    if (_delay != 0)
                        strFileName += " " + _delay + "ms";
                    strFileName += "." + strExtension;
                }
                else
                    strFileName = System.IO.Path.GetFileName(_sourceFileName);
                return strFileName;
            }
        }
    }
}
