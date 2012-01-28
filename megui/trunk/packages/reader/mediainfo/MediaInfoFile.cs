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
                    v.Info = new MeGUI.core.details.TrackInfo(t.Language, t.Title);

                    ulong width = ulong.Parse(t.Width);
                    ulong height = ulong.Parse(t.Height);
                    ulong frameCount = ulong.Parse(t.FrameCount);
                    double fps = double.Parse(t.FrameRate);

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
                    a.Info = new MeGUI.core.details.TrackInfo(t.Language, t.Title);

                    a.StreamInfo = new AudioInfo();

                    a.TrackNumber = uint.Parse(t.ID);

                    tracks.Add(a);
                }

                foreach (MediaInfoWrapper.TextTrack t in m.Text)
                {
                    SubtitleTrack s = new SubtitleTrack();
                    s.Codec = s.SCodec = getSubtitleCodec(t.Codec);
                    s.Info = new MeGUI.core.details.TrackInfo(t.Language, t.Title);
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

        private static readonly CultureInfo culture = new CultureInfo("en-us");

        #region variables
        private static Dictionary<string, VideoCodec> knownVideoDescriptions;
        private static Dictionary<string, AudioCodec> knownAudioDescriptions;
        private static Dictionary<string, ContainerType> knownContainerTypes;
        private static Dictionary<string, ContainerType> knownContainerDescriptions;
        private IMediaFile videoSourceFile = null;
        private IVideoReader videoReader = null;
        private VideoCodec vCodec;
        private AudioCodec[] aCodecs;
        private AudioType aType;
        private List<AudioTrackInfo> _arrAudioTracks = new List<AudioTrackInfo>();
        private List<int> _arrSubtitleTracks = new List<int>();
        private VideoType vType;
        private ContainerType cType;
        private string _strContainer = "";
        private string _strVideoScanType = "";
        private string _strVideoCodec = "";
        private BitrateManagementMode[] aBitrateModes;
        private string _file;
        private MediaFileInfo _MediaFileInfo;
        private bool bHasVideo;
        private int _trackID;
        #endregion
        #region properties
        public MediaFileInfo Info
        {
            get { return _MediaFileInfo; }
        }
        public AudioType AudioType
        {
            get { return aType; }
        }
        public VideoType VideoType
        {
            get { return vType; }
        }
        public ContainerType ContainerFileType
        {
            get { return cType; }
        }
        public VideoCodec VCodec
        {
            get { return vCodec; }
        }
        public AudioCodec[] ACodecs
        {
            get { return aCodecs; }
        }
        public BitrateManagementMode[] ABitrateModes
        {
            get { return aBitrateModes; }
        }
        public string ContainerFileTypeString
        {
            get { return _strContainer; }
        }
        public string VideoScanType
        {
            get { return _strVideoScanType;  }
        }
        public string VideoCodecString
        {
            get { return _strVideoCodec; }
        }
        public List<AudioTrackInfo> AudioTracks
        {
            get { return _arrAudioTracks; }
        }
        public int FirstVideoTrackID
        {
            get { return _trackID; }
        }
        #endregion

        public MediaInfoFile(string file, ref LogItem oLog)
        {
            GetSourceInformation(file, oLog);
        }

        public MediaInfoFile(string file)
        {
            GetSourceInformation(file, null);
        }

        /// <summary>
        /// gets information about a source using MediaInfo
        /// </summary>
        /// <param name="file">the file to be analyzed</param>
        /// <param name="oLog">the log item</param>
        public void GetSourceInformation(string file, LogItem oLog)
        {
            _file = file;

            try
            {
                // if an index file is used extract the real file name
                if (Path.GetExtension(file).ToLower().Equals(".d2v") ||
                    Path.GetExtension(file).ToLower().Equals(".dga") ||
                    Path.GetExtension(file).ToLower().Equals(".dgi"))
                {
                    using (StreamReader sr = new StreamReader(file, Encoding.Default))
                    {
                        string line = null;
                        int iLineCount = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            iLineCount++;
                            if (iLineCount == 3 && !Path.GetExtension(file).ToLower().Equals(".dgi"))
                            {
                                string strSourceFile = line;
                                if (Path.GetExtension(file).ToLower().Equals(".dgi"))
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
                }

                MediaInfo info = new MediaInfo(file);

                if (info.AudioCount == 0 && info.VideoCount == 0 && Path.GetExtension(file).ToLower().Equals(".avs"))
                {
                    // AVS Input File
                    using (AvsFile avi = AvsFile.OpenScriptFile(file))
                    {
                        MediaInfoWrapper.VideoTrack oVideo = new MediaInfoWrapper.VideoTrack();
                        MediaInfoWrapper.AudioTrack oAudio = new MediaInfoWrapper.AudioTrack();

                        info.General[0].Format = "AVS";
                        info.General[0].FormatString = "AviSynth Script";
                        if (avi.Info.HasVideo || avi.Info.HasAudio)
                            info.General[0].PlayTimeString3 = (TimeSpan.FromMilliseconds((avi.Info.FrameCount / avi.Info.FPS) * 1000)).ToString();

                        if (avi.Info.HasVideo)
                        {
                            oVideo.ID = "0";
                            oVideo.Width = avi.Info.Width.ToString();
                            oVideo.Height = avi.Info.Height.ToString();
                            oVideo.FrameCount = avi.Info.FrameCount.ToString();
                            oVideo.FrameRate = avi.Info.FPS.ToString();
                            if (avi.Clip.interlaced_frame > 0)
                                oVideo.ScanTypeString = "Interlaced";
                            else
                                oVideo.ScanTypeString = "Progressive";
                            oVideo.Codec = "AVS Video";
                            oVideo.CodecString = "AVS";
                            oVideo.Format = "AVS";
                            oVideo.AspectRatio = avi.Info.DAR.ToString();
                            oVideo.Delay = "0";
                            info.Video.Add(oVideo);
                        }

                        if (avi.Info.HasAudio)
                        {
                            oAudio.ID = "0";
                            oAudio.Format = "AVS";
                            oAudio.SamplingRate = avi.Clip.AudioSampleRate.ToString();
                            oAudio.SamplingRateString = avi.Clip.AudioSampleRate.ToString();
                            oAudio.Channels = avi.Clip.ChannelsCount.ToString();
                            oAudio.ChannelsString = avi.Clip.ChannelsCount.ToString() + " channels";
                            oAudio.ChannelPositionsString2 = AudioUtil.getChannelPositionsFromAVSFile(file);
                            oAudio.BitRateMode = "CBR";
                            oAudio.Delay = "0";
                            info.Audio.Add(oAudio);
                        }
                    }
                }

                if (oLog != null)
                    WriteSourceInformation(info, file, oLog);

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
                aCodecs = new AudioCodec[info.Audio.Count];
                aBitrateModes = new BitrateManagementMode[info.Audio.Count];

                if (aCodecs.Length == 1)
                    aType = getAudioType(aCodecs[0], cType, file);
                else
                    aType = null;

                for (int counter = 0; counter < info.Audio.Count; counter++)
                {
                    MediaInfoWrapper.AudioTrack atrack = info.Audio[counter];

                    aCodecs[counter] = getAudioCodec(atrack.Format);
                    if (atrack.BitRateMode == "VBR")
                        aBitrateModes[counter] = BitrateManagementMode.VBR;
                    else
                        aBitrateModes[counter] = BitrateManagementMode.CBR;

                    AudioTrackInfo ati = new AudioTrackInfo();
                    // DGIndex expects audio index not ID for TS
                    ati.ContainerType = info.General[0].Format;
                    ati.Index = counter;
                    int iID = 0;
                    if (info.General[0].Format == "CDXA/MPEG-PS")
                        // MediaInfo doesn't give TrackID for VCD, specs indicate only MP1L2 is supported
                        ati.TrackID = (0xC0 + counter);
                    else if (atrack.ID != "0" && atrack.ID != "" &&
                             (Int32.TryParse(atrack.ID, out iID) ||
                             (atrack.ID.Contains("-") && Int32.TryParse(atrack.ID.Split('-')[1], out iID))))
                        ati.TrackID = iID;
                    else
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
                    if (atrack.FormatProfile != "") // some tunings to have a more useful info instead of a typical audio Format
                    {
                        switch (atrack.FormatProfile)
                        {
                            case "Dolby Digital": ati.Type = "AC-3"; break;
                            case "HRA / Core":
                            case "HRA": ati.Type = "DTS-HD High Resolution"; break;
                            case "Layer 1": ati.Type = "MPA"; break;
                            case "Layer 2": ati.Type = "MP2"; break;
                            case "Layer 3": ati.Type = "MP3"; break;
                            case "LC": ati.Type = "AAC"; break;
                            case "MA":
                            case "MA / Core": ati.Type = "DTS-HD Master Audio"; break;
                            case "TrueHD": ati.Type = "TrueHD"; break;
                            case "ES": ati.Type = "DTS-ES"; break;
                            default: ati.Type = atrack.Format; break;
                        }
                    }
                    else
                        ati.Type = atrack.Format;
                    ati.NbChannels = atrack.ChannelsString;
                    ati.ChannelPositions = atrack.ChannelPositionsString2;
                    ati.SamplingRate = atrack.SamplingRateString;
                    if (atrack.LanguageString == "") // to retrieve Language 
                    {
                        if (Path.GetExtension(file.ToLower()) == ".vob")
                        {
                            string ifoFile;
                            string fileNameNoPath = Path.GetFileName(file);

                            // Languages are not present in VOB, so we check the main IFO
                            if (fileNameNoPath.Substring(0, 4) == "VTS_")
                                ifoFile = file.Substring(0, file.LastIndexOf("_")) + "_0.IFO";
                            else ifoFile = Path.ChangeExtension(file, ".IFO");

                            if (File.Exists(ifoFile))
                                atrack.LanguageString = IFOparser.getAudioLanguage(ifoFile, counter);
                        }
                    }
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
                    ati.TrackInfo = new TrackInfo(atrack.LanguageString, null);
                    _arrAudioTracks.Add(ati);
                }

                foreach (TextTrack oTextTrack in info.Text)
                {
                    int trackID = -1;
                    Int32.TryParse(oTextTrack.ID, out trackID);
                    _arrSubtitleTracks.Add(trackID);
                }
                    
                // video detection
                bHasVideo = (info.Video.Count > 0);
                if (bHasVideo)
                {
                    MediaInfoWrapper.VideoTrack track = info.Video[0];
                    checked
                    {
                        _trackID = 0;
                        Int32.TryParse(track.ID, out _trackID);
                        ulong width = (ulong)easyParseInt(track.Width).Value;
                        ulong height = (ulong)easyParseInt(track.Height).Value;
                        ulong frameCount = (ulong)(easyParseInt(track.FrameCount) ?? 0);
                        double fps = (easyParseDouble(track.FrameRate) ?? 25.0);
                        _strVideoScanType = track.ScanTypeString;
                        _strVideoCodec = track.CodecString;
                        vCodec = getVideoCodec(track.Codec);
                        if (vCodec == null)
                            vCodec = getVideoCodec(track.Format); // sometimes codec info is not available, check the format then...
                        vType = getVideoType(vCodec, cType, file);
                        Dar? dar = null;
                        if (width == 720 && (height == 576 || height == 480))
                        {
                            if (!MainForm.Instance.Settings.UseITUValues)
                            {
                                if (track.AspectRatioString.Equals("16:9"))
                                    dar = Dar.STATIC16x9;
                                else if (track.AspectRatioString.Equals("4:3"))
                                    dar = Dar.STATIC4x3; 
                            }
                            else if (height == 576)
                            {
                                if (track.AspectRatioString.Equals("16:9"))
                                    dar = Dar.ITU16x9PAL;
                                else if (track.AspectRatioString.Equals("4:3"))
                                    dar = Dar.ITU4x3PAL;
                            }
                            else
                            {
                                if (info.Video[0].AspectRatioString.Equals("16:9"))
                                    dar = Dar.ITU16x9NTSC;
                                else if (info.Video[0].AspectRatioString.Equals("4:3"))
                                    dar = Dar.ITU4x3NTSC;
                            }
                        }
                        if (dar == null)
                        {
                            dar = new Dar((decimal?)easyParseDouble(track.AspectRatio), width, height);
                        }
                        this._MediaFileInfo = new MediaFileInfo(bHasVideo, width, height, (Dar)dar, frameCount, fps, 0, 1, aCodecs.Length > 0);
                    }
                }
                else
                {
                    this._MediaFileInfo = new MediaFileInfo(false, 0, 0, Dar.A1x1, 0, 0, 0, 1, aCodecs.Length > 0);
                }
                info.Dispose();
            }
            catch (Exception ex)
            {
                if (oLog == null)
                    oLog = MainForm.Instance.Log.Info("MediaInfo");
                oLog.LogValue("MediaInfo - Unhandled Error", ex, ImageType.Error);
                this._MediaFileInfo = new MediaFileInfo(false, 0, 0, Dar.A1x1, 0, 0, 0, 1, false);
            }
        }

        private void WriteSourceInformation(MediaInfo oInfo, String strFile, LogItem oLog)
        {
            LogItem infoLog = oLog.LogValue("MediaInfo", String.Empty);
            infoLog.Info("File: " + strFile);

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

                    infoLog.Add(oTrack);
                }

                // text track
                foreach (MediaInfoWrapper.TextTrack t in oInfo.Text)
                {
                    LogItem oTrack = new LogItem("Text");

                    oTrack.Info("ID: " + t.ID);
                    oTrack.Info("StreamOrder: " + t.StreamOrder);
                    oTrack.Info("Codec: " + t.Codec);
                    oTrack.Info("Delay: " + t.Delay);
                    oTrack.Info("Title: " + t.Title);
                    oTrack.Info("Language: " + t.Language);
                    oTrack.Info("LanguageString: " + t.LanguageString);

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

                    infoLog.Add(oTrack);
                }                
            }
            catch (Exception ex)
            {
                infoLog.LogValue("Error parsing media file", ex, ImageType.Information);
            }
        }

        #region methods

        /// <summary>gets the first video track ID for muxing with mkvmerge</summary>
        /// <returns>trackID or 0</returns>
        public int GetFirstVideoTrackID()
        {
            // check if the file is a video file
            if (!HasVideo)
                return 0;

            int iCount = 0;
            if (_trackID > 0)
                iCount = _trackID - 1;
            return iCount;
        }

        /// <summary>gets the first audio track ID for muxing with mkvmerge</summary>
        /// <returns>trackID or 0</returns>
        public int GetFirstAudioTrackID()
        {
            // check if the file is a audio file
            if (!HasAudio)
                return 0;

            int iCount = 0;
            if (_arrAudioTracks[0].TrackID > 0)
                iCount = _arrAudioTracks[0].TrackID - 1;
            return iCount;
        }

        /// <summary>gets the first subtitle track ID for muxing with mkvmerge</summary>
        /// <returns>trackID or 0</returns>
        public int GetFirstSubtitleTrackID()
        {
            // check if the file is a subtitle file
            if (_arrSubtitleTracks.Count == 0)
                return 0;

            int iCount = 0;
            if (_arrSubtitleTracks[0] > 0)
                iCount = _arrSubtitleTracks[0] - 1;
            return iCount;
        }

        /// <summary>checks if the file is indexable by DGIndexNV</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isDGIIndexable()
        {
            // check if the file is a video file
            if (!bHasVideo)
                return false;

            // check if the indexer and the license file is available
            if (!VideoUtil.isDGIIndexerAvailable())
                return false;

            // only AVC, VC1 and MPEG2 are supported
            if (!_strVideoCodec.ToUpper().Equals("AVC") &&
                !_strVideoCodec.ToUpper().Equals("VC-1") &&
                !_strVideoCodec.ToUpper().Equals("MPEG-2 VIDEO"))
                return false;

            // only the following container formats are supported
            if (!_strContainer.ToUpper().Equals("MATROSKA") &&
                !_strContainer.ToUpper().Equals("MPEG-TS") &&
                !_strContainer.ToUpper().Equals("MPEG-PS") &&
                !_strContainer.ToUpper().Equals("MPEG VIDEO") &&
                !_strContainer.ToUpper().Equals("VC-1") &&
                !_strContainer.ToUpper().Equals("AVC") &&
                !_strContainer.ToUpper().Equals("BDAV"))
                return false;

            return true;
        }

        /// <summary>checks if the file is indexable by DGIndex</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isD2VIndexable()
        {
            // check if the file is a video file
            if (!bHasVideo)
                return false;

            // check if the indexer is available
            if (!File.Exists(MainForm.Instance.Settings.DgIndexPath))
                return false;

            // only MPEG1 and MPEG2 are supported
            if (!_strVideoCodec.ToUpper().Equals("MPEG-1 VIDEO") &&
                !_strVideoCodec.ToUpper().Equals("MPEG-2 VIDEO"))
                return false;

            // only the following container formats are supported
            if (!_strContainer.ToUpper().Equals("MPEG-TS") &&
                !_strContainer.ToUpper().Equals("MPEG-PS") &&
                !_strContainer.ToUpper().Equals("MPEG VIDEO") &&
                !_strContainer.ToUpper().Equals("BDAV"))
                return false;

            return true;
        }

        /// <summary>checks if the file is indexable by DGIndex</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isDGAIndexable()
        {
            // check if the file is a video file
            if (!bHasVideo)
                return false;

            // check if the indexer is available
            if (!File.Exists(MainForm.Instance.Settings.DgavcIndexPath))
                return false;

            // only AVC is supported
            if (!_strVideoCodec.ToUpper().Equals("AVC"))
                return false;

            // only the following container formats are supported
            if (!_strContainer.ToUpper().Equals("MPEG-TS") &&
                !_strContainer.ToUpper().Equals("MPEG-PS") &&
                !_strContainer.ToUpper().Equals("MPEG VIDEO") &&
                !_strContainer.ToUpper().Equals("AVC") &&
                !_strContainer.ToUpper().Equals("BDAV"))
                return false;

            return true;
        }

        /// <summary>checks if the file is indexable by FFMSindex</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isFFMSIndexable()
        {
            // check if the file is a video file
            if (!bHasVideo)
                return false;

            // check if the indexer is available
            if (!File.Exists(MainForm.Instance.Settings.FFMSIndexPath))
                return false;

            // interlaced VC-1 is not supported
            if (_strVideoCodec.ToUpper().Equals("VC-1") &&
                !_strVideoScanType.ToUpper().Equals("PROGRESSIVE"))
                return false;

            // only the following container formats are supported
            if (_strContainer.ToUpper().Equals("MATROSKA") ||
                _strContainer.ToUpper().Equals("MPEG-TS") ||
                _strContainer.ToUpper().Equals("MPEG-PS") ||
                _strContainer.ToUpper().Equals("MPEG VIDEO") ||
                _strContainer.ToUpper().Equals("AVI") ||
                _strContainer.ToUpper().Equals("MPEG-4") ||
                _strContainer.ToUpper().Equals("FLASH VIDEO") ||
                _strContainer.ToUpper().Equals("OGG") ||
                _strContainer.ToUpper().Equals("WINDOWS MEDIA") ||
                _strContainer.ToUpper().Equals("BDAV"))
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
                return false;
            }
            return true;
        }

        /// <summary>checks if the file can be demuxed with eac3to</summary>
        /// <returns>true if demuxable, false if not</returns>
        public bool isEac3toDemuxable()
        {
            // check if the file is a video file
            if (!bHasVideo)
                return false;

            // check if the indexer is available
            if (!File.Exists(MainForm.Instance.Settings.EAC3toPath))
                return false;

            // only the following container formats are supported
            // EVO is missing / not confirmed
            if (_strContainer.ToUpper().Equals("MATROSKA") ||
                _strContainer.ToUpper().Equals("MPEG-TS") ||
                (_strContainer.ToUpper().Equals("MPEG-PS") && Path.GetExtension(_file).ToLower().Equals(".vob")) ||
                _strContainer.ToUpper().Equals("EVO") ||
                _strContainer.ToUpper().Equals("BDAV"))
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
            description = description.ToLower();
            foreach (string knownDescription in knownContainerDescriptions.Keys)
                if (description.Contains(knownDescription))
                    return knownContainerDescriptions[knownDescription];
            return null;
        }

        private static AudioCodec getAudioCodec(string description)
        {
            description = description.ToLower();
            foreach (string knownDescription in knownAudioDescriptions.Keys)
                if (description.Contains(knownDescription))
                    return knownAudioDescriptions[knownDescription];
            return null; ;
        }

        private static VideoCodec getVideoCodec(string description)
        {
            description = description.ToLower();
            foreach (string knownDescription in knownVideoDescriptions.Keys)
                if (description.Contains(knownDescription))
                    return knownVideoDescriptions[knownDescription];
            return null;
        }

        private static VideoType getVideoType(VideoCodec codec, ContainerType cft, string filename)
        {
            string extension = Path.GetExtension(filename).ToLower();
            foreach (VideoType t in ContainerManager.VideoTypes.Values)
            {
                if (t.ContainerType == cft && Array.IndexOf<VideoCodec>(t.SupportedCodecs, codec) >= 0 && "." + t.Extension == extension)
                    return t;
            }
            return null;
        }

        private static AudioType getAudioType(AudioCodec codec, ContainerType cft, string filename)
        {
            string extension = Path.GetExtension(filename).ToLower();
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
            get { return (ACodecs.Length > 0); }
        }

        public bool HasVideo
        {
            get { return bHasVideo; }
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
            if (!Info.HasVideo || !CanReadVideo)
                throw new Exception("Can't read the video stream");
            if (videoSourceFile == null || videoReader == null)
                lock (this)
                {
                    if (videoSourceFile == null)
                    {
                        videoSourceFile = AvsFile.ParseScript(ScriptServer.GetInputLine(_file, false,
                        PossibleSources.directShow, false, false, false, Info.FPS, false));
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
}
