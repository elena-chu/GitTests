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
    class CompositeOverlayVmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //try
            //{
            //    return value;
            //    if (value == null)
            //        return null;

                //var commands = value as OverlayCommands;
                //var layersStr = (string)parameter;

                //var layersArr = layersStr.Split(',');
                //var layers = layersArr.Select(s => (UiMode)Enum.Parse(typeof(UiMode), s));

                //return commands.GetCompositeVm(layers.ToArray());
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
