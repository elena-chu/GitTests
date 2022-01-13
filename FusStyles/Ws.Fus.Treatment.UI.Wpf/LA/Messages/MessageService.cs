using System;
using System.Linq;
using System.Windows;
using Ws.Fus.Treatment.UI.Wpf.LA.Messages.ViewModels;
using Ws.Fus.Treatment.UI.Wpf.LA.Messages.Views;

namespace Ws.Fus.Treatment.UI.Wpf.LA.Messages
{
    //Temp class 
    public class MessageService
    {
        public static void ShowMessage(MessageRequestedEventArgs ea, Action<MessageRequestedEventArgs> closeCallback = null)
        {
            Application.Current.Dispatcher.Invoke(() => DoShowMessage(ea, closeCallback));
        }

        private const string MESSAGE_WINDOW_STYLE_NAME = "XStyle.Window.Message";
        public static void DoShowMessage(MessageRequestedEventArgs ea, Action<MessageRequestedEventArgs> closeCallback = null)
        {
            var vm = new GenericMessageViewModel();
            vm.ActionChecked = ea.ActionChecked;
            vm.ActionText = ea.ActionText;
            vm.HasAction = ea.HasAction;
            vm.MessageId = ea.MessageId;
            vm.MessageText = ea.MessageText;
            vm.Title = ea.MessageTitle;
            vm.MessageType = ea.MessageType;
            if (ea.Buttons == null)
            {
                vm.Buttons.Add(new GenericMessageButton() { ButtonText = "OK", ButtonResult = GenericMessageReply.Ok });
            }
            else
            {
                ea.Buttons
                    .Select(o => new GenericMessageButton { ButtonText = string.IsNullOrWhiteSpace(o.Text) ? null : o.Text, ButtonTip = string.IsNullOrWhiteSpace(o.Tip) ? null : o.Tip, ButtonResult = o.Result })
                    .ToList()
                    .ForEach(o => vm.Buttons.Add(o));
            }

            var wnd = new Window();
            wnd.Style = (Style)Application.Current.TryFindResource(MESSAGE_WINDOW_STYLE_NAME);
            var v = new GenericMessageView();
            v.DataContext = vm;
            wnd.Content = v;
            wnd.Owner = Application.Current.MainWindow;
            wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            vm.CloseWindow = () =>
            {
                ea.ActionChecked = vm.ActionChecked;
                ea.SelectedButtonResult = vm.SelectedButtonResult;

                wnd.Close();
                closeCallback?.Invoke(ea);
            };
            wnd.ShowDialog();

            //ea.ActionChecked = vm.ActionChecked;
            //ea.SelectedButtonResult = vm.SelectedButtonResult;
        }
    }
}
