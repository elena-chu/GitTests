using Prism.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Ws.Extensions.Mvvm.ViewModels;
using Ws.Extensions.UI.Wpf.Behaviors;
using System.Windows.Media.Media3D;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using Ws.Fus.Treatment.UI.Wpf.LA.Messages;
using System.Collections.Generic;

namespace Ws.Fus.Treatment.UI.Wpf.LA
{
    public class DebugViewModel: ViewModelBase
    {
        public DebugViewModel()
        {
            ProgressCommand = new DelegateCommand(ToggleProgress);
            ApprovalCommand = new DelegateCommand(ToggleApproval);
            SendMessageCommand = new DelegateCommand(SendMessage);
            InitMenu();

            StoppedValue = 5;
        }


        #region Progress

        private ProgressState _progressState = ProgressState.Regular;
        public ProgressState ProgressState
        {
            get { return _progressState; }
            set
            {
                _progressState = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ProgressCommand { get; private set; }
        private void ToggleProgress()
        {
            switch (ProgressState)
            {
                case ProgressState.Regular:
                    OnChangeProgress(true);
                    break;
                case ProgressState.Active:
                    OnChangeProgress(false);
                    break;
                case ProgressState.Completed:
                default:
                    break;
            }
        }

        private CancellationTokenSource source;
        private void OnChangeProgress(bool go)
        {
            if (go)
            {
                ProgressState = ProgressState.Active;
                IncrementProgress();
            }
            else if (source != null)
            {
                source.Cancel();
            }
        }

        private async void IncrementProgress()
        {
            source = new CancellationTokenSource();
            source.Token.ThrowIfCancellationRequested();

            try
            {
                await Task.Delay(3000, source.Token);
                ProgressState = ProgressState.Completed;
            }
            catch (OperationCanceledException)
            {
                ProgressState = ProgressState.Regular;
                source.Dispose();
                return;
            }

            if (ProgressState == ProgressState.Completed)
            {
                await Task.Delay(5000);
                ProgressState = ProgressState.Regular;
            }

            source.Dispose();
        }

        #endregion


        #region Approval

        private ProgressState _approvalState = ProgressState.Regular;
        public ProgressState ApprovalState
        {
            get { return _approvalState; }
            set
            {
                _approvalState = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ApprovalCommand { get; private set; }
        private void ToggleApproval()
        {
            switch (ApprovalState)
            {
                case ProgressState.Regular:
                    ApprovalState = ProgressState.Completed;
                    break;
                case ProgressState.Completed:
                    ApprovalState = ProgressState.Regular;
                    break;
                case ProgressState.Active:
                default:
                    break;
            }
        }

        #endregion


        #region Menu

        private void InitMenu()
        {
            ZoomCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Zoom", x); } );
            PanCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Pan", x); } );
            LineCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Line", x); } );
            AreaCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Area", x); } );
            OverlaysCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Overlays", x); } );
            ImageInfoCommand = new DelegateCommand<bool?>(x => { MenuItemClicked("Image Info", x); } );
    }

        public ICommand ZoomCommand { get; set; }
        public ICommand PanCommand { get; set; }
        public ICommand LineCommand { get; set; }
        public ICommand AreaCommand { get; set; }
        public ICommand OverlaysCommand { get; set; }
        public ICommand ImageInfoCommand { get; set; }

        private void MenuItemClicked(string name, bool? param)
        {
            string status = param.HasValue ? param.Value.ToString() : "null";
            Console.WriteLine("LA >>> " + name + " is calling. I am " + status);
        }

        #endregion


        #region Triplet

        private Point3D? _point;
        public Point3D? Point
        {
            get { return _point; }
            set { SetProperty(ref _point, value); }
        }

        private double? _value;
        public double? Value
        {
            get { return _value; }
            set
            {
                if (SetProperty(ref _value, value))
                    Debug.WriteLine("Value set: " + _value);
            }
        }

        private double? _stoppedValue;
        public double? StoppedValue
        {
            get { return _stoppedValue; }
            set
            {
                if (SetProperty(ref _stoppedValue, value))
                    Debug.WriteLine("StoppedValue set: " + _stoppedValue);
            }
        }

        #endregion


        #region Info

        public BitmapImage TestImage
        {
            get
            {
                var imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\ImageLoad\ImageLoad\Images\");
                var fullPath = Path.GetFullPath(new Uri(imageFolder).LocalPath);
                var imageName = Directory.GetFiles(fullPath, "*.*").First(file => file.ToLower().EndsWith("png") || file.ToLower().EndsWith("bmp"));
                return new BitmapImage(new Uri(imageName));
            }
        }

        #endregion


        #region Message

        private int status = 0;
        private bool isChecked = false;
        private List<string> quotes = new List<string>()
        {
            "Hello. My name is Inigo Montoya. You killed my father. Prepare to die",
            "But it's so simple. All I have to do is divine from what I know of you: are you the sort of man who would put the poison into his own goblet or his enemy's? Now, a clever man would put the poison into his own goblet, because he would know that only a great fool would reach for what he was given. I am not a great fool, so I can clearly not choose the wine in front of you. But you must have known I was not a great fool, you would have counted on it, so I can clearly not choose the wine in front of me.",
            "Anybody want a peanut?",
            "Death cannot stop true love. All it can do is delay it for a while.",
            "Prince Humperdinck: First things first, to the death. Westley: No. To the pain. Prince Humperdinck: I don't think I'm quite familiar with that phrase. Westley: I'll explain and I'll use small words so that you'll be sure to understand, you warthog faced buffoon. Prince Humperdinck: That may be the first time in my life a man has dared insult me. Westley: It won't be the last. To the pain means the first thing you will lose will be your feet below the ankles. Then your hands at the wrists. Next your nose. Prince Humperdinck: And then my tongue I suppose, I killed you too quickly the last time. A mistake I don't mean to duplicate tonight. Westley: I wasn't finished. The next thing you will lose will be your left eye followed by your right. Prince Humperdinck: And then my ears, I understand let's get on with it. Westley: WRONG. Your ears you keep and I'll tell you why. So that every shriek of every child at seeing your hideousness will be yours to cherish. Every babe that weeps at your approach, every woman who cries out, Dear God! What is that thing, will echo in your perfect ears. That is what to the pain means. It means I leave you in anguish, wallowing in freakish misery forever. Prince Humperdinck: I think you're bluffing. Westley: It's possible, Pig, I might be bluffing. It's conceivable, you miserable, vomitous mass, that I'm only lying here because I lack the strength to stand. But, then again... perhaps I have the strength after all."
        };
        public ICommand SendMessageCommand { get; set; }
        private void SendMessage()
        {
            MessageService.ShowMessage(new MessageRequestedEventArgs()
            {
                MessageText = quotes[status],
                MessageTitle = "The Princess Bride",
                MessageType = (GenericMessageType)status,
                MessageId = "12345",
                HasAction = isChecked,
                ActionText = "Vizzini",
                ActionChecked = false,
                Buttons = new MessageRequestedButton[]
                {
                    new MessageRequestedButton()
                    {
                        Text = "Fezzik",
                        Tip = "Anybody want a peanut?",
                        Result = GenericMessageReply.No,
                    },
                    new MessageRequestedButton()
                    {
                        Text = "Inigo",
                        Tip = "Prepare to die",
                        Result = GenericMessageReply.Cancel,
                    },
                    new MessageRequestedButton()
                    {
                        Text = "Westley",
                        Tip = "As you wish",
                        Result = GenericMessageReply.Ok,
                    },
                },
            });

            status = (status + 1) % 5;
            isChecked = !isChecked;
        }
        #endregion
    }
}
