using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using Ws.Fus.ImageViewer.Interfaces.Entities;

namespace Ws.Fus.Surgical.UI.Wpf
{
    public class SliceSelectorViewModel : BindableBase
    {
        private readonly Action<int> _switchPlaneAction;

        public SliceSelectorViewModel(Action<int> switchPlaneAction)
        {
            _switchPlaneAction = switchPlaneAction;

            IncreaseSliceNumberCommand = new DelegateCommand(IncreaseExecute, IncreaseCanExecute);
            DecreaseSliceNumberCommand = new DelegateCommand(DecreaseExecute, DecreaseCanExecute);
            CycleSliceNumberCommand = new DelegateCommand(CycleSliceNumberExecute);
        }

        #region Commands

        public DelegateCommand IncreaseSliceNumberCommand { get; }
        public void IncreaseExecute()
        {
            _switchPlaneAction?.Invoke(++CurrentSliceOffset);
        }
        public bool IncreaseCanExecute()
        {
            return CurrentSliceOffset < _maxSliceNumber;
        }

        public DelegateCommand DecreaseSliceNumberCommand { get; }
        public void DecreaseExecute()
        {
            _switchPlaneAction?.Invoke(--CurrentSliceOffset);
        }
        public bool DecreaseCanExecute()
        {
            return CurrentSliceOffset > _minSliceNumber;
        }

        public DelegateCommand CycleSliceNumberCommand { get; }
        public void CycleSliceNumberExecute()
        {
            if (CurrentSliceOffset >= _maxSliceNumber)
                CurrentSliceOffset = _minSliceNumber;
            else
                IncreaseSliceNumberCommand.Execute();
        }

        #endregion


        #region Slices

        private int _minSliceNumber = 0;
        private int _maxSliceNumber = 0;

        private int _currentSliceOffset = 0;
        public int CurrentSliceOffset
        {
            get { return _currentSliceOffset; }
            set 
            { 
                if(SetProperty(ref _currentSliceOffset, value))
                {
                    IncreaseSliceNumberCommand.RaiseCanExecuteChanged();
                    DecreaseSliceNumberCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private int _sliceCount = 1;
        public int SliceCount
        {
            get { return _sliceCount; }
            set
            {
                if (SetProperty(ref _sliceCount, value))
                {
                    _maxSliceNumber = (int)Math.Floor(value / 2d);
                    _minSliceNumber = -1 * _maxSliceNumber;

                    RaisePropertyChanged(nameof(IsMultiSlice));
                    IncreaseSliceNumberCommand.RaiseCanExecuteChanged();
                    DecreaseSliceNumberCommand.RaiseCanExecuteChanged();
                    SetSliceNumbers();
                }
            }
        }

        public bool IsMultiSlice => SliceCount > 1;

        public void UpdateByCurrentStrip(int sliceCount, int offset)
        {
            SliceCount = sliceCount;
            if (SliceCount > 1)
                CurrentSliceOffset = offset;
        }

        public ObservableCollection<int> SliceNumbers { get; private set; }
        private void SetSliceNumbers()
        {
            SliceNumbers = new ObservableCollection<int>();
            for (int i = _minSliceNumber; i <= _maxSliceNumber; i++)
                SliceNumbers.Add(i);
            RaisePropertyChanged(nameof(SliceNumbers));
        }

        #endregion


        #region Orientation

        private StripOrientation _scanOrientation;
        public StripOrientation ScanOrientation
        {
            get { return _scanOrientation; }
            set { SetProperty(ref _scanOrientation, value); }
        }

        #endregion
    }
}

