using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ws.Extensions.Mvvm.ViewModels.Switch
{
    public class SubSwitches<T> : BindableBase, ISubSwitches<T>
    {
        private readonly MultiSwitch<T> _ms;
        private readonly Dictionary<T, SubSwitchInfo> _subSwitches = new Dictionary<T, SubSwitchInfo>();

        public SubSwitches(MultiSwitch<T> ms)
        {
            _ms = ms;

            ms.PropertyChanged += OnMsPropertyChanged;
        }

        public bool this[T index]
        {
            get
            {
                SubSwitchInfo info;
                if (!_subSwitches.TryGetValue(index, out info))
                {
                    info = _ms.GetSubSwitchInfoFunc(index);

                    if (info.IsOn && _subSwitches.Any(kvp => kvp.Value.IsOn))
                        throw new InvalidOperationException("Only one subswitch can be on at the same time");

                    _subSwitches.Add(index, info);
                }

                return info.IsOn && _ms.IsOn;
            }
        }

        public bool ContainsSubSwitch(T index) => _subSwitches.ContainsKey(index);

        public bool CanSwitch(T index)
        {
            SubSwitchInfo info;
            return _subSwitches.TryGetValue(index, out info) ? info.CanSwitch : false;
        }

        public bool SetCanSwitch(T index, bool canSwitch)
        {
            SubSwitchInfo info;
            if (_subSwitches.TryGetValue(index, out info) && info.CanSwitch != canSwitch)
            {
                info.CanSwitch = canSwitch;
                return true;
            }

            return false; // nothing happened
        }

        public bool Switch(T index) => Switch(index, null);

        public bool Switch(T index, bool? switchOn)
        {
            if (!CanSwitch(index))
                throw new InvalidOperationException($"Can't switch in {index} mode");

            bool isOn = switchOn.HasValue ? _subSwitches[index].Switch(switchOn.Value) : _subSwitches[index].Switch();

            if (isOn) // switch the rest off
            {
                foreach (var key in _subSwitches.Keys.ToArray())
                {
                    if (!key.Equals(index))
                        _subSwitches[key].Switch(false);
                }
            }

            RaisePropertyChanged("Item[]");

            return isOn;
        }

        private void OnMsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_ms.IsOn))
            {
                if (!_ms.IsOn)
                    foreach (var key in _subSwitches.Keys.ToArray())
                        _subSwitches[key].Switch(false);

                RaisePropertyChanged("Item[]");
            }
        }
    }

    public class MultiSwitch<T> : SwitchBase, IMultiSwitch<T>
    {
        private readonly SubSwitches<T> _subSwitches;
        private readonly Action<T, bool> _subSwitchAction;
        private readonly Func<T, SubSwitchInfo> _getSubSwitchInfoFunc;
        private readonly DelegateCommand<T> _switchCommand;

        public MultiSwitch(bool isOn, bool canSwitch, Action<bool> switchAction, Action<T, bool> subSwitchAction, Func<T, SubSwitchInfo> getSubSwitchInfoFunc) :
            base(isOn, canSwitch, switchAction)
        {
            _subSwitchAction = subSwitchAction;
            _getSubSwitchInfoFunc = getSubSwitchInfoFunc;

            _switchCommand = new DelegateCommand<T>(DoSwitch, CheckCanSwitch).ObservesProperty(() => CanSwitch);

            _subSwitches = new SubSwitches<T>(this);
        }

        public override ICommand SwitchCommand => _switchCommand;

        public SubSwitches<T> SubSwitches => _subSwitches;

        ISubSwitches<T> IMultiSwitch<T>.SubSwitches => SubSwitches;

        /// <summary>
        /// Will be used by <see cref="SubSwitches{T}"/>
        /// </summary>
        internal Func<T, SubSwitchInfo> GetSubSwitchInfoFunc => _getSubSwitchInfoFunc;

        public void SetCanSubSwitch(T index, bool canSubSwitch)
        {
            if (_subSwitches.SetCanSwitch(index, canSubSwitch))
                _switchCommand.RaiseCanExecuteChanged();
        }

        private bool CheckCanSwitch(T arg)
        {
            if (!CanSwitch || arg == null)
                return CanSwitch;

            return _subSwitches.CanSwitch(arg);
        }

        private void DoSwitch(T obj)
        {
            if (obj == null)
            {
                SwitchAction(!IsOn);
            }
            else
            {
                var isOn = _subSwitches.Switch(obj);
                _subSwitchAction(obj, isOn);
            }
        }
    }
}
