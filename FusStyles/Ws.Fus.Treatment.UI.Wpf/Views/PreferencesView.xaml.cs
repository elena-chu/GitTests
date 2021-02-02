using Serilog;
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

namespace Ws.Fus.Treatment.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for PreferencesView.xaml
    /// </summary>
    public partial class PreferencesView : UserControl
    {
        private static readonly ILogger _logger = Log.ForContext<PreferencesView>();

        public PreferencesView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to initialize view");
                throw;
            }
        }
    }
}
