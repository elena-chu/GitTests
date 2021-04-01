using System.Windows;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    public enum ControlTheme
    {
        None,
        Primary,
        Secondary
    }

    public enum Placement
    {
        Left,
        Middle,
        Right
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
        public static ControlTheme GetControlTheme(DependencyObject obj) { return (ControlTheme)obj.GetValue(ControlThemeProperty); }
        public static void SetControlTheme(DependencyObject obj, ControlTheme value) { obj.SetValue(ControlThemeProperty, value); }

        /// <summary>
        /// Used for anything that needs a running param from 0 to 1 - animation, rotation, etc.
        /// </summary>
        public static readonly DependencyProperty RunningPercentageProperty = DependencyProperty.RegisterAttached("RunningPercentage", typeof(double), typeof(ControlExtensions), new PropertyMetadata(0.0));
        public static double GetRunningPercentage(DependencyObject obj) { return (double)obj.GetValue(RunningPercentageProperty); }
        public static void SetRunningPercentage(DependencyObject obj, double value) { obj.SetValue(RunningPercentageProperty, value); }

        /// <summary>
        /// Used for setting a control's style when placed in a consecutive line
        /// e.g. the leftmost button in a ToolBar might have rounded corners as opposed to middle buttons
        /// </summary>
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement", typeof(Placement), typeof(ControlExtensions), new PropertyMetadata(Placement.Middle));
        public static Placement GetPlacement(DependencyObject obj) { return (Placement)obj.GetValue(PlacementProperty); }
        public static void SetPlacement(DependencyObject obj, Placement value) { obj.SetValue(PlacementProperty, value); }

        /// <summary>
        /// Used for setting a control's alternative text
        /// e.g. watermark for combo
        /// </summary>
        public static readonly DependencyProperty AlternativeTextProperty = DependencyProperty.RegisterAttached("AlternativeText", typeof(string), typeof(ControlExtensions), new PropertyMetadata(string.Empty));
        public static string GetAlternativeText(DependencyObject obj) { return (string)obj.GetValue(AlternativeTextProperty); }
        public static void SetAlternativeText(DependencyObject obj, string value) { obj.SetValue(AlternativeTextProperty, value); }
    }
}
