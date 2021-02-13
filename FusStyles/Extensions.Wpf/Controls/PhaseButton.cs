using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ws.Extensions.Mvvm.ViewModels;

namespace Ws.Extensions.UI.Wpf.Controls
{
    public enum ProcessPhase
    {
        NotStarted,
        Preparing,
        Executing,
        Paused,
        Canceling,
        Completed
    }

    public class PhaseButton : Button, INotifyPropertyChanged
    {
        public ProcessPhase CurrentPhase
        {
            get { return (ProcessPhase)GetValue(CurrentPhaseProperty); }
            set { SetValue(CurrentPhaseProperty, value); }
        }
        public static readonly DependencyProperty CurrentPhaseProperty = 
            DependencyProperty.Register("CurrentPhase", typeof(ProcessPhase), typeof(PhaseButton), new PropertyMetadata(ProcessPhase.NotStarted, OnPhaseChanged));

        private static void OnPhaseChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as PhaseButton).NotifyPhaseChanged();
        }

        public string CurrentCaption { get => Phases?[CurrentPhase].Caption; }

        public Geometry CurrentIcon { get => Phases?[CurrentPhase].Icon; }

        public void NotifyPhaseChanged()
        {
            OnPropertyChanged(nameof(CurrentCaption));
            OnPropertyChanged(nameof(CurrentIcon));
        }

        public class PhaseMaterials : NotifyPropertyChangedImplementation
        {
            private string _caption;
            public string Caption
            {
                get => _caption;
                set
                {
                    if (value != _caption)
                    {
                        _caption = value;
                        OnPropertyChanged();
                    }
                }
            }

            private Geometry _icon;
            public Geometry Icon
            {
                get => _icon;
                set
                {
                    _icon = value;
                    OnPropertyChanged();
                }
            }

            public PhaseMaterials(string caption, Geometry icon)
            {
                Caption = caption;
                Icon = icon;
            }
        }

        private ObservableDictionary<ProcessPhase, PhaseMaterials> Phases;

        private void SetPhaseCaption(ProcessPhase phase, string caption)
        {
            if (Phases == null)
            {
                Phases = new ObservableDictionary<ProcessPhase, PhaseMaterials>();
                OnPropertyChanged(nameof(Phases));
            }

            if (!Phases.ContainsKey(phase))
                Phases[phase] = new PhaseMaterials(caption, null);
            else
                Phases[phase].Caption = caption;

            if (phase == CurrentPhase)
                OnPropertyChanged(nameof(CurrentCaption));
        }

        public string CaptionNotStarted { set { SetPhaseCaption(ProcessPhase.NotStarted, value); } }
        public string CaptionPreparing { set { SetPhaseCaption(ProcessPhase.Preparing, value); } }
        public string CaptionExecuting { set { SetPhaseCaption(ProcessPhase.Executing, value); } }
        public string CaptionPaused { set { SetPhaseCaption(ProcessPhase.Paused, value); } }
        public string CaptionCanceling { set { SetPhaseCaption(ProcessPhase.Canceling, value); } }
        public string CaptionCompleted { set { SetPhaseCaption(ProcessPhase.Completed, value); } }

        private void SetPhaseIcon(ProcessPhase phase, Geometry icon)
        {
            if (Phases == null)
                Phases = new ObservableDictionary<ProcessPhase, PhaseMaterials>();

            if (!Phases.ContainsKey(phase))
                Phases[phase] = new PhaseMaterials(null, icon);
            else
                Phases[phase].Icon = icon;

            if (phase == CurrentPhase)
                OnPropertyChanged(nameof(CurrentIcon));
        }

        public Geometry IconNotStarted { set { SetPhaseIcon(ProcessPhase.NotStarted, value); } }
        public Geometry IconPreparing { set { SetPhaseIcon(ProcessPhase.Preparing, value); } }
        public Geometry IconExecuting { set { SetPhaseIcon(ProcessPhase.Executing, value); } }
        public Geometry IconPaused { set { SetPhaseIcon(ProcessPhase.Paused, value); } }
        public Geometry IconCanceling { set { SetPhaseIcon(ProcessPhase.Canceling, value); } }
        public Geometry IconCompleted { set { SetPhaseIcon(ProcessPhase.Completed, value); } }


        public double ProgressPercentage
        {
            get { return (double)GetValue(ProgressPercentageProperty); }
            set { SetValue(ProgressPercentageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressPercentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressPercentageProperty =
            DependencyProperty.Register("ProgressPercentage", typeof(double), typeof(PhaseButton), new PropertyMetadata(0.0));


        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
