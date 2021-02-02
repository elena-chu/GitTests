using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using WpfUI.Views;
using Prism.Regions;
using Prism.Mvvm;
//using WpfUI.Screens.Controllers;
//using WpfUI.Screens.Views;
//using WpfUI.Screens.Interfaces;
//using Ws.Fus.UI.Wpf.Menus.Views;
//using Ws.Fus.UI.Wpf.Menus.ViewModels;
using System.Windows;
//using WpfUI.ViewModels;
using System.Diagnostics;
using Ws.Fus.ImageViewer.UI.Wpf;
using Ws.Fus.ImageViewer.UI.Wpf.Navigation.Controllers;
using Ws.Fus.ImageViewer.UI.Wpf.Navigation;

namespace WpfUI.Module
{
    class WpfUiModule : IModule
    {
        private const string PsDictionariesPath = "Screens/Assets/PlanningScans/Dictionaries/";

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //var thisAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            //var psDataTemplates = new ResourceDictionary
            //{
            //    Source = new Uri($"/{thisAssemblyName};component/{PsDictionariesPath}DataTemplates.xaml", UriKind.RelativeOrAbsolute)
            //};

            //Application.Current.Resources.MergedDictionaries.Add(psDataTemplates);

            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(RegionNames.ApplicationMainRegion, typeof(ApplicationView));
            regionManager.RegisterViewWithRegion(RegionNames.ModuleMainRegion, typeof(PlanningMainView));
            //regionManager.RegisterViewWithRegion(RegionNames.PlanningScreensRegion, typeof(CalibrationView));

            regionManager.RequestNavigate(RegionNames.ApplicationMainRegion, ViewNames.ApplicationMainView);
            

            //NavigationController nc = containerProvider.Resolve<NavigationController>();
            //nc.SwitchModule(ApplicationModule.MainScreen);

            return;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ApplicationView>(ViewNames.ApplicationMainView);

            //containerRegistry.RegisterForNavigation<MainView>();
            containerRegistry.RegisterForNavigation<PlanningMainView>(ApplicationModule.Planning.ToString());

            //containerRegistry.RegisterForNavigation<MDView>(ScreenMode.MD.ToString());
            //containerRegistry.RegisterForNavigation<NPRView>(ScreenMode.NPR.ToString());
            //containerRegistry.RegisterForNavigation<CalibrationView>(ScreenMode.Calibration.ToString());

            //containerRegistry.RegisterSingleton<ScreensNavigationController>();

            //containerRegistry.RegisterSingleton<ScreenModeControllersFactory>();
            //containerRegistry.Register<IScreenModeController, DefaultModeController>(ScreenMode.Default.ToString());
            //containerRegistry.Register<IScreenModeController, CalibrationController>(ScreenMode.Calibration.ToString());
            //containerRegistry.Register<IScreenModeController, MDController>(ScreenMode.MD.ToString());
            ////containerRegistry.Register<IScreenModeController, PlanningScanController>(ScreenMode.PlanningScan.ToString());
            //containerRegistry.Register<IScreenModeController, NprController>(ScreenMode.NPR.ToString());
            //containerRegistry.Register<IScreenModeController, TargetingController>(ScreenMode.Targeting.ToString());

            //ViewModelLocationProvider.Register<ToolMenusContainerView, ToolsMenusContainerViewModel>();
            //ViewModelLocationProvider.Register<MainModuleMenusContainerView, MainModuleMenusContainerViewModel>();

            //ViewModelLocationProvider.Register<MDView, MdViewModel>();
            //ViewModelLocationProvider.Register<NPRView, NPRViewModel>();
            //ViewModelLocationProvider.Register<CalibrationView, CalibrationViewModel>();

            Debug.WriteLine("WpfUiModule RegisterTypes passed");


        }
    }
}
