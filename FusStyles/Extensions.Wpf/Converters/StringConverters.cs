using System;
using System.Globalization;
using System.Windows;

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
            ConverterAssists.GetInvert(parameter, out bool invert);
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
            ConverterAssists.GetInvertAndNegativeVisibility(parameter, out bool invert, out Visibility negativeVisibility);
            return invert ? (isStringWithContent ? negativeVisibility : Visibility.Visible) :
                            (isStringWithContent ? Visibility.Visible : negativeVisibility);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
