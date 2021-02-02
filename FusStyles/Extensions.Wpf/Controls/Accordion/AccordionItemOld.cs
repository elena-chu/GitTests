using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ws.Extensions.UI.Wpf.Controls
{
    #region commands

    public class ToggleCommandOld : ICommand
    {
        private AccordionItemOld owner;

        public ToggleCommandOld(AccordionItemOld owner)
        {
            this.owner = owner;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter)
        {
            return /*owner.IsExpanderBehaviour ||*/ !owner.IsExpanded;
        }

        public void Execute(object parameter)
        {
            owner.OnToggle();
        }
    }

    #endregion

    public class AccordionItemOld : HeaderedContentControl, INotifyPropertyChanged
    {
        private AccordionItemToggleButton _toggleButton;
        public delegate void OnCollapsed(AccordionItemOld item);

        public delegate void OnExpanded(AccordionItemOld item);

        public event OnCollapsed OnCollapsedEvent;

        public event OnExpanded OnExpandedEvent;

        private AccordionItemToggleButton ToggleButton
        {
            get
            {
                if(_toggleButton == null)
                    _toggleButton = Template.FindName("ExpanderButton", this) as AccordionItemToggleButton;

                return _toggleButton;
            }
        }

        #region overrides

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion overrides
        public AccordionItemOld()
        {
            ToggleCommand = new ToggleCommandOld(this);
        }

        #region event handlers

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            return;

            string resourceName = "RevealExpanderTemp";
            ControlTemplate template = null;
            var appResources = Application.Current.Resources;
            if (appResources != null && appResources.Contains(resourceName))
            {
                template = appResources[resourceName] as ControlTemplate;
            }
            if (template == null)
            {
                //var resources = new ResourceDictionary
                //{
                //    Source = new Uri("/Ws.Extensions.UI.Wpf;component/Controls/Accordion/AccordionItem.xaml", UriKind.RelativeOrAbsolute)
                //};
                //this.Resources = resources;
                //template = resources[resourceName] as ControlTemplate;
                if (Debugger.IsAttached)
                    Debugger.Break();
            }
            Template = template;
        }

        private void OnAccordionItemControlLoaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        #region methods     

        internal void Collapse()
        {
            IsExpanded = false;

            if (ToggleButton != null)
            {
                ToggleButton.IsExpanded = false;
            }
        }

        internal void Expand()
        {
            IsExpanded = true;

            if (ToggleButton != null)
            {
                ToggleButton.IsExpanded = true;
            }
        }

        internal void OnToggle()
        {
            if (IsExpanded)
            {
                OnCollapsedEvent?.Invoke(this);

                Collapse();
            }
            else
            {
                OnExpandedEvent?.Invoke(this);

                Expand();
            }
        }
        #endregion

        #region properties

        public static readonly DependencyProperty ArrowVisibilityProperty =
        DependencyProperty.RegisterAttached("ArrowVisibility", typeof(Visibility), typeof(AccordionItemOld), new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty HeaderBackgroundProperty =
        DependencyProperty.RegisterAttached("HeaderBackground", typeof(Brush), typeof(AccordionItemOld), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty HeaderIconProperty =
        DependencyProperty.RegisterAttached("HeaderIcon", typeof(object), typeof(AccordionItemOld), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty HeaderSelectedBackgroundProperty =
        DependencyProperty.RegisterAttached("HeaderSelectedBackground", typeof(Brush), typeof(AccordionItemOld), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty IsExpanderBehaviourProperty =
        DependencyProperty.Register("IsExpanderBehaviour", typeof(bool), typeof(AccordionItemOld), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsItemExpandedProperty =
        DependencyProperty.RegisterAttached("IsExpanded", typeof(bool), typeof(AccordionItemOld), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty ToggleCommandProperty =
        DependencyProperty.RegisterAttached("ToggleCommand", typeof(ICommand), typeof(AccordionItemOld), new FrameworkPropertyMetadata(null));

        public Visibility ArrowVisibility
        {
            get { return (Visibility)GetValue(ArrowVisibilityProperty); }
            set { SetValue(ArrowVisibilityProperty, value); }
        }

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public object HeaderIcon
        {
            get { return GetValue(HeaderIconProperty); }
            set { SetValue(HeaderIconProperty, value); }
        }

        public Brush HeaderSelectedBackground
        {
            get { return (Brush)GetValue(HeaderSelectedBackgroundProperty); }
            set { SetValue(HeaderSelectedBackgroundProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsItemExpandedProperty); }
            private set { SetValue(IsItemExpandedProperty, value); }
        }
        public bool IsExpanderBehaviour
        {
            get { return (bool)GetValue(IsExpanderBehaviourProperty); }
            set
            {
                SetValue(IsExpanderBehaviourProperty, value);
            }
        }

        public ICommand ToggleCommand
        {
            get { return (ICommand)GetValue(ToggleCommandProperty); }
            set { SetValue(ToggleCommandProperty, value); }
        }
        #endregion properties        
    }
}
