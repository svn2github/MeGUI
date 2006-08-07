using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class MediaFileFactory
    {
        private MainForm mainForm;
        public MediaFileFactory(MainForm mainForm)
        {
            this.mainForm = mainForm;
#warning need to register media file factories
            /*            Register(new AvsFileFactory());
            Register(new d2vFileFactory());
            Register(new MediaInfoFileFactory());*/
        }

        public IMediaFile Open(string file)
        {
            int bestHandleLevel = -1;
            IMediaFile bestMediaFile = null;
            foreach (IMediaFileFactory factory in mainForm.PackageSystem.MediaFileTypes.Values)
            {
                int handleLevel = factory.HandleLevel(file);
                if (handleLevel < 0)
                    continue;
                IMediaFile mFile = factory.Open(file);
                if (mFile != null && handleLevel > bestHandleLevel)
                {
                    bestHandleLevel = handleLevel;
                    bestMediaFile = mFile;
                }
                else if (mFile != null)
                    mFile.Dispose();
            }
            return bestMediaFile;
        }
    }
}
