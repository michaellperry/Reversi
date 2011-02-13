using System.Collections.Generic;
using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class RemoteRowViewModel : ViewModelBase, IRowViewModel
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
            get { return Get(() => GetSquares()); }
        }

        private IEnumerable<ISquareViewModel> GetSquares()
        {
            for (int column = 0; column < Square.NumberOfColumns; column++)
            {
                yield return new RemoteSquareViewModel(_gameState, Square.FromCoordinates(_row, column));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            RemoteRowViewModel that = obj as RemoteRowViewModel;
            if (that == null)
                return false;
            return this._gameState.Equals(that._gameState) && this._row == that._row;
        }

        public override int GetHashCode()
        {
            return _gameState.GetHashCode() * Square.NumberOfRows + _row;
        }
    }
}
