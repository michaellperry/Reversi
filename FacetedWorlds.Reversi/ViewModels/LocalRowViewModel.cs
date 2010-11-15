using System.Collections.Generic;
using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class LocalRowViewModel : IRowViewModel
    {
        private LocalGameState _gameState;
        private int _row;

        public LocalRowViewModel(LocalGameState gameState, int row)
        {
            _gameState = gameState;
            _row = row;
        }

        public IEnumerable<ISquareViewModel> Squares
        {
            get
            {
                for (int column = 0; column < Square.NumberOfColumns; column++)
                {
                    yield return new LocalSquareViewModel(_gameState, new Square(_row, column));
                }
            }
        }
    }
}
