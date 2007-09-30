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

namespace MeGUI
{

//    public class OneClickPostProcessor 
    public partial class OneClickWindow : Form
    {
        CQMComboBox[] audioTrack;
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
            audioTrack = new CQMComboBox[] { audioTrack1, audioTrack2 };
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
        private bool ignoreRestrictions = false;
        private ContainerType[] acceptableContainerTypes;
        private AudioEncoderProvider audioEncoderProvider = new AudioEncoderProvider();
        private VideoEncoderProvider videoEncoderProvider = new VideoEncoderProvider();
        private VideoUtil vUtil;
        private bool beingCalled = false, updateAudioBeingCalled = false;
        private bool outputChosen = false;
        private MainForm mainForm;
        private BitrateCalculator calc;
        private List<string> audioLanguages;
        private MuxProvider muxProvider;
        private int lastSelectedAudioTrackNumber = 0;
        #endregion
        #region init
        public OneClickWindow(MainForm mainForm, JobUtil jobUtil, VideoEncoderProvider vProv, AudioEncoderProvider aProv)
        {
            this.mainForm = mainForm;
            calc = mainForm.BitrateCalculator;
            vUtil = new VideoUtil(mainForm);
            audioLanguages = new List<string>();
            this.muxProvider = mainForm.MuxProvider;
            acceptableContainerTypes = muxProvider.GetSupportedContainers().ToArray();

            InitializeComponent();

            // Fill the filesize combo box
            this.filesizeComboBox.Items.AddRange(calc.getPredefinedOutputSizes());
            this.filesizeComboBox.Items.Add("Custom");
            this.filesizeComboBox.Items.Add("Don't care");
            this.filesizeComboBox.SelectedIndex = 2;
            this.arComboBox.SelectedIndex = 0;

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
        private void openInput(string fileName)
        {
            input.Filename = fileName;
            AspectRatio ar;
            int maxHorizontalResolution;
            List<AudioTrackInfo> audioTracks;
            List<SubtitleInfo> subtitles;
            int pgc;
            vUtil.openVideoSource(fileName, out audioTracks, out subtitles, out ar, out maxHorizontalResolution, out pgc);
            
            List<object> trackNames = new List<object>();
            foreach (object o in audioTracks)
                trackNames.Add(o);
            trackNames.Insert(0, "None");

            audioTrack1.StandardCQMs = trackNames.ToArray();
            audioTrack2.StandardCQMs = trackNames.ToArray();

            audioTrack1.SelectedIndex = audioTrack2.SelectedIndex = 0;
            
            foreach (AudioTrackInfo ati in audioTracks)
            {
                if (ati.Language.ToLower().Equals(mainForm.Settings.DefaultLanguage1.ToLower()) &&
                    audioTrack1.SelectedIndex == 0)
                {
                    audioTrack1.SelectCQM(ati.ToString());
                    continue;
                }

                if (ati.Language.ToLower().Equals(mainForm.Settings.DefaultLanguage2.ToLower()) &&
                    audioTrack2.SelectedIndex == 0)
                {
                    audioTrack2.SelectCQM(ati.ToString());
                    continue;
                }
            }

            horizontalResolution.Maximum = maxHorizontalResolution;
            string chapterFile = VideoUtil.getChapterFile(fileName);
            if (File.Exists(chapterFile))
            {
                this.chapterFile.Filename = chapterFile;
            }
            audioLanguages.Clear();
            workingDirectory.Filename = Path.GetDirectoryName(fileName);
            workingName.Text = extractWorkingName(fileName);
            this.updateFilename();
            this.setAspectRatio(ar);
        }

        private string extractWorkingName(string fileName)
        {
            string A = Path.GetFileNameWithoutExtension(fileName); // In case they all fail
            int count = 0;
            while (Path.GetDirectoryName(fileName).Length > 0 && count < 3)
            {
                string temp = Path.GetFileNameWithoutExtension(fileName).ToLower();
                if (!temp.Contains("vts") && !temp.Contains("video") && !temp.Contains("audio"))
                {
                    A = temp;
                    break;
                }
                fileName = Path.GetDirectoryName(fileName);
                count++;
            }

            // Format it nicely:
            char[] chars = A.ToCharArray();
            bool beginningOfWord = true;
            for (int i = 0; i < chars.Length; i++)
            {
                // Capitalize the beginning of words
                if (char.IsLetter(chars[i]) && beginningOfWord) chars[i] = char.ToUpper(chars[i]);
                // Turn '_' into ' '
                if (chars[i] == '_') chars[i] = ' ';

                beginningOfWord = !char.IsLetter(chars[i]);
            }

            A = new string(chars);
            return A;
/*            string B = Path.GetFileName(Path.GetDirectoryName(fileName));
            return "al";*/
        }

        private void filesizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.filesizeKB.ReadOnly = true;
            filesizeKB.Text = calc.getOutputSizeKBs(filesizeComboBox.SelectedIndex).ToString();
            if (filesizeComboBox.SelectedIndex == 10) // Custom
                this.filesizeKB.ReadOnly = false;
            if (filesizeComboBox.SelectedIndex == 11) // Don't care
                this.filesizeKB.Text = "-1";
        }

