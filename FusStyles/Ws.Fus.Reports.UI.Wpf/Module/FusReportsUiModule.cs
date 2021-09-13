using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Ws.Fus.Reports.UI.Wpf.Views;
//using Ws.Extensions.Mvvm.Services;
//using Ws.Extensions.UI.Wpf.Services;
//using Ws.Fus.Reports.UI.Wpf.Views;
using Ws.Fus.UI.Navigation.Wpf;

namespace Ws.Fus.Reports.UI.Wpf
{
    public class FusReportsUiModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            //regionManager.RegisterViewWithRegion(RegionNames.ModuleMainRegion, typeof(ReportsMainView));
            //regionManager.RegisterViewWithRegion(RegionNames.ReportsScreensRegion, typeof(SonicationReportView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<FusReportsView>(ViewName.FusReports);


            //containerRegistry.RegisterForNavigation<DqaReportView>(ViewName.DQAReport);
            //containerRegistry.RegisterForNavigation<ScreeningReportView>(ViewName.ScreeningReport);
            //containerRegistry.RegisterForNavigation<TreatmentReportView>(ViewName.TreatmentReport);
            //containerRegistry.RegisterForNavigation<PlanningReportView>(ViewName.PlanningReport);

            //containerRegistry.RegisterForNavigation<ImportActionView>(PopupViews.ImportActions);
            //containerRegistry.RegisterForNavigation<SonicationReportView>(PopupViews.SonicationSummaries);
            //containerRegistry.RegisterSingleton<IOpenFileDialogService, OpenFileDialogService>();

            containerRegistry.Register<IModuleController, ReportsModuleController>(ApplicationModule.Reports.ToString());
            //containerRegistry.RegisterSingleton<IMapperService, MapperService>();
            //containerRegistry.RegisterSingleton<IReportService, ReportService>();

            containerRegistry.RegisterForNavigation<ReportsMainView>(ApplicationModule.Reports.ToString());

            ViewModelLocationProvider.Register<ReportsMainView, ReportsMainViewModel>();

        }
    }
}
