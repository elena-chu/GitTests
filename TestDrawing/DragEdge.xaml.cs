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

namespace TestDrawing
{
    /// <summary>
    /// Interaction logic for DragEdge.xaml
    /// </summary>
    public partial class DragEdge : UserControl
    {
        public DragEdge()
        {
            InitializeComponent();
        }

        private bool _isTo;
        public bool IsTo
        {
            get { return _isTo; }
            set
            {
                _isTo = value;
                Dragger.Tag = value;
                if (value)
                {
                    ScaleTransform.ScaleX = -1;
                }
            }
        }
    }
}
