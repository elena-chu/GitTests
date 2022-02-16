using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ws.Fus.PlanningScans.UI.Wpf.Controls.ScanBox
{
    public class MainOrientationPanel : BaseActionPanel
    {
        public override void BuildSpecific(ScanBoxParams initialParams)
        {
            _mainSquare.Width = InitialParams.FOV_X;
            _mainSquare.Height = InitialParams.FOV_Y;
            Canvas.SetLeft(_mainSquare, (this.Width / 2 - _mainSquare.Width / 2));
            Canvas.SetTop(_mainSquare, (this.Height / 2 - _mainSquare.Height / 2));
        }
    }
}
