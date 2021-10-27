using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Ws.Extensions.Geometry
{
    public static class Point3DExtensions
    {
        public static Point3D Create(double[] v, int start = 0) => new Point3D(v[start], v[start + 1], v[start + 2]);

        public static bool IsEqualWithDelta(this Point3D point1, Point3D point2, double delta = GeometryConsts.COMPARISION_DELTA)
        {
            delta = Math.Abs(delta);
            return Math.Abs(point1.X - point2.X) <= delta &&
                Math.Abs(point1.Y - point2.Y) <= delta &&
                Math.Abs(point1.Z - point2.Z) <= delta;
        }

        public static Point3D Rounded(this Point3D point, int precision = 2)
        {
            return new Point3D(Math.Round(point.X, precision), Math.Round(point.Y, precision), Math.Round(point.Z, precision));
        }

        public static void Round(this Point3D point, int precision = 2)
        {
            point.X = Math.Round(point.X, precision);
            point.Y = Math.Round(point.Y, precision);
            point.Z = Math.Round(point.Z, precision);
        }

        public static string AsRasString(this Point3D point, int precision = 1)
        {
            char rl = point.X < 0 ? 'L' : 'R';
            char ap = point.Y < 0 ? 'P' : 'A';
            char si = point.Z < 0 ? 'I' : 'S';

            return $"{rl}{Math.Round(Math.Abs(point.X), precision)} {ap}{Math.Round(Math.Abs(point.Y), precision)} {si}{Math.Round(Math.Abs(point.Z), precision)}";
        }
    }

}
