using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ws.Extensions.Async
{
    public static class AsyncExtensions
    {
        public static async Task<E> ToAsync<T, E>(this T obj, Action<T> method, string eventName, int timeoutMs, CancellationToken ct = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<E>();

            EventHandler<E> handler = null;

            var info = obj.GetType().GetEvent(eventName);

            try
            {
                handler += (s, e) =>
                {
                    tcs.TrySetResult(e);
                };

                info.AddEventHandler(obj, handler);

                method(obj);

                using (ct.Register(() => { tcs.TrySetCanceled(); }))
                {
                    var t = await Task.WhenAny(tcs.Task, Task.Delay(timeoutMs));

                    if (t != tcs.Task)
                    {
                        throw new TimeoutException("Operation timeout");
                    }

                    return tcs.Task.Result;
                }
            }
            finally
            {
                info.RemoveEventHandler(obj, handler);
            }
        }
    }
}
