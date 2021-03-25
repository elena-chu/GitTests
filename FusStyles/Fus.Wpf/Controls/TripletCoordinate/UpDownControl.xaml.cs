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
using Ws.Fus.UI.Wpf.ViewModels.TripletCoordinate;

namespace Ws.Fus.UI.Wpf.Controls.TripletCoordinate
{


    /// <summary>
    /// Interaction logic for Single.xaml
    /// </summary>
    public partial class UpDownControl : UserControl
    {
        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
          nameof(Value), typeof(double), typeof(UpDownControl), new PropertyMetadata());

        public string ValueDisplay
        {
            get { return (string)this.GetValue(ValueDisplayProperty); }
            set { this.SetValue(ValueDisplayProperty, value); }
        }
        public static readonly DependencyProperty ValueDisplayProperty = DependencyProperty.Register(
          nameof(ValueDisplay), typeof(string), typeof(UpDownControl), new PropertyMetadata());


        public string Footer
        {
            get { return (string)this.GetValue(FooterProperty); }
            set { this.SetValue(FooterProperty, value); }
        }
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
          nameof(Footer), typeof(string), typeof(UpDownControl), new PropertyMetadata());


        public bool IsReadOnly
        {
            get { return (bool)this.GetValue(IsReadOnlyProperty); }
            set { this.SetValue(IsReadOnlyProperty, value); }
        }
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
          nameof(IsReadOnly), typeof(bool), typeof(UpDownControl), new PropertyMetadata(false));






        public SeverityLevel SeverityLevel
        {
            get { return (SeverityLevel)this.GetValue(SeverityLevelProperty); }
            set { this.SetValue(SeverityLevelProperty, value); }
        }
        public static readonly DependencyProperty SeverityLevelProperty = DependencyProperty.Register(
          nameof(SeverityLevel), typeof(SeverityLevel), typeof(UpDownControl), new PropertyMetadata());
        public double MinValue
        {
            get { return (double)this.GetValue(MinValueProperty); }
            set { this.SetValue(MinValueProperty, value); }
        }
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
          nameof(MinValue), typeof(double), typeof(UpDownControl), new PropertyMetadata(Double.MinValue));
        public double MaxValue
        {
            get { return (double)this.GetValue(MaxValueProperty); }
            set { this.SetValue(MaxValueProperty, value); }
        }
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
          nameof(MaxValue), typeof(double), typeof(UpDownControl), new PropertyMetadata(Double.MaxValue));

        public double Increment
        {
            get { return (double)this.GetValue(IncrementProperty); }
            set { this.SetValue(IncrementProperty, value); }
        }
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
          nameof(Increment), typeof(double), typeof(UpDownControl), new PropertyMetadata(1.0));


        public GridLength PartNumber
        {
            get { return (GridLength)this.GetValue(PartNumberProperty); }
            set { this.SetValue(PartNumberProperty, value); }
        }
        public static readonly DependencyProperty PartNumberProperty = DependencyProperty.Register(
          nameof(PartNumber), typeof(GridLength), typeof(UpDownControl), new PropertyMetadata(new GridLength(5, GridUnitType.Star)));
        public GridLength PartButtons
        {
            get { return (GridLength)this.GetValue(PartButtonsProperty); }
            set { this.SetValue(PartButtonsProperty, value); }
        }
        public static readonly DependencyProperty PartButtonsProperty = DependencyProperty.Register(
          nameof(PartButtons), typeof(GridLength), typeof(UpDownControl), new PropertyMetadata(new GridLength(3, GridUnitType.Star)));
        public bool IsChildWithFocus
        {
            get { return (bool)this.GetValue(IsChildWithFocusProperty); }
            set { this.SetValue(IsChildWithFocusProperty, value); }
        }
        public static readonly DependencyProperty IsChildWithFocusProperty = DependencyProperty.Register(
          nameof(IsChildWithFocus), typeof(bool), typeof(UpDownControl), new PropertyMetadata());


        public DisplayStatus DisplayStatus
        {
            get { return (DisplayStatus)this.GetValue(DisplayStatusProperty); }
            set { this.SetValue(DisplayStatusProperty, value); }
        }
        public static readonly DependencyProperty DisplayStatusProperty = DependencyProperty.Register(
          nameof(DisplayStatus), typeof(DisplayStatus), typeof(UpDownControl), new PropertyMetadata());

        public Visibility ButtonsVisibility
        {
            get { return (Visibility)this.GetValue(ButtonsVisibilityProperty); }
            set { this.SetValue(ButtonsVisibilityProperty, value); }
        }
        public static readonly DependencyProperty ButtonsVisibilityProperty = DependencyProperty.Register(
          nameof(ButtonsVisibility), typeof(Visibility), typeof(UpDownControl), new PropertyMetadata());

        public bool UpDownByKeyboard
        {
            get { return (bool)this.GetValue(UpDownByKeyboardProperty); }
            set { this.SetValue(UpDownByKeyboardProperty, value); }
        }
        public static readonly DependencyProperty UpDownByKeyboardProperty = DependencyProperty.Register(
          nameof(UpDownByKeyboard), typeof(bool), typeof(UpDownControl), new PropertyMetadata(false));

        public Visibility FooterVisibility
        {
            get { return (Visibility)this.GetValue(FooterVisibilityProperty); }
            set { this.SetValue(FooterVisibilityProperty, value); }
        }
        public static readonly DependencyProperty FooterVisibilityProperty = DependencyProperty.Register(
          nameof(FooterVisibility), typeof(Visibility), typeof(UpDownControl), new PropertyMetadata());


        public UpDownControl()
        {
            InitializeComponent();
            Loaded += Single_Loaded;
        }

        private void Single_Loaded(object sender, RoutedEventArgs e)
        {
            InitCommands();

            Number.GotFocus += Number_GotFocus;
            Number.LostFocus += Number_LostFocus;
            ButtonUpControl.GotFocus += ButtonControl_GotFocus;
            ButtonDownControl.GotFocus += ButtonControl_GotFocus;
            ButtonUpControl.LostFocus += ButtonnControl_LostFocus;
            ButtonDownControl.LostFocus += ButtonnControl_LostFocus;
            Number.TextChanged += Number_TextChanged;
            Number.GotMouseCapture += Number_GotMouseCapture;

        }



        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is ICoordinateValidation)
            {
                ICoordinateValidation coordinateValidation = (ICoordinateValidation)DataContext;
                bool ok = coordinateValidation.IsValid(Number.Text);
                if (ok)
                {
                    e.Handled = true;
                    Number.Tag = Number.Text;

                }
                else
                {
                    Dispatcher.BeginInvoke((Action)(() => {
                        Number.Text = (string)Number.Tag;
                        Number.CaretIndex = Number.Text.Length;
                    }));
                }
                e.Handled = true;
            }
        }


        private void ButtonnControl_LostFocus(object sender, RoutedEventArgs e)
        {
            IsChildWithFocus = false;
        }

        private void ButtonControl_GotFocus(object sender, RoutedEventArgs e)
        {
            IsChildWithFocus = true;
        }

        private void Number_LostFocus(object sender, RoutedEventArgs e)
        {
            IsChildWithFocus = false;

        }

        private void Number_GotFocus(object sender, RoutedEventArgs e)
        {
            SelectAll();
        }
        private void Number_GotMouseCapture(object sender, MouseEventArgs e)
        {
            SelectAll();
        }
        private void SelectAll()
        {
            Number.CaretIndex = Number.Text.Length;
            Number.SelectAll();
            IsChildWithFocus = true;
        }




        private void InitCommands()
        {
            CommandBinding down = new CommandBinding(UpDownCommands.Down, DownExecuted, DownCanExecuted);
            this.CommandBindings.Add(down);
            CommandBinding up = new CommandBinding(UpDownCommands.Up, UpExecuted, UpCanExecuted);
            up.PreviewCanExecute += UpCanExecuted;
            up.PreviewExecuted += UpExecuted;
            this.CommandBindings.Add(up);
            this.PreviewKeyDown += Single_PreviewKeyDown;
        }

        private void Single_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (UpDownByKeyboard)
            {
                if (e.Key == Key.Up)
                {
                    ButtonUp();
                    e.Handled = true;
                }
                if (e.Key == Key.Down)
                {
                    ButtonDown();
                    e.Handled = true;
                }
            }
            if (e.Key == Key.Enter && e.OriginalSource is TextBox) // TODO this feature of enter for echo is not working . Not clear why . maybe this is due to the dp assignment
            {
                ValueDisplay = ((TextBox)e.OriginalSource).Text;
                ((TextBox)e.OriginalSource).Text = ValueDisplay;
                e.Handled = true;
            }
            if (e.Key == Key.Enter && e.OriginalSource is RepeatButton)// ignore the buttons enter - to avoid mistakes
            {
                e.Handled = true;
            }
        }

        void DownExecuted(object target, ExecutedRoutedEventArgs e)
        {
            this.ButtonDown();
            e.Handled = true;
        }
        void DownCanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Value <= MinValue)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }


        }
        void UpExecuted(object target, ExecutedRoutedEventArgs e)
        {
            ButtonUp();
            e.Handled = true;
        }
        void UpCanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Value >= MaxValue)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }


        }













        private void ButtonUp()
        {
            if (IsReadOnly | Value >= MaxValue)
            {
                return;
            }
            Value += this.Increment;

        }


        private void ButtonDown()
        {
            if (IsReadOnly | Value <= MinValue)
            {
                return;
            }
            Value -= this.Increment;
        }


    }
}
