using System.Linq;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;
using UpdateControls;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class NewGameViewModel
    {
        private enum StateId
        {
            YourName,
            PendingUserName,
            Challenge
        }

        private Identity _identity;
        private MainNavigationModel _mainNavigation;
        private NameNavigationModel _nameNavigation;

        private StateId _state;
        private ChallengeViewModel _challenge;
        private YourNameViewModel _yourName;
        private PendingUserNameViewModel _pendingUserName;

        private Dependent _depState;
        private Dependent _depChallenge;
        private Dependent _depYourName;
        private Dependent _depPendingUserName;

        public NewGameViewModel(Identity identity, MainNavigationModel mainNavigation, NameNavigationModel nameNavigation)
        {
            _identity = identity;
            _mainNavigation = mainNavigation;
            _nameNavigation = nameNavigation;

            _depState = new Dependent(delegate
            {
                _state =
                    _identity.User != null ? StateId.Challenge :
                    _identity.Claims.Any(claim => !claim.Responses.Any()) ? StateId.PendingUserName :
                    StateId.YourName;
            });
            _depChallenge = new Dependent(delegate
            {
                _depState.OnGet();
                _challenge = _state == StateId.Challenge
                    ? new ChallengeViewModel(_identity.User, _mainNavigation)
                    : null;
            });
            _depYourName = new Dependent(delegate
            {
                _depState.OnGet();
                _yourName = _state == StateId.YourName
                    ? new YourNameViewModel(_identity, _nameNavigation)
                    : null;
            });
            _depPendingUserName = new Dependent(delegate
            {
                _depState.OnGet();
                _pendingUserName = _state == StateId.PendingUserName
                    ? new PendingUserNameViewModel(_identity.Claims.FirstOrDefault(claim => !claim.Responses.Any()))
                    : null;
            });
        }

        public string Title
        {
            get
            {
                _depState.OnGet();
                return
                    _state == StateId.YourName ? "your name" :
                    _state == StateId.PendingUserName ? "checking" :
                    "new game";
            }
        }

        public ChallengeViewModel Challenge
        {
            get { _depChallenge.OnGet(); return _challenge; }
        }

        public YourNameViewModel YourName
        {
            get { _depYourName.OnGet(); return _yourName; }
        }

        public PendingUserNameViewModel PendingUserName
        {
            get { _depPendingUserName.OnGet(); return _pendingUserName; }
        }
    }
}
