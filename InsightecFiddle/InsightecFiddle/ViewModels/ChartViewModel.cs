using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using Telerik.Windows.Data;

namespace InsightecFiddle.ViewModels
{
    public class DataPoint
    {
        public double Seconds { get; set; }
        public int Temperature { get; set; }
    }

    public class ChartViewModel : MinimalViewModel
    {
        #region Start

        public ChartViewModel()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(TimerInterval);
            timer.Tick += OnTimer;

            InitSeries();
            AverageTemperature = 0;
        }

        private void InitSeries()
        {
            InitTemperatureData();
        }

        #endregion


        #region Timer

        private DispatcherTimer timer;
        private int _currentSecond;
        private const int TimerInterval = 500;

        public void StartTimer()
        {
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        public void UpdateTimer(double interval)
        {
            timer.Interval = TimeSpan.FromMilliseconds(interval);
        }

        private void OnTimer(object sender, EventArgs e)
        {
            TemperatureData.SuspendNotifications();
            AddTemperaturePoint();
            TemperatureData.ResumeNotifications();

            _currentSecond++;
            if (_currentSecond > 12)
            {
                timer.Tick -= OnTimer;
                StopTimer();
            }
        }

        #endregion


        #region State

        private SonicationState _sonicationState = SonicationState.Preparation;
        public SonicationState SonicationState
        {
            get => _sonicationState;
            set
            {
                _sonicationState = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Axis

        private int _minTemperature = 35;
        public int MinTemperature
        {
            get => _minTemperature;
            set
            {
                _minTemperature = value;
                OnPropertyChanged();
            }
        }

        private int _maxTemperature = 50;
        public int MaxTemperature
        {
            get => _maxTemperature;
            set
            {
                _maxTemperature = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Temperature Data

        private List<int> _temperatures = new List<int>() { 35, 45, 49, 51, 53, 55, 59, 54, 50, 43, 42, 41, 39 };

        public RadObservableCollection<DataPoint> _temperatureData;
        public RadObservableCollection<DataPoint> TemperatureData
        {
            get => _temperatureData;
            set
            {
                if (_temperatureData != value)
                {
                    _temperatureData = value;
                    OnPropertyChanged();
                }
            }
        }

        private void InitTemperatureData()
        {
            TemperatureData = new RadObservableCollection<DataPoint>();
            AddTemperaturePoint();
        }

        private void AddTemperaturePoint()
        {
            DataPoint temperaturePoint = new DataPoint()
            {
                Temperature = _temperatures[_currentSecond],
                Seconds = _currentSecond
            };
            TemperatureData.Add(temperaturePoint);
            OnAddTemperatureData();
        }

        private int _averageTemperature;
        public int AverageTemperature
        {
            get => _averageTemperature;
            set
            {
                if (_averageTemperature != value)
                {
                    _averageTemperature = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnAddTemperatureData()
        {
            AverageTemperature = (int)TemperatureData.Select(point => point.Temperature).Average();
            if (TemperatureData.Last().Temperature >= MaxTemperature)
                MaxTemperature = TemperatureData.Last().Temperature + 3;
            if (TemperatureData.Last().Temperature < MinTemperature)
                MinTemperature = TemperatureData.Last().Temperature - 3;
            //if (TemperatureData.Count() > 5)
            //    CutoffTop = 51;
        }

        #endregion


        #region Temperature Cutoff

        private double _temperatureCutoffTop = double.NaN;
        public double TemperatureCutoffTop
        {
            get => _temperatureCutoffTop;
            set
            {
                _temperatureCutoffTop = value;
                OnPropertyChanged();
            }
        }

        private double _temperatureCutoffBottom = double.NaN;
        public double TemperatureCutoffBottom
        {
            get => _temperatureCutoffBottom;
            set
            {
                _temperatureCutoffBottom = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Limit, Target

        private double _limit = 50;
        public double Limit
        {
            get => _limit;
            set
            {
                if (value != _limit)
                {
                    _limit = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _target = 45;
        public double Target
        {
            get => _target;
            set
            {
                if (value != _target)
                {
                    _target = value;
                    OnPropertyChanged();
                }
            }
        }

        // Tolerance above target
        private int _toleranceAboveTarget = 4;
        public int ToleranceAboveTarget
        {
            get => _toleranceAboveTarget;
            set
            {
                _toleranceAboveTarget = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TargetProportion));
            }
        }

        // Tolerance below target
        private int _toleranceBelowTarget = 2;
        public int ToleranceBelowTarget
        {
            get => _toleranceBelowTarget;
            set
            {
                _toleranceBelowTarget = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TargetProportion));
            }
        }

        public double TargetProportion
        {
            get
            {
                return (double)ToleranceAboveTarget / (double)(ToleranceAboveTarget + ToleranceBelowTarget);
            }
        }

        #endregion


        #region Time Indication

        private double _timeIndication = 3;
        public double TimeIndication
        {
            get => _timeIndication;
            set
            {
                if (value != _timeIndication)
                {
                    _timeIndication = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }

    public enum SonicationState
    {
        Preparation,
        Active,
        Postactivation
    }
}
