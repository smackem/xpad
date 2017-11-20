using System;
using System.Collections.Generic;

namespace XPad.Engine
{
    /// <summary>
    /// The assembly's main class. The program contains a set of instructions, which
    /// make up the program that is executed against a critter.
    /// When <see cref="Tick(CritterState)"/> is called, the program advances
    /// one step, potentially mutating the passed state of the critter.
    /// </summary>
    /// <remarks>
    /// This class is not thread-safe.
    /// </remarks>
    public sealed class Program
    {
        readonly RootInstruction root;

        /// <summary>
        /// Initializes a new instance of <see cref="Engine.Program"/>.
        /// </summary>
        /// <param name="program">The set of instructions that make up the program to execute.</param>
        public Program(IEnumerable<Instruction> program)
        {
            this.root = new RootInstruction(program);
        }

        /// <summary>
        /// Gets the set of instructions that make up the program to execute.
        /// </summary>
        public IReadOnlyList<Instruction> Instructions => this.root.Instructions;

        /// <summary>
        /// Advances the program by one step, potentially mutating the passed
        /// <see cref="CritterState"/>.
        /// </summary>
        /// <param name="critterState">The state of the critter that is controlled by the program.</param>
        /// <returns><code>true</code> if the program has more steps to execute.
        /// <code>false</code> if the program has finished.</returns>
        public bool Tick(CritterState critterState)
        {
            return this.root.Tick(critterState);
        }

        class RootInstruction : InstructionContainer
        {
            internal RootInstruction(IEnumerable<Instruction> instructions)
                : base(instructions)
            {
            }

            public override TResult Accept<TState, TResult>(IInstructionVisitor<TState, TResult> visitor, TState state)
            {
                throw new InvalidOperationException($"{nameof(Accept)} is not supported for {nameof(RootInstruction)}");
            }

            public override bool Tick(CritterState critterState)
            {
                return base.InternalTick(critterState);
            }
        }
    }
}
