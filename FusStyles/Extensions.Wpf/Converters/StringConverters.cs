using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Ws.Extensions.UI.Wpf.Utils;

namespace Ws.Extensions.UI.Wpf.Converters
{
    /// <summary>
    /// Receives:
    /// value: string
    /// param (optional): '!' in param will invert results
    /// Returns: true if string has content
    /// </summary>
    public class StringHasContentToBooleanConverter : ConverterMarkupExtension<StringHasContentToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isStringWithContent = ConverterAssists.CheckValueValidity(value) && value is string && !string.IsNullOrEmpty(value as string);
            bool invert;
            ConverterAssists.GetInvert(parameter, out invert);
            return invert ? !isStringWithContent : isStringWithContent;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Receives:
    /// value: string
    /// param (optional): '!' in param will invert results
    /// 'Hidden' or '#' in param will return Visibility.Hidden instead of Visibility.Collapsed
    /// </summary>
    public class StringHasContentToVisibilityConverter : ConverterMarkupExtension<StringHasContentToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isStringWithContent = ConverterAssists.CheckValueValidity(value) && value is string && !string.IsNullOrEmpty(value as string);
            bool invert;
            Visibility negativeVisibility;
            ConverterAssists.GetInvertAndNegativeVisibility(parameter, out invert, out negativeVisibility);
            return invert ? (isStringWithContent ? negativeVisibility : Visibility.Visible) :
                            (isStringWithContent ? Visibility.Visible : negativeVisibility);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Receives:
    /// value[0]: FrameworkElement holding text
    /// valud[1]: Available width
    /// param (optional): '!' in param will invert results
    /// Returns: true if measured text is Trimmed or Wrapped (wider than its container)
    /// </summary>
    public class TextTrimmedOrWrappedToBooleanConverter : MultiConverterMarkupExtension<TextTrimmedOrWrappedToBooleanConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckAllValuesValidity(values) || !(values[0] is FrameworkElement frameworkElement) || !(values[1] is double availableWidth))
                return Binding.DoNothing;

            ConverterAssists.GetInvert(parameter, out bool invert);
            return invert ? !TextAssists.IsTextOverflowingWidth(frameworkElement, availableWidth) :
                             TextAssists.IsTextOverflowingWidth(frameworkElement, availableWidth);
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class PrefixFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var prefix = parameter != null ? parameter.ToString() : string.Empty;
            var val = value ?? string.Empty;
            return $"{prefix}{val}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class PostfixFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var postFix = parameter != null ? parameter.ToString() : string.Empty;
            var val = value ?? string.Empty;
            return $"{val}{postFix}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FormatConverter : IValueConverter
    {
        public string Format { get; set; } = "{0}";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var format = parameter as string ?? Format;
                return string.Format(format, value);
            }
            catch (Exception ex)
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    class MultiFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return string.Format(parameter as string, values);
            }
            catch (Exception ex)
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToNullableNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value as string;
            if (stringValue != null)
            {
                if (targetType == typeof(int?))
                {
                    int result;
                    if (int.TryParse(stringValue, out result))
                        return result;

                    return null;
                }

                if (targetType == typeof(decimal?))
                {
                    decimal result;
                    if (decimal.TryParse(stringValue, out result))
                        return result;

                    return null;
                }

                if (targetType == typeof(long?))
                {
                    long result;
                    if (long.TryParse(stringValue, out result))
                        return result;

                    return null;
                }

                if (targetType == typeof(double?))
                {
                    double result;
                    if (double.TryParse(stringValue, out result))
                        return result;

                    return null;
                }

                if (targetType == typeof(float?))
                {
                    float result;
                    if (float.TryParse(stringValue, out result))
                        return result;

                    return null;
                }
            }

            return value;
        }
    }
}
