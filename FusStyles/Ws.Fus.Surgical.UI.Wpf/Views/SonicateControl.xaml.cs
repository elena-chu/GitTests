using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Fus.Surgical.UI.Wpf
{
    public partial class SonicateControl : UserControl, INotifyPropertyChanged
    {
        public SonicateControl()
        {
            InitializeComponent();
        }


        #region Dimensions

        public double SonicateWidth
        {
            get { return (double)GetValue(SonicateWidthProperty); }
            set { SetValue(SonicateWidthProperty, value); }
        }
        public static readonly DependencyProperty SonicateWidthProperty = DependencyProperty.Register(nameof(SonicateWidth), typeof(double), typeof(SonicateControl), new PropertyMetadata(OnSonicateDimensionsChanged));

        public double SonicateHeight
        {
            get { return (double)GetValue(SonicateHeightProperty); }
            set { SetValue(SonicateHeightProperty, value); }
        }
        public static readonly DependencyProperty SonicateHeightProperty = DependencyProperty.Register(nameof(SonicateHeight), typeof(double), typeof(SonicateControl), new PropertyMetadata(OnSonicateDimensionsChanged));

        private static void OnSonicateDimensionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SonicateControl sonicateControl)
                sonicateControl.CalculateRadius();
        }

        public double Chord { get; private set; }

        private double _arcHeight;
        public double ArcHeight
        {
            get => _arcHeight;
            private set
            {
                _arcHeight = value;
                OnPropertyChanged();
            }
        }

        public double Radius { get; private set; }
        private void CalculateRadius()
        {
            if (SonicateHeight != 0)
            {
                Chord = IndependentSize.CalculateProportionalDouble(SonicateWidth);
                ArcHeight = IndependentSize.CalculateProportionalDouble(SonicateHeight);
                Radius = (0.5 * ArcHeight) + (0.125 * Math.Pow(Chord, 2) / ArcHeight);
                Width = 2 * Radius;
                Height = Width;
                OnPropertyChanged(nameof(Radius));
                CalculateAlpha();
            }
        }

        public double Alpha { get; private set; }
        private void CalculateAlpha()
        {
            if (Radius != 0)
            {
                Chord = IndependentSize.CalculateProportionalDouble(SonicateWidth);
                Alpha = 2 * Math.Asin(0.5 * Chord / Radius);
                OnPropertyChanged(nameof(Alpha));
            }
        }

        #endregion


        #region Sonicate State

        public SonicateState SonicateState
        {
            get { return (SonicateState)GetValue(SonicateStateProperty); }
            set { SetValue(SonicateStateProperty, value); }
        }
        public static readonly DependencyProperty SonicateStateProperty = DependencyProperty.Register(nameof(SonicateState), typeof(SonicateState), typeof(SonicateControl), new PropertyMetadata(SonicateState.CoolingRunning));

        #endregion


        #region Command

        public ICommand SonicateStartPressCommand
        {
            get { return (ICommand)GetValue(SonicateStartPressCommandProperty); }
            set { SetValue(SonicateStartPressCommandProperty, value); }
        }
        public static readonly DependencyProperty SonicateStartPressCommandProperty = DependencyProperty.Register(nameof(SonicateStartPressCommand), typeof(ICommand), typeof(SonicateControl));

        public ICommand SonicateEndPressCommand
        {
            get { return (ICommand)GetValue(SonicateEndPressCommandProperty); }
            set { SetValue(SonicateEndPressCommandProperty, value); }
        }
        public static readonly DependencyProperty SonicateEndPressCommandProperty = DependencyProperty.Register(nameof(SonicateEndPressCommand), typeof(ICommand), typeof(SonicateControl));

        private void OnPressAnimationEnd(object sender, EventArgs e)
        {
            SonicateEndPressCommand?.Execute(null);
        }

        #endregion


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
