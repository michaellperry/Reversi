using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class ChallengeViewModel
    {
        private User _user;
        private MainNavigationModel _mainNavigation;

        public ChallengeViewModel(User user, MainNavigationModel mainNavigation)
        {
            _user = user;
            _mainNavigation = mainNavigation;
        }

        public string OpponentName
        {
            get { return _mainNavigation.OpponentName; }
            set { _mainNavigation.OpponentName = value; }
        }

        public IEnumerable<string> MyFriends
        {
            get
            {
                return _user.RelatedUsers
                    .Distinct()
                    .Where(user => user != _user)
                    .Select(user => user.UserName);
            }
        }

        public ICommand Challenge
        {
            get
            {
                return MakeCommand
                    .When(() => User.IsNameValid(_mainNavigation.OpponentName))
                    .Do(() =>
                    {
                        _mainNavigation.SelectedPlayer = _user.Challenge(_mainNavigation.OpponentName);
                        _mainNavigation.OpponentName = null;
                    });
            }
        }

        public void RequestGame()
        {
            _mainNavigation.SelectedGameRequest = _user.RequestGame();
            _mainNavigation.OpponentName = null;
        }
    }
}
