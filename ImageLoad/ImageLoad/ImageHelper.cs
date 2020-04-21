using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ImageLoad
{
    public class ImageHelper
    {
        private static Dictionary<int, int> GammaCorrectedValues = null;
        private static Dictionary<int, int> GammaCorrectedValues1 = null;

        private static Dictionary<float, float> BrightnessCorrectedValues = null;

        private const float minColorValue = 0.1f;
        private const float maxColorValue = 0.9f;

        public static bool AreByteArraysEqual(byte[] array1, byte[] array2)
        {
            if (array1 == null || array2 == null)
            {
                return false;
            }
            DateTime startTime = DateTime.Now;
            bool equal = Compare_enumarable(array1, array2);
            DateTime afterEnumer = DateTime.Now;
            equal = Compare_loop(array1, array2);
            DateTime afterLoop = DateTime.Now;
            equal = Compare_point(array1, array2);
            DateTime afterPoint = DateTime.Now;

            Debug.WriteLine($"Compare_enumarable duration={(afterEnumer - startTime).TotalMilliseconds}ms");
            Debug.WriteLine($"Compare_loop duration={(afterLoop - afterEnumer).TotalMilliseconds}mc");
            Debug.WriteLine($"Compare_Point duration={(afterPoint - afterLoop).Milliseconds}mc");

            return equal;
        }

        private static bool Compare_enumarable(byte[] array1, byte[] array2) //37ms
        {
            return Enumerable.SequenceEqual(array1, array2);
        }

        private static bool Compare_loop(byte[] array1, byte[] array2) //6ms
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
                if (array1[i] != array2[i])
                    return false;

            return true;
        }

        private static unsafe bool Compare_point(byte[] a, byte[] b)//1ms
        {
            if (a.Length != b.Length) return false;
            int len = a.Length;
            unsafe
            {
                fixed (byte* ap = a, bp = b)
                {
                    long* alp = (long*)ap, blp = (long*)bp;
                    for (; len >= 8; len -= 8)
                    {
                        if (*alp != *blp) return false;
                        alp++;
                        blp++;
                    }
                    byte* ap2 = (byte*)alp, bp2 = (byte*)blp;
                    for (; len > 0; len--)
                    {
                        if (*ap2 != *bp2) return false;
                        ap2++;
                        bp2++;
                    }
                }
            }
            return true;
        }

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
            Debug.WriteLine($"Alfa duration={(endTime - startTime).TotalMilliseconds}ms");
            return targ;
        }
        // Convert an RGB value into an HLS value.
        public static void RgbToHls(int r, int g, int b,
            out double h, out double l, out double s)
        {
            // Convert RGB to a 0.0 to 1.0 range.
            double double_r = r / 255.0;
            double double_g = g / 255.0;
            double double_b = b / 255.0;

            // Get the maximum and minimum RGB components.
            double max = double_r;
            if (max < double_g) max = double_g;
            if (max < double_b) max = double_b;

            double min = double_r;
            if (min > double_g) min = double_g;
            if (min > double_b) min = double_b;

            double diff = max - min;
            l = (max + min) / 2;
            if (Math.Abs(diff) < 0.00001)
            {
                s = 0;
                h = 0;  // H is really undefined.
            }
            else
            {
                if (l <= 0.5) s = diff / (max + min);
                else s = diff / (2 - max - min);

                double r_dist = (max - double_r) / diff;
                double g_dist = (max - double_g) / diff;
                double b_dist = (max - double_b) / diff;

                if (double_r == max) h = b_dist - g_dist;
                else if (double_g == max) h = 2 + r_dist - b_dist;
                else h = 4 + g_dist - r_dist;

                h = h * 60;
                if (h < 0) h += 360;
            }
        }

        // Convert an HLS value into an RGB value.
        public static void HlsToRgb(double h, double l, double s,
            out int r, out int g, out int b)
        {
            double p2;
            if (l <= 0.5) p2 = l * (1 + s);
            else p2 = l + s - l * s;

            double p1 = 2 * l - p2;
            double double_r, double_g, double_b;
            if (s == 0)
            {
                double_r = l;
                double_g = l;
                double_b = l;
            }
            else
            {
                double_r = QqhToRgb(p1, p2, h + 120);
                double_g = QqhToRgb(p1, p2, h);
                double_b = QqhToRgb(p1, p2, h - 120);
            }

            // Convert RGB to the 0 to 255 range.
            r = (int)(double_r * 255.0);
            g = (int)(double_g * 255.0);
            b = (int)(double_b * 255.0);
        }

        private static double QqhToRgb(double q1, double q2, double hue)
        {
            if (hue > 360) hue -= 360;
            else if (hue < 0) hue += 360;

            if (hue < 60) return q1 + (q2 - q1) * hue / 60;
            if (hue < 180) return q2;
            if (hue < 240) return q1 + (q2 - q1) * (240 - hue) / 60;
            return q1;
        }

        public static byte[] SetFusion(ImageData image1, ImageData image2, double coeff)
        {
            if (!image1.IsImadedataValid() || !image2.IsImadedataValid() || image1.Array.Length != image2.Array.Length)
                return image1.Array;

            int length = image1.Array.Length;
            int bytesPerPixel = image1.BytesPerPixel;

            double col1_h, col1_l, col1_s, col2_h, col2_l, col2_s;
            ColorHelper.RgbToHls(image1.AddedColor.R, image1.AddedColor.G, image1.AddedColor.B, out col1_h, out col1_l, out col1_s);
            ColorHelper.RgbToHls(image2.AddedColor.R, image2.AddedColor.G, image2.AddedColor.B, out col2_h, out col2_l, out col2_s);


            byte[] targ = new byte[length];
            for (int i = 0; i < length; i += bytesPerPixel)
            {
                int r1 = image1.Array[i];
                int g1 = image1.Array[i + 1];
                int b1 = image1.Array[i + 2];

                int r2 = image2.Array[i];
                int g2 = image2.Array[i + 1];
                int b2 = image2.Array[i + 2];

                //double h1, l1, s1, h2, l2, s2;
                ////ColorHelper.RgbToHls(r1, g1, b1, out h1, out l1, out s1);
                ////ColorHelper.RgbToHls(r2, g2, b2, out h2, out l2, out s2);


                //double l1_adj = l1 * (1 - coeff);
                //double l2_adj = l2 * coeff;
                double l1_adj = r1 * (1 - coeff);//Source is gray, so for lightness enough one value: r or b or g
                double l2_adj = r2 * coeff;


                double h, l, s;
                int r, g, b;

                l = (l1_adj + l2_adj) / 255;
                //l = ((lc1 > lc2) ? cl1 : cl2) * l;
                //h = (lc1 > lc2) ? ch1 : ch2;
                h = (l1_adj > l2_adj) ? col1_h : col2_h;

                ////Start . Adds green color in-between
                //var d = col2_h - col1_h;
                //var delta = d + ((Math.Abs(d) > 180) ? ((d < 0) ? 360 : -360) : 0);
                //h = ((col1_h + (delta * (l2_adj/l))) + 360) % 360;
                ////End . Adds green color in-between


                //s = l;
                //s = s1 * (1 - coeff) + s2 * coeff;
                //s = (lc1 > lc2) ? cs1*(1 - coeff) : cs2*coeff;
                s = ((l1_adj > l2_adj) ? col1_s : col2_s) * l; //this desaturates black/dark gray colors 
                //s = ((l1_adj > l2_adj) ? col1_s * (1-coeff) : col2_s * coeff); //saturation intensity depends on coeff. Based of selected color's saturation

                l = l * (1.0 - Math.Abs(coeff - 0.5)); //To colorize white lightness should be 0.5 for full intensity

                ColorHelper.HlsToRgb(h, l, s, out r, out g, out b);

                /* 
                 //Pre-multiplied alpha compositing

                Color orig, tgt;
                if(lc1 > lc2)
                {
                    orig = image1.AddedColor;
                    orig.A = (byte)(lc1 *255);
                    tgt = image2.AddedColor;
                    tgt.A = (byte)(lc2 * 255);
                }
                else
                {
                    orig = image2.AddedColor;
                    orig.A = (byte)(lc2 * 255);
                    tgt = image1.AddedColor;
                    tgt.A = (byte)(lc1 * 255);
                }

                int a_n = ((orig.A * orig.A) >> 8) + ((tgt.A * (255 - orig.A)) >> 8);
                int r_n = ((orig.R * orig.A) >> 8) + ((tgt.R * (255 - orig.A)) >> 8);
                int g_n = ((orig.G * orig.A) >> 8) + ((tgt.G * (255 - orig.A)) >> 8);
                int b_n = ((orig.B * orig.A) >> 8) + ((tgt.B * (255 - orig.A)) >> 8);

                double h_n, l_n, s_n;
                RgbToHls(r_n, g_n, b_n, out h_n, out l_n, out s_n);

                HlsToRgb(h_n, l, s, out r, out g, out b);
                */

                //HlsToRgb(0, l, 0, out r, out g, out b);

                targ[i] = (byte)b;
                targ[i + 1] = (byte)g;
                targ[i + 2] = (byte)r;
                if (image1.BytesPerPixel > 3)
                    targ[i + 3] = image1.Array[i + 3];
            }

            return targ;
        }

        public static byte[] SetFusionHsv(ImageData image1, ImageData image2, double coeff)
        {
            if (!image1.IsImadedataValid() || !image2.IsImadedataValid() || image1.Array.Length != image2.Array.Length)
                return image1.Array;

            int length = image1.Array.Length;
            int bytesPerPixel = image1.BytesPerPixel;

            double col1_h, col1_v, col1_s, col2_h, col2_v, col2_s;
            ColorHelper.RgbToHvs(image1.AddedColor.R, image1.AddedColor.G, image1.AddedColor.B, out col1_h, out col1_v, out col1_s);
            ColorHelper.RgbToHvs(image2.AddedColor.R, image2.AddedColor.G, image2.AddedColor.B, out col2_h, out col2_v, out col2_s);

            var rand = new Random();

            byte[] targ = new byte[length];
            for (int i = 0; i < length; i += bytesPerPixel)
            {
                int r1 = image1.Array[i];
                int g1 = image1.Array[i + 1];
                int b1 = image1.Array[i + 2];

                int r2 = image2.Array[i];
                int g2 = image2.Array[i + 1];
                int b2 = image2.Array[i + 2];

                double h, v, s;
                double l1_adj, l2_adj, l;

                //Version1
                l1_adj = r1 * (1 - coeff);//Source is gray, so for lightness enough one value: r or b or g
                l2_adj = r2 * coeff;
                l = Math.Min(l1_adj + l2_adj, 255);

                h = (l1_adj > l2_adj) ? col1_h : col2_h;
                if (Math.Abs(l1_adj - l2_adj) < 20)
                {
                    //h = rand.Next(0,1) == 0? col1_h : col2_h;
                    h = 1 % 2 == 1 ? col1_h : col2_h;
                }
                v = Math.Min(l / 255 * 1.25, 1);
                //s = ((l1_adj > l2_adj) ? col1_s * v* (1 - coeff) : col2_s * v* coeff);
                s = ((l1_adj > l2_adj) ? col1_s * (1 - coeff) : col2_s * coeff);

                //Version2
                //l1_adj = Math.Min((coeff <= 0.55 ? r1 : r1 * (1 - coeff) *2)/255, 1);
                //l2_adj = Math.Min((coeff > 0.45 ? r2 : r2 * coeff*2)/255, 1);
                //l = Math.Min(l1_adj +  l2_adj, 1);
                //h = (l1_adj > l2_adj) ? col1_h : col2_h;
                //v = l ;
                //s = ((l1_adj > l2_adj) ? col1_s : col2_s) /* (coeff % 0.5) * (1.0 - ((l1_adj + l2_adj) / 2.0))*/;
                //if(v>0.9 && l1_adj + l2_adj >1)
                //{
                //    s = s * (1.0 - ((l1_adj + l2_adj) / 2.0));
                //}

                //Version3
                //l1_adj = Math.Min((coeff <= 0.5 ? r1 : r1 * ((1 - coeff) * 2)) / 255, 1);
                //l2_adj = Math.Min((coeff >= 0.5 ? r2 : r2 * coeff * 2) / 255, 1);
                ////l = Math.Min(l1_adj + l2_adj, 1);
                //l = Math.Max(l1_adj, l2_adj);
                //h = (l1_adj > l2_adj) ? col1_h : col2_h;
                //v = l < 0.5 ? l : 1;
                //s = l1_adj + l2_adj > 1 ? l1_adj + l2_adj - 1 : 1;


                int r, g, b;
                ColorHelper.HvsToRgb(h, v, s, out r, out g, out b);

                targ[i] = (byte)b;
                targ[i + 1] = (byte)g;
                targ[i + 2] = (byte)r;
                if (image1.BytesPerPixel > 3)
                    targ[i + 3] = image1.Array[i + 3];
            }

            return targ;
        }

        public static byte[] Colorize(ImageData image, bool isWhiteColorized, bool colorizeColored = false)
        {
            if (!image.IsImadedataValid())
                return image.Array;

            double c0l_h, col_l, col_s;
            //RgbToHls(image.AddedColor.R, image.AddedColor.G, image.AddedColor.B, out ch, out cl, out cs);
            ColorHelper.RgbToHls(image.AddedColor.R, image.AddedColor.G, image.AddedColor.B, out c0l_h, out col_l, out col_s);

            int length = image.Array.Length;
            int bytesPerPixel = image.BytesPerPixel;
            byte[] targ = new byte[length];
            for (int i = 0; i < length; i += bytesPerPixel)
            {
                int b1 = image.Array[i];
                int g1 = image.Array[i + 1];
                int r1 = image.Array[i + 2];

                bool isGray = r1 == g1 && g1 == b1;
                if (!isGray && !colorizeColored)
                {
                    targ[i] = image.Array[i];
                    targ[i + 1] = image.Array[i + 1];
                    targ[i + 2] = image.Array[i + 2];
                    if (image.BytesPerPixel > 3)
                        targ[i + 3] = image.Array[i + 3];
                    continue;
                }

                double h1, l1, s1;

                l1 = Math.Min(r1 / 255.0 * 1.2, 1);
                s1 = col_s * l1;

                if (isWhiteColorized)
                {
                    l1 = col_l * l1 * 1.2;
                }

                int r, g, b;

                //HlsToRgb(ch, l1, s1, out r, out g, out b);
                ColorHelper.HlsToRgb(c0l_h, l1, s1, out r, out g, out b);

                //targ[i] = (byte)GetGammaCorrectedValue(b); //Makes brighter

                targ[i] = (byte)b;
                targ[i + 1] = (byte)g;
                targ[i + 2] = (byte)r;
                if (image.BytesPerPixel > 3)
                    targ[i + 3] = image.Array[i + 3];
            }

            return targ;
        }

        public static byte[] ColorizeHsv(ImageData image, bool isWhiteColorized, bool colorizeColored = false)
        {
            if (!image.IsImadedataValid())
                return image.Array;

            double col_h, col_v, col_s;
            ColorHelper.RgbToHvs(image.AddedColor.R, image.AddedColor.G, image.AddedColor.B, out col_h, out col_v, out col_s);

            int length = image.Array.Length;
            int bytesPerPixel = image.BytesPerPixel;
            byte[] targ = new byte[length];
            for (int i = 0; i < length; i += bytesPerPixel)
            {
                int b1 = image.Array[i];
                int g1 = image.Array[i + 1];
                int r1 = image.Array[i + 2];

                bool isGray = r1 == g1 && g1 == b1;
                if (!isGray && !colorizeColored)
                {
                    targ[i] = image.Array[i];
                    targ[i + 1] = image.Array[i + 1];
                    targ[i + 2] = image.Array[i + 2];
                    if (image.BytesPerPixel > 3)
                        targ[i + 3] = image.Array[i + 3];
                    continue;
                }

                double v = Math.Min(r1 / 255.0, 1); //Source is gray, so for value is enough one: r or b or g
                double s = col_s * 0.8;
                double valThreshold = 0.6;
                if (!isWhiteColorized && v > valThreshold)
                {
                    //s = s * (1 - Math.Sqrt(1 - (1 - v) * (1 - v)));
                    s = s * ((1 - v) / (1 - valThreshold));
                }

                int r, g, b;
                ColorHelper.HvsToRgb(col_h, v, s, out r, out g, out b);

                //targ[i] = (byte)GetGammaCorrectedValue(b, 1.5); //Makes brighter
                //targ[i + 1] = (byte)GetGammaCorrectedValue(g, 1.5);
                //targ[i + 2] = (byte)GetGammaCorrectedValue(r, 1.5);

                targ[i] = (byte)b;
                targ[i + 1] = (byte)g;
                targ[i + 2] = (byte)r;
                if (image.BytesPerPixel > 3)
                    targ[i + 3] = image.Array[i + 3];
            }

            return targ;

        }

        /// <summary>
        /// Gamma gives more brightness in dark part of image (1 - nothing, higher gamma - brighter image)
        /// Very slow
        /// </summary>
        /// <param name="gamma"></param>
        private static void CreateGammaCorrectedValues(double gamma)
        {
            GammaCorrectedValues = new Dictionary<int, int>(256);
            for (int i = 0; i < 256; i++)
            {
                var res = (int)((255.0 * Math.Pow(i / 255.0, 1 / gamma)) + 0.5);
                res = Math.Min(Math.Max(0, res), 255);
                GammaCorrectedValues[i] = res;
            }
        }

        private static void CreateGammaCorrectedValues1(double gamma)
        {
            GammaCorrectedValues1 = new Dictionary<int, int>(100);
            for (int i = 0; i < 100; i++)
            {
                var res = (int)((Math.Pow(i / 100, 1 / gamma)) + 0.005);
                res = Math.Min(Math.Max(0, res), 1);
                GammaCorrectedValues[i] = res;
            }
        }


        private static int GetGammaCorrectedValue(int val, double gamma = 1.2)
        {
            if (GammaCorrectedValues == null)
                CreateGammaCorrectedValues(gamma);


            val = Math.Min(Math.Max(0, val), 255);
            return GammaCorrectedValues[val];
        }

        private static int GetGammaCorrectedValue1(int val, double gamma = 1.2)
        {
            if (GammaCorrectedValues1 == null)
                CreateGammaCorrectedValues1(gamma);


            val = Math.Min(Math.Max(0, val), 1);
            return GammaCorrectedValues[val];
        }

        public static byte[] ColorizeRgb(ImageData image, bool isWhiteColorized, bool colorizeColored = false)
        {
            if (!image.IsImadedataValid())
                return image.Array;

            int length = image.Array.Length;
            int bytesPerPixel = image.BytesPerPixel;

            float col1_r = image.AddedColor.R / 255f;
            float col1_g = image.AddedColor.G / 255f;
            float col1_b = image.AddedColor.B / 255f;

            byte[] targ = new byte[length];
            for (int i = 0; i < length; i += bytesPerPixel)
            {
                int b1 = image.Array[i];
                int g1 = image.Array[i + 1];
                int r1 = image.Array[i + 2];

                bool isGray = r1 == g1 && g1 == b1;
                if (!isGray && !colorizeColored)
                {
                    targ[i] = image.Array[i];
                    targ[i + 1] = image.Array[i + 1];
                    targ[i + 2] = image.Array[i + 2];
                    if (image.BytesPerPixel > 3)
                        targ[i + 3] = image.Array[i + 3];
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
                if (image.BytesPerPixel > 3)
                    targ[i + 3] = image.Array[i + 3];

                //77ms - Using Color.Multiply
                //Color n = Color.Multiply(image.AddedColor, gr); 
                //targ[i] = n.B;
                //targ[i + 1] = n.G;
                //targ[i + 2] = n.R;
                //if (image.BytesPerPixel > 3)
                //    targ[i + 3] = image.Array[i + 3];
            }

            return targ;
        }


        public static byte[] SetFusionRgb(ImageData image1, ImageData image2, double coeff)
        {
            if (!image1.IsImadedataValid() || !image2.IsImadedataValid() || image1.Array.Length != image2.Array.Length)
                return image1.Array;

            int length = image1.Array.Length;
            int bytesPerPixel = image1.BytesPerPixel;

            //float col1_r = image1.AddedColor.R / 255f;
            //float col1_g = image1.AddedColor.G / 255f;
            //float col1_b = image1.AddedColor.B / 255f;
            float col1_r = (255 - image2.AddedColor.R) / 255f;
            float col1_g = (255 - image2.AddedColor.G) / 255f;
            float col1_b = (255 - image2.AddedColor.B) / 255f;

            float col2_r = image2.AddedColor.R / 255f;
            float col2_g = image2.AddedColor.G / 255f;
            float col2_b = image2.AddedColor.B / 255f;
            //float col2_r = (255 - image1.AddedColor.R) / 255f;
            //float col2_g = (255 - image1.AddedColor.G) / 255f;
            //float col2_b = (255 - image1.AddedColor.B) / 255f;


            byte[] targ = new byte[length];
            for (int i = 0; i < length; i += bytesPerPixel)
            {
                float gr1 = image1.Array[i] / 255f;
                gr1 = NormalizeEdges(gr1);

                float gr2 = image2.Array[i] / 255f;
                gr2 = NormalizeEdges(gr2);

                int r, g, b;

                //--- regular a blending ----
                
                //img = V1 * amber * (1 - alpha) + V2 * blue * alpha    
                //r = Math.Min((int)((gr1 * col1_r * (1 - coeff) + gr2 * col2_r * coeff) * 255f + 0.5), 255);
                //g = Math.Min((int)((gr1 * col1_g * (1 - coeff) + gr2 * col2_g * coeff) * 255f + 0.5), 255);
                //b = Math.Min((int)((gr1 * col1_b * (1 - coeff) + gr2 * col2_b * coeff) * 255f + 0.5), 255);
                
                //--- end of regular a blending ----

                //--- enhanced a blending ---

                float coeff_a = Math.Min((2f - 2f * (float)coeff), 1f);
                float coeff_b = Math.Min((2f * (float)coeff), 1f);

                //img = np.clip(amber*(V1*a+V2*(1-a))+blue*(V2*b+V1*(1-b)), 0.0, 1.0)
                r = (int)((col1_r * (gr1 * coeff_a ) + col2_r * (gr2 * coeff_b )) * 255 + 0.5);
                g = (int)((col1_g * (gr1 * coeff_a) + col2_g * (gr2 * coeff_b)) * 255 + 0.5);
                b = (int)((col1_b * (gr1 * coeff_a) + col2_b * (gr2 * coeff_b )) * 255 + 0.5);
                
                //--- end of enhanced a blending ---


                targ[i] = (byte)b;
                targ[i + 1] = (byte)g;
                targ[i + 2] = (byte)r;
                if (image1.BytesPerPixel > 3)
                    targ[i + 3] = image1.Array[i + 3];
            }

            return targ;
        }

        public static byte[] SetFusionRgb_2(ImageData image1, ImageData image2, double coeff)
        {
            if (!image1.IsImadedataValid() || !image2.IsImadedataValid() || image1.Array.Length != image2.Array.Length)
                return image1.Array;

            int length = image1.Array.Length;
            int bytesPerPixel = image1.BytesPerPixel;

            //float col1_r = image1.AddedColor.R / 255f;
            //float col1_g = image1.AddedColor.G / 255f;
            //float col1_b = image1.AddedColor.B / 255f;
            float col1_r = (255 - image2.AddedColor.R) / 255f;
            float col1_g = (255 - image2.AddedColor.G) / 255f;
            float col1_b = (255 - image2.AddedColor.B) / 255f;

            float col2_r = image2.AddedColor.R / 255f;
            float col2_g = image2.AddedColor.G / 255f;
            float col2_b = image2.AddedColor.B / 255f;
            //float col2_r = (255 - image1.AddedColor.R) / 255f;
            //float col2_g = (255 - image1.AddedColor.G) / 255f;
            //float col2_b = (255 - image1.AddedColor.B) / 255f;

            byte[] targ = new byte[length];
            for (int i = 0; i < length; i += bytesPerPixel)
            {
                float gr1 = image1.Array[i] / 255f;
                //gr1 = GetDoubleInRange(gr1);

                float gr2 = image2.Array[i] / 255f;
                //gr2 = GetDoubleInRange(gr2);

                float coeff_a = Math.Min((2f - 2f * (float)coeff), 1f);
                float coeff_b = Math.Min((2f * (float)coeff), 1f);

                int r, g, b;

                //img = np.clip(amber*(V1*a+V2*(1-a))+blue*(V2*b+V1*(1-b)), 0.0, 1.0)
                r = (int)((col1_r * (gr1 * coeff_a + gr2 * (1f - coeff_a)) + col2_r * (gr2*coeff_b + gr1*(1 - coeff_b))) * 255 + 0.5);
                g = (int)((col1_g * (gr1 * coeff_a + gr2 * (1f - coeff_a)) + col2_g * (gr2 * coeff_b + gr1 * (1 - coeff_b))) * 255 + 0.5);
                b = (int)((col1_b * (gr1 * coeff_a + gr2 * (1f - coeff_a)) + col2_b * (gr2 * coeff_b + gr1 * (1 - coeff_b))) * 255 + 0.5);

                r = Math.Max(Math.Min(r, 255), 0);
                g = Math.Max(Math.Min(g, 255), 0);
                b = Math.Max(Math.Min(b, 255), 0);

                targ[i] = (byte)b;
                targ[i + 1] = (byte)g;
                targ[i + 2] = (byte)r;
                if (image1.BytesPerPixel > 3)
                    targ[i + 3] = image1.Array[i + 3];
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

        private static void CreatedBrightnessCorrectedValues()
        {
            if (BrightnessCorrectedValues != null)
                return;

            BrightnessCorrectedValues = new Dictionary<float, float>(256);
            for (int i=0; i<256; ++i)
            {
                float val = i / 255f;
                BrightnessCorrectedValues[val] = val <= minColorValue ? 0f : val>= maxColorValue? 1f: (val - minColorValue) / (maxColorValue - minColorValue);
            }
        }
    }
}
