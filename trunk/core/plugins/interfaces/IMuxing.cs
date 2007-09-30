using System;
using System.Collections.Generic;
using System.Text;
using MeGUI.core.util;

namespace MeGUI
{
    public enum ProcessingLevel { NONE, SOME, ALL };
    /// <summary>
    /// interface for muxer providers
    /// this interface offers a number of methods used to find out if a muxjob can be processed
    /// and how it can be processed
    /// </summary>
    public interface IMuxing : IIDable
    {
        bool SupportsVideoCodec(VideoCodec codec);
        bool SupportsAudioCodec(AudioCodec codec);
        List<VideoType> GetSupportedVideoTypes();
        List<VideoCodec> GetSupportedVideoCodecs();
        List<AudioCodec> GetSupportedAudioCodecs();
        List<AudioType> GetSupportedAudioTypes();
        List<SubtitleType> GetSupportedSubtitleTypes();
        List<ChapterType> GetSupportedChapterTypes();
        List<ContainerType> GetSupportedContainers();
        List<ContainerType> GetSupportedContainerTypes();
        List<ContainerType> GetSupportedContainerInputTypes();
        /// <summary>
        /// checks all the given input stream types if they can be handled by this muxer
        /// </summary>
        /// <param name="inputTypes">all input stream types</param>
        /// <param name="handledInputTypes">input stream types this muxer can handle</param>
        /// <param name="unhandledInputTypes">input stream types this muxer cannot handle</param>
        /// <returns>ProcessingLevel indicating how much can be processed</returns>
        ProcessingLevel CanBeProcessed(MuxableType[] inputTypes, out List<MuxableType> handledInputTypes, out List<MuxableType> unhandledInputTypes);
        /// <summary>
        /// checks if the given input stream types and container input type can be handled by this muxer
        /// </summary>
        /// <param name="inputTypes">all input stream types</param>
        /// <param name="handledInputTypes">input stream types this muxer can handle</param>
        /// <param name="unhandledInputTypes">input stream types this muxer cannot handle</param>
        /// <returns>ProcessingLevel indicating how much can be processed</returns>
        ProcessingLevel CanBeProcessed(ContainerType[] inputContainer, MuxableType[] inputTypes, out List<MuxableType> handledInputTypes,
            out List<MuxableType> unhandledInputTypes);
        MuxerType MuxerType { get; }
        IJobProcessor GetMuxer(MeGUISettings settings);
        MuxCommandlineGenerator CommandlineGenerator { get;}
        string GetOutputTypeFilter(ContainerType containerType);
        string GetVideoInputFilter();
        string GetAudioInputFilter();
        string GetSubtitleInputFilter();
        string GetChapterInputFilter();
        string GetOutputTypeFilter();
        string GetMuxedInputFilter();
        string Name { get;}
        List<ContainerType> GetContainersInCommon(IMuxing iMuxing);
    }
    public interface IEncoding<TCodec, TType, TEncoderType>
    where TType : OutputType
    {
        List<TEncoderType> GetSupportedEncoderTypes();
        List<TCodec> GetSupportedCodecs();
        List<TType> GetSupportedOutputTypes(TEncoderType encoder);
        IJobProcessor GetEncoder(TEncoderType codec, TType type, MeGUISettings settings);
        string GetOutputTypeFilter(TEncoderType codec, TType type);
        string GetInputTypeFilter();
    }

    /// <summary>
    /// AudioStream is a simple container for audio streams and their related properties and settings
    /// </summary>
    public struct AudioStream
    {
        public string cutlist;
        public string path, output;
        public AudioCodecSettings settings;
        private AudioType type;
        private long size;
        private int delay;
        private BitrateManagementMode bitrateMode;
        /// <summary>
        /// gets / sets the size of this stream
        /// this is used for bitrate calculations
        /// </summary>
        public long SizeBytes
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
        /// <summary>
        /// gets / sets the delay of the audio stream (delay compared to the start of the video stream)
        /// </summary>
        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }
    }

    public class VideoStream
    {
        string input, output;
        ulong numberOfFrames;
        Dar? dar = null;
        double framerate;
        MuxableType videoType;
        VideoCodecSettings settings;
        public VideoStream()
        {
            input = "";
            numberOfFrames = 0;
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
        public Dar? DAR
        {
            get { return dar; }
            set { dar = value; }
        }
        public ulong NumberOfFrames
        {
          get { return numberOfFrames; }
          set { numberOfFrames = value; }
        }
        public double Framerate
        {
          get { return framerate; }
          set { framerate = value; }
        }
        public MuxableType VideoType
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
