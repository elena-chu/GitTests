using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Ws.Extensions.Patterns.Profiles;

namespace Ws.Extensions.UI.Wpf.Converters
{
    public class ProfileNameConverter : IValueConverter
    {
        private static readonly ILogger _logger = Log.ForContext<ProfileNameConverter>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return value.ToString();
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, "Conversion from {value} failed", value);
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (ProfileName)(value as string);
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, "Conversion back from {value} failed", value);
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
