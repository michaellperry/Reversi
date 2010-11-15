using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class RemoteSquareViewModel : ISquareViewModel
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
            get { return _gameState.PieceAt(_square); }
        }

        public PieceColor OpponentColor
        {
            get { return _gameState.MyColor.Opposite(); }
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
    }
}
