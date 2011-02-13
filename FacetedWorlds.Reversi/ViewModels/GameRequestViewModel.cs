using System;
using System.Linq;
using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;
using UpdateControls;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class GameRequestViewModel : PlayerGameViewModel
    {
        private GameRequest _gameRequest;

        public GameRequestViewModel(GameRequest gameRequest, MainNavigationModel mainNavigation) :
            base(mainNavigation)
        {
            _gameRequest = gameRequest;
        }

        protected override Player Player
        {
            get { return _gameRequest.Player.FirstOrDefault(); }
        }

        public override void ClearSelectedPlayer()
        {
            MainNavigation.SelectedGameRequest = null;
        }

        public override bool IsWaiting
        {
            get { return Get(() => !_gameRequest.Player.Any()); }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            GameRequestViewModel that = obj as GameRequestViewModel;
            if (that == null)
                return false;
            return _gameRequest == that._gameRequest;
        }

        public override int GetHashCode()
        {
            return _gameRequest.GetHashCode();
        }
    }
}
