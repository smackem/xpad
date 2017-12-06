using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPad.Desktop.Application;

namespace XPad.Desktop.Test
{
    [TestClass]
    public class InstructionSourceTest
    {
        [TestMethod]
        public void TestMoveInstructionSourceCompile()
        {
            var source = CreateMoveInstructionSource(100, 100);
            var move = Compile<Engine.MoveInstruction>(source);

            Assert.AreEqual(move.DestX, 100);
            Assert.AreEqual(move.DestY, 100);

            for (int i = 0; i < 10; i++)
            {
                var move2 = source.Compile(); // not modfied => should return same object
                Assert.IsTrue(object.ReferenceEquals(move, move2));
            }
        }

        [TestMethod]
        public void TestMoveInstructionSourceModifiedFlag()
        {
            var source = CreateMoveInstructionSource(100, 100);
            Assert.IsTrue(source.IsModified);

            var move = Compile<Engine.MoveInstruction>(source);
            Assert.IsFalse(source.IsModified);

            source.DestX = 125;
            Assert.IsTrue(source.IsModified);
            move = Compile<Engine.MoveInstruction>(source);
            Assert.IsFalse(source.IsModified);
            Assert.AreEqual(move.DestX, 125);

            source.DestY = 125;
            Assert.IsTrue(source.IsModified);
            move = Compile<Engine.MoveInstruction>(source);
            Assert.IsFalse(source.IsModified);
            Assert.AreEqual(move.DestY, 125);
        }

        [TestMethod]
        public void TestLoopInstructionCompile()
        {
            var source = new LoopInstructionSource
            {
                Repetitions = 20,
            };

            Assert.AreEqual(source.Repetitions, 20);

            var loop = Compile<Engine.LoopInstruction>(source);

            Assert.AreEqual(loop.Repetitions, 20);

            for (int i = 0; i < 10; i++)
            {
                var loop2 = source.Compile(); // not modfied => should return same object
                Assert.IsTrue(object.ReferenceEquals(loop, loop2));
            }
        }

        [TestMethod]
        public void TestLoopInstructionModifiedFlag()
        {
            var source = new LoopInstructionSource
            {
                Repetitions = 20,
            };
            Assert.IsTrue(source.IsModified);
            Assert.AreEqual(20, source.Repetitions);

            var loop = Compile<Engine.LoopInstruction>(source);
            Assert.IsFalse(source.IsModified);
            Assert.AreEqual(20, loop.Repetitions);

            source.Repetitions = 250;
            Assert.IsTrue(source.IsModified);

            loop = Compile<Engine.LoopInstruction>(source);
            Assert.IsFalse(source.IsModified);
            Assert.AreEqual(250, loop.Repetitions);

            // add nested to source -> source should become modified
            var nested = CreateMoveInstructionSource(100, 100);
            source.Sources.Add(nested);
            Assert.IsTrue(source.IsModified);

            // compile source -> nested and source should become unmodified
            source.Compile();
            Assert.IsFalse(source.IsModified);
            Assert.IsFalse(nested.IsModified);

            // modify nested -> nested and source should become modified
            nested.DestX = 250;
            nested.DestY = 250;
            Assert.IsTrue(nested.IsModified);
            Assert.IsTrue(source.IsModified);

            // compile source -> source and nested should become unmodified
            loop = Compile<Engine.LoopInstruction>(source);
            Assert.IsFalse(nested.IsModified);
            Assert.IsFalse(source.IsModified);
            Assert.AreEqual(250, ((Engine.MoveInstruction)loop.Instructions.Single()).DestX);
            Assert.AreEqual(250, ((Engine.MoveInstruction)loop.Instructions.Single()).DestY);

            // remove nested, then compile, then modify nested -> source should remain unmodified
            source.Sources.Remove(nested);
            Assert.IsTrue(source.IsModified);
            source.Compile();
            Assert.IsFalse(source.IsModified);
            nested.DestX = 1666;
            Assert.IsFalse(source.IsModified);
        }

        [TestMethod]
        public void TestNestedLoopInstructionModifiedFlag()
        {
            var root = new LoopInstructionSource();
            var leaf = root;
            for (int i = 0; i < 100; i++)
            {
                var nested = new LoopInstructionSource();
                leaf.Sources.Add(nested);
                leaf = nested;
            }

            root.Compile();
            int count = 0;
            for (var walk = root; walk != null; walk = walk.Sources.OfType<LoopInstructionSource>().FirstOrDefault())
            {
                Assert.IsFalse(walk.IsModified);
                count++;
            }
            Assert.AreEqual(101, count);

            leaf.Repetitions = 123;
            for (var walk = root; walk != null; walk = walk.Sources.OfType<LoopInstructionSource>().FirstOrDefault())
                Assert.IsTrue(walk.IsModified);

            root.Compile();
            for (var walk = root; walk != null; walk = walk.Sources.OfType<LoopInstructionSource>().FirstOrDefault())
                Assert.IsFalse(walk.IsModified);

            leaf.Sources.Add(new MoveInstructionSource());
            for (var walk = root; walk != null; walk = walk.Sources.OfType<LoopInstructionSource>().FirstOrDefault())
                Assert.IsTrue(walk.IsModified);
        }

        // ====================================================================

        MoveInstructionSource CreateMoveInstructionSource(double destX, double destY)
        {
            var source = new MoveInstructionSource
            {
                DestX = destX,
                DestY = destY,
            };

            Assert.AreEqual(source.DestX, 100);
            Assert.AreEqual(source.DestY, 100);

            return source;
        }

        TInstruction Compile<TInstruction>(InstructionSource source)
            where TInstruction : Engine.Instruction
        {
            var instr = source.Compile();
            Assert.IsInstanceOfType(instr, typeof(TInstruction));
            return (TInstruction)instr;
        }
    }
}
