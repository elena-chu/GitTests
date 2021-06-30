using System;

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
        Unregistered,
        Ready,
        Approved
    };

    public static class StripServices
    {
        public static string FriendlyName(this StripOrientation orientation)
        {
            switch (orientation)
            {
                case StripOrientation.eTOR_MRI_NO_ORIENTATION:
                    return string.Empty;
                case StripOrientation.eTOR_MRI_CORONAL_SLICE:
                    return "COR";
                case StripOrientation.eTOR_MRI_AXIAL_SLICE:
                    return "AX";
                case StripOrientation.eTOR_MRI_SAGITTAL_SLICE:
                    return "SAG";
                case StripOrientation.eTOR_MRI_OBLIQUE_CORONAL_SLICE:
                    return "OBL COR";
                case StripOrientation.eTOR_MRI_OBLIQUE_AXIAL_SLICE:
                    return "OBL AX";
                case StripOrientation.eTOR_MRI_OBLIQUE_SAGITTAL_SLICE:
                    return "OBL SAG";
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation)); ;
            }
        }
    }
}
