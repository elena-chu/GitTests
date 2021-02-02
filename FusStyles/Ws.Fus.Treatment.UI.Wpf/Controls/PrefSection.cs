using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
using Ws.Fus.Treatment.UI.Wpf.Entities;
using Ws.Fus.Treatment.UI.Wpf.ViewModels;

namespace Ws.Fus.Treatment.UI.Wpf.Controls
{
    class PrefSection : ContentControl
    {
        public static readonly DependencyProperty PrefUidProperty =
            DependencyProperty.Register(nameof(PrefUid), typeof(string), typeof(PrefSection), new PropertyMetadata(null));

        public static readonly DependencyProperty PrefSubSectionProperty =
            DependencyProperty.Register(nameof(PrefSubSection), typeof(string), typeof(PrefSection), new PropertyMetadata(null));

        public static readonly DependencyProperty PrefModeProperty =
            DependencyProperty.Register(nameof(PrefMode), typeof(PreferencesMode), typeof(PrefSection), new PropertyMetadata(PreferencesMode.Local));
               
        public PrefSection()
        {
            Initialized += OnInitialized;
        }        

        public string PrefUid
        {
            get { return (string)GetValue(PrefUidProperty); }
            set { SetValue(PrefUidProperty, value); }
        }

        public string PrefSubSection
        {
            get { return (string)GetValue(PrefSubSectionProperty); }
            set { SetValue(PrefSubSectionProperty, value); }
        }

        public PreferencesMode PrefMode
        {
            get { return (PreferencesMode)GetValue(PrefModeProperty); }
            set { SetValue(PrefModeProperty, value); }
        }

        private void OnInitialized(object sender, EventArgs e)
        {
            var vm = DataContext as PreferencesViewModel;
            if (vm == null)
                return;

            var resourceKey = PrefUid;
            if (!string.IsNullOrWhiteSpace(PrefSubSection))
                resourceKey += $"/{PrefSubSection}";

            ContentTemplate = FindResource(resourceKey) as DataTemplate;

            vm.PropertyChanged += OnVmPropertyChanged;

            IsEnabled = false;

            var prefUid = PrefUid;
            var prefMode = PrefMode;
            Task.Run(() => vm.CreateSectionAsync(prefUid, prefMode));
        }

        private void OnVmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                if (e.PropertyName != PrefUid)
                    return;

                var content = (sender as PreferencesViewModel).GetSectionViewModel(PrefUid);

                if (content != Content)
                {
                    Content = content;
                    IsEnabled = true;
                };
            });
        }
    }
}
