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
        /// <summary>
        /// One of most important propertis of HeaderMenuItem.
        /// Responsible for Header Visual status(presentation) that depends on user interaction and on statuses of its SubmenuItems
        /// </summary>
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
        public event Activated ActivatedChanged;

        private void NotifyActivationState()
        {
            ActivatedChanged?.Invoke(this);
        }

        /// <summary>
        /// Decides about IsActive status of Header depending on ToolbarItemType and SubItems statuses.
        /// </summary>
        private void UpdateActiveStatusByCheckedItems()
        {
            if (HasCheckableItems() || ToolbarItemType == ToolbarItemType.SelectCaptionToggle || ToolbarItemType == ToolbarItemType.SelectToggle)
            {
                var checkedItems = Items.Cast<ToolbarMenuItem>().Where(x => x.IsChecked);
                if (ToolbarItemType == ToolbarItemType.Toggle) //Overlays
                    IsActive = checkedItems.Any(el => el.Command != null && el.Command.CanExecute(null));
                else
                    IsActive = checkedItems.Any();

                if (ToolbarItemType == ToolbarItemType.SelectCaptionToggle && _selectedItem != null)//Compare
                    ActiveHeader = _selectedItem.Header;
            }

            NotifyActivationState();
        }

        #endregion


        #region Header Click Command

        /// <summary>
        /// Inner HeaderClickCommand that used inside control template for innner bindings.
        /// This Command aim is to deside which command or action call for outside bindings
        /// </summary>
        public static readonly DependencyProperty HeaderClickCommandProperty = DependencyProperty.Register(nameof(HeaderClickCommand), typeof(DelegateCommand), typeof(ToolbarMenuHeader));
        public DelegateCommand HeaderClickCommand
        {
            get { return (DelegateCommand)GetValue(HeaderClickCommandProperty); }
            set { SetValue(HeaderClickCommandProperty, value); }
        }

        private void HeaderClickExecute()
        {
            bool requestedCheck = !this.IsActive;
            if (ToolbarItemType == ToolbarItemType.Fire)
            {
                Command?.Execute(CommandParameter);
                return;
            }

            //Check whether some of SubmenuItems defined in UI as checkable and have some UI control for change status
            //For such items are executing bulk operation
            if (HasCheckableItems())
            {
                IEnumerable<ToolbarMenuItem> items = requestedCheck ? _activeSet : Items.Cast<ToolbarMenuItem>();

                _isBulkUpdating = true;
                foreach (var item in items)
                    ExecuteItemCommandByHeaderStatus(item, requestedCheck);

                _isBulkUpdating = false;
            }

            //If Header is of type Selectable it means it represents some action
            //of selected SubmenuItem
            if (ToolbarItemType.IsSelectable())
            {
                ExecuteItemCommandByHeaderStatus(_selectedItem, requestedCheck);
            }

            UpdateActiveStatusByCheckedItems();
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
                if (x.Command != null)
                    x.Command.CanExecuteChanged += OnCommandCanExecuteChanged;
            });
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            UpdateActiveStatusByCheckedItems();
            HeaderClickCommand.RaiseCanExecuteChanged();
        }

        private void UnregisterEvents()
        {
            Items.Cast<ToolbarMenuItem>().ToList().ForEach(x =>
            {
                x.Click -= MenuItemClicked;
                x.Checked -= OnCheckedChanged;
                x.Unchecked -= OnCheckedChanged;
                if (x.Command != null)
                    x.Command.CanExecuteChanged -= OnCommandCanExecuteChanged;
            });
        }

        private bool HasCheckableItems()
        {
            return Items.Cast<ToolbarMenuItem>().Any(x => x.IsCheckable);
        }

        #endregion


        #region Items and Active Set

        /// <summary>
        /// Active set (relevant for Toggle ToolbarItemType like Overlays) defines set 
        /// on which executing bulk operation
        /// </summary>
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

        /// <summary>
        /// Handler for MenuItem checked property changed. 
        /// This event is relevant for Checkable MenuItems that have UI control for changing status
        /// as well as the regular MenuItems that have only Checked property but don't have UI option for selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (_isBulkUpdating)
                return;

            ToolbarMenuItem item = sender as ToolbarMenuItem;
            if (item == null)
                return;

            /* Overlay menu has different behavior than other menus- its type is Toggle 
             * and it toggles action for whole group od items that user defines when menu is open.
             * When menu is closed some background actions can happend (coming from Fus etc.)
             * and can influence the header status
             * */
            if (ToolbarItemType == ToolbarItemType.Toggle && item.IsCheckable) //Overlay
            {
                if (IsSubmenuOpen)
                {
                    if (!this.IsActive)
                    {
                        //If changed item check when menu is open and whole menu status is Hidden
                        //It means that user creates the new set for futher group action
                        if (item.IsCheckable && item.IsChecked)
                            _activeSet = new List<ToolbarMenuItem>() { item };
                    }
                    else
                        UpdateActiveSet();
                }
                else //background actions while menu is closed
                {
                    //Add to _activeSet for futher usage in bulk show/hide
                    if (item.IsChecked && !_activeSet.Contains(item))
                        _activeSet.Add(item);
                    else if (!item.IsChecked && !_activeSet.Contains(item))
                        _activeSet.Remove(item);
                }
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

        /// <summary>
        /// Acion which decides which Command and with what CommandParameter to call
        /// depending on selected item and requested action for Header
        /// </summary>
        /// <param name="item">Current selected item</param>
        /// <param name="requestedCheck">Requested action for Header</param>
        private void ExecuteItemCommandByHeaderStatus(ToolbarMenuItem item, bool requestedCheck)
        {
            if (item == null || item.Command == null)
                return;

            object commandParam = null;
            if (requestedCheck)
            {
                commandParam = CheckedCommandParameter != null ? CheckedCommandParameter : _selectedItem.CommandParameter;
            }
            else
                commandParam = UncheckedCommandParameter != null ? UncheckedCommandParameter : null;

            try
            {
                item.Command.Execute(commandParam);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ToolbarMenuHeader {item.Header?.ToString()} item.Command?.Execute(commandParam) Exception: {ex.Message}");
            }
        }

        private void UpdateActiveSet()
        {
            _activeSet = Items?.Cast<ToolbarMenuItem>().Where(x => x.IsCheckable && x.IsChecked).ToList();/*.ToDictionary(x => x, x => x.IsChecked)*/;
        }

        #endregion


        #region Selected Item

        private ToolbarMenuItem _selectedItem;

        private void InitSelectedItem()
        {
            if (Items != null && !Items.IsEmpty)
            {
                var selectableItems = Items.Cast<ToolbarMenuItem>().Where(x => x.ToolbarItemType.IsSelectable());
                var selectableItem = selectableItems.FirstOrDefault(el => el.IsChecked) ?? selectableItems.FirstOrDefault();
                if (selectableItem != null)
                    SetSelectedItem(selectableItem);
            }
        }

        /// <summary>
        /// Is Header is of type Selectable it means that it should visually represent selected tem
        /// This function updates visual properties of Header by Selected item
        /// </summary>
        /// <param name="item"></param>
        private void SetSelectedItem(ToolbarMenuItem item)
        {
            _selectedItem = item;
            if (_selectedItem == null)
            {
                HeaderClickCommand.RaiseCanExecuteChanged();
                return;
            }

            SetValue(IconedButton.IconProperty, _selectedItem.GetValue(IconedButton.IconProperty));
            SetValue(IconedButton.ActiveIconProperty, _selectedItem.GetValue(IconedButton.ActiveIconProperty));
            SetValue(IconedButton.InactiveIconProperty, _selectedItem.GetValue(IconedButton.InactiveIconProperty));
            if (_selectedItem.ToolbarItemType == ToolbarItemType.SelectCaptionToggle)
                ActiveHeader = _selectedItem.Header;

            HeaderClickCommand.RaiseCanExecuteChanged();
        }

        #endregion

        //Not in use
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
