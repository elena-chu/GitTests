using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Extensions.Patterns.Profiles;
using System.ComponentModel;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    class PreferncesSectionLocalProfileVm : PreferncesSectionProfileVm
    {
        //private static readonly ILogger _logger = Log.ForContext<PreferncesSectionLocalProfileVm>();

        //private ProfileName _profileName;

        //public PreferncesSectionLocalProfileVm(IUserInput userInput, IProfilesMgr profilesMgr, ProfileName profileName, PreferencesViewModel vm) :
        //    base(userInput, profilesMgr)
        //{
        //    if (vm == null)
        //        throw new ArgumentNullException(nameof(vm));

        //    ProfileName = profileName;

        //    vm.PropertyChanged += OnVmPropertyChanged;
        //}        

        //public override bool IsEmpty => false;

        //private ProfileName ProfileName
        //{
        //    get { return _profileName; }
        //    set
        //    {
        //        if (IsEmpty)
        //        {
        //            _logger.Warning("Trying to set profile name {name} of an empty section", value);
        //            return;
        //        }

        //        if (ProfileName != value)
        //        {
        //            _profileName = value;
        //            if (!ProfileName.IsNullOrEmpty(value))
        //                ResetUserInputModel();
        //        }
        //    }
        //}

        //protected override Profile GetProfile() => _profilesMgr.GetProfileObj(ProfileName);

        ///// <summary>
        ///// Applies the user input model <see cref="IUserInput.Model"/> to the data of the underlying section profile (<see cref="Profile.DataObj"/>).
        ///// If <paramref name="newProfileName"/> specified, a new profile with the specified name will be created,
        ///// and the user input model model will be applied to the data of that profile.
        ///// </summary>
        ///// <param name="newProfileName">Ignored</param>
        ///// <returns>Profile where the user input model was applied to its data.</returns>
        //protected override Profile ApplyUserInputModel(ProfileName newProfileName)
        //{
        //    Profile profile = null;

        //    if (!ProfileName.IsNullOrEmpty(newProfileName))
        //    {
        //        if (_profilesMgr.Profiles.Contains(newProfileName))
        //            throw new ArgumentException($"Profile {newProfileName} already exists", nameof(newProfileName));

        //        profile = _profilesMgr.Copy(ProfileName, newProfileName, null);

        //        // Don't use the property, since we don't want to reset the user input model.
        //        _profileName = newProfileName;
        //    }
        //    else
        //    {
        //        profile = _profilesMgr.GetProfileObj(ProfileName, createIfNotExists: false);
        //    }

        //    profile.DataObj = _userInput.Model;

        //    return profile;
        //}

        //private void OnVmPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    var vm = sender as PreferencesViewModel;
        //    if (e.PropertyName == nameof(vm.SelectedTreatmentProfile))
        //        ProfileName = vm.SelectedTreatmentProfile;
        //}
    }
}
