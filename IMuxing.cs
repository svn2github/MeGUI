using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public enum VideoCodec { LMP4, X264, SNOW, XVID, ANY };
    public enum AudioCodec { MP3, AAC, VORBIS, ANY };
    public enum ContainerType { AVI, NONE, MP4, MKV };

    public enum ProcessingLevel { NONE, SOME, ALL };
    /// <summary>
    /// interface for muxer providers
    /// this interface offers a number of methods used to find out if a muxjob can be processed
    /// and how it can be processed
    /// </summary>
    public interface IMuxing
    {
        List<VideoType> GetSupportedVideoTypes();
        List<AudioType> GetSupportedAudioTypes();
        List<SubtitleType> GetSupportedSubtitleTypes();
        List<ChapterType> GetSupportedChapterTypes();
        List<ContainerFileType> GetSupportedContainers();
        List<ContainerType> GetSupportedContainerTypes();
        List<ContainerFileType> GetSupportedInputContainers();
        List<ContainerType> GetSupportedContainerInputTypes();
        /// <summary>
        /// checks all the given input stream types if they can be handled by this muxer
        /// </summary>
        /// <param name="inputTypes">all input stream types</param>
        /// <param name="handledInputTypes">input stream types this muxer can handle</param>
        /// <param name="unhandledInputTypes">input stream types this muxer cannot handle</param>
        /// <returns>ProcessingLevel indicating how much can be processed</returns>
        ProcessingLevel CanBeProcessed(OutputType[] inputTypes, out List<OutputType> handledInputTypes, out List<OutputType> unhandledInputTypes);
        /// <summary>
        /// checks if the given input stream types and container input type can be handled by this muxer
        /// </summary>
        /// <param name="inputTypes">all input stream types</param>
        /// <param name="handledInputTypes">input stream types this muxer can handle</param>
        /// <param name="unhandledInputTypes">input stream types this muxer cannot handle</param>
        /// <returns>ProcessingLevel indicating how much can be processed</returns>
        ProcessingLevel CanBeProcessed(ContainerType[] inputContainer, OutputType[] inputTypes, out List<OutputType> handledInputTypes,
            out List<OutputType> unhandledInputTypes);
        MuxerType MuxerType { get; }
        Muxer GetMuxer(MeGUISettings settings);
        string GetOutputTypeFilter(ContainerType containerType);
        string GetVideoInputFilter();
        string GetAudioInputFilter();
        string GetSubtitleInputFilter();
        string GetChapterInputFilter();
        string GetOutputTypeFilter();
        string GetMuxedInputFilter();
        string Name { get;}
        List<ContainerFileType> GetContainersInCommon(IMuxing iMuxing);
    }
    public interface IVideoEncoding
    {
        List<VideoCodec> GetSupportedCodecs();
        List<VideoType> GetSupportedOutputTypes(VideoCodec codec);
        VideoEncoder GetEncoder(VideoCodec codec, VideoType type, string encoderPath);
        string GetOutputTypeFilter(VideoCodec codec, VideoType type);
        string GetInputTypeFilter();
    }
    public interface IAudioEncoding
    {
        List<AudioCodec> GetSupportedCodecs();
        List<AudioType> GetSupportedOutputTypes(AudioCodec codec);
        AudioEncoder GetEncoder(AudioCodec codec, AudioType type, MeGUISettings settings);
        string GetOutputTypeFilter(AudioCodec codec, AudioType type);
        string GetInputTypeFilter();
    }

    /// <summary>
    /// AudioStream is a simple container for audio streams and their related properties and settings
    /// </summary>
    public struct AudioStream
    {
        public string path, output;
        public AudioCodecSettings settings;
        private AudioType type;
        private long size;
        private BitrateManagementMode bitrateMode;
        /// <summary>
        /// gets / sets the size of this stream
        /// this is used for bitrate calculations
        /// </summary>
        public long Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }
        /// <summary>
        /// gets / sets the audio type
        /// </summary>
        public AudioType Type
        {
            get { return type; }
            set { type = value; }
        }
        /// <summary>
        /// gets / sets the bitrate mode
        /// if a settings object is available, the mode is taken from the settings, else it's taken from the 
        /// internal variable (which is set for bitrate calculation purposes)
        /// </summary>
        public BitrateManagementMode BitrateMode
        {
            get
            {
                if (settings != null)
                    return settings.BitrateMode;
                else
                    return bitrateMode;
            }
            set
            {
                bitrateMode = value;
            }
        }
    }

    public class VideoStream
    {
        string input, output;
        int numberOfFrames, parx, pary;
        double framerate;
        VideoType videoType;
        VideoCodecSettings settings;
        public VideoStream()
        {
            input = "";
            numberOfFrames = 0;
            parx = 0;
            pary = 0;
        }
        public string Output
        {
          get { return output; }
          set { output = value; }
        }
        public string Input
        {
          get { return input; }
          set { input = value; }
        }
        public int ParY
        {
          get { return pary; }
          set { pary = value; }
        }
        public int ParX
        {
          get { return parx; }
          set { parx = value; }
        }
        public int NumberOfFrames
        {
          get { return numberOfFrames; }
          set { numberOfFrames = value; }
        }
        public double Framerate
        {
          get { return framerate; }
          set { framerate = value; }
        }
        public VideoType VideoType
        {
            get { return videoType; }
            set { videoType = value; }
        }
        public VideoCodecSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }
    }
}
