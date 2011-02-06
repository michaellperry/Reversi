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
        private Community _community;
        private Identity _identity;
        private ViewModelLocator _viewModelLocator;
        private LicenseInformation _licenseInformation = new LicenseInformation();

        private DispatcherTimer _synchronizeTimer = new DispatcherTimer();
        private MainNavigationModel _mainNavigation = new MainNavigationModel();
        private NameNavigationModel _nameNavigationModel = new NameNavigationModel();

        public Presenter()
        {
            IStorageStrategy storageStrategy = IsolatedStorageStorageStrategy.Load();
            //IStorageStrategy storageStrategy = new MemoryStorageStrategy();
            _community = new Community(storageStrategy)
                .AddAsynchronousCommunicationStrategy(new POXAsynchronousCommunicationStrategy(new POXConfigurationProvider()))
                .RegisterAssembly(typeof(User))
                .Subscribe(() => _identity)
                .Subscribe(() => _identity.Claims)
                .Subscribe(() => _identity.ApprovedUsers)
                .Subscribe(() => _identity.ApprovedUsers
                    .SelectMany(user => user.ActivePlayers)
                    .Select(player => player.Game)
                )
                .Subscribe(() => _identity.ApprovedUsers
                    .SelectMany(user => user.PendingGameRequests)
                );

            string anid = UserExtendedProperties.GetValue("ANID") as string;
            string anonymousUserId = String.IsNullOrEmpty(anid)
                ? "test:user12"
                : "liveid:" + ParseAnonymousId(anid);
            _identity = _community.AddFact(new Identity(anonymousUserId));

            // Synchronize whenever the user has something to send.
            _community.FactAdded += delegate
            {
                Synchronize();
            };

            // Synchronize periodically while waiting for a response to a claim.
            _synchronizeTimer.Interval = TimeSpan.FromSeconds(3.0);
            _synchronizeTimer.Tick += (sender, e) =>
            {
                if (_identity.User != null)
                    _synchronizeTimer.Stop();
                else if (_identity.Claims.Any(claim => !claim.Responses.Any()))
                    Synchronize();
            };
            _synchronizeTimer.Start();

            // Synchronize when the network becomes available.
            System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += (sender, e) =>
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                    Synchronize();
            };

            // And synchronize on startup or resume.
            Synchronize();

            _viewModelLocator = new ViewModelLocator(_identity, this, _mainNavigation, _nameNavigationModel);
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
            _community.BeginSynchronize(delegate(IAsyncResult result)
            {
                if (_community.EndSynchronize(result))
                    Synchronize();
            }, null);
        }

        public void LoadNavigation(IDictionary<string, object> state)
        {
            User user = _identity.User;

            if (user != null)
            {
                Get<Guid>(state, STR_SelectedPlayerGameGuid, value =>
                    _mainNavigation.SelectedPlayer = user.ActivePlayers
                        .FirstOrDefault(player => player.Game._unique == value));
                Get<Guid>(state, STR_SelectedGameRequestGuid, value =>
                    _mainNavigation.SelectedGameRequest = user.GameRequests
                        .FirstOrDefault(gameRequest => gameRequest._unique == value));
            }
            Get<string>(state, STR_OpponentName, value => _mainNavigation.OpponentName = value);
            Get<string>(state, STR_MessageBody, value => _mainNavigation.MessageBody = value);
            Get<string>(state, STR_Name, value => _nameNavigationModel.Name = value);
        }

        public void SaveNavigation(IDictionary<string, object> state)
        {
            if (_mainNavigation.SelectedPlayer != null)
                Set(state, STR_SelectedPlayerGameGuid, _mainNavigation.SelectedPlayer.Game._unique);
            if (_mainNavigation.SelectedGameRequest != null)
                Set(state, STR_SelectedGameRequestGuid, _mainNavigation.SelectedGameRequest._unique);

            Set(state, STR_OpponentName, _mainNavigation.OpponentName);
            Set(state, STR_MessageBody, _mainNavigation.MessageBody);
            Set(state, STR_Name, _nameNavigationModel.Name);
        }

        private static string ParseAnonymousId(string anid)
        {
            string[] parts = anid.Split('&');
            IEnumerable<string[]> pairs = parts.Select(part => part.Split('='));
            string id = pairs
                .Where(pair => pair.Length == 2 && pair[0] == "A")
                .Select(pair => pair[1])
                .FirstOrDefault();
            return id;
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
