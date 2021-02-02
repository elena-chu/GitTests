using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.ImageViewer.UI.Wpf.Navigation.Interfaces
{
    public interface INavigatable
    {
        string Name { get; }
        event EventHandler IsNavigatableChanged;
        bool IsNavigatable { get; }
    }
}
