using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;

namespace Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips
{
    public class StripVm : BindableBase
    {
        protected readonly IStrip _strip;


        private bool _disposed = false;

        public DelegateCommand<bool?> SwitchCommand { get; }
        public DelegateCommand<bool?> SpreadCommand { get; }
        public DelegateCommand DeleteCommand { get; }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        private bool _isLoaded;
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set { SetProperty(ref _isLoaded, value); }
        }

        private bool _isSecondary;
        public bool IsSecondary
        {
            get { return _isSecondary; }
            set { SetProperty(ref _isSecondary, value); }
        }



        public IStrip Strip => _strip;

        public virtual object Clone() => MemberwiseClone();

    }
}
