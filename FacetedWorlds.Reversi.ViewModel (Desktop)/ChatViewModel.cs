using System;
using System.Linq;
using FacetedWorlds.Reversi.Model;
using UpdateControls;
using UpdateControls.Correspondence;
using System.Windows.Input;
using UpdateControls.XAML;
using System.Collections.Generic;

namespace FacetedWorlds.Reversi.ViewModel
{
    public class ChatViewModel
    {
        private Player _selectedPlayer;
        private ChatNavigationModel _navigation;

        public ChatViewModel(Player selectedPlayer, ChatNavigationModel navigation)
        {
            _selectedPlayer = selectedPlayer;
            _navigation = navigation;
        }

        public string MessageBody
        {
            get { return _navigation.MessageBody; }
            set { _navigation.MessageBody = value; }
        }

        public ICommand Send
        {
            get
            {
                return MakeCommand
                    .When(() => !String.IsNullOrEmpty(_navigation.MessageBody))
                    .Do(() =>
                    {
                        _selectedPlayer.SendMessage(_navigation.MessageBody);
                        _navigation.MessageBody = string.Empty;
                    });
            }
        }

        public IEnumerable<MessageViewModel> Messages
        {
            get
            {
                return _selectedPlayer.Game.Messages
                    .OrderBy(message => message.Index)
                    .Select(message => new MessageViewModel(message));
            }
        }
    }
}
