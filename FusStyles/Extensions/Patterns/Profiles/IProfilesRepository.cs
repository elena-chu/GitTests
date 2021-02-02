using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ws.Extensions.Patterns.Profiles;

namespace Ws.Extensions.Patterns.Profiles
{
    public interface IProfilesRepository
    {
        Task<IEnumerable<Profile<T>>> GetProfilesAsync<T>(int userId, CancellationToken ct = default(CancellationToken));

        Task<IEnumerable<Profile<T>>> GetProfilesAsync<T>(IEnumerable<int> userIds, CancellationToken ct = default(CancellationToken));

        Task SaveProfileAsync<T>(Profile<T> profile, CancellationToken ct = default(CancellationToken));

        Task SaveProfilesAsync(IEnumerable<Profile> profiles, CancellationToken ct = default(CancellationToken));

        Profile<T> CreateProfile<T>();

        string Serialize(object data);

        T Deserialize<T>(string dataStr);

        /// <summary>
        /// Creates manager for default profile group and for default user group.
        /// </summary>
        /// <typeparam name="TProfile"></typeparam>
        /// <param name="defaultProfileFactory">Factory to create <typeparamref name="TProfile"/> with default values.</param>
        /// <returns></returns>
        //IProfileMgr<TProfile> CreateManager<TProfile>(Func<TProfile> defaultProfileFactory);
        /// <summary>
        /// Creates manager for default profile group and for default user group.
        /// </summary>
        /// <typeparam name="TProfile"></typeparam>
        /// <returns></returns>
        //IProfileMgr<TProfile> CreateManager<TProfile>() where TProfile : new();
        /// <summary>
        /// Creates manager for specified profile group name and profile user group name. string.Empty can be specified for default group.
        /// </summary>
        /// <typeparam name="TProfile"></typeparam>
        /// <param name="profileGroupName"></param>
        /// <param name="profileUserGroupName"></param>
        /// <param name="defaultProfileFactory">Factory to create <typeparamref name="TProfile"/> with default values.</param>
        /// <returns></returns>
        //IProfileMgr<TProfile> CreateManager<TProfile>(string profileGroupName, string profileUserGroupName, Func<TProfile> defaultProfileFactory);
        /// <summary>
        /// Creates manager for specified profile group name and profile user group name. string.Empty can be specified for default group.
        /// </summary>
        /// <typeparam name="TProfile"></typeparam>
        /// <param name="profileGroupName"></param>
        /// <param name="profileUserGroupName"></param>
        /// <returns></returns>
        //IProfileMgr<TProfile> CreateManager<TProfile>(string profileGroupName, string profileUserGroupName) where TProfile : new();



        //IProfileMgr CreateManager(int userId, Type profileType);

        //IProfileMgr CreateManager(Type profileType, Func<object> defaultProfileFactory);
        //IProfileMgr CreateManager(Type profileType);
        //IProfileMgr CreateManager(Type profileType, string profileGroupName, string profileUserGroupName, Func<object> defaultProfileFactory);
        //IProfileMgr CreateManager(Type profileType, string profileGroupName, string profileUserGroupName);
    }
}
