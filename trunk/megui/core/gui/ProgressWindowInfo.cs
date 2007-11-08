using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI.core.plugins.interfaces
{
    public struct ProgressWindowInfo
    {
        string ProgressLabel;
        string DataLabel;
        string RateLabel;
        string RateUnit;
        string Name;
        public ProgressWindowInfo(string pLabel, string dataLabel, string rateLabel,
            string rateUnit, string name)
        {
            this.ProgressLabel = pLabel;
            this.DataLabel = dataLabel;
            this.RateLabel = rateLabel;
            this.RateUnit = rateUnit;
            this.Name = name;
        }
    }
}
