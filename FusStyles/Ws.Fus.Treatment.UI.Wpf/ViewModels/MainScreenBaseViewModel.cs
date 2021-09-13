using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Fus.Treatment.UI.Wpf;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    public abstract class MainScreenBaseViewModel : ViewModelBase, IMainScreenAction
    {
        public MainScreenAction MainScreenAction { get; protected set; }
        
        #region Support for INavigatable

        public event EventHandler IsNavigatableChanged;
        public bool IsNavigatable
        {
            get { return true; }
        }

        public string Name
        {
            get { return MainScreenAction.ToString(); }
        }

        #endregion
    }
}
