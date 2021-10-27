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
    /// Interaction logic for SurgicalProgressView.xaml
    /// </summary>
    public partial class SurgicalProgressView : UserControl
    {
        public SurgicalProgressView()
        {
            InitializeComponent();
        }

        private void OnTxtMouseUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            SurgicalMainViewModel vm = this.DataContext as SurgicalMainViewModel;
            if (tb != null && vm!= null)
            {
                if (tb == DefineTxt)
                    vm.ChangeSurgicalStageCommand.Execute(SurgicalMode.Definition);
                else if(tb == SonicateTxt)
                    vm.ChangeSurgicalStageCommand.Execute(SurgicalMode.Sonication);
                else if(tb == ReviewTxt)
                    vm.ChangeSurgicalStageCommand.Execute(SurgicalMode.Dosimetry);
            }
        }
    }
}
