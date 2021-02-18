using System;
using System.Globalization;
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

    /// <summary>
    /// Returns scalar (param) * value
    /// </summary>
    public class ScalarConverter : ConverterMarkupExtension<ScalarConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(value) || !ConverterAssists.CheckValueValidity(parameter) ||
                System.Convert.ToDouble(value) == 0 || System.Convert.ToDouble(parameter) == 0)
                return Binding.DoNothing;
            return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
