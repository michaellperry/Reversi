using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.NavigationModels;
using FacetedWorlds.Reversi.ViewModels;
using UpdateControls;

namespace FacetedWorlds.Reversi.Presenters
{
    public class ViewModelLocator
    {
        private Identity _identity;
        private INetworkServices _networkServices;
        private MainNavigationModel _mainNavigationModel;

        private NameNavigationModel _nameNavigationModel;
        private MainViewModel _main;
        private NewGameViewModel _newGame;
        private GameViewModel _game;
        private ChatViewModel _chat;

        private Dependent _depNewGame;
        private Dependent _depGame;
        private Dependent _depChat;

        public ViewModelLocator(Identity identity, INetworkServices networkServices, MainNavigationModel mainNavigationModel, NameNavigationModel nameNavigationModel)
        {
            _identity = identity;
            _networkServices = networkServices;
            _mainNavigationModel = mainNavigationModel;
            _nameNavigationModel = nameNavigationModel;

            _main = new MainViewModel(_networkServices, _identity, _mainNavigationModel, _nameNavigationModel);
            _depNewGame = new Dependent(delegate
            {
                _newGame = _identity.User == null
                    ? null
                    : new NewGameViewModel(_identity.User, _mainNavigationModel);
            });
            _depGame = new Dependent(delegate
            {
                _game = _mainNavigationModel.SelectedPlayer == null
                    ? null
                    : new GameViewModel(_mainNavigationModel.SelectedPlayer, _mainNavigationModel);
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
            get { _depNewGame.OnGet(); return _newGame; }
        }

        public GameViewModel Game
        {
            get { _depGame.OnGet(); return _game; }
        }

        public ChatViewModel Chat
        {
            get { _depChat.OnGet(); return _chat; }
        }
    }
}
