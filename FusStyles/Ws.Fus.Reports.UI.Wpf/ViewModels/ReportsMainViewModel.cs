using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Extensions.Mvvm;
using Ws.Fus.UI.Navigation.Wpf;

namespace Ws.Fus.Reports.UI.Wpf
{
    public class ReportsMainViewModel : ModuleMainViewModel
    {
        public ReportsMainViewModel(/*IDispatcher dispatcher,*/ 
            NavigationController navigationController, 
            IEnumerable<IModuleController> moduleControllers) 
            : base(/*dispatcher,*/ navigationController, moduleControllers)
        {
        }

        public override ApplicationModule ApplicationModule => ApplicationModule.Reports;
    }
}
