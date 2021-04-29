using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Ws.Extensions.Mvvm.Commands;
using Ws.Extensions.Mvvm.ViewModels;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    public interface IToolbarMenuViewModel
    {
        void SetMenuItemCheckedStatus(ToolbarMenuItemType menuItemType, bool isChecked);
        void SetMenuHeaderCheckedStatus(ToolbarMenuItemType menuItemType, bool isChecked);

        void SetMenuItemCallbackCommand(ToolbarMenuItemType menuItemType, ICommand command);
        void SetMenuHeaderCallbackCommand(ToolbarMenuItemType menuItemType, ICommand command);
    }

    public class ToolbarMenuViewModel: ViewModelBase, IToolbarMenuViewModel
    {
        public ToolbarMenuViewModel(ICommand callbackCommand)
        {
            CallbackCommand = callbackCommand;
            MenuItemClickedCommand = new RelayCommand<ToolbarMenuItemCallbackData>(MenuItemClicked);
            BuildMenu();
            NotifyAllItems();
        }

        private ObservableCollection<ToolbarMenuHeaderViewModel> _menuHeaders;
        public ObservableCollection<ToolbarMenuHeaderViewModel> MenuHeaders
        {
            get { return _menuHeaders; }
            set
            {
                _menuHeaders = value;
                RaisePropertyChanged();
            }
        }

        private void BuildMenu()
        {
            ObservableCollection<ToolbarMenuHeaderViewModel> menuHeaders = new ObservableCollection<ToolbarMenuHeaderViewModel>()
            {
                new ToolbarMenuHeaderViewModel(ToolbarMenuItemType.Cycle, MenuItemClickedCommand, null),
                new ToolbarMenuHeaderViewModel(ToolbarMenuItemType.ViewZoom, MenuItemClickedCommand, new ObservableCollection<ToolbarMenuItemViewModel>()
                {
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.ViewZoom, MenuItemClickedCommand) { IsSelectable = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.ViewPan, MenuItemClickedCommand) { IsSelectable = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.ViewWindow, MenuItemClickedCommand) { IsSelectable = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.ViewResetView, MenuItemClickedCommand),
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.ViewCenterOnClick, MenuItemClickedCommand) { IsSelectable = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.ViewCenterOnTarget, MenuItemClickedCommand),
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.ViewScreenshot, MenuItemClickedCommand)
                }) { IsSelectable = true },
                new ToolbarMenuHeaderViewModel(ToolbarMenuItemType.DrawLine, MenuItemClickedCommand, new ObservableCollection<ToolbarMenuItemViewModel>()
                {
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.DrawLine, MenuItemClickedCommand) { IsSelectable = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.DrawArea, MenuItemClickedCommand) { IsSelectable = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.DrawAngle, MenuItemClickedCommand) { IsSelectable = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.DrawAngle90, MenuItemClickedCommand) { IsSelectable = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.DrawFiducial, MenuItemClickedCommand) { IsSelectable = true },
                }) { IsSelectable = true },
                new ToolbarMenuHeaderViewModel(ToolbarMenuItemType.OverlaysShowHide, MenuItemClickedCommand, new ObservableCollection<ToolbarMenuItemViewModel>(){
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysCrossReferenceLines, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysImageInfo, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysLinesAndAreas, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysFiducials, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysTransducer, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysEnvelope, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysGhostCursor, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysNonPassRegions, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysSkull, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysAcPcMarkers, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysActiveElements, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysDisabledElements, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysTracts, MenuItemClickedCommand) { IsToggle = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.OverlaysGrid, MenuItemClickedCommand) { IsToggle = true }
                }) { IsToggle = true },
                new ToolbarMenuHeaderViewModel(ToolbarMenuItemType.Delete, MenuItemClickedCommand, new ObservableCollection<ToolbarMenuItemViewModel>(){
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.Delete, MenuItemClickedCommand),
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.DeleteAll, MenuItemClickedCommand)
                }),
                new ToolbarMenuHeaderViewModel(ToolbarMenuItemType.CompareSwipeOpacity, MenuItemClickedCommand, new ObservableCollection<ToolbarMenuItemViewModel>()
                {
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.CompareSwipeOpacity, MenuItemClickedCommand) { IsSelectable = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.ComparePlay, MenuItemClickedCommand) { IsSelectable = true },
                    new ToolbarMenuItemViewModel(ToolbarMenuItemType.CompareFlicker, MenuItemClickedCommand) { IsSelectable = true }
                }) { IsSelectable = true },
                new ToolbarMenuHeaderViewModel(ToolbarMenuItemType.CompareLink, MenuItemClickedCommand, null) { IsVisible = false },
                new ToolbarMenuHeaderViewModel(ToolbarMenuItemType.CompareShowHide, MenuItemClickedCommand, null) { IsVisible = false, IsToggle = true },
                new ToolbarMenuHeaderViewModel(ToolbarMenuItemType.CompareColor, MenuItemClickedCommand, null) { IsVisible = false, IsToggle = true },
                new ToolbarMenuHeaderViewModel(ToolbarMenuItemType.CompareClearImages, MenuItemClickedCommand, null) { IsVisible = false }
            };

            MenuHeaders = menuHeaders;
        }

        private void NotifyAllItems()
        {
            if (MenuHeaders != null)
            {
                foreach (var menuHeader in MenuHeaders)
                    menuHeader.NotifyAll();
            }
        }

        public ICommand CallbackCommand { get; private set; }
        public ICommand MenuItemClickedCommand { get; set; }
        private void MenuItemClicked(ToolbarMenuItemCallbackData callbackData)
        {
            CallbackCommand?.Execute(callbackData);
        }

        public void SetMenuItemCheckedStatus(ToolbarMenuItemType menuItemType, bool isChecked)
        {
            foreach (var menuHeader in MenuHeaders)
            {
                if (menuHeader.MenuItems != null && menuHeader.MenuItems.Any())
                {
                    var menuItem = menuHeader.MenuItems.FirstOrDefault(x => x.MenuItemType == menuItemType);
                    if (menuItem != null)
                    {
                        menuItem.IsChecked = isChecked;
                        return;
                    }
                }
            }
        }

        public void SetMenuHeaderCheckedStatus(ToolbarMenuItemType menuItemType, bool isChecked)
        {
            foreach (var menuHeader in MenuHeaders)
            {
                if (menuHeader.MenuItemType == menuItemType)
                {
                    menuHeader.IsChecked = isChecked;
                    return;
                }
            }
        }

        public void SetMenuItemCallbackCommand(ToolbarMenuItemType menuItemType, ICommand callbackCommand)
        {
            foreach (var menuHeader in MenuHeaders)
            {
                if (menuHeader.MenuItems != null && menuHeader.MenuItems.Any())
                {
                    var menuItem = menuHeader.MenuItems.FirstOrDefault(x => x.MenuItemType == menuItemType);
                    if (menuItem != null)
                    {
                        menuItem.CallbackCommand = callbackCommand;
                        return;
                    }
                }
            }
        }

        public void SetMenuHeaderCallbackCommand(ToolbarMenuItemType menuItemType, ICommand callbackCommand)
        {
            foreach (var menuHeader in MenuHeaders)
            {
                if (menuHeader.MenuItemType == menuItemType)
                {
                    menuHeader.CallbackCommand = callbackCommand;
                    return;
                }
            }
        }
    }
}
