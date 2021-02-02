using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Patterns.Profiles
{
    /// <summary>
    /// Usually both services needed. Also can be injected as one package instead of two separate interfaces.
    /// </summary>
    public class ProfileServices
    {
        public IProfilesRepository Repository { get; }
        public IUserProfilesService UserProfiles { get; }

        public ProfileServices(IProfilesRepository repository, IUserProfilesService userProfiles)
        {
            Repository = repository;
            UserProfiles = userProfiles;
        }
    }
}
