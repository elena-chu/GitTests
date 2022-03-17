using System.Threading.Tasks;
using System.Windows.Controls;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Fus.Treatment.UI.Wpf.LA
{
    public partial class DebugViewArchive : UserControl
    {
        public DebugViewArchive()
        {
            InitializeComponent();
        }

        //private void ChangeControlState(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    switch (ControlExtensions.GetControlState(PlusButton))
        //    {
        //        case ControlState.Normal:
        //            ControlExtensions.SetControlState(PlusButton, ControlState.CallToAction);
        //            break;
        //        case ControlState.CallToAction:
        //            ControlExtensions.SetControlState(PlusButton, ControlState.Busy);
        //            DelayAndReset();
        //            break;
        //        case ControlState.Busy:
        //            ControlExtensions.SetControlState(PlusButton, ControlState.Normal);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //private async Task DelayAndReset()
        //{
        //    await Task.Delay(18000);
        //    ControlExtensions.SetControlState(PlusButton, ControlState.Normal);
        //}
    }
}
