using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ws.Extensions.Mvvm.ViewModels.Switch
{
    public abstract class SwitchBase : BindableBase, ISwitch
    {
        private readonly Action<bool> _switchAction;

        private bool _isOn;
        private bool _canSwitch;

        public SwitchBase(bool isOn, bool canSwitch, Action<bool> switchAction)
        {
            _switchAction = switchAction;

            IsOn = isOn;
            CanSwitch = canSwitch;
        }

        public virtual bool IsOn
        {
            get { return _isOn; }

            set
            {
                SetProperty(ref _isOn, value);
            }
        }

        public virtual bool CanSwitch
        {
            get { return _canSwitch; }

            set
            {
                SetProperty(ref _canSwitch, value);
            }
        }

        public abstract ICommand SwitchCommand { get; }

        protected Action<bool> SwitchAction => _switchAction;
    }
}
