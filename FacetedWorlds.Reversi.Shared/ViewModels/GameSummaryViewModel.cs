using System.Linq;
using System.Windows.Input;
using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class GameSummaryViewModel : ViewModelBase
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
            get { return Get(() => _gameState.MyTurn); }
        }

        public string OpponentName
        {
            get
            {
                return Get(() => Opponent == null ? null : Opponent.User.UserName);
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
            get { return Get(() => _gameState.BlackCount); }
        }

        public int WhiteCount
        {
            get { return Get(() => _gameState.WhiteCount); }
        }

        public bool HasNewMessages
        {
            get
            {
                return Get(() => Opponent == null ? false : Opponent.NewMessages.Any());
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
