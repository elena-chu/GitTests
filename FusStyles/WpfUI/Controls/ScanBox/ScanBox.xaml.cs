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

namespace Ws.Fus.PlanningScans.UI.Wpf.Controls.ScanBox
{
    /// <summary>
    /// Interaction logic for ScanBox.xaml
    /// </summary>
    public partial class ScanBox : UserControl
    {
        /// <summary>
        /// Property defining whether the current window is Main Orientation - showing FOV square
        /// </summary>
        public static readonly DependencyProperty IsMainOrientationProperty =
            DependencyProperty.Register("IsMainOrientation", typeof(bool?), typeof(ScanBox),
            new FrameworkPropertyMetadata(null));

        public bool? IsMainOrientation
        {
            get { return (bool)GetValue(IsMainOrientationProperty); }
            set { SetValue(IsMainOrientationProperty, value); }
        }

        /// <summary>
        /// Property should be bound to class responsible for
        /// exchanging data between Application and ScanBox controls
        /// </summary>
        public static readonly DependencyProperty ScanBoxAwareProperty =
                    DependencyProperty.Register("ScanBoxAware", typeof(IScanBoxAware), typeof(ScanBox),
                    new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnScanBoxAwareChanged)));

        public IScanBoxAware ScanBoxAware
        {
            get { return (IScanBoxAware)GetValue(ScanBoxAwareProperty); }
            set { SetValue(ScanBoxAwareProperty, value); }
        }
        private static void OnScanBoxAwareChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue || !(e.NewValue is IScanBoxAware))
                return;

        }


        public ScanBox()
        {
            InitializeComponent();
        }
    }
}
