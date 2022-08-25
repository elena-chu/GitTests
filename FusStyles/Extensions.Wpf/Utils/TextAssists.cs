using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Ws.Extensions.UI.Wpf.Utils
{
    public static class TextAssists
    {
        public static bool IsFrameworkElementTrimmed<T>(T frameworkElement) where T : FrameworkElement
        {
            if (frameworkElement == null)
                return false;

            // Simple TextBlock
            if (frameworkElement is TextBlock textBlock)
                return IsTextBlockTrimmed(textBlock);

            if (frameworkElement is TextBox textBox)
                return IsTextBoxTrimmed(textBox);

            if (frameworkElement is ComboBox comboBox)
                return IsComboBoxTrimmed(comboBox);

            textBlock = (frameworkElement as FrameworkElement).GetFirstDescendantOfType<TextBlock>();
            return textBlock == null ? false : IsTextBlockTrimmed(textBlock);
        }

        public static bool IsTextBlockTrimmed(TextBlock textBlock)
        {
            if (textBlock == null || textBlock.Text == string.Empty)
                return false;

            FormattedText formattedText = GetFormattedText(textBlock.Text, textBlock);
            formattedText.MaxTextWidth = Math.Ceiling(textBlock.ActualWidth);
            if (textBlock.LineStackingStrategy == LineStackingStrategy.BlockLineHeight && textBlock.LineHeight != double.NaN)
                formattedText.LineHeight = textBlock.LineHeight;
            return Math.Floor(formattedText.Height) > textBlock.ActualHeight || Math.Floor(formattedText.MinWidth) > formattedText.MaxTextWidth;
        }

        public static bool IsTextBoxTrimmed(TextBox textBox)
        {
            if (textBox == null || textBox.Text == string.Empty || textBox.ActualWidth == 0)
                return false;

            return Math.Floor(GetFormattedText(textBox).Width) > textBox.ViewportWidth;
        }

        public static bool IsComboBoxTrimmed(ComboBox comboBox)
        {
            if (comboBox == null || comboBox.ActualWidth == 0)
                return false;

            bool noSelectedItem = comboBox.Items.Count == 0 ||
                                  comboBox.SelectedIndex < 0 ||
                                  comboBox.SelectedItem == null ||
                                  comboBox.SelectedValue == null ||
                                  (comboBox.SelectedValue is string selectedValueString && string.IsNullOrEmpty(selectedValueString));

            if (noSelectedItem)
                return false;

            TextBlock selectedItemTextBlock = comboBox.GetFirstDescendantOfType<TextBlock>();
            return IsTextBlockTrimmed(selectedItemTextBlock);
        }

        public static string GetFrameworkElementText<T>(T frameworkElement) where T : FrameworkElement
        {
            if (frameworkElement is TextBlock textBlock)
                return textBlock.Text;

            if (frameworkElement is ComboBox comboBox && comboBox.SelectedValue != null)
            {
                if (comboBox.SelectedValue is string)
                    return comboBox.SelectedValue.ToString();
                else
                {
                    if (comboBox.SelectedItem is FrameworkElement selectedItem)
                    {
                        var selectedItemChildTextBlock = selectedItem.GetFirstDescendantOfType<TextBlock>();
                        return selectedItemChildTextBlock != null ? selectedItemChildTextBlock.Text : string.Empty;
                    }
                }
            }

            if (frameworkElement is TextBox textBox)
                return textBox.Text;

            var childTextBlock = frameworkElement.GetFirstDescendantOfType<TextBlock>();
            if (childTextBlock != null)
                return childTextBlock.Text;

            return string.Empty;
        }

        public static FormattedText GetFormattedText(string text, TextBlock textBlockContainingFormat)
        {
            var typeface = new Typeface(textBlockContainingFormat.FontFamily, textBlockContainingFormat.FontStyle, textBlockContainingFormat.FontWeight, textBlockContainingFormat.FontStretch);
            DpiScale dpiScale = VisualTreeHelper.GetDpi(textBlockContainingFormat);
            double pixelsPerDip = dpiScale.PixelsPerDip;
            return new FormattedText(text, 
                                     CultureInfo.CurrentCulture,
                                     textBlockContainingFormat.FlowDirection,
                                     typeface, textBlockContainingFormat.FontSize,
                                     textBlockContainingFormat.Foreground, pixelsPerDip);
        }

        public static FormattedText GetFormattedText(TextBox textBox)
        {
            var typeface = new Typeface(textBox.FontFamily, textBox.FontStyle, textBox.FontWeight, textBox.FontStretch);
            DpiScale dpiScale = VisualTreeHelper.GetDpi(textBox);
            double pixelsPerDip = dpiScale.PixelsPerDip;
            return new FormattedText(textBox.Text, CultureInfo.CurrentCulture, textBox.FlowDirection, typeface, textBox.FontSize, textBox.Foreground, pixelsPerDip);
        }

        public static bool ContainsText(this FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
                return false;

            if (frameworkElement is TextBlock ||
                frameworkElement is Label ||
                frameworkElement is AccessText ||
                frameworkElement is TextBoxBase ||
                frameworkElement is PasswordBox)
                return true;

            TextBlock textBlock = frameworkElement.GetFirstDescendantOfType<TextBlock>();
            if (textBlock != null)
                return true;

            return false;
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
    }
}
