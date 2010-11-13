using System.Collections.Generic;
using System.Linq;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Mementos;
using System;

/**
/ For use with http://graphviz.org/
digraph "FacetedWorlds.Reversi.Model"
{
    rankdir=BT
    Claim -> Identity [color="red"]
    Claim -> User
    Claim -> IdentityService [color="red"]
    ClaimResponse -> Claim
    Player -> User [color="red"]
    Player -> Game [color="red"]
    Message -> Player
    Acknowledge -> Message
    Move -> Player
    Outcome -> Game
    Outcome -> Player [label="  ?"]
}
**/

namespace FacetedWorlds.Reversi.Model
{
    [CorrespondenceType]
    public partial class Identity : CorrespondenceFact
    {
        // Roles

        // Queries
        public static Query QueryClaims = new Query()
            .JoinSuccessors(Claim.RoleIdentity)
            ;

        // Predicates

        // Predecessors

        // Fields
        [CorrespondenceField]
        public string _uri;

        // Results
        private Result<Claim> _claims;

        // Business constructor
        public Identity(
            string uri
            )
        {
            InitializeResults();
            _uri = uri;
        }

        // Hydration constructor
        public Identity(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _claims = new Result<Claim>(this, QueryClaims);
        }

        // Predecessor access

        // Field access
        public string Uri
        {
            get { return _uri; }
        }

        // Query result access
        public IEnumerable<Claim> Claims
        {
            get { return _claims; }
        }
    }
    
    [CorrespondenceType]
    public partial class IdentityService : CorrespondenceFact
    {
        // Roles

        // Queries
        public static Query QueryPendingClaims = new Query()
            .JoinSuccessors(Claim.RoleIdentityService, Condition.WhereIsEmpty(Claim.QueryIsPending)
            )
            ;

        // Predicates

        // Predecessors

        // Fields

        // Results
        private Result<Claim> _pendingClaims;

        // Business constructor
        public IdentityService(
            )
        {
            InitializeResults();
        }

        // Hydration constructor
        public IdentityService(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _pendingClaims = new Result<Claim>(this, QueryPendingClaims);
        }

        // Predecessor access

        // Field access

        // Query result access
        public IEnumerable<Claim> PendingClaims
        {
            get { return _pendingClaims; }
        }
    }
    
    [CorrespondenceType]
    public partial class Claim : CorrespondenceFact
    {
        // Roles
        public static Role<Identity> RoleIdentity = new Role<Identity>("identity", RoleRelationship.Pivot);
        public static Role<User> RoleUser = new Role<User>("user");
        public static Role<IdentityService> RoleIdentityService = new Role<IdentityService>("identityService", RoleRelationship.Pivot);

        // Queries
        public static Query QueryResponses = new Query()
            .JoinSuccessors(ClaimResponse.RoleClaim)
            ;
        public static Query QueryIsPending = new Query()
            .JoinSuccessors(ClaimResponse.RoleClaim)
            ;

        // Predicates
        public static Condition IsPending = Condition.WhereIsEmpty(QueryIsPending);

        // Predecessors
        private PredecessorObj<Identity> _identity;
        private PredecessorObj<User> _user;
        private PredecessorObj<IdentityService> _identityService;

        // Fields

        // Results
        private Result<ClaimResponse> _responses;

        // Business constructor
        public Claim(
            Identity identity
            ,User user
            ,IdentityService identityService
            )
        {
            InitializeResults();
            _identity = new PredecessorObj<Identity>(this, RoleIdentity, identity);
            _user = new PredecessorObj<User>(this, RoleUser, user);
            _identityService = new PredecessorObj<IdentityService>(this, RoleIdentityService, identityService);
        }

        // Hydration constructor
        public Claim(FactMemento memento)
        {
            InitializeResults();
            _identity = new PredecessorObj<Identity>(this, RoleIdentity, memento);
            _user = new PredecessorObj<User>(this, RoleUser, memento);
            _identityService = new PredecessorObj<IdentityService>(this, RoleIdentityService, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
            _responses = new Result<ClaimResponse>(this, QueryResponses);
        }

        // Predecessor access
        public Identity Identity
        {
            get { return _identity.Fact; }
        }
        public User User
        {
            get { return _user.Fact; }
        }
        public IdentityService IdentityService
        {
            get { return _identityService.Fact; }
        }

        // Field access

        // Query result access
        public IEnumerable<ClaimResponse> Responses
        {
            get { return _responses; }
        }
    }
    
    [CorrespondenceType]
    public partial class ClaimResponse : CorrespondenceFact
    {
        // Roles
        public static Role<Claim> RoleClaim = new Role<Claim>("claim");

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Claim> _claim;

        // Fields
        [CorrespondenceField]
        public int _approved;

        // Results

        // Business constructor
        public ClaimResponse(
            Claim claim
            ,int approved
            )
        {
            InitializeResults();
            _claim = new PredecessorObj<Claim>(this, RoleClaim, claim);
            _approved = approved;
        }

