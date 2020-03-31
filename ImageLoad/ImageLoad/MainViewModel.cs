using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;

namespace ImageLoad
{
    public class MainViewModel : BindableBase
    {
        private string path1 = @"D:\Elena\Tests\ImageLoad\ImageLoad\Images\Img_01.png";
        private string path2 = @"D:\Elena\Tests\ImageLoad\ImageLoad\Images\Img_02.png";

        private WriteableBitmap _writableBitmapUpper;
        private WriteableBitmap _writableBitmapLower;

        private int _oldSourceWidth;
        private int _oldSourceHeight;
        private Matrix _toDevice;

        private byte[] _prevArrayUpper = null;
        private byte[] _prevArrayLower = null;

        public MainViewModel()
        {
            LoadImageCommand = new DelegateCommand<object>(LoadImageExecute);
            IsLower = true;
        }

        public DelegateCommand<object> LoadImageCommand { get; set; }
        public int Width { get; private set; } = 512;
        public int Height { get; private set; } = 512;

        private ImageSource _imageLower;
        public ImageSource ImageLower
        {
            get { return _imageLower; }
            set { SetProperty(ref _imageLower, value); }
        }

        private ImageSource _imageUpper;
        public ImageSource ImageUpper
        {
            get { return _imageUpper; }
            set { SetProperty(ref _imageUpper, value); }
        }

        private bool _isLower;
        public bool IsLower
        {
            get { return _isLower; }
            set { SetProperty(ref _isLower, value);}
        }

        public void LoadImageExecute(object obj)
        {
            int id = int.Parse(obj.ToString());
            string path = path1;
            if(id != 1)
            {
                path = path2;
            }

            if (_oldSourceWidth != Width || _oldSourceHeight != Height)
            {
                _oldSourceWidth = Width;
                _oldSourceHeight = Height;
                _writableBitmapUpper = new WriteableBitmap(Width, Height, 96.0, 96.0, PixelFormats.Bgra32, null/*new BitmapPalette(new List<Color>() { Colors.White, Colors.Black })*/);
                ImageUpper = _writableBitmapUpper;
                _writableBitmapLower = new WriteableBitmap(Width, Height, 96.0, 96.0, PixelFormats.Bgra32, null/*new BitmapPalette(new List<Color>() { Colors.White, Colors.Black })*/);
                ImageLower = _writableBitmapLower;
            }
            byte[] array = LoadFromFile(path);


            if(ImageHelper.AreByteArraysEqual(IsLower? _prevArrayLower: _prevArrayUpper, array))
            {
                Debug.WriteLine("ArraySegment are equals");
                return;
            }

            DateTime startTime = DateTime.Now;
            Debug.WriteLine($"Write started at{startTime.ToString("hh:mm.fffff")}");

            byte[] alfaArrray = ImageHelper.SetAlfa(array, _writableBitmapUpper.Format.BitsPerPixel/8);
            if(IsLower)
            {
                _writableBitmapLower.WritePixels(new Int32Rect(0, 0, Width, Height), alfaArrray, GetStride(), 0);
                _prevArrayLower = array;
            }
            else
            {
                _writableBitmapUpper.WritePixels(new Int32Rect(0, 0, Width, Height), alfaArrray, GetStride(), 0);
                _prevArrayUpper = array;

            }

            DateTime endTime = DateTime.Now;
            var duration = (endTime - startTime).TotalMilliseconds;
            Debug.WriteLine($"Write ended at{endTime.ToString("hh:mm.fffff")}, duration={duration}ms");
        }

        private byte[] LoadFromFile(string path)
        {
            byte[] array;
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(fileStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                BitmapSource bitmapSource = decoder.Frames[0];
                int stride = GetStride();
                array = new byte[stride*Height];
                bitmapSource.CopyPixels(array, stride, 0);
            }
            return array;
        }

        private int GetStride()
        {
            WriteableBitmap writableBitmap = _writableBitmapUpper ?? _writableBitmapLower;
            var stride = (Width * writableBitmap.Format.BitsPerPixel + 7) / 8;
            return stride;
        }

    }
}
