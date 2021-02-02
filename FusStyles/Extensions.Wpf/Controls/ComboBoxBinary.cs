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

namespace Ws.Extensions.UI.Wpf.Controls
{
    public class ComboBoxBinary : ComboBox
    {
        public ComboBoxBinary()
        {
            SelectedValuePath = nameof(Tag);
        }

        public object FalseContent
        {
            get { return null; }
            set
            {
                AddContent(value, false);
            }
        }

        public object TrueContent
        {
            get { return null; }
            set
            {
                AddContent(value, true);
            }
        }

        private void AddContent(object content, bool tag)
        {
            var item = new ComboBoxItem
            {
                Content = content,
                Tag = tag,
            };

            Items.Add(item);
        }
    }
}
