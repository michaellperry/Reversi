using System.Collections.Generic;
using System.Linq;
using FacetedWorlds.Reversi.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Memory;

namespace FacetedWorlds.Reversi.UnitTest
{
    [TestClass]
    public class IdentityServiceTest
    {
        private ICommunity _serverCommunity;
        private ICommunity _clientCommunity;
        private ICommunity _otherClientCommunity;
        private IdentityService _service;
        private Identity _identity;
        private Identity _otherIdentity;

        [TestInitialize()]
        public void Initialize()
        {
            MemoryCommunicationStrategy sharedCommunication = new MemoryCommunicationStrategy();
            _serverCommunity = new Community(new MemoryStorageStrategy())
                .AddCommunicationStrategy(sharedCommunication)
                .RegisterAssembly(typeof(Identity))
                .Subscribe(() => _service);
            _clientCommunity = new Community(new MemoryStorageStrategy())
                .AddCommunicationStrategy(sharedCommunication)
                .RegisterAssembly(typeof(Identity))
                .Subscribe(() => _identity.Claims)
                .Subscribe(() => _identity);
            _otherClientCommunity = new Community(new MemoryStorageStrategy())
                .AddCommunicationStrategy(sharedCommunication)
                .RegisterAssembly(typeof(Identity))
                .Subscribe(() => _otherIdentity.Claims);

            _service = _serverCommunity.AddFact(new IdentityService());
            _identity = _clientCommunity.AddFact(new Identity("liveid:12345"));
            _otherIdentity = _otherClientCommunity.AddFact(new Identity("liveid:12345"));
        }

        [TestMethod]
        public void IdentityServiceInitiallyGetsNoClaims()
        {
            Assert.IsFalse(_service.PendingClaims.Any(), "Should not see any claims.");
        }

        [TestMethod]
        public void WhenClientClaimsAUserNameIdentityServiceSeesClaim()
        {
            _identity.ClaimUserName("mperry");
            Synchronize();
            Assert.IsTrue(_service.PendingClaims.Any(), "Should see a claim.");
        }

        [TestMethod]
        public void WhenClaimIsUniqueServiceApprovesTheClaim()
        {
            _identity.ClaimUserName("mperry");
            Synchronize();
            RunService();
            Synchronize();
            Assert.AreEqual(1, _identity.Claims.Single().Responses.Single().Approved);
            Assert.AreEqual("mperry", _identity.User.UserName);
        }

        [TestMethod]
        public void WhenClaimIsNotUniqueServiceDeniesTheClaim()
        {
            Identity otherIdentity = _clientCommunity.AddFact(new Identity("liveid:23456"));
            otherIdentity.ClaimUserName("mperry");
            Synchronize();
            RunService();
            Synchronize();

            _identity.ClaimUserName("mperry");
            Synchronize();
            RunService();
            Synchronize();
            Assert.AreEqual(0, _identity.Claims.Single().Responses.Single().Approved);
            Assert.IsNull(_identity.User);
        }

        [TestMethod]
        public void WhenDifferesOnlyByCapitalizationServiceDeniesTheClaim()
        {
            Identity otherIdentity = _clientCommunity.AddFact(new Identity("liveid:23456"));
            otherIdentity.ClaimUserName("MPerry");
            Synchronize();
            RunService();
            Synchronize();

            _identity.ClaimUserName("mperry");
            Synchronize();
            RunService();
            Synchronize();
            Assert.AreEqual(0, _identity.Claims.Single().Responses.Single().Approved);
            Assert.IsNull(_identity.User);
        }

        [TestMethod]
        public void WhenUserClaimsTwoNamesServiceDeniesTheClaim()
        {
            _identity.ClaimUserName("michaellperry");
            Synchronize();
            RunService();
            Synchronize();

            _identity.ClaimUserName("mperry");
            Synchronize();
            RunService();
            Synchronize();

            Claim michaellperry = _identity.Claims.Single(claim => claim.User.UserName == "michaellperry");
            Claim mperry = _identity.Claims.Single(claim => claim.User.UserName == "mperry");
            Assert.AreEqual(1, michaellperry.Responses.Single().Approved);
            Assert.AreEqual(0, mperry.Responses.Single().Approved);
            Assert.AreEqual("michaellperry", _identity.User.UserName);
        }

        [TestMethod]
        public void WhenClaimOnOtherClientThisClientSeesApproval()
        {
            _otherIdentity.ClaimUserName("mperry");
            Synchronize();
            RunService();
            Synchronize();
            Assert.AreEqual(1, _identity.Claims.Single().Responses.Single().Approved);
            Assert.AreEqual("mperry", _identity.User.UserName);
        }

        private void Synchronize()
        {
            while (_clientCommunity.Synchronize() || _serverCommunity.Synchronize() || _otherClientCommunity.Synchronize()) ;
        }

        private void RunService()
        {
            IEnumerable<Claim> pendingClaims = _service.PendingClaims;
            foreach (Claim pendingClaim in pendingClaims)
            {
                bool approved =
                    IsNameValid(pendingClaim.User.UserName) &&
                    !pendingClaim.User.IsReserved &&
                    pendingClaim.Identity.User == null;
                if (approved)
                    pendingClaim.Approve();
                else
                    pendingClaim.Deny();
            }

            Assert.IsFalse(_service.PendingClaims.Any(), "The queue should be empty after running the service.");
        }

        private static bool IsNameValid(string name)
        {
            return
                name != null &&
                name.Length > 3 &&
                !name.Any(character => !IsValidNameCharacter(character));
        }

        private static bool IsValidNameCharacter(char character)
        {
            string validCharacters = @".@_";
            return
                char.IsLetterOrDigit(character) ||
                validCharacters.Contains(character);
        }
    }
}
