using System;
using System.Windows;
using System.Windows.Controls;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    public class ToolbarMenuItem : MenuItem
    {
        public bool IsSelectable
        {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }
        public static readonly DependencyProperty IsSelectableProperty = DependencyProperty.Register("IsSelectable", typeof(bool), typeof(ToolbarMenuItem), new PropertyMetadata(false));
    }
}
