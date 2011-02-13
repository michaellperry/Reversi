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
    public class LocalGameViewModel : ViewModelBase, IGameViewModel
    {
        private LocalGameState _gameState;
        private MainNavigationModel _mainNavigation;
        private static int _instanceCount = 0;

        public LocalGameViewModel(LocalGame localGame, MainNavigationModel mainNavigation)
        {
            System.Diagnostics.Debug.WriteLine(this.GetType().Name + " " + ++_instanceCount);
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

        public bool CanResign
        {
            get { return Get(() => _gameState.Game.Outcome == null); }
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
            get { return Get(() => _gameState.MyColor); }
        }

        public PieceColor OpponentColor
        {
            get { return Get(() => _gameState.MyColor.Opposite()); }
        }

        public bool MyTurn
        {
            get { return Get(() => !_gameState.IsMovePending && _gameState.Game.Outcome == null); }
        }

        public string Outcome
        {
            get
            {
                return Get(() =>
                    _gameState.BlackWon ? "Black Won" :
                    _gameState.WhiteWon ? "White Won" :
                    _gameState.Draw ? "Draw" :
                    _gameState.Resigned ? "Resigned" :
                    string.Empty);
            }
        }

        public IEnumerable<IRowViewModel> Rows
        {
            get { return Get(() => GetRows()); }
        }

        private IEnumerable<IRowViewModel> GetRows()
        {
            for (int row = 0; row < Square.NumberOfRows; row++)
                yield return new LocalRowViewModel(_gameState, row);
        }

        public bool IsMovePending
        {
            get { return Get(() => _gameState.IsMovePending); }
        }

        public bool PreviewMove(int row, int column)
        {
            return _gameState.SetPreviewMove(Square.FromCoordinates(row, column));
        }

        public void ClearPreviewMove()
        {
            _gameState.SetPreviewMove(null);
        }

        public void MakeMove(int row, int column)
        {
            _gameState.MakeMove(Square.FromCoordinates(row, column));
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

        public bool IsWaiting
        {
            get { return false; }
        }

        public int BlackCount
        {
            get { return Get(() => _gameState.BlackCount); }
        }

        public int WhiteCount
        {
            get { return Get(() => _gameState.WhiteCount); }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            LocalGameViewModel that = obj as LocalGameViewModel;
            if (that == null)
                return false;
            return this._gameState.Equals(that._gameState);
        }

        public override int GetHashCode()
        {
            return _gameState.GetHashCode();
        }
    }
}
