using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Game
    {
        #region Static members definition
        public static readonly Dictionary<string, int> HoldEmRanks = new Dictionary<string, int>()
        {
            {"2", 1},
            {"3", 2},
            {"4", 3},
            {"5", 4},
            {"6", 5},
            {"7", 6},
            {"8", 7},
            {"9", 8},
            {"10", 9},
            {"Jack", 10},
            {"Queen", 11},
            {"King", 12},
            {"Ace", 13},
        };

        public static readonly Dictionary<PossibleGames, Deck.DeckStyles> GameDeckStyles = new Dictionary<PossibleGames, Deck.DeckStyles>()
        {
            {PossibleGames.TexasHoldEm, Deck.DeckStyles.Standard52},
        };

        public static readonly Dictionary<PossibleGames, int> CommunityCardsSize = new Dictionary<PossibleGames, int>()
        {
            {PossibleGames.TexasHoldEm, 5},
        };

        public enum PossibleGames { TexasHoldEm };
        #endregion

        #region Members definition
        private Deck _cardDeck;
        public Deck CardDeck
        {
            get { return _cardDeck; }
        }

        private Card[] _communityCards;
        public Card[] CommunityCards
        {
            get { return _communityCards; }
        }

        private List<Player> _playerList;
        public List<Player> PlayerList
        {
            get { return _playerList; }
        }

        private List<Player> _foldedPlayerList;
        public List<Player> FoldedPlayerList
        {
            get { return _foldedPlayerList; }
        }

        private PossibleGames _gameType;
        public PossibleGames GameType
        {
            get { return _gameType; }
        }

        private int _currentBet;
        public int CurrentBet
        {
            get { return _currentBet; }
        }

        private int _currentPot;
        public int CurrentPot
        {
            get { return _currentPot; }
        }

        private Player _humanPlayer;
        public Player HumanPlayer
        {
            get { return _humanPlayer; }
        }

        private Computer _computerPlayer;
        public Computer ComputerPlayer
        {
            get { return _computerPlayer; }
        }
        #endregion

        #region Constructors definition
        public Game(PossibleGames selectedGame)
        {
            this._cardDeck = new Deck(GameDeckStyles[selectedGame]);
            this._communityCards = new Card[CommunityCardsSize[selectedGame]];
            CardDeck.Shuffle();
            this._gameType = selectedGame;
            this._playerList = new List<Player>();
            this._foldedPlayerList = new List<Player>();
        }
        #endregion

        #region Methods definition
        /// <summary>
        /// Add the given Player object to the current game's list of players
        /// </summary>
        /// <param name="cPlayer">The Player object to add</param>
        public void AddPlayer(Player cPlayer)
        {
            if (!PlayerList.Contains<Player>(cPlayer))
            {
                PlayerList.Add(cPlayer);
                if (cPlayer.RegisteredGame == null)
                {
                    // Player object is not registered to a Game
                    cPlayer.RegisterGame(this);
                }

                if (cPlayer is Computer)
                {
                    // This is the Computer player object
                    this._computerPlayer = (Computer)cPlayer;
                }
                else
                {
                    // This is the human Player object
                    this._humanPlayer = cPlayer;
                }
            }
        }

        /// <summary>
        /// Distribute cards to each player to fill their hands
        /// </summary>
        public void DistributeHands()
        {
            foreach (Player cPlayer in this.PlayerList)
            {
                for (int i = 1; i <= cPlayer.HandSize; i++)
                {
                    // TODO: error handling
                    cPlayer.AddCardToHand(this.CardDeck.DrawCard());
                }
                cPlayer.AddChips(5);
            }
        }

        /// <summary>
        /// Run through a round of betting, going to each unfolded Player and receive bets
        /// </summary>
        public void PlayBettingRound()
        {
            foreach (Player cPlayer in this.PlayerList)
            {
                if (!this.FoldedPlayerList.Contains(cPlayer))
                {
                    // Player has not folded out of this hand
                    this.GetNextBet(cPlayer);
                }
            }

            // Reset _currentBet
            this._currentBet = 0;
        }

        private void GetNextBet(Player cPlayer)
        {
            BetChoice returnedBet = cPlayer.MakeBet(this.CurrentBet);
            if (returnedBet.BetAction == BetChoice.BetActions.Raise)
            {
                // Currently betting Player raised
                if (returnedBet.BetAmount >= this.CurrentBet)
                {
                    // Player could match or beat the current bet. Set CurrentBet to the bet made by the Player.
                    this._currentBet = returnedBet.BetAmount;
                    this._currentPot += returnedBet.BetAmount;
                }
                else
                {
                    // Player could not match the current bet. Leave CurrentBet as it was
                    this._currentPot += returnedBet.BetAmount;
                }
            }
            else if (returnedBet.BetAction == BetChoice.BetActions.Call)
            {
                // Currently betting Player called
                this._currentPot += returnedBet.BetAmount;
            }
            else if (returnedBet.BetAction == BetChoice.BetActions.Fold)
            {
                // Currently betting Played folded
                // Add to this round's folded Players list
                this.FoldedPlayerList.Add(cPlayer);
            }
        }

        /// <summary>
        /// Draw three cards and place them in this Game's community cards array for the flop
        /// </summary>
        public void DrawFlop()
        {
            for (int i = 0; i < 3; i++)
            {
                // Loop three times and draw a card each time for the flop
                this.CommunityCards[i] = this.CardDeck.DrawCard();
            }
        }

        /// <summary>
        /// Draw one card and place it in this Game's community cards array for the turn
        /// </summary>
        public void DrawTurn()
        {
            this.CommunityCards[3] = this.CardDeck.DrawCard();
        }
        #endregion
    }

    class BetReadyEventArgs : EventArgs
    {
        private Player _bettingPlayer;
        public Player BettingPlayer
        {
            get { return _bettingPlayer; }
            set { _bettingPlayer = value; }
        }

        private int _currentBet;
        public int CurrentBet
        {
            get { return _currentBet; }
        }

        public BetReadyEventArgs(Player bettingPlayer, int currentBet)
        {
            this.BettingPlayer = bettingPlayer;
            this._currentBet = currentBet;
        }
    }
}
