using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

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

    public interface IStrip
    {
        //StripId Id { get; }

        //StripId ParentId { get; }

        string StripName { get; }

        StripOrientation Orientation { get; }

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
            set { SetProperty(ref _orientation, value); }
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



}
