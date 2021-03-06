using System.Windows;
using System.Windows.Controls;
using Ws.Extensions.UI.Wpf.Utils;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    public static class ToolTipExtension
    {
        /// <summary>
        /// Gets the value of the AutoToolTipProperty dependency property
        /// </summary>
        public static bool GetAutoToolTip(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoToolTipProperty);
        }

        /// <summary>
        /// Sets the value of the AutoToolTipProperty dependency property
        /// </summary>
        public static void SetAutoToolTip(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoToolTipProperty, value);
        }

        /// <summary>
        /// Identified the attached AutoToolTip property. When true, this will set the TextBlock TextTrimming
        /// property to WordEllipsis, and display a tooltip with the full text whenever the text is trimmed.
        /// </summary>
        public static readonly DependencyProperty AutoToolTipProperty =
            DependencyProperty.RegisterAttached("AutoToolTip", typeof(bool), typeof(ToolTipExtension), new PropertyMetadata(false, OnAutoToolTipPropertyChanged));


        private static void OnAutoToolTipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = d as FrameworkElement;
            if (frameworkElement == null)
                return;

            if (!frameworkElement.IsLoaded)
            {
                if (e.NewValue.Equals(true))
                    frameworkElement.Loaded += OnLoadTurnAutoToolTipOn;
                else
                    frameworkElement.Loaded += OnLoadTurnAutoToolTipOff;

                return;
            }

            OnAutoToolTipChanged((bool)e.NewValue, frameworkElement);
        }

        private static void OnLoadTurnAutoToolTipOn(object sender, RoutedEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            OnAutoToolTipChanged(true, frameworkElement);
            frameworkElement.Loaded -= OnLoadTurnAutoToolTipOn;
        }

        private static void OnLoadTurnAutoToolTipOff(object sender, RoutedEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            OnAutoToolTipChanged(false, frameworkElement);
            frameworkElement.Loaded -= OnLoadTurnAutoToolTipOff;
        }

        private static void OnAutoToolTipChanged(bool turnOnAutoToolTip, FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
                return;

            if (turnOnAutoToolTip)
            {
                TextBlock textBlock = frameworkElement.GetFirstDescendantOfType<TextBlock>();
                if (textBlock == null)
                    return;

                textBlock.TextTrimming = TextTrimming.WordEllipsis;
                SetTrimmedTextAsToolTip(frameworkElement);
                frameworkElement.SizeChanged += FrameworkElement_SizeChanged;
                if (frameworkElement is ComboBox)
                    ((ComboBox)frameworkElement).SelectionChanged += ComboBox_SelectionChanged;
            }
            else
            {
                frameworkElement.SizeChanged -= FrameworkElement_SizeChanged;
                if (frameworkElement is ComboBox)
                    ((ComboBox)frameworkElement).SelectionChanged -= ComboBox_SelectionChanged;
            }
        }

        private static void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            SetTrimmedTextAsToolTip(frameworkElement);
        }

        private static void FrameworkElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            SetTrimmedTextAsToolTip(frameworkElement);
        }

        /// <summary>
        /// Assigns the ToolTip for the given FrameworkElement based on whether the text is trimmed
        /// </summary>
        private static void SetTrimmedTextAsToolTip(FrameworkElement frameworkElement)
        {
            if (IsFrameworkElementTrimmed(frameworkElement))
                ToolTipService.SetToolTip(frameworkElement, GetFrameworkElementText(frameworkElement));
            else
                ToolTipService.SetToolTip(frameworkElement, null);
        }

        public static bool IsFrameworkElementTrimmed<T>(T frameworkElement) where T : FrameworkElement
        {
            if (frameworkElement == null)
                return false;

            // Get text and measure
            TextBlock textBlock = (frameworkElement as FrameworkElement).GetFirstDescendantOfType<TextBlock>();
            if (textBlock == null)
                return false;
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            // Get text's ContentPresenter if exists
            var contentPresenter = textBlock.ParentOfType<ContentPresenter>();
            if (contentPresenter != null && contentPresenter.IsDescendantOf(frameworkElement))
                return textBlock.DesiredSize.Width > contentPresenter.ActualWidth;

            return textBlock.DesiredSize.Width > frameworkElement.ActualWidth;
        }

        public static string GetFrameworkElementText<T>(T frameworkElement) where T : FrameworkElement
        {
            var textBlock = (frameworkElement as FrameworkElement).GetFirstDescendantOfType<TextBlock>();
            if (textBlock != null)
                return textBlock.Text;

            if (frameworkElement is ComboBox && (frameworkElement as ComboBox).SelectedValue != null)
                return (frameworkElement as ComboBox).SelectedValue.ToString();

            return string.Empty;
        }
    }
}