        private void workingName_TextChanged(object sender, EventArgs e)
        {
            updateFilename();
        }

        private void arComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            double dar = 0;
            switch (arComboBox.SelectedIndex)
            {
                case 0:
                    dar = 1.7778;
                    break;
                case 1:
                    dar = 1.3333;
                    break;
                case 2:
                    dar = 1;
                    break;
            }
            AR.Text = dar.ToString();
            if (arComboBox.SelectedIndex == 3)
                AR.ReadOnly = false;
            else
                AR.ReadOnly = true;
        }

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

                    if (audioTrack[i].CQMName.IsStandard)
                    {
                        AudioTrackInfo ati = (AudioTrackInfo)audioTrack[i].CQMName.Tag;
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

        private void splitOutput_CheckedChanged(object sender, EventArgs e)
        {
            if (splitOutput.Checked)
                splitSize.Enabled = true;
            else
            {
                splitSize.Enabled = false;
                splitSize.Text = "0";
            }
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
                try { filesizeComboBox.SelectedItem = settings.StorageMediumName; }
                catch (Exception)
                {
                    MessageBox.Show("The filesize '" + settings.StorageMediumName + "' could not be properly set. Presumably that preset no longer exists.", "Some options misconfigured", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                audio1.DontEncode = settings.DontEncodeAudio;
                audio2.DontEncode = settings.DontEncodeAudio;

                // bools
                signalAR.Checked = settings.SignalAR;
                splitOutput.Checked = settings.Split;
                autoDeint.Checked = settings.AutomaticDeinterlacing;

                // ints
                splitSize.Text = settings.SplitSize.ToString();
                filesizeKB.Text = settings.Filesize.ToString();
                horizontalResolution.Value = settings.OutputResolution;


                // Clean up after those settings were set
                updatePossibleContainers();
                filesizeComboBox_SelectedIndexChanged(null, null);
                containerFormat_SelectedIndexChanged_1(null, null);
                splitOutput_CheckedChanged(null, null);
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
                long desiredSize;
                try
                {
                    desiredSize = Int64.Parse(filesizeKB.Text) * 1024;
                }
                catch (Exception f)
                {
                    MessageBox.Show("I'm not sure how you want me to reach a target size of <empty>.\r\nWhere I'm from that number doesn't exist.\r\n",
                        "Target size undefined", MessageBoxButtons.OK);
                    Console.Write(f.Message);
                    return;
                }

                string infoFile = VideoUtil.getInfoFileName(input.Filename);
                if (infoFile.Length > 0)
                    this.audioLanguages = vUtil.getAudioLanguages(infoFile);
                if (audioLanguages.Count == 0) // add 8 dummy tracks
                    audioLanguages.AddRange(new string[] { "", "", "", "", "", "", "", "" });

                List<PartialAudioStream> audioStreams = new List<PartialAudioStream>();
                for (int i = 0; i < audio.Length; ++i)
                {
                    if (audioTrack[i].SelectedIndex == 0) // "None"
                        continue;

                    PartialAudioStream s = new PartialAudioStream();
                    s.useExternalInput = !audioTrack[i].CQMName.IsStandard;
                    if (!s.useExternalInput)
                    {
                        s.trackNumber = audioTrack[i].SelectedIndex - 1; // since "None" is first
                        s.language = audioLanguages[s.trackNumber];
                    }
                    s.input = audioTrack[i].SelectedText;
                    s.dontEncode = audio[i].DontEncode;
                    s.settings = audio[i].Settings;
                    audioStreams.Add(s);
                }

                string d2vName = workingDirectory.Filename + @"\" + workingName.Text + ".d2v";
                DGIndexPostprocessingProperties dpp = new DGIndexPostprocessingProperties();
                switch (arComboBox.SelectedIndex)
                {
                    case 0:
                        dpp.AR = AspectRatio.ITU16x9;
                        break;
                    case 1:
                        dpp.AR = AspectRatio.ITU4x3;
                        break;
                    case 2:
                        dpp.AR = AspectRatio.A1x1;
                        break;
                    case 3:
                        dpp.AR = AspectRatio.CUSTOM;
                        dpp.CustomAR = Double.Parse(AR.Text);
                        break;
                    case 4:
                        dpp.AutoDeriveAR = true;
                        break;
                }
                dpp.AudioStreams = audioStreams.ToArray();
                dpp.AutoDeinterlace = autoDeint.Checked;
                dpp.AviSynthScript = "";
                dpp.AvsSettings = avsSettingsProvider.GetCurrentSettings();
                dpp.ChapterFile = chapterFile.Filename;
                dpp.Container = (ContainerType)containerFormat.SelectedItem;
                dpp.FinalOutput = output.Filename;
                dpp.HorizontalOutputResolution = (int)horizontalResolution.Value;
                dpp.OutputSize = desiredSize;
                dpp.SignalAR = signalAR.Checked;
                dpp.SplitSize = this.getSplitSize();
                dpp.VideoSettings = VideoSettings.clone();
                IndexJob job = mainForm.JobUtil.generateIndexJob(this.input.Filename, d2vName, 1,
                    audioTrack1.SelectedIndex - 1, audioTrack2.SelectedIndex - 1, dpp);
                mainForm.Jobs.addJobsToQueue(job);
                this.Close();
            }
            else
                MessageBox.Show("You must select audio and video profile, output name and working directory to continue",
                    "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        #endregion
        #region helper methods
        /// <summary>
        /// gets the split size for the muxed output
        /// </summary>
        /// <returns></returns>
        public int getSplitSize()
        {
            int splitSize = 0;
            try
            {
                splitSize = Int32.Parse(this.splitSize.Text) * 1024;
            }
            catch (Exception e)
            {
                MessageBox.Show("I'm not sure how you want me to split the output at an undefinied position.\r\nWhere I'm from that number doesn't exist.\r\n" +
                    "I'm going to assume you meant to not split the output", "Split size undefined", MessageBoxButtons.OK);
                Console.Write(e.Message);
            }
            return splitSize;
        }



        private void setAspectRatio(AspectRatio ratio)
        {
            switch (ratio)
            {
                case AspectRatio.ITU16x9:
                    arComboBox.SelectedIndex = 0;
                    arComboBox_SelectedIndexChanged(null, null);
                    break;
                case AspectRatio.ITU4x3:
                    arComboBox.SelectedIndex = 1;
                    arComboBox_SelectedIndexChanged(null, null);
                    break;
                case AspectRatio.A1x1:
                    arComboBox.SelectedIndex = 2;
                    arComboBox_SelectedIndexChanged(null, null);
                    break;
                default:
                    arComboBox.SelectedIndex = 4;
                    arComboBox_SelectedIndexChanged(null, null);
                    break;

            }
        }
        #endregion
        #region Properties
        private VideoCodecSettings VideoSettings
        {
            get { return VideoSettingsProvider.GetCurrentSettings(); }
        }
        private ISettingsProvider<VideoCodecSettings, VideoInfo, VideoCodec, VideoEncoderType> VideoSettingsProvider
        {
            get { return videoCodecHandler.CurrentSettingsProvider; }
        }
        public string Input
        {
            set
            {
                openInput(value);
            }
        }
        #endregion
        #region profile management

        private string verifyAudioSettings()
        {
            for (int i = 0; i < audioTrack.Length; ++i)
            {
                if (audioTrack[i].CQMName.IsStandard)
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


        #endregion

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
            if (!audioTrack1.CQMName.IsStandard)
                audio1.openAudioFile(audioTrack1.CQMName.Name);
        }
        
        private void audioTrack2_SelectionChanged(object sender, string val)
        {
            if (!audioTrack2.CQMName.IsStandard)
                audio2.openAudioFile(audioTrack2.CQMName.Name);
        }

        private void audio1_SomethingChanged(object sender, EventArgs e)
        {
            updatePossibleContainers();
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
                info.Audio.AudioEncoderProvider))
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

        public OneClickPostProcessor(MainForm mainForm, IndexJob ijob)
        {
            this.job = ijob;
            this.mainForm = mainForm;
            this.jobUtil = mainForm.JobUtil;
            this.vUtil = new VideoUtil(mainForm);
        }

        public void postprocess()
        {
            StringBuilder logBuilder = new StringBuilder();
            VideoUtil vUtil = new VideoUtil(mainForm);
            Dictionary<int, string> audioFiles = vUtil.getAllDemuxedAudio(job.Output, 8);
            List<AudioStream> encodableAudioStreams = new List<AudioStream>();
            List<SubStream> muxOnlyAudioStreams = new List<SubStream>();
            int counter = 0; // The counter is only used to find the track number in case of an error
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
                        SubStream newStream = new SubStream();
                        newStream.path = input;
                        newStream.name = "";
                        newStream.language = language;
                        muxOnlyAudioStreams.Add(newStream);
                    }
                    else
                    {
                        AudioStream encodeStream = new AudioStream();
                        encodeStream.path = input;
                        encodeStream.output = output;
                        encodeStream.settings = settings;
                        encodableAudioStreams.Add(encodeStream);
                    }
                }
            }

            logBuilder.Append("Desired size of this automated encoding series: " + job.PostprocessingProperties.OutputSize
                + " bytes, split size: " + job.PostprocessingProperties.SplitSize + "\r\n");
            VideoCodecSettings videoSettings = job.PostprocessingProperties.VideoSettings;

            string videoOutput = Path.Combine(Path.GetDirectoryName(job.Output),
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
            ulong length;
            double framerate;
            jobUtil.getInputProperties(out length, out framerate, videoInput);
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
                    job.PostprocessingProperties.SplitSize, job.PostprocessingProperties.Container,
                    false, muxOnlyAudioStreams.ToArray(), intermediateFiles);
                /*                    vUtil.generateJobSeries(videoInput, videoOutput, muxedOutput, videoSettings,
                                        audioStreams, audio, subtitles, job.PostprocessingProperties.ChapterFile,
                                        job.PostprocessingProperties.OutputSize, job.PostprocessingProperties.SplitSize,
                                        containerOverhead, type, new string[] { job.Output, videoInput });*/
            }
            mainForm.addToLog(logBuilder.ToString());
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
            IMediaFile d2v = new d2vFile(path);
            IVideoReader reader = d2v.GetVideoReader();
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
                customDAR = (double)d2v.DARX / (double)d2v.DARY;
                if (customDAR > 0)
                    logBuilder.AppendFormat("Found aspect ratio of {0}.{1}", customDAR, Environment.NewLine);
                else
                {
                    customDAR = VideoUtil.getAspectRatio(AspectRatio.ITU16x9);
                    logBuilder.AppendFormat("No aspect ratio found, defaulting to {0}.{1}", customDAR, Environment.NewLine);
                }
            }

