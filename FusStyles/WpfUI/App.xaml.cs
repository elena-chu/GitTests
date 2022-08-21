using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using WpfUI.Module;
using Ws.Extensions.UI.Wpf.Behaviors;
using Ws.Fus.ImageViewer.UI.Wpf.Module;
using Ws.Fus.Mr.UI.Wpf;
using Ws.Fus.Reports.UI.Wpf;
using Ws.Fus.Surgical.UI.Wpf;
using Ws.Fus.Treatment.UI.Wpf.Module;
using Ws.Fus.UI.Navigation.Wpf;

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
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");

                // If you do not set this property, the binding engine uses the Language property of the binding target object.
                // In XAML this defaults to "en-US" or inherits the value from the root element (or any element) of the page,
                // if one has been explicitly set.
                FrameworkElement.LanguageProperty.OverrideMetadata(
                    typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

                InitializeComponent();
            }
            catch (Exception)
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
            moduleCatalog.AddModule<FusNavigationModule>();
            moduleCatalog.AddModule<ImageViewerModule>();
            moduleCatalog.AddModule<FusTreatmentUiModule>();
            moduleCatalog.AddModule<FusMrUiModule>();
            moduleCatalog.AddModule<FusSurgicalUiModule>();
            moduleCatalog.AddModule<FusReportsUiModule>();

            moduleCatalog.AddModule<WpfUiModule>();
        }

    }
}
