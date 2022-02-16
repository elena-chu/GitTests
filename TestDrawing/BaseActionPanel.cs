using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TestDrawing
{
    public abstract class BaseActionPanel : Canvas
    {
        public static readonly DependencyProperty ScaleFactorProperty =
            DependencyProperty.Register("ScaleFactor", typeof(double), typeof(BaseActionPanel),
            new FrameworkPropertyMetadata(1d, new PropertyChangedCallback(OnScaleFactorChanged)));

        public double ScaleFactor
        {
            set { SetValue(ScaleFactorProperty, value); }
            get { return (double)GetValue(ScaleFactorProperty); }
        }
        private static void OnScaleFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BaseActionPanel panel = d as BaseActionPanel;
            if (panel.InitialParams != null)
                panel.BuildSpecific(panel.InitialParams);
        }


        protected bool _isInitialized = false;
        protected TranslateTransform _translateTransform;
        protected RotateTransform _rotateTransform;

        protected Rectangle _mainSquare;
        protected Rectangle _secondSquare;
        protected Ellipse _vectorPoint;

        protected Point _startingCurMovePoint;
        protected Point _startingMoveLeftTopPoint;

        protected double _origOffsetX;
        protected double _origOffsetY;

        protected double _accumOffsetX = 0;
        protected double _accumOffsetY = 0;

        protected static event EventHandler<Point> _centerMoving;
        protected static event EventHandler<Point> _centerChanged;

        public BaseActionPanel()
        {
            this.Width = 1000;
            this.Height = 1000;

            TransformGroup tg = new TransformGroup();
            _translateTransform = new TranslateTransform(0, 0);
            tg.Children.Add(_translateTransform);
            _rotateTransform = new RotateTransform(0);
            tg.Children.Add(_rotateTransform);
            this.RenderTransform = tg;

            this.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        public bool IsZoomable { get; set; }

        public bool IsRotatable { get; set; }

        private bool _isMovable;
        public bool IsMovable
        {
            get { return _isMovable; }
            set { _isMovable = value; }
        }

        public bool IsBig { get; set; }

        public BuildParameters InitialParams { get; private set; }


        public void Build(BuildParameters initialParams)
        {
            InitialParams = initialParams;

            _translateTransform.X = 0;
            _translateTransform.Y = 0;
            _rotateTransform.Angle = 0;

            _accumOffsetX = 0;
            _accumOffsetY = 0;
            ScaleFactor = 1;

            BuildSpecific(InitialParams);

            CreateCenter();

            //double left = initialParams.CenterCoord.X - this.Width / 2;
            //Canvas.SetLeft(this, left);
            //double top = initialParams.CenterCoord.Y - this.Height / 2;
            //Canvas.SetTop(this, top);
            SetViewPositionByCenterCoord(initialParams.CenterCoord);

            _centerMoving += OnCenterMoving;
            _centerChanged += OnCenterChanged;

            _isInitialized = true;
        }

        private void OnCenterChanged(object sender, Point center)
        {
            OnCenterMoving(sender, center);

            InitialParams.CenterCoord = center;

        }

        protected virtual void OnCenterMoving(object sender, Point center)
        {
            if (sender == this)
                return;

            SetViewPositionByCenterCoord(center);
        }

        public abstract void BuildSpecific(BuildParameters initialParams);

        protected void SetViewPositionByCenterCoord(Point center)
        {
            double left = center.X - this.Width / 2;
            Canvas.SetLeft(this, left);
            double top = center.Y - this.Height / 2;
            Canvas.SetTop(this, top);
        }


        protected Rectangle FindMain(string name)
        {
            Rectangle main = this.FindName(name) as Rectangle;
            if (main == null)
            {
                main = new Rectangle();
                main.Name = name;
                Style mainStyle = (Style)Application.Current.FindResource(name);
                main.Style = mainStyle;

                this.Children.Add(main);
            }

            return main;
        }


        protected Rectangle FindSecondary(string name)
        {
            Rectangle second = this.FindName(name) as Rectangle;
            if (second == null)
            {
                second = new Rectangle();
                second.Name = name;
                Style secStyle = (Style)Application.Current.FindResource(name);
                second.Style = secStyle;
                this.Children.Add(second);
            }

            return second;
        }

        protected void OnSecondSquareMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).CaptureMouse();
            _origOffsetX = _translateTransform.X;
            _origOffsetY = _translateTransform.Y;
            _startingCurMovePoint = Mouse.GetPosition((FrameworkElement)this.VisualParent);
            _startingMoveLeftTopPoint = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));

            //Debug.WriteLine($"LeftButtonDown. X: {_startingCurMovePoint.X}, Y: {_startingCurMovePoint.Y}");

            _secondSquare.MouseMove += OnSecondSquareMouseMove;
        }

        protected void OnSecondSquareMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var curPos = Mouse.GetPosition((FrameworkElement)this.VisualParent);
                double xOffset = _origOffsetX + (curPos.X - _startingCurMovePoint.X);
                double yOffset = _origOffsetY + (curPos.Y - _startingCurMovePoint.Y);
                //Debug.WriteLine($"MouseMove. X: {curPos.X}, Y: {curPos.Y}, offsetX: { xOffset }, offsetY: {yOffset}");

                //_translateTransform.X = xOffset;
                //_translateTransform.Y = yOffset;

                //double left = _startingMoveLeftTopPoint.X + xOffset;
                //double top = _startingMoveLeftTopPoint.Y + yOffset;
                //Canvas.SetLeft(this, left);
                //Canvas.SetTop(this, top);

                double x = InitialParams.CenterCoord.X + xOffset;
                double y = InitialParams.CenterCoord.Y + yOffset;
                Point newCenter = new Point(x, y);
                SetViewPositionByCenterCoord(newCenter);
                _centerMoving?.Invoke(this, newCenter);
            }
        }

        protected void OnSecondSquareMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _secondSquare.MouseMove -= OnSecondSquareMouseMove;
            var curPos = Mouse.GetPosition((FrameworkElement)this.VisualParent);
            double xOffset = _origOffsetX + (curPos.X - _startingCurMovePoint.X);
            double yOffset = _origOffsetY + (curPos.Y - _startingCurMovePoint.Y);

            _accumOffsetX += xOffset;
            _accumOffsetY += yOffset;

            _translateTransform.X = 0;
            _translateTransform.Y = 0;

            //double left = InitialParams.CenterCoord.X - this.Width / 2 + _accumOffsetX;
            //Canvas.SetLeft(this, left);
            //double top = InitialParams.CenterCoord.Y - this.Height / 2 + _accumOffsetY;
            //Canvas.SetTop(this, top);

            //double left = _startingMoveLeftTopPoint.X + xOffset;
            //Canvas.SetLeft(this, left);
            //double top = _startingMoveLeftTopPoint.Y + yOffset;
            //Canvas.SetTop(this, top);

            double x = InitialParams.CenterCoord.X + xOffset;
            double y = InitialParams.CenterCoord.Y + yOffset;
            SetViewPositionByCenterCoord(new Point(x, y));


            ((FrameworkElement)sender).ReleaseMouseCapture();

            Debug.WriteLine($"MouseLeftButtonUp. accumX: {_accumOffsetX}, accumY: {_accumOffsetY}");
            //Debug.WriteLine($"Move XY: {_translateTransform.X}, {_translateTransform.Y}: Matrix: {this.RenderTransform.Value}");
        }

        protected void CreateCenter()
        {

            Ellipse center = this.FindName("CentralPoint") as Ellipse;
            if(center == null)
            {
                center = new Ellipse();
                Style centralStyle = (Style)Application.Current.FindResource("CentralPoint");
                center.Style = centralStyle;
                Canvas.SetLeft(center, (this.Width / 2 - center.Width / 2));
                Canvas.SetTop(center, (this.Height / 2 - center.Height / 2));
                this.Children.Add(center);
            }
            SetVectorPoint();
        }

        protected void SetVectorPoint()
        {
            if (_vectorPoint == null)
            {
                _vectorPoint = this.FindName("VectorPoint") as Ellipse;
                if (_vectorPoint == null)
                {
                    _vectorPoint = new Ellipse();
                    _vectorPoint.Name = "VectorPoint";
                    Style centralStyle = (Style)Application.Current.FindResource("CentralPoint");
                    _vectorPoint.Style = centralStyle;
                    this.Children.Add(_vectorPoint);
                }
            }
            Canvas.SetLeft(_vectorPoint, (this.Width / 2 - _vectorPoint.Width / 2));
            Canvas.SetTop(_vectorPoint, (this.Height / 2 - _vectorPoint.Height / 2 - _mainSquare.Height / 2));

        }
    }
}
