using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ws.Fus.PlanningScans.UI.Wpf.Controls.ScanBox
{
    /// <summary>
    /// Parameters necessary for ScanBox build
    /// </summary>
    public class ScanBoxParams
    {
        public Point Center { get; set; }

        public double From { get; set; }
        public double To { get; set; }
        public double FOV_X { get; set; }
        public double FOV_Y { get; set; }
        public int SliceNumber { get; set; }

        public double MinFrom { get; set; }
        public double MaxFrom { get; set; }
        public double MinTo { get; set; }
        public double MaxTo { get; set; }

        public double MinAnglePerRotation { get; set; }
        public double MaxAnglePerRotation { get; set; }

        public bool KeepRotation { get; set; }

        public bool IsValid()
        {
            return FOV_X > 0 && FOV_Y > 0 && MinFrom > 0 && MinTo > 0 && From > 0 && To > 0;
        }
    }
}
