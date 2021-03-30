using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    public enum ProgressState
    {
        Regular,
        Active,
        Completed,
    }

    /// <summary>
    /// ProgressStatusDisplayer class enabling to add to any DependencyObject info about object's progress stage
    /// </summary>
    public class ProgressStatusDisplayer
    {
        /// <summary>
        /// ProgressState property enables to add to any object info about progress state.
        /// Available values: Regular, Active, Completed
        /// For disabled state use standard IsEnabled property of Control.
        /// </summary>
        public static readonly DependencyProperty ProgressStateProperty = DependencyProperty.RegisterAttached("ProgressState",
            typeof(ProgressState), typeof(ProgressStatusDisplayer), new UIPropertyMetadata(ProgressState.Regular));

        public static ProgressState GetProgressState(DependencyObject obj)
        {
            return (ProgressState)obj.GetValue(ProgressStateProperty);
        }
        public static void SetProgressState(DependencyObject obj, ProgressState value)
        {
            obj.SetValue(ProgressStateProperty, value);
        }


        public static readonly DependencyProperty ActiveAsForegroundProperty = DependencyProperty.RegisterAttached("ActiveAsForeground",
            typeof(bool), typeof(ProgressStatusDisplayer), new UIPropertyMetadata(true));

        public static bool GetActiveAsForeground(DependencyObject obj)
        {
            return (bool)obj.GetValue(ActiveAsForegroundProperty);
        }
        public static void SetActiveAsForeground(DependencyObject obj, bool value)
        {
            obj.SetValue(ActiveAsForegroundProperty, value);
        }



        /// <summary>
        /// Property, enabling to add to any object info about progress percentage. 
        /// Valid values are from 0 to 1
        /// </summary>
        public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.RegisterAttached("ProgressValue",
            typeof(double), typeof(ProgressStatusDisplayer), new UIPropertyMetadata(0d, null, CoerceProgressValue));

        public static double GetProgressValue(DependencyObject obj)
        {
            return (double)obj.GetValue(ProgressValueProperty);
        }
        public static void SetProgressValue(DependencyObject obj, double value)
        {
            obj.SetValue(ProgressValueProperty, value);
        }
        private static object CoerceProgressValue(DependencyObject d, object value)
        {
            double val = 0d;
            if (double.TryParse(value.ToString(), out val))
            {
                val = Math.Max(Math.Min(val, 1), 0);
            }
            return val;
        }

        /// <summary>
        /// Animate when in Active state
        /// </summary>
        public static readonly DependencyProperty AnimateActiveProperty = 
            DependencyProperty.RegisterAttached("AnimateActive", typeof(bool), typeof(ProgressStatusDisplayer), new UIPropertyMetadata(false));
        public static bool GetAnimateActive(DependencyObject obj) { return (bool)obj.GetValue(AnimateActiveProperty); }
        public static void SetAnimateActive(DependencyObject obj, bool value) { obj.SetValue(AnimateActiveProperty, value); }
    }
}
