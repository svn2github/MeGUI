using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public enum MuxerType { MP4BOX, MKVMERGE, AVC2AVI, AVIMUXGUI, DIVXMUX, FFMPEG, ATOMCHANGER };
    
    public class MuxProvider
    {
        List<IMuxing> registeredMuxers;
        MuxPathComparer comparer;
        VideoEncoderProvider vProvider = new VideoEncoderProvider();
        AudioEncoderProvider aProvider = new AudioEncoderProvider();
        public MuxProvider()
        {
            registeredMuxers = new List<IMuxing>();
            registeredMuxers.Add(new MP4BoxMuxerProvider());
            registeredMuxers.Add(new MKVMergeMuxerProvider());
            registeredMuxers.Add(new AVC2AVIMuxerProvider());
            registeredMuxers.Add(new DivXMuxProvider());
//            registeredMuxers.Add(new AVIMuxGUIMuxerProvider() as IMuxing);
            comparer = new MuxPathComparer();
        }

        public List<IMuxing> GetRegisteredMuxers()
        {
            return registeredMuxers;
        }

        public IMuxing GetMuxer(MuxerType type)
        {
            foreach (IMuxing muxing in registeredMuxers)
                if (muxing.MuxerType == type)
                    return muxing;
            return null;
        }

        public Muxer GetMuxer(MuxerType type, MeGUISettings settings)
        {
            IMuxing muxer = GetMuxer(type);
            if (muxer == null)
                return null;
            return muxer.GetMuxer(settings);
        }

        public MuxPath GetMuxPath(VideoCodec videoCodec, AudioCodec[] audioCodecs, OutputType[] dictatedTypes,
            ContainerType containerType, int initialFiles)
        {
            List<object> inputCodecs = new List<object>();
            inputCodecs.Add(videoCodec);
            foreach (AudioCodec ac in audioCodecs)
                inputCodecs.Add(ac);
            
            List<OutputType> decidedTypeList = new List<OutputType>();
            foreach (OutputType st in dictatedTypes)
                decidedTypeList.Add(st);
            return findBestMuxPathAndConfig(inputCodecs, decidedTypeList, containerType, initialFiles);
        }

        private MuxPath findBestMuxPathAndConfig(List<object> undecidedInputs, List<OutputType> decidedInputs, ContainerType containerType, int initialFiles)
        {
            if (undecidedInputs.Count == 0)
            {
                return getShortestMuxPath(new MuxPath(decidedInputs, initialFiles, containerType), decidedInputs, containerType);
            }
            else
            {
                List<MuxPath> allPaths = new List<MuxPath>();
                object undecidedInput = undecidedInputs[0];
                undecidedInputs.RemoveAt(0);
                if (undecidedInput is VideoCodec)
                {
                    VideoType[] allTypes = vProvider.GetSupportedOutput((VideoCodec)undecidedInput);
                    foreach (VideoType v in allTypes)
                    {
                        decidedInputs.Add(v);
                        MuxPath path = findBestMuxPathAndConfig(undecidedInputs, decidedInputs, containerType, initialFiles);
                        if (path != null)
                        {
                            allPaths.Add(path);
                        }
                        decidedInputs.Remove(v);
                    }
                }
                if (undecidedInput is AudioCodec)
                {
                    AudioType[] allTypes = aProvider.GetSupportedOutput((AudioCodec)undecidedInput);
                    foreach (AudioType a in allTypes)
                    {
                        decidedInputs.Add(a);
                        MuxPath path = findBestMuxPathAndConfig(undecidedInputs, decidedInputs, containerType, initialFiles);
                        if (path != null)
                        {
                            allPaths.Add(path);
                        }
                        decidedInputs.Remove(a);
                    }
                }
                undecidedInputs.Insert(0, undecidedInput);
                return comparer.GetBestMuxPath(allPaths);
            }
        }

        public MuxPath GetMuxPath(VideoType videoType, AudioType[] audioTypes, SubtitleType[] subtitleTypes,
            ContainerType containerType, int initialFiles)
        {
            List<OutputType> inputTypes = new List<OutputType>();
            List<MuxPath> possibleMuxPaths = new List<MuxPath>();
            inputTypes.Add(videoType);
            foreach (OutputType aType in audioTypes)
            {
                inputTypes.Add(aType);
            }
            foreach (OutputType sType in subtitleTypes)
            {
                inputTypes.Add(sType);
            }
            MuxPath shortestPath = getShortestMuxPath(new MuxPath(inputTypes, initialFiles, containerType), inputTypes, containerType);
            return shortestPath;
        }
        
        public bool CanBeMuxed(VideoType videoType, AudioType[] audioTypes, SubtitleType[] subtitleTypes,
            ContainerType containerType, int initialFiles)
        {
            MuxPath muxPath = GetMuxPath(videoType, audioTypes, subtitleTypes, containerType, initialFiles);
            if (muxPath != null)
                return true;
            else
                return false;
        }

        public bool CanBeMuxed(VideoCodec codec, AudioCodec[] audioCodecs, OutputType[] subtitleTypes,
            ContainerType containerType, int initialFiles)
        {
            MuxPath muxPath = GetMuxPath(codec, audioCodecs, subtitleTypes, containerType, initialFiles);
            if (muxPath != null)
                return true;
            else
                return false;
        }

        private MuxPath getShortestMuxPath(MuxPath currentMuxPath,
            List<IMuxing> remainingMuxers, ContainerType desiredContainerType)
        {
            List<MuxPath> allMuxPaths = new List<MuxPath>();

            if (currentMuxPath.IsCompleted())
                return currentMuxPath;

            List<IMuxing> newRemainingMuxers = new List<IMuxing>();
            newRemainingMuxers.AddRange(remainingMuxers);

            foreach (IMuxing muxer in remainingMuxers)
            {
                bool supportsInput = currentMuxPath[currentMuxPath.Length - 1].muxerInterface.GetContainersInCommon(muxer).Count > 0;
                if (supportsInput)
                {
                    MuxPath newMuxPath = currentMuxPath.Clone();

                    MuxPathLeg currentMPL = new MuxPathLeg();
                    currentMPL.muxerInterface = muxer;
                    currentMPL.handledInputTypes = new List<OutputType>();
                    currentMPL.unhandledInputTypes = new List<OutputType>();
                    newMuxPath.Add(currentMPL);
                    
                    newRemainingMuxers.Remove(muxer);
                    MuxPath shortestPath = getShortestMuxPath(newMuxPath, newRemainingMuxers, desiredContainerType);
                    if (shortestPath != null)
                    {
                        allMuxPaths.Add(shortestPath);
                    }
                    newRemainingMuxers.Add(muxer);
                }
            }
            return comparer.GetBestMuxPath(allMuxPaths);
        }

        /// <summary>
        /// Recurses to find the shortest mux path.
        /// </summary>
        /// Initial stage: if currentMuxPath is empty, it creates a first leg.
        /// Recursive step: It tries out adding all possible muxers to the current mux path,
        ///                 and calls itself with this extended mux path. It returns the shortest path.
        /// Final step: This will stop recursing if there are no muxers that can help, or if a muxer 
        ///                 is found in one step that finalizes the path. This is guaranteed to finish:
        ///                 if no progress is made, then it will not recurse. Otherwise, there is a finite
        ///                 amount of progress (progress is the number of streams muxed), so it will eventually
        ///                 stop progressing.
        /// <param name="currentMuxPath">Current mux path to be worked on</param>
        /// <param name="unhandledDesiredInputTypes">What remains to be muxed</param>
        /// <param name="desiredContainerType">Container type we are aiming at</param>
        /// <returns></returns>
        private MuxPath getShortestMuxPath(MuxPath currentMuxPath, 
            List<OutputType> unhandledDesiredInputTypes, ContainerType desiredContainerType)
        {
            List<OutputType> handledInputTypes = new List<OutputType>();
            List<OutputType> unhandledInputTypes = new List<OutputType>();
            List<MuxPath> allMuxPaths = new List<MuxPath>();

            if (currentMuxPath.IsCompleted())
                return currentMuxPath;
            
            foreach (IMuxing muxer in this.registeredMuxers)
            {
                ProcessingLevel level;
                if (currentMuxPath.Length > 0)
                {
                    level = muxer.CanBeProcessed(
                        currentMuxPath[currentMuxPath.Length - 1].muxerInterface.GetSupportedContainerTypes().ToArray(),
                        unhandledDesiredInputTypes.ToArray(), out handledInputTypes, out unhandledInputTypes);
                }
                else
                {
                    level = muxer.CanBeProcessed(unhandledDesiredInputTypes.ToArray(), out handledInputTypes, out unhandledInputTypes);
                }
                if (level != ProcessingLevel.NONE)
                {
                    MuxPath newMuxPath = currentMuxPath.Clone();

                    MuxPathLeg currentMPL = new MuxPathLeg();
                    currentMPL.muxerInterface = muxer;
                    currentMPL.handledInputTypes = handledInputTypes;
                    currentMPL.unhandledInputTypes = unhandledInputTypes;
                    newMuxPath.Add(currentMPL);

                    bool progressMadeThisStep = (handledInputTypes.Count > 0);

                    if (unhandledInputTypes.Count == 0)
                    {
                        List<IMuxing> allMuxers = new List<IMuxing>();
                        allMuxers.AddRange(this.registeredMuxers);
                        MuxPath shortestPath = getShortestMuxPath(newMuxPath, allMuxers, desiredContainerType);
                        if (shortestPath != null)
                            allMuxPaths.Add(shortestPath);
                    }
                    // Check if we have muxed anything more in. 
                    // Otherwise, we will get an endless string of the same muxer.
                    else if (progressMadeThisStep)
                    {
                        MuxPath shortestPath = getShortestMuxPath(newMuxPath, unhandledInputTypes, desiredContainerType);
                        if (shortestPath != null)
                            allMuxPaths.Add(shortestPath);
                    }
                }
                #region old
/*                if (index > previousMuxerIndex) // skip previously seen muxers to reduce complexity to n*n/2
                {
                    ProcessingLevel type = nextMuxer.CanBeProcessed(previousMuxer.GetSupportedContainerTypes().ToArray(),
                        unhandledDesiredInputTypes.ToArray(), out handledInputTypes, out unhandledInputTypes);
                    if (type != ProcessingLevel.NONE) // some progress
                    {
                        if (type == ProcessingLevel.ALL) // all the input is handled
                        {
                            MuxPathLeg mpl = new MuxPathLeg();
                            mpl.muxerInterface = nextMuxer;
                            mpl.handledInputTypes = handledInputTypes;
                            eligibleMuxPaths.Add(mpl);
                            if (nextMuxer.GetSupportedContainerTypes().Contains(desiredContainerType))// desired target format achieved
                            {
                                found = true;
                                break;
                            }
                            else // all input is handled, now we just need a muxer to the final destination
                            {
                                getMuxPath(eligibleMuxPaths, nextMuxer, index, new List<object>(), desiredContainerType);
                            }
                        }
                        else // the input is partially handled
                        {
                            MuxPathLeg mpl = new MuxPathLeg();
                            mpl.muxerInterface = nextMuxer;
                            mpl.handledInputTypes = handledInputTypes;
                            eligibleMuxPaths.Add(mpl);
                            getMuxPath(eligibleMuxPaths, nextMuxer, index, unhandledInputTypes, desiredContainerType);
                        }
                    }
                }
                else
                    index++;
 * */
#endregion
            }
            return comparer.GetBestMuxPath(allMuxPaths);
        }
        /// <summary>
        /// gets the video type from the container type
        /// if it cannot be identified, null is returned
        /// </summary>
        /// <param name="containerType">the container type</param>
        /// <returns>the video type</returns>
/*        private OutputType videoTypeFromContainerType(ContainerType containerType)
        {
            switch (containerType)
            {
                case ContainerType.AVI:
                    return VideoType.AVI;
                case ContainerType.MKV:
                    return VideoType.MKV;
                case ContainerType.MP4:
                    return VideoType.MP4;
            }
            return null;
        }*/

        public List<ContainerFileType> GetSupportedContainers()
        {
            List<ContainerFileType> supportedContainers = new List<ContainerFileType>();
            List<ContainerType> containerTypes = new List<ContainerType>();
            foreach (IMuxing muxerInterface in registeredMuxers)
            {
                List<ContainerFileType> outputTypes = muxerInterface.GetSupportedContainers();
                foreach (ContainerFileType type in outputTypes)
                {
                    if (!containerTypes.Contains(type.ContainerType))
                        supportedContainers.Add(type);
                    containerTypes.Add(type.ContainerType);
                }
            }
            return supportedContainers;
        }
        /// <summary>
        /// gets all the containers that can be supported given the video and a list of audio types
        /// this is used to limit the container dropdown in the autoencode window
        /// </summary>
        /// <param name="videoType">the desired video type</param>
        /// <param name="audioTypes">the desired audio types</param>
        /// <returns>a list of containers that can be supported</returns>
        public List<ContainerFileType> GetSupportedContainers(VideoType videoType, AudioType[] audioTypes, SubtitleType[] subTypes, int initialFiles)
        {
            List<ContainerFileType> supportedContainers = new List<ContainerFileType>();
            foreach (ContainerFileType cot in GetSupportedContainers())
            {
                if (CanBeMuxed(videoType, audioTypes, subTypes, cot.ContainerType, initialFiles))
                {
                    if (!supportedContainers.Contains(cot))
                        supportedContainers.Add(cot);
                }
            }
            return supportedContainers;
        }

        public List<ContainerFileType> GetSupportedContainers(VideoCodec videoCodec, AudioCodec[] audioCodecs, int initialFiles)
        {
            return GetSupportedContainers(videoCodec, audioCodecs, new List<OutputType>(), initialFiles);
        }

        public List<ContainerFileType> GetSupportedContainers(VideoCodec videoCodec, AudioCodec[] audioCodecs, List<OutputType> dictatedOutputTypes, int initialFiles)
        {
            List<ContainerFileType> supportedContainers = new List<ContainerFileType>();
            List<ContainerFileType> allKnownContainers = GetSupportedContainers();
            foreach (ContainerFileType cot in allKnownContainers)
            {
                if (CanBeMuxed(videoCodec, audioCodecs, dictatedOutputTypes.ToArray(), cot.ContainerType, initialFiles))
                {
                    if (!supportedContainers.Contains(cot))
                        supportedContainers.Add(cot);
                }
            }
            return supportedContainers;
        }
    }
    public struct MuxPathLeg
    {
        public IMuxing muxerInterface;
        public List<OutputType> handledInputTypes;
        public List<OutputType> unhandledInputTypes; // those remain for the next leg
    }
    #region muxer providers
    public class MP4BoxMuxerProvider : MuxerProvider
    {
        
        public MP4BoxMuxerProvider() : base()
        {
            supportedVideoTypes.Add(VideoType.RAWASP);
            supportedVideoTypes.Add(VideoType.RAWAVC);
            supportedVideoTypes.Add(VideoType.MP4);
            supportedAudioTypes.Add(AudioType.RAWAAC);
            supportedAudioTypes.Add(AudioType.MP4AAC);
            supportedAudioTypes.Add(AudioType.MP3);
            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            supportedChapterTypes.Add(ChapterType.OGG_TXT);
            supportedContainers.Add(ContainerFileType.MP4);
            supportedContainerInputTypes.Add(ContainerType.MP4);
            base.type = MuxerType.MP4BOX;
            maxFilesOfType = new int[] { 1, -1, -1 };
            generator = CommandLineGenerator.generateMP4BoxCommandline;
            name = "MP4 Muxer";
//            base.audioInputFilter = "All supported types (*.aac, *.mp3, *.mp4)|*.aac;*.mp3;*.mp4|RAW AAC Files (*.aac)|*.aac|MP3 Files (*.mp3)|*.mp3|MP4 Audio Files (*.mp4)|*.mp4";
//            base.videoInputFilter = "All supported types (*.m4v, *.264, *.mp4)|*.m4v;*.264;*.mp4|RAW MPEG-4 ASP Files (*.m4v)|*.m4v|RAW MPEG-4 AVC Files (*.264)|*.264|MP4 Files (*.mp4)|*.mp4";
//            base.subtitleInputFilter = "All supported types (*.srt)|*.srt";
        }

        public override Muxer GetMuxer(MeGUISettings settings)
        {
            return new MP4BoxMuxer(settings.Mp4boxPath);
        }
    }
    public class MKVMergeMuxerProvider : MuxerProvider
    {
        public MKVMergeMuxerProvider() : base()
        {
            supportedVideoTypes.Add(VideoType.AVI);
            supportedVideoTypes.Add(VideoType.MKV);
            supportedVideoTypes.Add(VideoType.MP4);
            supportedAudioTypes.Add(AudioType.RAWAAC);
            supportedAudioTypes.Add(AudioType.MP4AAC);
            supportedAudioTypes.Add(AudioType.MP3);
            supportedAudioTypes.Add(AudioType.VORBIS);
            supportedAudioTypes.Add(AudioType.MP2);
            supportedAudioTypes.Add(AudioType.AC3);
            supportedAudioTypes.Add(AudioType.DTS);
            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            supportedChapterTypes.Add(ChapterType.OGG_TXT);
            supportedContainers.Add(ContainerFileType.MKV);
            supportedContainerInputTypes.Add(ContainerType.MP4);
            supportedContainerInputTypes.Add(ContainerType.AVI);
            supportedContainerInputTypes.Add(ContainerType.MKV);
            maxFilesOfType = new int[] { -1, -1, -1 };
            base.type = MuxerType.MKVMERGE;
            generator = CommandLineGenerator.generateMkvmergeCommandline;
            name = "Mkv muxer";
//            base.audioInputFilter = "All supported types (*.aac, *.ac3, *.dts, *.mp2, *.mp3, *.mp4, *.ogg)|*.aac;*.ac3;*.dts;*.mp2;*.mp3;*.mp4;*.ogg|RAW AAC Files (*.aac)|*.aac|AC3 Files (*.ac3)|*.ac3|DTS Files (*.dts)|*.dts" +
//                "MP2 Files (*.mp2)|*.mp2|MP3 Files (*.mp3)|*.mp3|MP4 Audio Files (*.mp4)|*.mp4|Ogg Vorbis Files (*.ogg)|*.ogg";
//            base.videoInputFilter = "All supported types (*.avi, *.mkv, *.mp4)|*.avi;*.mkv;*.mp4|AVI Files (*.avi)|*.avi|Matroska Files (*.mkv)|*.mkv|MP4 Files (*.mp4)|*.mp4";
//            base.subtitleInputFilter = "All supported types (*.srt, *.idx)|*.srt;*.idx|Subrip Files (*.srt)|*.srt|VobSub Files (*.idx)|*.idx";
        }

        public override Muxer GetMuxer(MeGUISettings settings)
        {
            return new MkvMergeMuxer(settings.MkvmergePath);
        }
    }
    public class AVIMuxGUIMuxerProvider : MuxerProvider
    {
        public AVIMuxGUIMuxerProvider(): base()
        {
            supportedVideoTypes.Add(VideoType.AVI);
            supportedAudioTypes.Add(AudioType.AC3);
            supportedAudioTypes.Add(AudioType.DTS);
            supportedAudioTypes.Add(AudioType.MP3);
            supportedAudioTypes.Add(AudioType.VORBIS);
            supportedAudioTypes.Add(AudioType.RAWAAC);
            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            supportedContainers.Add(ContainerFileType.AVI);
            supportedContainerInputTypes.Add(ContainerType.AVI);
            maxFilesOfType = new int[] { 1, -1, -1 };
            base.type = MuxerType.AVIMUXGUI;
            name = "AVI Muxer";
            generator = CommandLineGenerator.generateAVIMuxCommandline;
        }

        public override Muxer GetMuxer(MeGUISettings settings)
        {
            throw new Exception("AVI-Mux GUI muxer not supported yet");
            //return new AVIMuxGUIMuxer(settings.AviMuxGUIPath);
        }
    }
    public class DivXMuxProvider : MuxerProvider
    {
        public DivXMuxProvider()
            : base()
        {
            supportedVideoTypes.Add(VideoType.AVI);
            supportedAudioTypes.Add(AudioType.AC3);
            supportedAudioTypes.Add(AudioType.MP3);
            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            supportedSubtitleTypes.Add(SubtitleType.VOBSUB);
            supportedContainerInputTypes.Add(ContainerType.AVI);
            supportedContainers.Add(ContainerFileType.AVI);
            maxFilesOfType = new int[] { 1, -1, -1 };
            base.type = MuxerType.DIVXMUX;
            generator = CommandLineGenerator.generateDivXMuxCommandline;
            name = "DivX AVI Muxer";
        }

        public override Muxer GetMuxer(MeGUISettings meguiSettings)
        {
            return new DivXMuxer(meguiSettings.DivXMuxPath);
        }
    }
    public class AVC2AVIMuxerProvider : MuxerProvider
    {
        public AVC2AVIMuxerProvider() : base()
        {
            supportedVideoTypes.Add(VideoType.RAWAVC);
            supportedContainers.Add(ContainerFileType.AVI);
            base.type = MuxerType.AVC2AVI;
            name = "AVC2AVI";
            maxFilesOfType = new int[] { 1, 0, 0 };
            generator = CommandLineGenerator.GenerateAVC2AVICommandline;
//            base.videoInputFilter = "RAW MPEG-4 AVC Files (*.264)|*.264";
        }

        public override Muxer GetMuxer(MeGUISettings settings)
        {
            return new Avc2AviMuxer(settings.Avc2aviPath);
        }
    }
    #endregion
    #region top level providers
    public abstract class MuxerProvider : IMuxing
    {
        protected List<VideoType> supportedVideoTypes;
        protected List<AudioType> supportedAudioTypes;
        protected List<SubtitleType> supportedSubtitleTypes;
        protected List<ChapterType> supportedChapterTypes;
        protected List<ContainerFileType> supportedContainers;
        protected List<ContainerType> supportedContainerInputTypes;
        protected MuxCommandlineGenerator generator;
        protected string videoInputFilter, audioInputFilter, subtitleInputFilter;
        protected int[] maxFilesOfType;
        protected string name;
        protected MuxerType type;
        public MuxerProvider()
        {
            supportedVideoTypes = new List<VideoType>();
            supportedAudioTypes = new List<AudioType>();
            supportedSubtitleTypes = new List<SubtitleType>();
            supportedChapterTypes = new List<ChapterType>();
            supportedContainers = new List<ContainerFileType>();
            supportedContainerInputTypes = new List<ContainerType>();
            videoInputFilter = audioInputFilter = subtitleInputFilter = "";
        }
        #region IMuxing Members
        public MuxCommandlineGenerator CommandlineGenerator
        {
            get { return generator; }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public MuxerType MuxerType
        {
            get { return type; }
        }

        public List<VideoType> GetSupportedVideoTypes()
        {
            return this.supportedVideoTypes;
        }

        public List<AudioType> GetSupportedAudioTypes()
        {
            return this.supportedAudioTypes;
        }

        public List<SubtitleType> GetSupportedSubtitleTypes()
        {
            return this.supportedSubtitleTypes;
        }

        public List<ChapterType> GetSupportedChapterTypes()
        {
            return this.supportedChapterTypes;
        }

        public List<ContainerFileType> GetSupportedContainers()
        {
            return this.supportedContainers;
        }

        public List<ContainerType> GetSupportedContainerTypes()
        {
            List<ContainerFileType> supportedOutputTypes = GetSupportedContainers();
            List<ContainerType> supportedContainers = new List<ContainerType>();
            foreach (ContainerFileType cot in supportedOutputTypes)
            {
                supportedContainers.Add(cot.ContainerType);
            }
            return supportedContainers;
        }

        public List<ContainerFileType> GetSupportedInputContainers()
        {
            List<ContainerFileType> supportedFileTypes = new List<ContainerFileType>();
            MuxProvider containerProvider = new MuxProvider();
            List<ContainerType> supportedInputTypes = GetSupportedContainerInputTypes();
            foreach (ContainerFileType cft in containerProvider.GetSupportedContainers())
            {
                if (supportedInputTypes.Contains(cft.ContainerType))
                    supportedFileTypes.Add(cft);
            }
            return supportedFileTypes;
        }
        
        public List<ContainerType> GetSupportedContainerInputTypes()
        {
            return this.supportedContainerInputTypes;
        }

        public List<ContainerFileType> GetContainersInCommon(IMuxing iMuxing)
        {
            List<ContainerFileType> supportedOutputTypes = GetSupportedContainers();
            List<ContainerType> nextSupportedInputTypes = iMuxing.GetSupportedContainerInputTypes();
            List<ContainerFileType> commonContainers = new List<ContainerFileType>();
            foreach (ContainerFileType eligibleType in supportedOutputTypes)
            {
                if (nextSupportedInputTypes.Contains(eligibleType.ContainerType))
                    commonContainers.Add(eligibleType);
            }
            return commonContainers;
        }

        public abstract Muxer GetMuxer(MeGUISettings meguiSettings);

        /// <summary>
        /// Returns the number of the type if it is supported, otherwise -1
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int getSupportedType(OutputType type)
        {
            if (type is VideoType && supportedVideoTypes.Contains((VideoType)type))
                return 0;
            if (type is AudioType && supportedAudioTypes.Contains((AudioType)type))
                return 1;
            if (type is SubtitleType && supportedSubtitleTypes.Contains((SubtitleType)type))
                return 2;
            return -1;
        }

        public ProcessingLevel CanBeProcessed(OutputType[] inputTypes, out List<OutputType> handledInputTypes,
            out List<OutputType> unhandledInputTypes)
        {
            handledInputTypes = new List<OutputType>();
            unhandledInputTypes = new List<OutputType>();
            int[] filesOfType = new int[3];
            foreach (OutputType inputType in inputTypes)
            {
                int type = getSupportedType(inputType);
                if (type >= 0)
                {
                    if (maxFilesOfType[type] < 0 // We ignore it in this case
                        || filesOfType[type] < maxFilesOfType[type])
                    {
                        handledInputTypes.Add(inputType);
                        filesOfType[type]++;
                    }
                    else
                        unhandledInputTypes.Add(inputType);
                }
                else
                    unhandledInputTypes.Add(inputType);
            }
            ProcessingLevel retval = ProcessingLevel.NONE;
            if (handledInputTypes.Count > 0)
                retval = ProcessingLevel.SOME;
            if (unhandledInputTypes.Count == 0 || inputTypes.Length == 0)
                retval = ProcessingLevel.ALL;
            return retval;
        }


        public ProcessingLevel CanBeProcessed(ContainerType[] inputContainers, OutputType[] inputTypes, out List<OutputType> handledInputTypes,
            out List<OutputType> unhandledInputTypes)
        {
            bool commonContainerFound = false;
            foreach (ContainerType inputContainer in inputContainers)
            {
                if (GetSupportedContainerInputTypes().Contains(inputContainer))
                {
                    commonContainerFound = true;
                    break;
                }
            }
            if (commonContainerFound)
                return CanBeProcessed(inputTypes, out handledInputTypes, out unhandledInputTypes);
            else
            {
                handledInputTypes = new List<OutputType>();
                unhandledInputTypes = new List<OutputType>();
                return ProcessingLevel.NONE;
            }
        }

        public string GetOutputTypeFilter(ContainerType containerType)
        {
            foreach (ContainerFileType type in this.supportedContainers)
            {
                if (type.ContainerType == containerType)
                {
                    return type.OutputFilterString;
                }
            }
            return "";
        }

        public string GetOutputTypeFilter()
        {
            return VideoUtil.GenerateCombinedFilter(supportedContainers.ToArray());
        }

        public string GetVideoInputFilter()
        {
            return VideoUtil.GenerateCombinedFilter(supportedVideoTypes.ToArray());
        }

        public string GetAudioInputFilter()
        {
            return VideoUtil.GenerateCombinedFilter(supportedAudioTypes.ToArray());
        }

        public string GetChapterInputFilter()
        {
            return VideoUtil.GenerateCombinedFilter(supportedChapterTypes.ToArray());
        }

        public string GetSubtitleInputFilter()
        {
            return VideoUtil.GenerateCombinedFilter(supportedSubtitleTypes.ToArray());
        }

        public string GetMuxedInputFilter()
        {
            return VideoUtil.GenerateCombinedFilter(GetSupportedInputContainers().ToArray());
        }
        #endregion
    }
    public class VideoEncoderProvider
    {
        List<IVideoEncoding> registeredEncoders;
        public VideoEncoderProvider()
        {
            registeredEncoders = new List<IVideoEncoding>();
            this.RegisterEncoder(new MencoderEncoderProvider() as IVideoEncoding);
            this.RegisterEncoder(new X264EncoderProvider() as IVideoEncoding);
            this.RegisterEncoder(new XviDEncoderProvider() as IVideoEncoding);
        }
        /// <summary>
        /// checks all available video encoders to see if one supports the desired video codec with the desired
        /// output type
        /// </summary>
        /// <param name="videoCodec">the desired video codec</param>
        /// <param name="outputType">the desired video output</param>
        /// <returns>true if the codec/output type combo can be fullfilled, false if not</returns>
        public bool IsSupported(VideoCodec videoCodec, VideoType outputType)
        {
            VideoEncoder encoder = GetEncoder("", videoCodec, outputType);
            if (encoder != null)
                return true;
            else
                return false;
        }
        /// <summary>
        /// gets the input filter string for a given codec type (based on the encoder that is capable of encoding
        /// the desired codec)
        /// </summary>
        /// <param name="codec">the desired codec</param>
        /// <returns>the input filter string for the desired codec</returns>
        public string GetSupportedInput(VideoCodec codec)
        {
            IVideoEncoding enc = null;
            foreach (IVideoEncoding encoder in this.registeredEncoders)
            {
                if (encoder.GetSupportedCodecs().Contains(codec))
                {
                    enc = encoder;
                    break;
                }
            }
            if (enc == null)
            {
                return "";
            }
            else
            {
                return enc.GetInputTypeFilter();
            }
        }
        /// <summary>
        /// gets all input types supported by the first encoder capable of handling the desired codec
        /// </summary>
        /// <param name="codec">the desired codec</param>
        /// <returns>a list of output types that the encoder for this desired codec can deliver directly</returns>
        public VideoType[] GetSupportedOutput(VideoCodec codec)
        {
            IVideoEncoding enc = null;
            foreach (IVideoEncoding encoder in this.registeredEncoders)
            {
                if (encoder.GetSupportedCodecs().Contains(codec))
                {
                    enc = encoder;
                    break;
                }
            }
            if (enc == null)
            {
                return new VideoType[0];
            }
            else
            {
                List<VideoType> supportedTypes = enc.GetSupportedOutputTypes(codec);
                return supportedTypes.ToArray();
            }
        }
        /// <summary>
        /// gets an encoder for the given codec and output type
        /// </summary>
        /// <param name="videoCodec">the desired video codec</param>
        /// <param name="outputType">the desired output type</param>
        /// <returns>the encoder found or null if no encoder was found</returns>
        public VideoEncoder GetEncoder(string encoderPath, VideoCodec videoCodec, VideoType outputType)
        {
            VideoEncoder encoder = null;
            foreach (IVideoEncoding encodingInterface in this.registeredEncoders)
            {
                if (!encodingInterface.GetSupportedCodecs().Contains(videoCodec))
                    continue;
                if (!encodingInterface.GetSupportedOutputTypes(videoCodec).Contains(outputType))
                    continue;
                encoder = encodingInterface.GetEncoder(videoCodec, outputType, "");
            }
            return encoder;
        }
        /// <summary>
        /// registers a new encoder to the program
        /// </summary>
        /// <param name="encoder"></param>
        public void RegisterEncoder(IVideoEncoding encoder)
        {
            this.registeredEncoders.Add(encoder);
        }
    }
    public class AudioEncoderProvider
    {
        List<IAudioEncoding> registeredEncoders;
        public AudioEncoderProvider()
        {
            registeredEncoders = new List<IAudioEncoding>();
            RegisterEncoder(new AviSynthEncoderProvider() as IAudioEncoding);

        }
        /// <summary>
        /// checks all registered audio encoders for one that can deliver the desired codec and output type
        /// returns true if one can satisfy the requirements, and false if not
        /// </summary>
        /// <param name="audioCodec">the desired codec</param>
        /// <param name="outputType">the desired output type</param>
        /// <returns>true if an encoder can be found, false if not</returns>
        public bool IsSupported(AudioCodec audioCodec, AudioType outputType)
        {
            AudioEncoder encoder = GetEncoder("", audioCodec, outputType);
            if (encoder != null)
                return true;
            else
                return false;
        }
        /// <summary>
        /// gets the input filter string for a given codec type (based on the encoder that is capable of encoding
        /// the desired codec)
        /// </summary>
        /// <param name="codec">the desired codec</param>
        /// <returns>the input filter string for the desired codec</returns>
        public string GetSupportedInput(AudioCodec codec)
        {
            IAudioEncoding enc = null;
            foreach (IAudioEncoding encoder in this.registeredEncoders)
            {
                if (encoder.GetSupportedCodecs().Contains(codec))
                {
                    enc = encoder;
                    break;
                }
            }
            if (enc == null)
            {
                return "";
            }
            else
            {
                return enc.GetInputTypeFilter();
            }

        }
        /// <summary>
        /// gets all supported output types for a given audio codec
        /// </summary>
        /// <param name="codec">the desired audio codec</param>
        /// <returns>all output types we support for this codec</returns>
        public AudioType[] GetSupportedOutput(AudioCodec codec)
        {
            IAudioEncoding enc = null;
            foreach (IAudioEncoding encoder in this.registeredEncoders)
            {
                if (encoder.GetSupportedCodecs().Contains(codec))
                {
                    enc = encoder;
                    break;
                }
            }
            if (enc == null)
            {
                return new AudioType[0];
            }
            else
            {
                List<AudioType> supportedTypes = enc.GetSupportedOutputTypes(codec);
                AudioType[] retval = new AudioType[supportedTypes.Count];
                for (int i = 0; i < supportedTypes.Count; i++)
                {
                    retval[i] = supportedTypes[i];
                }
                return retval;
            }
        }
        /// <summary>
        /// gets an encoder for the desired audio codec and audio output type and initializes it with the path given as parameter
        /// returns null if no encoder can be found
        /// </summary>
        /// <param name="encoderPath">path of the encoder</param>
        /// <param name="audioCodec">desired audio codec</param>
        /// <param name="outputType">desired audio output type</param>
        /// <returns>the encoder or null if none can satisfy the required output</returns>
        public AudioEncoder GetEncoder(string encoderPath, AudioCodec audioCodec, AudioType outputType)
        {
            AudioEncoder encoder = null;
            foreach (IAudioEncoding encodingInterface in this.registeredEncoders)
            {
                if (!encodingInterface.GetSupportedCodecs().Contains(audioCodec))
                    continue;
                if (!encodingInterface.GetSupportedOutputTypes(audioCodec).Contains(outputType))
                    continue;
                encoder = encodingInterface.GetEncoder(audioCodec, outputType, null);
            }
            return encoder;
        }
        /// <summary>
        /// registers a new encoder to the program
        /// </summary>
        /// <param name="encoder"></param>
        public void RegisterEncoder(IAudioEncoding encoder)
        {
            this.registeredEncoders.Add(encoder);
        }
    }
    #endregion
    #region video encoding providers
    public class XviDEncoderProvider : IVideoEncoding
    {
        private List<VideoCodec> supportedCodecs;
        private List<VideoType> supportedTypes;

        public XviDEncoderProvider()
        {
            supportedCodecs = new List<VideoCodec>();
            supportedCodecs.Add(VideoCodec.XVID);
            supportedTypes = new List<VideoType>();
            supportedTypes.Add(VideoType.AVI);
            supportedTypes.Add(VideoType.MKV);
            supportedTypes.Add(VideoType.RAWASP);
        }

        #region IVideoEncoding Members

        public List<VideoCodec> GetSupportedCodecs()
        {
            return supportedCodecs;
        }

        public List<VideoType> GetSupportedOutputTypes(VideoCodec codec)
        {
            if (codec == VideoCodec.XVID)
                return supportedTypes;
            else
                return new List<VideoType>();
        }

        public VideoEncoder GetEncoder(VideoCodec codec, VideoType outputType, string encoderPath)
        {
            if (supportedCodecs.Contains(codec))
            {
                foreach (VideoType vto in GetSupportedOutputTypes(codec))
                {
                    if (vto.GetType() == outputType.GetType())
                        return new XviDEncoder(encoderPath);
                }
            }
            return null;
        }

        public string GetOutputTypeFilter(VideoCodec codec, VideoType outputType)
        {
            if (codec == VideoCodec.XVID)
            {
                foreach (VideoType vto in GetSupportedOutputTypes(codec))
                {
                    if (vto.GetType() == outputType.GetType())
                        return vto.OutputFilterString;
                }
                return null;
            }
            return "";
        }

        public string GetInputTypeFilter()
        {
            return "AviSynth script files(*.avs)|*.avs";
        }

        #endregion
    }

    public class X264EncoderProvider : IVideoEncoding
    {
        private List<VideoCodec> supportedCodecs;
        private List<VideoType> supportedTypes;

        public X264EncoderProvider()
        {
            supportedCodecs = new List<VideoCodec>();
            supportedCodecs.Add(VideoCodec.X264);
            supportedTypes = new List<VideoType>();
            supportedTypes.Add(VideoType.MP4);
            supportedTypes.Add(VideoType.MKV);
            supportedTypes.Add(VideoType.RAWAVC);
        }

        #region IVideoEncoding Members

        public List<VideoCodec> GetSupportedCodecs()
        {
            return supportedCodecs;
        }

        public List<VideoType> GetSupportedOutputTypes(VideoCodec codec)
        {
            if (supportedCodecs.Contains(codec))
            {
                return supportedTypes;
            }
            return new List<VideoType>();
        }

        public VideoEncoder GetEncoder(VideoCodec codec, VideoType outputType, string encoderPath)
        {
            if (supportedCodecs.Contains(codec))
            {
                foreach (VideoType vto in GetSupportedOutputTypes(codec))
                {
                    if (vto.GetType() == outputType.GetType())
                        return new x264Encoder(encoderPath);
                }
            }
            return null;
        }

        public string GetOutputTypeFilter(VideoCodec codec, VideoType type)
        {
            if (supportedCodecs.Contains(codec))
            {
                foreach (VideoType vto in GetSupportedOutputTypes(codec))
                {
                    if (vto.GetType() == type.GetType())
                        return vto.OutputFilterString;
                }
            }
            return "";
        }

        public string GetInputTypeFilter()
        {
            return "AviSynth script files(*.avs)|*.avs";
        }

        #endregion
    }

    public class MencoderEncoderProvider : IVideoEncoding
    {
        private List<VideoCodec> supportedCodecs;
        private List<VideoType> supportedTypes;

        public MencoderEncoderProvider()
        {
            supportedCodecs = new List<VideoCodec>();
            supportedCodecs.Add(VideoCodec.LMP4);
            supportedCodecs.Add(VideoCodec.SNOW);
            supportedTypes = new List<VideoType>();
            supportedTypes.Add(VideoType.AVI);
            supportedTypes.Add(VideoType.RAWASP);
        }

        #region IVideoEncoding Members

        public List<VideoCodec> GetSupportedCodecs()
        {
            return supportedCodecs;
        }

        public List<VideoType> GetSupportedOutputTypes(VideoCodec codec)
        {
            if (supportedCodecs.Contains(codec))
            {
                if (codec == VideoCodec.LMP4)
                    return supportedTypes;
                else if (codec == VideoCodec.SNOW)
                {
                    List<VideoType> typeList = new List<VideoType>();
                    typeList.Add(VideoType.AVI);
                    return typeList;
                }
            }
            return new List<VideoType>();
        }

        public VideoEncoder GetEncoder(VideoCodec codec, VideoType outputType, string encoderPath)
        {
            if (supportedCodecs.Contains(codec))
            {
                foreach (VideoType vto in GetSupportedOutputTypes(codec))
                {
                    if (vto == outputType)
                        return new mencoderEncoder(encoderPath);
                }
            }
            return null;
        }

        public string GetOutputTypeFilter(VideoCodec codec, VideoType outputType)
        {
            if (supportedCodecs.Contains(codec))
            {
                foreach (VideoType vto in GetSupportedOutputTypes(codec))
                {
                    if (vto.GetType() == outputType.GetType())
                        return vto.OutputFilterString;
                }
            }
            return "";
        }

        public string GetInputTypeFilter()
        {
            return "AviSynth script files(*.avs)|*.avs";
        }

        #endregion
    }
    #endregion
    #region audio encoding providers
    public class AviSynthEncoderProvider : IAudioEncoding
    {
        private List<AudioCodec> supportedCodecs;
        private List<AudioType> supportedTypes;
        public AviSynthEncoderProvider()
        {
            supportedCodecs = new List<AudioCodec>();
            supportedCodecs.Add(AudioCodec.AAC);
            supportedCodecs.Add(AudioCodec.MP3);
            supportedTypes = new List<AudioType>();
            supportedTypes.Add(AudioType.MP4AAC);
            supportedTypes.Add(AudioType.RAWAAC);
            supportedTypes.Add(AudioType.MP3);
        }

        #region IAudioEncoding Members

        public List<AudioCodec> GetSupportedCodecs()
        {
            return supportedCodecs;
        }

        public List<AudioType> GetSupportedOutputTypes(AudioCodec codec)
        {
            if (supportedCodecs.Contains(codec))
            {
                if (codec == AudioCodec.AAC)
                {
                    List<AudioType> typeList = new List<AudioType>();
                    typeList.Add(AudioType.MP4AAC);
                    typeList.Add(AudioType.RAWAAC);
                    return typeList;
                }
                else if (codec == AudioCodec.MP3)
                {
                    List<AudioType> typeList = new List<AudioType>();
                    typeList.Add(AudioType.MP3);
                    return typeList;
                }
            }
            return new List<AudioType>();
        }

        public AudioEncoder GetEncoder(AudioCodec codec, AudioType type, MeGUISettings settings)
        {
            if (supportedCodecs.Contains(codec))
            {
                foreach (AudioType aot in GetSupportedOutputTypes(codec))
                {
                    if (aot == type)
                        return new AviSynthAudioEncoder(settings);
                }
            }
            return null;
        }

        public string GetOutputTypeFilter(AudioCodec codec, AudioType type)
        {
            if (supportedCodecs.Contains(codec))
            {
                foreach (AudioType aot in GetSupportedOutputTypes(codec))
                {
                    if (aot == type)
                        return aot.OutputFilterString;
                }
            }
            return "";
        }

        public string GetInputTypeFilter()
        {
            return "All supported types|*.avs;*.wav;*.pcm;*.mpa;*.mp2;*.mp3;*.ac3;*.dts";
        }

        #endregion
    }
    #endregion
}
