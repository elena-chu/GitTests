using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.UI.Navigation.Wpf
{
    public static class RegionNames
    {
        //Region for hosting whole Application
        public const string ApplicationMainRegion = "ApplicationMainRegion";
        //Region for hosting floating screens
        public const string FloatingScreenRegion = "FloatingScreenRegion";
        //Region for hosting SeriesSelector
        public const string SeriesSelectorRegion = "SeriesSelectorRegion";

        //Region for hosting application's modules: MainScreen, Planning, Surgery
        public const string ModuleMainRegion = "ModuleMainRegion";

        //Region's names per modules hosting module's inner screens
        public const string PlanningScreensRegion = "PlanningScreensRegion";
        public const string MainScreenRegion = "MainScreenRegion";
        public const string SurgicalScreensRegion = "SurgicalScreensRegion";
        public const string ReportsScreensRegion = "ReportsScreensRegion";

        //Utilities regions
        public const string SystemStatusIndicationRegion = "SystemStatusIndicationRegion";
        public const string WaitMessageRegion = "WaitMessageRegion";
        public const string ScanProgressBar = "ScanProgressBarRegion";

    }

    public static class ViewNames
    {
        public const string EntranceView = "EntranceView";
        public const string ApplicationMainView = "ApplicationMainView";
        public const string MainScreenMainView = "MainScreenMainView";
        public const string PlanningMainView = "PlanningMainView";
        public const string SurgicalMainView = "SurgicalMainView";
        public const string ReportsMainView = "ReportsMainView";
        public const string ReplayMainView = "ReplayMainView";

        public const string MainView = "MainView";
        public const string DummyView = "DummyView";
    }

    public static class FloatingViewNames
    {
        public const string MDView = "MdView";
        public const string SeriesSelectorView = "SeriesSelectorView";
        public const string ReplayView = "ReplayView";
        public const string ImageGalleryView = "ImageGalleryView";
        public const string TreatmentSummaryView = "TreatmentSummaryView";
    }

    public static class NavigationParameterNames
    {
        public const string ErrorParam = "Error";

        public const string TargetParam = "target";
        public const string CurrentScreenParam = "CurrentScreen";

        public const string CurrentModuleParam = "CurrentModule";
        public const string TargetModuleParam = "TargetModule";

        public const string TreatmentKeyParam = "TreatmentKey";
        public const string CallbackParam = "Callback";

        public const string SilentExitParam = "SilentExit";
        public const string RequestedNumberParam = "RequestedNumber";

        public const string PatientId = nameof(PatientId);

    }

    public static class SonnicationSummaryNavigationParametersNames
    {
        public const string CurrentSonicationNumber = "CurrentSonicationNumber";
        public const string IsDosimetrySurgicalMode = "IsDosimetrySurgicalMode";
    }
}
