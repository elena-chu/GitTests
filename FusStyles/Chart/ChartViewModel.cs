using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using Telerik.Windows.Data;

namespace InsightecFiddle.ViewModels
{
    public class DataPoint
    {
        public int Seconds { get; set; }
        public int Temperature { get; set; }
    }

    public class ChartViewModel : MinimalViewModel
    {
        private DispatcherTimer timer;
        private int _currentSecond;
        private const int TimerInterval = 500;
        private List<int> _temperatures = new List<int>() { 0, 47, 57, 62, 67, 72, 65, 60, 50, 43, 42, 41, 39};

        public ChartViewModel()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(TimerInterval);
            timer.Tick += OnTimer;

            FillData();
            AverageTemperature = 0;
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

        public RadObservableCollection<DataPoint> _data;
        public RadObservableCollection<DataPoint> Data
        {
            get => _data;
            set
            {
                if (_data != value)
                {
                    _data = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private void FillData()
        {
            RadObservableCollection<DataPoint> collection = new RadObservableCollection<DataPoint>();
            collection.Add(CreateBusinessObject());
            Data = collection;
        }

        private void OnTimer(object sender, EventArgs e)
        {
            Data.SuspendNotifications();

            Data.Add(CreateBusinessObject());
            Data.ResumeNotifications();
            UpdateMetrics();
            if (_currentSecond > 12)
            {
                timer.Tick -= OnTimer;
                StopTimer();
            }
        }

        private void UpdateMetrics()
        {
            AverageTemperature = (int)Data.Select(point => point.Temperature).Average();
        }

        private DataPoint CreateBusinessObject()
        {
            DataPoint obj = new DataPoint();

            obj.Temperature = _temperatures[_currentSecond];
            obj.Seconds = _currentSecond;
            _currentSecond++;

            return obj;
        }
    }
}
