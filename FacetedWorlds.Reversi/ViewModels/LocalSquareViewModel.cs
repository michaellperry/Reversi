using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class LocalSquareViewModel : ISquareViewModel
    {
        private LocalGameState _gameState;
        private Square _square;

        public LocalSquareViewModel(LocalGameState gameState, Square square)
        {
            _gameState = gameState;
            _square = square;
        }

        public PieceColor Color
        {
            get { return _gameState.PieceAt(_square); }
        }

        public PieceColor OpponentColor
        {
            get { return _gameState.ToMove; }
        }

        public bool IsPreviewCapture
        {
            get
            {
                return
                    _gameState.IsPreviewCapture(_square) ||
                    _gameState.IsPriorCapture(_square) ||
                    _gameState.IsPriorMove(_square);
            }
        }

        public bool IsPreviewCede
        {
            get { return _gameState.IsPreviewCede(_square); }
        }

        public void MakeMove()
        {
            _gameState.MakeMove(_square);
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            LocalSquareViewModel that = obj as LocalSquareViewModel;
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
