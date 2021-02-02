//using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
//using Ws.Extensions.Mvvm.ViewModels.Switch;
//using Ws.Fus.ImageViewer.UI.Wpf.Commands;
using Ws.Fus.Interfaces.Overlays;

namespace Ws.Fus.ImageViewer.UI.Wpf.Behaviors
{
    public class UiModeShow
    {
        //private static readonly ILogger _logger = Log.ForContext<UiModeShow>();

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.RegisterAttached("Mode", typeof(UiMode), typeof(UiModeShow), new FrameworkPropertyMetadata(UiMode.None/*, OnModeChanged*/));

        /// <summary>
        /// Only one property either <see cref="ModeProperty"/> or <see cref="ModesProperty"/> is supposed to be set.
        /// But if set twice, Modes overrules
        /// </summary>
        public static readonly DependencyProperty ModesProperty =
            DependencyProperty.RegisterAttached("Modes", typeof(string), typeof(UiModeShow), new FrameworkPropertyMetadata(null/*, OnModeChanged*/));

        public static UiMode GetMode(Control obj)
        {
            return (UiMode)obj.GetValue(ModeProperty);
        }

        public static void SetMode(Control obj, UiMode value)
        {
            obj.SetValue(ModeProperty, value);
        }

        public static string GetModes(Control obj)
        {
            return (string)obj.GetValue(ModesProperty);
        }

        public static void SetModes(Control obj, string value)
        {
            obj.SetValue(ModesProperty, value);
        }

        //private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if ((d is MenuItem) || (d is ToggleButton))
        //    {
        //        var ctrl = d as Control;

        //        // prevent double subscription if by mistake, both propertires (Mode and Modes) set
        //        ctrl.DataContextChanged -= OnDataContextChanged;
        //        ctrl.Unloaded -= OnUnloaded;
        //        ctrl.DataContextChanged += OnDataContextChanged;
        //        ctrl.Unloaded += OnUnloaded;
        //    }
        //}

        //private static void OnUnloaded(object sender, RoutedEventArgs e)
        //{
        //    var c = sender as Control;

        //    c.DataContextChanged -= OnDataContextChanged;
        //    c.Unloaded -= OnUnloaded;
        //}

        //private static void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (!(sender is MenuItem) && !(sender is ToggleButton))
        //        return;

        //    var ctrl = sender as Control;

        //    var ovlCommands = ctrl.DataContext as IOverlayCommands;
        //    if (ovlCommands == null)
        //    {
        //        _logger.Warning("Data context not as expected {@ctx}", ctrl.DataContext);
        //        return;
        //    }

        //    var mnu = ctrl as MenuItem;
        //    var btn = ctrl as ToggleButton;
        //    ISwitch swtch = null;

        //    var modesStr = GetModes(ctrl);
        //    if (!string.IsNullOrWhiteSpace(modesStr))
        //    {
        //        try
        //        {
        //            var modesStrArr = modesStr.Split(',');
        //            var modes = modesStrArr.Select(s => (UiMode)Enum.Parse(typeof(UiMode), s));
        //            swtch = ovlCommands.GetCompositeVm(modes.ToArray());
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.Error(ex, "Failed to process multiple modes from {modesStr}", modesStr);
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        var mode = GetMode(ctrl);
        //        if (mode == UiMode.None)
        //            return;

        //        swtch = ovlCommands[mode];
        //    }

        //    if (swtch == null)
        //    {
        //        ctrl.IsEnabled = false;
        //        return;
        //    }

        //    var isCheckedBinding = new Binding
        //    {
        //        Source = swtch,
        //        Path = new PropertyPath(nameof(swtch.IsOn)),
        //        Mode = BindingMode.OneWay,
        //        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //    };

        //    if (mnu != null)
        //    {
        //        mnu.Command = swtch.SwitchCommand;
        //        BindingOperations.SetBinding(mnu, MenuItem.IsCheckedProperty, isCheckedBinding);
        //    }
        //    else
        //    {
        //        btn.Command = swtch.SwitchCommand;
        //        BindingOperations.SetBinding(btn, ToggleButton.IsCheckedProperty, isCheckedBinding);
        //    }
        //}
    }
}
