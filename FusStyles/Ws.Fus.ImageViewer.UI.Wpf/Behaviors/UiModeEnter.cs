using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
//using Ws.Fus.ImageViewer.UI.Wpf.Commands;
using Ws.Fus.Interfaces.Overlays;

namespace Ws.Fus.ImageViewer.UI.Wpf.Behaviors
{
    public class UiModeEnter
    {
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.RegisterAttached("Mode", typeof(UiMode), typeof(UiModeEnter), new FrameworkPropertyMetadata(UiMode.None/*, OnModeChanged*/));

        public static readonly DependencyProperty SubModeProperty =
            DependencyProperty.RegisterAttached("SubMode", typeof(string), typeof(UiModeEnter), new PropertyMetadata(null));

        public static UiMode GetMode(Control obj)
        {
            return (UiMode)obj.GetValue(ModeProperty);
        }

        public static void SetMode(Control obj, UiMode value)
        {
            obj.SetValue(ModeProperty, value);
        }

        public static string GetSubMode(Control obj)
        {
            return (string)obj.GetValue(SubModeProperty);
        }

        public static void SetSubMode(Control obj, string value)
        {
            obj.SetValue(SubModeProperty, value);
        }

        //private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if ((d is MenuItem) || (d is ToggleButton))
        //    {
        //        (d as Control).DataContextChanged += OnDataContextChanged;
        //        (d as Control).Unloaded += OnUnloaded;
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

        //    var mode = GetMode(ctrl);
        //    if (mode == UiMode.None)
        //        return;

        //    var msr = ctrl.DataContext as IMeasureCommands;
        //    if (msr == null)
        //        return;

        //    UiMode subModeEnum;
        //    var subModeStr = GetSubMode(ctrl);
        //    if (Enum.TryParse<UiMode>(subModeStr, out subModeEnum))
        //        subModeStr = ((int)subModeEnum).ToString();

        //    var mnu = ctrl as MenuItem;
        //    var btn = ctrl as ToggleButton;

        //    var isCheckedBinding = new Binding
        //    {
        //        Mode = BindingMode.OneWay,
        //        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //    };

        //    if (string.IsNullOrWhiteSpace(subModeStr))
        //    {
        //        var sw = msr[mode];
        //        if (sw == null)
        //        {
        //            ctrl.IsEnabled = false;
        //            return;
        //        }

        //        isCheckedBinding.Source = sw;
        //        isCheckedBinding.Path = new PropertyPath(nameof(sw.IsOn));

        //        if (mnu != null)
        //        {
        //            mnu.Command = sw.SwitchCommand;
        //            BindingOperations.SetBinding(mnu, MenuItem.IsCheckedProperty, isCheckedBinding);
        //        }
        //        else
        //        {
        //            btn.Command = sw.SwitchCommand;
        //            BindingOperations.SetBinding(btn, ToggleButton.IsCheckedProperty, isCheckedBinding);
        //        }
        //    }
        //    else
        //    {
        //        var sw = msr.GetMultiMeasureVm(mode);
        //        if (sw == null)
        //        {
        //            ctrl.IsEnabled = false;
        //            return;
        //        }

        //        isCheckedBinding.Source = sw;
        //        // IsChecked="{Binding SubSwitches[subMode]}"
        //        isCheckedBinding.Path = new PropertyPath($"{nameof(sw.SubSwitches)}[{subModeStr}]");

        //        if (mnu != null)
        //        {
        //            mnu.Command = sw.SwitchCommand;
        //            mnu.CommandParameter = subModeStr;
        //            BindingOperations.SetBinding(mnu, MenuItem.IsCheckedProperty, isCheckedBinding);
        //        }
        //        else
        //        {
        //            btn.Command = sw.SwitchCommand;
        //            btn.CommandParameter = subModeStr;
        //            BindingOperations.SetBinding(btn, ToggleButton.IsCheckedProperty, isCheckedBinding);
        //        }
        //    }
        //}
    }
}
