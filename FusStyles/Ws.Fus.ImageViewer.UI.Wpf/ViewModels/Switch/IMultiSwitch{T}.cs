using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Mvvm.ViewModels.Switch
{
    public interface ISubSwitches<T> : INotifyPropertyChanged
    {
        /// <summary>
        /// Whether the specified subswitch is On.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool this[T index] { get; }
    }

    public interface IMultiSwitch<T> : ISwitch
    {
        ISubSwitches<T> SubSwitches { get; }
    }
}
