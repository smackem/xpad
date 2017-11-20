using System;
using System.Text;

namespace XPad.Engine
{
    /// <summary>
    /// Base class of all instructions.
    /// </summary>
    public abstract class Instruction
    {
        /// <summary>
        /// When implemented in a derived class, accepts a visitor. See OO pattern Visitor.
        /// </summary>
        /// <typeparam name="TState">Generic state to pass to the visiting method.</typeparam>
        /// <typeparam name="TResult">Generic result of the visiting method.</typeparam>
        /// <param name="visitor">The visitor to accept.</param>
        /// <param name="state">The state to pass to the visiting method.</param>
        /// <returns>Whatever the visiting method returns.</returns>
        public abstract TResult Accept<TState, TResult>(IInstructionVisitor<TState, TResult> visitor, TState state);

        /// <summary>
        /// When implemented in a derived class, advances to the next step in
        /// instruction execution, potentially mutating the passed <see cref="CritterState"/>.
        /// </summary>
        /// <param name="critterState">The critter state to work on.</param>
        /// <returns><code>true</code> if more execution steps must be executed
        /// to finish the instruction execution. <code>false</code> if
        /// instruction execution has finished.</returns>
        public abstract bool Tick(CritterState critterState);
    }
}
