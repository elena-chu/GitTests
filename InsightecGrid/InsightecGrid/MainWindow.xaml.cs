using System.Windows;
using ResizableGrid.ViewModels;

namespace InsightecGrid
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new DebugViewModel();
            InitializeComponent();
        }
    }
}
