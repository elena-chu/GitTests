using System.Windows;
using System.Windows.Controls;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    public class ToolbarMenuItem : MenuItem
    {
        public static readonly DependencyProperty ToolbarItemTypeProperty = DependencyProperty.Register("ToolbarItemType", typeof(ToolbarItemType), typeof(ToolbarMenuItem), new PropertyMetadata(ToolbarItemType.Fire));
        public ToolbarItemType ToolbarItemType
        {
            get { return (ToolbarItemType)GetValue(ToolbarItemTypeProperty); }
            set { SetValue(ToolbarItemTypeProperty, value); }
        }
    }
}
