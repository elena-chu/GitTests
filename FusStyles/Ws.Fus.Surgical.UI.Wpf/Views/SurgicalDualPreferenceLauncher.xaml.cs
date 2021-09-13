using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ws.Fus.Surgical.UI.Wpf
{
    /// <summary>
    /// Interaction logic for SurgicalDualPreferenceLauncher.xaml
    /// </summary>
    public partial class SurgicalDualPreferenceLauncher : UserControl
    {
        public SurgicalDualPreferenceLauncher()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty BaseContentProperty =
        DependencyProperty.Register("BaseContent", typeof(object), typeof(SurgicalDualPreferenceLauncher));

        public object BaseContent
        {
            get { return (object)GetValue(BaseContentProperty); }
            set { SetValue(BaseContentProperty, value); }
        }



        public static readonly DependencyProperty AdvancedContentProperty =
        DependencyProperty.Register("AdvancedContent", typeof(object), typeof(SurgicalDualPreferenceLauncher));

        public object AdvancedContent
        {
            get { return (object)GetValue(AdvancedContentProperty); }
            set { SetValue(AdvancedContentProperty, value); }
        }

        private void AdvancedClick(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = !popup.IsOpen;
        }
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
        }
    }
}
