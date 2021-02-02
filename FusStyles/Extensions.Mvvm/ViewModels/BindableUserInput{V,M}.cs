using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Mvvm.ViewModels
{
    public abstract class BindableUserInput<V,M> : BindableWrapper<M>, IUserInput<V, M> where V : BindableUserInput<V, M>
    {
        public BindableUserInput(M model) : base(model)
        {
        }

        protected BindableUserInput()
        {
        }

        public V ViewModel => this as V;

        /// <summary>
        /// The Value comes from the <see cref="BindableWrapper{M}"/>
        /// </summary>
        public M Model => Value;

        object IUserInput.ViewModel => ViewModel;

        object IUserInput.Model => Model;

        public abstract bool IsValid { get; }

        public abstract bool Apply();

        void IUserInput.Reset(object model) => Reset((M)model);
    }
}
