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

namespace Ws.Fus.PlanningScans.UI.Wpf.Controls.ScanBox
{
    public class SecondaryOrientationPanel: BaseActionPanel
    {
        private const double _horizontalAngle = 90;

        private Line _axis;
        private FrameworkElement _rotator;
        private FrameworkElement _fromDragger;
        private FrameworkElement _toDragger;

        protected Point _startingCurRotatePoint;
        protected Point _startingCenterRotatePoint;
        protected Vector _startDirection;
        protected double _origRotAngle;

        protected Point _startingCurEdgeMovePoint;
        protected Point _startingMoveEdgeLeftTopPoint;
        bool? _isFromDragging;
        double _startingEdgePosition;

        public SecondaryOrientationPanel()
        {}

        protected override void InitializeElements()
        {
            base.InitializeElements();
            _axis = this.FindName("lineAxis") as Line;
            _rotator = this.FindName("Rotator") as FrameworkElement;

            FrameworkElement fd = this.FindName("FromDragger") as FrameworkElement;
            _fromDragger = fd.FindName("Dragger") as FrameworkElement;

            FrameworkElement td = this.FindName("ToDragger") as FrameworkElement;
            _toDragger = td.FindName("Dragger") as FrameworkElement;
        }

        public override void BuildSpecific(ScanBoxParams initialParams)
        {
            if (ScanBoxAware == null || InitialParams == null)
                return;

            _mainSquare.Width = InitialParams.From + InitialParams.To;
            _mainSquare.Height = (ScanBoxAware != null && ScanBoxAware.IsAxisHorizontal) ? InitialParams.FOV_X : InitialParams.FOV_Y;
            Canvas.SetLeft(_mainSquare, (this.Width/2 - InitialParams.To));
            Canvas.SetTop(_mainSquare, (this.Height/2 - _mainSquare.Height/2));

            _axis.Y2 = _mainSquare.Height + 40;
            Canvas.SetLeft(_axis, (this.Width/2));
            Canvas.SetTop(_axis, (this.Height/2 - _mainSquare.Height/2 - 20));

            if (IsRotatable)
            {
                Canvas.SetLeft(_rotator, (this.Width/2 - _rotator.ActualWidth/2));
                Canvas.SetTop(_rotator, (Canvas.GetTop(_axis) - 15));
                _rotator.Visibility = Visibility.Visible;
            }
            else
            {
                _rotator.Visibility = Visibility.Hidden;
                Canvas.SetLeft(_rotator, (-1000));
                Canvas.SetTop(_rotator, (-1000));
            }

            if (!initialParams.KeepRotation && ScanBoxAware != null && ScanBoxAware.IsAxisHorizontal)
            {
                _rotateTransform.Angle = _horizontalAngle;
            }
        }

        private void OnRotatorMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).CaptureMouse();
            _origRotAngle = _rotateTransform.Angle;
            _startingCurMovePoint = Mouse.GetPosition((FrameworkElement)this.VisualParent);
            _startingCenterRotatePoint = new Point((InitialParams.Center.X), InitialParams.Center.Y);

            //Debug.WriteLine($"Rotate LeftButtonDown. X: {_startingCurMovePoint.X}, Y: {_startingCurMovePoint.Y}");

            Vector cnt = new Vector(_startingCenterRotatePoint.X, _startingCenterRotatePoint.Y);
            Vector cur = new Vector(_startingCurMovePoint.X, _startingCurMovePoint.Y);
            var angle = Vector.AngleBetween(cnt, cur);
            _startDirection = Vector.Subtract(cur, cnt);

            //Debug.WriteLine($"Rotate V-Center: {cnt.ToString()}, V-cur: {cur.ToString()}, Angle: {angle}, V-Subtract: {_startDirection.ToString()}");

