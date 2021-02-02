using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ws.Extensions.UI.Wpf.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Ws.Extensions.UI.Wpf.Controls.Accordion"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Ws.Extensions.UI.Wpf.Controls.Accordion;assembly=Ws.Extensions.UI.Wpf.Controls.Accordion"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:AccordionItem/>
    ///
    /// </summary>
    public class AccordionItem : HeaderedContentControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ArrowVisibilityProperty =
        DependencyProperty.RegisterAttached("ArrowVisibility", typeof(Visibility), typeof(AccordionItem), new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty HeaderBackgroundProperty =
        DependencyProperty.RegisterAttached("HeaderBackground", typeof(Brush), typeof(AccordionItem), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty HeaderIconProperty =
        DependencyProperty.RegisterAttached("HeaderIcon", typeof(object), typeof(AccordionItem), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty HeaderSelectedBackgroundProperty =
        DependencyProperty.RegisterAttached("HeaderSelectedBackground", typeof(Brush), typeof(AccordionItem), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty IsExpanderBehaviourProperty =
        DependencyProperty.Register("IsExpanderBehaviour", typeof(bool), typeof(AccordionItem), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsItemExpandedProperty =
        DependencyProperty.Register("IsExpanded", typeof(bool), typeof(AccordionItem), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty ToggleCommandProperty =
        DependencyProperty.RegisterAttached("ToggleCommand", typeof(ICommand), typeof(AccordionItem), new FrameworkPropertyMetadata(null));

        private AccordionItemToggleButton _toggleButton;

        static AccordionItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AccordionItem), new FrameworkPropertyMetadata(typeof(AccordionItem)));
        }

        public AccordionItem()
        {
            ToggleCommand = new ToggleCommand(this);
            Loaded += AccordionItem_Loaded;
        }

        private void AccordionItem_Loaded(object sender, RoutedEventArgs e)
        {
            var res = Template.FindName("ABTEST", this);

            return;
        }

        public delegate void OnCollapsed(AccordionItem item);

        public delegate void OnExpanded(AccordionItem item);

        public event OnCollapsed OnCollapsedEvent;

        public event OnExpanded OnExpandedEvent;

        public event PropertyChangedEventHandler PropertyChanged;

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

        private AccordionItemToggleButton ToggleButton
        {
            get
            {                
                if (_toggleButton == null)
                    _toggleButton = Template.FindName("ExpanderButton", this) as AccordionItemToggleButton;

                return _toggleButton;
            }
        }

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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnAccordionItemControlLoaded(object sender, RoutedEventArgs e)
        {
        }
    }

    public class ToggleCommand : ICommand
    {
        private AccordionItem owner;

        public ToggleCommand(AccordionItem owner)
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
}
