using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ws.Extensions.UI.Wpf.Utils;
using Ws.Fus.ImageViewer.UI.Wpf.Entities;
using Ws.Fus.Fge.Interfaces;
using Ws.Fus.Fge.Interfaces.Entities;
using System.Diagnostics;

namespace Ws.Fus.Fge.Interfaces.Entities
{
    public enum GraphicEngineMouseEvent
    {
        MouseMove,
        MouseDown,
        MouseUp,
        MouseEnter,
        MouseLeave,
        MouseDoubleClick,
        MouseWheel,

    }
    public enum GraphicEngineMouseButton
    {
        None,
        LeftButton,
        RightButton,
        MiddleButton

    }
}

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls
{
    public class GraphicEngineInteraction : Decorator
    {
        //Synchronizing support between Fge windows
        private static int _startActionWindowId;
        private static MouseButtonEventArgs _mousePressedEvent;

        private static DependencyProperty GraphicEngineEventCommandProperty =
            DependencyProperty.Register("GraphicEngineEventCommand", typeof(ICommand), typeof(GraphicEngineInteraction),
            new PropertyMetadata((s, a) => { ((GraphicEngineInteraction)s)._graphicEngineEventCommand = (ICommand)a.NewValue; }));

        public ICommand GraphicEngineEventCommand
        {
            get { return (ICommand)GetValue(GraphicEngineEventCommandProperty); }
            set { SetValue(GraphicEngineEventCommandProperty, value); }
        }


        private static DependencyProperty SelectCommandProperty =
            DependencyProperty.Register("SelectCommand", typeof(ICommand), typeof(GraphicEngineInteraction),
            new PropertyMetadata(null));

        public ICommand SelectCommand
        {
            get { return (ICommand)GetValue(SelectCommandProperty); }
            set { SetValue(SelectCommandProperty, value); }
        }

        /// <summary>
        /// IsMouseEventsDisabled notifies that somewhere else mouse is used in UI operations 
        /// and its action should not be delivered to FUS
        /// </summary>
        public static readonly DependencyProperty IsMouseEventsDisabledProperty =
            DependencyProperty.Register("IsMouseEventsDisabled", typeof(bool), typeof(GraphicEngineInteraction),
            new FrameworkPropertyMetadata(false));

        public bool IsMouseEventsDisabled
        {
            get { return (bool)GetValue(IsMouseEventsDisabledProperty); }
            set { SetValue(IsMouseEventsDisabledProperty, value); }
        }

        /// <summary>
        /// WindowId - identifier of window
        /// </summary>
        public static readonly DependencyProperty WindowIdProperty =
            DependencyProperty.Register("WindowId", typeof(int), typeof(GraphicEngineInteraction));

        public int WindowId
        {
            get { return (int)GetValue(WindowIdProperty); }
            set { SetValue(WindowIdProperty, value); }
        }


        private ICommand _graphicEngineEventCommand;

        public GraphicEngineInteraction()
        {
        }


        protected override void OnMouseEnter(MouseEventArgs e)
        {
            //Support the scenario when mouse released outside of any Fge window
            if(_mousePressedEvent != null && (e.LeftButton | e.MiddleButton | e.RightButton) == MouseButtonState.Released)
            {
                OnPreviewMouseUp(new MouseButtonEventArgs(_mousePressedEvent.MouseDevice, _mousePressedEvent.Timestamp, _mousePressedEvent.ChangedButton));
            }
            ExecuteGraphicEngineCommand(GraphicEngineMouseEvent.MouseEnter, MouseButton.XButton2, e);
        }

        protected override void OnMouseLeave(MouseEventArgs e) => ExecuteGraphicEngineCommand(GraphicEngineMouseEvent.MouseLeave, MouseButton.XButton2, e);
        //{
        //    SendEventToGraphicEngine(GraphicEngineEventType.MouseLeave, MouseButton.XButton2, e.GetPosition(this));
        //}

        protected override void OnMouseWheel(MouseWheelEventArgs e) => ExecuteGraphicEngineCommand(GraphicEngineMouseEvent.MouseWheel, MouseButton.XButton2, e);
        //{
        //    SendEventToGraphicEngine(GraphicEngineEventType.MouseWheel, MouseButton.XButton2, e.GetPosition(this), e.Delta);
        //}

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ExecuteGraphicEngineCommand(GraphicEngineMouseEvent.MouseDoubleClick, e.ChangedButton, e);
                //SendEventToGraphicEngine(GraphicEngineEventType.MouseDoubleClick, e.ChangedButton, e.GetPosition(this));
            }
            else
            {
                //SendEventToGraphicEngine(GraphicEngineEventType.MouseDown, e.ChangedButton, e.GetPosition(this));
                _startActionWindowId = WindowId;
                _mousePressedEvent = e;
                ExecuteGraphicEngineCommand(GraphicEngineMouseEvent.MouseDown, e.ChangedButton, e);
                SelectCommand?.Execute(null);
            }
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (_startActionWindowId != 0 && WindowId != _startActionWindowId)
            {
                //Debug.WriteLine($"OnPreviewMouseMove, current:{WindowId}, start: {_startActionWindowId}");
                return;
            }
            ExecuteGraphicEngineCommand(GraphicEngineMouseEvent.MouseMove, MouseButton.XButton2, e);
            //SendEventToGraphicEngine(GraphicEngineEventType.MouseMove, MouseButton.XButton2, e.GetPosition(this));
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            _startActionWindowId = 0;
            _mousePressedEvent = null;
            ExecuteGraphicEngineCommand(GraphicEngineMouseEvent.MouseUp, e.ChangedButton, e);
        }

        //private void SendEventToGraphicEngine(GraphicEngineEventType eventType, System.Windows.Input.MouseButton button, Point mousePosition, int amount = 0)
        //{
        //    _graphicEngineEventCommand?.Execute(new GraphicEngineEventInfo
        //    {
        //        //WindowId = Id,
        //        EventType = eventType,
        //        MouseButton = button,
        //        MousePosition = mousePosition,
        //        Amount = amount
        //    });
        //}

        private void ExecuteGraphicEngineCommand(GraphicEngineMouseEvent eventType, MouseButton button, MouseEventArgs e)
        {

            GraphicEngineMouseButton geMouseButton = button == MouseButton.Left ? GraphicEngineMouseButton.LeftButton :
                button == MouseButton.Middle ? GraphicEngineMouseButton.MiddleButton :
                button == MouseButton.Right ? GraphicEngineMouseButton.RightButton :
                GraphicEngineMouseButton.None;

            if (IsMouseEventsDisabled)
                geMouseButton = GraphicEngineMouseButton.None;

            int posX, posY;
            DpiUtils.ConvertMousePosition(this, e, out posX, out posY);

            _graphicEngineEventCommand?.Execute(new GraphicEngineEventInfo
            {
                EventType = eventType,
                MouseButton = geMouseButton,
                MousePosX = posX,
                MousePosY = posY,
                Amount = (e is MouseWheelEventArgs) ? (e as MouseWheelEventArgs).Delta : 0
            });
        }
    }
}
