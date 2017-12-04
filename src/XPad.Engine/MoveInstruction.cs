using System;

namespace XPad.Engine
{
    /// <summary>
    /// Moves the critter.
    /// </summary>
    public sealed class MoveInstruction : Instruction
    {
        /// <summary>
        /// The distance to move the critter with each Tick.
        /// </summary>
        public static readonly double StepLength = 4.0;

        /// <summary>
        /// Initializes a new instance of <see cref="MoveInstruction"/>.
        /// </summary>
        /// <param name="destX">The destination X coordinate.</param>
        /// <param name="destY">The destination Y coordinate.</param>
        public MoveInstruction(double destX, double destY)
        {
            DestX = destX;
            DestY = destY;
        }

        /// <summary>
        /// Gets the destination X coordinate.
        /// </summary>
        public double DestX { get; }

        /// <summary>
        /// Gets the destination Y coordinate.
        /// </summary>
        public double DestY { get; }

        public override TResult Accept<TState, TResult>(IInstructionVisitor<TState, TResult> visitor, TState state)
        {
            return visitor.Visit(this, state);
        }

        /// <summary>
        /// Advances the critter in direction of the destination by <see cref="StepLength"/>.
        /// </summary>
        /// <returns><code>false</code> if the critter has arrived at its destination.</returns>
        public override bool Tick(CritterState critterState)
        {
            // calculate difference vector from current pos to dest
            var pos = new Vector(critterState.PositionX, critterState.PositionY);
            var dest = new Vector(DestX, DestY);
            var diff = dest - pos;
            if (diff.Length < StepLength)
            {
                critterState.PositionX = DestX;
                critterState.PositionY = DestY;
                return false;
            }

            // normalize diff vector so that length is 1, then multiply it
            // with StepLength to get the increment vector.
            var unit = diff.Normalize();
            var increment = unit * StepLength;

            // add increment vector to current pos to get new pos
            var newPos = pos + increment;
            critterState.PositionX = newPos.X;
            critterState.PositionY = newPos.Y;
            return true;
        }
    }
}
