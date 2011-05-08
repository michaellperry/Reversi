using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;
using FacetedWorlds.Reversi.ViewModels;
using Microsoft.Phone.Info;
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Net.NetworkInformation;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.IsolatedStorage;
using UpdateControls.Correspondence.POXClient;
using UpdateControls.Correspondence.Strategy;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi.Presenters
{
    public class Presenter : IPresentationServices
    {
        private const string STR_SelectedPlayerGameGuid = "SelectedPlayerGameGuid";
        private const string STR_SelectedGameRequestGuid = "SelectedGameRequestGuid";
        private const string STR_OpponentName = "OpponentName";
        private const string STR_MessageBody = "MessageBody";
        private const string STR_Name = "Name";
        private SynchronizationService _synchronizationService = new SynchronizationService();
        private ViewModelLocator _viewModelLocator;
        private LicenseInformation _licenseInformation = new LicenseInformation();

        private MainNavigationModel _mainNavigation = new MainNavigationModel();
        private NameNavigationModel _nameNavigationModel = new NameNavigationModel();

        public Presenter()
        {
            _viewModelLocator = new ViewModelLocator(_synchronizationService._identity, this, _mainNavigation, _nameNavigationModel);
        }


        public bool ForceInTrialMode { get; set; }

        public bool IsInTrialMode
        {
            get
            {
                return
#if DEBUG
                    ForceInTrialMode ||
#endif
                    _licenseInformation.IsTrial();
            }
        }

        public object ViewModelLocator
        {
            get { return ForView.Wrap(_viewModelLocator); }
        }

        public void Synchronize()
        {
            _synchronizationService.Synchronize();
        }

        public void LoadNavigation(IDictionary<string, object> state)
        {
            User user = _synchronizationService._identity.User;

            if (user != null)
            {
                Get<Guid>(state, STR_SelectedPlayerGameGuid, value =>
                    _mainNavigation.SelectedPlayer = user.ActivePlayers
                        .FirstOrDefault(player => player.Game.Unique == value));
                Get<Guid>(state, STR_SelectedGameRequestGuid, value =>
                    _mainNavigation.SelectedGameRequest = user.GameRequests
                        .FirstOrDefault(gameRequest => gameRequest.Unique == value));
            }
            Get<string>(state, STR_OpponentName, value => _mainNavigation.OpponentName = value);
            Get<string>(state, STR_MessageBody, value => _mainNavigation.MessageBody = value);
            Get<string>(state, STR_Name, value => _nameNavigationModel.Name = value);
        }

        public void SaveNavigation(IDictionary<string, object> state)
        {
            if (_mainNavigation.SelectedPlayer != null)
                Set(state, STR_SelectedPlayerGameGuid, _mainNavigation.SelectedPlayer.Game.Unique);
            if (_mainNavigation.SelectedGameRequest != null)
                Set(state, STR_SelectedGameRequestGuid, _mainNavigation.SelectedGameRequest.Unique);

            Set(state, STR_OpponentName, _mainNavigation.OpponentName);
            Set(state, STR_MessageBody, _mainNavigation.MessageBody);
            Set(state, STR_Name, _nameNavigationModel.Name);
        }

        private static void Set(IDictionary<string, object> state, string key, object value)
        {
            if (state.ContainsKey(key))
                state.Remove(key);
            if (value != null)
                state.Add(key, value);
        }

        private static void Get<T>(IDictionary<string, object> state, string key, Action<T> assignment)
        {
            object value;
            if (state.TryGetValue(key, out value) && value is T)
                assignment((T)value);
        }
    }
}
