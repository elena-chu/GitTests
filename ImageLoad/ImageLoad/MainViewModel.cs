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
using System.Threading;

namespace ImageLoad
{
    public class MainViewModel : BindableBase
    {
        private string path1 = @"D:\Elena\Tests\ImageLoad\ImageLoad\Images\Img_01.png";
        private string path2 = @"D:\Elena\Tests\ImageLoad\ImageLoad\Images\Img_02.png";

        //private string path1 = @"Images\Img_01.png";
        //private string path2 = @"Images\Img_02.png";

        private WriteableBitmap _writableBitmapUpper;
        private WriteableBitmap _writableBitmapLower;
        private WriteableBitmap _writableBitmapSingle;

        private int _oldSourceWidth;
        private int _oldSourceHeight;
        private Matrix _toDevice;

        private byte[] _prevArrayUpper = null;
        private byte[] _prevArrayLower = null;
        //private bool _isFusion;

        private bool _isFusing;

        public MainViewModel()
        {
            LoadImageCommand = new DelegateCommand<object>(LoadImageExecute);
            FusionCommand = new DelegateCommand(FusionExecte);

            IsLower = true;
            IsAlfa = true;
            IsRgb = true;
            IsFusionGrayed = true;
            IsFusionColored = true;

            LowerColor = Color.FromRgb(255, 189, 39);
            //UpperColor = Color.FromRgb((byte)(255 - LowerColor.R), (byte)(255 - LowerColor.G), (byte)(255 - LowerColor.B));
            UpperColor = Color.FromRgb(109, 158, 255);


            IsUpperColored = true;
        }

        public DelegateCommand<object> LoadImageCommand { get; set; }
        public DelegateCommand ColorizeCommand { get; set; }
        public DelegateCommand FusionCommand { get; set; }

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

        private ImageSource _imageSingle;
        public ImageSource ImageSingle
        {
            get { return _imageSingle; }
            set { SetProperty(ref _imageSingle, value); }
        }

        private bool _isLower;
        public bool IsLower
        {
            get { return _isLower; }
            set { SetProperty(ref _isLower, value); }
        }

        public double _curtainSlidedValue = 0.5;
        public double CurtainSlidedValue
        {
            get { return _curtainSlidedValue; }
            set
            {
                SetProperty(ref _curtainSlidedValue, value);
            }
        }

        public double _opacitySlidedValue = 1;
        public double OpacitySlidedValue
        {
            get { return _opacitySlidedValue; }
            set
            {
                SetProperty(ref _opacitySlidedValue, value);
            }
        }

        public double _fusionSlidedValue = 0.5;
        public double FusionSlidedValue
        {
            get { return _fusionSlidedValue; }
            set
            {
                if (SetProperty(ref _fusionSlidedValue, value))
                {
                    FusionExecte();
                }
            }
        }

        private bool _isAlfa;
        public bool IsAlfa
        {
            get { return _isAlfa; }
            set
            {
                SetProperty(ref _isAlfa, value);
                DoColor(null);
            }
        }

        private bool _isWhiteColorized;
        public bool IsWhiteColorized
        {
            get { return _isWhiteColorized; }
            set
            {
                SetProperty(ref _isWhiteColorized, value);
                DoColor(null);
            }
        }

        private bool _isHsv;
        public bool IsHsv
        {
            get { return _isHsv; }
            set
            {
                SetProperty(ref _isHsv, value);
                if(value)
                {
                    IsHsl = false;
                    IsRgb = false;
                    DoColor(null);
                }
            }
        }

        private bool _isHsl;
        public bool IsHsl
        {
            get { return _isHsl; }
            set
            {
                SetProperty(ref _isHsl, value);
                if (value)
                {
                    IsHsv = false;
                    IsRgb = false;
                    DoColor(null);
                }
            }
        }

        private bool _isRgb;
        public bool IsRgb
        {
            get { return _isRgb; }
            set
            {
                SetProperty(ref _isRgb, value);
                if (value)
                {
                    IsHsv = false;
                    IsHsl = false;
                    DoColor(null);
                }
            }
        }

