using System.Collections.Generic;
using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class RowViewModel
    {
        private GameState _gameState;
        private int _row;

        public RowViewModel(GameState gameState, int row)
        {
            _gameState = gameState;
            _row = row;
        }

        public IEnumerable<SquareViewModel> Squares
        {
            get
            {
                for (int column = 0; column < Square.NumberOfColumns; column++)
                {
                    yield return new SquareViewModel(_gameState, new Square(_row, column));
                }
            }
        }
    }
}
