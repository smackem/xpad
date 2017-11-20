using System;
using System.Collections.Generic;
using System.Text;

namespace XPad.Engine
{
    /// <summary>
    /// The base class for all instructions that hold nested instructions themselves.
    /// </summary>
    /// <remarks>
    /// This class and its derived classes are not thread-safe.
    /// </remarks>
    public abstract class InstructionContainer : Instruction
    {
        readonly List<Instruction> instructions;
        int pc;

        /// <summary>
        /// Initializes a new instance of <see cref="InstructionContainer"/>.
        /// </summary>
        /// <param name="instructions">The nested instructions to hold.</param>
        protected InstructionContainer(IEnumerable<Instruction> instructions)
        {
            this.instructions = new List<Instruction>(instructions);
        }

        /// <summary>
        /// Gets the nested instructions contained in this <see cref="InstructionContainer"/>.
        /// </summary>
        public IReadOnlyList<Instruction> Instructions => this.instructions;

        /// <summary>
        /// Gets the index of the currently executing nested instruction.
        /// </summary>
        protected int PC => this.pc;

        /// <summary>
        /// Resets the index of the currently executing nested instruction to 0.
        /// </summary>
        protected void ResetPC()
        {
            this.pc = 0;
        }

        /// <summary>
        /// Calls the Tick method of the currently executing nested instruction
        /// and if necessary advances the PC.
        /// </summary>
        /// <param name="critterState">The state of the critter to work on.</param>
        /// <returns><code>true</code> if more execution steps must be executed
        /// to finish the instruction execution. <code>false</code> if
        /// instruction execution has finished.</returns>
        protected bool InternalTick(CritterState critterState)
        {
            if (critterState == null)
                throw new ArgumentNullException(nameof(critterState));
            if (this.pc < 0)
                throw new IndexOutOfRangeException("PC out of range");

            if (this.pc >= this.instructions.Count)
                return false;

            var instr = this.instructions[this.pc];
            if (instr.Tick(critterState) == false)
                this.pc++;

            return this.pc < this.instructions.Count;
        }
    }
}
