using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace MeGUI.test
{
    [SetUpFixture]
    public class StandardSetup : AssertionHelper
    {
        public static string ColorBarsAvs = null;

        private List<string> filesToOpen = new List<string>();


        [SetUp]
        public void SetUp()
        {
            CreateColorBarsAvs();
            TryToOpenFiles();
        }

        private void TryToOpenFiles()
        {
            foreach (string f in filesToOpen)
                using (AvsFile v = AvsFile.ParseScript(f))
                {
                    IVideoReader r = v.GetVideoReader();
                }
        }

        private void CreateColorBarsAvs()
        {
            ColorBarsAvs = 
@"ColorBars(width=640, height=480)
AssumeFPS(25)
Trim(0, -1500)
";
            filesToOpen.Add(ColorBarsAvs);
        }
    }
}
