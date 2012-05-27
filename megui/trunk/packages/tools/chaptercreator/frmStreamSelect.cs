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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class frmStreamSelect : Form
    {
        public frmStreamSelect(ChapterExtractor extractor) : this(extractor, SelectionMode.One)
        {
        }

        public frmStreamSelect(ChapterExtractor extractor, SelectionMode selectionMode)
        {
            InitializeComponent();
            listBox1.SelectionMode = selectionMode;

            extractor.StreamDetected += (sender, arg) =>
            {
                listBox1.Items.Add(arg.ProgramChain);
            };
            extractor.ChaptersLoaded += (sender, arg) =>
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (((ChapterInfo)listBox1.Items[i]).SourceName == arg.ProgramChain.SourceName)
                    {
                        listBox1.Items[i] = arg.ProgramChain;
                        break;
                    }
                }
            };
            extractor.ExtractionComplete += (sender, arg) =>
            {
                btnSortDuration.Checked = true;
            };
        }

        public ChapterInfo SelectedSingleChapterInfo
        {
            get { return listBox1.SelectedItems.Count > 0 ? listBox1.SelectedItem as ChapterInfo : null; }
        }

        public List<ChapterInfo> SelectedMultipleChapterInfo
        {
            get { return listBox1.SelectedItems.Count > 0 ? new List<ChapterInfo>(listBox1.SelectedItems.Cast<ChapterInfo>()) : null; }
        }

        public int ChapterCount
        {
            get { return listBox1.Items.Count; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count >= 1)
                DialogResult = DialogResult.OK;
            else
                MessageBox.Show("Please select a stream", "Selection missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSortName_CheckedChanged(object sender, EventArgs e)
        {
            if (!btnSortName.Checked || listBox1.Items.Count == 0)
                return;

            List<ChapterInfo> oSelectedList = new List<ChapterInfo>(listBox1.SelectedItems.Cast<ChapterInfo>());

            List<ChapterInfo> list = new List<ChapterInfo>(listBox1.Items.Cast<ChapterInfo>());
            list = list.OrderBy(p => (p.Title + p.TitleNumber)).ToList();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(list.ToArray());

            if (oSelectedList.Count > 0)
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    foreach (ChapterInfo oSelectedItem in oSelectedList)
                    {
                        if ((ChapterInfo)listBox1.Items[i] == oSelectedItem)
                        {
                            listBox1.SetSelected(i, true);
                            break;
                        }
                    }
                }
            }
            else
                listBox1.SelectedIndex = 0;
        }

        private void btnSortDuration_CheckedChanged(object sender, EventArgs e)
        {
            if (!btnSortDuration.Checked || listBox1.Items.Count == 0)
                return;

            List<ChapterInfo> oSelectedList = new List<ChapterInfo>(listBox1.SelectedItems.Cast<ChapterInfo>());

            List<ChapterInfo> list = new List<ChapterInfo>(listBox1.Items.Cast<ChapterInfo>());
            list = list.OrderByDescending(p => p.Duration).ToList();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(list.ToArray());

            if (oSelectedList.Count > 0)
            {
                for(int i = 0; i < listBox1.Items.Count; i++)
                {
                    foreach (ChapterInfo oSelectedItem in oSelectedList)
                    {
                        if ((ChapterInfo)listBox1.Items[i] == oSelectedItem)
                        {
                            listBox1.SetSelected(i, true);
                            break;
                        }
                    }
                }
            }
            else
                listBox1.SelectedIndex = 0;
        }
    }
}
