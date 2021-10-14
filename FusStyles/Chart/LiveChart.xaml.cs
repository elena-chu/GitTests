using InsightecFiddle.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InsightecFiddle.UserControls
{
    public partial class LiveChart : UserControl
    {
        public LiveChart()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ChartViewModel).StartTimer();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ChartViewModel).StopTimer();
        }
        private Point _startLimitDrag;
        private void LimitTrackBall_PreviewMouseDown(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _startLimitDrag = e.GetPosition(null);
        }

        private void LimitTrackBall_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point currentMousePosition = e.GetPosition(null);
            Vector diff = _startLimitDrag - currentMousePosition;

            if (e.LeftButton == MouseButtonState.Pressed && Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                var obj = RadCartesianChart.ConvertPointToData(currentMousePosition).SecondValue;
                int newLimit = (int)Math.Round((double)obj);
                (DataContext as ChartViewModel).OnLimitChanged(newLimit);
            }
        }
    }
}
