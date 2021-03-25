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
using Ws.Fus.UI.Wpf.ViewModels.TripletCoordinate;

namespace Ws.Fus.UI.Wpf.Controls.TripletCoordinate
{
    /// <summary>
    /// Interaction logic for Triplet.xaml
    /// </summary>
    public partial class TripletCoordinateControl : UserControl
    {
        public TripletCoordinateControl()
        {
            InitializeComponent();
            DataContextChanged += Triplet_DataContextChanged;
        }

        private void Triplet_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && !(e.NewValue is TripletDm))
            {
                throw new Exception($"Expecting {nameof(TripletDm)}, Getting: {e.NewValue.GetType()}");
            }

        }

        public SeverityLevel SeverityLevel
        {
            get { return (SeverityLevel)this.GetValue(SeverityLevelProperty); }
            set { this.SetValue(SeverityLevelProperty, value); }
        }
        public static readonly DependencyProperty SeverityLevelProperty = DependencyProperty.Register(
          nameof(SeverityLevel), typeof(SeverityLevel), typeof(TripletCoordinateControl), new PropertyMetadata());




        public DisplayStatus DisplayStatus
        {
            get { return (DisplayStatus)this.GetValue(DisplayStatusProperty); }
            set { this.SetValue(DisplayStatusProperty, value); }
        }
        public static readonly DependencyProperty DisplayStatusProperty = DependencyProperty.Register(
          nameof(DisplayStatus), typeof(DisplayStatus), typeof(TripletCoordinateControl), new PropertyMetadata());

    }
}
