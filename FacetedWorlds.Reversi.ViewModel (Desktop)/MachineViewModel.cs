using System;
using System.Linq;
using FacetedWorlds.Reversi.Model;
using UpdateControls;
using UpdateControls.Correspondence;
using System.Windows.Input;
using UpdateControls.XAML;
using System.Collections.Generic;

namespace FacetedWorlds.Reversi.ViewModel
{
    public class MachineViewModel
    {
        private Community _community;
        private MachineNavigationModel _navigation;
        private ChatNavigationModel _chatNavigationModel = new ChatNavigationModel();

        private User _user;
        private Dependent _depUser;

        public MachineViewModel(
            Community community,
            MachineNavigationModel navigation)
        {
            _community = community;
            _navigation = navigation;

            _depUser = new Dependent(delegate
            {
                _user = String.IsNullOrEmpty(_navigation.UserName)
                    ? null
                    : _community.AddFact(new User(_navigation.UserName));
            });
        }

        public User User
        {
            get { _depUser.OnGet(); return _user; }
        }

        public string Username
        {
            get { return _navigation.UserName; }
            set { _navigation.UserName = value; }
        }

        public bool IsLoggedOn
        {
            get { return !String.IsNullOrEmpty(_navigation.UserName); }
        }

        public string OpponentName
        {
            get { return _navigation.OpponentName; }
            set { _navigation.OpponentName = value; }
        }

        public ICommand Challenge
        {
            get
            {
                return MakeCommand
                    .When(() => User != null && !string.IsNullOrEmpty(_navigation.OpponentName))
                    .Do(() =>
                    {
                        _navigation.SelectedPlayer = User.Challenge(_navigation.OpponentName);
                        _navigation.OpponentName = null;
                    });
            }
        }

        public IEnumerable<GameSummaryViewModel> YourMove
        {
            get
            {
                if (User == null)
                    return Enumerable.Empty<GameSummaryViewModel>();
                return User.ActivePlayers
                    .Select(p => new GameSummaryViewModel(p))
                    .Where(vm => vm.MyTurn);
            }
        }

        public IEnumerable<GameSummaryViewModel> TheirMove
        {
            get
            {
                if (User == null)
                    return Enumerable.Empty<GameSummaryViewModel>();
                return User.ActivePlayers
                    .Select(p => new GameSummaryViewModel(p))
                    .Where(vm => !vm.MyTurn);
            }
        }

        public IEnumerable<GameSummaryViewModel> Wins
        {
            get
            {
                if (User == null)
                    return Enumerable.Empty<GameSummaryViewModel>();
                return User.FinishedPlayers
                    .Where(p => p.Game.Outcome.Winner == p)
                    .Select(p => new GameSummaryViewModel(p));
            }
        }

        public IEnumerable<GameSummaryViewModel> Losses
        {
            get
            {
                if (User == null)
                    return Enumerable.Empty<GameSummaryViewModel>();
                return User.FinishedPlayers
                    .Where(p => p.Game.Outcome.Winner != p)
                    .Select(p => new GameSummaryViewModel(p));
            }
        }

        public GameSummaryViewModel SelectedGame
        {
            get
            {
                return _navigation.SelectedPlayer == null
                    ? null
                    : new GameSummaryViewModel(_navigation.SelectedPlayer);
            }
            set
            {
                if (value != null)
                    _navigation.SelectedPlayer = value.Player;
            }
        }

        public GameViewModel SelectedGameDetail
        {
            get
            {
                return _navigation.SelectedPlayer == null
                    ? null
                    : new GameViewModel(_navigation.SelectedPlayer);
            }
        }

        public ChatViewModel SelectedChat
        {
            get
            {
                return _navigation.SelectedPlayer == null
                    ? null
                    : new ChatViewModel(_navigation.SelectedPlayer, _chatNavigationModel);
            }
        }
    }
}
