using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MeGUI.core.util;

namespace MeGUI.test.bugreports
{
    [TestFixture]
    public class Bug1813777 : AssertionHelper
    {
        private string ColorBars = 
@"ColorBars(width=640, height=480)
AssumeFPS(25)
Trim(0, -1500)
";

        private void testCropping(string file, int left, int right, int top, int bottom)
        {
            using (AvsFile f = AvsFile.ParseScript(file))
            {
                IVideoReader r = f.GetVideoReader();
                CropValues c = Autocrop.autocrop(r);

                Expect(c.left, EqualTo(left));
                Expect(c.right, EqualTo(right));
                Expect(c.top, EqualTo(top));
                Expect(c.bottom, EqualTo(bottom));
            }
        }

        [Test]
        public void CropsNothing()
        {
            testCropping(ColorBars, 0, 0, 0, 0);
        }

        [Test]
        public void CropsBorder()
        {
            testCropping(ColorBars + "AddBorders(4, 7, 3, 0)", 4, 4, 8, 0);
        }
    }
}
