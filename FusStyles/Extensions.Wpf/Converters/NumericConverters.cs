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
    /// Returns True if input is more than param
    /// </summary>
    public class MoreThanToBooleanConverter : ConverterMarkupExtension<MoreThanToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(value) || !ConverterAssists.CheckValueValidity(parameter))
                return Binding.DoNothing;

            double valueNum, paramNum;
            if (double.TryParse(value.ToString(), out valueNum) && double.TryParse(parameter.ToString(), out paramNum))
                return valueNum > paramNum;

            return Binding.DoNothing;
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

    /// <summary>
    /// Returns True if input is less than param
    /// </summary>
    public class LessThanToBooleanConverter : ConverterMarkupExtension<LessThanToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(value) || !ConverterAssists.CheckValueValidity(parameter))
                return Binding.DoNothing;

            double valueNum, paramNum;
            if (double.TryParse(value.ToString(), out valueNum) && double.TryParse(parameter.ToString(), out paramNum))
                return valueNum < paramNum;

            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Returns Visible if input is less than param
    /// </summary>
    public class LessThanToVisibilityConverter : ConverterMarkupExtension<LessThanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(value) || !ConverterAssists.CheckValueValidity(parameter))
                return Binding.DoNothing;

            double valueNum, paramNum;
            if (double.TryParse(value.ToString(), out valueNum) && double.TryParse(parameter.ToString(), out paramNum))
                return (valueNum < paramNum) ? Visibility.Visible : Visibility.Collapsed;

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
            string valueString = value as string;
            if (!string.IsNullOrWhiteSpace(valueString))
                int.TryParse(valueString, out numerator);
            if (value is int)
                numerator = (int)value;

            int denominator = 0;
            string paramString = parameter as string;
            if (!string.IsNullOrWhiteSpace(paramString))
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

    /// <summary>
    /// Returns: sum of all values
    /// </summary>
    public class AdditionConverter : MultiConverterMarkupExtension<AdditionConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckAllValuesValidity(values))
                return Binding.DoNothing;

            double sum = 0.0;
            foreach (var value in values)
            {
                if (double.TryParse(value.ToString(), out double parsedValue))
                    sum += parsedValue;
            }

            return sum;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Subtracts all following values from first value
    /// Will not return negative values
    /// </summary>
    public class NonNegativeSubtractionConverter : MultiConverterMarkupExtension<NonNegativeSubtractionConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckAllValuesValidity(values))
                return Binding.DoNothing;

            double diff = 0.0;
            if (double.TryParse(values[0].ToString(), out double parsedValue))
            {
                diff = parsedValue;
                if (values.Length > 1)
                {
                    for (int i = 1; i < values.Length; i++)
                    {
                        if (values[i] != null)
                        {
                            if (double.TryParse(values[i].ToString(), out parsedValue))
                                diff -= parsedValue;
                        }
                    }
                }
            }

            return diff >= 0.0 ? diff : 0.0;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SignedNumberConverter : IValueConverter
    {
        private const string ValueMode = "value";
        private const string SignMode = "sign";

        private double _dbl;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _dbl = (double)value;

            var mode = (string)parameter;
            switch (mode.ToLower())
            {
                case ValueMode:
                    return Math.Abs(_dbl);
                case SignMode:
                    return _dbl < 0;
                default:
                    return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var mode = (string)parameter;
                switch (mode.ToLower())
                {
                    case ValueMode:
                        var strDbl = (string)value;
                        var dbl = 0.0;
                        double.TryParse(strDbl, out dbl);
                        _dbl = _dbl < 0 ? -dbl : dbl;

                        return _dbl;

                    case SignMode:
                        var isNegative = (bool)value;

                        _dbl = Math.Abs(_dbl);
                        if (isNegative)
                            _dbl = -_dbl;

                        return _dbl;

                    default:
                        return DependencyProperty.UnsetValue;
                }
            }
            catch (Exception ex)
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
