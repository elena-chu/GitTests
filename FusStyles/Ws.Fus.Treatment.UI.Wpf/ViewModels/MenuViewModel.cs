using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Ws.Extensions.Mvvm.Commands;
using Ws.Extensions.Mvvm.ViewModels;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    public interface IMenuViewModel
    {
        void SetMenuItemCheckedStatus(MenuItemType menuItemType, bool isChecked);
        void SetMenuHeaderCheckedStatus(MenuItemType menuItemType, bool isChecked);

        void SetMenuItemCallbackCommand(MenuItemType menuItemType, ICommand command);
        void SetMenuHeaderCallbackCommand(MenuItemType menuItemType, ICommand command);
    }

    public class MenuViewModel: ViewModelBase, IMenuViewModel
    {
        public MenuViewModel(ICommand callbackCommand)
        {
            CallbackCommand = callbackCommand;
            MenuItemClickedCommand = new RelayCommand<MenuItemCallbackData>(MenuItemClicked);
            BuildMenu();
        }

        private ObservableCollection<MenuHeaderViewModel> _menuHeaders;
        public ObservableCollection<MenuHeaderViewModel> MenuHeaders
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
            ObservableCollection<MenuHeaderViewModel> menuHeaders = new ObservableCollection<MenuHeaderViewModel>()
            {
                new MenuHeaderViewModel(MenuItemType.Cycle, MenuItemClickedCommand, null),
                new MenuHeaderViewModel(MenuItemType.ViewZoom, MenuItemClickedCommand, new ObservableCollection<MenuItemViewModel>()
                {
                    new MenuItemViewModel(MenuItemType.ViewZoom, MenuItemClickedCommand) { IsSelectable = true },
                    new MenuItemViewModel(MenuItemType.ViewPan, MenuItemClickedCommand) { IsSelectable = true },
                    new MenuItemViewModel(MenuItemType.ViewWindow, MenuItemClickedCommand) { IsSelectable = true },
                    new MenuItemViewModel(MenuItemType.ViewResetView, MenuItemClickedCommand),
                    new MenuItemViewModel(MenuItemType.ViewCenterOnClick, MenuItemClickedCommand) { IsSelectable = true },
                    new MenuItemViewModel(MenuItemType.ViewCenterOnTarget, MenuItemClickedCommand),
                    new MenuItemViewModel(MenuItemType.ViewScreenshot, MenuItemClickedCommand)
                }) { IsSelectable = true },
                new MenuHeaderViewModel(MenuItemType.DrawLine, MenuItemClickedCommand, new ObservableCollection<MenuItemViewModel>()
                {
                    new MenuItemViewModel(MenuItemType.DrawLine, MenuItemClickedCommand) { IsSelectable = true },
                    new MenuItemViewModel(MenuItemType.DrawArea, MenuItemClickedCommand) { IsSelectable = true },
                    new MenuItemViewModel(MenuItemType.DrawAngle, MenuItemClickedCommand) { IsSelectable = true },
                    new MenuItemViewModel(MenuItemType.DrawAngle90, MenuItemClickedCommand) { IsSelectable = true },
                    new MenuItemViewModel(MenuItemType.DrawFiducial, MenuItemClickedCommand) { IsSelectable = true },
                }) { IsSelectable = true },
                new MenuHeaderViewModel(MenuItemType.OverlaysShowHide, MenuItemClickedCommand, new ObservableCollection<MenuItemViewModel>(){
                    new MenuItemViewModel(MenuItemType.OverlaysCrossReferenceLines, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysImageInfo, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysLinesAndAreas, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysFiducials, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysTransducer, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysEnvelope, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysGhostCursor, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysNonPassRegions, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysSkull, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysAcPcMarkers, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysActiveElements, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysDisabledElements, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysTracts, MenuItemClickedCommand) { IsToggle = true },
                    new MenuItemViewModel(MenuItemType.OverlaysGrid, MenuItemClickedCommand) { IsToggle = true }
                }) { IsToggle = true },
                new MenuHeaderViewModel(MenuItemType.Delete, MenuItemClickedCommand, new ObservableCollection<MenuItemViewModel>(){
                    new MenuItemViewModel(MenuItemType.Delete, MenuItemClickedCommand),
                    new MenuItemViewModel(MenuItemType.DeleteAll, MenuItemClickedCommand)
                }),
                new MenuHeaderViewModel(MenuItemType.CompareSwipeOpacity, MenuItemClickedCommand, new ObservableCollection<MenuItemViewModel>()
                {
                    new MenuItemViewModel(MenuItemType.CompareSwipeOpacity, MenuItemClickedCommand) { IsSelectable = true },
                    new MenuItemViewModel(MenuItemType.ComparePlay, MenuItemClickedCommand) { IsSelectable = true },
                    new MenuItemViewModel(MenuItemType.CompareFlicker, MenuItemClickedCommand) { IsSelectable = true }
                }) { IsSelectable = true },
                new MenuHeaderViewModel(MenuItemType.CompareLink, MenuItemClickedCommand, null) { IsVisible = false },
                new MenuHeaderViewModel(MenuItemType.CompareShowHide, MenuItemClickedCommand, null) { IsVisible = false, IsToggle = true },
                new MenuHeaderViewModel(MenuItemType.CompareColor, MenuItemClickedCommand, null) { IsVisible = false, IsToggle = true },
                new MenuHeaderViewModel(MenuItemType.CompareClearImages, MenuItemClickedCommand, null) { IsVisible = false }
            };

            MenuHeaders = menuHeaders;
        }

        public ICommand CallbackCommand { get; private set; }
        public ICommand MenuItemClickedCommand { get; set; }
        private void MenuItemClicked(MenuItemCallbackData callbackData)
        {
            CallbackCommand?.Execute(callbackData);
        }

        public void SetMenuItemCheckedStatus(MenuItemType menuItemType, bool isChecked)
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

        public void SetMenuHeaderCheckedStatus(MenuItemType menuItemType, bool isChecked)
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

        public void SetMenuItemCallbackCommand(MenuItemType menuItemType, ICommand callbackCommand)
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

        public void SetMenuHeaderCallbackCommand(MenuItemType menuItemType, ICommand callbackCommand)
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
