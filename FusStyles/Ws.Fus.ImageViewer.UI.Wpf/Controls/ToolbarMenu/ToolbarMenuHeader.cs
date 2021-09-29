using Prism.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ws.Extensions.UI.Wpf.Behaviors;
using System.Windows.Input;
using System;
using System.Diagnostics;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    public class ToolbarMenuHeader : MenuItem
    {
        bool _isBulkUpdating = false;


        #region General

        public static readonly DependencyProperty ToolbarItemTypeProperty = DependencyProperty.Register("ToolbarItemType", typeof(ToolbarItemType), typeof(ToolbarMenuHeader), new PropertyMetadata(ToolbarItemType.Fire));
        public ToolbarItemType ToolbarItemType
        {
            get { return (ToolbarItemType)GetValue(ToolbarItemTypeProperty); }
            set { SetValue(ToolbarItemTypeProperty, value); }
        }

        public static readonly DependencyProperty ActiveHeaderProperty = DependencyProperty.Register(nameof(ActiveHeader), typeof(object), typeof(ToolbarMenuHeader));
        public object ActiveHeader
        {
            get { return (object)GetValue(ActiveHeaderProperty); }
            set { SetValue(ActiveHeaderProperty, value); }
        }

        public static readonly DependencyProperty InactiveHeaderProperty = DependencyProperty.Register(nameof(InactiveHeader), typeof(object), typeof(ToolbarMenuHeader));
        public object InactiveHeader
        {
            get { return (object)GetValue(InactiveHeaderProperty); }
            set { SetValue(InactiveHeaderProperty, value); }
        }

        #endregion


        #region Active/Inactive State

        // Cannot use MenuItem's IsChecked because it doesn't fire when not IsCheckable (see above)
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ToolbarMenuHeader), new PropertyMetadata(false, OnActivationStateChanged));
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        private static void OnActivationStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menuHeader = d as ToolbarMenuHeader;
            if (menuHeader != null)
                menuHeader.NotifyActivationState();
        }

        public delegate void Activated(ToolbarMenuHeader menuHeader);
        public event Activated ActivatedEvent;

        private void NotifyActivationState()
        {
            if (IsActive)
                ActivatedEvent?.Invoke(this);
        }

        private void UpdateActiveStatusByCheckedItems()
        {
            if (HasCheckableItems())
                IsActive = Items.Cast<ToolbarMenuItem>().Any(x => x.IsChecked);
        }

        #endregion


        #region Header Click

        public static readonly DependencyProperty HeaderClickCommandProperty = DependencyProperty.Register(nameof(HeaderClickCommand), typeof(DelegateCommand), typeof(ToolbarMenuHeader));
        public DelegateCommand HeaderClickCommand
        {
            get { return (DelegateCommand)GetValue(HeaderClickCommandProperty); }
            set { SetValue(HeaderClickCommandProperty, value); }
        }

        private void HeaderClickExecute()
        {
            bool requestedCheck = !this.IsActive;

            if (ToolbarItemType.IsSelectable())
            {
                ExecuteItemCommandByHeaderStatus(_selectedItem, requestedCheck);
                if (ToolbarItemType.IsToggleable())
                    IsActive = requestedCheck;
            }
            else if (HasCheckableItems())
            {
                IEnumerable<ToolbarMenuItem> items = requestedCheck ? _activeSet : Items.Cast<ToolbarMenuItem>();

                _isBulkUpdating = true;
                foreach (var item in items)
                    ExecuteItemCommandByHeaderStatus(item, requestedCheck);

                _isBulkUpdating = false;
                UpdateActiveStatusByCheckedItems();
            }
        }

        private bool HeaderClickCanExecute()
        {
            if (ToolbarItemType.IsSelectable())
                return _selectedItem == null || _selectedItem.Command == null || _selectedItem.Command.CanExecute(_selectedItem.CommandParameter);

            return true;
        }

        #endregion


        #region Start, End
        
        public ToolbarMenuHeader()
        {
            HeaderClickCommand = new DelegateCommand(HeaderClickExecute, HeaderClickCanExecute);
            Loaded += OnLoad;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            Unloaded += OnUnload;
            UpdateActiveSet();
            InitSelectedItem();
            UpdateActiveStatusByCheckedItems();
            RegisterItemEvents();
        }

        public void OnUnload(object sender, RoutedEventArgs e)
        {
            UnregisterEvents();
            Unloaded -= OnUnload;
        }

        private void RegisterItemEvents()
        {
            UnregisterEvents();
            Items.Cast<ToolbarMenuItem>().ToList().ForEach(x =>
            {
                x.Click += MenuItemClicked;
                x.Checked += OnCheckedChanged;
                x.Unchecked += OnCheckedChanged;
            });
        }

        private void UnregisterEvents()
        {
            Items.Cast<ToolbarMenuItem>().ToList().ForEach(x =>
            {
                x.Click -= MenuItemClicked;
                x.Checked -= OnCheckedChanged;
                x.Unchecked -= OnCheckedChanged;
            });
        }

        private bool HasCheckableItems()
        {
            return Items.Cast<ToolbarMenuItem>().Any(x => x.IsCheckable);
        }

        #endregion


        #region Items and Active Set

        private List<ToolbarMenuItem> _activeSet = new List<ToolbarMenuItem>();

        public static readonly DependencyProperty UncheckedCommandParameterProperty = DependencyProperty.Register("UncheckedCommandParameter", typeof(object), typeof(ToolbarMenuHeader), new PropertyMetadata(null));
        public object UncheckedCommandParameter
        {
            get { return GetValue(UncheckedCommandParameterProperty); }
            set { SetValue(UncheckedCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CheckedCommandParameterProperty = DependencyProperty.Register("CheckedCommandParameter", typeof(object), typeof(ToolbarMenuHeader), new PropertyMetadata(null));
        public object CheckedCommandParameter
        {
            get { return GetValue(CheckedCommandParameterProperty); }
            set { SetValue(CheckedCommandParameterProperty, value); }
        }

        private void OnCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (_isBulkUpdating)
                return;

            ToolbarMenuItem item = sender as ToolbarMenuItem;
            if (item != null && !IsSubmenuOpen)
            {
                if (!this.IsActive)
                {
                    if (item.IsChecked)
                        _activeSet = new List<ToolbarMenuItem>() { item };
                }
                else
                    UpdateActiveSet();
            }
            UpdateActiveStatusByCheckedItems();
        }


        protected virtual void MenuItemClicked(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as ToolbarMenuItem;
            if (menuItem == null)
                return;

            if (menuItem.IsCheckable)
                UpdateActiveSet();

            if (menuItem.ToolbarItemType.IsSelectable())
                SetSelectedItem(menuItem);

            UpdateActiveStatusByCheckedItems();
        }

        private void ExecuteItemCommandByHeaderStatus(ToolbarMenuItem item, bool requestedCheck)
        {
            object commandParam = null;
            if (requestedCheck)
            {
                commandParam = CheckedCommandParameter != null ? CheckedCommandParameter : _selectedItem.CommandParameter;
            }
            else
                commandParam = UncheckedCommandParameter != null ? UncheckedCommandParameter : null;

            try
            {
                item.Command?.Execute(commandParam);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ToolbarMenuHeader {item.Header?.ToString()} item.Command?.Execute(commandParam) Exception: {ex.Message}");
            }
        }

        private void UpdateActiveSet()
        {
            _activeSet = Items?.Cast<ToolbarMenuItem>().Where(x => x.IsChecked).ToList();/*.ToDictionary(x => x, x => x.IsChecked)*/;
        }

        #endregion


        #region Selected Item

        private ToolbarMenuItem _selectedItem;

        private void InitSelectedItem()
        {
            if (Items != null && !Items.IsEmpty)
            {
                var selectableItem = Items.Cast<ToolbarMenuItem>().FirstOrDefault(x => x.ToolbarItemType.IsSelectable());
                if (selectableItem != null)
                    SetSelectedItem(selectableItem);
            }
        }

        private void SetSelectedItem(ToolbarMenuItem item)
        {
            if (_selectedItem != null && _selectedItem.Command != null)
                _selectedItem.Command.CanExecuteChanged -= OnSelectedItemCommandCanExecuteChanged; ;
            _selectedItem = item;
            if (_selectedItem != null && _selectedItem.Command != null)
                _selectedItem.Command.CanExecuteChanged += OnSelectedItemCommandCanExecuteChanged; ;

            ToolbarItemType = _selectedItem.ToolbarItemType;
            SetValue(IconedButton.IconProperty, _selectedItem.GetValue(IconedButton.IconProperty));
            SetValue(IconedButton.ActiveIconProperty, _selectedItem.GetValue(IconedButton.ActiveIconProperty));
            SetValue(IconedButton.InactiveIconProperty, _selectedItem.GetValue(IconedButton.InactiveIconProperty));
            if (_selectedItem.ToolbarItemType == ToolbarItemType.SelectCaptionToggle)
                ActiveHeader = _selectedItem.Header;
            //Command = _selectedItem.Command;

            HeaderClickCommand.RaiseCanExecuteChanged();

        }

        private void OnSelectedItemCommandCanExecuteChanged(object sender, System.EventArgs e)
        {
            HeaderClickCommand.RaiseCanExecuteChanged();
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
