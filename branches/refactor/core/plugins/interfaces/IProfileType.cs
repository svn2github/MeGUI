using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI.core.plugins.interfaces
{
    interface IProfileType : IIDable
    {
        Type ProfileType { get;}
        string FolderName { get;}
    }
}
