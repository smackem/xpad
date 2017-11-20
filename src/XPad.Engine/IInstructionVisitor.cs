using System;
using System.Collections.Generic;
using System.Text;

namespace XPad.Engine
{
    /// <summary>
    /// Used to implement the Visitor pattern (see OO pattern Visitor).
    /// </summary>
    /// <typeparam name="TState">Generic state to pass to the visitor methods.</typeparam>
    /// <typeparam name="TResult">Generic result of the visitor methods.</typeparam>
    public interface IInstructionVisitor<TState, TResult>
    {
        TResult Visit(MoveInstruction instruction, TState state);
        TResult Visit(LoopInstruction instruction, TState state);
        TResult Visit(CustomInstruction instruction, TState state);
    }
}
