using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ws.Extensions.UI.Wpf.Controls
{
    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 1.0;
            for (int i = 0; i < values.Length; i++)
            {
                double d = double.MinValue;
                if(values[i]!= null)
                {
                    if (values[i] is double)
                        d = (double)values[i];
                    else
                        double.TryParse(values[i].ToString(), out d);
                }
                if (d != double.MinValue)
                {
                    result *= d;
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}
