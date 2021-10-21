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
            //CalculateVerticalAxisHeight();
            (DataContext as ChartViewModel).StartTimer();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ChartViewModel).StopTimer();
        }
    }
}
