using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

        public static BitmapSource ColorizeImage(this BitmapSource image, ColorMatrix matrix)
        {
            var bitmap = image.ToBitmap();
            bitmap = ApplyMatrix(bitmap, matrix);
            return bitmap.ToBitmapImage();
        }

        public static Bitmap ToBitmap(this BitmapSource bitmapSource)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapSource));
                enc.Save(outStream);
                return new Bitmap(outStream);
            }
        }

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static ColorMatrix COLORMATRIX_TEAL = new ColorMatrix(new float[][]
        {
            new float[] {1, 0, 0, 0, 0},
            new float[] {0, 1.4f, 0, 0, 0},
            new float[] {0, 0, 1.5f, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0.14f, 0.14f, 0, 1}
        });

        private static Bitmap ApplyMatrix(Image image, ColorMatrix matrix)
        {
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix);

            Point[] points =
            {
                new Point(0, 0),
                new Point(image.Width, 0),
                new Point(0, image.Height),
            };
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(image, points, rect, GraphicsUnit.Pixel, attributes);
            }

            return bitmap;
        }
    }
}
