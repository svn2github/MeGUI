using System;
using System.Collections.Generic;
using System.Text;
using MeGUI.core.plugins.interfaces;

namespace MeGUI.core.details
{
    public class PackageSystem : GenericRegisterer<IPackage>
    {
        public override bool Register(IPackage registerable)
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
        }
        GenericRegisterer<ITool> tools = new GenericRegisterer<ITool>();
        GenericRegisterer<IMediaFileFactory> mediaFileTypes = new GenericRegisterer<IMediaFileFactory>();
        GenericRegisterer<ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>> videoSettingsProviders = new GenericRegisterer<ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>>();
        GenericRegisterer<ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>> audioSettingsProviders = new GenericRegisterer<ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>>();
        GenericRegisterer<IMuxing> muxers = new GenericRegisterer<IMuxing>();
        GenericRegisterer<JobPreProcessor> jobPreProcessors = new GenericRegisterer<JobPreProcessor>();
        GenericRegisterer<JobPostProcessor> jobPostProcessors = new GenericRegisterer<JobPostProcessor>();
        GenericRegisterer<JobProcessorFactory> jobProcessors = new GenericRegisterer<JobProcessorFactory>();


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
    }
}
