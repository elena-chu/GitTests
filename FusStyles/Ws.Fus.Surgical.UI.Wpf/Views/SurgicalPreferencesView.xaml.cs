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
    /// Interaction logic for SurgicalPreferencesView.xaml
    /// </summary>
    public partial class SurgicalPreferencesView : UserControl
    {
        //private bool _isOpen = false;

        public SurgicalPreferencesView()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                DataContext = new object();
            };
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    _isOpen = !_isOpen;
        //    if (_isOpen)
        //    {
        //        this.tabControl.Height = 300;
        //    }
        //    else
        //    {
        //        tabControl.Height = 50;
        //    }
        //}
    }
}
