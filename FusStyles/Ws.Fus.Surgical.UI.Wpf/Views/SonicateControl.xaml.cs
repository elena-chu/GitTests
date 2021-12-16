using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Fus.Surgical.UI.Wpf
{
    public enum SonicateControlState
    {
        None,
        Cooling,
        SonicateReady,
        SonicatePress,
        SonicateDisabled
    }

    public partial class SonicateControl : UserControl, INotifyPropertyChanged
    {
        #region General

        public SonicateControl()
        {
            SonicatePressCommand = new DelegateCommand(SonicatePress);
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            InitAnimationStoryboards();
        }

        #endregion


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


        #region Control State

        private SonicateControlState _sonicateControlState = SonicateControlState.None;
        public SonicateControlState SonicateControlState
        {
            get => _sonicateControlState;
            private set
            {
                if (value != _sonicateControlState)
                {
                    _sonicateControlState = value;
                    OnPropertyChanged();
                    OnSonicateControlStateChanged();
                }
            }
        }

        private void OnSonicateControlStateChanged()
        {
            switch (SonicateControlState)
            {
                case SonicateControlState.None:
                    if (_resetRequired)
                        AnimateReset();
                    break;
                case SonicateControlState.Cooling:
                    CalculateCoolingIncrementAngle();
                    _resetRequired = true;
                    break;
                case SonicateControlState.SonicateReady:
                    _resetRequired = true;
                    AnimateSonicateReady();
                    break;
                case SonicateControlState.SonicatePress:
                    _resetRequired = true;
                    AnimateSonicatePress();
                    break;
                case SonicateControlState.SonicateDisabled:
                    _resetRequired = true;
                    break;
                default:
                    break;
            }
        }

        private static void OnExternalStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SonicateControl sonicateControl)
                sonicateControl.ExternalStateChanged();
        }

        private void ExternalStateChanged()
        {
            if (CoolingAvailable)
                SonicateControlState = SonicateControlState.Cooling;
            else if (!SonicationAvailable)
                SonicateControlState = SonicateControlState.None;
            else if (SonicationEnabled)
                SonicateControlState = SonicateControlState.SonicateReady;
            else
                SonicateControlState = SonicateControlState.SonicateDisabled;
        }

        #endregion


        #region Sonication

        public bool SonicationAvailable
        {
            get { return (bool)GetValue(SonicationAvailableProperty); }
            set { SetValue(SonicationAvailableProperty, value); }
        }
        public static readonly DependencyProperty SonicationAvailableProperty = DependencyProperty.Register(nameof(SonicationAvailable), typeof(bool), typeof(SonicateControl), new PropertyMetadata(false, OnExternalStateChanged));

        public bool SonicationEnabled
        {
            get { return (bool)GetValue(SonicationEnabledProperty); }
            set { SetValue(SonicationEnabledProperty, value); }
        }
        public static readonly DependencyProperty SonicationEnabledProperty = DependencyProperty.Register(nameof(SonicationEnabled), typeof(bool), typeof(SonicateControl), new PropertyMetadata(false, OnExternalStateChanged));

        public ICommand SonicateCommand
        {
            get { return (ICommand)GetValue(SonicateCommandProperty); }
            set { SetValue(SonicateCommandProperty, value); }
        }
        public static readonly DependencyProperty SonicateCommandProperty = DependencyProperty.Register(nameof(SonicateCommand), typeof(ICommand), typeof(SonicateControl));

        public ICommand SonicatePressCommand { get; set; }
        private void SonicatePress()
        {
            SonicateControlState = SonicateControlState.SonicatePress;
        }

        #endregion


        #region Cooling

        public bool CoolingAvailable
        {
            get { return (bool)GetValue(CoolingAvailableProperty); }
            set { SetValue(CoolingAvailableProperty, value); }
        }
        public static readonly DependencyProperty CoolingAvailableProperty = DependencyProperty.Register(nameof(CoolingAvailable), typeof(bool), typeof(SonicateControl), new PropertyMetadata(false, OnExternalStateChanged));

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


        #region Animation

        private void InitAnimationStoryboards()
        {
            _sonicateReadySwellStoryboard = FindResource(_sonicateReadySwellStoryboardName) as Storyboard;

            _sonicateReadyOscillateStoryboards = new List<Storyboard>();
            foreach (var storyBoardName in _sonicateReadyOscillateStoryboardNames)
                _sonicateReadyOscillateStoryboards.Add(FindResource(storyBoardName) as Storyboard);

            _sonicatePressStoryboard = FindResource(_sonicatePressStoryboardName) as Storyboard;

            _resetStoryboard = FindResource(_resetStoryboardName) as Storyboard;
        }

        private const string _sonicateReadySwellStoryboardName = "LStoryboard.SonicateReady.Swell";
        private Storyboard _sonicateReadySwellStoryboard;
        private void AnimateSonicateReady()
        {
            _sonicateReadySwellStoryboard.Completed -= SonicateReadySwellAnimationCompleted;
            _sonicateReadySwellStoryboard.Completed += SonicateReadySwellAnimationCompleted;
            _sonicateReadySwellStoryboard.Begin();
        }

        private void SonicateReadySwellAnimationCompleted(object sender, EventArgs e)
        {
            AnimateSonicateReadyOscillate();
        }

        private List<string> _sonicateReadyOscillateStoryboardNames = new List<string>()
        {
            "LStoryboard.SonicateReady.Oscillate.InnerRipple",
            "LStoryboard.SonicateReady.Oscillate.OuterRipple",
            "LStoryboard.SonicateReady.Oscillate.LongArc",
            "LStoryboard.SonicateReady.Oscillate.ShortArc",
            "LStoryboard.SonicateReady.Oscillate.TrioEllipse1",
            "LStoryboard.SonicateReady.Oscillate.TrioEllipse2",
            "LStoryboard.SonicateReady.Oscillate.TrioPath"
        };

        private List<Storyboard> _sonicateReadyOscillateStoryboards;
        private void AnimateSonicateReadyOscillate()
        {
            foreach (var storyboard in _sonicateReadyOscillateStoryboards)
                storyboard.Begin();
        }

        private const string _sonicatePressStoryboardName = "LStoryboard.SonicatePress";
        private Storyboard _sonicatePressStoryboard;
        private void AnimateSonicatePress()
        {
            foreach (var storyboard in _sonicateReadyOscillateStoryboards)
                storyboard.Stop();

            _sonicatePressStoryboard.Completed -= SonicatePressAnimationCompleted;
            _sonicatePressStoryboard.Completed += SonicatePressAnimationCompleted;
            _sonicatePressStoryboard.Begin();
        }

        private void SonicatePressAnimationCompleted(object sender, EventArgs e)
        {
            _sonicatePressStoryboard.Completed -= SonicatePressAnimationCompleted;
            SonicateCommand?.Execute(null);
        }

        private bool _resetRequired = false;

        private const string _resetStoryboardName = "LStoryboard.Reset";
        Storyboard _resetStoryboard;
        private void AnimateReset()
        {
            _resetStoryboard.Begin();
            _resetRequired = false;
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
