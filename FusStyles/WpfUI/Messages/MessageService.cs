using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfUI.Messages.ViewModels;
using WpfUI.Messages.Views;

namespace WpfUI.Messages
{
    //Temp class 
    public class MessageService
    {
        public static void ShowMessage(MessageRequestedEventArgs ea, Action<MessageRequestedEventArgs> closeCallback = null)
        {
            Application.Current.Dispatcher.Invoke(() => DoShowMessage(ea, closeCallback));
        }
		
        public static void DoShowMessage(MessageRequestedEventArgs ea, Action<MessageRequestedEventArgs> closeCallback = null)
        {
            var vm = new GenericMessageViewModel();
            vm.ActionChecked = ea.ActionChecked;
            vm.ActionText = ea.ActionText;
            vm.HasAction = ea.HasAction;
            vm.MessageId = ea.MessageId;
            vm.MessageText = ea.MessageText;
            vm.MessageType = ea.MessageType;
            if(ea.Buttons == null)
            {
                vm.Buttons.Add(new GenericMessageButton() { ButtonText = "OK", ButtonResult =GenericMessageReply.Ok });
            }
            else
            {
                ea.Buttons
                    .Select(o => new GenericMessageButton { ButtonText = string.IsNullOrWhiteSpace(o.Text) ? null : o.Text, ButtonTip = string.IsNullOrWhiteSpace(o.Tip) ? null : o.Tip, ButtonResult = o.Result })
                    .ToList()
                    .ForEach(o => vm.Buttons.Add(o));
            }

            var wnd = new Window();
            wnd.SizeToContent = SizeToContent.WidthAndHeight;
            wnd.ResizeMode = ResizeMode.NoResize;
            wnd.WindowStyle = WindowStyle.None;
            var v = new GenericMessageView();
            v.DataContext = vm;
            wnd.Content = v; //vm;
            wnd.Owner = Application.Current.MainWindow;
            wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            wnd.Topmost = true;
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
