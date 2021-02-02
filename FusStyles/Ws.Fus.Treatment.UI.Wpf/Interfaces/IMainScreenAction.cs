using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Fus.ImageViewer.UI.Wpf.Navigation.Interfaces;
using Ws.Fus.Treatment.UI.Wpf;

namespace Ws.Fus.Treatment.UI.Wpf
{
    public interface IMainScreenAction: INavigatable
    {
        MainScreenAction MainScreenAction { get; }
    }
}
