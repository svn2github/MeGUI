using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.details;

namespace MeGUI.core.gui
{
    public partial class AudioEncodingWindow : Form
    {
        public static readonly IDable<ReconfigureJob> Configurer = new IDable<ReconfigureJob>(
            "audio_reconfigure", delegate(Job j)
        {
            if (!(j is AudioJob)) return null;

            AudioEncodingWindow w = new AudioEncodingWindow();
            w.audioEncodingTab1.AudioJob = (AudioJob)j;

            if (w.ShowDialog() == DialogResult.OK)
                j = w.audioEncodingTab1.AudioJob;

            MainForm.Instance.Audio.RefreshProfiles();

            return j;
        });

        public AudioEncodingWindow()
        {
            InitializeComponent();
            audioEncodingTab1.InitializeDropdowns();
            audioEncodingTab1.QueueJob = delegate(AudioJob j)
            {
                this.DialogResult = DialogResult.OK;
            };
        }
    }
}