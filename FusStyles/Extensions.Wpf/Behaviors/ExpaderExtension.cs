using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    public class ExpaderExtension
    {

        public static readonly DependencyProperty OpenHeaderTextProperty = DependencyProperty.RegisterAttached("OpenHeaderText", typeof(string), typeof(ExpaderExtension), new PropertyMetadata(null));
        public static string GetOpenHeaderText(DependencyObject obj) { return (string)obj.GetValue(OpenHeaderTextProperty); }
        public static void SetOpenHeaderText(DependencyObject obj, string value) { obj.SetValue(OpenHeaderTextProperty, value); }


        public static readonly DependencyProperty CloseHeaderTextProperty = DependencyProperty.RegisterAttached("CloseHeaderText", typeof(string), typeof(ExpaderExtension), new PropertyMetadata(null));
        public static string GetCloseHeaderText(DependencyObject obj) { return (string)obj.GetValue(CloseHeaderTextProperty); }
        public static void SetCloseHeaderText(DependencyObject obj, string value) { obj.SetValue(CloseHeaderTextProperty, value); }


    }
}
