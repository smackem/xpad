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
            Instruction = instruction;
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

        public static ObservableCollection<InstructionModel> FromCollection(IEnumerable<Engine.Instruction> instructions)
        {
            var models = instructions.Select(instr => FromInstruction(instr));
            var collection = new ObservableCollection<InstructionModel>(models);
            collection.Add(new AddInstructionModel(collection));
            return collection;
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
        readonly ObservableCollection<InstructionModel> instructions;

        public LoopInstructionModel(Engine.LoopInstruction instruction)
            : base(instruction)
        {
            this.instructions = InstructionModel.FromCollection(instruction.Instructions);

            Repetitions = instruction.Repetitions;
        }

        public int Repetitions { get; set; }
        public IList<InstructionModel> Instructions => this.instructions;

        public override string ToString()
        {
            return $"Loop {Repetitions} times over {Instructions.Count} instructions";
        }
    }

    [Feather(FeatherAction.NotifyPropertyChanged)]
    public class AddInstructionModel : InstructionModel
    {
        public AddInstructionModel(IList<InstructionModel> parentCollection)
            : base(null)
        {
            ParentCollection = parentCollection;
        }

        public IList<InstructionModel> ParentCollection { get; }

        public override string ToString()
        {
            return "Add Instruction";
        }

        public void AddNewInstructionToParentCollection()
        {
            var list = ParentCollection;
            var newModel = new MoveInstructionModel(new Engine.MoveInstruction(100, 100));

            if (list.Count > 0)
                list.Insert(list.Count - 1, newModel);
            else
                list.Add(newModel);
        }
    }
}
