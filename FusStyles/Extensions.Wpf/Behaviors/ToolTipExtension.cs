using System.Windows;
using System.Windows.Controls;
using Ws.Extensions.UI.Wpf.Utils;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    public static class ToolTipExtension
    {
        #region AutoToolTip attached property

        public static bool GetAutoToolTip(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoToolTipProperty);
        }

        public static void SetAutoToolTip(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoToolTipProperty, value);
        }

        public static readonly DependencyProperty AutoToolTipProperty =
            DependencyProperty.RegisterAttached("AutoToolTip", typeof(bool), typeof(ToolTipExtension), new PropertyMetadata(false, OnAutoToolTipPropertyChanged));


        #endregion


        #region Events

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
            }
            else
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

        private static void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            if (frameworkElement != null)
                SetToolTipIfTrimmed(frameworkElement);
        }

        private static void FrameworkElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;
            if (frameworkElement != null)
                SetToolTipIfTrimmed(frameworkElement);
        }

        #endregion


        #region Set ToolTip

        private static void OnAutoToolTipChanged(bool turnOnAutoToolTip, FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
                return;

            if (turnOnAutoToolTip)
            {
                TextBlock textBlock = frameworkElement.GetFirstDescendantOfType<TextBlock>();
                if (textBlock == null)
                    return;

                textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                SetToolTipIfTrimmed(frameworkElement);
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

        private static void SetToolTipIfTrimmed(FrameworkElement frameworkElement)
        {
            if (TextAssists.IsFrameworkElementTrimmedOrWrapped(frameworkElement))
                SetToolTip(frameworkElement);
            else
                ToolTipService.SetToolTip(frameworkElement, null);
        }

        private static void SetToolTip(FrameworkElement frameworkElement)
        {
            if (frameworkElement.IsHitTestVisible)
            {
                ToolTipService.SetToolTip(frameworkElement, TextAssists.GetFrameworkElementText(frameworkElement));
                return;
            }

            TextBlock textBlock = frameworkElement as TextBlock;
            if (textBlock != null)
            {
                // LA: To be used when moving to VS2019 (remove function IsToolTipable):
                //Func<FrameworkElement, bool> condition = x => x is Control && x.IsHitTestVisible;
                //var parent = textBlock.ParentOfTypeOnCondition(condition);
                var parent = textBlock.ParentOfTypeOnCondition<FrameworkElement>(IsToolTipableControl);
                ToolTipService.SetToolTip(parent ?? textBlock, textBlock.Text);
            }
        }

        private static bool IsToolTipableControl(FrameworkElement frameworkElement)
        {
            return frameworkElement != null && frameworkElement is Control && frameworkElement.IsHitTestVisible;
        }

        #endregion
    }
}