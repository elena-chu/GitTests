using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Fus.UI.Navigation.Wpf;

namespace WpfUI.Controllers
{
    public class PlanningModuleController : ModuleControllerBase
    {
        public PlanningModuleController(NavigationController navController) 
            : base(navController)
        {
            ApplicationModule = ApplicationModule.Planning;
        }
    }
}
