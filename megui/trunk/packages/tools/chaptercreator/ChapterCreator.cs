// ****************************************************************************
// 
// Copyright (C) 2005-2015 Doom9 & al
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
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI
{

	/// <summary>
	/// Summary description for ChapterCreator.
	/// </summary>
	public partial class ChapterCreator : Form
	{

		private Chapter[] chapters;
		private string videoInput;
		private VideoPlayer player;
		private int introEndFrame = 0, creditsStartFrame = 0;
        private MainForm mainForm;
        private ChapterInfo pgc;
        private int intIndex;
        private bool bFPSKnown;

		#region start / stop
		public ChapterCreator(MainForm mainForm)
		{
			InitializeComponent();
            intIndex = 0;
			chapters = new Chapter[0];
            this.mainForm = mainForm;
            pgc = new ChapterInfo()
            {
                Chapters = new List<Chapter>(),
                FramesPerSecond = 0,
                LangCode = string.Empty
            };
		}

        private void ChapterCreator_Load(object sender, EventArgs e)
        {
            if (VistaStuff.IsVistaOrNot)
                VistaStuff.SetWindowTheme(chapterListView.Handle, "explorer", null);
            if (minimumTitleLength.Maximum >= MainForm.Instance.Settings.ChapterCreatorMinimumLength &&
                minimumTitleLength.Minimum <= MainForm.Instance.Settings.ChapterCreatorMinimumLength)
                minimumTitleLength.Value = MainForm.Instance.Settings.ChapterCreatorMinimumLength;
            else
                minimumTitleLength.Value = 900;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            MainForm.Instance.Settings.ChapterCreatorMinimumLength = (int)minimumTitleLength.Value;
            if (player != null)
                player.Close();
            base.OnClosing(e);
        }
		#endregion
		#region helper methods
		/// <summary>
		/// shows an array of chapters in the GUI
		/// </summary>
		/// <param name="chaps">the chapters to be shown</param>
		private void showChapters(Chapter[] chaps)
		{
			this.chapterListView.Items.Clear();
			foreach (Chapter chap in chaps)
			{
				ListViewItem item = new ListViewItem(new string[] {chap.timecode, chap.name});
				item.Tag = chap;
				chapterListView.Items.Add(item);
                if (item.Index % 2 != 0)
                    item.BackColor = Color.White;
                else
                    item.BackColor = Color.FromArgb(255, 225, 235, 255);
			}
		}

        private void FreshChapterView()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.chapterListView.Items.Clear();
                //fill list
                foreach (Chapter c in pgc.Chapters)
                {
                    ListViewItem item = new ListViewItem(new string[] { c.Time.ToString(), c.Name });
                    chapterListView.Items.Add(item);
                    if (item.Index % 2 != 0)
                        item.BackColor = Color.White;
                    else
                        item.BackColor = Color.FromArgb(255, 225, 235, 255);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { this.Cursor = Cursors.Default; }
        }

        private void updateTimeLine()
        {
            for (int i = 0; i < chapterListView.Items.Count; i++)
            {
                if (chapterListView.Items[i].SubItems[0].Text.Length == 8)
                    chapterListView.Items[i].SubItems[0].Text = chapterListView.Items[i].SubItems[0].Text + ".000";
                else
                    chapterListView.Items[i].SubItems[0].Text = chapterListView.Items[i].SubItems[0].Text.Substring(0, 12);
            }
        }
		#endregion
		#region buttons
		private void removeZoneButton_Click(object sender, System.EventArgs e)
		{
            if (chapterListView.Items.Count < 1 || pgc.Chapters.Count < 1) return;
            if (chapterListView.SelectedIndices.Count == 0) return;
            intIndex = chapterListView.SelectedIndices[0];
            pgc.Chapters.Remove(pgc.Chapters[intIndex]);
            if (intIndex != 0) intIndex--;
            FreshChapterView();
            updateTimeLine();
		}

		private void clearZonesButton_Click(object sender, System.EventArgs e)
		{
            pgc.Chapters.Clear();
            FreshChapterView();
            intIndex = 0;
		}

		private void chapterListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (chapterListView.Items.Count < 1) return;

            chapterName.TextChanged -= new System.EventHandler(this.chapterName_TextChanged);
            startTime.TextChanged -= new System.EventHandler(this.chapterName_TextChanged);            
            ListView lv = (ListView)sender;

            if (lv.SelectedItems.Count == 1) intIndex = lv.SelectedItems[0].Index;
            if (pgc.Chapters.Count > 0)
            {
                this.startTime.Text = FileUtil.ToShortString(pgc.Chapters[intIndex].Time);
                this.chapterName.Text = pgc.Chapters[intIndex].Name;
            }

            chapterName.TextChanged += new System.EventHandler(this.chapterName_TextChanged);
            startTime.TextChanged += new System.EventHandler(this.chapterName_TextChanged); 
		}

		private void addZoneButton_Click(object sender, System.EventArgs e)
		{
            Chapter c;
            if (chapterListView.Items.Count != 0)
                 intIndex = chapterListView.Items.Count;
            else intIndex = 0;
            TimeSpan ts = new TimeSpan(0);
            try
            {//try to get a valid time input					
                 ts = TimeSpan.Parse(startTime.Text);
            }
            catch (Exception parse)
            { //invalid time input
                startTime.Focus();
                startTime.SelectAll();
                MessageBox.Show("Cannot parse the timecode you have entered.\nIt must be given in the hh:mm:ss.ccc format"
                                + Environment.NewLine + parse.Message, "Incorrect timecode", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //create a new chapter
            c = new Chapter() { Time = ts, Name = chapterName.Text };
            pgc.Chapters.Insert(intIndex, c);
            FreshChapterView();
            updateTimeLine();
		}
		#endregion
		#region saving files
		private void saveButton_Click(object sender, System.EventArgs e)
		{
            if (String.IsNullOrEmpty(output.Text))
            {
                MessageBox.Show("Please select the output file first", "Configuration Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!bFPSKnown && !rbTXT.Checked)
            {
                if (MessageBox.Show("The FPS value for the input file is unknown. Please make sure that the correct value is selected.\nCurrently " +
                    fpsChooser.Value + " will be applied.\n\nDo you want to continue?", "FPS unknown", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                bFPSKnown = true;
            }

            if (!Directory.Exists(Path.GetDirectoryName(output.Text)))
            {
                btOutput_Click(null, null);
                if (!Directory.Exists(Path.GetDirectoryName(output.Text)))
                    return;
            }

            if (FileUtil.IsDirWriteable(Path.GetDirectoryName(output.Text)))
            {
                pgc.FramesPerSecond = (double)fpsChooser.Value;
                if (rbQPF.Checked)
                    pgc.SaveQpfile(output.Text);
                else if (rbXML.Checked)
                    pgc.SaveXml(output.Text);
                else
                    pgc.SaveText(output.Text);
                if (this.closeOnQueue.Checked)
                    this.Close();
            }
            else
                MessageBox.Show("MeGUI cannot write to the path " + Path.GetDirectoryName(output.Text) + "\n" +
            "Please select another output path to save your file.", "Configuration Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

        private void btOutput_Click(object sender, EventArgs e)
        {
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = "Chapter Files (*.txt)|*.txt";
            if (rbXML.Checked)
            {
                saveFileDialog.DefaultExt = "xml";
                saveFileDialog.Filter = "Matroska Chapters files (*.xml)|*.xml";
            }
            else if (rbQPF.Checked)
            {
                saveFileDialog.DefaultExt = "qpf";
                saveFileDialog.Filter = "x264 qp Files (*.qpf)|*.qpf";
            }
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = output.Text;

            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!FileUtil.IsDirWriteable(Path.GetDirectoryName(saveFileDialog.FileName)))
                    MessageBox.Show("MeGUI cannot write to the path " + Path.GetDirectoryName(saveFileDialog.FileName) + "\n" +
                "Please select another output path to save your file.", "Configuration Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    output.Text = saveFileDialog.FileName;
            }
        }

		#endregion

        private void btInput_Click(object sender, EventArgs e)
        {
            if (rbFromFile.Checked)
            {
                openFileDialog.Filter = "IFO files (*.ifo)|*.ifo|MKV files (*.mkv)|*.mkv|MPLS files (*.mpls)|*.mpls|Text files (*.txt)|*.txt|All supported files (*.ifo,*.mkv,*.mpls,*.txt)|*.ifo;*.mkv;*.mpls;*.txt";
                openFileDialog.FilterIndex = 5;

                if (this.openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    input.Text = openFileDialog.FileName;

                    if (input.Text.ToLowerInvariant().EndsWith("ifo"))
                    {
                        ChapterExtractor ex = new DvdExtractor();
                        using (frmStreamSelect frm = new frmStreamSelect(ex))
                        {
                            if (ex is DvdExtractor)
                                frm.Text = "Select your PGC";
                            else
                                frm.Text = "Select your Playlist";
                            ex.GetStreams(input.Text);
                            if (frm.ChapterCount <= 1 || frm.ShowDialog(this) == DialogResult.OK)
                            {
                                if (frm.ChapterCount == 0)
                                {
                                    if (MainForm.Instance.Settings.ChapterCreatorMinimumLength > 0)
                                        MessageBox.Show("No titles found. Please try to reduce the \"Minimum title length\"");
                                    else
                                        MessageBox.Show("No titles found.");
                                    return;
                                }
                                else
                                {
                                    pgc = frm.SelectedSingleChapterInfo;
                                    if (pgc.FramesPerSecond == 0)
                                    {
                                        MediaInfoFile oInfo = new MediaInfoFile(input.Text);
                                        pgc.FramesPerSecond = oInfo.VideoInfo.FPS;
                                    }
                                    if (String.IsNullOrEmpty(pgc.LangCode))
                                        pgc.LangCode = "und";
                                }
                            }
                        }
                        FreshChapterView();
                        updateTimeLine();
                    }
                    else if (input.Text.ToLowerInvariant().EndsWith("mpls"))
                    {
                        ChapterExtractor ex = new MplsExtractor();
                        pgc = ex.GetStreams(input.Text)[0];
                        FreshChapterView();
                        updateTimeLine();
                    }
                    else if (input.Text.ToLowerInvariant().EndsWith("txt"))
                    {
                        ChapterExtractor ex = new TextExtractor();
                        pgc = ex.GetStreams(input.Text)[0];
                        FreshChapterView();
                        updateTimeLine();
                    }
                    else if (input.Text.ToLowerInvariant().EndsWith("mkv"))
                    {
                        ChapterExtractor ex = new MkvExtractor();
                        pgc = ex.GetStreams(input.Text)[0];
                        FreshChapterView();
                        updateTimeLine();
                    }
                    else
                    {
                        MessageBox.Show("The input file is not supported.");
                        return;
                    }
                }
                else
                    return;
            }
            else
            {
                using (FolderBrowserDialog d = new FolderBrowserDialog())
                {
                    d.ShowNewFolderButton = false;
                    d.Description = "Select DVD, BluRay disc or folder.";
                    if (d.ShowDialog() == DialogResult.OK)
                    {
                        input.Text = d.SelectedPath;
                        try
                        {
                            ChapterExtractor ex =
                              Directory.Exists(Path.Combine(input.Text, "VIDEO_TS")) ?
                              new DvdExtractor() as ChapterExtractor :
                              File.Exists(Path.Combine(input.Text, "VIDEO_TS.IFO")) ?
                              new DvdExtractor() as ChapterExtractor :
                              Directory.Exists(Path.Combine(Path.Combine(input.Text, "BDMV"), "PLAYLIST")) ?
                              new BlurayExtractor() as ChapterExtractor :
                              null;

                            if (ex == null)
                            {
                                MessageBox.Show("The input folder is not supported.");
                                return;
                            }

                            using (frmStreamSelect frm = new frmStreamSelect(ex))
                            {
                                if (ex is DvdExtractor)
                                    frm.Text = "Select your Title";
                                else
                                    frm.Text = "Select your Playlist";
                                ex.GetStreams(input.Text);
                                if (frm.ChapterCount <= 1 || frm.ShowDialog(this) == DialogResult.OK)
                                {
                                    if (frm.ChapterCount == 0)
                                    {
                                        if (MainForm.Instance.Settings.ChapterCreatorMinimumLength > 0)
                                            MessageBox.Show("No titles found. Please try to reduce the \"Minimum title length\"");
                                        else
                                            MessageBox.Show("No titles found.");
                                        return;
                                    }
                                    else
                                    {
                                        pgc = frm.SelectedSingleChapterInfo;
                                        if (pgc.FramesPerSecond == 0)
                                        {
                                            MediaInfoFile oInfo = new MediaInfoFile(input.Text);
                                            pgc.FramesPerSecond = oInfo.VideoInfo.FPS;
                                        }
                                        if (String.IsNullOrEmpty(pgc.LangCode))
                                            pgc.LangCode = "und";
                                    }
                                }
                            }
                            FreshChapterView();
                            updateTimeLine();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                        return;
                }
            }

            if (chapterListView.Items.Count != 0)
                chapterListView.Items[0].Selected = true;

            if (pgc.FramesPerSecond > 0)
            {
                fpsChooser.Value = (decimal)pgc.FramesPerSecond;
                bFPSKnown = true;
            }
            else
                bFPSKnown = false;

            string path = String.Empty;
            if (Directory.Exists(MainForm.Instance.Settings.DefaultOutputDir) && FileUtil.IsDirWriteable(MainForm.Instance.Settings.DefaultOutputDir))
                path = MainForm.Instance.Settings.DefaultOutputDir;
            else if (!String.IsNullOrEmpty(input.Text) && Directory.Exists(Path.GetDirectoryName(input.Text)) && FileUtil.IsDirWriteable(Path.GetDirectoryName(input.Text)))
                path = Path.GetDirectoryName(input.Text);

            string file = String.Empty;
            if (String.IsNullOrEmpty(Path.GetFileNameWithoutExtension(input.Text)))
                file = "Chapter Information.txt";
            else
                file = Path.GetFileNameWithoutExtension(input.Text) + " - Chapter Information.txt";
            if (rbXML.Checked)
                Path.ChangeExtension(file, "xml");
            else if (rbQPF.Checked)
                Path.ChangeExtension(file, "qpf");

            if (String.IsNullOrEmpty(path))
                output.Text = file;
            else
                output.Text = Path.Combine(path, file);
        }

		private void showVideoButton_Click(object sender, System.EventArgs e)
		{
			if (!this.videoInput.Equals(""))
			{
				if (player == null)
				{
					player = new VideoPlayer();
					bool videoLoaded = player.loadVideo(mainForm, videoInput, PREVIEWTYPE.CHAPTERS, false);
					if (videoLoaded)
					{
						player.Closed += new EventHandler(player_Closed);
						player.ChapterSet += new ChapterSetCallback(player_ChapterSet);
						if (introEndFrame > 0)
							player.IntroEnd = this.introEndFrame;
						if (creditsStartFrame > 0)
							player.CreditsStart = this.creditsStartFrame;
                        player.Show();
                        player.SetScreenSize();
                        this.TopMost = player.TopMost = true;
                        if (!mainForm.Settings.AlwaysOnTop)
                            this.TopMost = player.TopMost = false;
					}
					else
						return;
				}
                if (chapterListView.SelectedItems.Count == 1 && chapterListView.SelectedItems[0].Tag != null) // a zone has been selected, show that zone
				{
					Chapter chap = (Chapter)chapterListView.SelectedItems[0].Tag;
					int time = Util.getTimeCode(chap.timecode);
					double framerate = player.Framerate;
                    int frameNumber = Util.convertTimecodeToFrameNumber(time, framerate);
					player.CurrentFrame = frameNumber;

				}
				else // no chapter has been selected.. but if start time is configured, show the frame in the preview
				{
					if (!startTime.Text.Equals(""))
					{
						int time = Util.getTimeCode(startTime.Text);
						double framerate = player.Framerate;
                        int frameNumber = Util.convertTimecodeToFrameNumber(time, framerate);
						player.CurrentFrame = frameNumber;
					}
				}
			}
			else
				MessageBox.Show("Please configure video input first", "No video input found", MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		#region properties
		/// <summary>
		/// sets the video input to be used for a zone preview
		/// </summary>
		public string VideoInput
		{
			set 
			{
				this.videoInput = value;
				showVideoButton.Enabled = true;
			}
		}
		/// <summary>
		/// gets / sets the start frame of the credits
		/// </summary>
		public int CreditsStartFrame
		{
			get {return this.creditsStartFrame;}
			set {creditsStartFrame = value;}
		}
		/// <summary>
		/// gets / sets the end frame of the intro
		/// </summary>
		public int IntroEndFrame
		{
			get {return this.introEndFrame;}
			set {introEndFrame = value;}
		}
		#endregion
		private void player_Closed(object sender, EventArgs e)
		{
			player = null;
		}

		private void player_ChapterSet(int frameNumber)
		{
            string strChapter;
            startTime.Text = Util.converFrameNumberToTimecode(frameNumber, player.Framerate);
            if (chapterListView.SelectedIndices.Count == 0)
                strChapter = "Chapter " + (chapterListView.Items.Count + 1).ToString("00");
            else
                strChapter = "Chapter " + (chapterListView.SelectedIndices[0] + 1).ToString("00");            
            if (!chapterName.Text.Equals(strChapter))
                chapterName.Text = strChapter;
            if (chapterListView.SelectedIndices.Count == 0)
                addZoneButton_Click(null, null);
		}

        private void chapterName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (chapterListView.SelectedIndices.Count == 0)
                    return;
                intIndex = chapterListView.SelectedIndices[0];
                pgc.Chapters[intIndex] = new Chapter()
                {
                    Time = TimeSpan.Parse(startTime.Text),
                    Name = chapterName.Text
                };
                chapterListView.SelectedItems[0].SubItems[0].Text = startTime.Text;
                chapterListView.SelectedItems[0].SubItems[1].Text = chapterName.Text;
            }
            catch (Exception parse)
            { //invalid time input
                startTime.Focus();
                startTime.SelectAll();
                MessageBox.Show("Cannot parse the timecode you have entered.\nIt must be given in the hh:mm:ss.ccc format"
                                + Environment.NewLine + parse.Message, "Incorrect timecode", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void rbTXT_CheckedChanged(object sender, EventArgs e)
        {
            fpsChooser.Visible = lblFPS.Visible = false;
            output.Text = Path.ChangeExtension(output.Text, "txt");
        }

        private void rbQPF_CheckedChanged(object sender, EventArgs e)
        {
            fpsChooser.Visible = lblFPS.Visible = true;
            output.Text = Path.ChangeExtension(output.Text, "qpf");
        }

        private void rbXML_CheckedChanged(object sender, EventArgs e)
        {
            fpsChooser.Visible = lblFPS.Visible = true;
            output.Text = Path.ChangeExtension(output.Text, "xml");
        }

        private void minimumTitleLength_ValueChanged(object sender, EventArgs e)
        {
            MainForm.Instance.Settings.ChapterCreatorMinimumLength = (int)minimumTitleLength.Value;
        }
	}
	public struct Chapter
	{
		public string timecode;
        public TimeSpan StartTime;
		public string name;

        //add-on
        public TimeSpan Time { get; set; }
        public string Name { get; set; }

        //public string Lang { get; set; }
        public override string ToString()
        {
            return Time.ToString() + ": " + Name;
        }
	}

    public class ChapterCreatorTool : MeGUI.core.plugins.interfaces.ITool
    {

        #region ITool Members

        public string Name
        {
            get { return "Chapter Creator"; }
        }

        public void Run(MainForm info)
        {
            ChapterCreator cc = new ChapterCreator(info);
            cc.VideoInput = info.Video.Info.VideoInput;
            cc.CreditsStartFrame = info.Video.Info.CreditsStartFrame;
            cc.IntroEndFrame = info.Video.Info.IntroEndFrame;
            cc.Show();
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlH }; }
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "chapter_creator"; }
        }

        #endregion
    }
}