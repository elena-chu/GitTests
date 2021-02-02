using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.ImageViewer.UI.Wpf
{
    public static class RegionNames
    {
        //Region for hosting whole Application
        public const string ApplicationMainRegion = "ApplicationMainRegion";
        //Region for hosting floating screens
        public const string FloatingScreenRegion = "FloatingScreenRegion";

        //Region for hosting application's modules: MainScreen, Planning, Surgery
        public const string ModuleMainRegion = "ModuleMainRegion";

        //Region's names per modules hosting module's inner screens
        public const string PlanningScreensRegion = "PlanningScreensRegion";
        public const string MainScreenRegion = "MainScreenRegion";
        public const string SurgeryScreensRegion = "SurgeryScreensRegion";

        //Utilities regions
        public const string SystemStatusIndicationRegion = "SystemStatusIndicationRegion";
        public const string WaitMessageRegion = "WaitMessageRegion";

    }

    public static class ViewNames
    {
        public const string EntranceView = "EntranceView";
        public const string ApplicationMainView = "ApplicationMainView";
        public const string MainScreenMainView = "MainScreenMainView";
        public const string PlanningMainView = "PlanningMainView";
        public const string SurgeryMainView = "SurgeryMainView";

        public const string MainView = "MainView";
    }

    public static class FloatingViewNames
    {
        public const string MDView = "MDView";
        public const string SettingsView = "SettingsView";
        public const string ReportView = "ReportView";
    }

    public static class NavigationParameterNames
    {
        public const string ErrorParam = "Error";
        public const string TargetParam = "target";
    }

}
