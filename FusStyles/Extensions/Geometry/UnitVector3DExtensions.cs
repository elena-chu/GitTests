using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Ws.Extensions.Geometry
{
    public static class UnitVector3DExtensions
    {
        public static Vector3D Create(double[] v, int start = 0) => new Vector3D(v[start], v[start + 1], v[start + 2]);
    }
}
