using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Ws.Extensions.UI.Wpf.Utils
{
    public static class ImageUtils
    {
        private const float minColorValue = 0.1f;
        private const float maxColorValue = 0.9f;

        public static byte[] SetAlfa(byte[] array, int bytesPerPixel) 
        {
            if (array == null || array.Length <= bytesPerPixel)
            {
                return array;
            }

            DateTime startTime = DateTime.Now;
            byte[] targ = new byte[array.Length];
            for (int i = 0; i < array.Length; i += bytesPerPixel)
            {
                targ[i] = array[i];
                targ[i + 1] = array[i + 1];
                targ[i + 2] = array[i + 2];
                if ((array[i] & array[i + 1] & array[i + 2]) == array[i])
                {
                    targ[i + 3] = (byte)(255 - array[i]);
                }
                else
                {
                    targ[i + 3] = array[i + 3];
                }
            }
            DateTime endTime = DateTime.Now;
            //Debug.WriteLine($"Alfa duration={(endTime - startTime).TotalMilliseconds}ms");
            return targ;
        }

        public static bool AreImagesEqual(byte[] array1, byte[] array2)
        {
            return ByteUtils.Compare_point(array1, array2);
        }

        public static byte[] FusionTwoImages(byte[] array1, byte[] array2, Color color1, Color color2, double coeff = 0, int bytesPerPixel = 4)
        {
            if(array1 == null || array2==null || array1.Length != array2.Length)
            {
                return array1;
            }
            int length = array1.Length;

            //float col1_r = color1.R / 255f;
            //float col1_g = color1.G / 255f;
            //float col1_b = color1.B / 255f;
            
            //Complementary color calculation
            float col1_r = (255 - color2.R) / 255f; 
            float col1_g = (255 - color2.G) / 255f;
            float col1_b = (255 - color2.B) / 255f;

            float col2_r = color2.R / 255f;
            float col2_g = color2.G / 255f;
            float col2_b = color2.B / 255f;
            
            //Complementary color calculation
            //float col2_r = (255 - color1.R) / 255f;
            //float col2_g = (255 - color1.G) / 255f;
            //float col2_b = (255 - color1.B) / 255f;

            byte[] targ = new byte[length];
            for (int i = 0; i < length; i += bytesPerPixel)
            {
                float gr1 = array1[i] / 255f;
                float gr2 = array2[i] / 255f;

                //gr1 = NormalizeEdges(gr1); //Check whether to use
                //gr2 = NormalizeEdges(gr2);

                int r, g, b;

                //--- regular a blending ----

                //r = Math.Min((int)((gr1 * col1_r * (1 - coeff) + gr2 * col2_r * coeff) * 255f + 0.5), 255);
                //g = Math.Min((int)((gr1 * col1_g * (1 - coeff) + gr2 * col2_g * coeff) * 255f + 0.5), 255);
                //b = Math.Min((int)((gr1 * col1_b * (1 - coeff) + gr2 * col2_b * coeff) * 255f + 0.5), 255);

                //--- end of regular a blending ----

                //--- enhanced a blending ---

                float coeff_a = Math.Min((2f - 2f * (float)coeff), 1f);
                float coeff_b = Math.Min((2f * (float)coeff), 1f);

                r = (int)((col1_r * (gr1 * coeff_a) + col2_r * (gr2 * coeff_b)) * 255 + 0.5);
                g = (int)((col1_g * (gr1 * coeff_a) + col2_g * (gr2 * coeff_b)) * 255 + 0.5);
                b = (int)((col1_b * (gr1 * coeff_a) + col2_b * (gr2 * coeff_b)) * 255 + 0.5);

                //--- end of enhanced a blending ---

                targ[i] = (byte)b;
                targ[i + 1] = (byte)g;
                targ[i + 2] = (byte)r;
                if (bytesPerPixel > 3)
                    targ[i + 3] = array1[i + 3];
            }

            return targ;
        }

        public static byte[] FusionTwoImagesWithGray(byte[] array1, byte[] array2, Color color1, Color color2, double coeff = 0, int bytesPerPixel = 4)
        {
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return array1;
            }
            int length = array1.Length;

            //float col1_r = color1.R / 255f;
            //float col1_g = color1.G / 255f;
            //float col1_b = color1.B / 255f;

            //Complementary color calculation
            float col1_r = (255 - color2.R) / 255f;
            float col1_g = (255 - color2.G) / 255f;
            float col1_b = (255 - color2.B) / 255f;

            float col2_r = color2.R / 255f;
            float col2_g = color2.G / 255f;
            float col2_b = color2.B / 255f;

            //Complementary color calculation
            //float col2_r = (255 - color1.R) / 255f;
            //float col2_g = (255 - color1.G) / 255f;
            //float col2_b = (255 - color1.B) / 255f;

            byte[] targ = new byte[length];
            for (int i = 0; i < length; i += bytesPerPixel)
            {
                float gr1 = array1[i] / 255f;
                float gr2 = array2[i] / 255f;

                //gr1 = NormalizeEdges(gr1); //Check whether to use
                //gr2 = NormalizeEdges(gr2);

                float coeff_a = Math.Min((2f - 2f * (float)coeff), 1f);
                float coeff_b = Math.Min((2f * (float)coeff), 1f);

                int r, g, b;

                r = (int)((col1_r * (gr1 * coeff_a + gr2 * (1f - coeff_a)) + col2_r * (gr2 * coeff_b + gr1 * (1 - coeff_b))) * 255 + 0.5);
                g = (int)((col1_g * (gr1 * coeff_a + gr2 * (1f - coeff_a)) + col2_g * (gr2 * coeff_b + gr1 * (1 - coeff_b))) * 255 + 0.5);
                b = (int)((col1_b * (gr1 * coeff_a + gr2 * (1f - coeff_a)) + col2_b * (gr2 * coeff_b + gr1 * (1 - coeff_b))) * 255 + 0.5);

                r = Math.Max(Math.Min(r, 255), 0);
                g = Math.Max(Math.Min(g, 255), 0);
                b = Math.Max(Math.Min(b, 255), 0);

                targ[i] = (byte)b;
                targ[i + 1] = (byte)g;
                targ[i + 2] = (byte)r;
                if (bytesPerPixel > 3)
                    targ[i + 3] = array1[i + 3];
            }

            return targ;
        }


        private static float NormalizeEdges(float value)
        {
            if (value <= minColorValue)
                return 0f;
            if (value >= maxColorValue)
                return 1f;
            float ret = (value - minColorValue) / (maxColorValue - minColorValue);
            return ret;
        }

        public static byte[] ColorizeRgb(byte[] array, Color color, bool isWhiteColorized, bool colorizeColored = false, int bytesPerPixel = 4)
        {
            if (array == null || array.Length <= bytesPerPixel)
            {
                return array;
            }

            int length = array.Length;

            float col1_r = color.R / 255f;
            float col1_g = color.G / 255f;
            float col1_b = color.B / 255f;

            byte[] targ = new byte[length];
            for (int i = 0; i < length; i += bytesPerPixel)
            {
                int b1 = array[i];
                int g1 = array[i + 1];
                int r1 = array[i + 2];

                bool isGray = r1 == g1 && g1 == b1;
                if (!isGray && !colorizeColored)
                {
                    targ[i] = array[i];
                    targ[i + 1] = array[i + 1];
                    targ[i + 2] = array[i + 2];
                    if (bytesPerPixel > 3)
                        targ[i + 3] = array[i + 3];
                    continue;
                }


                float gr = b1 / 255f;

                //17ms using regular calculation
                int r = Math.Min((int)((gr * col1_r) * 255f + 0.5), 255);
                int g = Math.Min((int)((gr * col1_g) * 255f + 0.5), 255);
                int b = Math.Min((int)((gr * col1_b) * 255f + 0.5), 255);

                targ[i] = (byte)b;
                targ[i + 1] = (byte)g;
                targ[i + 2] = (byte)r;
                if (bytesPerPixel > 3)
                    targ[i + 3] = array[i + 3];

                //77ms - Using Color.Multiply
                //Color n = Color.Multiply(color, gr); 
                //targ[i] = n.B;
                //targ[i + 1] = n.G;
                //targ[i + 2] = n.R;
                //if (bytesPerPixel > 3)
                //    targ[i + 3] = array[i + 3];
            }

            return targ;
        }
    }
}
