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
using System.Windows.Controls.Primitives;
using Ws.Extensions.UI.Wpf.Utils;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    /// <summary>
    /// Interaction logic for ToolbarMenu.xaml
    /// </summary>
    public partial class ToolbarMenu : UserControl
    {
        private ToggleButton _innerToggle;

        public ToolbarMenu()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnToolbarMenuHeaderActivated(ToolbarMenuHeader menuHeader)
        {
            if (_innerToggle == null)
                _innerToggle = menuHeader.FindVisualChildren<ToggleButton>().FirstOrDefault(el => el.Name == "MenuToggleBtn");

            if (_innerToggle != null)
                _innerToggle.IsChecked = menuHeader.IsActive;
        }

        private void MenuOpenerButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleButton menuOpenerToggleButton = (ToggleButton)sender;
            MenuItem menuItem = menuOpenerToggleButton.ParentOfType<MenuItem>() as MenuItem;
            if (menuItem != null)
            {
                menuItem.IsSubmenuOpen = !menuItem.IsSubmenuOpen;
            }
        }

        private void CloseCompareMenu(object sender, System.Windows.RoutedEventArgs e)
        {
            CompareMenuHeader.IsActive = false;
        }

        private void OnToolbarCompareMenuHeaderActivated(ToolbarMenuHeader menuHeader)
        {
            var btn = menuHeader.FindVisualChildren<ToggleButton>().FirstOrDefault(el => el.Name == "MenuToggleBtn");
            if (btn == null)
                    return;

            menuHeader.IsActive = btn.IsChecked.HasValue ? btn.IsChecked.Value : false;

            //if (menuHeader.IsActive)
            //    Menu.Items.Cast<ToolbarMenuHeader>().Where(x => !x.Equals(menuHeader)).ToList().ForEach(x => x.InactivateExclusiveGroupMember());
        }

    }
}
