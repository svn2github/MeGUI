using System;
using System.Collections.Generic;
using System.Text;
using MeGUI.core.plugins.interfaces;

namespace MeGUI.core.details
{

    /// <summary>
    /// Shows a dialog (normally) which allows the user to view the job's settings
    /// and reconfigure them. If a dialog is shown, the reconfigured job must be returned.
    /// 
    /// If null is returned, this is taken to mean that the job configurer is 
    /// not valid for this type of job.
    /// </summary>
    /// <param name="j"></param>
    /// <returns></returns>
    public delegate Job ReconfigureJob(Job j);

    public class PackageSystem 
    {

        GenericRegisterer<ITool> tools = new GenericRegisterer<ITool>();
        GenericRegisterer<IOption> options = new GenericRegisterer<IOption>();
        GenericRegisterer<IMediaFileFactory> mediaFileTypes = new GenericRegisterer<IMediaFileFactory>();
        GenericRegisterer<ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>> videoSettingsProviders = new GenericRegisterer<ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>>();
        GenericRegisterer<ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>> audioSettingsProviders = new GenericRegisterer<ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>>();
        GenericRegisterer<IMuxing> muxers = new GenericRegisterer<IMuxing>();
        GenericRegisterer<JobPreProcessor> jobPreProcessors = new GenericRegisterer<JobPreProcessor>();
        GenericRegisterer<JobPostProcessor> jobPostProcessors = new GenericRegisterer<JobPostProcessor>();
        GenericRegisterer<JobProcessorFactory> jobProcessors = new GenericRegisterer<JobProcessorFactory>();
        
        
        
        
        public GenericRegisterer<IDable<ReconfigureJob>> JobConfigurers = new GenericRegisterer<IDable<ReconfigureJob>>();

        public GenericRegisterer<ITool> Tools
        {
            get { return tools; }
        }
        public GenericRegisterer<IOption> Options
        {
            get { return options; }
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
