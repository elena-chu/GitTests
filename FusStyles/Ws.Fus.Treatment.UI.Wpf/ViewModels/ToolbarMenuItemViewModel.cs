using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Ws.Extensions.Mvvm.Commands;
using Ws.Extensions.Mvvm.ViewModels;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    public enum ToolbarMenuItemType
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
        CompareFastForward,
        CompareVeryFastForward,
        CompareLink,
        CompareShowHide,
        CompareColor,
        CompareClearImages
    }

    public static class ToolbarMenuItemTypeExtension
    {
        public static string Caption(this ToolbarMenuItemType menuItemType, bool isGroupHeader)
        {
            switch (menuItemType)
            {
                case ToolbarMenuItemType.None:
                    return string.Empty;

                case ToolbarMenuItemType.Cycle:
                    return "Cycle";

                case ToolbarMenuItemType.ViewZoom:
                    return isGroupHeader ? "View" : "Zoom";
                case ToolbarMenuItemType.ViewPan:
                    return isGroupHeader ? "View" : "Pan";
                case ToolbarMenuItemType.ViewWindow:
                    return isGroupHeader ? "View" : "Window";
                case ToolbarMenuItemType.ViewResetView:
                    return "Reset View";
                case ToolbarMenuItemType.ViewCenterOnClick:
                    return isGroupHeader ? "View" : "Center on Click";
                case ToolbarMenuItemType.ViewCenterOnTarget:
                    return "Center on Target";
                case ToolbarMenuItemType.ViewScreenshot:
                    return "Screenshot";

                case ToolbarMenuItemType.DrawLine:
                    return isGroupHeader ? "Draw" : "Line";
                case ToolbarMenuItemType.DrawArea:
                    return isGroupHeader ? "Draw" : "Area";
                case ToolbarMenuItemType.DrawAngle:
                    return isGroupHeader ? "Draw" : "Angle";
                case ToolbarMenuItemType.DrawAngle90:
                    return isGroupHeader ? "Draw" : "90\u00B0 Angle";
                case ToolbarMenuItemType.DrawFiducial:
                    return isGroupHeader ? "Draw" : "Fiducial";

                case ToolbarMenuItemType.OverlaysShowHide:
                    return "Overlays";
                case ToolbarMenuItemType.OverlaysCrossReferenceLines:
                    return "Cross Reference Lines";
                case ToolbarMenuItemType.OverlaysImageInfo:
                    return "Image Info";
                case ToolbarMenuItemType.OverlaysLinesAndAreas:
                    return "Lines & Areas";
                case ToolbarMenuItemType.OverlaysFiducials:
                    return "Fiducials";
                case ToolbarMenuItemType.OverlaysTransducer:
                    return "Transducer";
                case ToolbarMenuItemType.OverlaysEnvelope:
                    return "Envelope";
                case ToolbarMenuItemType.OverlaysGhostCursor:
                    return "Ghost Cursor";
                case ToolbarMenuItemType.OverlaysNonPassRegions:
                    return "Non-Pass Regions";
                case ToolbarMenuItemType.OverlaysSkull:
                    return "Skull";
                case ToolbarMenuItemType.OverlaysAcPcMarkers:
                    return "AC-PC Markers";
                case ToolbarMenuItemType.OverlaysActiveElements:
                    return "Active Elements";
                case ToolbarMenuItemType.OverlaysDisabledElements:
                    return "Disabled Elements";
                case ToolbarMenuItemType.OverlaysTracts:
                    return "Tracts";
                case ToolbarMenuItemType.OverlaysGrid:
                    return "Grid";

                case ToolbarMenuItemType.Delete:
                    return "Delete";
                case ToolbarMenuItemType.DeleteAll:
                    return "Delete All";

                case ToolbarMenuItemType.CompareSwipeOpacity:
                    return isGroupHeader ? "Compare" : "Swipe/Opacity";
                case ToolbarMenuItemType.CompareFlicker:
                    return isGroupHeader ? "Compare" : "Flicker";
                case ToolbarMenuItemType.ComparePlay:
                case ToolbarMenuItemType.CompareFastForward:
                case ToolbarMenuItemType.CompareVeryFastForward:
                    return "Play";
                case ToolbarMenuItemType.CompareLink:
                    return "Link";
                case ToolbarMenuItemType.CompareShowHide:
                    return "Show/Hide";
                case ToolbarMenuItemType.CompareColor:
                    return "Color";
                case ToolbarMenuItemType.CompareClearImages:
                    return "Clear Images";
                
                default:
                    Debug.Assert(false, "Undefined nameof(menuItemType)");
                    return string.Empty;
            }
        }

        public static UIElement Icon(this ToolbarMenuItemType menuItemType, bool isChecked)
        {
            string resourceName = string.Empty;
            switch (menuItemType)
            {
                case ToolbarMenuItemType.None:
                    break;

                case ToolbarMenuItemType.Cycle:
                    resourceName = "XCanvas.Menu.Cycle";
                    break;

                case ToolbarMenuItemType.ViewZoom:
                    resourceName = "XCanvas.Menu.View.Zoom";
                    break;
                case ToolbarMenuItemType.ViewPan:
                    resourceName = "XCanvas.Menu.View.Pan";
                    break;
                case ToolbarMenuItemType.ViewWindow:
                    resourceName = "XCanvas.Menu.View.Window";
                    break;
                case ToolbarMenuItemType.ViewResetView:
                    resourceName = "XCanvas.Menu.View.ResetView";
                    break;
                case ToolbarMenuItemType.ViewCenterOnClick:
                    resourceName = "XCanvas.Menu.View.CenterOnClick";
                    break;
                case ToolbarMenuItemType.ViewCenterOnTarget:
                    resourceName = "XCanvas.Menu.View.CenterOnTarget";
                    break;
                case ToolbarMenuItemType.ViewScreenshot:
                    resourceName = "XCanvas.Menu.View.Screenshot";
                    break;

                case ToolbarMenuItemType.DrawLine:
                    resourceName = "XCanvas.Menu.Draw.Line";
                    break;
                case ToolbarMenuItemType.DrawArea:
                    resourceName = "XCanvas.Menu.Draw.Area";
                    break;
                case ToolbarMenuItemType.DrawAngle:
                    resourceName = "XCanvas.Menu.Draw.Angle";
                    break;
                case ToolbarMenuItemType.DrawAngle90:
                    resourceName = "XCanvas.Menu.Draw.Angle90";
                    break;
                case ToolbarMenuItemType.DrawFiducial:
                    resourceName = "XCanvas.Menu.Draw.Fiducial";
                    break;

                case ToolbarMenuItemType.OverlaysShowHide:
                    resourceName = isChecked ? "XCanvas.Menu.Overlays.Shown" : "XCanvas.Menu.Overlays.Hidden";
                    break;
                case ToolbarMenuItemType.OverlaysCrossReferenceLines:
                    resourceName = "XCanvas.Menu.Overlays.ReferenceLines";
                    break;
                case ToolbarMenuItemType.OverlaysImageInfo:
                    resourceName = "XCanvas.Menu.Overlays.Info";
                    break;
                case ToolbarMenuItemType.OverlaysLinesAndAreas:
                    resourceName = "XCanvas.Menu.Overlays.LineArea";
                    break;
                case ToolbarMenuItemType.OverlaysFiducials:
                    resourceName = "XCanvas.Menu.Overlays.Fiducials";
                    break;
                case ToolbarMenuItemType.OverlaysTransducer:
                    resourceName = "XCanvas.Menu.Overlays.Transducer";
                    break;
                case ToolbarMenuItemType.OverlaysEnvelope:
                    resourceName = "XCanvas.Menu.Overlays.Envelope";
                    break;
                case ToolbarMenuItemType.OverlaysGhostCursor:
                    resourceName = "XCanvas.Menu.Overlays.GhostCursor";
                    break;
                case ToolbarMenuItemType.OverlaysNonPassRegions:
                    resourceName = "XCanvas.Menu.Overlays.NonPassRegions";
                    break;
                case ToolbarMenuItemType.OverlaysSkull:
                    resourceName = "XCanvas.Menu.Overlays.Skull";
                    break;
                case ToolbarMenuItemType.OverlaysAcPcMarkers:
                    resourceName = "XCanvas.Menu.Overlays.AcPc";
                    break;
                case ToolbarMenuItemType.OverlaysActiveElements:
                    resourceName = "XCanvas.Menu.Overlays.ActiveElements";
                    break;
                case ToolbarMenuItemType.OverlaysDisabledElements:
                    resourceName = "XCanvas.Menu.Overlays.DisabledElements";
                    break;
                case ToolbarMenuItemType.OverlaysTracts:
                    resourceName = "XCanvas.Menu.Overlays.Tracts";
                    break;
                case ToolbarMenuItemType.OverlaysGrid:
                    resourceName = "XCanvas.Menu.Overlays.Grid";
                    break;

                case ToolbarMenuItemType.Delete:
                    resourceName = "XCanvas.Menu.Delete";
                    break;

                case ToolbarMenuItemType.DeleteAll:
                    resourceName = "XCanvas.Menu.DeleteAll";
                    break;

                case ToolbarMenuItemType.CompareSwipeOpacity:
                    resourceName = "XCanvas.Menu.Compare.SwipeOpacity";
                    break;
                case ToolbarMenuItemType.CompareFlicker:
                    resourceName = "XCanvas.Menu.Compare.Flicker";
                    break;
                case ToolbarMenuItemType.ComparePlay:
                    resourceName = "XCanvas.Menu.Compare.Play";
                    break;
                case ToolbarMenuItemType.CompareFastForward:
                    resourceName = "XCanvas.Menu.Compare.FastForward";
                    break;
                case ToolbarMenuItemType.CompareVeryFastForward:
                    resourceName = "XCanvas.Menu.Compare.VeryFastForward";
                    break;
                case ToolbarMenuItemType.CompareLink:
                    resourceName = "XCanvas.Menu.Compare.Link";
                    break;
                case ToolbarMenuItemType.CompareShowHide:
                    resourceName = isChecked ? "XCanvas.Menu.Compare.Shown" : "XCanvas.Menu.Compare.Hidden";
                    break;
                case ToolbarMenuItemType.CompareColor:
                    resourceName = "XCanvas.Menu.Compare.Color";
                    break;
                case ToolbarMenuItemType.CompareClearImages:
                    resourceName = "XCanvas.Menu.Compare.ClearImages";
                    break;

                default:
                    Debug.Assert(false, "Undefined nameof(menuItemType)");
                    break;
            }

            var resource = (UIElement)Application.Current.TryFindResource(resourceName);
            return resource.GetCopy();
        }

        public static T GetCopy<T>(this T element) where T : UIElement
        {
            using (var memoryStream = new MemoryStream())
            {
                XamlWriter.Save(element, memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (T)XamlReader.Load(memoryStream);
            }
        }
    }

    public class ToolbarMenuItemCallbackData
    {
        public ToolbarMenuItemCallbackData(ToolbarMenuItemType menuItemType, bool requestedCheckStatus)
        {
            MenuItemType = menuItemType;
            RequestedCheckStatus = requestedCheckStatus;
        }

        public ToolbarMenuItemType MenuItemType { get; set; }
        public bool RequestedCheckStatus { get; set; }
    }

    public class ToolbarMenuItemViewModel : ViewModelBase
    {
        public ToolbarMenuItemViewModel(ToolbarMenuItemType menuItemType, ICommand callbackCommand)
        {
            SetMenuItemType(menuItemType);
            CallbackCommand = callbackCommand;
            MenuItemClickedCommand = new RelayCommand(MenuItemClicked);
        }

        public ToolbarMenuItemType MenuItemType { get; protected set; } = ToolbarMenuItemType.None;
        protected void SetMenuItemType(ToolbarMenuItemType menuItemType)
        {
            MenuItemType = menuItemType;
            NotifyAll();
        }

        public virtual string Caption { get { return MenuItemType.Caption(false); } }
        public virtual UIElement Icon { get { return MenuItemType.Icon(IsChecked); } }

        private bool _isChecked = false;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Icon));
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
                CallbackCommand?.Execute(new ToolbarMenuItemCallbackData(MenuItemType, !IsChecked));
            else
                CallbackCommand?.Execute(new ToolbarMenuItemCallbackData(MenuItemType, true));

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

        public virtual void NotifyAll()
        {
            RaisePropertyChanged(nameof(Caption));
            RaisePropertyChanged(nameof(Icon));
        }
    }

    public class ToolbarMenuHeaderViewModel : ToolbarMenuItemViewModel
    {
        public ToolbarMenuHeaderViewModel(ToolbarMenuItemType menuItemType, ICommand command, ObservableCollection<ToolbarMenuItemViewModel> menuItems)
            : base(menuItemType, command)
        {
            if (menuItems != null && menuItems.Count > 0)
            {
                MenuItems = menuItems;
                foreach (var menuItem in MenuItems)
                {
                    if (menuItem.IsSelectable)
                        menuItem.Selected += (sender, args) => OnMenuItemSelected(sender as ToolbarMenuItemViewModel);
                }
            }
        }

        public override string Caption { get { return MenuItemType.Caption(true); } }

        private ObservableCollection<ToolbarMenuItemViewModel> _menuItems;
        public ObservableCollection<ToolbarMenuItemViewModel> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                RaisePropertyChanged();
            }
        }

        private void OnMenuItemSelected(ToolbarMenuItemViewModel menuItem)
        {
            SetMenuItemType(menuItem.MenuItemType);
            CallbackCommand = menuItem.CallbackCommand;
            IsToggle = menuItem.IsToggle;
        }

        public void SetMenuItemCheckedStatus(ToolbarMenuItemType menuItemType, bool isChecked)
        {
            if (MenuItems != null)
            {
                var menuItem = MenuItems.FirstOrDefault(x => x.MenuItemType == menuItemType);
                if (menuItem != null)
                    menuItem.IsChecked = isChecked;
            }
        }

        public override void NotifyAll()
        {
            base.NotifyAll();
            if (MenuItems != null)
            {
                foreach (var menuItem in MenuItems)
                    menuItem.NotifyAll();
            }
        }
    }
}
