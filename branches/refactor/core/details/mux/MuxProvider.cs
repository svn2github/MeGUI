using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public enum MuxerType { MP4BOX, MKVMERGE, AVC2AVI, AVIMUXGUI, DIVXMUX, FFMPEG, ATOMCHANGER };
    public class MuxableType
    {
        public OutputType outputType;
        public ICodec codec;
        public MuxableType(OutputType type, ICodec codec)
        {
            this.outputType = type;
            this.codec = codec;
        }
    }
    public class MuxProvider
    {
//        List<IMuxing> registeredMuxers;
        MainForm mainForm;
        MuxPathComparer comparer;
        VideoEncoderProvider vProvider = new VideoEncoderProvider();
        AudioEncoderProvider aProvider = new AudioEncoderProvider();
        public MuxProvider(MainForm mainForm)
        {
            this.mainForm = mainForm;
#warning need to register muxers
            /*            registeredMuxers = new List<IMuxing>();
            registeredMuxers.Add(new MP4BoxMuxerProvider());
            registeredMuxers.Add(new MKVMergeMuxerProvider());
            registeredMuxers.Add(new AVC2AVIMuxerProvider());
            registeredMuxers.Add(new DivXMuxProvider());*/
//            registeredMuxers.Add(new AVIMuxGUIMuxerProvider() as IMuxing);
            comparer = new MuxPathComparer();
        }

/*        public List<IMuxing> GetRegisteredMuxers()
        {
            return registeredMuxers;
        }*/

        public IMuxing GetMuxer(MuxerType type)
        {
            foreach (IMuxing muxing in mainForm.PackageSystem.MuxerProviders.Values)
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

        public MuxPath GetMuxPath(VideoEncoderType videoCodec, AudioEncoderType[] audioCodecs, ContainerType containerType, 
            params MuxableType[] dictatedTypes)
        {
            List<IEncoderType> inputCodecs = new List<IEncoderType>();
            if (videoCodec != null) 
                inputCodecs.Add(videoCodec);
            foreach (AudioEncoderType ac in audioCodecs)
                inputCodecs.Add(ac);
            
            List<MuxableType> decidedTypeList = new List<MuxableType>();
            foreach (MuxableType st in dictatedTypes)
                decidedTypeList.Add(st);
            return findBestMuxPathAndConfig(inputCodecs, decidedTypeList, containerType);
        }

        private MuxPath findBestMuxPathAndConfig(List<IEncoderType> undecidedInputs, List<MuxableType> decidedInputs, ContainerType containerType)
        {
            if (undecidedInputs.Count == 0)
            {
                return getShortestMuxPath(new MuxPath(decidedInputs, containerType), decidedInputs, containerType);
            }
            else
            {
                List<MuxPath> allPaths = new List<MuxPath>();
                IEncoderType undecidedInput = undecidedInputs[0];
                undecidedInputs.RemoveAt(0);

                if (undecidedInput is VideoEncoderType)
                {
                    VideoType[] allTypes = vProvider.GetSupportedOutput((VideoEncoderType)undecidedInput);
                    foreach (VideoType v in allTypes)
                    {
                        MuxableType input = new MuxableType(v, undecidedInput.Codec);
                        decidedInputs.Add(input);
                        MuxPath path = findBestMuxPathAndConfig(undecidedInputs, decidedInputs, containerType);
                        if (path != null)
                        {
                            allPaths.Add(path);
                        }
                        decidedInputs.Remove(input);
                    }
                }
                if (undecidedInput is AudioEncoderType)
                {
                    AudioType[] allTypes = aProvider.GetSupportedOutput((AudioEncoderType)undecidedInput);
                    foreach (AudioType a in allTypes)
                    {
                        MuxableType input = new MuxableType(a, undecidedInput.Codec);
                        decidedInputs.Add(input);
                        MuxPath path = findBestMuxPathAndConfig(undecidedInputs, decidedInputs, containerType);
                        if (path != null)
                        {
                            allPaths.Add(path);
                        }
                        decidedInputs.Remove(input);
                    }
                }
                undecidedInputs.Insert(0, undecidedInput);
                return comparer.GetBestMuxPath(allPaths);
            }
        }

        public MuxPath GetMuxPath(ContainerType containerType, params MuxableType[] allTypes)
        {
            List<MuxableType> inputTypes = new List<MuxableType>();
            inputTypes.AddRange(allTypes);
            MuxPath shortestPath = getShortestMuxPath(new MuxPath(inputTypes, containerType), inputTypes, containerType);
            return shortestPath;
        }

        public bool CanBeMuxed(ContainerType containerType, params MuxableType[] allTypes)
        {
            MuxPath muxPath = GetMuxPath(containerType, allTypes);
            if (muxPath != null)
                return true;
            else
                return false;
        }

        public bool CanBeMuxed(VideoEncoderType codec, AudioEncoderType[] audioCodecs, ContainerType containerType,
            params MuxableType[] decidedTypes)
        {
            MuxPath muxPath = GetMuxPath(codec, audioCodecs, containerType, decidedTypes);
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
                    currentMPL.handledInputTypes = new List<MuxableType>();
                    currentMPL.unhandledInputTypes = new List<MuxableType>();
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

        private MuxPath getShortestMuxPath(MuxPath currentMuxPath, IMuxing muxer, List<MuxableType> decidedHandledTypes,
            List<MuxableType> undecidedPossibleHandledTypes, List<MuxableType> unhandledInputTypes, ContainerType desiredContainerType)
        {
            if (undecidedPossibleHandledTypes.Count == 0)
            {
                MuxPathLeg mpl = new MuxPathLeg();
                mpl.muxerInterface = muxer;
                mpl.handledInputTypes = new List<MuxableType>(decidedHandledTypes);
                mpl.unhandledInputTypes = new List<MuxableType>(unhandledInputTypes);
                MuxPath newMuxPath = currentMuxPath.Clone();
                newMuxPath.Add(mpl);
                if (decidedHandledTypes.Count == 0)
                    return null;
                return getShortestMuxPath(newMuxPath, unhandledInputTypes, desiredContainerType);
            }
            else
            {
                List<MuxPath> allMuxPaths = new List<MuxPath>();
                MuxableType type = undecidedPossibleHandledTypes[0];
                undecidedPossibleHandledTypes.RemoveAt(0);

                decidedHandledTypes.Add(type);
                MuxPath shortestMuxPath = getShortestMuxPath(currentMuxPath, muxer, decidedHandledTypes, undecidedPossibleHandledTypes, unhandledInputTypes, desiredContainerType);
                if (shortestMuxPath != null)
                    allMuxPaths.Add(shortestMuxPath);
                decidedHandledTypes.Remove(type);

                unhandledInputTypes.Add(type);
                shortestMuxPath = getShortestMuxPath(currentMuxPath, muxer, decidedHandledTypes, undecidedPossibleHandledTypes, unhandledInputTypes, desiredContainerType);
                if (shortestMuxPath != null)
                    allMuxPaths.Add(shortestMuxPath);
                unhandledInputTypes.Remove(type);

                undecidedPossibleHandledTypes.Add(type);

                return (comparer.GetBestMuxPath(allMuxPaths));
            }
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
            List<MuxableType> unhandledDesiredInputTypes, ContainerType desiredContainerType)
        {
            List<MuxableType> handledInputTypes = new List<MuxableType>();
            List<MuxableType> unhandledInputTypes = new List<MuxableType>();
            List<MuxPath> allMuxPaths = new List<MuxPath>();

            if (currentMuxPath.IsCompleted())
                return currentMuxPath;
            
            foreach (IMuxing muxer in mainForm.PackageSystem.MuxerProviders.Values)
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

                    if (unhandledInputTypes.Count == 0)
                    {
                        List<IMuxing> allMuxers = new List<IMuxing>();
                        allMuxers.AddRange(mainForm.PackageSystem.MuxerProviders.Values);
                        MuxPath shortestPath = getShortestMuxPath(newMuxPath, allMuxers, desiredContainerType);
                        if (shortestPath != null)
                            allMuxPaths.Add(shortestPath);
                    }

                    MuxPath aShortestPath = getShortestMuxPath(currentMuxPath, muxer, new List<MuxableType>(), handledInputTypes, unhandledInputTypes, desiredContainerType);
                    if (aShortestPath != null)
                        allMuxPaths.Add(aShortestPath);
                }
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

        public List<ContainerType> GetSupportedContainers()
        {
            List<ContainerType> supportedContainers = new List<ContainerType>();
            List<ContainerType> containerTypes = new List<ContainerType>();
            foreach (IMuxing muxerInterface in mainForm.PackageSystem.MuxerProviders.Values)
            {
                List<ContainerType> outputTypes = muxerInterface.GetSupportedContainers();
                foreach (ContainerType type in outputTypes)
                {
                    if (!containerTypes.Contains(type))
                        supportedContainers.Add(type);
                    containerTypes.Add(type);
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
        public List<ContainerType> GetSupportedContainers(params MuxableType[] inputTypes)
        {
            List<ContainerType> supportedContainers = new List<ContainerType>();
            foreach (ContainerType cot in GetSupportedContainers())
            {
                if (CanBeMuxed(cot, inputTypes))
                {
                    if (!supportedContainers.Contains(cot))
                        supportedContainers.Add(cot);
                }
            }
            return supportedContainers;
        }

        public List<ContainerType> GetSupportedContainers(VideoEncoderType videoCodec, AudioEncoderType[] audioCodecs,
            params MuxableType[] dictatedOutputTypes)
        {
            List<ContainerType> supportedContainers = new List<ContainerType>();
            List<ContainerType> allKnownContainers = GetSupportedContainers();
            foreach (ContainerType cot in allKnownContainers)
            {
                if (CanBeMuxed(videoCodec, audioCodecs, cot, dictatedOutputTypes))
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
        public List<MuxableType> handledInputTypes;
        public List<MuxableType> unhandledInputTypes; // those remain for the next leg
    }
    #region muxer providers
    public class MP4BoxMuxerProvider : MuxerProvider
    {
        
        public MP4BoxMuxerProvider() : base("MP4Box")
        {
            supportedVideoTypes.Add(VideoType.RAWASP);
            supportedVideoTypes.Add(VideoType.RAWAVC);
            supportedVideoTypes.Add(VideoType.MP4);
            supportedAudioTypes.Add(AudioType.RAWAAC);
            supportedAudioTypes.Add(AudioType.MP4AAC);
            supportedAudioTypes.Add(AudioType.MP3);
            supportedAudioTypes.Add(AudioType.VBRMP3);
            supportedAudioTypes.Add(AudioType.CBRMP3);
            supportedVideoCodecs.Add(VideoCodec.ASP);
            supportedVideoCodecs.Add(VideoCodec.AVC);
            supportedAudioCodecs.Add(AudioCodec.AAC);
            supportedAudioCodecs.Add(AudioCodec.MP3);
            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            supportedSubtitleTypes.Add(SubtitleType.VOBSUB);
            supportedChapterTypes.Add(ChapterType.OGG_TXT);
            supportedContainers.Add(ContainerType.MP4);
            supportedContainerInputTypes.Add(ContainerType.MP4);
            supportsAnyInputtableAudioCodec = true;
            supportsAnyInputtableVideoCodec = true;
            base.type = MuxerType.MP4BOX;
            maxFilesOfType = new int[] { 1, -1, -1, 1};
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
        public MKVMergeMuxerProvider() : base("mkvmerge")
        {
            supportedVideoTypes.Add(VideoType.AVI);
            supportedVideoTypes.Add(VideoType.MKV);
            supportedVideoTypes.Add(VideoType.MP4);
            supportedAudioTypes.Add(AudioType.RAWAAC);
            supportedAudioTypes.Add(AudioType.MP4AAC);
            supportedAudioTypes.Add(AudioType.MP3);
            supportedAudioTypes.Add(AudioType.VBRMP3);
            supportedAudioTypes.Add(AudioType.CBRMP3);
            supportedAudioTypes.Add(AudioType.VORBIS);
            supportedAudioTypes.Add(AudioType.MP2);
            supportedAudioTypes.Add(AudioType.AC3);
            supportedAudioTypes.Add(AudioType.DTS);
            supportsAnyInputtableAudioCodec = true;
            supportsAnyInputtableVideoCodec = true;
            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            supportedSubtitleTypes.Add(SubtitleType.VOBSUB);
            supportedChapterTypes.Add(ChapterType.OGG_TXT);
            supportedContainers.Add(ContainerType.MKV);
            supportedContainerInputTypes.Add(ContainerType.MP4);
            supportedContainerInputTypes.Add(ContainerType.AVI);
            supportedContainerInputTypes.Add(ContainerType.MKV);
            maxFilesOfType = new int[] { -1, -1, -1, 1 };
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
        public AVIMuxGUIMuxerProvider(): base("AVIMuxGUI")
        {
            supportedVideoTypes.Add(VideoType.AVI);
            supportedAudioTypes.Add(AudioType.AC3);
            supportedAudioTypes.Add(AudioType.DTS);
            supportedAudioTypes.Add(AudioType.MP3);
            supportedAudioTypes.Add(AudioType.VBRMP3);
            supportedAudioTypes.Add(AudioType.CBRMP3);
            supportsAnyInputtableAudioCodec = true;
            supportsAnyInputtableVideoCodec = true;
            supportedAudioTypes.Add(AudioType.VORBIS);
            supportedAudioTypes.Add(AudioType.RAWAAC);
            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            supportedContainers.Add(ContainerType.AVI);
            supportedContainerInputTypes.Add(ContainerType.AVI);
            maxFilesOfType = new int[] { 1, -1, -1, 1 };
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
            : base("DivXMux")
        {
            supportedVideoTypes.Add(VideoType.AVI);
            supportedAudioTypes.Add(AudioType.AC3);
            supportedAudioTypes.Add(AudioType.MP3);
            supportedAudioTypes.Add(AudioType.VBRMP3);
            supportedAudioTypes.Add(AudioType.CBRMP3);
            supportedSubtitleTypes.Add(SubtitleType.SUBRIP);
            supportedSubtitleTypes.Add(SubtitleType.VOBSUB);
            supportedVideoCodecs.Add(VideoCodec.ASP);
            supportedAudioCodecs.Add(AudioCodec.AC3);
            supportedAudioCodecs.Add(AudioCodec.MP3);
            supportedContainerInputTypes.Add(ContainerType.AVI);
            supportedContainers.Add(ContainerType.AVI);
            maxFilesOfType = new int[] { 1, -1, -1, 1 };
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
        public AVC2AVIMuxerProvider() : base("avc2avi")
        {
            supportedVideoTypes.Add(VideoType.RAWAVC);
            supportedContainers.Add(ContainerType.AVI);
            supportedVideoCodecs.Add(VideoCodec.AVC);
            base.type = MuxerType.AVC2AVI;
            name = "AVC2AVI";
            maxFilesOfType = new int[] { 1, 0, 0, 0 };
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
        protected List<VideoCodec> supportedVideoCodecs;
        protected List<AudioCodec> supportedAudioCodecs;
        protected List<AudioType> supportedAudioTypes;
        protected List<SubtitleType> supportedSubtitleTypes;
        protected List<ChapterType> supportedChapterTypes;
        protected List<ContainerType> supportedContainers;
        protected List<ContainerType> supportedContainerInputTypes;
        protected MuxCommandlineGenerator generator;
        protected bool supportsAnyInputtableAudioCodec = false;
        protected bool supportsAnyInputtableVideoCodec = false;
        protected string videoInputFilter, audioInputFilter, subtitleInputFilter;
        protected int[] maxFilesOfType;
        protected string name;
        protected string id;
        protected MuxerType type;
        public MuxerProvider(string id)
        {
            supportedVideoTypes = new List<VideoType>();
            supportedAudioTypes = new List<AudioType>();
            supportedSubtitleTypes = new List<SubtitleType>();
            supportedChapterTypes = new List<ChapterType>();
            supportedAudioCodecs = new List<AudioCodec>();
            supportedVideoCodecs = new List<VideoCodec>();
            supportedContainers = new List<ContainerType>();
            supportedContainerInputTypes = new List<ContainerType>();
            videoInputFilter = audioInputFilter = subtitleInputFilter = "";
            this.id = id;
        }
        #region IMuxing Members
        public string ID
        {
            get
            {
                return id;
            }
        }
        public bool SupportsVideoCodec(VideoCodec codec)
        {
            if (supportsAnyInputtableVideoCodec)
                return true;
            return (GetSupportedVideoCodecs().Contains(codec));
        }

        public bool SupportsAudioCodec(AudioCodec codec)
        {
            if (supportsAnyInputtableAudioCodec)
                return true;
            return (GetSupportedAudioCodecs().Contains(codec));
        }

        public List<VideoCodec> GetSupportedVideoCodecs()
        {
            return supportedVideoCodecs;
        }

        public List<AudioCodec> GetSupportedAudioCodecs()
        {
            return supportedAudioCodecs;
        }

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

        public List<ContainerType> GetSupportedContainers()
        {
            return this.supportedContainers;
        }

        public List<ContainerType> GetSupportedContainerTypes()
        {
            List<ContainerType> supportedOutputTypes = GetSupportedContainers();
            List<ContainerType> supportedContainers = new List<ContainerType>();
            foreach (ContainerType cot in supportedOutputTypes)
            {
                supportedContainers.Add(cot);
            }
            return supportedContainers;
        }

        public List<ContainerType> GetSupportedContainerInputTypes()
        {
            return this.supportedContainerInputTypes;
        }

        public List<ContainerType> GetContainersInCommon(IMuxing iMuxing)
        {
            List<ContainerType> supportedOutputTypes = GetSupportedContainers();
            List<ContainerType> nextSupportedInputTypes = iMuxing.GetSupportedContainerInputTypes();
            List<ContainerType> commonContainers = new List<ContainerType>();
            foreach (ContainerType eligibleType in supportedOutputTypes)
            {
                if (nextSupportedInputTypes.Contains(eligibleType))
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
        private int getSupportedType(MuxableType type)
        {
            if (type.outputType is VideoType && supportedVideoTypes.Contains((VideoType)type.outputType) &&
                (supportsAnyInputtableVideoCodec || supportedVideoCodecs.Contains((VideoCodec)type.codec)))
                return 0;
            if (type.outputType is AudioType && supportedAudioTypes.Contains((AudioType)type.outputType) &&
                (supportsAnyInputtableAudioCodec || supportedAudioCodecs.Contains((AudioCodec)type.codec)))
                return 1;
            if (type.outputType is SubtitleType && supportedSubtitleTypes.Contains((SubtitleType)type.outputType))
                return 2;
            if (type.outputType is ChapterType && supportedChapterTypes.Contains((ChapterType)type.outputType))
                return 3;
            return -1;
        }

        public ProcessingLevel CanBeProcessed(MuxableType[] inputTypes, out List<MuxableType> handledInputTypes,
            out List<MuxableType> unhandledInputTypes)
        {
            handledInputTypes = new List<MuxableType>();
            unhandledInputTypes = new List<MuxableType>();
            int[] filesOfType = new int[4];
            foreach (MuxableType inputType in inputTypes)
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


        public ProcessingLevel CanBeProcessed(ContainerType[] inputContainers, MuxableType[] inputTypes, out List<MuxableType> handledInputTypes,
            out List<MuxableType> unhandledInputTypes)
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
                handledInputTypes = new List<MuxableType>();
                unhandledInputTypes = new List<MuxableType>();
                return ProcessingLevel.NONE;
            }
        }

        public string GetOutputTypeFilter(ContainerType containerType)
        {
            foreach (ContainerType type in this.supportedContainers)
            {
                if (type == containerType)
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
            return VideoUtil.GenerateCombinedFilter(GetSupportedContainerInputTypes().ToArray());
        }
        #endregion
    }

    #region generic providers
    public abstract class EncodingProvider<TCodec, TType, TEncoderType> : IEncoding<TCodec, TType, TEncoderType>
        where TType : OutputType
    {
        protected List<TType> supportedTypes;
        protected List<TCodec> supportedCodecs;
        protected List<TEncoderType> supportedEncoderTypes;

        public abstract IJobProcessor CreateEncoder(MeGUISettings settings);

        public EncodingProvider()
        {
            supportedTypes = new List<TType>();
            supportedCodecs = new List<TCodec>();
            supportedEncoderTypes = new List<TEncoderType>();
        }

        #region IVideoEncoding Members

        public List<TEncoderType> GetSupportedEncoderTypes()
        {
            return supportedEncoderTypes;
        }

        public List<TCodec> GetSupportedCodecs()
        {
            return supportedCodecs;
        }

        public List<TType> GetSupportedOutputTypes(TEncoderType codec)
        {
            if (supportedEncoderTypes.Contains(codec))
                return supportedTypes;
            else
                return new List<TType>();
        }

        public IJobProcessor GetEncoder(TEncoderType codec, TType outputType, MeGUISettings settings)
        {
            if (supportedEncoderTypes.Contains(codec))
            {
                foreach (TType vto in GetSupportedOutputTypes(codec))
                {
                    if (vto.GetType() == outputType.GetType())
                        return CreateEncoder(settings);
                }
            }
            return null;
        }

        public string GetOutputTypeFilter(TEncoderType codec, TType outputType)
        {
            if (supportedEncoderTypes.Contains(codec))
            {
                foreach (TType vto in GetSupportedOutputTypes(codec))
                {
                    if (vto.GetType() == outputType.GetType())
                        return vto.OutputFilterString;
                }
                return null;
            }
            return "";
        }

        public virtual string GetInputTypeFilter()
        {
            return "AviSynth script files(*.avs)|*.avs";
        }
        #endregion

    }


    public class AllEncoderProvider<TCodec, TType, TEncoderType>
        where TType : OutputType
    {
        List<IEncoding<TCodec, TType, TEncoderType>> registeredEncoders;
        public AllEncoderProvider()
        {
            registeredEncoders = new List<IEncoding<TCodec, TType, TEncoderType>>();
        }
        /// <summary>
        /// checks all available video encoders to see if one supports the desired video codec with the desired
        /// output type
        /// </summary>
        /// <param name="videoCodec">the desired video codec</param>
        /// <param name="outputType">the desired video output</param>
        /// <returns>true if the codec/output type combo can be fullfilled, false if not</returns>
        public bool IsSupported(TEncoderType codec, TType outputType)
        {
            IJobProcessor encoder = GetEncoder(new MeGUISettings(), codec, outputType);
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
        public string GetSupportedInput(TCodec codec)
        {
            IEncoding<TCodec, TType, TEncoderType> enc = null;
            foreach (IEncoding<TCodec, TType, TEncoderType> encoder in this.registeredEncoders)
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
        public TType[] GetSupportedOutput(TEncoderType encoderType)
        {
            IEncoding<TCodec, TType, TEncoderType> enc = null;
            foreach (IEncoding<TCodec, TType, TEncoderType> encoder in this.registeredEncoders)
            {
                if (encoder.GetSupportedEncoderTypes().Contains(encoderType))
                {
                    enc = encoder;
                    break;
                }
            }
            if (enc == null)
            {
                return new TType[0];
            }
            else
            {
                List<TType> supportedTypes = enc.GetSupportedOutputTypes(encoderType);
                return supportedTypes.ToArray();
            }
        }
        /// <summary>
        /// gets an encoder for the given codec and output type
        /// </summary>
        /// <param name="videoCodec">the desired video codec</param>
        /// <param name="outputType">the desired output type</param>
        /// <returns>the encoder found or null if no encoder was found</returns>
        public IJobProcessor GetEncoder(MeGUISettings settings, TEncoderType codec, TType outputType)
        {
            IJobProcessor encoder = null;
            foreach (IEncoding<TCodec, TType, TEncoderType> encodingInterface in this.registeredEncoders)
            {
                if (!encodingInterface.GetSupportedEncoderTypes().Contains(codec))
                    continue;
                if (!encodingInterface.GetSupportedOutputTypes(codec).Contains(outputType))
                    continue;
                encoder = encodingInterface.GetEncoder(codec, outputType, settings);
            }
            return encoder;
        }
        /// <summary>
        /// registers a new encoder to the program
        /// </summary>
        /// <param name="encoder"></param>
        public void RegisterEncoder(IEncoding<TCodec, TType, TEncoderType> encoder)
        {
            this.registeredEncoders.Add(encoder);
        }
    }
    #endregion

    public class AudioEncodingProvider : EncodingProvider<AudioCodec, AudioType, AudioEncoderType>
    {
        public AudioEncodingProvider()
            : base()
        { }
        
        public override string GetInputTypeFilter()
        {
            return "All supported types|*.avs;*.wav;*.pcm;*.mpa;*.mp2;*.mp3;*.ac3;*.dts";
        }

        public override IJobProcessor CreateEncoder(MeGUISettings settings)
        {
            return new AviSynthAudioEncoder(settings);
        }
    }
    public class VideoEncoderProvider : AllEncoderProvider<VideoCodec, VideoType, VideoEncoderType>
    {
        public VideoEncoderProvider()
            : base()
        {
            this.RegisterEncoder(new MencoderEncoderProvider());
            this.RegisterEncoder(new X264EncoderProvider());
            this.RegisterEncoder(new XviDEncoderProvider());
        }
    }
    public class AudioEncoderProvider : AllEncoderProvider<AudioCodec, AudioType, AudioEncoderType>
    {
        public AudioEncoderProvider()
            : base()
        {
            RegisterEncoder(new WinAmpAACEncodingProvider());
            RegisterEncoder(new NeroAACEncodingProvider());
            RegisterEncoder(new LameMP3EncodingProvider());
            RegisterEncoder(new AudXEncodingProvider());
            RegisterEncoder(new FAACEncodingProvider());
            RegisterEncoder(new VorbisEncodingProvider());
            RegisterEncoder(new AC3EncodingProvider());
            RegisterEncoder(new MP2EncodingProvider());
        }
    }
    #endregion
    #region video encoding providers
    public class XviDEncoderProvider : EncodingProvider<VideoCodec, VideoType, VideoEncoderType>
    {
        public XviDEncoderProvider()
        {
            supportedCodecs.Add(VideoCodec.ASP);
            supportedTypes.Add(VideoType.AVI);
            supportedTypes.Add(VideoType.MKV);
            supportedTypes.Add(VideoType.RAWASP);
            supportedEncoderTypes.Add(VideoEncoderType.XVID);
        }

        public override IJobProcessor CreateEncoder(MeGUISettings settings)
        {
            return new XviDEncoder(settings.XviDEncrawPath);
        }
    }

    public class X264EncoderProvider : EncodingProvider<VideoCodec, VideoType, VideoEncoderType>
    {
        public X264EncoderProvider()
        {
            supportedCodecs.Add(VideoCodec.AVC);
            supportedTypes.Add(VideoType.MP4);
            supportedTypes.Add(VideoType.MKV);
            supportedTypes.Add(VideoType.RAWAVC);
            supportedEncoderTypes.Add(VideoEncoderType.X264);
        }

        public override IJobProcessor CreateEncoder(MeGUISettings settings)
        {
            return new x264Encoder(settings.X264Path);
        }
    }

    public class MencoderEncoderProvider : EncodingProvider<VideoCodec, VideoType, VideoEncoderType>
    {
        public MencoderEncoderProvider()
        {
            supportedCodecs.Add(VideoCodec.SNOW);
            supportedCodecs.Add(VideoCodec.ASP);
            supportedTypes.Add(VideoType.AVI);
            supportedTypes.Add(VideoType.RAWASP);
            supportedEncoderTypes.Add(VideoEncoderType.LMP4);
            supportedEncoderTypes.Add(VideoEncoderType.SNOW);
        }

        public override IJobProcessor CreateEncoder(MeGUISettings settings)
        {
            return new mencoderEncoder(settings.MencoderPath);
        }
    }
    #endregion
    #region audio encoding providers
    public class WinAmpAACEncodingProvider : AudioEncodingProvider
    {
        public WinAmpAACEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.AAC);
            supportedTypes.Add(AudioType.RAWAAC);
            supportedEncoderTypes.Add(AudioEncoderType.WAAC);
        }
    }

    public class NeroAACEncodingProvider : AudioEncodingProvider
    {
        public NeroAACEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.AAC);
            supportedTypes.Add(AudioType.MP4AAC);
            supportedEncoderTypes.Add(AudioEncoderType.NAAC);
        }
    }

    public class LameMP3EncodingProvider : AudioEncodingProvider
    {
        public LameMP3EncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.MP3);
            supportedTypes.Add(AudioType.MP3);
            supportedEncoderTypes.Add(AudioEncoderType.LAME);
        }
    }

    public class AudXEncodingProvider : AudioEncodingProvider
    {
        public AudXEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.MP3);
            supportedTypes.Add(AudioType.MP3);
            supportedEncoderTypes.Add(AudioEncoderType.AUDX);
        }
    }

    public class FAACEncodingProvider : AudioEncodingProvider
    {
        public FAACEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.AAC);
            supportedTypes.Add(AudioType.MP4AAC);
            supportedTypes.Add(AudioType.RAWAAC);
            supportedEncoderTypes.Add(AudioEncoderType.FAAC);
        }
    }

    public class VorbisEncodingProvider : AudioEncodingProvider
    {
        public VorbisEncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.VORBIS);
            supportedTypes.Add(AudioType.VORBIS);
            supportedEncoderTypes.Add(AudioEncoderType.VORBIS);
        }
    }

    public class AC3EncodingProvider : AudioEncodingProvider
    {
        public AC3EncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.AC3);
            supportedTypes.Add(AudioType.AC3);
            supportedEncoderTypes.Add(AudioEncoderType.FFAC3);
        }
    }

    public class MP2EncodingProvider : AudioEncodingProvider
    {
        public MP2EncodingProvider()
            : base()
        {
            supportedCodecs.Add(AudioCodec.DTS);
            supportedTypes.Add(AudioType.MP2);
            supportedEncoderTypes.Add(AudioEncoderType.FFMP2);
        }
    }

    #endregion
}
