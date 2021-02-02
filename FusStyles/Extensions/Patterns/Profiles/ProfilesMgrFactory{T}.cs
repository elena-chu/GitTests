using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ws.Extensions.Patterns.Profiles
{
    public abstract class ProfilesMgrFactory<T> : IProfilesMgrFactory<T>
    {
        private ProfilesMgr<T> _profilesMgr;
        private SemaphoreSlim _profilesMgrLock = new SemaphoreSlim(1, 1);

        public async Task<IProfilesMgr<T>> CreateAsync(ProfileServices profileServices, UserProfilesServicePolicy policy, CancellationToken ct)
        {
            if (_profilesMgr == null)
            {
                await _profilesMgrLock.WaitAsync(ct).ConfigureAwait(false);

                try
                {
                    if (_profilesMgr == null)
                    {
                        var profiles = await profileServices.UserProfiles.GetProfilesAsync<T>(profileServices.Repository, policy, ct);
                        _profilesMgr = CreateMgr(profiles);

                        _profilesMgr.ProfileFactory = (name, desc, data) =>
                            profileServices.UserProfiles.CreateProfile(profileServices.Repository, name, desc, data);

                        _profilesMgr.Serializer = profileServices.Repository.Serialize;
                        _profilesMgr.DeSerializer = (s) => profileServices.Repository.Deserialize<T>(s);
                    }
                }
                finally
                {
                    _profilesMgrLock.Release();
                }
            }

            return _profilesMgr;
        }

        public async Task<IProfilesMgr> CreateObjAsync(ProfileServices profileServices, UserProfilesServicePolicy policy, CancellationToken ct) =>
            await CreateAsync(profileServices, policy, ct).ConfigureAwait(false);


        protected abstract ProfilesMgr<T> CreateMgr(IEnumerable<Profile<T>> profiles);
    }
}
