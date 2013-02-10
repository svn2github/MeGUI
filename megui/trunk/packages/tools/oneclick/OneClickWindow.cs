// ****************************************************************************
// 
// Copyright (C) 2005-2013 Doom9 & al
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.util;
using MeGUI.packages.tools.oneclick;

namespace MeGUI
{
    public partial class OneClickWindow : Form
    {
        #region variable declaration
        LogItem _oLog;
        List<OneClickStreamControl> audioTracks;
        List<OneClickStreamControl> subtitleTracks;
        OneClickSettings _oSettings;
        private bool bLock;

        /// <summary>
        /// whether the current processing should be done without user interaction
        /// </summary>
        private bool bAutomatedProcessing;

        /// <summary>
        /// whether the current processing is a part of batch processing
        /// </summary>
        private bool bBatchProcessing;

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
        private MediaInfoFile _videoInputInfo;

        /// <summary>
        /// whether the user has selected an output filename
        /// </summary>
        private bool outputChosen = false;
        #endregion

        #region profiles
        #region OneClick profiles
        private void initOneClickHandler()
        {
            oneclickProfile.Manager = mainForm.Profiles;
        }

        private void initTabs()
        {
            audioTracks = new List<OneClickStreamControl>();
            audioTracks.Add(oneClickAudioStreamControl1);
            oneClickAudioStreamControl1.StandardStreams = new object[] { "None" };
            oneClickAudioStreamControl1.SelectedStreamIndex = 0;
            oneClickAudioStreamControl1.Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.AudioTypes.ValuesArray);
            
            subtitleTracks = new List<OneClickStreamControl>();
            subtitleTracks.Add(oneClickSubtitleStreamControl1);
            oneClickSubtitleStreamControl1.StandardStreams = new object[] { "None" };
            oneClickSubtitleStreamControl1.SelectedStreamIndex = 0;
            oneClickSubtitleStreamControl1.Filter = VideoUtil.GenerateCombinedFilter(ContainerManager.SubtitleTypes.ValuesArray);
        }

        void OneClickProfileChanged(object sender, EventArgs e)
        {
            SetOneClickProfile((OneClickSettings)oneclickProfile.SelectedProfile.BaseSettings);
        }
        #endregion
        #region Video profiles
        private VideoCodecSettings VideoSettings
        {
            get { return (VideoCodecSettings)videoProfile.SelectedProfile.BaseSettings; }
        }
        #endregion
        #region Audio profiles
        private void initAudioHandler()
        {
            oneClickAudioStreamControl1.initProfileHandler();
            oneClickSubtitleStreamControl1.initProfileHandler();
        }
        #endregion
        #endregion
        
        #region init
        public OneClickWindow(MainForm mainForm)
        {
            this.mainForm = mainForm;
            this._oLog = mainForm.OneClickLog;
            if (_oLog == null)
            {
                _oLog = mainForm.Log.Info("OneClick");
                mainForm.OneClickLog = _oLog;
            }
            vUtil = new VideoUtil(mainForm);
            this.muxProvider = mainForm.MuxProvider;
            acceptableContainerTypes = muxProvider.GetSupportedContainers().ToArray();
            InitializeComponent();
            videoProfile.Manager = mainForm.Profiles;
            initTabs();
            initAudioHandler();
            avsProfile.Manager = mainForm.Profiles;
            initOneClickHandler();

            //if containerFormat has not yet been set by the oneclick profile, add supported containers
            if (containerFormat.Items.Count == 0)
            {
                containerFormat.Items.AddRange(muxProvider.GetSupportedContainers().ToArray());
                this.containerFormat.SelectedIndex = 0;
            }

            //add device type
            if (devicetype.Items.Count == 0)
            {
                devicetype.Items.Add("Standard");
                devicetype.Items.AddRange(muxProvider.GetSupportedDevices((ContainerType)this.containerFormat.SelectedItem).ToArray());
            }
            if (containerFormat.SelectedItem.ToString().Equals(mainForm.Settings.AedSettings.Container))
            {
                foreach (object o in devicetype.Items) // I know this is ugly, but using the DeviceOutputType doesn't work unless we're switching to manual serialization
                {
                    if (o.ToString().Equals(mainForm.Settings.AedSettings.DeviceOutputType))
                    {
                        devicetype.SelectedItem = o;
                        break;
                    }
                }
            }
            else
                this.devicetype.SelectedIndex = 0;

            bLock = true;
            cbGUIMode.DataSource = EnumProxy.CreateArray(new object[] { MeGUISettings.OCGUIMode.Basic, MeGUISettings.OCGUIMode.Default, MeGUISettings.OCGUIMode.Advanced });
            bLock = false;

            if (MainForm.Instance.Settings.IsDGIIndexerAvailable())
                input.Filter = "All supported files|*.avs;*.ifo;*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.264;*.h264;*.avc;*.m2t*;*.m2ts;*.mts;*.tp;*.ts;*.trp;*.vob;*.ifo;*.mpg;*.mpeg;*.m1v;*.m2v;*.mpv;*.pva;*.vro;*.vc1;*.mpls|All DGAVCIndex supported files|*.264;*.h264;*.avc;*.m2t*;*.m2ts;*.mts;*.tp;*.ts;*.trp|All DGIndex supported files|*.vob;*.mpg;*.mpeg;*.m1v;*.m2v;*.mpv;*.tp;*.ts;*.trp;*.m2t;*.m2ts;*.pva;*.vro|All DGIndexNV supported files|*.264;*.h264;*.avc;*.m2v;*.mpv;*.vc1;*.mkv;*.vob;*.mpg;*.mpeg;*.m2t;*.m2ts;*.mts;*.tp;*.ts;*.trp|All FFMS Indexer supported files|*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.vob;*.mpg;*.m2ts;*.ts|AviSynth Scripts|*.avs|IFO DVD files|*.ifo|Blu-Ray Playlist|*.mpls|All files|*.*";
            else
                input.Filter = "All supported files|*.avs;*.ifo;*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.264;*.h264;*.avc;*.m2t*;*.m2ts;*.mts;*.tp;*.ts;*.trp;*.vob;*.ifo;*.mpg;*.mpeg;*.m1v;*.m2v;*.mpv;*.pva;*.vro;*.mpls|All DGAVCIndex supported files|*.264;*.h264;*.avc;*.m2t*;*.m2ts;*.mts;*.tp;*.ts;*.trp|All DGIndex supported files|*.vob;*.mpg;*.mpeg;*.m1v;*.m2v;*.mpv;*.tp;*.ts;*.trp;*.m2t;*.m2ts;*.pva;*.vro|All FFMS Indexer supported files|*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.vob;*.mpg;*.m2ts;*.ts|AviSynth Scripts|*.avs|IFO DVD files|*.ifo|Blu-Ray Playlist|*.mpls|All files|*.*";
            DragDropUtil.RegisterMultiFileDragDrop(input, setInput, delegate() { return input.Filter + "|All folders|*."; });
            DragDropUtil.RegisterSingleFileDragDrop(output, setOutput);
            DragDropUtil.RegisterSingleFileDragDrop(chapterFile, null, delegate() { return chapterFile.Filter; });
            DragDropUtil.RegisterSingleFileDragDrop(workingDirectory, setWorkingDirectory);
        }
        #endregion

        #region event handlers
        private void cbGUIMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumProxy o = cbGUIMode.SelectedItem as EnumProxy;
            if (o == null)
                return;

            if (bLock)
                cbGUIMode.SelectedItem = EnumProxy.Create(MainForm.Instance.Settings.OneClickGUIMode);
            else
                MainForm.Instance.Settings.OneClickGUIMode = (MeGUISettings.OCGUIMode)o.RealValue;
            
