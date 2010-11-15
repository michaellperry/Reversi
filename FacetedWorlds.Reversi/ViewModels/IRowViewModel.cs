using System.Collections.Generic;

namespace FacetedWorlds.Reversi.ViewModels
{
    public interface IRowViewModel
    {
        IEnumerable<ISquareViewModel> Squares { get; }
    }
}
