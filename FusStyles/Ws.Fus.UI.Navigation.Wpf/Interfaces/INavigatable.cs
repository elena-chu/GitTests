using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.UI.Navigation.Wpf
{
    public interface INavigatable
    {
        string Name { get; }
        event EventHandler IsNavigatableChanged;
        bool IsNavigatable { get; }
    }
}
