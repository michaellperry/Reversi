using System.Collections.Generic;
using FacetedWorlds.Reversi.Model;

namespace FacetedWorlds.Reversi.Client.NavigationModels
{
    public class LocalMoveComparer : IComparer<LocalMove>
    {
        public int Compare(LocalMove x, LocalMove y)
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
