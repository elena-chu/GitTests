namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    public enum ToolbarItemType
    {
        Fire,
        Select,
        SelectToggle,
        SelectCaptionToggle,
        Toggle
    }

    public static class ToolbarMenuExtension
    {
        public static bool IsSelectable(this ToolbarItemType toolbarItemType)
        {
            return toolbarItemType == ToolbarItemType.Select || toolbarItemType == ToolbarItemType.SelectToggle || toolbarItemType == ToolbarItemType.SelectCaptionToggle;
        }

        public static bool IsToggleable(this ToolbarItemType toolbarItemType)
        {
            return toolbarItemType == ToolbarItemType.Toggle || toolbarItemType == ToolbarItemType.SelectToggle || toolbarItemType == ToolbarItemType.SelectCaptionToggle;
        }
    }
}
