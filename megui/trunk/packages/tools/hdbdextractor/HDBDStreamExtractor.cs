// ****************************************************************************
// 
// Copyright (C) 2005-2012 Doom9 & al
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
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using MeGUI.core.plugins.interfaces;
using MeGUI.core.details;
using eac3to;

namespace MeGUI.packages.tools.hdbdextractor
{
    public partial class HdBdStreamExtractor : Form
    {
        List<Feature> features;
        List<Stream> streams;
        MainForm info;
        BackgroundWorker backgroundWorker;
        string eac3toPath;
        private MeGUISettings settings;
        private HDStreamsExJob lastJob = null;
        private int inputType = 1;
        string dummyInput = "";

        public HdBdStreamExtractor(MainForm info)
        {
            this.info = info;
            this.settings = info.Settings;
            InitializeComponent();

            eac3toPath = settings.EAC3toPath;
        }

        struct eac3toArgs
        {
            public string eac3toPath { get; set; }
            public string inputPath { get; set; }
            public string workingFolder { get; set; }
            public string featureNumber { get; set; }
            public string args { get; set; }
            public ResultState resultState { get; set; }

            public eac3toArgs(string eac3toPath, string inputPath, string args)
                : this()
            {
                this.eac3toPath = eac3toPath;
                this.inputPath = inputPath;
                this.args = args;
            }
        }

        public enum ResultState
        {
            [StringValue("Feature Retrieval Completed")]
            FeatureCompleted,
            [StringValue("Stream Retrieval Completed")]
            StreamCompleted,
            [StringValue("Stream Extraction Completed")]
            ExtractCompleted
        }

        #region backgroundWorker
        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            eac3toArgs args = (eac3toArgs)e.Argument;

            using (Process compiler = new Process())
            {
                compiler.StartInfo.FileName = args.eac3toPath;
                // use tester to debug posted logs from forums
                //compiler.StartInfo.FileName = "tester.exe";

                switch (args.resultState)
                {
                    case ResultState.FeatureCompleted:
                        compiler.StartInfo.Arguments = string.Format("\"{0}\"", args.inputPath);
                        //use commented line below for debuging posted feature logs from forums
                        //compiler.StartInfo.Arguments = "\"Tests\\featuers\\New Text Document.txt\"";
                        break;
                    case ResultState.StreamCompleted:
                        if (args.args == string.Empty)
                            compiler.StartInfo.Arguments = string.Format("\"{0}\"", args.inputPath);
                        else compiler.StartInfo.Arguments = string.Format("\"{0}\" {1}) {2}", args.inputPath, args.args, "-progressnumbers");
                        //use commented line below for debuging posted stream logs from forums
                        //compiler.StartInfo.Arguments = "\"Tests\\streams\\New Text Document (4).txt\"";
                        break;
                    case ResultState.ExtractCompleted:
                        if (FileSelection.Checked)
                            compiler.StartInfo.Arguments = string.Format("\"{0}\" {1}", args.inputPath, args.args + " -progressnumbers");
                        else compiler.StartInfo.Arguments = string.Format("\"{0}\" {1}) {2}", args.inputPath, args.featureNumber, args.args + "-progressnumbers");
                        break;
                }

                WriteToLog(string.Format("Arguments: {0}", compiler.StartInfo.Arguments));

                compiler.StartInfo.WorkingDirectory = args.workingFolder;
                compiler.StartInfo.CreateNoWindow = true;
                compiler.StartInfo.UseShellExecute = false;
                compiler.StartInfo.RedirectStandardOutput = true;
                compiler.StartInfo.RedirectStandardError = true;
                compiler.StartInfo.ErrorDialog = false;
                compiler.EnableRaisingEvents = true;

                compiler.EnableRaisingEvents = true;
                compiler.Exited += new EventHandler(backgroundWorker_Exited);
                compiler.ErrorDataReceived += new DataReceivedEventHandler(backgroundWorker_ErrorDataReceived);
                compiler.OutputDataReceived += new DataReceivedEventHandler(backgroundWorker_OutputDataReceived);

                try
                {
                    compiler.Start();
                    compiler.BeginErrorReadLine();
                    compiler.BeginOutputReadLine();

                    while (!compiler.HasExited)
                        if (backgroundWorker.CancellationPending)
                            compiler.Kill();
                    while (!compiler.HasExited) // wait until the process has terminated without locking the GUI
                    {
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                    compiler.WaitForExit();
                }
                catch (Exception ex)
                {
                    //e.Cancel = true;
                    //e.Result = ex.Message;
                    WriteToLog(ex.Message);
                }
                finally
                {
                    compiler.ErrorDataReceived -= new DataReceivedEventHandler(backgroundWorker_ErrorDataReceived);
                    compiler.OutputDataReceived -= new DataReceivedEventHandler(backgroundWorker_OutputDataReceived);
                }
            }

            e.Result = args.resultState;
        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SetToolStripProgressBarValue(e.ProgressPercentage);

            if (e.UserState != null)
                SetToolStripLabelText(e.UserState.ToString());
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                WriteToLog("Work was cancelled");

            if (e.Error != null)
            {
                WriteToLog(e.Error.Message);
                WriteToLog(e.Error.TargetSite.Name);
                WriteToLog(e.Error.Source);
                WriteToLog(e.Error.StackTrace);
            }

            SetToolStripProgressBarValue(0);
            SetToolStripLabelText(Extensions.GetStringValue(((ResultState)e.Result)));

            if (e.Result != null)
            {
                WriteToLog(Extensions.GetStringValue(((ResultState)e.Result)));

                switch ((ResultState)e.Result)
                {
                    case ResultState.FeatureCompleted:
                        FeatureDataGridView.DataSource = features;
                        FeatureButton.Enabled = true;
                        FeatureDataGridView.SelectionChanged += new System.EventHandler(this.FeatureDataGridView_SelectionChanged);
                        break;
                    case ResultState.StreamCompleted:
                        if (FileSelection.Checked)
                            StreamDataGridView.DataSource = streams;
                        else
                            StreamDataGridView.DataSource = ((eac3to.Feature)FeatureDataGridView.SelectedRows[0].DataBoundItem).Streams;
                        SelectTracks();
                        FeatureButton.Enabled = true;
                        break;
                    case ResultState.ExtractCompleted:
                        QueueButton.Enabled = true;
                        break;
                }
            }
        }

