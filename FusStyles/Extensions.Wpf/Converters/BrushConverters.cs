using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Extensions.UI.Wpf.Converters
{
    /// <summary>
    /// Serves ColorAnimations within Templates.
    /// 
    /// Receives: 
    /// 1. double (a percentage running on some Property)
    /// 2. ControlTheme
    /// Returns: Brush of calculated Color for use on Background/Foreground/whatever
    /// </summary>
    public class PercentageToPressedBrushConverter : MultiConverterMarkupExtension<PercentageToPressedBrushConverter>
    {
        private Color FromColor { get; set; }
        private Color ToColor { get; set; }
        private Color DiffColor { get; set; }
        private ControlTheme _controlTheme = ControlTheme.None;
        private ControlTheme ControlTheme
        {
            get { return _controlTheme; }
            set
            {
                if (_controlTheme != value)
                {
                    _controlTheme = value;
                    SetColors();
                }
            }
        }

        public PercentageToPressedBrushConverter()
        {
            SetColors();
        }

        private void SetColors()
        {
            FromColor = Colors.Transparent;
            ToColor = Colors.Transparent;

            string resourceNameFromColor = null;
            string resourceNameToColor = null;

            switch (ControlTheme)
            {
                case ControlTheme.Primary:
                    {
                        resourceNameFromColor = "XColor.AlmostTransparent";
                        resourceNameToColor = "XColor.Button.Primary.Background.Pressed";
                    }
                    break;

                case ControlTheme.Secondary:
                    {
                        resourceNameFromColor = "XColor.AlmostTransparent";
                        resourceNameToColor = "XColor.Button.Secondary.Background.Pressed";
                    }
                    break;

                case ControlTheme.None:
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(resourceNameToColor) && !string.IsNullOrEmpty(resourceNameFromColor))
            {
                FromColor = (Color)Application.Current.TryFindResource(resourceNameFromColor);
                ToColor = (Color)Application.Current.TryFindResource(resourceNameToColor);
            }

            DiffColor = Color.Subtract(ToColor, FromColor);
        }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] == null || !(values[0] is double) ||
                values[1] == null || !(values[1] is Enum) || FromColor == null || ToColor == null)
                return Binding.DoNothing;

            ControlTheme = (ControlTheme)values[1];

            double percentage = (double)values[0];
            Color intermediateColor = Color.Multiply(DiffColor, (float)percentage);
            intermediateColor = Color.Add(intermediateColor, FromColor);

            return new SolidColorBrush(intermediateColor);
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Serves ColorAnimations within Templates.
    /// 
    /// Receives: 
    /// 1. double (a percentage running on some Property)
    /// 2. ControlTheme
    /// Returns: Brush of calculated Color for use on Background/Foreground/whatever
    /// </summary>
    public class PercentageToHoverBrushConverter : MultiConverterMarkupExtension<PercentageToHoverBrushConverter>
    {
        private Color FromColor { get; set; }
        private Color ToColor { get; set; }
        private Color DiffColor { get; set; }
        private ControlTheme _controlTheme = ControlTheme.None;
        private ControlTheme ControlTheme
        {
            get { return _controlTheme; }
            set
            {
                if (_controlTheme != value)
                {
                    _controlTheme = value;
                    SetColors();
                }
            }
        }

        public PercentageToHoverBrushConverter()
        {
            SetColors();
        }

        private void SetColors()
        {
            FromColor = Colors.Transparent;
            ToColor = Colors.Transparent;

            string resourceNameFromColor = null;
            string resourceNameToColor = null;

            switch (ControlTheme)
            {
                case ControlTheme.Primary:
                    {
                        resourceNameFromColor = "XColor.AlmostTransparent";
                        resourceNameToColor = "XColor.Button.Primary.Background.Hover";
                    }
                    break;

                case ControlTheme.Secondary:
                    {
                        resourceNameFromColor = "XColor.AlmostTransparent";
                        resourceNameToColor = "XColor.Button.Secondary.Background.Hover";
                    }
                    break;

                case ControlTheme.None:
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(resourceNameToColor) && !string.IsNullOrEmpty(resourceNameFromColor))
            {
                FromColor = (Color)Application.Current.TryFindResource(resourceNameFromColor);
                ToColor = (Color)Application.Current.TryFindResource(resourceNameToColor);
            }

            DiffColor = Color.Subtract(ToColor, FromColor);
        }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] == null || !(values[0] is double) ||
                values[1] == null || !(values[1] is Enum) || FromColor == null || ToColor == null)
                return Binding.DoNothing;

            ControlTheme = (ControlTheme)values[1];

            double percentage = (double)values[0];
            Color intermediateColor = Color.Multiply(DiffColor, (float)percentage);
            intermediateColor = Color.Add(intermediateColor, FromColor);

            return new SolidColorBrush(intermediateColor);
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Receives: ControlTheme
    /// Returns: Background brush
    /// </summary>
    public class ControlThemeToBackgroundConverter : ConverterMarkupExtension<ControlThemeToBackgroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Enum))
                return Binding.DoNothing;

            SolidColorBrush backgroundBrush = new SolidColorBrush();
            switch ((ControlTheme)value)
            {
                case ControlTheme.Primary:
                    backgroundBrush = (SolidColorBrush)Application.Current.TryFindResource("XBrush.Button.Primary.Background");
                    break;

                case ControlTheme.Secondary:
                    backgroundBrush = (SolidColorBrush)Application.Current.TryFindResource("XBrush.Button.Secondary.Background");
                    break;

                case ControlTheme.None:
                default:
                    return Binding.DoNothing;
            }

            return backgroundBrush;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Receives: ControlTheme
    /// Returns: Background brush when pressed
    /// </summary>
    public class ControlThemeToBackgroundPressedConverter : ConverterMarkupExtension<ControlThemeToBackgroundPressedConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Enum))
                return Binding.DoNothing;

            SolidColorBrush backgroundBrush = new SolidColorBrush();
            switch ((ControlTheme)value)
            {
                case ControlTheme.Primary:
                    backgroundBrush = (SolidColorBrush)Application.Current.TryFindResource("XBrush.Button.Primary.Background.Pressed");
                    break;

                case ControlTheme.Secondary:
                    backgroundBrush = (SolidColorBrush)Application.Current.TryFindResource("XBrush.Button.Secondary.Background.Pressed");
                    break;

                case ControlTheme.None:
                default:
                    return Binding.DoNothing;
            }

            return backgroundBrush;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Receives: ControlTheme
    /// Returns: Background brush when disabled
    /// </summary>
    public class ControlThemeToBackgroundDisabledConverter : ConverterMarkupExtension<ControlThemeToBackgroundDisabledConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Enum))
                return Binding.DoNothing;

            SolidColorBrush backgroundBrush = new SolidColorBrush();
            switch ((ControlTheme)value)
            {
                case ControlTheme.Primary:
                    backgroundBrush = (SolidColorBrush)Application.Current.TryFindResource("XBrush.Button.Primary.Background.Disabled");
                    break;

                case ControlTheme.Secondary:
                    backgroundBrush = (SolidColorBrush)Application.Current.TryFindResource("XBrush.Control.Background.Disabled");
                    break;

                case ControlTheme.None:
                default:
                    return Binding.DoNothing;
            }

            return backgroundBrush;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Receives: ControlTheme
    /// Returns: Foreground brush
    /// </summary>
    public class ControlThemeToForegroundConverter : ConverterMarkupExtension<ControlThemeToForegroundConverter>
        {
            public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            if (value == null || !(value is Enum))
                return Binding.DoNothing;

            SolidColorBrush foregroundBrush = new SolidColorBrush();
            switch ((ControlTheme)value)
            {
                case ControlTheme.Primary:
                    foregroundBrush = (SolidColorBrush)Application.Current.TryFindResource("XBrush.Button.Primary.Foreground");
                    break;

                case ControlTheme.Secondary:
                    foregroundBrush = (SolidColorBrush)Application.Current.TryFindResource("XBrush.Button.Secondary.Foreground");
                    break;

                case ControlTheme.None:
                default:
                    return Binding.DoNothing;
            }

            return foregroundBrush;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Receives: ControlTheme
    /// Returns: Foreground brush when disabled
    /// </summary>
    public class ControlThemeToForegroundDisabledConverter : ConverterMarkupExtension<ControlThemeToForegroundDisabledConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Enum))
                return Binding.DoNothing;

            SolidColorBrush foregroundBrush = new SolidColorBrush();
            switch ((ControlTheme)value)
            {
                case ControlTheme.Primary:
                    foregroundBrush = (SolidColorBrush)Application.Current.TryFindResource("XBrush.Button.Primary.Foreground.Disabled");
                    break;

                case ControlTheme.Secondary:
                    foregroundBrush = (SolidColorBrush)Application.Current.TryFindResource("XBrush.Control.Foreground.Disabled");
                    break;

                case ControlTheme.None:
                default:
                    return Binding.DoNothing;
            }

            return foregroundBrush;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

