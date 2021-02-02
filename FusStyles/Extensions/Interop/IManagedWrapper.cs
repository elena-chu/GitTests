using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Interop
{
    public interface IManagedWrapper : IDisposable
    {
        /// <summary>
        /// Returns the wrapped native object.
        /// </summary>
        //IntPtr NativePtr { get; }

        /// <summary>
        /// Example:
        /// If the managed wrapper implementation holds std::shared_ptr<T>* as <see cref="NativePtr"/>, then T* will be returned by the property.
        /// </summary>
        IntPtr BusinessNativePtr { get; }

        /// <summary>
        /// Whether delete or not the native <see cref="NativePtr"/> when the instance is disposed.
        /// </summary>
        bool ShouldDestroy { get; }

        /// <summary>
        /// Releases ownership of its stored <see cref="NativePtr"/>, by returning its value and replacing it with a null pointer.
        /// This call does not destroy the managed <see cref="NativePtr"/>, but the <see cref="IManagedWrapper"/> object is released from the responsibility of deleting the object.
        /// Some other entity must take responsibility for deleting the object at some point.
        /// The method has the importance when the <see cref="ShouldDestroy"/> is <see cref="true"/>
        /// </summary>
        /// <returns></returns>
        IntPtr Release();
    }

    public interface IManagedWrapperHost
    {
        IManagedWrapper Wrapper { get; }
    }

    public static class ManagedWrapperExtensions
    {
        public static IManagedWrapper ToManagedWrapper<T>(this T obj)
        {
            if (typeof(IManagedWrapper).IsAssignableFrom(obj.GetType()))
                return obj as IManagedWrapper;
            else if (typeof(IManagedWrapperHost).IsAssignableFrom(obj.GetType()))
                return (obj as IManagedWrapperHost).Wrapper;
            else
                throw new InvalidCastException($"{obj.GetType()}: The object passed for casting is neither assignable from {typeof(IManagedWrapper)} nor from {typeof(IManagedWrapperHost)}");
        }
    }

    //public interface IManagedWrapperFactory<out T>
    //{
    //    T Create(IntPtr nativePtr);
    //    T Create(IManagedWrapper wrapper);
    //}

    //public class DefaultManagedWrapperFactory<T> : IManagedWrapperFactory<T>
    //{
    //    public T Create(IntPtr nativePtr) => (T)Activator.CreateInstance(typeof(T), nativePtr);
    //    public T Create(IManagedWrapper wrapper) => (T)Activator.CreateInstance(typeof(T), wrapper);
    //}
}
