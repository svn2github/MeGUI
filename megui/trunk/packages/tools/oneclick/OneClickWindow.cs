// ****************************************************************************
// 
// Copyright (C) 2005-2011  Doom9 & al
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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.details.video;
using MeGUI.core.gui;
using MeGUI.core.plugins.interfaces;
using MeGUI.core.util;
using MeGUI.packages.tools.oneclick;

namespace MeGUI
{

//    public class OneClickPostProcessor 
    public partial class OneClickWindow : Form
    {
        List<FileSCBox> audioTrack;
        List<Label> trackLabel;
        List<AudioConfigControl> audioConfigControl;
        FileIndexerWindow.IndexType oIndexerToUse;
        LogItem _oLog;

        #region profiles
        void ProfileChanged(object sender, EventArgs e)
        {
            if (VideoSettings.EncoderType.ID == "x264")
                usechaptersmarks.Enabled = true;
            else
            {
                usechaptersmarks.Enabled = false;
                usechaptersmarks.Checked = usechaptersmarks.Enabled;
            }            
            updatePossibleContainers();
        }

        #region OneClick profiles
        private void initOneClickHandler()
        {
            oneclickProfile.Manager = mainForm.Profiles;
        }

        private void initTabs()
        {
            audioTrack = new List<FileSCBox>();
            audioTrack.Add(audioTrack1);
            audioTrack.Add(audioTrack2);

            trackLabel = new List<Label>();
            trackLabel.Add(track1Label);
            trackLabel.Add(track2Label);

            audioConfigControl = new List<AudioConfigControl>();
            audioConfigControl.Add(audio1);
            audioConfigControl.Add(audio2);
        }

