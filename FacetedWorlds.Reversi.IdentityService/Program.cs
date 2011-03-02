using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FacetedWorlds.Reversi.Model;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.SSCE;
using UpdateControls.Correspondence.WebServiceClient;

namespace FacetedWorlds.Reversi.IdentityServices
{
    class Program
    {
        private static ManualResetEvent _stop = new ManualResetEvent(false);
        private static IdentityService _service;

        static void Main(string[] args)
        {
            Console.WriteLine("Identity service is running. Press a key to stop.");
            Thread serviceThread = new Thread(ServiceThreadProc) { Name = "Service thread" };
            serviceThread.Start();
            Console.ReadKey();
            _stop.Set();
            serviceThread.Join();
        }

        private static void ServiceThreadProc()
        {
            // Load the correspondence database.
            string databaseFileName = Directory.ApplicationData / "FacetedWorlds" / "ReversiIdentificationService" / "Correspondence.sdf";

            Community community = new Community(new SSCEStorageStrategy(databaseFileName))
                .AddCommunicationStrategy(new WebServiceCommunicationStrategy())
                .Register<CorrespondenceModule>()
                .Subscribe(() => _service);

            _service = community.AddFact(new IdentityService());

            while (!_stop.WaitOne(TimeSpan.FromSeconds(15.0)))
            {
                try
                {
                    Synchronize(community);
                    if (RunService())
                        Synchronize(community);
                }
                catch (Exception ex)
                {
                    // Wait 1 minute between exceptions.
                    _stop.WaitOne(TimeSpan.FromMinutes(1.0));
                }
            }
        }

        private static void Synchronize(Community community)
        {
            while (community.Synchronize());
        }

        private static bool RunService()
        {
            List<Claim> pendingClaims = _service.PendingClaims.ToList();
            foreach (Claim pendingClaim in pendingClaims)
            {
                bool approved =
                    IsNameValid(pendingClaim.User.UserName) &&
                    !pendingClaim.User.IsReserved &&
                    pendingClaim.Identity.User == null;
                Console.WriteLine(String.Format("Claim by {0} for user name {1} {2}.", 
                    pendingClaim.Identity.Uri, 
                    pendingClaim.User.UserName, 
                    approved ? "approved" : "denied"));
                if (approved)
                    pendingClaim.Approve();
                else
                    pendingClaim.Deny();
            }
            return pendingClaims.Any();
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
