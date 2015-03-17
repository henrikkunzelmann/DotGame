using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DotGame;
using DotGame.Graphics;

namespace DotGame.Common.Tests
{
    [TestClass]
    public class ColorTest
    {
        [TestMethod]
        public void TestColorPack()
        {
            var color1 = Color.FromArgb(255, 15, 135, 183);
            var argb = color1.ToArgb();
            var color2 = Color.FromArgb(argb);
            Assert.AreEqual(color1, color2);
        }
    }
}
