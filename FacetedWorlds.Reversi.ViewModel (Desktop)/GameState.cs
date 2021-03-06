using System;
using System.Collections.Generic;
using System.Linq;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.GameLogic;
using UpdateControls;

namespace FacetedWorlds.Reversi.ViewModel
{
    public class GameState
    {
        private Player _player;
        private PieceColor _myColor;

        private GameBoard _gameBoard;
        private Dependent _depGameBoard;

        public GameState(Player player)
        {
            _player = player;
            if (_player != null)
            {
                _myColor = _player.Index == 0 ? PieceColor.Black : PieceColor.White;
            }

            _depGameBoard = new Dependent(UpdateGameBoard);
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
                return _gameBoard != null && _gameBoard.ToMove == _myColor;
            }
        }

        public bool IWon
        {
            get
            {
                return
                    _player != null &&
                    _player.Game.Outcome != null &&
                    _player.Game.Outcome.Winner == _player;
            }
        }

        public bool ILost
        {
            get
            {
                if (_player != null && _player.Game.Outcome != null)
                {
                    Player winner = _player.Game.Outcome.Winner;
                    return winner != null && winner != _player;
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

        public void MakeMove(Square square)
        {
            _depGameBoard.OnGet();
            if (MyTurn && _gameBoard.LegalMoves.Contains(square))
            {
                _player.MakeMove(_gameBoard.MoveIndex, square.Index);

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

        private void UpdateGameBoard()
        {
            _gameBoard = null;
            if (_player != null)
            {
                List<Move> moves = _player.Game.Moves.ToList();
                moves.Sort(new MoveComparer());
                int expectedIndex = 0;
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

                    _gameBoard = _gameBoard.AfterMove(square);
                    ++expectedIndex;
                }
            }
        }
    }
}
