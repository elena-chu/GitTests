using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ws.Extensions.Patterns.Profiles
{
    public interface IUserProfilesService
    {
        //IEnumerable<Profile<T>> Profiles { get; }

        /// <summary>
        /// Returns specified profile from the collection.
        /// Waiting to use .net version where implementation code is allowed in interfaces
        /// </summary>
        /// <param name="name">Profile to return.</param>
        /// <returns>Profile from collection if exists, null - otherwise.</returns>
        //Profile<T> this[ProfileName name] { get; }

        Task<IEnumerable<Profile<T>>> GetProfilesAsync<T>(IProfilesRepository repository, UserProfilesServicePolicy policy, CancellationToken ct = default(CancellationToken));

        Profile<T> CreateProfile<T>(IProfilesRepository repository, ProfileName name, string description, T data);

        Task SaveProfilesAsync(IProfilesRepository repository, IEnumerable<Profile> toSave, CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Adds profile to collection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="data"></param>
        /// <returns>The created profile</returns>
        //Profile<T> Add(ProfileName name, string description, T data);

        /// <summary>
        /// Removes the specified profile from the collection.
        /// </summary>
        /// <param name="name">Profile to remove.</param>
        /// <returns>True, if the specified profile exists in collection and was removed, false if the specified profile not exists in the collection.</returns>
        //bool Remove(ProfileName name);

        /// <summary>
        /// Saves profiles from the collection to persistent store.
        /// </summary>
        /// <param name="name">Profile name to save</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ProfileException">When the specified profiles not in the collection</exception>
        /// <exception cref="ProfileException">When failed to save.</exception>
        //void Save(ProfileName name);

        /// <summary>
        /// Creates profile data with default values.
        /// </summary>
        /// <returns></returns>
        //T CreateDefaultData();

        /// <summary>
        /// Validates the profile
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="validationResults">A collection to hold each failed validation.</param>
        /// <returns>true if the profile validates; otherwise, false.</returns>
        //bool TryValidate(Profile<T> profile, ICollection<ValidationResult> validationResults);
    }

    public static class ProfilesCollectionExtensions
    {
        //private static readonly ILogger _logger = Log.ForContext<ProfilesCollectionExtensions>();

        //public static Profile<T> GetProfile<T>(this IProfilesCollection<T> collection, ProfileName name, bool createIfNotExists = true)
        //{
        //    var profile = collection[name];

        //    if (profile == null && createIfNotExists)
        //    {
        //        var data = collection.CreateDefaultData();

        //        collection.Add(name, name, data);
        //    }

        //    return profile;
        //}
    }
}
