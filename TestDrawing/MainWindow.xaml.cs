using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestDrawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //BuildMatrix();
        }

        private void BuildClick(object sender, RoutedEventArgs e)
        {
            BuildParameters initParams = CreateParams();
            MainOrientation.Build(initParams);
        }

        private void BuildSecondClick(object sender, RoutedEventArgs e)
        {
            BuildParameters initParams = CreateParams();
            SecondaryOrientation.Build(initParams);
        }

        private BuildParameters CreateParams()
        {
            BuildParameters ret = new BuildParameters()
            {
                CenterCoord = new Point(180, 150),
                FOV = 200d,
                From = 40d,
                To = 30d,
                MinFrom = 10d,
                MaxFrom = 100d,
                MinTo = 10d,
                MaxTo = 100d
            };

            return ret;
        }

        private void BuildMatrix()
        {

            int ang1 = 15;
            int ang2 = 15;
            Point3D p1 = new Point3D(100, 100, 100);
            Point3D p2 = new Point3D(200, 200, 200);

            Matrix3D final = new Matrix3D();
            Matrix3D rot1 = new Matrix3D();
            Quaternion quant1 = new Quaternion(new Vector3D(0, 0, 1), ang1);
            rot1.RotateAt(quant1, p1);
            final *= rot1;
            Matrix3D rot2 = new Matrix3D();
            Quaternion quant2 = new Quaternion(new Vector3D(0, 0, 1), ang2);
            rot2.RotateAt(quant2, p2);
            final *= rot2;
            Debug.WriteLine($"Rotation {ang1} + {ang2}° around Point({p1}) and then Point({p2})");
            Debug.WriteLine($"{final}");
            Debug.WriteLine(final.ToColumnsString());
            Debug.WriteLine(final.ToRowsString());
            Matrix3D rot3 = new Matrix3D();
            Quaternion quant3 = new Quaternion(new Vector3D(0, 0, 1), ang1 + ang2);
            rot3.RotateAt(quant3, p2);
            Debug.WriteLine($"Rotation {ang1 + ang2}° around Point({p2})");
            Debug.WriteLine($"{rot3}");
        }
    }
}
