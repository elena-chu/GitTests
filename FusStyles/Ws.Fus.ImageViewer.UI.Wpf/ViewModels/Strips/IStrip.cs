using Prism.Mvvm;
using System.Windows.Media.Imaging;

namespace Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips
{
    public interface IStrip
    {
        //StripId Id { get; }

        //StripId ParentId { get; }

        string StripName { get; }

        StripOrientation Orientation { get; }

        bool IsAvailable { get; }

        bool IsLoaded { get; }

        bool IsCompareMode { get; }

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

        public string OrientationString { get => Orientation.FriendlyName(); }

        protected bool _isAvailable = false;
        virtual public bool IsAvailable
        {
            get { return _isAvailable; }
            set
            {
                SetProperty(ref _isAvailable, value);
                if (!_isAvailable)
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
                    IsAvailable = true;
            }
        }

        protected bool _isCompareMode = false;
        virtual public bool IsCompareMode
        {
            get { return _isCompareMode; }
            set { SetProperty(ref _isCompareMode, value); }
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

    }

    public class RegistrationStrip : Strip
    {
        private RegistrationStatus _registrationStatus = RegistrationStatus.Unregistered;
        public RegistrationStatus RegistrationStatus
        {
            get { return _registrationStatus; }
            set { SetProperty(ref _registrationStatus, value); }
        }

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


        override public bool IsAvailable
        {
            get { return _isAvailable; }
            set
            {
                if (!IsCompareMode && IsReference && value)
                    return;
                if (IsCompareMode && IsReference && !value)
                    return;
                base.IsAvailable = value;
            }
        }

        override public bool IsCompareMode
        {
            get { return _isCompareMode; }
            set
            {
                base.IsCompareMode = value;
                UpdateAvailability();
            }
        }

        private void UpdateAvailability()
        {
            if (IsCompareMode && IsReference)
                IsAvailable = true;
            if (!IsCompareMode && IsReference)
                IsAvailable = false;
        }
    }

}
