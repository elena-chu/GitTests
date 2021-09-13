using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace Ws.Fus.UI.Navigation.Wpf
{
    /// <summary>
    /// Helper class for Prism navigation 
    /// </summary>
    public class NavigationController
    {
        Dictionary<ApplicationModule, string> _moduleScreensRegion = new Dictionary<ApplicationModule, string>()
        {
            {ApplicationModule.MainScreen, RegionNames.MainScreenRegion },
            {ApplicationModule.Planning, RegionNames.PlanningScreensRegion },
            {ApplicationModule.Surgical, RegionNames.SurgicalScreensRegion },
            {ApplicationModule.Reports, RegionNames.ReportsScreensRegion },
        };
        
        public NavigationController(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public event EventHandler<ApplicationModule?> ModuleChanged;
        public event EventHandler<bool> FloatingScreenOpenChanged;

        #region Public Properties

        /// <summary>
        ///  CurrentChangedEventManager active module MainScreen, Planning, Surgical, Reports
        /// </summary>
        public ApplicationModule? CurrentModule { get; private set; }

        private readonly IRegionManager _regionManager;
        public IRegionManager RegionManager => _regionManager;


        private bool _isFloatingScreenOpen;
        public bool IsFloatingScreenOpen
        {
            get { return _isFloatingScreenOpen; }
            private set
            {
                _isFloatingScreenOpen = value;
                FloatingScreenOpenChanged?.Invoke(this, _isFloatingScreenOpen);
            }
        } 

        #endregion

        #region Public Methods

        public bool SwitchModule(ApplicationModule module, Dictionary<string, object> navParam = null)
        {
            bool succeeded = SwitchScreen(RegionNames.ModuleMainRegion, module.ToString(), navParam);
            if (succeeded)
            {
                CurrentModule = module;
                ModuleChanged?.Invoke(this, CurrentModule);
            }

            return succeeded;
        }

        public bool SwitchScreen<T>(ApplicationModule module, T screen, Dictionary<string, object> navParam = null) where T : struct
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
            if (navParam == null)
                navParam = new Dictionary<string, object>();
            navParam.Add(NavigationParameterNames.CurrentModuleParam, CurrentModule);

            bool succeeded = SwitchScreen(RegionNames.FloatingScreenRegion, screen.ToString(), navParam);
            SetIsFloatingScreenOpen();
        }

        public void CloseFloatingScreen()
        {
            _regionManager.RequestNavigate(RegionNames.FloatingScreenRegion, "Dummy");
            _regionManager.Regions[RegionNames.FloatingScreenRegion].RemoveAll();
            SetIsFloatingScreenOpen();
        }

        public void CloseAllFloatingScreen()
        {
            CloseFloatingScreen();
            CloseSeriesSelectorScreen();
        }

        public void OpenSeriesSelectorScreen(Dictionary<string, object> navParam = null)
        {
            if (navParam == null)
                navParam = new Dictionary<string, object>();
            navParam.Add(NavigationParameterNames.CurrentModuleParam, CurrentModule);

            bool succeeded = SwitchScreen(RegionNames.SeriesSelectorRegion, FloatingViewNames.SeriesSelectorView, navParam);
            SetIsFloatingScreenOpen();
        }

        public void CloseSeriesSelectorScreen()
        {
            _regionManager.RequestNavigate(RegionNames.SeriesSelectorRegion, ViewNames.DummyView);
            _regionManager.Regions[RegionNames.SeriesSelectorRegion].RemoveAll();
            SetIsFloatingScreenOpen();
        }

        #endregion

        #region Private Methods

        private bool SwitchScreen(string regionName, string screenName, Dictionary<string, object> navParam)
        {
            var param = CreateNavigationParams(navParam);
            bool succeeded = false;

            Action<NavigationResult> callback = (navResult) =>
            {
                if (IsNavigationSucceeded(navResult))
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

        private void SetIsFloatingScreenOpen()
        {
            bool open = (_regionManager.Regions[RegionNames.FloatingScreenRegion].Views != null && _regionManager.Regions[RegionNames.FloatingScreenRegion].Views.Count() > 0)
                || (_regionManager.Regions[RegionNames.SeriesSelectorRegion].Views != null && _regionManager.Regions[RegionNames.SeriesSelectorRegion].Views.Count() > 0);

            IsFloatingScreenOpen = open;
        } 
        #endregion
    }
}
