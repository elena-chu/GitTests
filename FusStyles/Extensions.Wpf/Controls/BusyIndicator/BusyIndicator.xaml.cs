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

namespace Ws.Extensions.UI.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for BusyIndicator.xaml
    /// </summary>
    public partial class BusyIndicator : UserControl
    {
        public BusyIndicator()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Identifies the IsBusy dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsBusyChanged)));

        /// <summary>
        /// Gets or sets a value indicating whether the busy indicator should show.
        /// </summary>
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        /// <summary>
        /// IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="d">BusyIndicator that changed its IsBusy.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator)d).OnIsBusyChanged(e);
        }

        /// <summary>
        /// IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
            }
            else
            {
            }

            //ChangeVisualState(true);
        }

        /// <summary>
        /// BusyText dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyTextProperty = DependencyProperty.Register(
            "BusyText",
            typeof(string),
            typeof(BusyIndicator),
            new PropertyMetadata("Loading..."));

        /// <summary>
        /// Gets or sets a value of Busy Text.
        /// </summary>
        public string BusyText
        {
            get { return (string)GetValue(BusyTextProperty); }
            set { SetValue(BusyTextProperty, value); }
        }

    }
}
