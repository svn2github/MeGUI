using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MeGUI.core.util;

namespace MeGUI.core.details.mux
{
    public partial class MuxStreamControl : UserControl
    {
        public MuxStreamControl()
        {
            InitializeComponent();
            subtitleLanguage.Items.AddRange(new List<string>(LanguageSelectionContainer.Languages.Keys).ToArray());
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MuxStream Stream
        {
            get
            {
                if (string.IsNullOrEmpty(input.Filename))
                    return null;

                string language = null;
                if (subtitleLanguage.Text != null && LanguageSelectionContainer.Languages.ContainsKey(subtitleLanguage.Text))
                    language = LanguageSelectionContainer.Languages[subtitleLanguage.Text];
                return new MuxStream(input.Filename, subtitleLanguage.Text, subName.Text, (int)audioDelay.Value);
            }

            set
            {
                if (value == null)
                {
                    removeSubtitleTrack_Click(null, null);
                    return;
                }

                input.Filename = value.path;
                if (!string.IsNullOrEmpty(value.language))
                    subtitleLanguage.SelectedValue = value.language;
                subName.Text = value.name;
                audioDelay.Value = value.delay;
            }
        }

        private bool showDelay;
        public bool ShowDelay
        {
            set
            {
                showDelay = value;
                delayLabel.Visible = value;
                audioDelay.Visible = value;
                if (!value) audioDelay.Value = 0;
            }
            get
            {
                return showDelay;
            }
        }

        public string Filter
        {
            get { return input.Filter; }
            set { input.Filter = value; }
        }

        public void SetLanguage(string lang)
        {
            subtitleLanguage.SelectedItem = lang;
        }

        private void removeSubtitleTrack_Click(object sender, EventArgs e)
        {
            input.Text = "";
            subtitleLanguage.SelectedIndex = -1;
            subName.Text = "";
            audioDelay.Value = 0;
            raiseEvent();
        }

        private void raiseEvent()
        {
            if (FileUpdated != null)
                FileUpdated(this, new EventArgs());
        }

        public event EventHandler FileUpdated;

        private void input_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            audioDelay.Value = PrettyFormatting.getDelayAndCheck(input.Filename) ?? 0;
            raiseEvent();
        }
    }
}
