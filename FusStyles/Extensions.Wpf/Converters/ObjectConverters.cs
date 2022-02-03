using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Ws.Extensions.UI.Wpf.Converters
{
    public class ObjectToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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

    /// <summary>
    /// Returns object != null
    /// </summary>
    public class ObjectNotNullToBooleanConverter : ConverterMarkupExtension<ObjectNotNullToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Returns true if value[0] is equal to any of the following values
    /// </summary>
    public class ObjectEqualityToBooleanConverter : MultiConverterMarkupExtension<ObjectEqualityToBooleanConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return Binding.DoNothing;

            ConverterAssists.GetInvert(parameter, out bool invert);

            bool equalToAnother = false;

            if (values[0] == null)
                equalToAnother = values.Skip(1).Any(x => x == null);
            else
            {
                for (int i = 1; i < values.Length; i++)
                {
                    if (values[i] != null && values[0].Equals(values[i]))
                    {
                        equalToAnother = true;
                        break;
                    }
                }
            }
            return invert ? !equalToAnother : equalToAnother;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
