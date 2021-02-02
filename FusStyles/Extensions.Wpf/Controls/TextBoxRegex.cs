using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public class TextBoxRegex : TextBox
    {
        public static readonly DependencyProperty RegexProperty =
            DependencyProperty.Register(nameof(Regex), typeof(string), typeof(TextBoxRegex), new PropertyMetadata(string.Empty, OnRegexChanged));

        public static readonly DependencyProperty IsNumericProperty =
            DependencyProperty.Register(nameof(IsNumeric), typeof(bool), typeof(TextBoxRegex), new PropertyMetadata(false));        

        private Regex _regex;

        public string Regex
        {
            get { return (string)GetValue(RegexProperty); }
            set { SetValue(RegexProperty, value); }
        }

        public bool IsNumeric
        {
            get { return (bool)GetValue(IsNumericProperty); }
            set { SetValue(IsNumericProperty, value); }
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (!(_regex?.IsMatch(e.Text) ?? true))
                e.Handled = true;
            else if (IsNumeric && e.Text == "." && Text.Contains(".") && !SelectedText.Contains(".")) // the input is ".", while the text already contains it, but not in the selected text
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private static void OnRegexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (TextBoxRegex)d;
            var regex = e.NewValue as string;

            self._regex = string.IsNullOrWhiteSpace(regex) ? null : new Regex(regex);
        }
    }
}
