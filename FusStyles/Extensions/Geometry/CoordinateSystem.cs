using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Ws.Extensions.Geometry
{
    public class CoordinateSystem
    {
        /// <summary>
        /// Create default(identity) Coordinate System
        /// </summary>
        public CoordinateSystem() : this(new Vector3D(1,0,0), new Vector3D(0, 1, 0), new Vector3D(0, 0, 1))
        { }

        /// <summary>
        /// Create Coordinate System by 3 Axes
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <param name="zAxis"></param>
        public CoordinateSystem(Vector3D xAxis, Vector3D yAxis, Vector3D zAxis) : this(xAxis, yAxis, zAxis, new Point3D())
        { }

        /// <summary>
        /// Create Coordinate System by 3 Axes and center
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <param name="zAxis"></param>
        /// <param name="center"></param>
        public CoordinateSystem(Vector3D xAxis, Vector3D yAxis, Vector3D zAxis, Point3D center)
        {
            xAxis.Normalize();
            yAxis.Normalize();
            zAxis.Normalize();
            Center = center;

            TransformationMatrix = new Matrix3D()
            {
                M11 = xAxis.X,
                M12 = xAxis.Y,
                M13 = xAxis.Z,
                M14 = 0,

                M21 = yAxis.X,
                M22 = yAxis.Y,
                M23 = yAxis.Z,
                M24 = 0,

                M31 = zAxis.X,
                M32 = zAxis.Y,
                M33 = zAxis.Z,
                M34 = 0,

                OffsetX = 0,
                OffsetY = 0,
                OffsetZ = 0,
                M44 = 1
            };
        }

        /// <summary>
        /// Create Coordinate System by transformation matrix and center
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="center"></param>
        public CoordinateSystem(Matrix3D matrix, Point3D center)
        {
            TransformationMatrix = matrix;
            Center = center;
        }

        public Point3D Center { get; private set; }

        public Matrix3D TransformationMatrix { get; private set; }

        public Vector3D XAxis
        {
            get { return new Vector3D(TransformationMatrix.M11, TransformationMatrix.M12, TransformationMatrix.M13); }
        }

        public Vector3D YAxis
        {
            get { return new Vector3D(TransformationMatrix.M21, TransformationMatrix.M22, TransformationMatrix.M23); }
        }

        public Vector3D ZAxis
        {
            get { return new Vector3D(TransformationMatrix.M31, TransformationMatrix.M32, TransformationMatrix.M33); }
        }

        /// <summary>
        /// Rotate around any Vector
        /// </summary>
        /// <param name="vector">Vector around to rotate</param>
        /// <param name="angle">An angle of rotation in degrees</param>
        public void RotateAroundVector(Vector3D vector, double angle)
        {
            if (angle == 0)
                return;
            
            // Build the rotation matrix reflecting the current rotation matrix
            Matrix3D rotationMatrix = new Matrix3D();
            Quaternion quant = new Quaternion(vector, angle);
            rotationMatrix.RotateAt(quant, Center);

            // Update the reformat transformation matrix
            TransformationMatrix *= rotationMatrix;
        }

        /// <summary>
        /// Rotate about X axis
        /// </summary>
        /// <param name="angle">An angle of rotation in degrees</param>
        public void Roll(double angle)
        {
            RotateAroundVector(XAxis, angle);
        }

        /// <summary>
        /// Rotate about Y axis
        /// </summary>
        /// <param name="angle">An angle of rotation in degrees</param>
        public void Pitch(double angle)
        {
            RotateAroundVector(YAxis, angle);
        }

        /// <summary>
        /// Rotate about Z axis
        /// </summary>
        /// <param name="angle">An angle of rotation in degrees</param>
        public void Yaw(double angle)
        {
            RotateAroundVector(ZAxis, angle);
        }

        /// <summary>
        /// Update Center
        /// </summary>
        /// <param name="center"></param>
        public void UpdateCenter(Point3D center)
        {
            Center = center;
        }

        /// <summary>
        /// Compares two Coordinate Systems
        /// </summary>
        /// <param name="other">Other Coordinate System for comparison</param>
        /// <param name="delta">delta for comparison</param>
        /// <returns></returns>
        public bool IsEqual(CoordinateSystem other, double delta = GeometryConsts.COMPARISION_DELTA)
        {
            bool isEqual = TransformationMatrix.IsEqualWithDelta(other.TransformationMatrix, delta);
            if(isEqual)
                isEqual = isEqual && Center.IsEqualWithDelta(other.Center, delta);

            return isEqual;
        }

        /// <summary>
        /// Compares rotation component of two Coordinate Systems
        /// </summary>
        /// <param name="other">Other Coordinate System for comparison</param>
        /// <param name="delta">delta for comparison</param>
        /// <returns></returns>
        public bool IsEqualRotation(CoordinateSystem other, double delta = GeometryConsts.COMPARISION_DELTA)
        {
            return TransformationMatrix.IsEqualRotationWithDelta(other.TransformationMatrix, delta);
        }

        /// <summary>
        /// Compares translation component of two Coordinate Systems
        /// </summary>
        /// <param name="other">Other Coordinate System for comparison</param>
        /// <param name="delta">delta for comparison</param>
        /// <returns></returns>
        public bool IsEqualTranslation(CoordinateSystem other, double delta = GeometryConsts.COMPARISION_DELTA)
        {
            return TransformationMatrix.IsEqualTranslationWithDelta(other.TransformationMatrix, delta);
        }

        /// <summary>
        /// Compares Centers of two Coordinate Systems
        /// </summary>
        /// <param name="other">Other Coordinate System for comparison</param>
        /// <param name="delta">delta for comparison</param>
        /// <returns></returns>
        public bool IsEqualCenter(CoordinateSystem other, double delta = GeometryConsts.COMPARISION_DELTA)
        {
            return Center.IsEqualWithDelta(other.Center, delta);
        }

    }
}
