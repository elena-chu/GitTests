using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
