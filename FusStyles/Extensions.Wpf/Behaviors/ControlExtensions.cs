using System.Windows;
using System.Windows.Input;

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
        /// Used for animation when hovering over controls
        /// </summary>
        public static readonly DependencyProperty HoverPercentageProperty = DependencyProperty.RegisterAttached("HoverPercentage", typeof(double), typeof(ControlExtensions), new PropertyMetadata(0.0));
        public static double GetHoverPercentage(DependencyObject obj) { return (double)obj.GetValue(HoverPercentageProperty); }
        public static void SetHoverPercentage(DependencyObject obj, double value) { obj.SetValue(HoverPercentageProperty, value); }

        /// <summary>
        /// Used for animation when pressing controls
        /// </summary>
        public static readonly DependencyProperty PressedPercentageProperty = DependencyProperty.RegisterAttached("PressedPercentage", typeof(double), typeof(ControlExtensions), new PropertyMetadata(0.0));
        public static double GetPressedPercentage(DependencyObject obj) { return (double)obj.GetValue(PressedPercentageProperty); }
        public static void SetPressedPercentage(DependencyObject obj, double value) { obj.SetValue(PressedPercentageProperty, value); }

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


        #region IsPressed

        /// <summary>
        /// Attach IsPressed property to any FrameworkElement
        /// </summary>
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.RegisterAttached("IsPressed", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false));
        public static bool GetIsPressed(UIElement element) { return (bool)element.GetValue(IsPressedProperty); }
        public static void SetIsPressed(UIElement element, bool val) { element.SetValue(IsPressedProperty, val); }

        public static readonly DependencyProperty AttachIsPressedProperty = DependencyProperty.RegisterAttached("AttachIsPressed", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false, OnAttachIsPressedChanged));
        public static bool GetAttachIsPressed(UIElement element) { return (bool)element.GetValue(AttachIsPressedProperty); }
        public static void SetAttachIsPressed(UIElement element, bool val) { element.SetValue(AttachIsPressedProperty, val); }

        public static void OnAttachIsPressedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)d;
            if (element != null)
            {
                if ((bool)e.NewValue)
                {
                    element.MouseDown += new MouseButtonEventHandler(element_MouseDown);
                    element.MouseUp += new MouseButtonEventHandler(element_MouseUp);
                    element.MouseLeave += new MouseEventHandler(element_MouseLeave);
                }
                else
                {
                    element.MouseDown -= new MouseButtonEventHandler(element_MouseDown);
                    element.MouseUp -= new MouseButtonEventHandler(element_MouseUp);
                    element.MouseLeave -= new MouseEventHandler(element_MouseLeave);
                }
            }
        }

        static void element_MouseLeave(object sender, MouseEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            if (element != null)
                element.SetValue(IsPressedProperty, false);
        }

        static void element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            if (element != null)
                element.SetValue(IsPressedProperty, false);
        }

        static void element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            if (element != null)
                element.SetValue(IsPressedProperty, true);
        }

        #endregion
    }
}
