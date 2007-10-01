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
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MeGUI.core.util;

namespace MeGUI
{
    /// <summary>
    /// JobUtil is used to perform various job related tasks like loading/saving jobs, 
	/// generating all types of jobs, update bitrates in jobs, and get the properties
	/// of a video input file
	/// </summary>
	public class JobUtil
    {
       
        #region start/stop
		MainForm mainForm;
        AVCLevels al;
		public JobUtil(MainForm mainForm)
		{
			this.mainForm = mainForm;
            al = new AVCLevels();
        }
        #endregion
        #region loading and saving jobs
        /// <summary>
		/// saves a job to programdirectory\jobs\jobname.xml
		/// using the XML Serializer we get a humanly readable file
		/// </summary>
		/// <param name="job">the Job object to be saved</param>
		/// <param name="path">The path where the program was launched from</param>
		public void saveJob(Job job, string path)
		{
			XmlSerializer ser = null;
			string fileName = path + "\\jobs\\" + job.Name + ".xml";
			using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
			{
				try
				{
                    Type p = job.GetType();
                    ser = new XmlSerializer(typeof(Job));
					ser.Serialize(s, job);
				}
				catch (Exception e)
				{
					Console.Write(e.Message);
				}
			}
		}
		/// <summary>
		/// loads a job with a given name from programdirectory\jobs\jobname.xml
		/// </summary>
		/// <param name="name">name of the job to be loaded (corresponds to the filename)</param>
		/// <returns>the Job object that was read from the harddisk</returns>
		public Job loadJob(string name)
		{
			XmlSerializer ser = null;
			using (Stream s = File.OpenRead(name))
			{
				try
				{
					ser = new XmlSerializer(typeof(Job));
					return (Job)ser.Deserialize(s);
				}
				catch (Exception e)
				{
					DialogResult r = MessageBox.Show("Job " + name + " could not be loaded. Delete?", "Error loading Job", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (r == DialogResult.Yes)
                    {
                        try { s.Close(); File.Delete(name); }
                        catch (Exception) { }
                    }
                    Console.Write(e.Message);
					return null;
				}
			}
		}
		#endregion
		#region job generation
		#region single job generation
		/// <summary>
		/// generates a dgindex job
		/// </summary>
		/// <param name="input">video input</param>
		/// <param name="output">dgindex project name</param>
		/// <param name="demuxType">type of audio demux</param>
		/// <param name="trackID1">ID of first audio track</param>
		/// <param name="trackID2">ID of second audio track</param>
		/// <param name="properties">postprocessing properties</param>
		/// <returns>the indexing job</returns>
		public IndexJob generateIndexJob(string input, string output, int demuxType, int trackID1, int trackID2, 
			DGIndexPostprocessingProperties properties)
		{
			IndexJob job = new IndexJob();
			job.Input = input;
			job.Output = output;
			job.DemuxMode = demuxType;
			job.AudioTrackID1 = trackID1;
			job.AudioTrackID2 = trackID2;
			job.AutoForceFilm = mainForm.Settings.AutoForceFilm;
			job.ForceFilmThreshold = (double)mainForm.Settings.ForceFilmThreshold;
			job.PostprocessingProperties = properties;
            job.Commandline = CommandLineGenerator.generateDGIndexCommandline(demuxType, trackID1,
				trackID2, input, output);
			return job;
		}
        /// <summary>
        /// generates a vobsub indexing job
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="demuxAllTracks"></param>
        /// <param name="trackIDs"></param>
        /// <returns></returns>
        public SubtitleIndexJob generateSubtitleIndexJob(string input, string output, bool demuxAllTracks, List<int> trackIDs, int pgc)
        {
            SubtitleIndexJob job = new SubtitleIndexJob();
            job.Input = input;
            job.Output = output;
            string scriptOutput = Path.GetDirectoryName(output) + @"\" + Path.GetFileNameWithoutExtension(output);
            job.IndexAllTracks = demuxAllTracks;
            job.TrackIDs = trackIDs;
            bool fileError = false;
            string configFile = Path.ChangeExtension(input, ".vobsub");
            job.ScriptFile = configFile;
            using (StreamWriter sw = new StreamWriter(configFile, false, Encoding.Default))
            {
                try
                {
                    sw.WriteLine(input);
                    sw.WriteLine(scriptOutput);
                    sw.WriteLine(pgc);
                    sw.WriteLine("0"); // we presume angle processing has been done before
                    if (demuxAllTracks)
                        sw.WriteLine("ALL");
                    else
                    {
                        foreach (int id in trackIDs)
                        {
                            sw.Write(id + " ");
                        }
                        sw.Write(sw.NewLine);
                    }
                    sw.WriteLine("CLOSE");
                }
                catch (Exception)
                {
                    fileError = true;
                }
            }
            job.Commandline = CommandLineGenerator.generateVobSubCommandline(configFile);
            if (fileError)
                return null;
            return job;
        }
        public AviSynthJob generateAvisynthJob(string input)
        {
            AviSynthJob job = new AviSynthJob();
            job.Input = input;
            ulong nbFrames = 0;
            double framerate = 0.0;
            bool videoOK = getInputProperties(out nbFrames, out framerate, job.Input);
            if (!videoOK)
            {
                return null;
            }
            job.NumberOfFrames = nbFrames;
            job.Framerate = framerate;
            return job;
        }
        
        public VideoJob generateVideoJob(string input, string output, VideoCodecSettings settings, Dar? dar)
        {
            return generateVideoJob(input, output, settings, false, dar);
        }
        
        /// <summary>
		/// generates a videojob from the given settings
		/// returns the job and whether or not this is an automated job (in which case another job
		/// will have to be created)
		/// </summary>
		/// <param name="input">the video input (avisynth script)</param>
		/// <param name="output">the video output</param>
		/// <param name="settings">the codec settings for this job</param>
		/// <returns>the generated job or null if there was an error with the video source</returns>
		public VideoJob generateVideoJob(string input, string output, VideoCodecSettings settings, bool skipVideoCheck, Dar? dar)
		{
			VideoJob job = new VideoJob();
			job.Input = input;
			job.Output = output;
            job.DAR = dar;
			if (mainForm.Settings.AutoSetNbThreads)
				adjustNbThreads(settings);
			if (Path.GetDirectoryName(settings.Logfile).Equals("")) // no path set
				settings.Logfile = Path.ChangeExtension(input, ".stats");
			job.Settings = settings;
			if (job.Settings.EncodingMode == 4) // automated 2 pass, change type to 2 pass 2nd pass
			{
				job.Settings.EncodingMode = 3;
				job.Settings.Turbo = false;
			}
			else if (job.Settings.EncodingMode == 8) // automated 3 pass, change type to 3 pass first pass
			{
				if (mainForm.Settings.OverwriteStats)
					job.Settings.EncodingMode = 7;
				else
					job.Settings.EncodingMode = 3; // 2 pass 2nd pass.. doesn't overwrite the stats file
				job.Settings.Turbo = false;
			}
			ulong nbOfFrames = 0;
			double framerate = 0.0;
            bool videoOK = true;
            if (!skipVideoCheck)
                videoOK = getInputProperties(out nbOfFrames, out framerate, job.Input);
			if (!videoOK && !skipVideoCheck) // abort if the video is not okay
			{
                return null;
			}
			job.NumberOfFrames = nbOfFrames;
			job.Framerate = framerate;
            job.Commandline = CommandLineGenerator.generateVideoCommandline(job.Settings, input, output, dar);
			return job;
		}
        /// <summary>
        /// sets the number of encoder threads in function of the number of processors found on the system
        /// </summary>
        /// <param name="settings"></param>
        private void adjustNbThreads(VideoCodecSettings settings)
        {
            string nbProc = System.Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS");
            if (!String.IsNullOrEmpty(nbProc))
            {
                int nbCPUs = Int32.Parse(System.Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"));
                settings.setAdjustedNbThreads(nbCPUs);
            }
        }
        /// <summary>
		/// generates an audio encoding job from the currently configured audio settings in the GUI
		/// returns that job and the commandline generated from the settings (as opposed to the commandline in the commandline GUI field
		/// </summary>
		/// <param name="stream">the audio stream to be used as input for this job</param>
		/// <returns>a fully configured AudioJob</returns>
		public AudioJob generateAudioJob(AudioStream stream)
		{
			AudioJob job = new AudioJob();
			job.Input = stream.path;
			job.Output = stream.output;
            job.CutFile = stream.cutlist;
			job.Settings = stream.settings;
			// job.Commandline = CommandLineGenerator.generateAudioCommandline(mainForm.Settings, job.Settings, job.Input, job.Output); // no longer necessary
			return job;
		}

        public MuxJob[] GenerateMuxJobs(VideoStream video, SubStream[] audioStreamsArray, MuxableType[] audioTypes,
            SubStream[] subtitleStreamsArray, MuxableType[] subTypes,
            string chapterFile, MuxableType chapterInputType, ContainerType container, string output, FileSize? splitSize)
        {
            MuxProvider prov = mainForm.MuxProvider;
            List<MuxableType> allTypes = new List<MuxableType>();
            allTypes.Add(video.VideoType);
            allTypes.AddRange(audioTypes);
            allTypes.AddRange(subTypes);
            if (chapterInputType != null)
                allTypes.Add(chapterInputType);
            MuxPath muxPath = prov.GetMuxPath(container, allTypes.ToArray());
            List<MuxJob> jobs = new List<MuxJob>();
            List<SubStream> subtitleStreams = new List<SubStream>(subtitleStreamsArray);
            List<SubStream> audioStreams = new List<SubStream>(audioStreamsArray);
            int index = 0;
            int tempNumber = 1;
            string previousOutput = null;
            List<string> muxedFilesToDelete = new List<string>();
            foreach (MuxPathLeg mpl in muxPath)
            {
                MuxJob mjob = new MuxJob();

                if (previousOutput != null)
                {
                    mjob.Settings.MuxedInput = previousOutput;
                }

                mjob.NbOfFrames = video.NumberOfFrames;
                mjob.NbOfBFrames = video.Settings.NbBframes;
                mjob.Codec = video.Settings.Codec.ToString();
                mjob.Settings.Framerate = video.Framerate;

                string tempOutputName = Path.Combine(Path.GetDirectoryName(output),
                    Path.GetFileNameWithoutExtension(output) + tempNumber + ".");
                tempNumber++;
                foreach (MuxableType o in mpl.handledInputTypes)
                {
                    if (o.outputType is VideoType)
                    {
                        mjob.Settings.VideoInput = video.Output;
                        mjob.Settings.DAR = video.DAR;
                    }
                    else if (o.outputType is AudioType)
                    {
                        foreach (SubStream audioStream in audioStreams)
                        {
                            if (VideoUtil.guessAudioType(audioStream.path) == o.outputType)
                            {
                                mjob.Settings.AudioStreams.Add(audioStream);
                                //audioStreams.Remove(audioStream); // So that we don't mux this too many times
                            }
                        }
                    }
                    else if (o.outputType is SubtitleType)
                    {
                        foreach (SubStream subStream in subtitleStreams)
                        {
                            if ((VideoUtil.guessSubtitleType(subStream.path) == o.outputType))
                            {
                                mjob.Settings.SubtitleStreams.Add(subStream);
                                //audioStreams.Remove(subStream); // So that we don't mux this too many times
                            }
                        }
                    }
                    else if (o.outputType is ChapterType)
                    {
                        if ((VideoUtil.guessChapterType(chapterFile) == o.outputType))
                            mjob.Settings.ChapterFile = chapterFile;
                    }
                }
                foreach (SubStream s in mjob.Settings.AudioStreams)
                {
                    audioStreams.Remove(s);
                }
                foreach (SubStream s in mjob.Settings.SubtitleStreams)
                {
                    subtitleStreams.Remove(s);
                }
                if (index == muxPath.Length - 1)
                {
                    mjob.Settings.MuxedOutput = output;
                    mjob.Settings.SplitSize = splitSize;
                    mjob.Settings.DAR = video.DAR;
                    mjob.ContainerType = container;
                    mjob.FilesToDelete.AddRange(muxedFilesToDelete);
                }
                else
                {
                    ContainerType cot = mpl.muxerInterface.GetContainersInCommon(muxPath[index + 1].muxerInterface)[0];
                    mjob.Settings.MuxedOutput = tempOutputName + cot.Extension;
                    mjob.ContainerType = cot;
                    muxedFilesToDelete.Add(mjob.Settings.MuxedOutput);
                }
                previousOutput = mjob.Settings.MuxedOutput;
                index++;
                jobs.Add(mjob);
                if (string.IsNullOrEmpty(mjob.Settings.VideoInput))
                    mjob.Input = mjob.Settings.MuxedInput;
                else
                    mjob.Input = mjob.Settings.VideoInput;
                mjob.Output = mjob.Settings.MuxedOutput;
                mjob.MuxType = mpl.muxerInterface.MuxerType;
                mjob.Commandline = CommandLineGenerator.generateMuxCommandline(mjob.Settings, mjob.MuxType, mainForm);
            }

            for (int i = 1; i < jobs.Count; i++)
                jobs[i].AddDependency(jobs[i - 1]);

            return jobs.ToArray();
        }
/*		/// <summary>
		/// generates a muxing job based on a video and audio job that precede the muxing
		/// </summary>
		/// <param name="vjob">the video job whose output is to be muxed into an mp4</param>
		/// <param name="audioStreams">the audio streams to be muxed into the mp4</param>
		/// <returns>the complete MuxJob</returns>
		public MuxJob generateMuxJob(VideoJob vjob, SubStream[] audioStreams, SubStream[] subtitleStreams, 
			string chapterFile, MuxerType type, string output)
		{
			MuxJob job = new MuxJob();
			job.Input = vjob.Output;
			job.Output = output;
			job.NbOfFrames = vjob.NumberOfFrames;
			job.Settings.Framerate = vjob.Framerate;
			job.NbOfBFrames = vjob.Settings.NbBframes;
			job.Codec = vjob.CodecString;
			job.Settings.PARX = vjob.Settings.PARX;
			job.Settings.PARY = vjob.Settings.PARY;
			if (audioStreams != null)
			{
				foreach (SubStream stream in audioStreams)
				{
					job.Settings.AudioStreams.Add(stream);
				}
			}
			if (subtitleStreams != null)
			{
				foreach (SubStream stream in subtitleStreams)
				{
					job.Settings.SubtitleStreams.Add(stream);
				}
			}
			job.Settings.ChapterFile = chapterFile;
			job.Priority = mainForm.Settings.DefaultPriority;
			job.MuxType = type;
            job.Commandline = gen.generateMuxCommandline(job.Settings, job.MuxType);
			return job;
		}*/
		#endregion
		#region job preparation (aka multiple job generation)
        public bool AddAudioJob(string audioInput, string audioOutput, string cutFile, AudioCodecSettings settings)
        {
            AudioStream stream = new AudioStream();
            stream.path = audioInput;
            stream.output = audioOutput;
            stream.settings = settings;
            stream.cutlist = cutFile;
            AudioJob job = this.generateAudioJob(stream);
            if (job != null)
            {
                this.mainForm.Jobs.addJobsToQueue(job);
                return true;
            }
            return false;
        }
        public bool AddVideoJobs(string movieInput, string movieOutput, VideoCodecSettings settings,
            int introEndFrame, int creditsStartFrame, Dar? dar, bool prerender, bool checkVideo)
        {
            bool cont = getFinalZoneConfiguration(settings, introEndFrame, creditsStartFrame);
            if (!cont) // abort
                return false;
            VideoJob[] jobs = prepareVideoJob(movieInput, movieOutput, settings, dar, prerender, checkVideo);
            if (jobs == null)
                return false;
            if (jobs.Length > 0)
            {
                mainForm.Jobs.addJobsToQueue(jobs);
/*                if (jobs.Length > 1) // complex naming
                {
                    Job prevJob = null;
                    int number = 1;
                    foreach (VideoJob job in jobs)
                    {
                        job.Name = "job" + freeJobNumber + "-" + number;
                        if (prevJob != null)
                        {
                            job.Previous = prevJob;
                            prevJob.Next = job;
                        }
                        number++;
                        prevJob = job;
                    }
                    mainForm.Jobs.addJobsToQueue(jobs);
                }
                else // simple naming
                {
                    jobs[0].Name = "job" + freeJobNumber;
                    this.mainForm.Jobs.addJobsToQueue(jobs[0]);
                }*/
                return true;
            }
            return false;
        }
        public VideoJob[] prepareVideoJob(string movieInput, string movieOutput, VideoCodecSettings settings, Dar? dar)
        {
            return prepareVideoJob(movieInput, movieOutput, settings, dar, false, false);
        }
		/// <summary>
		/// at first, the job from the currently configured settings is generated. In addition, we find out if this job is 
		/// a part of an automated series of jobs. If so, it means the first generated job was the second pass, and we have
		/// to create the first pass using the same settings
		/// then, all the generated jobs are returned
		/// </summary>
		/// <returns>an Array of VideoJobs in the order they are to be encoded</returns>
		public VideoJob[] prepareVideoJob(string movieInput, string movieOutput, VideoCodecSettings settings, Dar? dar, bool prerender, bool checkVideo)
		{
			bool twoPasses = false, turbo = settings.Turbo, threePasses = false;
			if (settings.EncodingMode == 4) // automated twopass
				twoPasses = true;
			else if (settings.EncodingMode == 8) // automated threepass
				threePasses = true;

            VideoJob prerenderJob = null;
            string hfyuFile = null;
            string inputAVS = movieInput;
            if (prerender)
            {
                hfyuFile = Path.Combine(Path.GetDirectoryName(movieInput), "hfyu_" + 
                    Path.GetFileNameWithoutExtension(movieInput) + ".avi");
                inputAVS = Path.ChangeExtension(hfyuFile, ".avs");
                if (File.Exists(hfyuFile))
                {
                    if (MessageBox.Show("The intended temporary file, " + hfyuFile + " already exists.\r\n" +
                        "Do you wish to over-write it?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                        == DialogResult.No)
                        return null;
                }
                if (File.Exists(inputAVS))
                {
                    if (MessageBox.Show("The intended temporary file, " + inputAVS + " already exists.\r\n" +
                        "Do you wish to over-write it?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                        == DialogResult.No)
                        return null;
                }
                try
                {
                    StreamWriter hfyuWrapper = new StreamWriter(inputAVS);
                    hfyuWrapper.WriteLine("AviSource(\"" + hfyuFile + "\")");
                    hfyuWrapper.Close();
                }
                catch (IOException)
                {
                    return null;
                }
                prerenderJob = this.generateVideoJob(movieInput, hfyuFile, new hfyuSettings(), dar);
                if (prerenderJob == null)
                    return null;
            }
            if (checkVideo)
            {
                VideoUtil vUtil = new VideoUtil(mainForm);
                string error = vUtil.checkVideo(movieInput);
                if (error != null)
                {
                    bool bContinue = mainForm.DialogManager.createJobs(error);
                    if (!bContinue)
                    {
                        MessageBox.Show("Job creation aborted due to invalid AviSynth script");
                        return null;
                    }
                }
            }
            VideoJob job = this.generateVideoJob(inputAVS, movieOutput, settings, prerender, dar);
            if (prerender)
            {
                job.Framerate = prerenderJob.Framerate;
                job.NumberOfFrames = prerenderJob.NumberOfFrames;
            }
			VideoJob firstpass = null;
			VideoJob middlepass = null;
			if (job != null)
			{
				if (twoPasses || threePasses) // we just created the last pass, now create previous one(s)
				{
					job.FilesToDelete.Add(job.Settings.Logfile);
                    firstpass = cloneJob(job);
					firstpass.Output = ""; // the first pass has no output
					firstpass.Settings.EncodingMode = 2;
					firstpass.Settings.Turbo = turbo;
                    firstpass.Commandline = CommandLineGenerator.generateVideoCommandline(firstpass.Settings, 
						firstpass.Input, firstpass.Output, dar);
					if (threePasses)
					{
						firstpass.Settings.EncodingMode = 5; // change to 3 pass 3rd pass just for show
						middlepass = cloneJob(job);
						middlepass.Settings.EncodingMode = 6; // 3 pass 2nd pass
						middlepass.Settings.Turbo = false;
                        if (mainForm.Settings.Keep2of3passOutput) // give the 2nd pass a new name
                        {
                            middlepass.Output = Path.Combine(Path.GetDirectoryName(job.Output), Path.GetFileNameWithoutExtension(job.Output)
                                + "-2ndpass" + Path.GetExtension(job.Output));
                            job.FilesToDelete.Add(middlepass.Output);
                        }
                        middlepass.Commandline = CommandLineGenerator.generateVideoCommandline(middlepass.Settings, 
							middlepass.Input, middlepass.Output, dar);
					}
				}
                if (prerender)
                {
                    job.FilesToDelete.Add(hfyuFile);
                    job.FilesToDelete.Add(inputAVS);
                }
                List<VideoJob> jobList = new List<VideoJob>();
                if (prerenderJob != null)
                    jobList.Add(prerenderJob);
                if (firstpass != null)
                    jobList.Add(firstpass);
                if (middlepass != null) // we have a middle pass
                    jobList.Add(middlepass);
                jobList.Add(job);

                for (int i = 1; i < jobList.Count; i++)
                {
                    jobList[i].AddDependency(jobList[i - 1]);
                }
                return jobList.ToArray();
			}
			return null;
		}
		/// <summary>
		/// creates a copy of the most important parameters of a job
		/// </summary>
		/// <param name="oldJob">the job to be cloned</param>
		/// <returns>the cloned job</returns>
		private VideoJob cloneJob(VideoJob oldJob)
		{
			VideoJob job = new VideoJob();
			job.Input = oldJob.Input;
			job.NumberOfFrames = oldJob.NumberOfFrames;
			job.Framerate = oldJob.Framerate;
			job.Output = oldJob.Output;
			job.Settings = oldJob.Settings.clone();
			return job;
		}
		#endregion
		#endregion
		#region bitrate updates
		/// <summary>
		/// updates the video bitrate of a video job with the given bitrate
		/// in addition, the commandline is regenerated to reflect the bitrate change
		/// </summary>
		/// <param name="job">the job whose video bitrate is to be updated</param>
		/// <param name="bitrate">the new desired video bitrate</param>
		public void updateVideoBitrate(VideoJob job, int bitrate)
		{
			job.Settings.BitrateQuantizer = bitrate;
            job.Commandline = CommandLineGenerator.generateVideoCommandline(job.Settings, job.Input, job.Output, job.DAR);
		}
		#endregion
		#region input properties
		/// <summary>
		/// gets the number of frames and framerate from an avisynth script
		/// </summary>
		/// <param name="nbOfFrames">number of frames of the source</param>
		/// <param name="framerate">framerate of the source</param>
		/// <param name="video">path of the source</param>
		/// <returns>true if the input file could be opened, false if not</returns>
		public bool getInputProperties(out ulong nbOfFrames, out double framerate, string video)
		{
            int d1, d2;
            Dar d;
            return getAllInputProperties(out nbOfFrames, out framerate, out d1, out d2, out d, video);
		}

        public static void GetAllInputProperties(out ulong nbOfFrames, out double framerate, out int hRes, 
			out int vRes, out Dar dar, string video)
		{
            nbOfFrames = 0;
            hRes = vRes = 0;
            framerate = 0.0;
            try
			{
                using (AvsFile avi = AvsFile.OpenScriptFile(video))
                {
                    checked { nbOfFrames = (ulong)avi.Info.FrameCount; }
                    framerate = avi.Info.FPS;
                    hRes = (int)avi.Info.Width;
                    vRes = (int)avi.Info.Height;
                    dar = avi.Info.DAR;
                }
			}
			catch (Exception e)
			{
                throw new JobRunException("The file " + video + " cannot be opened.\r\n"
                     + "Error message for your reference: " + e.Message, e);
            }
			
		}


		/// <summary>
		/// gets the number of frames, framerate, horizontal and vertical resolution from a video source
		/// </summary>
		/// <param name="nbOfFrames">the number of frames</param>
		/// <param name="framerate">the framerate</param>
		/// <param name="hRes">the horizontal resolution</param>
		/// <param name="vRes">the vertical resolution</param>
		/// <param name="video">the video whose properties are to be read</param>
		/// <returns>whether the source could be opened or not</returns>
		public bool getAllInputProperties(out ulong nbOfFrames, out double framerate, out int hRes, 
			out int vRes, out Dar dar, string video)
		{
            try
            {
                GetAllInputProperties(out nbOfFrames, out framerate, out hRes, out vRes, out dar, video);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                        "Cannot open video input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                nbOfFrames = 0;
                hRes = vRes = 0;
                framerate = 0;
                dar = Dar.ITU16x9;
                return false;
            }
		}

        /// <summary>
		/// validates a source against a given AVC level taking into account the rest of the configuration
		/// </summary>
		/// <param name="source">the source to be validated</param>
		/// <param name="level">the level that this source should correspond to</param>
		/// <param name="bframeType">type of b-frames used. 0 = none, 1 = b-frames without pyramid, 
		/// 2 = b-frames with pyramid order</param>
		/// <param name="nbReferences">the number of reference frames used</param>
		/// <param name="compliantLevel">the first avc level that can be used to encode this source</param>
		/// <returns>whether or not the current level is okay, if false and compliantLevel is -1, 
		/// the source could not be read</returns>
		public bool validateAVCLevel(string source, x264Settings settings, out int compliantLevel)
		{
			int hRes, vRes;
            Dar d;
            ulong nbFrames;
			double framerate;
			compliantLevel = -1;
			if (this.getAllInputProperties(out nbFrames, out framerate, out hRes, out vRes, out d, source))
			{
				return this.al.validateAVCLevel(hRes, vRes, framerate, settings, out compliantLevel);
			}
			else
				return false;
		}
		/// <summary>
		/// gets the number of frames of a videostream
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public ulong getNumberOfFrames(string path)
		{
			ulong retval = 0;
			double framerate = 0.0;
			bool succ = getInputProperties(out retval, out framerate, path);
			return retval;
		}
		/// <summary>
		/// gets the framerate of a video stream
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public double getFramerate(string path)
		{
			ulong retval = 0;
			double framerate = 0.0;
			bool succ = getInputProperties(out retval, out framerate, path);
			return framerate;
		}
		#endregion
		#region zones
		/// <summary>
		/// takes a series of non overlapping zones and adds zones with weight 1.0 in between
		/// this is used for xvid which doesn't know zone end frames
		/// </summary>
		/// <param name="zones">a set of zones to be analyzed</param>
		/// <param name="nbOfFrames">number of frames the video source has</param>
		/// <returns>an array of all the zones</returns>
		public Zone[] createHelperZones(Zone[] zones, int nbOfFrames)
		{
			ArrayList newZones = new ArrayList();
			Zone z = zones[0];
			Zone newZone = new Zone();
			newZone.mode = ZONEMODE.WEIGHT;
			newZone.modifier = (decimal)100;
			if (z.startFrame > 0) // zone doesn't start at the beginning, add zone before the first configured zone
			{
				newZone.startFrame = 0;
				newZone.endFrame = z.startFrame - 1;
				newZones.Add(newZone);
			}
			if (zones.Length == 1) // special case
			{
				newZones.Add(z);
				if (z.endFrame < nbOfFrames -1) // we hav to add an end zone
				{
					newZone.startFrame = z.endFrame + 1;
					newZone.endFrame = nbOfFrames - 1;
					newZones.Add(newZone);
				}
			}
			else if (zones.Length == 2)
			{
				newZones.Add(z);
				Zone second = zones[1];
				if (z.endFrame + 1 < second.startFrame) // new zone needs to go in between
				{
					newZone.startFrame = z.endFrame + 1;
					newZone.endFrame = second.startFrame - 1;
					newZones.Add(newZone);
				}
				newZones.Add(second);
				if (second.endFrame < nbOfFrames - 1) // add end zone
				{
					newZone.startFrame = second.endFrame + 1;
					newZone.endFrame = nbOfFrames - 1;
					newZones.Add(newZone);
				}
			}
			else
			{
				for (int i = 0; i <= zones.Length - 2; i++)
				{
					Zone first = zones[i];
					Zone second = zones[i+1];
					if (first.endFrame + 1 == second.startFrame) // zones are adjacent
					{
						newZones.Add(first);
						continue;
					}
					else // zones are not adjacent, create filler zone
					{
						newZone.startFrame = first.endFrame + 1;
						newZone.endFrame = second.startFrame - 1;
						newZones.Add(first);
						newZones.Add(newZone);
					}
				}
				z = zones[zones.Length - 1];
				newZones.Add(z);
				if (z.endFrame != nbOfFrames - 1) // we have to add another zone extending till the end of the video
				{
					newZone.startFrame = z.endFrame + 1;
					newZone.endFrame = nbOfFrames - 1;
					newZones.Add(newZone);
				}
			}
			Zone[] retval = new Zone[newZones.Count];
			int index = 0;
			foreach (object o in newZones)
			{
				z = (Zone)o;
				if (index < 64)
				{
					retval[index] = z;
					index++;
				}
				else
				{
					DialogResult dr = MessageBox.Show("XviD only supports 64 zones. Including filler zones your current\r\nconfiguration yields " + retval.Length + " zones. Do you want to discard the "
						+ "remaining zones?\r\nPress Cancel to reconfigure your zones. Keep in mind that if you have no adjacent zones, a filler zone will have to be added\r\nso 32 non adjacent zones is the "
						+ "maximum number of zones you can have. Both intro and credits region also require a zone.", "Too many zones", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
					if (dr == DialogResult.OK)
						break;
					else // user wants to abort
						return null;
				}
			}
			return retval;
		}
		/// <summary>
		/// compiles the final zone configuration based on intro end frame, credits start frame and the configured zones
		/// </summary>
		/// <param name="vSettings">the video settings containing the list of configured zones</param>
		/// <param name="introEndFrame">the frame where the intro ends</param>
		/// <param name="creditsStartFrame">the frame where the credits begin</param>
		/// <param name="newZones">the zones that are returned</param>
		/// <returns>an array of zones objects in the proper order</returns>
		public bool getFinalZoneConfiguration(VideoCodecSettings vSettings, int introEndFrame, int creditsStartFrame)
		{
			Zone introZone = new Zone();
			Zone creditsZone = new Zone();
			ulong nbOfFrames = getNumberOfFrames(mainForm.Video.Info.VideoInput);
			bool doIntroZone = false, doCreditsZone = false;
			int flushZonesStart = 0, flushZonesEnd = 0;
			if (introEndFrame > 0) // add the intro zone
			{
				introZone.startFrame = 0;
				introZone.endFrame = introEndFrame;
				introZone.mode = ZONEMODE.QUANTIZER;
				introZone.modifier = vSettings.CreditsQuantizer;
				if (vSettings.Zones.Length > 0)
				{
					Zone z = vSettings.Zones[0];
					if (z.startFrame > introZone.endFrame) // the first configured zone starts after the intro zone
						doIntroZone = true;
					else
					{
						flushZonesStart = 1;
						int numberOfConfiguredZones = vSettings.Zones.Length;
						while (flushZonesStart <= numberOfConfiguredZones)// iterate through all zones backwards until we find the first that goes with the intro
						{
							Zone conflict = vSettings.Zones[flushZonesStart];
							if (conflict.startFrame <= introZone.endFrame) // zone starts before the end of the intro -> conflict
								flushZonesStart++;
							else
								break;
						}
						DialogResult dr = MessageBox.Show("Your intro zone overlaps " + flushZonesStart + " zone(s) configured\nin the codec settings.\n"
							+ "Do you want to remove those zones and add the intro zone instead?", "Zone overlap detected", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
						if (dr == DialogResult.Yes)
							doIntroZone = true;
						else if (dr == DialogResult.Cancel) // abort
							return false;
						else // discard the intro zone
							flushZonesStart = 0;
					}
				}
				else
					doIntroZone = true;
			}
			if (creditsStartFrame > 0) // add the credits zone
			{
				creditsZone.startFrame = creditsStartFrame;
				creditsZone.endFrame = (int)nbOfFrames-1;
				creditsZone.mode = ZONEMODE.QUANTIZER;
				creditsZone.modifier = vSettings.CreditsQuantizer;
				if (vSettings.Zones.Length > 0)
				{
					Zone z = vSettings.Zones[vSettings.Zones.Length - 1]; // get the last zone
					if (z.endFrame < creditsZone.startFrame) // the last configured zone ends before the credits start zone
						doCreditsZone = true;
					else
					{
						flushZonesEnd = 1;
						int numberOfConfiguredZones = vSettings.Zones.Length;
						while (numberOfConfiguredZones - flushZonesEnd -1 >= 0)// iterate through all zones backwards until we find the first that goes with the credits
						{
							Zone conflict = vSettings.Zones[numberOfConfiguredZones - flushZonesEnd -1];
							if (conflict.endFrame >= creditsZone.startFrame) // zone ends after the end of the credits -> conflict
								flushZonesEnd++;
							else
								break;
						}
						DialogResult dr = MessageBox.Show("Your credits zone overlaps " + flushZonesEnd + " zone(s) configured\nin the codec settings.\n"
							+ "Do you want to remove those zones and add the credits zone instead?", "Zone overlap detected", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
						if (dr == DialogResult.Yes)
							doCreditsZone = true;
						else if (dr == DialogResult.Cancel) // abort
							return false;
						else // discard the credits zone
							flushZonesEnd = 0;
					}
				}
				else // no additional zones configured
					doCreditsZone = true;
			}
			int newZoneSize = vSettings.Zones.Length - flushZonesStart - flushZonesEnd;
			if (doIntroZone)
				newZoneSize++;
			if (doCreditsZone)
				newZoneSize++;
			Zone[] newZones = new Zone[newZoneSize];
			int index = 0;
			if (doIntroZone)
			{
				newZones[index] = introZone;
				index++;
			}
			for (int i = flushZonesStart; i < vSettings.Zones.Length - flushZonesEnd; i++)
			{
				newZones[index] = vSettings.Zones[i];
				index++;
			}
			if (doCreditsZone)
			{
				newZones[index] = creditsZone;
				index++;
			}
			if (vSettings is xvidSettings && newZones.Length > 0)
			{
				Zone[] xvidZones = createHelperZones(newZones, (int)nbOfFrames);
				if (xvidZones == null)
					return false;
				else
				{
					vSettings.Zones = xvidZones;
					return true;
				}
			}
			vSettings.Zones = newZones;
			return true;
		}
		#endregion
	}
}
