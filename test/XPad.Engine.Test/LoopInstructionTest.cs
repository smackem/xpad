using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace XPad.Engine.Test
{
    /// <summary>
    /// Tests for <see cref="LoopInstruction"/>.
    /// </summary>
    [TestClass]
    public class LoopInstructionTest
    {
        /// <summary>
        /// Set up two custom instructions that increment a value until it
        /// matches a certain modulo.
        /// Then loop over these instructions and test if the mutated values
        /// match the expectation.
        /// </summary>
        [TestMethod]
        public void TestLoopInstruction()
        {
            var count1 = 0;
            var count2 = 0;

            var instructions = new List<Instruction>
            {
                new CustomInstruction(_ => ++count1 % 10 != 0),
                new CustomInstruction(_ => ++count2 % 15 != 0),
            };

            var loop = new LoopInstruction(instructions, 10);
            var critterState = new CritterState();

            var count = 0;
            for (; count < 1000; count++)
            {
                if (loop.Tick(critterState) == false)
                    break;
            }

            if (count >= 1000)
                Assert.Fail("Too many ticks");

            Assert.AreEqual(100, count1);
            Assert.AreEqual(150, count2);
        }
    }
}
