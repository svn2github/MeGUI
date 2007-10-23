using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MeGUI.core.util;

namespace TestSuite.bugreports
{
    [TestFixture]
    public class Bug1818832 : AssertionHelper
    {
        [Test]
        public void TestPrettyFormatting()
        {
            Expect(PrettyFormatting.ExtractWorkingName(@"D:\Encode\Satellite\FileName.mpg"), EqualTo("Filename"));
        }
    }
}
