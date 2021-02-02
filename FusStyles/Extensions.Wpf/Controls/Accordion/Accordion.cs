using System;
using System.Collections.Generic;
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
    ///     <MyNamespace:Accordion/>
    ///
    /// </summary>
    public class Accordion : ItemsControl
    {
        public static readonly DependencyProperty ExpandDirectionProperty =
        DependencyProperty.RegisterAttached("ExpandDirection", typeof(ExpandDirection), typeof(Accordion), new FrameworkPropertyMetadata(ExpandDirection.Down));

        public static readonly DependencyProperty InitialOpenItemBehaviourProperty =
        DependencyProperty.Register("InitialOpenItemBehaviour", typeof(OpenItemBehaviour), typeof(Accordion), new FrameworkPropertyMetadata(OpenItemBehaviour.FirstOpen));

        public static readonly DependencyProperty IsExpanderBehaviourProperty =
        DependencyProperty.Register("IsExpanderBehaviour", typeof(bool), typeof(Accordion), new FrameworkPropertyMetadata(false, OnIsExpanderBehaviourPropertyChanged));

        public static readonly DependencyProperty ShowArrowProperty =
        DependencyProperty.Register("ShowArrow", typeof(bool), typeof(Accordion), new FrameworkPropertyMetadata(true, OnShowArrowPropertyChanged));

        static Accordion()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Accordion), new FrameworkPropertyMetadata(typeof(Accordion)));
        }

        public Accordion()
        {
            Loaded += OnControlLoaded;
        }

        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }

        public OpenItemBehaviour InitialOpenItemBehaviour
        {
            get { return (OpenItemBehaviour)GetValue(InitialOpenItemBehaviourProperty); }
            set { SetValue(InitialOpenItemBehaviourProperty, value); }
        }

        public bool IsExpanderBehaviour
        {
            get { return (bool)GetValue(IsExpanderBehaviourProperty); }
            set { SetValue(IsExpanderBehaviourProperty, value); }
        }

        public bool ShowArrow
        {
            get { return (bool)GetValue(ShowArrowProperty); }
            set
            {
                SetValue(ShowArrowProperty, value);
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //var factoryPanel = new FrameworkElementFactory(typeof(StackPanel));
            //factoryPanel.SetValue(StackPanel.IsItemsHostProperty, true);

            //if (ExpandDirection == ExpandDirection.Up || ExpandDirection == ExpandDirection.Down)
            //{
            //    factoryPanel.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
            //}
            //else
            //{
            //    factoryPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            //}

            //var template = new ItemsPanelTemplate
            //{
            //    VisualTree = factoryPanel
            //};
            //ItemsPanel = template;
        }

        private static void OnIsExpanderBehaviourPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Accordion;
            control.InitialOpenItemBehaviour = OpenItemBehaviour.AllOpen;
            foreach (var item in control.Items)
            {
                var accordionItem = item as AccordionItem;
                if (accordionItem != null)
                {
                    accordionItem.IsExpanderBehaviour = (bool)e.NewValue;
                }
            }
        }

        private static void OnShowArrowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Accordion;
            foreach (var item in control.Items)
            {
                var accordionItem = item as AccordionItem;
                if (accordionItem != null)
                {
                    accordionItem.ArrowVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        void OnAccordionItemCollapsed(AccordionItem collapsedItem)
        {
            if (IsExpanderBehaviour)
                return;

            foreach (var item in Items)
            {
                var accordionItem = item as AccordionItem;
                if (accordionItem != null && collapsedItem != accordionItem && accordionItem.IsExpanded)
                {
                    return;
                }
                collapsedItem.Expand();
            }
        }

        void OnAccordionItemExpanded(AccordionItem expandedItem)
        {
            if (IsExpanderBehaviour)
                return;

            foreach (var item in Items)
            {
                var accordionItem = item as AccordionItem;
                if (accordionItem != null && expandedItem != accordionItem && accordionItem.IsExpanded)
                {
                    accordionItem.Collapse();
                }
            }
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {

            var nonAccordionItems = new List<object>();
            foreach (var item in Items)
            {
                var accordionItem = item as AccordionItem;
                if (accordionItem != null)
                {
                    RegisterAccordionItem(accordionItem);
                }
                else
                {
                    nonAccordionItems.Add(item);
                }
            }

            foreach (var item in nonAccordionItems)
            {
                Items.Remove(item); // remove from items

                // create accordion item for it
                var newAccordionItem = new AccordionItem
                {
                    Content = item,
                    Header = new TextBlock()
                };
                Items.Add(newAccordionItem);
                RegisterAccordionItem(newAccordionItem);
            }

            if (InitialOpenItemBehaviour == OpenItemBehaviour.FirstOpen)
                Items.Cast<AccordionItem>().First().Expand();
        }

        private void RegisterAccordionItem(AccordionItem item)
        {
            item.ArrowVisibility = ShowArrow ? Visibility.Visible : Visibility.Hidden;
            item.IsExpanderBehaviour = IsExpanderBehaviour;

            if (IsExpanderBehaviour && InitialOpenItemBehaviour == OpenItemBehaviour.AllOpen)
                item.Expand();
            else
                item.Collapse();

            item.OnExpandedEvent += OnAccordionItemExpanded;
            item.OnCollapsedEvent += OnAccordionItemCollapsed;
        }
    }
}
