using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ws.Extensions.Mvvm.ViewModels
{
    public class NotifyPropertyChangedImplementation : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
        #endregion
    }
}
