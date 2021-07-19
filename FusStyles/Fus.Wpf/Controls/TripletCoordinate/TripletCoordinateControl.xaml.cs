using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
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
            Loaded += TripletCoordinateControl_Loaded;
            Triplet = new TripletDm();
        }

        private void TripletCoordinateControl_Loaded(object sender, RoutedEventArgs e)
        {
           
            Triplet.PropertyChanged += Triplet_PropertyChanged;
        }

        private void Triplet_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           
            switch (e.PropertyName)
            {
                case nameof(TripletDm.Point):
                    //GW    move from DM to DP
                    if (Triplet.HasValue)
                    {
                        Point = Triplet.Point;
                    }
                    break;
                case nameof(TripletDm.HasValue):
                    //GW
                    if (Triplet.HasValue)
                    {
                        Point = Triplet.Point;
                    }
                    break;
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
           public Point3D? Point    
        {
            get { return (Point3D?)this.GetValue(PointProperty); }
            set
            {
                this.SetValue(PointProperty, value);
            }
        }
        public static readonly DependencyProperty PointProperty = DependencyProperty.Register(
          nameof(Point), typeof(Point3D?), typeof(TripletCoordinateControl), new PropertyMetadata(OnPointChangedCallBack));

        public TripletDm Triplet
        {
            get { return (TripletDm)this.GetValue(TripletProperty); }
            set { this.SetValue(TripletProperty, value); }
        }
        public static readonly DependencyProperty TripletProperty = DependencyProperty.Register(
          nameof(Triplet), typeof(TripletDm), typeof(TripletCoordinateControl), null);

        private static void OnPointChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TripletCoordinateControl us = sender as TripletCoordinateControl;
            if (us != null)
            {
                if (e.Property.Name == nameof(Point) && us.Triplet != null)
                    if (!us.Triplet.Point.Equals(us.Point))
                    {
                        // GW nullable
                        //us.Triplet.Point = us.Point;
                        if (us.Point.HasValue)
                        {
                            us.Triplet.Point = us.Point.Value;
                        }
                        else
                        {
                            us.Triplet.Reset();
                        }
                    }
            }
        }
    }
}
