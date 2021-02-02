using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Mvvm.ViewModels
{
    public class BindableWrapper<T> : BindableBase
    {
        public virtual T Value { get; protected set; }

        public BindableWrapper(T value)
        {
            Value = value;
        }

        protected BindableWrapper()
        {
        }

        public virtual void Reset(T value)
        {
            Value = value;
            Reset();
            RaisePropertyChanged(null);
        }

        protected virtual void Reset()
        {
        }
    }
}
