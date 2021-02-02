using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Ws.Extensions.Geometry
{
    public static class Matrix3DExtension
    {
        public static string ToColumnsString(this Matrix3D matrix)
        {
            List<List<double>> columns = new List<List<double>>(4);
            List<double> col1 = new List<double>(4) { matrix.M11, matrix.M12, matrix.M13, matrix.M14 };
            columns.Add(col1);
            List<double> col2 = new List<double>(4) { matrix.M21, matrix.M22, matrix.M23, matrix.M24 };
            columns.Add(col2);
            List<double> col3 = new List<double>(4) { matrix.M31, matrix.M32, matrix.M33, matrix.M34 };
            columns.Add(col3);
            List<double> col4 = new List<double>(4) { matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ, 1d };
            columns.Add(col4);

            return ToFormattedString(columns, true);
        }

        public static string ToRowsString(this Matrix3D matrix)
        {
            List<List<double>> columns = new List<List<double>>(4);
            List<double> col1 = new List<double>(4) { matrix.M11, matrix.M21, matrix.M31, matrix.OffsetX };
            columns.Add(col1);
            List<double> col2 = new List<double>(4) { matrix.M12, matrix.M22, matrix.M32, matrix.OffsetY };
            columns.Add(col2);
            List<double> col3 = new List<double>(4) { matrix.M13, matrix.M23, matrix.M33, matrix.OffsetZ };
            columns.Add(col3);
            List<double> col4 = new List<double>(4) { matrix.M14, matrix.M24, matrix.M34, 1d };
            columns.Add(col4);

            return ToFormattedString(columns, false);
        }

        public static bool IsEqualWithDelta(this Matrix3D matrix1, Matrix3D matrix2, double delta = GeometryConsts.COMPARISION_DELTA)
        {
            if (matrix1.IsIdentity && matrix2.IsIdentity)
                return true;

            delta = Math.Abs(delta);
            return Math.Abs(matrix1.M11 - matrix2.M11) <= delta &&
                   Math.Abs(matrix1.M12 - matrix2.M12) <= delta &&
                   Math.Abs(matrix1.M13 - matrix2.M13) <= delta &&
                   Math.Abs(matrix1.M14 - matrix2.M14) <= delta &&
                   Math.Abs(matrix1.M21 - matrix2.M21) <= delta &&
                   Math.Abs(matrix1.M22 - matrix2.M22) <= delta &&
                   Math.Abs(matrix1.M23 - matrix2.M23) <= delta &&
                   Math.Abs(matrix1.M24 - matrix2.M24) <= delta &&
                   Math.Abs(matrix1.M31 - matrix2.M31) <= delta &&
                   Math.Abs(matrix1.M32 - matrix2.M32) <= delta &&
                   Math.Abs(matrix1.M33 - matrix2.M33) <= delta &&
                   Math.Abs(matrix1.M34 - matrix2.M34) <= delta &&
                   Math.Abs(matrix1.OffsetX - matrix2.OffsetX) <= delta &&
                   Math.Abs(matrix1.OffsetY - matrix2.OffsetY) <= delta &&
                   Math.Abs(matrix1.OffsetZ - matrix2.OffsetZ) <= delta &&
                   Math.Abs(matrix1.M44 - matrix2.M44) <= delta;
        }

        public static bool IsEqualRotationWithDelta(this Matrix3D matrix1, Matrix3D matrix2, double delta = GeometryConsts.COMPARISION_DELTA)
        {
            if (matrix1.IsIdentity && matrix2.IsIdentity)
                return true;

            delta = Math.Abs(delta);
            return Math.Abs(matrix1.M11 - matrix2.M11) <= delta &&
                   Math.Abs(matrix1.M12 - matrix2.M12) <= delta &&
                   Math.Abs(matrix1.M13 - matrix2.M13) <= delta &&
                   Math.Abs(matrix1.M14 - matrix2.M14) <= delta &&
                   Math.Abs(matrix1.M21 - matrix2.M21) <= delta &&
                   Math.Abs(matrix1.M22 - matrix2.M22) <= delta &&
                   Math.Abs(matrix1.M23 - matrix2.M23) <= delta &&
                   Math.Abs(matrix1.M24 - matrix2.M24) <= delta &&
                   Math.Abs(matrix1.M31 - matrix2.M31) <= delta &&
                   Math.Abs(matrix1.M32 - matrix2.M32) <= delta &&
                   Math.Abs(matrix1.M33 - matrix2.M33) <= delta &&
                   Math.Abs(matrix1.M34 - matrix2.M34) <= delta;
        }

        public static bool IsEqualTranslationWithDelta(this Matrix3D matrix1, Matrix3D matrix2, double delta = GeometryConsts.COMPARISION_DELTA)
        {
            if (matrix1.IsIdentity && matrix2.IsIdentity)
                return true;

            delta = Math.Abs(delta);
            return Math.Abs(matrix1.OffsetX - matrix2.OffsetX) <= delta &&
                   Math.Abs(matrix1.OffsetY - matrix2.OffsetY) <= delta &&
                   Math.Abs(matrix1.OffsetZ - matrix2.OffsetZ) <= delta &&
                   Math.Abs(matrix1.M44 - matrix2.M44) <= delta;
        }

        private static string ToFormattedString(List<List<double>> columns, bool isColumn = false)
        {
            string numFormat = ":0.0#####";
            List<string> names = new List<string>(4) { "X", "Y", "Z", "Offs" };
            List<string> colFormats = new List<string>(4);
            for (int i = 0; i < 4; i++)
            {
                int len = columns[i].Max(el => el.ToString("0" + numFormat).Length);
                string format = "{0, " + (len + 2) + numFormat + "}";
                colFormats.Add(format);
            }

            StringBuilder sb = new StringBuilder();
            if (isColumn)
            {
                for (int i = 0; i < 4; i++)
                    sb.AppendFormat(colFormats[i], names[i]);

                sb.AppendLine();
            }
            for (int i = 0; i < 4; i++)
            {
                if (!isColumn)
                    sb.AppendFormat("{0, -4}", names[i]);

                for (int j = 0; j < 4; j++)
                    sb.AppendFormat(colFormats[j], columns[j][i]);

                if (i < 3)
                    sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
