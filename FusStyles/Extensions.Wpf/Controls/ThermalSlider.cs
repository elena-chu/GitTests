﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Ws.Extensions.UI.Wpf.Controls
{
    public class ThermalSlider : Slider, INotifyPropertyChanged
    {
        public ThermalSlider() : base() { }
        static ThermalSlider()
        {
            MaximumProperty.OverrideMetadata(typeof(ThermalSlider), new FrameworkPropertyMetadata(OnMaximumChanged));
            MinimumProperty.OverrideMetadata(typeof(ThermalSlider), new FrameworkPropertyMetadata(OnMinimumChanged));
            ValueProperty.OverrideMetadata(typeof(ThermalSlider), new FrameworkPropertyMetadata(OnValueChanged));
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ThermalSlider thermalSlider)
                thermalSlider.UpdateDegreesAboveValue();
        }

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ThermalSlider thermalSlider)
                thermalSlider.UpdateDegreesBelowValue();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ThermalSlider thermalSlider)
            {
                thermalSlider.UpdateDegreesAboveValue();
                thermalSlider.UpdateDegreesBelowValue();
            }
        }

        private void UpdateDegreesAboveValue()
        {
            int numberOfDegrees = (int)Maximum - (int)Value + 1;
            ObservableCollection<int> degreesAboveValue = new ObservableCollection<int>();
            for (int i = numberOfDegrees-1; i >= 0; i--)
                degreesAboveValue.Add(i + (int)Value);
            DegreesAboveValue = degreesAboveValue;
            OnPropertyChanged(nameof(DegreesAboveValue));
        }

        private void UpdateDegreesBelowValue()
        {
            int numberOfDegrees = (int)Value - (int)Minimum;
            ObservableCollection<int> degreesBelowValue = new ObservableCollection<int>();
            for (int i = numberOfDegrees-1; i >= 0; i--)
                degreesBelowValue.Add(i + (int)Minimum);
            DegreesBelowValue = degreesBelowValue;
            OnPropertyChanged(nameof(DegreesBelowValue));
        }

        public ObservableCollection<int> DegreesAboveValue { get; private set; } = new ObservableCollection<int>();
        public ObservableCollection<int> DegreesBelowValue { get; private set; } = new ObservableCollection<int>();


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
