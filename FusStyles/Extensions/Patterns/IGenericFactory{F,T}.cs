using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Patterns
{
    public interface IGenericFactory<in F, out T>
    {
        /// <summary>
        /// Creates <typeparamref name="T"/> from some input <typeparamref name="F"/>
        /// </summary>
        /// <param name="from">The input, based on which the object of type <typeparamref name="T"/> should be created</param>
        /// <returns></returns>
        T Create(F from);
    }

    public class DefaultGenericFactory<F,T> : IGenericFactory<F, T>
    {
        public T Create(F from) => (T)Activator.CreateInstance(typeof(T), from);
    }
}
