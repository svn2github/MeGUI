using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI.packages.reader.dvddecrypter
{
    public class DVDD_FileFactory : IMediaFileFactory
    {
        #region IMediaFileFactory Members

        IMediaFile IMediaFileFactory.Open(string file)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        int IMediaFileFactory.HandleLevel(string file)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IIDable Members

        string IIDable.ID
        {
            get { return "DVDDecrypterFile"; }
        }

        #endregion
    }
   
    class DVDD_File
    {
    }
}
