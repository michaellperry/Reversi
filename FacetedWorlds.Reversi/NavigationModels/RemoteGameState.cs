using System;
using System.Collections.Generic;
using System.Linq;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.GameLogic;
using UpdateControls;
using FacetedWorlds.Reversi.NavigationModels;

namespace FacetedWorlds.Reversi.Client.NavigationModels
{
    public class RemoteGameState
    {
        private Player _player;
        private MainNavigationModel _mainNavigation;
        private PieceColor _myColor;

        private GameBoard _priorBoard;
        private GameBoard _gameBoard;
        private Dependent _depGameBoard;

        private GameBoard _previewBoard;
        private Dependent _depPreviewBoard;

        public RemoteGameState(Player player, MainNavigationModel mainNavigation)
        {
            _player = player;
            _mainNavigation = mainNavigation;
            if (_player != null)
            {
                _myColor = _player.Index == 0 ? PieceColor.Black : PieceColor.White;
            }

            _depGameBoard = new Dependent(UpdateGameBoard);
            _depPreviewBoard = new Dependent(UpdatePreviewBoard);
        }

        public PieceColor MyColor
        {
            get
            {
                bool gameIsOngoing = _player != null && _player.Game.Outcome == null;
                return gameIsOngoing ? _myColor : PieceColor.Empty;
            }
        }

        public bool MyTurn
        {
            get
            {
                _depGameBoard.OnGet();
                return _gameBoard != null && _gameBoard.ToMove == _myColor && _player != null && _player.Game.Outcome == null;
            }
        }

        public bool IWon
        {
            get
            {
                return
                    _player != null &&
                    _player.Game.Outcome != null &&
                    _player.Game.Outcome.Winner == _player &&
                    _player.Game.Outcome.Resigned != 1;
            }
        }

        public bool HeResigned
        {
            get
            {
                return
                    _player != null &&
                    _player.Game.Outcome != null &&
                    _player.Game.Outcome.Winner == _player &&
                    _player.Game.Outcome.Resigned == 1;
            }
        }

        public bool ILost
        {
            get
            {
                if (_player != null && _player.Game.Outcome != null)
                {
                    Player winner = _player.Game.Outcome.Winner;
                    return winner != null && winner != _player && _player.Game.Outcome.Resigned != 1;
                }
                else
                    return false;
            }
        }

        public bool IResigned
        {
            get
            {
                if (_player != null && _player.Game.Outcome != null)
                {
                    Player winner = _player.Game.Outcome.Winner;
                    return winner != null && winner != _player && _player.Game.Outcome.Resigned == 1;
                }
                else
                    return false;
            }
        }

        public bool IDrew
        {
            get
            {
                return
                    _player != null &&
                    _player.Game.Outcome != null &&
                    _player.Game.Outcome.Winner == null;
            }
        }

        public PieceColor PieceAt(Square square)
        {
            _depGameBoard.OnGet();
            return _gameBoard == null ? PieceColor.Empty : _gameBoard.PieceAt(square);
        }

        public bool IsPriorMove(Square square)
        {
            _depGameBoard.OnGet();
            return
                _mainNavigation.PreviewMove == null &&
                _priorBoard.PieceAt(square) == PieceColor.Empty &&
                _gameBoard.PieceAt(square) != PieceColor.Empty;
        }

        public bool IsPriorCapture(Square square)
        {
            _depGameBoard.OnGet();
            return
                _mainNavigation.PreviewMove == null &&
                _priorBoard.PieceAt(square) != PieceColor.Empty &&
                _gameBoard.PieceAt(square) != _priorBoard.PieceAt(square);
        }

        public bool IsPreviewCapture(Square square)
        {
            _depPreviewBoard.OnGet();
            return
                _mainNavigation.PreviewMove != square &&
                _gameBoard.PieceAt(square) != _previewBoard.PieceAt(square);
        }

