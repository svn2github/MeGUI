using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace MeGUI.core.util
{
    public class BitrateCalculatorPreProcessor
    {
        #region bitrate calculation preprocessing
        public static JobPreProcessor CalculationProcessor = new JobPreProcessor(calculateBitrate, "CalculationProcessor");
        private static void AddJob(Job currentJob, List<VideoJob> allVideoJobs, List<AudioJob> allAudioJobs, StringBuilder logBuilder)
        {
            if (currentJob is VideoJob)
            {
                logBuilder.AppendFormat("Found another video job: {0}.{1}", currentJob.Name,
                    Environment.NewLine);
                allVideoJobs.Add((VideoJob)currentJob);
            }
            else if (currentJob is AudioJob)
            {
                logBuilder.AppendFormat("Found audio stream: {0}.{1}", currentJob.Name,
                    Environment.NewLine);
                allAudioJobs.Add((AudioJob)currentJob);
            }
        }
        /// <summary>
        /// postprocesses an audio job followed by a video job
        /// this constellation happens in automated or one click encoding where we have an audio job linked
        /// to a video job
        /// first, any audio jobs previous to the audio job in question will be located
        /// then we get the size of all audio tracks
        /// from the desired final output size stored in the first video job, we calculate the video bitrate
        /// we have to use to obtain the final file size, taking container overhead into account
        /// the calculated bitrate is then applied to all video jobs
        /// </summary>
        /// <param name="firstAudio">the audio job that is linked to a video job</param>
        /// <param name="firstpass">the video job to which the audio job is linked</param>
        public static void calculateBitrate(MainForm mainForm, Job ajob)
        {
            if (!(ajob is VideoJob)) return;
            VideoJob job = (VideoJob)ajob;
            if (job.BitrateCalculationInfo == null) return;

            BitrateCalculationInfo b = job.BitrateCalculationInfo;
            mainForm.addToLog("AAAA");
            StringBuilder logBuilder = new StringBuilder();
            logBuilder.AppendFormat("Job '{0}' requires bitrate calculation. Calculating now...\r\n", job.Name);
            //            mainForm.addToLog("We have an audio job followed by a video job\r\n");
            //            logBuilder.AppendFormat("The audio job is named {0}. The first video job is named {1}.", firstAudio.Name, firstpass.Name);
            List<VideoJob> allVideoJobs = new List<VideoJob>();
            List<AudioJob> allAudioJobs = new List<AudioJob>();
            foreach (Job j in b.VideoJobs)
                allVideoJobs.Add((VideoJob)j);
            foreach (Job j in b.AudioJobs)
                allAudioJobs.Add((AudioJob)j);

            List<AudioStream> audioStreams = new List<AudioStream>();
            mainForm.addToLog("BBBB");

            foreach (SubStream stream in b.MuxOnlyStreams)
            {
                FileInfo fi = new FileInfo(stream.path);
                AudioStream newStream = new AudioStream();
                newStream.SizeBytes = fi.Length;
                newStream.Type = VideoUtil.guessAudioType(stream.path);
                newStream.BitrateMode = BitrateManagementMode.VBR;
                audioStreams.Add(newStream);
                logBuilder.Append("Encoded audio file is present: " + stream.path +
                    " It has a size of " + fi.Length + " bytes. \r\n");
            }

            mainForm.addToLog("CCCC");
            //            mainForm.addToLog("The audio job is named " + firstAudio.Name + " the first pass " + firstpass.Name + ".\r\n");
            logBuilder.AppendFormat("The video job has a desired final output size of {0} bytes and video bitrate of {1}kbit/s\r\n",
                b.DesiredSizeBytes, job.Settings.BitrateQuantizer);
            logBuilder.AppendFormat("Examining audio jobs found...\r\n");
            foreach (AudioJob aJob in allAudioJobs)
            {
                logBuilder.AppendFormat("Audio job '{0}':{1}", aJob.Name, Environment.NewLine);
                if (aJob.Status == JobStatus.DONE)
                    logBuilder.AppendFormat("    This job completed successfully, taking size into account...");
                else
                {
                    logBuilder.AppendFormat("    This job didn't complete successfully, ignoring...{0}", Environment.NewLine);
                    continue;
                }

                try
                {
                    FileInfo fi = new FileInfo(aJob.Output);
                    long audioSize = fi.Length;
                    AudioType audioType = VideoUtil.guessAudioType(aJob.Output);
                    if (audioType == null)
                    {
                        logBuilder.AppendFormat("Unable to determine audio type of file, ignoring.\r\n");
                        continue;
                    }
                    AudioStream audioStream = new AudioStream();
                    audioStream.Type = audioType;
                    audioStream.SizeBytes = audioSize;
                    audioStreams.Add(audioStream);
                    logBuilder.AppendFormat("The size is of this track is {0} bytes, and the type is {1}. Taking this into account in the bitrate calculation.{2}",
                        audioSize, audioType, Environment.NewLine);

                }
                catch (IOException e)
                {
                    logBuilder.AppendFormat("IOException when trying to get size of audio. Error message: {0}. Ignoring audio file.{1}",
                        e.Message, Environment.NewLine);
                    continue;
                }
            }

            mainForm.addToLog("DDDD");
            MuxJob mux = (MuxJob)b.MuxJob;

            int bitrateKBits = 0;
            VideoCodec vCodec = job.Settings.Codec;
            long videoSizeKB = 0;
            bitrateKBits = mainForm.BitrateCalculator.CalculateBitrateKBits(vCodec, job.Settings.NbBframes > 0, mux.ContainerType,
                audioStreams.ToArray(), b.DesiredSizeBytes, job.NumberOfFrames,
                job.Framerate, out videoSizeKB);

            logBuilder.AppendFormat("Desired video size after substracting audio size is " + videoSizeKB + "KBs. ");
            logBuilder.AppendFormat("Setting the desired bitrate of the subsequent video jobs to " + bitrateKBits + " kbit/s.\r\n");
            foreach (VideoJob vjob in allVideoJobs)
            {
                mainForm.JobUtil.updateVideoBitrate(vjob, bitrateKBits);
                vjob.BitrateCalculationInfo = null;
            }
            mainForm.addToLog(logBuilder.ToString());
            mainForm.addToLog("EEEE");
        }
        #endregion
    }

    public class BitrateCalculationInfo
    {
        public List<string> _AudioJobNames
        {
            get { return MainForm.Instance.Jobs.toStringList(AudioJobs); }
            set { AudioJobs = MainForm.Instance.Jobs.toJobList(value); }
        }

        public List<string> _VideoJobNames
        {
            get { return MainForm.Instance.Jobs.toStringList(VideoJobs); }
            set { VideoJobs = MainForm.Instance.Jobs.toJobList(value); }
        }

        public string _MuxJobName
        {
            get { return MainForm.Instance.Jobs.toStringList(new Job[] { MuxJob })[0]; }
            set { MuxJob = MainForm.Instance.Jobs.toJobList(new string[] { value })[0]; }
        }

        public long DesiredSizeBytes;

        
        public List<SubStream> MuxOnlyStreams;

        [XmlIgnore]
        public List<Job> AudioJobs;
        
        [XmlIgnore]
        public List<Job> VideoJobs;

        [XmlIgnore]
        public Job MuxJob;
    }
}
