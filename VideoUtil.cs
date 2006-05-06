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
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace MeGUI
{
	/// <summary>
	/// VideoUtil is used to perform various video related tasks, namely autocropping, 
	/// auto resizing
	/// </summary>
	public class VideoUtil
	{
		private MeGUI mainForm;
		private JobUtil jobUtil;
		public VideoUtil(MeGUI mainForm)
		{
			this.mainForm = mainForm;
			jobUtil = new JobUtil(mainForm);
        }
        #region Autocrop
        public static CropValues autocrop(VideoReader reader) 
		{
			int pos = reader.FrameCount / 4;
			int tenPercent = reader.FrameCount / 20;
			CropValues[] cropValues = new CropValues[10];
			for (int i = 0; i < 10; i++)
			{
				Bitmap b = reader.ReadFrameBitmap(pos);
				cropValues[i] = getAutoCropValues(b);
				pos += tenPercent;
			}
			bool error = false;
			CropValues final = getFinalAutocropValues(cropValues);
			if (!error)
			{
				return final;
			}
			else
			{
				final.left = -1;
				final.right = -1;
				final.top = -1;
				final.bottom = -1;
				return final;
			}
		}

		/// <summary>
		/// iterates through a set of CropValues and tries to find a majority of matching crop values. If a match is found, the crop values are returned
		/// if not, the minimum found value is returned for the value in question
		/// </summary>
		/// <param name="values">the CropValues array to be analyzed</param>
		/// <returns>the final CropValues</returns>
		public static CropValues getFinalAutocropValues(CropValues[] values)
		{
			int matchingLeftValues = 0, matchingTopValues = 0, matchingRightValues = 0, matchingBottomValues = 0;
			int minLeft = values[0].left, minTop = values[0].top, minRight = values[0].right, minBottom = values[0].bottom;
			CropValues retval = values[0].Clone();
			for (int i = 1; i < values.Length; i++)
			{
				if (values[i].left == values[i-1].left)
				{
					retval.left = values[i].left;
					matchingLeftValues++;
				}
				if (values[i].top == values[i-1].top)
				{
					retval.top = values[i].top;
					matchingTopValues++;
				}
				if (values[i].right == values[i-1].right)
				{
					retval.right = values[i].right;
					matchingRightValues++;
				}
				if (values[i].bottom == values[i-1].bottom)
				{
					retval.bottom = values[i].bottom;
					matchingBottomValues++;
				}
				if (values[i].left < minLeft)
					minLeft = values[i].left;
				if (values[i].top < minTop)
					minTop = values[i].top;
				if (values[i].right < minRight)
					minRight = values[i].right;
				if (values[i].bottom < minBottom)
					minBottom = values[i].bottom;
			}
			if (matchingLeftValues < values.Length / 2) // we have less than 50% matching values, use minimum found
				retval.left = minLeft;
			if (matchingTopValues < values.Length / 2)
				retval.top = minTop;
			if (matchingRightValues < values.Length / 2)
				retval.right = minRight;
			if (matchingBottomValues < values.Length / 2)
				retval.bottom = minBottom;
			return retval;
		}
		
		private static bool isBadPixel(int pixel)
		{
			int comp = 12632256;
			int res = pixel & comp;
			return (res != 0);
		}
		/// <summary>
		/// iterates through the lines and columns of the bitmap and compares the pixel values with the value of the upper left corner pixel
		/// if a pixel that doesn't have the same color value is found, it is assumed that this is the first line that does not need to be cropped away
		/// </summary>
		/// <param name="b">the bitmap to be analyzed</param>
		/// <returns>struct containing the number of lines to be cropped away from the left, top, right and bottom</returns>
		public static unsafe CropValues getAutoCropValues(Bitmap b)
		{
			// When locking the pixels into memory, they are currently being converted from 24bpp to 32bpp. This incurs a small (5%) speed penalty,
			// but means that pixel management is easier, because each pixel is a 4-byte int.
			BitmapData image = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			int* pointer = (int*) image.Scan0.ToPointer();
			int* lineBegin, pixel;
			int stride = image.Stride / 4;
			CropValues retval = new CropValues();
			bool lineFound = false;
			int badPixelThreshold = 50;
			int widthBadPixelThreshold = b.Width / badPixelThreshold;
			int heightBadPixelThreshold = b.Height / badPixelThreshold;
			int nbBadPixels = 0;
	
			lineBegin = pointer;
			for (int i = 0; i < b.Width; i++)
			{
				pixel = lineBegin;
				for (int j = 0; j < b.Height; j++)
				{
					//if (b.GetPixel(i, j) != prevColor)
					//if (isBadPixel(b.GetPixel(i, j)))
					if (isBadPixel(*pixel))
						nbBadPixels++;
					if (nbBadPixels  > heightBadPixelThreshold)
					{
						retval.left = i;
						if (retval.left < 0)
							retval.left = 0;
						if (retval.left % 2 != 0)
							retval.left++;
						lineFound = true;
						break;
					}
					pixel += stride;
				}
				nbBadPixels = 0;
				if (lineFound)
					break;
				lineBegin += 1; // 4-byte Argb
			}
			nbBadPixels = 0;
			lineFound = false;
			lineBegin = pointer;
			for (int i = 0; i < b.Height; i++)
			{
				pixel = lineBegin;
				for (int j = 0; j < b.Width; j++)
				{
					//if (b.GetPixel(j, i) != prevColor)
					//if (isBadPixel(b.GetPixel(j, i)))
					if (isBadPixel(*pixel))
						nbBadPixels++;
					if (nbBadPixels > widthBadPixelThreshold)
					{
						retval.top = i;
						if (retval.top < 0)
							retval.top = 0;
						if (retval.top % 2 != 0)
							retval.top++;
						lineFound = true;
						break;
					}
					pixel += 1; // 4-byte Argb
				}
				nbBadPixels = 0;
				if (lineFound)
					break;
				lineBegin += stride;
			}
			nbBadPixels = 0;
			lineFound = false;
			lineBegin = pointer + b.Width - 1;
			for (int i = b.Width - 1; i >= 0 ; i--)
			{
				pixel = lineBegin;
				for (int j = 0; j < b.Height; j++)
				{
					//if (b.GetPixel(i, j) != prevColor)
					//if (isBadPixel(b.GetPixel(i, j)))
					if (isBadPixel(*pixel))
						nbBadPixels++;
					if (nbBadPixels > heightBadPixelThreshold)
					{
						retval.right = b.Width - i;
						if (retval.right < 0)
							retval.right = 0;
						if (retval.right % 2 != 0)
							retval.right++;
						lineFound = true;
						break;
					}
					pixel += stride;
				}
				nbBadPixels = 0;
				if (lineFound)
					break;
				lineBegin -= 1; // Backwards across 4-byte Argb
			}
			nbBadPixels = 0;
			lineFound = false;
			lineBegin = pointer + stride * (b.Height-1);
			for (int i = b.Height - 1; i >= 0 ; i--)
			{
				pixel = lineBegin;
				for (int j = 0; j < b.Width; j++)
				{
					//if (b.GetPixel(j, i) != prevColor)
					//if (isBadPixel(b.GetPixel(j, i)))
					if (isBadPixel(*pixel))
						nbBadPixels++;
					if (nbBadPixels > widthBadPixelThreshold)
					{
						retval.bottom = b.Height - i;
						if (retval.bottom < 0)
							retval.bottom = 0;
						if (retval.bottom % 2 != 0)
							retval.bottom++;
						lineFound = true;
						break;
					}
					pixel += 1;// 4-byte Argb
				}
				nbBadPixels = 0;
				if (lineFound)
					break;
				lineBegin -= stride;
			}
			return retval;
		}
		#endregion
		#region SuggestResolution
		/// <summary>
		/// if enabled, each change of the horizontal resolution triggers this method
		/// it calculates the ideal mod 16 vertical resolution that matches the desired horizontal resolution
		/// </summary>
		/// <param name="readerHeight">height of the source</param>
		/// <param name="readerWidth">width of the source</param>
		/// <param name="customDAR">custom display aspect ratio to be taken into account for resizing</param>
		/// <param name="cropping">the crop values for the source</param>
		/// <param name="horizontalResolution">the desired horizontal resolution of the output</param>
		/// <param name="signalAR">whether or not we're going to signal the aspect ratio (influences the resizing)</param>
		/// <param name="sarX">horizontal pixel aspect ratio (used when signalAR = true)</param>
		/// <param name="sarY">vertical pixel aspect ratio (used when signalAR = true)</param>
		/// <returns>the suggested horizontal resolution</returns>
		public static int suggestResolution(double readerHeight, double readerWidth, double customDAR, CropValues cropping, int horizontalResolution,
			bool signalAR, int acceptableAspectError, out int sarX, out int sarY)
		{
            double fractionOfWidth = (readerWidth - (double)cropping.left - (double)cropping.right) / readerWidth;
            double inputWidthOnHeight = (readerWidth - (double)cropping.left - (double)cropping.right) /
                                          (readerHeight - (double)cropping.top - (double)cropping.bottom);
            double sourceHorizontalResolution = readerHeight * customDAR * fractionOfWidth;
            double sourceVerticalResolution = readerHeight - (double)cropping.top - (double)cropping.bottom;
            double realAspectRatio = sourceHorizontalResolution / sourceVerticalResolution; // the real aspect ratio of the video
            realAspectRatio = getAspectRatio(realAspectRatio, acceptableAspectError); // Constrains DAR to a set of limited possibilities
			double resizedVerticalResolution = (double)horizontalResolution / realAspectRatio;

            int scriptVerticalResolution = ((int)Math.Round(resizedVerticalResolution / 16.0)) * 16;

            if (signalAR)
			{
                resizedVerticalResolution = (double)horizontalResolution / inputWidthOnHeight; // Scale vertical resolution appropriately
                scriptVerticalResolution = ((int)Math.Round(resizedVerticalResolution / 16.0) * 16);

                int parX = 0;
                int parY = 0;
                double distance = 999999;
                for (int i = 1; i < 101; i++)
                {
                    // We create a fraction with integers, and then convert back to a double, and see how big the rounding error is
                    double fractionApproximation = (double)Math.Round(realAspectRatio * ((double)i)) / (double)i;
                    double approximationDifference = Math.Abs(realAspectRatio - fractionApproximation);
                    if (approximationDifference < distance)
                    {
                        distance = approximationDifference;
                        parY = i;
                        parX = (int)Math.Round(realAspectRatio * ((double)parY));
                    }
                }
				//sarX = (int)Math.Round(realAspectRatio * resizedVerticalResolution);
                //sarY = (int)Math.Round(horizontalResolution);
                sarX = parX;
                sarY = parY;
				
				return scriptVerticalResolution;
			}
			else
			{
				sarX = 0;
				sarY = 0;
				return scriptVerticalResolution;
			}
		}
		
		/// <summary>
		/// finds the aspect ratio closest to the one giving as parameter (which is an approximation using the selected DAR for the source and the cropping values)
		/// </summary>
		/// <param name="calculatedAR">the aspect ratio to be approximated</param>
		/// <returns>the aspect ratio that most closely matches the input</returns>
		private static double getAspectRatio(double calculatedAR, int acceptableAspectErrorPercent)
		{
			double[] availableAspectRatios = {1.0, 1.33333, 1.66666, 1.77778, 1.85, 2.35};
			double[] distances = new double[availableAspectRatios.Length];
			double minDist = 1000.0;
			double realAspectRatio = 1.0;
			foreach (double d in availableAspectRatios)
			{
                double dist = Math.Abs(d - calculatedAR);
				if (dist < minDist)
				{
					minDist = dist;
					realAspectRatio = d;
				}
			}
            double aspectError = realAspectRatio / calculatedAR;
            if (Math.Abs(aspectError - 1.0) * 100.0 < acceptableAspectErrorPercent)
                return realAspectRatio;
            else
                return calculatedAR;
		}

        /// <summary>
        /// rounds the output PAR to the closest matching predefined xvid profile
        /// </summary>
        /// <param name="sarX">horizontal component of the pixel aspect ratio</param>
        /// <param name="sarY">vertical component of the pixel aspect ratio</param>
        /// <param name="height">height of the desired output</param>
        /// <param name="width">width of the desired output</param>
        /// <returns>the closest profile match</returns>
        public static int roundXviDPAR(int parX, int parY, int height, int width)
        {
            double par = (double) parX / (double) parY;
            double[] pars = { 1, 1.090909, 1.454545, 0.090909, 1.212121 };
            double minDist = 1000;
            int closestIndex = 0;
            for (int i = 0; i < pars.Length; i++)
            {
                double first = Math.Max(par, pars[i]);
                double second = Math.Min(par, pars[i]);
                double dist = first - second;
                if (dist < minDist)
                {
                    minDist = dist;
                    closestIndex = i;
                }
            }
            return closestIndex;
        }
        #endregion
		#region finding source information
		/// <summary>
		/// gets the dvd decrypter generated stream information file
		/// </summary>
		/// <param name="fileName">name of the first vob to be loaded</param>
		/// <returns>full name of the info file or an empty string if no file was found</returns>
		public static string getInfoFileName(string fileName)
		{
			string path = MeGUI.GetDirectoryName(fileName);
			string name = Path.GetFileName(fileName);
			string vts = name.Substring(0, 6);
			string infoFile = "";
			string[] files = Directory.GetFiles(path, vts + "*.txt");
			foreach (string file in files)
			{
				if (file.IndexOf("Stream Information") != -1) // we found our file
				{
					infoFile = file;
					break;
				}
			}
			return infoFile;
		}
		/// <summary>
		/// gets the dvd decrypter generated chapter file
		/// </summary>
		/// <param name="fileName">name of the first vob to be loaded</param>
		/// <returns>full name of the chapter file or an empty string if no file was found</returns>
		public static string getChapterFile(string fileName)
		{
			string path = MeGUI.GetDirectoryName(fileName);
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
		public void getSourceInfo(string infoFile, out AudioTrackInfo[] audioTracks,
			out AspectRatio aspectRatio, out int maxHorizontalResolution)
		{
			StreamReader sr = null;
			ArrayList aTracks = new ArrayList();
			aspectRatio = AspectRatio.CUSTOM;
            maxHorizontalResolution = 5000;
			try
			{
				sr = new StreamReader(infoFile, System.Text.Encoding.Default);
				string line = "";
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
						if (ar.Equals("16:9"))
							aspectRatio = AspectRatio.A16x9;
						else if (ar.Equals("4:3"))
							aspectRatio = AspectRatio.A4x3;
						else if (ar.Equals("1:1"))
							aspectRatio = AspectRatio.A1x1;
						else
							aspectRatio = AspectRatio.CUSTOM;
					}
					if (line.IndexOf("Audio") != -1)
					{
						char[] separator = {'/'};
						string[] split = line.Split(separator, 1000); 
						AudioTrackInfo ati = new AudioTrackInfo();
						ati.type = split[0].Substring(split[0].LastIndexOf("-") + 1).Trim();
						if (ati.type.Equals("DTS"))
							continue; // skip DTS tracks as BeSweet can't handle them
						string trackID = split[0].Substring(3, 1);
						ati.trackID = Int32.Parse(trackID) + 1;
						ati.nbChannels = split[1].Trim();
						ati.language = split[4].Trim();
						aTracks.Add(ati);
					}
				}
			}
			catch (Exception i)
			{
				MessageBox.Show("The following error ocurred when parsing the info file " + infoFile + "\r\n" + i.Message, "Error parsing info file", MessageBoxButtons.OK);
				aTracks.Clear();
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
			audioTracks = new AudioTrackInfo[aTracks.Count];
			int index = 0;
			foreach (object o in aTracks)
			{
				audioTracks[index] = (AudioTrackInfo)o;
				index++;
			}
		}
		/// <summary>
		/// gets all the audio languages from a defined source info file
		/// </summary>
		/// <param name="infoFile">the info file containing the language info</param>
		/// <returns>an array listing all tracks and their language</returns>
		public List<string> getAudioLanguages(string infoFile)
		{
			List<string> retval = new List<string>();
			AudioTrackInfo[] audioTracks;
			AspectRatio ar;
            int maxHorizontalResolution;
			getSourceInfo(infoFile, out audioTracks, out ar, out maxHorizontalResolution);
			foreach (AudioTrackInfo ati in audioTracks)
			{
				retval.Add(ati.language);
			}
			return retval;

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
		public bool openVideoSource(string fileName, ComboBox track1, ComboBox track2, out List<string> audioTypes, out AspectRatio ar, out int maxHorizontalResolution)
		{
			audioTypes = new List<string>();
            string infoFile = VideoUtil.getInfoFileName(fileName);
			bool putDummyTracks = true; // indicates whether audio tracks have been found or not
			ar = AspectRatio.CUSTOM;
            maxHorizontalResolution = 5000;
			if (!infoFile.Equals(""))
			{
				AudioTrackInfo[] atis;
				getSourceInfo(infoFile, out atis, out ar, out maxHorizontalResolution);
				if (atis.Length > 0)
				{
					putDummyTracks = false;
				}
				int index = 0;
				foreach (AudioTrackInfo ati in atis)
				{
                    audioTypes.Add(ati.type);
                    track1.Items.Add(ati.language + " " + ati.type + " " + ati.nbChannels);
					track2.Items.Add(ati.language + " " + ati.type + " " + ati.nbChannels);
					if (ati.language.Equals(mainForm.Settings.DefaultLanguage1) && track1.SelectedIndex == -1)
						track1.SelectedIndex = index;
					if (ati.language.Equals(mainForm.Settings.DefaultLanguage2) && track2.SelectedIndex == -1)
						track2.SelectedIndex = index;
					index++;
				}	
			}
			else
			{
				if (Path.GetExtension(fileName).ToLower().Equals(".vob") || Path.GetExtension(fileName).ToLower().Equals(".ifo"))
					MessageBox.Show("Could not find DVD Decrypter generated info file " + infoFile, "Missing File", MessageBoxButtons.OK);
			}
			if (putDummyTracks)
			{
				track1.Items.AddRange(new string[] {"Track 1", "Track 2", "Track 3", "Track 4", "Track 5", "Track 6", "Track 7", "Track 8"});
				track2.Items.AddRange(new string[] {"Track 1", "Track 2", "Track 3", "Track 4", "Track 5", "Track 6", "Track 7", "Track 8"});
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
			string[] files = Directory.GetFiles(MeGUI.GetDirectoryName(projectName) + @"\",
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
		public string eliminatedDuplicateFilenames(ref string videoOutput, ref string muxedOutput, AudioStream[] aStreams)
		{
            StringBuilder logBuilder = new StringBuilder();
            videoOutput = Path.GetFullPath(videoOutput);
            muxedOutput = Path.GetFullPath(muxedOutput);

            if (File.Exists(videoOutput))
            {
                logBuilder.AppendFormat("Video output file '{0}' already exists. Finding new name...{1}", videoOutput, Environment.NewLine);
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

                logBuilder.AppendFormat("New filename found: '{0}'{1}", videoOutput, Environment.NewLine);
            }

            if (File.Exists(muxedOutput) || muxedOutput == videoOutput)
            {
                logBuilder.AppendFormat("Muxed output file '{0}' already exists. Finding new name...{1}", muxedOutput, Environment.NewLine);
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

                logBuilder.AppendFormat("New filename found: '{0}'{1}", muxedOutput, Environment.NewLine);
            }

			for (int i = 0; i < aStreams.Length; i++)
			{
				string name = Path.GetFullPath(aStreams[i].output);
				if (name.Equals(videoOutput) || name.Equals(muxedOutput)) // audio will be overwritten -> no good
				{
					name = MeGUI.GetDirectoryName(name) + @"\" + Path.GetFileNameWithoutExtension(name) + i.ToString() + Path.GetExtension(name);
					logBuilder.Append("Encodable audio stream number " + i + " has the same name as a video file\r\n" 
						+ "Renaming stream to " + name);
					aStreams[i].output = name;
				}
			}
			return logBuilder.ToString();

		}
        #endregion
        #region SAR calculation
        public static void findSAR(int parX, int parY, int hres, int vres, out int sarX, out int sarY)
        {
            // sarX
            // ----   must be the amount the video needs to be stretched horizontally.
            // sarY
            //
            //    horizontalResolution
            // --------------------------  is the ratio of the pixels. This must be stretched to equal realAspectRatio
            //  scriptVerticalResolution
            //
            // To work out the stretching amount, we then divide realAspectRatio by the ratio of the pixels:
            // sarX      parX        horizontalResolution        realAspectRatio * scriptVerticalResolution
            // ---- =    ---- /   -------------------------- =  --------------------------------------------   
            // sarY      parY     scriptVerticalResolution               horizontalResolution
            sarX = parX * vres;
            sarY = parY * hres;
            reduce(ref sarX, ref sarY);

            if (parX < 1 || parY < 1)
            {
                sarX = sarY = -1;
            }
        }

        /// <summary>
        /// Puts x and y in simplest form, by dividing by all their factors.
        /// </summary>
        /// <param name="x">First number to reduce</param>
        /// <param name="y">Second number to reduce</param>
        public static void reduce(ref int x, ref int y)
        {
            int i = 2;
            while (i < x && i < y)
            {
                if (x % i == 0 &&
                    y % i == 0)
                {
                    x /= i;
                    y /= i;
                }
                else
                    i++;
            }
        }
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
                using (AvsReader avi = AvsReader.OpenScriptFile(avsFile))
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
        public void GenerateJobSeries(VideoStream video, string muxedOutput, AudioStream[] audioStreams,
            SubStream[] subtitles, string chapters, long desiredSize, int splitSize, ContainerType container, bool prerender, SubStream[] muxOnlyAudio)
        {
            StringBuilder logBuilder = new StringBuilder();
            BitrateCalculator calc = new BitrateCalculator();
            if (desiredSize > 0)
            {
                logBuilder.Append("Generating jobs. Desired size: " + desiredSize + " bytes\r\n");
                if (video.Settings.EncodingMode != 4 && video.Settings.EncodingMode != 8) // no automated 2/3 pass
                {
                    if (this.mainForm.Settings.NbPasses == 2)
                        video.Settings.EncodingMode = 4; // automated 2 pass
                    else if (video.Settings.MaxNumberOfPasses == 3)
                        video.Settings.EncodingMode = 8;
                }
            }
            else
                logBuilder.Append("Generating jobs. No desired size.\r\n");
            bool encodedAudioPresent = true;
            if (audioStreams.Length > 0)
                encodedAudioPresent = false;
            fixFileNameExtensions(video, audioStreams, container);
            string videoOutput = video.Output;
            logBuilder.Append(eliminatedDuplicateFilenames(ref videoOutput, ref muxedOutput, audioStreams));
            video.Output = videoOutput;
            int freeJobNumber = this.mainForm.getFreeJobNumber();
            VideoJob[] vjobs = jobUtil.prepareVideoJob(video.Input, video.Output, video.Settings, prerender, true);
            List<Job> jobs = new List<Job>();

            List<SubStream> allAudioToMux = new List<SubStream>();
            allAudioToMux.AddRange(muxOnlyAudio);
            foreach (AudioStream stream in audioStreams)
            {
                SubStream newStream = new SubStream();
                newStream.language = "";
                newStream.name = "";
                newStream.path = stream.output;
                allAudioToMux.Add(newStream);
            }

            if (vjobs.Length > 0) // else the user aborted and we cannot proceed
            {
                /* Here, we guess the types of the files based on extension.
                 * This is guaranteed to work with MeGUI-encoded files, because
                 * the extension will always be recognised. For non-MeGUI files,
                 * we can only ever hope.*/
                List<AudioType> allInputAudioTypes = new List<AudioType>();
                foreach (SubStream muxStream in allAudioToMux)
                    if (VideoUtil.guessAudioType(muxStream.path) != null)
                        allInputAudioTypes.Add(VideoUtil.guessAudioType(muxStream.path));

                List<SubtitleType> allInputSubtitleTypes = new List<SubtitleType>();
                foreach (SubStream muxStream in subtitles)
                    if (VideoUtil.guessSubtitleType(muxStream.path) != null) 
                        allInputSubtitleTypes.Add(VideoUtil.guessSubtitleType(muxStream.path));

                MuxJob[] muxJobs = this.jobUtil.GenerateMuxJobs(video, allAudioToMux.ToArray(), allInputAudioTypes.ToArray(),
                    subtitles, allInputSubtitleTypes.ToArray(), chapters, container, muxedOutput, splitSize);

                int index = 0;
                foreach (AudioStream astream in audioStreams) // generate audio encoding jobs
                {
                    AudioJob jo = jobUtil.generateAudioJob(astream);
                    jobs.Add(jo);
                    index++;
                }
                index = 0;
                jobs.AddRange(vjobs);
                jobs.AddRange(muxJobs);
                /*
                foreach (VideoJob job in vjobs)
                {
                    jobs.Add(job);
                }
                foreach (MuxJob job in muxJobs)
                {
                    jobs.Add(job);
                }
                 */
                string prevName = "";
                int number = 1;
                foreach (Job job in jobs)
                {
                    job.Name = "job" + freeJobNumber + "-" + number;
                    if (!prevName.Equals(""))
                        job.Previous = prevName;
                    if (jobs.Count > number)
                    {
                        int n = number + 1;
                        job.Next = "job" + freeJobNumber + "-" + n;
                    }
                    number++;
                    prevName = job.Name;
                }
                int bitrate = 0;
                if (desiredSize > 0) // We have a target filesize
                {
                    if (encodedAudioPresent) // no audio encoding, we can calculate the video bitrate directly
                    {
                        logBuilder.Append("No audio encoding. Calculating desired video bitrate directly.\r\n");
                        List<AudioStream> calculationAudioStreams = new List<AudioStream>();
                        foreach (SubStream stream in muxOnlyAudio)
                        {
                            FileInfo fi = new FileInfo(stream.path);
                            AudioStream newStream = new AudioStream();
                            newStream.Size = fi.Length;
                            newStream.Type = guessAudioType(stream.path);
                            newStream.BitrateMode = BitrateManagementMode.VBR;
                            calculationAudioStreams.Add(newStream);
                            logBuilder.Append("Encoded audio file is present: " + stream.path +
                                " It has a size of " + fi.Length + " bytes. \r\n");
                        }
                        
                        long videoSize;
                        bool useBframes = false;
                        if (video.Settings.NbBframes > 0)
                            useBframes = true;
                        
                        calc.CalculateBitrate(video.Settings.Codec, useBframes, container, calculationAudioStreams.ToArray(),
                            desiredSize, video.NumberOfFrames, video.Framerate, out videoSize);
                        desiredSize = (long)videoSize * 1024L; // convert kb back to bytes
                        logBuilder.Append("Setting video bitrate for the video jobs to " + bitrate + " kbit/s\r\n");
                        foreach (VideoJob vJob in vjobs)
                        {
                            jobUtil.updateVideoBitrate(vJob, bitrate);
                        }
                    }
                    logBuilder.Append("Setting desired size of video to " + desiredSize + " bytes\r\n");
                    foreach (VideoJob vJob in vjobs)
                    {
                        vJob.DesiredSize = desiredSize;
                    }
                }
                else
                {
                    logBuilder.Append("User doesn't care what the filesize is. Leaving bitrate/qp/crf at the profile's value");
                    foreach (VideoJob vJob in vjobs)
                    {
                        vJob.DesiredSize = -1;
                    }
                }
                foreach (Job job in jobs)
                {
                    this.mainForm.addJobToQueue(job);
                }
                mainForm.addToLog(logBuilder.ToString());
                if (mainForm.Settings.AutoStartQueue)
                    mainForm.startEncoding(jobs[0]);
            }

        }

        private void fixFileNameExtensions(VideoStream video, AudioStream[] audioStreams, ContainerType container)
        {
#warning This should be rearranged to work better
            // This is a very convoluted way to match up the audio and video streams with the extension given by the types in the MuxPath
            AudioCodec[] audioCodecs = new AudioCodec[audioStreams.Length];
            for (int i = 0; i < audioStreams.Length; i++)
            {
                audioCodecs[i] = audioStreams[i].settings.Codec;
            }
            MuxPath path = new MuxProvider().GetMuxPath(video.Settings.Codec, audioCodecs, new SubtitleType[0], container, 1 + audioStreams.Length);
            if (path == null)
                return;
            List<AudioType> audioTypes = new List<AudioType>();
            foreach (OutputType type in path.InitialInputTypes)
            {
                if (type is VideoType)
                {
                    video.Output = Path.ChangeExtension(video.Output,type.Extension);
                    video.VideoType = (VideoType)type;
                }
                if (type is AudioType)
                {
                    audioTypes.Add((AudioType)type);
                }
            }
            AudioEncoderProvider aProvider = new AudioEncoderProvider();
            for (int i = 0; i < audioStreams.Length; i++)
            {
                AudioType[] types = aProvider.GetSupportedOutput(audioStreams[i].settings.Codec);
                foreach (AudioType type in types)
                {
                    if (audioTypes.Contains(type))
                    {
                        audioStreams[i].output = Path.ChangeExtension(audioStreams[i].output,
                            type.Extension);
                        audioStreams[i].Type = type;
                        break;
                    }
                }
            }
        }

        public static SubtitleType guessSubtitleType(string p)
        {
            switch (Path.GetExtension(p.ToLower()))
            {
                case ".srt":
                    return SubtitleType.SUBRIP;
                case ".idx":
                    return SubtitleType.VOBSUB;
            }
            return null;
        }

        public static VideoType guessVideoType(string p)
        {
            switch (Path.GetExtension(p.ToLower()))
            {
                case ".avi":
                    return VideoType.AVI;
                case ".mkv":
                    return VideoType.MKV;
                case ".mp4":
                    return VideoType.MP4;
                case ".m4v":
                    return VideoType.RAWASP;
                case ".264":
                    return VideoType.RAWAVC;
            }
            return null;
        }
 
        public static AudioType guessAudioType(string p)
        {
            switch (Path.GetExtension(p.ToLower()))
            {
                case ".mp3":
                    return AudioType.MP3;

                case ".mp2":
                    return AudioType.MP2;

                case ".ac3":
                    return AudioType.AC3;

                case ".dts":
                    return AudioType.DTS;

                case ".mp4":
                    return AudioType.MP4AAC;

                case ".aac":
                case ".m4a":
                    return AudioType.RAWAAC;

                case ".ogg":
                    return AudioType.VORBIS;
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
                    ScriptServer.GetInputLine(filename, sourceType, false, false, false, -1));
                output.Close();
            }
            catch (IOException)
            {
                return null;
            }
            return outputFile;
        }

        public static string GenerateCombinedFilter(OutputType[] types)
        {
            StringBuilder initialFilterName = new StringBuilder();
            StringBuilder initialFilter = new StringBuilder();
            StringBuilder allSmallFilters = new StringBuilder();
            initialFilterName.Append("All supported files (");
            foreach (OutputType type in types)
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
	public sealed class CropValues
	{
		public int left, top, right, bottom;
        public CropValues Clone()
        {
            return (CropValues)this.MemberwiseClone();
        }
	}
	public struct AudioTrackInfo
	{
		public string language, nbChannels, type;
		public int trackID;
	}
	#endregion
}
