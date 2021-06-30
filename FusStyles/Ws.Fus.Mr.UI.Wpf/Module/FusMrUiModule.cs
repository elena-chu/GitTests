using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System.Runtime.CompilerServices;
using Ws.Fus.Mr.UI.Wpf.Views;
using Ws.Fus.UI.Assets.Wpf;
using Prism.Regions;
using Ws.Fus.ImageViewer.UI.Wpf;

namespace Ws.Fus.Mr.UI.Wpf
{
    public class FusMrUiModule : IModule
    {
        [MethodImpl(MethodImplOptions.NoInlining)]

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //SkinManager.LoadResources();
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.ScanProgressBar, typeof(ScanProgressView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.Register<BindableBase, MrActiveStudyViewModel>(UiConstants.MrActiveStudyContentId);
        }
    }
}
