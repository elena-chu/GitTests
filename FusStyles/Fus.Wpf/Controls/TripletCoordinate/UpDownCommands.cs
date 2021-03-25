using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ws.Fus.UI.Wpf.Controls.TripletCoordinate
{
    public static class UpDownCommands
    {
        public static readonly RoutedUICommand Up = new RoutedUICommand
            (
                "Up",
                "Up",
                typeof(UpDownCommands),
                new InputGestureCollection()
                {
                    //new KeyGesture(Key.Up)
                }
            );
        public static readonly RoutedUICommand Down = new RoutedUICommand
            (
                "Down",
                "Down",
                typeof(UpDownCommands),
                new InputGestureCollection()
                {
                    //new KeyGesture(Key.Down)
                }
            );

        //Define more commands here, just like the one above
    }
}
