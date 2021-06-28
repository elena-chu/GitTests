using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.StripsMenu
{
    public class StripOrientationToTextConverter : IValueConverter
    {
        public enum StripOrientation
        {
            eTOR_MRI_NO_ORIENTATION,
            eTOR_MRI_CORONAL_SLICE,
            eTOR_MRI_AXIAL_SLICE,
            eTOR_MRI_SAGITTAL_SLICE,
            eTOR_MRI_OBLIQUE_CORONAL_SLICE,
            eTOR_MRI_OBLIQUE_AXIAL_SLICE,
            eTOR_MRI_OBLIQUE_SAGITTAL_SLICE

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var orientation = (StripOrientation)value;

                switch (orientation)
                {
                    case StripOrientation.eTOR_MRI_NO_ORIENTATION:
                        return "N/A";
                    case StripOrientation.eTOR_MRI_CORONAL_SLICE:
                    case StripOrientation.eTOR_MRI_OBLIQUE_CORONAL_SLICE:
                        return "COR";
                    case StripOrientation.eTOR_MRI_AXIAL_SLICE:
                    case StripOrientation.eTOR_MRI_OBLIQUE_AXIAL_SLICE:
                        return "AX";
                    case StripOrientation.eTOR_MRI_SAGITTAL_SLICE:
                    case StripOrientation.eTOR_MRI_OBLIQUE_SAGITTAL_SLICE:
                        return "SAG";
                    default:
                        return "N/A";
                }

            }
            catch (Exception)
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
