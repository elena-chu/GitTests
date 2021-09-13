using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ws.Extensions.UI.Wpf.Converters
{
    public class ComparisonConverter : IValueConverter
    {
        public object TrueValue { get; set; }
        public object FalseValue { get; set; }


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool equal = value != null && value.Equals(parameter);
            if (TrueValue != null && FalseValue != null)
            {
                return equal ? TrueValue : FalseValue;
            }
            else
                return equal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }
}
