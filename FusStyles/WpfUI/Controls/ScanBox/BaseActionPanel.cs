using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Ws.Extensions.UI.Wpf.Utils;

namespace Ws.Fus.PlanningScans.UI.Wpf.Controls.ScanBox
{
    /// <summary>
    /// Base class for Orientation Panels
    /// </summary>
    public abstract class BaseActionPanel : Canvas
    {
        #region Dependency Properties

        /// <summary>
        /// Property should be bound to class responsible for
        /// exchanging data between Application and ScanBox controls
        /// </summary>
        public static readonly DependencyProperty ScanBoxAwareProperty =
                    DependencyProperty.Register("ScanBoxAware", typeof(IScanBoxAware), typeof(BaseActionPanel),
                    new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnScanBoxAwareChanged)));

        public IScanBoxAware ScanBoxAware
        {
            get { return (IScanBoxAware)GetValue(ScanBoxAwareProperty); }
            set { SetValue(ScanBoxAwareProperty, value); }
        }
        protected static void OnScanBoxAwareChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue || !(e.NewValue is IScanBoxAware))
                return;

            BaseActionPanel panel = (BaseActionPanel)d;
            if(panel != null)
            {
                panel.UnsubscribeFromScanboxAwareEvents(e.OldValue as IScanBoxAware);
                panel.InitialParams = null;
                IScanBoxAware sba = e.NewValue as IScanBoxAware;
                if(sba != null)
                {
                    FrameworkElement parent = (FrameworkElement)(panel.VisualParent);
                    Size size = new Size(parent.ActualWidth, parent.ActualHeight);
                    sba.ContainerSize = size;
                    panel.SubscribeToScanboxAwareEvents(sba);
                }
                panel.OnGeometryChanged(panel, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Property enabling interaction with Fge - enabling/disabling the Fge reaction to mouse events.
        /// </summary>
        public static readonly DependencyProperty IsFgeMouseEventsDisabledProperty =
            DependencyProperty.Register("IsFgeMouseEventsDisabled", typeof(bool), typeof(BaseActionPanel),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsFgeMouseEventsDisabledChanged)));

        public bool IsFgeMouseEventsDisabled
        {
            get { return (bool)GetValue(IsFgeMouseEventsDisabledProperty); }
            set { SetValue(IsFgeMouseEventsDisabledProperty, value); }
        }
        protected static void OnIsFgeMouseEventsDisabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(!(bool)e.NewValue)
            {
                BaseActionPanel panel = (BaseActionPanel)d;
                if (panel != null)
                {
                   panel._fgeDisabledByDrag = false;
                }
            }
        }

        /// <summary>
        /// Slice Number property, updated on changing From/To borders
        /// </summary>
        public static readonly DependencyProperty SliceNumberProperty =
            DependencyProperty.Register("SliceNumber", typeof(int), typeof(BaseActionPanel));

        public int SliceNumber
        {
            get { return (int)GetValue(SliceNumberProperty); }
            set { SetValue(SliceNumberProperty, value); }
        }

        #endregion

        protected bool _isInitialized = false;
        protected bool _fgeDisabledByDrag;

        protected TranslateTransform _translateTransform;
        protected RotateTransform _rotateTransform;

        protected Rectangle _mainSquare;
        protected Rectangle _secondSquare;
        protected Ellipse _vectorPoint;

        protected Point _startingCurMovePoint;
        protected Point _startingCenterPoint;


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

            this.Loaded += OnLoaded;
        }

        #region Public Properties

        public bool IsRotatable
        {
            get { return ScanBoxAware != null ? ScanBoxAware.IsRotatable : false; }
        }

        public bool IsMovable
        {
            get { return ScanBoxAware != null ? ScanBoxAware.IsMovable : true; }
        }


        public abstract void BuildSpecific(ScanBoxParams initialParams);

        private ScanBoxParams _initialParams;
        public ScanBoxParams InitialParams
        {
            get
            {
                if (_initialParams == null)
                {
                    _initialParams = ScanBoxAware?.GetScanBoxParams();
                }
                return _initialParams;
            }
            protected set { _initialParams = value; }
        }

        #endregion

        protected void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded += OnUnloaded;

            FrameworkElement parent = (FrameworkElement)(this.VisualParent);
            Size size = new Size(parent.ActualWidth, parent.ActualHeight);
            if (ScanBoxAware != null)
                ScanBoxAware.ContainerSize = size;

            parent.SizeChanged += OnContainerSizeChanged;
        }

        protected void OnUnloaded(object sender, RoutedEventArgs e)
        {
            UnsubsribeFromEvents();
        }

        protected void OnContainerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ScanBoxAware != null)
                ScanBoxAware.ContainerSize = e.NewSize;
        }

        #region Building methods

        protected void Build()
        {
            if (ScanBoxAware == null || InitialParams == null)
                return;

            _translateTransform.X = 0;
            _translateTransform.Y = 0;
            if(!InitialParams.KeepRotation)
            {
                _rotateTransform.Angle = 0;
            }

            if (!_isInitialized)
                InitializeElements();

            SubscribeToMovementMouseEvents();
            SubscribeToRotationMouseEvents();

            BuildSpecific(InitialParams);
            SliceNumber = InitialParams.SliceNumber;

            SetViewPositionByCenterCoord(InitialParams.Center);

            _isInitialized = true;
        }

        protected virtual void InitializeElements()
        {
            _mainSquare = FindMain("MainSquare");
            _secondSquare = FindSecondary("SecSquare");
            CreateCenter();
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

        protected void CreateCenter()
        {
            Ellipse center = this.FindName("CentralPoint") as Ellipse;
            if (center == null)
            {
                center = new Ellipse();
                Style centralStyle = (Style)this.ParentOfType<ScanBox>().FindResource("CentralPoint");
                center.Style = centralStyle;
                this.Children.Add(center);
            }
            Canvas.SetLeft(center, (this.Width / 2 - center.Width / 2));
            Canvas.SetTop(center, (this.Height / 2 - center.Height / 2));
        }

        protected void SetViewPositionByCenterCoord(Point center)
        {
            double left = center.X - this.Width / 2;
            Canvas.SetLeft(this, left);
            double top = center.Y - this.Height / 2;
            Canvas.SetTop(this, top);
        } 
        #endregion

        #region Movement events

        protected virtual void UnsubscribeFromMovementMouseEvents()
        {
            if (!_isInitialized || _secondSquare == null)
                return;

            _secondSquare.MouseEnter -= OnDraggableElementMouseEnter;
            _secondSquare.MouseLeave -= OnDraggableElementMouseLeave;
            _secondSquare.MouseLeftButtonDown -= OnSecondSquareMouseLeftButtonDown;
            _secondSquare.MouseLeftButtonUp -= OnSecondSquareMouseLeftButtonUp;
        }

        protected virtual void SubscribeToMovementMouseEvents()
        {
            UnsubscribeFromMovementMouseEvents();

            if (IsMovable && _secondSquare != null)
            {
                _secondSquare.MouseEnter += OnDraggableElementMouseEnter;
                _secondSquare.MouseLeave += OnDraggableElementMouseLeave;
                _secondSquare.MouseLeftButtonDown += OnSecondSquareMouseLeftButtonDown;
                _secondSquare.MouseLeftButtonUp += OnSecondSquareMouseLeftButtonUp;
            }
        }


        protected void OnDraggableElementMouseEnter(object sender, MouseEventArgs e)
        {
            //Not disabling Fge interaction while other action is ongoing
            if (e.LeftButton == MouseButtonState.Pressed
                || e.RightButton == MouseButtonState.Pressed
                || e.MiddleButton == MouseButtonState.Pressed)
                return;

            //Disable the Fge reaction to events
            SetFgeMouseEventsDisabled(true);
        }

        protected void OnDraggableElementMouseLeave(object sender, MouseEventArgs e)
        {
            //Enable back the Fge reaction to events if is not inside dragging action
            if (!_fgeDisabledByDrag)
                SetFgeMouseEventsDisabled(false);
        }

        protected void OnSecondSquareMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).CaptureMouse();

            _startingCurMovePoint = Mouse.GetPosition((FrameworkElement)this.VisualParent);
            _startingCenterPoint = new Point(InitialParams.Center.X, InitialParams.Center.Y);
            //Debug.WriteLine($"LeftButtonDown cursor X: {_startingCurMovePoint.X}, Y: {_startingCurMovePoint.Y}");

            _secondSquare.MouseMove += OnSecondSquareMouseMove;

            //Disable the Fge reaction to events and notify that disabling made on starting dragging
            _fgeDisabledByDrag = true;
            SetFgeMouseEventsDisabled(true);
        }

        protected void OnSecondSquareMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            var curPos = Mouse.GetPosition((FrameworkElement)this.VisualParent);
            double xOffset = curPos.X - _startingCurMovePoint.X;
            double yOffset = curPos.Y - _startingCurMovePoint.Y;
            //Debug.WriteLine($"MouseMove. X: {curPos.X}, Y: {curPos.Y}, offsetX: { xOffset }, offsetY: {yOffset}");

            double x = _startingCenterPoint.X + xOffset;
            double y = _startingCenterPoint.Y + yOffset;
            Point newCenter = new Point(x, y);

            SetViewPositionByCenterCoord(newCenter);

            //Debug.WriteLine($" {this.GetType().Name} OnSecondSquareMouseMove, left: {x}, right: {y} ");
            ScanBoxAware?.UpdateCenterMoving(newCenter, _startingCenterPoint);
        }

        protected void OnSecondSquareMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Not react if mouse down was not initialized by control itself
            if (!_fgeDisabledByDrag)
                return;

            _secondSquare.MouseMove -= OnSecondSquareMouseMove;

            var curPos = Mouse.GetPosition((FrameworkElement)this.VisualParent);
            double xOffset = curPos.X - _startingCurMovePoint.X;
            double yOffset = curPos.Y - _startingCurMovePoint.Y;

            double x = _startingCenterPoint.X + xOffset;
            double y = _startingCenterPoint.Y + yOffset;
            Point newCenter = new Point(x, y);
            SetViewPositionByCenterCoord(newCenter);

            ((FrameworkElement)sender).ReleaseMouseCapture();

            InitialParams.Center = newCenter;
            ScanBoxAware.UpdateCenterMoved(newCenter, _startingCenterPoint);

            SetFgeMouseEventsDisabled(false);
        }

        #endregion

        #region ScanBoxAware(Geometry) events

        protected virtual void UnsubscribeFromScanboxAwareEvents(IScanBoxAware scanBoxAware = null)
        {
            if (scanBoxAware == null && ScanBoxAware == null)
                return;

            IScanBoxAware sba = scanBoxAware != null ? scanBoxAware : ScanBoxAware;

            sba.CenterChanged -= OnCenterChanged;
            sba.GeometryChanged -= OnGeometryChanged;
            sba.SliceNumberChanged -= OnSliceNumberChanged;
            sba.Disposing -= OnDisposing;
        }

        protected virtual void SubscribeToScanboxAwareEvents(IScanBoxAware scanBoxAware = null)
        {
            if (scanBoxAware == null && ScanBoxAware == null)
                return;

            IScanBoxAware sba = scanBoxAware != null ? scanBoxAware : ScanBoxAware;

            UnsubscribeFromScanboxAwareEvents();

            sba.CenterChanged += OnCenterChanged;
            sba.GeometryChanged += OnGeometryChanged;
            sba.SliceNumberChanged += OnSliceNumberChanged;
            sba.Disposing += OnDisposing;
        }

        private void OnSliceNumberChanged(object sender, int e)
        {
            SliceNumber = e;
        }

        protected virtual void OnGeometryChanged(object sender, EventArgs e)
        {
            if (ScanBoxAware == null)
            {
                InitialParams = null;
                return;
            }

            InitialParams = ScanBoxAware.GetScanBoxParams();
            Build();
        }

        protected virtual void OnCenterChanged(object sender, Point center)
        {
            if (sender == this || !_isInitialized)
                return;

            //Debug.WriteLine($" {this.GetType().Name} OnCenterChanged, left: {center.X}, top: {center.Y} ");
            if (InitialParams != null)
                InitialParams.Center = center;

            SetViewPositionByCenterCoord(center);
        } 

        #endregion

        protected virtual void OnDisposing(object sender, EventArgs e)
        {
            UnsubsribeFromEvents();
        }

        protected virtual void UnsubscribeFromRotationMouseEvents()
        { }

        protected virtual void SubscribeToRotationMouseEvents()
        {
            UnsubscribeFromRotationMouseEvents();
        }

        protected virtual void UnsubsribeFromEvents()
        {
            ((FrameworkElement)(this.VisualParent)).SizeChanged -= OnContainerSizeChanged;

            UnsubscribeFromScanboxAwareEvents();
            UnsubscribeFromRotationMouseEvents();
            UnsubscribeFromMovementMouseEvents();
        }

        protected virtual void SetFgeMouseEventsDisabled(bool isDisabled)
        {
            IsFgeMouseEventsDisabled = isDisabled;
        }
    }
}
