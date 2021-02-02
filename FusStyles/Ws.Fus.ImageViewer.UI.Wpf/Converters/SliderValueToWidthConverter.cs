using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ws.Fus.ImageViewer.UI.Wpf.Converters
{
    public class SliderValueToWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length<2)
            {
                return values;
            }

            double coeff;
            double fullWidth;
            if (double.TryParse(values[0].ToString(), out coeff) && double.TryParse(values[1].ToString(), out fullWidth))
            {
                var ret = coeff * fullWidth;
                return ret;
            }
            else
            {
                return values;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
