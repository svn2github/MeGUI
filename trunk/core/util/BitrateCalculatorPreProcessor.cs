using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using MeGUI.core.details;

namespace MeGUI.core.util
{
    public class BitrateCalculatorPreProcessor
    {
        #region bitrate calculation preprocessing
        public static JobPreProcessor CalculationProcessor = new JobPreProcessor(calculateBitrate, "CalculationProcessor");
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
            mainForm.addToLog("Doing bitrate calculation...");

            List<AudioBitrateCalculationStream> audioStreams = new List<AudioBitrateCalculationStream>();
            foreach (string s in b.AudioFiles)
                audioStreams.Add(new AudioBitrateCalculationStream(s));

            double framerate;
            ulong framecount;
            JobUtil.getInputProperties(out framecount, out framerate, job.Input);

            int bitrateKBits;
            ulong videoSizeKB = 0;

            bitrateKBits = BitrateCalculator.CalculateBitrateKBits(job.Settings.Codec, job.Settings.NbBframes > 0, b.Container,
                audioStreams.ToArray(), b.DesiredSize.Bytes, framecount, framerate, out videoSizeKB);

            mainForm.addToLog("Desired video size after subtracting audio size is {0}KBs. Setting the desired bitrate of the subsequent video jobs to {1} kbit/s.",
                videoSizeKB, bitrateKBits);

            foreach (TaggedJob t in b.VideoJobs)
                ((VideoJob)t.Job).Settings.BitrateQuantizer = bitrateKBits;
        }
        #endregion
    }

    public class BitrateCalculationInfo
    {
        public List<string> _VideoJobNames
        {
            get { return MainForm.Instance.Jobs.toStringList(VideoJobs); }
            set { VideoJobs = MainForm.Instance.Jobs.toJobList(value); }
        }

        public FileSize DesiredSize;

        public List<string> AudioFiles;

        [XmlIgnore]
        internal List<TaggedJob> VideoJobs;

        [XmlIgnore]
        public ContainerType Container;

        public string _ContainerName
        {
            get { return Container.ID; }
            set { Container = ContainerType.ByName(value); }
        }
    }
}
