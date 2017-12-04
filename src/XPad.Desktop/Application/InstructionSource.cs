using FeatherSharp.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPad.Desktop.Application
{
    [Feather(FeatherAction.NotifyPropertyChanged)]
    public abstract class InstructionSource : NotifyPropertyChanged
    {
        Engine.Instruction instruction;

        protected InstructionSource()
        {
            // mark as dirty when property changes
            this.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName != nameof(IsModified))
                    IsModified = true;
            };
        }

        public bool IsModified { get; protected set; }

        public Engine.Instruction Compile()
        {
            if (IsModified)
            {
                this.instruction = InternalCompile();
                IsModified = false;
            }

            return this.instruction;
        }

        protected abstract Engine.Instruction InternalCompile();
    }

    [Feather(FeatherAction.NotifyPropertyChanged)]
    public class MoveInstructionSource : InstructionSource
    {
        public double DestX { get; set; }
        public double DestY { get; set; }

        protected override Engine.Instruction InternalCompile()
        {
            return new Engine.MoveInstruction(DestX, DestY);
        }

        public override string ToString()
        {
            return $"MoveTo ({DestX}, {DestY})";
        }
    }

    [Feather(FeatherAction.NotifyPropertyChanged)]
    public class LoopInstructionSource : InstructionSource
    {
        readonly ObservableCollection<InstructionSource> sources = new ObservableCollection<InstructionSource>();

        public LoopInstructionSource()
        {
            // mark as dirty when collection changes
            this.sources.Add(new AddPseudoInstructionSource(this.sources));
            this.sources.CollectionChanged += (sender, e) => IsModified = true;
        }

        public int Repetitions { get; set; }
        public IList<InstructionSource> Sources => this.sources;

        protected override Engine.Instruction InternalCompile()
        {
            return new Engine.LoopInstruction(this.sources
                .Select(source => source.Compile())
                .Where(instr => instr != null),
                Repetitions);
        }

        public override string ToString()
        {
            return $"Loop {Repetitions} times over {Sources.Count} instructions";
        }
    }

    public class PseudoInstructionSource : InstructionSource
    {
        protected override Engine.Instruction InternalCompile()
        {
            return null;
        }
    }

    [Feather(FeatherAction.NotifyPropertyChanged)]
    public class AddPseudoInstructionSource : PseudoInstructionSource
    {
        static readonly Random random = new Random();

        public AddPseudoInstructionSource(IList<InstructionSource> parentCollection)
        {
            ParentCollection = parentCollection;
        }

        public IList<InstructionSource> ParentCollection { get; }

        public override string ToString()
        {
            return "Add Instruction";
        }

        public void AddMoveInstructionToParentCollection()
        {
            var newModel = new MoveInstructionSource
            {
                DestX = random.Next(0, 400),
                DestY = random.Next(0, 400),
            };

            AddInstructionToParentCollection(newModel);
        }

        public void AddLoopInstructionToParentCollection()
        {
            var newModel = new LoopInstructionSource
            {
                Repetitions = 3,
            };

            AddInstructionToParentCollection(newModel);
        }

        // ====================================================================

        void AddInstructionToParentCollection(InstructionSource instr)
        {
            var list = ParentCollection;

            Debug.Assert(list.LastOrDefault() == this);

            list.Insert(list.Count - 1, instr);
        }
    }
}
