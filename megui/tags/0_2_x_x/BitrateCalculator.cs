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
            AudioBitrateCalculationStream[] audioStreams, ulong nbOfFrames, double framerate)
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
        }

        public FileSize VideoOverhead;
        public FileSize AudioOverhead;
        public FileSize AudioSize;

        private decimal nbOfSeconds;

        public Tuple<ulong, FileSize> getBitrateAndVideoSize(FileSize desiredSize)
        {
            try
            {
                FileSize videoSize = desiredSize - VideoOverhead - AudioOverhead - AudioSize;
                ulong sizeInBits = videoSize.Bytes * 8;
                return new Tuple<ulong, FileSize>((ulong)(sizeInBits / (nbOfSeconds * 1000)), videoSize);
            }
            catch (OverflowException e)
            {
                throw new CalculationException("The filesize cannot be obtained", e);
            }
        }

        public Tuple<FileSize, FileSize> getFileAndVideoSize(ulong bitrateKBits)
        {

            decimal bytesPerSecond = bitrateKBits * 1000 / 8;

            FileSize videoSize = new FileSize(Unit.B, (nbOfSeconds * bytesPerSecond));

            return new Tuple<FileSize, FileSize>(videoSize + VideoOverhead + AudioOverhead + AudioSize, videoSize);
        }


        #region overheads
        private static FileSize getAudioOverhead(ContainerType container, AudioType type, decimal nbSeconds, ulong nbOfFrames)
        {
            if (container == ContainerType.MP4)
                return FileSize.Empty;
            else if (container == ContainerType.MKV)
                return new FileSize(Unit.B, getMKVAudioOverhead(type, 48000, (double)nbSeconds));
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
            int nbSamples = (int)((double)samplingRate * length);
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


		#region predefined output sizes
/*		/// <summary>
		/// gets the predefined output size given the index of the dropdown
		/// </summary>
		/// <param name="index">the index which is currently selected in the size selection dropdown</param>
		/// <returns>the size in kilobytes</returns>
		public static int getOutputSizeKBs(int index)
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
		public static string[] getPredefinedOutputSizes()
		{
			string[] values = {"1/4 CD", "1/2 CD", "1 CD", "2 CD", "3 CD", "1/3 DVD-R", "1/4 DVD-R", "1/5 DVD-R", "DVD-5", "DVD-9"};
			return values;
		}*/
		#endregion

        public static long CalculateFileSizeKB(VideoCodec vCodec, bool p, ContainerType containerType, AudioBitrateCalculationStream[] audioStreams, int bitrate, ulong nbOfFrames, double framerate, out int vidSize)
        {
            BitrateCalculator c = new BitrateCalculator(vCodec, p, containerType, audioStreams, nbOfFrames, framerate);
            FileSize a, b;
            c.getFileAndVideoSize((ulong)bitrate).get(out a, out b);
            vidSize = (int)b.KB;
            return (long)a.KB;
        }

        public static int CalculateBitrateKBits(VideoCodec vCodec, bool p, ContainerType containerType, AudioBitrateCalculationStream[] audioStreams, ulong muxedSizeBytes, ulong numberOfFrames, double framerate, out ulong videoSizeKBs)
        {
            BitrateCalculator c = new BitrateCalculator(vCodec, p, containerType, audioStreams, numberOfFrames, framerate);
            FileSize b;
            ulong a;
            c.getBitrateAndVideoSize(new FileSize(Unit.B, muxedSizeBytes)).get(out a, out b);
            videoSizeKBs = b.KB;
            return (int)a;
        }
    }
}
