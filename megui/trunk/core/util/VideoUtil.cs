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

using Utils.MessageBoxExLib;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.gui;
using MeGUI.core.util;

namespace MeGUI
{
	/// <summary>
	/// VideoUtil is used to perform various video related tasks, namely autocropping, 
	/// auto resizing
	/// </summary>
	public class VideoUtil
    {

        private MainForm mainForm;
		private JobUtil jobUtil;
		public VideoUtil(MainForm mainForm)
		{
			this.mainForm = mainForm;
			jobUtil = new JobUtil(mainForm);
        }

		#region finding source information
		/// <summary>
		/// gets the dvd decrypter generated stream information file
		/// </summary>
		/// <param name="fileName">name of the first vob to be loaded</param>
		/// <returns>full name of the info file or an empty string if no file was found</returns>
		public static string getInfoFileName(string fileName)
		{
			int pgc = 1;
			string path = Path.GetDirectoryName(fileName);
			string name = Path.GetFileName(fileName);
			string vts = name.Substring(0, 6);
			string infoFile = "";
			string[] files = Directory.GetFiles(path, vts + "*.txt");
			foreach (string file in files)
			{
				if (file.IndexOf("Stream Information") != -1) // we found our file
				{
                    int index = file.IndexOf("_PGC_");
                    if (index != -1) // PGC number is in the filename
                    {
                        string pgcString = file.Substring(index + 5, 2);
                        try
                        {
                            pgc = Int32.Parse(pgcString);
                        }
                        catch (Exception)
                        {
                        }
                    }
					infoFile = file;
					break;
				}
			}
            // pgc is parsed, but unused. Might be useful later
            return infoFile;
		}

