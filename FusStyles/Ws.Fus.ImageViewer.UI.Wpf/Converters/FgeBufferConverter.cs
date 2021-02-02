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
using Ws.Extensions.UI.Wpf.Converters;
using Ws.Extensions.UI.Wpf.Utils;
//using Ws.Fus.Fge.Interfaces.Entities;
//using Ws.Fus.ImageViewer.UI.Wpf.Entities;

namespace Ws.Fus.ImageViewer.UI.Wpf.Converters
{
    class FgeBufferConverter : ParametrizedConverter<Visual>
    {
        private WriteableBitmap _writableBitmap;
        private int _prevSourceWidth;
        private int _prevSourceHeight;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return _writableBitmap;

            //try
            //{
            //    var ea = value as ImageBuffer;

            //    if (_writableBitmap == null || _prevSourceWidth != ea.Width || _prevSourceHeight != ea.Height)
            //    {
            //        double dpiX, dpiY;

            //        DpiUtils.GetDpi(Parameter, out dpiX, out dpiY);

            //        _writableBitmap = new WriteableBitmap(ea.Width, ea.Height, dpiX, dpiY, PixelFormats.Bgra32, new BitmapPalette(
            //                new List<Color>() { Colors.White, Colors.Black }));

            //        _prevSourceWidth = ea.Width;
            //        _prevSourceHeight = ea.Height;
            //    }

            //    _writableBitmap.WritePixels(new Int32Rect(0, 0, ea.Width, ea.Height), ea.Buffer, ea.Width * 4, 0);

            //    return _writableBitmap;
            //}
            //catch (Exception ex)
            //{
                return Binding.DoNothing;
            //}
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
