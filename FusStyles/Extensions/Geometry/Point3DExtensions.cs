using System;
using System.Collections.Generic;
using System.Linq;
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
    }

}