            if (MainForm.Instance.Settings.OneClickGUIMode == MeGUISettings.OCGUIMode.Advanced)
            {
                audioTab.Height = 175;
                subtitlesTab.Location = new Point(subtitlesTab.Location.X, 258);
                subtitlesTab.Visible = true;
                outputTab.Location = new Point(outputTab.Location.X, 379);
                this.Height = 583;
                if (!tabControl1.TabPages.Contains(encoderConfigTab))
                    tabControl1.TabPages.Add(encoderConfigTab);
            }
            else if (MainForm.Instance.Settings.OneClickGUIMode == MeGUISettings.OCGUIMode.Basic)
            {
                audioTab.Height = 90;
                subtitlesTab.Visible = false;
                outputTab.Location = new Point(outputTab.Location.X, 173);
                this.Height = 377;
                if (tabControl1.TabPages.Contains(encoderConfigTab))
                    tabControl1.TabPages.Remove(encoderConfigTab);
            }
            else
            {
                audioTab.Height = 115;
                subtitlesTab.Location = new Point(subtitlesTab.Location.X, 198);
                subtitlesTab.Visible = true;
                outputTab.Location = new Point(outputTab.Location.X, 319);
                this.Height = 523;
                if (tabControl1.TabPages.Contains(encoderConfigTab))
                    tabControl1.TabPages.Remove(encoderConfigTab);
            }
        }

        private void containerFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.containerFormat.Text == "MKV")
                this.devicetype.Enabled = false;
            else 
                this.devicetype.Enabled = true;
            updateFilename();

            //add device types
            devicetype.Items.Clear();
            devicetype.Items.Add("Standard");
            devicetype.Items.AddRange(muxProvider.GetSupportedDevices((ContainerType)this.containerFormat.SelectedItem).ToArray());
            if (containerFormat.SelectedItem.ToString().Equals(mainForm.Settings.AedSettings.Container))
            {
                foreach (object o in devicetype.Items) // I know this is ugly, but using the DeviceOutputType doesn't work unless we're switching to manual serialization
                {
                    if (o.ToString().Equals(mainForm.Settings.AedSettings.DeviceOutputType))
                    {
                        devicetype.SelectedItem = o;
                        break;
                    }
                }
            }
            else
                this.devicetype.SelectedIndex = 0;
        }

        private void updateWorkingName(String strInputFile)
        {
            int iPGCNumber = 0;
            if (_videoInputInfo != null)
                iPGCNumber = _videoInputInfo.VideoInfo.PGCNumber;
            
            if (iPGCNumber > 1)
            {
                String strTempName = Path.GetFileNameWithoutExtension(strInputFile);
                if (strTempName.StartsWith("VTS_", StringComparison.InvariantCultureIgnoreCase) &&
                    strTempName.EndsWith("1", StringComparison.InvariantCultureIgnoreCase))
                {
                    strTempName = strTempName.Substring(0, strTempName.Length - 1) + iPGCNumber;
                    strTempName = Path.Combine(Path.GetDirectoryName(strInputFile), strTempName + Path.GetExtension(strInputFile));
                    workingName.Text = PrettyFormatting.ExtractWorkingName(strTempName, _oSettings.LeadingName, _oSettings.WorkingNameReplace, _oSettings.WorkingNameReplaceWith);
                }
                else
                    workingName.Text = PrettyFormatting.ExtractWorkingName(strInputFile, _oSettings.LeadingName, _oSettings.WorkingNameReplace, _oSettings.WorkingNameReplaceWith);
            }
            else
                workingName.Text = PrettyFormatting.ExtractWorkingName(strInputFile, _oSettings.LeadingName, _oSettings.WorkingNameReplace, _oSettings.WorkingNameReplaceWith);

            this.updateFilename();
        }

        private void updateFilename()
        {
            if (!outputChosen)
            {
                String strVideoInput = input.SelectedText;
                if (!String.IsNullOrEmpty(strVideoInput) && File.Exists(strVideoInput))
                {
                    if (!String.IsNullOrEmpty(_oSettings.DefaultOutputDirectory) && Directory.Exists(_oSettings.DefaultOutputDirectory))
                    {
                        output.Filename = Path.Combine(_oSettings.DefaultOutputDirectory, workingName.Text + "." +
                            ((ContainerType)containerFormat.SelectedItem).Extension);
                    }
                    else if (!String.IsNullOrEmpty(MainForm.Instance.Settings.DefaultOutputDir) && Directory.Exists(MainForm.Instance.Settings.DefaultOutputDir))
                    {
                        output.Filename = Path.Combine(MainForm.Instance.Settings.DefaultOutputDir, workingName.Text + "." +
                            ((ContainerType)containerFormat.SelectedItem).Extension);
                    }
                    else
                    {
                        output.Filename = Path.Combine(Path.GetDirectoryName(strVideoInput), workingName.Text + "." +
                            ((ContainerType)containerFormat.SelectedItem).Extension);
                    }
                }
                else
                {
                    output.Filename = Path.ChangeExtension(output.Filename, ((ContainerType)containerFormat.SelectedItem).Extension);
                }
                outputChosen = false;
            }
            else
            {
                output.Filename = Path.ChangeExtension(output.Filename, ((ContainerType)containerFormat.SelectedItem).Extension);
            }
            output.Filter = ((ContainerType)containerFormat.SelectedItem).OutputFilterString;
        }

        private void input_SelectionChanged(object sender, string val)
        {
            if (!String.IsNullOrEmpty(input.SelectedText))
            {
                if (!bAutomatedProcessing && !bLock)
                    openInput(input.SelectedText);
            }   
        }

        private void setOutput(string strFileName)
        {
            output.Filename = strFileName;
            output_FileSelected(null, null);  
        }

        private void output_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            outputChosen = true;
            updateFilename();
        }

        private void setWorkingDirectory(string strFolder)
        {
            workingDirectory.Filename = strFolder;
            updateFilename();
        }

        private void workingDirectory_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            if (File.Exists(workingDirectory.Filename))
                workingDirectory.Filename = Path.GetDirectoryName(workingDirectory.Filename);
            updateFilename();
        }

        private void workingName_TextChanged(object sender, EventArgs e)
        {
            updateFilename();
        }

        public void setInput(string strFileorFolderName)
        {
            input.AddCustomItem(strFileorFolderName);
            input.SelectedObject = strFileorFolderName;
        }

        public void setInput(string[] strFileorFolderName)
        {
            List<OneClickFilesToProcess> arrFilesToProcess = new List<OneClickFilesToProcess>();
            foreach (string strFile in strFileorFolderName)
                if (File.Exists(strFile))
                    arrFilesToProcess.Add(new OneClickFilesToProcess(strFile, 1));
            if (arrFilesToProcess.Count == 0)
            {
                MessageBox.Show("These files or folders cannot be used in OneClick mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            goButton.Enabled = false;
            OneClickProcessing oProcessor = new OneClickProcessing(this, arrFilesToProcess, _oSettings, _oLog);
        }
        
        private void openInput(string fileName)
        {
            if (!Directory.Exists(fileName) && !File.Exists(fileName))
            {
                MessageBox.Show("Input " + fileName + " does not exists", "Input not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            goButton.Enabled = false;
            OneClickProcessing oProcessor = new OneClickProcessing(this, fileName, _oSettings, _oLog);
        }

        public void setOpenFailure()
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
            MessageBox.Show("This file or folder cannot be used in OneClick mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void setBatchProcessing(List<OneClickFilesToProcess> arrFilesToProcess, OneClickSettings oSettings)
        {
            bBatchProcessing = bAutomatedProcessing = true;
            SetOneClickProfile(oSettings);
            OneClickProcessing oProcessor = new OneClickProcessing(this, arrFilesToProcess, oSettings, _oLog);
            return;
        }

        public void setInputData(MediaInfoFile iFile, List<OneClickFilesToProcess> arrFilesToProcess)
        {
            if (iFile == null)
                return;

            if (!bAutomatedProcessing && arrFilesToProcess.Count > 0)
            {
                string question = "Do you want to process all " + (arrFilesToProcess.Count + 1) + " files/tracks in the selection?\r\nThey all will be processed with the current settings\r\nin the OneClick profile \"" + oneclickProfile.SelectedProfile.Name + "\".\r\nOther settings will be ignored.";
                DialogResult dr = MessageBox.Show(question, "Automated folder processing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    bAutomatedProcessing = true;
                    SetOneClickProfile((OneClickSettings)oneclickProfile.SelectedProfile.BaseSettings);
                    _oSettings.LeadingName = MeGUI.core.gui.InputBox.Show("If desired please enter a leading name", "Please enter a leading name", _oSettings.LeadingName);
                }
            }
            bLock = true;
            if (input.SelectedSCItem == null || !iFile.FileName.Equals((string)input.SelectedObject))
            {
                input.StandardItems = new object[] { iFile.FileName };
                input.SelectedIndex = 0; 
            }
            bLock = false;

            _videoInputInfo = iFile;

            int maxHorizontalResolution = int.Parse(iFile.VideoInfo.Width.ToString());
       
            List<OneClickStream> arrAudioTrackInfo = new List<OneClickStream>();
            foreach (AudioTrackInfo oInfo in iFile.AudioInfo.Tracks)
                arrAudioTrackInfo.Add(new OneClickStream(oInfo));
            AudioResetTrack(arrAudioTrackInfo, _oSettings);

            List<OneClickStream> arrSubtitleTrackInfo = new List<OneClickStream>();
            foreach (SubtitleTrackInfo oInfo in iFile.SubtitleInfo.Tracks)
                arrSubtitleTrackInfo.Add(new OneClickStream(oInfo));
            SubtitleResetTrack(arrSubtitleTrackInfo, _oSettings);

            horizontalResolution.Maximum = maxHorizontalResolution;
            
            // Detect Chapters
            if (_videoInputInfo.hasMKVChapters())
                this.chapterFile.Filename = "<internal chapters>";
            else if (Path.GetExtension(iFile.FileName).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".vob"))
                this.chapterFile.Filename = "<internal chapters>";
            else if (_videoInputInfo.getEac3toChaptersTrack() > -1)
                this.chapterFile.Filename = "<internal chapters>";
            else
            {
                string chapterFile = VideoUtil.getChapterFile(iFile.FileName);
                if (File.Exists(chapterFile))
                    this.chapterFile.Filename = chapterFile;
            }

            if (string.IsNullOrEmpty(workingDirectory.Filename))
                workingDirectory.Filename = Path.GetDirectoryName(iFile.FileName);

            updateWorkingName(iFile.FileName);

            this.ar.Value = _videoInputInfo.VideoInfo.DAR;

            if (VideoSettings.EncoderType.ID == "x264" && this.chapterFile.Filename != null)
                this.usechaptersmarks.Enabled = true;

            if (bAutomatedProcessing)
                createOneClickJob(arrFilesToProcess);

            this.Cursor = System.Windows.Forms.Cursors.Default;
            goButton.Enabled = true;
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

            if (audioTracks != null)
            {
                for (int i = 0; i < audioTracks.Count; ++i)
                {
                    if (audioTracks[i].SelectedStreamIndex == 0) // "None"
                        continue;

                    if (audioTracks[i].SelectedStream.EncoderSettings != null && audioTracks[i].SelectedStream.EncodingMode != AudioEncodingMode.Never)
                        audioCodecs.Add(audioTracks[i].SelectedStream.EncoderSettings.EncoderType);
                    else if (audioTracks[i].SelectedStream.EncodingMode == AudioEncodingMode.Never)
                    {
                        string typeString;
                        if (audioTracks[i].SelectedItem.IsStandard)
                        {
                            AudioTrackInfo ati = (AudioTrackInfo)audioTracks[i].SelectedStream.TrackInfo;
                            typeString = "file." + ati.Codec;
                        }
                        else
                        {
                            typeString = audioTracks[i].SelectedFile;
                        }

                        if (VideoUtil.guessAudioType(typeString) != null)
                            dictatedOutputTypes.Add(VideoUtil.guessAudioMuxableType(typeString, false));
                    }
                }
            }

            if (subtitleTracks != null)
            {
                for (int i = 0; i < subtitleTracks.Count; ++i)
                {
                    if (subtitleTracks[i].SelectedStreamIndex == 0) // "None"
                        continue;

                    string typeString;
                    if (subtitleTracks[i].SelectedItem.IsStandard)
                        typeString = subtitleTracks[i].SelectedStream.TrackInfo.DemuxFileName;
                    else
                        typeString = subtitleTracks[i].SelectedFile;

                    SubtitleType subtitleType = VideoUtil.guessSubtitleType(typeString);
                    if (subtitleType != null)
                        dictatedOutputTypes.Add((new MuxableType(subtitleType, null)));
                }
            }

            List<ContainerType> tempSupportedOutputTypes = this.muxProvider.GetSupportedContainers(
                VideoSettings.EncoderType, audioCodecs.ToArray(), dictatedOutputTypes.ToArray());

            List<ContainerType> supportedOutputTypes = new List<ContainerType>();

            foreach (ContainerType c in acceptableContainerTypes)
                if (tempSupportedOutputTypes.Contains(c))
                    supportedOutputTypes.Add(c);

            ignoreRestrictions = false;
            if (supportedOutputTypes.Count == 0)
            {
                if (tempSupportedOutputTypes.Count > 0 && !ignoreRestrictions)
                {
                    if (!bAutomatedProcessing)
                    {
                        string message = string.Format(
                        "No container type could be found that matches the list of acceptable types " +
                        "in your chosen one click profile. {0}" +
                        "Your restrictions are now being ignored.", Environment.NewLine);
                        MessageBox.Show(message, "Filetype restrictions too restrictive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    ignoreRestrictions = true;
                }
                if (ignoreRestrictions)
                    supportedOutputTypes = tempSupportedOutputTypes;
                if (tempSupportedOutputTypes.Count == 0)
                {
                    if (bAutomatedProcessing)
                        ignoreRestrictions = true;
                    else
                        MessageBox.Show("No container type could be found for your current settings. Please modify the codecs you use", "No container found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (supportedOutputTypes.Count > 0)
            {
                this.containerFormat.Items.Clear();
                this.containerFormat.Items.AddRange(supportedOutputTypes.ToArray());
                this.containerFormat.SelectedIndex = 0;
                this.output.Filename = Path.ChangeExtension(output.Filename, (this.containerFormat.SelectedItem as ContainerType).Extension);
            }
            beingCalled = false;
        }

        private void SetOneClickProfile(OneClickSettings settings)
        {
            _oSettings = settings.Clone();

            if (_videoInputInfo != null)
            {
                List<OneClickStream> arrAudioTrackInfo = new List<OneClickStream>();
                foreach (AudioTrackInfo oInfo in _videoInputInfo.AudioInfo.Tracks)
                    arrAudioTrackInfo.Add(new OneClickStream(oInfo));
                AudioResetTrack(arrAudioTrackInfo, settings);

                List<OneClickStream> arrSubtitleTrackInfo = new List<OneClickStream>();
                foreach (SubtitleTrackInfo oInfo in _videoInputInfo.SubtitleInfo.Tracks)
                    arrSubtitleTrackInfo.Add(new OneClickStream(oInfo));
                SubtitleResetTrack(arrSubtitleTrackInfo, settings);
            }
            else
                ResetAudioSettings(settings);

            videoProfile.SetProfileNameOrWarn(settings.VideoProfileName);
            avsProfile.SetProfileNameOrWarn(settings.AvsProfileName);

            List<ContainerType> temp = new List<ContainerType>();
            List<ContainerType> allContainerTypes = muxProvider.GetSupportedContainers();
            foreach (string s in settings.ContainerCandidates)
            {
                ContainerType ct = allContainerTypes.Find(new Predicate<ContainerType>(delegate(ContainerType t) { return t.ToString() == s; }));
                if (ct != null)
                    temp.Add(ct);
            }
            acceptableContainerTypes = temp.ToArray();

            // bools
            chkDontEncodeVideo.Checked = settings.DontEncodeVideo;
            signalAR.Checked = settings.SignalAR;
            autoDeint.Checked = settings.AutomaticDeinterlacing;
            autoCrop.Checked = settings.AutoCrop;
            keepInputResolution.Checked = settings.KeepInputResolution;
            addPrerenderJob.Checked = settings.PrerenderVideo;
            if (usechaptersmarks.Enabled)
                usechaptersmarks.Checked = settings.UseChaptersMarks;

            splitting.Value = settings.SplitSize;
            fileSize.Value = settings.Filesize;
            if (settings.OutputResolution <= horizontalResolution.Maximum)
                horizontalResolution.Value = settings.OutputResolution;
            workingDirectory.Filename = settings.DefaultWorkingDirectory;

            // device type
            devicetype.Text = settings.DeviceOutputType;

            // Clean up after those settings were set
            updatePossibleContainers();
            containerFormat_SelectedIndexChanged(null, null);

            if (!string.IsNullOrEmpty(input.SelectedText))
                updateWorkingName(input.SelectedText);
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            goButton.Enabled = false;
            createOneClickJob(null);
        }

        private void createOneClickJob(List<OneClickFilesToProcess> arrFilesToProcess)
        {
            // checks if there is a suitable container
            if (ignoreRestrictions)
            {
                _oLog.LogEvent(_videoInputInfo.FileName + ": No container type could be found that matches the list of acceptable types in your chosen one click profile. Skipping...");
                if (arrFilesToProcess != null)
                    setBatchProcessing(arrFilesToProcess, _oSettings);
                return;
            }

            // set random working directory
            string strWorkingDirectory = string.Empty;
            if (Directory.Exists(workingDirectory.Filename))
                strWorkingDirectory = workingDirectory.Filename;
            else
                strWorkingDirectory = Path.GetDirectoryName(output.Filename);
            do
                strWorkingDirectory = Path.Combine(strWorkingDirectory, Path.GetRandomFileName());
            while (Directory.Exists(strWorkingDirectory));

            if (!verifyInputSettings(_videoInputInfo, strWorkingDirectory))
            {
                goButton.Enabled = true;
                return;
            }

            ContainerType inputContainer = _videoInputInfo.ContainerFileType;
            JobChain prepareJobs = null;

            // set initial oneclick job settings
            OneClickPostprocessingProperties dpp = new OneClickPostprocessingProperties();
            dpp.DAR = ar.Value;
            dpp.AvsSettings = (AviSynthSettings)avsProfile.SelectedProfile.BaseSettings;
            dpp.Container = (ContainerType)containerFormat.SelectedItem;
            dpp.FinalOutput = output.Filename;
            dpp.DeviceOutputType = devicetype.Text;
            dpp.VideoSettings = VideoSettings.Clone();
            dpp.Splitting = splitting.Value;
            dpp.VideoInput = _videoInputInfo.FileName;
            dpp.IndexType = _videoInputInfo.IndexerToUse;
            dpp.TitleNumberToProcess = _videoInputInfo.VideoInfo.PGCNumber;
            if (arrFilesToProcess != null)
            {
                dpp.FilesToProcess = arrFilesToProcess;
                dpp.OneClickSetting = _oSettings;
            }
            dpp.WorkingDirectory = strWorkingDirectory;
            dpp.FilesToDelete.Add(dpp.WorkingDirectory);
            dpp.ChapterFile = chapterFile.Filename;
            dpp.AutoCrop = autoCrop.Checked;


            // mux input file into MKV if possible
            if (inputContainer != ContainerType.MKV 
                && (dpp.IndexType == FileIndexerWindow.IndexType.FFMS || dpp.IndexType == FileIndexerWindow.IndexType.AVISOURCE))
            {
                // and necessary
                bool bRemuxInput = false;
                if (chkDontEncodeVideo.Checked && dpp.Container == ContainerType.MKV)
                    bRemuxInput = true;

                foreach (OneClickStreamControl oStreamControl in audioTracks)
                {
                    if (!oStreamControl.SelectedItem.IsStandard)
                        continue;

                    if (oStreamControl.SelectedStreamIndex <= 0) // not NONE
                        continue;

                    bRemuxInput = true;
                }

                foreach (OneClickStreamControl oStreamControl in subtitleTracks)
                {
                    if (!oStreamControl.SelectedItem.IsStandard)
                        continue;

                    if (oStreamControl.SelectedStreamIndex <= 0) // not NONE
                        continue;

                    bRemuxInput = true;
                }

                if (bRemuxInput && _videoInputInfo.MuxableToMKV())
                {
                    // create job
                    MuxJob mJob = new MuxJob();
                    mJob.MuxType = MuxerType.MKVMERGE;
                    mJob.Input = dpp.VideoInput;
                    mJob.Output = Path.Combine(dpp.WorkingDirectory, Path.GetFileNameWithoutExtension(dpp.VideoInput) + ".mkv"); ;
                    mJob.Settings.MuxAll = true;
                    mJob.Settings.MuxedInput = mJob.Input;
                    mJob.Settings.MuxedOutput = mJob.Output;
                    dpp.FilesToDelete.Add(mJob.Output);

                    // change input file properties
                    inputContainer = ContainerType.MKV;
                    dpp.VideoInput = mJob.Output;

                    // add job to queue
                    prepareJobs = new SequentialChain(mJob);
                }
            }


            // create eac3to demux job if needed
            if (_videoInputInfo.isEac3toDemuxable())
            {
                dpp.Eac3toDemux = true;
                StringBuilder sb = new StringBuilder();

                dpp.VideoInput = Path.Combine(dpp.WorkingDirectory, Path.GetFileNameWithoutExtension(_videoInputInfo.FileName) + ".mkv");
                sb.Append(string.Format("{0}:\"{1}\" ", _videoInputInfo.VideoInfo.Track.TrackID, dpp.VideoInput));
                inputContainer = ContainerType.MKV;
                dpp.FilesToDelete.Add(dpp.VideoInput);
                dpp.FilesToDelete.Add(Path.Combine(dpp.WorkingDirectory, Path.GetFileNameWithoutExtension(dpp.VideoInput) + " - Log.txt"));
                
                if (dpp.ChapterFile.Equals("<internal chapters>"))
                {
                    int iTrackNumber = _videoInputInfo.getEac3toChaptersTrack();
                    if (iTrackNumber > -1)
                    {
                        dpp.ChapterFile = Path.Combine(dpp.WorkingDirectory, Path.GetFileNameWithoutExtension(dpp.VideoInput) + " - chapter.txt");
                        sb.Append(string.Format("{0}:\"{1}\" ", iTrackNumber, dpp.ChapterFile)); 
                        dpp.FilesToDelete.Add(dpp.ChapterFile);
                    }
                }

                foreach (OneClickStreamControl oStreamControl in audioTracks)
                {
                    if (!oStreamControl.SelectedItem.IsStandard)
                        continue;

                    if (oStreamControl.SelectedStreamIndex <= 0) // not NONE
                        continue;

                    bool bCoreOnly = false;
                    if (!oStreamControl.IsDontEncodePossible() || oStreamControl.SelectedStream.EncodingMode != AudioEncodingMode.Never)
                    {
                        //check if core must be extracted
                        if (oStreamControl.SelectedStream.TrackInfo.Codec.ToLower(System.Globalization.CultureInfo.InvariantCulture).Contains("truehd"))
                        {
                            oStreamControl.SelectedStream.TrackInfo.Codec = "AC-3";
                            bCoreOnly = true;
                        }
                        else if (oStreamControl.SelectedStream.TrackInfo.Codec.StartsWith("DTS-HD", StringComparison.InvariantCultureIgnoreCase))
                        {
                            oStreamControl.SelectedStream.TrackInfo.Codec = "DTS";
                            bCoreOnly = true;
                        }
                    }

                    sb.Append(string.Format("{0}:\"{1}\" ", oStreamControl.SelectedStream.TrackInfo.TrackID,
                        Path.Combine(dpp.WorkingDirectory, oStreamControl.SelectedStream.TrackInfo.DemuxFileName)));
                    if (bCoreOnly)
                        sb.Append("-core ");

                    dpp.FilesToDelete.Add(Path.Combine(dpp.WorkingDirectory, oStreamControl.SelectedStream.TrackInfo.DemuxFileName));
                }

                foreach (OneClickStreamControl oStreamControl in subtitleTracks)
                {
                    if (!oStreamControl.SelectedItem.IsStandard)
                        continue;

                    if (oStreamControl.SelectedStreamIndex <= 0) // not NONE
                        continue;

                    string strDemuxFilePath = Path.Combine(dpp.WorkingDirectory, oStreamControl.SelectedStream.TrackInfo.DemuxFileName);
                    sb.Append(string.Format("{0}:\"{1}\" ", oStreamControl.SelectedStream.TrackInfo.MMGTrackID, strDemuxFilePath));
                    oStreamControl.SelectedStream.DemuxFilePath = strDemuxFilePath;
                    dpp.FilesToDelete.Add(strDemuxFilePath);
                    dpp.SubtitleTracks.Add(oStreamControl.SelectedStream);
                }

                if (sb.Length != 0)
                    prepareJobs = new SequentialChain(prepareJobs, new HDStreamsExJob(new List<string>() { _videoInputInfo.FileName }, dpp.WorkingDirectory, null, sb.ToString(), 2));
            }

            // set video mux handling
            if (chkDontEncodeVideo.Checked)
            {
                if (dpp.Container != ContainerType.MKV)
                    _oLog.LogEvent("\"Don't encode video\" has been disabled as at the moment only the target container MKV is supported");
                else if (inputContainer != ContainerType.MKV)
                    _oLog.LogEvent("\"Don't encode video\" has been disabled as at the moment only the source container MKV is supported");
                else
                    dpp.VideoFileToMux = dpp.VideoInput;
            }

            // set oneclick job settings
            if (String.IsNullOrEmpty(dpp.VideoFileToMux))
            {
                dpp.AutoDeinterlace = autoDeint.Checked;
                dpp.KeepInputResolution = keepInputResolution.Checked;
                dpp.OutputSize = fileSize.Value;
                dpp.PrerenderJob = addPrerenderJob.Checked;
                dpp.UseChaptersMarks = usechaptersmarks.Checked;
                dpp.SignalAR = signalAR.Checked;
            }
            else
            {
                dpp.AutoDeinterlace = dpp.PrerenderJob = dpp.UseChaptersMarks = false;
                dpp.KeepInputResolution = dpp.PrerenderJob = dpp.UseChaptersMarks = false;
                dpp.OutputSize = null;
                dpp.SignalAR = false;
            }

            if (keepInputResolution.Checked || !String.IsNullOrEmpty(dpp.VideoFileToMux))
                dpp.HorizontalOutputResolution = 0;
            else
                dpp.HorizontalOutputResolution = (int)horizontalResolution.Value;

            // create pgcdemux job if needed
            if (Path.GetExtension(_videoInputInfo.FileName.ToUpper(System.Globalization.CultureInfo.InvariantCulture)) == ".VOB")
            {
                string videoIFO;
                // PGC numbers are not present in VOB, so we check the main IFO
                if (Path.GetFileName(_videoInputInfo.FileName).ToUpper(System.Globalization.CultureInfo.InvariantCulture).Substring(0, 4) == "VTS_")
                    videoIFO = _videoInputInfo.FileName.Substring(0, _videoInputInfo.FileName.LastIndexOf("_")) + "_0.IFO";
                else
                    videoIFO = Path.ChangeExtension(_videoInputInfo.FileName, ".IFO");

                if (File.Exists(videoIFO))
                {
                    dpp.IFOInput = videoIFO;
                    if (IFOparser.getPGCnb(videoIFO) > 1)
                    {
                        // more than one PGC - therefore pgcdemux must be used
                        prepareJobs = new SequentialChain(new PgcDemuxJob(videoIFO, dpp.WorkingDirectory, _videoInputInfo.VideoInfo.PGCNumber));
                        for (int i = 1; i < 10; i++)
                            dpp.FilesToDelete.Add(Path.Combine(dpp.WorkingDirectory, "VTS_01_" + i + ".VOB"));
                        dpp.VideoInput = Path.Combine(dpp.WorkingDirectory, "VTS_01_1.VOB");
                    }
                }
            }

            // MKV tracks which need to be extracted
            List<TrackInfo> oExtractMKVTrack = new List<TrackInfo>();

            // get audio information
            JobChain audioJobs = null;
            List<AudioTrackInfo> arrAudioTrackInfo = new List<AudioTrackInfo>();
            foreach (OneClickStreamControl oStreamControl in audioTracks)
            {
                if (oStreamControl.SelectedStreamIndex <= 0) // "None"
                    continue;

                string aInput = null;
                string strLanguage = null;
                string strName = null;
                string strAudioCodec = null;
                bool bExtractMKVTrack = false;
                AudioTrackInfo oAudioTrackInfo = null;
                int delay = oStreamControl.SelectedStream.Delay;
                if (oStreamControl.SelectedItem.IsStandard)
                {
                    oAudioTrackInfo = (AudioTrackInfo)oStreamControl.SelectedStream.TrackInfo;
                    if (dpp.IndexType == FileIndexerWindow.IndexType.AVISOURCE && inputContainer != ContainerType.MKV)
                    {
                        _oLog.LogEvent("Internal audio track " + oAudioTrackInfo.TrackID + " will be skipped as AVISOURCE is going to be used", ImageType.Warning);
                        continue;
                    }

                    arrAudioTrackInfo.Add(oAudioTrackInfo);
                    if (dpp.IndexType != FileIndexerWindow.IndexType.NONE && !dpp.Eac3toDemux)
                        aInput = "::" + oAudioTrackInfo.TrackID + "::";
                    else
                        aInput = Path.Combine(dpp.WorkingDirectory, oAudioTrackInfo.DemuxFileName);
                    strName = oAudioTrackInfo.Name;
                    strLanguage = oAudioTrackInfo.Language;
                    strAudioCodec = oAudioTrackInfo.Codec;
                    if (inputContainer == ContainerType.MKV && !dpp.Eac3toDemux) // only if container MKV and no demux with eac3to
                    {
                        oExtractMKVTrack.Add(oStreamControl.SelectedStream.TrackInfo);
                        bExtractMKVTrack = true;
                    }
                }
                else
                {
                    aInput = oStreamControl.SelectedFile;
                    MediaInfoFile oInfo = new MediaInfoFile(aInput, ref _oLog);
                    if (oInfo.AudioInfo.Tracks.Count > 0)
                        strAudioCodec = oInfo.AudioInfo.Tracks[0].Codec;
                    strName = oStreamControl.SelectedStream.Name;
                    strLanguage = oStreamControl.SelectedStream.Language;
                }

                if (oStreamControl.IsDontEncodePossible() &&
                    (oStreamControl.SelectedStream.EncodingMode == AudioEncodingMode.Never ||
                    (oStreamControl.SelectedStream.EncodingMode == AudioEncodingMode.NeverOnlyCore && dpp.Eac3toDemux) ||
                    (oStreamControl.SelectedStream.EncodingMode == AudioEncodingMode.IfCodecDoesNotMatch &&
                    oStreamControl.SelectedStream.EncoderSettings.EncoderType.ACodec.ID.Equals(strAudioCodec, StringComparison.InvariantCultureIgnoreCase))))
                {
                    dpp.AudioTracks.Add(new OneClickAudioTrack(null, new MuxStream(aInput, strLanguage, strName, delay, false, false, null), oAudioTrackInfo, bExtractMKVTrack));
                }
                else
                {
                    // audio track will be encoded
                    string strFileName = string.Empty;
                    if (!oStreamControl.SelectedItem.IsStandard || !dpp.Eac3toDemux)
                    {
                        if (strAudioCodec.ToLower(System.Globalization.CultureInfo.InvariantCulture).Contains("truehd"))
                        {
                            strAudioCodec = "AC-3";
                            if (oStreamControl.SelectedItem.IsStandard)
                            {
                                strFileName = Path.Combine(strWorkingDirectory, oStreamControl.SelectedStream.TrackInfo.DemuxFileName);
                                strFileName = Path.ChangeExtension(strFileName, "ac3");
                                oAudioTrackInfo.Codec = strAudioCodec;
                                aInput = FileUtil.AddToFileName(strFileName, "_core");
                            }
                            else
                            {
                                strFileName = oStreamControl.SelectedFile;
                                aInput = FileUtil.AddToFileName(Path.ChangeExtension(strFileName, "ac3"), "_core");
                                aInput = Path.Combine(strWorkingDirectory, Path.GetFileName(aInput));
                            }

                            HDStreamsExJob oJob = new HDStreamsExJob(new List<string>() { strFileName }, aInput, null, "\"" + aInput + "\"", 2);
                            audioJobs = new SequentialChain(audioJobs, new SequentialChain(oJob));
                            dpp.FilesToDelete.Add(FileUtil.AddToFileName(Path.ChangeExtension(aInput, "txt"), " - Log"));
                            dpp.FilesToDelete.Add(aInput);
                        }
                        else if (strAudioCodec.StartsWith("DTS-HD", StringComparison.InvariantCultureIgnoreCase))
                        {
                            strAudioCodec = "DTS";
                            if (oStreamControl.SelectedItem.IsStandard)
                            {
                                strFileName = Path.Combine(strWorkingDirectory, oStreamControl.SelectedStream.TrackInfo.DemuxFileName);
                                oAudioTrackInfo.Codec = strAudioCodec;
                                aInput = FileUtil.AddToFileName(Path.ChangeExtension(strFileName, "dts"), "_core");
                            }
                            else
                            {
                                strFileName = oStreamControl.SelectedFile;
                                aInput = FileUtil.AddToFileName(Path.ChangeExtension(strFileName, "dts"), "_core");
                                aInput = Path.Combine(strWorkingDirectory, Path.GetFileName(aInput));
                            }

                            HDStreamsExJob oJob = new HDStreamsExJob(new List<string>() { strFileName }, aInput, null, "\"" + aInput + "\" -core", 2);
                            audioJobs = new SequentialChain(audioJobs, new SequentialChain(oJob));
                            dpp.FilesToDelete.Add(FileUtil.AddToFileName(Path.ChangeExtension(aInput, "txt"), " - Log"));
                            dpp.FilesToDelete.Add(aInput);
                        }
                    }

                    if (oStreamControl.SelectedStream.EncodingMode == AudioEncodingMode.NeverOnlyCore)
                        dpp.AudioTracks.Add(new OneClickAudioTrack(null, new MuxStream(aInput, strLanguage, strName, delay, false, false, null), oAudioTrackInfo, bExtractMKVTrack));
                    else
                        dpp.AudioTracks.Add(new OneClickAudioTrack(new AudioJob(aInput, null, null, oStreamControl.SelectedStream.EncoderSettings, delay, strLanguage, strName), null, oAudioTrackInfo, bExtractMKVTrack));
                }
            }


            // subtitle handling
            foreach (OneClickStreamControl oStream in subtitleTracks)
            {
                if (oStream.SelectedStreamIndex <= 0) // not NONE
                    continue;

                if (oStream.SelectedItem.IsStandard)
                {
                    string strExtension = Path.GetExtension(oStream.SelectedStream.TrackInfo.SourceFileName.ToLower(System.Globalization.CultureInfo.InvariantCulture));
                    if (strExtension.Equals(".ifo") || strExtension.Equals(".vob"))
                    {
                        string strInput = oStream.SelectedStream.TrackInfo.SourceFileName;
                        if (strExtension.Equals(".vob"))
                        {
                            if (Path.GetFileName(strInput).ToUpper(System.Globalization.CultureInfo.InvariantCulture).Substring(0, 4) == "VTS_")
                                strInput = strInput.Substring(0, strInput.LastIndexOf("_")) + "_0.IFO";
                            else
                                strInput = Path.ChangeExtension(strInput, ".IFO");
                        }
                        string outputFile = Path.Combine(dpp.WorkingDirectory, Path.GetFileNameWithoutExtension(strInput)) + "_" + oStream.SelectedStream.TrackInfo.MMGTrackID + ".idx";
                        SubtitleIndexJob oJob = new SubtitleIndexJob(strInput, outputFile, false, new List<int> { oStream.SelectedStream.TrackInfo.MMGTrackID }, _videoInputInfo.VideoInfo.PGCNumber);
                        prepareJobs = new SequentialChain(new SequentialChain(prepareJobs), new SequentialChain(oJob));
                        oStream.SelectedStream.DemuxFilePath = outputFile;
                        dpp.FilesToDelete.Add(outputFile);
                        dpp.FilesToDelete.Add(Path.ChangeExtension(outputFile, ".sub"));
                        dpp.SubtitleTracks.Add(oStream.SelectedStream);
                    }
                    else if (inputContainer == ContainerType.MKV && !dpp.Eac3toDemux) // only if container MKV and no demux with eac3to
                    {
                        oExtractMKVTrack.Add(oStream.SelectedStream.TrackInfo);
                        dpp.SubtitleTracks.Add(oStream.SelectedStream);
                    }
                }
                else
                    dpp.SubtitleTracks.Add(oStream.SelectedStream);
            }


            // create MKV extract job if required
            if (oExtractMKVTrack.Count > 0)
            {
                MkvExtractJob extractJob = new MkvExtractJob(dpp.VideoInput, dpp.WorkingDirectory, oExtractMKVTrack);
                prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(extractJob));
            }


            // add audio job to chain if required
            prepareJobs = new SequentialChain(prepareJobs, new SequentialChain(audioJobs));

            JobChain finalJobChain = null;
            if (dpp.IndexType == FileIndexerWindow.IndexType.D2V || dpp.IndexType == FileIndexerWindow.IndexType.DGA)
            {   
                string indexFile = string.Empty;
                IndexJob job = null;
                if (dpp.IndexType == FileIndexerWindow.IndexType.D2V)
                {
                    indexFile = Path.Combine(dpp.WorkingDirectory, workingName.Text + ".d2v");
                    job = new D2VIndexJob(dpp.VideoInput, indexFile, 2, arrAudioTrackInfo, false, false);
                }
                else
                {
                    indexFile = Path.Combine(dpp.WorkingDirectory, workingName.Text + ".dga");
                    job = new DGAIndexJob(dpp.VideoInput, indexFile, 2, arrAudioTrackInfo, false, false);
                }
                OneClickPostProcessingJob ocJob = new OneClickPostProcessingJob(dpp.VideoInput, indexFile, dpp);
                finalJobChain = new SequentialChain(prepareJobs, new SequentialChain(job), new SequentialChain(ocJob));
            }
            else if (dpp.IndexType == FileIndexerWindow.IndexType.DGI || dpp.IndexType == FileIndexerWindow.IndexType.FFMS)
            {
                string indexFile = string.Empty;
                if (dpp.IndexType == FileIndexerWindow.IndexType.DGI)
                    indexFile = Path.Combine(dpp.WorkingDirectory, workingName.Text + ".dgi");
                else
                    indexFile = Path.Combine(dpp.WorkingDirectory, Path.GetFileName(dpp.VideoInput) + ".ffindex");
                OneClickPostProcessingJob ocJob = new OneClickPostProcessingJob(dpp.VideoInput, indexFile, dpp);

                IndexJob job = null;
                if (inputContainer == ContainerType.MKV)
                {
                    if (dpp.IndexType == FileIndexerWindow.IndexType.DGI)
                        job = new DGIIndexJob(dpp.VideoInput, indexFile, 0, null, false, false);
                    else
                        job = new FFMSIndexJob(dpp.VideoInput, indexFile, 0, null, false);
                    if (!String.IsNullOrEmpty(dpp.VideoFileToMux) && dpp.Container == ContainerType.MKV)
                    {
                        finalJobChain = new SequentialChain(prepareJobs, new SequentialChain(ocJob));
                        dpp.IndexType = FileIndexerWindow.IndexType.NONE;
                    }
                    else
                        finalJobChain = new SequentialChain(prepareJobs, new SequentialChain(job), new SequentialChain(ocJob));
                }
                else
                {
                    if (dpp.IndexType == FileIndexerWindow.IndexType.DGI)
                        job = new DGIIndexJob(dpp.VideoInput, indexFile, 2, arrAudioTrackInfo, false, false);
                    else
                        job = new FFMSIndexJob(dpp.VideoInput, indexFile, 2, arrAudioTrackInfo, false);
                    finalJobChain = new SequentialChain(prepareJobs, new SequentialChain(job), new SequentialChain(ocJob));
                }
            }
            else
            {
                // no indexer
                if (inputContainer == ContainerType.MKV && dpp.IndexType == FileIndexerWindow.IndexType.AVISOURCE && String.IsNullOrEmpty(dpp.VideoFileToMux))
                    dpp.VideoInput = _videoInputInfo.FileName;
                OneClickPostProcessingJob ocJob = new OneClickPostProcessingJob(dpp.VideoInput, null, dpp);
                finalJobChain = new SequentialChain(prepareJobs, new SequentialChain(ocJob));
            }


            // write all to be processed tracks into the log
            _oLog.LogEvent("Video: " + _videoInputInfo.FileName);
            foreach (OneClickAudioTrack oTrack in dpp.AudioTracks)
            {
                if (oTrack.AudioTrackInfo != null)
                    _oLog.LogEvent("Audio: " + oTrack.AudioTrackInfo.SourceFileName + " (" + oTrack.AudioTrackInfo.ToString() + ")");
                else if (oTrack.AudioJob != null)
                    _oLog.LogEvent("Audio: " + oTrack.AudioJob.Input);
            }
            foreach (OneClickStream oTrack in dpp.SubtitleTracks)
            {
                if (oTrack.TrackInfo != null)
                    _oLog.LogEvent("Subtitle: " + oTrack.TrackInfo.SourceFileName + " (" + oTrack.TrackInfo.ToString() + ")");
            }
            
            // add jobs to queue
            mainForm.Jobs.addJobsWithDependencies(finalJobChain, !bBatchProcessing);

            if (this.openOnQueue.Checked && !bAutomatedProcessing)
            {
                if (!string.IsNullOrEmpty(this.chapterFile.Filename))
                    this.chapterFile.Filename = string.Empty; // clean up
                tabControl1.SelectedTab = tabControl1.TabPages[0];
                input.SelectedIndex = 0;
                goButton.Enabled = true;
            }
            else
                this.Close();
        }

        private bool verifyInputSettings(MediaInfoFile oVideoInputInfo, string strWorkingDirectory)
        {
            if (oVideoInputInfo == null)
            {
                MessageBox.Show("Please select a valid input file!", "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (String.IsNullOrEmpty(output.Filename) || !File.Exists(oVideoInputInfo.FileName))
            {
                MessageBox.Show("Please select valid input and output file!", "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (!FileUtil.IsDirWriteable(Path.GetDirectoryName(output.Filename)))
            {
                MessageBox.Show("MeGUI cannot write on the disc " + Path.GetDirectoryName(output.Filename) + " \n" +
                                 "Please, select another output path to save your project...", "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (!FileUtil.IsDirWriteable(strWorkingDirectory))
            {
                MessageBox.Show("MeGUI cannot write on the disc " + strWorkingDirectory + " \n" +
                                 "Please, select another working path to save your project...", "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if ((verifyStreamSettings() != null) || (VideoSettings == null) || string.IsNullOrEmpty(workingName.Text))
            {
                MessageBox.Show("MeGUI cannot process this job", "Wrong configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            for (int i = 0; i < audioTracks.Count - 1; i++)
            {
                if (audioTracks[i].SelectedStreamIndex <= 0) // not NONE
                    continue;

                for (int j = i + 1; j < audioTracks.Count; j++)
                {
                    if (audioTracks[j].SelectedStreamIndex <= 0) // not NONE
                        continue;

                    // compare the two controls
                    if (audioTracks[i].SelectedStream.DemuxFilePath.Equals(audioTracks[j].SelectedStream.DemuxFilePath) &&
                        audioTracks[i].SelectedStream.Language.Equals(audioTracks[j].SelectedStream.Language) &&
                        audioTracks[i].SelectedStream.Name.Equals(audioTracks[j].SelectedStream.Name) &&
                        audioTracks[i].SelectedStream.DefaultStream == audioTracks[j].SelectedStream.DefaultStream &&
                        audioTracks[i].SelectedStream.Delay == audioTracks[j].SelectedStream.Delay &&
                        audioTracks[i].SelectedStream.EncoderSettings.Equals(audioTracks[j].SelectedStream.EncoderSettings) &&
                        audioTracks[i].SelectedStream.EncodingMode == audioTracks[j].SelectedStream.EncodingMode &&
                        audioTracks[i].SelectedStream.ForcedStream == audioTracks[j].SelectedStream.ForcedStream)
                    {
                        DialogResult dr = MessageBox.Show("The audio tracks " + (i + 1) + " and " + (j + 1) + " are identical. Are you sure you want to proceed?", "Duplicate audio tracks found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == System.Windows.Forms.DialogResult.No)
                            return false;
                    }  
                }
            }

            for (int i = 0; i < subtitleTracks.Count - 1; i++)
            {
                if (subtitleTracks[i].SelectedStreamIndex <= 0) // not NONE
                    continue;

                for (int j = i + 1; j < subtitleTracks.Count; j++)
                {
                    if (subtitleTracks[j].SelectedStreamIndex <= 0) // not NONE
                        continue;

                    // compare the two controls
                    if (subtitleTracks[i].SelectedStream.DemuxFilePath.Equals(subtitleTracks[j].SelectedStream.DemuxFilePath) &&
                        subtitleTracks[i].SelectedStream.Language.Equals(subtitleTracks[j].SelectedStream.Language) &&
                        subtitleTracks[i].SelectedStream.Name.Equals(subtitleTracks[j].SelectedStream.Name) &&
                        subtitleTracks[i].SelectedStream.DefaultStream == subtitleTracks[j].SelectedStream.DefaultStream &&
                        subtitleTracks[i].SelectedStream.Delay == subtitleTracks[j].SelectedStream.Delay &&
                        subtitleTracks[i].SelectedStream.EncoderSettings.Equals(subtitleTracks[j].SelectedStream.EncoderSettings) &&
                        subtitleTracks[i].SelectedStream.EncodingMode == subtitleTracks[j].SelectedStream.EncodingMode &&
                        subtitleTracks[i].SelectedStream.ForcedStream == subtitleTracks[j].SelectedStream.ForcedStream)
                    {
                        DialogResult dr = MessageBox.Show("The subtitle tracks " + (i + 1) + " and " + (j + 1) + " are identical. Are you sure you want to proceed?", "Duplicate subtitle tracks found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == System.Windows.Forms.DialogResult.No)
                            return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region profile management

        private string verifyStreamSettings()
        {
            for (int i = 0; i < audioTracks.Count; ++i)
            {
                if (audioTracks[i].SelectedItem.IsStandard || audioTracks[i].SelectedStreamIndex <= 0)
                    continue;

                string r = MainForm.verifyInputFile(audioTracks[i].SelectedFile);
                if (r != null) 
                    return r;
            }
            for (int i = 0; i < subtitleTracks.Count; ++i)
            {
                if (subtitleTracks[i].SelectedItem.IsStandard || subtitleTracks[i].SelectedStreamIndex <= 0)
                    continue;

                string r = MainForm.verifyInputFile(subtitleTracks[i].SelectedFile);
                if (r != null) 
                    return r;
            }
            return null;
        }
        #endregion
        
        private void audio1_SomethingChanged(object sender, EventArgs e)
        {
            updatePossibleContainers();
        }

        void ProfileChanged(object sender, EventArgs e)
        {
            if (videoProfile.SelectedProfile.FQName.StartsWith("x264") && !chkDontEncodeVideo.Checked)
                usechaptersmarks.Enabled = true;
            else
                usechaptersmarks.Enabled = false;
            updatePossibleContainers();
        }

        private void keepInputResolution_CheckedChanged(object sender, EventArgs e)
        {
            if (keepInputResolution.Checked)
            {
                horizontalResolution.Enabled = autoCrop.Enabled = signalAR.Enabled = false;
                signalAR.Checked = true;
            }
            else
                horizontalResolution.Enabled = autoCrop.Enabled = signalAR.Enabled = true;
        }

        private void chkDontEncodeVideo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDontEncodeVideo.Checked)
            {
                horizontalResolution.Enabled = autoCrop.Enabled = signalAR.Enabled = videoProfile.Enabled = false;
                usechaptersmarks.Enabled = keepInputResolution.Enabled = addPrerenderJob.Enabled = false;
                autoDeint.Enabled = fileSize.Enabled = avsProfile.Enabled = ar.Enabled = false;
            }
            else
            {
                videoProfile.Enabled = keepInputResolution.Enabled = addPrerenderJob.Enabled = true;
                autoDeint.Enabled = fileSize.Enabled = avsProfile.Enabled = true;
                if (videoProfile.SelectedProfile.FQName.StartsWith("x264"))
                    usechaptersmarks.Enabled = true;
                else
                    usechaptersmarks.Enabled = false;
                if (keepInputResolution.Checked)
                    horizontalResolution.Enabled = autoCrop.Enabled = signalAR.Enabled = ar.Enabled = false;
                else
                    horizontalResolution.Enabled = autoCrop.Enabled = signalAR.Enabled = ar.Enabled = true;
            }
        }


        // Subtitle Track Handling
        private void subtitleMenu_Opening(object sender, CancelEventArgs e)
        {
            subtitleRemoveTrack.Enabled = (iSelectedSubtitleTabPage != subtitleTracks.Count);
        }

        private void subtitleAddTrack_Click(object sender, EventArgs e)
        {
            SubtitleAddTrack(true);
        }

        private void subtitleRemoveTrack_Click(object sender, EventArgs e)
        {
            SubtitleRemoveTrack(iSelectedSubtitleTabPage);
        }

        private void SubtitleAddTrack(bool bChangeFocus)
        {
            TabPage p = new TabPage("Subtitle " + (subtitleTracks.Count + 1));
            p.UseVisualStyleBackColor = subtitlesTab.TabPages[0].UseVisualStyleBackColor;
            p.Padding = subtitlesTab.TabPages[0].Padding;

            OneClickStreamControl a = new OneClickStreamControl();
            a.Dock = subtitleTracks[0].Dock;
            a.Padding = subtitleTracks[0].Padding;
            a.ShowDelay = subtitleTracks[0].ShowDelay;
            a.ShowDefaultStream = subtitleTracks[0].ShowDefaultStream;
            a.ShowForceStream = subtitleTracks[0].ShowForceStream;
            a.chkDefaultStream.CheckedChanged += new System.EventHandler(this.chkDefaultStream_CheckedChanged);
            a.SomethingChanged += new EventHandler(audio1_SomethingChanged);
            a.Filter = subtitleTracks[0].Filter;
            a.FileUpdated += oneClickSubtitleStreamControl_FileUpdated;
            a.StandardStreams = subtitleTracks[0].StandardStreams;
            a.CustomStreams = subtitleTracks[0].CustomStreams;
            a.SelectedStreamIndex = 0;
            a.initProfileHandler();
            if (this.Visible)
                a.enableDragDrop();

            subtitlesTab.TabPages.Insert(subtitlesTab.TabCount - 1, p);
            p.Controls.Add(a);
            subtitleTracks.Add(a);

            if (bChangeFocus)
                subtitlesTab.SelectedTab = p;
        }

        private void SubtitleRemoveTrack(int iTabPageIndex)
        {
            if (iTabPageIndex == subtitlesTab.TabCount - 1)
                return;

            if (iTabPageIndex == 0 && subtitlesTab.TabCount == 1)
                SubtitleAddTrack(true);

            subtitlesTab.TabPages.RemoveAt(iTabPageIndex);
            subtitleTracks.RemoveAt(iTabPageIndex);

            for (int i = 0; i < subtitlesTab.TabCount - 1; i++)
                subtitlesTab.TabPages[i].Text = "Subtitle " + (i + 1);

            updatePossibleContainers();
        }

        private void chkDefaultStream_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == false)
                return;

            foreach (OneClickStreamControl oTrack in subtitleTracks)
            {
                if (sender != oTrack.chkDefaultStream && oTrack.chkDefaultStream.Checked == true)
                    oTrack.chkDefaultStream.Checked = false;
            }
        }

        private int iSelectedSubtitleTabPage = -1;
        private void subtitlesTab_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            Point p = e.Location;
            for (int i = 0; i < subtitlesTab.TabCount; i++)
            {
                Rectangle rect = subtitlesTab.GetTabRect(i);
                rect.Offset(2, 2);
                rect.Width -= 4;
                rect.Height -= 4;
                if (rect.Contains(p))
                {
                    iSelectedSubtitleTabPage = i;
                    subtitleMenu.Show(subtitlesTab, e.Location);
                    break;
                }
            }
        }

        private void oneClickSubtitleStreamControl_FileUpdated(object sender, EventArgs e)
        {
            if (bLock)
                return;

            int i = subtitleTracks.IndexOf((OneClickStreamControl)sender);

            if (i < 0)
                return;

            OneClickStreamControl track = subtitleTracks[i];
            foreach (OneClickStreamControl oControl in subtitleTracks)
            {
                if (oControl == track)
                    continue;

                if (oControl.CustomStreams.Length != track.CustomStreams.Length)
                {
                    int iIndex = -1;
                    if (!track.SelectedItem.IsStandard)
                        iIndex = oControl.SelectedStreamIndex;
                    bLock = true;
                    oControl.CustomStreams = track.CustomStreams;
                    bLock = false;
                    if (iIndex >= 0 && oControl.SelectedStreamIndex != iIndex)
                        oControl.SelectedStreamIndex = iIndex;
                }
            }

            updatePossibleContainers();
        }

        private void SubtitleResetTrack(List<OneClickStream> arrSubtitleTrackInfo, OneClickSettings settings)
        {
            // generate track names
            List<object> trackNames = new List<object>();
            trackNames.Add("None");
            foreach (object o in arrSubtitleTrackInfo)
                trackNames.Add(o);
            subtitleTracks[0].StandardStreams = trackNames.ToArray();
            subtitleTracks[0].CustomStreams = new object[0];
            subtitleTracks[0].SelectedStreamIndex = 0;

            // delete all tracks beside the first and last one
            for (int i = subtitlesTab.TabCount - 1; i > 1; i--)
            {
                subtitlesTab.TabPages.RemoveAt(i - 1);
                subtitleTracks.RemoveAt(i - 1);
            }

            foreach (string strLanguage in settings.DefaultSubtitleLanguage)
                if (strLanguage.Equals("[none]"))
                    return;

            int iCounter = 0;
            foreach (string strLanguage in settings.DefaultSubtitleLanguage)
            {
                for (int i = 0; i < arrSubtitleTrackInfo.Count; i++)
                {
                    if (arrSubtitleTrackInfo[i].Language.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(strLanguage.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                    {
                        if (iCounter > 0)
                            SubtitleAddTrack(false);
                        subtitleTracks[iCounter++].SelectedStreamIndex = i + 1;
                    }
                }
            }

            if (iCounter == 0 && arrSubtitleTrackInfo.Count > 0 && !settings.UseNoLanguagesAsFallback)
            {
                for (int i = 0; i < arrSubtitleTrackInfo.Count; i++)
                {
                    if (iCounter > 0)
                        SubtitleAddTrack(false);
                    subtitleTracks[iCounter++].SelectedStreamIndex = i + 1;
                }
            }
        }

        private void subtitlesTab_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SubtitleAddTrack(true);
        }


        // Audio Track Handling
        private void audioMenu_Opening(object sender, CancelEventArgs e)
        {
            audioRemoveTrack.Enabled = (iSelectedAudioTabPage != audioTracks.Count);
        }

        private void audioAddTrack_Click(object sender, EventArgs e)
        {
           AudioAddTrack(true);
        }

        private void audioRemoveTrack_Click(object sender, EventArgs e)
        {
            AudioRemoveTrack(iSelectedAudioTabPage);
        }

        private void AudioAddTrack(bool bChangeFocus)
        {
            TabPage p = new TabPage("Audio " + (audioTracks.Count + 1));
            p.UseVisualStyleBackColor = audioTab.TabPages[0].UseVisualStyleBackColor;
            p.Padding = audioTab.TabPages[0].Padding;

            OneClickStreamControl a = new OneClickStreamControl();
            a.Dock = audioTracks[0].Dock;
            a.Padding = audioTracks[0].Padding;
            a.ShowDelay = audioTracks[0].ShowDelay;
            a.ShowDefaultStream = audioTracks[0].ShowDefaultStream;
            a.ShowForceStream = audioTracks[0].ShowForceStream;
            a.Filter = audioTracks[0].Filter;
            a.FileUpdated += oneClickAudioStreamControl_FileUpdated;
            a.StandardStreams = audioTracks[0].StandardStreams;
            a.CustomStreams = audioTracks[0].CustomStreams;
            a.SelectedStreamIndex = 0;
            a.SomethingChanged += new EventHandler(audio1_SomethingChanged);
            a.EncodingMode = audioTracks[0].EncodingMode;
            a.initProfileHandler();
            a.SelectProfileNameOrWarn(audioTracks[0].EncoderProfile);
            if (this.Visible)
                a.enableDragDrop();

            audioTab.TabPages.Insert(audioTab.TabCount - 1, p);
            p.Controls.Add(a);
            audioTracks.Add(a);

            if (bChangeFocus)
                audioTab.SelectedTab = p;
        }

        private void AudioRemoveTrack(int iTabPageIndex)
        {
            if (iTabPageIndex == audioTab.TabCount - 1)
                return;

            if (iTabPageIndex == 0 && subtitlesTab.TabCount == 1)
                AudioAddTrack(true);

            audioTab.TabPages.RemoveAt(iTabPageIndex);
            audioTracks.RemoveAt(iTabPageIndex);

            for (int i = 0; i < audioTab.TabCount - 1; i++)
                audioTab.TabPages[i].Text = "Audio " + (i + 1);

            updatePossibleContainers();
        }

        private int iSelectedAudioTabPage = -1;
        private void audioTab_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            Point p = e.Location;
            for (int i = 0; i < audioTab.TabCount; i++)
            {
                Rectangle rect = audioTab.GetTabRect(i);
                rect.Offset(2, 2);
                rect.Width -= 4;
                rect.Height -= 4;
                if (rect.Contains(p))
                {
                    iSelectedAudioTabPage = i;
                    audioMenu.Show(audioTab, e.Location);
                    break;
                }
            }
        }

        private void oneClickAudioStreamControl_FileUpdated(object sender, EventArgs e)
        {
            if (bLock)
                return;
            
            int i = audioTracks.IndexOf((OneClickStreamControl)sender);

            if (i < 0)
                return;

            OneClickStreamControl track = audioTracks[i];
            if (!track.SelectedItem.IsStandard)
                track.SelectedStream.Delay = PrettyFormatting.getDelayAndCheck(track.SelectedStream.DemuxFilePath) ?? 0;
            if (_videoInputInfo != null && _videoInputInfo.IndexerToUse == FileIndexerWindow.IndexType.FFMS 
                && (_videoInputInfo.ContainerFileType == ContainerType.M2TS || _videoInputInfo.ContainerFileType == null)
                && !_videoInputInfo.isEac3toDemuxable() && track.SelectedItem.IsStandard)
                audioTracks[i].DisableDontEncode(true);
            else
                audioTracks[i].DisableDontEncode(false);

            foreach (OneClickStreamControl oControl in audioTracks)
            {
                if (oControl == track)
                    continue;

                if (oControl.CustomStreams.Length != track.CustomStreams.Length)
                {
                    int iIndex = -1;
                    if (!track.SelectedItem.IsStandard)
                        iIndex = oControl.SelectedStreamIndex;
                    bLock = true;
                    oControl.CustomStreams = track.CustomStreams;
                    bLock = false;
                    if (iIndex >= 0 && oControl.SelectedStreamIndex != iIndex)
                        oControl.SelectedStreamIndex = iIndex;
                }
            }

            updatePossibleContainers();
        }

        private void AudioResetTrack(List<OneClickStream> arrAudioTrackInfo, OneClickSettings settings)
        {
            // generate track names
            List<object> trackNames = new List<object>();
            trackNames.Add("None");
            foreach (OneClickStream o in arrAudioTrackInfo)
                trackNames.Add(o);
            audioTracks[0].StandardStreams = trackNames.ToArray();
            audioTracks[0].CustomStreams = new object[0];
            audioTracks[0].SelectedStreamIndex = 0;

            // delete all tracks beside the first and last one
            try
            {
                while (audioTab.TabCount > 2)
                    audioTab.TabPages.RemoveAt(1);
            }
            catch (Exception) {}
            audioTracks.RemoveRange(1, audioTracks.Count - 1);

            foreach (string strLanguage in settings.DefaultAudioLanguage)
                if (strLanguage.Equals("[none]"))
                    return;

            int iCounter = 0;
            foreach (string strLanguage in settings.DefaultAudioLanguage)
            {
                for (int i = 0; i < arrAudioTrackInfo.Count; i++)
                {
                    if (arrAudioTrackInfo[i].Language.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(strLanguage.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                    {
                        // should only the first audio track for this language be processed?
                        bool bUseFirstTrackOnly = settings.AudioSettings[0].UseFirstTrackOnly;
                        foreach (OneClickAudioSettings oAudioSettings in settings.AudioSettings)
                        {
                            if (arrAudioTrackInfo[i].Language.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(oAudioSettings.Language.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                            {
                                bUseFirstTrackOnly = oAudioSettings.UseFirstTrackOnly;
                                break;
                            }
                        }

                        bool bAddTrack = true;
                        if (bUseFirstTrackOnly)
                        {
                            foreach (OneClickStreamControl oAudioControl in audioTracks)
                            {
                                if (oAudioControl.SelectedStreamIndex > 0 &&
                                    oAudioControl.SelectedStream.Language.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(arrAudioTrackInfo[i].Language.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                                {
                                    bAddTrack = false;
                                    break;
                                }
                            }
                        }

                        if (!bAddTrack)
                            break;

                        if (iCounter > 0)
                            AudioAddTrack(false);
                        audioTracks[iCounter++].SelectedStreamIndex = i + 1;
                    }
                }
            }

            if (iCounter == 0 && arrAudioTrackInfo.Count > 0 && !settings.UseNoLanguagesAsFallback)
            {
                for (int i = 0; i < arrAudioTrackInfo.Count; i++)
                {
                    // should only the first audio track for this language be processed?
                    bool bUseFirstTrackOnly = settings.AudioSettings[0].UseFirstTrackOnly;
                    foreach (OneClickAudioSettings oAudioSettings in settings.AudioSettings)
                    {
                        if (arrAudioTrackInfo[i].Language.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(oAudioSettings.Language.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                        {
                            bUseFirstTrackOnly = oAudioSettings.UseFirstTrackOnly;
                            break;
                        }
                    }

                    bool bAddTrack = true;
                    if (bUseFirstTrackOnly)
                    {
                        foreach (OneClickStreamControl oAudioControl in audioTracks)
                        {
                            if (oAudioControl.SelectedStreamIndex > 0 &&
                                oAudioControl.SelectedStream.Language.ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(arrAudioTrackInfo[i].Language.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                            {
                                bAddTrack = false;
                                break;
                            }
                        }
                    }

                    if (!bAddTrack)
                        break;

                    if (iCounter > 0)
                        AudioAddTrack(false);
                    audioTracks[iCounter++].SelectedStreamIndex = i + 1;
                }
            }

            ResetAudioSettings(settings);
        }

        private void ResetAudioSettings(OneClickSettings settings)
        {
            foreach (OneClickStreamControl a in audioTracks)
            {
                bool bFound = false;
                for (int i = 1; i < settings.AudioSettings.Count; i++)
                {
                    if (a.SelectedStream.Language.Equals(settings.AudioSettings[i].Language))
                    {
                        a.SelectProfileNameOrWarn(settings.AudioSettings[i].Profile);
                        if (a.IsDontEncodePossible() == true)
                            a.EncodingMode = settings.AudioSettings[i].AudioEncodingMode;
                        bFound = true;
                        break;
                    }
                }
                if (!bFound)
                {
                    if (settings.AudioSettings.Count > 0)
                    {
                        a.SelectProfileNameOrWarn(settings.AudioSettings[0].Profile);
                        if (a.IsDontEncodePossible() == true)
                            a.EncodingMode = settings.AudioSettings[0].AudioEncodingMode;
                    }
                    else
                        a.EncodingMode = AudioEncodingMode.IfCodecDoesNotMatch;
                }
            }
        }

        private void audioTab_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AudioAddTrack(true);
        }

        private void OneClickWindow_Shown(object sender, EventArgs e)
        {
            oneClickAudioStreamControl1.enableDragDrop();
            oneClickSubtitleStreamControl1.enableDragDrop();
        }

        private void audioTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (audioTab.SelectedTab.Text.Equals("   +"))
                AudioAddTrack(true);
        }

        private void audioTab_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                AudioRemoveTrack(audioTab.SelectedIndex);
        }

        private void subtitlesTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (subtitlesTab.SelectedTab.Text.Equals("   +"))
                SubtitleAddTrack(true);
        }

        private void subtitlesTab_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                SubtitleRemoveTrack(audioTab.SelectedIndex);
        }
    }

    public class OneClickFilesToProcess
    {
        public string FilePath;
        public int TrackNumber;

        public OneClickFilesToProcess() : this(string.Empty, 1)
        {

        }

        public OneClickFilesToProcess(string strPath, int iNumber)
        {
            FilePath = strPath;
            TrackNumber = iNumber;
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
            OneClickWindow ocmt = new OneClickWindow(info);
            ocmt.Show();
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlF1 }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "one_click"; }
        }

        #endregion

    }
}
