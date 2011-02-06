using FacetedWorlds.Reversi.Client.NavigationModels;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class RemoteGameViewModel : PlayerGameViewModel
    {
        private Player _player;

        public RemoteGameViewModel(Player player, MainNavigationModel mainNavigation) :
            base(mainNavigation)
        {
            _player = player;
        }

        protected override Player Player
        {
            get { return _player; }
        }

        public override void ClearSelectedPlayer()
        {
            MainNavigation.SelectedPlayer = null;
        }

        public override bool IsWaiting
        {
            get { return false; }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            RemoteGameViewModel that = obj as RemoteGameViewModel;
            if (that == null)
                return false;
            return this._player.Equals(that._player);
        }

        public override int GetHashCode()
        {
            return _player.GetHashCode();
        }
    }
}
