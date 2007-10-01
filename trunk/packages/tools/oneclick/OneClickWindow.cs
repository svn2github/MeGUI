using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms;



using MeGUI.core.details.video;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.gui;
using MeGUI.packages.tools.oneclick;
using MeGUI.core.util;
using MeGUI.core.details;

namespace MeGUI
{

//    public class OneClickPostProcessor 
    public partial class OneClickWindow : Form
    {
        FileSCBox[] audioTrack;
        AudioConfigControl[] audio;

        #region profiles
        void ProfileChanged(object sender, Profile prof)
        {
            updatePossibleContainers();
        }

        #region OneClick profiles
        ISettingsProvider<OneClickSettings, Empty, int, int> oneClickSettingsProvider = new SettingsProviderImpl2<
    MeGUI.packages.tools.oneclick.OneClickConfigPanel, Empty, OneClickSettings, OneClickSettings, int, int>("OneClick", 0, 0);
        private void initOneClickHandler()
        {
            // Init oneclick handlers
            ProfilesControlHandler<OneClickSettings, Empty> profileHandler = new ProfilesControlHandler<OneClickSettings, Empty>(
    "OneClick", mainForm, profileControl2, oneClickSettingsProvider.EditSettings, Empty.Getter,
    new SettingsGetter<OneClickSettings>(oneClickSettingsProvider.GetCurrentSettings), new SettingsSetter<OneClickSettings>(oneClickSettingsProvider.LoadSettings));
            SingleConfigurerHandler<OneClickSettings, Empty, int, int> configurerHandler = new SingleConfigurerHandler<OneClickSettings, Empty, int, int>(profileHandler, oneClickSettingsProvider);
            profileHandler.ProfileChanged += new SelectedProfileChangedEvent(OneClickProfileChanged);
            profileHandler.ConfigureCompleted += new EventHandler(profileHandler_ConfigureCompleted);
            profileHandler.RefreshProfiles();
            audioTrack = new FileSCBox[] { audioTrack1, audioTrack2 };
            audio = new AudioConfigControl[] { audio1, audio2 };
        }

        private void refreshAssistingProfiles()
        {
            videoProfileHandler.RefreshProfiles();
            audio1.RefreshProfiles();
            audio2.RefreshProfiles();
            avsProfileHandler.RefreshProfiles();
        }

        void profileHandler_ConfigureCompleted(object sender, EventArgs e)
        {
            refreshAssistingProfiles();
        }

