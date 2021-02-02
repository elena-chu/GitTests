using System;
using Prism.Commands;
using Ws.Extensions.UI.Wpf.Controls;
using Ws.Fus.ImageViewer.UI.Wpf.Navigation.Controllers;
using Ws.Fus.ImageViewer.UI.Wpf;

namespace WpfUI.ViewModels
{
    /// <summary>
    /// ApplicationViewModel responsible for shell screen holding Application Modules 
    /// </summary>
    public class ApplicationViewModel: Ws.Extensions.Mvvm.ViewModels.ViewModelBase/*, ISubScreenHolder*/
    {
        private readonly AdjustedTimer _timer;
        private readonly NavigationController _navigationController;

        private bool _isMainModuleActive
        {
            get { return _navigationController?.CurrentModule == ApplicationModule.MainScreen; }
        }

        public ApplicationViewModel(NavigationController navigationController)
        {
            _navigationController = navigationController;
            _navigationController.ModuleChanged += (_, __) => {
                OpenSettingsCommand.RaiseCanExecuteChanged();
                QuitCommand.RaiseCanExecuteChanged();
            };

            HelpCommand = new DelegateCommand(HelpExecute);
            QuitCommand = new DelegateCommand(QuitExecute, QuitCanExecute);
            OpenSettingsCommand = new DelegateCommand(OpenSettingsExecute, OpenSettingsCanExecute);

            _timer = new AdjustedTimer(() => TimeNow = DateTime.Now);
            _timer.IntervalMilliSeconds = 1000;
            _timer.ExecuteActionOnStart = true;
            _timer.StartTimer();
        }

        #region Commands

        public DelegateCommand HelpCommand { get; private set; }
        public void HelpExecute()
        { }

        public DelegateCommand QuitCommand { get; private set; }
        public void QuitExecute()
        {
            _navigationController.SwitchModule(ApplicationModule.MainScreen);
        }
        public bool QuitCanExecute()
        {
            return !_isMainModuleActive;
        }

        public DelegateCommand OpenSettingsCommand { get; private set; }
        public void OpenSettingsExecute()
        { }
        public bool OpenSettingsCanExecute()
        {
            return !_isMainModuleActive;
        } 

        #endregion

        //TODO: Set DateTime string by time format supplied by system preferences

        private DateTime _timeNow;
        public DateTime TimeNow
        {
            get { return _timeNow; }
            set { SetProperty(ref _timeNow, value); }
        }

    }
}
