using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace Ws.Fus.UI.Navigation.Wpf
{
    /// <summary>
    /// Base class for specific ModuleControllers responsible for Module navigation buttons
    /// </summary>
    public abstract class ModuleControllerBase : BindableBase, IModuleController
    {
        protected readonly NavigationController _navigationController;

        public ModuleControllerBase(NavigationController navController)
        {
            _navigationController = navController;
            SwitchModuleCommand = new DelegateCommand(SwitchModuleExecute, SwitchModuleCanExecute);
        }

        public ApplicationModule ApplicationModule { get; protected set; }

        public DelegateCommand SwitchModuleCommand { get; protected set; }
        protected virtual void SwitchModuleExecute()
        {
            if (!SwitchModuleCanExecute() || IsActive)
                return;

            _navigationController.SwitchModule(ApplicationModule);
           Application.Current.Dispatcher.BeginInvoke((Action)(()=> RaisePropertyChanged(nameof(IsActive))));
        }
        protected  bool SwitchModuleCanExecute()
        {
            bool isInSonication = false;
            return !isInSonication && SwitchModuleSpecificCanExecute();
        }


        protected bool _isActive;
        /// <summary>
        /// Denotes whether the mode's screen is now visible and active accordingly
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            protected set { SetProperty(ref _isActive, value); }
        }


        public void UpdateModuleActive(ApplicationModule module, bool isActive)
        {
            if (module != ApplicationModule)
                return;
            IsActive = isActive;
        }


        protected virtual bool SwitchModuleSpecificCanExecute()
        {
            return true;
        }


    }
}
