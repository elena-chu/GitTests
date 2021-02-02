using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Extensions.Patterns.Seed;

namespace Ws.Extensions.Patterns.Profiles
{
    public interface IProfileSeedsCollection
    {
        IEnumerable<Profile> ProfileSeeds { get; }
    }
}
