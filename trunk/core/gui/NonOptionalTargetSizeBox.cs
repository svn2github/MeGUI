using System;
using System.Collections.Generic;
using System.Text;
using MeGUI.core.util;
using System.Diagnostics;

namespace MeGUI.core.gui
{
    public class NonOptionalTargetSizeBox : TargetSizeSCBox
    {
        public NonOptionalTargetSizeBox()
            : base()
        {
            base.StandardItems = TargetSizeSCBox.PredefinedFilesizes;
        }

        /// <summary>
        /// Gets / sets the target, or null if the user doesn't care about filesize
        /// </summary>
        FileSize CertainValue
        {
            get 
            {
                return base.Value.Value;
            }

            set
            {
                base.Value = value;
            }
        }
    }
}
