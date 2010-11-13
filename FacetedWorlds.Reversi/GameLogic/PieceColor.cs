using System;

namespace FacetedWorlds.Reversi.GameLogic
{
    public enum PieceColor
    {
        Empty,
        White,
        Black
    }

    public static class PieceColorExtensions
    {
        public static PieceColor Opposite(this PieceColor pieceColor)
        {
            return
                pieceColor == PieceColor.White ? PieceColor.Black :
                pieceColor == PieceColor.Black ? PieceColor.White :
                    PieceColor.Empty;
        }
    }
}
