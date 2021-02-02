using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Patterns.Profiles
{
    public interface IProfilesMgr<T> : IProfilesMgr
    {
        Profile<T> GetProfile(ProfileName name, bool createIfNotExists = true);

        Profile<T> GetProfile(bool createIfNotExists = true);

        T CopyProfileData(Profile<T> profile);
    }
}
