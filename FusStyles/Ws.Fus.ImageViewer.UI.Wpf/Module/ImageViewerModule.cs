using Prism.Modularity;
using Prism.Ioc;
using System.Runtime.CompilerServices;
using Ws.Fus.ImageViewer.UI.Wpf.Navigation.Controllers;

namespace Ws.Fus.ImageViewer.UI.Wpf.Module
{
    public class ImageViewerModule : IModule
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<NavigationController>();

        }
    }
}
