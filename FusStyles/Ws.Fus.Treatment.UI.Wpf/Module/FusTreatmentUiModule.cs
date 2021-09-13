using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Ws.Fus.Treatment.UI.Wpf.Views;
using Ws.Fus.UI.Assets.Wpf;
using System.Runtime.CompilerServices;
using Ws.Fus.Treatment.UI.Wpf.ViewModels;
using Ws.Extensions.Mvvm.ViewModels;
using Prism.Mvvm;
//using Ws.Fus.Sys.Interfaces.Entities;
using Ws.Fus.ImageViewer.UI.Wpf;
using Prism.Regions;
using Ws.Fus.UI.Navigation.Wpf;

namespace Ws.Fus.Treatment.UI.Wpf.Module
{
    public class FusTreatmentUiModule : IModule
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //SkinManager.LoadResources();
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(RegionNames.ModuleMainRegion, typeof(MainScreenMainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainScreenRegion, typeof(TreatmentView));

            regionManager.RequestNavigate(RegionNames.MainScreenRegion, MainScreenAction.Treatment.ToString());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Registration for module navigation
            containerRegistry.RegisterForNavigation<MainScreenMainView>();

            //containerRegistry.Register<IUserInput, SystemInfoViewModel>(Constants.SystemInfoUri);

            containerRegistry.RegisterForNavigation<MainScreenMainView>(ApplicationModule.MainScreen.ToString());

            //containerRegistry.RegisterForNavigation<PreferencesView>();
            //containerRegistry.RegisterForNavigation<SystemView>();
            containerRegistry.RegisterForNavigation<TreatmentView>(MainScreenAction.Treatment.ToString());
            containerRegistry.RegisterForNavigation<DqaView>(MainScreenAction.DQA.ToString());
            containerRegistry.RegisterForNavigation<ScreeningView>(MainScreenAction.Screening.ToString());
            containerRegistry.RegisterForNavigation<PrePlanningView>(MainScreenAction.PrePlanning.ToString());
            containerRegistry.RegisterForNavigation<DataBaseView>(MainScreenAction.DataBase.ToString());
            containerRegistry.RegisterForNavigation<SettingsView>(MainScreenAction.Settings.ToString());

            //ViewModelLocationProvider.Register<SystemView, PreferencesViewModel>();
        }
    }
}
