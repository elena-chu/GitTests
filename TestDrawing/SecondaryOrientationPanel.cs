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
    public class SecondaryOrientationPanel : BaseActionPanel
    {
        private Line _axis;
        private FrameworkElement _rotator;
        private FrameworkElement _fromDragger;
        private FrameworkElement _toDragger;

        protected Point _startingCurRotatePoint;
        protected Point _startingCenterRotatePoint;
        protected Vector _startDirection;
        protected double _origRotAngle;

        TranslateTransform _startTransform;


        protected Point _startingCurEdgeMovePoint;
        protected Point _startingMoveEdgeLeftTopPoint;
        bool? _isToDragging;
        double _startingEngePosition;


        public SecondaryOrientationPanel()
        {
            IsMovable = true;
            IsRotatable = true;
        }

        public override void BuildSpecific(BuildParameters initialParams)
        {
            if (!_isInitialized)
            {
                _mainSquare = FindMain("MainSquareS");
                _secondSquare = FindSecondary("SecSquareS");
                _axis = this.FindName("lineAxis") as Line;
                _rotator = this.FindName("Rotator") as FrameworkElement;
                FrameworkElement fel = this.FindName("FromDragger") as FrameworkElement;
                _fromDragger = fel.FindName("Dragger") as FrameworkElement;
                FrameworkElement tel = this.FindName("ToDragger") as FrameworkElement;
                _toDragger = tel.FindName("Dragger") as FrameworkElement;

                if (IsMovable)
                {
                    _secondSquare.MouseLeftButtonDown += OnSecondSquareMouseLeftButtonDown;
                    _secondSquare.MouseLeftButtonUp += OnSecondSquareMouseLeftButtonUp;
                    _fromDragger.MouseLeftButtonDown += OnDragEdgeMouseLeftButtonDown;
                    _fromDragger.MouseLeftButtonUp += OnDragEdgeMouseLeftButtonUp;
                    _toDragger.MouseLeftButtonDown += OnDragEdgeMouseLeftButtonDown;
                    _toDragger.MouseLeftButtonUp += OnDragEdgeMouseLeftButtonUp;
                }
                if (IsRotatable)
                {
                    _rotator.MouseLeftButtonDown += OnRotatorMouseLeftButtonDown;
                    _rotator.MouseLeftButtonUp += OnRotatorMouseLeftButtonUp;
                }

            }

            _mainSquare.Width = InitialParams.From * ScaleFactor + InitialParams.To * ScaleFactor;
            _mainSquare.Height = InitialParams.FOV * ScaleFactor;
            Canvas.SetLeft(_mainSquare, (this.Width / 2 - InitialParams.From * ScaleFactor));
            Canvas.SetTop(_mainSquare, (this.Height / 2 - _mainSquare.Height / 2));

            _axis.Y2 = _mainSquare.Height + 40;
            Canvas.SetLeft(_axis, (this.Width / 2));
            Canvas.SetTop(_axis, (this.Height / 2 - _mainSquare.Height / 2 - 20));

            //Canvas.SetLeft(_fromDragger, Canvas.GetLeft(_mainSquare) - _fromDragger.Width +2);
            //Canvas.SetLeft(_toDragger, Canvas.GetLeft(_mainSquare) + _mainSquare.Width + _fromDragger.Width -2);

            if (IsRotatable)
            {
                Canvas.SetLeft(_rotator, (this.Width / 2 - _rotator.ActualWidth/2));
                Canvas.SetTop(_rotator, (Canvas.GetTop(_axis) - _rotator.ActualHeight /2));
            }

            SetVectorPoint();
        }

        private void OnRotatorMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).CaptureMouse();
            _origRotAngle = _rotateTransform.Angle;
            _startingCurMovePoint = Mouse.GetPosition((FrameworkElement)this.VisualParent);
            //_startingCenterRotatePoint = new Point((InitialParams.CenterCoord.X), InitialParams.CenterCoord.Y);
            _startingCenterRotatePoint = new Point((InitialParams.CenterCoord.X + _translateTransform.X), InitialParams.CenterCoord.Y + _translateTransform.Y);

            Debug.WriteLine($"Rotate LeftButtonDown. X: {_startingCurMovePoint.X}, Y: {_startingCurMovePoint.Y}");

            Vector cnt = new Vector(_startingCenterRotatePoint.X, _startingCenterRotatePoint.Y);
            Vector cur = new Vector(_startingCurMovePoint.X, _startingCurMovePoint.Y);
            var angle = Vector.AngleBetween(cnt, cur);
            _startDirection = Vector.Subtract(cur, cnt);

            _startTransform =_translateTransform.Clone();

            Debug.WriteLine($"Rotate V-Center: {cnt.ToString()}, V-cur: {cur.ToString()}, Angle: {angle}, V-Subtract: {_startDirection.ToString()}");

            _rotator.MouseMove += OnRotatorMouseMove;
        }

        private void OnRotatorMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).ReleaseMouseCapture();
            _rotator.MouseMove -= OnRotatorMouseMove;
            Debug.WriteLine($"Rotate Angle: {_rotateTransform.Angle}, Move: {_translateTransform.X}, {_translateTransform.Y}; {_translateTransform.Value}");
            Debug.WriteLine($"Rotate Matrix: {this.RenderTransform.Value}");
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
                var angle = Vector.AngleBetween( _startDirection, curSbst);

                //double radians = Math.Atan((curPos.Y - _startingCurRotatePoint.Y - (InitialParams.CenterCoord.Y + _translateTransform.Y)) /
                //                       (curPos.X - _startingCurRotatePoint.X - (InitialParams.CenterCoord.X + _translateTransform.X)));
                //var angle = radians * 180 / Math.PI;

                //Debug.WriteLine($"Rotate V-_startDirection: {_startDirection.ToString()}, V-cur: {cur.ToString()}, Angle: {angle}, V-Subtract: {curSbst.ToString()}");

                _rotateTransform.Angle = _origRotAngle + angle;
            }
        }

        public void OnDragEdgeMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            ////this.CaptureMouse();

            //DragEdge dragEdge = sender as DragEdge;
            _startingCurEdgeMovePoint = Mouse.GetPosition(this);

            //_isToDragging = dragEdge.IsTo;
            bool isTo = false;
            if(fe.Tag!= null && bool.TryParse(fe.Tag.ToString(), out isTo))
                _isToDragging = isTo;


            _startingEngePosition = _isToDragging.Value ? InitialParams.To * ScaleFactor : InitialParams.From * ScaleFactor;
            //_startingMoveEdgeLeftTopPoint = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));

            //this.MouseMove += OnDragEdgeMouseMove;
            //dragEdge.CaptureMouse();
            //dragEdge.MouseMove += OnDragEdgeMouseMove;

            fe.CaptureMouse();
            fe.MouseMove += OnDragEdgeMouseMove;
        }

        protected void OnDragEdgeMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _isToDragging.HasValue)
            {
                var curPos = Mouse.GetPosition(this);
                double xOffset = curPos.X - _startingCurEdgeMovePoint.X;
                if (!_isToDragging.Value)
                {
                    double from = _startingEngePosition - xOffset;
                    //Debug.WriteLine($"Drag FromEdge. xOffset: {xOffset}, From old {InitialParams.From},  from {from} ");
                    double newFrom = Math.Max(Math.Min(InitialParams.MaxFrom * ScaleFactor, from), InitialParams.MinFrom * ScaleFactor);
                    InitialParams.From = newFrom / ScaleFactor;
                }
                else
                {
                    double to = _startingEngePosition + xOffset;
                    //Debug.WriteLine($"Drag ToEdge. xOffset: {xOffset}, To old {InitialParams.To},  to {to} ");
                    double newTo = Math.Max(Math.Min(InitialParams.MaxTo * ScaleFactor, to), InitialParams.MinTo * ScaleFactor);
                    InitialParams.To = newTo/ScaleFactor;
                }
                BuildSpecific(InitialParams);
            }
        }

        public void OnDragEdgeMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            fe.MouseMove -= OnDragEdgeMouseMove;
            this.MouseMove -= OnDragEdgeMouseMove;
            _isToDragging = null;
            fe.ReleaseMouseCapture();
        }
    }
}
