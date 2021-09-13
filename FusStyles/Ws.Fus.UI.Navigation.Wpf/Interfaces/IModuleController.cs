using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace Ws.Fus.UI.Navigation.Wpf
{
    /// <summary>
    /// Base class for specific ModuleControllers responsible for Module navigation buttons
    /// </summary>
    public interface IModuleController
    {
        ApplicationModule ApplicationModule { get; }
        DelegateCommand SwitchModuleCommand { get; }

        bool IsActive { get; }

        void UpdateModuleActive(ApplicationModule module, bool isActive);
    }
}
