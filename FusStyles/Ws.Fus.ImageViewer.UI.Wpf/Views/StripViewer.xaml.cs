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
//using Ws.Fus.ImageViewer.UI.Wpf.Entities;
//using Ws.Fus.ImageViewer.UI.Wpf.ViewModels;
//using Ws.Fus.Strips.Interfaces.Entities;

namespace Ws.Fus.ImageViewer.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for StripViewer2.xaml
    /// </summary>
    public partial class StripViewer : UserControl
    {
        //private static ILogger _logger = Log.ForContext<StripViewer>();

        public StripViewer()
        {
            InitializeComponent();
        }

        //private StripViewerViewModel ViewModel => DataContext as StripViewerViewModel;

        private void Strip_DragEnter(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DragNDropFormats.PrimaryStripDnd))
            //{
            //    var stripVm = e.Data.GetData(DragNDropFormats.PrimaryStripDnd) as IStripVm<IStrip>;
            //    if (stripVm != null && stripVm.IsEnabled)
            //    {
            //        var canSwitch = ViewModel.CanSwitchStrip(stripVm.Strip);
            //        if (canSwitch != CanSwitchStripResult.CantSwitch)
            //        {
            //            e.Effects = DragDropEffects.Copy;
            //            e.Handled = true;
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        if (stripVm == null)
            //            _logger.Warning("Got null stripVm on drag enter");
            //        else if (!stripVm.IsEnabled)
            //            _logger.Warning("Got disabled stripVm on drag enter: {name}", stripVm.Strip.StripName);
            //    }
            //}
            //else if (e.Data.GetDataPresent(DragNDropFormats.SecondaryStripDnd))
            //{
            //    var strip = e.Data.GetData(DragNDropFormats.SecondaryStripDnd) as IStripVm<IStrip>;
            //    if (ViewModel.CanSwitchSecondaryStrip(strip))
            //        return;
            //}

            //e.Handled = true;
            //e.Effects = DragDropEffects.None;
        }

        private void Strip_Drop(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DragNDropFormats.PrimaryStripDnd))
            //{
            //    var stripVm = e.Data.GetData(DragNDropFormats.PrimaryStripDnd) as IStripVm<IStrip>;

            //    if (stripVm == null)
            //        _logger.Warning("Got null stripVm on drag enter");
            //    else if (!stripVm.IsEnabled)
            //        _logger.Warning("Got disabled stripVm on drag enter: {name}", stripVm.Strip.StripName);
            //    else
            //    {
            //        var ok = ViewModel.TrySwitchStrip(stripVm.Strip);
            //        if (!ok)
            //            _logger.Warning("Strip was not switched: {name}", stripVm.Strip.StripName);
            //    }
            //}
            //else if (e.Data.GetDataPresent(DragNDropFormats.SecondaryStripDnd))
            //{
            //    var strip = e.Data.GetData(DragNDropFormats.SecondaryStripDnd) as IStripVm<IStrip>;
            //    ViewModel.SwitchSecondaryStrip(strip);
            //}
        }
    }
}
