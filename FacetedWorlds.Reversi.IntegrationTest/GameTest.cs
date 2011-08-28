using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FacetedWorlds.Reversi.Model;
using System.Collections.Generic;
using System.Threading;

namespace FacetedWorlds.Reversi.IntegrationTest
{
    [TestClass]
    public class GameTest
    {
        private Machine _alan;
        private Machine _flynn;

        [TestInitialize]
        public void Initialize()
        {
            _alan = new Machine("testuseralan");
            _flynn = new Machine("testuserflynn");
            Synchronize();
            Synchronize();
        }

        [TestMethod]
        public void AlanChallengesFlynn()
        {
            int before = _flynn.User.ActivePlayers.Count();
            var playerAlan = _alan.User.Challenge("testuserflynn");
            Synchronize();
            int after = _flynn.User.ActivePlayers.Count();

            Resign(playerAlan);
            Synchronize();

            Assert.AreEqual(before + 1, after);
        }

        [TestMethod]
        public void AlanAndFlynnGetRandomGame()
        {
            int before = _flynn.User.ActivePlayers.Count();
            _alan.User.RequestGame();
            _flynn.User.RequestGame();
            Synchronize();
            Thread.Sleep(2000);
            Synchronize();
            int after = _flynn.User.ActivePlayers.Count();

            foreach (Player player in _alan.User.ActivePlayers)
                Resign(player);
            Synchronize();

            Assert.AreEqual(before + 1, after);
        }

        private void Synchronize()
        {
            _alan.Synchronize();
            _flynn.Synchronize();
            _alan.Synchronize();
        }

        private static void Resign(Player player)
        {
            player.Game.DeclareWinner(player.Game.Players.FirstOrDefault(p => p != player), true);
        }
    }
}
