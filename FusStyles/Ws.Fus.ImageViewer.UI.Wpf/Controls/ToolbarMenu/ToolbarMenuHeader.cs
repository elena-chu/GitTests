using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ws.Extensions.UI.Wpf.Behaviors;
using Ws.Fus.ImageViewer.Interfaces.Entities;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    public class ToolbarMenuHeader : MenuItem, IDisposable
    {
        #region Start, End

        public ToolbarMenuHeader()
        {
            Loaded += OnLoad;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            UpdateCheckedItems();
            InitSelectedItem();
            RegisterItemsClick();
        }

        public void Dispose()
        {
            UnregisterItemsClick();
        }

        private void RegisterItemsClick()
        {
            Items.Cast<ToolbarMenuItem>().ToList().ForEach(x => x.Click += MenuItemClicked);
        }

        private void UnregisterItemsClick()
        {
            Items.Cast<ToolbarMenuItem>().ToList().ForEach(x => x.Click -= MenuItemClicked);
        }

        #endregion


        #region Checked Items

        private Dictionary<ToolbarMenuItem, bool> _itemsPreviousCheckStatus = null;
        private void UpdateCheckedItems()
        {
            if (Items != null && !Items.IsEmpty && _itemsPreviousCheckStatus == null)
                _itemsPreviousCheckStatus = new Dictionary<ToolbarMenuItem, bool>();

            _itemsPreviousCheckStatus = Items.Cast<ToolbarMenuItem>().Where(x => x.IsCheckable).ToDictionary(x => x, x => x.IsChecked);
        }

        #endregion


        #region Click


        protected virtual void MenuItemClicked(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as ToolbarMenuItem;
            if (menuItem == null)
                return;

            if (menuItem.IsCheckable)
                UpdateCheckedItems();

            if (menuItem.IsSelectable)
                SetSelectedItem(menuItem);
        }

        #endregion


        #region Selected Item

        private ToolbarMenuItem _selectedItem;

        private void InitSelectedItem()
        {
            if (Items != null && !Items.IsEmpty)
                _selectedItem = Items.Cast<ToolbarMenuItem>().FirstOrDefault(x => x.IsSelectable);
        }

        private void SetSelectedItem(ToolbarMenuItem item)
        {
            _selectedItem = item;
            SetValue(IconedButton.IconProperty, _selectedItem.GetValue(IconedButton.IconProperty));
            Command = _selectedItem.Command;
        }


        #endregion


        // Cannot use MenuItem's IsCheckable because then it wouldn't be allowed to present sub-Items
        public bool IsToggle
        {
            get { return (bool)GetValue(IsToggleProperty); }
            set { SetValue(IsToggleProperty, value); }
        }
        public static readonly DependencyProperty IsToggleProperty = DependencyProperty.Register("IsToggle", typeof(bool), typeof(ToolbarMenuHeader), new PropertyMetadata(false));


    }
}
