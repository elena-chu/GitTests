//using Serilog;
using System;
using System.Windows;
using System.Windows.Controls;
using Ws.Extensions.UI.Wpf.Behaviors;
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


        #region Handlers

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem == null)
                return;

            MenuItem menuHeader = null;
            string param = (string)menuItem.GetValue(ControlExtensions.HandlerParameterProperty);
            switch (param)
            {
                case "View":
                    menuHeader = ViewMenuHeader;
                    break;
                case "Draw":
                    menuHeader = DrawMenuHeader;
                    break;
                case "Compare":
                    menuHeader = CompareMenuHeader;
                    break;
                default:
                    break;
            }

            menuHeader.SetValue(IconedButton.IconProperty, menuItem.GetValue(IconedButton.IconProperty));
            menuHeader.Command = menuItem.Command;
        }


        private void CompareMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi == null)
                return;

            MenuItem_Click(sender, e);

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

        #endregion

    }
}
