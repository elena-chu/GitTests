using System;
using System.Globalization;
using System.Windows.Data;

namespace InsightecFiddle.Converters
{
    // Receives: int
    // Param: what it should divide by
    // Returns: True if divides with no remainder
    public class DividesByToBooleanConverter : ConverterMarkupExtension<DividesByToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || (!(parameter is int) && !(parameter is string)) ||
                value == null || (!(value is int) && !(value is string)))
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

