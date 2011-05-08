using System.Collections.Generic;
using System.Linq;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Mementos;
using UpdateControls.Correspondence.Strategy;
using System;
using System.IO;

/**
/ For use with http://graphviz.org/
digraph "FacetedWorlds.Reversi.Model"
{
    rankdir=BT
    Claim -> Identity [color="red"]
    Claim -> User
    Claim -> IdentityService [color="red"]
    ClaimResponse -> Claim [color="red"]
    ChatEnable -> User
    DisableToastNotification -> Identity
    EnableToastNotification -> DisableToastNotification
    Player -> User [color="red"]
    Player -> Game [color="red"]
    Message -> Player
    Acknowledge -> Message
    Move -> Player
    Outcome -> Game
    Outcome -> Player [label="  ?"]
    OutcomeAcknowledge -> Player
    OutcomeAcknowledge -> Outcome
    LocalGame -> Identity
    LocalPlayer -> LocalGame
    LocalMove -> LocalPlayer
    LocalOutcome -> LocalGame
    LocalOutcome -> LocalPlayer [label="  ?"]
    LocalOutcomeAcknowledge -> LocalOutcome
    GameRequest -> MatchmakingService [color="red"]
    GameRequest -> User
    GameRequestCompletion -> GameRequest [color="red"]
    GameRequestCompletion -> Player
}
**/

namespace FacetedWorlds.Reversi.Model
{
    public partial class Identity : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Identity newFact = new Identity(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._uri = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Identity fact = (Identity)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._uri);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.Identity", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles

        // Queries
        public static Query QueryClaims = new Query()
            .JoinSuccessors(Claim.RoleIdentity)
            ;
        public static Query QueryActiveLocalGames = new Query()
            .JoinSuccessors(LocalGame.RoleIdentity, Condition.WhereIsEmpty(LocalGame.QueryIsActive)
            )
            ;
        public static Query QueryIsToastNotificationDisabled = new Query()
            .JoinSuccessors(DisableToastNotification.RoleIdentity, Condition.WhereIsEmpty(DisableToastNotification.QueryIsReenabled)
            )
            ;

        // Predicates

        // Predecessors

        // Fields
        private string _uri;

        // Results
        private Result<Claim> _claims;
        private Result<LocalGame> _activeLocalGames;
        private Result<DisableToastNotification> _isToastNotificationDisabled;

        // Business constructor
        public Identity(
            string uri
            )
        {
            InitializeResults();
            _uri = uri;
        }

        // Hydration constructor
        private Identity(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _claims = new Result<Claim>(this, QueryClaims);
            _activeLocalGames = new Result<LocalGame>(this, QueryActiveLocalGames);
            _isToastNotificationDisabled = new Result<DisableToastNotification>(this, QueryIsToastNotificationDisabled);
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
        public IEnumerable<LocalGame> ActiveLocalGames
        {
            get { return _activeLocalGames; }
        }
        public IEnumerable<DisableToastNotification> IsToastNotificationDisabled
        {
            get { return _isToastNotificationDisabled; }
        }

        // Mutable property access

    }
    
    public partial class IdentityService : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				IdentityService newFact = new IdentityService(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				IdentityService fact = (IdentityService)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.IdentityService", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

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
        private IdentityService(FactMemento memento)
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

        // Mutable property access

    }
    
