using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Ws.Extensions.Mvvm.Commands;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    public class DebugViewModel: ViewModelBase
    {
        public DebugViewModel()
        {
            ProgressCommand = new RelayCommand(ToggleProgress);
            ApprovalCommand = new RelayCommand(ToggleApproval);
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
            MenuItemClickedCommand = new RelayCommand<ToolbarMenuItemCallbackData>(MenuItemClicked);
            ToolbarMenuViewModel = new ToolbarMenuViewModel(MenuItemClickedCommand);
        }

        public IToolbarMenuViewModel ToolbarMenuViewModel { get; set; }

        public ICommand MenuItemClickedCommand { get; set; }
        private void MenuItemClicked(ToolbarMenuItemCallbackData callbackData)
        {
            if (callbackData != null)
            {
                string checkRequestedStatus = callbackData.RequestedCheckStatus ? "Checked" : "Unchecked";
                Console.WriteLine(callbackData.MenuItemType.Caption(false) + " is calling. Requesting to be " + checkRequestedStatus);
            }
        }

        #endregion
    }
}
