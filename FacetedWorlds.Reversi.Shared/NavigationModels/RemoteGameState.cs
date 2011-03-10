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

        private GameBoard _priorBoardRaw;
        private GameBoard _gameBoardRaw;
        private Dependent _depGameBoardRaw;

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

            _depGameBoardRaw = new Dependent(UpdateGameBoardRaw);
            _depGameBoard = new Dependent(UpdateGameBoard);
            _depPreviewBoard = new Dependent(UpdatePreviewBoard);
        }

        public PieceColor MyColor
        {
            get { return _myColor; }
        }

        public PieceColor ToMove
        {
            get
            {
                GameBoard previewBoard = PreviewBoard;
                return previewBoard.ToMove;
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
            return
                _mainNavigation.PreviewMove != square &&
                _gameBoard.PieceAt(square) != PreviewBoard.PieceAt(square);
        }

        public bool IsPreviewCede(Square square)
        {
            return PreviewBoard.LegalMoves.Contains(square);
        }

        public int BlackCount
        {
            get
            {
                _depPreviewBoard.OnGet();
                return _previewBoard.BlackCount;
            }
        }

        public int WhiteCount
        {
            get
            {
                _depPreviewBoard.OnGet();
                return _previewBoard.WhiteCount;
            }
        }

        public bool SetPreviewMove(Square square)
        {
            _depGameBoard.OnGet();
            if (square != null && !_gameBoard.LegalMoves.Contains(square))
                square = null;
            if (!Object.Equals(_mainNavigation.PreviewMove, square))
            {
                _mainNavigation.PreviewMove = square;
                return square != null;
            }
            return false;
        }

        public void MakeMove(Square square)
        {
            _mainNavigation.PreviewMove = null;
            _depGameBoard.OnGet();
            if (_mainNavigation.PendingMove == null && MyTurn && _gameBoard.LegalMoves.Contains(square))
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

        private void UpdateGameBoardRaw()
        {
            _priorBoardRaw = GameBoard.OpeningPosition;
            _gameBoardRaw = GameBoard.OpeningPosition;

            List<Move> moves = _player.Game.Moves.ToList();
            moves.Sort(new RemoteMoveComparer());
            int expectedIndex = 0;
            foreach (Move move in moves)
            {
                if (move.Index != expectedIndex)
                    return;
                if (move.Player.Index == 0 && _gameBoardRaw.ToMove != PieceColor.Black)
                    return;
                if (move.Player.Index == 1 && _gameBoardRaw.ToMove != PieceColor.White)
                    return;

                Square square = Square.FromIndex(move.Square);
                if (!_gameBoardRaw.LegalMoves.Contains(square))
                    return;

                _priorBoardRaw = _gameBoardRaw;
                _gameBoardRaw = _priorBoardRaw.AfterMove(square);
                ++expectedIndex;
            }
        }

        private void UpdateGameBoard()
        {
            _depGameBoardRaw.OnGet();
            _priorBoard = _priorBoardRaw;
            _gameBoard = _gameBoardRaw;

            if (_mainNavigation.PendingMove != null && _gameBoard.LegalMoves.Contains(_mainNavigation.PendingMove))
            {
                _priorBoard = _gameBoard;
                _gameBoard = _priorBoard.AfterMove(_mainNavigation.PendingMove);
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

        private GameBoard PreviewBoard
        {
            get
            {
                _depPreviewBoard.OnGet();
                return _previewBoard;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            RemoteGameState that = obj as RemoteGameState;
            if (that == null)
                return false;
            return this._player.Equals(that._player);
        }

        public override int GetHashCode()
        {
            return _player.GetHashCode();
        }
    }
}
