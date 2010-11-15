using System.Collections.Generic;
using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class RemoteRowViewModel : IRowViewModel
    {
        private RemoteGameState _gameState;
        private int _row;

        public RemoteRowViewModel(RemoteGameState gameState, int row)
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
                    yield return new RemoteSquareViewModel(_gameState, new Square(_row, column));
                }
            }
        }
    }
}
