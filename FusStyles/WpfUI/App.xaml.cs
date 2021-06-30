using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using WpfUI.Module;
using Ws.Extensions.UI.Wpf.Behaviors;
using Ws.Fus.ImageViewer.UI.Wpf.Module;
using Ws.Fus.Mr.UI.Wpf;
using Ws.Fus.Treatment.UI.Wpf.Module;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ProviderBh.Resolver = (type, name) => Container.Resolve(type, name);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ImageViewerModule>();
            moduleCatalog.AddModule<FusTreatmentUiModule>();
            moduleCatalog.AddModule<FusMrUiModule>();


            moduleCatalog.AddModule<WpfUiModule>();
        }

    }
}
