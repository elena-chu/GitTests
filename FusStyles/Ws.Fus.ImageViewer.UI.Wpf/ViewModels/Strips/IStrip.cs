using Prism.Mvvm;
using System.Windows.Media.Imaging;
using System;
using System.Diagnostics;
using Ws.Extensions.UI.Wpf.Utils;
using System.Windows.Media;

namespace Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips
{
    public interface IStrip
    {
        //StripId Id { get; }

        //StripId ParentId { get; }

        string StripName { get; }

        StripOrientation Orientation { get; }

        bool IsEnabled { get; }

        bool IsLoaded { get; }

        bool IsSecondary { get; }

        bool IsLive { get; }

        bool IsReformatted { get; }

        /// <summary>
        /// #of images in the strip
        /// </summary>
        int ImageCount { get; }

        /// <summary>
        /// The representative image of the strip.
        /// </summary>
        BitmapSource Image { get; }

        Series Series { get; }
    }

    public class Strip: BindableBase, IStrip
    {
        //StripId Id { get; }

        //StripId ParentId { get; }

        private string _stripName;
        public string StripName
        {
            get { return _stripName; }
            set { SetProperty(ref _stripName, value); }
        }

        private StripOrientation _orientation;
        public StripOrientation Orientation
        {
            get { return _orientation; }
            set
            {
                SetProperty(ref _orientation, value);
                RaisePropertyChanged(nameof(OrientationString));
            }
        }

        public string OrientationString { get { return Orientation.FriendlyName(); } }

        protected bool _isEnabled = false;
        virtual public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                SetProperty(ref _isEnabled, value);
                if (!_isEnabled)
                    IsLoaded = false;
            }
        }

        private bool _isLoaded = false;
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                SetProperty(ref _isLoaded, value);
                if (_isLoaded)
                    IsEnabled = true;
            }
        }

        protected bool _isSecondary = false;
        virtual public bool IsSecondary
        {
            get { return _isSecondary; }
            set
            {
                bool previousCompareMode = _isSecondary;
                SetProperty(ref _isSecondary, value);
                if (IsSecondary && !previousCompareMode)
                {
                    var start = DateTime.Now;
                    var matrixCol = Image.ColorizeImage(StripServices.COLORMATRIX_TEAL);
                    var end1= DateTime.Now;
                    Debug.WriteLine($"Matrix colorize {end1 - start}");

                    //Temp other example of conversion
                    FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap(Image, PixelFormats.Bgra32, null, 0.0);
                    byte[] arr1 = GetBytesFromBitmapSource(newFormatedBitmapSource);
                    var colorized = ImageUtils.ColorizeRgb(arr1, Colors.Teal, false, true);
                    BitmapSource bitmap = BitmapSource.Create(Image.PixelWidth,
                        Image.PixelHeight,
                        Image.DpiX, Image.DpiY,
                        PixelFormats.Bgra32, null,
                        colorized,
                        GetStrideFromBitmapSource(newFormatedBitmapSource));
                    var end2 = DateTime.Now;
                    Debug.WriteLine($"Colorize Utile {end2 - end1}");

                    Image = matrixCol;

                    //Matrix colorize 00:00:00.1050105
                    //Colorize Utile 00:00:00.0100010
                    //Matrix colorize 00:00:00.1010101
                    //Colorize Utile 00:00:00.0120012

                }
            }
        }

        private bool _isLive;
        public bool IsLive
        {
            get { return _isLive; }
            set { SetProperty(ref _isLive, value); }
        }

        private bool _isReformatted;
        public bool IsReformatted
        {
            get { return _isReformatted; }
            set { SetProperty(ref _isReformatted, value); }
        }

        private int _imageCount;
        public int ImageCount
        {
            get { return _imageCount; }
            set { SetProperty(ref _imageCount, value); }
        }

        private BitmapSource _image;
        public BitmapSource Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        private Series _series;
        public Series Series
        {
            get { return _series; }
            set { SetProperty(ref _series, value); }
        }

        private byte[] GetBytesFromBitmapSource(BitmapSource bmp)
        {
            int width = bmp.PixelWidth;
            int height = bmp.PixelHeight;
            int stride = GetStrideFromBitmapSource(bmp);

            byte[] pixels = new byte[height * stride];

            bmp.CopyPixels(pixels, stride, 0);

            return pixels;
        }

        private int GetStrideFromBitmapSource(BitmapSource bmp)
        {
            int width = bmp.PixelWidth;
            int height = bmp.PixelHeight;
            int stride = width * ((bmp.Format.BitsPerPixel + 7) / 8);

            return stride;
        }

    }

    public class RegistrationStrip : Strip
    {
        //private RegistrationStatus _registrationStatus = RegistrationStatus.Unregistered;
        //public RegistrationStatus RegistrationStatus
        //{
        //    get { return _registrationStatus; }
        //    set { SetProperty(ref _registrationStatus, value); }
        //}

        private bool _isReference;
        public bool IsReference
        {
            get { return _isReference; }
            set
            {
                SetProperty(ref _isReference, value);
                UpdateAvailability();
            }
        }

        public bool _isRegistered;
        public bool IsRegistered
        {
            get { return _isRegistered; }
            set { SetProperty(ref _isRegistered, value); }
        }

        private bool _hasAutoRegistration;
        public bool HasAutoRegistration
        {
            get { return _hasAutoRegistration; }
            set { SetProperty(ref _hasAutoRegistration, value); }
        }

        override public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (!IsSecondary && IsReference && value)
                    return;
                if (IsSecondary && IsReference && !value)
                    return;
                base.IsEnabled = value;
            }
        }

        override public bool IsSecondary
        {
            get { return _isSecondary; }
            set
            {
                base.IsSecondary = value;
                UpdateAvailability();
            }
        }

        private void UpdateAvailability()
        {
            if (IsSecondary && IsReference)
                IsEnabled = true;
            if (!IsSecondary && IsReference)
                IsEnabled = false;
        }
    }

}
