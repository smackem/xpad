using System;
using System.Collections.Generic;
using System.Text;

namespace XPad.Engine
{
    /// <summary>
    /// The current mutable state of a critter.
    /// </summary>
    /// <remarks>
    /// This class is not thread-safe.
    /// </remarks>
    public class CritterState
    {
        public double PositionX { get; set; }
        public double PositionY { get; set; }
    }
}
