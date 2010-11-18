using System.Collections.Generic;
using System.Linq;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;
using System;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class MainViewModel
    {
        private IPresentationServices _presentationServices;
        private Identity _identity;
        private MainNavigationModel _mainNavigation;

        public MainViewModel(IPresentationServices presentationServices, Identity identity, MainNavigationModel mainNavigation)
        {
            _presentationServices = presentationServices;
            _identity = identity;
            _mainNavigation = mainNavigation;
        }

        public bool IsUserApproved
        {
            get { return User != null; }
        }

        public bool IsUserNotApproved
        {
            get { return User == null; }
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
            _presentationServices.Synchronize();
        }

        public bool CanChallenge
        {
            get
            {
                if (User == null)
                {
                    // If the user does not have an alias, let them sign up.
                    return true;
                }
                else if (!_presentationServices.IsInTrialMode)
                {
                    // If the user has paid, let them challenge.
                    return true;
                }
                else if (!User.ActivePlayers.Any() && !User.FinishedPlayers.Any())
                {
                    // If the user has an alias and has not paid,
                    // let them create their first game.
                    return true;
                }
                else
                {
                    // Trial is over.
                    return false;
                }
            }
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
