using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace XPad.Desktop.Controls
{
    public class DropDownButton : ToggleButton
    {
        public DropDownButton()
        {
            var binding = new Binding("ContextMenu.IsOpen")
            {
                Source = this
            };

            SetBinding(IsCheckedProperty, binding);
        }

        protected override void OnClick()
        {
            var menu = ContextMenu;

            if (menu != null)
            {
                menu.PlacementTarget = this;
                menu.Placement = PlacementMode.Bottom;
                menu.IsOpen = true;
            }
        }
    }
}