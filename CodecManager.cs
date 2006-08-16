using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    class CodecManager
    {
        // Audio Codecs
        public static readonly IAudioSettingsProvider NAAC = new NeroAACSettingsProvider();
        public static readonly IAudioSettingsProvider WAAC = new WinAmpAACSettingsProvider();
        public static readonly IAudioSettingsProvider Lame = new LameMP3SettingsProvider();
        public static readonly IAudioSettingsProvider FAAC = new FaacSettingsProvider();
        public static readonly IAudioSettingsProvider Vorbis = new OggVorbisSettingsProvider();
        public static readonly IAudioSettingsProvider AudX = new AudXSettingsProvider();
        public static readonly IAudioSettingsProvider AC3 = new AC3SettingsProvider();
        public static readonly IAudioSettingsProvider MP2 = new MP2SettingsProvider();


        // Video Codecs
        public static readonly IVideoSettingsProvider X264 = new X264SettingsProvider();
        public static readonly IVideoSettingsProvider Snow = new SnowSettingsProvider();
        public static readonly IVideoSettingsProvider Lavc = new LavcSettingsProvider();
        public static readonly IVideoSettingsProvider XviD = new XviDSettingsProvider();

        // All Audio Codecs
        public static readonly IAudioSettingsProvider[] ListOfAudioCodecs = new IAudioSettingsProvider[] { NAAC, WAAC, Lame, AudX, Vorbis, AC3, MP2, FAAC};
        // All Audio Codecs
        public static readonly IVideoSettingsProvider[] ListOfVideoCodecs = new IVideoSettingsProvider[] { Lavc, X264, Snow, XviD };

        public static VideoCodec VideoCodecFromEncoderType(VideoEncoderType vet)
        {
            foreach (IVideoSettingsProvider provider in ListOfVideoCodecs)
            {
                if (provider.EncoderType == vet)
                    return provider.CodecType;
            }
            return VideoCodec.OTHER;
        }

        public static AudioCodec AudioCodecFromEncoderType(AudioEncoderType aet)
        {
            foreach (IAudioSettingsProvider provider in ListOfAudioCodecs)
            {
                if (provider.EncoderType == aet)
                    return provider.CodecType;
            }
            return AudioCodec.OTHER;
        }
    }
    #region Video/Audio/Subtitle Types
    public class VideoType : OutputType
    {
        private VideoCodec[] supportedCodecs;

        public VideoCodec[] SupportedCodecs
        {
            get { return supportedCodecs; }
        }

        public VideoType(string name, string filterName, string extension, ContainerType containerType, VideoCodec supportedCodec)
            : this(name, filterName, extension, containerType, new VideoCodec[] { supportedCodec }) { }

        public VideoType(string name, string filterName, string extension, ContainerType containerType, VideoCodec[] supportedCodecs)
            : base(name, filterName, extension, containerType) {
                this.supportedCodecs = supportedCodecs;
        }
        public static readonly VideoType MP4 = new VideoType("MP4", "MP4 Files", "mp4", ContainerType.MP4, new VideoCodec[] { VideoCodec.ASP, VideoCodec.AVC });
        public static readonly VideoType RAWASP = new VideoType("RAW", "RAW MPEG-4 ASP Files", "m4v", ContainerType.NONE, VideoCodec.ASP);
        public static readonly VideoType RAWAVC = new VideoType("RAW", "RAW MPEG-4 AVC Files", "264", ContainerType.NONE, VideoCodec.AVC);
        public static readonly VideoType MKV = new VideoType("MKV", "Matroska Files", "mkv", ContainerType.MKV, new VideoCodec[] { VideoCodec.ASP, VideoCodec.AVC, VideoCodec.OTHER, VideoCodec.SNOW });
        public static readonly VideoType AVI = new VideoType("AVI", "AVI Files", "avi", ContainerType.AVI, new VideoCodec[] { VideoCodec.ASP, VideoCodec.AVC, VideoCodec.OTHER, VideoCodec.SNOW });
    }
    public class AudioType : OutputType
    {
        private AudioCodec[] supportedCodecs;

        public AudioCodec[] SupportedCodecs
        {
            get { return supportedCodecs; }
        }

        public AudioType(string name, string filterName, string extension, ContainerType containerType, AudioCodec supportedCodec)
            : this(name, filterName, extension, containerType, new AudioCodec[] { supportedCodec }) { }

        public AudioType(string name, string filterName, string extension, ContainerType containerType, AudioCodec[] supportedCodecs)
            : base(name, filterName, extension, containerType) {
                this.supportedCodecs = supportedCodecs;
        }
        public static readonly AudioType MP4AAC = new AudioType("MP4-AAC", "MP4 AAC Files", "mp4", ContainerType.MP4, AudioCodec.AAC);
        public static readonly AudioType RAWAAC = new AudioType("Raw-AAC", "RAW AAC Files", "aac", ContainerType.NONE, AudioCodec.AAC);
        public static readonly AudioType MP3 = new AudioType("MP3", "MP3 Files", "mp3", ContainerType.NONE, AudioCodec.MP3);
        public static readonly AudioType VORBIS = new AudioType("Ogg", "Ogg Vorbis Files", "ogg", ContainerType.NONE, AudioCodec.VORBIS);
        public static readonly AudioType AC3 = new AudioType("AC3", "AC3 Files", "ac3", ContainerType.NONE, AudioCodec.AC3);
        public static readonly AudioType MP2 = new AudioType("MP2", "MP2 Files", "mp2", ContainerType.NONE, AudioCodec.MP2);
        public static readonly AudioType DTS = new AudioType("DTS", "DTS Files", "dts", ContainerType.NONE, AudioCodec.DTS);
        public static readonly AudioType CBRMP3 = new AudioType("CBR MP3", "CBR MP3 Files", "mp3", ContainerType.NONE, AudioCodec.MP3);
        public static readonly AudioType VBRMP3 = new AudioType("VBR MP3", "VBR MP3 Files", "mp3", ContainerType.NONE, AudioCodec.MP3);
    }
    public class SubtitleType : OutputType
    {
        public SubtitleType(string name, string filterName, string extension, ContainerType containerType)
            : base(name, filterName, extension, containerType) { }
        public static readonly SubtitleType SUBRIP = new SubtitleType("Subrip", "Subrip Subtitle Files", "srt", ContainerType.NONE);
        public static readonly SubtitleType VOBSUB = new SubtitleType("Vobsub", "Vobsub Subtitle Files", "idx", ContainerType.NONE);
    }
    public class ChapterType : OutputType
    {
        public ChapterType(string name, string filterName, string extension, ContainerType containerType)
            : base(name, filterName, extension, containerType) { }
        public static readonly ChapterType OGG_TXT = new ChapterType("Ogg chapter", "Ogg chapter files", "txt", ContainerType.NONE);
    }
    public class ContainerFileType : OutputType
    {
        public ContainerFileType(string name, string filterName, string extension, ContainerType containerType)
            : base(name, filterName, extension, containerType) { }
        public static readonly ContainerFileType MP4 = new ContainerFileType("MP4", "MP4 Files", "mp4", ContainerType.MP4);
        public static readonly ContainerFileType MKV = new ContainerFileType("MKV", "Matroska Files", "mkv", ContainerType.MKV);
        public static readonly ContainerFileType AVI = new ContainerFileType("AVI", "AVI Files", "avi", ContainerType.AVI);
    }
    #endregion
    public class ContainerManager
    {
        private VideoType[] knownVideoTypes;
        private AudioType[] knownAudioTypes;
        private SubtitleType[] knownSubtitleTypes;
        private ChapterType[] knownChapterTypes;

        private static ContainerManager manager;
        /// <summary>
        /// containermanager contains a collection of all containers megui is aware of
        /// it solely serves the purpose of making container selection dropdowns possible
        /// </summary>
        private ContainerManager()
        {
            this.knownVideoTypes = new VideoType[] { VideoType.AVI, VideoType.RAWASP, VideoType.RAWAVC,
                VideoType.MP4, VideoType.MKV };
            this.knownAudioTypes = new AudioType[] { 
                AudioType.AC3, 
                AudioType.MP3,
                AudioType.MP4AAC,
                AudioType.RAWAAC,
                AudioType.DTS,
                AudioType.MP2,
                AudioType.CBRMP3,
                AudioType.VBRMP3,
                AudioType.VORBIS};
            this.knownSubtitleTypes = new SubtitleType[] { SubtitleType.SUBRIP, SubtitleType.VOBSUB };
            this.knownChapterTypes = new ChapterType[] { ChapterType.OGG_TXT };
        }
        public static ContainerManager GetContainerManager()
        {
            if (manager == null)
                manager = new ContainerManager();
            return manager;
        }
        /// <summary>
        /// returns all containers known to MeGUI
        /// </summary>
        public VideoType[] VideoTypes
        {
            get { return knownVideoTypes; }
        }

        public AudioType[] AudioTypes
        {
            get { return knownAudioTypes; }
        }
        public SubtitleType[] SubtitleTypes
        {
            get { return knownSubtitleTypes; }
        }
        public ChapterType[] ChapterTypes
        {
            get { return knownChapterTypes; }
        }
    }

    public class OutputType
    {
        public OutputType(string name, string filterName, string extension, ContainerType containerType)
        {
            this.name = name;
            this.filterName = filterName;
            this.extension = extension;
            this.containerType = containerType;
        }
        
        private ContainerType containerType;
        private string name, filterName, extension;
        /// <summary>
        /// used to display the output type in dropdowns
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.name;
        }
        public string OutputFilter
        {
            get { return "*." + extension; }
        }
        /// <summary>
        /// gets a valid filter string for file dialogs based on the known extension
        /// </summary>
        /// <returns></returns>
        public string OutputFilterString
        {
            get {return filterName + " (*." + extension + ")|*." + extension;}
        }
        /// <summary>
        /// gets the extension for this file type
        /// </summary>
        public string Extension
        {
            get {return this.extension;}
        }
        /// <summary>
        /// gets the underlying container type
        /// </summary>
        public ContainerType ContainerType
        {
            get { return this.containerType; }
        }
    }
}
