using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using MeGUI.core.plugins.interfaces;

namespace MeGUI.core.details
{
    public class ManagableSystem : IIDable
    {
        private IRegisterer system;
        public IRegisterer ManagedSystem
        {
            get { return system; }
        }

        private string id;
        public string ID
        {
            get { return id; }
        }

        private Type type;
        public Type ManagedType
        {
            get { return type; }
        }

        public ManagableSystem(string id, IRegisterer system, Type managedType)
        {
            this.id = id;
            this.system = system;
            this.type = managedType;
        }
    }
    public class PackageSystem
    {
        class LibraryInfo
        {
            public string name;
            
            [XmlIgnore()]
            public ILibrary library;
            
            public bool active;

            public LibraryInfo() { }
            
            public LibraryInfo(string name, ILibrary library, bool active)
            {
                this.name = name;
                this.library = library;
                this.active = active;
            }
        }

        private MainForm mainForm;
        List<LibraryInfo> libraries;
        public PackageSystem(MainForm mf)
        {
            this.mainForm = mf;
        }

        #region access to packages
        #region old
        /*        public override bool Register(IPackage registerable)
        {
            lock (this)
            {
                try
                {
                    foreach (string id in registerable.RequiredPackageIDs)
                    {
                        IPackage temp = this[id];
                    }
                }
                catch (KeyNotFoundException)
                {
                    return false;
#warning should pass the reason why it failed on
                }
                if (!base.Register(registerable))
                    return false;

                foreach (ITool tool in registerable.Tools)
                {
                    tools.Register(tool);
                }
                foreach (IMediaFileFactory factory in registerable.MediaFileTypes)
                {
                    mediaFileTypes.Register(factory);
                }
                foreach (ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> provider in registerable.VideoSettingsProviders)
                {
                    videoSettingsProviders.Register(provider);
                }
                foreach (ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType> provider in registerable.AudioSettingsProviders)
                {
                    audioSettingsProviders.Register(provider);
                }
                foreach (IMuxing muxing in registerable.MuxerProviders)
                {
                    muxers.Register(muxing);
                }
                foreach (JobPreProcessor pp in registerable.JobPreProcessors)
                {
                    jobPreProcessors.Register(pp);
                }
                foreach (JobPostProcessor pp in registerable.JobPostProcessors)
                {
                    jobPostProcessors.Register(pp);
                }
                foreach (JobProcessorFactory pp in registerable.JobProcessors)
                {
                    jobProcessors.Register(pp);
                }
                return true;
            }
        }*/
        #endregion
        GenericRegisterer<ManagableSystem> managedSystems = new GenericRegisterer<ManagableSystem>();

        #region standard registerers
        GenericRegisterer<ITool> tools = new GenericRegisterer<ITool>();
        GenericRegisterer<IMediaFileFactory> mediaFileTypes = new GenericRegisterer<IMediaFileFactory>();
        GenericRegisterer<ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>> videoSettingsProviders = new GenericRegisterer<ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>>();
        GenericRegisterer<ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>> audioSettingsProviders = new GenericRegisterer<ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>>();
        GenericRegisterer<IMuxing> muxers = new GenericRegisterer<IMuxing>();
        GenericRegisterer<JobPreProcessor> jobPreProcessors = new GenericRegisterer<JobPreProcessor>();
        GenericRegisterer<JobPostProcessor> jobPostProcessors = new GenericRegisterer<JobPostProcessor>();
        GenericRegisterer<JobProcessorFactory> jobProcessors = new GenericRegisterer<JobProcessorFactory>();

        private void addStandardSystems()
        {
            wrap(tools, "Tool", typeof(ITool));
            wrap(mediaFileTypes, "Media File", typeof(IMediaFileFactory));
            wrap(videoSettingsProviders, "Video Codec", typeof(ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>));
            wrap(audioSettingsProviders, "Audio Codec", typeof(ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>));
            wrap(muxers, "Muxer", typeof(IMuxing));
            wrap(jobPreProcessors, "Preprocessor", typeof(JobPreProcessor));
            wrap(jobPostProcessors, "Postprocessor", typeof(JobPostProcessor));
            wrap(jobProcessors, "Processor", typeof(JobProcessorFactory));
        }

        private void wrap(IRegisterer o, string id, Type t)
        {
            managedSystems.Register(new ManagableSystem(id, o, t));
        }
        #endregion

        public GenericRegisterer<ITool> Tools
        {
            get { return tools; }
        }
        public GenericRegisterer<IMediaFileFactory> MediaFileTypes
        {
            get { return mediaFileTypes; }
        }
        public GenericRegisterer<ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>> VideoSettingsProviders
        {
            get { return videoSettingsProviders; }
        }
        public GenericRegisterer<ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>> AudioSettingsProviders
        {
            get { return audioSettingsProviders; }
        }
        public GenericRegisterer<IMuxing> MuxerProviders
        {
            get { return muxers; }
        }
        public GenericRegisterer<JobPreProcessor> JobPreProcessors
        {
            get { return jobPreProcessors; }
        }
        public GenericRegisterer<JobPostProcessor> JobPostProcessors
        {
            get { return jobPostProcessors; }
        }
        public GenericRegisterer<JobProcessorFactory> JobProcessors
        {
            get { return jobProcessors; }
        }
        #endregion


        void load()
        {
            using (Stream input = File.OpenRead(mainForm.MeGUIPath + @"\packages\info.xml"))
            {
                XmlSerializer s = new XmlSerializer(libraries.GetType());
                libraries = (List<LibraryInfo>)s.Deserialize(input);
            }
            refreshLibraries();
        }

        private void refreshLibraries()
        {
            List<LibraryInfo> oldLibraries = libraries;
            
            List<LibraryInfo> newLibraries = new List<LibraryInfo>();
            foreach (string file in
                Directory.GetFiles(mainForm.MeGUIPath + @"\packages", "*.dll"))
            {
                try
                {
                    Assembly myAssembly = Assembly.LoadFile(file);
                    foreach (Type type in myAssembly.GetExportedTypes())
                    {
                        Type[] interfaces = type.GetInterfaces();
                        if (Array.IndexOf(interfaces, typeof(ILibrary)) > -1)
                        {
                            ILibrary lib = ((ILibrary)Activator.CreateInstance(type));
                            newLibraries.Add(new LibraryInfo(lib.ID, lib, false));
                        }
                    }
                }
                catch (Exception) { }
            }
            List<LibraryInfo> lostLibraries = new List<LibraryInfo>();
            foreach (LibraryInfo lib in oldLibraries)
            {
                LibraryInfo otherLib = newLibraries.Find(
                    new Predicate<LibraryInfo>(delegate(LibraryInfo otherLib)
                {
                    return (lib.name == otherLib.name);
                }));
                if (otherLib != null) otherLib.active = lib.active;
                else lostLibraries.Add(lib);
            }
            libraries = newLibraries;
            // Lost libraries are known, but discarded here
        }

        void save()
        {
            using (Stream output = File.OpenWrite(mainForm.MeGUIPath + @"\packages\info.xml"))
            {
                XmlSerializer s = new XmlSerializer(libraries.GetType());
                s.Serialize(output, libraries);
            }
        }
    }
}
