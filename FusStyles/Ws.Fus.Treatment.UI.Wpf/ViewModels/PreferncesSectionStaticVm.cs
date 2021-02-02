using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Extensions.Patterns.Profiles;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    class PreferncesSectionStaticVm : PreferncesSectionVm
    {
        public PreferncesSectionStaticVm(IUserInput userInput) : base(userInput)
        {
        }

        public override bool IsEmpty => false;

        /// <summary>
        /// Static sections aren't based on profile data.
        /// </summary>
        /// <param name="newProfileName">Ignored</param>
        /// <param name="profile">Set to null</param>
        /// <returns>True, if user input validates. False - otherwise.</returns>
        public override bool TryApply(ProfileName newProfileName, out Profile profile)
        {
            profile = null;
            return _userInput.Apply();
        }
    }
}
