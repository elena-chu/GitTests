using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Ws.Extensions.Mvvm.Commands;
using Ws.Extensions.Mvvm.ViewModels;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    public enum MenuItemType
    {
        None,

        Cycle,
        
        ViewZoom,
        ViewPan,
        ViewWindow,
        ViewResetView,
        ViewCenterOnClick,
        ViewCenterOnTarget,
        ViewScreenshot,

        DrawLine,
        DrawArea,
        DrawAngle,
        DrawAngle90,
        DrawFiducial,

        OverlaysShowHide,
        OverlaysCrossReferenceLines,
        OverlaysImageInfo,
        OverlaysLinesAndAreas,
        OverlaysFiducials,
        OverlaysTransducer,
        OverlaysEnvelope,
        OverlaysGhostCursor,
        OverlaysNonPassRegions,
        OverlaysSkull,
        OverlaysAcPcMarkers,
        OverlaysActiveElements,
        OverlaysDisabledElements,
        OverlaysTracts,
        OverlaysGrid,

        Delete,
        DeleteAll,

        CompareSwipeOpacity,
        CompareFlicker,
        ComparePlay,
        CompareLink,
        CompareShowHide,
        CompareColor,
        CompareClearImages
    }

    public static class MenuItemTypeExtension
    {
        public static string Caption(this MenuItemType menuItemType, bool isGroupHeader)
        {
            switch (menuItemType)
            {
                case MenuItemType.None:
                    return string.Empty;

                case MenuItemType.Cycle:
                    return "Cycle";

                case MenuItemType.ViewZoom:
                    return isGroupHeader ? "View" : "Zoom";
                case MenuItemType.ViewPan:
                    return isGroupHeader ? "View" : "Pan";
                case MenuItemType.ViewWindow:
                    return isGroupHeader ? "View" : "Window";
                case MenuItemType.ViewResetView:
                    return "Reset View";
                case MenuItemType.ViewCenterOnClick:
                    return isGroupHeader ? "View" : "Center on Click";
                case MenuItemType.ViewCenterOnTarget:
                    return "Center on Target";
                case MenuItemType.ViewScreenshot:
                    return "Screenshot";
                case MenuItemType.DrawLine:
                    return isGroupHeader ? "Draw" : "Line";
                case MenuItemType.DrawArea:
                    return isGroupHeader ? "Draw" : "Area";
                case MenuItemType.DrawAngle:
                    return isGroupHeader ? "Draw" : "Angle";
                case MenuItemType.DrawAngle90:
                    return isGroupHeader ? "Draw" : "90\u00B0 Angle";
                case MenuItemType.DrawFiducial:
                    return isGroupHeader ? "Draw" : "Fiducial";

                case MenuItemType.OverlaysShowHide:
                    return "Overlays";
                case MenuItemType.OverlaysCrossReferenceLines:
                    return "Cross Reference Lines";
                case MenuItemType.OverlaysImageInfo:
                    return "Image Info";
                case MenuItemType.OverlaysLinesAndAreas:
                    return "Lines & Areas";
                case MenuItemType.OverlaysFiducials:
                    return "Fiducials";
                case MenuItemType.OverlaysTransducer:
                    return "Transducer";
                case MenuItemType.OverlaysEnvelope:
                    return "Envelope";
                case MenuItemType.OverlaysGhostCursor:
                    return "Ghost Cursor";
                case MenuItemType.OverlaysNonPassRegions:
                    return "Non-Pass Regions";
                case MenuItemType.OverlaysSkull:
                    return "Skull";
                case MenuItemType.OverlaysAcPcMarkers:
                    return "AC-PC Markers";
                case MenuItemType.OverlaysActiveElements:
                    return "Active Elements";
                case MenuItemType.OverlaysDisabledElements:
                    return "Disabled Elements";
                case MenuItemType.OverlaysTracts:
                    return "Tracts";
                case MenuItemType.OverlaysGrid:
                    return "Grid";

                case MenuItemType.Delete:
                    return "Delete";
                case MenuItemType.DeleteAll:
                    return isGroupHeader ? "Delete" : "Delete All";

                case MenuItemType.CompareSwipeOpacity:
                    return isGroupHeader ? "Compare" : "Swipe/Opacity";
                case MenuItemType.CompareFlicker:
                    return isGroupHeader ? "Compare" : "Flicker";
                case MenuItemType.ComparePlay:
                    return "Play";
                case MenuItemType.CompareLink:
                    return "Link";
                case MenuItemType.CompareShowHide:
                    return "Show/Hide";
                case MenuItemType.CompareColor:
                    return "Color";
                case MenuItemType.CompareClearImages:
                    return "Clear Images";
                
                default:
                    Debug.Assert(false, "Undefined nameof(menuItemType)");
                    return string.Empty;
            }
        }
    }

    public class MenuItemCallbackData
    {
        public MenuItemCallbackData(MenuItemType menuItemType, bool requestedCheckStatus)
        {
            MenuItemType = menuItemType;
            RequestedCheckStatus = requestedCheckStatus;
        }

        public MenuItemType MenuItemType { get; set; }
        public bool RequestedCheckStatus { get; set; }
    }

    public class MenuItemViewModel : ViewModelBase
    {
        public MenuItemViewModel(MenuItemType menuItemType, ICommand callbackCommand)
        {
            SetMenuItemType(menuItemType);
            CallbackCommand = callbackCommand;
            MenuItemClickedCommand = new RelayCommand(MenuItemClicked);
        }

        public MenuItemType MenuItemType { get; protected set; } = MenuItemType.None;
        protected void SetMenuItemType(MenuItemType menuItemType)
        {
            MenuItemType = menuItemType;
            RaisePropertyChanged(nameof(Caption));
        }

        public virtual string Caption { get { return MenuItemType.Caption(false); } }

        private bool _isChecked = false;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged();
            }
        }

        public bool IsToggle { get; set; } = false;
        public bool IsSelectable { get; set; } = false;

        public EventHandler Selected;

        public ICommand CallbackCommand { get; set; }
        public ICommand MenuItemClickedCommand { get; set; }
        private void MenuItemClicked()
        {
            if (IsToggle)
                CallbackCommand?.Execute(new MenuItemCallbackData(MenuItemType, !IsChecked));
            else
                CallbackCommand?.Execute(new MenuItemCallbackData(MenuItemType, true));

            if (IsSelectable)
                Selected?.Invoke(this, new EventArgs());
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChanged();
            }
        }
    }

    public class MenuHeaderViewModel : MenuItemViewModel
    {
        public MenuHeaderViewModel(MenuItemType menuItemType, ICommand command, ObservableCollection<MenuItemViewModel> menuItems)
            : base(menuItemType, command)
        {
            if (menuItems != null && menuItems.Count > 0)
                MenuItems = menuItems;

            foreach (var menuItem in MenuItems)
            {
                if (menuItem.IsSelectable)
                    menuItem.Selected += (sender, args) => OnMenuItemSelected(sender as MenuItemViewModel);
            }
        }

        public override string Caption { get { return MenuItemType.Caption(true); } }

        private ObservableCollection<MenuItemViewModel> _menuItems;
        public ObservableCollection<MenuItemViewModel> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                RaisePropertyChanged();
            }
        }

        private void OnMenuItemSelected(MenuItemViewModel menuItem)
        {
            SetMenuItemType(menuItem.MenuItemType);
            CallbackCommand = menuItem.CallbackCommand;
            IsToggle = menuItem.IsToggle;
        }

        public void SetMenuItemCheckedStatus(MenuItemType menuItemType, bool isChecked)
        {
            if (MenuItems != null)
            {
                var menuItem = MenuItems.FirstOrDefault(x => x.MenuItemType == menuItemType);
                if (menuItem != null)
                    menuItem.IsChecked = isChecked;
            }
        }
    }
}
