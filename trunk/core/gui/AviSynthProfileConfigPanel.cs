using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MeGUI.core.gui
{
    public partial class AviSynthProfileConfigPanel : UserControl, MeGUI.core.plugins.interfaces.Gettable<AviSynthSettings>
    {
        public AviSynthProfileConfigPanel(MainForm mainForm, MeGUI.core.details.video.Empty dummy) 
        {
            InitializeComponent();
            this.resizeFilterType.DataSource = ScriptServer.ListOfResizeFilterType;
            this.resizeFilterType.BindingContext = new BindingContext();
            this.noiseFilterType.DataSource = ScriptServer.ListOfDenoiseFilterType;
            this.noiseFilterType.BindingContext = new BindingContext();
//            this.path = mainForm.MeGUIPath;
//            this.pluginsDirectory = MeGUISettings.AvisynthPluginsPath;
        }

        #region Gettable<AviSynthSettings> Members

        public AviSynthSettings Settings
        {
            get
            {
                mod16Method method = (mod16Method)mod16Box.SelectedIndex;
                if (!signalAR.Checked)
                    method = mod16Method.none;
                return new AviSynthSettings(avisynthScript.Text,
                    (ResizeFilterType)(resizeFilterType.SelectedItem as EnumProxy).RealValue,
                    resize.Checked,
                    (DenoiseFilterType)(noiseFilterType.SelectedItem as EnumProxy).RealValue,
                    noiseFilter.Checked,
                    mpeg2Deblocking.Checked,
                    colourCorrect.Checked,
                    method);
            }
            set
            {
                avisynthScript.Text = value.Template;
                resize.Checked = value.Resize;
                resizeFilterType.SelectedItem = EnumProxy.Create(value.ResizeMethod);
                noiseFilterType.SelectedItem = EnumProxy.Create(value.DenoiseMethod);
                noiseFilter.Checked = value.Denoise;
                mpeg2Deblocking.Checked = value.MPEG2Deblock;
                colourCorrect.Checked = value.ColourCorrect;
                signalAR.Checked = (value.Mod16Method != mod16Method.none);
                mod16Box.SelectedIndex = (int)value.Mod16Method;
            }
        }

        #endregion

        #region event handlers
        private void signalAR_CheckedChanged(object sender, EventArgs e)
        {
            mod16Box.Enabled = signalAR.Checked;
        }
        #endregion

        private void insert_Click(object sender, EventArgs e)
        {
            string text = (sender as Control).Tag as string;
            string avsScript = avisynthScript.Text;
			string avsScriptA = avsScript.Substring(0, avisynthScript.SelectionStart);
			string avsScriptB = avsScript.Substring(avisynthScript.SelectionStart + avisynthScript.SelectionLength);
			avisynthScript.Text = avsScriptA + text + avsScriptB;
        }

        private void dllBar_FileSelected(FileBar sender, FileBarEventArgs args)
        {
            avisynthScript.Text = "LoadPlugin(\"" + args.NewFileName + "\")\r\n" + avisynthScript.Text;
        }

        private void noiseFilter_CheckedChanged(object sender, EventArgs e)
        {
            noiseFilterType.Enabled = noiseFilter.Checked;
        }

        private void resize_CheckedChanged(object sender, EventArgs e)
        {
            resizeFilterType.Enabled = resize.Checked;
        }
    }
}
