using System;
using System.Collections.Generic;
using System.Text;
using MediaInfoWrapper;
using System.Globalization;

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
        private static readonly CultureInfo culture = new CultureInfo("en-us");
        #region variables
        private static Dictionary<string, VideoCodec> knownVideoDescriptions;
        private static Dictionary<string, AudioCodec> knownAudioDescriptions;
        private static Dictionary<string, ContainerType> knownContainerTypes;
        private static Dictionary<string, ContainerType> knownContainerDescriptions;
        private IMediaFile videoSourceFile = null;
        private IVideoReader videoReader = null;
        private int width, height, darX = 0, darY = 0, frameCount;
        private bool hasVideo;
        private double fps;
        private VideoCodec vCodec;
        private AudioCodec[] aCodecs;
        private AudioType aType;
        private VideoType vType;
        private ContainerType cType;
        private BitrateManagementMode[] aBitrateModes;
        private string file;
        #endregion
        #region properties
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
        public bool HasVideo
        {
            get { return hasVideo; }
        }
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }
        public int DARX
        {
            get { return darX; }
        }
        public int DARY
        {
            get { return darY; }
        }
        public int FrameCount
        {
            get { return frameCount; }
        }
        public double FPS
        {
            get { return fps; }
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
        #endregion


        public MediaInfoFile(string file)
        {
            this.file = file;
            MediaInfo info = new MediaInfo(file);
            hasVideo = (info.Video.Count > 0);
            if (hasVideo)
            {
                VideoTrack track = info.Video[0];
                width = easyParseInt(track.Width);
                height = easyParseInt(track.Height);
                frameCount = easyParseInt(track.FrameCount);
                fps = easyParseDouble(track.FrameRate);
                vCodec = getVideoCodec(track.Codec);
#warning should parse DAR properly, as commented below
                darX = width;
                darY = height; 
//                darX = easyParseInt(track.AspectRatio.Substring()
            }
            aCodecs = new AudioCodec[info.Audio.Count];
            aBitrateModes = new BitrateManagementMode[info.Audio.Count];
            int i = 0;
            foreach (AudioTrack track in info.Audio)
            {
                aCodecs[i] = getAudioCodec(track.Codec);
                if (track.BitRateMode == "VBR")
                    aBitrateModes[i] = BitrateManagementMode.VBR;
                else
                    aBitrateModes[i] = BitrateManagementMode.CBR;
            }
            if (info.General.Count < 1)
                cType = null;
            else
                cType = getContainerType(info.General[0].Format, info.General[0].FormatString);
            
            if (hasVideo)
                vType = getVideoType(vCodec, cType);
            else
                vType = null;
            
            if (aCodecs.Length == 1)
                aType = getAudioType(aCodecs[0], cType);
            else
                aType = null;

        }

        #region methods
        private int easyParseInt(string value)
        {
            try
            {
                return (int.Parse(value, culture));
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private double easyParseDouble(string value)
        {
            try
            {
                return double.Parse(value, culture);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private ContainerType getContainerType(string codec, string description)
        {
            if (knownContainerTypes.ContainsKey(codec))
                return knownContainerTypes[codec];
            description = description.ToLower();
            foreach (string knownDescription in knownContainerDescriptions.Keys)
                if (description.Contains(knownDescription))
                    return knownContainerDescriptions[knownDescription];
            return null;
        }

        private AudioCodec getAudioCodec(string description)
        {
            description = description.ToLower();
            foreach (string knownDescription in knownAudioDescriptions.Keys)
                if (description.Contains(knownDescription))
                    return knownAudioDescriptions[knownDescription];
            return null; ;
        }

        private VideoCodec getVideoCodec(string description)
        {
            description = description.ToLower();
            foreach (string knownDescription in knownVideoDescriptions.Keys)
                if (description.Contains(knownDescription))
                    return knownVideoDescriptions[knownDescription];
            return null;
        }

        private VideoType getVideoType(VideoCodec codec, ContainerType cft)
        {
            ContainerType type = null;
            if (cft != null)
                type = cft;
            foreach (VideoType t in ContainerManager.VideoTypes.Values)
            {
                if (t.ContainerType == type && Array.IndexOf<VideoCodec>(t.SupportedCodecs, codec) >= 0)
                    return t;
            }
            return null;
        }

        private AudioType getAudioType(AudioCodec codec, ContainerType cft)
        {
            ContainerType type = null;
            if (cft != null)
                type = cft;
            foreach (AudioType t in ContainerManager.AudioTypes.Values)
            {
                if (t.ContainerType == type && Array.IndexOf<AudioCodec>(t.SupportedCodecs, codec) >= 0)
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
            knownVideoDescriptions.Add("h.264", VideoCodec.AVC);
            knownVideoDescriptions.Add("huffman", VideoCodec.HFYU);
            knownVideoDescriptions.Add("ffvh", VideoCodec.HFYU);
            knownVideoDescriptions.Add("snow", VideoCodec.SNOW);

            knownAudioDescriptions = new Dictionary<string, AudioCodec>();
            knownAudioDescriptions.Add("aac", AudioCodec.AAC);
            knownAudioDescriptions.Add("ac3", AudioCodec.AC3);
            knownAudioDescriptions.Add("dts", AudioCodec.DTS);
            knownAudioDescriptions.Add("vorbis", AudioCodec.VORBIS);
            knownAudioDescriptions.Add(" l3", AudioCodec.MP3);
            knownAudioDescriptions.Add("mpeg-2 audio", AudioCodec.MP2);
            knownAudioDescriptions.Add("mpeg-4 audio", AudioCodec.AAC);

            knownContainerTypes = new Dictionary<string, ContainerType>();
            knownContainerTypes.Add("AVI", ContainerType.AVI);
            knownContainerTypes.Add("Matroska", ContainerType.MKV);
            knownContainerTypes.Add("MPEG-4", ContainerType.MP4);
            knownContainerTypes.Add("3GPP", ContainerType.MP4);

            knownContainerDescriptions = new Dictionary<string,ContainerType>();
        }

        #region IMediaFile Members


        public bool HasAudio
        {
            get { return (ACodecs.Length > 0); }
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
            if (!HasVideo || !CanReadVideo)
                throw new Exception("Can't read the video stream");
            if (videoSourceFile == null || videoReader == null)
                lock (this)
                {
                    if (videoSourceFile == null)
                    {
                        videoSourceFile = AvsFile.ParseScript(ScriptServer.GetInputLine(file, false,
                        PossibleSources.directShow, false, false, false, FPS));
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
