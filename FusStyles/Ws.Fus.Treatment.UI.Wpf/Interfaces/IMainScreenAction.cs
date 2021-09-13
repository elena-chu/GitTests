using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Fus.Treatment.UI.Wpf;
using Ws.Fus.UI.Navigation.Wpf;

namespace Ws.Fus.Treatment.UI.Wpf
{
    public interface IMainScreenAction: INavigatable
    {
        MainScreenAction MainScreenAction { get; }
    }
}
