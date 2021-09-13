using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Ws.Extensions.Mvvm;
using Ws.Fus.UI.Navigation.Wpf;

namespace Ws.Fus.UI.Navigation.Wpf
{
    /// <summary>
    /// Base class for Modul's MainViewModel
    /// </summary>
    public abstract class ModuleMainViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        //protected readonly IDispatcher _dispatcher;
        protected readonly NavigationController _navigationController;
        protected readonly Dictionary<ApplicationModule, IModuleController> _moduleControllers;

        public ModuleMainViewModel(/*IDispatcher dispatcher,*/
           NavigationController navigationController,
           IEnumerable<IModuleController> moduleControllers)
        {
            //_dispatcher = dispatcher;
            _navigationController = navigationController;
            _moduleControllers = moduleControllers.ToDictionary(el => el.ApplicationModule);
        }


        public abstract ApplicationModule ApplicationModule { get; }

        protected IModuleController ModuleController => _moduleControllers.ContainsKey(ApplicationModule) ?  _moduleControllers[ApplicationModule] : null;

        public IModuleController this[ApplicationModule module]
        {
            get { return _moduleControllers.ContainsKey(module) ? _moduleControllers[module] : null; }
        }

        #region InavigationAware

        public bool KeepAlive => false;

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            ModuleController?.UpdateModuleActive(ApplicationModule, false);
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            ModuleController?.UpdateModuleActive(ApplicationModule, true);
        }

        #endregion

    }
}
