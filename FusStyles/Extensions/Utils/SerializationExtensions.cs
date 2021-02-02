using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Utils
{
    public static class SerializationExtensions
    {
        public static T Clone<T>(this T obj)
        {
            var from = JsonConvert.SerializeObject(obj);
            var to = JsonConvert.DeserializeObject<T>(from);

            return to;
        }
            
    }
}
