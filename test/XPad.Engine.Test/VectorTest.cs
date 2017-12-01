using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace XPad.Engine.Test
{
    /// <summary>
    /// Tests for <see cref="Vector"/>.
    /// </summary>
    [TestClass]
    public class VectorTest
    {
        static readonly double Sqrt2 = Math.Sqrt(2.0);

        [TestMethod]
        public void TestConstruction()
        {
            var vector = new Vector(1.0, 2.0);
            Assert.AreEqual(vector.X, 1.0);
            Assert.AreEqual(vector.Y, 2.0);
        }

        [TestMethod]
        public void TestLength()
        {
            var vector = new Vector(1.0, 0.0);
            Assert.AreEqual(vector.Length, 1.0);

            vector = new Vector(0.0, 1.0);
            Assert.AreEqual(vector.Length, 1.0);

            vector = new Vector(1.0, 1.0);
            Assert.AreEqual(vector.Length, Sqrt2);
        }

        [TestMethod]
        public void TestNormalize()
        {
            var vector = new Vector(10.0, 12.0);
            Assert.AreNotEqual(vector.Length, 1.0);

            var normalized = vector.Normalize();
            Assert.AreEqual(normalized.Length, 1.0);
        }

        [TestMethod]
        public void TestDistance()
        {
            var v1 = new Vector(1.0, 2.0);
            var v2 = new Vector(1.0, 3.0);
            Assert.AreEqual(Vector.Distance(v1, v2), 1.0);
            Assert.AreEqual(Vector.Distance(v2, v1), 1.0);

            v1 = new Vector(2.0, 1.0);
            v2 = new Vector(1.0, 2.0);
            Assert.AreEqual(Vector.Distance(v1, v2), Sqrt2);
            Assert.AreEqual(Vector.Distance(v2, v1), Sqrt2);
        }

        [TestMethod]
        public void TestEquals()
        {
            var v1 = new Vector(1.0, 2.0);
            Assert.AreNotEqual(v1, null);

            var v2 = new Vector(1.0, 2.0);
            Assert.IsTrue(v1.Equals(v2 as object));
            Assert.AreEqual(v1, v2);
            Assert.AreEqual(v1, v2);
            Assert.AreEqual(v1.GetHashCode(), v2.GetHashCode());
            Assert.IsTrue(v1 == v2);
            Assert.IsFalse(v1 != v2);

            var v3 = new Vector(3.0, 3.0);
            Assert.IsFalse(v1.Equals(v3 as object));
            Assert.AreNotEqual(v1, v3);
            Assert.AreNotEqual(v2, v3);
            Assert.IsFalse(v1 == v3);
            Assert.IsTrue(v2 != v3);
        }

        [TestMethod]
        public void TestOperators()
        {
            var v1 = new Vector(1.0, 2.0);
            var v2 = new Vector(2.0, 3.0);

            Assert.AreEqual(v1 + v2, new Vector(3.0, 5.0));
            Assert.AreEqual(v2 - v1, new Vector(1.0, 1.0));
            Assert.AreEqual(v1 - v2, new Vector(-1.0, -1.0));

            Assert.AreEqual(v1 * 2.0, new Vector(2.0, 4.0));
        }

        /// <summary>
        /// Tests whether multiplying a vector with N also multiplies the
        /// vectors length by N.
        /// </summary>
        [TestMethod]
        public void TestLengthMultiplication()
        {
            var vector = new Vector(1.0, 1.0);
            var doubled = vector * 2.0;

            Assert.AreEqual(vector.Length * 2.0, doubled.Length);
        }
    }
}
