using System.Collections.Generic;
using System.Linq;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;
using UpdateControls;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class MainViewModel
    {
        private enum UserNameState
        {
            None,
            Pending,
            Approved
        }

        private INetworkServices _networkServices;
        private Identity _identity;
        private MainNavigationModel _mainNavigation;
        private NameNavigationModel _nameNavigation;

        private UserNameState _state;
        private Dependent _depUserNameState;

        public MainViewModel(INetworkServices networkServices, Identity identity, MainNavigationModel mainNavigation, NameNavigationModel nameNavigation)
        {
            _networkServices = networkServices;
            _identity = identity;
            _mainNavigation = mainNavigation;
            _nameNavigation = nameNavigation;

            _depUserNameState = new Dependent(delegate
            {
                _state =
                    _identity.Claims.Any(claim => !claim.Responses.Any()) ?
                        UserNameState.Pending :
                    _identity.User == null ?
                        UserNameState.None :
                        UserNameState.Approved;
            });
        }

        public string Title
        {
            get
            {
                _depUserNameState.OnGet();
                return
                    _state == UserNameState.None ?
                        "your name" :
                    _state == UserNameState.Pending ?
                        "checking" :
                        "your games";
            }
        }

        public bool ShowYourName
        {
            get { _depUserNameState.OnGet(); return _state == UserNameState.None; }
        }

        public bool ShowPendingApproval
        {
            get { _depUserNameState.OnGet(); return _state == UserNameState.Pending; }
        }

        public bool ShowYourGames
        {
            get { _depUserNameState.OnGet(); return _state == UserNameState.Approved; }
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
		
		public string ApprovedUserName
		{
			get { return User == null ? null : User.UserName; }
		}

        public IEnumerable<GameSummaryViewModel> YourMove
        {
            get
            {
                return User == null
                    ? Enumerable.Empty<GameSummaryViewModel>()
                    : User.ActivePlayers
                        .Select(p => new GameSummaryViewModel(p, _mainNavigation))
                        .Where(vm => vm.MyTurn);
            }
        }

        public IEnumerable<GameSummaryViewModel> TheirMove
        {
            get
            {
                return User == null
                    ? Enumerable.Empty<GameSummaryViewModel>()
                    : User.ActivePlayers
                        .Select(p => new GameSummaryViewModel(p, _mainNavigation))
                        .Where(vm => !vm.MyTurn);
            }
        }

        public IEnumerable<GameSummaryViewModel> Wins
        {
            get
            {
                return User == null
                    ? Enumerable.Empty<GameSummaryViewModel>()
                    : User.FinishedPlayers
                        .Where(p => p.Game.Outcome.Winner == p)
                        .Select(p => new GameSummaryViewModel(p, _mainNavigation));
            }
        }

        public IEnumerable<GameSummaryViewModel> Losses
        {
            get
            {
                return User == null
                    ? Enumerable.Empty<GameSummaryViewModel>()
                    : User.FinishedPlayers
                        .Where(p => p.Game.Outcome.Winner != p)
                        .Select(p => new GameSummaryViewModel(p, _mainNavigation));
            }
        }

        public void Synchronize()
        {
            _networkServices.Synchronize();
        }

        public bool HasSelectedPlayer
        {
            get { return _mainNavigation.SelectedPlayer != null; }
        }

        private User User
        {
            get { return _identity.User; }
        }
    }
}
