using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Ws.Fus.ImageViewer.Interfaces.Entities;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    public class ToolbarMenuHeaderCompare : ToolbarMenuHeader, INotifyPropertyChanged
    {
        private CompareMode _compareMode = CompareMode.Simple;
        public CompareMode CompareMode
        {
            get { return _compareMode; }
            set
            {
                _compareMode = value;
                OnPropertyChanged();
            }
        }


        protected override void MenuItemClicked(object sender, RoutedEventArgs e)
        {
            base.MenuItemClicked(sender, e);

            var menuItem = sender as ToolbarMenuItem;
            if (menuItem == null)
                return;

            CompareMode mode = CompareMode.Simple;
            if (menuItem.CommandParameter != null)
                mode = (CompareMode)Enum.Parse(typeof(CompareMode), menuItem.CommandParameter.ToString());
            CompareMode = mode;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
