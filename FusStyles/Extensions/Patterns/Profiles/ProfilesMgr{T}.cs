using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Patterns.Profiles
{
    public abstract class ProfilesMgr<T> : IProfilesMgr<T>
    {
        private static readonly ILogger _logger = Log.ForContext<ProfilesMgr<T>>();
        
        private readonly Dictionary<ProfileName, Profile<T>> _profiles = new Dictionary<ProfileName, Profile<T>>();

        public ProfilesMgr(IEnumerable<Profile<T>> profiles)
        {
            _logger.Information("Creating profiles manager");
            _logger.Verbose("Got profiles: {@Profiles}", profiles);

            foreach (var profile in profiles)
            {
                if (_profiles.ContainsKey(profile.Name))
                {
                    _logger.Warning("Got profile with duplicate name. Will be ignored. The duplicate profile: {@Profile}", profile);
                    continue;
                }

                var profileToAdd = profile;

                var validationResults = new LinkedList<ValidationResult>();

                if (!TryValidate(profile, validationResults))
                {
                    _logger.Warning("Got invalid profile - default values will be used. The invalid profile: {@Profile}", profile);
                    profileToAdd = CreateDefaultProfile(profile.Name, profile.Description);
                }

                _profiles[profileToAdd.Name] = profileToAdd;
            }
        }

        public IEnumerable<ProfileName> Profiles => _profiles.Keys;

        /// <summary>
        /// Should be set by <see cref="ProfilesMgrFactory{T}"/>
        /// </summary>
        internal Func<ProfileName, string, T, Profile<T>> ProfileFactory { get; set; }

        /// <summary>
        /// Should be set by <see cref="ProfilesMgrFactory{T}"/>
        /// </summary>
        internal Func<object, string> Serializer { get; set; }

        /// <summary>
        /// Should be set by <see cref="ProfilesMgrFactory{T}"/>
        /// </summary>
        internal Func<string, T> DeSerializer { get; set; }

        public Profile<T> GetProfile(ProfileName name, bool createIfNotExists)
        {
            if (ProfileName.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Profile<T> profile = null;
            if (!_profiles.TryGetValue(name, out profile) && createIfNotExists)
            {
                profile = CreateDefaultProfile(name, name);
                _profiles[profile.Name] = profile;
            }

            return profile;
        }

        public virtual Profile<T> GetProfile(bool createIfNotExists) => GetProfile((ProfileName)"Default", createIfNotExists);

        public void Rename(ProfileName from, ProfileName to, string toDesc, bool saveCopy)
        {
            if (ProfileName.IsNullOrEmpty(from))
                throw new ArgumentNullException(nameof(from));
            if (ProfileName.IsNullOrEmpty(to))
                throw new ArgumentNullException(nameof(to));
            if (from == to)
                throw new ArgumentException("Same profile name to rename", nameof(to));
            if (!_profiles.ContainsKey(from))
                throw new ArgumentException($"profile {from} not exists", nameof(from));
            if (_profiles.ContainsKey(to))
                throw new ArgumentException($"profile {to} already exists", nameof(to));

            var profile = _profiles[from];
            _profiles.Remove(from); // remove from dictionary since we are changing the key

            Profile<T> profileCopy = null;

            if (saveCopy)
            {
                var dataCopy = CreateDataCopy(profile.Data);
                profileCopy = ProfileFactory(from, profile.Description, dataCopy);
            }

            profile.Name = to;
            profile.Description = toDesc ?? profile.Description;
            _profiles[to] = profile;

            if (profileCopy != null)
                _profiles[from] = profileCopy;
        }

        public Profile Copy(ProfileName from, ProfileName to, string toDesc)
        {
            if (ProfileName.IsNullOrEmpty(from))
                throw new ArgumentNullException(nameof(from));
            if (ProfileName.IsNullOrEmpty(to))
                throw new ArgumentNullException(nameof(to));
            if (from == to)
                throw new ArgumentException("Same profile name to rename", nameof(to));
            if (!_profiles.ContainsKey(from))
                throw new ArgumentException($"profile {from} not exists", nameof(from));
            if (_profiles.ContainsKey(to))
                throw new ArgumentException($"profile {to} already exists", nameof(to));

            var profileFrom = _profiles[from];

            if (string.IsNullOrWhiteSpace(toDesc))
                toDesc = $"Copied from {from}: {profileFrom.Description}";

            var dataCopy = CreateDataCopy(profileFrom.Data);
            var profileCopy = ProfileFactory(to, toDesc, dataCopy);

            _profiles.Add(to, profileCopy);

            return profileCopy;
        }

        public Profile GetProfileObj(ProfileName name, bool createIfNotExists) => GetProfile(name, createIfNotExists);

        public Profile GetProfileObj(bool createIfNotExists) => GetProfile(createIfNotExists);

        public T CopyProfileData(Profile<T> profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            Profile<T> myProfile; // from my dictionary
            if (!_profiles.TryGetValue(profile.Name, out myProfile))
                throw new ArgumentException($"No profile with name {profile.Name} not found", nameof(profile));

            if (myProfile != profile)
                throw new ArgumentException($"The specified profile instance with name {profile.Name} not found", nameof(profile));

            return CreateDataCopy(myProfile.Data);
        }

        public object CopyProfileData(Profile profile) => CopyProfileData((Profile<T>)profile);

        protected Profile<T> CreateDefaultProfile(ProfileName name, string description)
        {
            var defaultProfile = ProfileFactory(name, description, CreateDefaultData());

            _logger.Verbose("Default profile created: {@Profile}", defaultProfile);

            return defaultProfile;
        }

        protected bool TryValidate(Profile<T> profile, ICollection<ValidationResult> validationResults)
        {
            if (!TryValidateSpecific(profile, validationResults))
            {
                _logger.Error("Specific profile validation failed. Profile: {@Profile}, Results: {@Results}", profile, validationResults);
                return false;
            }

            var valid = Validator.TryValidateObject(profile, new ValidationContext(profile), validationResults);
            if (!valid)
                _logger.Error("Profile validation failed. Profile: {@Profile}, Results: {@Results}", profile, validationResults);

            return valid;
        }

        /// <summary>
        /// Creates a default treatment profile data. Naive version. In most case will be overrrided by derived classes.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual T CreateDefaultData()
        {
            // Check if the type has default (parameterless) ctor, including private
            var ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

            return ctor != null ? Activator.CreateInstance<T>() : default(T);
        }

        /// <summary>
        /// The validation using the validation attributes is performed here in <see cref="TryValidate(Profile{T})"/> method.
        /// For specific (beyond the capabilities of validation attributes) validations, this method must be overriden in derived classes.
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        protected virtual bool TryValidateSpecific(Profile<T> profile, ICollection<ValidationResult> validationResults) => true;
        
        private T CreateDataCopy(T toCopy)
        {
            return DeSerializer(Serializer(toCopy));
        }        
    }
}
