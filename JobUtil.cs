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
        bool finished = false;
        DeinterlaceFilter[] filters;
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
					MessageBox.Show("Profile " + name + " could not be loaded. Is it a valid Job created by MeGUI?", "Error loading Job", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            int nbFrames = 0;
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
        
        public VideoJob generateVideoJob(string input, string output, VideoCodecSettings settings, int parX, int parY)
        {
            return generateVideoJob(input, output, settings, false, parX, parY);
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
		public VideoJob generateVideoJob(string input, string output, VideoCodecSettings settings, bool skipVideoCheck, int parX, int parY)
		{
			VideoJob job = new VideoJob();
			job.Input = input;
			job.Output = output;
            job.DARX = parX;
            job.DARY = parY;
			if (mainForm.Settings.AutoSetNbThreads)
				adjustNbThreads(settings);
			if (MainForm.GetDirectoryName(settings.Logfile).Equals("")) // no path set
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
			int nbOfFrames = 0;
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
			job.Priority = mainForm.Settings.DefaultPriority;
            job.Commandline = CommandLineGenerator.generateVideoCommandline(job.Settings, input, output, parX, parY);
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
                settings.NbThreads = nbCPUs;
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
			job.Settings = stream.settings;
			job.Priority = mainForm.Settings.DefaultPriority;
            //job.Commandline = CommandLineGenerator.generateAudioCommandline(mainForm.Settings, job.Settings, job.Input, job.Output);  // no longer necessary for the avisynth encoder
			return job;
		}

        public MuxJob[] GenerateMuxJobs(VideoStream video, SubStream[] audioStreamsArray, MuxableType[] audioTypes,
            SubStream[] subtitleStreamsArray, MuxableType[] subTypes,
            string chapterFile, MuxableType chapterInputType, ContainerType container, string output, int splitSize)
        {
            MuxProvider prov = new MuxProvider();
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
                mjob.Priority = this.mainForm.Settings.DefaultPriority;

                string tempOutputName = Path.Combine(Path.GetDirectoryName(output),
                    Path.GetFileNameWithoutExtension(output) + tempNumber + ".");
                tempNumber++;
                foreach (MuxableType o in mpl.handledInputTypes)
                {
                    if (o.outputType is VideoType)
                    {
                        mjob.Settings.VideoInput = video.Output;
                        mjob.Settings.PARX = video.ParX;
                        mjob.Settings.PARY = video.ParX;
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
                    mjob.Settings.PARX = video.ParX;
                    mjob.Settings.PARY = video.ParY;
                    mjob.ContainerType = container;
                    mjob.FilesToDelete.AddRange(muxedFilesToDelete);
                }
                else
                {
                    ContainerFileType cot = mpl.muxerInterface.GetContainersInCommon(muxPath[index + 1].muxerInterface)[0];
                    mjob.Settings.MuxedOutput = tempOutputName + cot.Extension;
                    mjob.ContainerType = cot.ContainerType;
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
                mjob.Commandline = CommandLineGenerator.generateMuxCommandline(mjob.Settings, mjob.MuxType);
            }
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
        public bool AddAudioJob(string audioInput, string audioOutput, AudioCodecSettings settings)
        {
            AudioStream stream = new AudioStream();
            stream.path = audioInput;
            stream.output = audioOutput;
            stream.settings = settings;
            AudioJob job = this.generateAudioJob(stream);
            if (job != null)
            {
                int freeJobNumber = this.mainForm.getFreeJobNumber();
                job.Name = "job" + freeJobNumber;
                this.mainForm.addJobToQueue(job);
                return true;
            }
            return false;
        }
        public bool AddVideoJobs(string movieInput, string movieOutput, VideoCodecSettings settings,
            int introEndFrame, int creditsStartFrame, int parX, int parY, bool prerender, bool checkVideo)
        {
            bool cont = getFinalZoneConfiguration(settings, introEndFrame, creditsStartFrame);
            if (!cont) // abort
                return false;
            VideoJob[] jobs = prepareVideoJob(movieInput, movieOutput, settings, parX, parY, prerender, checkVideo);
            if (jobs == null)
                return false;
            int freeJobNumber = this.mainForm.getFreeJobNumber();
            if (jobs.Length > 0)
            {
                if (jobs.Length > 1) // complex naming
                {
                    string prevName = "";
                    int number = 1;
                    foreach (VideoJob job in jobs)
                    {
                        job.Name = "job" + freeJobNumber + "-" + number;
                        if (!prevName.Equals(""))
                            job.Previous = prevName;
                        if (jobs.Length > number)
                        {
                            int n = number + 1;
                            job.Next = "job" + freeJobNumber + "-" + n;
                        }
                        number++;
                        prevName = job.Name;
                    }
                    foreach (VideoJob job in jobs)
                    {
                        this.mainForm.addJobToQueue(job);
                    }
                }
                else // simple naming
                {
                    jobs[0].Name = "job" + freeJobNumber;
                    this.mainForm.addJobToQueue(jobs[0]);
                }
                return true;
            }
            return false;
        }
        public VideoJob[] prepareVideoJob(string movieInput, string movieOutput, VideoCodecSettings settings, int parX, int parY)
        {
            return prepareVideoJob(movieInput, movieOutput, settings, parX, parY, false, false);
        }
		/// <summary>
		/// at first, the job from the currently configured settings is generated. In addition, we find out if this job is 
		/// a part of an automated series of jobs. If so, it means the first generated job was the second pass, and we have
		/// to create the first pass using the same settings
		/// then, all the generated jobs are returned
		/// </summary>
		/// <returns>an Array of VideoJobs in the order they are to be encoded</returns>
		public VideoJob[] prepareVideoJob(string movieInput, string movieOutput, VideoCodecSettings settings, int parX, int parY, bool prerender, bool checkVideo)
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
                hfyuFile = MainForm.GetDirectoryName(movieInput) + "\\hfyu_" + 
                    Path.GetFileNameWithoutExtension(movieInput) + ".avi";
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
                prerenderJob = this.generateVideoJob(movieInput, hfyuFile, new hfyuSettings(), parX, parY);
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
            VideoJob job = this.generateVideoJob(inputAVS, movieOutput, settings, prerender, parX, parY);
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
						firstpass.Input, firstpass.Output, parX, parY);
					if (threePasses)
					{
						firstpass.Settings.EncodingMode = 5; // change to 3 pass 3rd pass just for show
						middlepass = cloneJob(job);
						middlepass.Settings.EncodingMode = 6; // 3 pass 2nd pass
						middlepass.Settings.Turbo = false;
                        if (mainForm.Settings.Keep2of3passOutput) // give the 2nd pass a new name
                        {
                            middlepass.Output = MainForm.GetDirectoryName(job.Output) + @"\" + Path.GetFileNameWithoutExtension(job.Output)
                                + "-2ndpass" + Path.GetExtension(job.Output);
                            job.FilesToDelete.Add(middlepass.Output);
                        }
                        middlepass.Commandline = CommandLineGenerator.generateVideoCommandline(middlepass.Settings, 
							middlepass.Input, middlepass.Output, parX, parY);
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
			job.OutputType = oldJob.OutputType;
			job.Output = oldJob.Output;
			job.Priority = oldJob.Priority;
			job.Settings = oldJob.Settings.clone();
			return job;
		}
		#endregion
		#endregion
		#region job postprocessing
		/// <summary>
		/// postprocesses an indexing job
		/// if the job was created from the d2v creator and the load operation was activated
		/// all audio files will be located, loaded into the audio section of the GUI, and the
		/// d2v script will be opened in the avisynth script creator window
		/// if the job was created from the oneclick window, an avisynth project will automatically 
		/// be created from the job using the desired resizing settings, including automatic cropping
		/// then, an audio job with the given audio profile will be created for every audio track
		/// demuxed, then video jobs will be created using the avisynth script as input
		/// and finally the appropriate muxing job will be created and everything will be queued
		/// </summary>
		/// <param name="job">the indexjob to be postprocessed</param>
		public void postprocessIndexJob(IndexJob job)
		{
			StringBuilder logBuilder = new StringBuilder();
			VideoUtil vUtil = new VideoUtil(this.mainForm);
			Dictionary<int, string> audioFiles = vUtil.getAllDemuxedAudio(job.Output, 8);
			if (job.PostprocessingProperties == null) // d2v creator job
			{
                if (job.LoadSources)
				{
					if (job.DemuxMode != 0)
					{
						int counter = 0;
						foreach (int i in audioFiles.Keys)
						{
                            mainForm.setAudioTrack(counter, audioFiles[i]);
                            if (counter >= 2)
                                break;
                            counter++;
						}
					}
                    AviSynthWindow asw = new AviSynthWindow(this.mainForm, job.Output);
					asw.OpenScript += new OpenScriptCallback(mainForm.openVideoFile);
					asw.Show();
				}
			}
			else if (job.PostprocessingProperties != null) // full postprocessing mode
			{
                List<AudioStream> encodableAudioStreams = new List<AudioStream>();
                List<SubStream> muxOnlyAudioStreams = new List<SubStream>();
                int counter = 0; // The counter is only used to find the track number in case of an error
				foreach (OneClickWindow.PartialAudioStream propertiesStream in job.PostprocessingProperties.AudioStreams)
				{
                    counter++; // The track number starts at 1, so we increment right here. This also ensures it will always be incremented
                    
                    bool error = false;
                    string input = null, output = null, language = null;
                    int delay = 0;
                    AudioCodecSettings settings = null;
                    // Input
                    if (string.IsNullOrEmpty(propertiesStream.input))
                        continue; // Here we have an unconfigured stream. Let's just go on to the next one

                    if (propertiesStream.useExternalInput)
                        input = propertiesStream.input;
                    else if (audioFiles.ContainsKey(propertiesStream.trackNumber))
                        input = audioFiles[propertiesStream.trackNumber];
                    else
                        error = true;
                    delay = MainForm.getDelay(input);
                    // Settings
                    if (propertiesStream.dontEncode)
                    {
                        settings = null;
                    }
                    else if (propertiesStream.settings != null)
                        settings = propertiesStream.settings;
                    else
                        error = true;

                    // Output
                    if (propertiesStream.dontEncode)
                        output = input;
                    else
                        output = Path.Combine(
                            Path.GetDirectoryName(input),
                            Path.GetFileNameWithoutExtension(input) + "_" + 
                            propertiesStream.trackNumber + ".file");

                    // Language
                    if (!string.IsNullOrEmpty(propertiesStream.language))
                        language = propertiesStream.language;
                    else
                        language = "";

                    if (error)
                    {
                        logBuilder.AppendFormat("Trouble with audio track {0}. Skipping track...{1}", counter, Environment.NewLine);
                        output = null;
                        input = null;
                        input = null;
                    }
                    else
                    {
                        if (propertiesStream.dontEncode)
                        {
                            SubStream newStream = new SubStream();
                            newStream.path = input;
                            newStream.name = "";
                            newStream.language = language;
                            newStream.delay = delay;
                            muxOnlyAudioStreams.Add(newStream);
                        }
                        else
                        {
                            AudioStream encodeStream = new AudioStream();
                            encodeStream.path = input;
                            encodeStream.output = output;
                            encodeStream.settings = settings;
                            if (delay != 0)
                            {
                                settings.DelayEnabled = true;
                                settings.Delay = delay;
                            }
                            encodableAudioStreams.Add(encodeStream);
                        }
                    }
				}

				logBuilder.Append("Desired size of this automated encoding series: " + job.PostprocessingProperties.OutputSize 
					+ " bytes, split size: " + job.PostprocessingProperties.SplitSize + "\r\n");
				VideoCodecSettings videoSettings = job.PostprocessingProperties.VideoSettings;

				string videoOutput = Path.Combine(MainForm.GetDirectoryName(job.Output),
                    Path.GetFileNameWithoutExtension(job.Output) + "_Video");
				string muxedOutput = job.PostprocessingProperties.FinalOutput;

                /*
                SubStream[] audio = new SubStream[audioStreams.Length];
				int j = 0;
				//Configure audio muxing inputs
				foreach (AudioStream stream in audioStreams)
				{
					audio[j].language = "";
                    audio[j].name = "";
					if (type == MuxerType.MP4BOX || type == MuxerType.MKVMERGE)
					{
                        if (Path.GetExtension(stream.output).ToLower().Equals(".mp4"))
                            audio[j].path = stream.output;
						if (stream.settings == null)
							audio[j].path = stream.path;
						logBuilder.Append("Language of track " + (j+1) + " is " + job.PostprocessingProperties.AudioLanguages[j]);
						logBuilder.Append(". The ISO code that this corresponds to is ");
                        string lang = null;
                        try
                        {
                            lang = (string)LanguageSelectionContainer.Languages[job.PostprocessingProperties.AudioLanguages[j]];
                        }
                        catch (KeyNotFoundException)
                        { }
						
                        if (lang != null)
						{
							audio[j].language = lang;
							logBuilder.Append(lang + ".\r\n");
						}
						else
						{
							logBuilder.Append("unknown.\r\n");
						}
					}
					else if (type == MuxerType.AVIMUXGUI)
					{
                        if (Path.GetExtension(stream.output).ToLower().Equals(".mp3"))
                        {
							audio[j].path = stream.output;
							break; // jump out of loop, only one audio track for AVI
						}
					}
					j++;
				}
				if ((audioStreams.Length == 1 && audioStreams[0].settings == null) ||
					(audioStreams.Length == 2 && audioStreams[0].settings == null && audioStreams[1].settings == null))
					audioStreams = new AudioStream[0];*/
			
				//Open the video
				int parX, parY;
				string videoInput = openVideo(job.Output, (int)job.PostprocessingProperties.AR, job.PostprocessingProperties.CustomAR,
                    job.PostprocessingProperties.HorizontalOutputResolution, job.PostprocessingProperties.SignalAR, logBuilder, 
					job.PostprocessingProperties.AvsSettings, job.PostprocessingProperties.AutoDeinterlace, videoSettings, out parX, out parY);

                VideoStream myVideo = new VideoStream();
                int length;
                double framerate;
                getInputProperties(out length, out framerate, videoInput);
                myVideo.Input = videoInput;
                myVideo.Output = videoOutput;
                myVideo.NumberOfFrames = length;
                myVideo.Framerate = framerate;
                myVideo.ParX = parX;
                myVideo.ParY = parY;
                myVideo.VideoType = new MuxableType((new VideoEncoderProvider().GetSupportedOutput(videoSettings.EncoderType))[0], videoSettings.Codec);
                myVideo.Settings = videoSettings;
                List<string> intermediateFiles = new List<string>();
                intermediateFiles.Add(videoInput);
                intermediateFiles.Add(job.Output);
                intermediateFiles.AddRange(audioFiles.Values);
                if (!videoInput.Equals(""))
				{
					//Create empty subtitles for muxing (subtitles not supported in one click mode)
					SubStream[] subtitles = new SubStream[0];
                    vUtil.GenerateJobSeries(myVideo, muxedOutput, encodableAudioStreams.ToArray(), subtitles,
                        job.PostprocessingProperties.ChapterFile, job.PostprocessingProperties.OutputSize,
                        job.PostprocessingProperties.SplitSize, job.PostprocessingProperties.Container.ContainerType,
                        false, muxOnlyAudioStreams.ToArray(), intermediateFiles);
/*                    vUtil.generateJobSeries(videoInput, videoOutput, muxedOutput, videoSettings,
                        audioStreams, audio, subtitles, job.PostprocessingProperties.ChapterFile,
                        job.PostprocessingProperties.OutputSize, job.PostprocessingProperties.SplitSize,
                        containerOverhead, type, new string[] { job.Output, videoInput });*/
				}
				mainForm.addToLog(logBuilder.ToString());
			}
		}
		/// <summary>
		/// opens a dgindex script
		/// if the file can be properly opened, auto-cropping is performed, then depending on the AR settings
		/// the proper resolution for automatic resizing, taking into account the derived cropping values
		/// is calculated, and finally the avisynth script is written and its name returned
		/// </summary>
		/// <param name="path">dgindex script</param>
		/// <param name="aspectRatio">aspect ratio selection to be used</param>
		/// <param name="customDAR">custom display aspect ratio for this source</param>
		/// <param name="horizontalResolution">desired horizontal resolution of the output</param>
		/// <param name="logBuilder">stringbuilder where to append log messages</param>
		/// <param name="settings">the codec settings (used only for x264)</param>
		/// <param name="sarX">pixel aspect ratio X</param>
		/// <param name="sarY">pixel aspect ratio Y</param>
		/// <param name="height">the final height of the video</param>
		/// <param name="signalAR">whether or not ar signalling is to be used for the output 
		/// (depending on this parameter, resizing changes to match the source AR)</param>
		/// <returns>the name of the AviSynth script created, empty of there was an error</returns>
		private string openVideo(string path, int aspectRatio, double customDAR, int horizontalResolution,
            bool signalAR, StringBuilder logBuilder, AviSynthSettings avsSettings, bool autoDeint, 
            VideoCodecSettings settings, out int sarX, out int sarY)
		{
			sarX = sarY = -1;
			VideoReader reader = (VideoReader)new d2vReader(path, true);
			if (reader.FrameCount < 1)
			{
				logBuilder.Append("DGDecode reported 0 frames in this file.\r\nThis is a fatal error.\r\n\r\nPlease recreate the DGIndex project");
				return "";
			}
			
			//Autocrop
			CropValues final = VideoUtil.autocrop(reader);
			bool error = (final.left == -1);
			if (!error)
			{
				logBuilder.Append("Autocropping successful. Using the following crop values: left: " + final.left +
					", top: " + final.top + ", right: " + final.right + ", bottom: " + final.bottom + ".\r\n");
			}
			else
			{
				logBuilder.Append("Autocropping did not find 3 frames that have matching crop values\r\n"
					+ "Autocrop failed, aborting now");
				return "";
			}

            customDAR = VideoUtil.getAspectRatio((AspectRatio)aspectRatio);

			//Check if AR needs to be autodetected now
			if (aspectRatio == 4) // it does
			{
				logBuilder.Append("Aspect Ratio set to auto-detect later, detecting now. ");
                customDAR = (double)reader.DARX / (double)reader.DARY;
                if (customDAR > 0)
                    logBuilder.AppendFormat("Found aspect ratio of {0}.{1}", customDAR, Environment.NewLine);
                else
                {
                    customDAR = VideoUtil.getAspectRatio(AspectRatio.ITU16x9);
                    logBuilder.AppendFormat("No aspect ratio found, defaulting to {0}.{1}", customDAR, Environment.NewLine);
                }
			}

			//Suggest a resolution (taken from AvisynthWindow.suggestResolution_CheckedChanged)
			int scriptVerticalResolution = VideoUtil.suggestResolution(reader.Height, reader.Width, customDAR, 
				final, horizontalResolution, signalAR, mainForm.Settings.AcceptableAspectErrorPercent, out sarX, out sarY);
            if (settings != null && settings is x264Settings) // verify that the video corresponds to the chosen avc level, if not, change the resolution until it does fit
			{
				x264Settings xs = (x264Settings)settings;
				if (xs.Level != 15)
				{
					int compliantLevel = 15;
                    while (!this.al.validateAVCLevel(horizontalResolution, scriptVerticalResolution, reader.Framerate, xs, out compliantLevel))
					{ // resolution not profile compliant, reduce horizontal resolution by 16, get the new vertical resolution and try again
						AVCLevels al = new AVCLevels();
						string levelName = al.getLevels()[xs.Level];
						logBuilder.Append("Your chosen AVC level " + levelName + " is too strict to allow your chosen resolution of " +
							horizontalResolution + "*" + scriptVerticalResolution + ". Reducing horizontal resolution by 16.\r\n");
						horizontalResolution -= 16;
						scriptVerticalResolution = VideoUtil.suggestResolution(reader.Height, reader.Width, customDAR, 
							final, horizontalResolution, signalAR, mainForm.Settings.AcceptableAspectErrorPercent, out sarX, out sarY);
					}
					logBuilder.Append("Final resolution that is compatible with the chosen AVC Level: " + horizontalResolution + "*" 
						+ scriptVerticalResolution + "\r\n");
				}
			}

            //Generate the avs script based on the template
            string inputLine = "#input";
            string deinterlaceLines = "#deinterlace";
            string denoiseLines = "#denoise";
            string cropLine = "#crop";
            string resizeLine = "#resize";

            inputLine = ScriptServer.GetInputLine(path, PossibleSources.d2v,
                avsSettings.ColourCorrect, avsSettings.MPEG2Deblock, false, 0);

            if (autoDeint)
            {
                logBuilder.AppendLine("Automatic deinterlacing was checked. Running now...");
                string d2v = path;
                SourceDetector sd = new SourceDetector(inputLine, d2v, false,
                    mainForm.Settings.SourceDetectorSettings,
                    new UpdateSourceDetectionStatus(analyseUpdate),
                    new FinishedAnalysis(finishedAnalysis));
                finished = false;
                sd.analyse();
                waitTillAnalyseFinished();
                deinterlaceLines = filters[0].Script;
                logBuilder.AppendLine("Deinterlacing used: " + deinterlaceLines);
            }
            
            cropLine = ScriptServer.GetCropLine(true, final);
            denoiseLines = ScriptServer.GetDenoiseLines(avsSettings.Denoise, (DenoiseFilterType)avsSettings.DenoiseMethod);
            resizeLine = ScriptServer.GetResizeLine(true, horizontalResolution, scriptVerticalResolution, (ResizeFilterType)avsSettings.ResizeMethod);

            string newScript = ScriptServer.CreateScriptFromTemplate(avsSettings.Template, inputLine, cropLine, resizeLine, denoiseLines, deinterlaceLines);
            if (sarX != -1 && sarY != -1)
                newScript = string.Format("global MeGUI_darx = {0}\r\nglobal MeGUI_dary = {1}\r\n{2}", sarX, sarY, newScript);
			logBuilder.Append("Avisynth script created:\r\n");
			logBuilder.Append(newScript);
			try
			{
				StreamWriter sw = new StreamWriter(Path.ChangeExtension(path, ".avs"));
				sw.Write(newScript);
				sw.Close();
			}
			catch (IOException i)
			{
				logBuilder.Append("An error ocurred when trying to save the AviSynth script:\r\n" + i.Message);
				return "";
			}
			return Path.ChangeExtension(path, ".avs");
		}


        public void finishedAnalysis(string text, DeinterlaceFilter[] filters, bool error, string errorMessage)
        {
            if (error)
            {
                MessageBox.Show(errorMessage, "Source detection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                filters = new DeinterlaceFilter[] {
                    new DeinterlaceFilter("Error", "#An error occurred in source detection. Doing no processing")};
            }
            else
                this.filters = filters;
            finished = true;
        }

        public void analyseUpdate(int amountDone, int total)
        { /*Do nothing*/ }

        private void waitTillAnalyseFinished()
        {
            while (!finished)
            {
                Thread.Sleep(500);
            }
        }
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
            job.Commandline = CommandLineGenerator.generateVideoCommandline(job.Settings, job.Input, job.Output, job.DARX, job.DARY);
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
		public bool getInputProperties(out int nbOfFrames, out double framerate, string video)
		{
            int d1, d2, d3, d4;
            return getAllInputProperties(out nbOfFrames, out framerate, out d1, out d2, out d3, out d4, video);
		}

        public static string GetAllInputProperties(out int nbOfFrames, out double framerate, out int hRes, 
			out int vRes, out int darX, out int darY, string video)
		{
            nbOfFrames = hRes = vRes = darX = darY = 0;
            framerate = 0.0;
            try
			{
                AvsReader avi = AvsReader.OpenScriptFile(video);
				nbOfFrames = avi.FrameCount;
				framerate = avi.Framerate;
				hRes = avi.Width;
				vRes = avi.Height;
                darX = avi.DARX;
                darY = avi.DARY;
				avi.Close();
				return null;
			}
			catch (Exception e)
			{
                return "The file " + video + " cannot be opened.\r\n"
                    + "Error message for your reference: " + e.Message;
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
		public bool getAllInputProperties(out int nbOfFrames, out double framerate, out int hRes, 
			out int vRes, out int darX, out int darY, string video)
		{
            string err  = GetAllInputProperties(out nbOfFrames, out framerate, out hRes, out vRes, out darX, out darY, video);
            if (err == null)
                return true;
            MessageBox.Show( err,
                    "Cannot open video input", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return false;

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
			int nbFrames, hRes, vRes, d1, d2;
			double framerate;
			compliantLevel = -1;
			if (this.getAllInputProperties(out nbFrames, out framerate, out hRes, out vRes, out d1, out d2, source))
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
		public int getNumberOfFrames(string path)
		{
			int retval = 0;
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
			int retval = 0;
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
			int nbOfFrames = getNumberOfFrames(mainForm.VideoIO[0]);
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
				creditsZone.endFrame = nbOfFrames-1;
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
				Zone[] xvidZones = createHelperZones(newZones, nbOfFrames);
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