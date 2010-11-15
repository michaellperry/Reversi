
namespace FacetedWorlds.Reversi.Model
{
    public partial class LocalPlayer
    {
        public void MakeMove(int index, int square)
        {
            Community.AddFact(new LocalMove(this, index, square));
        }
    }
}
