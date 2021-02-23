using System.Windows;
using System.Windows.Media;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    /// <summary>
    ///  IconedButton class enabling to add to any button properties describing button's icon
    /// </summary>
    public class IconedButton
    {
        /// <summary>
        /// Icon property is describing brush used to draw the button's icon
        /// </summary>
        public static DependencyProperty IconProperty = DependencyProperty.RegisterAttached("Icon",
            typeof(TileBrush), typeof(IconedButton));

        public static void SetIcon(FrameworkElement obj, TileBrush value)
        {
            obj.SetValue(IconProperty, value);
        }
        public static TileBrush GetIcon(FrameworkElement obj)
        {
            return (TileBrush)obj.GetValue(IconProperty);
        }

        /// <summary>
        /// Icon property is describing brush used to draw the button's active icon, if it is different from regular
        /// </summary>
        public static DependencyProperty ActiveIconProperty = DependencyProperty.RegisterAttached("ActiveIcon",
            typeof(TileBrush), typeof(IconedButton));

        public static void SetActiveIcon(FrameworkElement obj, TileBrush value)
        {
            obj.SetValue(ActiveIconProperty, value);
        }
        public static TileBrush GetActiveIcon(FrameworkElement obj)
        {
            return (TileBrush)obj.GetValue(ActiveIconProperty);
        }

        /// <summary>
        /// Alpha Geometry: geometry that will be colored with Alpha active color
        /// </summary>
        public static DependencyProperty AlphaGeometryProperty = DependencyProperty.RegisterAttached("AlphaGeometry", typeof(System.Windows.Media.Geometry), typeof(IconedButton));

        public static void SetAlphaGeometry(FrameworkElement obj, System.Windows.Media.Geometry value)
        {
            obj.SetValue(AlphaGeometryProperty, value);
        }
        public static System.Windows.Media.Geometry GetAlphaGeometry(FrameworkElement obj)
        {
            return (System.Windows.Media.Geometry)obj.GetValue(AlphaGeometryProperty);
        }

        /// <summary>
        /// Beta Geometry: geometry that will be colored with Beta active color
        /// </summary>
        public static DependencyProperty BetaGeometryProperty = DependencyProperty.RegisterAttached("BetaGeometry", typeof(System.Windows.Media.Geometry), typeof(IconedButton));

        public static void SetBetaGeometry(FrameworkElement obj, System.Windows.Media.Geometry value)
        {
            obj.SetValue(BetaGeometryProperty, value);
        }
        public static System.Windows.Media.Geometry GetBetaGeometry(FrameworkElement obj)
        {
            return (System.Windows.Media.Geometry)obj.GetValue(BetaGeometryProperty);
        }

        /// <summary>
        /// Horizontal Alignment of Icon
        /// </summary>
        public static DependencyProperty IconHorizontalAlignmentProperty = DependencyProperty.RegisterAttached("IconHorizontalAlignment", typeof(HorizontalAlignment), typeof(IconedButton));

        public static void SetIconHorizontalAlignment(FrameworkElement obj, HorizontalAlignment value)
        {
            obj.SetValue(IconHorizontalAlignmentProperty, value);
        }
        public static HorizontalAlignment GetIconHorizontalAlignment(FrameworkElement obj)
        {
            return (HorizontalAlignment)obj.GetValue(IconHorizontalAlignmentProperty);
        }

        /// <summary>
        /// Icon size
        /// </summary>
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.RegisterAttached("IconSize", typeof(double), typeof(IconedButton), new PropertyMetadata(20.0));

        public static double GetIconSize(DependencyObject obj)
        {
            return (double)obj.GetValue(IconSizeProperty);
        }

        public static void SetIconSize(DependencyObject obj, double value)
        {
            obj.SetValue(IconSizeProperty, value);
        }

    }
}
