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

namespace Ws.Fus.Surgical.UI.Wpf
{
    /// <summary>
    /// Interaction logic for SurgicalMainView.xaml
    /// </summary>
    public partial class SurgicalMainView : UserControl
    {
        public SurgicalMainView()
        {
            InitializeComponent();
        }

        private void OnSurgicalModeContentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SurgicalMainViewModel vm = DataContext as SurgicalMainViewModel;
            if(vm!= null)
            {
                vm.ModeChangedCommand.Execute(e.NewValue);
            }
        }
    }
}
