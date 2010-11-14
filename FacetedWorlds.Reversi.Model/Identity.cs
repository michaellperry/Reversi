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
            Community.AddFact(new LocalGame(this));
        }
    }
}
