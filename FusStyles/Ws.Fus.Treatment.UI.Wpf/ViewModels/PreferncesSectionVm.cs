using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Extensions.Patterns.Profiles;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    abstract class PreferncesSectionVm : BindableBase
    {
        private class PreferencesSectionEmptyVm : PreferncesSectionVm
        {
            public override bool IsEmpty => true;

            public override bool TryApply(ProfileName newProfileName, out Profile profile)
            {
                throw new NotImplementedException();
            }
        }

        public static PreferncesSectionVm Empty { get; } = new PreferencesSectionEmptyVm();

        protected readonly IUserInput _userInput;

        public abstract bool IsEmpty { get; }

        public PreferncesSectionVm(IUserInput userInput)
        {
            if (userInput == null)
                throw new ArgumentNullException(nameof(userInput));

            _userInput = userInput;
        }

        protected PreferncesSectionVm()
        {
        }

        public object ViewModel => _userInput.ViewModel;

        public object Model => _userInput.Model;

        public bool IsValid => _userInput.IsValid;

        /// <summary>
        /// Validates the user input <see cref="IUserInput"/> and if valid applies it to the underlying model <see cref="IUserInput.Model"/>
        /// by calling the <see cref="IUserInput.Apply"/>.
        /// In profile orinted sections (sections that extend <see cref="PreferncesSectionProfileVm"/>) the method will apply the user input model to
        /// the data of the underlying section profile (<see cref="Profile.DataObj"/>). The profile will be retuned via the <paramref name="profile"/>.
        /// In <see cref="PreferncesSectionStaticVm"/> this parameter will be set to <see cref="null"/>.
        /// In section <see cref="PreferncesSectionLocalProfileVm"/> there is an option to create a new profile, specified by <paramref name="newProfileName"/>
        /// and apply the model to that profile data.
        /// </summary>
        /// <param name="newProfileName">
        /// For section <see cref="PreferncesSectionLocalProfileVm"/> section only.
        /// The name of the new profile to create where the model will be applied.
        /// </param>
        /// <param name="profile">
        /// For sections extending <see cref="PreferncesSectionProfileVm"/> only. Profile with model applied to its data.
        /// </param>
        /// <returns>True, if user input validates. False - otherwise.</returns>
        public abstract bool TryApply(ProfileName newProfileName, out Profile profile);
    }
}
