using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Regions;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Fus.ImageViewer.UI.Wpf;
using Ws.Fus.Treatment.UI.Wpf;
using System.Windows.Threading;
using Ws.Fus.UI.Navigation.Wpf;
using System.Windows.Input;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    public class MainScreenMainViewModel: ViewModelBase, INavigationAware
    {
        private readonly NavigationController _navigationController;

        public MainScreenMainViewModel(NavigationController navigationController)
        {
            _navigationController = navigationController;

            SwitchScreenCommand = new DelegateCommand<object>(SwitchScreenExecute);
            ShutDownCommand = new DelegateCommand(ShutDownExecute);
            QuitTreatmentCommand = new DelegateCommand(QuitTreatment, () => CanIQuit);
        }

        #region Commands

        public DelegateCommand<object> SwitchScreenCommand { get; private set; }
        public void SwitchScreenExecute(object obj)
        {
            if (obj == null)
                return;

            MainScreenAction screen;
            if (Enum.TryParse(obj.ToString(), out screen))
            {
                bool succeeded = _navigationController.SwitchScreen(ApplicationModule.MainScreen, screen);
                if (succeeded)
                    CurrentScreen = screen;
            }
        }

        public DelegateCommand ShutDownCommand { get; private set; }
        public void ShutDownExecute()
        { }

        #endregion

        private MainScreenAction _currentScreen;
        public MainScreenAction CurrentScreen
        {
            get { return _currentScreen; }
            set { SetProperty(ref _currentScreen, value); }
        }

        #region Quit Treatment

        public DelegateCommand QuitTreatmentCommand { set; get; }

        private bool _canIQuit = true;
        public bool CanIQuit
        {
            get => _canIQuit;
            set
            {
                _canIQuit = value;
                RaisePropertyChanged();
                QuitTreatmentCommand.RaiseCanExecuteChanged();
            }
        }

        private void QuitTreatment()
        {
            //...
        }

        #endregion

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //Check whether specific screen is requested
            //if(navigationContext.Parameters.ContainsKey(NavigationParameterNames.TargetParam))
            //{
            //    MainScreenAction screen;
            //    if (Enum.TryParse(navigationContext.Parameters[NavigationParameterNames.TargetParam].ToString(), out screen))
            //    {
            //        _navigationController.SwitchScreen(ApplicationModule.MainScreen, screen);
            //        return;
            //    }
            //}

            ////Find first available navigation screen
            //var allScreens = Enum.GetValues(typeof(MainScreenAction));
            //foreach(var el in allScreens)
            //{
            //    //Check whether screen is available 
            //    MainScreenAction scr = (MainScreenAction)el;
            //    if (true)
            //    {
            //        Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => SwitchScreenExecute(scr)));
            //        break;
            //    }
            //}
            
            //RaisePropertyChanged(nameof(CurrentScreen));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
         
        #endregion
    }
}
