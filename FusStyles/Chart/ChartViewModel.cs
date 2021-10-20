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

        private List<int> _temperatures = new List<int>() { 35, 47, 57, 62, 67, 72, 65, 60, 50, 43, 42, 41, 39 };

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
                MaxTemperature = TemperatureData.Last().Temperature + 5;
            if (TemperatureData.Last().Temperature < MinTemperature)
                MinTemperature = TemperatureData.Last().Temperature - 5;
        }

        #endregion


        #region Limit Data

        private int _limit = 45;
        public int Limit
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

        #endregion
    }
}
