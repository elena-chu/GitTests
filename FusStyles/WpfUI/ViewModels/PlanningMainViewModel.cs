using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Prism.Events;
using Prism.Regions;
using Ws.Fus.ImageViewer.UI.Wpf;
//using Ws.Fus.ImageViewer.UI.Wpf.Entities;
using Prism.Commands;
using Ws.Extensions.Mvvm;
using Ws.Fus.UI.Navigation.Wpf;
using System.Windows;

namespace WpfUI.ViewModels
{
    public class PlanningMainViewModel: ModuleMainViewModel, IConfirmNavigationRequest
    {
        //private readonly ScreenModeControllersFactory _controllersFactory;
        private readonly IEventAggregator _eventAggregator;

        public PlanningMainViewModel(/*ScreenModeControllersFactory controllersFactory,*/
            IEventAggregator eventAggregator,
            //IDispatcher dispatcher,
            NavigationController navigationController,
            //SeriesSelectorUiController seriesSelectorUiController,
            IEnumerable<IModuleController> moduleControllers)
            :base(/*dispatcher,*/ navigationController, moduleControllers)
        {
            //_controllersFactory = controllersFactory;

            _eventAggregator = eventAggregator;
            //_eventAggregator.GetEvent<ChangePlanningStageRequestEvent>().Subscribe(NavigateToLastAvailableStage);

            //SelectSeriesCommand = seriesSelectorUiController.SelectSeriesCommand;
        }

        public override ApplicationModule ApplicationModule => ApplicationModule.Planning;

        //public IScreenModeController this[ScreenMode mode]
        //{
        //    get { return _controllersFactory.GetScreenModeController(mode); }
        //}

        public DelegateCommand SelectSeriesCommand { get; }

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (_navigationController.CurrentModule == ApplicationModule)
                return;

            base.OnNavigatedTo(navigationContext);
            
            //IScreenModeController destinationController = null;
            ////Check whether specific screen is requested
            //if (navigationContext.Parameters.ContainsKey(NavigationParameterNames.TargetParam))
            //{
            //    ScreenMode screen;
            //    if (Enum.TryParse(navigationContext.Parameters[NavigationParameterNames.TargetParam].ToString(), out screen))
            //        destinationController = GetValidControllerByScreenMode(screen);
            //}

            //if (destinationController == null)
            //{
            //    //Find first available navigation screen
            //    var allScreens = Enum.GetValues(typeof(ScreenMode)).OfType<ScreenMode>().Where(el => el >= ScreenMode.Calibration && el <= ScreenMode.Targeting);
            //    foreach (var el in allScreens)
            //    {
            //        destinationController = GetValidControllerByScreenMode(el);
            //        if (destinationController!= null)
            //            break;
            //    }
            //}

            //if (destinationController != null)
            //    Application.Current.Dispatcher.BeginInvoke((Action)(() => destinationController.SwitchScreenCommand.Execute()));
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            bool succeeded = _navigationController.SwitchScreen(ApplicationModule.Planning, ViewNames.DummyView);
            continuationCallback(succeeded);
        }

        #endregion

        //private IScreenModeController GetValidControllerByScreenMode(ScreenMode screen)
        //{
        //    var controller = _controllersFactory.GetScreenModeController(screen);
            
        //    //Check whether screen is available 
        //    if (controller.IsStageEnabledInSessionType && controller.SwitchScreenCommand.CanExecute())
        //        return controller;

        //    return null;
        //}

        //private void NavigateToLastAvailableStage(ScreenMode screenMode)
        //{
        //    var allStages = Enum.GetValues(typeof(ScreenMode)).OfType<ScreenMode>().Where(el => el >= ScreenMode.Calibration && el < screenMode).OrderByDescending(el=> (int)el);
        //    foreach(var stage in allStages)
        //    {
        //        IScreenModeController targetController = GetValidControllerByScreenMode(stage);
        //        if (targetController != null)
        //        {
        //            _dispatcher.BeginInvoke((Action)(() => targetController.SwitchScreenCommand.Execute()));
        //            break;
        //        }
        //    }
        //}

    }
}
