using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Mvvm.ViewModels.Switch
{
    public class SubSwitchInfo
    {
        public bool IsOn { get; set; }
        public bool CanSwitch { get; set; }

        internal bool Switch() => IsOn = !IsOn;

        internal bool Switch(bool isOn) => IsOn = isOn;
    }
}
