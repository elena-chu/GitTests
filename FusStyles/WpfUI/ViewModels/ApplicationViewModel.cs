using System;
using Prism.Commands;
using Ws.Extensions.UI.Wpf.Controls;
using Ws.Fus.ImageViewer.UI.Wpf;
using System.Collections.ObjectModel;
using Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips;
using System.Windows.Media.Imaging;
using System.IO;
using System.Linq;
using Ws.Fus.UI.Navigation.Wpf;

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
            get { return false; /*_navigationController?.CurrentModule == ApplicationModule.MainScreen;*/ }
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

            InitStrips(true, false);
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

        private void InitStrips(bool registration, bool compareMode)
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
                        StripName = item.i == 0 ? "Test some very very very very long" : stripNameBase + item.i,
                        Orientation = (StripOrientation)((randomNum1 % 6) + 1),
                        Image = new BitmapImage(new Uri(item.pngFile)),
                        ImageCount = randomNum1,
                        IsSecondary = compareMode,
                        IsEnabled = randomNum1 % 5 != 0,
                        IsLoaded = randomNum1 % 3 == 0,
                        Series = GetSeries(random),
                        IsReference = randomNum2 % 4 == 0,
                        IsRegistered = randomNum1 % 5 == 0,
                        HasAutoRegistration = randomNum2 % 4 != 0
                    });
                }
                else
                {
                    strips.Add(new Strip()
                    {
                        StripName = item.i == 0 ? "Test some very very very very long" : stripNameBase + item.i,
                        Orientation = (StripOrientation)((randomNum1 % 6) + 1),
                        Image = new BitmapImage(new Uri(item.pngFile)),
                        ImageCount = randomNum1,
                        IsSecondary = compareMode,
                        IsEnabled = randomNum1 % 5 != 0,
                        IsLoaded = randomNum1 % 3 == 0,
                        Series = GetSeries(random),
                    });
                }
            }

            Strips = strips;
        }

        private Series GetSeries(Random random)
        {
            int randomNumber = random.Next(700);

            string patientName = GetRandomName(random);
            string patientId = (randomNumber ^ 2).ToString();
            double sliceThickness = randomNumber % 23 / 10.0;
            double sliceSpacing = randomNumber % 19 / 10.0;
            string resolution = (randomNumber % 256) + "x" + (randomNumber % 256);
            int seriesNumber = randomNumber % 14;
            string seriesDescription = GetRandomString(random);

            return new Series(patientName, patientId, sliceThickness, sliceSpacing, resolution, seriesNumber, seriesDescription);
        }

        private string GetRandomString(Random random, int? requestedLength = null)
        {
            if (requestedLength == null && random.Next(429) % 5 == 0)
                return null;

            int length = requestedLength.HasValue ? requestedLength.Value : random.Next(4, 10);
            string randomString = string.Empty;
            for (int i = 0; i < length; i++)
                randomString += ((char)(random.Next(1, 26) + 64)).ToString();
            return randomString;
        }

        private string GetRandomName(Random random)
        {
            string name = GetRandomString(random, 1).ToUpper() +
                          GetRandomString(random)?.ToLower() +
                          " " +
                          GetRandomString(random, 1).ToUpper() +
                          GetRandomString(random)?.ToLower();
            return name;
        }

        #endregion
    }
}
