using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace InsightecFiddle.Converters
{
    // Receives: Temperature
    // Returns: Brush
    public class TemperatureToBrushConverter : ConverterMarkupExtension<TemperatureToBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is string))
                return Binding.DoNothing;
            
            SolidColorBrush brush = new SolidColorBrush();

            string temperatureString = value as string;
            if (int.TryParse(temperatureString, out int temperature))
            {
                if (temperature <= 35)
                    return brush;
                if (temperature > 60)
                    temperatureString = "60";
                if (temperature < 37)
                    temperatureString = "37";
            }

            string brushName = "XBrush.Temperature." + temperatureString;
            brush = (SolidColorBrush)Application.Current.TryFindResource(brushName);
            return brush;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

