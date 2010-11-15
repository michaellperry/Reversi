using System.Collections.Generic;
using System.Windows.Input;
using FacetedWorlds.Reversi.GameLogic;

namespace FacetedWorlds.Reversi.ViewModels
{
    public interface IGameViewModel
    {
        string Name { get; }
        bool HasNewMessages { get; }
        bool CanChat { get; }
        ICommand Resign { get; }
        PieceColor MyColor { get; }
        PieceColor OpponentColor { get; }
        bool MyTurn { get; }
        bool IWon { get; }
        bool ILost { get; }
        bool IDrew { get; }
        IEnumerable<IRowViewModel> Rows { get; }
        bool IsMovePending { get; }
        void PreviewMove(int row, int column);
        void ClearPreviewMove();
        void MakeMove(int row, int column);
        void CommitMove();
        void CancelMove();
        void ClearSelectedPlayer();
    }
}
