using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Ws.Extensions.UI.Wpf.Converters;
using Ws.Extensions.UI.Wpf.Utils;
//using Ws.Fus.ImageViewer.UI.Wpf.Entities;
//using Ws.Fus.Fge.Interfaces;
//using Ws.Fus.Fge.Interfaces.Entities;

namespace Ws.Fus.ImageViewer.UI.Wpf.Converters
{
    class ToGraphicEngineWindowSizeConverter : ParametrizedConverter<Visual>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var size = value == null ? Size.Empty : (Size)value;
            //int width, height;

            //DpiUtils.Convert(Parameter, size, out width, out height);

            //return new GraphicEngineWindowSize
            //{
            //    Width = width,
            //    Height = height,
            //};
            return DependencyProperty.UnsetValue;
        }
    }
}
