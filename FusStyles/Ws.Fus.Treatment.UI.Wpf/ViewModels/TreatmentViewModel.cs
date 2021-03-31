using System.Collections.ObjectModel;
using Prism.Commands;
using Ws.Fus.ImageViewer.UI.Wpf.Navigation.Controllers;
using Ws.Fus.ImageViewer.UI.Wpf.Navigation;
using Ws.Fus.ImageViewer.UI.Wpf;
using Ws.Extensions.UI.Wpf.Controls;
using System.Threading.Tasks;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    public class TreatmentViewModel: MainScreenBaseViewModel
    {
        private readonly NavigationController _navigationController;

        public TreatmentViewModel(NavigationController navigationController)
        {
            _navigationController = navigationController;

            MainScreenAction = MainScreenAction.Treatment;

            StartTreatmentCommand = new DelegateCommand(StartTreatmentExecute);
            RefreshExamInfoCommand = new DelegateCommand(RefreshExamInfoExecute);
        }

        public DelegateCommand StartTreatmentCommand { get; private set; }
        public void StartTreatmentExecute()
        {
            _navigationController.SwitchModule(ApplicationModule.Planning);
        }

        public DelegateCommand RefreshExamInfoCommand { get; private set; }
        public void RefreshExamInfoExecute()
        {
        }

        public string _selectedUser;
        public string SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        public ObservableCollection<string> UserNames { get; private set; }

        public string _selectedSide;
        public string SelectedSide
        {
            get { return _selectedSide; }
            set { SetProperty(ref _selectedSide, value); }
        }

        public string _examInfo;
        public string ExamInfo
        {
            get { return _examInfo; }
            set { SetProperty(ref _examInfo, value); }
        }

    }
}
