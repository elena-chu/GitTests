using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace InsightecFiddle.Converters
{
    // Receives: Temperature
    // Returns: Brush
    public class TemperatureToBrushConverter : MultiConverterMarkupExtension<TemperatureToBrushConverter>
    {
        private const int TEMPERATURE_SATURATION_TOP = 60;
        private const int TEMPERATURE_SATURATION_BOTTOM = 37;

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckAllValuesValidity(values) || !(values[0] is string))
                return Binding.DoNothing;
            
            SolidColorBrush brush = new SolidColorBrush();

            string temperatureString = values[0] as string;
            if (int.TryParse(temperatureString, out int temperature))
            {
                if (temperature <= 35)
                    return brush;
                if (temperature > TEMPERATURE_SATURATION_TOP)
                    temperatureString = "60";
                if (temperature < TEMPERATURE_SATURATION_BOTTOM)
                    temperatureString = "37";
            }
            string brushName = "XBrush.Temperature." + temperatureString;
            brush = (SolidColorBrush)Application.Current.TryFindResource(brushName);

            bool greyscale =
                (values.Length > 1 && values[1] is double temperatureCutoffTop && temperatureCutoffTop != double.NaN && temperature > temperatureCutoffTop) ||
                (values.Length > 2 && values[2] is double temperatureCutoffBottom && temperatureCutoffBottom != double.NaN && temperature < temperatureCutoffBottom);
            
            return greyscale ? RgbToGreyscale(brush) : brush;
        }

        private const double COEFFICIENT_R = 0.299;
        private const double COEFFICIENT_G = 0.587;
        private const double COEFFICIENT_B = 0.114;

        private SolidColorBrush RgbToGreyscale(SolidColorBrush colorBrush)
        {
            double calculatedGrey = COEFFICIENT_R * colorBrush.Color.R + COEFFICIENT_G * colorBrush.Color.G + COEFFICIENT_B * colorBrush.Color.B;
            Color greyColor = new Color
            {
                R = (byte)calculatedGrey,
                G = (byte)calculatedGrey,
                B = (byte)calculatedGrey,
                A = 0xFF
            };
            return new SolidColorBrush(greyColor);
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

