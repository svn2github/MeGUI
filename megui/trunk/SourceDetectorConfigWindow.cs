using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MeGUI
{
    public partial class SourceDetectorConfigWindow : Form
    {
        public SourceDetectorConfigWindow()
        {
            InitializeComponent();
        }

        private void portionsAllowed_CheckedChanged(object sender, EventArgs e)
        {
            portionThreshold.Enabled = portionsAllowed.Checked;
            maximumPortions.Enabled = portionsAllowed.Checked;
        }
        public SourceDetectorSettings Settings
        {
            get
            {
                SourceDetectorSettings settings = new SourceDetectorSettings();
                settings.AnalysePercent = (int)analysisPercent.Value;
                settings.HybridFOPercent = (int)hybridFOThreshold.Value;
                settings.HybridPercent = (int)hybridThreshold.Value;
                settings.MinimumAnalyseSections = (int)minAnalyseSections.Value;
                settings.PortionsAllowed = portionsAllowed.Checked;
                if (settings.PortionsAllowed)
                {
                    settings.PortionThreshold = (double)portionThreshold.Value;
                    settings.MaxPortions = (int)maximumPortions.Value;
                }
                settings.Priority = (ThreadPriority)priority.SelectedIndex;
                return settings;
            }
            set
            {
                analysisPercent.Value = value.AnalysePercent;
                hybridFOThreshold.Value = value.HybridFOPercent;
                hybridThreshold.Value = value.HybridPercent;
                minAnalyseSections.Value = value.MinimumAnalyseSections;
                portionsAllowed.Checked = value.PortionsAllowed;
                portionThreshold.Value = (decimal)value.PortionThreshold;
                maximumPortions.Value = value.MaxPortions;
                priority.SelectedIndex = (int)value.Priority;
            }
        }
    }
}