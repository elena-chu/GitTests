using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLoad
{
    public class ImageHelper
    {
        public static bool AreByteArraysEqual(byte[] array1, byte[] array2)
        {
            if(array1 == null || array2 == null) 
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
            Debug.WriteLine($"Compare_loop duration={(afterLoop -afterEnumer).TotalMilliseconds}mc");
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
            if(array == null || array.Length <= bytesPerPixel)
            {
                return array;
            }

            DateTime startTime = DateTime.Now;
            byte[] targ = new byte[array.Length];
            for(int i=0; i< array.Length; i+= bytesPerPixel)
            {
                targ[i] = array[i];
                targ[i+1] = array[i+1];
                targ[i+2] = array[i+2];
                if((array[i] & array[i + 1] & array[i + 2]) == array[i])
                {
                    targ[i + 3] = (byte)(255 -array[i]);
                }
                else
                {
                    targ[i + 3] = array[i+3];
                }
            }
            DateTime endTime = DateTime.Now;
            Debug.WriteLine($"Alfa duration={(endTime - startTime).TotalMilliseconds}ms");
            return targ;
        }

    }
}
