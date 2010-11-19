using System.Linq;

namespace FacetedWorlds.Reversi.Model
{
    public partial class LocalGame
    {
        public LocalOutcome Outcome
        {
            get { return Outcomes.FirstOrDefault(); }
        }

        public void DeclareWinner(LocalPlayer winner, bool resigned)
        {
            Community.AddFact(new LocalOutcome(this, winner, resigned ? 1 : 0));
        }
    }
}