        public bool IsPreviewCede(Square square)
        {
            _depPreviewBoard.OnGet();
            return
                _mainNavigation.PreviewMove != null &&
                _previewBoard.ToMove != MyColor &&
                _previewBoard.LegalMoves.Contains(square);
        }

        public int BlackCount
        {
            get
            {
                _depGameBoard.OnGet();
                return _gameBoard.BlackCount;
            }
        }

        public int WhiteCount
        {
            get
            {
                _depGameBoard.OnGet();
                return _gameBoard.WhiteCount;
            }
        }

        public void SetPreviewMove(Square square)
        {
            if (_mainNavigation.PreviewMove != square)
                _mainNavigation.PreviewMove = square;
        }

        public void MakeMove(Square square)
        {
            _mainNavigation.PreviewMove = null;
            _depGameBoard.OnGet();
            if (MyTurn && _gameBoard.LegalMoves.Contains(square))
            {
                _mainNavigation.PendingMoveIndex = _gameBoard.MoveIndex;
                _mainNavigation.PendingMove = square;
            }
        }

        public bool IsMovePending
        {
            get { return _mainNavigation.PendingMove != null; }
        }

        public void CommitMove()
        {
            if (_mainNavigation.PendingMove != null)
            {
                _player.MakeMove(_mainNavigation.PendingMoveIndex, _mainNavigation.PendingMove.Index);
                _mainNavigation.PendingMove = null;

                _depGameBoard.OnGet();
                if (_gameBoard.ToMove == PieceColor.Empty)
                {
                    int blackCount = _gameBoard.BlackCount;
                    int whiteCount = _gameBoard.WhiteCount;
                    if (blackCount > whiteCount)
                        _player.Game.DeclareWinner(_player.Game.Players.Single(p => p.Index == 0), false);
                    else if (blackCount < whiteCount)
                        _player.Game.DeclareWinner(_player.Game.Players.Single(p => p.Index == 1), false);
                    else
                        _player.Game.DeclareWinner(null, false);
                }
            }
        }

        public void CancelMove()
        {
            if (_mainNavigation.PendingMove != null)
            {
                _mainNavigation.PendingMove = null;
            }
        }

        private void UpdateGameBoard()
        {
            _priorBoard = null;
            _gameBoard = null;
            if (_player != null)
            {
                Game game = _player.Game;
                IEnumerable<Move> gameMoves = game.Moves;
                List<Move> moves = gameMoves.ToList();
                moves.Sort(new RemoteMoveComparer());
                int expectedIndex = 0;
                _priorBoard = GameBoard.OpeningPosition;
                _gameBoard = GameBoard.OpeningPosition;
                foreach (Move move in moves)
                {
                    if (move.Index != expectedIndex)
                        return;
                    if (move.Player.Index == 0 && _gameBoard.ToMove != PieceColor.Black)
                        return;
                    if (move.Player.Index == 1 && _gameBoard.ToMove != PieceColor.White)
                        return;

                    Square square = Square.FromIndex(move.Square);
                    if (!_gameBoard.LegalMoves.Contains(square))
                        return;

                    _priorBoard = _gameBoard;
                    _gameBoard = _priorBoard.AfterMove(square);
                    ++expectedIndex;
                }

                if (_mainNavigation.PendingMove != null && _gameBoard.LegalMoves.Contains(_mainNavigation.PendingMove))
                {
                    _priorBoard = _gameBoard;
                    _gameBoard = _priorBoard.AfterMove(_mainNavigation.PendingMove);
                }
            }
        }

        private void UpdatePreviewBoard()
        {
            _depGameBoard.OnGet();
            if (_mainNavigation.PreviewMove != null && _gameBoard.LegalMoves.Contains(_mainNavigation.PreviewMove))
                _previewBoard = _gameBoard.AfterMove(_mainNavigation.PreviewMove);
            else
                _previewBoard = _gameBoard;
        }
    }
}
