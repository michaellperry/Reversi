using System.Collections.Generic;
using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class LocalRowViewModel : ViewModelBase, IRowViewModel
    {
        private LocalGameState _gameState;
        private int _row;
        private static int _instanceCount = 0;

        public LocalRowViewModel(LocalGameState gameState, int row)
        {
            System.Diagnostics.Debug.WriteLine(this.GetType().Name + " " + ++_instanceCount);
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
                yield return new LocalSquareViewModel(_gameState, Square.FromCoordinates(_row, column));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            LocalRowViewModel that = obj as LocalRowViewModel;
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
