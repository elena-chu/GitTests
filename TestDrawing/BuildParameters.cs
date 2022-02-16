using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestDrawing
{
    public class BuildParameters
    {
        public Point CenterCoord { get; set; }
        public double FOV { get; set; }
        public double From { get; set; }
        public double To { get; set; }

        public double MinFrom { get; set; }
        public double MaxFrom { get; set; }
        public double MinTo { get; set; }
        public double MaxTo { get; set; }

        public string FromP = "Ax";
        public string ToP = "Cor";

    }

    public class FGEParameters
    {
        //public Point CenterCoord { get; set; }
        //public Point SecondCoord { get; set; }

        public List<Point> Points;
        public int WindowId;
    }
}
