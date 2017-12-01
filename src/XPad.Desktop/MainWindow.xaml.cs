using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        readonly Application.MainWindowModel model = new Application.MainWindowModel();
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

            var critter = Application.CritterModel.Disassemble(program);
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

            critter = Application.CritterModel.Disassemble(program);
            this.model.Critters.Add(critter);

            this.timer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(40),
                DispatcherPriority.Normal,
                TimerTick,
                Dispatcher);
        }

        void runButton_Click(object sender, RoutedEventArgs e)
        {
            this.model.CompileAndStartProgram();
            this.timer.Start();
        }

        void TimerTick(object sender, EventArgs e)
        {
            if (this.model.Tick() == false)
                this.timer.Stop();
        }

        void addInstructionButton_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is FrameworkElement elem
                && elem.DataContext is Application.AddInstructionModel model)
            {
                model.AddNewInstructionToParentCollection();
            }
        }
    }
}
