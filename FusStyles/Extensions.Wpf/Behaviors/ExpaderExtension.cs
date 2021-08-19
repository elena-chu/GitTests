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


        public static readonly DependencyProperty ClosedHeaderTextProperty = DependencyProperty.RegisterAttached("ClosedHeaderText", typeof(string), typeof(ExpaderExtension), new PropertyMetadata(null));
        public static string GetClosedHeaderText(DependencyObject obj) { return (string)obj.GetValue(ClosedHeaderTextProperty); }
        public static void SetClosedHeaderText(DependencyObject obj, string value) { obj.SetValue(ClosedHeaderTextProperty, value); }


    }
}
