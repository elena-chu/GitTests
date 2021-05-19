using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ws.Extensions.Mvvm.ViewModels.Switch
{
    public class Switch : SwitchBase
    {
        private readonly DelegateCommand<bool?> _switchCommand;

        public Switch(bool isOn, bool canSwitch, Action<bool> switchAction) : base(isOn, canSwitch, switchAction)
        {
            _switchCommand = new DelegateCommand<bool?>(DoSwitch).ObservesCanExecute(() => CanSwitch);
        }        

        public override ICommand SwitchCommand => _switchCommand;

        private void DoSwitch(bool? on)
        {
            if (on.HasValue)
                SwitchAction(on.Value);
            else
                SwitchAction(!IsOn);
        }
    }
}
