using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Mvvm.Events
{
    [Obsolete("Awaiting for a better navigation implementation", false)]
    public class NavigationRequest
    {
        /// <summary>
        /// Where to navigate
        /// </summary>
        public string Uri { get; set; }

        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}
