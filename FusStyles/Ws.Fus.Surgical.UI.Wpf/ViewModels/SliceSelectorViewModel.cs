using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Fus.ImageViewer.Interfaces.Entities;

namespace Ws.Fus.Surgical.UI.Wpf
{
    public class SliceSelectorViewModel : BindableBase
    {
        private readonly Action<int> _switchPlaneAction;
        private int _minSliceNumber = 0;
        private int _maxSliceNumber = 0;

        public SliceSelectorViewModel(Action<int> switchPlaneAction)
        {
            _switchPlaneAction = switchPlaneAction;

            IncreaseSliceNumberCommand = new DelegateCommand(IncreaseExecute, IncreaseCanExecute);
            DecreaseSliceNumberCommand = new DelegateCommand(DecreaseExecute, DecreaseCanExecute);
        }

        #region Commands

        public DelegateCommand IncreaseSliceNumberCommand { get; }
        public void IncreaseExecute()
        {
            _switchPlaneAction(++CurrentSliceOffset);
        }
        public bool IncreaseCanExecute()
        {
            return CurrentSliceOffset < _maxSliceNumber;
        }

        public DelegateCommand DecreaseSliceNumberCommand { get; }
        public void DecreaseExecute()
        {
            _switchPlaneAction(--CurrentSliceOffset);
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
                }
            }
        }

        public bool IsMultiSlice => SliceCount > 1;

        private StripOrientation _scanOrientation;
        public StripOrientation ScanOrientation
        {
            get { return _scanOrientation; }
            set { SetProperty(ref _scanOrientation, value); }
        }

        public void UpdateByCurrentStrip(int sliceCount, int offset)
        {
            SliceCount = sliceCount;
            if (SliceCount > 1)
                CurrentSliceOffset = offset;
        }
    }
}