        private bool _isUpperColored;
        public bool IsUpperColored
        {
            get { return _isUpperColored; }
            set
            {
                SetProperty(ref _isUpperColored, value);
                DoColor(null);
            }
        }

        private bool _isLowerColored;
        public bool IsLowerColored
        {
            get { return _isLowerColored; }
            set
            {
                SetProperty(ref _isLowerColored, value);
                DoColor(null);
            }
        }

        public bool _isFusionGrayed;
        public bool IsFusionGrayed
        {
            get { return _isFusionGrayed; }
            set
            {
                SetProperty(ref _isFusionGrayed, value);
                SetFusion();
            }
        }

        public bool _isFusionColored;
        public bool IsFusionColored
        {
            get { return _isFusionColored; }
            set
            {
                SetProperty(ref _isFusionColored, value);
                SetFusion();
            }
        }

        public Color LowerColor { get; set; }

        public Color UpperColor { get; set; }

        public void LoadImageExecute(object obj)
        {
            int id = int.Parse(obj.ToString());
            string path = path1;
            if (id != 1)
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



            if (ImageHelper.AreByteArraysEqual(IsLower ? _prevArrayLower : _prevArrayUpper, array))
            {
                Debug.WriteLine("ArraySegment are equals");
                return;
            }

            DateTime startTime = DateTime.Now;
            Debug.WriteLine($"Write started at{startTime.ToString("hh:mm.fffff")}");



            //if (IsLower)
            //{
            //    _writableBitmapLower.WritePixels(new Int32Rect(0, 0, Width, Height), array, GetStride(), 0);
            //    _prevArrayLower = array;
            //}
            //else
            //{
            //    _prevArrayUpper = array;
            //    if (IsAlfa)
            //        SetAlfa();
            //    else
            //        SetColorize();
            //}
            if (IsLower)
            {
                _prevArrayLower = array;
            }
            else
            {
                _prevArrayUpper = array;
            }

            DoColor(IsLower);

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
                array = new byte[stride * Height];
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

        private void DoColor(bool? isLower)
        {
            if (IsAlfa)
            {
                SetColorize(true);
                SetAlfa();
            }
            else
            {
                if (isLower == null)
                {
                    SetColorize(true);
                    SetColorize(false);
                }
                else
                    SetColorize(isLower.Value);
            }
        }

        private void SetColorize(bool isLower)
        {
            if (isLower)
            {
                if (_writableBitmapLower == null)
                    return;

                if (IsLowerColored)
                {
                    var bytes = ColorizeImage(true);
                    if (bytes != null)
                        _writableBitmapLower.WritePixels(new Int32Rect(0, 0, Width, Height), bytes, GetStride(), 0);
                }
                else
                {
                    _writableBitmapLower.WritePixels(new Int32Rect(0, 0, Width, Height), _prevArrayLower, GetStride(), 0);
                }
            }
            else
            {
                if (_writableBitmapUpper == null)
                    return;

                if (IsUpperColored)
                {
                    var bytes = ColorizeImage();
                    if (bytes != null)
                        _writableBitmapUpper.WritePixels(new Int32Rect(0, 0, Width, Height), bytes, GetStride(), 0);
                }
                else
                {
                    _writableBitmapUpper.WritePixels(new Int32Rect(0, 0, Width, Height), _prevArrayUpper, GetStride(), 0);
                }
            }

        }

        private byte[] ColorizeImage(bool isLower = false)
        {
            WriteableBitmap writableBitmap = isLower ? _writableBitmapLower : _writableBitmapUpper;
            Color addedColor = isLower ? LowerColor : UpperColor;
            byte[] prevArray = isLower ? _prevArrayLower : _prevArrayUpper;

            if (writableBitmap == null)
                return null;

            //Color lowerCol = Colors.Orange;
            ImageData image1 = new ImageData()
            {
                Array = prevArray,
                AddedColor = addedColor,
                BytesPerPixel = 4,
            };

            DateTime startTime = DateTime.Now;
            //Debug.WriteLine($"Colorize started at{startTime.ToString("hh:mm.fffff")}");
            byte[] res;
            string type = "";
            if (IsHsv)
            {
                res = ImageHelper.ColorizeHsv(image1, IsWhiteColorized);
                type = "Hsv";
            }
            else if(IsHsl)
            {
                res = ImageHelper.Colorize(image1, IsWhiteColorized);
                type = "Hsl";
            }
            else
            {
                res = ImageHelper.ColorizeRgb(image1, IsWhiteColorized);
                type = "Rgb";
            }

            DateTime endTime = DateTime.Now;
            var duration = (endTime - startTime).TotalMilliseconds;
            Debug.WriteLine($"Colorize {type} ended at{endTime.ToString("hh:mm.fffff")}, duration={duration}ms");

            return res;
        }


        private void FusionExecte()
        {
            //_isFusion = true;
            SetFusion();
        }

        private void SetAlfa()
        {
            var bytes = GetAlfa();
            if (bytes != null)
                _writableBitmapUpper.WritePixels(new Int32Rect(0, 0, Width, Height), bytes, GetStride(), 0);
        }

        private byte[] GetAlfa()
        {
            if (_writableBitmapUpper == null)
                return null;
            var res = ImageHelper.SetAlfa(_prevArrayUpper, _writableBitmapUpper.Format.BitsPerPixel / 8);
            return res;
        }

        private async Task SetFusion()
        {
            if (_writableBitmapSingle == null)
            {
                _writableBitmapSingle = new WriteableBitmap(Width, Height, 96.0, 96.0, PixelFormats.Bgra32, null);
                ImageSingle = _writableBitmapSingle;
            }

            if (_isFusing)
            {
                Debug.WriteLine("Busy in Fusion");
                return;
            }

            var ti = Thread.CurrentThread.ManagedThreadId;
            Debug.WriteLine($"Current threaId before task: {ti}");
            _isFusing = true;

            var bytes = await Task.Run(() =>GetFusion());
            if (bytes != null)
                _writableBitmapSingle.WritePixels(new Int32Rect(0, 0, Width, Height), bytes, GetStride(), 0);

            _isFusing = false;
        }

        private byte[] GetFusion()
        {
            //Thread.Sleep(2000);
            var ti = Thread.CurrentThread.ManagedThreadId;
            Debug.WriteLine($"Current threaId inside task: {ti}");

            if (_writableBitmapUpper == null && _writableBitmapLower == null)
                return null;
                //return await Task.FromResult<byte[]>(null);

            ImageData image1 = new ImageData()
            {
                Array = _prevArrayLower,
                AddedColor = LowerColor,
                BytesPerPixel = 4,
            };

            ImageData image2 = new ImageData()
            {
                Array = _prevArrayUpper,
                AddedColor = UpperColor,
                BytesPerPixel = 4,
            };

            DateTime startTime = DateTime.Now;
            //Debug.WriteLine($"Fusion started at{startTime.ToString("hh:mm.fffff")}");
            byte[] res;
            string type = "";
            if (IsHsv)
            {
                res = ImageHelper.SetFusionHsv(image1, image2, FusionSlidedValue);
                type = "Hvs";
            }
            else if(IsHsl)
            {
                res = ImageHelper.SetFusion(image1, image2, FusionSlidedValue);
                type = "Hls";
            }
            else //Rgb
            {
                type = "Rgb";
                if (IsFusionGrayed)
                    res = ImageHelper.SetFusionRgb_2(image1, image2, FusionSlidedValue, IsFusionColored);
                else
                    res = ImageHelper.SetFusionRgb(image1, image2, FusionSlidedValue, IsFusionColored);
            }

            DateTime endTime = DateTime.Now;
            var duration = (endTime - startTime).TotalMilliseconds;
            Debug.WriteLine($"Fusion {type} ended at{endTime.ToString("hh:mm.fffff")}, duration={duration}ms");

            return res;
            //return await Task.FromResult(res);
        }

    }
}