    public partial class Claim : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Claim newFact = new Claim(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Claim fact = (Claim)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.Claim", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleIdentity = new Role(new RoleMemento(
			_correspondenceFactType,
			"identity",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Identity", 1),
			true));
        public static Role RoleUser = new Role(new RoleMemento(
			_correspondenceFactType,
			"user",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.User", 1),
			false));
        public static Role RoleIdentityService = new Role(new RoleMemento(
			_correspondenceFactType,
			"identityService",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.IdentityService", 1),
			true));

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
        private Claim(FactMemento memento)
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

        // Mutable property access

    }
    
    public partial class ClaimResponse : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				ClaimResponse newFact = new ClaimResponse(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._approved = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				ClaimResponse fact = (ClaimResponse)obj;
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._approved);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.ClaimResponse", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleClaim = new Role(new RoleMemento(
			_correspondenceFactType,
			"claim",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Claim", 1),
			true));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Claim> _claim;

        // Fields
        private int _approved;

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
        private ClaimResponse(FactMemento memento)
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

        // Mutable property access

    }
    
    public partial class User : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				User newFact = new User(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._userName = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				User fact = (User)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._userName);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.User", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

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
        public static Query QueryIsChatEnabled = new Query()
            .JoinSuccessors(ChatEnable.RoleUser)
            ;
        public static Query QueryGameRequests = new Query()
            .JoinSuccessors(GameRequest.RoleUser)
            ;
        public static Query QueryPendingGameRequests = new Query()
            .JoinSuccessors(GameRequest.RoleUser, Condition.WhereIsEmpty(GameRequest.QueryIsCompleted)
            )
            ;

        // Predicates

        // Predecessors

        // Fields
        private string _userName;

        // Results
        private Result<Claim> _claims;
        private Result<Player> _activePlayers;
        private Result<Player> _finishedPlayers;
        private Result<User> _relatedUsers;
        private Result<ChatEnable> _isChatEnabled;
        private Result<GameRequest> _gameRequests;
        private Result<GameRequest> _pendingGameRequests;

        // Business constructor
        public User(
            string userName
            )
        {
            InitializeResults();
            _userName = userName;
        }

        // Hydration constructor
        private User(FactMemento memento)
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
            _isChatEnabled = new Result<ChatEnable>(this, QueryIsChatEnabled);
            _gameRequests = new Result<GameRequest>(this, QueryGameRequests);
            _pendingGameRequests = new Result<GameRequest>(this, QueryPendingGameRequests);
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
        public IEnumerable<ChatEnable> IsChatEnabled
        {
            get { return _isChatEnabled; }
        }
        public IEnumerable<GameRequest> GameRequests
        {
            get { return _gameRequests; }
        }
        public IEnumerable<GameRequest> PendingGameRequests
        {
            get { return _pendingGameRequests; }
        }

        // Mutable property access

    }
    
    public partial class ChatEnable : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				ChatEnable newFact = new ChatEnable(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				ChatEnable fact = (ChatEnable)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.ChatEnable", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleUser = new Role(new RoleMemento(
			_correspondenceFactType,
			"user",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.User", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<User> _user;

        // Fields

        // Results

        // Business constructor
        public ChatEnable(
            User user
            )
        {
            InitializeResults();
            _user = new PredecessorObj<User>(this, RoleUser, user);
        }

        // Hydration constructor
        private ChatEnable(FactMemento memento)
        {
            InitializeResults();
            _user = new PredecessorObj<User>(this, RoleUser, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public User User
        {
            get { return _user.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class DisableToastNotification : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				DisableToastNotification newFact = new DisableToastNotification(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._unique = (Guid)_fieldSerializerByType[typeof(Guid)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				DisableToastNotification fact = (DisableToastNotification)obj;
				_fieldSerializerByType[typeof(Guid)].WriteData(output, fact._unique);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.DisableToastNotification", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleIdentity = new Role(new RoleMemento(
			_correspondenceFactType,
			"identity",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Identity", 1),
			false));

        // Queries
        public static Query QueryIsReenabled = new Query()
            .JoinSuccessors(EnableToastNotification.RoleDisable)
            ;

        // Predicates
        public static Condition IsReenabled = Condition.WhereIsNotEmpty(QueryIsReenabled);

        // Predecessors
        private PredecessorObj<Identity> _identity;

        // Unique
        private Guid _unique;

        // Fields

        // Results

        // Business constructor
        public DisableToastNotification(
            Identity identity
            )
        {
            _unique = Guid.NewGuid();
            InitializeResults();
            _identity = new PredecessorObj<Identity>(this, RoleIdentity, identity);
        }

        // Hydration constructor
        private DisableToastNotification(FactMemento memento)
        {
            InitializeResults();
            _identity = new PredecessorObj<Identity>(this, RoleIdentity, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Identity Identity
        {
            get { return _identity.Fact; }
        }

        // Field access
		public Guid Unique { get { return _unique; } }


        // Query result access

        // Mutable property access

    }
    
    public partial class EnableToastNotification : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				EnableToastNotification newFact = new EnableToastNotification(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				EnableToastNotification fact = (EnableToastNotification)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.EnableToastNotification", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleDisable = new Role(new RoleMemento(
			_correspondenceFactType,
			"disable",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.DisableToastNotification", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<DisableToastNotification> _disable;

        // Fields

        // Results

        // Business constructor
        public EnableToastNotification(
            DisableToastNotification disable
            )
        {
            InitializeResults();
            _disable = new PredecessorObj<DisableToastNotification>(this, RoleDisable, disable);
        }

        // Hydration constructor
        private EnableToastNotification(FactMemento memento)
        {
            InitializeResults();
            _disable = new PredecessorObj<DisableToastNotification>(this, RoleDisable, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public DisableToastNotification Disable
        {
            get { return _disable.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class Game : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Game newFact = new Game(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._unique = (Guid)_fieldSerializerByType[typeof(Guid)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Game fact = (Game)obj;
				_fieldSerializerByType[typeof(Guid)].WriteData(output, fact._unique);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.Game", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

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
        private Guid _unique;

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
        private Game(FactMemento memento)
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
		public Guid Unique { get { return _unique; } }


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

        // Mutable property access

    }
    
    public partial class Player : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Player newFact = new Player(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._index = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Player fact = (Player)obj;
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._index);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.Player", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleUser = new Role(new RoleMemento(
			_correspondenceFactType,
			"user",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.User", 1),
			true));
        public static Role RoleGame = new Role(new RoleMemento(
			_correspondenceFactType,
			"game",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Game", 1),
			true));

        // Queries
        public static Query QueryMoves = new Query()
            .JoinSuccessors(Move.RolePlayer)
            ;
        public static Query QueryNewMessages = new Query()
            .JoinSuccessors(Message.RoleSender, Condition.WhereIsEmpty(Message.QueryIsAcknowledged)
            )
            ;
        public static Query QueryIsActive = new Query()
            .JoinSuccessors(OutcomeAcknowledge.RolePlayer)
            ;
        public static Query QueryIsNotActive = new Query()
            .JoinSuccessors(OutcomeAcknowledge.RolePlayer)
            ;

        // Predicates
        public static Condition IsActive = Condition.WhereIsEmpty(QueryIsActive);
        public static Condition IsNotActive = Condition.WhereIsNotEmpty(QueryIsNotActive);

        // Predecessors
        private PredecessorObj<User> _user;
        private PredecessorObj<Game> _game;

        // Fields
        private int _index;

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
        private Player(FactMemento memento)
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

        // Mutable property access

    }
    
    public partial class Message : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Message newFact = new Message(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._body = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
						newFact._index = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Message fact = (Message)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._body);
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._index);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.Message", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleSender = new Role(new RoleMemento(
			_correspondenceFactType,
			"sender",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Player", 1),
			false));

        // Queries
        public static Query QueryIsAcknowledged = new Query()
            .JoinSuccessors(Acknowledge.RoleMessage)
            ;

        // Predicates
        public static Condition IsAcknowledged = Condition.WhereIsNotEmpty(QueryIsAcknowledged);

        // Predecessors
        private PredecessorObj<Player> _sender;

        // Fields
        private string _body;
        private int _index;

        // Results

        // Business constructor
        public Message(
            Player sender
            ,string body
            ,int index
            )
        {
            InitializeResults();
            _sender = new PredecessorObj<Player>(this, RoleSender, sender);
            _body = body;
            _index = index;
        }

        // Hydration constructor
        private Message(FactMemento memento)
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
        public int Index
        {
            get { return _index; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class Acknowledge : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Acknowledge newFact = new Acknowledge(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Acknowledge fact = (Acknowledge)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.Acknowledge", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleMessage = new Role(new RoleMemento(
			_correspondenceFactType,
			"message",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Message", 1),
			false));

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
        private Acknowledge(FactMemento memento)
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

        // Mutable property access

    }
    
    public partial class Move : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Move newFact = new Move(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._index = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
						newFact._square = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Move fact = (Move)obj;
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._index);
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._square);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.Move", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RolePlayer = new Role(new RoleMemento(
			_correspondenceFactType,
			"player",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Player", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Player> _player;

        // Fields
        private int _index;
        private int _square;

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
        private Move(FactMemento memento)
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

        // Mutable property access

    }
    
    public partial class Outcome : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Outcome newFact = new Outcome(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._resigned = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Outcome fact = (Outcome)obj;
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._resigned);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.Outcome", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleGame = new Role(new RoleMemento(
			_correspondenceFactType,
			"game",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Game", 1),
			false));
        public static Role RoleWinner = new Role(new RoleMemento(
			_correspondenceFactType,
			"winner",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Player", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Game> _game;
        private PredecessorOpt<Player> _winner;

        // Fields
        private int _resigned;

        // Results

        // Business constructor
        public Outcome(
            Game game
            ,Player winner
            ,int resigned
            )
        {
            InitializeResults();
            _game = new PredecessorObj<Game>(this, RoleGame, game);
            _winner = new PredecessorOpt<Player>(this, RoleWinner, winner);
            _resigned = resigned;
        }

        // Hydration constructor
        private Outcome(FactMemento memento)
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
        public int Resigned
        {
            get { return _resigned; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class OutcomeAcknowledge : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				OutcomeAcknowledge newFact = new OutcomeAcknowledge(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				OutcomeAcknowledge fact = (OutcomeAcknowledge)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.OutcomeAcknowledge", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RolePlayer = new Role(new RoleMemento(
			_correspondenceFactType,
			"player",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Player", 1),
			false));
        public static Role RoleOutcome = new Role(new RoleMemento(
			_correspondenceFactType,
			"outcome",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Outcome", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Player> _player;
        private PredecessorObj<Outcome> _outcome;

        // Fields

        // Results

        // Business constructor
        public OutcomeAcknowledge(
            Player player
            ,Outcome outcome
            )
        {
            InitializeResults();
            _player = new PredecessorObj<Player>(this, RolePlayer, player);
            _outcome = new PredecessorObj<Outcome>(this, RoleOutcome, outcome);
        }

        // Hydration constructor
        private OutcomeAcknowledge(FactMemento memento)
        {
            InitializeResults();
            _player = new PredecessorObj<Player>(this, RolePlayer, memento);
            _outcome = new PredecessorObj<Outcome>(this, RoleOutcome, memento);
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
        public Outcome Outcome
        {
            get { return _outcome.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class LocalGame : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				LocalGame newFact = new LocalGame(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._unique = (Guid)_fieldSerializerByType[typeof(Guid)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				LocalGame fact = (LocalGame)obj;
				_fieldSerializerByType[typeof(Guid)].WriteData(output, fact._unique);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.LocalGame", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleIdentity = new Role(new RoleMemento(
			_correspondenceFactType,
			"identity",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Identity", 1),
			false));

        // Queries
        public static Query QueryPlayers = new Query()
            .JoinSuccessors(LocalPlayer.RoleGame)
            ;
        public static Query QueryMoves = new Query()
            .JoinSuccessors(LocalPlayer.RoleGame)
            .JoinSuccessors(LocalMove.RolePlayer)
            ;
        public static Query QueryOutcomes = new Query()
            .JoinSuccessors(LocalOutcome.RoleGame)
            ;
        public static Query QueryIsActive = new Query()
            .JoinSuccessors(LocalOutcome.RoleGame)
            .JoinSuccessors(LocalOutcomeAcknowledge.RoleOutcome)
            ;

        // Predicates
        public static Condition IsActive = Condition.WhereIsEmpty(QueryIsActive);

        // Predecessors
        private PredecessorObj<Identity> _identity;

        // Unique
        private Guid _unique;

        // Fields

        // Results
        private Result<LocalPlayer> _players;
        private Result<LocalMove> _moves;
        private Result<LocalOutcome> _outcomes;

        // Business constructor
        public LocalGame(
            Identity identity
            )
        {
            _unique = Guid.NewGuid();
            InitializeResults();
            _identity = new PredecessorObj<Identity>(this, RoleIdentity, identity);
        }

        // Hydration constructor
        private LocalGame(FactMemento memento)
        {
            InitializeResults();
            _identity = new PredecessorObj<Identity>(this, RoleIdentity, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
            _players = new Result<LocalPlayer>(this, QueryPlayers);
            _moves = new Result<LocalMove>(this, QueryMoves);
            _outcomes = new Result<LocalOutcome>(this, QueryOutcomes);
        }

        // Predecessor access
        public Identity Identity
        {
            get { return _identity.Fact; }
        }

        // Field access
		public Guid Unique { get { return _unique; } }


        // Query result access
        public IEnumerable<LocalPlayer> Players
        {
            get { return _players; }
        }
        public IEnumerable<LocalMove> Moves
        {
            get { return _moves; }
        }
        public IEnumerable<LocalOutcome> Outcomes
        {
            get { return _outcomes; }
        }

        // Mutable property access

    }
    
    public partial class LocalPlayer : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				LocalPlayer newFact = new LocalPlayer(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._index = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				LocalPlayer fact = (LocalPlayer)obj;
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._index);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.LocalPlayer", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleGame = new Role(new RoleMemento(
			_correspondenceFactType,
			"game",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.LocalGame", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<LocalGame> _game;

        // Fields
        private int _index;

        // Results

        // Business constructor
        public LocalPlayer(
            LocalGame game
            ,int index
            )
        {
            InitializeResults();
            _game = new PredecessorObj<LocalGame>(this, RoleGame, game);
            _index = index;
        }

        // Hydration constructor
        private LocalPlayer(FactMemento memento)
        {
            InitializeResults();
            _game = new PredecessorObj<LocalGame>(this, RoleGame, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public LocalGame Game
        {
            get { return _game.Fact; }
        }

        // Field access
        public int Index
        {
            get { return _index; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class LocalMove : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				LocalMove newFact = new LocalMove(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._index = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
						newFact._square = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				LocalMove fact = (LocalMove)obj;
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._index);
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._square);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.LocalMove", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RolePlayer = new Role(new RoleMemento(
			_correspondenceFactType,
			"player",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.LocalPlayer", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<LocalPlayer> _player;

        // Fields
        private int _index;
        private int _square;

        // Results

        // Business constructor
        public LocalMove(
            LocalPlayer player
            ,int index
            ,int square
            )
        {
            InitializeResults();
            _player = new PredecessorObj<LocalPlayer>(this, RolePlayer, player);
            _index = index;
            _square = square;
        }

        // Hydration constructor
        private LocalMove(FactMemento memento)
        {
            InitializeResults();
            _player = new PredecessorObj<LocalPlayer>(this, RolePlayer, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public LocalPlayer Player
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

        // Mutable property access

    }
    
    public partial class LocalOutcome : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				LocalOutcome newFact = new LocalOutcome(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._resigned = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				LocalOutcome fact = (LocalOutcome)obj;
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._resigned);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.LocalOutcome", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleGame = new Role(new RoleMemento(
			_correspondenceFactType,
			"game",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.LocalGame", 1),
			false));
        public static Role RoleWinner = new Role(new RoleMemento(
			_correspondenceFactType,
			"winner",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.LocalPlayer", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<LocalGame> _game;
        private PredecessorOpt<LocalPlayer> _winner;

        // Fields
        private int _resigned;

        // Results

        // Business constructor
        public LocalOutcome(
            LocalGame game
            ,LocalPlayer winner
            ,int resigned
            )
        {
            InitializeResults();
            _game = new PredecessorObj<LocalGame>(this, RoleGame, game);
            _winner = new PredecessorOpt<LocalPlayer>(this, RoleWinner, winner);
            _resigned = resigned;
        }

        // Hydration constructor
        private LocalOutcome(FactMemento memento)
        {
            InitializeResults();
            _game = new PredecessorObj<LocalGame>(this, RoleGame, memento);
            _winner = new PredecessorOpt<LocalPlayer>(this, RoleWinner, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public LocalGame Game
        {
            get { return _game.Fact; }
        }
        public LocalPlayer Winner
        {
            get { return _winner.Fact; }
        }

        // Field access
        public int Resigned
        {
            get { return _resigned; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class LocalOutcomeAcknowledge : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				LocalOutcomeAcknowledge newFact = new LocalOutcomeAcknowledge(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				LocalOutcomeAcknowledge fact = (LocalOutcomeAcknowledge)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.LocalOutcomeAcknowledge", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleOutcome = new Role(new RoleMemento(
			_correspondenceFactType,
			"outcome",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.LocalOutcome", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<LocalOutcome> _outcome;

        // Fields

        // Results

        // Business constructor
        public LocalOutcomeAcknowledge(
            LocalOutcome outcome
            )
        {
            InitializeResults();
            _outcome = new PredecessorObj<LocalOutcome>(this, RoleOutcome, outcome);
        }

        // Hydration constructor
        private LocalOutcomeAcknowledge(FactMemento memento)
        {
            InitializeResults();
            _outcome = new PredecessorObj<LocalOutcome>(this, RoleOutcome, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public LocalOutcome Outcome
        {
            get { return _outcome.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class MatchmakingService : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				MatchmakingService newFact = new MatchmakingService(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				MatchmakingService fact = (MatchmakingService)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.MatchmakingService", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles

        // Queries
        public static Query QueryPendingGameRequests = new Query()
            .JoinSuccessors(GameRequest.RoleMatchmakingService, Condition.WhereIsEmpty(GameRequest.QueryIsCompleted)
            )
            ;

        // Predicates

        // Predecessors

        // Fields

        // Results
        private Result<GameRequest> _pendingGameRequests;

        // Business constructor
        public MatchmakingService(
            )
        {
            InitializeResults();
        }

        // Hydration constructor
        private MatchmakingService(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _pendingGameRequests = new Result<GameRequest>(this, QueryPendingGameRequests);
        }

        // Predecessor access

        // Field access

        // Query result access
        public IEnumerable<GameRequest> PendingGameRequests
        {
            get { return _pendingGameRequests; }
        }

        // Mutable property access

    }
    
    public partial class GameRequest : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				GameRequest newFact = new GameRequest(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._unique = (Guid)_fieldSerializerByType[typeof(Guid)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				GameRequest fact = (GameRequest)obj;
				_fieldSerializerByType[typeof(Guid)].WriteData(output, fact._unique);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.GameRequest", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleMatchmakingService = new Role(new RoleMemento(
			_correspondenceFactType,
			"matchmakingService",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.MatchmakingService", 1),
			true));
        public static Role RoleUser = new Role(new RoleMemento(
			_correspondenceFactType,
			"user",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.User", 1),
			false));

        // Queries
        public static Query QueryIsCompleted = new Query()
            .JoinSuccessors(GameRequestCompletion.RoleGameRequest)
            ;
        public static Query QueryPlayer = new Query()
            .JoinSuccessors(GameRequestCompletion.RoleGameRequest)
            .JoinPredecessors(GameRequestCompletion.RolePlayer)
            ;

        // Predicates
        public static Condition IsCompleted = Condition.WhereIsNotEmpty(QueryIsCompleted);

        // Predecessors
        private PredecessorObj<MatchmakingService> _matchmakingService;
        private PredecessorObj<User> _user;

        // Unique
        private Guid _unique;

        // Fields

        // Results
        private Result<Player> _player;

        // Business constructor
        public GameRequest(
            MatchmakingService matchmakingService
            ,User user
            )
        {
            _unique = Guid.NewGuid();
            InitializeResults();
            _matchmakingService = new PredecessorObj<MatchmakingService>(this, RoleMatchmakingService, matchmakingService);
            _user = new PredecessorObj<User>(this, RoleUser, user);
        }

        // Hydration constructor
        private GameRequest(FactMemento memento)
        {
            InitializeResults();
            _matchmakingService = new PredecessorObj<MatchmakingService>(this, RoleMatchmakingService, memento);
            _user = new PredecessorObj<User>(this, RoleUser, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
            _player = new Result<Player>(this, QueryPlayer);
        }

        // Predecessor access
        public MatchmakingService MatchmakingService
        {
            get { return _matchmakingService.Fact; }
        }
        public User User
        {
            get { return _user.Fact; }
        }

        // Field access
		public Guid Unique { get { return _unique; } }


        // Query result access
        public IEnumerable<Player> Player
        {
            get { return _player; }
        }

        // Mutable property access

    }
    
    public partial class GameRequestCompletion : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				GameRequestCompletion newFact = new GameRequestCompletion(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				GameRequestCompletion fact = (GameRequestCompletion)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.Reversi.Model.GameRequestCompletion", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleGameRequest = new Role(new RoleMemento(
			_correspondenceFactType,
			"gameRequest",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.GameRequest", 1),
			true));
        public static Role RolePlayer = new Role(new RoleMemento(
			_correspondenceFactType,
			"player",
			new CorrespondenceFactType("FacetedWorlds.Reversi.Model.Player", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<GameRequest> _gameRequest;
        private PredecessorObj<Player> _player;

        // Fields

        // Results

        // Business constructor
        public GameRequestCompletion(
            GameRequest gameRequest
            ,Player player
            )
        {
            InitializeResults();
            _gameRequest = new PredecessorObj<GameRequest>(this, RoleGameRequest, gameRequest);
            _player = new PredecessorObj<Player>(this, RolePlayer, player);
        }

        // Hydration constructor
        private GameRequestCompletion(FactMemento memento)
        {
            InitializeResults();
            _gameRequest = new PredecessorObj<GameRequest>(this, RoleGameRequest, memento);
            _player = new PredecessorObj<Player>(this, RolePlayer, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public GameRequest GameRequest
        {
            get { return _gameRequest.Fact; }
        }
        public Player Player
        {
            get { return _player.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    

	public class CorrespondenceModel : ICorrespondenceModel
	{
		public void RegisterAllFactTypes(Community community, IDictionary<Type, IFieldSerializer> fieldSerializerByType)
		{
			community.AddType(
				Identity._correspondenceFactType,
				new Identity.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Identity._correspondenceFactType }));
			community.AddQuery(
				Identity._correspondenceFactType,
				Identity.QueryClaims.QueryDefinition);
			community.AddQuery(
				Identity._correspondenceFactType,
				Identity.QueryActiveLocalGames.QueryDefinition);
			community.AddQuery(
				Identity._correspondenceFactType,
				Identity.QueryIsToastNotificationDisabled.QueryDefinition);
			community.AddType(
				IdentityService._correspondenceFactType,
				new IdentityService.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { IdentityService._correspondenceFactType }));
			community.AddQuery(
				IdentityService._correspondenceFactType,
				IdentityService.QueryPendingClaims.QueryDefinition);
			community.AddType(
				Claim._correspondenceFactType,
				new Claim.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Claim._correspondenceFactType }));
			community.AddQuery(
				Claim._correspondenceFactType,
				Claim.QueryResponses.QueryDefinition);
			community.AddQuery(
				Claim._correspondenceFactType,
				Claim.QueryIsPending.QueryDefinition);
			community.AddType(
				ClaimResponse._correspondenceFactType,
				new ClaimResponse.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { ClaimResponse._correspondenceFactType }));
			community.AddType(
				User._correspondenceFactType,
				new User.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { User._correspondenceFactType }));
			community.AddQuery(
				User._correspondenceFactType,
				User.QueryClaims.QueryDefinition);
			community.AddQuery(
				User._correspondenceFactType,
				User.QueryActivePlayers.QueryDefinition);
			community.AddQuery(
				User._correspondenceFactType,
				User.QueryFinishedPlayers.QueryDefinition);
			community.AddQuery(
				User._correspondenceFactType,
				User.QueryRelatedUsers.QueryDefinition);
			community.AddQuery(
				User._correspondenceFactType,
				User.QueryIsChatEnabled.QueryDefinition);
			community.AddQuery(
				User._correspondenceFactType,
				User.QueryGameRequests.QueryDefinition);
			community.AddQuery(
				User._correspondenceFactType,
				User.QueryPendingGameRequests.QueryDefinition);
			community.AddType(
				ChatEnable._correspondenceFactType,
				new ChatEnable.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { ChatEnable._correspondenceFactType }));
			community.AddType(
				DisableToastNotification._correspondenceFactType,
				new DisableToastNotification.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { DisableToastNotification._correspondenceFactType }));
			community.AddQuery(
				DisableToastNotification._correspondenceFactType,
				DisableToastNotification.QueryIsReenabled.QueryDefinition);
			community.AddType(
				EnableToastNotification._correspondenceFactType,
				new EnableToastNotification.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { EnableToastNotification._correspondenceFactType }));
			community.AddType(
				Game._correspondenceFactType,
				new Game.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Game._correspondenceFactType }));
			community.AddQuery(
				Game._correspondenceFactType,
				Game.QueryPlayers.QueryDefinition);
			community.AddQuery(
				Game._correspondenceFactType,
				Game.QueryMessages.QueryDefinition);
			community.AddQuery(
				Game._correspondenceFactType,
				Game.QueryMoves.QueryDefinition);
			community.AddQuery(
				Game._correspondenceFactType,
				Game.QueryOutcomes.QueryDefinition);
			community.AddType(
				Player._correspondenceFactType,
				new Player.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Player._correspondenceFactType }));
			community.AddQuery(
				Player._correspondenceFactType,
				Player.QueryMoves.QueryDefinition);
			community.AddQuery(
				Player._correspondenceFactType,
				Player.QueryNewMessages.QueryDefinition);
			community.AddQuery(
				Player._correspondenceFactType,
				Player.QueryIsActive.QueryDefinition);
			community.AddQuery(
				Player._correspondenceFactType,
				Player.QueryIsNotActive.QueryDefinition);
			community.AddType(
				Message._correspondenceFactType,
				new Message.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Message._correspondenceFactType }));
			community.AddQuery(
				Message._correspondenceFactType,
				Message.QueryIsAcknowledged.QueryDefinition);
			community.AddType(
				Acknowledge._correspondenceFactType,
				new Acknowledge.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Acknowledge._correspondenceFactType }));
			community.AddType(
				Move._correspondenceFactType,
				new Move.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Move._correspondenceFactType }));
			community.AddType(
				Outcome._correspondenceFactType,
				new Outcome.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Outcome._correspondenceFactType }));
			community.AddType(
				OutcomeAcknowledge._correspondenceFactType,
				new OutcomeAcknowledge.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { OutcomeAcknowledge._correspondenceFactType }));
			community.AddType(
				LocalGame._correspondenceFactType,
				new LocalGame.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { LocalGame._correspondenceFactType }));
			community.AddQuery(
				LocalGame._correspondenceFactType,
				LocalGame.QueryPlayers.QueryDefinition);
			community.AddQuery(
				LocalGame._correspondenceFactType,
				LocalGame.QueryMoves.QueryDefinition);
			community.AddQuery(
				LocalGame._correspondenceFactType,
				LocalGame.QueryOutcomes.QueryDefinition);
			community.AddQuery(
				LocalGame._correspondenceFactType,
				LocalGame.QueryIsActive.QueryDefinition);
			community.AddType(
				LocalPlayer._correspondenceFactType,
				new LocalPlayer.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { LocalPlayer._correspondenceFactType }));
			community.AddType(
				LocalMove._correspondenceFactType,
				new LocalMove.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { LocalMove._correspondenceFactType }));
			community.AddType(
				LocalOutcome._correspondenceFactType,
				new LocalOutcome.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { LocalOutcome._correspondenceFactType }));
			community.AddType(
				LocalOutcomeAcknowledge._correspondenceFactType,
				new LocalOutcomeAcknowledge.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { LocalOutcomeAcknowledge._correspondenceFactType }));
			community.AddType(
				MatchmakingService._correspondenceFactType,
				new MatchmakingService.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { MatchmakingService._correspondenceFactType }));
			community.AddQuery(
				MatchmakingService._correspondenceFactType,
				MatchmakingService.QueryPendingGameRequests.QueryDefinition);
			community.AddType(
				GameRequest._correspondenceFactType,
				new GameRequest.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { GameRequest._correspondenceFactType }));
			community.AddQuery(
				GameRequest._correspondenceFactType,
				GameRequest.QueryIsCompleted.QueryDefinition);
			community.AddQuery(
				GameRequest._correspondenceFactType,
				GameRequest.QueryPlayer.QueryDefinition);
			community.AddType(
				GameRequestCompletion._correspondenceFactType,
				new GameRequestCompletion.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { GameRequestCompletion._correspondenceFactType }));
		}
	}
}
