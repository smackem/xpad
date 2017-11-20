using System;
using System.Collections.Generic;
using System.Text;

namespace XPad.Engine
{
    /// <summary>
    /// An instruction with a custom Tick method. Useful for op-code injection
    /// for debugging or testing.
    /// </summary>
    public sealed class CustomInstruction : Instruction
    {
        readonly Func<CritterState, bool> tick;

        /// <summary>
        /// Initializes a new instance of <see cref="CustomInstruction"/>.
        /// </summary>
        /// <param name="tick">The function to call on Tick.</param>
        public CustomInstruction(Func<CritterState, bool> tick)
        {
            this.tick = tick ?? throw new ArgumentNullException(nameof(tick));
        }

        public override TResult Accept<TState, TResult>(IInstructionVisitor<TState, TResult> visitor, TState state)
        {
            return visitor.Visit(this, state);
        }

        public override bool Tick(CritterState critterState)
        {
            return this.tick(critterState);
        }
    }
}
