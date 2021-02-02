using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ws.Extensions.Mvvm.ViewModels.Switch
{
    public interface ISwitch : INotifyPropertyChanged
    {
        /// <summary>
        /// Should be notified via <see cref="INotifyPropertyChanged"/> by the implementor
        /// </summary>
        bool IsOn { get; }

        ICommand SwitchCommand { get; }
    }
}
