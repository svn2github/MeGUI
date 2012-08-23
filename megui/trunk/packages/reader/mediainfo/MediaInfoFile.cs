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
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using MediaInfoWrapper;

using MeGUI.core.util;
using MeGUI.core.details;

namespace MeGUI
{
    public class MediaInfoException : Exception
    {
        public MediaInfoException(Exception e)
        : base("Media info error: " + e.Message, e)
        {}
    }

    public class MediaInfoFileFactory : IMediaFileFactory
    {
        #region IMediaFileFactory Members

        public IMediaFile Open(string file)
        {
            return new MediaInfoFile(file);
        }

        #endregion

        #region IMediaFileFactory Members


        public int HandleLevel(string file)
        {
            return 5;
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "MediaInfo/DirectShowSource"; }
        }

        #endregion
    }

    public class MediaInfoFile : IMediaFile
    {
        public static MediaFile Open(string file)
        {
            try
            {
                MediaInfo m = new MediaInfo(file);

                // tracks
                List<MediaTrack> tracks = new List<MediaTrack>();
                foreach (MediaInfoWrapper.VideoTrack t in m.Video)
                {
                    VideoTrack v = new VideoTrack();
                    v.Codec = v.VCodec = getVideoCodec(t.Codec);
                    v.Info = new TrackInfo(t.Language, t.Title);

                    ulong width = ulong.Parse(t.Width);
                    ulong height = ulong.Parse(t.Height);
                    ulong frameCount = ulong.Parse(t.FrameCount);
                    double fps = (easyParseDouble(t.FrameRate) ?? easyParseDouble(t.FrameRateOriginal) ?? 99);

                    decimal? ar = easyParse<decimal>(delegate { return decimal.Parse(t.AspectRatio); });
                    Dar dar = new Dar(ar, width, height);

                    v.StreamInfo = new VideoInfo2(width, height, dar, frameCount, fps);
                    v.TrackNumber = uint.Parse(t.ID);
                    tracks.Add(v);
                }

                foreach (MediaInfoWrapper.AudioTrack t in m.Audio)
                {
                    AudioTrack a = new AudioTrack();
                    a.Codec = a.ACodec = getAudioCodec(t.Format);
                    a.Info = new TrackInfo(t.Language, t.Title);

                    a.StreamInfo = new AudioInfo();

                    a.TrackNumber = uint.Parse(t.ID);

                    tracks.Add(a);
                }

                foreach (MediaInfoWrapper.TextTrack t in m.Text)
                {
                    SubtitleTrack s = new SubtitleTrack();
                    s.Codec = s.SCodec = getSubtitleCodec(t.Codec);
                    s.Info = new TrackInfo(t.Language, t.Title);
                    s.StreamInfo = new SubtitleInfo2();
                    s.TrackNumber = uint.Parse(t.ID);

                    tracks.Add(s);
                }

                if (m.General.Count != 1)
                    throw new Exception("Expected one general track");

                GeneralTrack g = m.General[0];
                ContainerType cType = getContainerType(g.Format, g.FormatString);
                TimeSpan playTime = TimeSpan.Parse(g.PlayTimeString3);

                Chapters chapters = null;
                if (m.Chapters.Count == 1)
                    chapters = parseChapters(m.Chapters[0]);

                m.Dispose();
                return new MediaFile(tracks, chapters, playTime, cType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Regex chaptersRegex = new Regex(
            @"^(?<num>\d+)\s*:\s*(?<hours>\d+):(?<mins>\d+):(?<secs>\d+).(?<ms>\d+) (?<name>.*)$", 
            RegexOptions.Multiline| RegexOptions.Compiled);
        private static Chapters parseChapters(MediaInfoWrapper.ChaptersTrack t)
        {
            // sample:

/*Language             : English
1                    : 00:00:00.000 Part 1
2                    : 00:42:20.064 Part 2
3                    : 01:26:34.240 Part 3*/

            List<Chapter> chapters = new List<Chapter>();
            foreach (Match m in chaptersRegex.Matches(t.Inform))
            {
                Chapter c = new Chapter();
                c.name = m.Groups["name"].Value;
                c.StartTime = new TimeSpan(0,
                    int.Parse(m.Groups["hours"].Value),
                    int.Parse(m.Groups["mins"].Value),
                    int.Parse(m.Groups["secs"].Value),
                    int.Parse(m.Groups["ms"].Value));
                chapters.Add(c);
            }
            Chapters ch = new Chapters();
            ch.Data = chapters;
            return ch;
        }

        private static SubtitleCodec getSubtitleCodec(string p)
        {
            try
            {
                return null;
            }
            catch (Exception)
            {
               throw new Exception("The method or operation is not implemented.");
            }
        }

        public static T? easyParse<T>(Getter<T> parse)
            where T : struct
        {
            try
            {
                return parse();
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region variables
        private static readonly CultureInfo culture = new CultureInfo("en-us");
        private static Dictionary<string, VideoCodec> knownVideoDescriptions;
        private static Dictionary<string, AudioCodec> knownAudioDescriptions;
        private static Dictionary<string, ContainerType> knownContainerTypes;
        private static Dictionary<string, ContainerType> knownContainerDescriptions;
        private IMediaFile videoSourceFile = null;
        private IVideoReader videoReader = null; 
        private ContainerType cType;
        private string _strContainer = "";
        private string _file;
        private VideoInformation _VideoInfo;
        private AudioInformation _AudioInfo;
        private SubtitleInformation _SubtitleInfo;
        private MkvInfo _MkvInfo;
        private LogItem _Log;
        #endregion
        #region properties
        public AudioInformation AudioInfo
        {
            get { return _AudioInfo; }
        }

        public SubtitleInformation SubtitleInfo
        {
            get { return _SubtitleInfo; }
        }

        public VideoInformation VideoInfo
        {
            get { return _VideoInfo; }
        }

        public ContainerType ContainerFileType
        {
            get { return cType; }
        }

        public string ContainerFileTypeString
        {
            get { return _strContainer; }
        }
        #endregion

        public MediaInfoFile(string file, ref LogItem oLog) : this(file, ref oLog, 1)
        {
        }

        public MediaInfoFile(string file, ref LogItem oLog, int iPGCNumber)
        {
            GetSourceInformation(file, oLog, iPGCNumber);
        }

        public MediaInfoFile(string file) : this(file, 1)
        {
        }

        public MediaInfoFile(string file, int iPGCNumber)
        {
            GetSourceInformation(file, null, iPGCNumber);
        }

        /// <summary>
        /// gets information about a source using MediaInfo
        /// </summary>
        /// <param name="file">the file to be analyzed</param>
        /// <param name="oLog">the log item</param>
        private void GetSourceInformation(string file, LogItem oLog, int iPGCNumber)
        {
            _Log = oLog;
            _file = file;
            _indexerToUse = FileIndexerWindow.IndexType.NONE;
            this._AudioInfo = new AudioInformation();
            this._SubtitleInfo = new SubtitleInformation();
            this._VideoInfo = new VideoInformation(false, 0, 0, Dar.A1x1, 0, 0, 0, 1);
            
            try
            {
                LogItem infoLog = null;
                if (oLog != null)
                {
                    infoLog = oLog.LogValue("MediaInfo", String.Empty);
                    infoLog.Info("File: " + _file);
                }

                // if an index file is used extract the real file name
                if (Path.GetExtension(file).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".d2v") ||
                    Path.GetExtension(file).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".dga") ||
                    Path.GetExtension(file).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".dgi"))
                {
                    using (StreamReader sr = new StreamReader(file, Encoding.Default))
                    {
                        string line = null;
                        int iLineCount = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            iLineCount++;
                            if (iLineCount == 3 && !Path.GetExtension(file).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".dgi"))
                            {
                                string strSourceFile = line;
                                if (Path.GetExtension(file).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".dgi"))
                                    strSourceFile = line.Substring(0, line.LastIndexOf(" "));
                                if (File.Exists(strSourceFile))
                                    _file = file = strSourceFile;
                                break;
                            }
                            else if (iLineCount == 4)
                            {
                                string strSourceFile = line.Substring(0, line.LastIndexOf(" "));
                                if (File.Exists(strSourceFile))
                                    _file = file = strSourceFile;
                                break;
                            }
                        }
                    }
                    if (infoLog != null)
                        infoLog.Info("Indexed File: " + _file);
                }

                // get basic media information
                MediaInfo info = new MediaInfo(file);
                CorrectSourceInformation(ref info, file, infoLog, iPGCNumber);
                if (infoLog != null)
                    WriteSourceInformation(info, file, infoLog);

                // container detection
                if (info.General.Count < 1)
                {
                    cType = null;
                }
                else
                {
                    cType = getContainerType(info.General[0].Format, info.General[0].FormatString);
                    _strContainer = info.General[0].Format;
                }

                // audio detection
                _AudioInfo.Codecs = new AudioCodec[info.Audio.Count];
                _AudioInfo.BitrateModes = new BitrateManagementMode[info.Audio.Count];

                if (_AudioInfo.Codecs.Length == 1)
                    _AudioInfo.Type = getAudioType(_AudioInfo.Codecs[0], cType, file);
                else
                    _AudioInfo.Type = null;

                for (int counter = 0; counter < info.Audio.Count; counter++)
                {
                    MediaInfoWrapper.AudioTrack atrack = info.Audio[counter];

                    _AudioInfo.Codecs[counter] = getAudioCodec(atrack.Format);
                    if (atrack.BitRateMode == "VBR")
                        _AudioInfo.BitrateModes[counter] = BitrateManagementMode.VBR;
                    else
                        _AudioInfo.BitrateModes[counter] = BitrateManagementMode.CBR;

                    AudioTrackInfo ati = new AudioTrackInfo();
                    ati.SourceFileName = _file;
                    ati.DefaultTrack = atrack.DefaultString.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals("yes");
                    ati.ForcedTrack = atrack.ForcedString.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals("yes");
                    // DGIndex expects audio index not ID for TS
                    ati.ContainerType = info.General[0].Format;
                    ati.TrackIndex = counter;
                    int iID = 0;
                    if (info.General[0].Format == "CDXA/MPEG-PS")
                        // MediaInfo doesn't give TrackID for VCD, specs indicate only MP1L2 is supported
                        ati.TrackID = (0xC0 + counter);
                    else if (atrack.ID != "0" && atrack.ID != "" &&
                             (Int32.TryParse(atrack.ID, out iID) ||
                             (atrack.ID.Contains("-") && Int32.TryParse(atrack.ID.Split('-')[1], out iID))))
                        ati.TrackID = iID;
                    else
                    {
                        // MediaInfo failed to get ID try guessing based on codec
                        switch (atrack.Format.Substring(0, 3))
                        {
                            case "AC-":
                            case "AC3": ati.TrackID = (0x80 + counter); break;
                            case "PCM": ati.TrackID = (0xA0 + counter); break;
                            case "MPE": // MPEG-1 Layer 1/2/3
                            case "MPA": ati.TrackID = (0xC0 + counter); break;
                            case "DTS": ati.TrackID = (0x88 + counter); break;
                        }
                    }
                    if (Int32.TryParse(atrack.StreamOrder, out iID))
                        ati.MMGTrackID = iID;
                    if (atrack.FormatProfile != "") // some tunings to have a more useful info instead of a typical audio Format
                    {
                        switch (atrack.FormatProfile)
                        {
                            case "Dolby Digital": ati.Codec = "AC-3"; break;
                            case "HRA / Core":
                            case "HRA": ati.Codec = "DTS-HD High Resolution"; break;
                            case "Layer 1": ati.Codec = "MPA"; break;
                            case "Layer 2": ati.Codec = "MP2"; break;
                            case "Layer 3": ati.Codec = "MP3"; break;
                            case "LC": ati.Codec = "AAC"; break;
                            case "MA":
                            case "MA / Core": ati.Codec = "DTS-HD Master Audio"; break;
                            case "TrueHD": ati.Codec = "TrueHD"; break;
                            case "ES": ati.Codec = "DTS-ES"; break;
                            default: ati.Codec = atrack.Format; break;
                        }
                    }
                    else
                        ati.Codec = atrack.Format;
                    ati.NbChannels = atrack.ChannelsString;
                    ati.ChannelPositions = atrack.ChannelPositionsString2;
                    ati.SamplingRate = atrack.SamplingRateString;
                    int delay = 0;
                    Int32.TryParse(atrack.Delay, out delay);
                    ati.Delay = delay;
                    // gets SBR/PS flag from AAC streams
                    if (atrack.Format == "AAC")
                    {
                        ati.AACFlag = 0;
                        if (atrack.FormatSettingsSBR == "Yes")
                        {
                            if (atrack.FormatSettingsPS == "Yes")
                                ati.AACFlag = 2;
                            else 
                                ati.AACFlag = 1;
                        }
                        if (atrack.SamplingRate == "24000")
                        {
                            if ((atrack.Channels == "2") || (atrack.Channels == "1")) // workaround for raw aac
                                ati.AACFlag = 1;
                        }
                    }
                    ati.Language = atrack.LanguageString;
                    _AudioInfo.Tracks.Add(ati);
                }

                //subtitle detection
                foreach (TextTrack oTextTrack in info.Text)
                {
                    int mmgTrackID = 0;
                    Int32.TryParse(oTextTrack.StreamOrder, out mmgTrackID);
                    SubtitleTrackInfo oTrack = new SubtitleTrackInfo(mmgTrackID, oTextTrack.LanguageString, oTextTrack.Title);
                    oTrack.DefaultTrack = oTextTrack.DefaultString.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals("yes");
                    oTrack.ForcedTrack = oTextTrack.ForcedString.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals("yes");
                    oTrack.SourceFileName = file;
                    oTrack.Codec = oTextTrack.CodecString;
                    oTrack.ContainerType = _strContainer;
                    int delay = 0;
                    Int32.TryParse(oTextTrack.Delay, out delay);
                    oTrack.Delay = delay;
                    _SubtitleInfo.Tracks.Add(oTrack);
                }

                // video detection
                _VideoInfo.HasVideo = (info.Video.Count > 0);
                if (_VideoInfo.HasVideo)
                {
                    MediaInfoWrapper.VideoTrack track = info.Video[0];
                    checked
                    {
                        int _trackID = 0;
                        Int32.TryParse(track.ID, out _trackID);
                        int _mmgTrackID = 0;
                        Int32.TryParse(track.StreamOrder, out _mmgTrackID);

                        VideoTrackInfo videoInfo = new VideoTrackInfo(_trackID, _mmgTrackID, track.LanguageString, track.Title, track.CodecString);
                        videoInfo.ContainerType = _strContainer;
                        _VideoInfo.Track = videoInfo;

                        _VideoInfo.Width = (ulong)easyParseInt(track.Width).Value;
                        _VideoInfo.Height = (ulong)easyParseInt(track.Height).Value;
                        _VideoInfo.FrameCount = (ulong)(easyParseInt(track.FrameCount) ?? 0);
                        _VideoInfo.FPS = (easyParseDouble(track.FrameRate) ?? easyParseDouble(track.FrameRateOriginal) ?? 99);
                        _VideoInfo.ScanType = track.ScanTypeString;
                        _VideoInfo.Codec = getVideoCodec(track.Codec);
                        if (_VideoInfo.Codec == null)
                            _VideoInfo.Codec = getVideoCodec(track.Format); // sometimes codec info is not available, check the format then...
                        _VideoInfo.Type = getVideoType(_VideoInfo.Codec, cType, file);
                        Dar? dar = null;
                        if (_VideoInfo.Width == 720 && (_VideoInfo.Height == 576 || _VideoInfo.Height == 480))
                        {
                            if (!MainForm.Instance.Settings.UseITUValues)
                            {
                                if (track.AspectRatioString.Equals("16:9"))
                                    dar = Dar.STATIC16x9;
                                else if (track.AspectRatioString.Equals("4:3"))
                                    dar = Dar.STATIC4x3; 
                            }
                            else if (_VideoInfo.Height == 576)
                            {
                                if (track.AspectRatioString.Equals("16:9"))
                                    dar = Dar.ITU16x9PAL;
                                else if (track.AspectRatioString.Equals("4:3"))
                                    dar = Dar.ITU4x3PAL;
                            }
                            else
                            {
                                if (track.AspectRatioString.Equals("16:9"))
                                    dar = Dar.ITU16x9NTSC;
                                else if (track.AspectRatioString.Equals("4:3"))
                                    dar = Dar.ITU4x3NTSC;
                            }
                        }
                        if (dar == null)
                        {
                            dar = new Dar((decimal?)easyParseDouble(track.AspectRatio), _VideoInfo.Width, _VideoInfo.Height);
                        }
                        _VideoInfo.DAR = (Dar)dar;
                    }
                }
                info.Dispose();
            }
            catch (Exception ex)
            {
                if (oLog == null)
                    oLog = MainForm.Instance.Log.Info("MediaInfo");
                oLog.LogValue("MediaInfo - Unhandled Error", ex, ImageType.Error);
            }
        }

        private void WriteSourceInformation(MediaInfo oInfo, String strFile, LogItem infoLog)
        {
            try
            {
                // general track
                foreach (MediaInfoWrapper.GeneralTrack t in oInfo.General)
                {
                    LogItem oTrack = new LogItem("General");
                      
                    oTrack.Info("Format: " + t.Format);
                    oTrack.Info("FormatString: " + t.FormatString);
                    oTrack.Info("FileSize: " + t.FileSize);
                    oTrack.Info("PlayTime: " + t.PlayTimeString3);
                    if (_VideoInfo.PGCCount > 0)
                    {
                        oTrack.Info("PGCCount: " + _VideoInfo.PGCCount);
                        oTrack.Info("PGCNumber: " + _VideoInfo.PGCNumber);
                    }

                    infoLog.Add(oTrack);
                }

                // video track
                foreach (MediaInfoWrapper.VideoTrack t in oInfo.Video)
                {
                    LogItem oTrack = new LogItem("Video");

                    oTrack.Info("ID: " + t.ID);
                    oTrack.Info("StreamOrder: " + t.StreamOrder);
                    oTrack.Info("Width: " + t.Width);
                    oTrack.Info("Height: " + t.Height);
                    oTrack.Info("FrameCount: " + t.FrameCount);
                    oTrack.Info("FrameRate: " + t.FrameRate);
                    oTrack.Info("FrameRateOriginal: " + t.FrameRateOriginal);
                    oTrack.Info("FrameRateMode: " + t.FrameRateMode);
                    oTrack.Info("ScanType: " + t.ScanTypeString);
                    oTrack.Info("Codec: " + t.Codec);
                    oTrack.Info("CodecString: " + t.CodecString);
                    oTrack.Info("Format: " + t.Format);
                    oTrack.Info("AspectRatio: " + t.AspectRatio);
                    oTrack.Info("AspectRatioString: " + t.AspectRatioString);
                    oTrack.Info("Delay: " + t.Delay);
                    oTrack.Info("Title: " + t.Title);
                    oTrack.Info("Language: " + t.Language);
                    oTrack.Info("LanguageString: " + t.LanguageString);
                    oTrack.Info("Default: " + t.Default);
                    oTrack.Info("DefaultString: " + t.DefaultString);
                    oTrack.Info("Forced: " + t.Forced);
                    oTrack.Info("ForcedString: " + t.ForcedString);

                    infoLog.Add(oTrack);
                }

                // audio track
                foreach (MediaInfoWrapper.AudioTrack t in oInfo.Audio)
                {
                    LogItem oTrack = new LogItem("Audio");

                    oTrack.Info("ID: " + t.ID);
                    oTrack.Info("StreamOrder: " + t.StreamOrder);
                    oTrack.Info("Format: " + t.Format);
                    oTrack.Info("FormatProfile: " + t.FormatProfile);
                    oTrack.Info("FormatSettingsSBR: " + t.FormatSettingsSBR);
                    oTrack.Info("FormatSettingsPS: " + t.FormatSettingsPS);
                    oTrack.Info("SamplingRate: " + t.SamplingRate);
                    oTrack.Info("SamplingRateString: " + t.SamplingRateString);
                    oTrack.Info("Channels: " + t.Channels);
                    oTrack.Info("ChannelsString: " + t.ChannelsString);
                    oTrack.Info("ChannelPositionsString2: " + t.ChannelPositionsString2);
                    oTrack.Info("BitRateMode: " + t.BitRateMode);
                    oTrack.Info("Delay: " + t.Delay);
                    oTrack.Info("Title: " + t.Title);
                    oTrack.Info("Language: " + t.Language);
                    oTrack.Info("LanguageString: " + t.LanguageString);
                    oTrack.Info("Default: " + t.Default);
                    oTrack.Info("DefaultString: " + t.DefaultString);
                    oTrack.Info("Forced: " + t.Forced);
                    oTrack.Info("ForcedString: " + t.ForcedString);

                    infoLog.Add(oTrack);
                }

                // text track
                foreach (MediaInfoWrapper.TextTrack t in oInfo.Text)
                {
                    LogItem oTrack = new LogItem("Text");

                    oTrack.Info("ID: " + t.ID);
                    oTrack.Info("StreamOrder: " + t.StreamOrder);
                    oTrack.Info("Codec: " + t.Codec);
                    oTrack.Info("CodecString: " + t.CodecString);
                    oTrack.Info("Delay: " + t.Delay);
                    oTrack.Info("Title: " + t.Title);
                    oTrack.Info("Language: " + t.Language);
                    oTrack.Info("LanguageString: " + t.LanguageString);
                    oTrack.Info("Default: " + t.Default);
                    oTrack.Info("DefaultString: " + t.DefaultString);
                    oTrack.Info("Forced: " + t.Forced);
                    oTrack.Info("ForcedString: " + t.ForcedString);

                    infoLog.Add(oTrack);
                }

                // chapter track
                foreach (MediaInfoWrapper.ChaptersTrack t in oInfo.Chapters)
                {
                    LogItem oTrack = new LogItem("Chapters");

                    oTrack.Info("ID: " + t.ID);
                    oTrack.Info("StreamOrder: " + t.StreamOrder);
                    oTrack.Info("Codec: " + t.Codec);
                    oTrack.Info("Inform: " + t.Inform);
                    oTrack.Info("Title: " + t.Title);
                    oTrack.Info("Language: " + t.Language);
                    oTrack.Info("LanguageString: " + t.LanguageString);
                    oTrack.Info("Default: " + t.Default);
                    oTrack.Info("DefaultString: " + t.DefaultString);
                    oTrack.Info("Forced: " + t.Forced);
                    oTrack.Info("ForcedString: " + t.ForcedString);

                    infoLog.Add(oTrack);
                }                
            }
            catch (Exception ex)
            {
                infoLog.LogValue("Error parsing media file", ex, ImageType.Information);
            }
        }

        private void CorrectSourceInformation(ref MediaInfo oInfo, String strFile, LogItem infoLog, int iPGCNumber)
        {
            try
            {
                if (oInfo.Video.Count > 0 && Path.GetExtension(strFile.ToLower(System.Globalization.CultureInfo.InvariantCulture)) == ".vob")
                {
                    string ifoFile;
                    if (Path.GetFileName(strFile).ToUpper(System.Globalization.CultureInfo.InvariantCulture).Substring(0, 4) == "VTS_")
                        ifoFile = strFile.Substring(0, strFile.LastIndexOf("_")) + "_0.IFO";
                    else
                        ifoFile = Path.ChangeExtension(strFile, ".IFO");

                    if (File.Exists(ifoFile))
                    {
                        // DVD Input File
                        if (infoLog != null)
                            infoLog.Info("DVD source detected. Using IFO file: " + ifoFile);

                        // PGC handling
                        int iPGCCount = (int)IFOparser.getPGCnb(ifoFile);
                        _VideoInfo.PGCCount = iPGCCount;
                        if (iPGCNumber < 1 || iPGCNumber > iPGCCount)
                            _VideoInfo.PGCNumber = 1;
                        else
                            _VideoInfo.PGCNumber = iPGCNumber;

                        // AR information may be false in VOB, use IFO instead
                        string strResult = IFOparser.GetVideoAR(ifoFile);
                        if (!String.IsNullOrEmpty(strResult))
                            oInfo.Video[0].AspectRatioString = strResult;

                        // audio languages are not present in VOB, use IFO instead
                        for (int counter = 0; counter < oInfo.Audio.Count; counter++)
                        {
                            MediaInfoWrapper.AudioTrack atrack = oInfo.Audio[counter];
                            if (String.IsNullOrEmpty(atrack.LanguageString))
                                atrack.LanguageString = IFOparser.getAudioLanguage(ifoFile, counter);
                        }

                        // subtitle information is wrong in VOB, use IFO instead
                        oInfo.Text.Clear();
                        string[] arrSubtitle = IFOparser.GetSubtitlesStreamsInfos(ifoFile, iPGCNumber, false);      
                        foreach (string strSubtitle in arrSubtitle)
                        {
                            TextTrack oTextTrack = new TextTrack();
                            oTextTrack.StreamOrder = Int32.Parse(strSubtitle.Substring(1, 2)).ToString();
                            string[] strLanguage = strSubtitle.Split('-');
                            oTextTrack.LanguageString = strLanguage[1].Trim();
                            if (strSubtitle.IndexOf('-', 7) > 0)
                                oTextTrack.Title = strSubtitle.Substring(7);
                            if (strSubtitle.ToLower(System.Globalization.CultureInfo.InvariantCulture).Contains("force"))
                                oTextTrack.ForcedString = "yes";
                            oTextTrack.CodecString = SubtitleType.VOBSUB.ToString();
                            oInfo.Text.Add(oTextTrack);
                        }
                    }
                }
                else if (oInfo.Audio.Count == 0 && oInfo.Video.Count == 0 && Path.GetExtension(strFile).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".avs"))
                {
                    // AVS Input File
                    if (infoLog != null)
                        infoLog.Info("AVS input file detected. Getting media information from AviSynth.");

                    using (AvsFile avi = AvsFile.OpenScriptFile(strFile))
                    {
                        MediaInfoWrapper.VideoTrack oVideo = new MediaInfoWrapper.VideoTrack();
                        MediaInfoWrapper.AudioTrack oAudio = new MediaInfoWrapper.AudioTrack();

                        oInfo.General[0].Format = "AVS";
                        oInfo.General[0].FormatString = "AviSynth Script";
                        if (avi.Clip.HasVideo || avi.Clip.HasAudio)
                            oInfo.General[0].PlayTimeString3 = (TimeSpan.FromMilliseconds((avi.VideoInfo.FrameCount / avi.VideoInfo.FPS) * 1000)).ToString();

                        if (avi.Clip.HasVideo)
                        {
                            oVideo.ID = "0";
                            oVideo.Width = avi.VideoInfo.Width.ToString();
                            oVideo.Height = avi.VideoInfo.Height.ToString();
                            oVideo.FrameCount = avi.VideoInfo.FrameCount.ToString();
                            oVideo.FrameRate = avi.VideoInfo.FPS.ToString();
                            _VideoInfo.FPS_D = avi.VideoInfo.FPS_D;
                            _VideoInfo.FPS_N = avi.VideoInfo.FPS_N;
                            if (avi.Clip.interlaced_frame > 0)
                                oVideo.ScanTypeString = "Interlaced";
                            else
                                oVideo.ScanTypeString = "Progressive";
                            oVideo.Codec = "AVS Video";
                            oVideo.CodecString = "AVS";
                            oVideo.Format = "AVS";
                            oVideo.AspectRatio = avi.VideoInfo.DAR.ToString();
                            oVideo.Delay = "0";
                            oInfo.Video.Add(oVideo);
                        }

                        if (avi.Clip.HasAudio)
                        {
                            oAudio.ID = "0";
                            oAudio.Format = "AVS";
                            oAudio.SamplingRate = avi.Clip.AudioSampleRate.ToString();
                            oAudio.SamplingRateString = avi.Clip.AudioSampleRate.ToString();
                            oAudio.Channels = avi.Clip.ChannelsCount.ToString();
                            oAudio.ChannelsString = avi.Clip.ChannelsCount.ToString() + " channels";
                            oAudio.ChannelPositionsString2 = AudioUtil.getChannelPositionsFromAVSFile(strFile);
                            oAudio.BitRateMode = "CBR";
                            oAudio.Delay = "0";
                            oInfo.Audio.Add(oAudio);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                infoLog.LogValue("Error parsing media file", ex, ImageType.Information);
            }
        }

        #region methods
        /// <summary>checks if the file is a MKV file and has chapters</summary>
        /// <returns>true if MKV has chapters, false if not</returns>
        public bool hasMKVChapters()
        {
            if (cType != ContainerType.MKV)
                return false;

            if (_MkvInfo == null)
                _MkvInfo = new MkvInfo(_file, ref _Log);

            return _MkvInfo.HasChapters;
        }

        /// <summary>extracts the MKV chapters</summary>
        /// <returns>true if chapters have been extracted, false if not</returns>
        public bool extractMKVChapters(string strChapterFile)
        {
            if (cType != ContainerType.MKV)
                return false;

            if (_MkvInfo == null)
                return false;

            return _MkvInfo.extractChapters(strChapterFile);
        }

        /// <summary>checks if the file is indexable by DGIndexNV</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isDGIIndexable()
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            // check if the indexer and the license file are available
            if (!MainForm.Instance.Settings.IsDGIIndexerAvailable())
                return false;

            // only AVC, VC1 and MPEG2 are supported
            if (!_VideoInfo.Track.Codec.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("AVC") &&
                !_VideoInfo.Track.Codec.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("VC-1") &&
                !_VideoInfo.Track.Codec.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-2 VIDEO"))
                return false;

            // only the following container formats are supported
            if (!_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MATROSKA") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-TS") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-PS") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG VIDEO") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("VC-1") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("AVC") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("BDAV"))
                return false;

            return true;
        }

        /// <summary>checks if the file is indexable by DGIndex</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isD2VIndexable()
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            // check if the indexer is available
            if (!File.Exists(MainForm.Instance.Settings.DgIndexPath))
                return false;

            // only MPEG1 and MPEG2 are supported
            if (!_VideoInfo.Track.Codec.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-1 VIDEO") &&
                !_VideoInfo.Track.Codec.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-2 VIDEO"))
                return false;

            // only the following container formats are supported
            if (!_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-TS") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-PS") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG VIDEO") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("BDAV"))
                return false;

            return true;
        }

        /// <summary>checks if the file is indexable by DGIndex</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isDGAIndexable()
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            // check if the indexer is available
            if (!File.Exists(MainForm.Instance.Settings.DgavcIndexPath))
                return false;

            // only AVC is supported
            if (!_VideoInfo.Track.Codec.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("AVC"))
                return false;

            // only the following container formats are supported
            if (!_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-TS") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-PS") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG VIDEO") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("AVC") &&
                !_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("BDAV"))
                return false;

            return true;
        }

        /// <summary>checks if the file is indexable by FFMSindex</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isFFMSIndexable()
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            // check if the indexer is available
            if (!File.Exists(MainForm.Instance.Settings.FFMSIndexPath))
                return false;

            // interlaced VC-1 is not supported
            if (_VideoInfo.Track.Codec.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("VC-1") &&
                !_VideoInfo.ScanType.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("PROGRESSIVE"))
                return false;

            // only the following container formats are supported
            if (_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MATROSKA") ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-TS") ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-PS") ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG VIDEO") ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("AVI") ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-4") ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("FLASH VIDEO") ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("OGG") ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("WINDOWS MEDIA") ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("BDAV"))
                return true;

            return false;
        }

        /// <summary>gets the recommended indexer</summary>
        /// <param name="oType">the recommended indexer</param>
        /// <returns>true if a indexer can be recommended, false if no indexer is available</returns>
        public bool recommendIndexer(out FileIndexerWindow.IndexType oType)
        {
            if (isDGIIndexable())
                oType = FileIndexerWindow.IndexType.DGI;
            else if (isDGAIndexable())
                oType = FileIndexerWindow.IndexType.DGA;
            else if (isD2VIndexable())
                oType = FileIndexerWindow.IndexType.D2V;
            else if (isFFMSIndexable())
                oType = FileIndexerWindow.IndexType.FFMS;
            else
            {
                oType = FileIndexerWindow.IndexType.FFMS;
                if (_indexerToUse == FileIndexerWindow.IndexType.NONE)
                    _indexerToUse = oType;
                return false;
            }
            if (_indexerToUse == FileIndexerWindow.IndexType.NONE)
                _indexerToUse = oType;
            return true;
        }

        /// <summary>gets the recommended indexer based on the priority</summary>
        /// <param name="oType">the recommended indexer</param>
        /// <param name="arrIndexer">the indexer priority</param>
        /// <returns>true if a indexer can be recommended, false if no indexer is available</returns>
        public bool recommendIndexer(List<string> arrIndexer)
        {
            FileIndexerWindow.IndexType oType = FileIndexerWindow.IndexType.NONE;
            foreach (string strIndexer in arrIndexer)
            {
                if (strIndexer.Equals(FileIndexerWindow.IndexType.FFMS.ToString()))
                {
                    if (isFFMSIndexable())
                    {
                        oType = FileIndexerWindow.IndexType.FFMS;
                        break;
                    }
                    continue;
                }
                else if (strIndexer.Equals(FileIndexerWindow.IndexType.DGI.ToString()))
                {
                    if (isDGIIndexable())
                    {
                        oType = FileIndexerWindow.IndexType.DGI;
                        break;
                    }
                    continue;
                }
                else if (strIndexer.Equals(FileIndexerWindow.IndexType.DGA.ToString()))
                {
                    if (isDGAIndexable())
                    {
                        oType = FileIndexerWindow.IndexType.DGA;
                        break;
                    }
                    continue;
                }
                else if (strIndexer.Equals(FileIndexerWindow.IndexType.D2V.ToString()))
                {
                    if (isD2VIndexable())
                    {
                        oType = FileIndexerWindow.IndexType.D2V;
                        break;
                    }
                    continue;
                }
            }

            if (oType != FileIndexerWindow.IndexType.NONE)
            {
                if (_indexerToUse == FileIndexerWindow.IndexType.NONE)
                    _indexerToUse = oType;
                return true;
            }

            return recommendIndexer(out oType);
        }

        private FileIndexerWindow.IndexType _indexerToUse;
        /// <summary>gets the recommended indexer</summary>
        public FileIndexerWindow.IndexType IndexerToUse
        {
            get { return _indexerToUse; }
            set { _indexerToUse = value; }
        }

        /// <summary>checks if the file can be demuxed with eac3to</summary>
        /// <returns>true if demuxable, false if not</returns>
        public bool isEac3toDemuxable()
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            // check if the indexer is available
            if (!File.Exists(MainForm.Instance.Settings.EAC3toPath))
                return false;

            // only the following container formats are supported
            // EVO is missing / not confirmed
            if (_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MATROSKA") ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-TS") ||
                (_strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("MPEG-PS") && Path.GetExtension(_file).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".vob")) ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("EVO") ||
                _strContainer.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Equals("BDAV"))
                return true;

            return false;
        }

        private static int? easyParseInt(string value)
        {
            try
            {
                return (int.Parse(value, culture));
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static double? easyParseDouble(string value)
        {
            try
            {
                return double.Parse(value, culture);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static ContainerType getContainerType(string codec, string description)
        {
            if (knownContainerTypes.ContainsKey(codec))
                return knownContainerTypes[codec];
            description = description.ToLower(System.Globalization.CultureInfo.InvariantCulture);
            foreach (string knownDescription in knownContainerDescriptions.Keys)
                if (description.Contains(knownDescription))
                    return knownContainerDescriptions[knownDescription];
            return null;
        }

        private static AudioCodec getAudioCodec(string description)
        {
            description = description.ToLower(System.Globalization.CultureInfo.InvariantCulture);
            foreach (string knownDescription in knownAudioDescriptions.Keys)
                if (description.Contains(knownDescription))
                    return knownAudioDescriptions[knownDescription];
            return null; ;
        }

        private static VideoCodec getVideoCodec(string description)
        {
            description = description.ToLower(System.Globalization.CultureInfo.InvariantCulture);
            foreach (string knownDescription in knownVideoDescriptions.Keys)
                if (description.Contains(knownDescription))
                    return knownVideoDescriptions[knownDescription];
            return null;
        }

        private static VideoType getVideoType(VideoCodec codec, ContainerType cft, string filename)
        {
            string extension = Path.GetExtension(filename).ToLower(System.Globalization.CultureInfo.InvariantCulture);
            foreach (VideoType t in ContainerManager.VideoTypes.Values)
            {
                if (t.ContainerType == cft && Array.IndexOf<VideoCodec>(t.SupportedCodecs, codec) >= 0 && "." + t.Extension == extension)
                    return t;
            }
            return null;
        }

        private static AudioType getAudioType(AudioCodec codec, ContainerType cft, string filename)
        {
            string extension = Path.GetExtension(filename).ToLower(System.Globalization.CultureInfo.InvariantCulture);
            ContainerType type = null;
            if (cft != null)
                type = cft;
            foreach (AudioType t in ContainerManager.AudioTypes.Values)
            {
                if (t.ContainerType == type && Array.IndexOf<AudioCodec>(t.SupportedCodecs, codec) >= 0 && "." + t.Extension == extension)
                    return t;
            }
            return null;
        }
        #endregion
        static MediaInfoFile()
        {
            knownVideoDescriptions = new Dictionary<string, VideoCodec>();

            knownVideoDescriptions.Add("divx 5", VideoCodec.ASP);
            knownVideoDescriptions.Add("divx 4", VideoCodec.ASP);
            knownVideoDescriptions.Add("divx 6", VideoCodec.ASP);
            knownVideoDescriptions.Add("3ivx", VideoCodec.ASP);
            knownVideoDescriptions.Add("xvid", VideoCodec.ASP);
            knownVideoDescriptions.Add("asp", VideoCodec.ASP);
            knownVideoDescriptions.Add("mpeg-4 adv simple", VideoCodec.ASP);
            knownVideoDescriptions.Add("avc", VideoCodec.AVC);
            knownVideoDescriptions.Add("h264", VideoCodec.AVC);
            knownVideoDescriptions.Add("h.264", VideoCodec.AVC);
            knownVideoDescriptions.Add("huffman", VideoCodec.HFYU);
            knownVideoDescriptions.Add("ffvh", VideoCodec.HFYU);
            knownVideoDescriptions.Add("mpeg-4v", VideoCodec.ASP);
            knownVideoDescriptions.Add("vc-1", VideoCodec.VC1);
            knownVideoDescriptions.Add("mpeg-2v", VideoCodec.MPEG2);

            knownAudioDescriptions = new Dictionary<string, AudioCodec>();
            knownAudioDescriptions.Add("aac", AudioCodec.AAC);
            knownAudioDescriptions.Add("ac3", AudioCodec.AC3);
            knownAudioDescriptions.Add("ac-3", AudioCodec.AC3);
            knownAudioDescriptions.Add("dts", AudioCodec.DTS);
            knownAudioDescriptions.Add("vorbis", AudioCodec.VORBIS);
            knownAudioDescriptions.Add(" l3", AudioCodec.MP3);
            knownAudioDescriptions.Add("mpeg-2 audio", AudioCodec.MP2);
            knownAudioDescriptions.Add("mpeg-4 audio", AudioCodec.AAC);
            knownAudioDescriptions.Add("flac", AudioCodec.FLAC);

            knownContainerTypes = new Dictionary<string, ContainerType>();
            knownContainerTypes.Add("AVI", ContainerType.AVI);
            knownContainerTypes.Add("Matroska", ContainerType.MKV);
            knownContainerTypes.Add("MPEG-4", ContainerType.MP4);
            knownContainerTypes.Add("3GPP", ContainerType.MP4);
            knownContainerTypes.Add("BDAV", ContainerType.M2TS);

            knownContainerDescriptions = new Dictionary<string,ContainerType>();
        }

        #region IMediaFile Members


        public bool HasAudio
        {
            get { return (_AudioInfo.Codecs.Length > 0); }
        }

        public bool HasVideo
        {
            get { return _VideoInfo.HasVideo; }
        }

        public bool CanReadVideo
        {
            get { return true; }
        }

        public bool CanReadAudio
        {
            get { return false; }
        }

        public IVideoReader GetVideoReader()
        {
            if (!VideoInfo.HasVideo || !CanReadVideo)
                throw new Exception("Can't read the video stream");
            if (videoSourceFile == null || videoReader == null)
                lock (this)
                {
                    if (videoSourceFile == null)
                    {
                        videoSourceFile = AvsFile.ParseScript(ScriptServer.GetInputLine(_file, null, false,
                        PossibleSources.directShow, false, false, false, VideoInfo.FPS, false));
                        videoReader = null;
                    }
                    if (videoReader == null)
                    {
                        videoReader = videoSourceFile.GetVideoReader();
                    }
                }
            return videoReader;
        }

        public IAudioReader GetAudioReader(int track)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string FileName
        {
            get { return _file; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (videoSourceFile != null)
            {
                videoSourceFile.Dispose();
                videoSourceFile = null;
                videoReader = null;
            }
        }

        #endregion
    }

    public class VideoInformation
    {
        public bool HasVideo;
        public ulong Width;
        public ulong Height;
        public Dar DAR;
        public ulong FrameCount;
        public double FPS;
        public int FPS_N;  // online needed for AVS file check
        public int FPS_D;  // online needed for AVS file check
        public int PGCNumber;
        public int PGCCount;

        private string _strVideoScanType;
        private VideoCodec _vCodec;
        private VideoType _vType;
        private VideoTrackInfo _videoInfo;

        public VideoInformation(bool hasVideo,
            ulong width, ulong height,
            Dar dar, ulong frameCount,
            double fps, int fps_n,
            int fps_d)
        {
            HasVideo = hasVideo;
            Width = width;
            Height = height;
            DAR = dar;
            FrameCount = frameCount;
            FPS = fps;
            FPS_N = fps_n;
            FPS_D = fps_d;
            PGCCount = 0;
            PGCNumber = 0;
        }

        public VideoTrackInfo Track
        {
            get { return _videoInfo; }
            set { _videoInfo = value; }
        }

        public string ScanType
        {
            get { return _strVideoScanType; }
            set { _strVideoScanType = value; }
        }

        public VideoType Type
        {
            get { return _vType; }
            set { _vType = value; }
        }

        public VideoCodec Codec
        {
            get { return _vCodec; }
            set { _vCodec = value; }
        }

        public VideoInformation Clone()
        {
            return (VideoInformation)this.MemberwiseClone();
        }
    }

    public class AudioInformation
    {
        private BitrateManagementMode[] _aBitrateModes;
        private List<AudioTrackInfo> _arrAudioTracks = new List<AudioTrackInfo>();
        private AudioCodec[] _aCodecs;
        private AudioType _aType;

        public AudioInformation()
        {
            
        }

        public AudioType Type
        {
            get { return _aType; }
            set { _aType = value; }
        }

        public AudioCodec[] Codecs
        {
            get { return _aCodecs; }
            set { _aCodecs = value; }
        }

        public BitrateManagementMode[] BitrateModes
        {
            get { return _aBitrateModes; }
            set { _aBitrateModes = value; }
        }

        public List<AudioTrackInfo> Tracks
        {
            get { return _arrAudioTracks; }
            set { _arrAudioTracks = value; }
        }

        /// <summary>gets the first audio track ID for muxing with mkvmerge</summary>
        /// <returns>trackID or 0</returns>
        public int GetFirstTrackID()
        {
            // check if the file contains a video audio track
            if (_aCodecs.Length == 0)
                return 0;

            return _arrAudioTracks[0].MMGTrackID;
        }

        public VideoInformation Clone()
        {
            return (VideoInformation)this.MemberwiseClone();
        }
    }

    public class SubtitleInformation
    {
        private List<SubtitleTrackInfo> _arrSubtitleTracks = new List<SubtitleTrackInfo>();

        public SubtitleInformation()
        {

        }

        public List<SubtitleTrackInfo> Tracks
        {
            get { return _arrSubtitleTracks; }
            set { _arrSubtitleTracks = value; }
        }

        /// <summary>gets the first subtitle track ID for muxing with mkvmerge</summary>
        /// <returns>trackID or 0</returns>
        public int GetFirstTrackID()
        {
            // check if the file is a subtitle file
            if (_arrSubtitleTracks.Count == 0)
                return 0;
            return _arrSubtitleTracks[0].MMGTrackID;
        }

        public VideoInformation Clone()
        {
            return (VideoInformation)this.MemberwiseClone();
        }
    }
}