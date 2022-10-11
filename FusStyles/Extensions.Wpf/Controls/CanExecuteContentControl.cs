using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/// <summary>
/// Listens to a Command's CanExecute (will not actually execute Command)
/// </summary>
namespace Ws.Extensions.UI.Wpf.Controls
{
    public class CanExecuteContentControl : ContentControl, ICommandSource
    {
        #region Command

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty = 
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(CanExecuteContentControl), new PropertyMetadata(null, new PropertyChangedCallback(OnCommandChanged)));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CanExecuteContentControl canExecuteContentControl)
                canExecuteContentControl.HookUpCommand((ICommand)e.OldValue, (ICommand)e.NewValue);
        }

        private void HookUpCommand(ICommand oldCommand, ICommand newCommand)
        {
            if (oldCommand != null)
                RemoveCommand(oldCommand, newCommand);
            AddCommand(oldCommand, newCommand);
        }

        private void RemoveCommand(ICommand oldCommand, ICommand newCommand)
        {
            oldCommand.CanExecuteChanged -= OnCanExecuteChanged;
        }

        private void AddCommand(ICommand oldCommand, ICommand newCommand)
        {
            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += OnCanExecuteChanged;
                SetIsEnabled();
            }
        }

        #endregion


        #region CommandParameter

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(CanExecuteContentControl));

        #endregion


        #region CommandTarget

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(CanExecuteContentControl));

        #endregion


        #region CanExecute

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            SetIsEnabled();
        }

        private void SetIsEnabled()
        {
            if (Command != null)
            {
                if (Command is RoutedCommand routedCommand)
                    IsEnabled = routedCommand.CanExecute(CommandParameter, CommandTarget);
                else
                    IsEnabled = Command.CanExecute(CommandParameter);
            }
        }

        #endregion
    }
}
