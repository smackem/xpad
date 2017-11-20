using FeatherSharp.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPad.Desktop.ViewModel
{
    [Feather(FeatherAction.NotifyPropertyChanged)]
    public abstract class InstructionModel : NotifyPropertyChanged
    {
        public InstructionModel(Engine.Instruction instruction)
        {
            Instruction = instruction ?? throw new ArgumentNullException(nameof(instruction));
        }

        public Engine.Instruction Instruction { get; }

        public static InstructionModel FromInstruction(Engine.Instruction instruction)
        {
            switch (instruction)
            {
                case Engine.MoveInstruction instr:
                    return new MoveInstructionModel(instr);

                case Engine.LoopInstruction instr:
                    return new LoopInstructionModel(instr);

                default:
                    throw new ArgumentException($"Unsupported instruction type {instruction.GetType()}!");
            }
        }
    }

    [Feather(FeatherAction.NotifyPropertyChanged)]
    public class MoveInstructionModel : InstructionModel
    {
        public MoveInstructionModel(Engine.MoveInstruction instruction)
            : base(instruction)
        {
            DestX = instruction.DestX;
            DestY = instruction.DestY;
        }

        public double DestX { get; set; }
        public double DestY { get; set; }

        public override string ToString()
        {
            return $"MoveTo ({DestX}, {DestY})";
        }
    }

    [Feather(FeatherAction.NotifyPropertyChanged)]
    public class LoopInstructionModel : InstructionModel
    {
        readonly ObservableCollection<InstructionModel> instructions = new ObservableCollection<InstructionModel>();

        public LoopInstructionModel(Engine.LoopInstruction instruction)
            : base(instruction)
        {
            var models = instruction.Instructions
                .Select(instr => FromInstruction(instr));

            foreach (var model in models)
                this.instructions.Add(model);

            Repetitions = instruction.Repetitions;
        }

        public int Repetitions { get; set; }
        public IList<InstructionModel> Instructions => this.instructions;

        public override string ToString()
        {
            return $"Loop {Repetitions} times over {Instructions.Count} instructions";
        }
    }
}
