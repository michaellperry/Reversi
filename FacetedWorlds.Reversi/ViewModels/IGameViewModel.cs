using System.Collections.Generic;
using System.Windows.Input;
using FacetedWorlds.Reversi.GameLogic;
using System;

namespace FacetedWorlds.Reversi.ViewModels
{
    public interface IGameViewModel
    {
        string Name { get; }
        bool HasNewMessages { get; }
        bool CanChat { get; }
        bool CanResign { get; }
        ICommand Resign { get; }
        PieceColor MyColor { get; }
        PieceColor OpponentColor { get; }
        bool MyTurn { get; }
        string Outcome { get; }
        IEnumerable<IRowViewModel> Rows { get; }
        bool IsMovePending { get; }
        bool PreviewMove(int row, int column);
        void ClearPreviewMove();
        void MakeMove(int row, int column);
        void CommitMove();
        void CancelMove();
        void ClearSelectedPlayer();
        void AcknowledgeOutcome();
        bool IsChatEnabled { get; }
        void EnableChat();
        bool IsWaiting { get; }
        int BlackCount { get; }
        int WhiteCount { get; }
    }
}
