using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Regions;
using Ws.Extensions.Mvvm;
//using Ws.Fus.Surgical.Interfaces;
using Ws.Fus.Surgical.UI.Wpf;
using Ws.Fus.UI.Navigation.Wpf;

namespace Ws.Fus.Surgical.UI.Wpf
{
    /// <summary>
    /// Main ViewModel of Surgical Module
    /// </summary>
    public class SurgicalMainViewModel : ModuleMainViewModel, IConfirmNavigationRequest
    {
        //private ISurgicalService _surgicalService;
        //private ISurgicalController _surgivalController;

        public SurgicalMainViewModel(/*IDispatcher dispatcher,*/
            NavigationController navigationController,
            IEnumerable<IModuleController> moduleControllers/*,*/
                                                            //ISurgicalService surgicalService,
                                                            /*SurgicalViewModel surgicalVm*/)
            : base(/*dispatcher,*/ navigationController, moduleControllers)
        {
            //_surgicalService = surgicalService;
            //SurgicalStripsViewModel = surgicalVm;
            ModeChangedCommand = new DelegateCommand<object>(ModeChangedExecute);
            ChangeSurgicalStageCommand = new DelegateCommand<SurgicalMode?>(ChangeSurgicalStageExecute);
        }

        #region Commands

        /// <summary>
        /// Command called when Surgical subMode changed caused by changing in UI Mode Control ViewModel
        /// </summary>
        public DelegateCommand<object> ModeChangedCommand { get; }
        private void ModeChangedExecute(object modeControl)
        {
            //SurgicalControlVM = modeControl as SurgicalControlBaseViewModel;
        }

        public DelegateCommand<SurgicalMode?> ChangeSurgicalStageCommand { get; }
        private void ChangeSurgicalStageExecute(SurgicalMode? mode)
        {
            if(mode.HasValue)
             _navigationController.SwitchScreen(ApplicationModule.Surgical, mode.Value);
        }

        #endregion

        #region Public Properties

        public override ApplicationModule ApplicationModule => ApplicationModule.Surgical;

        //private SurgicalViewModel _surgicalViewModel;
        //public SurgicalViewModel SurgicalStripsViewModel
        //{
        //    get { return _surgicalViewModel; }
        //    private set
        //    {
        //        SetProperty(ref _surgicalViewModel, value);
        //    }
        //}

        //private SurgicalControlBaseViewModel _surgicalControlVM;
        //public SurgicalControlBaseViewModel SurgicalControlVM
        //{
        //    get { return _surgicalControlVM; }
        //    set { SetProperty(ref _surgicalControlVM, value); }
        //}

        #endregion

        #region INavigationAware, IConfirmNavigationRequest

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //SurgicalStripsViewModel.OnNavigatedTo(navigationContext);
            base.OnNavigatedTo(navigationContext);

            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                //_surgivalController = _surgicalService.StartSurgery();
                Dictionary<string, object> param = new Dictionary<string, object>();
                //param.Add(SurgicalNavigationConsts.SurgicalControllerParam, _surgivalController);
                //param.Add(SurgicalNavigationConsts.SurgicalStripsVMParam, SurgicalStripsViewModel);
                _navigationController.SwitchScreen(ApplicationModule.Surgical, SurgicalMode.Definition, param);
            }));
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //SurgicalStripsViewModel.OnNavigatedFrom(navigationContext);

            //_navigationController.SwitchScreen(ApplicationModule.Surgical, "Dummy");
            base.OnNavigatedFrom(navigationContext);

            //_surgivalController.StopSurgery();
            //_surgivalController = null;
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            //Temp
            //if (SurgicalControlVM.SurgicalMode == SurgicalMode.Sonication)
            //    continuationCallback(false);

            //Temp commented for debug
            //if (_surgicalService.IsSonicatioinActive)
            //  continuationCallback(false);


            bool succeeded = _navigationController.SwitchScreen(ApplicationModule.Surgical, ViewNames.DummyView);
            continuationCallback(succeeded);

        } 

        #endregion
    }
}
