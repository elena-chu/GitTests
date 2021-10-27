using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Ws.Extensions.Geometry;

namespace Ws.Dicom
{
    public class ImageGeometry
    {
        public virtual int WidthPix { get; protected set; }
        public virtual int HeightPix { get; protected set; }
        public virtual Point3D PointTopLeft { get; protected set; }
        public virtual Vector3D DirectionRow { get; protected set; }
        public virtual Vector3D DirectionColumn { get; protected set; }
        public virtual double PixelSpacingX { get; protected set; }
        public virtual double PixelSpacingY { get; protected set; }

        public ImageGeometry(double[] imagePatientPosition, double[] imagePatientOrientation, double[] pixelSpacing)
        {
            PointTopLeft = Point3DExtensions.Create(imagePatientPosition);
            DirectionRow = UnitVector3DExtensions.Create(imagePatientOrientation);
            DirectionColumn = UnitVector3DExtensions.Create(imagePatientOrientation, 3);
            PixelSpacingX = pixelSpacing[0];
            PixelSpacingY = pixelSpacing[1];
        }

        protected ImageGeometry()
        {
        }
    }
}
