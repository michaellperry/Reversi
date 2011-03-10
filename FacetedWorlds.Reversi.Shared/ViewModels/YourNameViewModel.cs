using System.Collections.Generic;
using System.Linq;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class YourNameViewModel
    {
        private Identity _identity;
        private NameNavigationModel _nameNavigation;

        public YourNameViewModel(Identity identity, NameNavigationModel nameNavigation)
        {
            _identity = identity;
            _nameNavigation = nameNavigation;
        }

        public string PendingName
        {
            get
            {
                return _identity.Claims
                    .Where(claim => !claim.Responses.Any())
                    .Select(claim => claim.User.UserName)
                    .FirstOrDefault();
            }
        }

        public string YourName
        {
            get { return _nameNavigation.Name; }
            set { _nameNavigation.Name = value; }
        }

        public bool IsNameValid
        {
            get { return User.IsNameValid(_nameNavigation.Name); }
        }

        public void ClaimName()
        {
            _identity.ClaimUserName(_nameNavigation.Name);
        }

        public bool HasRejectedNames
        {
            get { return RejectedNames.Any(); }
        }

        public IEnumerable<string> RejectedNames
        {
            get
            {
                return
                    from claim in _identity.Claims
                    where claim.Responses.Any(response => response.Approved == 0)
                    select claim.User.UserName;
            }
        }
    }
}
