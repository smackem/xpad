using FeatherSharp.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPad.Desktop.Application
{
    [Feather(FeatherAction.NotifyPropertyChanged)]
    class Critter : NotifyPropertyChanged
    {
        readonly ObservableCollection<InstructionSource> sources = new ObservableCollection<InstructionSource>();

        public Critter()
        {
            this.sources.Add(new AddPseudoInstructionSource(this.sources));
        }

        public double X { get; set; }
        public double Y { get; set; }
        public IList<InstructionSource> Sources => this.sources;

        public Engine.Program Program { get; private set; }

        public void Compile()
        {
            Program = new Engine.Program(this.sources
                .Select(source => source.Compile())
                .Where(instr => instr != null));
        }

        public bool Tick()
        {
            var program = Program;

            if (program == null)
                Compile();

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
