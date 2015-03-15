using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotGame.Common;

namespace DotGame.Common.Tests
{
    [TestClass]
    public class Vector2Test
    {
        [TestMethod]
        public void TestVector2Equals()
        {
            Vector2 vec1 = new Vector2(4, 8);
            Vector2 vec2 = new Vector2(4, 8);
            Vector2 vec3 = new Vector2(-5, 32);

            Assert.IsTrue(vec1.Equals(vec1));
            Assert.IsTrue(vec2.Equals(vec2));
            Assert.IsTrue(vec3.Equals(vec3));

            Assert.IsTrue(vec1.Equals(vec2));
            Assert.IsTrue(vec2.Equals(vec1));

            Assert.IsFalse(vec1.Equals(vec3));
            Assert.IsFalse(vec2.Equals(vec3));
        }

        [TestMethod]
        public void TestVector2Addition()
        {
            Vector2 vec1 = new Vector2(5, 6);
            Vector2 vec2 = new Vector2(4, 2);
            Vector2 result = new Vector2(9, 8);

            Assert.AreEqual(result, vec1 + vec2);
            Assert.AreEqual(result, vec2 + vec1);
        }

        [TestMethod]
        public void TestVector2Subtraction()
        {
            Vector2 vec1 = new Vector2(5, 6);
            Vector2 vec2 = new Vector2(4, 2);
            Vector2 result1 = new Vector2(1, 4);
            Vector2 result2 = new Vector2(-1, -4);

            Assert.AreEqual(result1, vec1 - vec2);
            Assert.AreEqual(result2, vec2 - vec1);
        }

        [TestMethod]
        public void TestVector2Methods()
        {
            Vector2 vec1 = new Vector2(4, 4);
            Vector2 vec2 = Vector2.UnitX * 3;

            Assert.AreEqual(Vector2.Clamp(vec1, Vector2.Zero, Vector2.UnitY), Vector2.UnitY);
            Assert.AreEqual(Vector2.Clamp(vec2, Vector2.One, new Vector2(3, 2)), new Vector2(3, 1));

            Assert.AreEqual(Vector2.Dot(new Vector2(1, 0), new Vector2(0, 1)), 0);
        }
    }
}
