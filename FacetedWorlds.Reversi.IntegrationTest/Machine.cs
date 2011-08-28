using FacetedWorlds.Reversi.Model;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Memory;
using UpdateControls.Correspondence.POXClient;
using System.Threading;

namespace FacetedWorlds.Reversi.IntegrationTest
{
    public class Machine
    {
        private Community _community;
        private User _user;

        public Machine(string userName)
        {
            _community = new Community(new MemoryStorageStrategy())
                .AddAsynchronousCommunicationStrategy(new POXAsynchronousCommunicationStrategy(new POXConfigurationProvider()))
                .Register<CorrespondenceModel>()
                .Subscribe(() => _user);

            _user = _community.AddFact(new User(userName));
        }

        public User User
        {
            get { return _user; }
        }

        public void Synchronize()
        {
            _community.BeginSending();
            _community.BeginReceiving();
            while (_community.Synchronizing)
            {
                Thread.Sleep(100);
            }
        }
    }
}
