using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Input;
using WpfUI.Messages.ViewModels;
using WpfUI.Messages.Views;
using Ws.Extensions.UI.Wpf.Utils;

namespace WpfUI.Messages
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
            wnd.MouseDown += Wnd_MouseDown;
            vm.CloseWindow = () =>
            {
                ea.ActionChecked = vm.ActionChecked;
                ea.SelectedButtonResult = vm.SelectedButtonResult;

                wnd.MouseDown -= Wnd_MouseDown;
                wnd.Close();
                closeCallback?.Invoke(ea);
            };

            Stream messageStream = Properties.Resources.Infomsg;
            switch (vm.MessageType)
            {
                case GenericMessageType.Safety:
                    messageStream = Properties.Resources.WarningMsg;
                    break;
                case GenericMessageType.UserError:
                    messageStream = Properties.Resources.CautionMsg;
                    break;
                case GenericMessageType.SystemError:
                    messageStream = Properties.Resources.ErrorMsg;
                    break;
                case GenericMessageType.UserInfo:
                case GenericMessageType.SystemStatus:
                default:
                    break;
            }
            SoundsHelper.PlaySound(messageStream);

            wnd.ShowDialog();

            //ea.ActionChecked = vm.ActionChecked;
            //ea.SelectedButtonResult = vm.SelectedButtonResult;
        }

        private static void Wnd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Window window && e.ChangedButton == MouseButton.Left)
                window.DragMove();
        }
    }
}
