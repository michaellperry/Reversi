using System.Collections.Generic;
using FacetedWorlds.Reversi.Model;

namespace FacetedWorlds.Reversi.ViewModel
{
    public class MoveComparer : IComparer<Move>
    {
        public int Compare(Move x, Move y)
        {
            if (x.Index < y.Index)
                return -1;
            else if (x.Index > y.Index)
                return 1;
            else
                return 0;
        }
    }
}
