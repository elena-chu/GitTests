//using NirDobovizki.MvvmMonkey;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using WpfUI.ViewModels;
//using Ws.Extensions.Mvvm.ViewModels;

namespace WpfUI.Messages.ViewModels
{

    //[TypeDescriptionProvider(typeof(MethodBinding))]
    public class GenericMessageViewModel : BindableBase
    {
		public GenericMessageViewModel()
		{
			Buttons = new ObservableCollection<GenericMessageButton>();
			SelectButtonCommand = new DelegateCommand<object>(SelectButtonExecute);
		}

		public DelegateCommand<object> SelectButtonCommand { get; }

		public Action CloseWindow;

		private GenericMessageType _messageType;
		public GenericMessageType MessageType
		{
			get { return _messageType; }
			set { SetProperty(ref _messageType, value); }
		}

		private string _messageText;
		public string MessageText
		{
			get { return _messageText; }
			set { SetProperty(ref _messageText, value); }
		}

		private bool _hasAction;
		public bool HasAction
		{
			get { return _hasAction; }
			set { SetProperty(ref _hasAction, value); }
		}

		private string _actionText;
		public string ActionText
		{
			get { return _actionText; }
			set { SetProperty(ref _actionText, value); }
		}

		private bool _actionChecked;
		public bool ActionChecked
		{
			get { return _actionChecked; }
			set { SetProperty(ref _actionChecked, value); }
		}

		public ObservableCollection<GenericMessageButton> Buttons { get; private set; }

		private string _messageId;
		public string MessageId
		{
			get { return _messageId; }
			set { SetProperty(ref _messageId, value); }
		}

		private GenericMessageReply _selectedButtonResult;
		public GenericMessageReply SelectedButtonResult
		{
			get { return _selectedButtonResult; }
			set { SetProperty(ref _selectedButtonResult, value); }
		}

		public void SelectButtonExecute(object buttonResult)
		{
			SelectedButtonResult = (GenericMessageReply)buttonResult;
			CloseWindow();
		}

	}
}
