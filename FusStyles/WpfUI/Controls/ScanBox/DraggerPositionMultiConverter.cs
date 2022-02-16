using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ws.Fus.PlanningScans.UI.Wpf.Controls.ScanBox
{
    public class DraggerPositionMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double ret = 0;
            double left = (values[0] != null && values[0] != DependencyProperty.UnsetValue) ? System.Convert.ToDouble(values[0]) : 0;
            double width = (values[1] != null && values[1] != DependencyProperty.UnsetValue) ? System.Convert.ToDouble(values[1]) : 0;
            DragEdge dragEdge = (values[2] != null && values[2] is DragEdge) ? values[2] as DragEdge : null;

            if (dragEdge != null)
            {
                if (!dragEdge.IsFrom)//From
                    ret = left - dragEdge.Width + 2;
                else //To
                    ret = left + width + dragEdge.Width - 2;
                //Debug.WriteLine($"DraggerPositionMultiConverter. Dragger IsTo: {dragEdge.IsTo}, MainSquareleft={left}, MainSquareWidht={width},  dragEdge.Width ={dragEdge.Width},  ret Canvas.Left = {ret} ");
            }

            return ret;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
