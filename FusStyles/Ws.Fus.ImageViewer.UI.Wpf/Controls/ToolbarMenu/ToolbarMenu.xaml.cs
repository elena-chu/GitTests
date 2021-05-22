using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Ws.Extensions.UI.Wpf.Utils;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    public partial class ToolbarMenu : UserControl
    {
        public ToolbarMenu()
        {
            InitializeComponent();
        }

        private void OnToolbarMenuHeaderActivated(ToolbarMenuHeader menuHeader)
        {
            if (menuHeader.IsActive)
                Menu.Items.Cast<ToolbarMenuHeader>().Where(x => !x.Equals(menuHeader)).ToList().ForEach(x => x.InactivateExclusiveGroupMember());
        }

        private void MenuOpenerButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleButton menuOpenerToggleButton = (ToggleButton)sender;
            MenuItem menuItem = menuOpenerToggleButton.ParentOfType<MenuItem>() as MenuItem;
            if (menuItem != null)
                menuItem.IsSubmenuOpen = !menuItem.IsSubmenuOpen;
        }
    }
}
