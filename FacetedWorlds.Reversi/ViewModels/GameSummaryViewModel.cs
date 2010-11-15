using System.Linq;
using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;
using FacetedWorlds.Reversi.Model;
using System.Windows.Input;
using UpdateControls.XAML;
using System;
using FacetedWorlds.Reversi.NavigationModels;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class GameSummaryViewModel
    {
        private Player _player;
        private MainNavigationModel _mainNavigation;

        private RemoteGameState _gameState;

        public GameSummaryViewModel(Player player, MainNavigationModel mainNavigation)
        {
            _player = player;
            _mainNavigation = mainNavigation;
            _gameState = new RemoteGameState(player, _mainNavigation);
        }

        public Player Player
        {
            get { return _player; }
        }

        public bool MyTurn
        {
            get { return _gameState.MyTurn; }
        }

        public string OpponentName
        {
            get
            {
                return Opponent == null ? null : Opponent.User.UserName;
            }
        }

        public PieceColor MyColor
        {
            get
            {
                return _player.Index == 0 ? PieceColor.Black : PieceColor.White;
            }
        }

        public int BlackCount
        {
            get { return _gameState.BlackCount; }
        }

        public int WhiteCount
        {
            get { return _gameState.WhiteCount; }
        }

        public bool HasNewMessages
        {
            get
            {
                Player opponent = Opponent;
                return opponent == null ? false : opponent.NewMessages.Any();
            }
        }

        public ICommand OpenGame
        {
            get
            {
                return MakeCommand
                    .Do(() =>
                    {
                        _mainNavigation.SelectedPlayer = _player;
                    });
            }
        }

        private Player Opponent
        {
            get { return _player.Game.Players.FirstOrDefault(p => p != _player); }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            GameSummaryViewModel that = obj as GameSummaryViewModel;
            if (that == null)
                return false;
            return that._player == this._player;
        }

        public override int GetHashCode()
        {
            return _player.GetHashCode();
        }
    }
}
