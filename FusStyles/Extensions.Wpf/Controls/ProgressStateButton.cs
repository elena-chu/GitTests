using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Extensions.Patterns;
using Ws.Extensions.UI.Wpf.Behaviors;

namespace Ws.Extensions.UI.Wpf.Controls
{
    public class ProgressStateButton : Button, INotifyPropertyChanged
    {
        public ProgressState CurrentProgressState
        {
            get { return (ProgressState)GetValue(CurrentProgressStateProperty); }
            set { SetValue(CurrentProgressStateProperty, value); }
        }
        public static readonly DependencyProperty CurrentProgressStateProperty = 
            DependencyProperty.Register("CurrentProgressState", typeof(ProgressState), typeof(ProgressStateButton), new PropertyMetadata(ProgressState.Regular, OnProgressStateChanged));

        private static void OnProgressStateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as ProgressStateButton).NotifyProgressStateChanged();
        }

        public string CurrentCaption { get => ProgressStates?[CurrentProgressState].Caption; }

        public Geometry CurrentIcon { get => ProgressStates?[CurrentProgressState].Icon; }

        public void NotifyProgressStateChanged()
        {
            OnPropertyChanged(nameof(CurrentCaption));
            OnPropertyChanged(nameof(CurrentIcon));
        }

        public class ProgressStateMaterials : NotifyPropertyChangedImplementation
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

            public ProgressStateMaterials(string caption, Geometry icon)
            {
                Caption = caption;
                Icon = icon;
            }
        }

        private ObservableDictionary<ProgressState, ProgressStateMaterials> ProgressStates;

        private void SetProgressStateCaption(ProgressState progressState, string caption)
        {
            if (ProgressStates == null)
            {
                ProgressStates = new ObservableDictionary<ProgressState, ProgressStateMaterials>();
                OnPropertyChanged(nameof(ProgressStates));
            }

            if (!ProgressStates.ContainsKey(progressState))
                ProgressStates[progressState] = new ProgressStateMaterials(caption, null);
            else
                ProgressStates[progressState].Caption = caption;

            if (progressState == CurrentProgressState)
                OnPropertyChanged(nameof(CurrentCaption));
        }

        public string CaptionRegular { set { SetProgressStateCaption(ProgressState.Regular, value); } }
        public string CaptionActive { set { SetProgressStateCaption(ProgressState.Active, value); } }
        public string CaptionCompleted { set { SetProgressStateCaption(ProgressState.Completed, value); } }

        private void SetProgressStateIcon(ProgressState progressState, Geometry icon)
        {
            if (ProgressStates == null)
                ProgressStates = new ObservableDictionary<ProgressState, ProgressStateMaterials>();

            if (!ProgressStates.ContainsKey(progressState))
                ProgressStates[progressState] = new ProgressStateMaterials(null, icon);
            else
                ProgressStates[progressState].Icon = icon;

            if (progressState == CurrentProgressState)
                OnPropertyChanged(nameof(CurrentIcon));
        }

        public Geometry IconRegular { set { SetProgressStateIcon(ProgressState.Regular, value); } }
        public Geometry IconActive { set { SetProgressStateIcon(ProgressState.Active, value); } }
        public Geometry IconCompleted { set { SetProgressStateIcon(ProgressState.Completed, value); } }


        public double ProgressPercentage
        {
            get { return (double)GetValue(ProgressPercentageProperty); }
            set { SetValue(ProgressPercentageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressPercentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressPercentageProperty =
            DependencyProperty.Register("ProgressPercentage", typeof(double), typeof(ProgressStateButton), new PropertyMetadata(0.0));


        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
