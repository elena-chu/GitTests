using Prism.Mvvm;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ws.Extensions.Mvvm.ViewModels;
using System.Runtime.Serialization;
using Prism.Regions;
using Prism.Commands;
using Ws.Extensions.Patterns.Profiles;
using Serilog;
using Ws.Fus.Treatment.UI.Wpf.Entities;
using System.Threading;
using Ws.Extensions.Mvvm.Commands;
using Ws.Extensions.Reflection;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    class PreferencesViewModel : BindableBase/*, INavigationAware, IRegionMemberLifetime*/
    {
        //private static readonly ILogger _logger = Log.ForContext<PreferencesViewModel>();

        ///// <summary>
        ///// The expected name for treatment profiles catalog profile.
        ///// The treatment profiles catalog is stored as a separate profile in the profiles repository
        ///// </summary>
        //private static ProfileName CatalogProfileName { get; } = (ProfileName)"Default";

        //private readonly IUserInput[] _userInputs;
        //private readonly IProfilesMgrFactory[] _profilesMgrFactories;

        //private readonly ProfileServices _profileServices;
        //private readonly IFusLoginContext _loginContext;

        //private readonly ReaderWriterLockSlim _sectionsLock = new ReaderWriterLockSlim();
        //private readonly Dictionary<string, PreferncesSectionVm> _sections = new Dictionary<string, PreferncesSectionVm>(StringComparer.OrdinalIgnoreCase);

        ///// <summary>
        ///// Factory treatment profiles catalog
        ///// </summary>
        //private Profile<TreatmentProfilesCatalog> _factoryCat;

        ///// <summary>
        ///// Current user tratment profiles catalog
        ///// </summary>
        //private Profile<TreatmentProfilesCatalog> _userCat;

        ///// <summary>
        ///// Task, responsible to get <see cref="_factoryCat"/> and <see cref="_userCat"/>
        ///// </summary>
        //private readonly Task _getTreatmentProfilesCatalogsTask;

        //private int _sectionTaskCount;

        //private ProfileName _selectedTreatmentProfile = ProfileName.Empty;

        //private ProfileName _saveAsTreatmentProfile;

        //private IRegionNavigationJournal _journal;

        //public AsyncCommand<SavePreferencesMode?> SaveCommand { get; }
        //public DelegateCommand GoBackCommand { get; }

        //public PreferencesViewModel(IUserInput[] userInputs, IProfilesMgrFactory[] profilesMgrFactories, ProfileServices profileServices, IFusLoginContext loginContext)
        //{
        //    _userInputs = userInputs;
        //    _profilesMgrFactories = profilesMgrFactories;
        //    _loginContext = loginContext;
        //    _profileServices = profileServices;

        //    _getTreatmentProfilesCatalogsTask = Task.Run(() => GetProfilesCatalogsAsync());

        //    SaveCommand = new AsyncCommand<SavePreferencesMode?>(SaveAsync, CanSave);

        //    GoBackCommand = new DelegateCommand(() => _journal.GoBack());
        //}

        //public IEnumerable<ProfileName> TreatmentProfiles => FactoryTreatmentProfiles.Concat(UserTreatmentProfiles);

        //public ProfileName SelectedTreatmentProfile
        //{
        //    get { return _selectedTreatmentProfile; }
        //    set
        //    {
        //        if (SetProperty(ref _selectedTreatmentProfile, value))
        //        {
        //            SaveCommand.RaiseCanExecuteChanged();
        //        }
        //    }
        //}

        //public ProfileName SaveAsTreatmentProfile
        //{
        //    get { return _saveAsTreatmentProfile; }
        //    set
        //    {
        //        if (SetProperty(ref _saveAsTreatmentProfile, value))
        //        {
        //            SaveCommand.RaiseCanExecuteChanged();
        //        }
        //    }
        //}

        //bool IRegionMemberLifetime.KeepAlive => true;

        //private bool IsFactoryProfilesEdit => _loginContext.User.Id == FusIdentityConstants.FactoryUserId; // should be more sophisticated        

        //private IEnumerable<ProfileName> FactoryTreatmentProfiles => _factoryCat?.Data?.TreatmentProfiles ?? Enumerable.Empty<ProfileName>();

        //private IEnumerable<ProfileName> UserTreatmentProfiles => _userCat?.Data?.TreatmentProfiles ?? Enumerable.Empty<ProfileName>();

        //bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext) => true;

        //void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        //{
        //    _journal = navigationContext.NavigationService.Journal;
        //}

        //void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        //{
        //}

        //internal async void CreateSectionAsync(string uid, PreferencesMode mode)
        //{
        //    try
        //    {
        //        IncrementSectionTaskCount();

        //        //if (uid == "http://insightec.com/fus/planning-scans/PlanningScanPreferences")
        //        //    await Task.Delay(5000);

        //        foreach (var uinp in _userInputs)
        //        {
        //            try
        //            {
        //                var section = await CreateSectionAsync(uinp, uid, mode).ConfigureAwait(false);

        //                if (section != null)
        //                {
        //                    _sectionsLock.EnterWriteLock();

        //                    try
        //                    {
        //                        _sections[uid] = section;
        //                    }
        //                    finally
        //                    {
        //                        _sectionsLock.ExitWriteLock();
        //                    }

        //                    RaisePropertyChanged(uid);
        //                    return;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.Error(ex, "Failed to create section for uid: {uid}, mode: {mode}, uinp: {@uinp}", uid, mode, uinp);
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        DecrementSectionTaskCount();
        //    }
        //}

        //internal object GetSectionViewModel(string uid)
        //{
        //    PreferncesSectionVm section;

        //    _sectionsLock.EnterReadLock();
        //    try
        //    {
        //        _sections.TryGetValue(uid, out section);
        //    }
        //    finally
        //    {
        //        _sectionsLock.ExitReadLock();
        //    }

        //    return section?.ViewModel;
        //}

        //private static Profile<TreatmentProfilesCatalog> CreateProfilesCatalog(ProfileServices profileServices) =>
        //    profileServices.UserProfiles.CreateProfile(
        //        profileServices.Repository, CatalogProfileName, "A list of treatment profiles", new TreatmentProfilesCatalog());

        //private static async Task<Profile<TreatmentProfilesCatalog>> GetProfilesCatalogAsync(ProfileServices profileServices, UserProfilesServicePolicy policy)
        //{
        //    var catalogs = await profileServices.UserProfiles.GetProfilesAsync<TreatmentProfilesCatalog>(
        //        profileServices.Repository, policy).ConfigureAwait(false);

        //    _logger.Debug("Treatment profiles catalogs for policy {policy}: {@catalogs}", policy, catalogs);

        //    if (catalogs.Count() > 1)
        //        _logger.Warning("Got several catalogs of treatment profiles: {@catalogs}. Only one profile with name {name} is expected", catalogs, CatalogProfileName);

        //    return catalogs.FirstOrDefault(c => c.Name == CatalogProfileName);
        //}

        //private async Task SaveAsync(SavePreferencesMode? mode, CancellationToken ct)
        //{
        //    if (!CanSave(mode))
        //    {
        //        SaveCommand.RaiseCanExecuteChanged();
        //        _logger.Warning("An attempt to save in mode {mode} when can't save", mode);
        //        return;
        //    }

        //    LinkedList<Profile> toSave = new LinkedList<Profile>();
        //    ProfileName newProfileName = mode.Value == SavePreferencesMode.SaveAs ? SaveAsTreatmentProfile : ProfileName.Empty;

        //    foreach (var kvp in _sections)
        //    {
        //        var section = kvp.Value;

        //        Profile profile;
        //        if (section.TryApply(newProfileName, out profile) && profile != null)
        //            toSave.AddLast(profile);
        //    }

        //    try
        //    {
        //        await _profileServices.UserProfiles.SaveProfilesAsync(_profileServices.Repository, toSave, ct);

        //        if (!ProfileName.IsNullOrEmpty(newProfileName))
        //        {
        //            if (IsFactoryProfilesEdit)
        //                _factoryCat.Data.TreatmentProfiles.Add(newProfileName);
        //            else
        //            {
        //                if (_userCat == null)
        //                    _userCat = CreateProfilesCatalog(_profileServices);

        //                _userCat.Data.TreatmentProfiles.Add(newProfileName);
        //            }

        //            SelectedTreatmentProfile = newProfileName;
        //            RaisePropertyChanged(nameof(TreatmentProfiles));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex, "Failed to save profiles: {@profiles}", toSave);
        //    }
        //}

        //private bool CanSave(SavePreferencesMode? mode)
        //{
        //    if (!mode.HasValue)
        //        return false;
        //    if (ProfileName.IsNullOrEmpty(SelectedTreatmentProfile))
        //        return false;

        //    if (_sectionTaskCount != 0) // still loading sections
        //        return false;

        //    if (_sections.Any(s => !s.Value.IsValid))
        //        return false;

        //    switch (mode.Value)
        //    {
        //        case SavePreferencesMode.Save:
        //            return UserTreatmentProfiles.Contains(SelectedTreatmentProfile) || (FactoryTreatmentProfiles.Contains(SelectedTreatmentProfile) && IsFactoryProfilesEdit);
        //        case SavePreferencesMode.SaveAs:
        //            return !ProfileName.IsNullOrEmpty(SaveAsTreatmentProfile) && !TreatmentProfiles.Contains(SaveAsTreatmentProfile);
        //        default:
        //            return false;
        //    }
        //}

        //private void IncrementSectionTaskCount()
        //{
        //    Interlocked.Increment(ref _sectionTaskCount);
        //    SaveCommand.RaiseCanExecuteChanged();
        //}

        //private void DecrementSectionTaskCount()
        //{
        //    Interlocked.Decrement(ref _sectionTaskCount);
        //    SaveCommand.RaiseCanExecuteChanged();
        //}

        //private async void GetProfilesCatalogsAsync()
        //{
        //    // When there is no catalog in the repository
        //    var defaultCatalog = CreateProfilesCatalog(_profileServices);
        //    defaultCatalog.Data.TreatmentProfiles.Add((ProfileName)"New Treatment");

        //    try
        //    {
        //        _factoryCat = await GetProfilesCatalogAsync(_profileServices, UserProfilesServicePolicy.FactoryOnly).ConfigureAwait(false);

        //        if (IsFactoryProfilesEdit && _factoryCat == null)
        //            _factoryCat = defaultCatalog;
        //        else if (!IsFactoryProfilesEdit)
        //        {
        //            _userCat = await GetProfilesCatalogAsync(_profileServices, UserProfilesServicePolicy.UserOnly);
        //            if (_userCat == null && _factoryCat == null)
        //                _userCat = defaultCatalog;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex, "Failed to get catalog of treatment profiles. Creating the default catalog");
        //        if (IsFactoryProfilesEdit && _factoryCat == null)
        //            _factoryCat = defaultCatalog;
        //        else if (!IsFactoryProfilesEdit && _userCat == null)
        //            _userCat = defaultCatalog;
        //    }

        //    RaisePropertyChanged(nameof(TreatmentProfiles));

        //    SelectedTreatmentProfile = TreatmentProfiles.First();
        //}

        //private async Task<PreferncesSectionVm> CreateSectionAsync(IUserInput uinp, string uid, PreferencesMode mode)
        //{
        //    if (mode == PreferencesMode.Static)
        //    {
        //        var staticSection = new PreferncesSectionStaticVm(uinp);
        //        _logger.Verbose("Static section created: {@section}", staticSection);
        //        return staticSection;
        //    }

        //    // try to cast to IUserInput<,> and get the generic parameters
        //    var genParams = uinp.GetGenericArguments();
        //    if (genParams == null)
        //    {
        //        _logger.Warning("The user input {@UserInput} doesn't implement {IUserUnputGen}, making impossible to determine the model type and thus to get the corresponding profiles of that type",
        //            uinp, typeof(IUserInput<,>));

        //        return null;
        //    }

        //    var modelType = genParams[1];

        //    var dataContractUid = modelType.GetDataContractUid();
        //    if (string.IsNullOrWhiteSpace(dataContractUid))
        //    {
        //        _logger.Warning("The model type {modelType} doesn't define the {dataContract} attribute, making impossible to determine the uid", modelType, typeof(DataContractAttribute));
        //        return null;
        //    }
        //    else if (uid != dataContractUid)
        //        return null;

        //    var section = PreferncesSectionVm.Empty;

        //    var factoryType = typeof(IProfilesMgrFactory<>).MakeGenericType(modelType);
        //    var factory = _profilesMgrFactories.SingleOrDefault(f => factoryType.IsAssignableFrom(f.GetType()));

        //    if (factory == null)
        //    {
        //        _logger.Warning("Factory of type {factoryType} was not received, making impossible to get profiles of type {modelType}", factoryType, modelType);
        //    }
        //    else
        //    {
        //        switch (mode)
        //        {
        //            case PreferencesMode.Local:
        //                {
        //                    var mgr = await factory.CreateObjAsync(_profileServices, UserProfilesServicePolicy.BothFactoryOverrides).ConfigureAwait(false);
        //                    section = new PreferncesSectionLocalProfileVm(uinp, mgr, SelectedTreatmentProfile, this);
        //                    break;
        //                }

        //            case PreferencesMode.Global:
        //                {
        //                    var mgr = await factory.CreateObjAsync(_profileServices, UserProfilesServicePolicy.BothUserOverrides).ConfigureAwait(false);
        //                    section = new PreferncesSectionGlobalProfileVm(uinp, mgr);
        //                    break;
        //                }
        //        }
        //    }

        //    _logger.Verbose("Section created: {@section}", section);

        //    return section;
        //}
    }
}