using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Mvvm.ViewModels
{
    public interface IUserInput<V,M> : IUserInput
    {
        void Reset(M model);
    }

    public static class UserInputExtensions
    {
        public static Type[] GetGenericArguments(this IUserInput uinp)
        {
            // Check the user input implements the interface
            var uinpGenType = uinp.GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IUserInput<,>))
                .FirstOrDefault();

            return uinpGenType?.GetGenericArguments();
        }
    }
}
