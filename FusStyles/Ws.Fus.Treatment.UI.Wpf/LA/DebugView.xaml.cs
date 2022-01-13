using System.Threading.Tasks;
using System.Windows.Controls;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Fus.Treatment.UI.Wpf.LA
{
    public partial class DebugView : UserControl
    {
        public DebugView()
        {
            InitializeComponent();
        }

        //private void ChangeControlState(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    switch (ControlExtensions.GetControlState(RippleButton))
        //    {
        //        case ControlState.Normal:
        //            ControlExtensions.SetControlState(RippleButton, ControlState.CallToAction);
        //            break;
        //        case ControlState.CallToAction:
        //            ControlExtensions.SetControlState(RippleButton, ControlState.Busy);
        //            DelayAndReset();
        //            break;
        //        case ControlState.Busy:
        //            ControlExtensions.SetControlState(RippleButton, ControlState.Normal);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //private async Task DelayAndReset()
        //{
        //    await Task.Delay(18000);
        //    ControlExtensions.SetControlState(RippleButton, ControlState.Normal);
        //}
    }
}
