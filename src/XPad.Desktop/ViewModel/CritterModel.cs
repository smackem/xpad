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
    class CritterModel : NotifyPropertyChanged
    {
        readonly ObservableCollection<InstructionModel> instructions = new ObservableCollection<InstructionModel>();

        public double X { get; set; }
        public double Y { get; set; }
        public IList<InstructionModel> Instructions => this.instructions;

        public Engine.Program Program { get; private set; }

        public void Disassemble(Engine.Program program)
        {
            this.instructions.Clear();

            foreach (var instr in program.Instructions)
                this.instructions.Add(InstructionModel.FromInstruction(instr));
        }

        public void Compile()
        {
            Program = new Engine.Program(this.instructions
                .Select(instr => instr.Instruction));
        }

        public bool Tick()
        {
            var program = Program;
            var isRunning = false;

            if (program != null)
            {
                var critterState = new Engine.CritterState
                {
                    PositionX = X,
                    PositionY = Y,
                };

                isRunning = program.Tick(critterState);

                X = critterState.PositionX;
                Y = critterState.PositionY;
            }

            return isRunning;
        }
    }
}
