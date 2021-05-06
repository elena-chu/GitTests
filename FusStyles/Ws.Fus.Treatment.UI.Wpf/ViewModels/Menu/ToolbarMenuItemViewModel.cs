using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Ws.Extensions.Mvvm.Commands;
using Ws.Extensions.Mvvm.ViewModels;

namespace Ws.Fus.Treatment.UI.Wpf.ViewModels
{
    public class ToolbarCallbackData
    {
        public ToolbarCallbackData(ToolbarMenuItemType menuItemType)
        {
            MenuItemType = menuItemType;
        }

        public ToolbarMenuItemType MenuItemType { get; set; }
        public bool IsChecked { get; set; } = true;
        public bool IsHeader { get; set; } = false;
    }

    public class ToolbarMenuItemViewModel : ViewModelBase
    {
        public ToolbarMenuItemViewModel(ToolbarMenuItemType menuItemType, ICommand callbackCommand)
        {
            _callbackData = new ToolbarCallbackData(menuItemType);
            CallbackCommand = callbackCommand;
            MenuItemClickedCommand = new RelayCommand(MenuItemClicked);
            NotifyAll();
        }

        public ToolbarMenuItemType MenuItemType { get { return _callbackData.MenuItemType; } }

        protected void SetMenuItemType(ToolbarMenuItemType menuItemType)
        {
            _callbackData.MenuItemType = menuItemType;
            NotifyAll();
        }

        public virtual string Caption { get { return MenuItemType.Caption(false); } }
        public virtual UIElement Icon { get { return MenuItemType.Icon(IsChecked); } }
        public virtual bool IsToggle { get { return MenuItemType.IsToggle(false); } }

        public bool IsChecked
        {
            get { return _callbackData.IsChecked; }
            set
            {
                _callbackData.IsChecked = value;
                MenuItemClicked();
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Icon));
            }
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSelectable { get; set; } = false;

        protected ToolbarCallbackData _callbackData;

        public EventHandler Selected;

        public ICommand CallbackCommand { get; set; }
        public ICommand MenuItemClickedCommand { get; set; }
        private void MenuItemClicked()
        {
            CallbackCommand?.Execute(_callbackData);
            if (IsSelectable)
                Selected?.Invoke(this, new EventArgs());
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChanged();
            }
        }

        public virtual void NotifyAll()
        {
            RaisePropertyChanged(nameof(Caption));
            RaisePropertyChanged(nameof(IsToggle));
            RaisePropertyChanged(nameof(Icon));
        }
    }

    public class ToolbarMenuHeaderViewModel : ToolbarMenuItemViewModel
    {
        public ToolbarMenuHeaderViewModel(ToolbarMenuItemType menuItemType, ICommand command, ObservableCollection<ToolbarMenuItemViewModel> menuItems)
            : base(menuItemType, command)
        {
            _callbackData.IsHeader = true;
            if (menuItems != null && menuItems.Count > 0)
            {
                MenuItems = menuItems;
                foreach (var menuItem in MenuItems)
                {
                    if (menuItem.IsSelectable)
                        menuItem.Selected += (sender, args) => OnMenuItemSelected(sender as ToolbarMenuItemViewModel);
                }
            }
        }

        public override string Caption { get { return MenuItemType.Caption(true); } }
        public override bool IsToggle { get { return MenuItemType.IsToggle(true); } }

        private ObservableCollection<ToolbarMenuItemViewModel> _menuItems;
        public ObservableCollection<ToolbarMenuItemViewModel> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                RaisePropertyChanged();
            }
        }

        private void OnMenuItemSelected(ToolbarMenuItemViewModel menuItem)
        {
            SetMenuItemType(menuItem.MenuItemType);
            CallbackCommand = menuItem.CallbackCommand;
            IsChecked = true;
            base.NotifyAll();
        }

        public void SetMenuItemCheckedStatus(ToolbarMenuItemType menuItemType, bool isChecked)
        {
            if (MenuItems != null)
            {
                var menuItem = MenuItems.FirstOrDefault(x => x.MenuItemType == menuItemType);
                if (menuItem != null)
                    menuItem.IsChecked = isChecked;
            }
        }

        public override void NotifyAll()
        {
            base.NotifyAll();
            if (MenuItems != null)
            {
                foreach (var menuItem in MenuItems)
                    menuItem.NotifyAll();
            }
        }
    }
}
