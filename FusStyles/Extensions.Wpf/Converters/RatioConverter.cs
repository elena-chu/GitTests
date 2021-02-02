using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ws.Extensions.UI.Wpf.Converters
{
    public class RatioConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var value = (double)values[0];
                var ratio = (double)values[1];

                return value * ratio;
            }
            catch (Exception)
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
