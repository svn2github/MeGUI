// ****************************************************************************
// 
// Copyright (C) 2005  Doom9
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;

namespace MeGUI
{
	/// <summary>
	/// Summary description for BitrateCalculator.
	/// </summary>
	public class BitrateCalculator
	{
		#region constants
		private decimal mp4OverheadWithBframes = new decimal(10.4);
		private decimal mp4OverheadWithoutBframes = new decimal(4.3);
		private double aviVideoOverhead = 24;
		private double cbrMP3Overhead = 23.75;
		private double vbrMP3Overhead = 40;
		private double ac3Overhead = 23.75;
		private int AACBlockSize = 1024;
		private int AC3BlockSize = 1536;
		private int MP3BlockSize = 1152;
		private int VorbisBlockSize = 1024;
		private int mkvAudioTrackHeaderSize = 140;
		private int mkvVorbisTrackHeaderSize = 4096;
		private int mkvIframeOverhead = 26;
		private int mkvPframeOverhead = 13;
		private int mkvBframeOverhead = 16;
		#endregion
		public BitrateCalculator()
		{}
		#region mp4
        /// <summary>
        /// calculates the video bitrate for a video to be put into the MP4 container
        /// </summary>
        /// <param name="audioStreams">the audio streams to be muxed</param>
        /// <param name="desiredOutputSize">the desired size of the muxed output</param>
        /// <param name="nbOfFrames">number of frames of the source</param>
        /// <param name="useBframes">whether we have b-frames in the video</param>
        /// <param name="framerate">framerate of the video</param>
        /// <param name="videoSize">size of the raw video stream</param>
        /// <returns>the calculated bitrate</returns>
        private int calculateMP4VideoBitrate(AudioStream[] audioStreams, long desiredOutputSizeBytes, int nbOfFrames,
            bool useBframes, double framerate, out long videoSizeKB)
        {
            double mp4Overhead = this.getMP4Overhead(useBframes);
            double totalOverhead = (double)nbOfFrames * mp4Overhead;
            double nbOfSeconds = (double)nbOfFrames / framerate;
            long audioSize = 0;
            foreach (AudioStream stream in audioStreams)
            {
                audioSize += stream.SizeBytes;
            }
            long videoTargetSize = desiredOutputSizeBytes - audioSize - (long)totalOverhead;
            videoSizeKB = videoTargetSize / 1024L;
            long sizeInBits = videoTargetSize * 8;
            int bitrate = (int)(sizeInBits / (nbOfSeconds * 1000));
            return bitrate;
        }
        /// <summary>
        /// calculates the size of a muxed mp4 file given the desired video bitrate and the audio streams to be muxed
        /// </summary>
        /// <param name="audioStreams">the audio streams to be muxed with the video</param>
        /// <param name="desiredBitrate">the desired video bitrate</param>
        /// <param name="nbOfFrames">the number of frames of the video source</param>
        /// <param name="useBframes">whether the sources uses b-frames</param>
        /// <param name="framerate">the framerate of the source</param>
        /// <param name="rawVideoSize">the raw video size the stream will have in the container</param>
        /// <returns>the size of the mp4 file in KB</returns>
        private long calculateMP4Size(AudioStream[] audioStreams, int desiredBitrate, int nbOfFrames, bool useBframes, double framerate, out int rawVideoSize)
        {
            double mp4Overhead = this.getMP4Overhead(useBframes);
            double totalOverhead = (double)nbOfFrames * mp4Overhead;
            double nbOfSeconds = (double)nbOfFrames / framerate;
            double bytesPerSecond = desiredBitrate * 1000 / 8;
            long videoSize = (long)(nbOfSeconds * bytesPerSecond);
            rawVideoSize = (int)(videoSize / (long)1024);
            long audioSize = 0;
            foreach (AudioStream stream in audioStreams)
            {
                audioSize += stream.SizeBytes;
            }
            long size = videoSize + audioSize + (long)totalOverhead;
            return size / 1024L;
        }
		#endregion
		#region matroska
        /// <summary>
        /// calculates the bitrate a video with the given properties needs to have in order to be placed into a matroska file along with the
        /// given audio tracks and end up having the desired size
        /// </summary>
        /// <param name="audioStreams">te audio streams to be muxed with this video</param>
        /// <param name="desiredOutputSize">the final size of this file</param>
        /// <param name="nbOfFrames">number of frames of the video</param>
        /// <param name="framerate">framerate of the video</param>
        /// <param name="useBframes">whether the video uses b-frames</param>
        /// <param name="videoSize">size of the raw video stream in KB</param>
        /// <returns>the video bitrate in kbit/s</returns>
        private int calculateMKVVideoBitrate(AudioStream[] audioStreams, long desiredOutputSize, int nbOfFrames, double framerate, bool useBframes, out long videoSize)
        {
            double totalOverhead = 0.0;
            int nbIframes = nbOfFrames / 10;
            int nbBframes = 0;
            if (useBframes)
                nbBframes = (nbOfFrames - nbIframes) / 2;
            int nbPframes = nbOfFrames - nbIframes - nbBframes;
            totalOverhead = (double)(4300 + 1400 + nbIframes * mkvIframeOverhead + nbPframes * mkvPframeOverhead +
                nbBframes * mkvBframeOverhead);
            double nbOfSeconds = (double)nbOfFrames / framerate;
            totalOverhead += nbOfSeconds / 2.0 * 12; // 12 bytes per cluster
            long audioSize = 0L;
            double audioOverhead = 0;
            foreach (AudioStream stream in audioStreams)
            {
                audioSize += stream.SizeBytes;
                audioOverhead += getMKVAudioOverhead(stream.Type, 48000, nbOfSeconds);
            }
            long videoTargetSize = desiredOutputSize - audioSize - (long)audioOverhead -
                (long)totalOverhead;
            videoSize = (int)(videoTargetSize / (long)1024);
            long sizeInBits = videoTargetSize * 8;
            int bitrate = (int)(sizeInBits / (nbOfSeconds * 1000));
            return bitrate;
        }
        /// <summary>
        /// calculates what size a given video and audio stream(s) will have at a given video bitrate
        /// </summary>
        /// <param name="audioStreams">the audio streams to be considered</param>
        /// <param name="desiredBitrate">the desired video bitrate</param>
        /// <param name="nbOfFrames">number of frames of the video source</param>
        /// <param name="framerate">framerate of the video source</param>
        /// <param name="useBframes">whether we use b-frames for the video</param>
        /// <param name="rawVideoSize">the raw size of the video stream in KB</param>
        /// <returns>the size of the final file in KB</returns>
        private long calculateMKVSize(AudioStream[] audioStreams, int desiredBitrate, int nbOfFrames, double framerate, bool useBframes, out int rawVideoSize)
        {
            double totalOverhead = 0.0;
            int nbIframes = nbOfFrames / 10;
            int nbBframes = 0;
            if (useBframes)
                nbBframes = (nbOfFrames - nbIframes) / 2;
            int nbPframes = nbOfFrames - nbIframes - nbBframes;
            totalOverhead = (double)(4300 + 1400 + nbIframes * mkvIframeOverhead + nbPframes * mkvPframeOverhead +
                nbBframes * mkvBframeOverhead);
            double nbOfSeconds = (double)nbOfFrames / framerate;
            totalOverhead += nbOfSeconds / 2.0 * 12; // 12 bytes per cluster

            long audioSize = 0L;
            double audioOverhead = 0;
            foreach (AudioStream stream in audioStreams)
            {
                audioSize += stream.SizeBytes;
                audioOverhead += getMKVAudioOverhead(stream.Type, 48000, nbOfSeconds);
            }

            totalOverhead += audioOverhead;
            double bytesPerSecond = desiredBitrate * 1000 / 8;
            long videoSize = (long)(nbOfSeconds * bytesPerSecond);
            rawVideoSize = (int)(videoSize / (long)1024);
            long size = videoSize + audioSize + (long)totalOverhead;
            return size / 1024L;
        }
		/// <summary>
		/// gets the overhead a given audio type will incurr in the matroska container
		/// given its length and sampling rate
		/// </summary>
		/// <param name="AudioType">type of the audio track</param>
		/// <param name="samplingRate">sampling rate of the audio track</param>
		/// <param name="length">length of the audio track</param>
		/// <returns>overhead this audio track will incurr</returns>
        public int getMKVAudioOverhead(AudioType audioType, int samplingRate, double length)
        {
            if (audioType == null)
                return 0;
            int nbSamples = (int)((double)samplingRate * length);
            int headerSize = mkvAudioTrackHeaderSize;
            int samplesPerBlock = 0;
            if (audioType == AudioType.MP4AAC)
                samplesPerBlock = AACBlockSize;
            else if (audioType == AudioType.VBRMP3 || audioType == AudioType.CBRMP3 || audioType == AudioType.MP3)
                samplesPerBlock = MP3BlockSize;
            else if (audioType == AudioType.AC3)
                samplesPerBlock = AC3BlockSize;
            else if (audioType == AudioType.VORBIS)
            {
                samplesPerBlock = VorbisBlockSize;
                headerSize = mkvVorbisTrackHeaderSize;
            }
            else // unknown types.. we presume the same overhead as for DTS
            {
                samplesPerBlock = AC3BlockSize;
            }
            double blockOverhead = (double)nbSamples / (double)samplesPerBlock * 22.0 / 8.0;
            int overhead = (int)(headerSize + 5 * length + blockOverhead);
            return overhead;
        }
		#endregion
		#region avi
        /// <summary>
        /// calculates the AVI bitrate given the desired audio streams and video stream properties
        /// </summary>
        /// <param name="audioStreams">the audio streams to be muxed to the video</param>
        /// <param name="desiredOutputSize">the desired final filesize</param>
        /// <param name="nbOfFrames">the number of frames of the source</param>
        /// <param name="framerate">the framerate of the source</param>
        /// <param name="videoSize">the size of the raw video stream</param>
        /// <returns>the bitrate in kbit/s</returns>
        private int calculateAVIBitrate(AudioStream[] audioStreams, long desiredOutputSize, int nbOfFrames, double framerate, out long videoSize)
        {
            double videoOverhead = (double)nbOfFrames * aviVideoOverhead;
            double nbOfSeconds = (double)nbOfFrames / framerate;
            double totalOverhead = videoOverhead;
            long audioSize = 0;
            foreach (AudioStream stream in audioStreams)
            {
                audioSize += stream.SizeBytes;
                if (stream.SizeBytes > 0)
                {
                    double audioOverhead = getAviAudioOverhead(stream.Type, stream.BitrateMode);
                    totalOverhead += audioOverhead * nbOfFrames;
                }
            }
            long videoTargetSize = desiredOutputSize - audioSize - (long)totalOverhead;
            videoSize = (int)(videoTargetSize / (long)1024);
            long sizeInBits = videoTargetSize * 8;
            int bitrate = (int)(sizeInBits / (nbOfSeconds * 1000));
            return bitrate;
        }
        private long calculateAVISize(AudioStream[] audioStreams, int desiredBitrate, int nbOfFrames, double framerate, out int rawVideoSize)
        {
            double videoOverhead = this.aviVideoOverhead;
            double nbOfSeconds = (double)nbOfFrames / framerate;
            long audioSize = 0L;
            double audioOverhead = 0;
            foreach (AudioStream stream in audioStreams)
            {
                audioSize += stream.SizeBytes;
                if (stream.SizeBytes > 0)
                    audioOverhead += getAviAudioOverhead(stream.Type, stream.BitrateMode) * nbOfFrames;
            }
            double totalOverhead = videoOverhead + audioOverhead;
            double bytesPerSecond = desiredBitrate * 1000 / 8;
            long videoSize = (long)(nbOfSeconds * bytesPerSecond);
            rawVideoSize = (int)(videoSize / (long)1024);
            long size = videoSize + (long)audioSize + (long)totalOverhead;
            return size / 1024L;
        }
        /// <summary>
        /// gets the avi container overhead for the given audio type and bitrate mode
        /// bitrate mode only needs to be taken into account for MP3 but it's there for all cases nontheless
        /// </summary>
        /// <param name="AudioType">the type of audio</param>
        /// <param name="bitrateMode">the bitrate mode of the given audio type</param>
        /// <returns>the overhead in bytes per frame</returns>
        public double getAviAudioOverhead(AudioType audioType, BitrateManagementMode bitrateMode)
        {
#warning overheads here are inconsistant with the ones on www.alexander-noe.com
            double audioOverhead = 0;
            if (audioType == AudioType.AC3)
                audioOverhead = ac3Overhead;
            else if (audioType == AudioType.MP3)
            {
                if (bitrateMode == BitrateManagementMode.CBR)
                    audioOverhead = cbrMP3Overhead;
                else
                    audioOverhead = vbrMP3Overhead;
            }
            else if (audioType == AudioType.VBRMP3)
                audioOverhead = vbrMP3Overhead;
            else if (audioType == AudioType.CBRMP3)
                audioOverhead = cbrMP3Overhead;
            else if (audioType == AudioType.RAWAAC)
                audioOverhead = vbrMP3Overhead;
            else if (audioType == AudioType.DTS)
                audioOverhead = ac3Overhead;
            else
                audioOverhead = 0;
            return audioOverhead;
        }
        /// <summary>
		/// gets the avi container overhead given the type of audio file to be muxed
		/// </summary>
		/// <param name="AudioType">the type of audio in question</param>
		/// <returns>the overhead per video frame for the given audio type</returns>
        public double getAviAudioOverhead(AudioType audioType)
        {
            double audioOverhead = 0;
            if (audioType == AudioType.AC3)
                audioOverhead = ac3Overhead;
            else if (audioType == AudioType.CBRMP3)
                audioOverhead = cbrMP3Overhead;
            else if (audioType == AudioType.VBRMP3)
                audioOverhead = vbrMP3Overhead;
            else if (audioType == AudioType.MP4AAC)
                audioOverhead = 0;
            else
                audioOverhead = 0;
            return audioOverhead;
        }
		#endregion
        #region generic calculations
        public int CalculateBitrateKBits(VideoCodec codec, bool useBframes, ContainerType container, AudioStream[] audioStreams, long desiredOutputSizeBytes, int nbOfFrames, double framerate, out long videoSizeKB)
        {
            switch (container)
            {
                case ContainerType.MP4:
                    return calculateMP4VideoBitrate(audioStreams, desiredOutputSizeBytes, nbOfFrames, useBframes, framerate, out videoSizeKB);
                case ContainerType.AVI:
                    return calculateAVIBitrate(audioStreams, desiredOutputSizeBytes, nbOfFrames, framerate, out videoSizeKB);
                case ContainerType.MKV:
                    return calculateMKVVideoBitrate(audioStreams, desiredOutputSizeBytes, nbOfFrames, framerate, useBframes, out videoSizeKB);
            }
            videoSizeKB = 0;
            return 0;
        }
        public long CalculateFileSizeKB(VideoCodec codec, bool useBframes, ContainerType container, AudioStream[] audioStreams, int desiredBitrate, int nbOfFrames, double framerate, out int rawVideoSize)
        {
            switch (container)
            {
                case ContainerType.MP4:
                    return calculateMP4Size(audioStreams, desiredBitrate, nbOfFrames, useBframes, framerate, out rawVideoSize);
                case ContainerType.AVI:
                    return calculateAVISize(audioStreams, desiredBitrate, nbOfFrames, framerate, out rawVideoSize);
                case ContainerType.MKV:
                    return calculateMKVSize(audioStreams, desiredBitrate, nbOfFrames, framerate, useBframes, out rawVideoSize);
            }
            rawVideoSize = 0;
            return 0;
        }
        #endregion
        /// <summary>
        /// gets the video container overhead per frame given the b-frame choice
        /// </summary>
        /// <param name="useBframes">whether we have b-frames in the source or not (causes a great increase in overhead)</param>
        /// <returns>the overhead per video frame</returns>
        private double getMP4Overhead(bool useBframes)
        {
            if (useBframes)
                return (double)mp4OverheadWithBframes;
            else
                return (double)mp4OverheadWithoutBframes;
        }