        void backgroundWorker_Exited(object sender, EventArgs e)
        {
            ResetCursor(Cursors.Default);
        }

        void backgroundWorker_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            string data;

            if (!String.IsNullOrEmpty(e.Data))
            {
                data = e.Data.TrimStart('\b').Trim();

                if (!string.IsNullOrEmpty(data))
                    WriteToLog("Error: " + e.Data);
            }
        }

        void backgroundWorker_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            string data;

            if (!string.IsNullOrEmpty(e.Data))
            {
                data = e.Data.TrimStart('\b').Trim();

                if (!string.IsNullOrEmpty(data))
                {
                    // Feature line
                    // 2) 00216.mpls, 0:50:19
                    if (Regex.IsMatch(data, @"^[0-99]+\).+$", RegexOptions.Compiled))
                    {
                        try
                        {
                            features.Add(eac3to.Feature.Parse(data));
                        }
                        catch (Exception ex)
                        {
                            WriteToLog(ex.Message);
                            WriteToLog(ex.Source);
                            WriteToLog(ex.StackTrace);
                        }

                        return;
                    }

                    // Feature name
                    // "Feature Name"
                    else if (Regex.IsMatch(data, "^\".+\"$", RegexOptions.Compiled))
                    {
                        if (FileSelection.Checked)
                            streams[streams.Count - 1].Name = Extensions.CapitalizeAll(data.Trim("\" .".ToCharArray()));
                        else features[features.Count - 1].Name = Extensions.CapitalizeAll(data.Trim("\" .".ToCharArray()));
                        return;
                    }

                    // Stream line on feature listing
                    // - h264/AVC, 1080p24 /1.001 (16:9)
                    else if (Regex.IsMatch(data, "^-.+$", RegexOptions.Compiled))
                        return;

                    // Playlist file listing
                    // [99+100+101+102+103+104+105+106+114].m2ts (blueray playlist *.mpls)
                    else if (Regex.IsMatch(data, @"^\[.+\].m2ts$", RegexOptions.Compiled))
                    {
                        foreach (string file in Regex.Match(data, @"\[.+\]").Value.Trim("[]".ToCharArray()).Split("+".ToCharArray()))
                            features[features.Count - 1].Files.Add(new File(file + ".m2ts", features[features.Count - 1].Files.Count + 1));

                        return;
                    }

                    // Stream listing feature header
                    // M2TS, 1 video track, 6 audio tracks, 9 subtitle tracks, 1:53:06
                    // EVO, 2 video tracks, 4 audio tracks, 8 subtitle tracks, 2:20:02
                    else if (Regex.IsMatch(data, "^M2TS, .+$", RegexOptions.Compiled) ||
                             Regex.IsMatch(data, "^EVO, .+$", RegexOptions.Compiled) ||
                             Regex.IsMatch(data, "^TS, .+$", RegexOptions.Compiled) ||
                             Regex.IsMatch(data, "^VOB, .+$", RegexOptions.Compiled) ||
                             Regex.IsMatch(data, "^MKV, .+$", RegexOptions.Compiled) ||
                             Regex.IsMatch(data, "^MKA, .+$", RegexOptions.Compiled)
                             )
                    {
                        WriteToLog(data);
                        return;
                    }

                    // Stream line
                    // 8: AC3, English, 2.0 channels, 192kbps, 48khz, dialnorm: -27dB
                    else if (Regex.IsMatch(data, "^[0-99]+:.+$", RegexOptions.Compiled))
                    {
                        if (FileSelection.Checked)
                        {
                            try
                            {
                                streams.Add(eac3to.Stream.Parse(data));
                            }
                            catch (Exception ex)
                            {
                                WriteToLog(ex.Message);
                                WriteToLog(ex.Source);
                                WriteToLog(ex.StackTrace);
                            }
                        }
                        else
                        {
                            try
                            {
                                if (FeatureDataGridView.SelectedRows.Count == 0)
                                    FeatureDataGridView.Rows[0].Selected = true;
                                ((Feature)FeatureDataGridView.SelectedRows[0].DataBoundItem).Streams.Add(Stream.Parse(data));
                            }
                            catch (Exception ex)
                            {
                                WriteToLog(ex.Message);
                                WriteToLog(ex.Source);
                                WriteToLog(ex.StackTrace);
                            }
                        }
                        return;
                    }

                    // Analyzing
                    // analyze: 100%
                    else if (Regex.IsMatch(data, "^analyze: [0-9]{1,3}%$", RegexOptions.Compiled))
                    {
                        if (backgroundWorker.IsBusy)
                            backgroundWorker.ReportProgress(int.Parse(Regex.Match(data, "[0-9]{1,3}").Value),
                                string.Format("Analyzing ({0}%)", int.Parse(Regex.Match(data, "[0-9]{1,3}").Value)));

                        return;
                    }

                    // Information line
                    // [a03] Creating file "audio.ac3"...
                    else if (Regex.IsMatch(data, @"^\[.+\] .+\.{3}$", RegexOptions.Compiled))
                    {
                        WriteToLog(data);
                        return;
                    }

                    else if (Regex.IsMatch(data, @"^\v .*...", RegexOptions.Compiled))
                    {
                        WriteToLog(data);
                        return;
                    }

                    else if (Regex.IsMatch(data, @"(core: .*)", RegexOptions.Compiled))
                    {
                        //WriteToLog(data);
                        return;
                    }

                    else if (Regex.IsMatch(data, @"(embedded: .*)", RegexOptions.Compiled))
                    {
                        //WriteToLog(data);
                        return;
                    }

                    // Creating file
                    // Creating file "C:\1_1_chapter.txt"...
                    else if (Regex.IsMatch(data, "^Creating file \".+\"\\.{3}$", RegexOptions.Compiled))
                    {
                        WriteToLog(data);
                        return;
                    }

                    // Processing
                    // process: 100%
                    else if (Regex.IsMatch(data, "^process: [0-9]{1,3}%$", RegexOptions.Compiled))
                    {
                        if (backgroundWorker.IsBusy)
                            backgroundWorker.ReportProgress(int.Parse(Regex.Match(data, "[0-9]{1,3}").Value),
                                string.Format("Processing ({0}%)", int.Parse(Regex.Match(data, "[0-9]{1,3}").Value)));

                        return;
                    }

                    // Progress
                    // progress: 100%
                    else if (Regex.IsMatch(data, "^progress: [0-9]{1,3}%$", RegexOptions.Compiled))
                    {
                        if (backgroundWorker.IsBusy)
                            backgroundWorker.ReportProgress(int.Parse(Regex.Match(data, "[0-9]{1,3}").Value),
                                string.Format("Progress ({0}%)", int.Parse(Regex.Match(data, "[0-9]{1,3}").Value)));

                        return;
                    }

                    // Done
                    // Done.
                    else if (data.Equals("Done."))
                    {
                        WriteToLog(data);
                        return;
                    }

                    #region Errors
                    // Source file not found
                    // Source file "x:\" not found.
                    else if (Regex.IsMatch(data, "^Source file \".*\" not found.$", RegexOptions.Compiled))
                    {
                        MessageBox.Show(data, "Source", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        WriteToLog(data);
                        return;
                    }

                    // Format of Source file not detected
                    // The format of the source file could not be detected.
                    else if (data.Equals("The format of the source file could not be detected."))
                    {
                        MessageBox.Show(data, "Source File Format", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        WriteToLog(data);
                        return;
                    }

                    // Audio conversion not supported
                    // This audio conversion is not supported.
                    else if (data.Equals("This audio conversion is not supported."))
                    {
                        MessageBox.Show(data, "Audio Conversion", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        WriteToLog(data);
                        return;
                    }
                    #endregion

                    // Unknown line
                    else
                    {
                        WriteToLog(string.Format("Unknown line: \"{0}\"", data));
                    }
                }
            }
        }
        #endregion

        #region GUI
        delegate void SetToolStripProgressBarValueCallback(int value);
        private void SetToolStripProgressBarValue(int value)
        {
            lock (this)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke(new SetToolStripProgressBarValueCallback(SetToolStripProgressBarValue), value);
                else
                    this.ToolStripProgressBar.Value = value;
            }
        }

        delegate void SetToolStripLabelTextCallback(string message);
        private void SetToolStripLabelText(string message)
        {
            lock (this)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke(new SetToolStripLabelTextCallback(SetToolStripLabelText), message);
                else
                    this.ToolStripStatusLabel.Text = message;
            }
        }

