using System.Windows;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
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

        /// <summary>
        /// IsActive imitates "IsChecked" property, for any control
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.RegisterAttached("IsActive", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false));
        public static bool GetIsActive(DependencyObject obj) { return (bool)obj.GetValue(IsActiveProperty); }
        public static void SetIsActive(DependencyObject obj, bool value) { obj.SetValue(IsActiveProperty, value); }

        /// <summary>
        /// Hover over control
        /// </summary>
        public static readonly DependencyProperty HoverProperty = DependencyProperty.RegisterAttached("Hover", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false));
        public static bool GetHover(DependencyObject obj) { return (bool)obj.GetValue(HoverProperty); }
        public static void SetHover(DependencyObject obj, bool value) { obj.SetValue(HoverProperty, value); }

        /// <summary>
        /// IsPressed for any control
        /// </summary>
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.RegisterAttached("IsPressed", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false));
        public static bool GetIsPressed(DependencyObject obj) { return (bool)obj.GetValue(IsPressedProperty); }
        public static void SetIsPressed(DependencyObject obj, bool value) { obj.SetValue(IsPressedProperty, value); }
    }
}
