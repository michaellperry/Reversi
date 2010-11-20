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
    public class LocalGameViewModel : IGameViewModel
    {
        private LocalGameState _gameState;
        private MainNavigationModel _mainNavigation;

        public LocalGameViewModel(LocalGame localGame, MainNavigationModel mainNavigation)
        {
            _gameState = new LocalGameState(localGame, mainNavigation);
            _mainNavigation = mainNavigation;
        }

        public string Name
        {
            get { return "pass the phone"; }
        }

        public bool HasNewMessages
        {
            get { return false; }
        }

        public bool CanChat
        {
            get { return false; }
        }

        public ICommand Resign
        {
            get
            {
                return MakeCommand
                    .Do(() =>
                    {
                        int otherPlayerIndex =
                            _gameState.MyColor == PieceColor.Black ? 1 :
                            _gameState.MyColor == PieceColor.White ? 0 :
                            -1;

                        LocalPlayer otherPlayer = _gameState.Game.Players.FirstOrDefault(
                            player => player.Index == otherPlayerIndex);
                        _gameState.Game.DeclareWinner(otherPlayer, true);
                    });
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
            get { return !_gameState.IsMovePending && _gameState.Game.Outcome == null; }
        }

        public string Outcome
        {
            get
            {
                return
                    _gameState.BlackWon ? "Black Won" :
                    _gameState.WhiteWon ? "White Won" :
                    _gameState.Draw ? "Draw" :
                    _gameState.Resigned ? "Resigned" :
                    string.Empty;
            }
        }

        public IEnumerable<IRowViewModel> Rows
        {
            get
            {
                for (int row = 0; row < Square.NumberOfRows; row++)
                    yield return new LocalRowViewModel(_gameState, row);
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

        public void ClearSelectedPlayer()
        {
            _mainNavigation.SelectedPlayer = null;
        }

        public void AcknowledgeOutcome()
        {
            foreach (LocalOutcome outcome in _gameState.Game.Outcomes)
            {
                outcome.Acknowledge();
            }
        }

        public bool IsChatEnabled
        {
            get { return false; }
        }

        public void EnableChat()
        {
        }
    }
}
