using FacetedWorlds.Reversi.Model;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class PendingUserNameViewModel
    {
        private Claim _claim;

        public PendingUserNameViewModel(Claim claim)
        {
            _claim = claim;
        }

        public string PendingName
        {
            get { return _claim.User.UserName; }
        }
    }
}
