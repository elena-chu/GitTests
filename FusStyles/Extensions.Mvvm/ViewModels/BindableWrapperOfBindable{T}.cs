using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Mvvm.ViewModels
{
    public class BindableWrapperOfBindable<T> : BindableWrapper<T> where T : BindableBase, new()
    {
        public BindableWrapperOfBindable(T value) : base(value)
        {
            Value.PropertyChanged += OnValuePropertyChanged;
        }

        public BindableWrapperOfBindable() : this(new T())
        {
        }

        public override void Reset(T value)
        {
            if (Value != value)
            {
                Value.PropertyChanged -= OnValuePropertyChanged;
                Value = value;
                Value.PropertyChanged += OnValuePropertyChanged;
                RaisePropertyChanged(string.Empty);
            }
        }

        private void OnValuePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) => RaisePropertyChanged(e.PropertyName);
    }
}
