//using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ws.Extensions.UI.Wpf.Behaviors
{
    public class ProviderBh
    {
        //private static readonly ILogger _logger = Log.ForContext<ProviderBh>();

        /// <summary>
        /// Must be set once, at application startup
        /// </summary>
        public static Func<Type, string, object> Resolver { get; set; }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(string), typeof(ProviderBh), new FrameworkPropertyMetadata(null, OnCommandChanged));

        public static string GetCommand(DependencyObject obj)
        {
            return (string)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, string value)
        {
            obj.SetValue(CommandProperty, value);
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Resolver == null)
            {
                //_logger.Warning("Resolver not set, {value} won't be applied", e.NewValue);
                return;
            }            

            var pi = d.GetType().GetProperty("Command");
            if (pi == null)
            {
                //_logger.Warning("The object doesn't have Command property. {value} won't be applied", e.NewValue);
                return;
            }

            var pt = pi.PropertyType;
            if (!typeof(ICommand).IsAssignableFrom(pt))
            {
                //_logger.Warning("The property is not {type}. {value} won't be applied", typeof(ICommand), e.NewValue);
                return;
            }

            var commandName = e.NewValue as string;
            if (string.IsNullOrWhiteSpace(commandName))
            {
                //_logger.Warning("Command name either not defined or has bad  type. {value} won't be applied", e.NewValue);
                return;
            }

            object commandObj = null;
            try
            {
                commandObj = Resolver(typeof(ICommand), commandName);
            }
            catch (Exception ex)
            {
                //_logger.Warning(ex, "Failed to resolve command {name}", commandName);
                return;
            }

            var command = commandObj as ICommand;

            if (commandObj == null)
            {
                //_logger.Warning("Unexpected type resolved {type}", commandObj.GetType());
                return;
            }

            //_logger.Debug("Setting command {name}", commandName);

            pi.SetValue(d, command);
        }
    }
}
