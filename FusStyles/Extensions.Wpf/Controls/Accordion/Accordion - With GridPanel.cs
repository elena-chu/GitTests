using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ws.Extensions.UI.Wpf.Controls
{
    public class Accordion : ItemsControl
    {
        public Accordion()
        {
            Loaded += OnControlLoaded;
        }

        #region methods

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //var factoryPanel = new FrameworkElementFactory(typeof(Grid));
            //factoryPanel.SetValue(Grid.IsItemsHostProperty, true);
            //factoryPanel.SetValue(HeightProperty, Double.NaN);
            //factoryPanel.SetValue(VerticalAlignmentProperty, VerticalAlignment.Top);

            //for (int i = 0; i<10; i++)
            //{
            //    FrameworkElementFactory row = new FrameworkElementFactory(typeof(RowDefinition));
            //    row.SetValue(RowDefinition.HeightProperty, new GridLength(1, GridUnitType.Auto));
            //    factoryPanel.AppendChild(row);
            //}

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

            int i = 0;
            foreach(AccordionItem item in Items.Cast<AccordionItem>())
            {
                item.SetValue(Grid.RowProperty, i);
                i++;
            }
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

        void OnAccordionItemExpanded(AccordionItem expandedItem)
        {
            if (!IsExpanderBehaviour)
            {
                foreach (var item in Items)
                {
                    var accordionItem = item as AccordionItem;
                    if (accordionItem != null)
                    {
                        if (expandedItem != accordionItem)
                        {
                            if (accordionItem.IsExpanded)
                            {
                                accordionItem.Collapse();
                            }
                        }
                    }
                }
            }

        }

        void OnAccordionItemCollapsed(AccordionItem collapsedItem)
        {
            if(!IsExpanderBehaviour)
            {
                foreach (var item in Items)
                {
                    var accordionItem = item as AccordionItem;
                    if (accordionItem != null)
                    {
                        if (collapsedItem != accordionItem)
                        {
                            if (accordionItem.IsExpanded)
                            {
                                return;
                            }
                        }
                    }
                }

                collapsedItem.Expand();
            }
        }

        #endregion

        #region properties

        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }
        public static readonly DependencyProperty ExpandDirectionProperty =
        DependencyProperty.RegisterAttached("ExpandDirection", typeof(ExpandDirection), typeof(Accordion), new FrameworkPropertyMetadata(ExpandDirection.Down));

        public bool ShowArrow
        {
            get { return (bool)GetValue(ShowArrowProperty); }
            set
            {
                SetValue(ShowArrowProperty, value);
            }
        }
        public static readonly DependencyProperty ShowArrowProperty =
        DependencyProperty.Register("ShowArrow", typeof(bool), typeof(Accordion), new FrameworkPropertyMetadata(true, OnShowArrowPropertyChanged));

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

        public bool IsExpanderBehaviour
        {
            get { return (bool)GetValue(IsExpanderBehaviourProperty); }
            set { SetValue(IsExpanderBehaviourProperty, value); }
        }
        public static readonly DependencyProperty IsExpanderBehaviourProperty =
        DependencyProperty.Register("IsExpanderBehaviour", typeof(bool), typeof(Accordion), new FrameworkPropertyMetadata(false, OnIsExpanderBehaviourPropertyChanged));

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

        public OpenItemBehaviour InitialOpenItemBehaviour
        {
            get { return (OpenItemBehaviour)GetValue(InitialOpenItemBehaviourProperty); }
            set { SetValue(InitialOpenItemBehaviourProperty, value); }
        }
        public static readonly DependencyProperty InitialOpenItemBehaviourProperty =
        DependencyProperty.Register("InitialOpenItemBehaviour", typeof(OpenItemBehaviour), typeof(Accordion), new FrameworkPropertyMetadata(OpenItemBehaviour.FirstOpen));

        #endregion        
    }
}
