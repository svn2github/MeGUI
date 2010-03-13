// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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
using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI
{
    public class CalculationException : MeGUIException
    {
        public CalculationException(string message, Exception inner)
            : base(message, inner) { }
    }

	/// <summary>
	/// Summary description for BitrateCalculator.
	/// </summary>
	public class BitrateCalculator
	{
		#region constants
		private static readonly decimal mp4OverheadWithBframes = 10.4M;
        private static readonly decimal mp4OverheadWithoutBframes = 4.3M;
        private static readonly decimal aviVideoOverhead = 24M;
        private static readonly decimal cbrMP3Overhead = 23.75M;
        private static readonly decimal vbrMP3Overhead = 40M;
        private static readonly decimal ac3Overhead = 23.75M;
        private static readonly int AACBlockSize = 1024;
        private static readonly int AC3BlockSize = 1536;
        private static readonly int MP3BlockSize = 1152;
        private static readonly int VorbisBlockSize = 1024;
        private static readonly int mkvAudioTrackHeaderSize = 140;
        private static readonly int mkvVorbisTrackHeaderSize = 4096;
		private static readonly uint mkvIframeOverhead = 26;
		private static readonly uint mkvPframeOverhead = 13;
		private static readonly uint mkvBframeOverhead = 16;
		#endregion

        // FileSize desiredSize, 
        // out FileSize targetVideoSize
        public BitrateCalculator(VideoCodec codec, bool useBframes, ContainerType container, 
            AudioBitrateCalculationStream[] audioStreams, ulong nbOfFrames, double framerate, string extra)
		{
            nbOfSeconds = (decimal)nbOfFrames / (decimal)framerate;

            VideoOverhead = getVideoOverhead(container, useBframes, nbOfFrames, nbOfSeconds);

            AudioOverhead = FileSize.Empty;
            AudioSize = FileSize.Empty;
            foreach (AudioBitrateCalculationStream s in audioStreams)
            {
                AudioSize += s.Size ?? FileSize.Empty;
                AudioOverhead += getAudioOverhead(container, s.AType, nbOfSeconds, nbOfFrames);
            }

            ExtraSize = (!string.IsNullOrEmpty(extra)) ? FileSize.Parse(extra) : FileSize.Empty;     
        }

        public FileSize VideoOverhead;
        public FileSize AudioOverhead;
        public FileSize AudioSize;
        public FileSize ExtraSize;

        private decimal nbOfSeconds;

        public Tuple<ulong, FileSize> getBitrateAndVideoSize(FileSize desiredSize, ContainerType container)
        {
            try
            {
                if (container == ContainerType.M2TS)
                {
                    desiredSize = desiredSize * 100 / 106;
                    AudioSize = AudioSize * 106 / 100;
                    ExtraSize = ExtraSize * 2;
                }

                FileSize videoSize = desiredSize - VideoOverhead - AudioOverhead - AudioSize - ExtraSize;
                ulong sizeInBits = videoSize.Bytes * 8;
                
                return new Tuple<ulong, FileSize>((ulong)(sizeInBits / (nbOfSeconds * 1000)), videoSize);
            }
            catch (OverflowException e)
            {
                throw new CalculationException("The filesize cannot be obtained", e);
            }
        }

        public Tuple<FileSize, FileSize> getFileAndVideoSize(ulong bitrateKBits, ContainerType container)
        {

            decimal bytesPerSecond = bitrateKBits * 1000 / 8;

            FileSize videoSize = new FileSize(Unit.B, (nbOfSeconds * bytesPerSecond));
            if (container == ContainerType.M2TS)
            {
                videoSize = videoSize * 106 / 100;
                AudioSize = AudioSize * 106 / 100;
                ExtraSize = ExtraSize * 2;
            }

            return new Tuple<FileSize, FileSize>(videoSize + VideoOverhead + AudioOverhead + AudioSize + ExtraSize, videoSize);
        }


        #region overheads
        private static FileSize getAudioOverhead(ContainerType container, AudioType type, decimal nbSeconds, ulong nbOfFrames)
        {
            if (container == ContainerType.MP4)
                return FileSize.Empty;
            else if (container == ContainerType.MKV)
                return new FileSize(Unit.B, getMKVAudioOverhead(type, 48000, (double)nbSeconds));
            else if (container == ContainerType.M2TS)
                return new FileSize(Unit.B, getM2TSAudioOverhead(type, 48000, (double)nbSeconds));
            else if (container == ContainerType.AVI)
                return new FileSize(Unit.B, getAviAudioOverhead(type) * nbOfFrames);

            throw new Exception();
        }

        private FileSize getVideoOverhead(ContainerType container, bool useBframes, ulong nbOfFrames, decimal nbofSeconds)
        {
            if (container == ContainerType.MP4)
            {
                return new FileSize(Unit.B,
                    (useBframes ? mp4OverheadWithBframes : mp4OverheadWithoutBframes) * nbOfFrames);
            }
            else if (container == ContainerType.MKV)
            {
                ulong nbIframes = nbOfFrames / 10;
                ulong nbBframes = useBframes ? (nbOfFrames - nbIframes) / 2 : 0;
                ulong nbPframes = nbOfFrames - nbIframes - nbBframes;
                return new FileSize(Unit.B,
                    (4300M + 1400M + nbIframes * mkvIframeOverhead + nbPframes * mkvPframeOverhead +
                    nbBframes * mkvBframeOverhead + 
                    nbofSeconds * 12 / 2 // this line for 12 bytes per cluster overhoad
                    ));
            }
            else if (container == ContainerType.M2TS)
            {
                return new FileSize(Unit.B, 0);
            }
            else if (container == ContainerType.AVI)
            {
                return new FileSize(Unit.B, nbOfFrames * aviVideoOverhead);
            }
            throw new Exception();
        }


		/// <summary>
		/// gets the overhead a given audio type will incurr in the matroska container
		/// given its length and sampling rate
		/// </summary>
		/// <param name="AudioType">type of the audio track</param>
		/// <param name="samplingRate">sampling rate of the audio track</param>
		/// <param name="length">length of the audio track</param>
		/// <returns>overhead this audio track will incurr</returns>
        public static int getMKVAudioOverhead(AudioType audioType, int samplingRate, double length)
        {
            if (audioType == null)
                return 0;
            Int64 nbSamples = Convert.ToInt64((double)samplingRate * length);
            int headerSize = mkvAudioTrackHeaderSize;
            int samplesPerBlock = 0;
            if (audioType == AudioType.MP4AAC || audioType == AudioType.M4A || audioType == AudioType.RAWAAC)
                samplesPerBlock = AACBlockSize;
            else if (audioType == AudioType.VBRMP3 || audioType == AudioType.CBRMP3 || audioType == AudioType.MP3 || audioType == AudioType.DTS)
                samplesPerBlock = MP3BlockSize;
            else if (audioType == AudioType.AC3)
                samplesPerBlock = AC3BlockSize;
            else if (audioType == AudioType.VORBIS)
            {
                samplesPerBlock = VorbisBlockSize;
                headerSize = mkvVorbisTrackHeaderSize;
            }
            else // unknown types..
            {
                samplesPerBlock = AC3BlockSize;
            }
            double blockOverhead = (double)nbSamples / (double)samplesPerBlock * 22.0 / 8.0;
            int overhead = (int)(headerSize + 5 * length + blockOverhead);
            return overhead;
        }

        /// <summary>
        /// gets the overhead a given audio type will incurr in the m2ts container
        /// given its length and sampling rate
        /// </summary>
        /// <param name="AudioType">type of the audio track</param>
        /// <param name="samplingRate">sampling rate of the audio track</param>
        /// <param name="length">length of the audio track</param>
        /// <returns>overhead this audio track will incurr</returns>
        public static int getM2TSAudioOverhead(AudioType audioType, int samplingRate, double length)
        {
            if (audioType == null)
                return 0;
            
            return 0;
        }

        /// <summary>
        /// gets the avi container overhead for the given audio type and bitrate mode
        /// bitrate mode only needs to be taken into account for MP3 but it's there for all cases nontheless
        /// </summary>
        /// <param name="AudioType">the type of audio</param>
        /// <param name="bitrateMode">the bitrate mode of the given audio type</param>
        /// <returns>the overhead in bytes per frame</returns>
        public static decimal getAviAudioOverhead(AudioType audioType)
        {
#warning overheads here are inconsistant with the ones on www.alexander-noe.com
            if (audioType == AudioType.AC3)
                return ac3Overhead;
            else if (audioType == AudioType.MP3)
                return vbrMP3Overhead;
            else if (audioType == AudioType.VBRMP3)
                return vbrMP3Overhead;
            else if (audioType == AudioType.CBRMP3)
                return cbrMP3Overhead;
            else if (audioType == AudioType.RAWAAC)
                return vbrMP3Overhead;
            else if (audioType == AudioType.DTS)
                return ac3Overhead;
            else
                return 0;
        }
        #endregion

        public static long CalculateFileSizeKB(VideoCodec vCodec, bool p, ContainerType containerType, AudioBitrateCalculationStream[] audioStreams, int bitrate, ulong nbOfFrames, double framerate, out int vidSize, string extra)
        {
            BitrateCalculator c = new BitrateCalculator(vCodec, p, containerType, audioStreams, nbOfFrames, framerate, extra);
            FileSize a, b;
            c.getFileAndVideoSize((ulong)bitrate, containerType).get(out a, out b);
            vidSize = (int)b.KB;
            return (long)a.KB;
        }

        public static int CalculateBitrateKBits(VideoCodec vCodec, bool p, ContainerType containerType, AudioBitrateCalculationStream[] audioStreams, ulong muxedSizeBytes, ulong numberOfFrames, double framerate, out ulong videoSizeKBs, string extra)
        {
            BitrateCalculator c = new BitrateCalculator(vCodec, p, containerType, audioStreams, numberOfFrames, framerate, extra);
            FileSize b;
            ulong a;
            c.getBitrateAndVideoSize(new FileSize(Unit.B, muxedSizeBytes), containerType).get(out a, out b);
            videoSizeKBs = b.KB;
            return (int)a;
        }
    }
}