
namespace FacetedWorlds.Reversi.Model
{
	public partial class Player
	{
		public void MakeMove(int index, int square)
		{
			Community.AddFact(new Move(this, index, square));
		}

        public void SendMessage(string messageBody)
        {
            Community.AddFact(new Message(this, messageBody));
        }
	}
}
