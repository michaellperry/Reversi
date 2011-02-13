using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class RemoteSquareViewModel : ViewModelBase, ISquareViewModel
    {
        private RemoteGameState _gameState;
        private Square _square;

        public RemoteSquareViewModel(RemoteGameState gameState, Square square)
        {
            _gameState = gameState;
            _square = square;
        }

        public PieceColor Color
        {
            get { return Get(() => _gameState.PieceAt(_square)); }
        }

        public PieceColor OpponentColor
        {
            get { return Get(() => _gameState.ToMove); }
        }

        public bool IsPreviewCapture
        {
            get
            {
                return Get(() =>
                    _gameState.IsPreviewCapture(_square) ||
                    _gameState.IsPriorCapture(_square) ||
                    _gameState.IsPriorMove(_square));
            }
        }

        public bool IsPreviewCede
        {
            get { return Get(() => _gameState.IsPreviewCede(_square)); }
        }

        public void MakeMove()
        {
            _gameState.MakeMove(_square);
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            RemoteSquareViewModel that = obj as RemoteSquareViewModel;
            if (that == null)
                return false;
            return this._gameState.Equals(that._gameState) && this._square.Equals(that._square);
        }

        public override int GetHashCode()
        {
            return _gameState.GetHashCode() * Square.NumberOfRows * Square.NumberOfColumns + _square.GetHashCode();
        }
    }
}
