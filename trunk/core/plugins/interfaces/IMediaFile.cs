using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using MeGUI.core.util;

namespace MeGUI
{
    public interface IMediaFileFactory : IIDable
    {
        /// <summary>
        /// Tries to open the given file. Returns null if impossible.
        /// </summary>
        /// <param name="file">The media file to open</param>
        /// <returns></returns>
        IMediaFile Open(string file);
        /// <summary>
        /// Returns how well a given file is expected to be handled. This is so that better handlers
        /// can be given preference. This should be implemented only by filename and not by opening the file.
        /// </summary>
        /// <param name="extension">The filename to be checked.</param>
        /// <returns>Higher number for better handling, negative for impossible.</returns>
        int HandleLevel(string file);
    }

    public class MediaFileInfo
    {
        public bool HasVideo;
        public ulong Width;
        public ulong Height;
        public Dar DAR;
        public ulong FrameCount;
        public double FPS;
        public bool HasAudio;

        public MediaFileInfo(bool hasVideo, 
            ulong width, ulong height,
            Dar dar, ulong frameCount,
            double fps, bool hasAudio)
        {
            HasVideo = hasVideo;
            Width = width;
            Height = height;
            DAR = dar;
            FrameCount = frameCount;
            FPS = fps;
            HasAudio = hasAudio;
        }

        public MediaFileInfo Clone()
        {
            return (MediaFileInfo)this.MemberwiseClone();
        }
    }
    public interface IMediaFile : IDisposable
    {
        MediaFileInfo Info
        {
            get;
        }

        bool CanReadVideo
        {
            get;
        }
        bool CanReadAudio
        {
            get;
        }
        IVideoReader GetVideoReader();
        IAudioReader GetAudioReader(int track);

    }
    
    public interface IAudioReader
    {
        /// <summary>
        /// Returns the number of samples readable.
        /// </summary>
        long SampleCount
        {
            get;
        }
        
        /// <summary>
        /// Gets whether ReadAudioSamples(long, int, IntPtr) is a supported operation.
        /// </summary>
        bool SupportsFastReading
        {
            get;
        }
        
        /// <summary>
        /// Reads up to nAmount samples into an array at buf. Fast method because it avoids the Marshaller.
        /// This needn't be supported; the SupportsFastReading property indicates whether it is.
        /// </summary>
        /// <param name="nStart">the index of the first sample</param>
        /// <param name="nAmount">the maximum number of samples to read</param>
        /// <param name="buf">a pointer to the memory for the samples</param>
        /// <returns>the number of samples read</returns>
        long ReadAudioSamples(long nStart, int nAmount, IntPtr buf);

        /// <summary>
        /// Reads up to nAmount samples into an array and returns it. Slow method because of Marshaller.
        /// This must be supported.
        /// </summary>
        /// <param name="nStart">the index of the first sample</param>
        /// <param name="nAmount">the maximum number of samples to read</param>
        /// <returns>a newly-constructed array of samples</returns>
        byte[] ReadAudioSamples(long nStart, int nAmount);
    }
    /// <summary>
    /// The interface for sourcing frames
    /// </summary>
    public interface IVideoReader
    {
        /// <summary>
        /// Returns the number of frames readable.
        /// </summary>
        int FrameCount
        {
            get;
        }

        /// <summary>
        /// Reads and returns frame 'framenumber' from the video stream. Slow method, because of Marshaller.
        /// This must be supported
        /// </summary>
        /// <param name="framenumber">the 0-indexed frame number to get</param>
        /// <returns>The frame just read</returns>
        Bitmap ReadFrameBitmap(int framenumber);
    }
}