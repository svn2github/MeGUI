// ****************************************************************************
// 
// Copyright (C) 2005-2009  Doom9 & al
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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI
{

	/// <summary>
	/// Summary description for ChapterCreator.
	/// </summary>
	public class ChapterCreator : System.Windows.Forms.Form
	{

		private Chapter[] chapters;
		private string videoInput;
		private VideoPlayer player;
		private int introEndFrame = 0, creditsStartFrame = 0;
        private MainForm mainForm;
        private ChapterInfo pgc;
        private int intIndex;

		private System.Windows.Forms.GroupBox chaptersGroupbox;
		private System.Windows.Forms.Button addZoneButton;
		private System.Windows.Forms.Button clearZonesButton;
		private System.Windows.Forms.Button updateZoneButton;
		private System.Windows.Forms.Button showVideoButton;
		private System.Windows.Forms.Button removeZoneButton;
		private System.Windows.Forms.ColumnHeader timecodeColumn;
		private System.Windows.Forms.ColumnHeader nameColumn;
		private System.Windows.Forms.Label startTimeLabel;
		private System.Windows.Forms.Label chapterNameLabel;
		private System.Windows.Forms.Button loadButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.TextBox startTime;
		private System.Windows.Forms.TextBox chapterName;
		private System.Windows.Forms.ListView chapterListView;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private MeGUI.core.gui.HelpButton helpButton1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#region start / stop
		public ChapterCreator(MainForm mainForm)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			chapters = new Chapter[0];
            this.mainForm = mainForm;
            pgc = new ChapterInfo()
            {
                Chapters = new List<Chapter>(),
                FramesPerSecond = 25.0,
                LangCode = string.Empty
            };
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChapterCreator));
            this.chaptersGroupbox = new System.Windows.Forms.GroupBox();
            this.helpButton1 = new MeGUI.core.gui.HelpButton();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.chapterName = new System.Windows.Forms.TextBox();
            this.chapterNameLabel = new System.Windows.Forms.Label();
            this.chapterListView = new System.Windows.Forms.ListView();
            this.timecodeColumn = new System.Windows.Forms.ColumnHeader();
            this.nameColumn = new System.Windows.Forms.ColumnHeader();
            this.startTime = new System.Windows.Forms.TextBox();
            this.startTimeLabel = new System.Windows.Forms.Label();
            this.addZoneButton = new System.Windows.Forms.Button();
            this.clearZonesButton = new System.Windows.Forms.Button();
            this.updateZoneButton = new System.Windows.Forms.Button();
            this.showVideoButton = new System.Windows.Forms.Button();
            this.removeZoneButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.chaptersGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // chaptersGroupbox
            // 
            this.chaptersGroupbox.Controls.Add(this.helpButton1);
            this.chaptersGroupbox.Controls.Add(this.saveButton);
            this.chaptersGroupbox.Controls.Add(this.loadButton);
            this.chaptersGroupbox.Controls.Add(this.chapterName);
            this.chaptersGroupbox.Controls.Add(this.chapterNameLabel);
            this.chaptersGroupbox.Controls.Add(this.chapterListView);
            this.chaptersGroupbox.Controls.Add(this.startTime);
            this.chaptersGroupbox.Controls.Add(this.startTimeLabel);
            this.chaptersGroupbox.Controls.Add(this.addZoneButton);
            this.chaptersGroupbox.Controls.Add(this.clearZonesButton);
            this.chaptersGroupbox.Controls.Add(this.updateZoneButton);
            this.chaptersGroupbox.Controls.Add(this.showVideoButton);
            this.chaptersGroupbox.Controls.Add(this.removeZoneButton);
            this.chaptersGroupbox.Location = new System.Drawing.Point(8, 8);
            this.chaptersGroupbox.Name = "chaptersGroupbox";
            this.chaptersGroupbox.Size = new System.Drawing.Size(458, 376);
            this.chaptersGroupbox.TabIndex = 23;
            this.chaptersGroupbox.TabStop = false;
            this.chaptersGroupbox.Text = "Chapters";
            // 
            // helpButton1
            // 
            this.helpButton1.ArticleName = "Chapter creator";
            this.helpButton1.AutoSize = true;
            this.helpButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.helpButton1.Location = new System.Drawing.Point(16, 347);
            this.helpButton1.Name = "helpButton1";
            this.helpButton1.Size = new System.Drawing.Size(38, 23);
            this.helpButton1.TabIndex = 41;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(392, 347);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(55, 23);
            this.saveButton.TabIndex = 40;
            this.saveButton.Text = "&Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(329, 347);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(55, 23);
            this.loadButton.TabIndex = 39;
            this.loadButton.Text = "&Load";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // chapterName
            // 
            this.chapterName.Location = new System.Drawing.Point(75, 305);
            this.chapterName.Name = "chapterName";
            this.chapterName.Size = new System.Drawing.Size(306, 21);
            this.chapterName.TabIndex = 38;
            this.chapterName.Text = "Chapter 01";
            this.chapterName.TextChanged += new System.EventHandler(this.chapterName_TextChanged);
            // 
            // chapterNameLabel
            // 
            this.chapterNameLabel.Location = new System.Drawing.Point(13, 308);
            this.chapterNameLabel.Name = "chapterNameLabel";
            this.chapterNameLabel.Size = new System.Drawing.Size(56, 17);
            this.chapterNameLabel.TabIndex = 37;
            this.chapterNameLabel.Text = "Name :";
            // 
            // chapterListView
            // 
            this.chapterListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timecodeColumn,
            this.nameColumn});
            this.chapterListView.FullRowSelect = true;
            this.chapterListView.HideSelection = false;
            this.chapterListView.Location = new System.Drawing.Point(16, 24);
            this.chapterListView.Name = "chapterListView";
            this.chapterListView.Size = new System.Drawing.Size(365, 240);
            this.chapterListView.TabIndex = 36;
            this.chapterListView.UseCompatibleStateImageBehavior = false;
            this.chapterListView.View = System.Windows.Forms.View.Details;
            this.chapterListView.SelectedIndexChanged += new System.EventHandler(this.chapterListView_SelectedIndexChanged);
            // 
            // timecodeColumn
            // 
            this.timecodeColumn.Text = "Timecode";
            this.timecodeColumn.Width = 100;
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 250;
            // 
            // startTime
            // 
            this.startTime.Location = new System.Drawing.Point(75, 274);
            this.startTime.Name = "startTime";
            this.startTime.Size = new System.Drawing.Size(306, 21);
            this.startTime.TabIndex = 23;
            this.startTime.Text = "00:00:00.000";
            // 
            // startTimeLabel
            // 
            this.startTimeLabel.Location = new System.Drawing.Point(13, 277);
            this.startTimeLabel.Name = "startTimeLabel";
            this.startTimeLabel.Size = new System.Drawing.Size(64, 16);
            this.startTimeLabel.TabIndex = 24;
            this.startTimeLabel.Text = "Start Time :";
            // 
            // addZoneButton
            // 
            this.addZoneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addZoneButton.Location = new System.Drawing.Point(392, 241);
            this.addZoneButton.Name = "addZoneButton";
            this.addZoneButton.Size = new System.Drawing.Size(55, 23);
            this.addZoneButton.TabIndex = 33;
            this.addZoneButton.Text = "&Add";
            this.addZoneButton.Click += new System.EventHandler(this.addZoneButton_Click);
            // 
            // clearZonesButton
            // 
            this.clearZonesButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.clearZonesButton.Location = new System.Drawing.Point(392, 24);
            this.clearZonesButton.Name = "clearZonesButton";
            this.clearZonesButton.Size = new System.Drawing.Size(55, 23);
            this.clearZonesButton.TabIndex = 29;
            this.clearZonesButton.Text = "&Clear";
            this.clearZonesButton.Click += new System.EventHandler(this.clearZonesButton_Click);
            // 
            // updateZoneButton
            // 
            this.updateZoneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.updateZoneButton.Location = new System.Drawing.Point(392, 53);
            this.updateZoneButton.Name = "updateZoneButton";
            this.updateZoneButton.Size = new System.Drawing.Size(55, 23);
            this.updateZoneButton.TabIndex = 35;
            this.updateZoneButton.Text = "&Update";
            this.updateZoneButton.Click += new System.EventHandler(this.updateZoneButton_Click);
            // 
            // showVideoButton
            // 
            this.showVideoButton.AutoSize = true;
            this.showVideoButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.showVideoButton.Enabled = false;
            this.showVideoButton.Location = new System.Drawing.Point(392, 82);
            this.showVideoButton.Name = "showVideoButton";
            this.showVideoButton.Size = new System.Drawing.Size(55, 23);
            this.showVideoButton.TabIndex = 34;
            this.showVideoButton.Text = "&Preview";
            this.showVideoButton.Click += new System.EventHandler(this.showVideoButton_Click);
            // 
            // removeZoneButton
            // 
            this.removeZoneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.removeZoneButton.Location = new System.Drawing.Point(392, 212);
            this.removeZoneButton.Name = "removeZoneButton";
            this.removeZoneButton.Size = new System.Drawing.Size(55, 23);
            this.removeZoneButton.TabIndex = 32;
            this.removeZoneButton.Text = "&Remove";
            this.removeZoneButton.Click += new System.EventHandler(this.removeZoneButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "txt";
            this.openFileDialog.Filter = "Blu-ray Playlist Files (*.mpls)|*.mpls|IFO Files (*.ifo)|*.ifo|Chapter Files (*.t" +
                "xt)|*.txt|All Files supported (*.ifo;*.txt;*.mpls)|*.ifo;*.mpls;*.txt";
            this.openFileDialog.FilterIndex = 4;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "x264 qp Files (.qpf)|*.qpf|Chapter Files (*.txt)|*.txt|All supported Files (*.qpf" +
                ";*.txt)|*.qpf;*.txt";
            this.saveFileDialog.FilterIndex = 3;
            // 
            // ChapterCreator
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(474, 392);
            this.Controls.Add(this.chaptersGroupbox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChapterCreator";
            this.ShowInTaskbar = false;
            this.Text = "MeGUI - Chapter Creator";
            this.Load += new System.EventHandler(this.ChapterCreator_Load);
            this.chaptersGroupbox.ResumeLayout(false);
            this.chaptersGroupbox.PerformLayout();
            this.ResumeLayout(false);

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
                    item.BackColor = Color.WhiteSmoke;
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
                    //don't show short last chapter depending on settings
                      if (pgc.Duration != TimeSpan.Zero && 
                      c.Equals(pgc.Chapters.Last()) && (pgc.Duration.Add(-c.Time).TotalSeconds < 10))
                       continue;

                    ListViewItem item = new ListViewItem(new string[] { c.Time.ToString(), c.Name });
                    chapterListView.Items.Add(item);
                    if (item.Index % 2 != 0)
                        item.BackColor = Color.White;
                    else
                        item.BackColor = Color.WhiteSmoke;
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
			if (this.chapterListView.SelectedItems.Count > 0)
			{
				foreach (ListViewItem item in chapterListView.SelectedItems)
				{
					chapterListView.Items.Remove(item);
				}
				Chapter[] newChapters = new Chapter[chapterListView.Items.Count];
				int index = 0;
				foreach (ListViewItem item in chapterListView.Items)
				{
					Chapter chap = (Chapter)item.Tag;
					newChapters[index] = chap;
					index++;
				}
				chapters = newChapters;
				showChapters(chapters);
			}
		}

		private void updateZoneButton_Click(object sender, System.EventArgs e)
		{
			if (this.chapterListView.SelectedIndices.Count == 1)
			{
				ListViewItem item = chapterListView.SelectedItems[0];
				Chapter chap = (Chapter)item.Tag;
				int timecode = Util.getTimeCode(startTime.Text);
				if (timecode < 0)
					MessageBox.Show("You must specify a valid timecode in the format hh:mm:ss.ccc", "Incorrect timecode", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				else
				{
					chap.timecode = startTime.Text;
					chap.name = chapterName.Text;
					chapters[item.Index] = chap;
					this.showChapters(chapters);
				}
			}
		}

		private void clearZonesButton_Click(object sender, System.EventArgs e)
		{
			this.chapterListView.Items.Clear();
			this.chapters = new Chapter[0];
		}

		private void chapterListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            ListView lv = (ListView)sender;

            if (lv.SelectedItems.Count == 1) intIndex = lv.SelectedItems[0].Index;
            if (pgc.Chapters.Count > 0)
            {
                this.startTime.Text = FileUtil.ToShortString(pgc.Chapters[intIndex].Time);
                this.chapterName.Text = pgc.Chapters[intIndex].Name;
            }
		}
		private void addZoneButton_Click(object sender, System.EventArgs e)
		{
			int timecode = Util.getTimeCode(startTime.Text);
			if (timecode >= 0)
			{
				Chapter newChapter = new Chapter();
				newChapter.timecode = startTime.Text;
				newChapter.name = chapterName.Text;
				Chapter[] newChapters = new Chapter[chapters.Length + 1];
				int index = 0, number = 0;
				bool interationAborted = false, chapterInserted = false;
				foreach (Chapter chap in chapters)
				{
					int chapTime = Util.getTimeCode(chap.timecode);
					if (chapTime > timecode) // the new chapter comes before the one we're currently looking at
					{
                        if (!chapterInserted)
                        {
                            if (newChapter.name.Equals("")) // add a default name just in case
                            {
                                number = index + 1;
                                newChapter.name = "Chapter" + number;
                            }
                            newChapters[index] = newChapter;
                            chapterInserted = true;
                            index++;
                            newChapters[index] = chap;
                        }
                        else
                            newChapters[index] = chap;
					}
					else if (chapTime < timecode) // new chapter comes at a later point
						newChapters[index] = chap;
					else // the two chapters match
					{
						MessageBox.Show("The chapter you're trying to add starts at the same point as the\nexisting chapter with name " + chap.name + ".\nYou cannot have two chapters that start at the same time.", 
							"Duplicate chapter detected", MessageBoxButtons.OK, MessageBoxIcon.Stop);
						interationAborted = true;
					}
					index++;
				}
				if (!chapterInserted) // chapter is the last one
				{
					if (newChapter.name.Equals(""))
					{
						number = index + 1;
						newChapter.name = "Chapter" + number;
					}
					newChapters[index] = newChapter;
				}
				if (!interationAborted)
				{
					chapters = newChapters;
					showChapters(newChapters);
				}
			}
			else
				MessageBox.Show("Cannot parse the timecode you have entered.\nIt must be given in the hh:mm:ss.ccc format", "Incorrect timecode", MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}
		#endregion
		#region loading / saving files
		private void loadButton_Click(object sender, System.EventArgs e)
		{
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
			{
                if (openFileDialog.FileName.ToLower().EndsWith("ifo"))
                {
                    ChapterExtractor ex = new IfoExtractor();
                    pgc = ex.GetStreams(openFileDialog.FileName)[0];
                    FreshChapterView();
                    updateTimeLine();
                }
                else if (openFileDialog.FileName.ToLower().EndsWith("mpls"))
                {
                    ChapterExtractor ex = new MplsExtractor();
                    pgc = ex.GetStreams(openFileDialog.FileName)[0];
                    FreshChapterView();
                    updateTimeLine();
                }
                else
                {
                    ChapterExtractor ex = new TextExtractor();
                    pgc = ex.GetStreams(openFileDialog.FileName)[0];
                    FreshChapterView();
                    updateTimeLine();
                }
			}

            if (chapterListView.Items.Count != 0)
                chapterListView.Items[0].Selected = true;
		}

		private void saveButton_Click(object sender, System.EventArgs e)
		{
			if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
			{
                string ext = Path.GetExtension(saveFileDialog.FileName).ToLower();
                if (ext == "qpf")
                    pgc.SaveQpfile(saveFileDialog.FileName);
                else if (ext == "txt")
                    pgc.SaveText(saveFileDialog.FileName);
                else
                    pgc.SaveText(saveFileDialog.FileName);
			}
		}
		#endregion

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
					}
					else
						return;
				}
				if (chapterListView.SelectedItems.Count == 1) // a zone has been selected, show that zone
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
			string timeCode = Util.converFrameNumberToTimecode(frameNumber, player.Framerate);
			startTime.Text = timeCode;
			chapterName.Text = "";
			addZoneButton_Click(null, null);
		}

        private void ChapterCreator_Load(object sender, EventArgs e)
        {

            if (VistaStuff.IsVistaOrNot)
            {
                VistaStuff.SetWindowTheme(chapterListView.Handle, "explorer", null);
            }
        }

        private void chapterName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                intIndex = chapterListView.SelectedIndices[0];
                pgc.Chapters[intIndex] = new Chapter()
                {
                    Time = TimeSpan.Parse(startTime.Text),
                    Name = chapterName.Text
                };
                chapterListView.SelectedItems[0].SubItems[0].Text = startTime.Text;
                chapterListView.SelectedItems[0].SubItems[1].Text = chapterName.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
