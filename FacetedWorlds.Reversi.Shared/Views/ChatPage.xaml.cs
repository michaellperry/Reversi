using System.Windows.Navigation;
using FacetedWorlds.Reversi.ViewModels;
using Microsoft.Phone.Controls;
using UpdateControls.XAML;
using System;
using Microsoft.Phone.Shell;
using UpdateControls;

namespace FacetedWorlds.Reversi.Views
{
    public partial class ChatPage : PhoneApplicationPage
    {
        private ApplicationBarIconButton _sendButton;
        private Dependent _depSendEnabled;

        public ChatPage()
        {
            InitializeComponent();

            ApplicationBar = new ApplicationBar();
            _sendButton = AddButton("/Images/appbar.send.rest.png", "send",
                (sender, args) => ViewModel.Send.Execute(null));
            _depSendEnabled = new Dependent(delegate
            {
                _sendButton.IsEnabled = ViewModel.Send.CanExecute(null);
            });
            _depSendEnabled.OnGet();
            _depSendEnabled.Invalidated += delegate
            {
                Dispatcher.BeginInvoke(() => _depSendEnabled.OnGet());
            };
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.OnClose();
            base.OnNavigatedFrom(e);
        }

        private ChatViewModel ViewModel
        {
            get { return ForView.Unwrap<ChatViewModel>(DataContext); }
        }

        private ApplicationBarIconButton AddButton(string imageUrl, string text, EventHandler click)
        {
            ApplicationBarIconButton newButton = new ApplicationBarIconButton(new Uri(imageUrl, UriKind.Relative));
            newButton.Text = text;
            newButton.Click += click;
            newButton.IsEnabled = true;
            ApplicationBar.Buttons.Add(newButton);
            return newButton;
        }
    }
}
