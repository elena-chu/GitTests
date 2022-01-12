﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUI.Messages
{
		public enum GenericMessageReply
	{
		Ok,Yes,No,Cancel,ClosedByProgram
	}

	public class MessageRequestedEventArgs : EventArgs
	{
		public string MessageText;
		public string MessageTitle;
		public GenericMessageType MessageType;
		public string MessageId;
		public bool HasAction;
		public string ActionText;
		public bool ActionChecked;
		public MessageRequestedButton[] Buttons;
		public GenericMessageReply SelectedButtonResult;
	}

	public class MessageRequestedButton
	{
		public string Text;
		public string Tip;
		public GenericMessageReply Result;
		public bool IsSecondary;
	}

	public delegate void MessageRequestedEventHandler(object sender, MessageRequestedEventArgs ea);
}
