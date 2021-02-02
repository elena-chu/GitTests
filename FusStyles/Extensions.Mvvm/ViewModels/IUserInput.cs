using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Mvvm.ViewModels
{
    public interface IUserInput
    {
        object ViewModel { get; }

        object Model { get; }

        //string Model

        bool IsValid { get; }

        /// <summary>
        /// Validates the user input (<see cref="ViewModel"/>). Applies it to the <see cref="Model"/> if validates.
        /// </summary>
        /// <returns>True, if user input validates. False - otherwise.</returns>
        bool Apply();

        void Reset(object model);
    }
}
