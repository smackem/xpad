using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace XPad.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly ViewModel.MainWindowModel model = new ViewModel.MainWindowModel();
        readonly DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this.model;

            var program = new Engine.Program(new[]
            {
                new Engine.LoopInstruction(new[]
                {
                    new Engine.MoveInstruction(200, 250),
                    new Engine.MoveInstruction(300, 50),
                    new Engine.MoveInstruction(0, 0),
                }, 3),
            });

            var critter = new ViewModel.CritterModel();
            critter.Disassemble(program);
            this.model.Critters.Add(critter);

            program = new Engine.Program(new[]
            {
                new Engine.LoopInstruction(new[]
                {
                    new Engine.MoveInstruction(100, 250),
                    new Engine.MoveInstruction(0, 550),
                    new Engine.MoveInstruction(0, 0),
                }, 2),
            });

            critter = new ViewModel.CritterModel();
            critter.Disassemble(program);
            this.model.Critters.Add(critter);

            this.timer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(40),
                DispatcherPriority.Normal,
                TimerTick,
                Dispatcher);
        }

        void runButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var critter in this.model.Critters)
                critter.Compile();

            this.timer.Start();
            this.model.IsProgramRunning = true;
        }

        void TimerTick(object sender, EventArgs e)
        {
            var isRunning = false;

            foreach (var critter in this.model.Critters)
                isRunning |= critter.Tick();

            this.model.IsProgramRunning = isRunning;

            if (isRunning == false)
                this.timer.Stop();
        }

        private void addInstructionButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
