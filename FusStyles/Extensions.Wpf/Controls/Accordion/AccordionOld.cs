using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ws.Extensions.UI.Wpf.Controls
{
    public class AccordionOld : ItemsControl
    {
        public AccordionOld()
        {
            Loaded += OnControlLoaded;
        }

        #region methods

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            var factoryPanel = new FrameworkElementFactory(typeof(StackPanel));
            factoryPanel.SetValue(StackPanel.IsItemsHostProperty, true);

            if (ExpandDirection == ExpandDirection.Up || ExpandDirection == ExpandDirection.Down)
            {
                factoryPanel.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
            }
            else
            {
                factoryPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            }

            var template = new ItemsPanelTemplate
            {
                VisualTree = factoryPanel
            };
            ItemsPanel = template;
        }

        #endregion

        #region event handlers

        void OnAccordionItemCollapsed(AccordionItemOld collapsedItem)
        {
            if (IsExpanderBehaviour)
                return;

            foreach (var item in Items)
            {
                var accordionItem = item as AccordionItemOld;
                if (accordionItem != null && collapsedItem != accordionItem && accordionItem.IsExpanded)
                {
                    return;
                }
                collapsedItem.Expand();
            }
        }

        void OnAccordionItemExpanded(AccordionItemOld expandedItem)
        {
            if (IsExpanderBehaviour)
                return;

            foreach (var item in Items)
            {
                var accordionItem = item as AccordionItemOld;
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
                var accordionItem = item as AccordionItemOld;
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
                var newAccordionItem = new AccordionItemOld
                {
                    Content = item,
                    Header = new TextBlock()
                };
                Items.Add(newAccordionItem);
                RegisterAccordionItem(newAccordionItem);
            }

            if (InitialOpenItemBehaviour == OpenItemBehaviour.FirstOpen)
                Items.Cast<AccordionItemOld>().First().Expand();
        }

        private void RegisterAccordionItem(AccordionItemOld item)
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
        #endregion

        #region properties

        public static readonly DependencyProperty ExpandDirectionProperty =
        DependencyProperty.RegisterAttached("ExpandDirection", typeof(ExpandDirection), typeof(AccordionOld), new FrameworkPropertyMetadata(ExpandDirection.Down));

        public static readonly DependencyProperty InitialOpenItemBehaviourProperty =
        DependencyProperty.Register("InitialOpenItemBehaviour", typeof(OpenItemBehaviour), typeof(AccordionOld), new FrameworkPropertyMetadata(OpenItemBehaviour.FirstOpen));

        public static readonly DependencyProperty IsExpanderBehaviourProperty =
        DependencyProperty.Register("IsExpanderBehaviour", typeof(bool), typeof(AccordionOld), new FrameworkPropertyMetadata(false, OnIsExpanderBehaviourPropertyChanged));

        public static readonly DependencyProperty ShowArrowProperty =
        DependencyProperty.Register("ShowArrow", typeof(bool), typeof(AccordionOld), new FrameworkPropertyMetadata(true, OnShowArrowPropertyChanged));

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
        private static void OnIsExpanderBehaviourPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AccordionOld;
            control.InitialOpenItemBehaviour = OpenItemBehaviour.AllOpen;
            foreach (var item in control.Items)
            {
                var accordionItem = item as AccordionItemOld;
                if (accordionItem != null)
                {
                    accordionItem.IsExpanderBehaviour = (bool)e.NewValue;
                }
            }
        }

        private static void OnShowArrowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AccordionOld;
            foreach (var item in control.Items)
            {
                var accordionItem = item as AccordionItemOld;
                if (accordionItem != null)
                {
                    accordionItem.ArrowVisibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }
        #endregion        
    }
}