            //Suggest a resolution (taken from AvisynthWindow.suggestResolution_CheckedChanged)
            int scriptVerticalResolution = VideoUtil.suggestResolution(d2v.Height, d2v.Width, customDAR,
                final, horizontalResolution, signalAR, mainForm.Settings.AcceptableAspectErrorPercent, out sarX, out sarY);
            if (settings != null && settings is x264Settings) // verify that the video corresponds to the chosen avc level, if not, change the resolution until it does fit
            {
                x264Settings xs = (x264Settings)settings;
                if (xs.Level != 15)
                {
                    int compliantLevel = 15;
                    while (!this.al.validateAVCLevel(horizontalResolution, scriptVerticalResolution, d2v.FPS, xs, out compliantLevel))
                    { // resolution not profile compliant, reduce horizontal resolution by 16, get the new vertical resolution and try again
                        AVCLevels al = new AVCLevels();
                        string levelName = al.getLevels()[xs.Level];
                        logBuilder.Append("Your chosen AVC level " + levelName + " is too strict to allow your chosen resolution of " +
                            horizontalResolution + "*" + scriptVerticalResolution + ". Reducing horizontal resolution by 16.\r\n");
                        horizontalResolution -= 16;
                        scriptVerticalResolution = VideoUtil.suggestResolution(d2v.Height, d2v.Width, customDAR,
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