        void OneClickProfileChanged(object sender, Profile prof)
        {
            if (prof != null)
            {
                this.Settings = ((OneClickSettings)prof.BaseSettings);
            }
        } 
        #endregion
        #region AVS profiles
        ISettingsProvider<AviSynthSettings, MeGUI.core.details.video.Empty, int, int> avsSettingsProvider = new SettingsProviderImpl2<
    MeGUI.core.gui.AviSynthProfileConfigPanel, MeGUI.core.details.video.Empty, AviSynthSettings, AviSynthSettings, int, int>("AviSynth", 0, 0);
        ProfilesControlHandler<AviSynthSettings, Empty> avsProfileHandler; 
        private void initAvsHandler()
        {
            // Init AVS handlers
            avsProfileHandler = new ProfilesControlHandler<AviSynthSettings, Empty>(
    "AviSynth", mainForm, profileControl1, avsSettingsProvider.EditSettings, Empty.Getter,
    new SettingsGetter<AviSynthSettings>(avsSettingsProvider.GetCurrentSettings), new SettingsSetter<AviSynthSettings>(avsSettingsProvider.LoadSettings));
            SingleConfigurerHandler<AviSynthSettings, Empty, int, int> configurerHandler = new SingleConfigurerHandler<AviSynthSettings, Empty, int, int>(avsProfileHandler, avsSettingsProvider);
        }
        #endregion
        #region Video profiles
        private VideoCodecSettings VideoSettings
        {
            get { return VideoSettingsProvider.GetCurrentSettings(); }
        }
        private ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> VideoSettingsProvider
        {
            get { return videoCodecHandler.CurrentSettingsProvider; }
        }
        MultipleConfigurersHandler<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> videoCodecHandler;
        ProfilesControlHandler<VideoCodecSettings, VideoInfo> videoProfileHandler;
        private void initVideoHandler()
        {
            this.videoCodec.Items.AddRange(mainForm.PackageSystem.VideoSettingsProviders.ValuesArray);
            try
            {
                this.videoCodec.SelectedItem = mainForm.PackageSystem.VideoSettingsProviders["x264"];
            }
            catch (Exception) { }
            // Init Video handlers
            videoCodecHandler =
                new MultipleConfigurersHandler<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType>(videoCodec);
            videoProfileHandler =
                new ProfilesControlHandler<VideoCodecSettings, VideoInfo>("Video", mainForm, videoProfileControl,
                videoCodecHandler.EditSettings, new InfoGetter<VideoInfo>(delegate() { return new VideoInfo(); }),
                videoCodecHandler.Getter, videoCodecHandler.Setter);
            videoCodecHandler.Register(videoProfileHandler);
            videoProfileHandler.ProfileChanged += new SelectedProfileChangedEvent(ProfileChanged);
        }
        #endregion
        #region Audio profiles
        private void initAudioHandler()
        {
            audio1.initHandler();
            audio2.initHandler();
        }
        #endregion
        #endregion

        #region Variable Declaration
        /// <summary>
        /// whether we ignore the restrictions on container output type set by the profile
        /// </summary>
        private bool ignoreRestrictions = false;
        
        /// <summary>
        /// the restrictions from above: the only containers we may use
        /// </summary>
        private ContainerType[] acceptableContainerTypes;
        
        private VideoUtil vUtil;
        private MainForm mainForm;
        private MuxProvider muxProvider;
        
        /// <summary>
        /// whether the user has selected an output filename
        /// </summary>
        private bool outputChosen = false;

        #endregion
        
        #region init
        public OneClickWindow(MainForm mainForm, JobUtil jobUtil, VideoEncoderProvider vProv, AudioEncoderProvider aProv)
        {
            this.mainForm = mainForm;
            vUtil = new VideoUtil(mainForm);
            this.muxProvider = mainForm.MuxProvider;
            acceptableContainerTypes = muxProvider.GetSupportedContainers().ToArray();

            InitializeComponent();

            audioTrack2.StandardItems = audioTrack1.StandardItems = new object[] { "None" };

            initVideoHandler();
            initAudioHandler();
            initAvsHandler();
            initOneClickHandler();
            
            containerFormat.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
            this.containerFormat.SelectedIndex = 0;
            
            showAdvancedOptions_CheckedChanged(null, null);
        }
        #endregion

        #region event handlers
        private void showAdvancedOptions_CheckedChanged(object sender, EventArgs e)
        {
            if (showAdvancedOptions.Checked)
            {
                if (!tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Add(tabPage2);
                if (!tabControl1.TabPages.Contains(encoderConfigTab))
                    tabControl1.TabPages.Add(encoderConfigTab);
            }
            else
            {
                if (tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Remove(tabPage2);
                if (tabControl1.TabPages.Contains(encoderConfigTab))
                    tabControl1.TabPages.Remove(encoderConfigTab);
            }
        }
        private void containerFormat_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            updateFilename();
        }

        private void updateFilename()
        {
            if (!outputChosen)
            {
                output.Filename = Path.Combine(workingDirectory.Filename, workingName.Text + "." +
                    ((ContainerType)containerFormat.SelectedItem).Extension);
                outputChosen = false;
            }
            else
            {
                output.Filename = Path.ChangeExtension(output.Filename, ((ContainerType)containerFormat.SelectedItem).Extension);
            }
            output.Filter = ((ContainerType)containerFormat.SelectedItem).OutputFilterString;
        }

        private void input_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            openInput(input.Filename);
        }

