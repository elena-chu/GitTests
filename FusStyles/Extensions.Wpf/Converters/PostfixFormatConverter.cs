using System;
using System.Globalization;
using System.Windows.Data;

namespace Ws.Extensions.UI.Wpf.Converters
{
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
}
