using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
//using Ws.Fus.ImageViewer.UI.Wpf.Modes.Controllers;
//using Ws.Fus.Interfaces.Overlays;

namespace Ws.Fus.ImageViewer.UI.Wpf.Converters
{
    public class MultiMeasureVmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //try
            //{
                //if (value == null)
                //    return null;

                //var commands = value as MeasureCommands;
                //var measureMode = (UiMode)Enum.Parse(typeof(UiMode), (string)parameter);
                //return commands.GetMultiMeasureVm(measureMode);
            //}
            //catch (Exception ex)
            //{
                return null;
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
