using Ws.Fus.Strips.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Ws.Fus.ImageViewer.UI.Wpf.Entities;
using Ws.Fus.ImageViewer.UI.Wpf.ViewModels;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.StripsMenu
{
    public class StripToCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var stripVm = value as IStripVm<IStrip>;
                var strip = stripVm.Strip;
                if (strip is Strip3dStub)
                    return "3D";

                switch (strip.Id.StripType)
                {
                    case StripType.eStripPrimary:
                    case StripType.eStripSecondary:
                    case StripType.eStripThird:
                        return "LIVE MR";
                    case StripType.eStripPreOpPrimaryMR:
                    case StripType.eStripPreOpSecondaryMR:
                    case StripType.eStripPreOpThirdMR:
                        return "PRE-OP MR";
                    case StripType.eStripPreOpCT:
                        return "PRE-OP CT";

                    default:
                        return "OTHER";
                }
            }
            catch (Exception ex)
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
