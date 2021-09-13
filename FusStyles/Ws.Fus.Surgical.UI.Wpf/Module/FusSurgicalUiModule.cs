using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Ws.Fus.UI.Navigation.Wpf;

namespace Ws.Fus.Surgical.UI.Wpf
{
    public class FusSurgicalUiModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //var regionManager = containerProvider.Resolve<IRegionManager>();

            //regionManager.RegisterViewWithRegion(RegionNames.ModuleMainRegion, typeof(SurgicalMainView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IModuleController, SurgicalModuleController>(ApplicationModule.Surgical.ToString());

            containerRegistry.RegisterForNavigation<SurgicalMainView>(ApplicationModule.Surgical.ToString());
            containerRegistry.RegisterForNavigation<SurgicalMenuDefinitionView>(SurgicalMode.Definition.ToString());
            containerRegistry.RegisterForNavigation<SurgicalMenuSonicationView>(SurgicalMode.Sonication.ToString());
            containerRegistry.RegisterForNavigation<SurgicalMenuDosimetryView>(SurgicalMode.Dosimetry.ToString());

            //ViewModelLocationProvider.Register<SurgicalMainView, SurgicalMainViewModel>();
            //ViewModelLocationProvider.Register<SurgicalMenuDefinitionView, SurgicalDefinitionViewModel>();
            //ViewModelLocationProvider.Register<SurgicalMenuSonicationView, SurgicalSonicationViewModel>();
            //ViewModelLocationProvider.Register<SurgicalMenuDosimetryView, SurgicalDosimetryViewModel>();
        }
    }
}
