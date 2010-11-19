
namespace FacetedWorlds.Reversi.Model
{
    public partial class LocalOutcome
    {
        public void Acknowledge()
        {
            Community.AddFact(new LocalOutcomeAcknowledge(this));
        }
    }
}
