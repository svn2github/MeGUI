using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

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
            this.chapterName.Text = "Chapter1";
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
            this.openFileDialog.Filter = "Chapter Files (*.txt)|*.txt";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Chapter Files (*.txt)|*.txt";
            // 
            // ChapterCreator
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(474, 392);
            this.Controls.Add(this.chaptersGroupbox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChapterCreator";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MeGUI - Chapter Creator";
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
		
		/// <summary>
		/// converts a string timecode into number of milliseconds
		/// </summary>
		/// <param name="timecode">the time string to be analyzed</param>
		/// <returns>the time in milliseconds from the string or -1 if there was an error parsing</returns>
		private int getTimeCode(string timecode)
		{
			if (timecode.Equals(""))
				return -1;
			else if (timecode.Length == 12) // must be 12 chars
			{
				char[] separator = new char[] {':'};
				string[] subItems = timecode.Split(separator);
				if (subItems.Length == 3)
				{
					int hours, minutes, seconds, milliseconds;
					try
					{
						hours = Int32.Parse(subItems[0]);
						minutes = Int32.Parse(subItems[1]);
						separator = new char[] {'.'};
						string[] str = subItems[2].Split(separator);
						if (str.Length == 2)
						{
							seconds = Int32.Parse(str[0]);
							milliseconds = Int32.Parse(str[1]);
						}
						else
							return -1;
						if (hours > 24 || minutes > 59 || seconds > 59)
							return -1;
						int retval = milliseconds + seconds * 1000 + minutes * 60 * 1000 + hours * 60 * 60 * 1000;
						return retval;
					}
					catch (Exception e) // integer parsing error
					{
						Console.Write(e.Message);
						return -1;
					}
				}
				else
					return -1;
			}
			else // incorrect length
				return -1;
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
				int timecode = getTimeCode(startTime.Text);
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
			if (chapterListView.SelectedIndices.Count != 0)
			{
				if (chapterListView.SelectedIndices.Count == 1)
				{
					ListViewItem item = chapterListView.SelectedItems[0];
					Chapter chap = (Chapter)item.Tag;
					startTime.Text = chap.timecode;
					chapterName.Text = chap.name;
				}
			}
		}
		private void addZoneButton_Click(object sender, System.EventArgs e)
		{
			int timecode = getTimeCode(startTime.Text);
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
					int chapTime = getTimeCode(chap.timecode);
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
				Chapter[] newChapters = loadChapterFile(openFileDialog.FileName);
				if (newChapters != null)
				{
					this.chapters = newChapters;
					this.showChapters(this.chapters);
				}
			}
		}
		/// <summary>
		/// loads chapters from a file
		/// </summary>
		/// <param name="fileName">name of the file containing the chapters to be loaded</param>
		/// <returns>the chapters loaded</returns>
		private Chapter[] loadChapterFile(string fileName)
		{
			StreamReader sr = null;
			string line = null;
			Chapter[] loadedChapters = null;
			try
			{
				sr = new StreamReader(fileName);
				ArrayList chapters = new ArrayList();
				Chapter chap = new Chapter();
				while ((line = sr.ReadLine()) != null)
				{
					if (line.IndexOf("NAME") == -1) // chapter time
					{
						string tc = line.Substring(line.IndexOf("=") + 1);
						chap.timecode = tc;
					}
					if (line.IndexOf("NAME") != -1) // chapter name
					{
						string name = line.Substring(line.IndexOf("=") + 1);
						chap.name = name;
						chapters.Add(chap);
					}
				}
				loadedChapters = new Chapter[chapters.Count];
				int index = 0;
				foreach (object o in chapters)
				{
					chap = (Chapter)o;
					loadedChapters[index] = chap;
					index++;
				}
			}
			catch (Exception f)
			{
				Console.Write(f.Message);
				loadedChapters = null;
			}
			finally
			{
				if (sr != null)
				{
					try
					{
						sr.Close();
					}
					catch (Exception f)
					{
						Console.Write(f.Message);
						loadedChapters = null;
					}
				}
			}
			return loadedChapters;
		}
		private void saveButton_Click(object sender, System.EventArgs e)
		{
			if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				StreamWriter sw = null;
				try
				{	// ANSI System instead of UTF-8 (needed for the mp4 muxer)
					sw = new StreamWriter(saveFileDialog.FileName, false, System.Text.Encoding.Default);
					int index = 1;
					foreach (Chapter chap in chapters)
					{
						string lineIdent = "CHAPTER";
						if (index <= 9)
							lineIdent += "0";
						lineIdent += index;
						sw.WriteLine(lineIdent + "=" + chap.timecode);
						sw.WriteLine(lineIdent + "NAME=" + chap.name);
						index++;
					}
				}
				catch (Exception f)
				{
					Console.Write(f.Message);
				}
				finally
				{
					if (sw != null)
					{
						try
						{
							sw.Close();
						}
						catch (Exception f)
						{
							Console.Write(f.Message);
						}
					}
				}
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
					int time = this.getTimeCode(chap.timecode);
					double framerate = player.Framerate;
					int frameNumber = convertTimecodeToFrameNumber(time, framerate);
					player.CurrentFrame = frameNumber;

				}
				else // no chapter has been selected.. but if start time is configured, show the frame in the preview
				{
					if (!startTime.Text.Equals(""))
					{
						int time = this.getTimeCode(startTime.Text);
						double framerate = player.Framerate;
						int frameNumber = convertTimecodeToFrameNumber(time, framerate);
						player.CurrentFrame = frameNumber;
					}
				}
			}
			else
				MessageBox.Show("Please configure video input first", "No video input found", MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}
		#region timecode <-> frame number conversion routines
		/// <summary>
		/// convers a timecode to a framenumber
		/// </summary>
		/// <param name="timeCode">position in the movie in milliseconds</param>
		/// <param name="framerate">framerate of the movie</param>
		/// <returns>the frame corresponding to the timecode</returns>
		private int convertTimecodeToFrameNumber(int timeCode, double framerate)
		{
			double millisecondsPerFrame = (double)(1000 / framerate);
			double frameNumber = (double)timeCode / millisecondsPerFrame;
			return (int)frameNumber;

		}
		/// <summary>
		/// converts a framenumber into a chapter format compatible timecode given the framerate of the video
		/// </summary>
		/// <param name="frameNumber">the position of the video</param>
		/// <param name="framerate">the framerate of the video</param>
		/// <returns>the chapter compatible timecode</returns>
		private string converFrameNumberToTimecode(int frameNumber, double framerate)
		{
			double millisecondsPerFrame = (double)(1000 / framerate);
			int milliseconds = (int)(millisecondsPerFrame * (double)frameNumber);
			int hours = milliseconds / (3600 * 1000);
			milliseconds -= hours * 3600 * 1000;
			int minutes = milliseconds / (60 * 1000);
			milliseconds -= minutes * 60 * 1000;
			int seconds = milliseconds / 1000;
			milliseconds -= seconds * 1000;
			string retval = "";
			if (hours < 10)
				retval += "0";
			retval += hours + ":";
			if (minutes < 10)
				retval += "0";
			retval += minutes + ":";
			if (seconds < 10)
				retval += "0";
			retval += seconds + ".";
			if (milliseconds < 100)
				retval += "0";
			if (milliseconds < 10)
				retval += "0";
			retval += milliseconds;
			return retval;
		}
		#endregion
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
			string timeCode = converFrameNumberToTimecode(frameNumber, player.Framerate);
			startTime.Text = timeCode;
			chapterName.Text = "";
			addZoneButton_Click(null, null);
		}
	}
	public struct Chapter
	{
		public string timecode;
        public TimeSpan StartTime;
		public string name;
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
