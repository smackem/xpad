using System;
using System.Collections.Generic;

namespace XPad.Engine
{
    /// <summary>
    /// Loops a number of times over a set of nested instructions.
    /// </summary>
    /// <remarks>
    /// This class is not thread-safe.
    /// </remarks>
    public sealed class LoopInstruction : InstructionContainer
    {
        int count;

        /// <summary>
        /// Initializes a new instance of <see cref="LoopInstruction"/>.
        /// </summary>
        /// <param name="instructions">The nested instructions to loop over.</param>
        /// <param name="repetitions">The number of times to loop. Must be greater
        /// or equal than 0.</param>
        public LoopInstruction(IEnumerable<Instruction> instructions, int repetitions)
            : base(instructions)
        {
            if (repetitions < 0)
                throw new ArgumentOutOfRangeException($"{nameof(repetitions)} must not be less than 0.");

            Repetitions = repetitions;
        }

        /// <summary>
        /// Gets the number of times to loop.
        /// </summary>
        public int Repetitions { get; }

        public override TResult Accept<TState, TResult>(IInstructionVisitor<TState, TResult> visitor, TState state)
        {
            return visitor.Visit(this, state);
        }

        /// <summary>
        /// See <see cref="Instruction.Tick(CritterState)"/>.
        /// </summary>
        public override bool Tick(CritterState critterState)
        {
            if (base.InternalTick(critterState) == false)
            {
                base.ResetPC();
                this.count++;
                if (this.count >= Repetitions)
                {
                    this.count = 0;
                    return false;
                }
            }

            return true;
        }
    }
}
