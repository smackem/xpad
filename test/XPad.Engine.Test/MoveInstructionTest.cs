using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace XPad.Engine.Test
{
    /// <summary>
    /// Tests for <see cref="MoveInstruction"/>.
    /// </summary>
    [TestClass]
    public class MoveInstructionTest
    {
        [TestMethod]
        public void TestMoveAlongYAxis()
        {
            InternalTest(new Vector(0.0, 0.0), new Vector(0.0, 100.0));
        }

        [TestMethod]
        public void TestMoveAcross()
        {
            InternalTest(new Vector(0.0, 0.0), new Vector(100.0, 100.0));
        }

        [TestMethod]
        public void TestMoveSkewed()
        {
            InternalTest(new Vector(0.0, 0.0), new Vector(150.0, 100.0));
        }

        [TestMethod]
        public void TestMoveLong()
        {
            InternalTest(new Vector(1000.123, -700.123), new Vector(-450.7, 100.6));
        }

        [TestMethod]
        public void TestMoveVeryShort()
        {
            // if steplength > sqrt2 -> already there!
            InternalTest(new Vector(0, 0), new Vector(1, 1));
        }

        [TestMethod]
        public void TestMoveShort()
        {
            InternalTest(new Vector(0, 0), new Vector(10, 10));
        }

        /// <summary>
        /// Test case: origin and destination are equal.
        /// </summary>
        [TestMethod]
        public void TestMoveAlreadyThere()
        {
            InternalTest(new Vector(0, 0), new Vector(0, 0));
        }

        void InternalTest(Vector origin, Vector dest)
        {
            var critterState = new CritterState
            {
                PositionX = origin.X,
                PositionY = origin.Y,
            };

            var instr = new MoveInstruction(dest.X, dest.Y);
            var ticks = 0;

            for (; ticks < 1000; ticks++)
            {
                var finished = instr.Tick(critterState) == false;
                var pos = new Vector(critterState.PositionX, critterState.PositionY);
                Console.WriteLine($"pos: {pos}");

                if (finished)
                {
                    Assert.IsTrue(Vector.Distance(pos, dest) < MoveInstruction.StepLength);
                    break;
                }
                else
                {
                    Assert.AreNotEqual(pos, origin);
                    Assert.IsTrue(Vector.Distance(pos, dest) < Vector.Distance(origin, dest));
                }
            }

            Console.WriteLine($"finished after {ticks + 1} ticks");
            if (ticks >= 1000)
                Assert.Fail("Too many ticks");
        }
    }
}
