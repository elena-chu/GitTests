using System;
using System.Globalization;

namespace Ws.Extensions.UI.Wpf.Converters
{
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

}
