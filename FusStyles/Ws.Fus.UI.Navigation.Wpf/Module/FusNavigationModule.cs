using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Modularity;

namespace Ws.Fus.UI.Navigation.Wpf
{
    public class FusNavigationModule : IModule
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<NavigationController>();

            containerRegistry.RegisterForNavigation<DummyView>(ViewNames.DummyView);
        }


    }
}
