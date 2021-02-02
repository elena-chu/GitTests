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
    public class CompositeSwitch : BindableBase, ISwitch
    {
        private readonly Switch[] _switches;
        private readonly CompositeCommand _compositeSwitchCommand = new CompositeCommand();

        private readonly DelegateCommand _switchCommand;

        public CompositeSwitch(params Switch[] switches)
        {
            _switches = switches;

            _switchCommand = new DelegateCommand(DoSwitch, CanSwitch);

            foreach (var s in switches)
            {
                _compositeSwitchCommand.RegisterCommand(s.SwitchCommand);

                s.PropertyChanged += OnSwitchPropertyChanged;
            }

            _compositeSwitchCommand.CanExecuteChanged += (s, e) =>
            {
                _switchCommand.RaiseCanExecuteChanged();
            };
        }

        public ICommand SwitchCommand => _switchCommand;

        public bool IsOn => _switches.All(s => s.IsOn);

        private void DoSwitch() => _compositeSwitchCommand.Execute(!IsOn);

        private bool CanSwitch() => _compositeSwitchCommand.CanExecute(null);

        private void OnSwitchPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Switch.IsOn))
                RaisePropertyChanged(nameof(IsOn));
        }
    }
}
