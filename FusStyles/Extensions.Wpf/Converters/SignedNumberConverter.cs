using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Ws.Extensions.UI.Wpf.Converters
{
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
