using InsightecFiddle.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
    }
}
