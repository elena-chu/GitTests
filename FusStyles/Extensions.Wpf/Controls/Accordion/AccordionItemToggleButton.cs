using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    ///     <MyNamespace:AccordionItemToggleButton/>
    ///
    /// </summary>
    public class AccordionItemToggleButton : ToggleButton
    {
        public static readonly DependencyProperty ArrowVisibilityProperty =
            DependencyProperty.RegisterAttached("ArrowVisibility", typeof(Visibility), typeof(AccordionItemToggleButton), new FrameworkPropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty HeaderIconProperty =
            DependencyProperty.RegisterAttached("HeaderIcon", typeof(object), typeof(AccordionItemToggleButton), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(AccordionItemToggleButton), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty SelectedBackgroundProperty =
            DependencyProperty.RegisterAttached("SelectedBackground", typeof(Brush), typeof(AccordionItemToggleButton), new FrameworkPropertyMetadata(null));

        static AccordionItemToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AccordionItemToggleButton), new FrameworkPropertyMetadata(typeof(AccordionItemToggleButton)));
        }

        public AccordionItemToggleButton()
        {
            return;
        }

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
