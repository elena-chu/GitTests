using System.Windows;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    public enum ControlTheme
    {
        None,
        Primary,
        Secondary
    }

    /// <summary>
    ///  ControlExtensions: extra abilities for Controls
    /// </summary>
    public class ControlExtensions
    {
        /// <summary>
        /// Used to set a Control's theme
        /// </summary>
        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.RegisterAttached("ControlTheme", typeof(ControlTheme), typeof(ControlExtensions), new PropertyMetadata(ControlTheme.Secondary));

        public static ControlTheme GetControlTheme(DependencyObject obj)
        {
            return (ControlTheme)obj.GetValue(ControlThemeProperty);
        }

        public static void SetControlTheme(DependencyObject obj, ControlTheme value)
        {
            obj.SetValue(ControlThemeProperty, value);
        }


        /// <summary>
        /// Used for anything that needs a running param from 0 to 1 - animation, rotation, etc.
        /// </summary>
        public static readonly DependencyProperty RunningPercentageProperty = DependencyProperty.RegisterAttached("RunningPercentage", typeof(double), typeof(ControlExtensions), new PropertyMetadata(0.0));

        public static double GetRunningPercentage(DependencyObject obj)
        {
            return (double)obj.GetValue(RunningPercentageProperty);
        }

        public static void SetRunningPercentage(DependencyObject obj, double value)
        {
            obj.SetValue(RunningPercentageProperty, value);
        }
    }
}
