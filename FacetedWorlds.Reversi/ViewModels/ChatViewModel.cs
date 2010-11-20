using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class ChatViewModel
    {
        private Player _selectedPlayer;
        private MainNavigationModel _mainNavigation;

        public ChatViewModel(Player selectedPlayer, MainNavigationModel mainNavigation)
        {
            _selectedPlayer = selectedPlayer;
            _mainNavigation = mainNavigation;
        }

        public string Title
        {
            get { return OtherPlayerName; }
        }

        public string MessageBody
        {
            get { return _mainNavigation.MessageBody; }
            set { _mainNavigation.MessageBody = value; }
        }

        public ICommand Send
        {
            get
            {
                return MakeCommand
                    .When(() => !String.IsNullOrEmpty(_mainNavigation.MessageBody))
                    .Do(() =>
                    {
                        _selectedPlayer.SendMessage(_mainNavigation.MessageBody);
                        _mainNavigation.MessageBody = string.Empty;
                    });
            }
        }

        public IEnumerable<MessageViewModel> Messages
        {
            get
            {
                return _selectedPlayer.Game.Messages
                    .Select(message => new MessageViewModel(message, _selectedPlayer))
                    .Reverse();
            }
        }

        public void OnClose()
        {
            Player otherPlayer = GetOtherPlayer();
            if (otherPlayer != null)
            {
                foreach (Message newMessage in otherPlayer.NewMessages)
                {
                    newMessage.AcknowledgeMessage();
                }
            }
        }

        private string OtherPlayerName
        {
            get
            {
                var otherPlayer = GetOtherPlayer();
                return otherPlayer == null ? "nobody" : otherPlayer.User.UserName;
            }
        }

        private Player GetOtherPlayer()
        {
            List<Player> players = _selectedPlayer.Game.Players.ToList();
            return players.FirstOrDefault(p => p != _selectedPlayer);
        }
    }
}