		/// <summary>
		/// gets the dvd decrypter generated chapter file
		/// </summary>
		/// <param name="fileName">name of the first vob to be loaded</param>
		/// <returns>full name of the chapter file or an empty string if no file was found</returns>
		public static string getChapterFile(string fileName)
		{
			string path = Path.GetDirectoryName(fileName);
			string name = Path.GetFileName(fileName);
			string vts = name.Substring(0, 6);
			string chapterFile = "";
			string[] files = Directory.GetFiles(path, vts + "*.txt");
			foreach (string file in files)
			{
				if (file.IndexOf("Chapter Information - OGG") != -1) // we found our file
				{
					chapterFile = file;
					break;
				}
			}
			return chapterFile;
		}
		/// <summary>
		/// gets information about a video source based on its DVD Decrypter generated info file
		/// </summary>
		/// <param name="infoFile">the info file to be analyzed</param>
		/// <param name="audioTracks">the audio tracks found</param>
		/// <param name="aspectRatio">the aspect ratio of the video</param>
        public void getSourceInfo(string infoFile, out List<AudioTrackInfo> audioTracks, out List<SubtitleInfo> subtitles,
			out Dar? aspectRatio, out int maxHorizontalResolution)
		{
			StreamReader sr = null;
            audioTracks = new List<AudioTrackInfo>();
            subtitles = new List<SubtitleInfo>();
            aspectRatio = null;
            maxHorizontalResolution = 5000;
			try
			{
				sr = new StreamReader(infoFile, System.Text.Encoding.Default);
                string line = ""; int LineCount = 0;
				while ((line = sr.ReadLine()) != null)
				{
					if (line.IndexOf("Video") != -1)
					{
						char[] separator = {'/'};
						string[] split = line.Split(separator, 1000);
                        string resolution = split[1];
                        resolution = resolution.Substring(1, resolution.IndexOf('x')-1);
                        maxHorizontalResolution = Int32.Parse(resolution);
						string ar = split[2].Substring(1, split[2].Length - 2);

                        aspectRatio = Dar.A1x1;
                        if (split[1].Contains("PAL"))
                        {
                            if (ar.Equals("16:9"))
                                aspectRatio = Dar.ITU16x9PAL;
                            else if (ar.Equals("4:3"))
                                aspectRatio = Dar.ITU4x3PAL;
                        }
                        else if (split[1].Contains("NTSC"))
                        {
                            if (ar.Equals("16:9"))
                                aspectRatio = Dar.ITU16x9NTSC;
                            else if (ar.Equals("4:3"))
                                aspectRatio = Dar.ITU4x3NTSC;
                        }
					}
					else if (line.IndexOf("Audio") != -1)
					{
						char[] separator = {'/'};
						string[] split = line.Split(separator, 1000); 
						AudioTrackInfo ati = new AudioTrackInfo();
						ati.Type = split[0].Substring(split[0].LastIndexOf("-") + 1).Trim();
						ati.NbChannels = split[1].Trim();
                        ati.TrackInfo = new TrackInfo(split[4].Trim(), null);
                        audioTracks.Add(ati);
					}
                    else if (line.IndexOf("Subtitle") != -1)
                    {
                        char[] separator = { '-' };
                        string[] split = line.Split(separator, 1000);
                        string language = split[2].Trim();
                        SubtitleInfo si = new SubtitleInfo(language, LineCount);
                        LineCount++; // must be there coz vobsub index begins to zero...                        
                        subtitles.Add(si);
                    }
				}
			}
			catch (Exception i)
			{
				MessageBox.Show("The following error ocurred when parsing the info file " + infoFile + "\r\n" + i.Message, "Error parsing info file", MessageBoxButtons.OK);
                audioTracks.Clear();
			}
			finally
			{
				if (sr != null)
				{
					try 
					{
						sr.Close();
					}
					catch (IOException i)
					{
						Trace.WriteLine("IO Exception when closing StreamReader in VobInputWindow: " + i.Message);
					}
				}
			}
		}
        /// <summary>
        /// gets basic information about a video source based on its DGindex generated log file
        /// </summary>
        /// <param name="logFile">the log file to be analyzed</param>
        /// <param name="audioTrackIDs">the audio tracks IDs found</param>
        public void getDGindexLogInfo(string logFile, out List<AudioTrackInfo> audioTrackIDs)
        {
            StreamReader sr = null;
            audioTrackIDs = new List<AudioTrackInfo>();
            try
            {
                sr = new StreamReader(logFile, System.Text.Encoding.Default);
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.IndexOf("Audio Stream") != -1)
                    {
                        char[] separator = { ':' };
                        string[] split = line.Split(separator, 1000);
                        AudioTrackInfo ati = new AudioTrackInfo();
                        string myTrackID = split[0].Substring(split[0].Length + 1, 2).Trim();
                        ati.TrackID = Int32.Parse(myTrackID);
                        ati.Type = split[1].Substring(split[1].Length + 1, 3).Trim();
                        audioTrackIDs.Add(ati);
                    }
                }
            }
            catch
                (Exception i)
            {
                MessageBox.Show("The following error ocurred when parsing the log file " + logFile + "\r\n" + i.Message, "Error parsing log file", MessageBoxButtons.OK);
                audioTrackIDs.Clear();
            }
        }
        
		#endregion
		#region dgindex preprocessing
		/// <summary>
		/// opens a video source and fills out the track selector dropdowns
		/// </summary>
		/// <param name="fileName">the video input file</param>
		/// <param name="track1">combobox for audio track selection</param>
		/// <param name="track2">combobox for audio track selection</param>
		/// <param name="ar">aspect ratio of the video</param>
		/// <param name="trackIDs">an arraylist that will contain the track IDs of the source if found</param>
		/// <returns>true if a source info file has been found, false if not</returns>
        public bool openVideoSource(string fileName, out List<AudioTrackInfo> audioTracks, out List<SubtitleInfo> subtitles, 
            out Dar? ar, out int maxHorizontalResolution)
		{
            audioTracks = new List<AudioTrackInfo>();
            subtitles = new List<SubtitleInfo>();
            string infoFile = VideoUtil.getInfoFileName(fileName);
			bool putDummyTracks = true; // indicates whether audio tracks have been found or not
			ar = null;
            maxHorizontalResolution = 5000;
			if (!string.IsNullOrEmpty(infoFile))
			{
                getSourceInfo(infoFile, out audioTracks, out subtitles, out ar, out maxHorizontalResolution);
                if (audioTracks.Count > 0)
					putDummyTracks = false;
			}
			else
			{
				if (Path.GetExtension(fileName).ToLower().Equals(".vob") || Path.GetExtension(fileName).ToLower().Equals(".ifo"))
					MessageBox.Show("Could not find DVD Decrypter generated info file " + infoFile, "Missing File", MessageBoxButtons.OK);
			}
			if (putDummyTracks)
			{
                for (int i = 1; i <= 8; i++)
                {
                    audioTracks.Add(new AudioTrackInfo("Track " + i, "", "", i));
                }
                subtitles.Clear();
                for (int i = 1; i <= 32; i++)
                {
                    subtitles.Add(new SubtitleInfo("Track " + i, i));
                }
			}
			return putDummyTracks;
		}
		#endregion
		#region dgindex postprocessing
		/// <summary>
		/// gets all demuxed audio files from a given dgindex project
		/// starts with the first file and returns the desired number of files
		/// </summary>
		/// <param name="projectName">the name of the dgindex project</param>
		/// <param name="cutoff">maximum number of results to be returned</param>
		/// <returns>an array of string of filenames</returns>
		public Dictionary<int, string> getAllDemuxedAudio(string projectName, int cutoff)
		{
			Dictionary<int, string> audioFiles = new Dictionary<int, string>();
			string[] files = Directory.GetFiles(Path.GetDirectoryName(projectName),
				Path.GetFileNameWithoutExtension(projectName) + "*");
			int counter = 0;
			while (counter < cutoff)
			{
				string trackNumber = "T" + ((counter+1).ToString()).PadLeft(2, '0');
				foreach (string file in files)
				{
					if (file.IndexOf(trackNumber) != -1 &&
                        (file.EndsWith(".ac3") || 
                         file.EndsWith(".mp3") ||
                         file.EndsWith(".mp2") ||
                         file.EndsWith(".mpa") ||
                         file.EndsWith(".dts") ||
                         file.EndsWith(".wav"))) // It is the right track
					{
						audioFiles.Add(counter, file);
                        break;
					}
				}
				counter++;
			}
            return audioFiles;
		}
		#endregion
		#region automated job generation
		/// <summary>
		/// ensures that video and audio don't have the same filenames which would lead to severe problems
		/// </summary>
		/// <param name="videoOutput">name of the encoded video file</param>
		/// <param name="muxedOutput">name of the final output</param>
		/// <param name="aStreams">all encodable audio streams</param>
		/// <param name="audio">all muxable audio streams</param>
		/// <returns>the info to be added to the log</returns>
		public LogItem eliminatedDuplicateFilenames(ref string videoOutput, ref string muxedOutput, AudioJob[] aStreams)
		{
            LogItem log = new LogItem("Eliminating duplicate filenames");
            videoOutput = Path.GetFullPath(videoOutput);
            muxedOutput = Path.GetFullPath(muxedOutput);

            log.LogValue("Video output file", videoOutput);
            if (File.Exists(videoOutput))
            {
                int counter = 0;
                string directoryname = Path.GetDirectoryName(videoOutput);
                string filename = Path.GetFileNameWithoutExtension(videoOutput);
                string extension = Path.GetExtension(videoOutput);

                while (File.Exists(videoOutput))
                {
                    videoOutput = Path.Combine(directoryname,
                        filename + "_" + counter + extension);
                    counter++;
                }

                log.LogValue("File already exists. New video output filename", videoOutput);
            }

            log.LogValue("Muxed output file", muxedOutput);
            if (File.Exists(muxedOutput) || muxedOutput == videoOutput)
            {
                int counter = 0;
                string directoryname = Path.GetDirectoryName(muxedOutput);
                string filename = Path.GetFileNameWithoutExtension(muxedOutput);
                string extension = Path.GetExtension(muxedOutput);

                while (File.Exists(muxedOutput) || muxedOutput == videoOutput)
                {
                    muxedOutput = Path.Combine(directoryname,
                        filename + "_" + counter + extension);
                    counter++;
                }

                log.LogValue("File already exists. New muxed output filename", muxedOutput);
            }

			for (int i = 0; i < aStreams.Length; i++)
			{
				string name = Path.GetFullPath(aStreams[i].Output);
                log.LogValue("Encodable audio stream " + i, name);
				if (name.Equals(videoOutput) || name.Equals(muxedOutput)) // audio will be overwritten -> no good
				{
					name = Path.Combine(Path.GetDirectoryName(name), Path.GetFileNameWithoutExtension(name) + i.ToString() + Path.GetExtension(name));
					aStreams[i].Output = name;
                    log.LogValue("Stream has the same name as video stream. New audio stream output", name);
				}
			}
            return log;

		}
        #endregion
        #region SAR calculation

        #endregion
        #region source checking
        public string checkVideo(string avsFile)
        {
            return checkVideo(avsFile, true);
        }
        
        private string checkVideo(string avsFile, bool tryToFix)
        {
            try
            {
                using (AvsFile avi = AvsFile.OpenScriptFile(avsFile))
                {
                    if (avi.Clip.OriginalColorspace != AviSynthColorspace.YV12)
                    {
                        if (tryToFix)
                        {
                            bool convert = mainForm.DialogManager.addConvertToYV12(avi.Clip.OriginalColorspace.ToString());
                            if (convert)
                            {
                                if (appendConvertToYV12(avsFile))
                                {
                                    string sResult = checkVideo(avsFile, false); // Check everything again, to see if it is all fixed now
                                    if (sResult == null)
                                    {
                                        MessageBox.Show("Successfully converted to YV12.");
                                        return null;
                                    }
                                    else
                                    {
                                        return sResult;
                                    }
                                }
                            }
                            return "You didn't want me to append ConvertToYV12(). You'll have to fix the colorspace problem yourself.";
                        }
                        return string.Format("AviSynth clip is in {0} not in YV12, even though ConvertToYV12() has been appended.", avi.Clip.OriginalColorspace.ToString());
                    }

                    if (avi.Clip.VideoHeight % 16 != 0 ||
                        avi.Clip.VideoWidth % 16 != 0)
                        return string.Format("AviSynth clip doesn't have mod16 dimensions:\r\nWidth: {0}\r\nHeight:{1}\r\n" +
                            "This could cause problems with some encoders,\r\n" +
                            "and will also result in a loss of compressibility.\r\n" +
                            "I suggest you resize to a mod16 resolution.", avi.Clip.VideoWidth, avi.Clip.VideoHeight);
                }
            }
            catch (Exception e)
            {
                return "Error in AviSynth script:\r\n" + e.Message;
            }
            return null;
        }

        private bool appendConvertToYV12(string file)
        {
            try
            {
                StreamWriter avsOut = new StreamWriter(file, true);
                avsOut.WriteLine();
                avsOut.WriteLine("ConvertToYV12()");
                avsOut.Close();
            }
            catch (IOException)
            {
                return false; 
            }
            return true;
        }
        #endregion

        #region new stuff
        public JobChain GenerateJobSeries(VideoStream video, string muxedOutput, AudioJob[] audioStreams,
            MuxStream[] subtitles, string chapters, FileSize? desiredSize, FileSize? splitSize, ContainerType container, bool prerender, MuxStream[] muxOnlyAudio, LogItem log)
        {
            log.LogValue("Desired size", desiredSize);
            if (desiredSize.HasValue)
            {
                if (video.Settings.EncodingMode != 4 && video.Settings.EncodingMode != 8) // no automated 2/3 pass
                {
                    if (this.mainForm.Settings.NbPasses == 2)
                        video.Settings.EncodingMode = 4; // automated 2 pass
                    else if (video.Settings.MaxNumberOfPasses == 3)
                        video.Settings.EncodingMode = 8;
                }
            }

            fixFileNameExtensions(video, audioStreams, container);
            string videoOutput = video.Output;
            log.Add(eliminatedDuplicateFilenames(ref videoOutput, ref muxedOutput, audioStreams));
            video.Output = videoOutput;

            JobChain vjobs = jobUtil.prepareVideoJob(video.Input, video.Output, video.Settings, video.DAR, prerender, true, null);

            if (vjobs == null) return null;
            /* Here, we guess the types of the files based on extension.
             * This is guaranteed to work with MeGUI-encoded files, because
             * the extension will always be recognised. For non-MeGUI files,
             * we can only ever hope.*/
            List<MuxStream> allAudioToMux = new List<MuxStream>();
            List<MuxableType> allInputAudioTypes = new List<MuxableType>();
            foreach (MuxStream muxStream in muxOnlyAudio)
            {
                if (VideoUtil.guessAudioMuxableType(muxStream.path, true) != null)
                {
                    allInputAudioTypes.Add(VideoUtil.guessAudioMuxableType(muxStream.path, true));
                    allAudioToMux.Add(muxStream);
                }
            }

            foreach (AudioJob stream in audioStreams)
            {
                allAudioToMux.Add(stream.ToMuxStream());
                allInputAudioTypes.Add(stream.ToMuxableType());
            }


            List<MuxableType> allInputSubtitleTypes = new List<MuxableType>();
            foreach (MuxStream muxStream in subtitles)
                if (VideoUtil.guessSubtitleType(muxStream.path) != null)
                    allInputSubtitleTypes.Add(new MuxableType(VideoUtil.guessSubtitleType(muxStream.path), null));

            MuxableType chapterInputType = null;
            if (!String.IsNullOrEmpty(chapters))
            {
                ChapterType type = VideoUtil.guessChapterType(chapters);
                if (type != null)
                    chapterInputType = new MuxableType(type, null);
            }

            List<string> inputsToDelete = new List<string>();
            inputsToDelete.Add(video.Output);
            inputsToDelete.AddRange(Array.ConvertAll<AudioJob, string>(audioStreams, delegate(AudioJob a) { return a.Output; }));

            JobChain muxJobs = this.jobUtil.GenerateMuxJobs(video, video.Framerate, allAudioToMux.ToArray(), allInputAudioTypes.ToArray(),
                subtitles, allInputSubtitleTypes.ToArray(), chapters, chapterInputType, container, muxedOutput, splitSize, inputsToDelete);

            if (desiredSize.HasValue)
            {
                BitrateCalculationInfo b = new BitrateCalculationInfo();
                
                List<string> audiofiles = new List<string>();
                foreach (MuxStream s in allAudioToMux)
                    audiofiles.Add(s.path);
                b.AudioFiles = audiofiles;

                b.Container = container;
                b.VideoJobs = new List<TaggedJob>(vjobs.Jobs);
                b.DesiredSize = desiredSize.Value;
                ((VideoJob)vjobs.Jobs[0].Job).BitrateCalculationInfo = b;
            }


            return 
                new SequentialChain(
                    new ParallelChain((Job[])audioStreams),
                    new SequentialChain(vjobs),
                    new SequentialChain(muxJobs));

        }

        private void fixFileNameExtensions(VideoStream video, AudioJob[] audioStreams, ContainerType container)
        {
            AudioEncoderType[] audioCodecs = new AudioEncoderType[audioStreams.Length];
            for (int i = 0; i < audioStreams.Length; i++)
            {
                audioCodecs[i] = audioStreams[i].Settings.EncoderType;
            }
            MuxPath path = mainForm.MuxProvider.GetMuxPath(video.Settings.EncoderType, audioCodecs, container);
            if (path == null)
                return;
            List<AudioType> audioTypes = new List<AudioType>();
            foreach (MuxableType type in path.InitialInputTypes)
            {
                if (type.outputType is VideoType)
                {
                    video.Output = Path.ChangeExtension(video.Output,type.outputType.Extension);
                    video.VideoType = type;
                }
                if (type.outputType is AudioType)
                {
                    audioTypes.Add((AudioType)type.outputType);
                }
            }
            AudioEncoderProvider aProvider = new AudioEncoderProvider();
            for (int i = 0; i < audioStreams.Length; i++)
            {
                AudioType[] types = aProvider.GetSupportedOutput(audioStreams[i].Settings.EncoderType);
                foreach (AudioType type in types)
                {
                    if (audioTypes.Contains(type))
                    {
                        audioStreams[i].Output = Path.ChangeExtension(audioStreams[i].Output,
                            type.Extension);
                        break;
                    }
                }
            }
        }

        public static SubtitleType guessSubtitleType(string p)
        {
            foreach (SubtitleType type in ContainerManager.SubtitleTypes.Values)
            {
                if (Path.GetExtension(p.ToLower()) == "." + type.Extension)
                    return type;
            }
            return null;
        }

        public static VideoType guessVideoType(string p)
        {
            foreach (VideoType type in ContainerManager.VideoTypes.Values)
            {
                if (Path.GetExtension(p.ToLower()) == "." + type.Extension)
                    return type;
            }
            return null;
        }
 
        public static AudioType guessAudioType(string p)
        {
            foreach (AudioType type in ContainerManager.AudioTypes.Values)
            {
                if (Path.GetExtension(p.ToLower()) == "." + type.Extension)
                    return type;
            }
            return null;
        }

        public static ChapterType guessChapterType(string p)
        {
            foreach (ChapterType type in ContainerManager.ChapterTypes.Values)
            {
                if (Path.GetExtension(p.ToLower()) == "." + type.Extension)
                    return type;
            }
            return null;
        }

        public static MuxableType guessVideoMuxableType(string p, bool useMediaInfo)
        {
            if (string.IsNullOrEmpty(p))
                return null;
            if (useMediaInfo)
            {
                MediaInfoFile info = new MediaInfoFile(p);
                if (info.Info.HasVideo)
                    return new MuxableType(info.VideoType, info.VCodec);
                // otherwise we may as well try the other route too
            }
            VideoType vType = guessVideoType(p);
            if (vType != null)
            {
                if (vType.SupportedCodecs.Length == 1)
                    return new MuxableType(vType, vType.SupportedCodecs[0]);
                else
                    return new MuxableType(vType, null);
            }
            return null;
        }

        public static MuxableType guessAudioMuxableType(string p, bool useMediaInfo)
        {
            if (string.IsNullOrEmpty(p))
                return null;
            if (useMediaInfo)
            {
                MediaInfoFile info = new MediaInfoFile(p);
                if (info.AudioType != null)
                    return new MuxableType(info.AudioType, info.ACodecs[0]);
            }
            AudioType aType = guessAudioType(p);
            if (aType != null)
            {
                if (aType.SupportedCodecs.Length == 1)
                    return new MuxableType(aType, aType.SupportedCodecs[0]);
                else
                    return new MuxableType(aType, null);
            }
            return null;
        }
        #endregion

        public static string createSimpleAvisynthScript(string filename)
        {
            PossibleSources sourceType = PossibleSources.directShow;
            if (filename.ToLower().EndsWith(".vdr"))
                sourceType = PossibleSources.vdr;
            string outputFile = filename + ".avs";
            if (File.Exists(outputFile))
            {
                DialogResult response = MessageBox.Show("The file, '" + outputFile + "' already exists.\r\n Do you want to overwrite it?",
                    "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (response == DialogResult.No)
                    return null;
            }
            try
            {
                StreamWriter output = new StreamWriter(outputFile);
                output.WriteLine(
                    ScriptServer.GetInputLine(filename, false, sourceType, false, false, false, -1));
                output.Close();
            }
            catch (IOException)
            {
                return null;
            }
            return outputFile;
        }

        public static string GetNameForNth(int n)
        {
            switch (n)
            {
                case 0:
                    return "zeroth";
                case 1:
                    return "first";
                case 2:
                    return "second";
                case 3:
                    return "third";
                case 4:
                    return "fourth";
                case 5:
                    return "fifth";
            }
            string number = n.ToString();
            if (number.EndsWith("1"))
                return number + "st";
            if (number.EndsWith("2"))
                return number + "nd";
            if (number.EndsWith("3"))
                return number + "rd";
            return number + "th";
        }

        public static string GenerateCombinedFilter(OutputFileType[] types)
        {
            StringBuilder initialFilterName = new StringBuilder();
            StringBuilder initialFilter = new StringBuilder();
            StringBuilder allSmallFilters = new StringBuilder();
            initialFilterName.Append("All supported files (");
            foreach (OutputFileType type in types)
            {
                initialFilter.Append(type.OutputFilter);
                initialFilter.Append(";");
                initialFilterName.Append(type.OutputFilter);
                initialFilterName.Append(", ");
                allSmallFilters.Append(type.OutputFilterString);
                allSmallFilters.Append("|");
            }

            string initialFilterTrimmed = initialFilterName.ToString().TrimEnd(' ', ',') + ")|" +
                initialFilter.ToString();

            if (types.Length > 1)
                return initialFilterTrimmed + "|" + allSmallFilters.ToString().TrimEnd('|');
            else
                return allSmallFilters.ToString().TrimEnd('|');
        }
    }
	#region helper structs
	/// <summary>
	/// helper structure for cropping
	/// holds the crop values for all 4 edges of a frame
	/// </summary>
	[LogByMembers]
    public sealed class CropValues
	{
		public int left, top, right, bottom;
        public CropValues Clone()
        {
            return (CropValues)this.MemberwiseClone();
        }
	}
    public class AudioTrackInfo
    {
        private string nbChannels, type;
        private int trackID;
        public AudioTrackInfo() :this(null, null, null, 0)
        {
        }
        public AudioTrackInfo(string language, string nbChannels, string type, int trackID)
        {
            TrackInfo = new TrackInfo(language, null);
            this.nbChannels = nbChannels;
            this.type = type;
            this.trackID = trackID;
        }

        public string Language
        {
            get
            {
                if (TrackInfo == null) return null;
                return TrackInfo.Language;
            }
            set
            {
                if (TrackInfo == null)
                    TrackInfo = new TrackInfo();
                TrackInfo.Language = value;
            }
        }

        public TrackInfo TrackInfo;
        public int TrackID
        {
            get { return trackID; }
            set { trackID = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string NbChannels
        {
            get { return nbChannels; }
            set { nbChannels = value; }
        }

        public override string ToString()
        {
            string fullString = TrackInfo.Language + " " + this.type + " " + this.nbChannels;
            return fullString.Trim();
        }
    }
    public class SubtitleInfo
    {
        private string name;
        private int index;
        public SubtitleInfo(string name, int index)
        {
            this.name = name;
            this.index = index;
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public override string ToString()
        {
            return this.name;
        }
    }
	#endregion
}