        delegate void ResetCursorCallback(System.Windows.Forms.Cursor cursor);
        private void ResetCursor(System.Windows.Forms.Cursor cursor)
        {
            lock (this)
            {
                if (this.InvokeRequired)
                    this.BeginInvoke(new ResetCursorCallback(ResetCursor), cursor);
                else
                    this.Cursor = cursor;
            }
        }

        delegate void WriteToLogCallback(string text);
        private void WriteToLog(string text)
        {
            if (LogTextBox.InvokeRequired)
                LogTextBox.BeginInvoke(new WriteToLogCallback(WriteToLog), text);
            else
            {
                lock (this)
                {
                    LogTextBox.AppendText(string.Format("[{0}] {1}{2}", DateTime.Now.ToString("HH:mm:ss"), text, Environment.NewLine));

                    //using (System.IO.StreamWriter SW = new System.IO.StreamWriter(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "HdBrStreamExtractor.txt"), true))
                    //{
                    //    SW.WriteLine(string.Format("[{0}] {1}{2}", DateTime.Now.ToString("HH:mm:ss"), text, Environment.NewLine));
                    //    SW.Close();
                    //}
                }
            }
        }

        private void FolderInputSourceButton_Click(object sender, EventArgs e)
        {
            string myinput = "";
            DialogResult dr;
            int idx = 0;

            if (FolderSelection.Checked)
            {
                folderBrowserDialog1.SelectedPath = MainForm.Instance.Settings.LastSourcePath;
                folderBrowserDialog1.Description = "Choose an input directory";
                folderBrowserDialog1.ShowNewFolderButton = false;
                dr = folderBrowserDialog1.ShowDialog();
                if (dr != DialogResult.OK)
                    return;
                if (folderBrowserDialog1.SelectedPath.EndsWith(":\\"))
                    myinput = folderBrowserDialog1.SelectedPath;
                else
                    myinput = folderBrowserDialog1.SelectedPath + System.IO.Path.DirectorySeparatorChar;
                if (dr == DialogResult.OK)
                    MainForm.Instance.Settings.LastSourcePath = myinput;
            }
            else
            {
                dr = openFileDialog1.ShowDialog();
                if (dr != DialogResult.OK)
                    return;
                inputType = 2;
                foreach (String file in openFileDialog1.FileNames)
                {
                    if (idx > 0) // seamless branching
                        myinput += "+" + file;
                    else
                        myinput = file;
                    idx++;
                }
            }

            FolderInputTextBox.Text = myinput;
            if (FolderInputTextBox.Text != "")
            {
                string projectPath;
                if (!string.IsNullOrEmpty(projectPath = MainForm.Instance.Settings.DefaultOutputDir))
                    FolderOutputTextBox.Text = projectPath;
                else
                {
                    if (string.IsNullOrEmpty(FolderOutputTextBox.Text))
                    {
                        if (idx > 0) // seamless branching
                            FolderOutputTextBox.Text = FolderInputTextBox.Text.Substring(0, (FolderInputTextBox.Text.LastIndexOf("\\") - FolderInputTextBox.Text.LastIndexOf("+")));
                        else
                            FolderOutputTextBox.Text = FolderInputTextBox.Text.Substring(0, FolderInputTextBox.Text.LastIndexOf("\\") + 1);
                    }
                }
                FeatureButton_Click(null, null);
            }
        }

