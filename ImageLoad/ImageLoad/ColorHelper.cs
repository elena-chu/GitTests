using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLoad
{
    public class ColorHelper
    {
        public static void RgbToHls(int r, int g, int b,
            out double h, out double l, out double s)
        {
            float r1 = (r / 255.0f);
            float g1 = (g / 255.0f);
            float b1 = (b / 255.0f);

            float min = Math.Min(Math.Min(r1, g1), b1);
            float max = Math.Max(Math.Max(r1, g1), b1);
            float delta = max - min;

            l = (max + min) / 2;

            if (delta == 0)
            {
                h = 0;
                s = 0;
            }
            else
            {
                s = (l <= 0.5) ? (delta / (max + min)) : (delta / (2 - max - min));

                if (r1 == max)
                    h = ((g1 - b1) / 6) / delta;
                else if (g1 == max)
                    h = (1.0f / 3) + ((b1 - r1) / 6) / delta;
                else
                    h = (2.0f / 3) + ((r1 - g1) / 6) / delta;

                if (h < 0)
                    h += 1;
                if (h > 1)
                    h -= 1;

                h = (int)(h * 360);
            }

        }

        public static void RgbToHvs(int r, int g, int b,
            out double h, out double v, out double s)
        {
            float r1 = (r / 255.0f);
            float g1 = (g / 255.0f);
            float b1 = (b / 255.0f);

            float delta, min, max;

            min = Math.Min(Math.Min(r1, g1), b1);
            max = Math.Max(Math.Max(r1, g1), b1);
            v = (double)max;
            delta = max - min;
            h = 0;

            if (v == 0.0)
                s = 0;
            else
                s = delta / v;

            if (s == 0)
                h = 0.0;
            else
            {
                if (r1 == max)
                    h = ((g1 - b1) / 6) / delta;
                else if (g1 == max)
                    h = (1.0f / 3) + ((b1 - r1) / 6) / delta;
                else
                    h = (2.0f / 3) + ((r1 - g1) / 6) / delta;

                if (h < 0)
                    h += 1;
                if (h > 1)
                    h -= 1;

                h = (int)(h * 360);
            }
        }

        //public static void RgbToHvs(int r, int g, int b,
        //    out double h, out double v, out double s)
        //{
        //    double delta, min;

        //    min = Math.Min(Math.Min(r, g), b);
        //    v = Math.Max(Math.Max(r, g), b);
        //    delta = v - min;
        //    h = 0;

        //    if (v == 0.0)
        //        s = 0;
        //    else
        //        s = delta / v;

        //    if (s == 0)
        //        h = 0.0;
        //    else
        //    {
        //        if (r == v)
        //            h = (double)((r - b) / delta);
        //        else if (g == v)
        //            h = 2.0 + (double)((b - r) / delta);
        //        else if (b == v)
        //            h = 4.0 + (double)((r - g) / delta);

        //        h *= 60;

        //        if (h < 0.0)
        //            h = h + 360.0;
        //    }
        //    v = v / 255.0;
        //}

        public static void HlsToRgb(double h, double l, double s,
            out int r, out int g, out int b)
        {
            if (s == 0)
            {
                r = g = b = (byte)(l * 255);
            }
            else
            {
                double v1, v2;
                double hue = h / 360;

                v2 = (l < 0.5) ? (l * (1 + s)) : ((l + s) - (l * s));
                v1 = 2 * l - v2;

                r = (byte)(255 * HueToRGB(v1, v2, hue + (1.0f / 3)));
                g = (byte)(255 * HueToRGB(v1, v2, hue));
                b = (byte)(255 * HueToRGB(v1, v2, hue - (1.0f / 3)));
            }
        }


        public static void HvsToRgb(double h, double v, double s,
            out int r, out int g, out int b)
        {
            double r1 = 0, g1 = 0, b1 = 0;

            if (s == 0)
            {
                r1 = v;
                g1 = v;
                b1 = v;
            }
            else
            {
                int i;
                double f, p, q, t;

                if (h == 360)
                    h = 0;
                else
                    h = h / 60;

                i = (int)Math.Truncate(h);
                f = h - i;

                p = v * (1.0 - s);
                q = v * (1.0 - (s * f));
                t = v * (1.0 - (s * (1.0 - f)));

                switch (i)
                {
                    case 0:
                        r1 = v;
                        g1 = t;
                        b1 = p;
                        break;
                    case 1:
                        r1 = q;
                        g1 = v;
                        b1 = p;
                        break;
                    case 2:
                        r1 = p;
                        g1 = v;
                        b1 = t;
                        break;
                    case 3:
                        r1 = p;
                        g1 = q;
                        b1 = v;
                        break;
                    case 4:
                        r1 = t;
                        g1 = p;
                        b1 = v;
                        break;
                    default:
                        r1 = v;
                        g1 = p;
                        b1 = q;
                        break;
                }
            }

            r = (int)(r1 * 255.0);
            g = (int)(g1 * 255.0);
            b = (int)(b1 * 255.0);
        }

        private static double HueToRGB(double v1, double v2, double vH)
        {
            if (vH < 0)
                vH += 1;

            if (vH > 1)
                vH -= 1;

            if ((6 * vH) < 1)
                return (v1 + (v2 - v1) * 6 * vH);

            if ((2 * vH) < 1)
                return v2;

            if ((3 * vH) < 2)
                return (v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6);

            return v1;
        }
    }
}
