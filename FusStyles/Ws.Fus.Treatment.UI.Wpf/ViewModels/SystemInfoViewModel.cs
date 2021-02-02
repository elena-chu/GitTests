using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Fus.Sys.Interfaces.Entities;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    class SystemInfoViewModel : IUserInput<SystemInfoViewModel, FusSystemInfo>
    {
        private readonly Lazy<FusSystemInfo> _systemInfo;

        public SystemInfoViewModel(IEventAggregator ea)
        {
            _systemInfo = new Lazy<FusSystemInfo>(() => GetSystemInfo(ea));
        }

        public FusSystemInfo SystemInfo => _systemInfo.Value;

        public object Model => SystemInfo;

        public object ViewModel => this;

        public  bool Apply() => true;

        public void Reset(object model)
        {
            throw new InvalidOperationException();
        }

        public void Reset(FusSystemInfo model)
        {
            throw new InvalidOperationException();
        }

        public bool IsValid => true;

        private FusSystemInfo GetSystemInfo(IEventAggregator ea)
        {
            var si = new FusSystemInfo();

            ea.GetEvent<PubSubEvent<FusSystemInfo>>().Publish(si);

            return si;
        }
    }
}