        // Hydration constructor
        public ClaimResponse(FactMemento memento)
        {
            InitializeResults();
            _claim = new PredecessorObj<Claim>(this, RoleClaim, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Claim Claim
        {
            get { return _claim.Fact; }
        }

        // Field access
        public int Approved
        {
            get { return _approved; }
        }

        // Query result access
    }
    
    [CorrespondenceType]
    public partial class User : CorrespondenceFact
    {
        // Roles

        // Queries
        public static Query QueryClaims = new Query()
            .JoinSuccessors(Claim.RoleUser)
            ;
        public static Query QueryActivePlayers = new Query()
            .JoinSuccessors(Player.RoleUser, Condition.WhereIsEmpty(Player.QueryIsActive)
            )
            ;
        public static Query QueryFinishedPlayers = new Query()
            .JoinSuccessors(Player.RoleUser, Condition.WhereIsNotEmpty(Player.QueryIsNotActive)
            )
            ;
        public static Query QueryRelatedUsers = new Query()
            .JoinSuccessors(Player.RoleUser)
            .JoinPredecessors(Player.RoleGame)
            .JoinSuccessors(Player.RoleGame)
            .JoinPredecessors(Player.RoleUser)
            ;

        // Predicates

        // Predecessors

        // Fields
        [CorrespondenceField]
        public string _userName;

        // Results
        private Result<Claim> _claims;
        private Result<Player> _activePlayers;
        private Result<Player> _finishedPlayers;
        private Result<User> _relatedUsers;

        // Business constructor
        public User(
            string userName
            )
        {
            InitializeResults();
            _userName = userName;
        }

        // Hydration constructor
        public User(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _claims = new Result<Claim>(this, QueryClaims);
            _activePlayers = new Result<Player>(this, QueryActivePlayers);
            _finishedPlayers = new Result<Player>(this, QueryFinishedPlayers);
            _relatedUsers = new Result<User>(this, QueryRelatedUsers);
        }

        // Predecessor access

        // Field access
        public string UserName
        {
            get { return _userName; }
        }

        // Query result access
        public IEnumerable<Claim> Claims
        {
            get { return _claims; }
        }
        public IEnumerable<Player> ActivePlayers
        {
            get { return _activePlayers; }
        }
        public IEnumerable<Player> FinishedPlayers
        {
            get { return _finishedPlayers; }
        }
        public IEnumerable<User> RelatedUsers
        {
            get { return _relatedUsers; }
        }
    }
    
    [CorrespondenceType]
    public partial class Game : CorrespondenceFact
    {
        // Roles

        // Queries
        public static Query QueryPlayers = new Query()
            .JoinSuccessors(Player.RoleGame)
            ;
        public static Query QueryMessages = new Query()
            .JoinSuccessors(Player.RoleGame)
            .JoinSuccessors(Message.RoleSender)
            ;
        public static Query QueryMoves = new Query()
            .JoinSuccessors(Player.RoleGame)
            .JoinSuccessors(Move.RolePlayer)
            ;
        public static Query QueryOutcomes = new Query()
            .JoinSuccessors(Outcome.RoleGame)
            ;

        // Predicates

        // Predecessors

        // Unique
        [CorrespondenceField]
        public Guid _unique;

        // Fields

        // Results
        private Result<Player> _players;
        private Result<Message> _messages;
        private Result<Move> _moves;
        private Result<Outcome> _outcomes;

        // Business constructor
        public Game(
            )
        {
            _unique = Guid.NewGuid();
            InitializeResults();
        }

        // Hydration constructor
        public Game(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _players = new Result<Player>(this, QueryPlayers);
            _messages = new Result<Message>(this, QueryMessages);
            _moves = new Result<Move>(this, QueryMoves);
            _outcomes = new Result<Outcome>(this, QueryOutcomes);
        }

        // Predecessor access

        // Field access

        // Query result access
        public IEnumerable<Player> Players
        {
            get { return _players; }
        }
        public IEnumerable<Message> Messages
        {
            get { return _messages; }
        }
        public IEnumerable<Move> Moves
        {
            get { return _moves; }
        }
        public IEnumerable<Outcome> Outcomes
        {
            get { return _outcomes; }
        }
    }
    
    [CorrespondenceType]
    public partial class Player : CorrespondenceFact
    {
        // Roles
        public static Role<User> RoleUser = new Role<User>("user", RoleRelationship.Pivot);
        public static Role<Game> RoleGame = new Role<Game>("game", RoleRelationship.Pivot);

        // Queries
        public static Query QueryMoves = new Query()
            .JoinSuccessors(Move.RolePlayer)
            ;
        public static Query QueryNewMessages = new Query()
            .JoinSuccessors(Message.RoleSender, Condition.WhereIsEmpty(Message.QueryIsAcknowledged)
            )
            ;
        public static Query QueryIsActive = new Query()
            .JoinPredecessors(Player.RoleGame)
            .JoinSuccessors(Outcome.RoleGame)
            ;
        public static Query QueryIsNotActive = new Query()
            .JoinPredecessors(Player.RoleGame)
            .JoinSuccessors(Outcome.RoleGame)
            ;

        // Predicates
        public static Condition IsActive = Condition.WhereIsEmpty(QueryIsActive);
        public static Condition IsNotActive = Condition.WhereIsNotEmpty(QueryIsNotActive);

        // Predecessors
        private PredecessorObj<User> _user;
        private PredecessorObj<Game> _game;

        // Fields
        [CorrespondenceField]
        public int _index;

        // Results
        private Result<Move> _moves;
        private Result<Message> _newMessages;

        // Business constructor
        public Player(
            User user
            ,Game game
            ,int index
            )
        {
            InitializeResults();
            _user = new PredecessorObj<User>(this, RoleUser, user);
            _game = new PredecessorObj<Game>(this, RoleGame, game);
            _index = index;
        }

        // Hydration constructor
        public Player(FactMemento memento)
        {
            InitializeResults();
            _user = new PredecessorObj<User>(this, RoleUser, memento);
            _game = new PredecessorObj<Game>(this, RoleGame, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
            _moves = new Result<Move>(this, QueryMoves);
            _newMessages = new Result<Message>(this, QueryNewMessages);
        }

        // Predecessor access
        public User User
        {
            get { return _user.Fact; }
        }
        public Game Game
        {
            get { return _game.Fact; }
        }

        // Field access
        public int Index
        {
            get { return _index; }
        }

        // Query result access
        public IEnumerable<Move> Moves
        {
            get { return _moves; }
        }
        public IEnumerable<Message> NewMessages
        {
            get { return _newMessages; }
        }
    }
    
    [CorrespondenceType]
    public partial class Message : CorrespondenceFact
    {
        // Roles
        public static Role<Player> RoleSender = new Role<Player>("sender");

        // Queries
        public static Query QueryIsAcknowledged = new Query()
            .JoinSuccessors(Acknowledge.RoleMessage)
            ;

        // Predicates
        public static Condition IsAcknowledged = Condition.WhereIsNotEmpty(QueryIsAcknowledged);

        // Predecessors
        private PredecessorObj<Player> _sender;

        // Fields
        [CorrespondenceField]
        public string _body;

        // Results

        // Business constructor
        public Message(
            Player sender
            ,string body
            )
        {
            InitializeResults();
            _sender = new PredecessorObj<Player>(this, RoleSender, sender);
            _body = body;
        }

        // Hydration constructor
        public Message(FactMemento memento)
        {
            InitializeResults();
            _sender = new PredecessorObj<Player>(this, RoleSender, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Player Sender
        {
            get { return _sender.Fact; }
        }

        // Field access
        public string Body
        {
            get { return _body; }
        }

        // Query result access
    }
    
    [CorrespondenceType]
    public partial class Acknowledge : CorrespondenceFact
    {
        // Roles
        public static Role<Message> RoleMessage = new Role<Message>("message");

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Message> _message;

        // Fields

        // Results

        // Business constructor
        public Acknowledge(
            Message message
            )
        {
            InitializeResults();
            _message = new PredecessorObj<Message>(this, RoleMessage, message);
        }

        // Hydration constructor
        public Acknowledge(FactMemento memento)
        {
            InitializeResults();
            _message = new PredecessorObj<Message>(this, RoleMessage, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Message Message
        {
            get { return _message.Fact; }
        }

        // Field access

        // Query result access
    }
    
    [CorrespondenceType]
    public partial class Move : CorrespondenceFact
    {
        // Roles
        public static Role<Player> RolePlayer = new Role<Player>("player");

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Player> _player;

        // Fields
        [CorrespondenceField]
        public int _index;
        [CorrespondenceField]
        public int _square;

        // Results

        // Business constructor
        public Move(
            Player player
            ,int index
            ,int square
            )
        {
            InitializeResults();
            _player = new PredecessorObj<Player>(this, RolePlayer, player);
            _index = index;
            _square = square;
        }

        // Hydration constructor
        public Move(FactMemento memento)
        {
            InitializeResults();
            _player = new PredecessorObj<Player>(this, RolePlayer, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Player Player
        {
            get { return _player.Fact; }
        }

        // Field access
        public int Index
        {
            get { return _index; }
        }
        public int Square
        {
            get { return _square; }
        }

        // Query result access
    }
    
    [CorrespondenceType]
    public partial class Outcome : CorrespondenceFact
    {
        // Roles
        public static Role<Game> RoleGame = new Role<Game>("game");
        public static Role<Player> RoleWinner = new Role<Player>("winner");

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Game> _game;
        private PredecessorOpt<Player> _winner;

        // Fields

        // Results

        // Business constructor
        public Outcome(
            Game game
            ,Player winner
            )
        {
            InitializeResults();
            _game = new PredecessorObj<Game>(this, RoleGame, game);
            _winner = new PredecessorOpt<Player>(this, RoleWinner, winner);
        }

        // Hydration constructor
        public Outcome(FactMemento memento)
        {
            InitializeResults();
            _game = new PredecessorObj<Game>(this, RoleGame, memento);
            _winner = new PredecessorOpt<Player>(this, RoleWinner, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Game Game
        {
            get { return _game.Fact; }
        }
        public Player Winner
        {
            get { return _winner.Fact; }
        }

        // Field access

        // Query result access
    }
    
}
