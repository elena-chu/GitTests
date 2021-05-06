using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Markup;

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
        public static string Caption(this ToolbarMenuItemType menuItemType, bool isHeader)
        {
            switch (menuItemType)
            {
                case ToolbarMenuItemType.None:
                    return string.Empty;

                case ToolbarMenuItemType.Cycle:
                    return "Cycle";

                case ToolbarMenuItemType.ViewZoom:
                    return isHeader ? "View" : "Zoom";
                case ToolbarMenuItemType.ViewPan:
                    return isHeader ? "View" : "Pan";
                case ToolbarMenuItemType.ViewWindow:
                    return isHeader ? "View" : "Window";
                case ToolbarMenuItemType.ViewResetView:
                    return "Reset View";
                case ToolbarMenuItemType.ViewCenterOnClick:
                    return isHeader ? "View" : "Center on Click";
                case ToolbarMenuItemType.ViewCenterOnTarget:
                    return "Center on Target";
                case ToolbarMenuItemType.ViewScreenshot:
                    return "Screenshot";

                case ToolbarMenuItemType.DrawLine:
                    return isHeader ? "Draw" : "Line";
                case ToolbarMenuItemType.DrawArea:
                    return isHeader ? "Draw" : "Area";
                case ToolbarMenuItemType.DrawAngle:
                    return isHeader ? "Draw" : "Angle";
                case ToolbarMenuItemType.DrawAngle90:
                    return isHeader ? "Draw" : "90\u00B0 Angle";
                case ToolbarMenuItemType.DrawFiducial:
                    return isHeader ? "Draw" : "Fiducial";

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
                    return isHeader ? "Compare" : "Swipe/Opacity";
                case ToolbarMenuItemType.CompareFlicker:
                    return isHeader ? "Compare" : "Flicker";
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

        public static bool IsToggle(this ToolbarMenuItemType menuItemType, bool isHeader)
        {
            switch (menuItemType)
            {
                case ToolbarMenuItemType.None:

                case ToolbarMenuItemType.Cycle:

                case ToolbarMenuItemType.ViewResetView:
                case ToolbarMenuItemType.ViewCenterOnTarget:
                case ToolbarMenuItemType.ViewScreenshot:

                case ToolbarMenuItemType.Delete:
                case ToolbarMenuItemType.DeleteAll:

                case ToolbarMenuItemType.CompareClearImages:
                    return false;

                case ToolbarMenuItemType.OverlaysShowHide:
                case ToolbarMenuItemType.OverlaysCrossReferenceLines:
                case ToolbarMenuItemType.OverlaysImageInfo:
                case ToolbarMenuItemType.OverlaysLinesAndAreas:
                case ToolbarMenuItemType.OverlaysFiducials:
                case ToolbarMenuItemType.OverlaysTransducer:
                case ToolbarMenuItemType.OverlaysEnvelope:
                case ToolbarMenuItemType.OverlaysGhostCursor:
                case ToolbarMenuItemType.OverlaysNonPassRegions:
                case ToolbarMenuItemType.OverlaysSkull:
                case ToolbarMenuItemType.OverlaysAcPcMarkers:
                case ToolbarMenuItemType.OverlaysActiveElements:
                case ToolbarMenuItemType.OverlaysDisabledElements:
                case ToolbarMenuItemType.OverlaysTracts:
                case ToolbarMenuItemType.OverlaysGrid:

                case ToolbarMenuItemType.CompareFastForward:
                case ToolbarMenuItemType.CompareVeryFastForward:
                case ToolbarMenuItemType.CompareLink:
                case ToolbarMenuItemType.CompareShowHide:
                case ToolbarMenuItemType.CompareColor:
                    return true;

                case ToolbarMenuItemType.ViewZoom:
                case ToolbarMenuItemType.ViewPan:
                case ToolbarMenuItemType.ViewWindow:
                case ToolbarMenuItemType.ViewCenterOnClick:

                case ToolbarMenuItemType.DrawLine:
                case ToolbarMenuItemType.DrawArea:
                case ToolbarMenuItemType.DrawAngle:
                case ToolbarMenuItemType.DrawAngle90:
                case ToolbarMenuItemType.DrawFiducial:

                case ToolbarMenuItemType.CompareSwipeOpacity:
                case ToolbarMenuItemType.CompareFlicker:
                case ToolbarMenuItemType.ComparePlay:
                    return isHeader;

                default:
                    Debug.Assert(false, "Undefined nameof(menuItemType)");
                    return false;
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
}
