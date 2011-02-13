using System.Linq;
using System.Collections.Generic;

namespace FacetedWorlds.Reversi.Model
{
    public partial class Identity
    {
        public Claim ClaimUserName(string userName)
        {
            return Community.AddFact(new Claim(
                this,
                Community.AddFact(new User(userName.ToLowerInvariant())),
                Community.AddFact(new IdentityService())));
        }

        public IEnumerable<User> ApprovedUsers
        {
            get
            {
                return Claims
                    .Where(claim => claim.Responses.Any(response => response.Approved == 1))
                    .Select(claim => claim.User);
            }
        }

        public User User
        {
            get { return ApprovedUsers.FirstOrDefault(); }
        }

        public void CreateLocalGame()
        {
            LocalGame game = Community.AddFact(new LocalGame(this));
            Community.AddFact(new LocalPlayer(game, 0));
            Community.AddFact(new LocalPlayer(game, 1));
        }

        public void EnableToastNotification(bool enabled)
        {
            if (enabled)
            {
                foreach (DisableToastNotification disable in IsToastNotificationDisabled)
                    Community.AddFact(new EnableToastNotification(disable));
            }
            else
            {
                if (!IsToastNotificationDisabled.Any())
                    Community.AddFact(new DisableToastNotification(this));
            }
        }
    }
}
