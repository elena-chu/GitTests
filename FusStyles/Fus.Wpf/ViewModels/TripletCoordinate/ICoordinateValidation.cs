using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Fus.UI.Wpf.ViewModels.TripletCoordinate
{
    public interface ICoordinateValidation
    {
        bool IsValid(string valueDisplay);
    }
}
