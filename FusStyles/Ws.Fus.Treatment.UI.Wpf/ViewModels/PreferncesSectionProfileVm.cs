using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Extensions.Patterns.Profiles;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    abstract class PreferncesSectionProfileVm : PreferncesSectionVm
    {
        //protected readonly IProfilesMgr _profilesMgr;

        //public PreferncesSectionProfileVm(IUserInput userInput, IProfilesMgr profilesMgr) : base(userInput)
        //{
        //    if (profilesMgr == null)
        //        throw new ArgumentNullException(nameof(profilesMgr));

        //    _profilesMgr = profilesMgr;
        //}

        //public override bool TryApply(ProfileName newProfileName, out Profile profile)
        //{
        //    if (!_userInput.Apply()) // apply changes to the model
        //    {
        //        profile = null;
        //        return false;
        //    }

        //    profile = ApplyUserInputModel(newProfileName);

        //    return true;
        //}

        //protected void ResetUserInputModel()
        //{
        //    Profile profile = GetProfile();

        //    var dataCopy = _profilesMgr.CopyProfileData(profile);
        //    _userInput.Reset(dataCopy);
        //}

        //protected abstract Profile GetProfile();

        ///// <summary>
        ///// Applies the user input model <see cref="IUserInput.Model"/> to the data of the underlying section profile (<see cref="Profile.DataObj"/>).
        ///// In section <see cref="PreferncesSectionLocalProfileVm"/> there is an option to create a new profile, specified by <paramref name="newProfileName"/>
        ///// and apply the model to that profile data. In section <see cref="PreferncesSectionGlobalProfileVm"/> this parameter will be ignored.
        ///// </summary>
        ///// <param name="newProfileName">
        ///// For <see cref="PreferncesSectionLocalProfileVm"/>. The name of the new profile to create where the user input model will be appied.
        ///// </param>
        ///// <returns>Profile where the user input model was applied to its data.</returns>
        //protected abstract Profile ApplyUserInputModel(ProfileName newProfileName);
    }
}
