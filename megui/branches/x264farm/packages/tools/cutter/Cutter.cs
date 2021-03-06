using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.util;

namespace MeGUI.packages.tools.cutter
{
    public partial class Cutter : Form
    {
        private bool cutsAdded = false;

        private VideoPlayer player;
        private Cuts cuts =  new Cuts(TransitionStyle.FADE);
        private string scriptName;

        public Cutter(MainForm mainForm, string scriptName)
        {
            InitializeComponent();
            this.scriptName = scriptName;

            transitionStyle.DataSource = EnumProxy.CreateArray(typeof(TransitionStyle));
            transitionStyle.BindingContext = new BindingContext();

            avsScript.Text += scriptName;

            openPreview(mainForm);
        }

        private void openPreview(MainForm mainForm)
        {
            player = new VideoPlayer();
            bool videoLoaded = player.loadVideo(mainForm, scriptName, PREVIEWTYPE.ZONES, false);
            if (!videoLoaded) return;

            startFrame.Maximum = endFrame.Maximum = player.Reader.FrameCount - 1;
            cuts.Framerate = player.Framerate;
            avsScript.Text += "   (" + cuts.Framerate.ToString("##.###") + "FPS)";

            player.AllowClose = false;
            player.ZoneSet += new ZoneSetCallback(player_ZoneSet);
            player.Show();
        }

        /// <summary>
        /// handler for the ZoneSet event
        /// updates zone start / end and adds the zone
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void player_ZoneSet(int start, int end)
        {
            startFrame.Value = start;
            endFrame.Value = end;
            addZoneButton_Click(null, null);
        }

        private void addZoneButton_Click(object sender, EventArgs e)
        {
            if (addSelectedSection())
                updateListView();
        }

        private bool addSelectedSection()
        {
            CutSection s = new CutSection();
            s.startFrame = (int)startFrame.Value;
            s.endFrame = (int)endFrame.Value;

            if (s.endFrame <= s.startFrame)
            {
                MessageBox.Show("The end frame must be larger than the start frame", "Invalid section configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (!cuts.addSection(s))
            {
                MessageBox.Show("Section overlaps with another", "Invalid configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            return true;
        }

        private void updateListView()
        {
            sections.BeginUpdate();
            sections.Items.Clear();
            foreach (CutSection cut in cuts.AllCuts)
            {
                ListViewItem item = new ListViewItem(new string[] { cut.startFrame.ToString(), cut.endFrame.ToString() });
                item.Tag = cut;
                sections.Items.Add(item);
            }
            sections.EndUpdate();
        }

        private void clearZonesButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all sections?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                 != DialogResult.Yes)
                return;

            cuts.Clear();
            updateListView();
        }

        private void updateZoneButton_Click(object sender, EventArgs e)
        {
            if (sections.SelectedItems.Count != 1)
            {
                MessageBox.Show("You must select exactly one section to update", "Bad selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Cuts old = cuts.clone();
            removeSelectedZones();
            
            if (addSelectedSection())
                updateListView();
            else
                cuts = old;
        }

        private void removeSelectedZones()
        {
            foreach (ListViewItem item in sections.SelectedItems)
            {
                cuts.remove((CutSection)item.Tag);
            }
        }

        private void removeZoneButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected sections?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                != DialogResult.Yes)
                return;
            removeSelectedZones();
            updateListView();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            player.AllowClose = true;
            player.Close();
            player = null;
            base.OnClosing(e);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void transitionStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            cuts.Style = (TransitionStyle)transitionStyle.SelectedIndex;
        }

        private void addCutsToScript_Click(object sender, EventArgs e)
        {
            if (cutsAdded)
            {
                MessageBox.Show("Cuts already added; can't cut again", "Cuts already added", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cuts.AllCuts.Count == 0)
            {
                MessageBox.Show("At least one section must be created", "No sections created", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FilmCutter.WriteCutsToScript(scriptName, cuts, false);
            MessageBox.Show("Cuts written!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cutsAdded = true;
            avsScript.Text += "*";
        }

        private void saveCuts_Click(object sender, EventArgs e)
        {
            if (cuts.AllCuts.Count == 0)
            {
                MessageBox.Show("At least one section must be created", "No sections created", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } 
            
            SaveFileDialog d = new SaveFileDialog();
            d.Filter = "MeGUI cut list (*.clt)|*.clt";
            d.Title = "Select a place to save the cut list";
            if (d.ShowDialog() != DialogResult.OK) return;

            FilmCutter.WriteCutsToFile(d.FileName, cuts);
            MessageBox.Show("Cuts written!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void doAllClose_Click(object sender, EventArgs e)
        {
            if (cuts.AllCuts.Count == 0)
            {
                MessageBox.Show("At least one section must be created", "No sections created", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cutsAdded)
            {
                if (MessageBox.Show("Cuts already added to script, if you have altered cuts the cutfile will not match\n\r Continue?", "Cuts already added", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }
            string savecutsto = scriptName + ".clt";
            if (System.IO.File.Exists(savecutsto))
            {
                DialogResult result = MessageBox.Show("Cutfile already exists, overwrite?", "Overwrite cutfile", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel) return;

                if (result == DialogResult.No)
                {
                    SaveFileDialog d = new SaveFileDialog();
                    d.Filter = "MeGUI cut list (*.clt)|*.clt";
                    d.Title = "Select a place to save the cut list";
                    if (d.ShowDialog() != DialogResult.OK) return;
                    savecutsto = d.FileName;
                }
            }

            if (!cutsAdded)
            {
                FilmCutter.WriteCutsToScript(scriptName, cuts, false);
            }
            FilmCutter.WriteCutsToFile(savecutsto, cuts);
            Close();

            
        }


    }

    public class CutterTool : MeGUI.core.plugins.interfaces.ITool
    {
        #region ITool Members

        public string Name
        {
            get { return "AVS Cutter"; }
        }

        public void Run(MainForm info)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "AviSynth scripts (*.avs)|*.avs";
            d.Title = "Select the input video";
            if (d.ShowDialog() != DialogResult.OK) return;

            (new Cutter(info, d.FileName)).Show();
        }

        public Shortcut[] Shortcuts
        {
            get { return new Shortcut[] { Shortcut.CtrlD }; }
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