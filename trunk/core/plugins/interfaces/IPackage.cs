using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using MeGUI;

namespace MeGUI.core.plugins.interfaces
{
    public interface IPackage : IIDable
    {
        ITool[] Tools { get;}
        IOption[] Options { get;}
        IMediaFileFactory[] MediaFileTypes { get;}
        JobPreProcessor[] JobPreProcessors { get;}
        JobPostProcessor[] JobPostProcessors { get;}
        JobProcessorFactory[] JobProcessors { get;}
        ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>[] VideoSettingsProviders { get;}
        ISettingsProvider<AudioCodecSettings, string[], AudioCodec, AudioEncoderType>[] AudioSettingsProviders { get;}
        IMuxing[] MuxerProviders { get;}
        string Name { get;}
        string[] RequiredPackageIDs { get;}
        bool IsSetUp(MainForm mainForm, out string error);
    }
    
    public interface ITool : IIDable
    {
        string Name { get;}
        void Run(MainForm info);
        System.Windows.Forms.Shortcut[] Shortcuts { get;}
    }

    public interface IOption : IIDable
    {
        string Name { get;}
        void Run(MainForm info);
        System.Windows.Forms.Shortcut[] Shortcuts { get;}
    }
}
