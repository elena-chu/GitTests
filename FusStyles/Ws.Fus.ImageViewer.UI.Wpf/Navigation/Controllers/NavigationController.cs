using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Ws.Fus.ImageViewer.UI.Wpf.Navigation;

namespace Ws.Fus.ImageViewer.UI.Wpf.Navigation.Controllers
{
    public class NavigationController
    {
        Dictionary<ApplicationModule, string> _moduleScreensRegion = new Dictionary<ApplicationModule, string>()
        {
            {ApplicationModule.MainScreen, RegionNames.MainScreenRegion },
            {ApplicationModule.Planning, RegionNames.PlanningScreensRegion },
            {ApplicationModule.Surgery, RegionNames.SurgeryScreensRegion },
        };

        private readonly IRegionManager _regionManager;

        public NavigationController(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public event EventHandler ModuleChanged;

        public ApplicationModule? CurrentModule { get; private set; }

        public bool SwitchModule(ApplicationModule module, Dictionary<string, object> navParam = null)
        {
            bool succeeded = SwitchScreen(RegionNames.ModuleMainRegion, module.ToString(), navParam);
            if (succeeded)
            {
                CurrentModule = module;
                ModuleChanged?.Invoke(this, EventArgs.Empty);
            }

            return succeeded;
        }

        public bool SwitchScreen<T>(ApplicationModule module, T screen, Dictionary<string, object> navParam = null) where T:struct
        {
            return SwitchScreen(module, screen.ToString(), navParam);
        }

        public bool SwitchScreen(ApplicationModule module, string screenName, Dictionary<string, object> navParam = null)
        {
            bool succeeded;
            if (CurrentModule != null && CurrentModule != module)
            {
                //Initiate module navigation and then enter screen 
                if (navParam == null)
                    navParam = new Dictionary<string, object>();
                navParam.Add(NavigationParameterNames.TargetParam, screenName);
                succeeded = SwitchModule(module, navParam);
                return succeeded;
            }

            succeeded = SwitchScreen(_moduleScreensRegion[module], screenName, navParam);
            return succeeded;
        }

        public void OpenFloatingScreen<T>(T screen, Dictionary<string, object> navParam = null)
        {
            SwitchScreen(RegionNames.FloatingScreenRegion, screen.ToString(), navParam);
        }

        public void CloseFloatingScreen()
        {
            _regionManager.Regions[RegionNames.FloatingScreenRegion].RemoveAll();
        }


        private bool SwitchScreen(string regionName, string screenName, Dictionary<string, object> navParam)
        {
            var param = CreateNavigationParams(navParam);
            bool succeeded = false; 

            Action<NavigationResult> callback = (navResult) =>
            {
                if(IsNavigationSucceeded(navResult))
                {
                    succeeded = true;
                    return;
                }
                
                //If navigation failed, normally because of returning 'continuationCallBack(false)' in the IConfirmNavigationResult.ConfirmNavigationRequest
                //and Error == null
                if (!(navResult.Result.HasValue && navResult.Result.Value) && navResult.Error == null)
                    return;

                if (navResult.Error != null)
                {
                    //TODO: Log the error
                    succeeded = false;
                    return;
                }
            };

            _regionManager.RequestNavigate(regionName, screenName, callback, param);
            return succeeded;
        }

        private NavigationParameters CreateNavigationParams(IEnumerable<KeyValuePair<string, object>> navParams = null)
        {
            var param = new NavigationParameters();
            if (navParams != null)
            {
                foreach (var par in navParams)
                    param.Add(par.Key, par.Value);
            }

            return param;
        }

        private bool IsNavigationSucceeded(NavigationResult navResult)
        {
            bool succeeded = navResult.Result == null || navResult.Result.Value && navResult.Error == null;
            return succeeded;
        }
    }
}
