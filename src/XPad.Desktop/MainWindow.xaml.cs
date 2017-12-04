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

            this.timer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(40),
                DispatcherPriority.Normal,
                TimerTick,
                Dispatcher);
        }

        void RunButton_Click(object sender, RoutedEventArgs e)
        {
            this.model.CompileAndStartProgram();
            this.timer.Start();
        }

        void TimerTick(object sender, EventArgs e)
        {
            if (this.model.Tick() == false)
                this.timer.Stop();
        }

        void AddMoveInstructionButton_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is FrameworkElement elem
                && elem.DataContext is Application.AddPseudoInstructionSource source)
            {
                source.AddMoveInstructionToParentCollection();
            }
        }

        void AddLoopInstructionButton_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is FrameworkElement elem
                && elem.DataContext is Application.AddPseudoInstructionSource source)
            {
                source.AddLoopInstructionToParentCollection();
            }
        }

        void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is TreeView tree)
                ProgramList.SelectedItem = tree.DataContext;
        }

        void AddCritterButton_Click(object sender, RoutedEventArgs e)
        {
            this.model.AddCritter();
        }
    }
}
