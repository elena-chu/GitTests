using Prism.Mvvm;
using System.Windows.Media.Imaging;

namespace Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips
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

    public enum RegistrationStatus
    {
        NotReady,
        Ready,
        Approved
    };

    public interface IStrip
    {
        //StripId Id { get; }

        //StripId ParentId { get; }

        string StripName { get; }

        StripOrientation Orientation { get; }

        bool IsAvailable { get; }

        bool IsLoaded { get; }

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
            set { SetProperty(ref _orientation, value); }
        }

        private bool _isAvailable = false;
        public bool IsAvailable
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
    }

    public class RegistrationStrip : Strip
    {
        private RegistrationStatus _registrationStatus = RegistrationStatus.NotReady;
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
                if (_isReference)
                    IsAvailable = false;    // TODO: 1. when Compare, should be Loaded & Available 2. override IsAvailable and do logic
            }
        }
    }

}
