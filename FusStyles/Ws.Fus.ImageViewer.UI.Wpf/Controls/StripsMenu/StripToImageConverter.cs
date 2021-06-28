using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ws.Fus.ImageViewer.UI.Wpf.ViewModels;
using Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.StripsMenu
{
    class StripToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var stripVm = value as StripVm;
                var strip = stripVm.Strip;
                return new WriteableBitmap(strip.Image);
            }
            catch (Exception ex)
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static WriteableBitmap CreateBitmap(byte[] buffer)
        {
            var pf = PixelFormats.Gray8;
            var stride = (512 * pf.BitsPerPixel + 7) / 8;

            var bitmap = BitmapSource.Create(512, 512, 96, 96, pf, null, buffer, stride);

            return new WriteableBitmap(bitmap);
        }
    }
}
