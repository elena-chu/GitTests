using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ws.Fus.UI.Navigation.Wpf;

namespace Ws.Fus.Surgical.UI.Wpf
{
    public class SurgicalModuleController : ModuleControllerBase
    {
        private bool _isTargetApproved = true; //Temp true;

        public SurgicalModuleController(NavigationController navController)
            : base(navController)
        {
            ApplicationModule = ApplicationModule.Surgical;

        }

        protected override bool SwitchModuleSpecificCanExecute()
        {
            return _isTargetApproved;
        }

        protected override void SwitchModuleExecute()
        {
            if (!SwitchModuleCanExecute() || IsActive)
                return;
            
            Dictionary<string, object> navParam = new Dictionary<string, object>();
            navParam.Add(SurgicalNavigationConsts.LastSurgicalModeParam, SurgicalMode.Definition);
            
            _navigationController.SwitchModule(ApplicationModule, navParam);
            Application.Current.Dispatcher.BeginInvoke((Action)(() => RaisePropertyChanged(nameof(IsActive))));
        }


        private void OnTargetApprovedChanged(object sender, bool isApproved)
        {
            _isTargetApproved = isApproved;
            SwitchModuleCommand.RaiseCanExecuteChanged();
        }



    }
}
