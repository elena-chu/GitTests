using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ws.Fus.ImageViewer.UI.Wpf.ViewModels;
using Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.StripsMenu
{
    /// <summary>
    /// The deault data template selector for the strips menu.
    /// </summary>
    public class StripsMenuDefaultDtSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is StripVm)
            {
                return SelectTemplateImp(item as StripVm, element);
            }

            return null;
        }

        protected virtual DataTemplate SelectTemplateImp(StripVm stripVm, FrameworkElement element)
        {
            IStrip strip = stripVm.Strip;
            // The default strip templates must be defined in this project resource dictionary under the requested keys
            //if (strip is Strip3dStub)
            //    return element.FindResource(StripsMenuConstants.DtDefault3d) as DataTemplate;


            var striplet = strip as Striplet;
            if (striplet != null)
            {
                //if (striplet.Strips.All(s => s.HasMission<IStripMissionLocalizer>()))
                    return element.FindResource(StripsMenuConstants.DtLocalizerStrip) as DataTemplate;
            }
            
            return element.FindResource(StripsMenuConstants.DtDefaultStrip) as DataTemplate;
        }
    }
}
