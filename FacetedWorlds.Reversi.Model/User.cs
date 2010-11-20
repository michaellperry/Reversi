using System.Linq;

namespace FacetedWorlds.Reversi.Model
{
    public partial class User
    {
        public static bool IsNameValid(string name)
        {
            return
                name != null &&
                name.Length > 3 &&
                !name.Any(character => !IsValidNameCharacter(character));
        }

        private static bool IsValidNameCharacter(char character)
        {
            string validCharacters = @".@_";
            return
                char.IsLetterOrDigit(character) ||
                validCharacters.Contains(character);
        }

        public Player Challenge(string opponentName)
        {
            return Challenge(Community.AddFact(new User(opponentName.ToLowerInvariant())));
        }

        public Player Challenge(User other)
        {
            Game game = Community.AddFact(new Game());
            Player player = Community.AddFact(new Player(this, game, 0));
            Community.AddFact(new Player(other, game, 1));

            return player;
        }

        public bool IsReserved
        {
            get
            {
                return Claims.Any(claim =>
                    claim.Responses.Any(response =>
                        response.Approved == 1));
            }
        }

        public void EnableChat()
        {
            Community.AddFact(new ChatEnable(this));
        }
    }
}
