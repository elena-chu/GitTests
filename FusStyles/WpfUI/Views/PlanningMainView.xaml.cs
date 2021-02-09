using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Commands;
using WpfUI.Controls;
using WpfUI.ViewModels;

namespace WpfUI.Views
{
    /// <summary>
    /// Interaction logic for PlanningMainView.xaml
    /// </summary>
    public partial class PlanningMainView : UserControl
    {
        private bool _firstTime = true; // hate that
        public PlanningMainView()
        {
            InitializeComponent();
            Loaded += MainView_Loaded;
            OpenStageCommand = new DelegateCommand<object>(OpenStageExecute);
        }

        //private PlanningMainViewModel ViewModel => DataContext as PlanningMainViewModel;

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_firstTime)
            {
                //ViewModel.SeriesSelected += OnViewModelSeriesSelected;
                _firstTime = false;
            }
            _stages = new Dictionary<string, Tuple<ProgressNavigationButton, UserControl>>()
            {
                {"Calibration", new Tuple<ProgressNavigationButton, UserControl>(CalibBtn, CalibrtionMenu )},
                {"PlanningScan", new Tuple<ProgressNavigationButton, UserControl>(PlanBtn, PlanningScanMenu )},
                {"Registration", new Tuple<ProgressNavigationButton, UserControl>(RegBtn, RegistrationMenu )},
                {"AcPc", new Tuple<ProgressNavigationButton, UserControl>(AcPcBtn, AcPcMenu )},
                {"Npr", new Tuple<ProgressNavigationButton, UserControl>(NprBtn, NprMenu )},
                {"Targeting", new Tuple<ProgressNavigationButton, UserControl>(TargBtn, TargetingMenu )},
            };
            OpenStageCommand.Execute("Calibration");
        }


        private void OnViewModelSeriesSelected(object sender, EventArgs args)
        {
            bool closed = false;
            EventHandler onClosed = null;

            onClosed = (o, e) =>
            {
                SeriesSelector.Closed -= onClosed;
                closed = true;
            };

            SeriesSelector.Closed += onClosed;
            Dispatcher.Invoke(() => { SeriesSelector.IsOpen = false; });

            while (!closed)
                Task.Delay(1).Wait();
        }

        private void OnLoadSeriesClick(object sender, RoutedEventArgs e) => SeriesSelector.IsOpen = true;

        private Dictionary<string, Tuple<ProgressNavigationButton, UserControl>> _stages;

        public DelegateCommand<object> OpenStageCommand { get; set; }
        public void OpenStageExecute(object obj)
        {
            foreach(var el in _stages)
            {
                el.Value.Item1.SetValue(ProgressNavigationButton.IsActiveProperty, false);
                el.Value.Item2.Visibility = Visibility.Collapsed;
            }
            string stage = obj?.ToString();
            _stages[stage].Item1.SetValue(ProgressNavigationButton.IsActiveProperty, true);
            _stages[stage].Item2.Visibility = Visibility.Visible;
        }

    }
}
