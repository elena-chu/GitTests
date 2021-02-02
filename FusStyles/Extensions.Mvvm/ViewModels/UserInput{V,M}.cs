using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Mvvm.ViewModels
{
    public abstract class UserInput<V, M> : IUserInput<V, M>
    {
        private V _vm;

        public UserInput(V vm)
        {
            _vm = vm;
        }

        public V ViewModel => _vm;

        public abstract M Model { get; }

        object IUserInput.ViewModel => ViewModel;

        object IUserInput.Model => Model;

        public abstract bool IsValid { get; }

        public abstract bool Apply();

        public abstract void Reset(M model);

        void IUserInput.Reset(object model) => Reset((M)model);
    }
}