        private void FolderOutputSourceButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = MainForm.Instance.Settings.LastDestinationPath;
            folderBrowserDialog1.Description = "Choose an output directory";
            folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult dr = folderBrowserDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                FolderOutputTextBox.Text = folderBrowserDialog1.SelectedPath;
                MainForm.Instance.Settings.LastDestinationPath = folderBrowserDialog1.SelectedPath;
            }
        }

        private void FeatureButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FolderInputTextBox.Text))
            {
                MessageBox.Show("Configure input source folder prior to retrieving features.", "Feature Retrieval", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            InitBackgroundWorker();
            eac3toArgs args = new eac3toArgs();

            args.eac3toPath = eac3toPath;
            args.inputPath = FolderInputTextBox.Text;
            args.workingFolder = string.IsNullOrEmpty(FolderOutputTextBox.Text) ? FolderOutputTextBox.Text : System.IO.Path.GetDirectoryName(args.eac3toPath);
            if (FolderSelection.Checked)
            {
                args.resultState = ResultState.FeatureCompleted;
                args.args = string.Empty;

                features = new List<Feature>();
                backgroundWorker.ReportProgress(0, "Retrieving features...");
                WriteToLog("Retrieving features...");
            }
            else
            {
                args.resultState = ResultState.StreamCompleted;
                args.args = string.Empty;

                streams = new List<Stream>();
                backgroundWorker.ReportProgress(0, "Retrieving streams...");
                WriteToLog("Retrieving streams...");

            }
            FeatureButton.Enabled = false;
            Cursor = Cursors.WaitCursor;

            backgroundWorker.RunWorkerAsync(args);
        }

        private void StreamDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in StreamDataGridView.Rows)
            {
                Stream s = row.DataBoundItem as Stream;
                DataGridViewComboBoxCell comboBox = row.Cells["StreamExtractAsComboBox"] as DataGridViewComboBoxCell;
                DataGridViewTextBoxCell tbLang = row.Cells["languageDataGridViewTextBoxColumn"] as DataGridViewTextBoxCell;
                comboBox.Items.Clear();
                comboBox.Items.AddRange(s.ExtractTypes);

                switch (s.Type)
                {
                    case eac3to.StreamType.Chapter:
                        comboBox.Value = "TXT";
                        break;
                    case eac3to.StreamType.Join:
                        if (s.Name == "Joined EVO")
                            comboBox.Value = "EVO";
                        else comboBox.Value = "VOB";
                        break;
                    case eac3to.StreamType.Subtitle:
                        switch (s.Description.Substring(11, 3))
                        {
                            case "ASS": comboBox.Value = "ASS"; break;
                            case "SSA": comboBox.Value = "SSA"; break;
                            case "SRT": comboBox.Value = "SRT"; break;
                            case "Vob": comboBox.Value = "IDX"; break;
                            default: comboBox.Value = "SUP"; break;
                        }
                        break;
                    case eac3to.StreamType.Video:
                        comboBox.Value = "MKV";
                        break;
                    case eac3to.StreamType.Audio:
                        comboBox.Value = comboBox.Items[0];
                        break;
                }

                if ((s.Type == eac3to.StreamType.Audio) || (s.Type == eac3to.StreamType.Subtitle))
                {
                    char[] separator = { ',' };
                    string[] split = s.Description.Split(separator, 100);

                    if (s.Name.Contains("Subtitle"))
                        s.Language = s.Name;
                    else
                        s.Language = split[1].Substring(1, split[1].Length - 1);

                    bool bFound = false;
                    foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                    {
                        if (s.Language.ToLower(System.Globalization.CultureInfo.InvariantCulture).Contains(strLanguage.Key.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                        {
                            s.Language = strLanguage.Key;
                            bFound = true;
                            break;
                        }
                    }
                    if (!bFound)
                    {
                        if (!FolderSelection.Checked && System.IO.Path.GetExtension(FolderInputTextBox.Text).ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(".mkv"))
                            s.Language = "English";
                        else
                            s.Language = "";
                    }
                }
                else
                    s.Language = "";
            }
        }

        private void InitBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }

        private void QueueButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FolderOutputTextBox.Text))
            {
                MessageBox.Show("Configure output target folder prior to queueing job.", "Queue Job", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            if (StreamDataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Retrieve streams prior to queueing job.", "Queue Job", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!IsStreamCheckedForExtract())
            {
                MessageBox.Show("Select stream(s) to extract prior to queueing job.", "Queue Job", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            if (FolderSelection.Checked && FeatureDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select feature prior to queueing job.", "Queue Job", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!Drives.ableToWriteOnThisDrive(System.IO.Path.GetPathRoot(FolderOutputTextBox.Text)))
            {
                MessageBox.Show("MeGUI cannot write on " + System.IO.Path.GetPathRoot(FolderOutputTextBox.Text) +
                                "\nPlease, select another Output path.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }


            eac3toArgs args = new eac3toArgs();
            HDStreamsExJob job;

            args.eac3toPath = eac3toPath;
            args.inputPath = FolderInputTextBox.Text;
            if (FolderSelection.Checked)
                args.featureNumber = ((Feature)FeatureDataGridView.SelectedRows[0].DataBoundItem).Number.ToString();
            args.workingFolder = string.IsNullOrEmpty(FolderOutputTextBox.Text) ? FolderOutputTextBox.Text : System.IO.Path.GetDirectoryName(args.eac3toPath);
            args.resultState = ResultState.ExtractCompleted;

            try
            {
                args.args = GenerateArguments();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Stream Extract", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            // Load to MeGUI job queue
            if (FolderSelection.Checked)
                job = new HDStreamsExJob(dummyInput, this.FolderOutputTextBox.Text + "xxx", args.featureNumber, args.args, inputType);
            else
                job = new HDStreamsExJob(this.FolderInputTextBox.Text, this.FolderOutputTextBox.Text + "xxx", null, args.args, inputType);

            lastJob = job;
            info.Jobs.addJobsToQueue(job);
            if (this.closeOnQueue.Checked)
                this.Close();
        }

        public HDStreamsExJob LastJob
        {
            get { return lastJob; }
            set { lastJob = value; }
        }
        public bool JobCreated
        {
            get { return lastJob != null; }
        }

        private string GenerateArguments()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataGridViewRow row in StreamDataGridView.Rows)
            {
                Stream stream = row.DataBoundItem as Stream;
                DataGridViewCheckBoxCell extractStream = row.Cells["StreamExtractCheckBox"] as DataGridViewCheckBoxCell;

                if (extractStream.Value != null && int.Parse(extractStream.Value.ToString()) == 1)
                {
                    if (row.Cells["StreamExtractAsComboBox"].Value == null)
                        throw new ApplicationException(string.Format("Specify an extraction type for stream:\r\n\n\t{0}: {1}", stream.Number, stream.Name));

                    if (FolderSelection.Checked)
                        sb.Append(string.Format("{0}:\"{1}\" {2} ", stream.Number,
                            System.IO.Path.Combine(FolderOutputTextBox.Text, string.Format("F{0}_T{1}_{2} - {3}.{4}", ((Feature)FeatureDataGridView.SelectedRows[0].DataBoundItem).Number, stream.Number, Extensions.GetStringValue(stream.Type), row.Cells["languageDataGridViewTextBoxColumn"].Value, (row.Cells["StreamExtractAsComboBox"].Value).ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture))),
                            row.Cells["StreamAddOptionsTextBox"].Value).Trim());
                    else
                        sb.Append(string.Format("{0}:\"{1}\" {2} ", stream.Number,
                            System.IO.Path.Combine(FolderOutputTextBox.Text, string.Format("T{0}_{1} - {2}.{3}", stream.Number, Extensions.GetStringValue(stream.Type), row.Cells["languageDataGridViewTextBoxColumn"].Value, (row.Cells["StreamExtractAsComboBox"].Value).ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture))),
                            row.Cells["StreamAddOptionsTextBox"].Value).Trim());

                    if (row.Cells["StreamExtractAsComboBox"].Value.Equals(AudioCodec.DTS.ID))
                        sb.Append(" -core");

                    sb.Append(" ");
                }
            }

            return sb.ToString();
        }

        private void CancelButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpButton2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://en.wikibooks.org/wiki/Eac3to/How_to_Use");
        }

        private void HdBrStreamExtractor_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (backgroundWorker != null)
            {
                if (backgroundWorker.IsBusy)
                    if (MessageBox.Show("A process is still running. Do you want to cancel it?", "Cancel process?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        backgroundWorker.CancelAsync();

                if (backgroundWorker.CancellationPending)
                    backgroundWorker.Dispose();
            }
        }

        private bool IsStreamCheckedForExtract()
        {
            bool enableQueue = false;

            foreach (DataGridViewRow row in StreamDataGridView.Rows)
                if (row.Cells["StreamExtractCheckBox"].Value != null && int.Parse(row.Cells["StreamExtractCheckBox"].Value.ToString()) == 1)
                    enableQueue = true;

            return enableQueue;
        }

        private void Eac3toLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.doom9.org/showthread.php?t=125966");
        }

        private void FeatureDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // only fire after the Databind has completed on grid and a row is selected
            if (FeatureDataGridView.Rows.Count == features.Count && FeatureDataGridView.SelectedRows.Count == 1)
            {
                if (backgroundWorker.IsBusy) // disallow selection change
                {
                    this.FeatureDataGridView.SelectionChanged -= new System.EventHandler(this.FeatureDataGridView_SelectionChanged);

                    FeatureDataGridView.CurrentRow.Selected = false;
                    FeatureDataGridView.Rows[int.Parse(FeatureDataGridView.Tag.ToString())].Selected = true;

                    this.FeatureDataGridView.SelectionChanged += new System.EventHandler(this.FeatureDataGridView_SelectionChanged);
                }
                else // backgroundworker is not busy, allow selection change
                {
                    Feature feature = FeatureDataGridView.SelectedRows[0].DataBoundItem as Feature;

                    // Check for Streams
                    if (feature.Streams == null || feature.Streams.Count == 0)
                    {
                        InitBackgroundWorker();
                        eac3toArgs args = new eac3toArgs();

                        args.eac3toPath = eac3toPath;
                        args.inputPath = FolderInputTextBox.Text;
                        args.workingFolder = string.IsNullOrEmpty(FolderOutputTextBox.Text) ? FolderOutputTextBox.Text : System.IO.Path.GetDirectoryName(args.eac3toPath);
                        args.resultState = ResultState.StreamCompleted;
                        args.args = ((Feature)FeatureDataGridView.SelectedRows[0].DataBoundItem).Number.ToString();

                        // create dummy input string for megui job
                        if (feature.Description.Contains("EVO"))
                        {
                            if (args.inputPath.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Contains("HVDVD_TS"))
                                dummyInput = args.inputPath + feature.Description.Substring(0, feature.Description.IndexOf(","));
                            else dummyInput = args.inputPath + "HVDVD_TS\\" + feature.Description.Substring(0, feature.Description.IndexOf(","));
                        }
                        else if (feature.Description.Contains("(angle"))
                        {
                            if (args.inputPath.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Contains("BDMV\\PLAYLIST"))
                                dummyInput = args.inputPath + feature.Description.Substring(0, feature.Description.IndexOf(" ("));
                            else if (args.inputPath.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Contains("BDMV\\STREAM"))
                                dummyInput = args.inputPath.Substring(0, args.inputPath.LastIndexOf("BDMV")) + "BDMV\\PLAYLIST\\" + feature.Description.Substring(0, feature.Description.IndexOf(" ("));
                            else dummyInput = args.inputPath + "BDMV\\PLAYLIST\\" + feature.Description.Substring(0, feature.Description.IndexOf(" ("));
                        }
                        else if (feature.Description.Substring(feature.Description.LastIndexOf(".") + 1, 4) == "m2ts")
                        {
                            string des = feature.Description.Substring(feature.Description.IndexOf(",") + 2, feature.Description.LastIndexOf(",") - feature.Description.IndexOf(",") - 2);

                            if (des.Contains("+")) // seamless branching
                            {
                                if (args.inputPath.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Contains("BDMV\\STREAM"))
                                    dummyInput = args.inputPath.Substring(0, args.inputPath.IndexOf("BDMV")) + "BDMV\\PLAYLIST\\" + feature.Description.Substring(0, feature.Description.IndexOf(","));
                                else
                                    dummyInput = args.inputPath + "BDMV\\PLAYLIST\\" + feature.Description.Substring(0, feature.Description.IndexOf(","));
                            }
                            else
                            {
                                if (args.inputPath.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Contains("BDMV\\STREAM"))
                                    dummyInput = args.inputPath + des;
                                else dummyInput = args.inputPath + "BDMV\\STREAM\\" + des;
                            }
                        }
                        else
                        {
                            if (args.inputPath.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Contains("BDMV\\PLAYLIST"))
                                dummyInput = args.inputPath + feature.Description.Substring(0, feature.Description.IndexOf(","));
                            else dummyInput = args.inputPath + "BDMV\\PLAYLIST\\" + feature.Description.Substring(0, feature.Description.IndexOf(","));
                        }

                        backgroundWorker.ReportProgress(0, "Retrieving streams...");
                        WriteToLog("Retrieving streams...");
                        Cursor = Cursors.WaitCursor;

                        backgroundWorker.RunWorkerAsync(args);
                    }
                    else // use already collected streams
                    {
                        StreamDataGridView.DataSource = feature.Streams;
                        SelectTracks();
                    }
                }
            }
        }

        private void FeatureDataGridView_DataBindingComplete(object sender, System.Windows.Forms.DataGridViewBindingCompleteEventArgs e)
        {
            FeatureDataGridView.ClearSelection();
        }

        private void FeatureDataGridView_RowLeave(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            FeatureDataGridView.Tag = e.RowIndex;
        }

        private void FeatureDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in FeatureDataGridView.Rows)
            {
                Feature feature = row.DataBoundItem as Feature;
                DataGridViewComboBoxCell comboBox = row.Cells["FeatureFileDataGridViewComboBoxColumn"] as DataGridViewComboBoxCell;

                if (feature != null)
                {
                    if (feature.Files != null || feature.Files.Count > 0)
                    {
                        foreach (File file in feature.Files)
                        {
                            comboBox.Items.Add(file.FullName);

                            if (file.Index == 1)
                                comboBox.Value = file.FullName;
                        }
                    }
                }
            }
        }
        #endregion

        private void SelectTracks()
        {
            if (!MainForm.Instance.Settings.AutoSelectHDStreams)
                return;

            bool bVideoSelected = false;
            foreach (DataGridViewRow row in StreamDataGridView.Rows)
            {
                DataGridViewCheckBoxCell extractStream = row.Cells["StreamExtractCheckBox"] as DataGridViewCheckBoxCell;
                if (row.Cells["StreamTypeTextBox"].Value.ToString().Equals("Chapter"))
                {
                    extractStream.Value = 1;
                }
                else if (!bVideoSelected && row.Cells["StreamTypeTextBox"].Value.ToString().Equals("Video"))
                {
                    extractStream.Value = 1;
                    bVideoSelected = true;
                }
                else if (row.Cells["languageDataGridViewTextBoxColumn"].Value.ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(MainForm.Instance.Settings.DefaultLanguage1.ToLower(System.Globalization.CultureInfo.InvariantCulture)) ||
                    row.Cells["languageDataGridViewTextBoxColumn"].Value.ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture).Equals(MainForm.Instance.Settings.DefaultLanguage2.ToLower(System.Globalization.CultureInfo.InvariantCulture)))
                {
                    extractStream.Value = 1;
                }
            }
        }
    }

    public class HdBdExtractorTool : ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "HD Streams Extractor"; }
        }

        public void Run(MainForm info)
        {
            (new HdBdStreamExtractor(info)).Show();
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlF7 }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return Name; }
        }

        #endregion
    }
}