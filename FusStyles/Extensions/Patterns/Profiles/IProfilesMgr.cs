using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Patterns.Profiles
{
    public interface IProfilesMgr
    {
        IEnumerable<ProfileName> Profiles { get; }

        Profile GetProfileObj(bool createIfNotExists = true);

        Profile GetProfileObj(ProfileName name, bool createIfNotExists = true);

        object CopyProfileData(Profile profile);

        void Rename(ProfileName from, ProfileName to, string toDesc, bool saveCopy = true);

        Profile Copy(ProfileName from, ProfileName to, string toDesc);
    }
}
