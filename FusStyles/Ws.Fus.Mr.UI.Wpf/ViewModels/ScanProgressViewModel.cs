using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Ws.Extensions.Mvvm;
//using Ws.Fus.Interfaces.Messages;

namespace Ws.Fus.Mr.UI.Wpf.ViewModels
{
    
    public class ScanProgressViewModel : BindableBase
    {
        string _scanName;
        bool _isScanActive = false;
        int _estimatedElapsedTime;

        int _timePassed;
        int _timeLeft;
        int _percentPassed;
        bool _askUser;
        //MrScanData _mrScanData;
        string _ScanStatusMessage;
        bool _isSunSetting;
        int _lingerTime;
        bool _isPreScan;
        bool _isTaskStarted;


        public ScanProgressViewModel()
        {
            //InitEvents();
            _scanName = "Calibration Scan";
            ConsiderCancelScan = new DelegateCommand(ConsiderCancelScanExecute);
            Dismiss = new DelegateCommand(DismissExecute);
            StopScan = new DelegateCommand(StopScanExecute, CanStopScan);
            IsScanActive = false;
            IsSunSetting = false;
            LingerTime = 1;
            _isPreScan = false;


        }
        
        //public void InitEvents()
        //{
        //    _mrService.NotifyScanData += _mrService_NotifyScanData;
        //    _mrService.NotifyScanStarted += _mrService_NotifyScanStarted;
        //    _mrService.NotifyScanEnded += _mrService_NotifyScanEnded;
           
        //}

        //private void _mrService_NotifyScanData(object sender, MrScanData e)
        //{
        //    _mrScanData = e;
        //    IsScanActive = true;
        //    ScanName = _mrScanData.ScanName ;
        //    EstimatedElapsedTime = _mrScanData.Duration;
        //    TimeLeft = EstimatedElapsedTime;
        //    RaisePropertyChanged(nameof(EstimatedTimeLeftDisplay));
        //    RaisePropertyChanged(nameof(EstimatedElapsedTimeDisplay));
        //    ScanStatusMessage = _mrScanData.PhaseName;
        //    IsSunSetting = false;
        //    IsPreScan = true;
        //    StopScan.RaiseCanExecuteChanged();
        //    StartCounting();
        //}

        //private void _mrService_NotifyScanEnded(object sender, EventArgs e)
        //{
            
        //    ScanStatusMessage = "Scan Ended";
        //    IsPreScan = false;
        //    SunSetting();
        //    StopScan.RaiseCanExecuteChanged();
        //    StartCounting();
        //}

        //private void _mrService_NotifyScanStarted(object sender, EventArgs e)
        //{
            
        //    IsScanActive = true;
        //    IsPreScan = false;
        //    StopScan.RaiseCanExecuteChanged();
        //    ScanName = _mrScanData.ScanName;
        //    TimePassed = 0;
        //    IsSunSetting = false;
        //    StartCounting();
           
        //}

        
        public bool IsScanActive
        {
            get { return _isScanActive; }
            set { SetProperty(ref _isScanActive, value); }
        }
        public bool AskUser
        {
            get { return _askUser; }
            set { SetProperty(ref _askUser, value); }
        }
        public string ScanName
        {
            get { return _scanName; }
            set { SetProperty(ref _scanName, value); }
        }
        public string ScanStatusMessage
        {
            get { return _ScanStatusMessage; }
            set { SetProperty(ref _ScanStatusMessage, value); }
        }
        public bool IsPreScan
        {
            get { return _isPreScan; }
            set { SetProperty(ref _isPreScan, value); }
        }
        public bool IsSunSetting
        {
            get { return _isSunSetting; }
            set { SetProperty(ref _isSunSetting, value); }
        }
        public int EstimatedElapsedTime
        {
            get { return _estimatedElapsedTime; }
            set
            {
                SetProperty(ref _estimatedElapsedTime, value);
            }
        }
        public string EstimatedElapsedTimeDisplay
        {
            get
            {
                return GetDisplayTime(EstimatedElapsedTime);
            }

        }
        public string EstimatedTimeLeftDisplay
        {
            get
            {
                return GetDisplayTime(TimeLeft);
            }

        }
        public int TimePassed
        {
            get { return _timePassed; }
            set { SetProperty(ref _timePassed, value); }
        }
        public int LingerTime
        {
            get { return _lingerTime; }
            set { SetProperty(ref _lingerTime, value); }
        }
        public int TimeLeft
        {
            get { return _timeLeft; }
            set { SetProperty(ref _timeLeft, value); }
        }
        public int PercentPassed
        {
            get { return _percentPassed; }
            set { SetProperty(ref _percentPassed, value); }
        }
        private string GetDisplayTime(int miliseconds)
        {
            if (miliseconds == 0) return "";
            int minutes = miliseconds / 1000 / 60;
            int seconds = (miliseconds / 1000) - (minutes * 60);
            return $"{(minutes>0?minutes:0)}:{(seconds>0?seconds:0):00}";
        }
        public DelegateCommand ConsiderCancelScan { get; private set; }
        public void ConsiderCancelScanExecute()
        {
            AskUser = true;
        }
        
        public DelegateCommand Dismiss { get; private set; }
        public void DismissExecute()
        {
            AskUser = false;

        }
        public DelegateCommand StopScan { get; private set; }
        private void StopScanExecute()
        {
            // one line of the following is to be commented out
            // AskUser = false;  
            // TaskCleanup();    
            //_mrService.AbortScan();

        }
        private bool CanStopScan()
        {
            return true; // !IsPreScan;
        }
        public void ScanEnded()
        {
            AskUser = false;

        }
        protected  void StartCounting()
        {
            if (!_isTaskStarted)
            {
                Task task = new Task(DoWork);
                task.Start();
            }

        }
        protected  void SunSetting()
        {
            IsSunSetting = true;
        }
        protected  void TaskCleanup()
        {
            IsScanActive = false;
        }
        private void DoWork()
        {
            _isTaskStarted = true;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (IsScanActive || IsSunSetting)
            {

                TimePassed = (int)sw.ElapsedMilliseconds;
                TimeLeft = (int)EstimatedElapsedTime - TimePassed;
                int percent = 0;
                if (EstimatedElapsedTime > 0)
                {
                    percent = TimePassed * 100 / EstimatedElapsedTime;
                }
                ReportProgress(percent);
                Task.Delay(10).Wait();
                if (IsSunSetting)
                {
                    ReportProgress(percent);
                    Task.Delay(LingerTime).Wait();
                    IsScanActive = false;
                    IsSunSetting = false;
                    ReportProgress(percent);
                }
            }

            IsScanActive = false;
            IsSunSetting = false;
            ScanEnded();
            _isTaskStarted = false;
        }
        private void ReportProgress(int percentDone)
        {
            Application.Current.Dispatcher
            .BeginInvoke((Action)(() => {

                PercentPassed = percentDone;
                RaisePropertyChanged(nameof(TimePassed));
                RaisePropertyChanged(nameof(EstimatedElapsedTimeDisplay));
                RaisePropertyChanged(nameof(EstimatedTimeLeftDisplay));
                RaisePropertyChanged(nameof(IsSunSetting));
                RaisePropertyChanged(nameof(IsScanActive));
                RaisePropertyChanged(nameof(ScanStatusMessage));

            }), DispatcherPriority.DataBind);

        }



    }

    
}
