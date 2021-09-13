using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Fus.UI.Navigation.Wpf;

namespace Ws.Fus.Reports.UI.Wpf
{
    public class ReportsModuleController : ModuleControllerBase
    {
        private bool _isSuccessfulSonicationExist = true;

        public ReportsModuleController(NavigationController navController) 
            : base(navController)
        {
            ApplicationModule = ApplicationModule.Reports;
        }

        protected override bool SwitchModuleSpecificCanExecute()
        {
            return _isSuccessfulSonicationExist;
        }
    }
}
