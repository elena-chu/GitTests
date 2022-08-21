using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Extensions.Patterns.Profiles;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    class PreferncesSectionGlobalProfileVm : PreferncesSectionProfileVm
    {
        //public PreferncesSectionGlobalProfileVm(IUserInput userInput, IProfilesMgr profilesMgr) : base(userInput, profilesMgr)
        //{
        //    ResetUserInputModel();
        //}

        //public override bool IsEmpty => false;

        //protected override Profile GetProfile() => _profilesMgr.GetProfileObj();

        ///// <summary>
        ///// Applies the user input model <see cref="IUserInput.Model"/> to the data of the underlying section profile (<see cref="Profile.DataObj"/>).
        ///// </summary>
        ///// <param name="newProfileName">Ignored</param>
        ///// <returns>Profile where the user input model was applied to its data.</returns>
        //protected override Profile ApplyUserInputModel(ProfileName newProfileName)
        //{
        //    var profile = _profilesMgr.GetProfileObj(false);
        //    profile.DataObj = _userInput.Model;

        //    return profile;
        //}
    }
}
