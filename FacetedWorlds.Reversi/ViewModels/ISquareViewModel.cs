using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.GameLogic;

namespace FacetedWorlds.Reversi.ViewModels
{
    public interface ISquareViewModel
    {
        PieceColor Color { get; }
        PieceColor OpponentColor { get; }
        bool IsPreviewCapture { get; }
        bool IsPreviewCede { get; }
    }
}
