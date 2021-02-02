using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ws.Extensions.Mvvm.ViewModels
{
    public class ConcurrentDataBag : INotifyPropertyChanged
    {
        private readonly ReaderWriterLockSlim _infoLock = new ReaderWriterLockSlim();
        private readonly Dictionary<string, object> _info = new Dictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        public object this[string index]
        {
            get
            {
                _infoLock.EnterReadLock();
                try
                {
                    object result = null;
                    _info.TryGetValue(index, out result);
                    return result;
                }
                finally
                {
                    _infoLock.ExitReadLock();
                }
            }
            set
            {
                Add(new Dictionary<string, object> { { index, value } });
            }
        }

        public void Add(IDictionary<string, object> info)
        {
            _infoLock.EnterWriteLock();
            try
            {
                foreach (var kvp in info)
                {
                    _info[kvp.Key] = kvp.Value;
                }
            }
            finally
            {
                _infoLock.ExitWriteLock();
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }
    }
}
