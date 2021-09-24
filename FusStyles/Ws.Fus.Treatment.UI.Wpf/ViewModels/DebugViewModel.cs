using Prism.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Extensions.UI.Wpf.Behaviors;
using System.Windows.Media.Media3D;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    public class DebugViewModel: ViewModelBase
    {
        public DebugViewModel()
        {
            ProgressCommand = new DelegateCommand(ToggleProgress);
            ApprovalCommand = new DelegateCommand(ToggleApproval);
            InitMenu();
        }


        #region Progress

        private ProgressState _progressState = ProgressState.Regular;
        public ProgressState ProgressState
        {
            get { return _progressState; }
            set
            {
                _progressState = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ProgressCommand { get; private set; }
        private void ToggleProgress()
        {
            switch (ProgressState)
            {
                case ProgressState.Regular:
                    OnChangeProgress(true);
                    break;
                case ProgressState.Active:
                    OnChangeProgress(false);
                    break;
                case ProgressState.Completed:
                default:
                    break;
            }
        }

        private CancellationTokenSource source;
        private void OnChangeProgress(bool go)
        {
            if (go)
            {
                ProgressState = ProgressState.Active;
                IncrementProgress();
            }
            else if (source != null)
            {
                source.Cancel();
            }
        }

        private async void IncrementProgress()
        {
            source = new CancellationTokenSource();
            source.Token.ThrowIfCancellationRequested();

            try
            {
                await Task.Delay(3000, source.Token);
                ProgressState = ProgressState.Completed;
            }
            catch (OperationCanceledException)
            {
                ProgressState = ProgressState.Regular;
                source.Dispose();
                return;
            }

            if (ProgressState == ProgressState.Completed)
            {
                await Task.Delay(5000);
                ProgressState = ProgressState.Regular;
            }

            source.Dispose();
        }

        #endregion


        #region Approval

        private ProgressState _approvalState = ProgressState.Regular;
        public ProgressState ApprovalState
        {
            get { return _approvalState; }
            set
            {
                _approvalState = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ApprovalCommand { get; private set; }
        private void ToggleApproval()
        {
            switch (ApprovalState)
            {
                case ProgressState.Regular:
                    ApprovalState = ProgressState.Completed;
                    break;
                case ProgressState.Completed:
                    ApprovalState = ProgressState.Regular;
                    break;
                case ProgressState.Active:
                default:
                    break;
            }
        }

        #endregion


        #region Menu

        private void InitMenu()
        {
            ZoomCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Zoom", x); } );
            PanCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Pan", x); } );
            LineCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Line", x); } );
            AreaCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Area", x); } );
            OverlaysCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Overlays", x); } );
            ImageInfoCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Image Info", x); } );
    }

        public ICommand ZoomCommand { get; set; }
        public ICommand PanCommand { get; set; }
        public ICommand LineCommand { get; set; }
        public ICommand AreaCommand { get; set; }
        public ICommand OverlaysCommand { get; set; }
        public ICommand ImageInfoCommand { get; set; }

        private void MenuItemClicked(string name, bool? param)
        {
            string status = param.HasValue ? param.Value.ToString() : "null";
            Console.WriteLine("LA >>> " + name + " is calling. I am " + status);
        }

        #endregion


        #region Triplet

        private Point3D? _point;
        public Point3D? Point
        {
            get { return _point; }
            set { SetProperty(ref _point, value); }
        }

        #endregion


        #region Info

        public BitmapImage TestImage
        {
            get
            {
                var imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\ImageLoad\ImageLoad\Images\");
                var fullPath = Path.GetFullPath(new Uri(imageFolder).LocalPath);
                var imageName = Directory.GetFiles(fullPath, "*.*").First(file => file.ToLower().EndsWith("png") || file.ToLower().EndsWith("bmp"));
                return new BitmapImage(new Uri(imageName));
            }
        }

        #endregion
    }
}
