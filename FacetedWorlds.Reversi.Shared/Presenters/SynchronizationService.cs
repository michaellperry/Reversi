using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using FacetedWorlds.Reversi.Model;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.IsolatedStorage;
using UpdateControls.Correspondence.POXClient;
using UpdateControls.Correspondence.Strategy;

namespace FacetedWorlds.Reversi.Presenters
{
    public class SynchronizationService
    {
        public Community _community;
        private DispatcherTimer _synchronizeTimer = new DispatcherTimer();
        public Identity _identity;

        public SynchronizationService()
        {
            IStorageStrategy storageStrategy = IsolatedStorageStorageStrategy.Load();
            //IStorageStrategy storageStrategy = new MemoryStorageStrategy();
            POXConfigurationProvider configurationProvider = new POXConfigurationProvider();
            _community = new Community(storageStrategy)
                .AddAsynchronousCommunicationStrategy(new POXAsynchronousCommunicationStrategy(configurationProvider))
                .Register<CorrespondenceModel>()
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

            _identity = _community.AddFact(new Identity(GetAnonymousUserId()));
            configurationProvider.Identity = _identity;

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
        }

        public void Synchronize()
        {
            _community.BeginSynchronize(delegate(IAsyncResult result)
            {
                if (_community.EndSynchronize(result))
                    Synchronize();
            }, null);
        }

        private static string GetAnonymousUserId()
        {
            try
            {
                string anid = UserExtendedProperties.GetValue("ANID") as string;
                string anonymousUserId = String.IsNullOrEmpty(anid)
                    ? "test:user12"
                    : "liveid:" + ParseAnonymousId(anid);
                return anonymousUserId;
            }
            catch (Exception)
            {
                return "test:user12";
            }
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
    }
}
