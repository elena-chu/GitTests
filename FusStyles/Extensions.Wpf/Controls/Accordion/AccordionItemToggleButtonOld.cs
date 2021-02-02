using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Ws.Extensions.UI.Wpf.Controls
{
    public class AccordionItemToggleButtonOld : ToggleButton
    {
        public static readonly DependencyProperty ArrowVisibilityProperty =
        DependencyProperty.RegisterAttached("ArrowVisibility", typeof(Visibility), typeof(AccordionItemToggleButtonOld), new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty HeaderIconProperty =
        DependencyProperty.RegisterAttached("HeaderIcon", typeof(object), typeof(AccordionItemToggleButtonOld), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty IsExpandedProperty =
        DependencyProperty.RegisterAttached("IsExpanded", typeof(bool), typeof(AccordionItemToggleButtonOld), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty SelectedBackgroundProperty =
        DependencyProperty.RegisterAttached("SelectedBackground", typeof(Brush), typeof(AccordionItemToggleButtonOld), new FrameworkPropertyMetadata(null));

        public Visibility ArrowVisibility
        {
            get { return (Visibility)GetValue(ArrowVisibilityProperty); }
            set { SetValue(ArrowVisibilityProperty, value); }
        }

        public object HeaderIcon
        {
            get { return GetValue(HeaderIconProperty); }
            set { SetValue(HeaderIconProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }
        public bool IsExpanderBehaviour { get; set; }

        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }

        protected override void OnToggle()
        {
            if (!IsExpanded)
            {
                base.OnToggle();
            }
        }
    }
}