        private void output_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            outputChosen = true;
        }

        private void workingDirectory_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            updateFilename();
        }

        private void workingName_TextChanged(object sender, EventArgs e)
        {
            updateFilename();
        }
        
        private void openInput(string fileName)
        {
            input.Filename = fileName;
            Dar? ar;
            int maxHorizontalResolution;
            List<AudioTrackInfo> audioTracks;
            List<SubtitleInfo> subtitles;
            vUtil.openVideoSource(fileName, out audioTracks, out subtitles, out ar, out maxHorizontalResolution);
            
            List<object> trackNames = new List<object>();
            trackNames.Add("None");
            foreach (object o in audioTracks)
                trackNames.Add(o);

            audioTrack1.StandardItems = trackNames.ToArray();
            audioTrack2.StandardItems = trackNames.ToArray();

            foreach (AudioTrackInfo ati in audioTracks)
            {
                if (ati.Language.ToLower().Equals(mainForm.Settings.DefaultLanguage1.ToLower()) &&
                    audioTrack1.SelectedIndex == 0)
                {
                    audioTrack1.SelectedObject = ati;
                    continue;
                }

                if (ati.Language.ToLower().Equals(mainForm.Settings.DefaultLanguage2.ToLower()) &&
                    audioTrack2.SelectedIndex == 0)
                {
                    audioTrack2.SelectedObject = ati;
                    continue;
                }
            }

            horizontalResolution.Maximum = maxHorizontalResolution;
            
            string chapterFile = VideoUtil.getChapterFile(fileName);
            if (File.Exists(chapterFile))
                this.chapterFile.Filename = chapterFile;

            workingDirectory.Filename = Path.GetDirectoryName(fileName);
            workingName.Text = PrettyFormatting.ExtractWorkingName(fileName);
            this.updateFilename();
            this.ar.Value = ar;
        }




        private bool beingCalled;
        private void updatePossibleContainers()
        {
            // Since everything calls everything else, this is just a safeguard to make sure we don't infinitely recurse
            if (beingCalled)
                return;
            beingCalled = true;

            List<AudioEncoderType> audioCodecs = new List<AudioEncoderType>();
            List<MuxableType> dictatedOutputTypes = new List<MuxableType>();

            for (int i = 0; i < audio.Length; ++i)
            {
                if (audioTrack[i].SelectedIndex == 0) // "None"
                    continue;

                if (audio[i].Settings != null && !audio[i].DontEncode)
                    audioCodecs.Add(audio[i].Settings.EncoderType);

                else if (audio[i].DontEncode)
                {
                    string typeString;

                    if (audioTrack[i].SelectedSCItem.IsStandard)
                    {
                        AudioTrackInfo ati = (AudioTrackInfo)audioTrack[i].SelectedObject;
                        typeString = "file." + ati.Type;
                    }
                    else
                    {
                        typeString = audioTrack[i].SelectedText;
                    }

                    if (VideoUtil.guessAudioType(typeString) != null)
                        dictatedOutputTypes.Add(VideoUtil.guessAudioMuxableType(typeString, false));
                }
            }

            List<ContainerType> tempSupportedOutputTypes = this.muxProvider.GetSupportedContainers(
                VideoSettingsProvider.EncoderType, audioCodecs.ToArray(), dictatedOutputTypes.ToArray());

            List<ContainerType> supportedOutputTypes = new List<ContainerType>();

            foreach (ContainerType c in acceptableContainerTypes)
            {
                if (tempSupportedOutputTypes.Contains(c))
                    supportedOutputTypes.Add(c);
            }
            
            if (supportedOutputTypes.Count == 0)
            {
                if (tempSupportedOutputTypes.Count > 0 && !ignoreRestrictions)
                {
                    string message = string.Format(
                    "No container type could be found that matches the list of acceptable types" +
                    "in your chosen one click profile. {0}" +
                    "Your restrictions are now being ignored.", Environment.NewLine);
                    MessageBox.Show(message, "Filetype restrictions too restrictive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ignoreRestrictions = true;
                }
                if (ignoreRestrictions) supportedOutputTypes = tempSupportedOutputTypes;
                if (tempSupportedOutputTypes.Count == 0)
                    MessageBox.Show("No container type could be found for your current settings. Please modify the codecs you use", "No container found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (supportedOutputTypes.Count > 0)
            {
                this.containerFormat.Items.Clear();
                this.containerFormat.Items.AddRange(supportedOutputTypes.ToArray());
                this.containerFormat.SelectedIndex = 0;
                this.output.Filename = Path.ChangeExtension(output.Filename, (this.containerFormat.SelectedItem as ContainerType).Extension);
            }
            else
            {
            }
            beingCalled = false;
        }

        private OneClickSettings Settings
        {
            set
            {
                OneClickSettings settings = value;

                refreshAssistingProfiles();

                // Do extra defaults config (same code as in OneClickDefaultWindow)
                // strings
                try
                {
                    audio1.SelectedProfile = settings.AudioProfileName;
                    audio2.SelectedProfile = settings.AudioProfileName;
                }
                catch (ProfileCouldntBeSelectedException e)
                {
                    MessageBox.Show("The audio profile '" + e.ProfileName + "' could not be properly configured. Presumably the profile no longer exists.", "Some options misconfigured", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
                try { videoProfileHandler.SelectedProfile = settings.VideoProfileName; }
                catch (ProfileCouldntBeSelectedException e)
                {
                    MessageBox.Show("The video profile '" + e.ProfileName + "' could not be properly configured. Presumably the profile no longer exists.", "Some options misconfigured", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                try { avsProfileHandler.SelectedProfile = settings.AvsProfileName; }
                catch (ProfileCouldntBeSelectedException e)
                {
                    MessageBox.Show("The Avisynth profile '" + e.ProfileName + "' could not be properly configured. Presumably the profile no longer exists.", "Some options misconfigured", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                List<ContainerType> temp = new List<ContainerType>();
                List<ContainerType> allContainerTypes = muxProvider.GetSupportedContainers();
                foreach (string s in settings.ContainerCandidates)
                {
                    ContainerType ct = allContainerTypes.Find(
                        new Predicate<ContainerType>(delegate(ContainerType t)
                        { return t.ToString() == s; }));
                    if (ct != null)
                        temp.Add(ct);
                }
                acceptableContainerTypes = temp.ToArray();

                ignoreRestrictions = false;

                audio1.DontEncode = settings.DontEncodeAudio;
                audio2.DontEncode = settings.DontEncodeAudio;

                // bools
                signalAR.Checked = settings.SignalAR;
                autoDeint.Checked = settings.AutomaticDeinterlacing;

                splitting.Value = settings.SplitSize;
                optionalTargetSizeBox1.Value = settings.Filesize;
                horizontalResolution.Value = settings.OutputResolution;


                // Clean up after those settings were set
                updatePossibleContainers();
                containerFormat_SelectedIndexChanged_1(null, null);
            }
        }



        private void goButton_Click(object sender, EventArgs e)
        {
            if ((verifyAudioSettings() == null)
                && (VideoSettings != null) 
                && (avsSettingsProvider.GetCurrentSettings() != null)
                && !string.IsNullOrEmpty(input.Filename)
                && !string.IsNullOrEmpty(workingName.Text))
            {
                FileSize? desiredSize = optionalTargetSizeBox1.Value;

                List<PartialAudioStream> audioStreams = new List<PartialAudioStream>();
                for (int i = 0; i < audio.Length; ++i)
                {
                    if (audioTrack[i].SelectedIndex == 0) // "None"
                        continue;

                    PartialAudioStream s = new PartialAudioStream();
                    s.useExternalInput = !audioTrack[i].SelectedSCItem.IsStandard;
                    if (!s.useExternalInput)
                    {
                        s.trackNumber = audioTrack[i].SelectedIndex - 1; // since "None" is first
                        s.language = ((AudioTrackInfo)audioTrack[i].SelectedObject).Language;
                    }
                    s.input = audioTrack[i].SelectedText;
                    s.dontEncode = audio[i].DontEncode;
                    s.settings = audio[i].Settings;
                    audioStreams.Add(s);
                }

                string d2vName = Path.Combine(workingDirectory.Filename, workingName.Text + ".d2v");
                
                DGIndexPostprocessingProperties dpp = new DGIndexPostprocessingProperties();
                dpp.DAR = ar.Value;
                dpp.AudioStreams = audioStreams.ToArray();
                dpp.AutoDeinterlace = autoDeint.Checked;
                dpp.AvsSettings = avsSettingsProvider.GetCurrentSettings();
                dpp.ChapterFile = chapterFile.Filename;
                dpp.Container = (ContainerType)containerFormat.SelectedItem;
                dpp.FinalOutput = output.Filename;
                dpp.HorizontalOutputResolution = (int)horizontalResolution.Value;
                dpp.OutputSize = desiredSize;
                dpp.SignalAR = signalAR.Checked;
                dpp.Splitting = splitting.Value;
                dpp.VideoSettings = VideoSettings.clone();
                IndexJob job = new IndexJob(input.Filename, d2vName, 1,
                    audioTrack1.SelectedIndex - 1, audioTrack2.SelectedIndex - 1, dpp, false);
                mainForm.Jobs.addJobsToQueue(job);
                this.Close();
            }
            else
                MessageBox.Show("You must select audio and video profile, output name and working directory to continue",
                    "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        #endregion

        #region profile management

        private string verifyAudioSettings()
        {
            for (int i = 0; i < audioTrack.Length; ++i)
            {
                if (audioTrack[i].SelectedSCItem.IsStandard)
                    continue;

                string r = MainForm.verifyInputFile(audioTrack[i].SelectedText);
                if (r != null) return r;
            }
            return null;
        }
        #endregion

        public struct PartialAudioStream
        {
            public string input;
            public string language;
            public bool useExternalInput;
            public bool dontEncode;
            public int trackNumber;
            public AudioCodecSettings settings;
        }

        #region updates


        #endregion



        private void input_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            Array data = e.Data.GetData("FileDrop") as Array;
            if (data != null)
            {
                if (data.GetValue(0) is String)
                {
                    string filename = ((string[])data)[0];

                    if (Path.GetExtension(filename).ToLower().Equals(".vob") || Path.GetExtension(filename).ToLower().Equals(".vro") ||
                        Path.GetExtension(filename).ToLower().Equals(".mpg") || Path.GetExtension(filename).ToLower().Equals(".mpeg") ||
                        Path.GetExtension(filename).ToLower().Equals(".m2v") || Path.GetExtension(filename).ToLower().Equals(".mpv") ||
                        Path.GetExtension(filename).ToLower().Equals(".ts")  || Path.GetExtension(filename).ToLower().Equals(".tp") ||
                        Path.GetExtension(filename).ToLower().Equals(".trp") || Path.GetExtension(filename).ToLower().Equals(".pva")                        
                       )
                    {
                        input.Filename = filename;
                        openInput(input.Filename);
                    }
                }
            }
        }

        private void input_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void audioTrack1_SelectionChanged(object sender, string val)
        {
            if (!audioTrack1.SelectedSCItem.IsStandard)
                audio1.openAudioFile((string)audioTrack1.SelectedObject);
        }
        
        private void audioTrack2_SelectionChanged(object sender, string val)
        {
            if (!audioTrack2.SelectedSCItem.IsStandard)
                audio2.openAudioFile((string)audioTrack2.SelectedObject);
        }

        private void audio1_SomethingChanged(object sender, EventArgs e)
        {
            updatePossibleContainers();
        }

        private void targetGroupBox_Enter(object sender, EventArgs e)
        {

        }
    }
    public class OneClickTool : MeGUI.core.plugins.interfaces.ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "One Click Encoder"; }
        }

        public void Run(MainForm info)
        {
            using (OneClickWindow ocmt = new OneClickWindow(info, info.JobUtil, info.Video.VideoEncoderProvider,
                new AudioEncoderProvider()))
            {
                ocmt.ShowDialog();
            }
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.Ctrl1 }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "one_click"; }
        }

        #endregion

    }
    public class OneClickPostProcessor
    {
        #region postprocessor
        private static void postprocess(MainForm mainForm, Job job)
        {
            if (!(job is IndexJob))
                return;
            IndexJob ijob = (IndexJob)job;
            if (ijob.PostprocessingProperties == null)
                return;
            OneClickPostProcessor p = new OneClickPostProcessor(mainForm, ijob);
            p.postprocess();
        }
        public static JobPostProcessor PostProcessor = new JobPostProcessor(postprocess, "OneClick_postprocessor");

        #endregion
        private MainForm mainForm;
        private JobUtil jobUtil;
        private VideoUtil vUtil;
        private IndexJob job;
        private AVCLevels al = new AVCLevels();
        private bool finished = false;
        private bool interlaced = false;
        private DeinterlaceFilter[] filters;
        private StringBuilder logBuilder = new StringBuilder();

        public OneClickPostProcessor(MainForm mainForm, IndexJob ijob)
        {
            this.job = ijob;
            this.mainForm = mainForm;
            this.jobUtil = mainForm.JobUtil;
            this.vUtil = new VideoUtil(mainForm);
        }

        public void postprocess()
        {
            Dictionary<int, string> audioFiles = vUtil.getAllDemuxedAudio(job.Output, 8);

            List<AudioJob> encodableAudioStreams = new List<AudioJob>();
            List<MuxStream> muxOnlyAudioStreams = new List<MuxStream>();
            int counter = 0; // The counter is only used to find the track number in case of an error

            getAudioStreams(audioFiles, job.PostprocessingProperties.AudioStreams, out encodableAudioStreams, out muxOnlyAudioStreams);


            logBuilder.Append("Desired size of this automated encoding series: " + job.PostprocessingProperties.OutputSize
                + " split size: " + job.PostprocessingProperties.Splitting + "\r\n");
            VideoCodecSettings videoSettings = job.PostprocessingProperties.VideoSettings;

            string videoOutput = Path.Combine(Path.GetDirectoryName(job.Output),
                Path.GetFileNameWithoutExtension(job.Output) + "_Video");
            string muxedOutput = job.PostprocessingProperties.FinalOutput;

            //Open the video
            Dar? dar;
            string videoInput = openVideo(job.Output, job.PostprocessingProperties.DAR, 
                job.PostprocessingProperties.HorizontalOutputResolution, job.PostprocessingProperties.SignalAR, logBuilder,
                job.PostprocessingProperties.AvsSettings, job.PostprocessingProperties.AutoDeinterlace, videoSettings, out dar);

            VideoStream myVideo = new VideoStream();
            ulong length;
            double framerate;
            JobUtil.getInputProperties(out length, out framerate, videoInput);
            myVideo.Input = videoInput;
            myVideo.Output = videoOutput;
            myVideo.NumberOfFrames = length;
            myVideo.Framerate = framerate;
            myVideo.DAR = dar;
            myVideo.VideoType = new MuxableType((new VideoEncoderProvider().GetSupportedOutput(videoSettings.EncoderType))[0], videoSettings.Codec);
            myVideo.Settings = videoSettings;
            List<string> intermediateFiles = new List<string>();
            intermediateFiles.Add(videoInput);
            intermediateFiles.Add(job.Output);
            intermediateFiles.AddRange(audioFiles.Values);
            if (!string.IsNullOrEmpty(videoInput))
            {
                //Create empty subtitles for muxing (subtitles not supported in one click mode)
                MuxStream[] subtitles = new MuxStream[0];
                JobChain c = vUtil.GenerateJobSeries(myVideo, muxedOutput, encodableAudioStreams.ToArray(), subtitles,
                    job.PostprocessingProperties.ChapterFile, job.PostprocessingProperties.OutputSize,
                    job.PostprocessingProperties.Splitting, job.PostprocessingProperties.Container,
                    false, muxOnlyAudioStreams.ToArray());
                /*                    vUtil.generateJobSeries(videoInput, videoOutput, muxedOutput, videoSettings,
                                        audioStreams, audio, subtitles, job.PostprocessingProperties.ChapterFile,
                                        job.PostprocessingProperties.OutputSize, job.PostprocessingProperties.SplitSize,
                                        containerOverhead, type, new string[] { job.Output, videoInput });*/
                c = CleanupJob.AddAfter(c, intermediateFiles);
            }
            mainForm.addToLog(logBuilder.ToString());
        }

        private void getAudioStreams(Dictionary<int, string> audioFiles, OneClickWindow.PartialAudioStream[] partialAudioStream, out List<AudioJob> encodableAudioStreams, out List<MuxStream> muxOnlyAudioStreams)
        {
            muxOnlyAudioStreams = new List<MuxStream>();
            encodableAudioStreams = new List<AudioJob>();
            int counter = 0;
            foreach (OneClickWindow.PartialAudioStream propertiesStream in job.PostprocessingProperties.AudioStreams)
            {
                counter++; // The track number starts at 1, so we increment right here. This also ensures it will always be incremented

                bool error = false;
                string input = null, output = null, language = null;
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

                // Settings
                if (propertiesStream.dontEncode)
                    settings = null;
                else if (propertiesStream.settings != null)
                    settings = propertiesStream.settings;
                else
                    error = true;

                // Output
                if (propertiesStream.dontEncode)
                    output = input;
                else if (!error)
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
                        MuxStream newStream = new MuxStream();
                        newStream.path = input;
                        newStream.name = "";
                        newStream.language = language;
                        muxOnlyAudioStreams.Add(newStream);
                    }
                    else
                    {
                        AudioJob encodeStream = new AudioJob();
                        encodeStream.Input = input;
                        encodeStream.Output = output;
                        encodeStream.Settings = settings;
                        encodableAudioStreams.Add(encodeStream);
                    }
                }
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
        private string openVideo(string path, Dar? AR, int horizontalResolution,
            bool signalAR, StringBuilder logBuilder, AviSynthSettings avsSettings, bool autoDeint,
            VideoCodecSettings settings, out Dar? dar)
        {
            dar = null;
            IMediaFile d2v = new d2vFile(path);
            IVideoReader reader = d2v.GetVideoReader();
            if (reader.FrameCount < 1)
            {
                logBuilder.Append("DGDecode reported 0 frames in this file.\r\nThis is a fatal error.\r\n\r\nPlease recreate the DGIndex project");
                return "";
            }

            //Autocrop
            CropValues final = Autocrop.autocrop(reader);
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

            decimal customDAR;

            //Check if AR needs to be autodetected now
            if (AR == null) // it does
            {
                logBuilder.Append("Aspect Ratio set to auto-detect later, detecting now. ");
                customDAR = d2v.Info.DAR.ar;
                if (customDAR > 0)
                    logBuilder.AppendFormat("Found aspect ratio of {0}.{1}", customDAR, Environment.NewLine);
                else
                {
                    customDAR = Dar.ITU16x9.ar;
                    logBuilder.AppendFormat("No aspect ratio found, defaulting to {0}.{1}", customDAR, Environment.NewLine);
                }
            }
            else customDAR = AR.Value.ar;

            //Suggest a resolution (taken from AvisynthWindow.suggestResolution_CheckedChanged)
            int scriptVerticalResolution = Resolution.suggestResolution(d2v.Info.Height, d2v.Info.Width, (double)customDAR,
                final, horizontalResolution, signalAR, mainForm.Settings.AcceptableAspectErrorPercent, out dar);
            if (settings != null && settings is x264Settings) // verify that the video corresponds to the chosen avc level, if not, change the resolution until it does fit
            {
                x264Settings xs = (x264Settings)settings;
                if (xs.Level != 15)
                {
                    int compliantLevel = 15;
                    while (!this.al.validateAVCLevel(horizontalResolution, scriptVerticalResolution, d2v.Info.FPS, xs, out compliantLevel))
                    { // resolution not profile compliant, reduce horizontal resolution by 16, get the new vertical resolution and try again
                        AVCLevels al = new AVCLevels();
                        string levelName = al.getLevels()[xs.Level];
                        logBuilder.Append("Your chosen AVC level " + levelName + " is too strict to allow your chosen resolution of " +
                            horizontalResolution + "*" + scriptVerticalResolution + ". Reducing horizontal resolution by 16.\r\n");
                        horizontalResolution -= 16;
                        scriptVerticalResolution = Resolution.suggestResolution(d2v.Info.Height, d2v.Info.Width, (double)customDAR,
                            final, horizontalResolution, signalAR, mainForm.Settings.AcceptableAspectErrorPercent, out dar);
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

            inputLine = ScriptServer.GetInputLine(path, false, PossibleSources.d2v,
                false, false, false, 0);

            if (autoDeint)
            {
                logBuilder.AppendLine("Automatic deinterlacing was checked. Running now...");
                string d2vPath = path;
                SourceDetector sd = new SourceDetector(inputLine, d2vPath, false,
                    mainForm.Settings.SourceDetectorSettings,
                    new UpdateSourceDetectionStatus(analyseUpdate),
                    new FinishedAnalysis(finishedAnalysis));
                finished = false;
                sd.analyse();
                waitTillAnalyseFinished();
                deinterlaceLines = filters[0].Script;
                logBuilder.AppendLine("Deinterlacing used: " + deinterlaceLines);
            }

            inputLine = ScriptServer.GetInputLine(path, interlaced, PossibleSources.d2v,
                avsSettings.ColourCorrect, avsSettings.MPEG2Deblock, false, 0);

            cropLine = ScriptServer.GetCropLine(true, final);
            denoiseLines = ScriptServer.GetDenoiseLines(avsSettings.Denoise, (DenoiseFilterType)avsSettings.DenoiseMethod);
            resizeLine = ScriptServer.GetResizeLine(true, horizontalResolution, scriptVerticalResolution, (ResizeFilterType)avsSettings.ResizeMethod);

            string newScript = ScriptServer.CreateScriptFromTemplate(avsSettings.Template, inputLine, cropLine, resizeLine, denoiseLines, deinterlaceLines);
            if (dar.HasValue)
                newScript = string.Format("global MeGUI_darx = {0}\r\nglobal MeGUI_dary = {1}\r\n{2}", dar.Value.X, dar.Value.Y, newScript);
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


        public void finishedAnalysis(SourceInfo info, bool error, string errorMessage)
        {
            if (error)
            {
                MessageBox.Show(errorMessage, "Source detection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                filters = new DeinterlaceFilter[] {
                    new DeinterlaceFilter("Error", "#An error occurred in source detection. Doing no processing")};
            }
            else
                this.filters = ScriptServer.GetDeinterlacers(info).ToArray();
            interlaced = (info.sourceType != SourceType.PROGRESSIVE);
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
    }
}
