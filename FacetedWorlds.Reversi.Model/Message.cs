
namespace FacetedWorlds.Reversi.Model
{
    public partial class Message
    {
        public void AcknowledgeMessage()
        {
            Community.AddFact(new Acknowledge(this));
        }
    }
}
