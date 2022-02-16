using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ws.Fus.PlanningScans.UI.Wpf.Controls.ScanBox
{
    /// <summary>
    /// Interaction logic for DragEdge.xaml
    /// </summary>
    public partial class DragEdge : UserControl
    {
        private SolidColorBrush _fromBrush = new SolidColorBrush(Colors.Coral);

        public DragEdge()
        {
            InitializeComponent();

        }
        
        private bool _isFrom;
        public bool IsFrom
        {
            get { return _isFrom; }
            set
            {
                _isFrom = value;
                Dragger.Tag = value;
                if (value)
                {
                    ScaleTransform.ScaleX = -1;

                    DragArrow.Fill = _fromBrush;
                    DragArrow.Stroke = _fromBrush;
                    DragLine.Stroke = _fromBrush;

                }
            }
        }

    }
}
