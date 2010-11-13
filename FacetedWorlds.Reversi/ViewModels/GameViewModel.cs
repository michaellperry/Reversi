using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;
using FacetedWorlds.Reversi.Model;
using UpdateControls.XAML;
using System;
using FacetedWorlds.Reversi.NavigationModels;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class GameViewModel
    {
        private Player _player;
        private GameState _gameState;
        private MainNavigationModel _mainNavigation;

        public GameViewModel(Player player, MainNavigationModel mainNavigation)
        {
            _player = player;
            _mainNavigation = mainNavigation;
            _gameState = new GameState(player, mainNavigation);
        }

        public string Name
        {
            get
            {
                return
                    MyColor == PieceColor.Black
                        ? string.Format("you vs. {0}", OtherPlayerName)
                        : String.Format("{0} vs. you", OtherPlayerName);
            }
        }

        public string ToMove
        {
            get
            {
                return _gameState.MyTurn
                    ? "your move"
                    : string.Format("{0}'s move", OtherPlayerName);
            }
        }

        public bool HasNewMessages
        {
            get
            {
                Player otherPlayer = GetOtherPlayer();
                return otherPlayer == null ? false : otherPlayer.NewMessages.Any();
            }
        }

        public ICommand Resign
        {
            get
            {
                return MakeCommand
                    .Do(() => _player.Game.DeclareWinner(GetOtherPlayer()));
            }
        }

        public PieceColor MyColor
        {
            get { return _gameState.MyColor; }
        }

        public PieceColor OpponentColor
        {
            get { return _gameState.MyColor.Opposite(); }
        }

        public bool MyTurn
        {
            get { return _gameState.MyTurn; }
        }

        public bool IWon
        {
            get { return _gameState.IWon; }
        }

        public bool ILost
        {
            get { return _gameState.ILost; }
        }

        public bool IDrew
        {
            get { return _gameState.IDrew; }
        }

        public IEnumerable<RowViewModel> Rows
        {
            get
            {
                for (int row = 0; row < Square.NumberOfRows; row++)
                    yield return new RowViewModel(_gameState, row);
            }
        }

        public bool IsMovePending
        {
            get { return _gameState.IsMovePending; }
        }

        public void PreviewMove(int row, int column)
        {
            _gameState.SetPreviewMove(new Square(row, column));
        }

        public void ClearPreviewMove()
        {
            _gameState.SetPreviewMove(null);
        }

        public void MakeMove(int row, int column)
        {
            _gameState.MakeMove(new Square(row, column));
        }

        public void CommitMove()
        {
            _gameState.CommitMove();
        }

        public void CancelMove()
        {
            _gameState.CancelMove();
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
            List<Player> players = _player.Game.Players.ToList();
            return players.FirstOrDefault(p => p != _player);
        }

        public void ClearSelectedPlayer()
        {
            _mainNavigation.SelectedPlayer = null;
        }
    }
}
