using System;
using FacetedWorlds.Reversi.GameLogic;

namespace FacetedWorlds.Reversi.ViewModel
{
    public class SquareViewModel
    {
        private GameState _gameState;
        private Square _square;

        public SquareViewModel(GameState gameState, Square square)
        {
            _gameState = gameState;
            _square = square;
        }

        public PieceColor Color
        {
            get { return _gameState.PieceAt(_square); }
        }

        public void OnClick()
        {
            _gameState.MakeMove(_square);
        }
    }
}
