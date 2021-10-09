using System;
using System.Globalization;
using System.Windows;
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

    /// <summary>
    /// Returns Visible if input is more than param
    /// </summary>
    public class MoreThanToVisibilityConverter : ConverterMarkupExtension<MoreThanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(value) || !ConverterAssists.CheckValueValidity(parameter))
                return Binding.DoNothing;

            double valueNum, paramNum;
            if (double.TryParse(value.ToString(), out valueNum) && double.TryParse(parameter.ToString(), out paramNum))
                return (valueNum > paramNum) ? Visibility.Visible : Visibility.Collapsed;

            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Receives: int
    // Param: Denominator
    // Returns: True if divides by Denominator with no remainder
    public class DividesByToBooleanConverter : ConverterMarkupExtension<DividesByToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(parameter) || (!(parameter is int) && !(parameter is string)) ||
                !ConverterAssists.CheckValueValidity(value) || (!(value is int) && !(value is string)))
                return Binding.DoNothing;

            int numerator = 0;
            if (value is string valueString)
                int.TryParse(valueString, out numerator);
            if (value is int)
                numerator = (int)value;

            int denominator = 0;
            if (parameter is string paramString)
                int.TryParse(paramString, out denominator);
            if (parameter is int)
                denominator = (int)parameter;

            return numerator % denominator == 0;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
