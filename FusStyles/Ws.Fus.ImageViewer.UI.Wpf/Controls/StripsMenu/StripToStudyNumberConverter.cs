using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Ws.Fus.ImageViewer.UI.Wpf.ViewModels;
using Ws.Fus.Strips.Interfaces.Entities;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.StripsMenu
{
    public class StripToStudyNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var stripVm = value as IStripVm<IStrip>;
                var strip = stripVm.Strip;
                var studyId = strip.Series?.Study?.StudyId;
                return string.IsNullOrWhiteSpace(studyId) ? string.Empty : "EXAM " + studyId;
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
