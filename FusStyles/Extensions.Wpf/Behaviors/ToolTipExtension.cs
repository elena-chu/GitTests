using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Ws.Extensions.UI.Wpf.Utils;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    public static class ToolTipExtension
    {
        #region ToolTip Content

        /// <summary>
        /// ToolTip content. If this is not set, content will be the text as it was before it was trimmed
        /// </summary>
        public static string GetAutoToolTipContent(DependencyObject obj) { return (string)obj.GetValue(AutoToolTipContentProperty); }
        public static void SetAutoToolTipContent(DependencyObject obj, string value) { obj.SetValue(AutoToolTipContentProperty, value); }
        public static readonly DependencyProperty AutoToolTipContentProperty = DependencyProperty.RegisterAttached("AutoToolTipContent", typeof(string), typeof(ToolTipExtension), new PropertyMetadata(null, OnAutoToolTipContentPropertyChanged));

        private static void OnAutoToolTipContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement frameworkElement)
                UpdateAutoToolTip(GetAutoToolTip(d), frameworkElement);
        }

        #endregion


        #region Auto ToolTip

        /// <summary>
        /// AutoToolTip On/Off
        /// </summary>
        public static bool GetAutoToolTip(DependencyObject obj) { return (bool)obj.GetValue(AutoToolTipProperty); }
        public static void SetAutoToolTip(DependencyObject obj, bool value) { obj.SetValue(AutoToolTipProperty, value); }
        public static readonly DependencyProperty AutoToolTipProperty = 
            DependencyProperty.RegisterAttached("AutoToolTip", typeof(bool), typeof(ToolTipExtension),
                                                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, OnAutoToolTipPropertyChanged));

        private static void OnAutoToolTipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement frameworkElement)
            {
                if (!frameworkElement.IsLoaded)
                {
                    if (e.NewValue.Equals(true))
                        frameworkElement.Loaded += OnLoadTurnAutoToolTipOn;
                    else
                        frameworkElement.Loaded += OnLoadTurnAutoToolTipOff;
                    return;
                }

                UpdateAutoToolTip((bool)e.NewValue, frameworkElement);
            }
        }

        private static void UpdateAutoToolTip(bool turnOnAutoToolTip, FrameworkElement frameworkElement)
        {
            if (!frameworkElement.ContainsText())
                return;

            if (turnOnAutoToolTip)
            {
                SetToolTipOnTrimmedTextAsync(frameworkElement, DispatcherPriority.DataBind);
                SetTextTrimming(frameworkElement);
                RegisterFrameworkElementEvents(frameworkElement);
            }
            else
                UnregisterFrameworkElementEvents(frameworkElement);

            frameworkElement.Unloaded += OnUnload;
        }

        /// <summary>
        /// Assigns the ToolTip for the given FrameworkElement based on whether the text is trimmed
        /// </summary>
        private static async Task SetToolTipOnTrimmedTextAsync(FrameworkElement frameworkElement, DispatcherPriority dispatcherPriority)
        {
            bool trimmed = false;
            if (frameworkElement.Visibility == Visibility.Visible && frameworkElement.ActualWidth > 0 && frameworkElement.ActualHeight > 0)
            {
                if (dispatcherPriority == DispatcherPriority.Normal)
                    trimmed = TextAssists.IsFrameworkElementTrimmed(frameworkElement);
                else
                {
                    await frameworkElement.Dispatcher.BeginInvoke((ThreadStart)delegate
                    {
                        trimmed = TextAssists.IsFrameworkElementTrimmed(frameworkElement);
                    }, dispatcherPriority);
                }
            }

            if (trimmed)
            {
                string toolTip = (string)frameworkElement.GetValue(AutoToolTipContentProperty);
                if (!string.IsNullOrEmpty(toolTip))
                    ToolTipService.SetToolTip(frameworkElement, toolTip);
                else
                    ToolTipService.SetToolTip(frameworkElement, TextAssists.GetFrameworkElementText(frameworkElement));
            }
            else
            {
                ToolTipService.SetToolTip(frameworkElement, null);
            }
        }

        #endregion


        #region FrameworkElement Events

        private static void OnLoadTurnAutoToolTipOn(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                UpdateAutoToolTip(true, frameworkElement);
                frameworkElement.Loaded -= OnLoadTurnAutoToolTipOn;
            }
        }

        private static void OnLoadTurnAutoToolTipOff(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                UpdateAutoToolTip(false, frameworkElement);
                frameworkElement.Loaded -= OnLoadTurnAutoToolTipOff;
            }
        }

        private static void OnUnload(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
                UnregisterFrameworkElementEvents(frameworkElement);
        }

        private static void RegisterFrameworkElementEvents(FrameworkElement frameworkElement)
        {
            if (frameworkElement != null)
            {
                frameworkElement.SizeChanged += OnFrameworkElementChanged;
                if (frameworkElement is ComboBox comboBox)
                    comboBox.SelectionChanged += OnComboBoxSelectionChanged;
                if (frameworkElement is TextBox textBox)
                    textBox.TextChanged += OnFrameworkElementChanged;
            }
        }

        private static void UnregisterFrameworkElementEvents(FrameworkElement frameworkElement)
        {
            if (frameworkElement != null)
            {
                frameworkElement.SizeChanged -= OnFrameworkElementChanged;
                if (frameworkElement is ComboBox comboBox)
                    comboBox.SelectionChanged -= OnComboBoxSelectionChanged;
                if (frameworkElement is TextBox textBox)
                    textBox.TextChanged -= OnFrameworkElementChanged;
            }
        }

        private static void OnFrameworkElementChanged(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
                SetToolTipOnTrimmedTextAsync(frameworkElement, DispatcherPriority.Normal);
        }

        private static void OnComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
                SetToolTipOnTrimmedTextAsync(comboBox, DispatcherPriority.ApplicationIdle);
        }

        #endregion


        #region Text Trimming

        /// <summary>
        /// Default TextTrimming is CharacterEllipsis. Word Ellipsis or no trimming have to set explicitly
        /// </summary>
        public static TextTrimming GetAutoToolTipTextTrimming(DependencyObject obj) { return (TextTrimming)obj.GetValue(AutoToolTipTextTrimmingProperty); }
        public static void SetAutoToolTipTextTrimming(DependencyObject obj, TextTrimming value) { obj.SetValue(AutoToolTipTextTrimmingProperty, value); }
        public static readonly DependencyProperty AutoToolTipTextTrimmingProperty = DependencyProperty.RegisterAttached("AutoToolTipTextTrimming", typeof(TextTrimming), typeof(ToolTipExtension), new PropertyMetadata(TextTrimming.CharacterEllipsis, OnAutoToolTipTextTrimmingPropertyChanged));

        private static void OnAutoToolTipTextTrimmingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement frameworkElement && GetAutoToolTipTextTrimming(d) != TextTrimming.None)
                SetTextTrimming(frameworkElement);
        }

        private static void SetTextTrimming(FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
                return;

            if (frameworkElement is TextBlock textBlock && textBlock.TextTrimming == TextTrimming.None)
            {
                textBlock.TextTrimming = GetAutoToolTipTextTrimming(frameworkElement);
                return;
            }

            TextBlock childTextBlock = frameworkElement.GetFirstDescendantOfType<TextBlock>();
            if (childTextBlock != null && childTextBlock.TextTrimming == TextTrimming.None)
                childTextBlock.TextTrimming = GetAutoToolTipTextTrimming(frameworkElement);
        }

        #endregion
    }
}