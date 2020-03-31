using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageLoad
{

    public class SliderValueToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null || parameter == null)
            {
                return value;
            }
            double coeff;
            double fullWidth;
            if(double.TryParse(value.ToString(), out coeff) && double.TryParse(parameter.ToString(), out fullWidth))
            {
                var ret = coeff * fullWidth;
                return ret;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
