﻿//using Serilog;
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

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    /// <summary>
    /// Interaction logic for ToolbarMenu.xaml
    /// </summary>
    public partial class ToolbarMenuOrig : UserControl
    {
        //private static readonly ILogger _logger = Log.ForContext<ToolbarMenu>();

        public ToolbarMenuOrig()
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
    }
}
