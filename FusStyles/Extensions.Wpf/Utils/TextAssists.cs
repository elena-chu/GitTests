using System.Windows;
using System.Windows.Controls;

namespace Ws.Extensions.UI.Wpf.Utils
{
    public static class TextAssists
    {
        public static bool IsFrameworkElementTrimmedOrWrapped<T>(T frameworkElement) where T : FrameworkElement
        {
            if (frameworkElement is TextBlock)
                return IsTextBlockTrimmed(frameworkElement as TextBlock);
            else
                return IsChildTextBlockTrimmedOrWrapped(frameworkElement);
        }

        public static bool IsTextBlockTrimmed(TextBlock textBlock)
        {
            if (string.IsNullOrEmpty(textBlock.Text))
                return false;

            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            // Measure against Border parent that might be narrower
            var border = textBlock.ParentOfType<Border>();
            if (border != null && textBlock.DesiredSize.Width > border.ActualWidth)
                return true;

            // Measure against ContentPresenter parent if exists
            var contentPresenter = textBlock.ParentOfType<ContentPresenter>();
            if (contentPresenter != null && textBlock.DesiredSize.Width > contentPresenter.ActualWidth)
                return true;

            return textBlock.DesiredSize.Width > textBlock.ActualWidth;
        }

        public static bool IsChildTextBlockTrimmedOrWrapped(FrameworkElement frameworkElement)
        {
            // Get text and measure
            TextBlock textBlock = frameworkElement.GetFirstDescendantOfType<TextBlock>();
            if (textBlock == null || string.IsNullOrEmpty(textBlock.Text))
                return false;

            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (frameworkElement is ContentPresenter || frameworkElement is Border)
                return textBlock.DesiredSize.Width > frameworkElement.ActualWidth;

            // Measure against Border if exists between frameworkElement and text
            var border = textBlock.ParentOfType<Border>();
            if (border != null && border.IsDescendantOf(frameworkElement) && textBlock.DesiredSize.Width > border.ActualWidth)
                return true;

            // Measure against ContentPresenter if exists between frameworkElement and text
            var contentPresenter = textBlock.ParentOfType<ContentPresenter>();
            if (contentPresenter != null && contentPresenter.IsDescendantOf(frameworkElement))
                return textBlock.DesiredSize.Width > contentPresenter.ActualWidth;

            return textBlock.DesiredSize.Width > frameworkElement.ActualWidth;
        }

        public static bool IsTextOverflowingWidth(FrameworkElement frameworkElement, double availableWidth)
        {
            if (frameworkElement == null || !(frameworkElement is FrameworkElement))
                return false;

            TextBlock textBlock = frameworkElement.GetFirstDescendantOfType<TextBlock>();
            if (textBlock != null)
            {
                if (string.IsNullOrEmpty(textBlock.Text))
                    return false;

                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                return textBlock.DesiredSize.Width > availableWidth;
            }

            return false;
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
