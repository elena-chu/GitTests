using System.Threading.Tasks;
using System.Windows;

namespace ResizableGrid.ViewModels
{
    public class DebugViewModel: ViewModelBase
    {
        #region Start, End

        public DebugViewModel()
        {
            SetStuffAsync();
        }

        private async Task SetStuffAsync()
        {
            await Task.Delay(3000);
            Resolution = 120;
            await Task.Delay(3000);
            Origin = new Point(-240, 240);
            await Task.Delay(3000);
            Resolution = 70;
            await Task.Delay(3000);
            Origin = new Point(900, 240);
            await Task.Delay(3000);
            Origin = new Point(0, 0);
            Resolution = 100;
            await Task.Delay(3000);
            Resolution = 70;
            Origin = new Point(300, 300);
        }

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

        private Point _origin = new Point(200, 200);
        public Point Origin
        {
            get => _origin;
            set
            {
                _origin = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
