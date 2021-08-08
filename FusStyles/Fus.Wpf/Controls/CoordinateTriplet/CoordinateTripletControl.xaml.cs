using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Ws.Extensions.UI.Wpf.Controls;

namespace Ws.Fus.UI.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for CoordinateTripletControl.xaml
    /// </summary>
    public partial class CoordinateTripletControl : UserControl
    {
        #region Statuses

        /// <summary>
        /// Propery defines the Value Severity level. Available values: Undefined, High, Low, Normal(default)
        /// </summary>
        public static readonly DependencyProperty SeverityLevelProperty = DependencyProperty.Register(
          nameof(SeverityLevel), typeof(SeverityLevel), typeof(CoordinateTripletControl), new PropertyMetadata(SeverityLevel.Normal));
        public SeverityLevel SeverityLevel
        {
            get { return (SeverityLevel)this.GetValue(SeverityLevelProperty); }
            set { this.SetValue(SeverityLevelProperty, value); }
        }

        /// <summary>
        /// Property notifies whether any of child elements has a focus now.
        /// </summary>
        public static readonly DependencyProperty DisplayStatusProperty = DependencyProperty.Register(
          nameof(DisplayStatus), typeof(DisplayStatus), typeof(CoordinateTripletControl), new PropertyMetadata(DisplayStatus.Active, OnPointChanged));
        public DisplayStatus DisplayStatus
        {
            get { return (DisplayStatus)this.GetValue(DisplayStatusProperty); }
            set { this.SetValue(DisplayStatusProperty, value); }
        }

        #endregion


        #region Display

        /// <summary>
        /// Size of gap between NumericUpDown elements
        /// </summary>
        public double Gap
        {
            get { return (double)GetValue(GapProperty); }
            set { SetValue(GapProperty, value); }
        }
        public static readonly DependencyProperty GapProperty = DependencyProperty.Register(nameof(Gap), typeof(double), typeof(CoordinateTripletControl), new PropertyMetadata(0.0));

        public bool IsNarrow
        {
            get { return (bool)GetValue(IsNarrowProperty); }
            set { SetValue(IsNarrowProperty, value); }
        }
        public static readonly DependencyProperty IsNarrowProperty = DependencyProperty.Register(nameof(IsNarrow), typeof(bool), typeof(CoordinateTripletControl), new PropertyMetadata(false));

        #endregion


        #region Point

        /// <summary>
        /// Coordinate Point of type Point3D. Nullable
        /// </summary>
        public static readonly DependencyProperty PointProperty = DependencyProperty.Register(
          nameof(Point), typeof(Point3D?), typeof(CoordinateTripletControl), new PropertyMetadata(OnPointChanged));
        public Point3D? Point
        {
            get { return (Point3D?)this.GetValue(PointProperty); }
            set { this.SetValue(PointProperty, value); }
        }

        private static void OnPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoordinateTripletControl control = d as CoordinateTripletControl;
            if (control == null || control._isPointUpdating)
                return;

            control._isPointUpdating = true;
            if (e.NewValue == null)
            {
                control.X = null;
                control.Y = null;
                control.Z = null;
            }
            else if (e.NewValue is Point3D)
            {
                Point3D point = (Point3D)e.NewValue;

                control.X = point.X;
                control.Y = point.Y;
                control.Z = point.Z;
            }
            control._isPointUpdating = false;
        }

        /// <summary>
        /// X coordinate of Point. For inner bindings of UpDownControl
        /// </summary>
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            nameof(X), typeof(double?), typeof(CoordinateTripletControl), new PropertyMetadata(null, OnSingleCoordinateChanged));
        public double? X
        {
            get { return (double?)this.GetValue(XProperty); }
            set { this.SetValue(XProperty, value); }
        }

        /// <summary>
        /// Y coordinate of Point. For inner bindings of UpDownControl
        /// </summary>
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            nameof(Y), typeof(double?), typeof(CoordinateTripletControl), new PropertyMetadata(null, OnSingleCoordinateChanged));

        public double? Y
        {
            get { return (double?)this.GetValue(YProperty); }
            set { this.SetValue(YProperty, value); }
        }

        /// <summary>
        /// Y coordinate of Point. For inner bindings of UpDownControl
        /// </summary>
        public static readonly DependencyProperty ZProperty = DependencyProperty.Register(
            nameof(Z), typeof(double?), typeof(CoordinateTripletControl), new PropertyMetadata(null, OnSingleCoordinateChanged));

        public double? Z
        {
            get { return (double?)this.GetValue(ZProperty); }
            set { this.SetValue(ZProperty, value); }
        }

        private static void OnSingleCoordinateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoordinateTripletControl control = d as CoordinateTripletControl;
            if (control == null || control._isPointUpdating || !control.HasValue)
                return;

            control._isPointUpdating = true;
            control.Point = new Point3D(control.X.Value, control.Y.Value, control.Z.Value);
            control._isPointUpdating = false;
        }

        #endregion

        private bool _isPointUpdating = false;

        public CoordinateTripletControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Notifies whether all Point's coordinates have numeric values
        /// </summary>
        public bool HasValue
        {
            get { return X.HasValue && Y.HasValue && Z.HasValue; }
        }

    }
}
