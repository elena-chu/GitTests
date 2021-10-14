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
            InitLimitData();
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
            LimitData.SuspendNotifications();
            AddTemperaturePoint();
            AddLimitPoints();
            TemperatureData.ResumeNotifications();
            LimitData.ResumeNotifications();

            _currentSecond++;
            if (_currentSecond > 12)
            {
                timer.Tick -= OnTimer;
                StopTimer();
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
            UpdateTemperatureMetrics();
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

        private void UpdateTemperatureMetrics()
        {
            AverageTemperature = (int)TemperatureData.Select(point => point.Temperature).Average();
        }

        #endregion


        #region Limit Data

        private int _limit = 67;

        public RadObservableCollection<DataPoint> _limitData;
        public RadObservableCollection<DataPoint> LimitData
        {
            get => _limitData;
            set
            {
                if (_limitData != value)
                {
                    _limitData = value;
                    OnPropertyChanged();
                }
            }
        }

        private void InitLimitData()
        {
            LimitData = new RadObservableCollection<DataPoint>();
            AddLimitPoints();
        }

        private void AddLimitPoints()
        {
            DataPoint limitPoint = new DataPoint()
            {
                Temperature = _limit,
                Seconds = _currentSecond
            };
            LimitData.Add(limitPoint);
            limitPoint = new DataPoint()
            {
                Temperature = _limit,
                Seconds = _currentSecond + 0.5
            };
            LimitData.Add(limitPoint);
        }

        public void OnLimitChanged(int change)
        {
            _limit = change;
            LimitData.SuspendNotifications();

            var newLimitData = new RadObservableCollection<DataPoint>();
            foreach (var limitPoint in LimitData)
                newLimitData.Add(new DataPoint() { 
                    Seconds = limitPoint.Seconds,
                    Temperature = _limit
                });
            LimitData = newLimitData;
            OnPropertyChanged(nameof(LimitData));
            LimitData.ResumeNotifications();
        }
        #endregion
    }
}
