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
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using MeGUI.core.util;

namespace MeGUI
{
    public class OneClickStream
    {
        private string _language;
        private string _name;
        private string _path;
        private string _container;
        private string _codec;
        private string _id;
        private int _delay;
        private bool _bDefaultTrack;
        private bool _bForceTrack;
        private TrackType _trackType;
        private AudioCodecSettings _encoderSettings;
        private AudioEncodingMode _encodingMode;

        // audio
        private AudioTrackInfo _audioInfo;

        // MKV Info
        private MkvInfoTrack _mkvInfo;

        public OneClickStream(string path, TrackType trackType, string codec, string container, string ID, string language, string name, int delay, bool bDefaultTrack, bool bForceTrack, AudioCodecSettings oSettings, AudioEncodingMode oEncodingMode, MkvInfoTrack trackInfo)
        {
            this._language = language;
            this._name = name;
            this._path = path;
            this._delay = delay;
            this._bDefaultTrack = bDefaultTrack;
            this._bForceTrack = bForceTrack;
            this._trackType = trackType;
            this._container = container;
            this._id = ID;
            this._codec = codec;
            this._mkvInfo = trackInfo;
            this._encoderSettings = oSettings;
            if ((int)oEncodingMode == -1)
                this._encodingMode = AudioEncodingMode.IfCodecDoesNotMatch;
            else
                this._encodingMode = oEncodingMode;
        }

        public OneClickStream(AudioTrackInfo oInfo) : this(oInfo, null) { }

        public OneClickStream(AudioTrackInfo oInfo, MkvInfoTrack oMkv) 
        {
            this._audioInfo = oInfo;
            this._trackType = TrackType.Audio;
            this._id = oInfo.TrackIDx;
            this._language = oInfo.Language;
            this._name = oInfo.Name;
            this._codec = oInfo.Type;
            this._mkvInfo = oMkv;
            this._encodingMode = AudioEncodingMode.IfCodecDoesNotMatch;
        }

        public OneClickStream() : this(null, TrackType.Unknown, null, null, null, null, null, 0, false, false, null, AudioEncodingMode.IfCodecDoesNotMatch, null) { }

        // Stream Type
        public TrackType Type
        {
            get { return _trackType; }
            set { _trackType = value; }
        }

        // Stream Language
        public string Language
        {
            get { return _language; }
            set 
            { 
                _language = value;
                if (_audioInfo != null)
                    _audioInfo.Language = _language;
                if (_mkvInfo != null)
                    _mkvInfo.Language = _language;
            }
        }

        // Stream Name
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (_audioInfo != null)
                    _audioInfo.Name = _name;
                if (_mkvInfo != null)
                    _mkvInfo.Name = _name;
            }
        }

        // Stream Name
        public string Path
        {
            get 
            {
                if (_mkvInfo != null)
                    return _mkvInfo.FileName;
                else
                    return _path; 
            }
            set { _path = value; }
        }

        // Stream ID
        public string TrackID
        {
            get { return _id; }
        }

        // Stream Delay
        public int Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        // Stream Delay
        public bool ForcedStream
        {
            get { return _bForceTrack; }
            set 
            { 
                _bForceTrack = value;
                if (_mkvInfo != null)
                    _mkvInfo.ForcedTrack = value;
            }
        }

        // Stream Delay
        public bool DefaultStream
        {
            get { return _bDefaultTrack; }
            set
            {
                _bDefaultTrack = value;
                if (_mkvInfo != null)
                    _mkvInfo.DefaultTrack = value;
            }
        }

        // Stream Info
        public MkvInfoTrack MkvInfo
        {
            get { return _mkvInfo; }
            set { _mkvInfo = value; }
        }

        // Audio Track Info
        public AudioTrackInfo AudioTrackInfo
        {
            get { return _audioInfo; }
            set { _audioInfo = value; }
        }

        // Audio Track Info
        public AudioCodecSettings EncoderSettings
        {
            get { return _encoderSettings; }
            set { _encoderSettings = value; }
        }

        // Audio Track Info
        public AudioEncodingMode EncodingMode
        {
            get { return _encodingMode; }
            set 
            {
                if ((int)value == -1)
                    _encodingMode = AudioEncodingMode.IfCodecDoesNotMatch;
                else
                    _encodingMode = value; 
            }
        }

        public override string ToString()
        {
            string fullString = "[" + _id + "] - " + this._codec;
            if (_audioInfo != null)
            {
                if (!string.IsNullOrEmpty(_audioInfo.NbChannels))
                    fullString += " - " + _audioInfo.NbChannels;
                if (!string.IsNullOrEmpty(_audioInfo.SamplingRate))
                    fullString += " / " + _audioInfo.SamplingRate;
            }
            if (!string.IsNullOrEmpty(_language))
                fullString += " / " + _language;
            return fullString.Trim();
        }
    }
}

