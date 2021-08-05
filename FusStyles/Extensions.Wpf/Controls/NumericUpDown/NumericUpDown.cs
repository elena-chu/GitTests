using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Ws.Extensions.UI.Wpf.Controls
{
    [TemplatePart(Name = "PART_UpButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DownButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_NumberTB", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_CodeLiteralTB", Type = typeof(TextBlock))]
    public class NumericUpDown : Control
    {
        #region Dependency Properties

        /// <summary>
        /// Positive Number single sign. Default Empty string.
        /// </summary>
        public static readonly DependencyProperty PlusCodeProperty = DependencyProperty.Register(
          nameof(PlusCode), typeof(string), typeof(NumericUpDown), new PropertyMetadata(""));
        public string PlusCode
        {
            get { return (string)this.GetValue(PlusCodeProperty); }
            set { this.SetValue(PlusCodeProperty, value); }
        }

        /// <summary>
        /// Positive Number literal description. Default "Positive".
        /// </summary>
        public static readonly DependencyProperty PlusCodeLiteralProperty = DependencyProperty.Register(
          nameof(PlusCodeLiteral), typeof(string), typeof(NumericUpDown), new PropertyMetadata("Positive"));
        public string PlusCodeLiteral
        {
            get { return (string)this.GetValue(PlusCodeLiteralProperty); }
            set { this.SetValue(PlusCodeLiteralProperty, value); }
        }

        /// <summary>
        /// Negative Number single sign. Default "-".
        /// </summary>
        public static readonly DependencyProperty MinusCodeProperty = DependencyProperty.Register(
          nameof(MinusCode), typeof(string), typeof(NumericUpDown), new PropertyMetadata("-"));
        public string MinusCode
        {
            get { return (string)this.GetValue(MinusCodeProperty); }
            set { this.SetValue(MinusCodeProperty, value); }
        }

        /// <summary>
        /// Negative Number literal description. Default "Negative".
        /// </summary>
        public static readonly DependencyProperty MinusCodeLiteralProperty = DependencyProperty.Register(
          nameof(MinusCodeLiteral), typeof(string), typeof(NumericUpDown), new PropertyMetadata("Negative"));
        public string MinusCodeLiteral
        {
            get { return (string)this.GetValue(MinusCodeLiteralProperty); }
            set { this.SetValue(MinusCodeLiteralProperty, value); }
        }

        /// <summary>
        /// Max text length property including sign and spaces
        /// </summary>
        public static readonly DependencyProperty DisplayLengthProperty = DependencyProperty.Register(
          nameof(DisplayLength), typeof(int), typeof(NumericUpDown), new PropertyMetadata(20));
        public int DisplayLength
        {
            get { return (int)this.GetValue(DisplayLengthProperty); }
            set { this.SetValue(DisplayLengthProperty, value); }
        }

        /// <summary>
        /// Numeric value. Is Nullable if IsNullable property set to true(default) otherwise 0. Default Value is null.
        /// When ones asigned to number only from outside can be set again to null(if IsNullable property set to true).
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double?), typeof(NumericUpDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        public double? Value
        {
            get { return (double?)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown control = d as NumericUpDown;
            if (control != null)
            {
                control.UpdateDisplay();
                control.IncreaseCommand.RaiseCanExecuteChanged();
                control.DecreaseCommand.RaiseCanExecuteChanged();
            }
        }

        private static readonly DependencyPropertyKey ValueDisplayKeyPropertyKey = DependencyProperty.RegisterReadOnly(
            "ValueDisplayKey", typeof(string), typeof(NumericUpDown), new PropertyMetadata(string.Empty/*, OnReadOnlyPropChanged*/));

        /// <summary>
        /// Readonly property returning shown text.
        /// </summary>
        public static readonly DependencyProperty ValueDisplayProperty = ValueDisplayKeyPropertyKey.DependencyProperty;
        public string ValueDisplay
        {
            get { return (string)GetValue(ValueDisplayProperty); }
            protected set { SetValue(ValueDisplayKeyPropertyKey, value); }
        }

        /// <summary>
        /// Propery defines the Value Severity level. Available values: Undefined, High, Low, Normal(default)
        /// </summary>
        public static readonly DependencyProperty SeverityLevelProperty = DependencyProperty.Register(
          nameof(SeverityLevel), typeof(SeverityLevel), typeof(NumericUpDown), new PropertyMetadata(SeverityLevel.Normal));
        public SeverityLevel SeverityLevel
        {
            get { return (SeverityLevel)this.GetValue(SeverityLevelProperty); }
            set { this.SetValue(SeverityLevelProperty, value); }
        }

        /// <summary>
        /// Min accepted Value
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
          nameof(MinValue), typeof(double), typeof(NumericUpDown), new PropertyMetadata(Double.MinValue, OnValidationRelevantPropertyChanged));
        public double MinValue
        {
            get { return (double)this.GetValue(MinValueProperty); }
            set { this.SetValue(MinValueProperty, value); }
        }
        private static void OnValidationRelevantPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //NumericUpDown control = d as NumericUpDown;
            //if (control != null)
            //    control.UpdateNumericValidator();
        }

        /// <summary>
        /// Max accepted Value
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
          nameof(MaxValue), typeof(double), typeof(NumericUpDown), new PropertyMetadata(Double.MaxValue, OnValidationRelevantPropertyChanged));
        public double MaxValue
        {
            get { return (double)this.GetValue(MaxValueProperty); }
            set { this.SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// Increament/Decreament step value
        /// </summary>
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
          nameof(Increment), typeof(double), typeof(NumericUpDown), new PropertyMetadata(1.0));
        public double Increment
        {
            get { return (double)this.GetValue(IncrementProperty); }
            set { this.SetValue(IncrementProperty, value); }
        }

        /// <summary>
        /// Property notifies whether any of child elements has a focus now.
        /// </summary>
        public static readonly DependencyProperty IsChildWithFocusProperty = DependencyProperty.Register(
          nameof(IsChildWithFocus), typeof(bool), typeof(NumericUpDown), new PropertyMetadata());
        public bool IsChildWithFocus
        {
            get { return (bool)this.GetValue(IsChildWithFocusProperty); }
            set { this.SetValue(IsChildWithFocusProperty, value); }
        }

        /// <summary>
        /// Property defines control's Display Status. Available values: Active(default), Disabled, Readonly.
        /// </summary>
        public static readonly DependencyProperty DisplayStatusProperty = DependencyProperty.Register(
          nameof(DisplayStatus), typeof(DisplayStatus), typeof(NumericUpDown), new PropertyMetadata(DisplayStatus.Active));
        public DisplayStatus DisplayStatus
        {
            get { return (DisplayStatus)this.GetValue(DisplayStatusProperty); }
            set { this.SetValue(DisplayStatusProperty, value); }
        }

        /// <summary>
        /// Property explicitly hides/shows Increament/Decreament buttons 
        /// </summary>
        public static readonly DependencyProperty ButtonsVisibilityProperty = DependencyProperty.Register(
          nameof(ButtonsVisibility), typeof(Visibility), typeof(NumericUpDown), new PropertyMetadata(Visibility.Visible, OnButtonsVisibilityChanged));
        public Visibility ButtonsVisibility
        {
            get { return (Visibility)this.GetValue(ButtonsVisibilityProperty); }
            set { this.SetValue(ButtonsVisibilityProperty, value); }
        }
        private static void OnButtonsVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown control = d as NumericUpDown;
            if (control == null)
                return;

            if (control.UpButtonElement != null)
                control.UpButtonElement.Visibility = (Visibility)e.NewValue;

            if (control.DownButtonElement != null)
                control.DownButtonElement.Visibility = (Visibility)e.NewValue;
        }

        /// <summary>
        /// Property describes whether keyboard action available for Up/Down keys
        /// </summary>
        public static readonly DependencyProperty UpDownByKeyboardEnabledProperty = DependencyProperty.Register(
          nameof(UpDownByKeyboardEnabled), typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false));
        public bool UpDownByKeyboardEnabled
        {
            get { return (bool)this.GetValue(UpDownByKeyboardEnabledProperty); }
            set { this.SetValue(UpDownByKeyboardEnabledProperty, value); }
        }

        /// <summary>
        /// Notifies whether to show Positive/Negative Values literal description
        /// </summary>
        public static readonly DependencyProperty CodeLiteralVisibilityProperty = DependencyProperty.Register(
          nameof(CodeLiteralVisibility), typeof(Visibility), typeof(NumericUpDown), new PropertyMetadata(Visibility.Visible, OnCodeLiteralVisibilityChanged));
        public Visibility CodeLiteralVisibility
        {
            get { return (Visibility)this.GetValue(CodeLiteralVisibilityProperty); }
            set { this.SetValue(CodeLiteralVisibilityProperty, value); }
        }
        private static void OnCodeLiteralVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown control = d as NumericUpDown;
            if (control != null && control.CodeLiteralElement != null)
            {
                control.CodeLiteralElement.Visibility = (Visibility)e.NewValue;
            }
        }

        /// <summary>
        /// Defines how many digits are shown after point.
        /// </summary>
        public static readonly DependencyProperty NumberOfFractionDigitsProperty = DependencyProperty.Register(
          nameof(NumberOfFractionDigits), typeof(int), typeof(NumericUpDown), new PropertyMetadata(2, OnValidationRelevantPropertyChanged));
        public int NumberOfFractionDigits
        {
            get { return (int)this.GetValue(NumberOfFractionDigitsProperty); }
            set { this.SetValue(NumberOfFractionDigitsProperty, value); }
        }

        /// <summary>
        /// Property notifies whether the Value can be null.
        /// </summary>
        public static readonly DependencyProperty IsNullableProperty = DependencyProperty.Register(
          nameof(IsNullable), typeof(bool), typeof(NumericUpDown), new PropertyMetadata(true, OnIsNullableChanged));
        public bool IsNullable
        {
            get { return (bool)this.GetValue(IsNullableProperty); }
            set { this.SetValue(IsNullableProperty, value); }
        }
        private static void OnIsNullableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown control = d as NumericUpDown;
            if (control == null)
                return;

            if (!(bool)e.NewValue)
            {
                if (control.Value == null)
                    control.Value = control.GetDefaultValue();
            }
        }

        /// <summary>
        /// Units to describe Value
        /// </summary>
        public string Units
        {
            get { return (string)GetValue(UnitsProperty); }
            set { SetValue(UnitsProperty, value); }
        }
        public static readonly DependencyProperty UnitsProperty = 
            DependencyProperty.Register(nameof(Units), typeof(string), typeof(NumericUpDown), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Where to place Units (on line = Center, below line = Subscript)
        /// </summary>
        public BaselineAlignment UnitsPlacement
        {
            get { return (BaselineAlignment)GetValue(UnitsPlacementProperty); }
            set { SetValue(UnitsPlacementProperty, value); }
        }
        public static readonly DependencyProperty UnitsPlacementProperty = 
            DependencyProperty.Register(nameof(UnitsPlacement), typeof(BaselineAlignment), typeof(NumericUpDown), new PropertyMetadata(BaselineAlignment.Center));

        #endregion


        #region Start, End

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        public NumericUpDown()
        {
            IncreaseCommand = new DelegateCommand(IncreaseExecute, IncreaseCanExecute);
            DecreaseCommand = new DelegateCommand(DecreaseExecute, DecreaseCanExecute);
            Loaded += OnUnload;
        }

        private void OnUnload(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.PreviewMouseDown -= OnTextElementLostFocus;
        }

        #endregion


        #region Commands

        private DelegateCommand IncreaseCommand;
        private void IncreaseExecute()
        {
            if (!IncreaseCanExecute())
                return;

            if (!Value.HasValue)
                Value = GetDefaultValue() + Increment;
            else
                Value += Increment;

            UpdateDisplay();
        }
        private bool IncreaseCanExecute()
        {
            return DisplayStatus == DisplayStatus.Active && (!Value.HasValue || Value.Value < MaxValue);
        }


        private DelegateCommand DecreaseCommand;
        private void DecreaseExecute()
        {
            if (!DecreaseCanExecute())
                return;

            if (!Value.HasValue)
                Value = GetDefaultValue() - Increment;
            else
                Value -= Increment;

            UpdateDisplay();
        }
        private bool DecreaseCanExecute()
        {
            return DisplayStatus == DisplayStatus.Active && (!Value.HasValue || Value.Value > MinValue);
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            UpButtonElement = GetTemplateChild("PART_UpButton") as RepeatButton;
            DownButtonElement = GetTemplateChild("PART_DownButton") as RepeatButton;
            TextElement = GetTemplateChild("PART_NumberTB") as TextBox;
            CodeLiteralElement = GetTemplateChild("PART_CodeLiteralTB") as TextBlock;

            this.PreviewKeyDown += OnControlPreviewKeyDown;


            UpdateDisplay();

            //UpdateStates(false);
        }

        #endregion

        #region Private Properties

        private RepeatButton _downButtonElement;

        private RepeatButton DownButtonElement
        {
            get { return _downButtonElement; }
            set
            {
                if (_downButtonElement != null)
                {
                    _downButtonElement.Command = null;
                    _downButtonElement.GotFocus -= OnButtonGotFocus;
                    _downButtonElement.LostFocus -= OnButtonLostFocus;
                }

                _downButtonElement = value;

                if (_downButtonElement != null)
                {
                    _downButtonElement.Command = DecreaseCommand;
                    _downButtonElement.GotFocus += OnButtonGotFocus;
                    _downButtonElement.LostFocus += OnButtonLostFocus;
                }
            }
        }

        private RepeatButton _upButtonElement;

        private RepeatButton UpButtonElement
        {
            get { return _upButtonElement; }
            set
            {
                if (_upButtonElement != null)
                {
                    _upButtonElement.Command = null;
                }

                _upButtonElement = value;

                if (_upButtonElement != null)
                {
                    _upButtonElement.Command = IncreaseCommand;
                }
            }
        }

        private TextBox _textElement;
        private TextBox TextElement
        {
            get { return _textElement; }
            set
            {
                if (_textElement != null)
                {
                    _textElement.GotFocus -= OnTextElementGotFocus;
                    _textElement.LostFocus -= OnTextElementLostFocus;
                    _textElement.TextChanged -= OnTextChanged;
                    _textElement.GotMouseCapture -= OnTextElementGotMouseCapture;
                }

                _textElement = value;

                if (_textElement != null)
                {
                    _textElement.GotFocus += OnTextElementGotFocus;
                    _textElement.LostFocus += OnTextElementLostFocus;
                    _textElement.TextChanged += OnTextChanged;
                    _textElement.GotMouseCapture += OnTextElementGotMouseCapture;
                }
            }
        }

        private TextBlock CodeLiteralElement { get; set; }

        public string ValueSignCode
        {
            get { return (!Value.HasValue || Math.Sign(Value.Value) >= 0) ? PlusCode : MinusCode; }
        }

        #endregion

        #region Private Methods

        private void OnButtonGotFocus(object sender, RoutedEventArgs e)
        {
            IsChildWithFocus = true;
        }

        private void OnButtonLostFocus(object sender, RoutedEventArgs e)
        {
            IsChildWithFocus = false;
        }

        private void OnTextElementLostFocus(object sender, RoutedEventArgs e)
        {
            UpdateValue(TextElement.Text);
            UpdateDisplay();

            IsChildWithFocus = false;
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(_textElement), null);
            Application.Current.MainWindow.PreviewMouseDown -= OnTextElementLostFocus;
        }

        private void OnTextElementGotFocus(object sender, RoutedEventArgs e)
        {
            SelectAll();
            Application.Current.MainWindow.PreviewMouseDown += OnTextElementLostFocus;
        }
        private void OnTextElementGotMouseCapture(object sender, MouseEventArgs e)
        {
            SelectAll();
        }

        private void OnControlPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (UpDownByKeyboardEnabled)
            {
                if (e.Key == Key.Up)
                {
                    IncreaseExecute();
                    e.Handled = true;
                }
                if (e.Key == Key.Down)
                {
                    DecreaseExecute();
                    e.Handled = true;
                }
            }
            if (e.Key == Key.Enter && e.OriginalSource is TextBox)
            {
                OnTextChanged(null, null);
                UpdateValue(TextElement.Text);
                UpdateDisplay();
                e.Handled = true;
            }
            if (e.Key == Key.Enter && e.OriginalSource is RepeatButton)// ignore the buttons enter - to avoid mistakes
            {
                e.Handled = true;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextElement.Text.Length > DisplayLength)
            {
                TextElement.Text = TextElement.Text.Substring(0, DisplayLength);
            }

            if (IsInputValid(TextElement.Text))
            {
                TextElement.Tag = TextElement.Text;
                if (e != null)
                    e.Handled = true;
            }
            else
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    TextElement.Text = (string)TextElement.Tag;
                    TextElement.CaretIndex = TextElement.Text.Length;
                }));
            }

            ValueDisplay = GetValueDisplay();

            if (e != null)
                e.Handled = true;
        }

        private void SelectAll()
        {
            TextElement.CaretIndex = TextElement.Text.Length;
            TextElement.SelectAll();
            IsChildWithFocus = true;
        }

        private string GetValueDisplay()
        {
            string format = GetFormat();
            if (Value.HasValue)
            {
                string displayedNumber = Math.Abs(Value.Value).ToString(format);
                return $"{ValueSignCode} {displayedNumber}";
            }
            else { return string.Empty; }
        }

        private void UpdateValue(string text)
        {
            if (!Value.HasValue && string.IsNullOrWhiteSpace(text))
                return;

            double number;
            if (TryParseToNumber(text, out number))
            {
                if (Value.HasValue && Value.Value == number)
                    return;

                number = Math.Round(number, NumberOfFractionDigits);
                Value = GetValueInMinMaxRange(number);
            }
        }

        private bool IsInputValid(string text)
        {
            double parsedNumber;
            if (TryParseToNumber(text, out parsedNumber))
            {
                if (Math.Round(parsedNumber, NumberOfFractionDigits) != parsedNumber)
                {
                    return false;
                }
                return true;
            }
            return false;

        }

        private bool TryParseToNumber(string inputString, out double number)
        {
            number = double.MinValue;
            string input = inputString.ToUpper().Trim();
            string prefix = "+";
            string numberStr = input;
            if ((!string.IsNullOrWhiteSpace(MinusCode) && input.StartsWith(MinusCode))
                || (!string.IsNullOrWhiteSpace(PlusCode) && input.StartsWith(PlusCode))
                || input.StartsWith("+") || input.StartsWith("-"))
            {
                prefix = input.Substring(0, 1).ToUpper();
                numberStr = input.Substring(1);
            }
            if (string.IsNullOrWhiteSpace(numberStr))
            {
                number = GetDefaultValue();
                return true;
            }

            double parsedNumber;
            if (double.TryParse(numberStr, out parsedNumber))
            {
                if (prefix == "-" || prefix == MinusCode)
                {
                    parsedNumber = -parsedNumber;
                }

                number = parsedNumber;
                return true;
            }

            return false;
        }

        private void UpdateDisplay()
        {
            if (TextElement == null || CodeLiteralElement == null)
                return;

            TextElement.Text = GetValueDisplay();
            CodeLiteralElement.Text = CodeLiteralVisibility == Visibility.Visible ? GetCodeLiteralDisplay() : string.Empty;

            //Update readonly property
            ValueDisplay = TextElement.Text;
        }

        private string GetCodeLiteralDisplay()
        {
            return Value.HasValue && Value.Value < default(double) ? MinusCodeLiteral : PlusCodeLiteral;
        }

        private string GetFormat()
        {
            string ret = "0";
            if (NumberOfFractionDigits > 0)
            {
                ret += ".";
                for (int i = 0; i < NumberOfFractionDigits; i++)
                {
                    ret += "0";
                }
            }
            return ret;
        }

        private double GetDefaultValue()
        {
            return GetValueInMinMaxRange(default(double));
        }

        private double GetValueInMinMaxRange(double value)
        {
            return Math.Max(Math.Min(value, MaxValue), MinValue);
        }

        #endregion
    }
}
