using System;
using System.Collections.Generic;
using System.Linq;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.GameLogic;
using UpdateControls;
using FacetedWorlds.Reversi.NavigationModels;

namespace FacetedWorlds.Reversi.Client.NavigationModels
{
    public class LocalGameState
    {
        private LocalGame _game;
        private MainNavigationModel _mainNavigation;

        private GameBoard _priorBoardRaw;
        private GameBoard _gameBoardRaw;
        private Dependent _depGameBoardRaw;

        private GameBoard _priorBoard;
        private GameBoard _gameBoard;
        private Dependent _depGameBoard;

        private GameBoard _previewBoard;
        private Dependent _depPreviewBoard;

        private static int _updateCount = 0;

        public LocalGameState(LocalGame game, MainNavigationModel mainNavigation)
        {
            _game = game;
            _mainNavigation = mainNavigation;

            _depGameBoardRaw = new Dependent(UpdateGameBoardRaw);
            _depGameBoard = new Dependent(UpdateGameBoard);
            _depPreviewBoard = new Dependent(UpdatePreviewBoard);
        }

        public LocalGame Game
        {
            get { return _game; }
        }

        public PieceColor MyColor
        {
            get
            {
                GameBoard gameBoard = GetGameBoard();
                return gameBoard != null ? gameBoard.ToMove : PieceColor.Empty;
            }
        }

        public PieceColor ToMove
        {
            get
            {
                GameBoard previewBoard = GetPreviewBoard();
                return previewBoard.ToMove;
            }
        }

        public bool Resigned
        {
            get
            {
                return
                    _game != null &&
                    _game.Outcome != null &&
                    _game.Outcome.Winner != null &&
                    _game.Outcome.Resigned == 1;
            }
        }

        public bool BlackWon
        {
            get
            {
                return
                    _game != null &&
                    _game.Outcome != null &&
                    _game.Outcome.Winner != null &&
                    _game.Outcome.Resigned != 1 &&
                    _game.Outcome.Winner.Index == 0;
            }
        }

        public bool WhiteWon
        {
            get
            {
                return
                    _game != null &&
                    _game.Outcome != null &&
                    _game.Outcome.Winner != null &&
                    _game.Outcome.Resigned != 1 &&
                    _game.Outcome.Winner.Index == 1;
            }
        }

        public bool Draw
        {
            get
            {
                return
                    _game != null &&
                    _game.Outcome != null &&
                    _game.Outcome.Winner == null;
            }
        }

        public PieceColor PieceAt(Square square)
        {
            GameBoard gameBoard = GetGameBoard();
            return gameBoard == null ? PieceColor.Empty : gameBoard.PieceAt(square);
        }

        public bool IsPriorMove(Square square)
        {
            GameBoard gameBoard = GetGameBoard();
            GameBoard priorBoard = GetPriorBoard();
            return
                _mainNavigation.PreviewMove == null &&
                priorBoard.PieceAt(square) == PieceColor.Empty &&
                gameBoard.PieceAt(square) != PieceColor.Empty;
        }

        public bool IsPriorCapture(Square square)
        {
            GameBoard gameBoard = GetGameBoard();
            GameBoard priorBoard = GetPriorBoard();
            return
                _mainNavigation.PreviewMove == null &&
                priorBoard.PieceAt(square) != PieceColor.Empty &&
                gameBoard.PieceAt(square) != priorBoard.PieceAt(square);
        }

        public bool IsPreviewCapture(Square square)
        {
            GameBoard gameBoard = GetGameBoard();
            GameBoard previewBoard = GetPreviewBoard();
            return
                _mainNavigation.PreviewMove != square &&
                gameBoard.PieceAt(square) != previewBoard.PieceAt(square);
        }

        public bool IsPreviewCede(Square square)
        {
            GameBoard previewBoard = GetPreviewBoard();
            return
                previewBoard.LegalMoves.Contains(square);
        }

        public int BlackCount
        {
            get
            {
                GameBoard gameBoard = GetPreviewBoard();
                return gameBoard.BlackCount;
            }
        }

        public int WhiteCount
        {
            get
            {
                GameBoard gameBoard = GetPreviewBoard();
                return gameBoard.WhiteCount;
            }
        }

        public bool SetPreviewMove(Square square)
        {
            GameBoard gameBoard = GetGameBoard();
            if (square != null && !gameBoard.LegalMoves.Contains(square))
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
            GameBoard gameBoard = GetGameBoard();
            if (_mainNavigation.PendingMove == null && gameBoard.LegalMoves.Contains(square))
            {
                _mainNavigation.PendingMoveIndex = gameBoard.MoveIndex;
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
                GameBoard priorBoard = GetPriorBoard();
                int playerIndexToMove = priorBoard.ToMove == PieceColor.Black ? 0 : 1;
                LocalPlayer playerToMove = _game.Players.FirstOrDefault(player => player.Index == playerIndexToMove);
                if (playerToMove != null)
                {
                    playerToMove.MakeMove(_mainNavigation.PendingMoveIndex, _mainNavigation.PendingMove.Index);
                    _mainNavigation.PendingMove = null;

                    GameBoard gameBoard = GetGameBoard();
                    if (gameBoard.ToMove == PieceColor.Empty)
                    {
                        int blackCount = gameBoard.BlackCount;
                        int whiteCount = gameBoard.WhiteCount;
                        if (blackCount > whiteCount)
                            _game.DeclareWinner(_game.Players.Single(p => p.Index == 0), false);
                        else if (blackCount < whiteCount)
                            _game.DeclareWinner(_game.Players.Single(p => p.Index == 1), false);
                        else
                            _game.DeclareWinner(null, false);
                    }
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
            if (_game != null)
            {
                List<LocalMove> moves = _game.Moves.ToList();
                moves.Sort(new LocalMoveComparer());
                int expectedIndex = 0;
                foreach (LocalMove move in moves)
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
            System.Diagnostics.Debug.WriteLine("UpdatePreviewBoard " + ++_updateCount);
            GameBoard gameBoard = GetGameBoard();
            if (_mainNavigation.PreviewMove != null && gameBoard.LegalMoves.Contains(_mainNavigation.PreviewMove))
                _previewBoard = gameBoard.AfterMove(_mainNavigation.PreviewMove);
            else
                _previewBoard = gameBoard;
        }

        private GameBoard GetGameBoard()
        {
            _depGameBoard.OnGet();
            return _gameBoard;
        }

        private GameBoard GetPriorBoard()
        {
            _depGameBoard.OnGet();
            return _priorBoard;
        }

        private GameBoard GetPreviewBoard()
        {
            _depPreviewBoard.OnGet();
            return _previewBoard;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            LocalGameState that = obj as LocalGameState;
            if (that == null)
                return false;
            return this._game.Equals(that._game);
        }

        public override int GetHashCode()
        {
            return _game.GetHashCode();
        }
    }
}
