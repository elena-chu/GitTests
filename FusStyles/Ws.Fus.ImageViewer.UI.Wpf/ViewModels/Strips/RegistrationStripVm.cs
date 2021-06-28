using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace Ws.Fus.ImageViewer.UI.Wpf.ViewModels.Strips
{
    public class RegistrationStripVm: StripVm
    {
        public DelegateCommand SetAsReferenceCommand { get; }
        public DelegateCommand CopyRegistrationsCommand { get; }

        public DelegateCommand CancelRegistrationsCommand { get; }

        private bool _isReference;
        public bool IsReference
        {
            get { return _isReference; }
            set { SetProperty(ref _isReference, value); }
        }

        private bool _isRegistered;
        public bool IsRegistered
        {
            get { return _isRegistered; }
            set { SetProperty(ref _isRegistered, value); }
        }

    }
}
