using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Ws.Extensions.UI.Wpf.Behaviors;
using Ws.Extensions.UI.Wpf.Utils;

namespace Ws.Fus.Treatment.UI.Wpf.LA
{
    public partial class DebugViewArchive : UserControl
    {
        public DebugViewArchive()
        {
            InitializeComponent();
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

        private void ChangeControlState(object sender, System.Windows.RoutedEventArgs e)
        {
            switch (ControlExtensions.GetControlState(PlusButton))
            {
                case ControlState.Normal:
                    ControlExtensions.SetControlState(PlusButton, ControlState.CallToAction);
                    break;
                case ControlState.CallToAction:
                    ControlExtensions.SetControlState(PlusButton, ControlState.Busy);
                    DelayAndReset();
                    break;
                case ControlState.Busy:
                    ControlExtensions.SetControlState(PlusButton, ControlState.Normal);
                    break;
                default:
                    break;
            }
        }

        private async Task DelayAndReset()
        {
            await Task.Delay(8000);
            ControlExtensions.SetControlState(PlusButton, ControlState.Normal);
        }
    }
}