            _rotator.MouseMove += OnRotatorMouseMove;
        }

        protected void OnRotatorMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var curPos = Mouse.GetPosition((FrameworkElement)this.VisualParent);
                Vector cur = new Vector(curPos.X, curPos.Y);
                Vector cnt = new Vector(_startingCenterRotatePoint.X, _startingCenterRotatePoint.Y);
                
                // Calculate an angle
                var curSbst = Vector.Subtract(cur, cnt);
                var angle = Vector.AngleBetween(_startDirection, curSbst);

                bool isHorizontal = ScanBoxAware.IsAxisHorizontal;
                var fullAngle = _origRotAngle + angle;
                if (isHorizontal)
                    fullAngle += (-_horizontalAngle);

                fullAngle = Math.Min(InitialParams.MaxAnglePerRotation, Math.Max(InitialParams.MinAnglePerRotation, fullAngle));

                if (isHorizontal)
                    fullAngle += _horizontalAngle;

                _rotateTransform.Angle = fullAngle;
                this.Cursor = _rotator.Cursor;
            }
        }

        private void OnRotatorMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).ReleaseMouseCapture();
            _rotator.MouseMove -= OnRotatorMouseMove;
            //Debug.WriteLine($"Rotate Angle: {_rotateTransform.Angle}");

            double angle = _rotateTransform.Angle;
            //Finding delta angle from previously fired angle
            angle = angle - _origRotAngle;

            ScanBoxAware.UpdateRotation(angle);
        }

        public void OnDragEdgeMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            _startingCurEdgeMovePoint = Mouse.GetPosition(this);

            bool isFrom = false;
            if (fe.Tag != null && bool.TryParse(fe.Tag.ToString(), out isFrom))
                _isFromDragging = isFrom;

            _startingEdgePosition = _isFromDragging.Value ? InitialParams.From : InitialParams.To;

            fe.CaptureMouse();
            fe.MouseMove += OnDragEdgeMouseMove;
        }

        protected void OnDragEdgeMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            if (_isFromDragging.HasValue)
            {
                var curPos = Mouse.GetPosition(this);
                double xOffset = curPos.X - _startingCurEdgeMovePoint.X;
                if (_isFromDragging.Value)
                {
                    double from = _startingEdgePosition + xOffset;
                    //Debug.WriteLine($"Drag ToEdge. xOffset: {xOffset}, To old {InitialParams.To},  to {to} ");
                    double newFrom = Math.Max(Math.Min(InitialParams.MaxFrom, from), InitialParams.MinFrom);
                    InitialParams.From = newFrom;
                }
                else
                {
                    double to = _startingEdgePosition - xOffset;
                    //Debug.WriteLine($"Drag FromEdge. xOffset: {xOffset}, From old {InitialParams.From},  from {from} ");
                    double newTo = Math.Max(Math.Min(InitialParams.MaxTo, to), InitialParams.MinTo);
                    InitialParams.To = newTo;
                }
                BuildSpecific(InitialParams);

                if (_isFromDragging.Value)
                    ScanBoxAware.UpdateFromMoving(InitialParams.From);
                else
                    ScanBoxAware.UpdateToMoving(InitialParams.To);
            }
            else
                Debug.WriteLine("OnDragEdgeMouseMove _isToDragging.HasValue is null");
        }

        public void OnDragEdgeMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            fe.MouseMove -= OnDragEdgeMouseMove;
            fe.ReleaseMouseCapture();

            if (_isFromDragging.HasValue)
            {
                if (_isFromDragging.Value)
                    ScanBoxAware.UpdateFrom(InitialParams.From);
                else
                    ScanBoxAware.UpdateTo(InitialParams.To);
            }
            else
                Debug.WriteLine("OnDragEdgeMouseUp _isToDragging.HasValue is null");

            _isFromDragging = null;
        }

        protected override void UnsubscribeFromMovementMouseEvents()
        {
            base.UnsubscribeFromMovementMouseEvents();

            if (!_isInitialized || _fromDragger == null || _toDragger == null)
                return;

            _fromDragger.MouseEnter -= OnDraggableElementMouseEnter;
            _fromDragger.MouseLeave -= OnDraggableElementMouseLeave;
            _toDragger.MouseEnter -= OnDraggableElementMouseEnter;
            _toDragger.MouseLeave -= OnDraggableElementMouseLeave;

            _fromDragger.MouseLeftButtonDown -= OnDragEdgeMouseLeftButtonDown;
            _fromDragger.MouseLeftButtonUp -= OnDragEdgeMouseLeftButtonUp;
            _toDragger.MouseLeftButtonDown -= OnDragEdgeMouseLeftButtonDown;
            _toDragger.MouseLeftButtonUp -= OnDragEdgeMouseLeftButtonUp;
        }

        protected override void SubscribeToMovementMouseEvents()
        {
            base.SubscribeToMovementMouseEvents();

            if(IsMovable)
            {
                _fromDragger.MouseEnter += OnDraggableElementMouseEnter;
                _fromDragger.MouseLeave += OnDraggableElementMouseLeave;
                _toDragger.MouseEnter += OnDraggableElementMouseEnter;
                _toDragger.MouseLeave += OnDraggableElementMouseLeave;

                _fromDragger.MouseLeftButtonDown += OnDragEdgeMouseLeftButtonDown;
                _fromDragger.MouseLeftButtonUp += OnDragEdgeMouseLeftButtonUp;
                _toDragger.MouseLeftButtonDown += OnDragEdgeMouseLeftButtonDown;
                _toDragger.MouseLeftButtonUp += OnDragEdgeMouseLeftButtonUp;
            }
        }

        protected override void UnsubscribeFromRotationMouseEvents()
        {
            base.UnsubscribeFromRotationMouseEvents();

            if (!_isInitialized || _rotator == null)
                return;

            _rotator.MouseEnter -= OnDraggableElementMouseEnter;
            _rotator.MouseLeave -= OnDraggableElementMouseLeave;

            _rotator.MouseLeftButtonDown -= OnRotatorMouseLeftButtonDown;
            _rotator.MouseLeftButtonUp -= OnRotatorMouseLeftButtonUp;
        }

        protected override void SubscribeToRotationMouseEvents()
        {
            base.SubscribeToRotationMouseEvents();

            if(IsRotatable && _rotator != null)
            {
                _rotator.MouseEnter += OnDraggableElementMouseEnter;
                _rotator.MouseLeave += OnDraggableElementMouseLeave;

                _rotator.MouseLeftButtonDown += OnRotatorMouseLeftButtonDown;
                _rotator.MouseLeftButtonUp += OnRotatorMouseLeftButtonUp;
            }
        }

        protected override void UnsubscribeFromScanboxAwareEvents(IScanBoxAware scanBoxAware = null)
        {
            base.UnsubscribeFromScanboxAwareEvents();

            if (scanBoxAware == null && ScanBoxAware == null)
                return;

            IScanBoxAware sba = scanBoxAware != null ? scanBoxAware : ScanBoxAware;

            sba.FromChanged -= OnFromChanged;
            sba.ToChanged -= OnToChanged;
            sba.KeepRotationChanged -= OnKeepRotationChanged;
        }

        protected override void SubscribeToScanboxAwareEvents(IScanBoxAware scanBoxAware = null)
        {
            base.SubscribeToScanboxAwareEvents();

            if (scanBoxAware == null && ScanBoxAware == null)
                return;

            IScanBoxAware sba = scanBoxAware != null ? scanBoxAware : ScanBoxAware;

            sba.FromChanged += OnFromChanged;
            sba.ToChanged += OnToChanged;
            sba.KeepRotationChanged += OnKeepRotationChanged;
        }

        private void OnKeepRotationChanged(object sender, bool keepRotation)
        {
            if (ScanBoxAware == null || InitialParams == null)
                return;

            InitialParams.KeepRotation = keepRotation;
        }

        protected void OnToChanged(object sender, double e)
        {
            if (ScanBoxAware == null || InitialParams == null)
                return;

            InitialParams.To = e;
            BuildSpecific(InitialParams);
        }

        protected void OnFromChanged(object sender, double e)
        {
            if (ScanBoxAware == null || InitialParams == null)
                return;

            InitialParams.From = e;
            BuildSpecific(InitialParams);
        }
    }
}
