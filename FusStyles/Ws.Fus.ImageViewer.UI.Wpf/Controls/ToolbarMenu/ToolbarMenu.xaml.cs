//using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ws.Fus.ImageViewer.Interfaces.Entities;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    /// <summary>
    /// Interaction logic for ToolbarMenu.xaml
    /// </summary>
    public partial class ToolbarMenu : UserControl
    {
        //private static readonly ILogger _logger = Log.ForContext<ToolbarMenu>();

        public ToolbarMenu()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                //_logger.Error(ex, "InitializeComponent");
                //throw;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi == null)
                return;

            CompareMode mode = CompareMode.Simple;
            if(mi.CommandParameter != null)
            {
                mode = (CompareMode)Enum.Parse(typeof(CompareMode), mi.CommandParameter.ToString());
            }

            DataTemplate contentRes;
            switch (mode)
            {
                case CompareMode.Flicker:
                    contentRes = (DataTemplate)CompareMenu.FindResource("FlickerModeControl");
                    break;
                case CompareMode.Slider:
                    contentRes = (DataTemplate)CompareMenu.FindResource("SliderModeControl");
                    break;
                case CompareMode.Fusion:
                    contentRes = (DataTemplate)CompareMenu.FindResource("FusionModeControl");
                    break;
                case CompareMode.Simple:
                default:
                    contentRes = (DataTemplate)CompareMenu.FindResource("SliderModeControl");
                    break;
            }

            CompareModeControl.ContentTemplate = contentRes;
        }
    }
}
