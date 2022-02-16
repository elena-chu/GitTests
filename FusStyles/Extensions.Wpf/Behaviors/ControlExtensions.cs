using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ws.Extensions.UI.Wpf.Controls;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    public enum HorizontalPlacement
    {
        Left,
        Middle,
        Right
    }

    public enum VerticalPlacement
    {
        Top,
        Middle,
        Bottom
    }

    public enum ControlState
    {
        Normal,
        CallToAction,
        Busy,
        Error
    }

    public enum Severity
    {
        Info,
        Warning,
        Error,
        Safety
    }

    /// <summary>
    ///  ControlExtensions: extra abilities for Controls
    /// </summary>
    public class ControlExtensions
    {
        /// <summary>
        /// Horizontal Placement for control in a series of controls
        /// </summary>
        public static readonly DependencyProperty HorizontalPlacementProperty = DependencyProperty.RegisterAttached("HorizontalPlacement", typeof(HorizontalPlacement), typeof(ControlExtensions), new PropertyMetadata(HorizontalPlacement.Middle));
        public static HorizontalPlacement GetHorizontalPlacement(DependencyObject obj) { return (HorizontalPlacement)obj.GetValue(HorizontalPlacementProperty); }
        public static void SetHorizontalPlacement(DependencyObject obj, HorizontalPlacement value) { obj.SetValue(HorizontalPlacementProperty, value); }

        /// <summary>
        /// Vertical Placement for control in a series of controls
        /// </summary>
        public static readonly DependencyProperty VerticalPlacementProperty = DependencyProperty.RegisterAttached("VerticalPlacement", typeof(VerticalPlacement), typeof(ControlExtensions), new PropertyMetadata(VerticalPlacement.Middle));
        public static VerticalPlacement GetVerticalPlacement(DependencyObject obj) { return (VerticalPlacement)obj.GetValue(VerticalPlacementProperty); }
        public static void SetVerticalPlacement(DependencyObject obj, VerticalPlacement value) { obj.SetValue(VerticalPlacementProperty, value); }

        /// <summary>
        /// Direction for control (Left, Right, Top, Bottom)
        /// </summary>
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.RegisterAttached("Direction", typeof(Dock), typeof(ControlExtensions), new PropertyMetadata(Dock.Left));
        public static Dock GetDirection(DependencyObject obj) { return (Dock)obj.GetValue(DirectionProperty); }
        public static void SetDirection(DependencyObject obj, Dock value) { obj.SetValue(DirectionProperty, value); }

        /// <summary>
        /// Used for setting a control's alternative text
        /// e.g. watermark for combo
        /// </summary>
        public static readonly DependencyProperty AlternativeTextProperty = DependencyProperty.RegisterAttached("AlternativeText", typeof(string), typeof(ControlExtensions), new PropertyMetadata(string.Empty));
        public static string GetAlternativeText(DependencyObject obj) { return (string)obj.GetValue(AlternativeTextProperty); }
        public static void SetAlternativeText(DependencyObject obj, string value) { obj.SetValue(AlternativeTextProperty, value); }

        /// <summary>
        /// Used for setting control's caption for Max Value etc.
        /// e.g. watermark for combo
        /// </summary>
        public static readonly DependencyProperty CaptionMaxProperty = DependencyProperty.RegisterAttached("CaptionMax", typeof(string), typeof(ControlExtensions), new PropertyMetadata(string.Empty));
        public static string GetCaptionMax(DependencyObject obj) { return (string)obj.GetValue(CaptionMaxProperty); }
        public static void SetCaptionMax(DependencyObject obj, string value) { obj.SetValue(CaptionMaxProperty, value); }

        /// <summary>
        /// Used for setting control's caption for Min Value etc.
        /// e.g. watermark for combo
        /// </summary>
        public static readonly DependencyProperty CaptionMinProperty = DependencyProperty.RegisterAttached("CaptionMin", typeof(string), typeof(ControlExtensions), new PropertyMetadata(string.Empty));
        public static string GetCaptionMin(DependencyObject obj) { return (string)obj.GetValue(CaptionMinProperty); }
        public static void SetCaptionMin(DependencyObject obj, string value) { obj.SetValue(CaptionMinProperty, value); }

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

        /// <summary>
        /// States for Control
        /// </summary>
        public static readonly DependencyProperty ControlStateProperty = DependencyProperty.RegisterAttached("ControlState", typeof(ControlState), typeof(ControlExtensions), new FrameworkPropertyMetadata(ControlState.Normal, FrameworkPropertyMetadataOptions.Inherits));
        public static ControlState GetControlState(DependencyObject obj) { return (ControlState)obj.GetValue(ControlStateProperty); }
        public static void SetControlState(DependencyObject obj, ControlState value) { obj.SetValue(ControlStateProperty, value); }

        /// <summary>
        /// Command for any control
        /// </summary>
        public static readonly DependencyProperty ControlCommandProperty = DependencyProperty.RegisterAttached("ControlCommand", typeof(ICommand), typeof(ControlExtensions));
        public static ICommand GetControlCommand(DependencyObject obj) { return (ICommand)obj.GetValue(ControlCommandProperty); }
        public static void SetControlCommand(DependencyObject obj, ICommand value) { obj.SetValue(ControlCommandProperty, value); }

        /// <summary>
        /// Severity
        /// </summary>
        public static readonly DependencyProperty SeverityProperty = DependencyProperty.RegisterAttached("Severity", typeof(Severity), typeof(ControlExtensions), new FrameworkPropertyMetadata(Severity.Info, FrameworkPropertyMetadataOptions.Inherits));
        public static Severity GetSeverity(DependencyObject obj) { return (Severity)obj.GetValue(SeverityProperty); }
        public static void SetSeverity(DependencyObject obj, Severity value) { obj.SetValue(SeverityProperty, value); }

        /// <summary>
        /// DisplayStatus
        /// </summary>
        public static readonly DependencyProperty DisplayStatusProperty = DependencyProperty.RegisterAttached("DisplayStatus", typeof(DisplayStatus), typeof(ControlExtensions), new FrameworkPropertyMetadata(DisplayStatus.Active, FrameworkPropertyMetadataOptions.Inherits));
        public static DisplayStatus GetDisplayStatus(DependencyObject obj) { return (DisplayStatus)obj.GetValue(DisplayStatusProperty); }
        public static void SetDisplayStatus(DependencyObject obj, DisplayStatus value) { obj.SetValue(DisplayStatusProperty, value); }




    }
}
