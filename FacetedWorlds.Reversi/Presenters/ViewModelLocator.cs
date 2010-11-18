using System.Linq;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;
using FacetedWorlds.Reversi.ViewModels;
using UpdateControls;

namespace FacetedWorlds.Reversi.Presenters
{
    public class ViewModelLocator
    {
        private Identity _identity;
        private IPresentationServices _presentationServices;
        private MainNavigationModel _mainNavigationModel;

        private NameNavigationModel _nameNavigationModel;
        private MainViewModel _main;
        private NewGameViewModel _newGame;
        private IGameViewModel _game;
        private ChatViewModel _chat;

        private Dependent _depGame;
        private Dependent _depChat;

        public ViewModelLocator(Identity identity, IPresentationServices presentationServices, MainNavigationModel mainNavigationModel, NameNavigationModel nameNavigationModel)
        {
            _identity = identity;
            _presentationServices = presentationServices;
            _mainNavigationModel = mainNavigationModel;
            _nameNavigationModel = nameNavigationModel;

            _main = new MainViewModel(_presentationServices, _identity, _mainNavigationModel);
            _newGame = new NewGameViewModel(_identity, _mainNavigationModel, _nameNavigationModel);
            _depGame = new Dependent(delegate
            {
                if (_mainNavigationModel.SelectedPlayer != null)
                {
                    _game = new RemoteGameViewModel(_mainNavigationModel.SelectedPlayer, _mainNavigationModel);
                }
                else
                {
                    LocalGame localGame = _identity.ActiveLocalGames.FirstOrDefault();
                    if (localGame != null)
                        _game = new LocalGameViewModel(localGame, _mainNavigationModel);
                    else
                        _game = null;
                }
            });
            _depChat = new Dependent(delegate
            {
                _chat = _mainNavigationModel.SelectedPlayer == null
                    ? null
                    : new ChatViewModel(_mainNavigationModel.SelectedPlayer, _mainNavigationModel);
            });
        }
        
        public MainViewModel Main
        {
            get { return _main; }
        }

        public NewGameViewModel NewGame
        {
            get { return _newGame; }
        }

        public IGameViewModel Game
        {
            get { _depGame.OnGet(); return _game; }
        }

        public ChatViewModel Chat
        {
            get { _depChat.OnGet(); return _chat; }
        }
    }
}
