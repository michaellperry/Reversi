using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;
using UpdateControls.XAML;
using UpdateControls;

namespace FacetedWorlds.Reversi.ViewModels
{
    public abstract class PlayerGameViewModel : IGameViewModel
    {
        private MainNavigationModel _mainNavigation;
        private RemoteGameState _gameState;

        private Dependent _depGameState;

        protected PlayerGameViewModel(MainNavigationModel mainNavigation)
        {
            _mainNavigation = mainNavigation;

            _depGameState = new Dependent(() =>
            {
                _gameState = Player == null
                    ? null
                    : new RemoteGameState(Player, _mainNavigation);
            });
        }

        protected abstract Player Player { get; }

        protected MainNavigationModel MainNavigation
        {
            get { return _mainNavigation; }
        }

        private RemoteGameState GameState
        {
            get
            {
                _depGameState.OnGet();
                return _gameState;
            }
        }

        public string Name
        {
            get
            {
                return
                    MyColor == PieceColor.Empty
                        ? "Waiting for opponent" :
                    MyColor == PieceColor.Black
                        ? string.Format("you vs. {0}", OtherPlayerName)
                        : String.Format("{0} vs. you", OtherPlayerName);
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

        public bool CanChat
        {
            get { return Player != null; }
        }

        public bool CanResign
        {
            get { return Player != null && Player.Game.Outcome == null; }
        }

        public ICommand Resign
        {
            get
            {
                return MakeCommand
                    .When(() => Player != null)
                    .Do(() => Player.Game.DeclareWinner(GetOtherPlayer(), true));
            }
        }

        public PieceColor MyColor
        {
            get { return GameState == null ? PieceColor.Empty : GameState.MyColor; }
        }

        public PieceColor OpponentColor
        {
            get { return GameState == null ? PieceColor.Empty : GameState.MyColor.Opposite(); }
        }

        public bool MyTurn
        {
            get { return GameState != null && GameState.MyTurn && !GameState.IsMovePending; }
        }

        public string Outcome
        {
            get
            {
                return
                    GameState == null ? "Waiting for opponent..." :
                    GameState.IWon ? "You Won" :
                    GameState.ILost ? "You Lost" :
                    GameState.IDrew ? "You Drew" :
                    GameState.IResigned ? "You Resigned" :
                    GameState.HeResigned ? "Opponent Resigned" :
                        string.Empty;
            }
        }

        public bool ILost
        {
            get { return GameState != null && GameState.ILost; }
        }

        public bool IDrew
        {
            get { return GameState != null && GameState.IDrew; }
        }

        public IEnumerable<IRowViewModel> Rows
        {
            get
            {
                if (GameState != null)
                    for (int row = 0; row < Square.NumberOfRows; row++)
                        yield return new RemoteRowViewModel(GameState, row);
            }
        }

        public bool IsMovePending
        {
            get { return GameState != null && GameState.IsMovePending; }
        }

        public bool PreviewMove(int row, int column)
        {
            return GameState != null && GameState.SetPreviewMove(Square.FromCoordinates(row, column));
        }

        public void ClearPreviewMove()
        {
            if (GameState != null)
                GameState.SetPreviewMove(null);
        }

        public void MakeMove(int row, int column)
        {
            if (GameState != null)
                GameState.MakeMove(Square.FromCoordinates(row, column));
        }

        public void CommitMove()
        {
            if (GameState != null)
                GameState.CommitMove();
        }

        public void CancelMove()
        {
            if (GameState != null)
                GameState.CancelMove();
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
            if (Player == null)
                return null;
            List<Player> players = Player.Game.Players.ToList();
            return players.FirstOrDefault(p => p != Player);
        }

        public abstract void ClearSelectedPlayer();
        public abstract bool IsWaiting { get; }

        public void AcknowledgeOutcome()
        {
            if (Player != null)
            {
                IEnumerable<Outcome> outcomes = Player.Game.Outcomes;
                foreach (Outcome outcome in outcomes)
                {
                    Player.Acknowledge(outcome);
                }
            }
        }

        public bool IsChatEnabled
        {
            get { return Player != null && Player.User.IsChatEnabled.Any(); }
        }

        public void EnableChat()
        {
            if (Player != null)
                Player.User.EnableChat();
        }
    }
}
