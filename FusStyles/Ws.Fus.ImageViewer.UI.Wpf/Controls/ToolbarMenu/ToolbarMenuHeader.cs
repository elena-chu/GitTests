using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    public class ToolbarMenuHeader : MenuItem
    {
        #region Start, End

        public ToolbarMenuHeader()
        {
            Loaded += OnLoad;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            Unloaded += OnUnload;
            UpdateCheckedItems();
            InitSelectedItem();
            RegisterClicks();
        }

        public void OnUnload(object sender, RoutedEventArgs e)
        {
            UnregisterClicks();
            Unloaded -= OnUnload;
        }

        private void RegisterClicks()
        {
            PreviewMouseDown += OnPreviewMouseDown;
            Items.Cast<ToolbarMenuItem>().ToList().ForEach(x => x.Click += MenuItemClicked);
        }

        private void UnregisterClicks()
        {
            PreviewMouseDown -= OnPreviewMouseDown;
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


        #region Clicks

        private void OnPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsActive = !IsActive;
            if (IsActive)
                ActivatedEvent?.Invoke(this);
            e.Handled = false;
        }

        public delegate void Activated(ToolbarMenuHeader menuHeader);
        public event Activated ActivatedEvent;

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
            {
                var selectableItem = Items.Cast<ToolbarMenuItem>().FirstOrDefault(x => x.IsSelectable);
                if (selectableItem != null)
                    SetSelectedItem(selectableItem);
            }
        }

        private void SetSelectedItem(ToolbarMenuItem item)
        {
            _selectedItem = item;
            SetValue(IconedButton.IconProperty, _selectedItem.GetValue(IconedButton.IconProperty));
            Command = _selectedItem.Command;
        }


        #endregion


        #region States

        // Cannot use MenuItem's IsCheckable because then it wouldn't be allowed to present sub-Items
        public bool IsToggle
        {
            get { return (bool)GetValue(IsToggleProperty); }
            set { SetValue(IsToggleProperty, value); }
        }
        public static readonly DependencyProperty IsToggleProperty = DependencyProperty.Register("IsToggle", typeof(bool), typeof(ToolbarMenuHeader), new PropertyMetadata(false));

        // Cannot use MenuItem's IsChecked because it doesn't fire when not IsCheckable (see above)
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ToolbarMenuHeader), new PropertyMetadata(false, OnActivationStateChanged));

        private static void OnActivationStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion


        #region Exclusive Group

        public bool MemberOfMutuallyExclusiveGroup
        {
            get { return (bool)GetValue(MemberOfMutuallyExclusiveGroupProperty); }
            set { SetValue(MemberOfMutuallyExclusiveGroupProperty, value); }
        }
        public static readonly DependencyProperty MemberOfMutuallyExclusiveGroupProperty = DependencyProperty.Register("MemberOfMutuallyExclusiveGroup", typeof(bool), typeof(ToolbarMenuHeader), new PropertyMetadata(false));

        public void InactivateExclusiveGroupMember()
        {
            if (MemberOfMutuallyExclusiveGroup && IsActive)
                IsActive = false;
        }

        #endregion
    }
}
