using Prism.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Fus.ImageViewer.UI.Wpf.Controls.ToolbarMenu
{
    public class ToolbarMenuHeader : MenuItem
    {
        bool _isBulkUpdating = false;
        private List<ToolbarMenuItem> _activeSet = new List<ToolbarMenuItem>();
        private ToolbarMenuItem _selectedItem;

        #region Dependency Properties

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


        public static readonly DependencyProperty ToolbarItemTypeProperty = DependencyProperty.Register("ToolbarItemType", typeof(ToolbarItemType), typeof(ToolbarMenuHeader), new PropertyMetadata(ToolbarItemType.Fire, OnHeaderTypeChanged));
        public ToolbarItemType ToolbarItemType
        {
            get { return (ToolbarItemType)GetValue(ToolbarItemTypeProperty); }
            set { SetValue(ToolbarItemTypeProperty, value); }
        }
        private static void OnHeaderTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menuHeader = d as ToolbarMenuHeader;

            if (menuHeader.ToolbarItemType != ToolbarItemType.Fire)
            {
                //TODO: if was command binding - may be save it for further use when returning to Fire back 
                menuHeader.Command = menuHeader.HeaderClickCommand;
            }
        }

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ToolbarMenuHeader), new PropertyMetadata(false, OnActivationStateChanged));
        // Cannot use MenuItem's IsChecked because it doesn't fire when not IsCheckable (see above)
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
        #endregion

        public ToolbarMenuHeader()
        {
            HeaderClickCommand = new DelegateCommand(HeaderClickExecute);
            Loaded += OnLoad;
        }


        private DelegateCommand HeaderClickCommand;
        private void HeaderClickExecute()
        {
            bool requestedCheck = !this.IsActive;

            if (ToolbarItemType == ToolbarItemType.Select || ToolbarItemType == ToolbarItemType.SelectAndToggle)
            {
                ExecuteItemCommandByHeaderStatus(_selectedItem, requestedCheck);
            }
            else if (ToolbarItemType == ToolbarItemType.Toggle)
            {
                IEnumerable<ToolbarMenuItem> items = requestedCheck ? _activeSet : Items.Cast<ToolbarMenuItem>();

                _isBulkUpdating = true;
                foreach (var item in items)
                    ExecuteItemCommandByHeaderStatus(item, requestedCheck);

                _isBulkUpdating = false;
                UpdateActiveStatus();
            }
        }
        
        #region Start, End

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            Unloaded += OnUnload;
            UpdateActiveSet();
            InitSelectedItem();
            UpdateActiveStatus();
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

        #endregion


        #region Events handling

        private void OnCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (_isBulkUpdating)
                return;

            ToolbarMenuItem item = sender as ToolbarMenuItem;
            if (item!= null && !IsSubmenuOpen)
            {
                if (!this.IsActive)
                {
                    if (item.IsChecked)
                        _activeSet = new List<ToolbarMenuItem>() { item };
                }
                else
                    UpdateActiveSet();
            }
            UpdateActiveStatus();
        }


        protected virtual void MenuItemClicked(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as ToolbarMenuItem;
            if (menuItem == null)
                return;

            if (menuItem.IsCheckable)
                UpdateActiveSet();

            if (menuItem.ToolbarItemType == ToolbarItemType.Select || menuItem.ToolbarItemType == ToolbarItemType.SelectAndToggle)
                SetSelectedItem(menuItem);

            UpdateActiveStatus();
        }

        public delegate void Activated(ToolbarMenuHeader menuHeader);
        public event Activated ActivatedEvent;

        private void NotifyActivationState()
        {
            if (IsActive)
                ActivatedEvent?.Invoke(this);
        }

        private void ExecuteItemCommandByHeaderStatus(ToolbarMenuItem item, bool requestedCheck)
        {
            object commandParam = null;
            if(requestedCheck)
            {
                commandParam = CheckedCommandParameter != null ? CheckedCommandParameter : _selectedItem.CommandParameter;
            }
            else
                commandParam = UncheckedCommandParameter != null ? UncheckedCommandParameter : null;

            item.Command?.Execute(commandParam);
            
            //Lena: temp
            item.IsChecked = requestedCheck;
        }

        private void UpdateActiveStatus()
        {
            bool hasCheckedItems = Items.Cast<ToolbarMenuItem>().Any(x => x.IsChecked);
            this.IsActive = hasCheckedItems;
        }

        private void UpdateActiveSet() 
        {
            _activeSet = Items?.Cast<ToolbarMenuItem>().Where(x => x.IsChecked).ToList();/*.ToDictionary(x => x, x => x.IsChecked)*/;
        }

        #endregion
        
        #region Selected Item

        private void InitSelectedItem()
        {
            if (Items != null && !Items.IsEmpty)
            {
                var selectableItem = Items.Cast<ToolbarMenuItem>().FirstOrDefault(x => x.ToolbarItemType == ToolbarItemType.Select || x.ToolbarItemType == ToolbarItemType.SelectAndToggle);
                if (selectableItem != null)
                    SetSelectedItem(selectableItem);
            }
        }

        private void SetSelectedItem(ToolbarMenuItem item)
        {
            _selectedItem = item;
            ToolbarItemType = _selectedItem.ToolbarItemType;
            SetValue(IconedButton.IconProperty, _selectedItem.GetValue(IconedButton.IconProperty));
            SetValue(IconedButton.ActiveIconProperty, _selectedItem.GetValue(IconedButton.ActiveIconProperty));
            SetValue(IconedButton.InactiveIconProperty, _selectedItem.GetValue(IconedButton.InactiveIconProperty));
            //Command = _selectedItem.Command;
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
