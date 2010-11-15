using System.Collections.Generic;
using System.Linq;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class MainViewModel
    {
        private INetworkServices _networkServices;
        private Identity _identity;
        private MainNavigationModel _mainNavigation;
        private NameNavigationModel _nameNavigation;

        public MainViewModel(INetworkServices networkServices, Identity identity, MainNavigationModel mainNavigation, NameNavigationModel nameNavigation)
        {
            _networkServices = networkServices;
            _identity = identity;
            _mainNavigation = mainNavigation;
            _nameNavigation = nameNavigation;
        }

        public bool IsUserApproved
        {
            get { return User != null; }
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

        public void CreateLocalGame()
        {
            if (!_identity.ActiveLocalGames.Any())
                _identity.CreateLocalGame();
        }

        private User User
        {
            get { return _identity.User; }
        }
    }
}
