// ****************************************************************************
// 
// Copyright (C) 2005-2014 Doom9 & al
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace MeGUI
{
	/// <summary>
	/// Summary description for SettingsForm.
	/// </summary>
	public partial class SettingsForm : Form
	{
		#region variables
        private MeGUISettings internalSettings = new MeGUISettings();
        private XmlDocument ContextHelp = new XmlDocument();
        private AutoEncodeDefaultsSettings autoEncodeDefaults;
        private SourceDetectorSettings sdSettings;
		#endregion

		#region start / stop
		public SettingsForm()
		{
			InitializeComponent();
            List<string> keys = new List<string>(LanguageSelectionContainer.Languages.Keys);
            defaultLanguage2.DataSource = defaultLanguage1.DataSource = keys;
            defaultLanguage2.BindingContext = new BindingContext();
            defaultLanguage1.BindingContext = new BindingContext();
            SetToolTips();
#if x86
            if (!OSInfo.isWow64())
                chkEnable64bitX264.Enabled = chkEnable64bitX264.Checked = false;
#endif
#if x64
            chkEnable64bitX264.Enabled = false;
            chkEnable64bitX264.Checked = true;
            chkEnable64bitX264.Visible = false;
#endif
            ffmsThreads.Maximum = System.Environment.ProcessorCount;
        }

        /// <summary>
        /// Sets any required tooltips
        /// </summary>
        private void SetToolTips()
        {
            try
            {
                string p = System.IO.Path.Combine(Application.StartupPath, "Data");
                p = System.IO.Path.Combine(p, "ContextHelp.xml");             
                ContextHelp.Load(p);
            }
            catch
            {
                MessageBox.Show("The ContextHelp.xml file could not be found. Please check in the 'Data' directory to see if it exists. Help tooltips will not be available.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            toolTipHelp.SetToolTip(chkAlwaysMuxMKV, SelectHelpText("alwaysmuxmkv"));
            toolTipHelp.SetToolTip(ffmsThreads, SelectHelpText("ffmsthreads"));
            toolTipHelp.SetToolTip(cbUseITUValues, SelectHelpText("useituvalues"));
        }

        /// <summary>
        /// Gets the help text
        /// </summary>
        private string SelectHelpText(string node)
        {
            StringBuilder HelpText = new StringBuilder(64);

            try
            {
                string xpath = "/ContextHelp/Form[@name='SettingsForm']/" + node;
                XmlNodeList nl = ContextHelp.SelectNodes(xpath); // Return the details for the specified node

                if (nl.Count == 1) // if it finds the required HelpText, count should be 1
                {
                    HelpText.Append(nl[0].Attributes["name"].Value);
                    HelpText.AppendLine();
                    HelpText.AppendLine(nl[0]["Text"].InnerText);
                }
                else // If count isn't 1, then theres no valid data.
                    HelpText.Append("Error: No data available");
            }
            catch
            {
                HelpText.Append("Error: No data available");
            }

            return (HelpText.ToString());
        }
		#endregion

        #region button handlers
        private void configSourceDetector_Click(object sender, EventArgs e)
        {
            SourceDetectorConfigWindow sdcWindow = new SourceDetectorConfigWindow();
            sdcWindow.Settings = sdSettings;
            if (sdcWindow.ShowDialog() == DialogResult.OK)
                sdSettings = sdcWindow.Settings;
        }
  
        private void resetDialogs_Click(object sender, EventArgs e)
        {
            internalSettings.DialogSettings = new DialogSettings();
            MessageBox.Show(this, "Successfully reset all dialogs", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void runCommand_CheckedChanged(object sender, EventArgs e)
        {
            command.Enabled = runCommand.Checked;
        }
        /// <summary>
        /// launches the autoencode default settings window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoEncodeDefaultsButton_Click(object sender, EventArgs e)
        {
            using (AutoEncodeDefaults aed = new AutoEncodeDefaults())
            {
                aed.Settings = this.autoEncodeDefaults;
                DialogResult dr = aed.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    this.autoEncodeDefaults = aed.Settings;
                }
            }
        }

        /// <summary>
        /// Launches configuration of auto-update servers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configureServersButton_Click(object sender, EventArgs e)
        {
            using (MeGUI.core.gui.AutoUpdateServerConfigWindow w = new MeGUI.core.gui.AutoUpdateServerConfigWindow())
            {
                w.ServerList = internalSettings.AutoUpdateServerLists;
                w.ServerListIndex = cbAutoUpdateServerSubList.SelectedIndex;
                if (w.ShowDialog() == DialogResult.OK)
                    internalSettings.AutoUpdateServerLists = w.ServerList;
            }
        }

        private void cbHttpProxyMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var httpProxyMode = (ProxyMode)this.cbHttpProxyMode.SelectedIndex;

            txt_httpproxyaddress.Enabled = httpProxyMode == ProxyMode.CustomProxy || httpProxyMode == ProxyMode.CustomProxyWithLogin;
            txt_httpproxyport.Enabled = httpProxyMode == ProxyMode.CustomProxy || httpProxyMode == ProxyMode.CustomProxyWithLogin;
            txt_httpproxyuid.Enabled = httpProxyMode == ProxyMode.CustomProxyWithLogin;
            txt_httpproxypwd.Enabled = httpProxyMode == ProxyMode.CustomProxyWithLogin;
        }

        private void clearDefaultOutputDir_Click(object sender, EventArgs e)
        {
            defaultOutputDir.Filename = "";
        }

		#endregion
		#region properties
		public MeGUISettings Settings
		{
			get 
			{
                MeGUISettings settings = internalSettings;
                settings.AcceptableFPSError = acceptableFPSError.Value; 
                
                settings.SourceDetectorSettings = sdSettings;
                settings.NeroAacEncPath = neroaacencLocation.Filename;
                settings.VideoExtension = videoExtension.Text;
                settings.AudioExtension = audioExtension.Text;
				settings.DefaultLanguage1 = defaultLanguage1.Text;
				settings.DefaultLanguage2 = defaultLanguage2.Text;
				settings.AutoForceFilm = autoForceFilm.Checked;
                settings.AutoLoadDG = cbAutoLoadDG.Checked;
                settings.EnsureCorrectPlaybackSpeed = chkEnsureCorrectPlaybackSpeed.Checked;
				settings.ForceFilmThreshold = forceFilmPercentage.Value;
				settings.DefaultPriority = (ProcessPriority)priority.SelectedIndex;
                settings.OpenAVSInThread = cbOpenAVSinThread.Checked;
                if (cbOpenAVSinThread.CheckState == CheckState.Checked)
                    settings.OpenAVSInThreadDuringSession = true;
                else
                    settings.OpenAVSInThreadDuringSession = false;
				settings.AutoStartQueue = this.autostartQueue.Checked;
                settings.AutoStartQueueStartup = this.cbAutoStartQueueStartup.Checked;
                settings.AlwaysMuxMKV = this.chkAlwaysMuxMKV.Checked;
                if (donothing.Checked)
                    settings.AfterEncoding = AfterEncoding.DoNothing;
                else if (shutdown.Checked)
                    settings.AfterEncoding = AfterEncoding.Shutdown;
                else if (rbCloseMeGUI.Checked)
                    settings.AfterEncoding = AfterEncoding.CloseMeGUI;
                else
                {
                    settings.AfterEncoding = AfterEncoding.RunCommand;
                    settings.AfterEncodingCommand = command.Text;
                }
                settings.AutoOpenScript = openScript.Checked;
				settings.DeleteCompletedJobs = deleteCompletedJobs.Checked;
				settings.DeleteIntermediateFiles = deleteIntermediateFiles.Checked;
				settings.DeleteAbortedOutput = deleteAbortedOutput.Checked;
				settings.OpenProgressWindow = openProgressWindow.Checked;
				settings.Keep2of3passOutput = keep2ndPassOutput.Checked;
				settings.OverwriteStats = keep2ndPassLogFile.Checked;
				settings.NbPasses = (int)nbPasses.Value;
                settings.AutoSelectHDStreams = chkSelectHDTracks.Checked;
                settings.AedSettings = this.autoEncodeDefaults;
                settings.AlwaysOnTop = chAlwaysOnTop.Checked;
                settings.DefaultOutputDir = defaultOutputDir.Filename;
                settings.TempDirMP4 = tempDirMP4.Filename;
                settings.AddTimePosition = cbAddTimePos.Checked;
                settings.Use64bitX264 = chkEnable64bitX264.Checked;
                settings.FFMSThreads = Decimal.ToInt32(ffmsThreads.Value);
                settings.AppendToForcedStreams = txtForcedName.Text;
                settings.UseITUValues = cbUseITUValues.Checked;

                // update server settings
                settings.AlwaysBackUpFiles = backupfiles.Checked;
                settings.AutoUpdate = useAutoUpdateCheckbox.Checked;
                MainForm.Instance.UpdateHandler.UpdateMode = settings.UpdateMode;
                settings.HttpProxyMode = (ProxyMode)this.cbHttpProxyMode.SelectedIndex;
                settings.HttpProxyAddress = txt_httpproxyaddress.Text;
                settings.HttpProxyPort = txt_httpproxyport.Text;
                settings.HttpProxyUid = txt_httpproxyuid.Text;
                settings.HttpProxyPwd = txt_httpproxypwd.Text;
                if (cbAutoUpdateServerSubList.SelectedIndex != internalSettings.AutoUpdateServerSubList)
                    settings.LastUpdateCheck = DateTime.Now.AddDays(-7).ToUniversalTime();
                settings.AutoUpdateServerSubList = cbAutoUpdateServerSubList.SelectedIndex;

                if (useDGIndexNV.Checked != internalSettings.UseDGIndexNV)
                    UpdateCacher.CheckPackage("dgindexnv", useDGIndexNV.Checked, false);
                settings.UseDGIndexNV = useDGIndexNV.Checked;

                if (useNeroAacEnc.Checked != internalSettings.UseNeroAacEnc)
                    UpdateCacher.CheckPackage("neroaacenc", useNeroAacEnc.Checked, false);
                settings.UseNeroAacEnc = useNeroAacEnc.Checked;

                if (useQAAC.Checked != internalSettings.UseQAAC)
                    UpdateCacher.CheckPackage("qaac", useQAAC.Checked, false);
                settings.UseQAAC = useQAAC.Checked;

                if (useX265.Checked != internalSettings.UseX265)
                    UpdateCacher.CheckPackage("x265", useX265.Checked, false);
                settings.UseX265 = useX265.Checked;

                settings.UseExternalMuxerX264 = chx264ExternalMuxer.Checked;
                settings.AlwaysUsePortableAviSynth = cbUseIncludedAviSynth.Checked;
				return settings;
			}
			set
			{
                internalSettings = value;
                MeGUISettings settings = value;
                acceptableFPSError.Value = settings.AcceptableFPSError;
                useAutoUpdateCheckbox.Checked = settings.AutoUpdate;
                neroaacencLocation.Filename = settings.NeroAacEncPath;
                sdSettings = settings.SourceDetectorSettings;
                videoExtension.Text = settings.VideoExtension;
                audioExtension.Text = settings.AudioExtension;
                chkEnsureCorrectPlaybackSpeed.Checked = settings.EnsureCorrectPlaybackSpeed;
				int index = this.defaultLanguage1.Items.IndexOf(settings.DefaultLanguage1);
				if (index != -1)
					defaultLanguage1.SelectedIndex = index;
				index = defaultLanguage2.Items.IndexOf(settings.DefaultLanguage2);
				if (index != -1)
					defaultLanguage2.SelectedIndex = index;
				autoForceFilm.Checked = settings.AutoForceFilm;
                cbAutoLoadDG.Checked = settings.AutoLoadDG;
				forceFilmPercentage.Value = settings.ForceFilmThreshold;
				priority.SelectedIndex = (int)settings.DefaultPriority;
                cbOpenAVSinThread.Checked = settings.OpenAVSInThread;
                if (settings.OpenAVSInThread && !settings.OpenAVSInThreadDuringSession)
                    cbOpenAVSinThread.CheckState = CheckState.Indeterminate;
				autostartQueue.Checked = settings.AutoStartQueue;
                cbAutoStartQueueStartup.Checked = settings.AutoStartQueueStartup;
                chkAlwaysMuxMKV.Checked = settings.AlwaysMuxMKV;
                donothing.Checked = settings.AfterEncoding == AfterEncoding.DoNothing;
                shutdown.Checked = settings.AfterEncoding == AfterEncoding.Shutdown;
                runCommand.Checked = settings.AfterEncoding == AfterEncoding.RunCommand;
                rbCloseMeGUI.Checked = settings.AfterEncoding == AfterEncoding.CloseMeGUI;
                command.Text = settings.AfterEncodingCommand;
				deleteCompletedJobs.Checked = settings.DeleteCompletedJobs;
                openScript.Checked = settings.AutoOpenScript;
				deleteIntermediateFiles.Checked = settings.DeleteIntermediateFiles;
				deleteAbortedOutput.Checked = settings.DeleteAbortedOutput;
				openProgressWindow.Checked = settings.OpenProgressWindow;
				keep2ndPassOutput.Checked = settings.Keep2of3passOutput;
				keep2ndPassLogFile.Checked = settings.OverwriteStats;
				nbPasses.Value = (decimal)settings.NbPasses;
                chkSelectHDTracks.Checked = settings.AutoSelectHDStreams;
                this.autoEncodeDefaults = settings.AedSettings;
                chAlwaysOnTop.Checked = settings.AlwaysOnTop;
                cbHttpProxyMode.SelectedIndex = (int)settings.HttpProxyMode;
                txt_httpproxyaddress.Text = settings.HttpProxyAddress;
                txt_httpproxyport.Text = settings.HttpProxyPort;
                txt_httpproxyuid.Text = settings.HttpProxyUid;
                txt_httpproxypwd.Text = settings.HttpProxyPwd;
                defaultOutputDir.Filename = settings.DefaultOutputDir;
                tempDirMP4.Filename = settings.TempDirMP4;
                cbAddTimePos.Checked = settings.AddTimePosition;
                backupfiles.Checked = settings.AlwaysBackUpFiles;
                cbAutoUpdateServerSubList.SelectedIndex = settings.AutoUpdateServerSubList;
                chkEnable64bitX264.Checked = settings.Use64bitX264;
                txtForcedName.Text = settings.AppendToForcedStreams;
                if (ffmsThreads.Maximum < settings.FFMSThreads)
                    ffmsThreads.Value = ffmsThreads.Maximum;
                else
                    ffmsThreads.Value = settings.FFMSThreads;
                cbUseITUValues.Checked = settings.UseITUValues;
                useDGIndexNV.Checked = settings.UseDGIndexNV;
                useNeroAacEnc.Checked = settings.UseNeroAacEnc;
                useQAAC.Checked = settings.UseQAAC;
                useX265.Checked = settings.UseX265;
                chx264ExternalMuxer.Checked = settings.UseExternalMuxerX264;
                cbUseIncludedAviSynth.Checked = settings.AlwaysUsePortableAviSynth;
			}
		}
		#endregion

        private void backupfiles_CheckedChanged(object sender, EventArgs e)
        {
            if (!backupfiles.Checked)
            {
                string meguiToolsFolder = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
                if (Directory.Exists(meguiToolsFolder))
                {
                    try
                    {  // remove all backup files found
                        Array.ForEach(Directory.GetFiles(meguiToolsFolder, "*.backup", SearchOption.AllDirectories),
                          delegate(string path) { File.Delete(path); });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClearMP4TempDirectory_Click(object sender, EventArgs e)
        {
            tempDirMP4.Filename = "";
        }

        private void btnClearOutputDirecoty_Click(object sender, EventArgs e)
        {
            defaultOutputDir.Filename = "";
        }

        private void tempDirMP4_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            if (System.IO.Path.GetPathRoot(tempDirMP4.Filename).Equals(tempDirMP4.Filename, StringComparison.CurrentCultureIgnoreCase))
            {
                // mp4box has in some builds problems if the tmp directory is in the root of a drive
                MessageBox.Show("A root folder cannot be selected!", "Wrong path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tempDirMP4.Filename = String.Empty;
            }
        }

        private void useNeroAacEnc_CheckedChanged(object sender, EventArgs e)
        {
            neroaacencLocation.Enabled = lblNero.Enabled = useNeroAacEnc.Checked;
            if (useNeroAacEnc.Checked && !internalSettings.UseNeroAacEnc)
                MessageBox.Show("You have to restart MeGUI in order to get access to NeroAacEnc.\r\nAlso the program files are not available on the MeGUI update server. They have to be downloaded from:\r\nhttp://www.nero.com/eng/downloads-nerodigital-nero-aac-codec.php", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void useQAAC_CheckedChanged(object sender, EventArgs e)
        {
            if (useQAAC.Checked && !internalSettings.UseQAAC)
                MessageBox.Show("You have to restart MeGUI in order to get access to QAAC.\r\nAlso external dependencies must be installed if not already available. More information can be found here:\r\nhttps://sites.google.com/site/qaacpage/", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void useX265_CheckedChanged(object sender, EventArgs e)
        {
            if (useX265.Checked && !internalSettings.UseX265)
                MessageBox.Show("You have to restart MeGUI in order to get access to x265.", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void useDGIndexNV_CheckedChanged(object sender, EventArgs e)
        {
            // check if the license file is available
            if (useDGIndexNV.Checked && !File.Exists(Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.DGIndexNV.Path), "license.txt")))
                MessageBox.Show("DGIndexNV cannot be used for free. Therefore you have to copy/create the file license.txt into your ..\\tools\\dgindexnv directory or you have to disable DGIndexNV.", "license.txt for DGIndexNV missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cbUseIncludedAviSynth_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseIncludedAviSynth.Checked != internalSettings.AlwaysUsePortableAviSynth)
                MessageBox.Show("You have to restart MeGUI so that the new AviSynth configuration can be changed.", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
	}
}