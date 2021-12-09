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

        private double _sonicateArcHeight;
        public double SonicateArcHeight
        {
            get => _sonicateArcHeight;
            private set
            {
                _sonicateArcHeight = value;
                OnPropertyChanged();
            }
        }

        public double SonicateRadius { get; private set; }
        private void CalculateRadius()
        {
            if (SonicateHeight > 0 && SonicateWidth > 0)
            {
                double chord = IndependentSize.CalculateProportionalDouble(SonicateWidth);
                SonicateArcHeight = IndependentSize.CalculateProportionalDouble(SonicateHeight);
                SonicateRadius = (0.5 * SonicateArcHeight) + (0.125 * Math.Pow(chord, 2) / SonicateArcHeight);
                Width = 2 * SonicateRadius;
                Height = Width;
                OnPropertyChanged(nameof(SonicateRadius));
                CalculateCoolingAlpha();
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


        #region Cooling Progress

        public double CoolingMinimum
        {
            get { return (double)GetValue(CoolingMinimumProperty); }
            set { SetValue(CoolingMinimumProperty, value); }
        }
        public static readonly DependencyProperty CoolingMinimumProperty = DependencyProperty.Register(nameof(CoolingMinimum), typeof(double), typeof(SonicateControl), new PropertyMetadata(0.0));

        public double CoolingMaximum
        {
            get { return (double)GetValue(CoolingMaximumProperty); }
            set { SetValue(CoolingMaximumProperty, value); }
        }
        public static readonly DependencyProperty CoolingMaximumProperty = DependencyProperty.Register(nameof(CoolingMaximum), typeof(double), typeof(SonicateControl), new PropertyMetadata(1.0));

        public double CoolingValue
        {
            get { return (double)GetValue(CoolingValueProperty); }
            set { SetValue(CoolingValueProperty, value); }
        }
        public static readonly DependencyProperty CoolingValueProperty = DependencyProperty.Register(nameof(CoolingValue), typeof(double), typeof(SonicateControl), new PropertyMetadata(0.0, OnCoolingValueChanged));

        private static void OnCoolingValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SonicateControl sonicateControl)
                sonicateControl.CalculateCoolingIncrementAngle();
        }

        public double CoolingIncrementAngle { get; private set; } = 0;
        private void CalculateCoolingIncrementAngle()
        {
            if (CoolingMaximum - CoolingMinimum <= 0)
                return;
            CoolingIncrementAngle = -0.5 * CoolingAlpha;
            if (CoolingValue >= CoolingMaximum)
                CoolingIncrementAngle += CoolingAlpha;
            if (CoolingValue >= CoolingMinimum && CoolingValue <= CoolingMaximum)
                CoolingIncrementAngle += CoolingAlpha * CoolingValue / (CoolingMaximum - CoolingMinimum);
            OnPropertyChanged(nameof(CoolingIncrementAngle));
        }

        private const string BOUNDARY_WIDTH_NAME = "LDouble.Width.Boundary";
        private const string COOLING_WIDTH_NAME = "LDouble.Width.Cooling";
        public double CoolingAlpha { get; private set; }
        private void CalculateCoolingAlpha()
        {
            // 1. Calculate CoolingArcHeight
            // Determine BoundaryArcHeight (proportional to SonicateArcHeight, as BoundaryRadius to SonicateRadius)
            // Diff between BoundaryRadius and CoolingRadius
            // CoolingArcHeight = BoundaryArcHeight - Diff
            double boundaryRadius = (double)TryFindResource(BOUNDARY_WIDTH_NAME) / 2;
            double boundaryArcHeight = SonicateArcHeight * boundaryRadius / SonicateRadius;
            double coolingRadius = (double)TryFindResource(COOLING_WIDTH_NAME) / 2;
            double radiusDiff = boundaryRadius - coolingRadius;
            double coolingArcHeight = boundaryArcHeight - radiusDiff;

            // 2. CoolingChord
            double coolingChord = Math.Sqrt((8 * coolingArcHeight * coolingRadius) - (4 * coolingArcHeight * coolingArcHeight));

            // 3. CoolingAlpha
            if (coolingChord > 0)
            {
                CoolingAlpha = 2 * Math.Asin(coolingChord / (2 * coolingRadius)) * (180 / Math.PI);
                OnPropertyChanged(nameof(CoolingAlpha));
                CalculateCoolingIncrementAngle();
            }
        }

        #endregion


        #region Commands

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
