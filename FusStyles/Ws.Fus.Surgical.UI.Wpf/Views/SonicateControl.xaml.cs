using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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

        public double Radius { get; private set; }
        private void CalculateRadius()
        {
            if (SonicateHeight != 0)
            {
                double chord = SonicateWidth;
                double arcHeight = SonicateHeight;
                Radius = (0.5 * arcHeight) + (0.125 * Math.Pow(chord, 2) / arcHeight);
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
                double chord = SonicateWidth;
                Alpha = 2 * Math.Asin(0.5 * chord / Radius);
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


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