        void OneClickProfileChanged(object sender, EventArgs e)
        {
            this.Settings = (OneClickSettings)oneclickProfile.SelectedProfile.BaseSettings;
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
        private MkvInfo oMkvInfo;
        
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

            initTabs();

            videoProfile.Manager = mainForm.Profiles;
            initAudioHandler();
            avsProfile.Manager = mainForm.Profiles;
            initOneClickHandler();

            audioTrack1.StandardItems = audioTrack2.StandardItems = new object[] { "None" };
            audioTrack1.SelectedIndex = audioTrack2.SelectedIndex = 0;

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

            showAdvancedOptions_CheckedChanged(null, null);

            if (VideoUtil.isDGIIndexerAvailable())
            {
                input.Filter = "All DGAVCIndex supported files|*.264;*.h264;*.avc;*.m2t*;*.m2ts;*.mts;*.tp;*.ts;*.trp|All DGIndex supported files|*.vob;*.mpg;*.mpeg;*.m1v;*.m2v;*.mpv;*.tp;*.ts;*.trp;*.m2t;*.m2ts;*.pva;*.vro|All DGIndexNV supported files|*.264;*.h264;*.avc;*.m2v;*.mpv;*.vc1;*.mkv;*.vob;*.mpg;*.mpeg;*.m2t;*.m2ts;*.mts;*.tp;*.ts;*.trp|All FFMS Indexer supported files|*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.vob;*.mpg;*.m2ts;*.ts|All supported files|*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.264;*.h264;*.avc;*.m2t*;*.m2ts;*.mts;*.tp;*.ts;*.trp;*.vob;*.mpg;*.mpeg;*.m1v;*.m2v;*.mpv;*.pva;*.vro;*.vc1|All files|*.*";
                input.FilterIndex = 5;
            }
            else
            {
                input.Filter = "All DGAVCIndex supported files|*.264;*.h264;*.avc;*.m2t*;*.m2ts;*.mts;*.tp;*.ts;*.trp|All DGIndex supported files|*.vob;*.mpg;*.mpeg;*.m1v;*.m2v;*.mpv;*.tp;*.ts;*.trp;*.m2t;*.m2ts;*.pva;*.vro|All FFMS Indexer supported files|*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.vob;*.mpg;*.m2ts;*.ts|All supported files|*.mkv;*.avi;*.mp4;*.flv;*.wmv;*.ogm;*.264;*.h264;*.avc;*.m2t*;*.m2ts;*.mts;*.tp;*.ts;*.trp;*.vob;*.mpg;*.mpeg;*.m1v;*.m2v;*.mpv;*.pva;*.vro|All files|*.*";
                input.FilterIndex = 4;
            }
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
        
        public void openInput(string fileName)
        {
            MediaInfoFile iFile = new MediaInfoFile(fileName);
            if (!iFile.recommendIndexer(out oIndexerToUse, false))
            {
                input.Filename = "";
                MessageBox.Show("This file cannot be used in OneClick mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_oLog == null)
                _oLog = mainForm.Log.Info("OneClick");

            // if the input container is MKV get the MkvInfo
            if (iFile.ContainerFileTypeString.ToUpper().Equals("MATROSKA"))
                oMkvInfo = new MkvInfo(input.Filename, ref _oLog);
            else
                oMkvInfo = null;

            input.Filename = fileName;
            Dar? ar = null;
            int maxHorizontalResolution = int.Parse(iFile.Info.Width.ToString());
            
            List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
            if (oMkvInfo != null)
            {
                foreach (MkvInfoTrack oTrack in oMkvInfo.Track)
                    if (oTrack.Type == MkvInfoTrackType.Audio)
                        audioTracks.Add(oTrack.AudioTrackInfo);
            }
            else
                audioTracks = iFile.AudioTracks;
            
            List<object> trackNames = new List<object>();
            trackNames.Add("None");
            foreach (object o in audioTracks)
                trackNames.Add(o);

            foreach (FileSCBox b in audioTrack)
                b.StandardItems = trackNames.ToArray();

            // select primary language
            foreach (AudioTrackInfo ati in audioTracks)
            {
                if (ati.Language.ToLower().Equals(mainForm.Settings.DefaultLanguage1.ToLower()) &&
                    audioTrack1.SelectedIndex == 0)
                {
                    audioTrack1.SelectedObject = ati;
                    break;
                }
            }

            // select secondary language
            if (mainForm.Settings.DefaultLanguage1 != mainForm.Settings.DefaultLanguage2)
            {
                foreach (AudioTrackInfo ati in audioTracks)
                {
                    if (ati.Language.ToLower().Equals(mainForm.Settings.DefaultLanguage2.ToLower()))
                    {
                        if (audioTrack1.SelectedIndex == 0)
                            audioTrack1.SelectedObject = ati;
                        else if (audioTrack2.SelectedIndex == 0)
                            audioTrack2.SelectedObject = ati;
                        break;
                    }
                }
            }

            // If nothing matches DefaultLanguage select 1st track
            if (audioTrack1.SelectedIndex == 0 && audioTracks.Count > 0)
            {
                audioTrack1.SelectedObject = audioTracks[0];
            }
  
            horizontalResolution.Maximum = maxHorizontalResolution;
            
            // Detect Chapters
            if (oMkvInfo != null && oMkvInfo.HasChapters)
                this.chapterFile.Filename = "<internal MKV chapters>";
            else if (Path.GetExtension(input.Filename).ToLower().Equals(".vob"))
                this.chapterFile.Filename = "<internal VOB chapters>";
            else
            {
                string chapterFile = VideoUtil.getChapterFile(fileName);
                if (File.Exists(chapterFile))
                    this.chapterFile.Filename = chapterFile;
            }

            if (string.IsNullOrEmpty(workingDirectory.Filename = mainForm.Settings.DefaultOutputDir))
                workingDirectory.Filename = Path.GetDirectoryName(fileName);

            workingName.Text = PrettyFormatting.ExtractWorkingName(fileName);
            this.updateFilename();
            this.ar.Value = ar;            

            if (VideoSettings.EncoderType.ID == "x264" && this.chapterFile.Filename != null)
                this.usechaptersmarks.Enabled = true; 
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

            for (int i = 0; i < audioConfigControl.Count; ++i)
            {
                if (audioTrack[i].SelectedIndex == 0) // "None"
                    continue;

                if (audioConfigControl[i].Settings != null && !audioConfigControl[i].DontEncode)
                    audioCodecs.Add(audioConfigControl[i].Settings.EncoderType);

                else if (audioConfigControl[i].DontEncode)
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
                VideoSettings.EncoderType, audioCodecs.ToArray(), dictatedOutputTypes.ToArray());

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
                    "No container type could be found that matches the list of acceptable types " +
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
            beingCalled = false;
        }

        private OneClickSettings Settings
        {
            set
            {
                OneClickSettings settings = value;

                foreach (AudioConfigControl a in audioConfigControl)
                    a.SelectProfileNameOrWarn(settings.AudioProfileName);

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

                ignoreRestrictions = false;

                foreach (AudioConfigControl a in audioConfigControl)
                    if (a.IsDontEncodePossible() == true)
                        a.DontEncode = settings.DontEncodeAudio;
                
                // bools
                signalAR.Checked = settings.SignalAR;
                autoDeint.Checked = settings.AutomaticDeinterlacing;
                autoCrop.Checked = settings.AutoCrop;
                keepInputResolution.Checked = settings.KeepInputResolution;
                addPrerenderJob.Checked = settings.PrerenderVideo;
                if (usechaptersmarks.Enabled)
                    usechaptersmarks.Checked = settings.UseChaptersMarks;

                splitting.Value = settings.SplitSize;
                optionalTargetSizeBox1.Value = settings.Filesize;
                if (settings.OutputResolution <= horizontalResolution.Maximum)
                    horizontalResolution.Value = settings.OutputResolution;

                // device type
                devicetype.Text = settings.DeviceOutputType;

                // Clean up after those settings were set
                updatePossibleContainers();
                containerFormat_SelectedIndexChanged(null, null);
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            if (Drives.ableToWriteOnThisDrive(Path.GetPathRoot(output.Filename)) || // check whether the output path is read-only
                Drives.ableToWriteOnThisDrive(Path.GetPathRoot(workingName.Text)))
            {
                if ((verifyAudioSettings() == null)
                    && (VideoSettings != null)
                    && !string.IsNullOrEmpty(input.Filename)
                    && !string.IsNullOrEmpty(workingName.Text))
                {
                    FileSize? desiredSize = optionalTargetSizeBox1.Value;

                    List<AudioJob> aJobs = new List<AudioJob>();
                    List<MuxStream> muxOnlyAudio = new List<MuxStream>();
                    List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
                    for (int i = 0; i < audioConfigControl.Count; ++i)
                    {
                        if (audioTrack[i].SelectedIndex == 0) // "None"
                            continue;

                        string aInput;
                        string strLanguage = null;
                        TrackInfo info = null;
                        int delay = audioConfigControl[i].Delay;
                        if (audioTrack[i].SelectedSCItem.IsStandard)
                        {
                            AudioTrackInfo a = (AudioTrackInfo)audioTrack[i].SelectedObject;
                            audioTracks.Add(a);
                            aInput = "::" + a.TrackID + "::";
                            info = a.TrackInfo;
                            strLanguage = a.Language;
                        }
                        else
                            aInput = audioTrack[i].SelectedText;

                        if (audioConfigControl[i].DontEncode)
                            muxOnlyAudio.Add(new MuxStream(aInput, info, delay, false, false));
                        else
                            aJobs.Add(new AudioJob(aInput, null, null, audioConfigControl[i].Settings, delay, strLanguage));
                    }

                    OneClickPostprocessingProperties dpp = new OneClickPostprocessingProperties();
                    dpp.DAR = ar.Value;
                    dpp.DirectMuxAudio = muxOnlyAudio.ToArray();
                    dpp.AudioJobs = aJobs.ToArray();
                    dpp.AutoDeinterlace = autoDeint.Checked;
                    dpp.AvsSettings = (AviSynthSettings)avsProfile.SelectedProfile.BaseSettings;
                    dpp.Container = (ContainerType)containerFormat.SelectedItem;
                    dpp.FinalOutput = output.Filename;
                    dpp.HorizontalOutputResolution = (int)horizontalResolution.Value;
                    dpp.OutputSize = desiredSize;
                    dpp.SignalAR = signalAR.Checked;
                    dpp.AutoCrop = autoCrop.Checked;
                    dpp.KeepInputResolution = keepInputResolution.Checked;
                    dpp.PrerenderJob = addPrerenderJob.Checked;
                    dpp.Splitting = splitting.Value;
                    dpp.DeviceOutputType = devicetype.Text;
                    dpp.UseChaptersMarks = usechaptersmarks.Checked;
                    dpp.VideoSettings = VideoSettings.Clone();

                    // chapter handling
                    if (!File.Exists(chapterFile.Filename))
                    {
                        if (oMkvInfo != null && oMkvInfo.HasChapters)
                        {
                            string strChapterFile = Path.GetDirectoryName(output.Filename) + "\\" + Path.GetFileNameWithoutExtension(output.Filename) + " - Chapter Information.txt";
                            if (oMkvInfo.extractChapters(strChapterFile))
                            {
                                chapterFile.Filename = strChapterFile;
                                dpp.FilesToDelete.Add(strChapterFile);
                            }
                        }
                        else if (Path.GetExtension(input.Filename).ToLower().Equals(".vob"))
                        {
                            chapterFile.Filename = VideoUtil.getChaptersFromIFO(input.Filename, false);
                            dpp.FilesToDelete.Add(chapterFile.Filename);
                        }
                    }
                    if (!File.Exists(chapterFile.Filename))
                        chapterFile.Filename = "";
                    dpp.ChapterFile = chapterFile.Filename;

                    if (oIndexerToUse == FileIndexerWindow.IndexType.D2V)
                    {
                        string d2vName = Path.Combine(workingDirectory.Filename, workingName.Text + ".d2v");
                        D2VIndexJob job = new D2VIndexJob(input.Filename, d2vName, 2, audioTracks, false, false);
                        OneClickPostProcessingJob ocJob = new OneClickPostProcessingJob(input.Filename, d2vName, dpp, audioTracks, FileIndexerWindow.IndexType.D2V);
                        JobChain c = new SequentialChain(new SequentialChain(job), new SequentialChain(ocJob));
                        mainForm.Jobs.addJobsWithDependencies(c);
                    }
                    else if (oIndexerToUse == FileIndexerWindow.IndexType.DGA)
                    {
                        string d2vName = Path.Combine(workingDirectory.Filename, workingName.Text + ".dga");
                        DGAIndexJob job = new DGAIndexJob(input.Filename, d2vName, 2, audioTracks, false, false);
                        OneClickPostProcessingJob ocJob = new OneClickPostProcessingJob(input.Filename, d2vName, dpp, audioTracks, FileIndexerWindow.IndexType.DGA);
                        JobChain c = new SequentialChain(new SequentialChain(job), new SequentialChain(ocJob));
                        mainForm.Jobs.addJobsWithDependencies(c);
                    }
                    else if (oIndexerToUse == FileIndexerWindow.IndexType.DGI)
                    {
                        string d2vName = Path.Combine(workingDirectory.Filename, workingName.Text + ".dgi");
                        OneClickPostProcessingJob ocJob = new OneClickPostProcessingJob(input.Filename, d2vName, dpp, audioTracks, FileIndexerWindow.IndexType.DGI);

                        DGIIndexJob job;
                        JobChain c;
                        if (oMkvInfo != null)
                        {
                            job = new DGIIndexJob(input.Filename, d2vName, 0, null, false, false);

                            List<MkvInfoTrack> oExtractTrack = new List<MkvInfoTrack>();
                            foreach (AudioTrackInfo oStream in audioTracks)
                            {
                                foreach (MkvInfoTrack oTrack in oMkvInfo.Track)
                                {
                                    if (oTrack.TrackNumber == oStream.TrackID)
                                    {
                                        oExtractTrack.Add(oTrack);
                                        dpp.MkvAudioFiles.Add(oTrack);
                                    }
                                }
                            }
                            if (oExtractTrack.Count > 0)
                            {
                                MkvExtractJob extractJob = new MkvExtractJob(input.Filename, Path.GetDirectoryName(dpp.FinalOutput), oExtractTrack);
                                c = new SequentialChain(new SequentialChain(job), new SequentialChain(extractJob), new SequentialChain(ocJob));
                            }
                            else
                            {
                                c = new SequentialChain(new SequentialChain(job), new SequentialChain(ocJob));
                            }
                        }
                        else
                        {
                            job = new DGIIndexJob(input.Filename, d2vName, 2, audioTracks, false, false);
                            c = new SequentialChain(new SequentialChain(job), new SequentialChain(ocJob));
                        }
                        mainForm.Jobs.addJobsWithDependencies(c);
                    }
                    else if (oIndexerToUse == FileIndexerWindow.IndexType.FFMS)
                    {
                        JobChain c;
                        FFMSIndexJob job;
                        OneClickPostProcessingJob ocJob = new OneClickPostProcessingJob(input.Filename, input.Filename + ".ffindex", dpp, audioTracks, FileIndexerWindow.IndexType.FFMS);
                        if (oMkvInfo != null)
                        {
                            job = new FFMSIndexJob(input.Filename, 0, null, false);

                            List<MkvInfoTrack> oExtractTrack = new List<MkvInfoTrack>();
                            foreach (AudioTrackInfo oStream in audioTracks)
                            {                             
                                foreach (MkvInfoTrack oTrack in oMkvInfo.Track)
                                {
                                    if (oTrack.TrackNumber == oStream.TrackID)
                                    {
                                        oExtractTrack.Add(oTrack);
                                        dpp.MkvAudioFiles.Add(oTrack);
                                    }
                                }
                            }
                            if (oExtractTrack.Count > 0)
                            {
                                MkvExtractJob extractJob = new MkvExtractJob(input.Filename, Path.GetDirectoryName(dpp.FinalOutput), oExtractTrack);
                                c = new SequentialChain(new SequentialChain(job), new SequentialChain(extractJob), new SequentialChain(ocJob));
                            }
                            else
                            {
                                c = new SequentialChain(new SequentialChain(job), new SequentialChain(ocJob));
                            }
                        }
                        else
                        {
                            job = new FFMSIndexJob(input.Filename, 2, audioTracks, false);
                            c = new SequentialChain(new SequentialChain(job), new SequentialChain(ocJob));
                        }
                        mainForm.Jobs.addJobsWithDependencies(c);
                    }
                    if (this.openOnQueue.Checked)
                    {
                        if (!string.IsNullOrEmpty(this.chapterFile.Filename))
                            this.chapterFile.Filename = string.Empty; // clean up
                        tabControl1.SelectedTab = tabControl1.TabPages[0];
                        input.PerformClick();
                    }
                    else
                        this.Close();
                }
                else
                    MessageBox.Show("You must select audio and video profile, output name and working directory to continue",
                        "Incomplete configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else MessageBox.Show("MeGUI cannot write on the disc " + Path.GetPathRoot(output.Filename) + " \n" +
                                 "Please, select another output path to save your project...", "Configuration Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        #endregion

        #region profile management

        private string verifyAudioSettings()
        {
            for (int i = 0; i < audioTrack.Count; ++i)
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

        private void AddTrack()
        {
            FileSCBox b = new FileSCBox();
            b.Filter = audioTrack1.Filter;
            b.Size = audioTrack1.Size;
            b.StandardItems = audioTrack1.StandardItems;
            b.SelectedIndex = 0;
            b.Anchor = audioTrack1.Anchor;
            b.SelectionChanged += new StringChanged(this.audioTrack1_SelectionChanged);
            
            int delta_y = audioTrack2.Location.Y - audioTrack1.Location.Y;
            b.Location = new Point(audioTrack1.Location.X, audioTrack[audioTrack.Count - 1].Location.Y + delta_y);

            Label l = new Label();
            l.Text = "Track " + (audioTrack.Count + 1);
            l.AutoSize = true;
            l.Location = new Point(track1Label.Location.X, trackLabel[trackLabel.Count - 1].Location.Y + delta_y);

            AudioConfigControl a = new AudioConfigControl();
            a.Dock = DockStyle.Fill;
            a.Location = audio1.Location;
            a.Size = audio1.Size;
            a.initHandler();
            a.SomethingChanged += new EventHandler(audio1_SomethingChanged);

            TabPage t = new TabPage("Audio track " + (audioTrack.Count + 1));
            t.UseVisualStyleBackColor = trackTabPage1.UseVisualStyleBackColor;
            t.Padding = trackTabPage1.Padding;
            t.Size = trackTabPage1.Size;
            t.Controls.Add(a);
            tabControl2.TabPages.Add(t);
            
            panel1.SuspendLayout();
            panel1.Controls.Add(l);
            panel1.Controls.Add(b);
            panel1.ResumeLayout();

            trackLabel.Add(l);
            audioTrack.Add(b);
            audioConfigControl.Add(a);
        }

        private void RemoveTrack()
        {
            panel1.SuspendLayout();
            panel1.Controls.Remove(trackLabel[trackLabel.Count - 1]);
            panel1.Controls.Remove(audioTrack[audioTrack.Count - 1]);
            panel1.ResumeLayout();
            
            tabControl2.TabPages.RemoveAt(tabControl2.TabPages.Count - 1);
            trackLabel.RemoveAt(trackLabel.Count - 1);
            audioTrack.RemoveAt(audioTrack.Count - 1);
        }

        private void audioTrack1_SelectionChanged(object sender, string val)
        {
            int i = audioTrack.IndexOf((FileSCBox)sender);
            Debug.Assert(i >= 0 && i < audioTrack.Count);
            
            FileSCBox track = audioTrack[i];
            if (!track.SelectedSCItem.IsStandard)
                audioConfigControl[i].openAudioFile((string)track.SelectedObject);
            audioConfigControl[i].DelayEnabled = !track.SelectedSCItem.IsStandard;
            if (oIndexerToUse == FileIndexerWindow.IndexType.FFMS && track.SelectedSCItem.IsStandard && oMkvInfo == null)
                audioConfigControl[i].DisableDontEncode(true);
            else
                audioConfigControl[i].DisableDontEncode(false);
        }
        
        private void audio1_SomethingChanged(object sender, EventArgs e)
        {
            updatePossibleContainers();
        }

        private void addTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTrack();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            removeTrackToolStripMenuItem.Enabled = (audioTrack.Count > 1);
        }

        private void removeTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveTrack();
        }

        private void keepInputResolution_CheckedChanged(object sender, EventArgs e)
        {
            if (keepInputResolution.Checked)
            {
                horizontalResolution.Enabled = false;
                autoCrop.Checked = false;
                autoCrop.Enabled = false;
                signalAR.Enabled = false;
                signalAR.Checked = false;
            }
            else
            {
                horizontalResolution.Enabled = true;
                autoCrop.Checked = true;
                autoCrop.Enabled = true;
                signalAR.Enabled = true;
            }
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
            OneClickWindow ocmt = new OneClickWindow(info, info.JobUtil, info.Video.VideoEncoderProvider,
                new AudioEncoderProvider());
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
