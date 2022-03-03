using System.Windows;

namespace ResizableGrid.ViewModels
{
    public class DebugViewModel: ViewModelBase
    {
        #region Start, End

        public DebugViewModel() {}

        #endregion


        #region Resolution

        private double _resolution = 60;
        public double Resolution
        {
            get => _resolution;
            set
            {
                _resolution = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Origin

        public Point Origin { get => new Point(OriginX, OriginY); }

        private double _originX = 200;
        public double OriginX
        {
            get => _originX;
            set
            {
                _originX = value;
                OnPropertyChanged(nameof(Origin));
            }
        }

        private double _originY = 200;
        public double OriginY
        {
            get => _originY;
            set
            {
                _originY = value;
                OnPropertyChanged(nameof(Origin));
            }
        }

        #endregion
    }
}
