using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Ws.Extensions.Reflection
{
    public static class DataContractExtensions
    {
        public static string GetDataContractUid(this Type type)
        {
            var dataContract = type.GetCustomAttribute<DataContractAttribute>(true);
            if (dataContract == null)
                return null;

            var dataContractUid = $"{dataContract.Namespace}/{dataContract.Name}";

            return dataContractUid;
        }
    }
}
