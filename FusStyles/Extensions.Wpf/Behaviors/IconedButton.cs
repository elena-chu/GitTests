using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
