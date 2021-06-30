using System;
using Prism.Commands;
using Ws.Extensions.UI.Wpf.Controls;
using Ws.Fus.ImageViewer.UI.Wpf.Navigation.Controllers;
using Ws.Fus.ImageViewer.UI.Wpf;
using System.Collections.ObjectModel;
using Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips;
using System.Windows.Media.Imaging;
using System.IO;
using System.Linq;

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

            InitStrips(true);
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


        #region Strips

        private ObservableCollection<IStrip> _strips;
        public ObservableCollection<IStrip> Strips
        {
            get { return _strips; }
            set
            {
                _strips = value;
                RaisePropertyChanged();
            }
        }

        private void InitStrips(bool registration)
        {
            var imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\ImageLoad\ImageLoad\Images\");
            var fullPath = Path.GetFullPath(new Uri(imageFolder).LocalPath);
            var imageFiles = Directory.GetFiles(fullPath, "*.*").Where(file => file.ToLower().EndsWith("png") || file.ToLower().EndsWith("bmp")).ToList();
            var random = new Random();
            
            string stripNameBase = "IMAGE 0";
            ObservableCollection<IStrip> strips = new ObservableCollection<IStrip>();

            foreach (var item in imageFiles.Select((pngFile, i) => new { i, pngFile }))
            {
                int randomNum1 = random.Next(500);
                int randomNum2 = random.Next(40);

                if (registration)
                {
                    strips.Add(new RegistrationStrip()
                    {
                        StripName = stripNameBase + item.i,
                        Orientation = (StripOrientation)((randomNum1 % 6) + 1),
                        Image = new BitmapImage(new Uri(item.pngFile)),
                        ImageCount = randomNum1,
                        IsAvailable = randomNum1 % 5 != 0,
                        IsLoaded = randomNum1 % 3 == 0,
                        RegistrationStatus = (RegistrationStatus)(randomNum2 % 3),
                        IsReference = randomNum2 % 4 == 0
                    });
                }
                else
                {
                    strips.Add(new Strip()
                    {
                        StripName = stripNameBase + item.i,
                        Orientation = (StripOrientation)((randomNum1 % 6) + 1),
                        Image = new BitmapImage(new Uri(item.pngFile)),
                        ImageCount = randomNum1,
                        IsAvailable = randomNum1 % 5 != 0,
                        IsLoaded = randomNum1 % 3 == 0
                    });
                }
            }

            Strips = strips;
        }

        #endregion
    }
}
