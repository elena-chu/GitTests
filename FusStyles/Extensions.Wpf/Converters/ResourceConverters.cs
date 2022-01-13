using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Extensions.UI.Wpf.Converters
{
    public static class TemperatureAssists
    {
        public const int TEMPERATURE_SATURATION_TOP = 60;
        public const int TEMPERATURE_SATURATION_BOTTOM = 37;
        public const int TEMPERATURE_MINIMAL = 35;


        private const double COEFFICIENT_R = 0.299;
        private const double COEFFICIENT_G = 0.587;
        private const double COEFFICIENT_B = 0.114;

        public static SolidColorBrush ToGreyscale(this SolidColorBrush colorBrush)
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

        private static string GetLimitedTemperatureAsString(int temperature)
        {
            if (temperature <= TEMPERATURE_MINIMAL)
                return string.Empty;
            if (temperature > TEMPERATURE_SATURATION_TOP)
                return TEMPERATURE_SATURATION_TOP.ToString();
            if (temperature < TEMPERATURE_SATURATION_BOTTOM)
                return TEMPERATURE_SATURATION_BOTTOM.ToString();
            return temperature.ToString();
        }

        public static SolidColorBrush GetTemperatureBrush(int temperature)
        {
            string temperatureString = GetLimitedTemperatureAsString(temperature);
            if (string.IsNullOrEmpty(temperatureString))
                return new SolidColorBrush();

            string brushName = "XBrush.Temperature." + temperatureString;
            var resourceBrush = (SolidColorBrush)Application.Current.TryFindResource(brushName);
            return resourceBrush != null ? resourceBrush : new SolidColorBrush();
        }
    }

    /// <summary>
    /// Receives: Temperature (string !)
    /// Optional: Cutoff Temperature Top (double)
    /// Optional: Cutoff Temperature Bottom (double)
    /// Returns: Brush representing Temperature color
    /// </summary>
    public class TemperatureAndCutoffsToBrushConverter : MultiConverterMarkupExtension<TemperatureAndCutoffsToBrushConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckAllValuesValidity(values) || !(values[0] is string))
                return Binding.DoNothing;

            SolidColorBrush brush = new SolidColorBrush();

            string temperatureString = values[0] as string;
            if (int.TryParse(temperatureString, out int temperature))
                brush = TemperatureAssists.GetTemperatureBrush(temperature);

            bool greyscale =
                (values.Length > 1 && values[1] is double temperatureCutoffTop && temperatureCutoffTop != double.NaN && temperature > temperatureCutoffTop) ||
                (values.Length > 2 && values[2] is double temperatureCutoffBottom && temperatureCutoffBottom != double.NaN && temperature < temperatureCutoffBottom);

            return greyscale ? brush.ToGreyscale() : brush;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Receives: Temperature (int or double)
    /// Returns: Brush representing Temperature color
    /// </summary>
    public class TemperatureToBrushConverter : ConverterMarkupExtension<TemperatureToBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(value))
                return Binding.DoNothing;

            if (value is int temperatureInt)
                return TemperatureAssists.GetTemperatureBrush(temperatureInt);
            if (value is double temperatureDouble)
                return TemperatureAssists.GetTemperatureBrush((int)temperatureDouble);
            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Receives: Temperature (int or double)
    /// Returns: Brush representing Greyscale Temperature color
    /// </summary>
    public class TemperatureToGreyscaleBrushConverter : ConverterMarkupExtension<TemperatureToGreyscaleBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(value))
                return Binding.DoNothing;

            if (value is int temperatureInt)
                return TemperatureAssists.GetTemperatureBrush(temperatureInt).ToGreyscale();
            if (value is double temperatureDouble)
                return TemperatureAssists.GetTemperatureBrush((int)temperatureDouble).ToGreyscale();
            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Receives: Severity
    /// Returns: Brush representing Severity color
    /// </summary>
    public class SeverityToBrushConverter : ConverterMarkupExtension<SeverityToBrushConverter>
    {
        public const string SAFETY_BRUSH_NAME = "XBrush.Safety";
        public const string ERROR_BRUSH_NAME = "XBrush.Error";
        public const string WARNING_BRUSH_NAME = "XBrush.Warning";
        public const string INFO_BRUSH_NAME = "XBrush.Info";
        
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(value))
                return Binding.DoNothing;

            string brushName = null;
            if (value is Severity severity)
            {
                switch (severity)
                {
                    case Severity.Info:
                        brushName = INFO_BRUSH_NAME;
                        break;
                    case Severity.Warning:
                        brushName = WARNING_BRUSH_NAME;
                        break;
                    case Severity.Error:
                        brushName = ERROR_BRUSH_NAME;
                        break;
                    case Severity.Safety:
                        brushName = SAFETY_BRUSH_NAME;
                        break;
                    default:
                        break;
                }
                return (SolidColorBrush)Application.Current.TryFindResource(brushName);
            }

            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Receives: Severity
    /// Returns: Icon (Canvas) representing Severity
    /// </summary>
    public class SeverityToIconConverter : ConverterMarkupExtension<SeverityToIconConverter>
    {
        public const string SAFETY_ICON_NAME = "XCanvas.Message.Safety";
        public const string ERROR_ICON_NAME = "XCanvas.Message.Error";
        public const string WARNING_ICON_NAME = "XCanvas.Message.Warning";
        public const string INFO_ICON_NAME = "XCanvas.Message.Info";
        
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterAssists.CheckValueValidity(value))
                return Binding.DoNothing;

            string iconName = null;
            if (value is Severity severity)
            {
                switch (severity)
                {
                    case Severity.Info:
                        iconName = INFO_ICON_NAME;
                        break;
                    case Severity.Warning:
                        iconName = WARNING_ICON_NAME;
                        break;
                    case Severity.Error:
                        iconName = ERROR_ICON_NAME;
                        break;
                    case Severity.Safety:
                        iconName = SAFETY_ICON_NAME;
                        break;
                    default:
                        break;
                }
                return (Canvas)Application.Current.TryFindResource(iconName);
            }

            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

