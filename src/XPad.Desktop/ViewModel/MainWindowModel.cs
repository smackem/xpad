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
    class MainWindowModel : NotifyPropertyChanged
    {
        readonly ObservableCollection<CritterModel> critters = new ObservableCollection<CritterModel>();

        public IList<CritterModel> Critters => this.critters;

        public bool IsProgramRunning { get; set; }
    }
}
