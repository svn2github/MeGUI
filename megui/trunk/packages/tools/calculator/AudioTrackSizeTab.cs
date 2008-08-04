using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MeGUI.core.details;
using MeGUI.core.util;

namespace MeGUI.packages.tools.calculator
{
    public partial class AudioTrackSizeTab : UserControl
    {
        private bool updating = false;

        public AudioTrackSizeTab()
        {
            InitializeComponent();
            this.audio1Type.Items.AddRange(ContainerManager.AudioTypes.ValuesArray);

            DragDropUtil.RegisterSingleFileDragDrop(this, selectAudioFile, this.filter);
        }

        public AudioJob Job
        {
            set
            {
                audio1Bitrate.Value = value.Settings.Bitrate;
                if (value.Type != null && audio1Type.Items.Contains(value.Type))
                    audio1Type.SelectedItem = value.Type;
            }
        }

        private long length;
        public long PlayLength
        {
            get { return length; }
            set {
                length = value;
                audio1Bitrate_ValueChanged(null, null);
            }
        }

        private void audio1Bitrate_ValueChanged(object sender, EventArgs e)
        {
            if (length <= 0)
                return;

            if (updating)
                return;
            
            updating = true;

            int bitrate = (int)audio1Bitrate.Value;
            if (bitrate > 0 && audio1Type.SelectedIndex == -1)
                audio1Type.SelectedItem = AudioType.VBRMP3;
            double bytesPerSecond = (double)bitrate * 1000.0 / 8.0;
            FileSize f = new FileSize((ulong)(length * bytesPerSecond));
            size.CertainValue = f;
            raiseEvent();
            updating = false;
        }

        private void raiseEvent()
        {
            if (SomethingChanged != null)
                SomethingChanged(this, EventArgs.Empty);
        }

        private void clearAudio1Button_Click(object sender, EventArgs e)
        {
            audio1Bitrate.Value = 0;
            size.CertainValue = FileSize.Empty;
            audio1Type.SelectedIndex = -1;
        }

        private void selectAudioFile(string file)
        {
            FileSize f = FileSize.Of2(file) ?? FileSize.Empty;
            size.CertainValue = f;

            AudioType aud2Type = VideoUtil.guessAudioType(file);
            if (audio1Type.Items.Contains(aud2Type))
                audio1Type.SelectedItem = aud2Type;

        }

        private readonly string filter = VideoUtil.GenerateCombinedFilter(ContainerManager.AudioTypes.ValuesArray);

        private void selectAudio1Button_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = this.filter;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectAudioFile(openFileDialog.FileName);
            }
        }

        public AudioBitrateCalculationStream Stream
        {
            get
            {
                if (audio1Type.SelectedIndex > -1)
                {
                    AudioBitrateCalculationStream stream = new AudioBitrateCalculationStream();
                    stream.Type = stream.AType = audio1Type.SelectedItem as AudioType;
                    stream.Size = size.CertainValue;
                    return stream;
                }
                return null;
            }
        }

        public event EventHandler SomethingChanged;

        private void size_SelectionChanged(object sender, string val)
        {
            if (length <= 0)
                return;

            if (updating)
                return;

            updating = true;

            FileSize s = size.CertainValue;
            if (s > FileSize.Empty && audio1Type.SelectedIndex == -1)
                audio1Type.SelectedItem = AudioType.VBRMP3;

            double bytesPerSecond = (double)s.Bytes / (double)length;
            int bitrate = (int)(bytesPerSecond * 8.0 / 1000.0);

            if (bitrate > audio1Bitrate.Maximum)
                audio1Bitrate.Maximum = bitrate;

            audio1Bitrate.Value = bitrate;
            raiseEvent();
            
            updating = false;
        }

        private void audio1Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool en = audio1Type.SelectedIndex > -1;
            audio1Bitrate.Enabled = en;
            size.Enabled = en;

            raiseEvent();
        }

    }
}
