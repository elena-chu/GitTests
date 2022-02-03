using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Ws.Extensions.UI.Wpf.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        //Set to true if you want to show control when boolean value is true
        //Set to false if you want to hide/collapse control when value is true
        private bool _triggerValue = true;
        public bool TriggerValue
        {
            get { return _triggerValue; }
            set { _triggerValue = value; }
        }

        private Visibility _invisible = Visibility.Collapsed;
        public Visibility Invisible
        {
            get { return _invisible; }
            set { _invisible = value; }
        }

        private object GetVisibility(object value)
        {
            if (!(value is bool))
                return DependencyProperty.UnsetValue;

            bool objValue = (bool)value;

            return objValue == TriggerValue ? Visibility.Visible : Invisible;
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            // Do the conversion from bool to visibility
            return GetVisibility(value);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            // Do the conversion from visibility to bool
            return null;
        }
    }

    public class BooleanToTrueFalseValueConverter : IValueConverter
    {
        public object TrueValue { get; set; }
        public object FalseValue { get; set; }
        public object NullValue { get; set; }
        public bool IsThreeState { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value is bool) && (bool)value ? TrueValue : IsThreeState && value == null ? NullValue : FalseValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvertedBooleanConverter : IValueConverter
    {
        public InvertedBooleanConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                return !((bool)value);
            }

            return true;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }

    }

    public class InvertedBooleanToVisibilityConverter : IValueConverter
    {
        public InvertedBooleanToVisibilityConverter()
        {
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is bool?)
            {
                flag = ((bool?)value).Value;
            }
            return (flag ? Visibility.Collapsed : Visibility.Visible);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (!(value is Visibility) ? false : (Visibility)value == Visibility.Collapsed);
        }
    }

    [ValueConversion(typeof(bool), typeof(Brush))]
    public class BoolToBrushConverter : MarkupExtension, IValueConverter
    {
        //private static readonly TraceSource m_trace = new TraceSource(Assembly.GetExecutingAssembly().GetName().Name);

        private static readonly Brush DefaultUnsetBrush = Brushes.Transparent;
        private static readonly Brush DefaultSetBrush = Brushes.Black;

        private Brush m_unsetBrush = DefaultUnsetBrush;
        private Brush m_setBrush = DefaultSetBrush;

        public string UnsetBrush
        {
            get
            {
                return m_unsetBrush.ToString();
            }
            set
            {
                m_unsetBrush = BrushFromString(value, DefaultUnsetBrush);
            }
        }

        public string SetBrush
        {
            get
            {
                return m_setBrush.ToString();
            }
            set
            {
                m_setBrush = BrushFromString(value, DefaultSetBrush);
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool))
            {
                return DependencyProperty.UnsetValue;
            }

            bool boolValue = (bool)value;

            return boolValue ? m_setBrush : m_unsetBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        private static Brush BrushFromString(string name, Brush defaultBrush)
        {
            if (string.IsNullOrEmpty(name))
                return defaultBrush;

            var pi = typeof(Brushes).GetProperty(name);

            if (pi == null)
            {
                //m_trace.TraceEvent(TraceEventType.Warning, 0, "Got unknown brush name: ", name);
                return defaultBrush;
            }

            return (Brush)pi.GetValue(null);
        }
    }
}