		#region predefined output sizes
		/// <summary>
		/// gets the predefined output size given the index of the dropdown
		/// </summary>
		/// <param name="index">the index which is currently selected in the size selection dropdown</param>
		/// <returns>the size in kilobytes</returns>
		public int getOutputSizeKBs(int index)
		{
			if (index == 0) // 1/4 CD
				return 179200;
			if (index == 1) // 1/2 CD
				return 358400;
			if (index == 2) // 1 CD
				return 716800;
			if (index == 3) // 2 CDs
				return 1433600;
			if (index == 4) // 3 CDs
				return 2150400;
			if (index == 5) // 1/3 DVD
				return 1501184;
			if (index == 6) // 1/4 DVD
				return 1126400;
			if (index == 7) // 1/5 DVD
				return 901120;
			if (index == 8) // 1 DVD
				return 4586496;
            if (index == 9) //1 DVD-9
                return 8333760;
			return 716800;
		}
		/// <summary>
		/// gets all the predefined output sizes
		/// </summary>
		/// <returns>an array of strings</returns>
		public string[] getPredefinedOutputSizes()
		{
			string[] values = {"1/4 CD", "1/2 CD", "1 CD", "2 CD", "3 CD", "1/3 DVD-R", "1/4 DVD-R", "1/5 DVD-R", "DVD-5", "DVD-9"};
			return values;
		}
		#endregion
	}
}