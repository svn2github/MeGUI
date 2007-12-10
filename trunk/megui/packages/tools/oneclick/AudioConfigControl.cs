using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.details.video;
using MeGUI.core.util;

namespace MeGUI.packages.tools.oneclick
{
    public partial class AudioConfigControl : UserControl
    {
        MainForm mainForm = MainForm.Instance;

        #region Audio profiles
        private void initAudioHandler()
        {
            audioProfile.Manager = mainForm.Profiles;
        }

        void audioCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            raiseEvent();
        }
        #endregion

        private void ProfileChanged(object o, EventArgs e)
        {
            raiseEvent();
        }

        public event EventHandler SomethingChanged;

        private void dontEncodeAudio_CheckedChanged(object sender, EventArgs e)
        {
            bool aChecked = dontEncodeAudio.Checked;
            audioProfile.Enabled = !aChecked;
            raiseEvent();
        }

        private void raiseEvent()
        {
            if (SomethingChanged != null) SomethingChanged(this, null);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AudioCodecSettings Settings
        {
            get { return (AudioCodecSettings)audioProfile.SelectedProfile.BaseSettings; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DelayEnabled
        {
            set { delay.Enabled = value; }
            get { return delay.Enabled; }
        }
        
        public void openAudioFile(string p)
        {
            delay.Value = PrettyFormatting.getDelayAndCheck(p) ?? 0;
        }

        public AudioConfigControl()
        {
            InitializeComponent();
        }

        internal void initHandler()
        {
            initAudioHandler();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DontEncode
        {
            get
            {
                return dontEncodeAudio.Checked;
            }
            set
            {
                dontEncodeAudio.Checked = value;
            }
        }

        public void SelectProfileNameOrWarn(string fqname)
        {
            audioProfile.SetProfileNameOrWarn(fqname);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Delay
        {
            get
            {
                return (int)delay.Value;
            }
            set
            {
                delay.Value = value;
            }
        }
    }
}
